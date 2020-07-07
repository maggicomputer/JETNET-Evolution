
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/DisplayFunctions.vb $
'$$Author: Matt $
'$$Date: 7/02/20 1:32p $
'$$Modtime: 7/02/20 1:22p $
'$$Revision: 97 $
'$$Workfile: DisplayFunctions.vb $
'
' ********************************************************************************

Public Class DisplayFunctions
    'A set of functions dealing primarily with displaying set data


    Public Shared Sub Resize_Image(ByVal temp_width As Integer, ByVal temp_height As Integer, ByVal desired_width As Integer, ByVal desired_height As Integer, ByRef returnString As String, ByVal imageSource As String, ByVal fAcpic_subject As String, ByVal cssClass As String)

        Try

            Dim temp_calc As Double = 0.0

            Dim temp_percent1 As Double = 0.0

            If temp_width > temp_height Then
                ' if the image is wider then the desired image width, then shirnk down to size.

                If (temp_width < desired_width) And (temp_height < desired_height) Then
                    temp_calc = (temp_height / temp_width)
                    If (temp_calc <= 0.7) Then  ' this is the ratio of the box, less than means just set width
                        'just force width, height will be fine
                        returnString += "<img border='0' class=""" + cssClass + """ src='" + imageSource + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & desired_width & "' />"
                    Else
                        temp_calc = (desired_height / temp_height)
                        temp_width = (temp_width * temp_calc)
                        returnString += "<img border='0' class=""" + cssClass + """ src='" + imageSource + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />"
                    End If
                Else
                    If temp_width > desired_width Then
                        temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                        temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                        temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                    End If

                    'assuming generally that a square is fine
                    If temp_height > desired_height Then
                        temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                        temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                        temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                    End If


                    returnString += "<img border='0' class=""" + cssClass + """ src='" + imageSource + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>"

                End If


            ElseIf temp_height > temp_width Then

                If (temp_width < desired_width) And (temp_height < desired_height) Then
                    temp_calc = (temp_width / temp_height)
                    If (temp_calc <= 0.7) Then  ' this is the ratio of the box, less than means just set width
                        'just force width, height will be fine

                        returnString += "<img border='0' class=""" + cssClass + """ src='" + imageSource + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />"

                    Else
                        temp_calc = (desired_height / temp_height)
                        temp_width = (temp_width * temp_calc)

                        returnString += "<img border='0' class=""" + cssClass + """ src='" + imageSource + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />"

                    End If
                Else
                    If temp_height > desired_height Then
                        temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                        temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                        temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                    End If

                    ' if the image is wider then the desired image width, then shirnk down to size.
                    If temp_width > desired_width Then
                        temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                        temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                        temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                    End If

                    returnString += "<img border='0' class=""" + cssClass + """ src='" + imageSource + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>"
                End If

            Else ' they are equal height and width

                'assuming generally that a square is fine
                If temp_height > desired_height Then
                    temp_percent1 = CDbl(CDbl(temp_width) / CDbl(temp_height))
                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))

                    returnString += "<img border='0' class=""" + cssClass + """ src='" + imageSource + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>"


                Else

                    returnString += "<img border='0' class=""" + cssClass + """ src='" + imageSource + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='350' />"

                End If

            End If

        Catch ex As Exception

            commonLogFunctions.forceLogError("UserError", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + "): " + ex.Message.Trim

        End Try

    End Sub

    Public Shared Function showEstAFTT(ByVal ac_airframe_tot_hrs As String, ByVal ac_est_airframe_hrs As String, ByVal ac_year As String, ByVal ac_times_as_of_date As String, ByVal bShowOnListing As Boolean, ByVal bShowOnTableHTML As Boolean) As String

        Dim htmlOutStr As String = ""

        Dim BASEACYEAR As Long = 2005
        Dim BASEACTIMES As Date = CDate("06/01/2005")

        Dim bShowEstAFTT As Boolean = True

        Dim nAcAFTT As Long = 0
        Dim nAcEstAFTT As Long = 0
        Dim nAcYear As Long = 0
        Dim dtAcTimesOfDate As Date = Now()

        If Not String.IsNullOrEmpty(ac_year.Trim) Then
            If IsNumeric(ac_year) Then
                nAcYear = CLng(ac_year.Trim)
            End If
        End If

        If Not String.IsNullOrEmpty(ac_times_as_of_date.Trim) Then
            If IsDate(ac_times_as_of_date) Then
                dtAcTimesOfDate = CDate(ac_times_as_of_date.Trim)
            End If
        End If

        If Not String.IsNullOrEmpty(ac_airframe_tot_hrs.Trim) Then
            If IsNumeric(ac_airframe_tot_hrs) Then
                nAcAFTT = CLng(ac_airframe_tot_hrs.Trim)
            End If
        End If

        If Not String.IsNullOrEmpty(ac_est_airframe_hrs.Trim) Then
            If IsNumeric(ac_est_airframe_hrs) Then
                nAcEstAFTT = CLng(ac_est_airframe_hrs.Trim)
            End If
        End If

        If nAcAFTT = 0 And nAcYear < BASEACYEAR Then
            bShowEstAFTT = False
        ElseIf dtAcTimesOfDate < BASEACTIMES Then
            bShowEstAFTT = False
        ElseIf nAcAFTT = nAcEstAFTT Then
            bShowEstAFTT = False
        End If

        If bShowOnListing Then
            If nAcAFTT > 0 Then
                htmlOutStr = "<span class=""""><span class=""label"">AFTT"
                htmlOutStr += IIf(bShowEstAFTT, " / <a href=""javascript:void();"" onclick=""openEstAFTTHelp();"" style=""color: rgb(164, 86, 86);"">EST AFTT</a>", "")
                htmlOutStr += "</span>:[" + nAcAFTT.ToString + "]"
                htmlOutStr += IIf(bShowEstAFTT, " / <span style=""color:rgb(164, 86, 86);"">[" + nAcEstAFTT.ToString + "]</span>", "") + "</span><br />"
            End If
        End If

        If bShowOnTableHTML Then
            If nAcAFTT > 0 Then
                htmlOutStr += "[" + nAcAFTT.ToString + "]"
                htmlOutStr += IIf(bShowEstAFTT, " / <span style=""color:rgb(164, 86, 86);"">[" + nAcEstAFTT.ToString + "]</span>", "") + "<br />"
            End If
        End If

        Return htmlOutStr

    End Function

    Public Shared Function TextToImage(ByVal stringToBeMadeAnImage As String, ByVal fontSize As Integer, ByVal fontName As String, ByVal DisplayWidth As String, ByVal altTag As String, Optional ByVal valign_text As String = "", Optional ByVal showk As Boolean = False, Optional ByVal fontNormal As Boolean = False) As String
        Dim ReturnString As String = ""
        Dim bmp As Drawing.Bitmap = New Drawing.Bitmap(1, 1)
        Dim canvas As Drawing.Graphics = Drawing.Graphics.FromImage(bmp)
        Dim size As Drawing.SizeF
        Dim memoryStream As New IO.MemoryStream()
        Dim pngData As Byte()
        Dim temp_number As String = ""
        Dim switch_back As Boolean = False
        Dim ErrorStringVar As String = ""
        Try

            If Not String.IsNullOrEmpty(stringToBeMadeAnImage) Then

                If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then


                    If InStr(Trim(stringToBeMadeAnImage), "$") > 0 Then
                        temp_number = Replace(stringToBeMadeAnImage, "$", "")
                    Else
                        temp_number = stringToBeMadeAnImage
                    End If

                    If InStr(Trim(temp_number), "k") > 0 Then
                        temp_number = Replace(temp_number, "k", "")
                    End If

                    If IsNumeric(temp_number) = True Then

                        If temp_number > 1000 Then
                            temp_number = FormatNumber(temp_number, 0)
                        Else
                            temp_number = CInt(temp_number / 10) ' divide by ten, which will cut down last variable
                            temp_number = FormatNumber((temp_number * 10), 0)
                        End If

                        switch_back = True
                    End If

                    'If InStr(Trim(stringToBeMadeAnImage), "$") > 0 Then
                    temp_number = "$" & temp_number
                    ' End If

                    If showk = True Then
                        If InStr(Trim(temp_number), "k") = 0 Then ' if there is no 0 in the number currently 
                            temp_number = temp_number & "k"
                        End If
                    End If


                    If switch_back = True Then
                        stringToBeMadeAnImage = temp_number
                    End If


                    ' Measure the string.
                    size = canvas.MeasureString(stringToBeMadeAnImage, New Drawing.Font(fontName, fontSize))
                    ErrorStringVar = "Canvas Measured"
                    ' Finally
                    'I removed the try finally here. 

                    canvas.Dispose()
                    bmp.Dispose()
                    'End Try

                    bmp = New Drawing.Bitmap(CInt(size.Width), CInt(size.Height))
                    ErrorStringVar = "New Bitmap Created"
                    canvas = Drawing.Graphics.FromImage(bmp)
                    ErrorStringVar = "Drawing.Graphics from BMP"

                    'Try
                    canvas.Clear(Drawing.Color.White)
                    ErrorStringVar = "Clear Canvas"

                    If fontNormal = False Then
                        canvas.DrawString(stringToBeMadeAnImage, New Drawing.Font(fontName, fontSize, Drawing.FontStyle.Underline), Drawing.Brushes.Red, 0, 0)
                    Else
                        canvas.DrawString(stringToBeMadeAnImage, New Drawing.Font(fontName, fontSize, Drawing.FontStyle.Regular), New Drawing.SolidBrush(Drawing.ColorTranslator.FromHtml("#474646")), 0, 0)
                    End If

                    ErrorStringVar = "Canvas DrawString"
                    canvas.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                    ErrorStringVar = "TextRenderingHint"
                    canvas.DrawImage(bmp, New System.Drawing.PointF)
                    ErrorStringVar = "Draw Image"

                    Using memoryStream 'As IO.MemoryStream()

                        bmp.Save(memoryStream, Drawing.Imaging.ImageFormat.Gif)
                        ErrorStringVar = "BMP Save"
                        pngData = memoryStream.ToArray()
                        ErrorStringVar = "Memory StreamToArray"
                        If Trim(valign_text) <> "" Then
                            ReturnString = "<img src=""data:image/gif;base64," & Convert.ToBase64String(pngData) & """ " & IIf(DisplayWidth <> "", "width='" & DisplayWidth & "'", "") & " " & IIf(altTag <> "", "alt='" & altTag & "'", "") & " " & IIf(altTag <> "", "title='" & altTag & "'", "") & " unselectable='on' style='display:inline-block; vertical-align:" & Trim(valign_text) & "' />"
                        Else
                            ReturnString = "<img src=""data:image/gif;base64," & Convert.ToBase64String(pngData) & """ " & IIf(DisplayWidth <> "", "width='" & DisplayWidth & "'", "") & " " & IIf(altTag <> "", "alt='" & altTag & "'", "") & " " & IIf(altTag <> "", "title='" & altTag & "'", "") & " unselectable='on'/>"
                        End If
                        ErrorStringVar = "ConvertToBase"

                        bmp.Dispose()
                        ErrorStringVar = "BMP Dispose"
                    End Using
                Else
                    ReturnString = "<font color='red'>" & stringToBeMadeAnImage & "</font>"
                End If
            End If



        Catch ex As Exception
            'Returning a text value.
            If Trim(valign_text) <> "" Then
                ReturnString = "<span  unselectable='on' class=""red_text underline"" style='display:inline-block; vertical-align:" & Trim(valign_text) & "'>" & stringToBeMadeAnImage & "</span>"
            Else
                ReturnString = "<span  unselectable='on' class=""red_text underline"">" & stringToBeMadeAnImage & "</span>"
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "DisplayFunctions", ex.Message)
            Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " TextToImage: " & ErrorStringVar & " " & Replace(ex.Message, "'", "''"), Nothing, 0, 0, 0, 0, 0)
        Finally
            size = Nothing
            pngData = Nothing

            bmp.Dispose()
            bmp = Nothing

            memoryStream.Close()
            memoryStream.Dispose()
            memoryStream = Nothing

            canvas.Dispose()
            canvas = Nothing
        End Try

        Return ReturnString
    End Function

    ''' <summary>
    ''' I want to create a function that will do the follow based on parameters.
    ''' Write the on click of all the Details Links Out.
    ''' This makes it easier. What if we decide to rename a page?
    ''' What if we don't want to use onclick anymore and want to use a href?
    ''' Plus we won't have to write the huge javascript statement anymore. We'll just pass this function.
    ''' This makes it a lot easier.
    ''' Basically pass the ID you're looking to display, 0's for everything else and the function
    ''' should take care of the rest.
    ''' </summary>
    ''' <param name="AircraftID">AC ID (0 if none)</param>
    ''' <param name="CompanyID">Company ID (0 if none)</param>
    ''' <param name="ContactID">Contact ID (0 if none)</param>
    ''' <param name="JournalID">Journal ID (0 if none)</param>
    ''' <param name="DisplayFullLink">This is a boolean that basically is used to say whether you're going to display the full link or not.
    ''' If you use this, it will include the starting tag. If you use this and the Link text, it will also include the link text and the closing tag.
    ''' Technically you could pass DisplayFullLink as true, yet not pass any link text. This would start the link, but not include the ending tag.</param>
    '''<param name="LinkClass">Link Class (if full link)</param>
    '''<param name="LinkText">Link Text (if full Link)</param>
    ''' <param name="ExtraParam">Extra parameter, like the map variable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function WriteDetailsLink(ByVal AircraftID As Long, ByVal CompanyID As Long, ByVal ContactID As Long, ByVal JournalID As Long, ByVal DisplayFullLink As Boolean, ByVal LinkText As String, ByVal LinkClass As String, ByVal ExtraParam As String) As String
        Dim ReturnURL As String = ""
        Dim NewWindow As Boolean = True

        If DisplayFullLink = True Then
            ReturnURL = "<a "
        End If

        If NewWindow = True Then
            If AircraftID <> 0 Then 'Aircraft Link
                ReturnURL += "href='#' onclick=""javascript:window.open('DisplayAircraftDetail.aspx?acid=" & AircraftID & IIf(JournalID <> 0, "&jid=" & JournalID, "") & ExtraParam & "');return false;"" "
            ElseIf CompanyID <> 0 And ContactID = 0 Then 'Company Link


                ReturnURL += "href='#' onclick=""javascript:window.open('DisplayCompanyDetail.aspx?compid=" & CompanyID & IIf(JournalID <> 0, "&jid=" & JournalID, "") & ExtraParam & "');return false;"" "
            ElseIf ContactID <> 0 Then
                ReturnURL += "href='#' onclick=""javascript:window.open('DisplayContactDetail.aspx?compid=" & CompanyID & IIf(JournalID <> 0, "&jid=" & JournalID, "") & "&conid=" & ContactID & ExtraParam & "');return false;"" "
            End If
        Else


            If AircraftID <> 0 Then 'Aircraft Link
                ReturnURL += "href='#' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" & AircraftID & IIf(JournalID <> 0, "&jid=" & JournalID, "") & ExtraParam & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"" "
            ElseIf CompanyID <> 0 And ContactID = 0 Then 'Company Link


                ReturnURL += "href='#' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" & CompanyID & IIf(JournalID <> 0, "&jid=" & JournalID, "") & ExtraParam & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"" "
            ElseIf ContactID <> 0 Then
                ReturnURL += "href='#' onclick=""javascript:load('DisplayContactDetail.aspx?compid=" & CompanyID & IIf(JournalID <> 0, "&jid=" & JournalID, "") & "&conid=" & ContactID & ExtraParam & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"" "
            End If

        End If

        If LinkText <> "" And DisplayFullLink = True Then
            If LinkClass <> "" Then
                ReturnURL += " class='" & LinkClass & "'"
            End If
            ReturnURL += ">" & LinkText & "</a>"
        End If

        Return ReturnURL
    End Function

    Public Shared Function WriteYachtDetailsLink(ByVal YachtID As Long, ByVal DisplayFullLink As Boolean, ByVal LinkText As String, ByVal LinkClass As String, ByVal ExtraParam As String) As String
        Dim ReturnURL As String = ""
        Dim NewWindow As Boolean = True

        If DisplayFullLink = True Then
            ReturnURL = "<a "
        End If

        If NewWindow = True Then
            ReturnURL += "href='DisplayYachtDetail.aspx?yid=" & YachtID & ExtraParam & "' target='_blank' "
        Else
            ReturnURL += "href='#' onclick=""javascript:load('DisplayYachtDetail.aspx?yid=" & YachtID & ExtraParam & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"" "

        End If

        If ExtraParam <> "" Then
            ReturnURL += ExtraParam
        End If

        If LinkText <> "" And DisplayFullLink = True Then
            If LinkClass <> "" Then
                ReturnURL += " class='" & LinkClass & "'"
            End If
            ReturnURL += ">" & LinkText & "</a>"
        End If

        Return ReturnURL
    End Function

    Public Shared Sub SingleModelLookupAndFill(ByRef makeModelDynamic As DropDownList, ByRef masterPage As Object)

        Dim ModelSelectedLookup As Long = 0
        Dim ModelSelectedValue As String = ""

        Try

            If Not IsNothing(HttpContext.Current.Session.Item("tabAircraftModel")) Then
                If IsNumeric(HttpContext.Current.Session.Item("tabAircraftModel")) Then
                    ModelSelectedLookup = commonEvo.ReturnAmodIDForItemIndex(HttpContext.Current.Session.Item("tabAircraftModel")).ToString
                End If
            End If

            Dim TempTable As New DataTable
            TempTable = masterPage.aclsData_Temp.GetAircraft_MakeModels("", "", HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, HttpContext.Current.Session.Item("localSubscription").crmJets_Flag, HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag, HttpContext.Current.Session.Item("localSubscription").crmTurboprops, "")
            For Each r As DataRow In TempTable.Rows
                If Not IsDBNull(r("amod_model_name")) And Not IsDBNull(r("amod_make_name")) Then
                    If Not IsDBNull(r("amod_id")) Then
                        If r("amod_id") = ModelSelectedLookup Then
                            ModelSelectedValue = r("amod_type_code").ToString & "|" & r("amod_airframe_type_code").ToString & "|" & r("amod_make_name").ToString & "|" & r("amod_id")
                        End If
                        makeModelDynamic.Items.Add(New ListItem(r("amod_make_name").ToString & " " & r("amod_model_name").ToString, r("amod_type_code").ToString & "|" & r("amod_airframe_type_code").ToString & "|" & r("amod_make_name").ToString & "|" & r("amod_id")))
                    End If
                End If
            Next
            TempTable.Dispose()

            makeModelDynamic.SelectedValue = ModelSelectedValue
        Catch ex As Exception

            commonLogFunctions.forceLogError("UserError", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + "): " + ex.Message.Trim

        End Try

    End Sub

    Public Shared Sub SingleModelLookupAndFillListbox(ByRef makeModelDynamic As Object, ByRef masterPage As Object)

        Dim ModelSelectedLookup As Long = 0
        Dim ModelSelectedValue As String = ""

        Try

            Dim selectedModels As New List(Of Integer)

            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSelectedModels.trim) Then
                Dim ModelSession As String() = Split(HttpContext.Current.Session.Item("localUser").crmSelectedModels, ",")

                For Each selectedItem In ModelSession
                    selectedModels.Add(CInt(Trim(selectedItem)))
                Next
            End If

            Dim TempTable As New DataTable
            TempTable = masterPage.aclsData_Temp.GetAircraft_MakeModels("", "", HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, HttpContext.Current.Session.Item("localSubscription").crmJets_Flag, HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag, HttpContext.Current.Session.Item("localSubscription").crmTurboprops, "")
            For Each r As DataRow In TempTable.Rows
                If Not IsDBNull(r("amod_model_name")) And Not IsDBNull(r("amod_make_name")) Then
                    If Not IsDBNull(r("amod_id")) Then

                        Dim newLI As New ListItem
                        newLI.Value = r("amod_id")
                        newLI.Text = r("amod_make_name").ToString & " " & r("amod_model_name").ToString

                        If selectedModels.Contains(r("amod_id")) Then
                            newLI.Selected = True
                        End If

                        makeModelDynamic.Items.Add(newLI)
                    End If
                End If
            Next
            TempTable.Dispose()


        Catch ex As Exception

            commonLogFunctions.forceLogError("UserError", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + "): " + ex.Message.Trim

        End Try

    End Sub
    Public Shared Function WriteModelDetailsLink(ByVal modelID As Long, ByVal LinkText As String, ByVal displayFullLink As Boolean)
        Dim returnURL As String = ""
        Dim newWindow As Boolean = True
        If displayFullLink = True Then
            returnURL = "<a "
        End If

        ' added MSW - to change for performance spec page 
        If HttpContext.Current.Session.Item("isMobile") Then
            newWindow = True
        End If

        If newWindow Then
            returnURL += "href='view_template.aspx?ViewID=1&ViewName=Model%20Market%20Summary&amod_id=" & modelID & "' target='_blank' "
        Else
            returnURL += "href='#' onclick=""javascript:load('DisplayModelDetail.aspx?id=" & modelID & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"""
        End If

        If LinkText <> "" And displayFullLink = True Then
            returnURL += ">" & LinkText & "</a>"
        End If
        Return returnURL
    End Function
    Public Shared Function WriteModelLink(ByVal modelID As Long, ByVal LinkText As String, ByVal displayFullLink As Boolean)
        Dim returnURL As String = ""
        Dim newWindow As Boolean = True
        If displayFullLink = True Then
            returnURL = "<a "
        End If

        If HttpContext.Current.Session.Item("isMobile") Then
            newWindow = True
        End If

        If newWindow Then
            returnURL += "href='view_template.aspx?noMaster=false&ViewID=1&ViewName=Model%20Market%20Summary&amod_id=" & modelID & "' target='_blank' "
        Else
            returnURL += "href='#' onclick=""javascript:load('view_template.aspx?noMaster=false&ViewID=1&ViewName=Model%20Market%20Summary&amod_id=" & modelID & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"""
        End If

        If LinkText <> "" And displayFullLink = True Then
            returnURL += ">" & LinkText & "</a>"
        End If
        Return returnURL
    End Function
    ''' <summary>
    ''' Function to write out notes and reminder links.
    ''' </summary>
    ''' <param name="UniqueID"></param>
    ''' <param name="AircraftID"></param>
    ''' <param name="DisplayFullLink"></param>
    ''' <param name="ExtraParam"></param>
    ''' <param name="LinkText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function WriteNotesRemindersLinks(ByVal UniqueID As Long, ByVal AircraftID As Long, ByVal CompanyID As Long, ByVal YachtID As Long, ByVal DisplayFullLink As Boolean, ByVal ExtraParam As String, ByVal LinkText As String) As String
        Dim ReturnURL As String = ""
        If DisplayFullLink = True Then
            ReturnURL = "<a "
        End If
        If AircraftID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:window.open('WebForm1.aspx?acid=" & AircraftID & "&lnoteID=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "')"""
        ElseIf YachtID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:window.open('WebForm1.aspx?ytid=" & YachtID & "&lnoteID=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "')"""
        ElseIf CompanyID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:window.open('WebForm1.aspx?compid=" & CompanyID & "&lnoteID=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "');return false;"""
        End If


        If LinkText <> "" And DisplayFullLink = True Then
            ReturnURL += ">" & LinkText & "</a>"
        End If

        Return ReturnURL
    End Function

    Public Shared Function CRM_WriteNotesRemindersLinks(ByVal UniqueID As Long, ByVal AircraftID As Long, ByVal CompanyID As Long, ByVal YachtID As Long, ByVal DisplayFullLink As Boolean, ByVal ExtraParam As String, ByVal LinkText As String) As String
        Dim ReturnURL As String = ""
        If DisplayFullLink = True Then
            ReturnURL = "<a "
        End If
        If AircraftID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:load('edit_note.aspx?ac_ID=" & AircraftID & "&id=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"""
        ElseIf CompanyID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:load('edit_note.aspx?comp_ID=" & CompanyID & "&id=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"""
        End If


        If LinkText <> "" And DisplayFullLink = True Then
            ReturnURL += " class=""text_underline"">" & LinkText & "</a>"
        End If

        Return ReturnURL
    End Function
    Public Shared Function WriteNotesRemindersLinks_Action(ByVal UniqueID As Long, ByVal AircraftID As Long, ByVal CompanyID As Long, ByVal YachtID As Long, ByVal DisplayFullLink As Boolean, ByVal ExtraParam As String, ByVal LinkText As String) As String
        Dim ReturnURL As String = ""
        If DisplayFullLink = True Then
            ReturnURL = "<a "
        End If

        If AircraftID <> 0 And CompanyID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:load('WebForm1.aspx?acid=" & AircraftID & "&compid=" & CompanyID & "&lnoteID=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"""
        ElseIf AircraftID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:load('WebForm1.aspx?acid=" & AircraftID & "&lnoteID=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"""
        ElseIf YachtID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:load('WebForm1.aspx?ytid=" & YachtID & "&lnoteID=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"""
        ElseIf CompanyID <> 0 Then
            ReturnURL += "href='#' onclick=""javascript:load('WebForm1.aspx?compid=" & CompanyID & "&lnoteID=" & UniqueID & IIf(ExtraParam <> "", ExtraParam, "") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"""
        End If


        If LinkText <> "" And DisplayFullLink = True Then
            ReturnURL += ">" & LinkText & "</a>"
        End If

        Return ReturnURL
    End Function

#Region "Functions for Appraisals Display"
    ''' <summary>
    ''' Function to write out appraisal links.
    ''' </summary>
    ''' <param name="AircraftID"></param>
    ''' <param name="DisplayFullLink"></param>
    ''' <param name="ExtraParam"></param>
    ''' <param name="LinkText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function WriteAppraisalsLinks(ByVal AircraftID As Long, ByVal AppraisalID As Long, ByVal DisplayFullLink As Boolean, ByVal ExtraParam As String, ByVal LinkText As String) As String
        Dim ReturnURL As String = ""
        If DisplayFullLink = True Then
            ReturnURL = "<a "
        End If

        ReturnURL += "href='#' onclick=""javascript:load('Appraisal.aspx?"

        'Edit mode?
        If AppraisalID > 0 Then
            ReturnURL += "ID=" & AppraisalID.ToString & "&"
        End If

        ReturnURL += "acID=" & AircraftID.ToString & IIf(ExtraParam <> "", ExtraParam, "") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"""

        If LinkText <> "" And DisplayFullLink = True Then
            ReturnURL += ">" & LinkText & "</a>"
        End If

        Return ReturnURL
    End Function
    Public Shared Function Display_Appraisals(ByVal appraisalsTable As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal aircraftID As Long) As String
        Dim ReturnString As String = ""

        If Not IsNothing(appraisalsTable) Then
            If appraisalsTable.Rows.Count > 0 Then
                ReturnString = "<table width=""100%"" cellpadding=""5"" cellspacing=""0"" class=""data_aircraft_grid"">"
                ReturnString += "<tr class=""header_row"">"
                ReturnString += "<td align='left' valign='top'><b>DATE</b></td>"
                ReturnString += "<td align='left' valign='top'><b>TYPE</b></td>"
                ReturnString += "<td align='left' valign='top'><b>AFTT</b></td>"
                ReturnString += "<td align='left' valign='top'><b>CYCLES</b></td>"
                ReturnString += "<td align='left' valign='top'><b>ASKING $</b></td>"
                ReturnString += "<td align='left' valign='top'><b>TAKE $</b></td>"
                ReturnString += "<td align='left' valign='top'><b>ESTIMATED $</b></td>"
                ReturnString += "</tr>"

                For Each r As DataRow In appraisalsTable.Rows
                    ReturnString += "<tr>"


                    ReturnString += "<td align='left' valign='top'>"
                    'Date
                    If Not IsDBNull(r("acappr_date")) Then
                        If IsDate(r("acappr_date")) Then
                            ReturnString += "<a href=" & DisplayFunctions.WriteAppraisalsLinks(aircraftID, r("acappr_id"), False, "", "") & " class='special'>"
                            ReturnString += FormatDateTime(r("acappr_date"), DateFormat.ShortDate)
                            ReturnString += "</a>"
                        End If
                    End If
                    ReturnString += "</td>"

                    ReturnString += "<td align='left' valign='top'>"
                    'Type
                    If Not IsDBNull(r("acappr_type")) Then
                        If Not String.IsNullOrEmpty(r("acappr_type")) Then
                            ReturnString += r("acappr_type").ToString
                        End If
                    End If
                    ReturnString += "</td>"

                    ReturnString += "<td align='left' valign='top'>"
                    'AFTT
                    If Not IsDBNull(r("acappr_airframe_tot_hrs")) Then
                        If Not String.IsNullOrEmpty(r("acappr_airframe_tot_hrs")) Then
                            ReturnString += FormatNumber(r("acappr_airframe_tot_hrs").ToString, 0)
                        End If
                    End If
                    ReturnString += "</td>"

                    ReturnString += "<td align='left' valign='top'>"
                    'Cycles
                    If Not IsDBNull(r("acappr_airframe_tot_landings")) Then
                        If Not String.IsNullOrEmpty(r("acappr_airframe_tot_landings")) Then
                            ReturnString += FormatNumber(r("acappr_airframe_tot_landings").ToString, 0)
                        End If
                    End If
                    ReturnString += "</td>"

                    ReturnString += "<td align='left' valign='top'>"
                    'Asking
                    If Not IsDBNull(r("acappr_asking_price")) Then
                        If Not String.IsNullOrEmpty(r("acappr_asking_price")) Then
                            ReturnString += clsGeneral.clsGeneral.ConvertIntoThousands(r("acappr_asking_price").ToString)
                        End If
                    End If
                    ReturnString += "</td>"

                    ReturnString += "<td align='left' valign='top'>"
                    'Take
                    If Not IsDBNull(r("acappr_take_price")) Then
                        If Not String.IsNullOrEmpty(r("acappr_take_price")) Then
                            ReturnString += clsGeneral.clsGeneral.ConvertIntoThousands(r("acappr_take_price").ToString)
                        End If
                    End If
                    ReturnString += "</td>"


                    ReturnString += "<td align='left' valign='top'>"
                    'Estimated
                    If Not IsDBNull(r("acappr_est_value")) Then
                        If Not String.IsNullOrEmpty(r("acappr_est_value")) Then
                            ReturnString += clsGeneral.clsGeneral.ConvertIntoThousands(r("acappr_est_value").ToString)
                        End If
                    End If
                    ReturnString += "</td>"

                    ReturnString += "</tr>"
                Next


                ReturnString += " </td>"
                ReturnString += " </tr>"
                ReturnString += "</table><br />"
            Else
                ReturnString = "<table width=""100%"" cellpadding=""5"" cellspacing=""0"" class=""data_aircraft_grid""><tr><td align='left' valign='top'>"
                ReturnString += "<span>No current appraisals available for display.</span>"
                ReturnString += "</td></tr></table><br />"
            End If
        End If

        Return ReturnString
    End Function
#End Region

    Public Shared Function ApplyXMLFormatting(ByVal strInput As String) As String

        strInput = Replace(strInput, "&", "&amp;")
        strInput = Replace(strInput, "'", "&apos;")
        strInput = Replace(strInput, """", "&quot;")
        strInput = Replace(strInput, ">", "&gt;")
        strInput = Replace(strInput, "<", "&lt;")

        Return strInput
    End Function

    Public Shared Function WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(ByVal acID As Long, ByVal compID As Long, ByVal noteType As String, ByVal rememberTab As Integer) As String
        Dim LinkText As String = ""
        Dim ReturnString As String = ""

        Select Case noteType
            Case "B"
                LinkText = "<img src='images/gold_plus_sign.png' alt='Add Prospect' title='Add Prospect'  />"
            Case "A"
                LinkText = "<img src='images/blue_plus_sign.png' alt='Add Note' title='Add Note' />"
            Case "P"
                LinkText = "<img src='images/red_plus_sign.png' alt='Add Action Item' title='Add Action Item' />"
        End Select
        ReturnString = "<a href='#' class='no_text_underline' onclick=""javascript:load('edit.aspx?prospectACID=" & acID & "&comp_ID=" & compID & "&source=JETNET&type=company&action=checkforcreation&note_type=" & noteType & "&from=view&rememberTab=" & rememberTab & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');""><strong>" & LinkText & "</strong></a>"

        Return ReturnString
    End Function
    ''' <summary>
    ''' Displays Analytics Table Summarized by Date. Used on home page analytics tab and individual detail page.
    ''' </summary>
    ''' <param name="ResultsTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 

    Public Shared Function CreateAnalyticsSummaryByDate(ByVal ResultsTable As DataTable, ByVal master As Object, ByVal captionText As String, ByVal PercentString As String, ByVal showGraph As Boolean, Optional ByVal bIsDealer As Boolean = False, Optional ByVal has_stats As Boolean = False, Optional ByVal CSSClassSwap As String = "data_aircraft_grid", Optional ByVal showTable As Boolean = True, Optional ByVal graph_num As String = "") As String
        Dim OutputString As String = ""
        Dim cssClass As String = ""
        Dim Old_Year As Integer = 0
        If Not IsNothing(ResultsTable) Then
            If ResultsTable.Rows.Count > 0 Then
                If captionText <> "" Then
                    OutputString += "<div class=""subHeader"">" & captionText & "</div>"
                End If

                OutputString = "<table width=""" + IIf(Not String.IsNullOrEmpty(PercentString.Trim), PercentString, "100") + "%"" cellpadding=""3"" cellspacing=""0"" class=""" & CSSClassSwap & " float_right fullWidthMobile"">"

                If showGraph = True Then
                    OutputString += "<tr class=""header_row""><td align=""left"" valign=""top"" colspan=""" + IIf(bIsDealer, "4", "3") + """><span class=""medium_text text_align_center padding""><b>Clicks per Month (Last 12 Months)</b></span></td></tr>"
                    OutputString += "<tr><td align=""center"" valign=""top"" colspan=""" + IIf(bIsDealer, "4", "3") + """><div id=""visualization" & graph_num & """ style=""width: 100%; height: 300px;"" class=""resizeChart""></div></td></tr>"
                End If


                If showTable Then
                    OutputString += "<tr class=""header_row"">"
                    OutputString += "<td align=""left"" valign=""top""><b class=""title"">Year</b></td>"
                    OutputString += "<td align=""left"" valign=""top""><b class=""title"">Month</b></td>"
                    OutputString += "<td align=""right"" valign=""top""><b class=""title"">Evolution Clicks</b></td>"

                    If bIsDealer And has_stats = True Then
                        OutputString += "<td align=""right"" valign=""top""><b class=""title"">Global Clicks</b></td>"
                    End If

                    OutputString += "</tr>"

                    For Each r As DataRow In ResultsTable.Rows

                        If CInt(r.Item("YTYEAR").ToString) <> Old_Year Then
                            OutputString += "<tr class=""" + cssClass + """><td align=""left"" valign=""top""><b class=""title"">" + r.Item("YTYEAR").ToString + "</b></td>"
                        Else
                            OutputString += "<tr class=""" + cssClass + """><td align=""left"" valign=""top""></td>"
                        End If

                        OutputString += "<td align=""left"" valign=""top"">" + MonthName(r.Item("YTMONTH")).ToUpper + "</td>"

                        OutputString += "<td align=""right"" valign=""top"">" + r.Item("tcount").ToString + "</td>"

                        If bIsDealer And has_stats = True Then
                            OutputString += "<td align=""right"" valign=""top"">" + r.Item("gcount").ToString + "</td>"
                        End If

                        OutputString += "</tr>"

                        Old_Year = CInt(r.Item("YTYEAR").ToString)

                        If cssClass = "" Then
                            cssClass = "alt_row"
                        Else
                            cssClass = ""
                        End If

                    Next
                End If
                OutputString += "</table>"
            Else
                OutputString = "<table width='100%' cellpadding='4' cellspacing='0'><tr><td align='left' valign='top'><p align='left'>Welcome " & HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName.ToString & " " & HttpContext.Current.Session.Item("localUser").crmLocalUserLastName.ToString & ".<br />There is no current analytics data.</td></tr></table>"
            End If
        Else
            'error logging here.
            master.LogError("home.aspx.vb - CreateAircraftAnalytics() - " & " " & master.aclsData_Temp.class_error)
            'clear error for data layer class
            master.aclsData_Temp.class_error = ""
        End If
        ResultsTable.Dispose()
        Return OutputString
    End Function

    ''' <summary>
    ''' This function excepts an HTML control and the field name for the textual display.
    ''' It builds the search text display based on that and the type of the control.
    ''' </summary>
    ''' <param name="HTMLSearchControl"></param>
    ''' <param name="TextFieldName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function BuildSearchTextDisplay(ByVal HTMLSearchControl As Object, ByVal TextFieldName As String) As String
        Dim ReturnString As String = ""
        If TypeOf HTMLSearchControl Is TextBox Then
            HTMLSearchControl = DirectCast(HTMLSearchControl, TextBox)
            ReturnString = TextFieldName & ": " & HTMLSearchControl.text & "<br />"
        End If
        If TypeOf HTMLSearchControl Is ListBox Then
            HTMLSearchControl = DirectCast(HTMLSearchControl, ListBox)
            ReturnString = TextFieldName & ": "

            For i = 0 To HTMLSearchControl.Items.Count - 1
                If HTMLSearchControl.Items(i).Selected Then
                    If HTMLSearchControl.Items(i).Value <> "" Then 'Here we check to see if there is a value, meaning there's no selection
                        If UCase(HTMLSearchControl.items(i).value) <> "ALL" Then 'Checking to make sure ALL isn't checked, if it is, we don't need to search
                            ReturnString += " " & HTMLSearchControl.items(i).text & ","
                        End If
                    End If
                End If

            Next
            ReturnString = ReturnString.TrimEnd(",")
            ReturnString = ReturnString & "<br />"
        End If
        If TypeOf HTMLSearchControl Is DropDownList Then
            HTMLSearchControl = DirectCast(HTMLSearchControl, DropDownList)
            ReturnString = TextFieldName & ": " & HTMLSearchControl.selecteditem.text & "<br />"
        End If
        If TypeOf HTMLSearchControl Is CheckBox Then
            HTMLSearchControl = DirectCast(HTMLSearchControl, CheckBox)
            ReturnString = TextFieldName & ": " & HTMLSearchControl.checked & "<br />"
        End If
        If TypeOf HTMLSearchControl Is String Then
            ReturnString = TextFieldName & ": " & HTMLSearchControl & "<br />"
        End If
        Return ReturnString
    End Function

    ''' <summary>
    ''' a.	String – Includes, Equals,  Begins With
    ''' b.	Numeric – Equals, Less Than, Greater Than, Between
    ''' c.	Date - Equals, Less Than, Greater Than, Between
    ''' This fils up the dropdown for the advanced search items.
    ''' Error reporting is included.
    ''' </summary>
    ''' <param name="TypeOfDropdown"></param>
    ''' <param name="dropDown"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Fill_Dropdown(ByVal TypeOfDropdown As String, ByVal dropDown As DropDownList, ByVal cef_values As String) As DropDownList

        If cef_values = "" Then 'if the values for this field are blank, it's not a dropdown list.
            Select Case TypeOfDropdown
                Case "String", "Char"
                    'dropDown.Items.Add(New ListItem("", "")) 'Empty
                    dropDown.Items.Add(New ListItem("Includes", "Includes"))
                    dropDown.CssClass = "display_none includes"
          'dropDown.Items.Add(New ListItem("Equals", "Equals"))
          'dropDown.Items.Add(New ListItem("Begins With", "Begins With"))
                Case "Date", "Numeric", "Year"
                    dropDown.Items.Add(New ListItem("", "")) 'Empty
                    dropDown.Items.Add(New ListItem("Equals", "Equals"))
                    dropDown.Items.Add(New ListItem("Less Than", "Less Than"))
                    dropDown.Items.Add(New ListItem("Greater Than", "Greater Than"))
                    dropDown.Items.Add(New ListItem("Between", "Between"))
            End Select
        Else 'else, it is
            ' dropDown.Items.Add(New ListItem("", "")) 'Empty
            dropDown.Items.Add(New ListItem("Equals", "Equals"))
            dropDown.CssClass = "display_none"
        End If
        If HttpContext.Current.Request.Form("project_search") = "Y" Then
            dropDown.SelectedValue = HttpContext.Current.Request.Form(dropDown.ID)
        End If


        Return dropDown
    End Function

    ''' <summary>
    ''' These are some rules to display the format rules for each advanced search box.
    ''' Some error reporting is included.
    ''' </summary>
    ''' <param name="TypeOfField"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayFormatRules(ByVal TypeOfField As String) As String
        DisplayFormatRules = ""
        Select Case TypeOfField
            Case "Date"
                Return """mm/dd/yyyy"", for Between use ""mm/dd/yyyy: mm/dd/yyyy"""
            Case "Numeric", "Year"
                Return """nnnn"", for Between use ""nnnn: nnnn"""
            Case "String"
                Return "Enter string as alphanumeric."
            Case "Dropdown"
                Return "Select from drop down."
        End Select
    End Function

    ''' <summary>
    ''' This is going to select information in the type of object that is, whether the selection is
    ''' The session, or pulled from advanced search.
    ''' Includes error logging.
    ''' </summary>
    ''' <param name="contr"></param>
    ''' <param name="selection"></param>
    ''' <remarks></remarks>
    Public Shared Sub SelectInformation(ByVal contr As Object, ByVal selection As Object)
        If Not IsNothing(selection) Then
            If Not String.IsNullOrEmpty(selection) Then
                If TypeOf contr Is ListBox Then
                    Dim MultipleSelection As Array
                    'We split the answer.
                    MultipleSelection = selection.Split("##")

                    contr.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                    'that the page defaults to.
                    For MultipleSelectionCount = 0 To UBound(MultipleSelection)
                        For ListBoxCount As Integer = 0 To contr.Items.Count() - 1
                            Dim ar As String = UCase(MultipleSelection(MultipleSelectionCount))
                            Dim va As String = UCase(contr.Items(ListBoxCount).Value)
                            If Replace(UCase(contr.Items(ListBoxCount).Value), "'", "") = UCase(MultipleSelection(MultipleSelectionCount)) Then
                                If UCase(MultipleSelection(MultipleSelectionCount)) <> "" Then
                                    contr.Items(ListBoxCount).Selected = True
                                End If
                            End If
                        Next
                    Next
                ElseIf TypeOf contr Is DropDownList Then
                    contr.selectedvalue = selection
                ElseIf TypeOf contr Is TextBox Then
                    contr.text = selection
                ElseIf TypeOf contr Is CheckBox Then
                    contr.checked = selection
                End If
            End If
        End If
    End Sub

    Public Shared Function BuildViewMarketAbsorptionGauge(ByVal absorp_rate As Object, ByVal largeGraph As Boolean, Optional ByVal temp_color As String = "") As String
        Dim gaugeString As String = ""
        Dim min_value As Integer = 0
        Dim max_value As Integer = 36
        'jScript += "$(""#" & valueGraphTextGuage1.ClientID & """).val($('#" & pngGuage1.ClientID & " img').prop('src'));"


        'absorp_rate = FormatNumber((FormatNumber(SalesPerTimeframe, 2) / localCriteria.ViewCriteriaTimeSpan), 2)
        'absorp_rate = (FormatNumber(ac_for_sale, 2) / FormatNumber(absorp_rate, 2))


        'us_reg & "/" & (th_stage - us_reg)

        ' BUILD GUAGE SECTION----------------------
        'Dim UsBased As Long = GetUSBasedByModel(NEW_PDF_AMOD_ID)
        'gaugeString = BuildUSForeignGauge(UsBased, th_stage)
        gaugeString &= "  function initGauge_AbsorptionRate() { var gauge = new RadialGauge({ renderTo:  'scripted-gauge',"
        If largeGraph Then
            gaugeString &= " width: 400, height: 300, "
        Else
            gaugeString &= " width: 190, height: 200, "
        End If

        gaugeString &= " units: false, " ' units: """ & absorp_rate & " Months"","
        gaugeString &= " fontTitleSize: ""34"","
        gaugeString &= " fontTitle:""Arial"","
        gaugeString &= "colorTitle:  '#4f5050',"
        'Color of the bottom text, like 9 months under the absorption rate gauge.
        gaugeString &= " colorUnits: ""#000000"","
        'Size of bottom text.
        gaugeString &= " fontUnitsSize: ""30"","

        If IsNumeric(absorp_rate) And Not Double.IsInfinity(absorp_rate) And Not Double.IsNaN(absorp_rate) Then
            gaugeString &= " title:  '" & FormatNumber(absorp_rate, 1) & IIf(absorp_rate > 9, " Mnths", " Months") & "',"
        Else
            gaugeString &= " title:  '0 Months',"
        End If

        gaugeString &= "  startAngle: 90, SweepAngle: 180, valueBox: false, ticksAngle: 180, exactTicks: true, "
        gaugeString &= "  minValue: " & min_value & ",  maxValue: " & max_value & ","
        gaugeString &= "majorTicks: false, strokeTicks: false, "
        gaugeString &= " minorTicks: 0,"

        gaugeString &= "  colorPlate: ""rgba(0,0,0,0)""," 'Make background transparent.
        gaugeString &= " colorStrokeTicks: '#fff', "

        If Trim(temp_color) <> "" Then
            gaugeString &= " highlights: false, animation: false, barWidth:25, barProgress: true, colorBarProgress: '" & Trim(temp_color) & "', needle: false, colorBar: '#eee',"
        Else
            gaugeString &= " highlights: false, animation: false, barWidth:25, barProgress: true, colorBarProgress: '#078fd7', needle: false, colorBar: '#eee',"
        End If


        gaugeString &= " numbersMargin: -18, "
        gaugeString &= "colorNumbers:  '#1d3566',"
        gaugeString &= "colorNeedle:  '#1d3566',"
        gaugeString &= "colorNeedleEnd:  '#2a62aa',"
        gaugeString &= "    borderShadowWidth: 0,"
        gaugeString &= "    borders: false,"
        gaugeString &= "  value: " & absorp_rate & ""
        gaugeString &= "}).draw();"



        gaugeString &= "  var canvas = document.getElementById(""scripted-gauge"");"
        gaugeString &= "  var img = canvas.toDataURL(""image/png"");}"
        ' gaugeString &= " document.getElementById('" & pngGauge1.ClientID & "').innerHTML = '<img src=""' + img + '"">'"
        '  gaugeString &= "$(""#" & valueGraphTextGauge.ClientID & """).val(img);"


        Return gaugeString
    End Function
    Public Shared Function BuildViewMarketSummaryBox(ByVal extraCssClass As String, ByVal table_color As String, ByVal totalinOpCount As Double, ByVal ac_for_sale As Integer, ByVal ac_exclusive_sale As Integer, ByVal ac_lease As Integer, ByVal per As Double, ByVal per2 As Double, ByVal per3 As Double, Optional ByVal us_reg As Long = 0, Optional ByVal th_stage As Long = 0) As String
        Dim getMarketStatus As String = ""
        getMarketStatus += "<div class=""Box marginTop""><table id='marketPlaceStatusTable' cellspacing='0' cellpadding='0' class='formatTable datagrid " & table_color & " " & extraCssClass & "' width='100%'><thead>" & vbCrLf '<tr>&nbsp;</tr>
        getMarketStatus += "<tr><th valign='top' align='left' colspan='2' class='center upperCase'><span class=""subHeader"">Market Summary</span></th></tr>" & vbCrLf
        getMarketStatus += "</thead><tbody>"


        If CLng(totalinOpCount) > 0 Then
            getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>In Operation:&nbsp;</td><td align='left'>" & FormatNumber(totalinOpCount, 0, True, False, True) & "</td></tr>" & vbCrLf
        Else
            getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>In Operation:&nbsp;</td><td align='left'>0</td></tr>" & vbCrLf
        End If

        If CLng(ac_for_sale) > 0 Then
            If Not HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
                getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>For Sale:&nbsp;</td><td align='left'>" & FormatNumber(ac_for_sale, 0, True, False, True) & "&nbsp;<span class='tiny'>(" & FormatNumber(per, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
            Else
                getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>For Sale:&nbsp;</td><td align='left'>" & FormatNumber(ac_for_sale, 0, True, False, True) & " &nbsp;<span class='tiny'>(" & FormatNumber(per, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
            End If
        Else
            getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>For Sale:&nbsp;</td><td align='left'>0 <span class='tiny'>(0% of In Operation)</span></td></tr>" & vbCrLf
        End If

        If Not HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
            ' THIS IS FOR ON EXCLUSIVE %
            If CLng(ac_exclusive_sale) > 0 Then
                getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>On Exclusive:&nbsp;</td><td align='left'>" & FormatNumber(ac_exclusive_sale, 0, True, False, True) & " <span class='tiny'>(" & FormatNumber(per2, 1) & "% of For Sale)</span></td></tr>" & vbCrLf
            Else
                getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>On Exclusive:&nbsp;</td><td align='left'>(0% of For Sale on Exclusive)</span></td></tr>" & vbCrLf
            End If

        End If

        If CLng(ac_lease) > 0 Then
            getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>Leased:&nbsp;</td><td align='left'>" & FormatNumber(ac_lease, 0, True, False, True) & " <span class='tiny'>(" & FormatNumber(per3, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
        Else
            getMarketStatus += "<tr><td valign='top' align='left' class='upperCase'>Leased:&nbsp;</td><td align='left'>0 <span class='tiny'>(0% of In Operation)</span></td></tr>" & vbCrLf
        End If

        ' getMarketStatus += "<tr><td valign=""top"" align=""left"">US/International:&nbsp;</td><td align=""left"">" + IIf(us_reg > 0, us_reg.ToString + " / " + (th_stage - us_reg).ToString, "") + "</td></tr>"

        getMarketStatus += "</tbody></table></div>"
        Return getMarketStatus
    End Function
    Public Shared Function BuildViewOwnershipBox(ByVal extraCSSClass As String, ByVal w_owner As Integer, ByVal s_owner As Integer, ByVal f_owner As Integer, ByVal totalInOpCount As Integer, ByVal alllow As Integer, ByVal allhigh As Integer) As String
        Dim fleetHTML As New StringBuilder
        fleetHTML.Append("<div class=""Box marginTop""><table id='ownershipTable' cellspacing='0' cellpadding='0' width='100%' class='formatTable " & extraCSSClass & " datagrid blue'>")
        fleetHTML.Append("<tr class='aircraft_list noBorder'><th valign='middle' align='center' colspan='2' class=""center upperCase""><span class=""subHeader"">OWNERSHIP <strong class=""tiny display_block"">IN OPERATION</strong></span></th></tr>")

        If w_owner > 0 Then
            fleetHTML.Append("<tr><td valign='top' align='left'>Whole:&nbsp;</td><td align='right'>" + FormatNumber(w_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHTML.Append("<tr><td valign='top' align='left'>Whole:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If

        If s_owner > 0 Then
            fleetHTML.Append("<tr><td valign='top' align='left' >Shared:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(s_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHTML.Append("<tr><td valign='top' align='left' >Shared:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If

        If f_owner > 0 Then
            fleetHTML.Append("<tr><td valign='top' align='left'>Fractional:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(f_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHTML.Append("<tr><td valign='top' align='left'>Fractional:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If

        If totalInOpCount > 0 Then
            fleetHTML.Append("<tr><td valign='top' align='left' nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(totalInOpCount, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHTML.Append("<tr><td valign='top' align='left' nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If


        'If (alllow > 0) And (allhigh > 0) And (allhigh <> CInt(Now().Year)) Then
        '  fleetHTML.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range " + alllow.ToString + " - " + allhigh.ToString + "</td></tr>")
        'ElseIf (alllow > 0) And (allhigh = CInt(Now().Year)) Then
        '  fleetHTML.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range " + alllow.ToString + " - To Present</td></tr>")
        'Else
        '  fleetHTML.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range&nbsp;:&nbsp;N/A</td></tr>")
        'End If

        fleetHTML.Append("</table></div>")
        Return fleetHTML.ToString
    End Function
    Public Shared Function BuildViewFleetCompBox(ByVal extraCSSClass As String, ByVal yearRange As String, ByVal afttRange As String, ByVal UsForeign As String) As String
        Dim fleetHtml As New StringBuilder
        fleetHtml.Append("<div class=""Box marginTop""><table id='lifeCycleTable' width='100%' cellspacing='0' cellpadding='0' class='formatTable " & extraCSSClass & " datagrid blue'>")
        fleetHtml.Append("<tr class='aircraft_list noBorder'><th valign='top' align='center' colspan='2'  class=""center upperCase""><span class=""subHeader"">COMPOSITION</span></th></tr>")

        If Not String.IsNullOrEmpty(yearRange) Then
            fleetHtml.Append("<tr><td valign='top' align='left' nowrap='nowrap'>MFR Year Range:&nbsp;</td><td align='right'>" & yearRange & "</td></tr>")
        End If

        If Not String.IsNullOrEmpty(afttRange) Then
            fleetHtml.Append("<tr><td valign='top' align='left' >AFTT Range:&nbsp;</td><td align='right'>" & afttRange & "</td></tr>")
        End If

        If Not String.IsNullOrEmpty(UsForeign) Then
            fleetHtml.Append("<tr><td valign='top' align='left' >US/Foreign:&nbsp;</td><td align='right'>" & UsForeign & "</td></tr>")
        End If

        fleetHtml.Append("</table></div>")
        Return fleetHtml.ToString
    End Function
    Public Shared Function BuildViewLifecycleBox(ByVal extraCSSClass As String, ByVal o_stage As Integer, ByVal t_stage As Integer, ByVal th_stage As Integer, ByVal f_stage As Integer, ByVal totalcount As Integer) As String
        Dim fleetHtml As New StringBuilder
        fleetHtml.Append("<div class=""Box marginTop""><table id='lifeCycleTable' width='100%' cellspacing='0' cellpadding='0' class='formatTable " & extraCSSClass & "  datagrid blue'>")
        fleetHtml.Append("<tr class='aircraft_list noBorder'><th valign='top' align='center' colspan='2' class=""center upperCase""><span class=""subHeader"">LIFE CYCLE</strong></th></tr>")

        If o_stage > 0 Then
            fleetHtml.Append("<tr><td valign='top' align='left' nowrap='nowrap'>In Production:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(o_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHtml.Append("<tr><td valign='top' align='left' nowrap='nowrap'>In Production:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If

        If t_stage > 0 Then
            fleetHtml.Append("<tr><td valign='top' align='left' >At MFR:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(t_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHtml.Append("<tr><td valign='top' align='left' >At MFR:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If

        If th_stage > 0 Then
            fleetHtml.Append("<tr><td valign='top' align='left' >In Operation:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(th_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHtml.Append("<tr><td valign='top' align='left' >In Operation:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If

        If f_stage > 0 Then
            fleetHtml.Append("<tr><td valign='top' align='left' >Retired/In Storage:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(f_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHtml.Append("<tr><td valign='top' align='left' >Retired/In Storage:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If

        If totalcount > 0 Then
            fleetHtml.Append("<tr><td valign='top' align='left'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(totalcount, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
        Else
            fleetHtml.Append("<tr><td valign='top' align='left'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
        End If

        fleetHtml.Append("</table></div>")
        Return fleetHtml.ToString
    End Function
    Public Shared Function BuildViewMarketCompositionBox(ByVal extraCSSClass As String, ByVal table_color As String, ByVal forsaleavlow As String, ByVal forsaleavg As String, ByVal forsaleavghigh As String, ByVal mfr_low_fs As Double, ByVal mfr_avg_fs As Double, ByVal mfr_high_fs As Double, ByVal lowdays As Double, ByVal days As Double, ByVal highdays As Double, ByVal aftt_low_fs As Double, ByVal aftt_avg_fs As Double, ByVal aftt_high_fs As Double, Optional ByVal displayEValues As Boolean = False, Optional ByVal evalues_low As Double = 0, Optional ByVal evalues_avg As Double = 0, Optional ByVal evalues_high As Double = 0, Optional ByVal landings_high As Long = 0, Optional ByVal landings_low As Long = 0, Optional ByVal landings_avg As Long = 0, Optional ByVal us_reg As Long = 0, Optional ByVal th_stage As Long = 0, Optional ByVal display_Only_ForSale As Boolean = False, Optional TotalAircraft As Double = 0, Optional ChartNumber As Double = 0, Optional ByVal ToggleMarketItems As Boolean = False, Optional ToggleFeature As Boolean = False) As String
        Dim GetMarketStatus As String = ""
        Dim value_color As String = "#078fd7"

        GetMarketStatus += "<div class=""Box marginTop""><table id='lifeCycleTable'  cellspacing='0' cellpadding='0' class='formatTable  datagrid " & table_color & " " & extraCSSClass & "' width='100%'><thead>" & vbCrLf

        If display_Only_ForSale = True Then
            GetMarketStatus += "<tr><th valign='top' align='left' class='upperCase'><span class=""subHeader"">COMPOSITION OF MARKET&nbsp;</span></th><th valign='top' class='upperCase right'>Low&nbsp;</th><th valign='top' class='upperCase right'>Avg&nbsp;</th><th valign='top' class='upperCase right'>High&nbsp;</th></tr>" & vbCrLf
        Else
            GetMarketStatus += "<tr><th valign='top' align='left' class='upperCase'><span class=""subHeader"">Composition&nbsp;</span></th><th valign='top' class='upperCase right'>Low&nbsp;</th><th valign='top' class='upperCase right'>Avg&nbsp;</th><th valign='top' class='upperCase right'>High&nbsp;</th></tr>" & vbCrLf
        End If

        GetMarketStatus += "</thead><tbody>"

        If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
            If ToggleMarketItems = False Then
                GetMarketStatus += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>Asking Price:&nbsp;</td>"
                GetMarketStatus += "<td align='right'>" & forsaleavlow & "</td>"
                GetMarketStatus += "<td align='right'>" & forsaleavg & "</td>"  '  " &  & "
                GetMarketStatus += "<td align='right'>" & forsaleavghigh & "</td>"
                GetMarketStatus += "</tr>"
            End If
        End If

        GetMarketStatus += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>MFR Year:&nbsp;</td>"
        If mfr_low_fs = 50000 Then
            GetMarketStatus += "<td align='right'>0</td>"
        Else
            GetMarketStatus += "<td align='right'>" & mfr_low_fs & "</td>"
        End If

        GetMarketStatus += "<td align='right'>" & mfr_avg_fs & "</td>"
        GetMarketStatus += "<td align='right'>" & mfr_high_fs & "</td>"
        GetMarketStatus += "</tr>"

        If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
            If ToggleMarketItems = False Then
                GetMarketStatus += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>Days on Market:&nbsp;</td>"

                If lowdays = 50000 Or lowdays = 10000 Then
                    GetMarketStatus += "<td align='right'>0</td>"
                Else
                    GetMarketStatus += "<td align='right'>" & FormatNumber(lowdays, 0) & "</td>"
                End If

                GetMarketStatus += "<td align='right'>" & FormatNumber(days, 0) & "</td>"
                GetMarketStatus += "<td align='right'>" & FormatNumber(highdays, 0) & "</td>"
                GetMarketStatus += "</tr>"
            End If
        End If

        GetMarketStatus += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>Airframe Time:&nbsp;</td>"

        If aftt_low_fs = 50000 Then
            GetMarketStatus += "<td align='right'>0</td>"
        Else
            GetMarketStatus += "<td align='right'>" & FormatNumber(aftt_low_fs, 0) & "</td>"
        End If

        GetMarketStatus += "<td align='right'>" & FormatNumber(aftt_avg_fs, 0) & "</td>"
        GetMarketStatus += "<td align='right'>" & FormatNumber(aftt_high_fs, 0) & "</td>"
        GetMarketStatus += "</tr>"

        If landings_low > 0 Or landings_avg > 0 Or landings_high > 0 Then
            GetMarketStatus += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>Landings:&nbsp;</td>"

            GetMarketStatus += "<td align='right'>" & FormatNumber(landings_low, 0) & "</td>"
            GetMarketStatus += "<td align='right'>" & FormatNumber(landings_avg, 0) & "</td>"
            GetMarketStatus += "<td align='right'>" & FormatNumber(landings_high, 0) & "</td>"
            GetMarketStatus += "</tr>"
        End If


        If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
            If ToggleMarketItems = False Then
                If displayEValues Then
                    If evalues_high > 0 Or evalues_avg > 0 Or evalues_low > 0 Then

                        GetMarketStatus += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'><a href=""javascript:void(0);""  title=""Asset Insight Estimated Value - Click to Learn More"" onclick='javascript:openSmallWindowJS(""/help/documents/809.pdf"",""HelpWindow"");'><span class=""text_underline " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>" & Constants.eValues_Refer_Name & ":</span></a>&nbsp;</td>"
                        GetMarketStatus += "<td align='right'><span class=""" & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>"

                        If evalues_low > 0 Then
                            GetMarketStatus += clsGeneral.clsGeneral.ConvertIntoThousands(evalues_low)
                        End If

                        GetMarketStatus += "</span></td>"

                        GetMarketStatus += "<td align='right'><span class=""" & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>"
                        If evalues_avg > 0 Then
                            GetMarketStatus += clsGeneral.clsGeneral.ConvertIntoThousands(evalues_avg)
                        End If

                        GetMarketStatus += "</span></td>"
                        GetMarketStatus += "<td align='right'><span class=""" & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>"

                        If evalues_high > 0 Then
                            GetMarketStatus += clsGeneral.clsGeneral.ConvertIntoThousands(evalues_high)
                        End If

                        GetMarketStatus += "</span></td>"
                        GetMarketStatus += "</tr>"
                    End If
                End If
            End If
        End If


        GetMarketStatus += "</tbody></table>"

        GetMarketStatus += "<br/><table id='lifeCycleTable'  cellspacing='0' cellpadding='0' class='formatTable blue datagrid' width='100%'>"
        GetMarketStatus += "<tr class=""noBorder""><td valign=""middle"">"



        If TotalAircraft > 0 Then
            GetMarketStatus += "<span class=""subHeader"" style=""padding-bottom:0px;"">TOTAL AIRCRAFT: "
            GetMarketStatus += "</span><br />"
        End If
        If us_reg > 0 Or th_stage > 0 Then
            GetMarketStatus += "<span class=""subHeader"" style=""padding-bottom:0px;"">US/International:&nbsp;</span>"
        End If

        GetMarketStatus += "</td><td align=""right"" valign=""middle"">"

        If TotalAircraft > 0 Then
            GetMarketStatus += "<span class=""display_block"">" & TotalAircraft.ToString & "</span><br />"
        End If


        If us_reg > 0 Or th_stage > 0 Then
            GetMarketStatus += "<span class=""display_block"">" & us_reg.ToString + " / " + (th_stage - us_reg).ToString & "</span>"
        End If
        GetMarketStatus += "</td>"

        If ToggleFeature = False Then
            If TotalAircraft > 0 Then
                GetMarketStatus += "<td valign=""top"" align=""right""><div id=""visualization" & ChartNumber.ToString & """ style=""width:152px;height:162px;""></div></td>"
            End If
        End If

        GetMarketStatus += "</tr>"


        GetMarketStatus += "</tbody></table></div>"
        Return GetMarketStatus

    End Function
    Public Shared Function ConvertToTitleCase(ByRef input As String) As String
        Dim ti As Globalization.TextInfo = Threading.Thread.CurrentThread.CurrentCulture.TextInfo
        'if a word is all in upper case, ToTitleCase method is not able to convert to title case. So we would make the input string all lower case.
        Return ti.ToTitleCase(input.ToLower())

    End Function

    Public Shared Sub SetPagingItem(ByVal company_per_page_dropdown As BulletedList)
        Dim TempRoundedPagingCount As Integer = 0

        'Once we add the rest of the pages paging, these needs to be moved 
        'Temporary paging count, rounding to 10's
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        TempRoundedPagingCount = Math.Round(HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage / 10.0) * 10

        'Making sure 500 is max (for now at least)
        If TempRoundedPagingCount > 500 Then
            TempRoundedPagingCount = 500
        End If

        'Temporarily resetting the session variable for this session.
        HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage = TempRoundedPagingCount

        'Let's figure out the paging situation, what should it be defaulted to?
        company_per_page_dropdown.Items.Clear()
        company_per_page_dropdown.Items.Add(New ListItem(TempRoundedPagingCount, TempRoundedPagingCount))
    End Sub
    ''' <summary>
    ''' Function to write the javascript to build the javascript map.
    ''' </summary>
    ''' <param name="controltoRegisterScript">This is generally going to be Me, however if you are using an update panel, you'll want to do me.updatepanel (example of that is on the ac page).</param>
    ''' <param name="Geocode">This determines what Draw Map Function you're using. Are you passing an address or do you have the Lat/Long?</param>
    ''' <param name="TypeOfCont">Generally Me.GetType()</param>
    ''' <remarks></remarks>
    Public Shared Sub BuildJavascriptMap(ByVal controltoRegisterScript As Control, ByVal TypeOfCont As System.Type, ByVal Geocode As Boolean, ByVal mapName As String, ByVal mapType As Integer, ByVal MultiPoint As Boolean, ByVal customImage As Boolean)
        Dim Javas As String = ""

        If customImage = True Then
            Javas = "airport_image;" & vbNewLine

            Javas = "//setting up images" & vbNewLine
            Javas += "airport_image = new google.maps.MarkerImage('../images/evoPlane.png', new google.maps.Size(32, 32), new google.maps.Point(0,0), new google.maps.Point(16, 20));" & vbNewLine
        End If

        If Geocode = False Then
            Javas += "function DrawMap(latitude, longitude, titleMarker){" & vbNewLine

            If MultiPoint = False Then
                Javas += " myOptions = {" & vbNewLine
                Javas += "zoom: 2, " & vbNewLine
                Javas += "center: new google.maps.LatLng(latitude, longitude)," & vbNewLine
                If mapType = 0 Then
                    Javas += " mapTypeId: google.maps.MapTypeId.ROADMAP" & vbNewLine
                Else
                    Javas += " mapTypeId: google.maps.MapTypeId.HYBRID" & vbNewLine
                End If
                Javas += "}" & vbNewLine
                Javas += "map = new google.maps.Map(document.getElementById(""" & mapName & """), myOptions);" & vbNewLine

            End If

            Javas += "marker = new google.maps.Marker({ //create a marker for the map." & vbNewLine
            Javas += "position: new google.maps.LatLng(latitude, longitude)," & vbNewLine
            If customImage = True Then
                Javas += "icon: airport_image," & vbNewLine
            End If
            Javas += "map: map" & vbNewLine
            Javas += "});" & vbNewLine
            Javas += "AddListener(marker,titleMarker, 1, map);" & vbNewLine
            Javas += "}" & vbNewLine & vbNewLine
        Else
            Javas += "function DrawMap(address, titleMarker){" & vbNewLine
            Javas += " geocoder = new google.maps.Geocoder();" & vbNewLine

            If MultiPoint = False Then

                Javas += " myOptions = {" & vbNewLine
                Javas += " zoom: 2," & vbNewLine
                Javas += " center: new google.maps.LatLng(0, 0)," & vbNewLine
                If mapType = 0 Then
                    Javas += " mapTypeId: google.maps.MapTypeId.ROADMAP" & vbNewLine
                Else
                    Javas += " mapTypeId: google.maps.MapTypeId.HYBRID" & vbNewLine
                End If
                Javas += "       }" & vbNewLine
                Javas += "  map = new google.maps.Map(document.getElementById(""" & mapName & """), myOptions);" & vbNewLine

            End If

            Javas += " geocoder.geocode( {'address': address}, function(results, status) {" & vbNewLine
            Javas += " if (status == google.maps.GeocoderStatus.OK) { " & vbNewLine
            Javas += " map.setCenter(results[0].geometry.location);" & vbNewLine
            Javas += "  var marker2 = new google.maps.Marker({" & vbNewLine
            If customImage = True Then
                Javas += "icon: airport_image," & vbNewLine
            End If
            Javas += " map: map," & vbNewLine
            Javas += " zoom:1," & vbNewLine
            Javas += " position: results[0].geometry.location" & vbNewLine
            Javas += "  });" & vbNewLine
            Javas += "   } else {" & vbNewLine
            Javas += "  alert(""Geocoder was not successful for the following reason: "" + status);" & vbNewLine
            Javas += "   }" & vbNewLine
            Javas += "AddListener(marker2,titleMarker, 1, map);" & vbNewLine
            Javas += "  }); " & vbNewLine

            Javas += "  }" & vbNewLine
        End If



        Javas += "function AddListener(marker,title, counter, temp_map) {" & vbNewLine
        Javas += "var infowindow = new google.maps.InfoWindow();" & vbNewLine
        Javas += "//Then go ahead and add the listener marker to the map." & vbNewLine
        Javas += "google.maps.event.addListener(marker, 'click', (function(marker, counter) {" & vbNewLine
        Javas += "return function() {" & vbNewLine
        Javas += "infowindow.setContent(title);" & vbNewLine
        Javas += "infowindow.open(temp_map, marker);" & vbNewLine
        Javas += "}" & vbNewLine
        Javas += "})(marker, counter));" & vbNewLine
        Javas += "}" & vbNewLine



        Javas += "function AddHover(marker,title, counter, temp_map, ID) {" & vbNewLine

        Javas += "var infowindow = new google.maps.InfoWindow();" & vbNewLine
        Javas += "//Then go ahead and add the listener marker to the map." & vbNewLine

        Javas += "// Show tooltip on mouseover event." & vbNewLine
        Javas += "google.maps.event.addListener(marker, 'mouseover', function() {" & vbNewLine
        Javas += "infowindow.setContent(title);" & vbNewLine
        Javas += "infowindow.open(temp_map, marker);" & vbNewLine
        Javas += " });" & vbNewLine
        Javas += "// Hide tooltip on mouseout event." & vbNewLine
        Javas += "google.maps.event.addListener(marker, 'mouseout', function() {" & vbNewLine
        Javas += "    infowindow.close();" & vbNewLine
        Javas += "});" & vbNewLine

        Javas += " google.maps.event.addListener(marker, 'click', (function(marker, counter) {" & vbNewLine
        Javas += "return function() {" & vbNewLine
        Javas += "var uri = 'DisplayAircraftDetail.aspx?acid=';"
        Javas += "javascript:load(uri + ID,'','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"
        Javas += "}" & vbNewLine
        Javas += "})(marker, counter));" & vbNewLine
        Javas += "}" & vbNewLine

        System.Web.UI.ScriptManager.RegisterStartupScript(controltoRegisterScript, TypeOfCont, "Draw Map JavaSc", Javas, True)
    End Sub

    ''' <summary>
    ''' Builds a dynamic table cell
    ''' </summary>
    ''' <param name="Bold"></param>
    ''' <param name="text"></param>
    ''' <param name="vAlign"></param>
    ''' <param name="hAlign"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function BuildTableCell(ByVal Bold As Boolean, ByVal text As String, ByVal vAlign As System.Web.UI.WebControls.VerticalAlign, ByVal hAlign As System.Web.UI.WebControls.HorizontalAlign) As TableCell
        Dim TD As New TableCell
        TD.VerticalAlign = vAlign
        TD.HorizontalAlign = hAlign
        TD.Font.Bold = Bold
        TD.Text = text

        BuildTableCell = TD
    End Function

    ''' <summary>
    ''' Returns folder image based on type.
    ''' </summary>
    ''' <param name="cfolder_method"></param>
    ''' <param name="cfolder_hide_flag"></param>
    ''' <param name="cfolder_share"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Shared Function ReturnFolderImage(ByVal cfolder_method As String, ByVal cfolder_hide_flag As String, ByVal cfolder_share As String) As String
        Dim Method_File As String = "" 'holds part of the method (active/static) for filename to avoid repitition.
        Dim hidden_File As String = "" 'holds part of the filename if hidden
        Dim ReturnURL As String = ""

        'check method, whether active or static.
        If cfolder_method = "A" Then
            Method_File = "refresh_"
        ElseIf cfolder_method = "S" Then
            Method_File = "static_"
        End If
        'check hidden, is the folder hidden?
        If cfolder_hide_flag = "Y" Then
            hidden_File = "_hidden"
        End If

        'final check for shared.
        If cfolder_share = "Y" Then
            ReturnURL = "images/" & Method_File & "shared_folder" & hidden_File & ".png" + My.Settings.SCRIPT_VERSION.ToString
        Else
            ReturnURL = "images/" & Method_File & "regular_folder" & hidden_File & ".png" + My.Settings.SCRIPT_VERSION.ToString
        End If
        Return ReturnURL
    End Function


    ''' <summary>
    ''' Fills all the local items, notes, reminders on company/aircraft page.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub DisplayLocalItems(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal AircraftID As Long, ByVal CompanyID As Long, ByVal YachtID As Long, ByVal notes_label As Label, ByVal action_label As Label, ByVal DisplayCompany As Boolean, ByVal DisplayAC As Boolean, ByVal DisplayYacht As Boolean, Optional ByVal FilterNotes As Boolean = False, Optional ByVal FilterNotesCount As Integer = 0, Optional ByRef showViewAllNoteLink As Boolean = False, Optional ByRef CRMView As Boolean = False, Optional ByRef CRMSource As String = "JETNET", Optional ByVal prospects_label As Label = Nothing, Optional ByVal shortenDescription As Boolean = False, Optional ByVal displayContact As Boolean = False, Optional ByVal contactID As Long = 0, Optional showAllOpenProspects As Boolean = False)

        Dim HoldingTable As New DataTable
        Dim NotesTable As New DataTable
        Dim ActionsTable As New DataTable
        Dim ProspectsTable As New DataTable


        'Doing a query on all the different items for an ac and then filtering them will
        'take away the need to do multiple queries on an aircraft for action items/notes, local notes table items.
        'First we should check and see if this is cloud notes or standard notes
        If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
            If CRMView Then
                If DisplayAC Then
                    If AircraftID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CRMSource = "CLIENT", AircraftID, 0), IIf(CRMSource = "CLIENT", 0, AircraftID), "", DisplayAC, DisplayCompany, False, displayContact)
                    ElseIf CompanyID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CRMSource = "CLIENT", CompanyID, 0), IIf(CRMSource = "CLIENT", 0, CompanyID), "", False, True, False, displayContact)
                    ElseIf contactID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CRMSource = "CLIENT", contactID, 0), IIf(CRMSource = "CLIENT", 0, contactID), "", DisplayAC, DisplayCompany, False, displayContact)
                    End If
                Else
                    If CompanyID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CRMSource = "CLIENT", CompanyID, 0), IIf(CRMSource = "CLIENT", 0, CompanyID), "", DisplayAC, DisplayCompany, False, displayContact)
                    ElseIf contactID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CRMSource = "CLIENT", contactID, 0), IIf(CRMSource = "CLIENT", 0, contactID), "", DisplayAC, DisplayCompany, False, displayContact)
                    End If
                End If
            Else
                If DisplayAC Then
                    If AircraftID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(0, AircraftID, "", DisplayAC, DisplayCompany, displayContact)
                    End If
                Else
                    If CompanyID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(0, CompanyID, "", DisplayAC, DisplayCompany, displayContact)
                    ElseIf contactID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CRMSource = "CLIENT", contactID, 0), IIf(CRMSource = "CLIENT", 0, contactID), "", DisplayAC, DisplayCompany, False, displayContact)
                    End If

                End If
            End If
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
            If DisplayAC Then
                If AircraftID > 0 Then
                    HoldingTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(AircraftID, "", DisplayAC, DisplayCompany, DisplayYacht, False)
                End If
            ElseIf DisplayYacht Then
                If YachtID > 0 Then
                    HoldingTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(YachtID, "", DisplayAC, DisplayCompany, DisplayYacht, False)
                End If
            Else
                If CompanyID > 0 Then
                    HoldingTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(CompanyID, "", DisplayAC, DisplayCompany, DisplayYacht, False)
                End If
            End If
        End If

        'Clone the tables before filtering to get the schemas.
        If Not IsNothing(HoldingTable) Then
            NotesTable = HoldingTable.Clone
            ActionsTable = HoldingTable.Clone
            ProspectsTable = HoldingTable.Clone

            'here's the notes table filter.
            Dim afiltered_Note As DataRow() = HoldingTable.Select("lnote_status = 'A'")

            If FilterNotes = True Then 'Only if we're filtering notes
                For i As Integer = 0 To FilterNotesCount - 1 'looping through the table based on the filtered #
                    If i < afiltered_Note.Length Then 'Only adding if the length is more than the row count we're on.
                        NotesTable.ImportRow(afiltered_Note(i)) 'importing the note
                        NotesTable.AcceptChanges() 'accepting the changes.
                    End If
                Next

                'This is the last thing we need to do, which is basically going to check to see if we need a view all link. If we do, we're going to send it back to the calling function.
                If (FilterNotesCount) < afiltered_Note.Length Then
                    showViewAllNoteLink = True
                End If
            Else 'We are not filtering notes and we can just carry on as normal.
                For Each atmpDataRow_Note In afiltered_Note
                    NotesTable.ImportRow(atmpDataRow_Note)
                Next
            End If

            'here's the action items filter.

            Dim afiltered_Action As DataRow() = HoldingTable.Select("lnote_status = 'P'")
            For Each atmpDataRow_Action In afiltered_Action
                ActionsTable.ImportRow(atmpDataRow_Action)
            Next

            If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                ' WHERE (lnote_opportunity_status in ('I','A','O','C')) and (lnote_status in ('B','O')) 

                If CRMView Then
                    Dim afiltered_Prospects As DataRow() = HoldingTable.Select("lnote_status in ('B','O')" & IIf(showAllOpenProspects = True, " and lnote_opportunity_status in ('O')", ""), "clicomp_name asc")
                    For Each atmpDataRow_Prospects In afiltered_Prospects
                        ProspectsTable.ImportRow(atmpDataRow_Prospects)
                    Next
                Else
                    Dim afiltered_Prospects As DataRow() = HoldingTable.Select("lnote_status in ('B','O')" & IIf(showAllOpenProspects = True, " and lnote_opportunity_status in ('O')", ""))
                    For Each atmpDataRow_Prospects In afiltered_Prospects
                        ProspectsTable.ImportRow(atmpDataRow_Prospects)
                    Next
                End If
            End If

        Else
            'prep for error
            Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " : " & Replace("DisplayFunctions.vb - DisplayLocalItems HoldingTable() - " & " " & aclsData_Temp.class_error, "'", "''"), Nothing, 0, 0, 0, 0, 0)
            'clear error for data layer class
            aclsData_Temp.class_error = ""
        End If

        If Not IsNothing(NotesTable) And Not IsNothing(notes_label) Then
            If CRMView Then
                notes_label.Text = CRMDisplay_Notes_Or_Actions(NotesTable, aclsData_Temp, False, IIf(DisplayAC, False, True), IIf(DisplayCompany, False, True), IIf(DisplayYacht, False, True), False, True, False, False, CRMView, CRMSource, False, IIf(showViewAllNoteLink Or shortenDescription, True, False))
            Else
                notes_label.Text = DisplayFunctions.Display_Notes_Or_Actions(NotesTable, aclsData_Temp, False, IIf(DisplayAC, False, True), IIf(DisplayCompany, False, True), IIf(DisplayYacht, False, True), False, True, False, False, IIf(showViewAllNoteLink Or shortenDescription, True, False))
            End If
        End If

        If Not IsNothing(NotesTable) Then
            NotesTable.Dispose()
        End If

        If Not IsNothing(ActionsTable) And Not IsNothing(action_label) Then
            If CRMView Then
                action_label.Text = CRMDisplay_Notes_Or_Actions(ActionsTable, aclsData_Temp, False, IIf(DisplayAC, False, True), IIf(DisplayCompany, False, True), IIf(DisplayYacht, False, True), False, False, True, False, CRMView, CRMSource)
            Else
                action_label.Text = DisplayFunctions.Display_Notes_Or_Actions(ActionsTable, aclsData_Temp, False, IIf(DisplayAC, False, True), IIf(DisplayCompany, False, True), IIf(DisplayYacht, False, True), False, False, True)
            End If
        End If

        If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True And CRMView Then
            If Not IsNothing(prospects_label) And Not IsNothing(prospects_label) Then
                If Not IsNothing(ProspectsTable) Then
                    If CRMView Then
                        If DisplayAC = True Then
                            prospects_label.Text = CRMDisplay_Notes_Or_Actions(ProspectsTable, aclsData_Temp, False, IIf(DisplayAC, False, True), IIf(DisplayCompany, False, True), IIf(DisplayYacht, False, True), False, False, True, False, CRMView, CRMSource, True, False, displayContact)
                        Else
                            prospects_label.Text = CRMDisplay_Notes_Or_Actions(ProspectsTable, aclsData_Temp, False, IIf(DisplayAC, False, True), IIf(DisplayCompany, False, True), IIf(DisplayYacht, False, True), False, False, True, False, CRMView, CRMSource, True, False, displayContact)
                        End If
                    Else
                        prospects_label.Text = DisplayFunctions.Display_Notes_Or_Actions(ProspectsTable, aclsData_Temp, False, IIf(DisplayAC, False, True), IIf(DisplayCompany, False, True), IIf(DisplayYacht, False, True), False, False, True, False, displayContact)
                    End If
                End If
            End If
        End If

        If Not IsNothing(ProspectsTable) Then
            ProspectsTable.Dispose()
        End If

        If Not IsNothing(HoldingTable) Then
            HoldingTable.Dispose()
        End If

        If Not IsNothing(HoldingTable) Then
            HoldingTable.Dispose()
        End If

    End Sub


    ''' <summary>
    ''' Displays Notes/Action Items, parameters may need to be passed to distinguish the two.
    ''' </summary>
    ''' <param name="notesTable"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="DisplayHeaderDate"></param>
    ''' <param name="DisplayACInfo"></param>
    ''' <param name="DisplayCompInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Display_Notes_Or_Actions(ByVal notesTable As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal DisplayHeaderDate As Boolean, ByVal DisplayACInfo As Boolean, ByVal DisplayCompInfo As Boolean, ByVal DisplayYachtInfo As Boolean, ByVal useUL As Boolean, ByVal ShowingNotes As Boolean, ByVal ShowingActions As Boolean, Optional ByVal is_home_page As Boolean = False, Optional ByRef ShortenNotes As Boolean = False) As String
        Dim aTempTable As New DataTable
        Dim ReturnString As String = ""
        Dim today As Date = FormatDateTime(Now(), 2)
        Dim week As Integer = Weekday(today)
        Dim monthint As Integer = Month(today)
        Dim monthdis As String = MonthName(monthint)
        Dim weekdis As String = WeekdayName(week)
        Dim yeardis As Integer = Year(today)
        Dim daydis As Integer = Day(today)
        Dim oldweekdis As Integer = 0
        Dim oldmonthint As Integer = 0
        Dim olddaydis As Integer = 0


        If Not IsNothing(notesTable) Then
            If notesTable.Rows.Count > 0 Then
                ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid"">"

                'Let's set up a display that doesn't show the header for the AC Details Page. 
                If DisplayHeaderDate = False Then
                    ' ReturnString += "<tr><td align=""left"" valign=""top"">"
                    If useUL = True Then
                        ReturnString += "<ul class=""circle"">"
                    End If
                End If

                For Each r As DataRow In notesTable.Rows
                    ' If r("lnote_jetnet_ac_id") > 0 Then
                    Dim timeofday = TimeValue(today)
                    Dim AC_Link_Text As String = ""
                    Dim Yacht_Link_Text As String = ""
                    Dim COMPANY_Link_Text As String = ""
                    Dim JETNET_AC_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_ac_id")), r("lnote_jetnet_ac_id"), 0)
                    Dim JETNET_COMPANY_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_comp_id")), r("lnote_jetnet_comp_id"), 0)
                    Dim JETNET_YACHT_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_yacht_id")), r("lnote_jetnet_yacht_id"), 0)
                    Dim DisplayDate As String = ""
                    If ShowingActions = True Then
                        DisplayDate = IIf(Not IsDBNull(r("lnote_schedule_start_date")), Format(CDate(r("lnote_schedule_start_date")), "MM/dd/yyyy") & " - ", "") & ""
                    Else
                        DisplayDate = IIf(Not IsDBNull(r("lnote_entry_date")), Format(CDate(r("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ""
                    End If

                    'Formatting for Action Items
                    'Edit - Rick Wanner - 1986 BEECHJET 400 - S/N# RJ-2, Reg# N369EA - Validate the aircraft is for sale and get asking price.
                    today = IIf(Not IsDBNull(r("lnote_schedule_start_date")), r("lnote_schedule_start_date"), Now())

                    week = Weekday(today)
                    daydis = Day(today)
                    weekdis = WeekdayName(week)
                    monthint = Month(today)
                    monthdis = Left(MonthName(monthint), 3)

                    If DisplayACInfo = True Then
                        If JETNET_AC_ID <> 0 Then
                            aTempTable = aclsData_Temp.GetJETNET_AC_NAME(JETNET_AC_ID, "")
                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then
                                    AC_Link_Text = CommonAircraftFunctions.Display_Aircraft_Information_For_Link(aTempTable, True, 0)
                                End If
                            End If
                            aTempTable.Dispose()
                        End If
                    End If

                    If DisplayYachtInfo = True Then
                        If JETNET_YACHT_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.DisplayYachtByID(JETNET_YACHT_ID)
                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then
                                    Yacht_Link_Text = Display_Yacht_Information_For_Link(aTempTable)
                                End If
                            End If
                            aTempTable.Dispose()
                        End If
                    End If

                    If DisplayCompInfo = True Then
                        If JETNET_COMPANY_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.GetLimited_CompanyInfo_ID(JETNET_COMPANY_ID, "JETNET", 0)
                            COMPANY_Link_Text = CompanyFunctions.Display_Company_Information_For_Link(aTempTable, False, 0)
                        End If
                    End If


                    If DisplayHeaderDate = True Then
                        If daydis <> olddaydis Or week <> oldweekdis Or monthint <> oldmonthint Then
                            If olddaydis <> 0 And oldweekdis <> 0 And oldmonthint <> 0 Then
                                If useUL = True Then
                                    ReturnString += "</ul>"
                                End If
                            End If
                            ReturnString += "<tr class=""header_row"">"
                            ReturnString += "<td align=""left"" valign=""top"">"
                            ReturnString += "<strong class=""blue_text"">" & weekdis & ", " & monthdis & " " & daydis & " " & Year(today) & "</strong>"
                            ReturnString += "</td>"
                            ReturnString += "</tr>"
                            ReturnString += "<tr>"
                            ReturnString += " <td align=""left"" valign=""top"">"
                            If useUL = True Then
                                ReturnString += "<ul class=""circle"">"
                            End If
                        End If
                    End If

                    If useUL = True Then
                        ReturnString += "<li>"
                    Else
                        ReturnString += "<tr><td align='left' valign='top'><span class='li'>"
                    End If

                    If is_home_page = False Then
                        If HttpContext.Current.Session.Item("localUser").crmUserContactID = r("lnote_user_id") Or HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag Then
                            ReturnString += WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit") & " - "
                        Else
                            If Not HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then 'If Not administrator view link only.
                                ReturnString += WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "View") & " - "
                            Else 'otherwise they can go ahead and edit.
                                ReturnString += WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit") & " - "
                            End If
                        End If
                    End If


                    If DisplayHeaderDate = False Then
                        ReturnString += DisplayDate
                    End If

                    If ShowingNotes = True Then         ' if its an notes item 
                        ReturnString += r("lnote_user_name").ToString
                    ElseIf ShowingActions = True Then   ' if its an action item

                        If Not IsDBNull(r("lnote_schedule_start_date")) Then
                            ReturnString += FormatDateTime(r("lnote_schedule_start_date").ToString, DateFormat.LongTime)
                            If is_home_page = False Then
                                ReturnString += " - "
                            End If
                        End If

                        If is_home_page = False Then
                            ReturnString += r("lnote_user_name").ToString
                        End If
                    End If




                    ReturnString += IIf(AC_Link_Text <> "", " - " & AC_Link_Text & "</a>", "") & " - " & IIf(COMPANY_Link_Text <> "", COMPANY_Link_Text & " - ", "") & " " & " " & IIf(Yacht_Link_Text <> "", Yacht_Link_Text & " - ", "") & IIf(ShortenNotes = False Or ShowingNotes = False, r("lnote_note").ToString, IIf(Len(r("lnote_note")) > 100, Left(r("lnote_note").ToString, 100) & "..", r("lnote_note").ToString)) & "."


                    If is_home_page = True Then
                        If HttpContext.Current.Session.Item("localUser").crmUserContactID = r("lnote_user_id") Or HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag Then
                            ReturnString += " - " & WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit")
                        Else
                            If Not HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then 'If Not administrator view link only.
                                ReturnString += " - " & WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "View")
                            Else 'otherwise they can go ahead and edit.
                                ReturnString += " - " & WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit")
                            End If
                        End If
                    End If


                    If useUL = True Then
                        ReturnString += "</li>"
                    Else
                        ReturnString += "</span></td></tr>"
                    End If
                    oldweekdis = week
                    oldmonthint = monthint
                    olddaydis = daydis
                    'End If
                Next

                ReturnString += "</ul>"
                ReturnString += " </td>"
                ReturnString += " </tr>"
                ReturnString += "</table>"
            Else
                ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid""><tr><td align='left' valign='top'  class=""noBorder"">"
                ReturnString += "<span>No current " & IIf(ShowingActions = True, "action items", "notes") & " available for display.</span>"
                ReturnString += "</td></tr></table>"
            End If
        End If

        Return ReturnString
    End Function

    Public Shared Function Display_Notes_Or_Actions_HB_Admin(ByVal notesTable As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal DisplayHeaderDate As Boolean, ByVal DisplayACInfo As Boolean, ByVal DisplayCompInfo As Boolean, ByVal DisplayYachtInfo As Boolean, ByVal useUL As Boolean, ByVal ShowingNotes As Boolean, ByVal ShowingActions As Boolean, Optional ByVal is_home_page As Boolean = False, Optional ByRef ShortenNotes As Boolean = False) As String
        Dim aTempTable As New DataTable
        Dim ReturnString As String = ""
        Dim today As Date = FormatDateTime(Now(), 2)
        Dim week As Integer = Weekday(today)
        Dim monthint As Integer = Month(today)
        Dim monthdis As String = MonthName(monthint)
        Dim weekdis As String = WeekdayName(week)
        Dim yeardis As Integer = Year(today)
        Dim daydis As Integer = Day(today)
        Dim oldweekdis As Integer = 0
        Dim oldmonthint As Integer = 0
        Dim olddaydis As Integer = 0


        If Not IsNothing(notesTable) Then
            If notesTable.Rows.Count > 0 Then
                'ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid"">"

                'Let's set up a display that doesn't show the header for the AC Details Page. 
                'If DisplayHeaderDate = False Then
                '    ' ReturnString += "<tr><td align=""left"" valign=""top"">"
                '    If useUL = True Then
                '        ReturnString += "<ul class=""circle"">"
                '    End If
                'End If

                For Each r As DataRow In notesTable.Rows
                    ' If r("lnote_jetnet_ac_id") > 0 Then
                    Dim timeofday = TimeValue(today)
                    Dim AC_Link_Text As String = ""
                    Dim Yacht_Link_Text As String = ""
                    Dim COMPANY_Link_Text As String = ""
                    Dim JETNET_AC_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_ac_id")), r("lnote_jetnet_ac_id"), 0)
                    Dim JETNET_COMPANY_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_comp_id")), r("lnote_jetnet_comp_id"), 0)
                    Dim JETNET_YACHT_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_yacht_id")), r("lnote_jetnet_yacht_id"), 0)
                    Dim DisplayDate As String = ""
                    If ShowingActions = True Then
                        DisplayDate = IIf(Not IsDBNull(r("lnote_schedule_start_date")), Format(CDate(r("lnote_schedule_start_date")), "MM/dd/yyyy") & " - ", "") & ""
                    Else
                        DisplayDate = IIf(Not IsDBNull(r("lnote_entry_date")), Format(CDate(r("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ""
                    End If

                    'Formatting for Action Items
                    'Edit - Rick Wanner - 1986 BEECHJET 400 - S/N# RJ-2, Reg# N369EA - Validate the aircraft is for sale and get asking price.
                    today = IIf(Not IsDBNull(r("lnote_schedule_start_date")), r("lnote_schedule_start_date"), Now())

                    week = Weekday(today)
                    daydis = Day(today)
                    weekdis = WeekdayName(week)
                    monthint = Month(today)
                    monthdis = Left(MonthName(monthint), 3)

                    If DisplayACInfo = True Then
                        If JETNET_AC_ID <> 0 Then
                            aTempTable = aclsData_Temp.GetJETNET_AC_NAME(JETNET_AC_ID, "")
                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then
                                    AC_Link_Text = CommonAircraftFunctions.Display_Aircraft_Information_For_Link(aTempTable, True, 0)
                                End If
                            End If
                            aTempTable.Dispose()
                        End If
                    End If

                    If DisplayYachtInfo = True Then
                        If JETNET_YACHT_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.DisplayYachtByID(JETNET_YACHT_ID)
                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then
                                    Yacht_Link_Text = Display_Yacht_Information_For_Link(aTempTable)
                                End If
                            End If
                            aTempTable.Dispose()
                        End If
                    End If

                    If DisplayCompInfo = True Then
                        If JETNET_COMPANY_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.GetLimited_CompanyInfo_ID(JETNET_COMPANY_ID, "JETNET", 0)
                            COMPANY_Link_Text = CompanyFunctions.Display_Company_Information_For_Link(aTempTable, False, 0)
                        End If
                    End If


                    'If DisplayHeaderDate = True Then
                    '    If daydis <> olddaydis Or week <> oldweekdis Or monthint <> oldmonthint Then
                    '        If olddaydis <> 0 And oldweekdis <> 0 And oldmonthint <> 0 Then
                    '            If useUL = True Then
                    '                ReturnString += "</ul>"
                    '            End If
                    '        End If
                    '        ReturnString += "<tr class=""header_row"">"
                    '        ReturnString += "<td align=""left"" valign=""top"">"
                    '        ReturnString += "<strong class=""blue_text"">" & weekdis & ", " & monthdis & " " & daydis & " " & Year(today) & "</strong>"
                    '        ReturnString += "</td>"
                    '        ReturnString += "</tr>"
                    '        ReturnString += "<tr>"
                    '        ReturnString += " <td align=""left"" valign=""top"">"
                    '        If useUL = True Then
                    '            ReturnString += "<ul class=""circle"">"
                    '        End If
                    '    End If
                    'End If

                    ReturnString += "<tr class=""header_row"">"
                    ReturnString += "<td align=""left"" valign=""top"">"
                    ReturnString += "<strong class=""blue_text"">" & weekdis & ", " & monthdis & " " & daydis & " " & Year(today) & "</strong>"
                    ReturnString += "</td><td align='left' valign='top'>"

                    If HttpContext.Current.Session.Item("localUser").crmUserContactID = r("lnote_user_id") Or HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag Then
                        ReturnString += WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit") & " - "
                    Else
                        If Not HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then 'If Not administrator view link only.
                            ReturnString += WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "View") & " - "
                        Else 'otherwise they can go ahead and edit.
                            ReturnString += WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit") & " - "
                        End If
                    End If



                    If DisplayHeaderDate = False Then
                        ReturnString += DisplayDate
                    End If

                    If ShowingNotes = True Then         ' if its an notes item 
                        ReturnString += r("lnote_user_name").ToString
                    ElseIf ShowingActions = True Then   ' if its an action item

                        If Not IsDBNull(r("lnote_schedule_start_date")) Then
                            ReturnString += FormatDateTime(r("lnote_schedule_start_date").ToString, DateFormat.LongTime)
                            If is_home_page = False Then
                                ReturnString += " - "
                            End If
                        End If

                        If is_home_page = False Then
                            ReturnString += r("lnote_user_name").ToString
                        End If
                    End If




                    ReturnString += IIf(AC_Link_Text <> "", " - " & AC_Link_Text & "</a>", "") & " - " & IIf(COMPANY_Link_Text <> "", COMPANY_Link_Text & " - ", "") & " " & " " & IIf(Yacht_Link_Text <> "", Yacht_Link_Text & " - ", "") & IIf(ShortenNotes = False Or ShowingNotes = False, r("lnote_note").ToString, IIf(Len(r("lnote_note")) > 100, Left(r("lnote_note").ToString, 100) & "..", r("lnote_note").ToString)) & "."



                    ReturnString += "</td></tr>"


                    oldweekdis = week
                    oldmonthint = monthint
                    olddaydis = daydis
                    'End If
                Next

                ReturnString += "</ul>"
                ReturnString += " </td>"
                ReturnString += " </tr>"
                '  ReturnString += "</table>"
            Else
                ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid""><tr><td align='left' valign='top'  class=""noBorder"">"
                ReturnString += "<span>No current " & IIf(ShowingActions = True, "action items", "notes") & " available for display.</span>"
                ReturnString += "</td></tr></table>"
            End If
        End If

        Return ReturnString
    End Function
    Public Shared Function Display_Notes_For_All_Export(ByVal notesTable As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal DisplayHeaderDate As Boolean, ByVal DisplayACInfo As Boolean, ByVal DisplayCompInfo As Boolean, ByVal DisplayYachtInfo As Boolean, ByVal useUL As Boolean, ByVal ShowingNotes As Boolean, ByVal ShowingActions As Boolean, Optional ByVal is_home_page As Boolean = False) As String
        Dim aTempTable As New DataTable
        Dim ReturnString As String = ""
        Dim temp_ReturnString As String = ""
        Dim today As Date = FormatDateTime(Now(), 2)
        Dim week As Integer = Weekday(today)
        Dim monthint As Integer = Month(today)
        Dim monthdis As String = MonthName(monthint)
        Dim weekdis As String = WeekdayName(week)
        Dim yeardis As Integer = Year(today)
        Dim daydis As Integer = Day(today)
        Dim oldweekdis As Integer = 0
        Dim oldmonthint As Integer = 0
        Dim olddaydis As Integer = 0
        Dim name_string As String = ""
        Dim header_row As String = ""
        Dim temp_row As String = ""
        Dim timeofday = TimeValue(today)
        Dim AC_Link_Text As String = ""
        Dim Yacht_Link_Text As String = ""
        Dim COMPANY_Link_Text As String = ""
        Dim JETNET_AC_ID As Long = 0
        Dim JETNET_COMPANY_ID As Long = 0
        Dim JETNET_YACHT_ID As Long = 0
        Dim DisplayDate As String = ""


        If Not IsNothing(notesTable) Then
            If notesTable.Rows.Count > 0 Then
                temp_ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid"">"
                ReturnString = ""

                header_row = "<tr class=""header_row"">"
                header_row += "<td align=""left"" valign=""top"">Date</td>"
                header_row += "<td align=""left"" valign=""top"">Note</td>"
                header_row += "<td align=""left"" valign=""top"">Make</td>"
                header_row += "<td align=""left"" valign=""top"">Model</td>"
                header_row += "<td align=""left"" valign=""top"">SerNo</td>"
                header_row += "<td align=""left"" valign=""top"">Reg No</td>"

                header_row += "<td align=""left"" valign=""top"">Company Name</td>"
                header_row += "<td align=""left"" valign=""top"">City</td>"
                header_row += "<td align=""left"" valign=""top"">State</td>"

                If HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag = True Then
                    DisplayYachtInfo = True
                Else
                    DisplayYachtInfo = False
                End If

                If DisplayYachtInfo = True Then
                    header_row += "<td align=""left"" valign=""top"">Yacht Name</td>"
                    header_row += "<td align=""left"" valign=""top"">Yacht Hull</td>"
                End If

                header_row += "</tr>"




                For Each r As DataRow In notesTable.Rows

                    timeofday = TimeValue(today)
                    AC_Link_Text = ""
                    Yacht_Link_Text = ""
                    COMPANY_Link_Text = ""
                    DisplayDate = ""

                    If Not IsDBNull(r("lnote_jetnet_ac_id")) Then
                        JETNET_AC_ID = r("lnote_jetnet_ac_id")
                    Else
                        JETNET_AC_ID = 0
                    End If

                    If Not IsDBNull(r("lnote_jetnet_comp_id")) Then
                        JETNET_COMPANY_ID = r("lnote_jetnet_comp_id")
                    Else
                        JETNET_COMPANY_ID = 0
                    End If

                    If Not IsDBNull(r("lnote_jetnet_yacht_id")) Then
                        JETNET_YACHT_ID = r("lnote_jetnet_yacht_id")
                    Else
                        JETNET_YACHT_ID = 0
                    End If


                    If Not IsDBNull(r("lnote_entry_date")) Then
                        today = Format(CDate(r("lnote_entry_date")), "MM/dd/yyyy")
                    Else
                        today = ""
                    End If


                    temp_row = ""
                    temp_row += "<tr class=""header_row"">"
                    temp_row += "<td align=""left"" valign=""top"">"
                    temp_row += "" & today & ""
                    temp_row += "</td>"

                    temp_row += "<td align=""left"" valign=""top"">"
                    temp_row += r("lnote_note").ToString
                    temp_row += "</td>"


                    If DisplayACInfo = True Then
                        If JETNET_AC_ID <> 0 Then
                            aTempTable = aclsData_Temp.GetJETNET_AC_NAME(JETNET_AC_ID, "")
                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then

                                    temp_row += " <td align=""left"" valign=""top"">"
                                    temp_row += aTempTable.Rows(0).Item("amod_make_name").ToString
                                    temp_row += "</td>"

                                    temp_row += " <td align=""left"" valign=""top"">"
                                    temp_row += aTempTable.Rows(0).Item("amod_model_name").ToString
                                    temp_row += "</td>"

                                    temp_row += " <td align=""left"" valign=""top"">"
                                    temp_row += aTempTable.Rows(0).Item("ac_ser_nbr").ToString
                                    temp_row += "</td>"


                                    'If there's a registration number.
                                    If Not IsDBNull(aTempTable.Rows(0).Item("ac_reg_nbr")) Then
                                        If aTempTable.Rows(0).Item("ac_reg_nbr").ToString <> "" Then

                                            temp_row += " <td align=""left"" valign=""top"">"
                                            temp_row += aTempTable.Rows(0).Item("ac_reg_nbr").ToString
                                            temp_row += "</td>"
                                        End If
                                    End If
                                End If
                            End If
                            aTempTable.Dispose()
                        Else
                            temp_row += " <td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td>"
                        End If
                    Else
                        temp_row += " <td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td>"
                    End If



                    If DisplayCompInfo = True Then
                        If JETNET_COMPANY_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.GetLimited_CompanyInfo_ID(JETNET_COMPANY_ID, "JETNET", 0)
                            COMPANY_Link_Text = CompanyFunctions.Display_Company_Information_For_Link(aTempTable, False, 0)
                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then

                                    temp_row += " <td align=""left"" valign=""top"">"
                                    temp_row += aTempTable.Rows(0).Item("comp_name").ToString
                                    temp_row += "</td>"

                                    temp_row += " <td align=""left"" valign=""top"">"
                                    temp_row += aTempTable.Rows(0).Item("comp_city").ToString
                                    temp_row += "</td>"

                                    temp_row += " <td align=""left"" valign=""top"">"
                                    temp_row += aTempTable.Rows(0).Item("comp_state").ToString
                                    temp_row += "</td>"

                                End If
                            End If
                        Else
                            temp_row += " <td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td>"
                        End If
                    Else
                        temp_row += " <td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td>"
                    End If


                    If DisplayYachtInfo = True Then
                        If JETNET_YACHT_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.DisplayYachtByID(JETNET_YACHT_ID)
                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then

                                    temp_row += "<td align=""left"" valign=""top"">"
                                    temp_row += aTempTable.Rows(0).Item("yt_yacht_name").ToString
                                    temp_row += "</td>"

                                    temp_row += "<td align=""left"" valign=""top"">"
                                    temp_row += aTempTable.Rows(0).Item("yt_hull_mfr_nbr").ToString
                                    temp_row += "</td>"

                                End If
                            End If
                            aTempTable.Dispose()
                        Else
                            temp_row += " <td align=""left"" valign=""top"">&nbsp;</td><td align=""left"" valign=""top"">&nbsp;</td>"
                        End If
                    End If


                    If DisplayHeaderDate = False Then
                        temp_row += DisplayDate
                    End If


                    '  temp_row += IIf(AC_Link_Text <> "", " - " & AC_Link_Text & "</a>", "") & " - " & IIf(COMPANY_Link_Text <> "", COMPANY_Link_Text & " - ", "") & " " & " " & IIf(Yacht_Link_Text <> "", Yacht_Link_Text & " - ", "") & r("lnote_note").ToString & "."

                    temp_row += "</tr>"


                    ReturnString &= temp_row
                Next


                ReturnString = temp_ReturnString & header_row & ReturnString
                ReturnString += " </td>"
                ReturnString += " </tr>"
                ReturnString += "</table>"
            Else
                ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid""><tr><td align='left' valign='top'>"
                ReturnString += "<span>No current " & IIf(ShowingActions = True, "action items", "notes") & " available for display.</span>"
                ReturnString += "</td></tr></table>"
            End If
        End If

        Return ReturnString
    End Function
    ''' <summary>
    ''' TO BE MOVED TO YACHT FUNCTIONS CLASS WHEN CHECKED BACK IN
    ''' </summary>
    ''' <param name="atempTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Display_Yacht_Information_For_Link(ByVal atempTable As DataTable)
        Dim ReturnString As String = ""

        ReturnString = "<a " & DisplayFunctions.WriteYachtDetailsLink(atempTable.Rows(0).Item("yt_id"), False, "", "", "") & ">" & atempTable.Rows(0).Item("yt_yacht_name").ToString & "</a> Hull # " & atempTable.Rows(0).Item("yt_hull_mfr_nbr").ToString
        Return ReturnString
    End Function

    'Public Shared Sub Fill_Yacht_Information_Tab(ByRef information_tab As AjaxControlToolkit.TabPanel, ByRef information_label As Label, ByRef master As Object, ByVal YachtID As Long, ByVal JournalID As Long, ByVal isNote As Boolean, ByRef modelIDLabel As Label)
    '    Dim InfoTable As New DataTable

    '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '    'Main Yacht Query
    '    InfoTable = master.aclsData_Temp.DisplayYachtByID(YachtID)
    '    If Not IsNothing(InfoTable) Then
    '        If InfoTable.Rows.Count > 0 Then
    '            'Fill the Label (static for now)
    '            information_label.Text = "<table width='100%' cellspacing='3' cellpadding='3'>"
    '            information_label.Text += "<tr>"
    '            information_label.Text += "<td align='left' valign='top' width='60%'>"

    '            If Not IsDBNull(InfoTable.Rows(0).Item("ym_brand_name")) And Not String.IsNullOrEmpty(InfoTable.Rows(0).Item("ym_brand_name").ToString) Then
    '                information_tab.HeaderText = InfoTable.Rows(0).Item("ym_brand_name") & " "
    '            End If

    '            If Not IsDBNull(InfoTable.Rows(0).Item("ym_model_name").ToString) And Not String.IsNullOrEmpty(InfoTable.Rows(0).Item("ym_model_name").ToString.ToString) Then
    '                information_tab.HeaderText += InfoTable.Rows(0).Item("ym_model_name").ToString & " "
    '            End If

    '            information_tab.HeaderText += """" & "<i>" & InfoTable.Rows(0).Item("yt_yacht_name").ToString & "</i>" & """" & " "

    '            information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("yt_year_mfr")), "<span class='li_no_bullet'><span class='label'>Year Mfr.: </span>" & InfoTable.Rows(0).Item("yt_year_mfr").ToString & "</span>", "")
    '            information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("yt_year_dlv")), "<span class='li_no_bullet'><span class='label'>Year Dlv.: </span>" & InfoTable.Rows(0).Item("yt_year_dlv").ToString & "</span>", "")

    '            information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("yt_imo_nbr")), "<span class='li_no_bullet'><span class='label'>IMO:</span> " & InfoTable.Rows(0).Item("yt_imo_nbr").ToString & "</span>", "")
    '            information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("yt_yacht_name")), "<span class='li_no_bullet'><span class='label'>Flag:</span> " & InfoTable.Rows(0).Item("yt_registered_country_flag").ToString & "</span>", "")

    '            information_label.Text += "</td>"
    '            information_label.Text += "<td align='left' valign='top' width='60%'>"
    '            information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("yt_year_mfr")), "<span class='li_no_bullet'><span class='label'>Hull #:</span> " & InfoTable.Rows(0).Item("yt_hull_mfr_nbr").ToString & "</span>", "")
    '            information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("yt_yacht_name")), "<span class='li_no_bullet'><span class='label'>MMSI:</span> " & InfoTable.Rows(0).Item("yt_mmsi_mobile_nbr").ToString & "</span>", "")
    '            modelIDLabel.Text = IIf(Not IsDBNull(InfoTable.Rows(0).Item("ym_model_id")), InfoTable.Rows(0).Item("ym_model_id"), 0)

    '            information_label.Text += "</td>"

    '        End If
    '    End If
    '    InfoTable.Dispose()
    '    information_label.Text += "</span>"




    '    information_label.Text += "</tr>"
    '    information_label.Text += "</table>"


    'End Sub

    'This code should all be self contained in this block, as well as the actual widget block is named TellJetnetAboutChanges
    'When coming into the page, these 2 panels are VISIBLE = FALSE. That means that certain actions have to occur (the code down below) 
    'To have it display. Plus the JQUERY (in the block down below) also needs to run (for the popup).

    'As a quick description of panels:
    'TellJetnetAboutChanges contains the actual little block on the page that's sticky and ever present.
    'TellJetnetAboutChangesForm contains the IFRAME that the popup displays. This is always invisible unless you actually click the sticky block, in 
    'which case it actually displays. But the jquery should handle visible/invisible, so nothing further would be needed there.
    Public Shared Sub BuildJavascriptTellJetnetAboutChanges(ByRef modalPostbackScript As StringBuilder, ByRef modalScript As StringBuilder, ByVal AircraftID As Long, ByVal journalID As Long, ByVal companyID As Long, ByVal TellJetnetAboutChanges As Panel, ByVal TellJetnetAboutChangesForm As Panel, ByVal includeJqueryTheme As Literal, ByVal notifyIframe As HtmlGenericControl)
        TellJetnetAboutChanges.Visible = True
        TellJetnetAboutChangesForm.Visible = True


        'Let's go ahead and set the iframe to pass the correct ID
        notifyIframe.Attributes.Add("src", "Notify.aspx?" & IIf(AircraftID > 0, "acID=" & AircraftID.ToString, "compid=" & companyID.ToString) & "&jID=" & journalID)


        'modalPostbackScript.Append(" jQuery(function(){")
        modalPostbackScript.Append("Sys.Application.add_load(function() {")

        modalPostbackScript.Append("jQuery(""#notifyJetnetDialog"").dialog({")
        modalPostbackScript.Append("autoOpen: false,")
        modalPostbackScript.Append("show: {")
        modalPostbackScript.Append("effect: ""fade"",")
        modalPostbackScript.Append("duration: 500")
        modalPostbackScript.Append("},")
        modalPostbackScript.Append("modal: true,")
        modalPostbackScript.Append("dialogClass: ""welcomeUserPopup"",")
        modalPostbackScript.Append("minHeight: 430,")
        modalPostbackScript.Append("resizable: false,")
        modalPostbackScript.Append("maxHeight: 430,")
        modalPostbackScript.Append("maxWidth: 490,")
        modalPostbackScript.Append("minWidth: 490,")
        modalPostbackScript.Append("draggable: false,")
        modalPostbackScript.Append("close: function( event, ui ) {")
        'We need to add a silly little item here. We're just going to go ahead and tell jquery that when the little blue box popup view link is clicked, we 
        'should have it refresh the src on the iframe. So in case they submit information and for some reason try to submit information again on the same aircraft, it will refresh.
        modalPostbackScript.Append("jQuery('#" & notifyIframe.ClientID & "').attr('src','Notify.aspx?&" & IIf(AircraftID > 0, "acID=" & AircraftID.ToString, "compid=" & companyID.ToString) & "&jID=" & journalID & "');")
        modalPostbackScript.Append("},")
        modalPostbackScript.Append("closeText:""X""")
        modalPostbackScript.Append("});")

        modalPostbackScript.Append("jQuery(""#closeTellJetnetChanges"").click(function() {")
        modalPostbackScript.Append("jQuery(""#TellJetnetChangesContainer"").css('display','none');")
        modalPostbackScript.Append("});")

        modalPostbackScript.Append("jQuery(""#tellJetnetAboutChangesLink"").click(function() {")
        modalPostbackScript.Append("jQuery(""#notifyJetnetDialog"").dialog(""open"");")
        modalPostbackScript.Append("});")


        If AircraftID > 0 Then
            'Only on aircraft details page.

            modalPostbackScript.Append("jQuery(""#tellJetnetAboutChangesLinkIntel"").click(function() {")
            modalPostbackScript.Append("jQuery(""#notifyJetnetDialog"").dialog(""open"");")
            modalPostbackScript.Append("});")
        End If
        'Add before final closing, not needed
        modalScript.Append(Replace(modalPostbackScript.ToString, "Sys.Application.add_load(function() {", ""))


        modalPostbackScript.Append("});")

    End Sub

    ''' <summary>
    ''' Creates a static folder table
    ''' 'EDITED: I hate optional parameters, however the files I need to edit are out, so I had to add the yacht ID as optional for now so the app doesn't break.
    ''' </summary>
    ''' <param name="AircraftID"></param>
    ''' <param name="JournalID"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateStaticFoldersTable(ByVal AircraftID As Long, ByVal CompanyID As Long, ByVal JournalID As Long, ByVal WantedID As Long, ByVal ContactID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal YachtID As Long) As Table
        Dim FoldersTable As New DataTable
        Dim ContainerTable As New Table
        Dim FolderTypeID As Long = 0
        Dim TR As New TableRow
        Dim TDHold As New TableCell
        Dim CheckHold As New CheckBox
        Dim SubmitButton As New LinkButton
        Dim SharedCheckboxList As New CheckBoxList
        SharedCheckboxList.ID = "SharedCheckboxList"
        Dim NonSharedCheckboxList As New CheckBoxList
        NonSharedCheckboxList.ID = "NonSharedCheckboxList"
        Dim ListedItem As New ListItem

        'Let's figure out the ID for the FolderType
        If AircraftID <> 0 Then
            If JournalID = 0 Then
                FolderTypeID = 3
            Else
                FolderTypeID = 8
            End If
        ElseIf CompanyID <> 0 Then
            FolderTypeID = 1
        ElseIf ContactID <> 0 Then
            FolderTypeID = 2
        ElseIf WantedID <> 0 Then
            FolderTypeID = 9
        ElseIf YachtID <> 0 Then
            FolderTypeID = 10
        End If
        FoldersTable = aclsData_Temp.GetEvolutionFolderssBySubscription(0, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", FolderTypeID, Nothing, "S")
        ContainerTable.CellPadding = 3
        ContainerTable.Width = Unit.Percentage(100D)

        ContainerTable.CssClass = "data_aircraft_grid"

        Dim tempCell As TableCell = DisplayFunctions.BuildTableCell(True, "Personal Folders", VerticalAlign.Top, HorizontalAlign.Left)
        tempCell.CssClass = "mediumText uppercase"
        tempCell.Width = Unit.Percentage(50)
        TR.Controls.Add(tempCell)

        tempCell = New TableCell
        tempCell.Width = Unit.Percentage(50)
        tempCell = DisplayFunctions.BuildTableCell(True, "Shared Folders", VerticalAlign.Top, HorizontalAlign.Left)
        tempCell.CssClass = "mediumText uppercase"
        TR.Controls.Add(tempCell)

        TR.CssClass = "header_row noBorder"
        ContainerTable.Controls.Add(TR)

        Dim SharedString As String = ""
        Dim NonShared As String = ""
        Dim ACArray As Array
        Dim TempCheckTable As New DataTable

        If Not IsNothing(FoldersTable) Then
            If FoldersTable.Rows.Count > 0 Then
                For Each r As DataRow In FoldersTable.Rows
                    ACArray = Split(r("cfolder_data").ToString, ",")

                    TempCheckTable = New DataTable
                    TempCheckTable = aclsData_Temp.GetEvolutionFoldersIndex(r("cfolder_id"), AircraftID, CompanyID, WantedID, ContactID, JournalID, YachtID)


                    If r("cfolder_share").ToString = "Y" Then
                        ListedItem = New ListItem

                        ListedItem.Text = r("cfolder_name").ToString
                        ListedItem.Value = r("cfolder_id")

                        If TempCheckTable.Rows.Count = 0 Then
                            ListedItem.Selected = False
                        Else
                            ListedItem.Selected = True
                        End If

                        SharedCheckboxList.Items.Add(ListedItem)
                    Else
                        ListedItem = New ListItem

                        ListedItem.Text = r("cfolder_name").ToString
                        ListedItem.Value = r("cfolder_id")

                        If TempCheckTable.Rows.Count = 0 Then
                            ListedItem.Selected = False
                        Else
                            ListedItem.Selected = True
                        End If

                        NonSharedCheckboxList.Items.Add(ListedItem)
                    End If

                Next
            End If
        End If

        TR = New TableRow
        TDHold = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)
        TDHold.Controls.Add(NonSharedCheckboxList)
        TR.Controls.Add(TDHold)

        TDHold = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)
        TDHold.Controls.Add(SharedCheckboxList)
        TR.Controls.Add(TDHold)
        ContainerTable.Controls.Add(TR)


        Return ContainerTable

    End Function
    ''' <summary>
    ''' Prepping this for use in evo listing pages.
    ''' </summary>
    ''' <param name="StartCount"></param>
    ''' <param name="EndCount"></param>
    ''' <param name="Dynamically_Configured_Datagrid"></param>
    ''' <param name="HoldTable"></param>
    ''' <param name="next_"></param>
    ''' <param name="prev_"></param>
    ''' <param name="next_all"></param>
    ''' <param name="prev_all"></param>
    ''' <param name="goToPage"></param>
    ''' <param name="pageNumber"></param>
    ''' <remarks></remarks>
    Public Shared Sub MovePage(ByRef StartCount As Integer, ByRef EndCount As Integer, ByVal Dynamically_Configured_Datagrid As DataGrid, ByVal Dynamically_Configured_DataList As Object, ByVal HoldTable As DataTable, ByVal next_ As Boolean, ByVal prev_ As Boolean, ByVal next_all As Boolean, ByVal prev_all As Boolean, ByVal goToPage As Boolean, ByVal pageNumber As Integer)
        Dim RecordsPerPage As Integer = 0
        Dim CurrentPage As Integer = 0
        Dim CurrentRecord As Integer = 0
        ' Dim EndCount As Integer = 0
        'Dim StartCount As Integer = 0
        Dim Paging_Table As New DataTable
        Dim CountString As String = ""
        Dim TotalPageNumber As Integer = 0


        'Initial(False)
        If HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage <> 0 Then
            RecordsPerPage = HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage
        End If

        If Not IsNothing(HoldTable) Then
            TotalPageNumber = Math.Ceiling(HoldTable.Rows.Count / RecordsPerPage)
            Dynamically_Configured_Datagrid.PageSize = RecordsPerPage

            Dynamically_Configured_Datagrid.DataSource = HoldTable

            If next_ Then
                Dynamically_Configured_Datagrid.CurrentPageIndex += 1
            ElseIf prev_ Then
                Dynamically_Configured_Datagrid.CurrentPageIndex -= 1
            ElseIf prev_all Then
                Dynamically_Configured_Datagrid.CurrentPageIndex = 0
            ElseIf next_all Then
                Dynamically_Configured_Datagrid.CurrentPageIndex = TotalPageNumber - 1
            Else
                Dynamically_Configured_Datagrid.CurrentPageIndex = pageNumber - 1
            End If



            'only bind if results is visible.
            If Dynamically_Configured_Datagrid.Visible = True Then
                Try
                    Dynamically_Configured_Datagrid.DataBind()
                Catch
                    Dynamically_Configured_Datagrid.CurrentPageIndex = 0
                    Dynamically_Configured_Datagrid.DataBind()
                End Try
            End If


            CurrentPage = Dynamically_Configured_Datagrid.CurrentPageIndex + 1
            CurrentRecord = (Dynamically_Configured_Datagrid.PageSize * Dynamically_Configured_Datagrid.CurrentPageIndex) - HoldTable.Rows.Count + HoldTable.Rows.Count
            If CurrentRecord = 0 Then
                StartCount = 1
            Else
                StartCount = CurrentRecord + 1
            End If

            If CurrentRecord + Dynamically_Configured_Datagrid.PageSize >= HoldTable.Rows.Count Then
                CountString = StartCount & "-" & HoldTable.Rows.Count
                EndCount = HoldTable.Rows.Count
            Else
                CountString = StartCount & "-" & CurrentRecord + Dynamically_Configured_Datagrid.PageSize
                EndCount = CurrentRecord + Dynamically_Configured_Datagrid.PageSize
            End If

            If Not IsNothing(Dynamically_Configured_DataList) Then
                If Dynamically_Configured_DataList.Visible = True Then
                    Paging_Table = HoldTable.Clone
                    Dim afiltered_Client As DataRow() = HoldTable.Select("comp_count >= " & StartCount & " and comp_count <= " & EndCount, "")
                    For Each atmpDataRow_Client In afiltered_Client
                        Paging_Table.ImportRow(atmpDataRow_Client)
                    Next

                    Dynamically_Configured_DataList.DataSource = Paging_Table
                    Dynamically_Configured_DataList.DataBind()
                End If
            End If


        End If

        Dynamically_Configured_Datagrid.Dispose()
    End Sub

    ''' <summary>
    ''' The purpose of this function is to grab the information pertaining just to the market tab on both the yacht and the aircraft listing pages
    ''' without having to write this out more than ones.
    ''' </summary>
    ''' <param name="MarketEvent"></param>
    ''' <param name="EventTypeOfSearch"></param>
    ''' <param name="MarketCategory"></param>
    ''' <param name="MarketType"></param>
    ''' <param name="Months"></param>
    ''' <param name="Days"></param>
    ''' <param name="Hours"></param>
    ''' <param name="Minutes"></param>
    ''' <param name="UseDefaultDate"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="BuildSearchString"></param>
    ''' <param name="NewSearchClass"></param>
    ''' <param name="events_market_categories"></param>
    ''' <param name="events_market_types"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToGrabTheEventOnlyInformation(ByVal MarketEvent As Boolean, ByRef EventTypeOfSearch As String, ByRef MarketCategory As String, ByRef MarketType As String, ByRef Months As Integer, ByRef Days As Integer, ByRef Hours As Integer, ByRef Minutes As Integer, ByRef UseDefaultDate As Boolean, ByRef StartDate As Date, ByRef BuildSearchString As String, ByRef NewSearchClass As SearchSelectionCriteria, ByRef events_market_categories As ListBox, ByRef events_market_types As ListBox, ByRef events_type_of_search As RadioButtonList, ByRef events_months As TextBox, ByRef event_days As TextBox, ByRef event_hours As TextBox, ByRef event_minutes As TextBox)

        If MarketEvent Then
            EventTypeOfSearch = events_type_of_search.SelectedValue
            NewSearchClass.SearchCriteriaEventSearchType = events_type_of_search.SelectedValue

            NewSearchClass.SearchCriteriaEventCategory = ""
            'This means it's an event search
            MarketCategory = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(events_market_categories, True, 0, False)

            If MarketCategory <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(events_market_categories, "Event Categories")
                'Setting up Event Category in session
                NewSearchClass.SearchCriteriaEventCategory = MarketCategory
            End If

            MarketType = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(events_market_types, True, 0, False)

            NewSearchClass.SearchCriteriaEventType = ""
            If MarketType <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(events_market_types, "Event Types")
                'Setting up Event Type in session
                NewSearchClass.SearchCriteriaEventType = MarketType
            End If

            NewSearchClass.SearchCriteriaEventMonths = 0
            If events_months.Text <> "" Then
                If IsNumeric(events_months.Text) Then
                    Months = events_months.Text
                    UseDefaultDate = False
                    'Setting up Event Months in session
                    NewSearchClass.SearchCriteriaEventMonths = Months
                End If
            End If

            NewSearchClass.SearchCriteriaEventDays = 1
            If event_days.Text <> "" Then
                If IsNumeric(event_days.Text) Then
                    Days = event_days.Text
                    UseDefaultDate = False
                    'Setting up Event Days in session
                    NewSearchClass.SearchCriteriaEventDays = Days
                End If
            End If
            NewSearchClass.SearchCriteriaEventHours = 0
            If event_hours.Text <> "" Then
                If IsNumeric(event_hours.Text) Then
                    Hours = event_hours.Text
                    UseDefaultDate = False
                    'Setting up Event Hours in session
                    NewSearchClass.SearchCriteriaEventHours = Hours
                End If
            End If
            NewSearchClass.SearchCriteriaEventMinutes = 0
            If event_minutes.Text <> "" Then
                If IsNumeric(event_minutes.Text) Then
                    Minutes = event_minutes.Text
                    UseDefaultDate = False
                    'Setting up Event Minutes in session
                    NewSearchClass.SearchCriteriaEventMinutes = Minutes
                End If
            End If

            If UseDefaultDate = False Then
                StartDate = DateAdd(DateInterval.Month, -Months, Now())
                StartDate = DateAdd(DateInterval.Day, -Days, StartDate)
                StartDate = DateAdd(DateInterval.Hour, -Hours, StartDate)
                StartDate = DateAdd(DateInterval.Minute, -Minutes, StartDate)
            Else
                NewSearchClass.SearchCriteriaEventDays = 1
                StartDate = DateAdd(DateInterval.Day, -1, Now())
                event_days.Text = 1
            End If

            BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Format(CDate(StartDate), "MM/dd/yyyy hh:mm:ss tt"), "Start Date")
            BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Format(CDate(Now()), "MM/dd/yyyy hh:mm:ss tt"), "End Date")

        End If
    End Sub

    Public Shared Sub FillUpFolderInformation(ByVal table2 As Table, ByVal close_current_folder As Label, ByVal cfolderData As String, ByVal FolderInformation As Label, ByVal FoldersTableData As DataTable, ByVal Company As Boolean, ByVal History As Boolean, ByVal MarketEvent As Boolean, ByVal Wanted As Boolean, ByVal Yacht As Boolean, ByVal Collapse_Panel As Panel, ByVal actions_submenu_dropdown As BulletedList, Optional ByVal SecondControlOptiontoFilter As Object = Nothing, Optional ByVal StaticFolderNewSearchLabel As Label = Nothing, Optional ByVal Control_Panel As Panel = Nothing, Optional ByRef StaticAIRCRAFTIDs As String = "", Optional ByVal PerformanceSpecs As Boolean = False, Optional ByVal OperatingCosts As Boolean = False, Optional ByVal MarketSummary As Boolean = False, Optional ByVal FolderSource As String = "JETNET")

        Dim folderTypeString As String = "AIRCRAFT"
        Dim JavascriptFunction As String = "SubMenuDropAircraft"
        Dim FolderRestartLabel As New Label
        Dim DefaultFolderLabel As New Label
        Dim FolderImageLabel As New Label
        Dim i As Integer = 0
        Dim data_spot As Integer = 0
        Dim info_spot As Integer = 0

        Try

            FolderInformation.Text = "<img src='" & DisplayFunctions.ReturnFolderImage(FoldersTableData.Rows(0).Item("cfolder_method").ToString, FoldersTableData.Rows(0).Item("cfolder_hide_flag").ToString, FoldersTableData.Rows(0).Item("cfolder_share").ToString) & "' border='0' />" & FoldersTableData.Rows(0).Item("cfolder_name").ToString

            If InStr(FolderInformation.Text, "Close Current Folder") = 0 Then
                FolderInformation.ToolTip = "This Information Bar displays your current selected Folder. Please click 'Close Current Folder' to reset your search criteria."
                If FolderSource = "JETNET" Then
                    If FoldersTableData.Rows(0).Item("cfolder_method").ToString.ToUpper.Contains("S") And Company Then
                        FolderInformation.Text += "&nbsp;&nbsp;<a href=""staticFolderEditor.aspx?folderID=" + FoldersTableData.Rows(0).Item("cfolder_id").ToString + """ class=""float_right padding"" onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"" id=""editCurrentFolderLink"" target=""new"">Edit Current Folder</a>"
                    ElseIf FoldersTableData.Rows(0).Item("cfolder_method").ToString.ToUpper.Contains("S") And Not Company And Not History Then
                        FolderInformation.Text += "&nbsp;&nbsp;<a href=""staticFolderEditor.aspx?folderID=" + FoldersTableData.Rows(0).Item("cfolder_id").ToString + "&aircraft=true"" class=""float_right padding"" onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"" id=""editCurrentFolderLink"" target=""new"">Edit Current Folder</a>"
                    End If
                End If

                FolderInformation.Text += "&nbsp;&nbsp;<a href='" & IIf(MarketSummary = True, "MarketSummary.aspx?restart=1", IIf(OperatingCosts = True, "Operating_Listing.aspx?restart=1", IIf(PerformanceSpecs = True, "Performance_Listing.aspx?restart=1", IIf(Wanted = True, "Wanted_Listing.aspx?restart=1", IIf(Company = True, "Company_Listing.aspx?restart=1", IIf(Yacht = True, "YachtListing.aspx?restart=1", "Aircraft_Listing.aspx?restart=1")))))) & IIf(History = True, "&h=1", "") & "" & IIf(MarketEvent = True, "&e=1", "") & "' class='float_right padding' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"" id=""closeCurrentFolderLink"">Close Current Folder</a>"
                If Not IsNothing(StaticFolderNewSearchLabel) Then
                    If FoldersTableData.Rows(0).Item("cfolder_method").ToString.ToUpper.Contains("S") Then
                        If Not IsNothing(Control_Panel) Then
                            Control_Panel.CssClass = "display_none"
                        End If
                        StaticFolderNewSearchLabel.Text = "<a href='" & IIf(MarketSummary = True, "MarketSummary.aspx?restart=1", IIf(OperatingCosts = True, "Operating_Listing.aspx?restart=1", IIf(PerformanceSpecs = True, "Performance_Listing.aspx?restart=1", IIf(Wanted = True, "Wanted_Listing.aspx?restart=1", IIf(Company = True, "Company_Listing.aspx?restart=1", IIf(Yacht = True, "YachtListing.aspx?restart=1", "Aircraft_Listing.aspx?restart=1")))))) & IIf(History = True, "&h=1", "") & "" & IIf(MarketEvent = True, "&e=1", "") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"" id=""NewSearchFolderLink""><img src='../Images/search_expand.jpg' alt='New Search' border='0' /></a>"
                    End If
                End If

            End If

            FolderInformation.Visible = True

            If FoldersTableData.Rows(0).Item("cfolder_method").ToString = "A" Or Company = True Or Wanted = True Or History = True Or Yacht = True Then 'company version doesn't matter if you have static or dynamic since we have no advanced tab
                If Company = True Then
                    folderTypeString = "COMPANY"
                ElseIf Wanted = True Then
                    folderTypeString = "WANTED"
                ElseIf Yacht = True Then
                    folderTypeString = "YACHTS"
                    If History = True Then
                        folderTypeString = "YACHT HISTORY"
                    End If
                    If MarketEvent = True Then
                        folderTypeString = "YACHT EVENTS"
                    End If
                ElseIf MarketEvent = True Then
                    folderTypeString = "EVENTS"
                ElseIf History = True Then
                    folderTypeString = "HISTORY"
                ElseIf PerformanceSpecs = True Then
                    folderTypeString = "PERFORMANCE SPECS"
                ElseIf OperatingCosts = True Then
                    folderTypeString = "OPERATING COSTS"
                ElseIf MarketSummary = True Then
                    folderTypeString = "MARKET SUMMARIES"
                End If



                If Company = True Or Wanted = True Or Yacht = True Or PerformanceSpecs = True Or MarketSummary = True Or OperatingCosts = True Then
                    JavascriptFunction = "SubMenuDrop"
                End If

                actions_submenu_dropdown.Items.Insert(IIf(actions_submenu_dropdown.Items.Count = 0, 0, 1), New ListItem("Save Current Folder", "javascript:" & JavascriptFunction & "(3, " & HttpContext.Current.Request.Form("project_id") & " " & IIf(JavascriptFunction = "SubMenuDrop", ",'" & folderTypeString & "'", "") & ");"))

                RefillUpFolderInformation(Company, cfolderData, Collapse_Panel, SecondControlOptiontoFilter)

                'small catch added to make static folders uneditable.
                If Company = True Or Wanted = True Or Yacht = True Then
                    If FoldersTableData.Rows(0).Item("cfolder_method").ToString <> "A" Then
                        table2.Visible = False
                        close_current_folder.Visible = True
                        HttpContext.Current.Session.Item("tabAircraftType") = ""
                        HttpContext.Current.Session.Item("tabAircraftMake") = ""
                        HttpContext.Current.Session.Item("tabAircraftModel") = ""
                        HttpContext.Current.Session.Item("tabAircraftModel") = ""
                        HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
                        HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
                        HttpContext.Current.Session.Item("tabAircraftSize") = ""
                    End If
                End If

                If FolderSource = "JETNET" Then
                    If FoldersTableData.Rows(0).Item("cfolder_share").ToString = "N" And FoldersTableData.Rows(0).Item("cfolder_default_flag").ToString = "N" Then
                        FolderInformation.Text += "<a id=""closeFolderListingButton"" href=""javascript:void(0);"" class=""float_right padding"" onclick=""javascript:SaveRemoveDefault(" & FoldersTableData.Rows(0).Item("cfolder_id").ToString & ",'" & folderTypeString & "', 'false', 'true');"">Set as Home Default</a>"
                    ElseIf FoldersTableData.Rows(0).Item("cfolder_share").ToString = "N" And FoldersTableData.Rows(0).Item("cfolder_default_flag").ToString = "Y" Then
                        FolderInformation.Text += "<a id=""closeFolderListingButton"" href=""javascript:void(0);"" class=""float_right padding"" onclick=""javascript:SaveRemoveDefault(" & FoldersTableData.Rows(0).Item("cfolder_id").ToString & ",'" & folderTypeString & "', 'true', 'false');"">Remove as Home Default</a>"
                    End If
                End If
            Else
                'have to reset the make/model/type to be all
                'static folder
                table2.Visible = False
                close_current_folder.Visible = True
                HttpContext.Current.Session.Item("tabAircraftType") = ""
                HttpContext.Current.Session.Item("tabAircraftMake") = ""
                HttpContext.Current.Session.Item("tabAircraftModel") = ""
                HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
                HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
                HttpContext.Current.Session.Item("tabAircraftSize") = ""
                actions_submenu_dropdown.Items.RemoveAt(0)

                'This is a small catch on the static aircraft folders. If we go ahead and 
                Dim SeperatedValues As Array
                SeperatedValues = cfolderData.Split("!~!")

                For j = 0 To UBound(SeperatedValues)
                    Dim TemporaryHold As String
                    Dim ValPar As Array
                    TemporaryHold = Replace(SeperatedValues(j), "!", "")
                    TemporaryHold = Replace(SeperatedValues(j), "~", "")
                    ValPar = TemporaryHold.Split("=")

                    ' changed both of the (1) below to (2) to accomodate new format - msw - 5/22/18
                    'If UBound(ValPar) = 2 Then 
                    info_spot = 0
                    data_spot = 0
                    For i = 0 To UBound(ValPar) - 1 ' dont do the last spot either 
                        If Trim(ValPar(i)) = "Equals!~!ac_id" And Trim(ValPar(i + 1)) <> "ac_id" Then
                            ValPar(i) = "ac_id"
                            info_spot = i
                            data_spot = i + 1
                        ElseIf Trim(ValPar(i)) = "ac_id" Then
                            ValPar(i) = "ac_id"
                            info_spot = i
                            data_spot = i + 1
                        End If
                    Next

                    If ValPar(info_spot) = "ac_id" Then
                        Dim value As String = ValPar(data_spot)
                        'this is the aircraft ID saving.
                        If Not IsNothing(StaticAIRCRAFTIDs) Then
                            StaticAIRCRAFTIDs = value
                        End If
                    End If
                    ' End If
                Next
            End If

        Catch ex As Exception

            commonLogFunctions.Log_User_Event_Data("UserError", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + "): " + ex.Message.Trim

        End Try

    End Sub

    Public Shared Function DisplayBaseInfo(ByVal baseCountry As Object, ByVal baseState As Object) As String
        Dim returnString As String = ""

        If Not IsDBNull(baseCountry) Then
            If Not String.IsNullOrEmpty(baseCountry) Then
                returnString += baseCountry
            End If
        End If
        If Not IsDBNull(baseState) Then
            If Not String.IsNullOrEmpty(baseState) Then
                If returnString <> "" Then
                    returnString += ", "
                End If
                returnString += baseState
            End If
        End If

        If returnString <> "" Then
            returnString = "<span class=""display_block div_clear"">" & returnString & "</span>"
        End If
        Return returnString
    End Function

    Public Shared Function DisplayMobileCompanies(ByVal acID As Long) As String
        Dim returnString As String = "<span class=""display_block"">"
        Dim CompTable As New DataTable
        CompTable = CompanyFunctions.MobileACLoadCompanies(acID, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag)

        If Not IsNothing(CompTable) Then
            If CompTable.Rows.Count > 0 Then
                For Each r As DataRow In CompTable.Rows
                    returnString += r("actype_name") & ": " & DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, r("comp_name").ToString, "compName", "")
                Next
            End If
        End If
        returnString += "</span>"
        Return returnString
    End Function

    Public Shared Function TrimName(ByVal acYear As Object, ByVal acMake As Object, ByRef acModel As Object, ByVal amodId As Long, ByVal serNo As Object, ByVal acID As Long)
        Dim returnString As String = ""

        If Not IsDBNull(acYear) Then
            If Not String.IsNullOrEmpty(acYear) Then
                returnString += acYear
            End If
        End If

        If Not IsDBNull(acMake) Then
            If Not String.IsNullOrEmpty(acMake) Then
                If returnString <> "" Then
                    returnString += " "
                End If
                returnString += acMake
            End If
        End If

        If Not IsDBNull(acModel) Then
            If Not String.IsNullOrEmpty(acModel) Then
                If returnString <> "" Then
                    returnString += " "
                End If
                returnString += acModel.ToString
            End If
        End If

        If Not IsDBNull(serNo) Then
            If Not String.IsNullOrEmpty(serNo) Then
                If returnString <> "" Then
                    returnString += " "
                End If

                'Dim total As Integer = Len(returnString) + Len(serNo.ToString)
                'Dim difference As Integer = total - 31
                'If Len(returnString) + Len(serNo.ToString) > 31 Then
                '  returnString += DisplayFunctions.WriteDetailsLink(acID, 0, 0, 0, True, serNo.ToString.Substring(0, Len(serNo.ToString) - difference) & "...", "", "")
                'Else
                returnString += "SN: " & DisplayFunctions.WriteDetailsLink(acID, 0, 0, 0, True, serNo.ToString, "", "")
                'End If
            End If
        End If


        Return returnString

    End Function

    Public Shared Sub RefillUpFolderInformation(ByVal Company As Boolean, ByVal CfolderData As String, ByVal Collapse_Panel As Panel, Optional ByVal SecondControlOptiontoFilter As Object = Nothing)

        Dim SeperatedValues As Array
        Dim sepArry(2) As Char

        Try
            sepArry(0) = Constants.cSvrRecordSeperator.Substring(0, 1)
            sepArry(1) = Constants.cSvrRecordSeperator.Substring(1, 1)
            sepArry(2) = Constants.cSvrRecordSeperator.Substring(2, 1)

            SeperatedValues = CfolderData.Split(sepArry, StringSplitOptions.RemoveEmptyEntries)

            For j = 0 To UBound(SeperatedValues)

                Dim ValPar As Array

                ValPar = SeperatedValues(j).Split("=")

                If UBound(ValPar) = 1 Then

                    Dim value As String = ValPar(1)
                    Dim compareObject1 As New Object
                    Dim compareObject2 As New Object
                    Dim cont As Object = Collapse_Panel.FindControl(ValPar(0)) 'Nothing

                    If Not IsNothing(SecondControlOptiontoFilter) Then
                        compareObject1 = SecondControlOptiontoFilter.FindControl(ValPar(0))
                        cont = compareObject1
                    End If

                    If Not IsNothing(cont) Then 'Checks to make sure the control actually exists.
                        If (cont.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox")) Then 'is it a textbox?
                            cont.text = value
                            'This is for the type dropdown list. 
                        ElseIf (cont.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList")) Or cont.GetType().ToString().Equals("System.Web.UI.WebControls.RadioButtonList") Then
                            cont.selectedvalue = value
                        ElseIf (cont.GetType().ToString().Equals("System.Web.UI.WebControls.ListBox")) Then 'is it a dropdown list? Is it a listbox?
                            'If the type is a listbox, we have to go through and account for multiple selections, if saved.
                            Dim MultipleSelection As Array
                            'We split the answer.
                            MultipleSelection = value.Split("##")
                            'We also need to account to make sure that the selection mode on the listbox is that of
                            'multiple selections. If it is, we run through and select all the picked ones
                            If cont.SelectionMode = ListSelectionMode.Multiple Then
                                cont.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                                If ValPar(0) = "model_cbo" And HttpContext.Current.Session.Item("localUser").crmEvo = False Then
                                    'that the page defaults to.
                                    For MultipleSelectionCount = 0 To UBound(MultipleSelection)
                                        For ListBoxCount As Integer = 0 To cont.Items.Count() - 1
                                            If Not String.IsNullOrEmpty(UCase(MultipleSelection(MultipleSelectionCount))) Then
                                                'Split the value up
                                                If UCase(cont.Items(ListBoxCount).Value) = "ALL" Then
                                                    If UCase(cont.Items(ListBoxCount).Value) = UCase(MultipleSelection(MultipleSelectionCount)) Then
                                                        cont.Items(ListBoxCount).Selected = True
                                                    End If
                                                ElseIf UCase(cont.Items(ListBoxCount).Value) <> "ALL" Then
                                                    Dim SavedSplitIDs As Array = Split(UCase(MultipleSelection(MultipleSelectionCount)), "|")
                                                    Dim ListboxSplitIDs As Array = Split(UCase(cont.Items(ListBoxCount).Value), "|")
                                                    'SavedSplitIDs(0) 'JetnetID Multiple Selection
                                                    'ListboxSplitIDs(0) 'Jetnet ListboxID
                                                    'SavedSplitIDs(4) 'Client Multiple Selection
                                                    'ListboxSplitIDs(4) 'Client ListboxID
                                                    'Compare Jetnet first, then compare client
                                                    If UBound(SavedSplitIDs) >= 4 Then
                                                        If (ListboxSplitIDs(0) = SavedSplitIDs(0)) Then 'Or (ListboxSplitIDs(4) = SavedSplitIDs(4)) Then
                                                            cont.Items(ListBoxCount).Selected = True
                                                        ElseIf ListboxSplitIDs(4) <> 0 And SavedSplitIDs(4) <> 0 Then 'Make sure the IDs for clients Exist.
                                                            If (ListboxSplitIDs(4) = SavedSplitIDs(4)) Then 'Make sure they match
                                                                cont.Items(ListBoxCount).Selected = True
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        Next
                                    Next

                                Else
                                    'that the page defaults to.
                                    For MultipleSelectionCount = 0 To UBound(MultipleSelection)
                                        For ListBoxCount As Integer = 0 To cont.Items.Count() - 1
                                            If Not String.IsNullOrEmpty(UCase(MultipleSelection(MultipleSelectionCount))) Then
                                                If UCase(cont.Items(ListBoxCount).Value) = UCase(MultipleSelection(MultipleSelectionCount)) Then
                                                    cont.Items(ListBoxCount).Selected = True
                                                End If
                                            End If
                                        Next
                                    Next
                                End If

                            Else
                                'This means that the listbox selection mode is single, which basically treats 
                                'it just like a listbox. The market status only allows one pick, because in that case
                                'it really doesn't make sense to pick more than one there.
                                cont.selectedvalue = value
                            End If
                            'This is in case the control is a checkbox, just adding a check to see if it's clicked or not.
                        ElseIf (cont.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox")) Then 'is it a checkbox?
                            cont.checked = IIf(value = "true", True, False)

                        End If
                    Else 'This means the control does not exist as a standard asp.net control.

                        'If we've picked either a typecode or a make name, we need to automatically go ahead 
                        'and run the function to get the rest of the listbox values. Meaning you get all the 
                        'correct make or correct models per your choice
                        'Since these aren't asp.net controls, we'll have to look for them 
                        'and then set the session variables so that they fill up.
                        If Company = False Then 'company does not have these controls.
                            'If ValPar(0) = "cboAircraftTypeID" Then 'Or ValPar(0) = "atype_name" Then
                            '    HttpContext.Current.Session.Item("tabAircraftType") = Replace(value, "##", ",")
                            'THE FOLLOWING IS A WORK IN PROGRESS, GOING TO COMBINE THESE CASES TOGETHER 
                            'TO MAKE THEM EASIER TO READ 10/28/13
                            If ValPar(0) = "cboYachtCategoryID" Or
                                           ValPar(0) = "yt_model_id" Or
                                           ValPar(0) = "ym_brand_name" Or
                                           ValPar(0) = "cboYachtModelID" Or
                                           ValPar(0) = "cboYachtBrandID" Or
                                           ValPar(0) = "cboYachtTypeID" Or
                                           ValPar(0) = "cboYachtSizeID" Or
                                           ValPar(0) = "atype_name" Or
                                           ValPar(0) = "cboAircraftTypeID" Or
                                           ValPar(0) = "amod_make_name" Or
                                           ValPar(0) = "cboAircraftMakeID" Or
                                           ValPar(0) = "amod_id" Or
                                           ValPar(0) = "cboAircraftModelID" Then

                                Dim ValueSaved As String = ""
                                Dim TypeValues As Array
                                Dim ValueList As String = ""

                                value = value.ToString.Trim
                                TypeValues = value.Split("##")
                                For m = 0 To UBound(TypeValues)
                                    If TypeValues(m) <> "" Then

                                        If ValueList <> "" Then
                                            ValueList += ","
                                        End If

                                        If ValPar(0) = "atype_name" Or ValPar(0) = "cboAircraftTypeID" Then
                                            Dim tempArray As Array = Split(TypeValues(m), "|")
                                            If UBound(tempArray) > 0 Then
                                                ValueList += commonEvo.FindIndexForFirstItem(UCase(tempArray(0)), crmWebClient.Constants.AIRFRAME_TYPE, tempArray(1), crmWebClient.Constants.AIRFRAME_FRAME).ToString
                                            Else
                                                ValueList += commonEvo.FindIndexForFirstItem(UCase(TypeValues(m)), crmWebClient.Constants.AIRFRAME_TYPE).ToString
                                            End If
                                        ElseIf ValPar(0) = "amod_make_name" Or ValPar(0) = "cboAircraftMakeID" Then
                                            Dim tempArray As Array = Split(TypeValues(m), "|")
                                            If UBound(tempArray) > 0 Then
                                                ValueList += commonEvo.FindIndexForItemByAmodID(CLng(tempArray(1))).ToString
                                                'ValueList += commonEvo.FindIndexForFirstItem(UCase(TypeValues(m)), crmWebClient.Constants.AIRFRAME_MAKE).ToString
                                            Else
                                                ValueList += commonEvo.FindIndexForFirstItem(UCase(TypeValues(m)), crmWebClient.Constants.AIRFRAME_MAKE).ToString
                                            End If
                                        ElseIf ValPar(0) = "amod_id" Or ValPar(0) = "cboAircraftModelID" Then
                                            ValueList += commonEvo.FindIndexForItemByAmodID(CLng(TypeValues(m))).ToString
                                        ElseIf ValPar(0) = "cboYachtCategoryID" Then 'The category needs to be saved with the motor type, otherwise you'll get the wrong category selected in the selectbox
                                            Dim tempArray As Array = Split(TypeValues(m), "|")
                                            If UBound(tempArray) > 0 Then
                                                ValueList += commonEvo.FindYachtIndexForFirstItem(UCase(tempArray(0)), crmWebClient.Constants.LOCYACHT_CATEGORY, tempArray(1), crmWebClient.Constants.LOCYACHT_MOTOR).ToString
                                            End If
                                        ElseIf ValPar(0) = "cboYachtBrandID" Or ValPar(0) = "ym_brand_name" Then 'the brand needs to be saved with the category, otherwise you'll get the wrong brand.
                                            Dim tempArray As Array = Split(TypeValues(m), "|")
                                            If UBound(tempArray) > 0 Then
                                                ValueList += commonEvo.FindYachtIndexForFirstItem(tempArray(0), crmWebClient.Constants.LOCYACHT_BRAND, tempArray(1), crmWebClient.Constants.LOCYACHT_CATEGORY).ToString
                                            Else
                                                ValueList += commonEvo.FindYachtIndexForFirstItem(TypeValues(m), crmWebClient.Constants.LOCYACHT_BRAND).ToString
                                            End If

                                        ElseIf ValPar(0) = "cboYachtModelID" Or ValPar(0) = "yt_model_id" Then
                                            ValueList += commonEvo.FindYachtIndexForItemByModelID(CLng(TypeValues(m))).ToString

                                        ElseIf ValPar(0) = "cboYachtTypeID" Then
                                            If Not TypeValues(m).ToString.ToUpper.Contains("ALL") And Not String.IsNullOrEmpty(TypeValues(m).ToString.Trim) Then
                                                ValueList += TypeValues(m).ToString
                                            End If
                                        ElseIf ValPar(0) = "cboYachtSizeID" Then
                                            If Not TypeValues(m).ToString.ToUpper.Contains("ALL") And Not String.IsNullOrEmpty(TypeValues(m).ToString.Trim) Then
                                                ValueList += TypeValues(m).ToString
                                            End If
                                        End If

                                    End If
                                Next

                                If ValPar(0) = "atype_name" Or ValPar(0) = "cboAircraftTypeID" Then
                                    HttpContext.Current.Session.Item("tabAircraftType") = ValueList
                                ElseIf ValPar(0) = "amod_make_name" Or ValPar(0) = "cboAircraftMakeID" Then
                                    HttpContext.Current.Session.Item("tabAircraftMake") = ValueList
                                ElseIf ValPar(0) = "amod_id" Or ValPar(0) = "cboAircraftModelID" Then
                                    HttpContext.Current.Session.Item("tabAircraftModel") = ValueList
                                ElseIf ValPar(0) = "cboYachtCategoryID" Then
                                    HttpContext.Current.Session.Item("tabYachtCategory") = ValueList
                                ElseIf ValPar(0) = "cboYachtTypeID" Or ValPar(0) = "ym_motor_type" Then
                                    HttpContext.Current.Session.Item("tabYachtType") = ValueList
                                ElseIf ValPar(0) = "cboYachtSizeID" Or ValPar(0) = "ym_category_size" Then
                                    HttpContext.Current.Session.Item("tabYachtSize") = ValueList
                                ElseIf ValPar(0) = "cboYachtBrandID" Or ValPar(0) = "ym_brand_name" Then
                                    HttpContext.Current.Session.Item("tabYachtBrand") = ValueList
                                ElseIf ValPar(0) = "cboYachtModelID" Or ValPar(0) = "yt_model_id" Then
                                    HttpContext.Current.Session.Item("tabYachtModel") = ValueList
                                End If

                            ElseIf ValPar(0).ToString.ToLower.Contains("ddlweightclass") Or ValPar(0).ToString.ToLower.Contains("amod_weight_class") Then
                                HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = value
                            ElseIf ValPar(0).ToString.ToLower.Contains("ddlmfrname") Or ValPar(0).ToString.ToLower.Contains("ddlmfrnameid") Then
                                HttpContext.Current.Session.Item("tabAircraftMfrNames") = Replace(value, Constants.cDymDataSeperator, Constants.cCommaDelim)
                            ElseIf ValPar(0).ToString.ToLower.Contains("ddlsizecat") Or ValPar(0).ToString.ToLower.Contains("ddlsizecatid") Then
                                HttpContext.Current.Session.Item("tabAircraftSize") = Replace(value, Constants.cDymDataSeperator, Constants.cCommaDelim)

                            ElseIf ValPar(0) = "cboTimeScaleID" Then
                                HttpContext.Current.Session.Item("marketTimeScale") = value
                            ElseIf ValPar(0) = "cboStartDateID" Then
                                HttpContext.Current.Session.Item("marketStartDate") = value
                            ElseIf ValPar(0) = "cboRangeSpanID" Then
                                HttpContext.Current.Session.Item("marketScaleSets") = value
                            ElseIf ValPar(0).ToString.ToLower.Contains("chkhelicopterfilterid") Then
                                HttpContext.Current.Session.Item("hasHelicopterFilter") = CBool(value.ToLower)
                                If value.ToLower Then
                                    HttpContext.Current.Session.Item("hasModelFilter") = True
                                End If
                            ElseIf ValPar(0).ToString.ToLower.Contains("chkbusinessfilterid") Then
                                HttpContext.Current.Session.Item("hasBusinessFilter") = CBool(value.ToLower)
                                If value.ToLower Then
                                    HttpContext.Current.Session.Item("hasModelFilter") = True
                                End If
                            ElseIf ValPar(0).ToString.ToLower.Contains("chkcommercialfilterid") Then
                                HttpContext.Current.Session.Item("hasCommercialFilter") = CBool(value.ToLower)
                                If value.ToLower Then
                                    HttpContext.Current.Session.Item("hasModelFilter") = True
                                End If
                            ElseIf ValPar(0).ToString.ToLower.Contains("hasmodelfilterid") Then
                                HttpContext.Current.Session.Item("hasModelFilter") = CBool(value.ToLower)
                            End If

                            Select Case (ValPar(0))
                                Case "radEventsValueID"
                                    HttpContext.Current.Session.Item("eventType") = value.ToUpper.Trim
                                Case "radEventsID"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.ToLower.Contains("false") Then
                                            HttpContext.Current.Session.Item("eventType") = "AIRCRAFT"
                                        End If
                                    End If
                                Case "radEventsID1"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.ToLower.Contains("false") Then
                                            HttpContext.Current.Session.Item("eventType") = "WANTED"
                                        End If
                                    End If
                                Case "radEventsID2"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.ToLower.Contains("false") Then
                                            HttpContext.Current.Session.Item("eventType") = "COMPANY"
                                        End If
                                    End If
                                Case "cboEventsCategoriesID"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.Trim.ToUpper.Contains("ALL") Then
                                            HttpContext.Current.Session.Item("eventCatType") = Replace(value, Constants.cDymDataSeperator, Constants.cCommaDelim)
                                        End If
                                    End If
                                Case "cboEventsTypeCodesID"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.Trim.ToUpper.Contains("ALL") Then
                                            HttpContext.Current.Session.Item("eventCatCode") = Replace(value, Constants.cDymDataSeperator, Constants.cCommaDelim)
                                        End If
                                    End If
                                Case "events_type_of_search"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.ToLower.Contains("false") Then
                                            HttpContext.Current.Session.Item("eventType") = "AIRCRAFT"
                                        End If
                                    End If
                                Case "events_type_of_search_1"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.ToLower.Contains("false") Then
                                            HttpContext.Current.Session.Item("eventType") = "WANTED"
                                        End If
                                    End If
                                Case "events_type_of_search_2"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.ToLower.Contains("false") Then
                                            HttpContext.Current.Session.Item("eventType") = "COMPANY"
                                        End If
                                    End If
                                Case "events_market_categories"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.Trim.ToUpper.Contains("ALL") Then
                                            HttpContext.Current.Session.Item("eventCatType") = Replace(value, Constants.cDymDataSeperator, Constants.cCommaDelim)
                                        End If
                                    End If
                                Case "events_market_types"
                                    If Not String.IsNullOrEmpty(value) Then
                                        If Not value.Trim.ToUpper.Contains("ALL") Then

                                            Dim eventCodes As Array = Split(value, Constants.cDymDataSeperator)
                                            Dim txCodeString As String = ""

                                            'Added on 6/14/2016.
                                            'In order to check against this array, it needs to be filled first. If someone has an old default folder set with events
                                            'Category/Type, this array won't be filled when you first come in. 

                                            If IsNothing(HttpContext.Current.Session.Item("EventCategoryArray")) Then
                                                commonEvo.fillEventCategoryArray("")
                                            End If

                                            For eventCodesLoop = 0 To UBound(eventCodes)

                                                For eventArrayLoop = 0 To UBound(CType(HttpContext.Current.Session.Item("EventCategoryArray"), Array))

                                                    If HttpContext.Current.Session.Item("EventCategoryArray")(eventArrayLoop, 1) = eventCodes(eventCodesLoop) Then
                                                        If String.IsNullOrEmpty(txCodeString.Trim) Then
                                                            txCodeString = HttpContext.Current.Session.Item("EventCategoryArray")(eventArrayLoop, 2)
                                                        Else
                                                            txCodeString += Constants.cCommaDelim + HttpContext.Current.Session.Item("EventCategoryArray")(eventArrayLoop, 2)
                                                        End If
                                                    End If

                                                Next

                                            Next

                                            HttpContext.Current.Session.Item("eventCatCode") = txCodeString
                                        End If
                                    End If
                            End Select

                        End If

                        'company region setup
                        If ValPar(0) = "cboCompanyRegionID" Then
                            HttpContext.Current.Session.Item("companyRegion") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "cboBaseRegionID" Then
                            HttpContext.Current.Session.Item("baseRegion") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "country_continent_name" Then
                            HttpContext.Current.Session.Item("companyRegionOrContinent") = "continent"
                            HttpContext.Current.Session.Item("companyRegion") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "comp_country" Then
                            HttpContext.Current.Session.Item("companyCountry") = value
                        ElseIf ValPar(0) = "cboCompanyCountryID" Then
                            HttpContext.Current.Session.Item("companyCountry") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "ac_aport_state" Or ValPar(0) = "ac_aport_state_name" Or ValPar(0) = "cboBaseStateID" Then
                            HttpContext.Current.Session.Item("baseState") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "ac_aport_country" Or ValPar(0) = "cboBaseCountryID" Then
                            HttpContext.Current.Session.Item("baseCountry") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "ac_country_continent_name" Then
                            HttpContext.Current.Session.Item("baseRegion") = Replace(value, "##", ",")
                            HttpContext.Current.Session.Item("baseRegionOrContinent") = "continent"
                        ElseIf ValPar(0) = "comp_state" Then
                            HttpContext.Current.Session.Item("companyState") = value
                        ElseIf ValPar(0) = "state_name" Then
                            HttpContext.Current.Session.Item("companyState") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "cboCompanyStateID" Then
                            HttpContext.Current.Session.Item("companyState") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "cboCompanyTimeZoneID" Then
                            HttpContext.Current.Session.Item("companyTimeZone") = Replace(value, "##", ",")
                        ElseIf ValPar(0) = "radContinentRegionID" Then
                            If value = "true" Then
                                'This means the radio for continent was checked
                                HttpContext.Current.Session.Item("companyRegionOrContinent") = "continent"
                            Else 'this means the radio for region was checked, since one or the other needs to be checked
                                HttpContext.Current.Session.Item("companyRegionOrContinent") = "region"
                            End If
                        ElseIf ValPar(0) = "radBaseContinentRegionID" Then
                            If value = "true" Then
                                'This means the radio for continent was checked
                                HttpContext.Current.Session.Item("baseRegionOrContinent") = "continent"
                            Else 'this means the radio for region was checked, since one or the other needs to be checked
                                HttpContext.Current.Session.Item("baseRegionOrContinent") = "region"
                            End If
                        ElseIf ValPar(0) = "COMPARE_journ_date_operator" Then
                            HttpContext.Current.Session.Item("searchCriteria").SearchCriteriaHistoryDateOperator = ValPar(1)
                            HttpContext.Current.Session.Item("searchCriteria").SearchCriteriaHistoryDate = ""
                        End If
                    End If
                End If
            Next

        Catch ex As Exception

            commonLogFunctions.Log_User_Event_Data("UserError", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + "): " + ex.Message.Trim

        End Try

    End Sub

    Public Shared Function ReturnProductCodeCount(ByVal productCodeCount As Integer) As Integer
        'Added a check to make sure that the product code exists and is set before using it in this function
        '1/26/16
        If Not IsNothing(HttpContext.Current.Session.Item("localPreferences").ProductCode) Then
            For nloop As Integer = 0 To UBound(HttpContext.Current.Session.Item("localPreferences").ProductCode)

                Select Case HttpContext.Current.Session.Item("localPreferences").ProductCode(nloop)
                    Case eProductCodeTypes.H
                        productCodeCount += 1
                    Case eProductCodeTypes.B, eProductCodeTypes.S, eProductCodeTypes.I
                        productCodeCount += 1
                    Case eProductCodeTypes.R
                    Case eProductCodeTypes.C
                        productCodeCount += 1
                    Case eProductCodeTypes.P
                    Case eProductCodeTypes.A
                    Case eProductCodeTypes.Y

                End Select

            Next
        End If
        Return productCodeCount
    End Function

    ''' <summary>
    ''' Setting up the session based on common type/make/model control
    ''' </summary>
    ''' <param name="sTypeMakeModelCtrlBaseName"></param>
    ''' <remarks></remarks>
    Public Shared Sub FillUpSessionForMakeTypeModel(ByVal sTypeMakeModelCtrlBaseName As String, ByVal ViewTMMDropDowns As crmWebClient.viewTypeMakeModelCtrl)

        Try

            ' because these values are needed on this page they need to match the control names in the control
            ' so the request header picks up the right values
            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Type")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Type").ToString) Then
                    If Not HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Type").ToString.ToLower.Contains("all") Then
                        HttpContext.Current.Session.Item("tabAircraftType") = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Type").ToString.Trim
                    Else
                        HttpContext.Current.Session.Item("tabAircraftModel") = ""
                        HttpContext.Current.Session.Item("tabAircraftMake") = ""
                        HttpContext.Current.Session.Item("tabAircraftType") = ""
                    End If
                End If
            End If

            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Make")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Make").ToString) Then
                    If Not HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Make").ToString.ToLower.Contains("all") Then
                        HttpContext.Current.Session.Item("tabAircraftMake") = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Make").ToString.Trim
                    Else
                        HttpContext.Current.Session.Item("tabAircraftModel") = ""
                        HttpContext.Current.Session.Item("tabAircraftMake") = ""
                    End If
                End If
            End If

            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Model")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Model").ToString) Then
                    If Not HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Model").ToString.ToLower.Contains("all") Then
                        HttpContext.Current.Session.Item("tabAircraftModel") = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Model").ToString.Trim
                    Else
                        HttpContext.Current.Session.Item("tabAircraftModel") = ""
                    End If
                End If
            End If

            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            Else
                If Not IsNothing(ViewTMMDropDowns) Then
                    If Not String.IsNullOrEmpty(ViewTMMDropDowns.getWeightClass.Trim) Then
                        If Not ViewTMMDropDowns.getWeightClass.ToString.ToLower.Contains("all") Then
                            HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ViewTMMDropDowns.getWeightClass.ToString
                        Else
                            HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
                        End If
                    Else
                        HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
                    End If
                Else
                    HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
                End If
            End If

            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("ddlMfrName")) Then ' 
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("ddlMfrName").ToString) Then
                        If Not HttpContext.Current.Request.Item("ddlMfrName").ToString.ToLower.Contains("all") Then
                            HttpContext.Current.Session.Item("tabAircraftMfrNames") = HttpContext.Current.Request.Item("ddlMfrName").ToString.Trim
                        Else
                            HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
                        End If
                    Else
                        HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
                    End If
                Else
                    HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
                End If
            End If

            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("ddlSizeCat")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("ddlSizeCat").ToString) Then
                        If Not HttpContext.Current.Request.Item("ddlSizeCat").ToString.ToLower.Contains("all") Then
                            HttpContext.Current.Session.Item("tabAircraftSize") = HttpContext.Current.Request.Item("ddlSizeCat").ToString.Trim
                        Else
                            HttpContext.Current.Session.Item("tabAircraftSize") = ""
                        End If
                    Else
                        HttpContext.Current.Session.Item("tabAircraftSize") = ""
                    End If
                Else
                    HttpContext.Current.Session.Item("tabAircraftSize") = ""
                End If
            End If

        Catch ex As Exception

            commonLogFunctions.forceLogError("UserError", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + "): " + ex.Message.Trim

        End Try

    End Sub
    ''' <summary>
    ''' This pulls back the information from the common type/make/model control
    ''' </summary>
    ''' <param name="sTypeMakeModelCtrlBaseName"></param>
    ''' <param name="BuildSearchString"></param>
    ''' <param name="ModelsString"></param>
    ''' <param name="MakeString"></param>
    ''' <param name="TypeString"></param>
    ''' <param name="AirframeTypeString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMakeModelTypeFromCommonControl(ByVal sTypeMakeModelCtrlBaseName As String, ByVal BuildSearchString As String,
                                                           ByRef ModelsString As String, ByRef MakeString As String,
                                                           ByRef TypeString As String, ByRef AirframeTypeString As String,
                                                           ByRef CombinedAirframeTypeString As String,
                                                           ByRef WeightClassDDL As Object, ByRef WeightClassStr As String,
                                                           ByRef ManufacturerStr As String, ByRef AcSizeStr As String,
                                                           Optional ByRef Business As Boolean = False,
                                                           Optional ByRef Helicopter As Boolean = False,
                                                           Optional ByRef Commercial As Boolean = False, Optional ByRef amod_id_list As String = "") As String
        Dim sAirframeType As String = ""
        Dim sAirType As String = ""
        Dim sMake As String = ""
        Dim sModel As String = ""
        Dim sUsage As String = ""
        Dim sAirFrame As String = ""

        Dim ModelTextDisplay As String = ""

        Try

            '-------------------------------------------------------------
            'BUSINESS CHECKBOX
            Dim VariableBusiness As Boolean = False
            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableBusiness = HttpContext.Current.Session.Item("chkBusinessFilter")
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("chkBusinessFilter")) Then
                    VariableBusiness = HttpContext.Current.Request.Item("chkBusinessFilter")
                End If
            End If

            'Added a small check. If their business flag is false, this is always false no matter. 
            If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False Then
                VariableBusiness = False
            End If

            If HttpContext.Current.Session.Item("hasModelFilter") Then
                VariableBusiness = HttpContext.Current.Session.Item("hasBusinessFilter")
            Else
                HttpContext.Current.Session.Item("hasBusinessFilter") = VariableBusiness
            End If

            Business = VariableBusiness

            '-------------------------------------------------------------
            'COMMERCIAL CHECKBOX
            Dim VariableCommercial As Boolean = False
            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableCommercial = HttpContext.Current.Session.Item("chkCommercialFilter")
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("chkCommercialFilter")) Then
                    VariableCommercial = HttpContext.Current.Request.Item("chkCommercialFilter")
                End If
            End If

            'Added a small check. If their Commercial flag is false, this is always false no matter. 
            If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
                VariableCommercial = False
            End If

            If HttpContext.Current.Session.Item("hasModelFilter") Then
                VariableCommercial = HttpContext.Current.Session.Item("hasCommercialFilter")
            Else
                HttpContext.Current.Session.Item("hasCommercialFilter") = VariableCommercial
            End If

            Commercial = VariableCommercial

            '-------------------------------------------------------------
            'HELICOPTER CHECKBOX
            Dim VariableHelicopter As Boolean = False
            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableHelicopter = HttpContext.Current.Session.Item("chkHelicopterFilter")
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("chkHelicopterFilter")) Then
                    VariableHelicopter = HttpContext.Current.Request.Item("chkHelicopterFilter")
                End If
            End If

            'Added a small check. If their Helicopter flag is false, this is always false no matter. 
            If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False Then
                VariableHelicopter = False
            End If

            If HttpContext.Current.Session.Item("hasModelFilter") Then
                VariableHelicopter = HttpContext.Current.Session.Item("hasHelicopterFilter")
            Else
                HttpContext.Current.Session.Item("hasHelicopterFilter") = VariableHelicopter
            End If

            Helicopter = VariableHelicopter

            ''''''''''''''''''''''''''''''''''' 
            'Here's one more small check.
            'We check to see if all three are false.
            'If they are, we set whatever's set up in session.
            If Business = False And Helicopter = False And Commercial = False Then
                'Setting up Business in Session
                Business = HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag
                'Setting up Helicopter in Session
                Helicopter = HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag
                'Setting up Commercial in session
                Commercial = HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag
            End If

            'TYPE 
            Dim VariableType As String = ""
            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableType = HttpContext.Current.Session.Item("tabAircraftType")
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftType")) Then
                    VariableType = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftType")
                End If
            End If

            If Not IsNothing(VariableType) Then
                If Not String.IsNullOrEmpty(VariableType.ToString) Then
                    If Not VariableType.ToString.ToLower.Contains("all") Then
                        HttpContext.Current.Session.Item("tabAircraftType") = VariableType.ToString.Trim

                        Dim TypeArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftType"), ",")
                        For MultipleModelCount = 0 To UBound(TypeArray)
                            Dim CurrentModelCount As Long = CLng(TypeArray(MultipleModelCount))

                            commonEvo.ReturnModelDataFromIndex(CurrentModelCount, sAirframeType, sAirType, sMake, sModel, sUsage)

                            If TypeString <> "" Then
                                TypeString += ", "
                                AirframeTypeString += ", "
                                CombinedAirframeTypeString += ","
                            End If

                            TypeString += "'" & sAirType & "'"
                            AirframeTypeString += "'" & sAirframeType & "'"
                            CombinedAirframeTypeString += sAirType & "|" & sAirframeType
                        Next
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(TypeString, "'", ""), "Type(s)")
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(AirframeTypeString, "'", ""), "Airframe Type(s)")
                    Else
                        HttpContext.Current.Session.Item("tabAircraftModel") = ""
                        HttpContext.Current.Session.Item("tabAircraftMake") = ""
                        HttpContext.Current.Session.Item("tabAircraftType") = ""
                    End If
                End If
            End If

            'MAKE
            Dim VariableMake As String = ""
            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableMake = HttpContext.Current.Session.Item("tabAircraftMake")
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftMake")) Then
                    VariableMake = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftMake")
                End If
            End If
            If Not IsNothing(VariableMake) Then
                If Not String.IsNullOrEmpty(VariableMake.ToString) Then
                    If Not VariableMake.ToString.ToLower.Contains("all") Then
                        HttpContext.Current.Session.Item("tabAircraftMake") = VariableMake.ToString.Trim

                        Dim MakeArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftMake"), ",")
                        For MultipleModelCount = 0 To UBound(MakeArray)
                            Dim CurrentModelCount As Long = CLng(MakeArray(MultipleModelCount))

                            commonEvo.ReturnModelDataFromIndex(CurrentModelCount, sAirframeType, sAirType, sMake, sModel, sUsage)

                            If MakeString <> "" Then
                                MakeString += ", "
                            End If

                            MakeString += "'" & Replace(sMake, "'", "''") & "'"
                        Next
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(MakeString, "'", ""), "Make(s)")

                    Else
                        HttpContext.Current.Session.Item("tabAircraftModel") = ""
                        HttpContext.Current.Session.Item("tabAircraftMake") = ""
                    End If
                End If
            End If

            'MODEL
            Dim VariableModel As String = ""
            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableModel = HttpContext.Current.Session.Item("tabAircraftModel")
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftModel")) Then
                    VariableModel = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftModel")
                End If
            End If
            If Not IsNothing(VariableModel) Then
                If Not String.IsNullOrEmpty(VariableModel.ToString) Then
                    If Not VariableModel.ToString.ToLower.Contains("all") Then
                        HttpContext.Current.Session.Item("tabAircraftModel") = VariableModel.ToString.Trim

                        Dim ModelArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftModel"), ",")

                        For MultipleModelCount = 0 To UBound(ModelArray)
                            Dim CurrentModelCount As Long = CLng(ModelArray(MultipleModelCount))

                            commonEvo.ReturnModelDataFromIndex(CurrentModelCount, sAirframeType, sAirType, sMake, sModel, sUsage, "", "", amod_id_list)

                            If ModelsString <> "" Then
                                ModelsString += ","
                                ModelTextDisplay += ", "
                            End If

                            ModelsString += commonEvo.ReturnAmodIDForItemIndex(CurrentModelCount).ToString
                            ModelTextDisplay += sModel
                        Next
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ModelTextDisplay, "Model(s)")
                    Else
                        HttpContext.Current.Session.Item("tabAircraftModel") = ""
                    End If
                End If
            End If

            'WEIGHT CLASS
            Dim VariableWeight As String = ""
            Dim displayWeightClassString As String = ""

            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableWeight = HttpContext.Current.Session.Item("tabAircraftModelWeightClass")
            Else
                If Not IsNothing(HttpContext.Current.Session.Item("tabAircraftModelWeightClass")) Then
                    If TypeOf WeightClassDDL Is DropDownList Then
                        VariableWeight = WeightClassDDL.selectedValue
                    End If
                End If
            End If

            If Not IsNothing(VariableWeight) Then
                If Not String.IsNullOrEmpty(VariableWeight.Trim) Then
                    If Not VariableWeight.ToLower.Contains("all") Then


                        For Each li In WeightClassDDL.Items
                            If li.Selected Then

                                If String.IsNullOrEmpty(WeightClassStr.Trim) Then
                                    WeightClassStr = li.Value.ToString.Trim
                                    displayWeightClassString = commonEvo.TranslateAcWeightClass(li.Value.ToString.Trim)
                                Else
                                    WeightClassStr += Constants.cCommaDelim + li.Value.ToString.Trim
                                    displayWeightClassString += Constants.cCommaDelim + commonEvo.TranslateAcWeightClass(li.Value.ToString.Trim)
                                End If

                            End If
                        Next

                        ' first add the "tick" for the text
                        WeightClassStr = WeightClassStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator)

                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(displayWeightClassString, "Weight Class")
                    Else
                        HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
                    End If
                End If
            End If

            'Manufacturer
            Dim VariableManufacturer As String = ""
            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableManufacturer = HttpContext.Current.Session.Item("tabAircraftMfrNames")
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("ddlMfrName")) Then
                    VariableManufacturer = HttpContext.Current.Request.Item("ddlMfrName")
                End If
            End If

            If Not IsNothing(VariableManufacturer) Then
                If Not String.IsNullOrEmpty(VariableManufacturer.Trim) Then
                    If Not VariableManufacturer.ToString.ToLower.Contains("all") Then

                        HttpContext.Current.Session.Item("tabAircraftMfrNames") = VariableManufacturer.ToString.Trim

                        Dim MfrArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftMfrNames"), ",")

                        For mfrNameCount = 0 To UBound(MfrArray)

                            If String.IsNullOrEmpty(ManufacturerStr.Trim) Then
                                ManufacturerStr = MfrArray(mfrNameCount)
                            Else
                                ManufacturerStr += Constants.cCommaDelim + MfrArray(mfrNameCount)
                            End If

                        Next

                        Dim displayMfrString As String = ManufacturerStr

                        ' first add the "tick" for the text
                        ManufacturerStr = ManufacturerStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator)

                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(displayMfrString, "Manufacturer")
                    Else
                        HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
                    End If
                End If
            End If

            'Ac Size
            Dim VariableAcSize As String = ""
            Dim displayAcSizeString As String = ""

            If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
                VariableAcSize = HttpContext.Current.Session.Item("tabAircraftSize")
            Else
                If Not IsNothing(HttpContext.Current.Request.Item("ddlSizeCat")) Then
                    VariableAcSize = HttpContext.Current.Request.Item("ddlSizeCat")
                End If
            End If

            If Not IsNothing(VariableAcSize) Then
                If Not String.IsNullOrEmpty(VariableAcSize.Trim) Then
                    If Not VariableAcSize.ToString.ToLower.Contains("all") Then

                        HttpContext.Current.Session.Item("tabAircraftSize") = VariableAcSize.ToString.Trim

                        Dim AcSizeArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftSize"), ",")


                        For acSizeCount = 0 To UBound(AcSizeArray)

                            If String.IsNullOrEmpty(AcSizeStr.Trim) Then
                                AcSizeStr = AcSizeArray(acSizeCount)
                                displayAcSizeString = commonEvo.TranslateAcSizes(AcSizeArray(acSizeCount))
                            Else
                                AcSizeStr += Constants.cCommaDelim + AcSizeArray(acSizeCount)
                                displayAcSizeString += Constants.cCommaDelim + commonEvo.TranslateAcSizes(AcSizeArray(acSizeCount))
                            End If

                        Next

                        ' first add the "tick" for the text
                        AcSizeStr = AcSizeStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator)

                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(displayAcSizeString, "Size")
                    Else
                        HttpContext.Current.Session.Item("tabAircraftSize") = ""
                    End If
                End If
            End If

        Catch ex As Exception

            commonLogFunctions.forceLogError("UserError", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + "): " + ex.Message.Trim

        End Try

        Return BuildSearchString

    End Function

    ''' <summary>
    ''' Pulls region information.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks> 
    Public Shared Function GetRegionInfoFromCommonControl(ByVal sTypeMakeModelCtrlBaseName As String, ByRef BuildSearchString As String, ByRef CountryString As String, ByRef TimeZoneString As String, ByRef ContinentString As String, ByRef RegionString As String, ByRef StateName As String) As String

        '---------------------------------------------------------------------------------------
        '-------------------------------Continent/Region----------------------------------------
        '---------------------------------------------------------------------------------------
        'Need to check if the continent box is checked, this means that search is continent/region
        Dim VariableContinentRegion As String = ""
        Dim HoldString As String = ""
        ' 
        'Getting the Continent, either from session (project) or from control (search)
        If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            VariableContinentRegion = HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Region")
        Else
            HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Region") = ""
            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Region")) Then
                VariableContinentRegion = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Region")
            End If
        End If
        'Set the radio button session
        If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "RegionOrContinent") = HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "RegionOrContinent")
        Else
            If Not IsNothing(HttpContext.Current.Request.Item("rad" & IIf(sTypeMakeModelCtrlBaseName = "Base", "Base", "") & "ContinentRegion")) Then
                HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "RegionOrContinent") = HttpContext.Current.Request.Item("rad" & IIf(sTypeMakeModelCtrlBaseName = "Base", "Base", "") & "ContinentRegion").ToString.ToLower
            End If
        End If
        '------------------------------------------------------------------------------------------------
        'Splitting The Continent to set up the string for the where clause and the search text display.
        If Not IsNothing(VariableContinentRegion) Then
            If Not String.IsNullOrEmpty(VariableContinentRegion.ToString) Then
                If Not VariableContinentRegion.ToString.ToLower.Contains("all") Then
                    HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Region") = VariableContinentRegion
                    Dim ContinentRegionArray As Array = Split(HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Region"), ",")
                    For MultipleContinentRegionCount = 0 To UBound(ContinentRegionArray)
                        Dim ContinentRegionCountry As String = CStr(ContinentRegionArray(MultipleContinentRegionCount))
                        If HoldString <> "" Then
                            HoldString += ", "
                        End If
                        HoldString += "'" & ContinentRegionCountry & "'"
                    Next
                    'Now we're overwriting the session variable to account for needing spaces
                    HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Region") = Replace(HoldString, "'", "")

                    'Check whether it's a region or a continent
                    If LCase(HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "RegionOrContinent")) = "continent" Then
                        ContinentString = HoldString
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(ContinentString, "'", ""), sTypeMakeModelCtrlBaseName & " Continent(s)")
                        If ContinentString <> "" Then 'in cases where an ampersand is used in the listbox, we need to replace it in our search variable that we're sending back.
                            'Replacing it here because otherwise it would need to be changed in every single search that uses region or continent.
                            ContinentString = Replace(ContinentString, "&amp;", "&")
                        End If
                        'ElseIf LCase(HttpContext.Current.Request("rad" & IIf(sTypeMakeModelCtrlBaseName = "Base", "Base", "") & "ContinentRegion")) = "region" Then
                    ElseIf LCase(HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "RegionOrContinent")) = "region" Then
                        RegionString = HoldString
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(RegionString, "'", ""), sTypeMakeModelCtrlBaseName & " Region(s)")
                    End If
                Else
                    ' HttpContext.Current.Session.Item("companyRegion") = ""
                End If
            End If
        End If
        '---------------------------------------------------------------------------------------
        '------------------------------------Timezone--------------------------------------------
        '---------------------------------------------------------------------------------------
        'Getting the Timezone, either from session (project) or from control (search)
        Dim VariableTimeZone As String = ""
        If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            VariableTimeZone = HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "TimeZone")
        Else
            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "TimeZone")) Then
                VariableTimeZone = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "TimeZone")
            End If
        End If
        '------------------------------------------------------------------------------------------------
        'Splitting The TimeZone to set up the string for the where clause and the search text display.
        If Not IsNothing(VariableTimeZone) Then
            If Not String.IsNullOrEmpty(VariableTimeZone.ToString) Then
                If Not VariableTimeZone.ToString.ToLower.Contains("all") Then
                    HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "TimeZone") = VariableTimeZone.ToString.Trim
                    Dim TimeZoneArray As Array = Split(HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "TimeZone"), ",")
                    For MultipleTimeZoneCount = 0 To UBound(TimeZoneArray)
                        Dim TimeZoneCountry As String = CStr(TimeZoneArray(MultipleTimeZoneCount))
                        If TimeZoneString <> "" Then
                            TimeZoneString += ", "
                        End If
                        TimeZoneString += "'" & TimeZoneCountry & "'"
                    Next
                    'Now we're overwriting the session variable to account for needing spaces
                    HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "TimeZone") = Replace(TimeZoneString, "'", "")

                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(TimeZoneString, "'", ""), sTypeMakeModelCtrlBaseName & " TimeZone(s)")
                End If
            End If
        End If
        '---------------------------------------------------------------------------------------
        '------------------------------------COUNTRY--------------------------------------------
        '---------------------------------------------------------------------------------------
        'Getting the country, either from session (project) or from control (search)
        Dim VariableCountry As String = ""
        If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            VariableCountry = HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Country")
        Else
            HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Country") = ""
            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Country")) Then
                VariableCountry = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Country")
            End If
        End If
        '------------------------------------------------------------------------------------------------
        'Splitting The country to set up the string for the where clause and the search text display.
        If Not IsNothing(VariableCountry) Then
            If Not String.IsNullOrEmpty(VariableCountry.ToString) Then
                If Not VariableCountry.ToString.ToLower.Contains("all,") And Not VariableCountry.ToString.ToLower = "all" Then
                    Dim SessionCountry As String = "" 'Created a new variable to just store the session information.
                    'This will clear up the single quote issue on reselection.
                    HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Country") = VariableCountry.ToString.Trim
                    Dim CountryArray As Array = Split(HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Country"), ",")
                    For MultipleCountryCount = 0 To UBound(CountryArray)
                        Dim CurrentCountry As String = CStr(CountryArray(MultipleCountryCount))
                        If CountryString <> "" Then
                            CountryString += ", "
                            SessionCountry += ", "
                        End If
                        CountryString += "'" & Replace(CurrentCountry, "'", "''") & "'"
                        SessionCountry += "" & CurrentCountry & ""
                    Next
                    'Now we're overwriting the session variable to account for needing spaces
                    HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "Country") = SessionCountry 'Replace(CountryString, "'", "")
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(CountryString, "'", ""), sTypeMakeModelCtrlBaseName & " Country(ies)")
                End If
            End If
        End If
        '---------------------------------------------------------------------------------------
        '-------------------------------------STATES--------------------------------------------
        '---------------------------------------------------------------------------------------
        '---------------------------------------------------------------------------------------
        ''''''''''''''''''''''''''''''''''''''''STATE NAME''''''''''''''''''''''''''''''''''''''
        ''Getting the states, either from session (project) or from control (search)
        'Dim VariableStateName As String = ""
        'If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
        '    VariableStateName = HttpContext.Current.Session.Item("translated" & sTypeMakeModelCtrlBaseName & "State")
        'Else
        '    If Not IsNothing(HttpContext.Current.Request.Item("translated" + sTypeMakeModelCtrlBaseName + "States")) Then
        '        VariableStateName = HttpContext.Current.Request.Item("translated" + sTypeMakeModelCtrlBaseName + "States")
        '    End If
        'End If

        ''------------------------------------------------------------------------------------------------
        ''Splitting The state to set up the string for the where clause and the search text display.
        'If Not IsNothing(VariableStateName) Then
        '    If Not String.IsNullOrEmpty(VariableStateName.ToString) Then
        '        If Not VariableStateName.ToString.ToLower.Contains("all") Then
        '            HttpContext.Current.Session.Item("translated" & sTypeMakeModelCtrlBaseName & "State") = VariableStateName.ToString.Trim
        '            Dim StateNameArray As Array = Split(HttpContext.Current.Session.Item("translated" & sTypeMakeModelCtrlBaseName & "State"), ",")
        '            For MultipleStateNameCount = 0 To UBound(StateNameArray)
        '                Dim CurrentStateName As String = CStr(StateNameArray(MultipleStateNameCount))
        '                If Trim(CurrentStateName) <> "" Then
        '                    If StateName <> "" Then
        '                        StateName += ", "
        '                    End If
        '                    StateName += "'" & CurrentStateName & "'"
        '                End If
        '            Next
        '            'Now we're overwriting the session variable to account for needing spaces
        '            HttpContext.Current.Session.Item("translated" & sTypeMakeModelCtrlBaseName & "State") = Replace(StateName, "'", "")
        '            BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(StateName, "'", ""), sTypeMakeModelCtrlBaseName & " State Name(s)/Province(s)")
        '        End If
        '    End If
        'End If

        ''''''''''''''''''''''''''''''''''''''''STATE CODE''''''''''''''''''''''''''''''''''''''
        'Getting the states, either from session (project) or from control (search)
        Dim VariableState As String = ""
        If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            VariableState = HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "State")
        Else
            HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "State") = ""
            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "State")) Then
                VariableState = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "State")
            End If
        End If

        '------------------------------------------------------------------------------------------------
        'Splitting The state to set up the string for the where clause and the search text display.
        If Not IsNothing(VariableState) Then
            If Not String.IsNullOrEmpty(VariableState.ToString) Then
                If Not VariableState.ToString.ToLower.Contains("all") Then
                    HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "State") = VariableState.ToString.Trim
                    Dim StateArray As Array = Split(HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "State"), ",")
                    For MultipleStateCount = 0 To UBound(StateArray)
                        Dim CurrentState As String = CStr(StateArray(MultipleStateCount))
                        If StateName <> "" Then
                            StateName += ", "
                        End If
                        StateName += "'" & Trim(CurrentState) & "'"
                    Next
                    'Now we're overwriting the session variable to account for needing spaces
                    HttpContext.Current.Session.Item(LCase(sTypeMakeModelCtrlBaseName) & "State") = Replace(StateName, "'", "")
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(StateName, "'", ""), sTypeMakeModelCtrlBaseName & " State(s)/Province(s)")
                End If
            End If
        End If
        '-----------------------------------------------------------------------------------------------------


        Return BuildSearchString
    End Function

    ''' <summary>
    ''' This is a work in progress for the yachts search page. 
    ''' </summary>
    ''' <param name="TypeofControl">tab or view, depending</param>
    ''' <param name="sYachtCategoryModelCtrlBaseName">the name you pick - on listing page 'yacht'</param>
    ''' <param name="YachtCategory">variable for category</param>
    ''' <param name="YachtBrand">variable for brand</param>
    ''' <param name="YachtModel">variable for model</param>
    ''' <param name="BuildSearchString">search string to be displayed as text.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetYachtBrandCategoryFromCommonControl(ByVal TypeofControl As String, ByVal sYachtCategoryModelCtrlBaseName As String, ByRef YachtCategory As String, ByRef YachtBrand As String, ByRef YachtModel As String, ByRef YachtCategoryCombinedMotor As String, ByRef BuildSearchString As String)
        Dim sMotor_type As String = ""
        Dim sCategory As String = ""
        Dim sBrand As String = ""
        Dim sModel As String = ""

        '----------------------------------------------------------------------------------------------------
        '---------------------------------------Yacht Category-----------------------------------------------
        '----------------------------------------------------------------------------------------------------
        '----------------------------------------------------------------------------------------------------
        Dim VariableYachtCategory As String = ""
        Dim HoldString As String = ""
        If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            VariableYachtCategory = HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category")
        Else
            HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category") = ""
            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Category")) Then
                VariableYachtCategory = HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Category")
            End If
        End If
        '------------------------------------------------------------------------------------------------
        'Splitting The Category to set up the string for the where clause and the search text display.
        If Not IsNothing(VariableYachtCategory) Then
            If Not String.IsNullOrEmpty(VariableYachtCategory.ToString) Then
                If Not VariableYachtCategory.ToString.ToLower.Contains("all") Then
                    Dim MotorDisplay As String = ""
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category") = VariableYachtCategory
                    Dim CategoryArray As Array = Split(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category"), ",")
                    For MultipleCategoryCount = 0 To UBound(CategoryArray)
                        Dim TempCategoryHold As String = CStr(CategoryArray(MultipleCategoryCount))
                        Dim ModelIDHold As Long = 0
                        ModelIDHold = commonEvo.ReturnYachtModelIDForItemIndex(CLng(TempCategoryHold))
                        If commonEvo.ReturnYachtModelDataFromIndex(CLng(TempCategoryHold), sMotor_type, sCategory, sBrand, sModel) Then
                            If HoldString <> "" Then
                                HoldString += ", "
                                YachtCategoryCombinedMotor += ","
                                MotorDisplay += ","
                            End If
                            HoldString += "'" & sCategory & "'"
                            YachtCategoryCombinedMotor += sCategory & "|" & sMotor_type
                            MotorDisplay += sMotor_type
                        End If
                    Next
                    'Now we're overwriting the session variable to account for needing spaces
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category") = Replace(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category"), "'", "")
                    YachtCategory = HoldString

                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(YachtCategory, "'", ""), sYachtCategoryModelCtrlBaseName & " Category(ies)")
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(MotorDisplay, ",", ", "), sYachtCategoryModelCtrlBaseName & " Motor(s)")

                Else
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category") = ""
                End If
            Else
                HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category") = ""
            End If
        End If
        '----------------------------------------------------------------------------------------------------
        '-----------------------------------------Yacht Brand------------------------------------------------
        '----------------------------------------------------------------------------------------------------
        '----------------------------------------------------------------------------------------------------
        Dim VariableYachtBrand As String = ""
        HoldString = ""
        If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            VariableYachtBrand = HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand")
        Else
            HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = ""
            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Brand")) Then
                VariableYachtBrand = HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Brand")
            End If
        End If

        '------------------------------------------------------------------------------------------------
        'Splitting The Brand to set up the string for the where clause and the search text display.
        If Not IsNothing(VariableYachtBrand) Then
            Dim CategoryHoldString As String = ""
            If Not String.IsNullOrEmpty(VariableYachtBrand.ToString) Then
                If Not VariableYachtBrand.ToString.ToLower.Contains("all") Then
                    YachtCategoryCombinedMotor = ""
                    'CategoryHoldString = ""
                    'YachtCategory = "" 'We're going to clear the category here so we can go ahead and rebuild it based on
                    'the brands that were picked. If we pick a brand and no category, it will go ahead and select the applicable category otherwise it will
                    'not select when the page comes back
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = VariableYachtBrand
                    Dim BrandArray As Array = Split(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand"), ",")
                    For MultipleBrandCount = 0 To UBound(BrandArray)
                        Dim TempBrandHold As String = CStr(BrandArray(MultipleBrandCount))
                        Dim ModelIDHold As Long = 0

                        ModelIDHold = commonEvo.ReturnYachtModelIDForItemIndex(CLng(TempBrandHold))
                        If commonEvo.ReturnYachtModelDataFromIndex(CLng(TempBrandHold), sMotor_type, sCategory, sBrand, sModel) Then
                            If HoldString <> "" Then
                                HoldString += ", "
                                CategoryHoldString += ", "
                                YachtCategoryCombinedMotor += ", "
                                YachtCategory += ", "
                            End If
                            HoldString += "'" & sBrand & "'"
                            'We need to look up the category index
                            YachtCategory += "'" & sCategory & "'"
                            YachtCategoryCombinedMotor += sCategory & "|" & sMotor_type
                            CategoryHoldString += "" & commonEvo.FindYachtIndexForFirstItem(sCategory, crmWebClient.Constants.LOCYACHT_CATEGORY, sBrand, crmWebClient.Constants.LOCYACHT_BRAND) & ""
                        End If
                    Next
                    'Now we're overwriting the session variable to account for needing spaces
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = Replace(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand"), "'", "")
                    YachtBrand = HoldString

                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(YachtBrand, "'", ""), sYachtCategoryModelCtrlBaseName & " Brand(s)")
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category") = CategoryHoldString 'Replace(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Category"), "'", "")
                Else
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = ""
                End If
            Else
                HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = ""
            End If
        End If
        '----------------------------------------------------------------------------------------------------
        '-----------------------------------------Yacht Model------------------------------------------------
        '----------------------------------------------------------------------------------------------------
        '----------------------------------------------------------------------------------------------------
        Dim VariableYachtModel As String = ""
        HoldString = ""
        If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
            VariableYachtModel = HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model")
        Else
            HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = ""
            If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Model")) Then
                VariableYachtModel = HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Model")
            End If
        End If
        '------------------------------------------------------------------------------------------------
        'Splitting The Model to set up the string for the where clause and the search text display.
        If Not IsNothing(VariableYachtModel) Then
            If Not String.IsNullOrEmpty(VariableYachtModel.ToString) Then
                If Not VariableYachtModel.ToString.ToLower.Contains("all") Then
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = VariableYachtModel
                    Dim ModelArray As Array = Split(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model"), ",")
                    For MultipleModelCount = 0 To UBound(ModelArray)
                        Dim TempModelHold As String = CStr(ModelArray(MultipleModelCount))
                        Dim ModelIDHold As Long = 0
                        ModelIDHold = commonEvo.ReturnYachtModelIDForItemIndex(CLng(TempModelHold))
                        If commonEvo.ReturnYachtModelDataFromIndex(CLng(TempModelHold), sMotor_type, sCategory, sBrand, sModel) Then
                            If HoldString <> "" Then
                                HoldString += ", "
                                YachtModel += ", "
                            End If
                            HoldString += "'" & sModel & "'"
                            YachtModel += "" & ModelIDHold & ""
                        End If
                    Next
                    'Now we're overwriting the session variable to account for needing spaces
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = Replace(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model"), "'", "")

                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(HoldString, "'", ""), sYachtCategoryModelCtrlBaseName & " Model(s)")

                Else
                    HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = ""
                End If
            Else
                HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = ""
            End If
        End If


        Return BuildSearchString
    End Function

    Public Shared Sub FillUpSessionForYachtCategoryBrand(ByVal PageOrigin As String, ByVal sTypeYachtBaseName As String, ByVal ViewTMMDropDowns As crmWebClient.yachtCatBrandModel)

        ' because these values are needed on this page they need to match the control names in the control
        ' so the request header picks up the right values
        If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Category")) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Category").ToString) Then
                If Not HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Category").ToString.ToLower.Contains("all") Then
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Category") = HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Category").ToString.Trim
                Else
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Category") = ""
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Brand") = ""
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = ""
                End If
            End If
        End If

        If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Brand")) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Brand").ToString) Then
                If Not HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Brand").ToString.ToLower.Contains("all") Then
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Brand") = HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Brand").ToString.Trim
                    Dim sCategory As String = ""
                    Dim CategoryHoldString As String = ""
                    Dim sBrand As String = ""
                    Dim BrandArray As Array = Split(HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Brand"), ",")
                    For MultipleBrandCount = 0 To UBound(BrandArray)
                        Dim TempBrandHold As String = CStr(BrandArray(MultipleBrandCount))
                        Dim ModelIDHold As Long = 0

                        ModelIDHold = commonEvo.ReturnYachtModelIDForItemIndex(CLng(TempBrandHold))
                        If commonEvo.ReturnYachtModelDataFromIndex(CLng(TempBrandHold), "", sCategory, sBrand, "") Then
                            If CategoryHoldString <> "" Then
                                CategoryHoldString += ", "
                            End If
                            CategoryHoldString += "" & commonEvo.FindYachtIndexForFirstItem(sCategory, crmWebClient.Constants.LOCYACHT_CATEGORY, sBrand, crmWebClient.Constants.LOCYACHT_BRAND) & ""
                        End If
                    Next

                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Category") = CategoryHoldString
                Else
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Brand") = ""
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = ""
                End If
            End If
        End If

        If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Model")) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Model").ToString) Then
                If Not HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Model").ToString.ToLower.Contains("all") Then
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Model").ToString.Trim
                Else
                    HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = ""
                End If
            End If
        End If

    End Sub

    Public Shared Function BuildNote(ByVal ID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal typeOfNote As String) As String
        Dim ResultsTable As New DataTable
        Dim ReturnString As String = ""
        Dim Yacht As Boolean = False
        Dim aircraft As Boolean = False
        Dim company As Boolean = False

        If typeOfNote = "YACHT" Then
            Yacht = True
        ElseIf typeOfNote = "AC" Then
            aircraft = True
        Else
            company = True
        End If

        Dim tmpNote As String = ""

        Try

            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then

                ResultsTable = commonEvo.getViewCustomerNotesDataTable(ID, 1)

                If Not IsNothing(ResultsTable) Then
                    If ResultsTable.Rows.Count > 0 Then

                        If ResultsTable.Rows(0).Item("journ_description").ToString.Length > 250 Then
                            tmpNote = ResultsTable.Rows(0).Item("journ_description").ToString.Substring(0, 250).Trim
                        Else
                            tmpNote = ResultsTable.Rows(0).Item("journ_description").ToString.Trim
                        End If

                        ReturnString = "<img src=""images/document.png"" class=""float_left"" height=""20"" alt='" + IIf(Not IsDBNull(ResultsTable.Rows(0).Item("journ_date")), Format(CDate(ResultsTable.Rows(0).Item("journ_date")), "MM/dd/yyyy") + " - ", "") + tmpNote.Trim + "' title='" + IIf(Not IsDBNull(ResultsTable.Rows(0).Item("journ_date")), Format(CDate(ResultsTable.Rows(0).Item("journ_date")), "MM/dd/yyyy") + " - ", "") + tmpNote.Trim + "'/>"
                    End If
                Else
                    Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " BuildNote()Admin: " & Replace(aclsData_Temp.class_error, "'", "''"), Nothing, 0, 0, 0, 0, 0)
                End If

            Else

                If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                    If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag = True Then
                        ResultsTable = aclsData_Temp.DUAL_Notes_LIMIT(typeOfNote, ID, "A", "JETNET", Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()), "lnote_entry_date desc", 1)
                        If Not IsNothing(ResultsTable) Then
                            If ResultsTable.Rows.Count > 0 Then
                                ReturnString = "<img src=""images/document.png"" alt=""Note"" class=""float_left"" height=""20"" alt='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "'/>"
                            End If
                        Else
                            Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " BuildNote()Ser: " & Replace(aclsData_Temp.class_error, "'", "''"), Nothing, 0, 0, 0, 0, 0)
                        End If
                    End If
                ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                    If typeOfNote = "AC" Or typeOfNote = "YACHT" Then
                        ResultsTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(ID, "A", aircraft, company, Yacht, True)
                        If Not IsNothing(ResultsTable) Then
                            If ResultsTable.Rows.Count > 0 Then
                                ReturnString = "<img src=""images/document.png"" alt=""Note"" class=""float_left"" height=""20"" alt='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "'/>"
                            End If
                        Else
                            Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " BuildNote()Cl: " & Replace(aclsData_Temp.class_error, "'", "''"), Nothing, 0, 0, 0, 0, 0)
                        End If
                    End If
                End If

            End If




        Catch ex As Exception
            Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " BuildNote(): " & Replace(aclsData_Temp.class_error & "-" & ex.Message, "'", "''"), Nothing, 0, 0, 0, 0, 0)
            Return ""
        End Try
        Return ReturnString
    End Function

    Public Shared Function BuildNote_Action(ByVal ID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal typeOfNote As String) As String
        Dim ResultsTable As New DataTable
        Dim ReturnString As String = ""
        Dim Yacht As Boolean = False
        Dim aircraft As Boolean = False
        Dim company As Boolean = False
        If typeOfNote = "YACHT" Then
            Yacht = True
        ElseIf typeOfNote = "AC" Then
            aircraft = True
        Else
            company = True
        End If
        If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag Then 'make sure the display is correct on the listing page
            'If HttpContext.Current.Session.Item("jetnetAppVersion") <> crmWebClient.Constants.ApplicationVariable.YACHT Then
            If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag = True Then
                    ResultsTable = aclsData_Temp.DUAL_Notes_LIMIT(typeOfNote, ID, "P", "JETNET", Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()), "lnote_entry_date desc", 1)
                    If Not IsNothing(ResultsTable) Then
                        If ResultsTable.Rows.Count > 0 Then
                            ReturnString = "<img src=""images/red_pin.png"" alt=""Note"" class=""float_left"" height=""20"" alt='" & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & ResultsTable.Rows(0).Item("lnote_note") & "'/>"
                        End If
                    End If
                End If
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                If typeOfNote = "AC" Or typeOfNote = "YACHT" Then
                    ResultsTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(ID, "P", aircraft, company, Yacht, True)
                    If Not IsNothing(ResultsTable) Then
                        If ResultsTable.Rows.Count > 0 Then
                            ReturnString = "<img src=""images/red_pin.png"" alt=""Note"" class=""float_left"" height=""20"" alt='" & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & ResultsTable.Rows(0).Item("lnote_note") & "'/>"
                        End If
                    End If
                End If
            End If
            'End If
        End If
        Return ReturnString
    End Function

    Public Shared Function BuildNote_ProspectView(ByVal companyID As Long, ByVal aircraftID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal typeOfNote As String, ByVal CRM_CONN_STRING As String, ByVal status As String) As String
        Dim ResultsTable As New DataTable
        Dim ReturnString As String = ""
        Dim Yacht As Boolean = False
        Dim aircraft As Boolean = False
        Dim company As Boolean = False
        Dim ImageURL As String = ""

        If typeOfNote = "YACHT" Then
            Yacht = True
        ElseIf typeOfNote = "AC" Then
            aircraft = True
        ElseIf Trim(typeOfNote) = "COMP_AC" Then
            aircraft = True
            company = True
        Else
            company = True
        End If

        If Trim(CRM_CONN_STRING) <> "" Then
            CRM_CONN_STRING = CRM_CONN_STRING
        Else
            CRM_CONN_STRING = CRM_CONN_STRING
        End If

        If status = "B" Then
            ImageURL = "images/gold_prospect_icon.png"
        ElseIf status = "A" Then
            ImageURL = "images/document.png"
        ElseIf status = "P" Then
            ImageURL = "images/red_pin.png"
        End If

        If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag Then 'make sure the display is correct on the listing page
            'If HttpContext.Current.Session.Item("jetnetAppVersion") <> crmWebClient.Constants.ApplicationVariable.YACHT Then
            If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag = True Then
                    ResultsTable = aclsData_Temp.DUAL_Notes_LIMIT_CRM(typeOfNote, companyID, aircraftID, status, "JETNET", Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()), "lnote_entry_date desc", 1, CRM_CONN_STRING)
                    If Not IsNothing(ResultsTable) Then
                        If ResultsTable.Rows.Count > 0 Then
                            ReturnString = "<img src=""" & ImageURL & """ alt=""Note"" class=""float_left"" height=""20"" alt='" & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & ResultsTable.Rows(0).Item("lnote_note") & "'/>"
                        End If
                    End If
                End If
                'I don't really think this part is needed, but I commented it just in case.
                'I don't think that someone logged into the CRM would have clouds notes. 
                'ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                '    If typeOfNote = "AC" Or typeOfNote = "YACHT" Then
                '        ResultsTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(companyID, "B", aircraft, company, Yacht, True)
                '        If Not IsNothing(ResultsTable) Then
                '            If ResultsTable.Rows.Count > 0 Then
                '                ReturnString = "<img src=""images/gold_prospect_icon.png"" alt=""Note"" class=""float_left"" height=""20"" alt='" & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & ResultsTable.Rows(0).Item("lnote_note") & "'/>"
                '            End If
                '        End If
                '    End If
            End If
            'End If
        End If
        Return ReturnString
    End Function

    Public Shared Sub AddEditFolderListOptionToFolderDropdown(ByRef folders_submenu_dropdown As BulletedList, ByVal TypeOfFolder As Integer)
        folders_submenu_dropdown.Items.Add(New ListItem("Edit Folder List", "javascript:load('FolderMaintenance.aspx?t=" & TypeOfFolder & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"))
    End Sub

    Public Shared Function LinkOutEventsCompanies(ByVal Description As Object, ByVal CompID As Long, ByVal ContactID As Long, ByVal Master As Object) As String
        Dim retStr As String = ""
        Try
            Dim tempTable As New DataTable
            Dim compName As String = ""
            Dim compLink As String = ""

            'This is going to look for the company ID, then perform a search on the company Name.
            If CompID > 0 Then

                tempTable = Master.aclsData_Temp.GetCompanyInfo_ID(CompID, "JETNET", 0)

                If Not IsNothing(tempTable) Then
                    If tempTable.Rows.Count > 0 Then
                        If Not IsDBNull(tempTable.Rows(0).Item("comp_name")) Then
                            compName = tempTable.Rows(0).Item("comp_name")
                        End If
                    End If
                End If

                If Not IsNothing(Description) Then

                    If Not IsDBNull(Description) Then

                        If Not String.IsNullOrEmpty(compName.Trim) Then
                            compLink = crmWebClient.DisplayFunctions.WriteDetailsLink(0, CompID, 0, 0, True, compName, "", "")
                        End If

                        retStr = "<span class=""tiny"">[" + Replace(Description, compName, compLink) + "]</span>"

                    End If

                End If

            End If

        Catch ex As Exception

            commonLogFunctions.Log_User_Event_Data("UserError", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + "): " + ex.Message.Trim

        End Try

        Return retStr
    End Function

    Public Shared Function BuildYachtInformationTab(ByRef YachtID As Long, ByRef stats_tab As AjaxControlToolkit.TabPanel, ByRef AclsData_Temp As clsData_Manager_SQL, ByRef yachtModelID As Label, ByRef status_tab As AjaxControlToolkit.TabPanel, ByRef usage_tab_container As AjaxControlToolkit.TabContainer, ByRef interior_redone As String, ByRef exterior_redone As String, ByRef engine_info As String, ByRef helipad_label As Label, ByVal helipad_string As String, ByRef status_tab_container As AjaxControlToolkit.TabContainer, ByRef description_label As Label, ByRef description_container As AjaxControlToolkit.TabContainer, ByRef aircraft_status_label As Label, ByRef features_label As Label, ByRef performance_label As Label, ByRef News_Label As Label, ByRef news_container As AjaxControlToolkit.TabContainer, ByVal isNoteView As Boolean) As String
        Dim DisplayTable As New DataTable
        BuildYachtInformationTab = "" 'default to display nothing.
        DisplayTable = AclsData_Temp.DisplayYachtSpecificationsByID(YachtID)

        Dim Build_Column_Number As Integer = 1
        Dim htmlOut As StringBuilder = New StringBuilder()

        Dim market_change As String = ""
        Dim temp_news As String = ""
        Dim news_link As String = ""
        Dim make_left As Integer = 0
        Dim Model_Info As String = ""

        If Not IsNothing(DisplayTable) Then
            'If this isn't present on the page, you should just be able to pass nothing to it, then it won't be filled up.
            If isNoteView Then
                If Not IsNothing(yachtModelID) Then
                    yachtModelID.Text = IIf(Not IsDBNull(DisplayTable.Rows(0).Item("ym_model_id")), DisplayTable.Rows(0).Item("ym_model_id"), 0)
                End If
            End If

            ' -----------------------------------------------------------------------------------------------------------------
            '''''''''''''''''''''''''''''''''''''''''FUNCTION TO SET UP Information Label''''''''''''''''''''''''''''''''''''''
            htmlOut.Append(Build_Information_Label_For_Yacht_Information_Tab(Build_Column_Number, stats_tab, DisplayTable))


            If Not isNoteView Then
                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''FUNCTION TO SET UP FEATURES LABEL''''''''''''''''''''''''''''''''''''''
                Build_Features_Label_For_Yacht_Information_Tab(features_label, DisplayTable, Build_Column_Number)

                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''FUNCTION TO SET UP STATUS LABEL''''''''''''''''''''''''''''''''''''''
                Build_Status_Label_For_Yacht_Information_Tab(Build_Column_Number, status_tab_container, usage_tab_container, status_tab, aircraft_status_label, market_change, DisplayTable)

                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''SPOT TO SET UP ENGINE'''''''''''''''''''''''''''''''''''''''''''''''''
                Build_Column_Number = 1
                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_nbr_engines")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_nbr_engines").ToString) Then
                    engine_info = engine_info & build_column_string(FormatNumber(DisplayTable.Rows(0).Item("yt_nbr_engines"), 0), "Number of Engines", 1, 0)
                End If

                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_engine_fuel_type")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_engine_fuel_type").ToString) Then
                    engine_info = engine_info & build_column_string(DisplayTable.Rows(0).Item("yt_engine_fuel_type"), "Fuel Type", 2, 0)
                End If

                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_engine_fuel_capacity_gals")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_engine_fuel_capacity_gals").ToString) Then
                    engine_info = engine_info & build_column_string(FormatNumber(DisplayTable.Rows(0).Item("yt_engine_fuel_capacity_gals"), 0), "Fuel Capacity/Gal", 1, 0)
                End If

                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_engine_emp")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_engine_emp").ToString) Then
                    If Trim(DisplayTable.Rows(0).Item("yt_engine_emp")) <> "" Then
                        engine_info = engine_info & build_column_string(DisplayTable.Rows(0).Item("yt_engine_emp"), "EMP", 2, 0)
                    End If
                End If

                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''FUNCTION TO SET UP INTERIOR STRING''''''''''''''''''''''''''''''''''''''

                Build_Column_Number = 1
                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_interior_redone_date")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_interior_redone_date").ToString) Then
                    interior_redone = interior_redone & build_column_string(DisplayTable.Rows(0).Item("yt_interior_redone_date").ToString, "Interior Redone Date", Build_Column_Number, 0)
                    interior_redone = interior_redone & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
                    interior_redone = interior_redone & "<td valign='top' align='left' width='50%'>&nbsp;"
                    interior_redone = interior_redone & "</td>"
                    interior_redone = interior_redone & "</tr>"
                End If

                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''FUNCTION TO SET UP EXTERIOR STRING''''''''''''''''''''''''''''''''''''''

                Build_Column_Number = 1
                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_exterior_redone_date")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_exterior_redone_date").ToString) Then
                    exterior_redone = exterior_redone & build_column_string(DisplayTable.Rows(0).Item("yt_exterior_redone_date").ToString, "Exterior Redone Date", Build_Column_Number, 0)
                    exterior_redone = exterior_redone & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
                    exterior_redone = exterior_redone & "<td valign='top' align='left' width='50%'>&nbsp;"
                    exterior_redone = exterior_redone & "</td>"
                    exterior_redone = exterior_redone & "</tr>"
                End If

                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''FUNCTION TO SET UP PERFORMANCE LABEL''''''''''''''''''''''''''''''''''''''

                Build_Performance_Label_For_Yacht_Information_Tab(DisplayTable, performance_label, Build_Column_Number)

                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''FUNCTION TO SET UP HELIPAD '''''''''''''''''''''''''''''''''''''''''

                Build_Helipad_String_For_Yacht_Information_Tab(DisplayTable, helipad_string, helipad_label)

                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''''''''SET UP COMMON NOTES'''''''''''''''''''''''''''''''''''''''''''''''''
                If Not IsNothing(description_container) Then
                    If Not IsDBNull(DisplayTable.Rows(0).Item("yt_common_notes")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_common_notes").ToString) Then
                        If Not IsNothing(description_label) Then
                            description_label.Text = "<table cellpadding='5' cellspacing='0' width='100%'><tr><td width='100%'>" & Replace(DisplayTable.Rows(0).Item("yt_common_notes").ToString, vbCrLf, "<br>") & "</td></tr></table>"
                        End If
                        description_container.Visible = True
                    Else
                        description_container.Visible = False
                    End If
                End If
                ' -----------------------------------------------------------------------------------------------------------------
                '''''''''''''''''''''''''''''''''''''''''''''SET UP NEWS CONTAINER'''''''''''''''''''''''''''''''''''''''''''''''''
                Build_Yacht_News_For_Information_Tab(AclsData_Temp, YachtID, News_Label, news_container)

            End If


            ' -----------------------------------------------------------------------------------------------------------------
            '''''''''''''''''''''''''''''''''''''''''FUNCTION TO SET UP MODEL STRING''''''''''''''''''''''''''''''''''''''
            Build_Column_Number = 1
            If Not IsDBNull(DisplayTable.Rows(0).Item("ym_motor_type")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ym_motor_type").ToString) And Not IsDBNull(DisplayTable.Rows(0).Item("ym_submotor_type")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ym_submotor_type").ToString) Then
                Model_Info = Model_Info & (build_column_string(DisplayTable.Rows(0).Item("ymt_description").ToString & "/" & DisplayTable.Rows(0).Item("ym_submotor_type").ToString, "Type", Build_Column_Number, 0))
            End If

            '    If Not IsDBNull(DisplayTable.Rows(0).Item("ym_submotor_type")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ym_submotor_type").ToString) Then
            '        model_info = model_info & (build_column_string(DisplayTable.Rows(0).Item("ym_submotor_type").ToString, "Sub Type", build_column_number, 0))
            '     End If

            If Not IsDBNull(DisplayTable.Rows(0).Item("ym_category_size")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ym_category_size").ToString) Then
                Model_Info = Model_Info & (build_column_string(DisplayTable.Rows(0).Item("ycs_description").ToString, "Size", Build_Column_Number, 0))
            End If

            If Not IsDBNull(DisplayTable.Rows(0).Item("ym_hull_configuration")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ym_hull_configuration").ToString) Then
                Model_Info = Model_Info & (build_column_string(DisplayTable.Rows(0).Item("ym_hull_configuration").ToString, "Hull Config", Build_Column_Number, 0))
            End If

            If Not IsDBNull(DisplayTable.Rows(0).Item("ym_mfr_comp_id")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ym_mfr_comp_id").ToString) Then
                Dim TemporaryCompanyTable As New DataTable
                Dim TemporaryCompanyName As String = ""
                TemporaryCompanyTable = AclsData_Temp.GetLimited_CompanyInfo_ID(DisplayTable.Rows(0).Item("ym_mfr_comp_id"), "JETNET", 0)
                If Not IsNothing(TemporaryCompanyTable) Then
                    If TemporaryCompanyTable.Rows.Count > 0 Then
                        TemporaryCompanyName = TemporaryCompanyTable.Rows(0).Item("comp_name").ToString
                    End If
                End If
                TemporaryCompanyTable.Dispose()
                Model_Info = Model_Info & (build_column_string(TemporaryCompanyName, "Manufacturer", Build_Column_Number, 0))
            End If


            BuildYachtInformationTab = "<table width='100%' cellpadding='0' cellspacing='0'>"
            BuildYachtInformationTab += htmlOut.ToString
            BuildYachtInformationTab += ("<tr><td colspan='6'><hr style='margin-top:5px;margin-bottom:5px'></td></tr>")
            BuildYachtInformationTab += Model_Info.ToString()
            BuildYachtInformationTab += "</table>"






        End If

        Return BuildYachtInformationTab

    End Function

    Private Shared Function Build_Information_Label_For_Yacht_Information_Tab(ByRef Build_Column_Number As Integer, ByVal stats_tab As AjaxControlToolkit.TabPanel, ByRef DisplayTable As DataTable) As String
        Build_Information_Label_For_Yacht_Information_Tab = ""


        If Not IsNothing(stats_tab) Then
            stats_tab.HeaderText = "<b>"
            ' -----------------------------------------------------------------------------------------------------------------
            If Not IsDBNull(DisplayTable.Rows(0).Item("ym_brand_name")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ym_brand_name").ToString) Then
                stats_tab.HeaderText = stats_tab.HeaderText & DisplayTable.Rows(0).Item("ym_brand_name") & " "
            End If
            If Not IsDBNull(DisplayTable.Rows(0).Item("ym_model_name")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ym_model_name").ToString) Then
                stats_tab.HeaderText = stats_tab.HeaderText & DisplayTable.Rows(0).Item("ym_model_name") & " "
            End If
            stats_tab.HeaderText = stats_tab.HeaderText & """" & "<i>" & DisplayTable.Rows(0).Item("yt_yacht_name") & "</i>" & """" & " "
            stats_tab.HeaderText = stats_tab.HeaderText & "</b>"
            stats_tab.HeaderText = UCase(stats_tab.HeaderText)
        End If

        Build_Column_Number = 1
        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_year_mfr")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_year_mfr").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += build_column_string(DisplayTable.Rows(0).Item("yt_year_mfr").ToString, "Year Mfr.", Build_Column_Number, 0)
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_launch_date")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_launch_date").ToString) Then
            If Year(DisplayTable.Rows(0).Item("yt_launch_date")) <> 1900 Then
                Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(Year(DisplayTable.Rows(0).Item("yt_launch_date")).ToString, "Year Dlv.", Build_Column_Number, 0))
            End If
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_hull_mfr_nbr")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_hull_mfr_nbr").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("yt_hull_mfr_nbr").ToString, "Hull #", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_imo_nbr")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_imo_nbr").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("yt_imo_nbr").ToString, "IMO", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_hull_id_nbr")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_hull_id_nbr").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("yt_hull_id_nbr").ToString, "Hull ID Number", Build_Column_Number, 0))
        End If


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_official_nbr")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_official_nbr").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("yt_official_nbr").ToString, "Official Nbr", Build_Column_Number, 0))
        End If


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_mmsi_mobile_nbr")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_mmsi_mobile_nbr").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("yt_mmsi_mobile_nbr").ToString, "MMSI", Build_Column_Number, 0))
        End If


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_radio_call_sign")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_radio_call_sign").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("yt_radio_call_sign").ToString, "Call Sign", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("ycst_society_name")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ycst_society_name").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("ycst_society_name").ToString, "Class", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_registered_country_flag")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_registered_country_flag").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("yt_registered_country_flag").ToString, "Flag", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("ly_port")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ly_port").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("ly_port").ToString, "Lying Port", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("home_port")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("home_port").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("home_port").ToString, "Home Port", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("reg_port")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("reg_port").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string(DisplayTable.Rows(0).Item("reg_port").ToString, "Reg Port", Build_Column_Number, 0))
        Else
            Build_Information_Label_For_Yacht_Information_Tab += (build_column_string("Unknown", "Reg Port", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_year_refitted")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_year_refitted").ToString) Then
            Build_Information_Label_For_Yacht_Information_Tab += build_column_string(DisplayTable.Rows(0).Item("yt_year_refitted").ToString, "Year Refitted", Build_Column_Number, 0)
        End If


    End Function

    Private Shared Sub Build_Performance_Label_For_Yacht_Information_Tab(ByRef DisplayTable As DataTable, ByRef performance_label As Label, ByRef Build_Column_Number As Integer)
        Dim PerformanceOutput As StringBuilder = New StringBuilder()
        Dim Make_Left As Integer = 0
        PerformanceOutput.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        Build_Column_Number = 1

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_cruise_speed_knots")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_cruise_speed_knots").ToString) Then
            PerformanceOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_cruise_speed_knots").ToString, "Cruise Speed KN", Build_Column_Number, 0))
            If Trim(DisplayTable.Rows(0).Item("yt_cruise_speed_knots")) <> 0.0 Then
                Make_Left = Make_Left + 1
            End If
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_max_speed_knots")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_max_speed_knots").ToString) Then
            PerformanceOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_max_speed_knots").ToString, "Max Speed KN", Build_Column_Number, 0))
            If Trim(DisplayTable.Rows(0).Item("yt_max_speed_knots")) <> 0.0 Then
                Make_Left = Make_Left + 1
            End If
        End If


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_range_miles")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_range_miles").ToString) Then
            PerformanceOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_range_miles").ToString, "Range(NM)", Build_Column_Number, 0))
            If Trim(DisplayTable.Rows(0).Item("yt_range_miles")) <> 0.0 Then
                Make_Left = Make_Left + 1
            End If
        End If

        If Make_Left = 1 Then
            PerformanceOutput.Append("<td>&nbsp;</td><td>&nbsp;</td></tr>")
        End If
        PerformanceOutput.Append("</table>")

        performance_label.Text = PerformanceOutput.ToString
    End Sub

    Private Shared Sub Build_Status_Label_For_Yacht_Information_Tab(ByRef Build_Column_Number As Integer, ByRef status_Tab_container As AjaxControlToolkit.TabContainer, ByRef usage_tab_container As AjaxControlToolkit.TabContainer, ByRef status_tab As AjaxControlToolkit.TabPanel, ByVal aircraft_status_Label As Label, ByRef market_change As String, ByRef DisplayTable As DataTable)
        Dim StatusOutput As StringBuilder = New StringBuilder()
        Build_Column_Number = 1
        If Not IsNothing(status_Tab_container) And Not IsNothing(usage_tab_container) Then
            If DisplayTable.Rows(0).Item("yt_forsale_flag").ToString.ToUpper = "Y" Or DisplayTable.Rows(0).Item("yt_for_lease_flag") = "Y" Or DisplayTable.Rows(0).Item("yt_for_charter_flag") = "Y" Then
                status_Tab_container.CssClass = "green-theme"
                usage_tab_container.CssClass = "dark-theme"
            Else
                status_Tab_container.CssClass = "dark-theme"
                usage_tab_container.CssClass = "dark-theme"
            End If
        End If

        StatusOutput.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_forsale_status")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_forsale_status").ToString) Then
            'htmlOut2.Append(build_column_string(DisplayTable.Rows(0).Item("yt_forsale_status").ToString, "Status", build_column_number))
            If Trim(DisplayTable.Rows(0).Item("yt_forsale_flag")) = "Y" And Trim(DisplayTable.Rows(0).Item("yt_for_lease_flag")) = "Y" And Trim(DisplayTable.Rows(0).Item("yt_for_charter_flag")) = "Y" Then
                market_change = "For Sale/Lease/Charter"
            ElseIf Trim(DisplayTable.Rows(0).Item("yt_forsale_flag")) = "Y" And Trim(DisplayTable.Rows(0).Item("yt_for_lease_flag")) = "Y" And Trim(DisplayTable.Rows(0).Item("yt_for_charter_flag")) = "N" Then
                market_change = "For Sale/Lease"
            ElseIf Trim(DisplayTable.Rows(0).Item("yt_forsale_flag")) = "Y" And Trim(DisplayTable.Rows(0).Item("yt_for_lease_flag")) = "N" And Trim(DisplayTable.Rows(0).Item("yt_for_charter_flag")) = "Y" Then
                market_change = "For Sale/Charter"
            ElseIf Trim(DisplayTable.Rows(0).Item("yt_forsale_flag")) = "N" And Trim(DisplayTable.Rows(0).Item("yt_for_lease_flag")) = "Y" And Trim(DisplayTable.Rows(0).Item("yt_for_charter_flag")) = "Y" Then
                market_change = "For Lease/Charter"
            ElseIf Trim(DisplayTable.Rows(0).Item("yt_forsale_flag")) = "Y" Then
                market_change = "For Sale"
            ElseIf Trim(DisplayTable.Rows(0).Item("yt_for_lease_flag")) = "Y" Then
                market_change = "For Lease"
            ElseIf Trim(DisplayTable.Rows(0).Item("yt_for_charter_flag")) = "Y" Then
                market_change = "For Charter"
            End If
            If Not IsNothing(status_tab) Then
                status_tab.HeaderText = "STATUS: " & UCase(DisplayTable.Rows(0).Item("yt_forsale_status").ToString) & " " & market_change
            End If
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_forsale_status")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_forsale_status").ToString) Then
            If Trim(DisplayTable.Rows(0).Item("yt_forsale_status")) = "For Sale" Then
                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_forsale_list_date")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_forsale_list_date").ToString) Then
                    If Year(DisplayTable.Rows(0).Item("yt_forsale_list_date")) <> "1900" Then
                        StatusOutput.Append(build_column_string(FormatDateTime(DisplayTable.Rows(0).Item("yt_forsale_list_date").ToString, DateFormat.ShortDate), "Date Listed", Build_Column_Number, 0))
                    End If
                End If
            End If
        End If


        'If Not IsDBNull(DisplayTable.Rows(0).Item("yt_for_lease_flag")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_lease_flag").ToString) Then
        '    If Trim(DisplayTable.Rows(0).Item("yt_for_lease_flag")) = "Y" Then
        '        htmlOut2.Append(build_column_string(yn_to_yes_no(DisplayTable.Rows(0).Item("yt_for_lease_flag").ToString), "Avail For Lease", build_column_number, 0))
        '    End If
        'End If


        'If Not IsDBNull(DisplayTable.Rows(0).Item("yt_for_charter_flag")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_for_charter_flag").ToString) Then
        '    If Trim(DisplayTable.Rows(0).Item("yt_for_charter_flag")) = "Y" Then
        '        htmlOut2.Append(build_column_string(yn_to_yes_no(DisplayTable.Rows(0).Item("yt_for_charter_flag").ToString), "Avail for Charter?", build_column_number, 0))
        '    End If
        'End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_asking_price")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_asking_price").ToString) Then
            StatusOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_asking_price").ToString, "Asking Price", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_foreign_asking_price")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_foreign_asking_price").ToString) Then
            If CDbl(DisplayTable.Rows(0).Item("yt_foreign_asking_price")) <> 0 Then
                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_foreign_currency_name")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_foreign_currency_name").ToString) Then
                    If Trim(DisplayTable.Rows(0).Item("yt_foreign_currency_name").ToString) <> "Dollar" Then
                        StatusOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_foreign_asking_price").ToString & " (" & DisplayTable.Rows(0).Item("yt_foreign_currency_name").ToString & ")", "Foreign Asking Price", Build_Column_Number, 0))
                    Else
                        StatusOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_foreign_asking_price").ToString, "Foreign Asking Price", Build_Column_Number, 0))
                    End If
                Else
                    StatusOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_foreign_asking_price").ToString, "Foreign Asking Price", Build_Column_Number, 0))
                End If
            End If

        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_purchased_date")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_purchased_date").ToString) Then
            StatusOutput.Append(build_column_string(FormatDateTime(DisplayTable.Rows(0).Item("yt_purchased_date").ToString, DateFormat.ShortDate), "Date Purchased", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_lifecycle_id")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_lifecycle_id").ToString) Then
            '  StatusOutput.Append(build_column_string(life_cycle_stage(DisplayTable.Rows(0).Item("yt_lifecycle_id").ToString), "", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_ownership_type")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_ownership_type").ToString) Then
            Select Case UCase(DisplayTable.Rows(0).Item("yt_ownership_type").ToString)
                Case "W"
                    StatusOutput.Append(build_column_string("Wholly Owned", "", Build_Column_Number, 0))
                Case "F"
                    StatusOutput.Append(build_column_string("Fractionally Owned", "", Build_Column_Number, 0))
                Case "S"
                    StatusOutput.Append(build_column_string("Shared Ownership", "", Build_Column_Number, 0))
            End Select

        End If

        'If Not IsDBNull(DisplayTable.Rows(0).Item("yt_vat_status")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_vat_status").ToString) Then
        '    htmlOut2.Append(build_column_string(DisplayTable.Rows(0).Item("yt_vat_status").ToString, "VAT", Build_Column_Number, 0))
        '    If Trim(DisplayTable.Rows(0).Item("yt_vat_status").ToString) = "Paid" Then
        '        ' If Not IsDBNull(DisplayTable.Rows(0).Item("yt_vat_amount_paid")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_vat_amount_paid").ToString) Then
        '        'htmlOut2.Append(build_column_string(DisplayTable.Rows(0).Item("yt_vat_amount_paid").ToString, "VAT Amount Paid", build_column_number))
        '        'End If
        '    End If
        'End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_central_agent_flag")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_central_agent_flag").ToString) Then
            If Trim(DisplayTable.Rows(0).Item("yt_central_agent_flag")) = "Y" Then
                StatusOutput.Append(build_column_string(ConvertYesToNo(DisplayTable.Rows(0).Item("yt_central_agent_flag").ToString), "Central Agent?", Build_Column_Number, 0))
            End If
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_not_in_usa_water")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_not_in_usa_water").ToString) Then
            If Trim(DisplayTable.Rows(0).Item("yt_not_in_usa_water").ToString) = "Y" Then
                StatusOutput.Append("<tr>")
                StatusOutput.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                StatusOutput.Append("<td valign='top' align='left' width='100%' colspan='4'><span class='li'>")
                StatusOutput.Append("Not available for sale or charter to US residents while in US waters")
                StatusOutput.Append("</span></td></tr>")
            End If
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_confidential_notes")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_confidential_notes").ToString) Then
            StatusOutput.Append("<tr>")
            StatusOutput.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
            StatusOutput.Append("<td valign='top' align='left' width='100%' colspan='4'><span class='li'>")
            StatusOutput.Append("<span class='label'>Notes: </span>")
            StatusOutput.Append(DisplayTable.Rows(0).Item("yt_confidential_notes").ToString)
            StatusOutput.Append("</span></td></tr>")
        End If

        StatusOutput.Append("</table>")

        aircraft_status_Label.Text = StatusOutput.ToString
    End Sub

    Private Shared Sub Build_Features_Label_For_Yacht_Information_Tab(ByRef features_label As Label, ByRef DisplayTable As DataTable, ByRef Build_Column_Number As Integer)
        Dim FeaturesOutput As StringBuilder = New StringBuilder()
        Build_Column_Number = 1
        FeaturesOutput.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        If (Not IsDBNull(DisplayTable.Rows(0).Item("yt_length_overall_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_length_overall_meters").ToString)) Or (Not IsDBNull(DisplayTable.Rows(0).Item("yt_beam_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_beam_water_line_meters").ToString)) Or (Not IsDBNull(DisplayTable.Rows(0).Item("yt_length_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_length_water_line_meters").ToString)) Or (Not IsDBNull(DisplayTable.Rows(0).Item("yt_draft_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_draft_water_line_meters").ToString)) Then
            FeaturesOutput.Append("<tr><td>&nbsp;</td><td><b>Dimensions (metrics)</b></td><td>&nbsp;</td><td><b>Dimensions (US Standard)</b></td></tr>")
            FeaturesOutput.Append("<tr><td colspan='6'><hr style='margin-top:5px;margin-bottom:5px'></td></tr>")
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_length_overall_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_length_overall_meters").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_length_overall_meters").ToString, "LOA (m)", Build_Column_Number, 2))
            FeaturesOutput.Append(build_column_string(convert_metric_to_us(DisplayTable.Rows(0).Item("yt_length_overall_meters").ToString), "LOA (ft)", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_beam_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_beam_water_line_meters").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_beam_water_line_meters").ToString, "Beam (m)", Build_Column_Number, 2))
            FeaturesOutput.Append(build_column_string(convert_metric_to_us(DisplayTable.Rows(0).Item("yt_beam_water_line_meters").ToString), "Beam (ft)", Build_Column_Number, 0))
        End If


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_length_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_length_water_line_meters").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_length_water_line_meters").ToString, "LWL (m)", Build_Column_Number, 2))
            FeaturesOutput.Append(build_column_string(convert_metric_to_us(DisplayTable.Rows(0).Item("yt_length_water_line_meters").ToString), "LWL (ft)", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_draft_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_draft_water_line_meters").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_draft_water_line_meters").ToString, "Draft (m)", Build_Column_Number, 2))
            FeaturesOutput.Append(build_column_string(convert_metric_to_us(DisplayTable.Rows(0).Item("yt_draft_water_line_meters").ToString), "Draft (ft)", Build_Column_Number, 0))
        End If

        If (Not IsDBNull(DisplayTable.Rows(0).Item("yt_length_overall_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_length_overall_meters").ToString)) Or (Not IsDBNull(DisplayTable.Rows(0).Item("yt_beam_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_beam_water_line_meters").ToString)) Or (Not IsDBNull(DisplayTable.Rows(0).Item("yt_length_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_length_water_line_meters").ToString)) Or (Not IsDBNull(DisplayTable.Rows(0).Item("yt_draft_water_line_meters")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_draft_water_line_meters").ToString)) Then
            FeaturesOutput.Append("<tr><td colspan='6'><hr style='margin-top:5px;margin-bottom:5px'></td></tr>")
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_gross_tons")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_gross_tons").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_gross_tons").ToString, "Gross Tons", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_displacement_tons")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_displacement_tons").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_displacement_tons").ToString, "Displacement Tons", Build_Column_Number, 0))
        End If

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_nbr_decks")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_nbr_decks").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_nbr_decks").ToString, "Number Of Decks", Build_Column_Number, 0))
        End If


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_hull_material")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_hull_material").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_hull_material").ToString, "Hull Material", Build_Column_Number, 0))
        End If


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_superstructure_material")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_superstructure_material").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_superstructure_material").ToString, "Superstructure Material", Build_Column_Number, 0))
        End If


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_nbr_staterooms")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_nbr_staterooms").ToString) Then
            FeaturesOutput.Append(build_column_string(DisplayTable.Rows(0).Item("yt_nbr_staterooms").ToString, "Number of Staterooms/Cabins", Build_Column_Number, 0))
        End If

        FeaturesOutput.Append("</table>")
        features_label.Text = FeaturesOutput.ToString
    End Sub

    Private Shared Sub Build_Helipad_String_For_Yacht_Information_Tab(ByRef DisplayTable As DataTable, ByRef Helipad_String As String, ByRef helipad_label As Label)

        Helipad_String = Helipad_String & "<table cellpadding='0' cellspacing='0' width='100%'>"

        Helipad_String = Helipad_String & "<tr>"
        Helipad_String = Helipad_String & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
        Helipad_String = Helipad_String & "<td valign='top' align='left' width='50%'>"

        Helipad_String = Helipad_String & "<span class='li'><span class='label'>Helipad?: </span>"

        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_helipad")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_helipad").ToString) Then
            If Trim(DisplayTable.Rows(0).Item("yt_helipad").ToString) = "N" Then
                Helipad_String = Helipad_String & "Unknown"
            Else
                Helipad_String = Helipad_String & "" & ConvertYesToNo(DisplayTable.Rows(0).Item("yt_helipad").ToString) & ""
            End If
        End If

        Helipad_String = Helipad_String & "</span>"

        Helipad_String = Helipad_String & "</td>"
        Helipad_String = Helipad_String & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
        Helipad_String = Helipad_String & "<td valign='top' align='left' width='50%'>"

        Helipad_String = Helipad_String & "<span class='li'><span class='label'>Helipad Hangar?:</span> "


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_helipad_hangar")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_helipad_hangar").ToString) Then
            If Trim(DisplayTable.Rows(0).Item("yt_helipad_hangar").ToString) = "N" Then
                Helipad_String = Helipad_String & "Unknown"
            Else
                Helipad_String = Helipad_String & "" & ConvertYesToNo(DisplayTable.Rows(0).Item("yt_helipad_hangar").ToString) & ""
            End If
        End If

        Helipad_String = Helipad_String & "</span>"

        Helipad_String = Helipad_String & "</td>"
        Helipad_String = Helipad_String & "</tr>"


        If Not IsDBNull(DisplayTable.Rows(0).Item("yt_helipad")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_helipad").ToString) Then
            If Trim(DisplayTable.Rows(0).Item("yt_helipad")) = "Y" Then

                Helipad_String = Helipad_String & "<tr>"

                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_helipad_approved_for_lbs")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_helipad_approved_for_lbs").ToString) Then
                    If Trim(DisplayTable.Rows(0).Item("yt_helipad_approved_for_lbs")) <> "0" Then
                        Helipad_String = Helipad_String & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
                        Helipad_String = Helipad_String & "<td valign='top' align='left' width='50%'>"

                        Helipad_String = Helipad_String & "<span class='li'><span class='label'>Approved Lbs: </span>"


                        Helipad_String = Helipad_String & "" & DisplayTable.Rows(0).Item("yt_helipad_approved_for_lbs").ToString & ""


                        Helipad_String = Helipad_String & "</span>"

                        Helipad_String = Helipad_String & "</td>"

                    End If
                End If

                If Not IsDBNull(DisplayTable.Rows(0).Item("yt_helipad_radius")) And Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("yt_helipad_radius").ToString) Then
                    If Trim(DisplayTable.Rows(0).Item("yt_helipad_radius")) <> "0" Then
                        Helipad_String = Helipad_String & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
                        Helipad_String = Helipad_String & "<td valign='top' align='left' width='50%'>"

                        Helipad_String = Helipad_String & "<span class='li'><span class='label'>Radius:</span> "



                        Helipad_String = Helipad_String & "" & DisplayTable.Rows(0).Item("yt_helipad_radius").ToString & ""


                        Helipad_String = Helipad_String & "</span>"

                        Helipad_String = Helipad_String & "</td>"
                    End If
                End If

                Helipad_String = Helipad_String & "</tr>"
            End If
        End If

        Helipad_String = Helipad_String & "</table>"

        If Not IsNothing(helipad_label) Then
            helipad_label.Text = Helipad_String.ToString
        End If
    End Sub

    Private Shared Sub Build_Yacht_News_For_Information_Tab(ByVal aclsdata_temp As clsData_Manager_SQL, ByVal yachtID As Long, ByRef News_Label As Label, ByRef News_Container As AjaxControlToolkit.TabContainer)
        Dim TempNewsString As String = ""
        Dim TempNewsLink As String = ""
        If Not IsNothing(News_Label) Then
            If Not IsNothing(News_Container) Then
                Dim YachtNews As DataTable = aclsdata_temp.ListOfYachtNews(yachtID)

                If Not IsNothing(YachtNews) Then
                    If YachtNews.Rows.Count > 0 Then
                        TempNewsString = "<table cellpadding='5' cellspacing='0' width='100%'>"
                        For Each r As DataRow In YachtNews.Rows
                            If Not IsDBNull(r("ytnews_web_address")) Then
                                TempNewsLink = r("ytnews_web_address")
                                If InStr(TempNewsLink, "http://") = 0 And Trim(TempNewsLink) <> "" Then
                                    TempNewsLink = "http://" & TempNewsLink
                                End If
                            End If
                            TempNewsString += "<tr><td>" & r("ytnews_date") & "-<A href='" & TempNewsLink & "' target='_blank'>" & r("ytnews_title") & "</a>:<br> " & Left(r("ytnews_description"), 300) & " [More At <A href='" & TempNewsLink & "' target='_blank'>" & r("ytnewssrc_name") & "</a>] </td></tr>"
                        Next
                        TempNewsString += "</table>"
                        News_Container.Visible = True
                    Else
                        News_Container.Visible = False
                    End If
                Else
                    News_Container.Visible = False
                End If
            End If
            News_Label.Text = TempNewsString
        End If
    End Sub

    Public Shared Function convert_metric_to_us(ByVal metric As Double) As String
        convert_metric_to_us = ""

        Dim english As Double
        Dim feet As Integer
        Dim inches As Integer


        english = (metric * 3.28084)
        feet = Int(english)
        inches = (english - feet) * 12
        inches = FormatNumber(inches, 0)

        convert_metric_to_us = feet & "' " & inches & "' "
    End Function

    Public Shared Function build_column_string(ByVal value, ByVal label, ByRef col_num, ByVal format_too) As String
        build_column_string = ""
        Dim temp_string As String = ""
        Dim temp_string2 As String = ""

        If Trim(value) <> "0" Then


            If col_num = 1 Then
                build_column_string = build_column_string & "<tr>"
            End If

            build_column_string = build_column_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
            build_column_string = build_column_string & "<td valign='top' align='left' width='50%'><span class='li'>"

            If Trim(label) <> "" Then
                If Trim(label) = "Foreign Asking Price" Then
                    build_column_string = build_column_string & "<span class='label'>Asking Price: </span>"
                Else
                    build_column_string = build_column_string & "<span class='label'>" & label & ": </span>"
                End If
            End If



            If Trim(label) = "Asking Price" Then
                build_column_string = build_column_string & "$"
            End If


            If InStr(value, "(") > 0 And Trim(label) = "Foreign Asking Price" Then
                temp_string = Right(value, value.ToString.Length - InStr(value, "(") + 1)
                temp_string2 = Trim(Left(value, InStr(value, "(") - 1))
                build_column_string = build_column_string & FormatNumber(temp_string2, 0) & temp_string
            ElseIf IsNumeric(value) And Trim(label) <> "Year Mfr." And Trim(label) <> "Hull #" And Trim(label) <> "MMSI" And Trim(label) <> "IMO" And Trim(label) <> "Year Dlv." And Trim(label) <> "Official Nbr" Then
                build_column_string = build_column_string & FormatNumber(value, format_too)
            ElseIf Trim(label) = "Size" Then
                If InStr(value, "-") > 0 Then
                    temp_string = Left(value, InStr(value, "-") - 1)
                    build_column_string = build_column_string & temp_string
                Else
                    build_column_string = build_column_string & value
                End If
            Else
                build_column_string = build_column_string & value
            End If


            build_column_string = build_column_string & "</span>"
            build_column_string = build_column_string & "</td>"

            If col_num = 2 Then
                build_column_string = build_column_string & "</tr>"
            End If


            If col_num = 1 Then
                col_num = 2
            Else
                col_num = 1
            End If


        End If

    End Function

    Public Shared Function ConvertYesToNo(ByVal temp_yn As String) As String
        Return IIf(UCase(temp_yn) = "Y", "Yes", "No")
    End Function

    Public Shared Sub load_google_chart_all(ByVal string_from_charts As String, ByRef page1 As Page, ByRef temp_panel As System.Web.UI.UpdatePanel)
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label


        'temp_string = "<script type=""text/javascript"">"

        temp_string &= "google.charts.setOnLoadCallback(function() {"
        temp_string &= "drawCharts();"


        temp_string &= "function drawCharts() {"

        temp_string &= string_from_charts

        temp_string &= " } "
        temp_string &= "}); "
        'temp_string &= "alert(document.getElementById('" & div_name & "'));"
        'temp_string &= "</script>"


        label_script.ID = "label_script"
        label_script.Text = temp_string


        'tab_to_add_to.Controls.AddAt(0, label_script)


        If Not page1.ClientScript.IsClientScriptBlockRegistered("GoogleChart1Tab") Then
            GoogleChart1TabScript.Append(temp_string)

            System.Web.UI.ScriptManager.RegisterStartupScript(temp_panel, page1.GetType(), "GoogleChart1Tab", GoogleChart1TabScript.ToString, True)
        End If

    End Sub

    Public Shared Sub load_google_chart_faa(ByVal array_string As String, ByVal map_title As String, ByVal y_axis_label As String, ByVal div_name As String, ByVal width As Integer, ByVal height As Integer, ByRef page1 As Page, ByRef temp_panel As System.Web.UI.UpdatePanel, Optional ByVal aport_id As Long = 0)
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label
        Dim chart_num As Integer = 1
        Dim line_or_points As String = "POINTS"
        Dim format_x_axis As Boolean = True


        If InStr(Right(Trim(div_name), 3), "all") = 0 Then
            temp_string = "<script type=""text/javascript"">"

            ' Load the Visualization API and the piechart package.
            '  temp_string &= "google.load('visualization', '1.0', {'packages':['corechart']});"
            temp_string &= "google.charts.setOnLoadCallback(function() {"
            ' Set a callback to run when the Google Visualization API is loaded.
            'temp_string &= "google.setOnLoadCallback(drawChart);"
            temp_string &= "drawChart" & chart_num.ToString.Trim & "();"

            ' Callback that creates and populates a data table,
            ' instantiates the pie chart, passes in the data and
            ' draws it.
            temp_string &= "}); "
            temp_string &= "function drawChart" & chart_num.ToString.Trim & "() {"
        End If


        If Trim(line_or_points) = "POINTS" Then
            temp_string &= "var data" & chart_num.ToString.Trim & " = new google.visualization.DataTable();"
        Else
            temp_string &= "var data" & chart_num.ToString.Trim & " = google.visualization.arrayToDataTable(["
        End If

        temp_string &= array_string

        temp_string &= "]);"

        ' Set chart options
        temp_string &= "var options" & chart_num.ToString.Trim & " = {"
        temp_string &= "'title':'" & map_title & "',"
        temp_string &= "'width':" & width & ","
        temp_string &= "curveType:  'function',"
        temp_string &= "'height':" & height & ","
        ' temp_string &= "legend:{}, "         ' , fontName:'xx'
        '  temp_string &= "tooltip:{textStyle:{fontSize:'6'}}, "
        temp_string &= "legend: { position: 'right', textStyle:{fontSize:'10'}},"

        temp_string &= "colors: ['#B7DCF6','#a3c28d', '#a84543'],"
        temp_string &= " 'chartArea': {'width': '70%', 'height': '95%'}, "


        'if its not all, then 5 space, if its all .. a little more space 
        If InStr(Right(Trim(div_name), 3), "all") = 0 Then
            temp_string &= " 'chartArea': {top:25}, "
        Else
            temp_string &= " 'chartArea': {top:25}, "
        End If


        If format_x_axis Then
            temp_string &= "hAxis: { textStyle:{fontSize:'10'}, slantedText:true, slantedTextAngle:70},"   ',
        Else
            temp_string &= "hAxis: { textStyle:{fontSize:'9'}},"   'slantedText:true, slantedTextAngle:70,
        End If



        temp_string &= "vAxis: { title: '" & y_axis_label & "'} "

        '  temp_string &= " seriesType: 'ScatterChart', "
        '  temp_string &= " series: {5: {type: 'line'}} "  
        temp_string &= ", series: { "
        temp_string &= "   0: { lineWidth: 2, pointSize: 3  } "
        temp_string &= ",  1: { lineWidth: 2, pointSize: 3  } "
        If aport_id > 0 Then
            temp_string &= ",  2: { lineWidth: 2, pointSize: 3  } "
        Else
            temp_string &= ",  2: { lineWidth: 0, pointSize: 5  } "
        End If


        ' temp_string &= ",  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
        'temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
        'temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
        temp_string &= " } "




        temp_string &= "};"



        ' Instantiate and draw our chart, passing in some options.

        temp_string &= "var chart" & chart_num.ToString.Trim & " = new google.visualization.LineChart(document.getElementById('" & div_name & "'));"
        temp_string &= "chart" & chart_num.ToString.Trim & ".draw(data" & chart_num.ToString.Trim & ", options" & chart_num.ToString.Trim & ");"






        If InStr(Right(Trim(div_name), 3), "all") = 0 Then
            'temp_string &= "alert(document.getElementById('" & div_name & "'));"
            temp_string &= "}"
            temp_string &= "</script>"
        End If




        'tab_to_add_to.Controls.AddAt(0, label_script)

        '  If InStr(Right(Trim(div_name), 3), "all") > 0 Then
        'string_to_return = string_to_return & temp_string
        '   Else
        label_script.ID = "label_script"
        label_script.Text = temp_string

        If Not page1.ClientScript.IsClientScriptBlockRegistered("GoogleChart1Tab") Then
            GoogleChart1TabScript.Append(temp_string)

            System.Web.UI.ScriptManager.RegisterStartupScript(temp_panel, page1.GetType(), "GoogleChart1Tab", GoogleChart1TabScript.ToString, False)
        End If
        ' End If
    End Sub

    Public Shared Sub load_google_chart(ByVal tab_to_add_to As AjaxControlToolkit.TabPanel, ByVal array_string As String, ByVal map_title As String, ByVal y_axis_label As String, ByVal div_name As String, ByVal width As Integer, ByVal height As Integer, ByVal line_or_points As String, ByVal chart_num As Integer, ByRef string_to_return As String, ByRef page1 As Page, ByRef temp_panel As System.Web.UI.UpdatePanel, ByVal format_x_axis As Boolean, ByVal show_only_asking As Boolean, Optional ByVal draw_line As Boolean = False, Optional ByVal show_only_sold As Boolean = False, Optional ByVal make_into_bar As Boolean = False, Optional ByVal hide_legend As Boolean = False, Optional ByVal show_my_asking_dashed As Boolean = False, Optional ByVal show_only_take As Boolean = False, Optional ByVal is_large_graph As Boolean = False, Optional ByVal make_into_pie As Boolean = False, Optional ByVal active_tab As Integer = 0, Optional ByVal align_legend As String = "", Optional ByVal client_id As String = "", Optional ByVal RemoveVisibleMarginsAndShrinkWhitespace As Boolean = False, Optional ByVal number_of_records As Integer = 0, Optional ByVal ticks_string As String = "", Optional ByVal series_string As String = "")
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label
        Dim line_width As Integer = 0
        Dim value_color As String = "#078fd7"
        Dim grey_color As String = "#B7B7B7"


        If InStr(Right(Trim(div_name), 3), "all") = 0 Then
            temp_string = "<script type=""text/javascript"">"

            ' Load the Visualization API and the piechart package.
            '  temp_string &= "google.load('visualization', '1.0', {'packages':['corechart']});"

            ' Set a callback to run when the Google Visualization API is loaded.
            'temp_string &= "google.setOnLoadCallback(drawChart);"
            temp_string &= "google.charts.setOnLoadCallback(drawChart" & chart_num.ToString.Trim & ");"

            ' Callback that creates and populates a data table,
            ' instantiates the pie chart, passes in the data and
            ' draws it.
            temp_string &= "function drawChart" & chart_num.ToString.Trim & "() {"
        End If

        '' Create the data table.
        'temp_string &= " var data = new google.visualization.DataTable();"
        'temp_string &= " data.addColumn('string', 'Topping');"
        'temp_string &= " data.addColumn('number', 'Slices');"
        'temp_string &= " data.addRows(["
        'temp_string &= "  ['Mushrooms', 4],"
        'temp_string &= "  ['Onions', 1],"
        'temp_string &= "  ['Olives', 1],"
        'temp_string &= "  ['Zucchini', 1],"
        'temp_string &= "  ['Pepperoni', 3]"
        'temp_string &= "]);" 


        If Trim(array_string) <> "" Then


            If Trim(line_or_points) = "POINTS" Then
                temp_string &= "var data" & chart_num.ToString.Trim & " = new google.visualization.DataTable();"
            Else
                temp_string &= "var data" & chart_num.ToString.Trim & " = google.visualization.arrayToDataTable(["
            End If

            If HttpContext.Current.Request.Item("dummy") = "Y" Then
                If HttpContext.Current.Request.Item("title1") <> "" And HttpContext.Current.Request.Item("title2") <> "" And HttpContext.Current.Request.Item("value1") <> "" And HttpContext.Current.Request.Item("value2") <> "" Then
                    temp_string &= "  ['Type', 'Count'], "
                    temp_string &= "  ['" & HttpContext.Current.Request.Item("title1") & "', " & HttpContext.Current.Request.Item("value1") & "],"
                    temp_string &= "  ['" & HttpContext.Current.Request.Item("title2") & "',  " & HttpContext.Current.Request.Item("value2") & "]"
                Else
                    temp_string &= "  ['Type', 'Count'], "
                    temp_string &= "  ['Yachts', 4498],"
                    temp_string &= "  ['Yachts & Aircraft', 405]"


                    'temp_string &= "  ['Country', 'Count'], "
                    'temp_string &= "  ['Australia', 8],"
                    'temp_string &= "  ['Bahamas', 1],"
                    'temp_string &= "  ['Bahrain', 1],"
                    'temp_string &= "  ['Belgium', 2],"
                    'temp_string &= "  ['Canada - (2.9%)', 15],"
                    'temp_string &= "  ['Cayman Islands',	1],"
                    'temp_string &= "  ['China', 2],"
                    'temp_string &= "  ['Cyprus', 1],"
                    'temp_string &= "  ['Denmark', 1],"
                    'temp_string &= "  ['Egypt', 2],"
                    'temp_string &= "  ['France', 7],"
                    'temp_string &= "  ['Germany', 7],"
                    'temp_string &= "  ['Hong Kong',	1],"
                    'temp_string &= "  ['India', 6],"
                    'temp_string &= "  ['Isle of Man',	2],"
                    'temp_string &= "  ['Italy - (2.9%)', 15],"
                    'temp_string &= "  ['Jersey', 1],"
                    'temp_string &= "  ['Malaysia', 2],"
                    'temp_string &= "  ['Malta', 3],"
                    'temp_string &= "  ['Mexico', 3],"
                    'temp_string &= "  ['Monaco', 1],"
                    'temp_string &= "  ['New Zealand - (2.5%)',	13],"
                    'temp_string &= "  ['Oman', 2],"
                    'temp_string &= "  ['Portugal', 1],"
                    'temp_string &= "  ['Puerto Rico',	1],"
                    'temp_string &= "  ['Qatar', 12],"
                    'temp_string &= "  ['Russian Federation',	11],"
                    'temp_string &= "  ['Saudi Arabia',	8],"
                    'temp_string &= "  ['Spain', 9],"
                    'temp_string &= "  ['Switzerland', 3],"
                    'temp_string &= "  ['Turkey', 4],"
                    'temp_string &= "  ['United Arab Emirates',	6],"
                    'temp_string &= "  ['United Kingdom - (3.9%)',	20],"
                    'temp_string &= "  ['United States - (65.6%)',	337],"
                    'temp_string &= "  ['Venezuela', 2],"
                    'temp_string &= "  ['Virgin Islands British', 3]"

                End If
            Else
                temp_string &= array_string
            End If

            temp_string &= "]);"

            ' Set chart options
            temp_string &= "var options" & chart_num.ToString.Trim & " = {"

            If Trim(client_id) <> "" Then
            Else
                temp_string &= "'title':'" & map_title & "',"
            End If

            If HttpContext.Current.Request.Item("dummy") = "Y" Then
                If HttpContext.Current.Request.Item("width") <> "" And HttpContext.Current.Request.Item("height") <> "" Then
                    temp_string &= "'width':" & HttpContext.Current.Request.Item("width") & ","
                    temp_string &= "'height':" & HttpContext.Current.Request.Item("height") & ","
                Else
                    temp_string &= "'width':800,"
                    temp_string &= "'height':800,"
                End If

            Else
                temp_string &= "'width':" & width & ","
                temp_string &= "'height':" & height & ","

            End If

            ' temp_string &= "legend:{}, "         ' , fontName:'xx'
            '  temp_string &= "tooltip:{textStyle:{fontSize:'6'}}, "


            If Trim(client_id) <> "" Then
                If make_into_pie And (Trim(div_name) = "chart_div_tab5_all" Or Trim(div_name) = "chart_div_tab6_all" Or Trim(div_name) = "chart_div_tab7_all" Or Trim(div_name) = "chart_div_tab8_all" Or Trim(div_name) = "chart_div_tab9_all" Or Trim(div_name) = "chart_div_tab10_all" Or Trim(div_name) = "chart_div_tab11_all" Or Trim(div_name) = "chart_div_tab12_all" Or Trim(div_name) = "chart_div_tab13_all" Or Trim(div_name) = "chart_div_tab14_all" Or Trim(div_name) = "chart_div_tab15_all" Or Trim(div_name) = "chart_div_tab16_all") Then
                    temp_string &= " 'chartArea': {'width': '92%', 'height': '72%'}, "
                ElseIf make_into_bar = True And (Trim(div_name) = "chart_div_tab4_all" Or Trim(div_name) = "chart_div_tab13_all" Or Trim(div_name) = "chart_div_tab14_all" Or Trim(div_name) = "chart_div_tab16_all" Or Trim(div_name) = "chart_div_tab19_all") Then   ' added MSW - for utilization report 4/18/19
                    If RemoveVisibleMarginsAndShrinkWhitespace = True Then
                        temp_string &= " 'chartArea': {'width': '85%', 'height': '80%', top:12,left:80}, "
                    Else
                        temp_string &= " 'chartArea': {'width': '85%', 'height': '72%'}, "
                    End If
                    temp_string &= " 'bar': {groupWidth: '82%'}, "
                ElseIf make_into_bar = True And (Trim(div_name) = "chart_div_tab17_all") Then
                    temp_string &= " 'chartArea': {'width': '85%', 'height': '65%', top:12,left:80}, "
                    temp_string &= " 'bar': {groupWidth: '82%'}, "
                ElseIf make_into_bar = True And (Trim(div_name) = "chart_div_tab8_all" Or Trim(div_name) = "chart_div_tab5_all" Or Trim(div_name) = "chart_div_tab18_all") Then  ' make height smaller to make room for text on these 2 
                    temp_string &= " 'chartArea': {'width': '52%', 'height': '90%', top:12,right:20}, "
                    temp_string &= " 'bar': {groupWidth: '82%'}, "
                ElseIf make_into_bar = True And (Trim(div_name) = "chart_div_tab11_all" Or Trim(div_name) = "chart_div_tab7_all") Then  ' make height smaller to make room for text on these 2 
                    temp_string &= " 'chartArea': {'width': '70%', 'height': '90%', top:12,right:20}, "
                    temp_string &= " 'bar': {groupWidth: '82%'}, "
                ElseIf Trim(div_name) = "chart_div_tab20_all" Or Trim(div_name) = "chart_div_tab21_all" Or Trim(div_name) = "chart_div_tab22_all" Or Trim(div_name) = "chart_div_tab23_all" Then
                    temp_string &= " 'chartArea': {'width': '100%', 'height': '100%', left:250, bottom: 50, top: 0, right:0}, "
                    temp_string &= " 'bar': {groupWidth: '38%'}, "
                ElseIf Trim(div_name) = "chart_div_tab15_all" Or Trim(div_name) = "chart_div_tab17_all" Then
                    temp_string &= " 'chartArea': {'width': '85%', 'height': '80%'}, "
                ElseIf Trim(div_name) = "chart_div_tab18_all" Then
                    temp_string &= " 'chartArea': {'width': '85%', 'height': '80%'}, "
                ElseIf Trim(div_name) = "chart_div_tab7_all" And Trim(align_legend) = "bottom" Then   'residual charts 
                    temp_string &= " 'chartArea': {'width': '89%', 'height': '78%'}, "
                ElseIf Trim(div_name) = "chart_div_tab11_all" Or Trim(div_name) = "chart_div_tab7_all" Or Trim(div_name) = "chart_div_tab11_all" Or Trim(div_name) = "chart_div_tab8_all" Then '    
                    temp_string &= " 'chartArea': {'width': '75%', 'height': '78%'}, "
                ElseIf Trim(div_name) = "chart_div_tab13_all" Then
                    temp_string &= " 'chartArea': {'width': '92%', 'height': '63%'}, "
                ElseIf Trim(div_name) = "chart_div_tabVal1_all" Or Trim(div_name) = "chart_div_tabVal2_all" Then
                    temp_string &= " 'chartArea': {'width': '78%', 'height': '74%', top:18}, "
                ElseIf Trim(div_name) = "chart_div_tab_Valuation1_all" Then
                    temp_string &= " 'chartArea': {'width': '84%', 'height': '77%'}, "
                ElseIf Trim(div_name) = "chart_div_tab_Valuation2_all" Then
                    temp_string &= " 'chartArea': {'width': '84%', 'height': '77%'}, "
                ElseIf Trim(div_name) = "chart_div_tab_Valuation3_all" Then
                    temp_string &= " 'chartArea': {'width': '84%', 'height': '77%'}, "
                ElseIf Trim(div_name) = "chart_div_tab_Valuation3_all" Then
                    temp_string &= " 'chartArea': {'width': '84%', 'height': '77%'}, "
                ElseIf Trim(div_name) = "chart_div_tab32_all" Then
                    temp_string &= " 'chartArea': {'width': '84%', 'height': '77%' }, "
                ElseIf Trim(div_name) = "chart_div_tab33_all" Then
                    temp_string &= " 'chartArea': {'width': '82%', 'height': '71%'}, "
                ElseIf Trim(div_name) = "chart_div_tab16_all" Then
                    temp_string &= " 'chartArea': {'width': '82%', 'height': '71%'}, "
                ElseIf Trim(div_name) = "chart_div_tab24_all" Then ' changed for PDF 
                    temp_string &= " 'chartArea': {'width': '80%', 'height': '71%'}, "
                ElseIf RemoveVisibleMarginsAndShrinkWhitespace = True Then   '  i mvoed this down belwo so that it 
                    temp_string &= " 'chartArea': {'width': '" & width - 150 & "', 'height': '" & height - 160 & "'}, "
                Else
                    temp_string &= " 'chartArea': {'width': '80%', 'height': '80%'}, "
                End If

                If hide_legend = True Then
                    temp_string &= "legend: { position: 'none' },"
                ElseIf make_into_pie = True And (Trim(div_name) = "chart_div_tab5_all" Or Trim(div_name) = "chart_div_tab7_all" Or Trim(div_name) = "chart_div_tab8_all" Or Trim(div_name) = "chart_div_tab9_all" Or Trim(div_name) = "chart_div_tab10_all" Or Trim(div_name) = "chart_div_tab11_all") Then
                    temp_string &= "legend: { position: 'right', textStyle:{fontSize:19} , maxLines: 5},"
                ElseIf make_into_pie = True And Trim(div_name) = "chart_div_tab6_all" Then
                    temp_string &= "legend: { position: 'right', textStyle:{fontSize:19} , maxLines: 5},"   ' position labeled tried 
                ElseIf Trim(div_name) = "chart_div_tab7_all" And Trim(align_legend) = "bottom" Then    'residual charts 
                    temp_string &= "legend: { position: 'top',  maxLines: 5, textStyle:{fontSize:12}},"
                ElseIf Trim(div_name) = "chart_div_tab32_all" Then
                    temp_string &= "legend: { position: 'top', textStyle:{fontSize:22}},"
                ElseIf Trim(div_name) = "chart_div_tab5_all" Or Trim(div_name) = "chart_div_tab6_all" Or Trim(div_name) = "chart_div_tab7_all" Or Trim(div_name) = "chart_div_tab8_all" Or Trim(div_name) = "chart_div_tab9_all" Or Trim(div_name) = "chart_div_tab10_all" Or Trim(div_name) = "chart_div_tab11_all" Or Trim(div_name) = "chart_div_tab12_all" Or Trim(div_name) = "chart_div_tab13_all" Or Trim(div_name) = "chart_div_tab14_all" Or Trim(div_name) = "chart_div_tab15_all" Or Trim(div_name) = "chart_div_tab16_all" Then
                    temp_string &= "legend: { position: " & IIf(align_legend <> "", "'" & align_legend & "'", "'right'") & ", textStyle:{fontSize:22}},"
                ElseIf Trim(div_name) = "chart_div_tab_Valuation1_all" Then
                    temp_string &= "legend: { position: 'top', textStyle:{fontSize:22}},"
                ElseIf Trim(div_name) = "chart_div_tab_Valuation2_all" Or Trim(div_name) = "chart_div_tab_Valuation3_all" Or Trim(div_name) = "chart_div_tab_Valuation4_all" Or Trim(div_name) = "chart_div_tab_Valuation5_all" Or Trim(div_name) = "chart_div_tab_Valuation6_all" Then
                    temp_string &= "legend: { position: 'top', textStyle:{fontSize:9} , maxLines: 3},"
                ElseIf Trim(client_id) <> "" And (Trim(div_name) = "chart_div_tabVal1_all" Or Trim(div_name) = "chart_div_tabVal2_all") Then
                    temp_string &= "legend: { position: 'top', textStyle:{fontSize:10}},"
                ElseIf Trim(client_id) <> "" Then '   ElseIf Trim(div_name) = "chart_div_tab18_all" Or Trim(div_name) = "chart_div_tab8_all" Then   '   
                    temp_string &= "legend: { position: 'top', textStyle:{fontSize:22}},"
                End If
            ElseIf (Trim(div_name) = "chart_div_port_tab9_all") Then
                temp_string &= " 'chartArea': {'width': '92%', 'height': '82%'}, "
                temp_string &= "legend: { position: 'none' },"
            ElseIf Trim(div_name) = "chart_div_port_tab2_all" And make_into_pie = False Then
                temp_string &= " 'chartArea':{width:'85%',height:'75%'}, legend: { position: 'none' }, "
            ElseIf (Trim(div_name) = "chart_div_port_tab2_all" Or Trim(div_name) = "chart_div_port_tab3_all") Then
                temp_string &= " 'chartArea':{width:'95%',height:'95%'}, legend: {position: 'right', textStyle:{fontSize:'12'}}, "
            ElseIf ((Trim(div_name) = "chart_div_value_history1_all" Or Trim(div_name) = "chart_div_value_history2_all") And clsGeneral.clsGeneral.isEValuesAvailable() = True) Then
                temp_string &= " 'chartArea': {'width': '72%', 'height': '65%'}, "
                temp_string &= "legend: { position: 'bottom', textStyle:{fontSize:'10'}},"
            ElseIf Trim(div_name) = "chart_div_tab15_all" Or Trim(div_name) = "chart_div_tab16_all" Or Trim(div_name) = "chart_div_tab18_all" Or Trim(div_name) = "chart_div_tab100_all" Then

                If Trim(div_name) = "chart_div_tab15_all" Then
                    temp_string &= " 'chartArea': {'width': '71%', 'height': '59%'}, "
                ElseIf Trim(div_name) = "chart_div_tab16_all" Then
                    temp_string &= " 'chartArea': {'width': '77%', 'height': '59%'}, "
                ElseIf Trim(div_name) = "chart_div_tab18_all" Then
                    temp_string &= " 'chartArea': {'width': '77%', 'height': '59%'}, "
                ElseIf Trim(div_name) = "chart_div_tab100_all" Then
                    temp_string &= " 'chartArea': {'width': '77%', 'height': '59%'}, "
                End If

                If hide_legend = True Then
                    temp_string &= "legend: { position: 'none' },"
                ElseIf Trim(client_id) <> "" Then '   ElseIf Trim(div_name) = "chart_div_tab18_all" Or Trim(div_name) = "chart_div_tab8_all" Then   '   
                    temp_string &= "legend: { position: 'top', textStyle:{fontSize:22}},"
                End If
            ElseIf Trim(map_title) = "Dealer Sales Per Year" And Trim(align_legend) <> "" Then
                temp_string &= " 'chartArea':{width:'55%',height:'78%',top:5,left:46},   "
                temp_string &= "legend: { position: '" & align_legend & "', textStyle:{fontSize:'11'}},"
            ElseIf Trim(align_legend) <> "" Then
                temp_string &= "legend: { position: '" & align_legend & "', textStyle:{fontSize:'11'}},"
            ElseIf HttpContext.Current.Request.Item("dummy") = "Y" Then
                temp_string &= "legend: { position: 'right', textStyle:{fontSize:'11'}},"
                temp_string &= " tooltip: { trigger: 'both' }, "
            ElseIf hide_legend = True Then
                temp_string &= "legend: { position: 'none' },"
            ElseIf Trim(div_name) = "chart_div_value_history" Then
                temp_string &= "legend: { position: 'right', textStyle:{fontSize:'10'}},"
            Else
                temp_string &= "legend: { position: 'right', textStyle:{fontSize:'11'}},"
            End If

            If make_into_pie = True Then
                If active_tab = 1 Then
                    temp_string &= "pieSliceText:  'label', "
                    temp_string &= " 'chartArea': {'width': '95%', 'height': '95%'}, "
                    temp_string &= " is3D: true, "
                ElseIf active_tab = 100 Then
                    temp_string &= " 'chartArea': {'width': '95%', 'height': '95%'}, "
                    temp_string &= " is3D: true, "
                ElseIf Trim(div_name) = "chart_div_tab9_all" Then
                    ' temp_string &= "pieSliceText:  'label', "
                    ' temp_string &= "pieSliceText:  'value-and-percentage', "
                    '  temp_string &= "pieSliceText:  'label', "
                End If
            End If

            If Trim(client_id) <> "" Then

                If Trim(div_name) = "chart_div_tabVal1_all" Then
                    temp_string &= "curveType:  'function',"
                    temp_string &= "  colors: ['" & value_color & "','" & value_color & "', '" & value_color & "', '#B7B7B7', '#B7B7B7', '#B7B7B7', '#a3c28d', '#eba059', '#a84543', '#a3c28d', '#eba059', '#a84543', 'purple'],"
                ElseIf Trim(div_name) = "chart_div_tabVal2_all" Then
                    temp_string &= "curveType:  'function',"
                    temp_string &= ("  colors: ['" & value_color & "','" & grey_color & "'],")
                ElseIf HttpContext.Current.Session.Item("graph_color").ToString.Trim <> "" Then
                    temp_string &= "curveType:  'function',"
                    temp_string &= "colors: ['" & HttpContext.Current.Session.Item("graph_color").ToString.Trim & "', '#a84543', 'green','blue', 'red', 'green'],"
                    'ElseIf Trim(div_name) = "chart_div_tab17_all" Then
                    '  temp_string &= "curveType:  'function',"
                    '  temp_string &= "colors: ['#B7DCF6', '#a84543', 'green','blue', 'red', 'green'],"
                Else

                    If Trim(div_name) = "chart_div_tab_Valuation1_all" Then
                        temp_string &= "curveType:  'function',"
                        temp_string &= "colors: ['#a3c28d','#92b0c4', '#B7B7B7', '#a3c28d', '#a84543'],"
                    ElseIf Trim(div_name) = "chart_div_tab_Valuation2_all" Then
                        temp_string &= "curveType:  'function',"
                        temp_string &= "colors: ['#92b0c4','#92b0c4', '#92b0c4', '#a3c28d', '#a84543'],"
                    ElseIf Trim(div_name) = "chart_div_tab_Valuation3_all" Then
                        temp_string &= "curveType:  'function',"
                        temp_string &= "  colors: ['" & value_color & "','" & value_color & "', '" & value_color & "', '#B7B7B7', '#B7B7B7', '#B7B7B7', '#a3c28d', '#eba059', '#a84543', '#a3c28d', '#eba059', '#a84543', 'purple'],"
                    ElseIf Trim(div_name) = "chart_div_tab_Valuation4_all" Then
                        temp_string &= "curveType:  'function',"
                        temp_string &= "colors: ['#B7DCF6', '#a84543', 'green','blue', 'red', 'green'],"
                    ElseIf Trim(div_name) = "chart_div_tab_Valuation5_all" Then
                        temp_string &= "curveType:  'function',"
                        temp_string &= "colors: ['#a3c28d','#a84543', '#92b0c4', '" & value_color & "', '" & value_color & "', '" & value_color & "', '" & value_color & "', '" & value_color & "', '" & value_color & "', '92b0c4', '#a3c28d', '#eba059', '#a84543'],"
                    Else
                        temp_string &= "curveType:  'function',"
                        temp_string &= "colors: ['#B7DCF6', '#a84543', 'green','blue', 'red', 'green'],"
                    End If

                End If
            ElseIf Trim(div_name) = "utilizationViewGraphall" Then
                temp_string &= "curveType:  'function',"
                temp_string &= "colors: ['#B7DCF6'],"
            ElseIf Trim(div_name) = "chart_div_value_history2_all" Then
                temp_string &= "curveType:  'function',"
                temp_string &= ("  colors: ['" & value_color & "','" & grey_color & "'],")
            ElseIf (Trim(div_name) = "chart_div_value_history1_all" And clsGeneral.clsGeneral.isEValuesAvailable() = True) Then
                temp_string &= "curveType:  'function',"
                temp_string &= "  colors: ['" & value_color & "','" & value_color & "', '" & value_color & "', '#B7B7B7', '#B7B7B7', '#B7B7B7', '#a3c28d', '#eba059', '#a84543', '#a3c28d', '#eba059', '#a84543', 'purple'],"
            ElseIf Trim(div_name) = "2chart_div_sold_avg_sold_all" Or Trim(div_name) = "chart_div_sold_avg_sold_all" Then
                temp_string &= "bar: {groupWidth: '75%'}, "
                temp_string &= "colors: ['green', 'blue', 'red','blue', 'red', 'green'],"
            ElseIf Trim(div_name) = "2chart_div_percent_asking_all" Or Trim(div_name) = "2chart_div_variance_all" Or Trim(div_name) = "chart_div_percent_asking_all" Or Trim(div_name) = "chart_div_variance_all" Then
                temp_string &= "bar: {groupWidth: '75%'}, "
                temp_string &= "colors: ['red', 'blue', 'green','blue', 'red', 'green'],"
            ElseIf make_into_pie = True Then
                'temp_string &= "colors: ['blue', 'red', 'green','purple', 'red', 'orange', 'red', 'red', 'red', 'red', 'red', 'red', 'red'],"
            ElseIf make_into_bar = True And show_only_sold = False Then
                temp_string &= "bar: {groupWidth: '75%'}, "
                temp_string &= "colors: ['" & value_color & "', 'red', 'green','blue', 'red', 'green'],"
            ElseIf show_my_asking_dashed Then
                temp_string &= "curveType: 'function',"
                temp_string &= "colors: ['" & value_color & "', 'green', 'red','purple', 'red', 'green'],"
            ElseIf make_into_bar = True And show_only_sold = True Then
                temp_string &= "bar: {groupWidth: '75%'}, "
                temp_string &= "colors: ['green', 'blue', 'red', 'blue', 'red', 'green'],"
            ElseIf Trim(div_name) = "chart_div_tab15_all" Or Trim(div_name) = "chart_div_tab16_all" Or Trim(div_name) = "chart_div_tab18_all" Or Trim(div_name) = "chart_div_tab100_all" Then
                temp_string &= "curveType:  'function',"
                temp_string &= "colors: ['#B7DCF6', 'red', 'green','blue', 'red', 'green'],"
            Else
                temp_string &= "curveType:  'function',"
                temp_string &= "colors: ['" & value_color & "', 'red', 'green','blue', 'red', 'green'],"
            End If

            If Trim(client_id) <> "" Then
            ElseIf ((Trim(div_name) = "chart_div_value_history1_all" Or Trim(div_name) = "chart_div_value_history2_all") And clsGeneral.clsGeneral.isEValuesAvailable() = True) Then
                ' dont do chart area command, already did it 
            ElseIf Trim(div_name) = "chart_div_tab15_all" Or Trim(div_name) = "chart_div_tab16_all" Or Trim(div_name) = "chart_div_tab18_all" Or Trim(div_name) = "chart_div_tab100_all" Then
            ElseIf make_into_pie = True Then
                ' do nothing
            ElseIf RemoveVisibleMarginsAndShrinkWhitespace = True Then
                temp_string &= " 'chartArea': {top:0,left:0}, "
            ElseIf Trim(div_name) = "chart_div_port_tab2_all" And make_into_pie = False Then
                ' do nothing 
            ElseIf Trim(div_name) = "chart_div_tab1_all" Or Trim(div_name) = "chart_div_tab2_all" Then
                If Trim(map_title) = "Dealer Sales Per Year" Then
                ElseIf Trim(y_axis_label) = "" Then
                    temp_string &= " 'chartArea': {top:5,left:35}, "
                Else
                    temp_string &= " 'chartArea': {top:5}, "
                End If
            ElseIf InStr(Right(Trim(div_name), 3), "all") = 0 And Trim(map_title) <> "Avg Asking vs Selling Price ($k)" And InStr(Trim(map_title), "FLIGHT ACTIVITY SUMMARY FOR LAST YEAR") = 0 Then
                'if its not all, then 5 space, if its all .. a little more space 
                If Trim(map_title) = "Dealer Sales Per Year" Then
                ElseIf Trim(y_axis_label) = "" Then
                    temp_string &= " 'chartArea': {top:5,left:35}, "
                Else
                    temp_string &= " 'chartArea': {top:5}, "
                End If
            ElseIf (Trim(div_name) = "chart_div_port_tab9_all") Then
                temp_string &= "" ' already did up top 
            Else
                temp_string &= " 'chartArea': {top:25}, "
            End If


            If Trim(client_id) <> "" Then
                If Trim(div_name) = "chart_div_tab9_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:22}},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:22}} "
                ElseIf Trim(div_name) = "chart_div_tab20_all" Or Trim(div_name) = "chart_div_tab21_all" Or Trim(div_name) = "chart_div_tab22_all" Or Trim(div_name) = "chart_div_tab23_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:22}},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:22}} "
                ElseIf make_into_bar = True And (Trim(div_name) = "chart_div_tab5_all" Or Trim(div_name) = "chart_div_tab18_all") Then
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'18'", "'19'") & "}, slantedText:true, slantedTextAngle:80},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'18'", "'20'") & "}} "
                ElseIf Trim(div_name) = "chart_div_tab13_all" And make_into_bar = False Then
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'9'", "'14'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'15'", "'18'") & "}} "
                ElseIf Trim(div_name) = "chart_div_tabVal1_all" Or Trim(div_name) = "chart_div_tabVal2_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'9'", "'14'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'9'", "'14'") & "}} "

                ElseIf Trim(div_name) = "chart_div_tab18_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'20'", "'22'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'26'", "'28'") & "}} "
                ElseIf Trim(div_name) = "chart_div_tab_Valuation1_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:12}, slantedText:true, slantedTextAngle:60},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:16}  "
                    If Trim(ticks_string) <> "" Then
                        temp_string &= ", ticks: [ " & ticks_string & "]"
                    End If
                    temp_string &= "} "
                ElseIf Trim(div_name) = "chart_div_tab_Valuation2_all" Or Trim(div_name) = "chart_div_tab_Valuation3_all" Or Trim(div_name) = "chart_div_tab_Valuation4_all" Or Trim(div_name) = "chart_div_value_history2_all" Or Trim(div_name) = "chart_div_tabVal2_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:12}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:15}  "
                    If Trim(ticks_string) <> "" Then
                        temp_string &= ", ticks: [ " & ticks_string & "]"
                    End If
                    temp_string &= "} "
                ElseIf Trim(div_name) = "chart_div_tab_Valuation5_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:10}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:15}  "
                    If Trim(ticks_string) <> "" Then
                        temp_string &= ", ticks: [ " & ticks_string & "]"
                    End If
                    temp_string &= "} "
                ElseIf Trim(div_name) = "chart_div_tab13_all" Or Trim(div_name) = "chart_div_tab14_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'16'", "'17'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'18'", "'20'") & "}} "
                ElseIf make_into_bar = True And (Trim(div_name) = "chart_div_tab17_all") Then
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'9'", "'10'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'15'", "'17'") & "}} "
                ElseIf make_into_bar = True And (Trim(div_name) = "chart_div_tab4_all") Then
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'16'", "'17'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "'"
                    If Trim(ticks_string) <> "" Then
                        temp_string &= ", ticks: [ " & ticks_string & "]"
                    End If
                    temp_string &= ", textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'18'", "'20'") & "}} "
                Else
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'16'", "'17'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'18'", "'20'") & "}} "
                End If
            ElseIf Trim(div_name) = "chart_div_tab15_all" Or Trim(div_name) = "chart_div_tab16_all" Or Trim(div_name) = "chart_div_tab18_all" Or Trim(div_name) = "chart_div_tab100_all" Then
                If Trim(div_name) = "chart_div_tab18_all" Then
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'11'", "'13'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'11'", "'13'") & "}} "
                Else
                    temp_string &= "hAxis: { textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'11'", "'13'") & "}, slantedText:true, slantedTextAngle:50},"   ', 
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', textStyle:{fontSize:" & IIf(RemoveVisibleMarginsAndShrinkWhitespace, "'11'", "'13'") & "}} "
                End If
            Else
                If format_x_axis Then
                    temp_string &= "hAxis: { textStyle:{fontSize:'9'}, slantedText:true, slantedTextAngle:70},"   ',
                Else
                    temp_string &= "hAxis: { textStyle:{fontSize:'9'}},"   'slantedText:true, slantedTextAngle:70,
                End If
                If Trim(map_title) = "Dealer Sales Per Year" Then
                    temp_string &= "vAxis: { textStyle:{fontSize:'8'}} "   '  title: '" & y_axis_label & "', 
                ElseIf div_name = "chart_div_top_all" Then
                    temp_string &= "vAxis: { title: '" & y_axis_label & "', format:  '#'} "
                Else
                    If Trim(y_axis_label) = "" Then
                        temp_string &= "vAxis: { title: '" & y_axis_label & "'} "
                    Else
                        temp_string &= "vAxis: { title: '" & y_axis_label & "'} "
                    End If
                End If
            End If

            If draw_line = True Then
                line_width = 2
            Else
                line_width = 0
            End If


            If is_large_graph = True Then

                If show_only_asking = True And show_only_take = True And show_only_sold = True Then ' all 
                    temp_string &= ", series: { "
                    temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3  } "
                    temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3 } "
                    temp_string &= ",  2: { lineWidth: " & line_width & ", pointSize: 3  } "
                    temp_string &= ",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false  } "
                    temp_string &= ",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false  } "
                    temp_string &= " } "
                ElseIf show_only_asking = True And show_only_take = False And show_only_sold = False Then ' only asking
                    temp_string &= ", series: { "
                    temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3  } "
                    temp_string &= ",  1: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  2: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= " } "
                ElseIf show_only_asking = False And show_only_take = True And show_only_sold = False Then ' only take
                    temp_string &= ", series: { "
                    temp_string &= "   0: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3} "
                    temp_string &= ",  2: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= " } "
                ElseIf show_only_asking = False And show_only_take = False And show_only_sold = True Then  ' only sold
                    temp_string &= ", series: { "
                    temp_string &= "   0: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  1: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  2: { lineWidth: " & line_width & ", pointSize: 3} "
                    temp_string &= ",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= " } "
                ElseIf show_only_asking = True And show_only_take = False And show_only_sold = True Then  ' only asking,sold
                    temp_string &= ", series: { "
                    temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3 } "
                    temp_string &= ",  1: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  2: { lineWidth: " & line_width & ", pointSize: 3} "
                    temp_string &= ",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= " } "
                ElseIf show_only_asking = True And show_only_take = True And show_only_sold = False Then  ' only asking,take
                    temp_string &= ", series: { "
                    temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3 } "
                    temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3 } "
                    temp_string &= ",  2: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= " } "
                ElseIf show_only_asking = False And show_only_take = True And show_only_sold = True Then  ' only take, sold
                    temp_string &= ", series: { "
                    temp_string &= "   0: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3 } "
                    temp_string &= ",  2: { lineWidth: " & line_width & ", pointSize: 3 } "
                    temp_string &= ",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                    temp_string &= " } "
                ElseIf show_only_take = True Then
                    temp_string &= ", series: { "
                    temp_string &= "   0: { lineWidth: 0, pointSize: 3, pointShape: 'star', visibleInLegend: false } "
                    temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3 } "
                    temp_string &= ",  2: { lineWidth: 0, pointSize: 3,  pointShape: 'star', visibleInLegend: false  } "
                    temp_string &= ",  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                    temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                    temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                End If
            ElseIf Trim(series_string) <> "" Then
                temp_string &= ", "
                temp_string &= Trim(series_string)
            ElseIf show_only_asking = True And show_only_take = True And show_only_sold = False Then

                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= ",  2: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= " } "

            ElseIf show_only_asking = True And show_only_take = False And show_only_sold = True Then  ' only asking,sold
                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3 } "
                temp_string &= ",  1: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                temp_string &= ",  2: { lineWidth: " & line_width & ", pointSize: 3} "
                temp_string &= ",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                temp_string &= ",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                temp_string &= ",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } "
                temp_string &= " } "
            ElseIf Trim(div_name) = "chart_div_tab_Valuation1_all" Then
                temp_string &= ", series: { "
                temp_string &= ("    0: { lineWidth: 0, pointSize: 4 } ")
                temp_string &= (" ,  1: { lineWidth: 0, pointSize: 4 } ")
                temp_string &= (" ,  2: { lineWidth: 1, pointSize: 2, lineDashStyle: [4, 4] } ")
                temp_string &= (",  3: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } ")
                temp_string &= (",  4: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } ")
                temp_string &= (",  5: { lineWidth: 0, pointSize: 0,  pointShape: 'star', visibleInLegend: false } ")
                temp_string &= (" } ")
            ElseIf Trim(div_name) = "chart_div_tab_Valuation2_all" Then
                temp_string &= ", series: { "
                temp_string &= ("    0: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  1: { lineWidth: 2, pointSize: 2  , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  2: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  3: { lineWidth: 0, pointSize: 3  } ")
                temp_string &= (" ,  4: { lineWidth: 0, pointSize: 3  } ")
                temp_string &= (" ,  5: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                temp_string &= (" ,  6: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                temp_string &= (" ,  7: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                temp_string &= (" } ")
            ElseIf Trim(div_name) = "chart_div_tab_Valuation3_all" Then
                temp_string &= ", series: { "
                temp_string &= ("    0: { lineWidth: 2, pointSize: 2 , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  1: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  2: { lineWidth: 2, pointSize: 2 , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  3: { lineWidth: 5, pointSize: 5, visibleInLegend: false  } ")
                temp_string &= (" ,  4: { lineWidth: 2, pointSize: 2, lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  5: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  6: { lineWidth: 2, pointSize: 2 , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  7: { lineWidth: 2, pointSize: 2  } ")
                temp_string &= (" ,  8: { lineWidth: 2, pointSize: 2  } ")
                temp_string &= (" ,  9: { lineWidth: 2, pointSize: 2  } ")
                temp_string &= (" ,  10: { lineWidth: 5, pointSize: 5, visibleInLegend: false  } ")
                temp_string &= (" ,  11: { lineWidth: 5, pointSize: 5, visibleInLegend: false  } ")
                temp_string &= (" ,  12: { lineWidth: 5, pointSize: 5, visibleInLegend: false  } ")
                temp_string &= (" ,  13: { lineWidth: 5, pointSize: 5, visibleInLegend: false  } ")

                temp_string &= (" } ")

            ElseIf Trim(div_name) = "chart_div_tab_Valuation4_all" Or Trim(div_name) = "chart_div_value_history2_all" Then
                temp_string &= ", series: { "
                temp_string &= ("  0: { lineWidth: 1, pointSize: 1 } ")

                For i = 1 To number_of_records - 1
                    temp_string &= (" ,  " & i & ": { lineWidth: 1, pointSize: 1 } ")
                Next

                For i = number_of_records To 50
                    temp_string &= (" ,  " & i & ": { lineWidth: 0, pointSize: 0, visibleInLegend: false  } ")
                Next
                temp_string &= (" } ")
            ElseIf Trim(div_name) = "chart_div_tabVal2_all" Then
                temp_string &= ", series: { "
                temp_string &= ("  0: { lineWidth: 1, pointSize: 1 } ")

                For i = 1 To number_of_records - 1
                    temp_string &= (" ,  " & i & ": { lineWidth: 1, pointSize: 1 } ")
                Next

                For i = number_of_records To 50
                    temp_string &= (" ,  " & i & ": { lineWidth: 0, pointSize: 0, visibleInLegend: false  } ")
                Next
                temp_string &= (" } ")


            ElseIf Trim(div_name) = "chart_div_port_tab2_all" Then
                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: 1, pointSize: 3  } "
                temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= ",  2: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= ",  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= " } "

            ElseIf Trim(div_name) = "chart_div_tab_Valuation5_all" Then
                temp_string &= ", series: { "
                temp_string &= ("    0: { lineWidth: 0, pointSize: 4 } ")
                temp_string &= (" ,  1: { lineWidth: 0, pointSize: 4 } ")
                temp_string &= (" ,  2: { lineWidth: 1, pointSize: 2, lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  3: { lineWidth: 1, pointSize: 2, lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  4: { lineWidth: 1, pointSize: 2, lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  5: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                temp_string &= (" ,  6: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                temp_string &= (" ,  7: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                temp_string &= (" } ")

            ElseIf make_into_bar = True And show_only_sold = False Then

                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= " } "
            ElseIf show_my_asking_dashed Then

                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3 } "
                temp_string &= ",  2: { lineWidth: 1, pointSize: 0, lineDashStyle: [10, 4] } "
                temp_string &= ",  3: { lineWidth: 1, pointSize: 0, lineDashStyle: [10, 4]  } "
                temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= " } "

            ElseIf show_only_asking Then
                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3  } "
                If Trim(div_name) = "chart_div_tab1_all" Or Trim(div_name) = "chart_div_tab7_all" Or Trim(div_name) = "chart_div_tab8_all" Or Trim(div_name) = "chart_div_tab11_all" Or Trim(div_name) = "chart_div_tab18_all" Then
                    temp_string &= " ,  1: { lineWidth: " & line_width & ", pointSize: 3  } "
                End If
                temp_string &= " } "
            ElseIf show_only_sold = True Then
                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: 0, pointSize: 3, pointShape: 'star', visibleInLegend: false } "
                temp_string &= ",  1: { lineWidth: 0, pointSize: 3,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  2: { lineWidth: " & line_width & ", pointSize: 3 } "
                temp_string &= ",  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= " } "
            ElseIf show_only_take = True And make_into_bar = False And make_into_pie = False And Trim(div_name) = "utilizationViewGraphall" Then ' probably messed up, but not gonna change curretly 
                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 4 } "
                temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 4 } "
                temp_string &= ",  2: { lineWidth: 0, pointSize: 3,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= " } "
            ElseIf show_only_take = True Then
                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: 0, pointSize: 3, pointShape: 'star', visibleInLegend: false } "
                temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3 } "
                temp_string &= ",  2: { lineWidth: 0, pointSize: 3,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= " } "
            ElseIf (Trim(div_name) = "chart_div_value_history1_all" Or Trim(div_name) = "chart_div_tabVal1_all" Or Trim(div_name) = "chart_div_tabVal2_all") And clsGeneral.clsGeneral.isEValuesAvailable() = True Then
                temp_string &= ", series: { "
                temp_string &= ("    0: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4] } ")
                temp_string &= (" ,  1: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4], visibleInLegend: false } ")
                temp_string &= (" ,  2: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4], visibleInLegend: false } ")
                temp_string &= (" ,  3: { lineWidth: 5, pointSize: 5 , visibleInLegend: false  } ")
                temp_string &= (" ,  4: { lineWidth: 3, pointSize: 3, lineDashStyle: [4, 4], visibleInLegend: false } ")
                temp_string &= (" ,  5: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4], visibleInLegend: false } ")
                temp_string &= (" ,  6: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4]} ")

                If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                    temp_string &= (" ,  7: { lineWidth: 3, pointSize: 3  } ")
                    temp_string &= (" ,  8: { lineWidth: 3, pointSize: 3  } ")
                    temp_string &= (" ,  9: { lineWidth: 3, pointSize: 3  } ")
                    temp_string &= (" ,  10: { lineWidth: 5, pointSize: 5  } ")
                    temp_string &= (" ,  11: { lineWidth: 5, pointSize: 5  } ")
                    temp_string &= (" ,  12: { lineWidth: 5, pointSize: 5  } ")
                    temp_string &= (" ,  13: { lineWidth: 5, pointSize: 5  } ")
                Else
                    temp_string &= (" ,  7: { lineWidth: 3, pointSize: 3 , visibleInLegend: false } ")
                    temp_string &= (" ,  8: { lineWidth: 3, pointSize: 3  } ")
                    temp_string &= (" ,  9: { lineWidth: 3, pointSize: 3  } ")
                    temp_string &= (" ,  10: { lineWidth: 5, pointSize: 5 , visibleInLegend: false } ")
                    temp_string &= (" ,  11: { lineWidth: 5, pointSize: 5 , visibleInLegend: false } ")
                    temp_string &= (" ,  12: { lineWidth: 5, pointSize: 5 , visibleInLegend: false } ")
                    temp_string &= (" ,  13: { lineWidth: 5, pointSize: 5 , visibleInLegend: false } ")
                End If




                temp_string &= " } "
            Else
                temp_string &= ", series: { "
                temp_string &= "   0: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= ",  1: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= ",  2: { lineWidth: " & line_width & ", pointSize: 3  } "
                temp_string &= ",  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= ",  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } "
                temp_string &= " } "
            End If

            ' do replaces for point size 
            If Trim(div_name) = "chart_div_tab20_all" Or Trim(div_name) = "chart_div_tab21_all" Or Trim(div_name) = "chart_div_tab22_all" Or Trim(div_name) = "chart_div_tab23_all" Or Trim(div_name) = "chart_div_tab_Valuation1_all" Or Trim(div_name) = "chart_div_tab_Valuation2_all" Or Trim(div_name) = "chart_div_tab_Valuation3_all" Or Trim(div_name) = "chart_div_tab_Valuation4_all" Or Trim(div_name) = "chart_div_tab_Valuation5_all" Or Trim(div_name) = "chart_div_tab_Valuation6_all" Or Trim(div_name) = "chart_div_tabVal1_all" Or Trim(div_name) = "chart_div_tabVal2_all" Then
                'temp_string = Replace(temp_string, "lineWidth: 2", "lineWidth: 1")
                'temp_string = Replace(temp_string, ", pointSize: 3", "")
            ElseIf Trim(client_id) <> "" Then
                temp_string = Replace(temp_string, ", pointSize: 3", "")
                temp_string = Replace(temp_string, "lineWidth: " & line_width, "lineWidth: 4")
                temp_string = Replace(temp_string, "lineWidth: 1", "lineWidth: 4")
                temp_string = Replace(temp_string, "ForSale", "For Sale")
                temp_string = Replace(temp_string, "'width': '800'", "'width': '880'")
            End If

            temp_string &= "};"

            ' Instantiate and draw our chart, passing in some options.
            If make_into_pie = True Then
                temp_string &= "var chart" & chart_num.ToString.Trim & " = new google.visualization.PieChart(document.getElementById('" & div_name & "'));"

                If Trim(client_id) <> "" Then
                    temp_string &= "   google.visualization.events.addListener(chart" & chart_num.ToString.Trim & ", 'ready', function() {"
                    temp_string &= "     console.log(chart" & chart_num.ToString.Trim & ".getImageURI());"
                    temp_string &= " document.getElementById('" & client_id & "').innerHTML = '<img src=""' + chart" & chart_num.ToString.Trim & ".getImageURI() + '"">'"
                    temp_string &= "});"
                End If

                temp_string &= "chart" & chart_num.ToString.Trim & ".draw(data" & chart_num.ToString.Trim & ", options" & chart_num.ToString.Trim & ");"
            ElseIf make_into_bar = True Then

                If Trim(div_name) = "chart_div_tab7_all" Or Trim(div_name) = "chart_div_tab8_all" Or Trim(div_name) = "chart_div_tab11_all" Or Trim(div_name) = "chart_div_tab20_all" Or Trim(div_name) = "chart_div_tab21_all" Or Trim(div_name) = "chart_div_tab22_all" Or Trim(div_name) = "chart_div_tab23_all" Or Trim(div_name) = "chart_div_tab5_all" Or Trim(div_name) = "chart_div_tab18_all" Then
                    temp_string &= "var chart" & chart_num.ToString.Trim & " = new google.visualization.BarChart(document.getElementById('" & div_name & "'));"
                Else
                    temp_string &= "var chart" & chart_num.ToString.Trim & " = new google.visualization.ColumnChart(document.getElementById('" & div_name & "'));"
                End If

                If Trim(client_id) <> "" Then
                    temp_string &= "   google.visualization.events.addListener(chart" & chart_num.ToString.Trim & ", 'ready', function() {"
                    temp_string &= "     console.log(chart" & chart_num.ToString.Trim & ".getImageURI());"
                    temp_string &= " document.getElementById('" & client_id & "').innerHTML = '<img src=""' + chart" & chart_num.ToString.Trim & ".getImageURI() + '"">'"
                    temp_string &= "});"
                End If


                temp_string &= "chart" & chart_num.ToString.Trim & ".draw(data" & chart_num.ToString.Trim & ", options" & chart_num.ToString.Trim & ");"
            Else
                temp_string &= "var chart" & chart_num.ToString.Trim & " = new google.visualization.LineChart(document.getElementById('" & div_name & "'));"

                If Trim(client_id) <> "" Then
                    temp_string &= "   google.visualization.events.addListener(chart" & chart_num.ToString.Trim & ", 'ready', function() {"
                    temp_string &= "     console.log(chart" & chart_num.ToString.Trim & ".getImageURI());"
                    temp_string &= " document.getElementById('" & client_id & "').innerHTML = '<img src=""' + chart" & chart_num.ToString.Trim & ".getImageURI() + '"">'"
                    temp_string &= "});"
                End If


                temp_string &= "chart" & chart_num.ToString.Trim & ".draw(data" & chart_num.ToString.Trim & ", options" & chart_num.ToString.Trim & ");"


            End If

        End If



        If InStr(Right(Trim(div_name), 3), "all") = 0 Then
            temp_string &= "}"
            temp_string &= "</script>"
        End If



        If InStr(Right(Trim(div_name), 3), "all") > 0 Then
            string_to_return = string_to_return & temp_string
        Else
            label_script.ID = "label_script"
            label_script.Text = temp_string

            If Not page1.ClientScript.IsClientScriptBlockRegistered("GoogleChart1Tab") Then
                GoogleChart1TabScript.Append(temp_string)

                System.Web.UI.ScriptManager.RegisterStartupScript(temp_panel, page1.GetType(), "GoogleChart1Tab", GoogleChart1TabScript.ToString, False)
            End If
        End If

    End Sub

    Public Shared Sub load_google_chart_dynamic(ByVal tab_to_add_to As AjaxControlToolkit.TabPanel, ByVal array_string As String, ByVal map_title As String, ByVal y_axis_label As String, ByVal div_name As String, ByVal width As Integer, ByVal height As Integer, ByVal line_or_points As String, ByVal chart_num As Integer, ByRef string_to_return As String, ByRef page1 As Page, ByRef temp_panel As System.Web.UI.UpdatePanel, ByVal format_x_axis As Boolean, ByVal draw_line As Boolean, ByVal labels_to_show As Integer, ByVal total_other_labels As Integer, ByVal globalDeclare As Boolean)
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label
        Dim line_width As Integer = 0
        Dim i As Integer = 0

        If InStr(Right(Trim(div_name), 3), "all") = 0 Then
            temp_string = "<script type=""text/javascript"">"

            ' Load the Visualization API and the piechart package.
            '  temp_string &= "google.load('visualization', '1.0', {'packages':['corechart']});"

            ' Set a callback to run when the Google Visualization API is loaded.
            'temp_string &= "google.setOnLoadCallback(drawChart);"
            temp_string &= "drawChart" & chart_num.ToString.Trim & "();"

            ' Callback that creates and populates a data table,
            ' instantiates the pie chart, passes in the data and
            ' draws it.
            temp_string &= "function drawChart" & chart_num.ToString.Trim & "() {"
        End If


        If Trim(array_string) <> "" Then

            If globalDeclare = False Then
                temp_string &= "var "
            End If
            If Trim(line_or_points) = "POINTS" Then
                temp_string &= " data" & chart_num.ToString.Trim & " = new google.visualization.DataTable();"
            Else
                temp_string &= " data" & chart_num.ToString.Trim & " = google.visualization.arrayToDataTable(["
            End If

            temp_string &= array_string

            temp_string &= "]);"

            ' Set chart options
            temp_string &= "var options" & chart_num.ToString.Trim & " = {"

            temp_string &= "'title':'" & map_title & "',"


            temp_string &= "'width':" & width & ","
            temp_string &= "'height':" & height & ","
            ' temp_string &= "legend:{}, "         ' , fontName:'xx'
            '  temp_string &= "tooltip:{textStyle:{fontSize:'6'}}, "


            ' temp_string &= "legend: { position: 'none' },"
            ' ElseIf Trim(div_name) = "chart_div_value_history" Then
            temp_string &= "legend: { position: 'right', textStyle:{fontSize:'11'}},"
            temp_string &= "curveType:  'function',"


            temp_string &= "colors: ['blue', 'red', 'green','blue', 'red', 'green'],"

            temp_string &= " 'chartArea': {top:25}, "


            If format_x_axis Then
                temp_string &= "hAxis: { textStyle:{fontSize:'9'}, slantedText:true, slantedTextAngle:70},"   ',
            Else
                temp_string &= "hAxis: { textStyle:{fontSize:'9'}},"   'slantedText:true, slantedTextAngle:70,
            End If


            temp_string &= "vAxis: { title: '" & y_axis_label & "'} "


            If draw_line = True Then
                line_width = 2
            Else
                line_width = 0
            End If


            temp_string &= ", series: { "

            For i = 0 To labels_to_show - 1
                If i = 0 Then
                    temp_string &= "   " & i & ": { lineWidth: " & line_width & ", pointSize: 3  } "
                Else
                    temp_string &= ", " & i & ": { lineWidth: " & line_width & ", pointSize: 3  } "
                End If
            Next


            For i = labels_to_show To total_other_labels + labels_to_show
                temp_string &= ", " & i & ": { lineWidth: 0, pointSize: 2, visibleInLegend: false  } "
            Next






            temp_string &= " } "


            temp_string &= "};"



            temp_string &= "var chart" & chart_num.ToString.Trim & " = new google.visualization.LineChart(document.getElementById('" & div_name & "'));"
            temp_string &= "chart" & chart_num.ToString.Trim & ".draw(data" & chart_num.ToString.Trim & ", options" & chart_num.ToString.Trim & ");"




        End If



        If InStr(Right(Trim(div_name), 3), "all") = 0 Then
            'temp_string &= "alert(document.getElementById('" & div_name & "'));"
            temp_string &= "}"
            temp_string &= "</script>"
        End If

        If InStr(Right(Trim(div_name), 3), "all") > 0 Then
            string_to_return = string_to_return & temp_string
        Else
            label_script.ID = "label_script"
            label_script.Text = temp_string

            If Not page1.ClientScript.IsClientScriptBlockRegistered("GoogleChart1Tab") Then
                GoogleChart1TabScript.Append(temp_string)

                System.Web.UI.ScriptManager.RegisterStartupScript(temp_panel, page1.GetType(), "GoogleChart1Tab", GoogleChart1TabScript.ToString, False)
            End If
        End If
        ' chart_label.ID = "chart_label"
        'chart_label.Text = "<div id=""chart_div""></div>"

        'tab_to_add_to.Controls.AddAt(1, chart_label)

    End Sub

    Public Shared Function CRMDisplay_Notes_Or_Actions_MPM(ByVal notesTable As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal DisplayHeaderDate As Boolean, ByVal DisplayACInfo As Boolean, ByVal DisplayCompInfo As Boolean, ByVal DisplayYachtInfo As Boolean, ByVal useUL As Boolean, ByVal ShowingNotes As Boolean, ByVal ShowingActions As Boolean, Optional ByVal is_home_page As Boolean = False, Optional ByRef CRMVIew As Boolean = False, Optional ByRef CRMSource As String = "JETNET", Optional ByVal is_prospector As Boolean = False, Optional ByRef ShortenNotes As Boolean = False, Optional ByVal FromContact As Boolean = False) As String
        Dim aTempTable As New DataTable
        Dim ReturnString As String = ""
        Dim today As Date = FormatDateTime(Now(), 2)
        Dim week As Integer = Weekday(today)
        Dim monthint As Integer = Month(today)
        Dim monthdis As String = MonthName(monthint)
        Dim weekdis As String = WeekdayName(week)
        Dim yeardis As Integer = Year(today)
        Dim daydis As Integer = Day(today)
        Dim oldweekdis As Integer = 0
        Dim oldmonthint As Integer = 0
        Dim olddaydis As Integer = 0
        Dim return_string_text As String = ""
        Dim edit_link As String = ""
        Dim user_link As String = ""
        Dim comp_contacts_string As String = ""
        Dim inner_text As String = ""
        Dim temp_note As String = ""

        If Not IsNothing(notesTable) Then

            If notesTable.Rows.Count > 0 Then
                ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid"">"

                'Let's set up a display that doesn't show the header for the AC Details Page. 
                If DisplayHeaderDate = False Then
                    ' ReturnString += "<tr><td align=""left"" valign=""top"">"
                    If useUL = True Then
                        ReturnString += "<ul class=""circle"">"
                    End If
                End If

                For Each r As DataRow In notesTable.Rows
                    ' If r("lnote_jetnet_ac_id") > 0 Then
                    Dim timeofday = TimeValue(today)
                    Dim AC_Link_Text As String = ""
                    Dim Yacht_Link_Text As String = ""
                    Dim COMPANY_Link_Text As String = ""

                    '   Dim JETNET_AC_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_ac_id")), r("lnote_jetnet_ac_id"), 0)
                    Dim JETNET_COMPANY_ID As Long = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0)
                    '  Dim JETNET_MODEL_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_amod_id")), r("lnote_jetnet_amod_id"), 0)
                    '  Dim CLIENT_MODEL_ID As Long = IIf(Not IsDBNull(r("lnote_client_amod_id")), r("lnote_client_amod_id"), 0)
                    '   Dim CLIENT_AC_ID As Long = IIf(Not IsDBNull(r("lnote_client_ac_id")), r("lnote_client_ac_id"), 0)
                    '   Dim CLIENT_COMPANY_ID As Long = IIf(Not IsDBNull(r("lnote_client_comp_id")), r("lnote_client_comp_id"), 0)

                    '  Dim JETNET_YACHT_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_yacht_id")), r("lnote_jetnet_yacht_id"), 0)
                    Dim jetnet_contact_id As Long = IIf(Not IsDBNull(r("cprospect_contact_id")), r("cprospect_contact_id"), 0)
                    '   Dim client_contact_id As Long = IIf(Not IsDBNull(r("lnote_client_contact_id")), r("lnote_client_contact_id"), 0)
                    Dim contacts_temp As String = ""
                    Dim lnoteStatus As String = IIf(Not IsDBNull(r("cprospect_status")), r("cprospect_status"), "")
                    Dim DisplayDate As String = ""
                    If ShowingActions = True Then
                        If Not IsDBNull(r("cprospect_target_date")) Then
                            DisplayDate = Format(CDate(r("cprospect_target_date")), "MM/dd/yyyy") & " - "
                        Else
                            DisplayDate = ""
                        End If
                        ' DisplayDate = IIf(Not IsDBNull(r("lnote_schedule_start_date")), Format(CDate(r("lnote_schedule_start_date")), "MM/dd/yyyy") & " - ", "") & ""

                    Else
                        DisplayDate = IIf(Not IsDBNull(r("cprospect_target_date")), Format(CDate(r("cprospect_target_date")), "MM/dd/yyyy") & " - ", "") & ""
                    End If

                    'Formatting for Action Items
                    'Edit - Rick Wanner - 1986 BEECHJET 400 - S/N# RJ-2, Reg# N369EA - Validate the aircraft is for sale and get asking price.
                    today = IIf(Not IsDBNull(r("cprospect_target_date")), r("cprospect_target_date"), Now())

                    week = Weekday(today)
                    daydis = Day(today)
                    weekdis = WeekdayName(week)
                    monthint = Month(today)
                    monthdis = Left(MonthName(monthint), 3)

                    If DisplayCompInfo = True Then
                        If JETNET_COMPANY_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.GetLimited_CompanyInfo_ID(JETNET_COMPANY_ID, "JETNET", 0)
                            COMPANY_Link_Text = CompanyFunctions.Display_Company_Information_For_Link(aTempTable, False, 0)
                            ' ElseIf CLIENT_COMPANY_ID <> 0 Then
                            '     aTempTable = New DataTable
                            '     aTempTable = aclsData_Temp.GetLimited_CompanyInfo_ID(CLIENT_COMPANY_ID, "CLIENT", 0)
                            '     COMPANY_Link_Text = CompanyFunctions.Display_Company_Information_For_Link(aTempTable, False, 0)
                        End If
                    End If

                    If DisplayHeaderDate = True Then
                        If daydis <> olddaydis Or week <> oldweekdis Or monthint <> oldmonthint Then
                            If olddaydis <> 0 And oldweekdis <> 0 And oldmonthint <> 0 Then
                                If useUL = True Then
                                    ReturnString += "</ul>"
                                End If
                            End If
                            ReturnString += "<tr class=""header_row"">"
                            ReturnString += "<td align=""left"" valign=""top"">"
                            ReturnString += "<strong class=""blue_text"">" & weekdis & ", " & monthdis & " " & daydis & " " & Year(today) & "</strong>"
                            ReturnString += "</td>"
                            ReturnString += "</tr>"
                            ReturnString += "<tr>"
                            ReturnString += " <td align=""left"" valign=""top"">"
                            If useUL = True Then
                                ReturnString += "<ul class=""circle"">"
                            End If
                        End If
                    End If

                    If useUL = True Then
                        ReturnString += "<li>"
                    Else
                        ReturnString += "<tr><td align='left' valign='top'><span class='li'>"
                    End If


                    inner_text = ""
                    edit_link = ""
                    'If CRMVIew = False Then
                    '    If HttpContext.Current.Session.Item("localUser").crmLocalUserID = r("lnote_user_id") Or HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag Then
                    '        edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit") & " - "
                    '        inner_text &= edit_link
                    '    Else
                    '        If Not HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then 'If Not administrator view link only.
                    '            edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "View") & " - "
                    '            inner_text &= edit_link
                    '        Else 'otherwise they can go ahead and edit.
                    '            edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit") & " - "
                    '            inner_text &= edit_link
                    '        End If
                    '    End If
                    'Else
                    'link?
                    If is_prospector = True Then
                        'If CRMSource = "CLIENT" Then
                        '  edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, CLIENT_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1&source=CLIENT", "&source=CLIENT"), "Edit") & " - "
                        '  inner_text &= edit_link
                        'Else
                        '  edit_link = CRM_WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, 0, True, IIf(ShowingNotes = True, "&action=edit&type=prospect&cat_key=0", "&action=edit&type=prospect&cat_key=0") & "&source=JETNET&from=aircraftDetails", "Edit") & " - "
                        '  inner_text &= edit_link
                        'End If
                        edit_link = "<a href=""javascript:void(0)"" style=""text-decoration:none !important;""  title=""Edit Prospect"" onclick=""javascript:load('"
                        edit_link &= "/edit_note.aspx?ViewID=18&refreshing=prospect&action=edit&type=prospect&id=" & r("cprospect_id").ToString & ""
                        'edit_link &= "/edit_note.aspx?ViewID=18&action=edit&type=prospect&id=" & r("lnote_id").ToString & "&source=" & CRMSource & "&from=" & IIf(FromContact, "contactDetails", "companyDetails") & ""
                        edit_link &= "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');""> "

                        inner_text &= edit_link
                    End If
                    '  End If


                    If DisplayHeaderDate = False Then
                        inner_text += DisplayDate
                    End If




                    comp_contacts_string = ""

                    If is_prospector = True Then

                        ' inner_text = ""
                        ' edit_link = Replace(edit_link, ">Edit</a> -", ">Note:</a> ")
                        inner_text = Replace(edit_link, "style=""text-decoration:none !important;""", "class=""noCase emphasisColor text_underline")

                        'lnote_opportunity_status
                        ' lnote_notecat_key
                        If Not IsDBNull(r("cprospect_service")) Then
                            If Not String.IsNullOrEmpty(r("cprospect_service")) Then
                                inner_text &= Trim(r("cprospect_service")) & " "
                            End If
                        End If


                        If Not IsDBNull(r("cprospect_type")) Then
                            If Trim(r("cprospect_type")) <> "Not Specified" Then
                                inner_text &= Trim(r("cprospect_type"))
                            End If
                        End If

                        inner_text &= "</a>"

                        If Not IsDBNull(r("cprospect_type")) Then
                            If Trim(r("cprospect_type")) <> "Not Specified" Then
                                inner_text &= " - "
                            End If
                        End If


                        If Not IsDBNull(r("cprospect_status")) Then
                            If Trim(r("cprospect_status")) = "B" Then
                                inner_text &= Trim(r("cprospect_status"))
                            End If
                        End If

                        'If Trim(r("lnote_status")) = "B" Then
                        '  If JETNET_AC_ID > 0 Or CLIENT_AC_ID > 0 Or CLIENT_MODEL_ID > 0 Or JETNET_MODEL_ID > 0 Then
                        '    inner_text &= " Interested in "
                        '  ElseIf JETNET_COMPANY_ID > 0 Or CLIENT_COMPANY_ID > 0 Then
                        '    inner_text &= " Interest By "
                        '  End If

                        'End If



                        If Trim(COMPANY_Link_Text) <> "" Then
                            If Trim(r("cprospect_status")) = "B" Then
                                inner_text &= " Interest By"
                            End If
                            inner_text &= "&nbsp;&nbsp;<span title=""View Company Details"">" & Replace(COMPANY_Link_Text, "href='#'", " class=""noCase emphasisColor text_underline"" href=""#""") & "</a>. "
                        End If


                        inner_text &= "</a>"


                        '1,000 thing ::: test

                        If Not IsDBNull(r("cprospect_details")) Then
                            If InStr(r("cprospect_details"), " ::: ") > 0 Then
                                Dim splTemp As String() = Split(r("cprospect_details"), " ::: ")
                                If UBound(splTemp) > 0 Then
                                    inner_text &= "<span class=""gray_text"">" & edit_link & splTemp(0) & "</a></span> " & splTemp(1) & "."
                                Else
                                    inner_text &= r("cprospect_details").ToString & "."
                                End If
                            Else
                                inner_text &= r("cprospect_details").ToString & "."
                            End If
                        End If


                        If Not IsDBNull(r("cprospect_value")) Then
                            If r("cprospect_value") > 0 Then
                                inner_text &= " $" & Trim(r("cprospect_value")) & " Opportunity"
                            End If
                        End If

                        If Not IsDBNull(r("cprospect_percent_win")) Then
                            If r("cprospect_percent_win") > 0 Then
                                inner_text &= " [" & Trim(r("cprospect_percent_win")) & "% Probability]"
                            End If
                        End If


                        If Not IsDBNull(r("cprospect_value")) Or Not IsDBNull(r("cprospect_percent_win")) Then
                            inner_text &= "."
                        End If

                        If Not IsDBNull(r("cprospect_user_id")) Then
                            inner_text &= " <em>Assigned To: " & r("cprospect_user_id") & "</em> "
                        ElseIf Trim(user_link) <> "" Then
                            inner_text &= " <em>Assigned To: " & user_link & "</em> "
                        End If

                    Else
                        inner_text &= IIf(AC_Link_Text <> "", " " & AC_Link_Text & "</a>", "") & " - " & IIf(COMPANY_Link_Text <> "", COMPANY_Link_Text & " - ", "") & " " & " " & IIf(Yacht_Link_Text <> "", Yacht_Link_Text & " - ", "") & IIf(ShortenNotes = False Or ShowingNotes = False, r("cprospect_details").ToString, IIf(Len(r("cprospect_details")) > 100, Left(r("cprospect_details").ToString, 100) & "..", r("cprospect_details").ToString)) & "."
                    End If


                    ReturnString &= inner_text & comp_contacts_string
                    inner_text = ""

                    If useUL = True Then
                        ReturnString += "</li>"
                    Else
                        ReturnString += "</span></td></tr>"
                    End If
                    oldweekdis = week
                    oldmonthint = monthint
                    olddaydis = daydis
                    'End If
                Next

                ReturnString += "</ul>"
                ReturnString += " </td>"
                ReturnString += " </tr>"
                ReturnString += "</table>"
            Else
                ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid""><tr><td align='left' valign='top' class=""noBorder"">"
                ReturnString += "<span>No current " & IIf(is_prospector, "prospects/opportunities", IIf(ShowingActions = True, "action items", "notes")) & " available for display.</span>"
                ReturnString += "</td></tr></table>"
            End If
        End If

        Return ReturnString
    End Function

    Public Shared Function CRMDisplay_Notes_Or_Actions(ByVal notesTable As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal DisplayHeaderDate As Boolean, ByVal DisplayACInfo As Boolean, ByVal DisplayCompInfo As Boolean, ByVal DisplayYachtInfo As Boolean, ByVal useUL As Boolean, ByVal ShowingNotes As Boolean, ByVal ShowingActions As Boolean, Optional ByVal is_home_page As Boolean = False, Optional ByRef CRMVIew As Boolean = False, Optional ByRef CRMSource As String = "JETNET", Optional ByVal is_prospector As Boolean = False, Optional ByRef ShortenNotes As Boolean = False, Optional ByVal FromContact As Boolean = False) As String
        Dim aTempTable As New DataTable
        Dim ReturnString As String = ""
        Dim today As Date = FormatDateTime(Now(), 2)
        Dim week As Integer = Weekday(today)
        Dim monthint As Integer = Month(today)
        Dim monthdis As String = MonthName(monthint)
        Dim weekdis As String = WeekdayName(week)
        Dim yeardis As Integer = Year(today)
        Dim daydis As Integer = Day(today)
        Dim oldweekdis As Integer = 0
        Dim oldmonthint As Integer = 0
        Dim olddaydis As Integer = 0
        Dim return_string_text As String = ""
        Dim edit_link As String = ""
        Dim user_link As String = ""
        Dim comp_contacts_string As String = ""
        Dim inner_text As String = ""
        Dim temp_note As String = ""

        If Not IsNothing(notesTable) Then

            If notesTable.Rows.Count > 0 Then
                ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid"">"

                'Let's set up a display that doesn't show the header for the AC Details Page. 
                If DisplayHeaderDate = False Then
                    ' ReturnString += "<tr><td align=""left"" valign=""top"">"
                    If useUL = True Then
                        ReturnString += "<ul class=""circle"">"
                    End If
                End If

                For Each r As DataRow In notesTable.Rows
                    ' If r("lnote_jetnet_ac_id") > 0 Then
                    Dim timeofday = TimeValue(today)
                    Dim AC_Link_Text As String = ""
                    Dim Yacht_Link_Text As String = ""
                    Dim COMPANY_Link_Text As String = ""
                    Dim JETNET_AC_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_ac_id")), r("lnote_jetnet_ac_id"), 0)
                    Dim JETNET_COMPANY_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_comp_id")), r("lnote_jetnet_comp_id"), 0)
                    Dim JETNET_MODEL_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_amod_id")), r("lnote_jetnet_amod_id"), 0)
                    Dim CLIENT_MODEL_ID As Long = IIf(Not IsDBNull(r("lnote_client_amod_id")), r("lnote_client_amod_id"), 0)
                    Dim CLIENT_AC_ID As Long = IIf(Not IsDBNull(r("lnote_client_ac_id")), r("lnote_client_ac_id"), 0)
                    Dim CLIENT_COMPANY_ID As Long = IIf(Not IsDBNull(r("lnote_client_comp_id")), r("lnote_client_comp_id"), 0)

                    Dim JETNET_YACHT_ID As Long = IIf(Not IsDBNull(r("lnote_jetnet_yacht_id")), r("lnote_jetnet_yacht_id"), 0)
                    Dim jetnet_contact_id As Long = IIf(Not IsDBNull(r("lnote_jetnet_contact_id")), r("lnote_jetnet_contact_id"), 0)
                    Dim client_contact_id As Long = IIf(Not IsDBNull(r("lnote_client_contact_id")), r("lnote_client_contact_id"), 0)
                    Dim contacts_temp As String = ""
                    Dim lnoteStatus As String = IIf(Not IsDBNull(r("lnote_status")), r("lnote_status"), "")
                    Dim DisplayDate As String = ""
                    If ShowingActions = True Then
                        If Not IsDBNull(r("lnote_schedule_start_date")) Then
                            DisplayDate = Format(CDate(r("lnote_schedule_start_date")), "MM/dd/yyyy") & " - "
                        Else
                            DisplayDate = ""
                        End If
                        ' DisplayDate = IIf(Not IsDBNull(r("lnote_schedule_start_date")), Format(CDate(r("lnote_schedule_start_date")), "MM/dd/yyyy") & " - ", "") & ""

                    Else
                        DisplayDate = IIf(Not IsDBNull(r("lnote_entry_date")), Format(CDate(r("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ""
                    End If

                    'Formatting for Action Items
                    'Edit - Rick Wanner - 1986 BEECHJET 400 - S/N# RJ-2, Reg# N369EA - Validate the aircraft is for sale and get asking price.
                    today = IIf(Not IsDBNull(r("lnote_schedule_start_date")), r("lnote_schedule_start_date"), Now())

                    week = Weekday(today)
                    daydis = Day(today)
                    weekdis = WeekdayName(week)
                    monthint = Month(today)
                    monthdis = Left(MonthName(monthint), 3)

                    If DisplayACInfo = True Then

                        If JETNET_AC_ID > 0 Or CLIENT_AC_ID > 0 Then
                            AC_Link_Text = clsGeneral.clsGeneral.DisplayAircraftName(IIf(CLIENT_AC_ID > 0, CLIENT_AC_ID, JETNET_AC_ID), IIf(CLIENT_AC_ID > 0, "CLIENT", "JETNET"), aclsData_Temp, True)  'CommonAircraftFunctions.Display_Aircraft_Information_For_Link(aTempTable, True, 0)
                        ElseIf JETNET_MODEL_ID > 0 Or CLIENT_MODEL_ID > 0 Then
                            AC_Link_Text = ReturnModel(IIf(CRMSource = "CLIENT", CLIENT_MODEL_ID, JETNET_MODEL_ID), CRMSource, aclsData_Temp) & "</a>"
                        End If
                    End If

                    If DisplayYachtInfo = True Then
                        If JETNET_YACHT_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.DisplayYachtByID(JETNET_YACHT_ID)
                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then
                                    Yacht_Link_Text = Display_Yacht_Information_For_Link(aTempTable)
                                End If
                            End If
                            aTempTable.Dispose()
                        End If
                    End If

                    If DisplayCompInfo = True Then
                        If JETNET_COMPANY_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.GetLimited_CompanyInfo_ID(JETNET_COMPANY_ID, "JETNET", 0)
                            COMPANY_Link_Text = CompanyFunctions.Display_Company_Information_For_Link(aTempTable, False, 0)
                        ElseIf CLIENT_COMPANY_ID <> 0 Then
                            aTempTable = New DataTable
                            aTempTable = aclsData_Temp.GetLimited_CompanyInfo_ID(CLIENT_COMPANY_ID, "CLIENT", 0)
                            COMPANY_Link_Text = CompanyFunctions.Display_Company_Information_For_Link(aTempTable, False, 0)
                        End If
                    End If

                    If DisplayHeaderDate = True Then
                        If daydis <> olddaydis Or week <> oldweekdis Or monthint <> oldmonthint Then
                            If olddaydis <> 0 And oldweekdis <> 0 And oldmonthint <> 0 Then
                                If useUL = True Then
                                    ReturnString += "</ul>"
                                End If
                            End If
                            ReturnString += "<tr class=""header_row"">"
                            ReturnString += "<td align=""left"" valign=""top"">"
                            ReturnString += "<strong class=""blue_text"">" & weekdis & ", " & monthdis & " " & daydis & " " & Year(today) & "</strong>"
                            ReturnString += "</td>"
                            ReturnString += "</tr>"
                            ReturnString += "<tr>"
                            ReturnString += " <td align=""left"" valign=""top"">"
                            If useUL = True Then
                                ReturnString += "<ul class=""circle"">"
                            End If
                        End If
                    End If

                    If useUL = True Then
                        ReturnString += "<li>"
                    Else
                        ReturnString += "<tr><td align='left' valign='top'><span class='li'>"
                    End If


                    inner_text = ""
                    edit_link = ""
                    If CRMVIew = False Then
                        If HttpContext.Current.Session.Item("localUser").crmLocalUserID = r("lnote_user_id") Or HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag Then
                            edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit") & " - "
                            inner_text &= edit_link
                        Else
                            If Not HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then 'If Not administrator view link only.
                                edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "View") & " - "
                                inner_text &= edit_link
                            Else 'otherwise they can go ahead and edit.
                                edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1", ""), "Edit") & " - "
                                inner_text &= edit_link
                            End If
                        End If
                    Else
                        'link?
                        If is_prospector = True Then
                            'If CRMSource = "CLIENT" Then
                            '  edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, CLIENT_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1&source=CLIENT", "&source=CLIENT"), "Edit") & " - "
                            '  inner_text &= edit_link
                            'Else
                            '  edit_link = CRM_WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, 0, True, IIf(ShowingNotes = True, "&action=edit&type=prospect&cat_key=0", "&action=edit&type=prospect&cat_key=0") & "&source=JETNET&from=aircraftDetails", "Edit") & " - "
                            '  inner_text &= edit_link
                            'End If
                            edit_link = "<a href=""javascript:void(0)"" class=""noCase emphasisColor text_underline""  title=""Edit Prospect"" onclick=""javascript:load('/edit_note.aspx?ViewID=18&action=edit&type=prospect&id=" & r("lnote_id").ToString & "&source=" & CRMSource & "&from=" & IIf(FromContact, "contactDetails", "companyDetails") & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');""> "
                            inner_text &= edit_link
                        Else
                            If CRMSource = "CLIENT" Then
                                'edit_link = WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, CLIENT_COMPANY_ID, JETNET_YACHT_ID, True, IIf(ShowingNotes = True, "&n=1&source=CLIENT", "&source=CLIENT"), "Edit") & " - "
                                'inner_text &= edit_link

                                'added MSW - for companies with only client record 
                                If JETNET_COMPANY_ID = 0 And CLIENT_COMPANY_ID > 0 Then
                                    edit_link = CRM_WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, CLIENT_COMPANY_ID, 0, True, IIf(ShowingNotes = True, "&action=edit&type=note&cat_key=0", "&action=edit&type=action&cat_key=0") & "&source=CLIENT&from=aircraftDetails", "Edit") & " - "
                                Else
                                    edit_link = CRM_WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, CLIENT_COMPANY_ID, 0, True, IIf(ShowingNotes = True, "&action=edit&type=note&cat_key=0", "&action=edit&type=action&cat_key=0") & "&source=JETNET&from=aircraftDetails", "Edit") & " - "
                                End If


                                inner_text &= edit_link
                            Else
                                edit_link = CRM_WriteNotesRemindersLinks(r("lnote_id"), JETNET_AC_ID, JETNET_COMPANY_ID, 0, True, IIf(ShowingNotes = True, "&action=edit&type=note&cat_key=0", "&action=edit&type=action&cat_key=0") & "&source=JETNET&from=aircraftDetails", "Edit") & " - "
                                inner_text &= edit_link
                            End If
                        End If
                    End If


                    If DisplayHeaderDate = False Then
                        inner_text += DisplayDate
                    End If

                    If ShowingNotes = True Then         ' if its an notes item 
                        inner_text += r("lnote_user_name").ToString
                        user_link = r("lnote_user_name").ToString
                    ElseIf ShowingActions = True Then   ' if its an action item

                        If Not IsDBNull(r("lnote_schedule_start_date")) Then
                            inner_text += FormatDateTime(r("lnote_schedule_start_date").ToString, DateFormat.LongTime) & " "

                            If is_home_page = False And is_prospector = False Then
                                ReturnString += " - "
                            End If
                        End If

                        If is_home_page = False Then
                            inner_text += r("lnote_user_name").ToString
                        End If

                        If is_prospector = True Then
                            user_link = r("lnote_user_name").ToString
                        End If
                    End If


                    comp_contacts_string = ""
                    'If is_prospector = True And DisplayACInfo = True Then
                    '  Dim Company_Data As New clsClient_Company
                    '  Dim tempData As New DataTable
                    '  Dim tempComp As New clsClient_Company
                    '  Dim tempContact As New DataTable
                    '  Dim Contact_Class_Array As New ArrayList
                    '  inner_text = ""

                    '  comp_contacts_string &= "<table cellspacing='0' cellpadding='0' border='0'><tr valign='top'><td>"

                    '  tempData = aclsData_Temp.GetCompanyInfo_ID(JETNET_COMPANY_ID, "JETNET", 0)
                    '  If Not IsNothing(tempData) Then
                    '    If tempData.Rows.Count > 0 Then
                    '      tempComp = clsGeneral.clsGeneral.Create_Company_Class(tempData, "JETNET", Nothing)

                    '      comp_contacts_string &= crmWebClient.DisplayFunctions.WriteDetailsLink(0, JETNET_COMPANY_ID, 0, 0, True, tempComp.clicomp_name, "", "")

                    '      comp_contacts_string &= "<Br/>" & clsGeneral.clsGeneral.Show_Company_Display(tempComp, False)
                    '    End If
                    '  End If

                    '  If jetnet_contact_id <> 0 Then
                    '    tempContact = aclsData_Temp.GetContacts_Details(jetnet_contact_id, "JETNET")
                    '    If Not IsNothing(tempContact) Then
                    '      If tempContact.Rows.Count > 0 Then
                    '        'Contact_Class_Array = clsGeneral.clsGeneral.Create_Array_Contact_Class(tempContact)
                    '        'For Each Con As clsClient_Contact In Contact_Class_Array
                    '        '  comp_contacts_string &= "</td><td>" & clsGeneral.clsGeneral.Show_Contact_Display(Con)
                    '        'Next
                    '        'better contacts function started 
                    '        ContactFunctions.Display_Contact_Details_label(tempContact, contacts_temp, JETNET_COMPANY_ID, 0, Nothing, True, True, True, "", CRMVIew, "JETNET")
                    '        comp_contacts_string &= "</td><td>"
                    '        comp_contacts_string &= contacts_temp
                    '        'GetContactInfoCompany_No_Query could be used here 
                    '      Else
                    '        comp_contacts_string &= "</td><td>"
                    '      End If
                    '    Else
                    '      comp_contacts_string &= "</td><td>"
                    '    End If
                    '  Else
                    '    comp_contacts_string &= "</td><td>"
                    '  End If

                    '  comp_contacts_string &= "&nbsp;</td></tr><tr><td colspan='2'>"

                    '  '  tempData = Master.aclsData_Temp.GetCompanyInfo_ID(CLIENT_COMPANY_ID, "JETNET", 0)
                    '  '  Company_Data = clsGeneral.clsGeneral.Create_Company_Class(Company_Results, company_source, Preferences_Table)
                    '  '  'Builds the company Display
                    '  ' company_info.Text = Company_Data.clicomp_name & " <br />" & clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)


                    '  '  ReturnString += IIf(AC_Link_Text <> "", " - " & AC_Link_Text & "</a>", "") & " - " & 
                    '  '  ReturnString += " " & " " & IIf(Yacht_Link_Text <> "", Yacht_Link_Text & " - ", "")
                    '  comp_contacts_string &= "" & edit_link & r("lnote_note").ToString & ". "
                    '  If Trim(user_link) <> "" Then
                    '    comp_contacts_string &= " (" & user_link & ")"
                    '  End If
                    '  comp_contacts_string &= "</td></tr></table>"
                    If is_prospector = True Then

                        ' inner_text = ""
                        ' edit_link = Replace(edit_link, ">Edit</a> -", ">Note:</a> ")
                        inner_text = edit_link

                        'lnote_opportunity_status
                        ' lnote_notecat_key
                        If Not IsDBNull(r("oppcat")) Then
                            ' inner_text &= Trim(r("lnote_notecat_key"))
                            If Trim(r("oppcat")) <> "Not Specified" Then
                                inner_text &= Trim(r("oppcat")) & " - "
                            End If
                        End If

                        If Not IsDBNull(r("lnote_opportunity_status")) Then
                            If Trim(r("lnote_opportunity_status")) = "B" Then
                                inner_text &= Trim(r("lnote_opportunity_status"))
                            End If
                        End If

                        'If Trim(r("lnote_status")) = "B" Then
                        '  If JETNET_AC_ID > 0 Or CLIENT_AC_ID > 0 Or CLIENT_MODEL_ID > 0 Or JETNET_MODEL_ID > 0 Then
                        '    inner_text &= " Interested in "
                        '  ElseIf JETNET_COMPANY_ID > 0 Or CLIENT_COMPANY_ID > 0 Then
                        '    inner_text &= " Interest By "
                        '  End If

                        'End If


                        If DisplayACInfo = True Then
                            If Trim(AC_Link_Text) <> "" Then
                                If Trim(r("lnote_status")) = "B" Then
                                    inner_text &= " Interested in "
                                End If
                                inner_text &= " " & AC_Link_Text & ". "
                            End If
                        Else
                            If Trim(COMPANY_Link_Text) <> "" Then
                                If Trim(r("lnote_status")) = "B" Then
                                    inner_text &= " Interest By"
                                End If
                                inner_text &= "</a>&nbsp;&nbsp;<span title=""View Company Details"">" & Replace(COMPANY_Link_Text, "href='#'", " class=""noCase emphasisColor text_underline"" href=""#""") & "</a>. "
                            End If
                        End If

                        If Trim(Yacht_Link_Text) <> "" Then
                            If Trim(AC_Link_Text) <> "" Then
                                inner_text &= " - "
                            End If
                            inner_text &= " " & IIf(Yacht_Link_Text <> "", Yacht_Link_Text & " - ", "") & ". "
                        End If


                        inner_text &= "</a>"


                        '1,000 thing ::: test

                        If Not IsDBNull(r("lnote_note")) Then
                            If InStr(r("lnote_note"), " ::: ") > 0 Then
                                Dim splTemp As String() = Split(r("lnote_note"), " ::: ")
                                If UBound(splTemp) > 0 Then
                                    inner_text &= "<span class=""gray_text"">" & edit_link & splTemp(0) & "</a></span> " & splTemp(1) & "."
                                Else
                                    inner_text &= r("lnote_note").ToString & "."
                                End If
                            Else
                                inner_text &= r("lnote_note").ToString & "."
                            End If
                        End If


                        If Not IsDBNull(r("lnote_cash_value")) Then
                            If r("lnote_cash_value") > 0 Then
                                inner_text &= " $" & Trim(r("lnote_cash_value")) & " Opportunity"
                            End If
                        End If

                        If Not IsDBNull(r("lnote_capture_percentage")) Then
                            If r("lnote_capture_percentage") > 0 Then
                                inner_text &= " [" & Trim(r("lnote_capture_percentage")) & "% Probability]"
                            End If
                        End If


                        If Not IsDBNull(r("lnote_cash_value")) Or Not IsDBNull(r("lnote_capture_percentage")) Then
                            inner_text &= "."
                        End If

                        If Trim(user_link) <> "" Then
                            inner_text &= " <em>Assigned To: " & user_link & "</em> "
                        End If

                    Else
                        inner_text &= IIf(AC_Link_Text <> "", " " & AC_Link_Text & "</a>", "") & " - " & IIf(COMPANY_Link_Text <> "", COMPANY_Link_Text & " - ", "") & " " & " " & IIf(Yacht_Link_Text <> "", Yacht_Link_Text & " - ", "") & IIf(ShortenNotes = False Or ShowingNotes = False, r("lnote_note").ToString, IIf(Len(r("lnote_note")) > 100, Left(r("lnote_note").ToString, 100) & "..", r("lnote_note").ToString)) & "."
                    End If


                    ReturnString &= inner_text & comp_contacts_string
                    inner_text = ""

                    If useUL = True Then
                        ReturnString += "</li>"
                    Else
                        ReturnString += "</span></td></tr>"
                    End If
                    oldweekdis = week
                    oldmonthint = monthint
                    olddaydis = daydis
                    'End If
                Next

                ReturnString += "</ul>"
                ReturnString += " </td>"
                ReturnString += " </tr>"
                ReturnString += "</table>"
            Else
                ReturnString = "<table width=""100%"" cellpadding=""" & IIf(useUL = True, "3", "5") & """ cellspacing=""0"" class=""data_aircraft_grid""><tr><td align='left' valign='top'  class=""noBorder"">"
                ReturnString += "<span>No current " & IIf(is_prospector, "prospects/opportunities", IIf(ShowingActions = True, "action items", "notes")) & " available for display.</span>"
                ReturnString += "</td></tr></table>"
            End If
        End If

        Return ReturnString
    End Function

    Public Shared Function ReturnModel(ByVal modelID As Long, ByVal modelSource As String, ByVal aclsData_Temp As clsData_Manager_SQL)
        Dim tempTable As New DataTable
        Dim returnString As String = ""

        If UCase(modelSource) = "CLIENT" Then
            tempTable = aclsData_Temp.Get_Clients_Aircraft_Model_amodID(modelID)
        Else
            tempTable = aclsData_Temp.GetJetnetModelInfo(modelID, False, "")
        End If

        If Not IsNothing(tempTable) Then
            If tempTable.Rows.Count > 0 Then

                If UCase(modelSource) = "CLIENT" Then
                    returnString = tempTable.Rows(0).Item("cliamod_make_name") & " " & tempTable.Rows(0).Item("cliamod_model_name")
                Else
                    returnString = tempTable.Rows(0).Item("amod_make_name") & " " & tempTable.Rows(0).Item("amod_model_name")
                End If
            End If
        End If

        Return returnString
    End Function

    Public Shared Function make_MTREND_CHANGE_TICKERS(ByVal amod_id As Long, ByVal timespan As Integer, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal ac_for_sale As Long, ByVal days As Long, ByVal forsaleavg As String, ByVal values_in_op As Double, ByVal total_inop As Long, Optional ByVal make_just_this_chart As String = "", Optional ByVal amod_id_list As String = "")
        make_MTREND_CHANGE_TICKERS = ""

        Dim rtable As New DataTable
        Dim htmlout As New StringBuilder

        Dim for_sale_up_down_value As Long = 0
        Dim for_Sale_up_down_percent As Double = 0
        Dim avg_asking_up_down_value As Long = 0
        Dim avg_asking_percent As Double = 0
        Dim dom_up_down_value As Long = 0
        Dim dom_percent As Double = 0
        Dim current_for_sale As Long = 0

        Dim fs_1 As Long = 0
        Dim sold_1 As Long = 0
        Dim avgasking_1 As Long = 0
        Dim avgsale_1 As Long = 0
        Dim avgdom_1 As Long = 0

        Dim fs_6 As Long = 0
        Dim sold_6 As Long = 0
        Dim avgasking_6 As Long = 0
        Dim avgsale_6 As Long = 0
        Dim avgdom_6 As Long = 0
        Dim inop_count_6 As Double = 0
        Dim in_op_percent As Double = 0
        Dim in_op_value As Double = 0
        Dim avg_days_count As Double = 0
        Dim models_found As Integer = 0

        Try

            rtable = aclsData_Temp.GET_MTREND_CHANGES(amod_id, timespan, amod_id_list)


            If rtable.Rows.Count > 0 Then

                For Each r As DataRow In rtable.Rows

                    If Not IsDBNull(r.Item("THELABEL")) Then
                        If Trim(r.Item("THELABEL")) = "LAST MONTH" Then  '  ignore the 6 months ago, it is now using live 
                            '  'fs_1 = r.Item("FORSALE")
                            '  'sold_1 = r.Item("NUMSOLD")
                            '  'avgasking_1 = r.Item("AVGASKPRICE")
                            '  'avgsale_1 = r.Item("AVGSALEPRICE")
                            '  'avgdom_1 = r.Item("AVGDAYSONMARKET")

                            '  fs_1 = ac_for_sale
                            '  sold_1 = 0
                            '  avgasking_1 = forsaleavg
                            '  avgsale_1 = 0
                            '  avgdom_1 = days
                        Else
                            fs_6 = r.Item("FORSALE")
                            sold_6 = r.Item("NUMSOLD")
                            avgasking_6 = r.Item("AVGASKPRICE")
                            avgasking_6 = (avgasking_6 / 1000)
                            avgsale_6 = r.Item("AVGSALEPRICE")
                            avgdom_6 = r.Item("AVGDAYSONMARKET")
                            inop_count_6 = r.Item("IN_OP_COUNT")
                            avg_days_count = r.Item("AVGDOM")
                            models_found += 1
                        End If
                    End If

                    '  End If
                    '  End If

                    'If timespan = 12 Then
                    'If Trim(r.Item("THELABEL")) = "1 YEAR AGO" Then



                    'If timespan = 6 Then
                    '  If Trim(r.Item("THELABEL")) = "6 MONTHS AGO" Then
                    '    fs_6 = r.Item("FORSALE")
                    '    sold_6 = r.Item("NUMSOLD")
                    '    avgasking_6 = r.Item("AVGASKPRICE")
                    '    avgasking_6 = (avgasking_6 / 1000)
                    '    avgsale_6 = r.Item("AVGSALEPRICE")
                    '    avgdom_6 = r.Item("AVGDAYSONMARKET")  ' this is for avg days for sold ac 
                    '    inop_count_6 = r.Item("IN_OP_COUNT")
                    '    avg_days_count = r.Item("AVGDOM")  ' this is for avg days historical
                    '  End If
                    'End If


                    'If timespan = 24 Then
                    '  If Trim(r.Item("THELABEL")) = "2 YEARS AGO" Then
                    '    fs_6 = r.Item("FORSALE")
                    '    sold_6 = r.Item("NUMSOLD")
                    '    avgasking_6 = r.Item("AVGASKPRICE")
                    '    avgasking_6 = (avgasking_6 / 1000)
                    '    avgsale_6 = r.Item("AVGSALEPRICE")
                    '    avgdom_6 = r.Item("AVGDAYSONMARKET")  ' this is for avg days for sold ac 
                    '    inop_count_6 = r.Item("IN_OP_COUNT")
                    '    avg_days_count = r.Item("AVGDOM")  ' this is for avg days historical
                    '  End If
                    'End If

                    'If timespan = 36 Then
                    '  If Trim(r.Item("THELABEL")) = "3 YEARS AGO" Then
                    '    fs_6 = r.Item("FORSALE")
                    '    sold_6 = r.Item("NUMSOLD")
                    '    avgasking_6 = r.Item("AVGASKPRICE")
                    '    avgasking_6 = (avgasking_6 / 1000)
                    '    avgsale_6 = r.Item("AVGSALEPRICE")
                    '    avgdom_6 = r.Item("AVGDAYSONMARKET")  ' this is for avg days for sold ac 
                    '    inop_count_6 = r.Item("IN_OP_COUNT")
                    '    avg_days_count = r.Item("AVGDOM")  ' this is for avg days historical
                    '  End If
                    'End If




                Next
            End If


            'If models_found > 1 Then
            '  fs_6 = (fs_6 / models_found)
            '  sold_6 = (sold_6 / models_found)
            '  avgasking_6 = (avgasking_6 / models_found)
            '  avgasking_6 = (avgasking_6 / models_found)
            '  avgsale_6 = (avgsale_6 / models_found)
            '  avgdom_6 = (avgdom_6 / models_found)
            '  inop_count_6 = (inop_count_6 / models_found)
            '  avg_days_count = (avg_days_count / models_found)
            'End If

            fs_1 = ac_for_sale
            sold_1 = 0
            avgasking_1 = forsaleavg
            avgsale_1 = 0
            avgdom_1 = days

            If fs_1 > fs_6 Then
                ' 36 - 29 = 7
                for_sale_up_down_value = (fs_1 - fs_6)
                for_Sale_up_down_percent = ((fs_1 - fs_6) / fs_6 * 100)
            ElseIf fs_1 < fs_6 Then
                ' 36 - 29 = 7
                for_sale_up_down_value = -(fs_6 - fs_1)
                for_Sale_up_down_percent = ((fs_6 - fs_1) / fs_6 * 100)
            Else
                for_sale_up_down_value = 0
                for_Sale_up_down_percent = 0
            End If


            If avgasking_1 > avgasking_6 Then
                ' 36 - 29 = 7
                If (avgasking_6 + 10) > avgasking_1 Then 'if numbers are within a thousand
                    avg_asking_up_down_value = 0
                    avg_asking_percent = 0
                Else
                    avg_asking_up_down_value = ((avgasking_1 - avgasking_6) * 1000)
                    avg_asking_percent = ((avgasking_1 - avgasking_6) / avgasking_6 * 100)
                End If
            ElseIf avgasking_1 < avgasking_6 Then
                ' 36 - 29 = 7
                If (avgasking_1 + 10) > avgasking_6 Then 'if numbers are within a thousand
                    avg_asking_up_down_value = 0
                    avg_asking_percent = 0
                Else
                    avg_asking_up_down_value = -((avgasking_6 - avgasking_1) * 1000)
                    avg_asking_percent = ((avgasking_6 - avgasking_1) / avgasking_1 * 100)
                End If
            Else
                avg_asking_up_down_value = 0
                avg_asking_percent = 0
            End If


            'If avgdom_1 > avgdom_6 Then
            '  ' 36 - 29 = 7
            '  dom_up_down_value = (avgdom_1 - avgdom_6)
            '  dom_percent = ((avgdom_1 - avgdom_6) / avgdom_6 * 100)
            'ElseIf avgdom_1 < avgdom_6 Then
            '  ' 36 - 29 = 7
            '  dom_up_down_value = -(avgdom_6 - avgdom_1)
            '  dom_percent = ((avgdom_6 - avgdom_1) / avgdom_1 * 100)
            'Else
            '  dom_up_down_value = 0
            '  dom_percent = 0
            'End If

            If avgdom_1 > avg_days_count Then
                ' 36 - 29 = 7
                dom_up_down_value = (avgdom_1 - avg_days_count)
                dom_percent = ((avgdom_1 - avg_days_count) / avg_days_count * 100)
            ElseIf avgdom_1 < avg_days_count Then
                ' 36 - 29 = 7
                dom_up_down_value = -(avg_days_count - avgdom_1)
                dom_percent = ((avg_days_count - avgdom_1) / avgdom_1 * 100)
            Else
                dom_up_down_value = 0
                dom_percent = 0
            End If


            inop_count_6 = ((fs_6 / inop_count_6) * 100)

            If values_in_op > inop_count_6 Then
                in_op_value = (values_in_op - inop_count_6)
                in_op_percent = ((values_in_op - inop_count_6) / values_in_op * 100)
            ElseIf values_in_op < inop_count_6 Then
                in_op_value = -(inop_count_6 - values_in_op)
                in_op_percent = ((inop_count_6 - values_in_op) / values_in_op * 100)
            Else
                in_op_percent = 0
                in_op_value = 0
            End If


            If Trim(make_just_this_chart) <> "" Then
                If Trim(make_just_this_chart) = "forsale" Then
                    htmlout.Append(DisplayFunctions.make_ticker_box("For Sale (" & timespan & " Months)", for_sale_up_down_value, for_Sale_up_down_percent, fs_1 & " FOR SALE", False, False))  ' current_for_sale 
                ElseIf Trim(make_just_this_chart) = "avgasking" Then
                    htmlout.Append(DisplayFunctions.make_ticker_box("Avg Asking (" & timespan & " Months)", avg_asking_up_down_value, avg_asking_percent, "", True, False))  ' "$" & FormatNumber((avgasking_1 / 1000), 0) & "k"
                ElseIf Trim(make_just_this_chart) = "percentforsale" Then
                    htmlout.Append(DisplayFunctions.make_ticker_box("% For Sale (" & timespan & " Months)", in_op_value, 0, FormatNumber(((ac_for_sale / total_inop) * 100), 1) & "% FOR SALE", False, True))  ' current_for_sale 
                ElseIf Trim(make_just_this_chart) = "daysonmarket" Then
                    htmlout.Append(DisplayFunctions.make_ticker_box("Days On Market (" & timespan & " Months)", dom_up_down_value, dom_percent, avgdom_1 & " DAYS", False, False))
                End If
            Else
                ' htmlout.Append("<table width='100%'>")
                htmlout.Append(DisplayFunctions.make_ticker_box("For Sale (" & timespan & " Months)", for_sale_up_down_value, for_Sale_up_down_percent, fs_1 & " FOR SALE", False, False))  ' current_for_sale 

                htmlout.Append(DisplayFunctions.make_ticker_box("Avg Asking (" & timespan & " Months)", avg_asking_up_down_value, avg_asking_percent, "", True, False))  ' "$" & FormatNumber((avgasking_1 / 1000), 0) & "k"

                htmlout.Append(DisplayFunctions.make_ticker_box("% For Sale (" & timespan & " Months)", in_op_value, 0, FormatNumber(((ac_for_sale / total_inop) * 100), 1) & "%", False, True))  ' current_for_sale 

                htmlout.Append(DisplayFunctions.make_ticker_box("Days On Market (" & timespan & " Months)", dom_up_down_value, dom_percent, avgdom_1 & " DAYS", False, False))
                'htmlout.Append("</table>")
            End If



            make_MTREND_CHANGE_TICKERS = htmlout.ToString

        Catch ex As Exception

        End Try

    End Function

    Public Shared Function make_ticker_box_growth(ByVal label_string As String, ByVal value_temp As Double, ByVal last_month As String, ByVal current_val As String, ByVal is_dollar_value As Boolean, ByVal is_percent_value As Boolean) As String
        Dim bgcolor1 As String = ""
        Dim ReturnString As String = ""
        Dim CssClass As String = "TrendBox NoBG" 'Swap from "TrendBox" to "TrendBox NoBG" to toggle background.
        Dim boxArrow As String = ""
        '<link rel=""stylesheet"" href=""/EvoStyles/stylesheets/additional_styles.css"" />

        If value_temp > 0 Then
            CssClass += " GreenTrend"
            boxArrow = "<i class=""fa fa-arrow-up"" aria-hidden=""true""></i>"
        ElseIf value_temp < 0 Then
            CssClass += " RedTrend"
            boxArrow = "<i class=""fa fa-arrow-down"" aria-hidden=""true""></i>"
        ElseIf value_temp = 0 Then
            boxArrow = "<i class=""fa fa-arrows-h"" aria-hidden=""true""></i>"

        End If

        ReturnString = "<div class='" & CssClass & "'>"
        ReturnString += "<span class=""trendLabel"">" & label_string & "</span><span class=""trendArrow"">" & boxArrow & "</span>"
        ReturnString += "<span class=""trendLarge"">"

        If is_dollar_value = True Then
            If IsNumeric(value_temp) Then
                value_temp = (value_temp / 1000)
                If value_temp > 0 Then
                    ReturnString += ("$" & FormatNumber(value_temp.ToString, 0) & "k")
                Else
                    ReturnString += ("-$" & FormatNumber(value_temp * -1, 0) & "k")
                End If
            Else
                ReturnString += ("" & value_temp.ToString & "")
            End If
        ElseIf is_percent_value = True Then
            If InStr(Trim(UCase(value_temp)), "INF") > 0 Then
                ReturnString += ("")
            Else
                ReturnString += ("" & FormatNumber(value_temp.ToString, 1) & " ")
            End If
        Else
            ReturnString += ("" & value_temp.ToString & "")
        End If

        ReturnString += "</span>"
        ReturnString += "<span class=""trendSmall"">"

        If String.IsNullOrEmpty(current_val) Then
            ReturnString += ("<span>&nbsp;</span>") 'This needs an empty span tag if there is no value to display here. This means the first number should sink to the bottom anyhow. This will help that.
        End If

        If Not String.IsNullOrEmpty(current_val) Then
            ReturnString += ("<span>" & current_val.ToString & "</span>")
        End If

        ReturnString += ("<span>" & last_month.ToString & "</span>")

        ReturnString += "</span>"

        ReturnString += "</div>"

        Return ReturnString

    End Function

    Public Shared Function make_ticker_box(ByVal label_string As String, ByVal value_temp As Double, ByVal percent_val As Double, ByVal current_val As String, ByVal is_dollar_value As Boolean, ByVal is_percent_value As Boolean) As String
        Dim bgcolor1 As String = ""
        Dim ReturnString As String = ""
        Dim CssClass As String = "TrendBox NoBG" 'Swap from "TrendBox" to "TrendBox NoBG" to toggle background.
        Dim boxArrow As String = ""
        '<link rel=""stylesheet"" href=""/EvoStyles/stylesheets/additional_styles.css"" />

        If value_temp > 0 And InStr(label_string, "Avg Asking") > 0 Then
            CssClass += " GreenTrend"
            boxArrow = "<i class=""fa fa-arrow-up"" aria-hidden=""true""></i>"
        ElseIf value_temp < 0 And InStr(label_string, "Avg Asking") > 0 Then  ' avg asking go, up down
            CssClass += " RedTrend"
            boxArrow = "<i class=""fa fa-arrow-down"" aria-hidden=""true""></i>"
        ElseIf value_temp > 0 Then
            CssClass += " RedTrend"
            boxArrow = "<i class=""fa fa-arrow-up"" aria-hidden=""true""></i>" ' and down is good 
        ElseIf value_temp < 0 Then
            CssClass += " GreenTrend"
            boxArrow = "<i class=""fa fa-arrow-down"" aria-hidden=""true""></i>" ' otherwise, up is bad
        ElseIf value_temp = 0 Then
            boxArrow = "<i class=""fa fa-arrows-h"" aria-hidden=""true""></i>"

        End If

        ReturnString = "<div class='" & CssClass & "'>"
        ReturnString += "<span class=""trendLabel"">" & label_string & "</span><span class=""trendArrow"">" & boxArrow & "</span>"
        ReturnString += "<span class=""trendLarge"">"

        If is_dollar_value = True Then
            If IsNumeric(value_temp) Then
                value_temp = (value_temp / 1000)
                If value_temp > 0 Then
                    ReturnString += ("$" & FormatNumber(value_temp.ToString, 0) & "k")
                Else
                    ReturnString += ("-$" & FormatNumber(value_temp * -1, 0) & "k")
                End If
            Else
                ReturnString += ("" & value_temp.ToString & "")
            End If
        ElseIf is_percent_value = True Then
            If InStr(Trim(UCase(value_temp)), "INF") > 0 Then
                ReturnString += ("")
            Else
                ReturnString += ("" & FormatNumber(value_temp.ToString, 1) & "%")
            End If
        Else
            ReturnString += ("" & value_temp.ToString & "")
        End If

        ReturnString += "</span>"
        ReturnString += "<span class=""trendSmall"">"

        If String.IsNullOrEmpty(current_val) Then
            ReturnString += ("<span>&nbsp;</span>") 'This needs an empty span tag if there is no value to display here. This means the first number should sink to the bottom anyhow. This will help that.
        End If

        'if its not a percent value by itself 
        If value_temp > 0 And is_percent_value = False Then
            If InStr(Trim(UCase(percent_val.ToString)), "INF") > 0 Then
                ReturnString += ("<span>&nbsp;</span>")
            Else
                ReturnString += ("<span>" & FormatNumber(percent_val.ToString, 0) & "%</span>")
            End If
        ElseIf value_temp < 0 And is_percent_value = False Then
            If InStr(Trim(UCase(percent_val.ToString)), "INF") > 0 Then
                ReturnString += ("<span>&nbsp;</span>")
            Else
                ReturnString += ("<span>-" & FormatNumber(percent_val.ToString, 0) & "%</span>")
            End If

        Else
            'See the note about the empty span tag above.
            ReturnString += ("<span>&nbsp;</span>")
        End If

        If Not String.IsNullOrEmpty(current_val) Then
            ReturnString += ("<span>" & current_val.ToString & "</span>")
        End If
        ReturnString += "</span>"

        ReturnString += "</div>"

        Return ReturnString

    End Function

    Public Shared Function ConvertDataTableToArrayCombinedFields(ByVal dt As DataTable, ByRef ColumnDataNames As String, ByVal searchCriteria As viewSelectionCriteriaClass, ByVal displaySerNoMenu As Boolean, ByVal Airport_ID_OVERALL As Long, ByVal toggleViewLinkOff As Boolean) As String
        Dim html As New StringBuilder
        Dim utilizationData As New utilization_functions

        ' ORIGINAL COLUMN LIST 
        'ColumnDataNames += "{ title: ""SEL"", data: ""SEL""}, "
        'ColumnDataNames += "{ title: ""Aircraft"", data: ""Aircraft""}, "
        'ColumnDataNames += "{ title: ""Ser#"", data: ""Serial Number""}, "
        'ColumnDataNames += "{ title: ""Reg#"", data: ""Registration Number""}, "


        'ColumnDataNames += "{ title: ""Date"", data: {"
        'ColumnDataNames += "_:    ""FLIGHT_DATE.0"","
        'ColumnDataNames += "sort: ""FLIGHT_DATE.1"""
        'ColumnDataNames += "} },"
        'ColumnDataNames += "{ title: ""Origin Airport"", data: ""Origin Airport""}, "
        'ColumnDataNames += "{ title: ""Destination Airport"", data: ""Destination Airport""}, "
        'ColumnDataNames += "{ title: ""Flight Time"", data: ""Flight Time""}, "
        'ColumnDataNames += "{ title: ""Distance (nm)"", data: ""Distance""}, "
        'ColumnDataNames += "{ title: ""EST Fuel Burn"", data: ""Est Fuel Burn""}, "
        'ColumnDataNames += "{ title: ""Operator"", data: {_: ""Operator.0"", sort: ""Operator.1""}}, "
        'ColumnDataNames += "{ title: ""Operator City"", data: ""Operator City""}, "
        'ColumnDataNames += "{ title: ""Email"", data: {_: ""Email.0"", sort: ""Email.1""}}, "
        'ColumnDataNames += "{ title: ""Office Phone"", data: ""Office Phone""}, "



        ColumnDataNames += "{ title: ""SEL"", data: ""SEL""}, "
        ColumnDataNames += "{ title: ""Make"", data: ""Make""}, "
        ColumnDataNames += "{ title: ""Model"", data: ""Model""}, "
        ColumnDataNames += "{ title: ""Ser#"", data: ""Serial Number""}, "
        ColumnDataNames += "{ title: ""Reg#"", data: ""Registration Number""}, "

        ColumnDataNames += "{ title: ""Date"", data: {"
        ColumnDataNames += "_:    ""FLIGHT_DATE.0"","
        ColumnDataNames += "sort: ""FLIGHT_DATE.1"""
        ColumnDataNames += "} },"

        ColumnDataNames += "{ title: ""Departure Airport"", data: ""Origin Airport""}, "
        ColumnDataNames += "{ title: ""Arrival Airport"", data: ""Destination Airport""}, "

        ColumnDataNames += "{ title: ""Flight Time"",className: ""text_align_right"", data: ""Flight Time""}, "
        ColumnDataNames += "{ title: ""Distance"",className: ""text_align_right"", data: ""Distance""}, "

        ColumnDataNames += "{ title: ""Total EST<br/>Fuel Burn"", width: ""50px"",className: ""text_align_right"", data: ""Est Fuel Burn""}, "

        ColumnDataNames += "{ title: ""Operator"", data:{_:""Operator.1"", sort: ""Operator.1""}, width: ""60px"", className: ""display_none"" }, "

        ColumnDataNames += "{ title: ""Operator"", data: {_: ""Operator.0"", sort: ""Operator.1""}}, "
        ColumnDataNames += "{ title: ""Operator City"", data: ""Operator City""}, "
        ColumnDataNames += "{ title: ""Email"", data: {_: ""Email.0"", sort: ""Email.1""}}, "
        ColumnDataNames += "{ title: ""Office Phone"", data: ""Office Phone""}, "


        ColumnDataNames += "{ title: ""Base"", data: ""Base""}, "
        ColumnDataNames += "{ title: ""Base City"", data: ""BaseCity""}, "
        ColumnDataNames += "{ title: ""Base State"", data: ""BaseState""}, "
        ColumnDataNames += "{ title: ""Base Country"", data: ""BaseCountry""}, "
        ColumnDataNames += "{ title: ""Base IATA"", data: ""BaseIATA""}, "
        ColumnDataNames += "{ title: ""Base ICAO"", data: ""BaseICAO""}, "

        '  ColumnDataNames += "{ title: ""Nbr Flights"", data: ""Flights""}, "
        '  ColumnDataNames += "{ title: ""Flights/Mo"", data: ""Flights_Month""}, "

        ColumnDataNames += "{ title: ""Departure City"", data: ""OriginCity""}, "
        ColumnDataNames += "{ title: ""Departure State"", data: ""OriginState""}, "
        ColumnDataNames += "{ title: ""Departure Country"", data: ""OriginCountry""}, "
        ColumnDataNames += "{ title: ""Departure Continent"", data: ""OriginContinent""}, "

        ColumnDataNames += "{ title: ""Arrival City"", data: ""DestinationCity""}, "
        ColumnDataNames += "{ title: ""Arrival State"", data: ""DestinationState""}, "
        ColumnDataNames += "{ title: ""Arrival Country"", data: ""DestinationCountry""}, "
        ColumnDataNames += "{ title: ""Arrival Continent"", data: ""DestinationContinent""}, "

        ColumnDataNames += "{ title: ""Size Category"", data: ""SizeCategory""}, "



        ' ColumnDataNames += "{ title: ""Avg Min<br/>Per Flight"", width: ""50px"",className: ""text_align_right"", data:""AvgMinPerFlights"" }, "
        '  ColumnDataNames += "{ title: ""Avg Fuel Burn<br/>(GAL) Per Flight"", width: ""80px"",className: ""text_align_right"", data:""TotalFuelBurnPerFlight"" }, "



        For Each r As DataRow In dt.Rows

            If Not String.IsNullOrEmpty(html.ToString) Then
                html.Append(",")
            End If

            html.Append("{")
            html.Append("""SEL"": """",")

            html.Append("""Make"": """)
            If Not IsDBNull(r("MAKE")) Then
                html.Append(r("MAKE"))
            End If
            html.Append(""",")

            html.Append("""Model"": """)
            If Not IsDBNull(r("MODEL")) Then
                html.Append(" " & r("MODEL"))
            End If
            html.Append(""",")


            If Not IsDBNull(r("SERNBR")) Then

                If displaySerNoMenu Then
                    html.Append("""Serial Number"": ""<ul class='cssMenu'><li><a href='#' class='expand_more'>" & r("SERNBR").ToString & "</a><ul>")
                    html.Append("<li><a class='underline' href='#' onclick=\""javascript:load('DisplayAircraftDetail.aspx?acid=" & r("AC_ID").ToString & "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;\""><img src='/images/aircraftDetailsDropdown.jpg' width='100%' /></a></li>") '& "<a class='underline' onclick=\""javascript:openSmallWindowJS('DisplayAircraftDetail.aspx?acid=" + r("AC_ID").ToString + "&jid=0','AircraftDetails');\"" title='Display Aircraft Details'><img src='/images/aircraftDetailsDropdown.jpg' width='100%' /></a></li>")
                    html.Append("<li><a class='underline' onclick=\""javascript:load('FAAFlightData.aspx?acid=" + r("AC_ID").ToString + "&jid=0")

                    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                        html.Append("&start_date=" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate))
                        html.Append("&end_date=" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate))
                    End If
                    html.Append("','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;\""><img src='/images/flightActivityDropdown.jpg'  width='100%'/></a></li>")
                    html.Append("</ul></li></ul>"",")
                Else
                    html.Append("""Serial Number"": """ & r("SERNBR").ToString & """,")
                End If
            Else
                html.Append("""Serial Number"": """",")
            End If

            If Not IsDBNull(r("REGNBR")) Then
                html.Append("""Registration Number"": """ & r("REGNBR") & """,")
            Else
                html.Append("""Registration Number"": """",")
            End If

            If Not IsDBNull(r("FLIGHT DATE")) Then
                html.Append("""FLIGHT_DATE"": [""" & FormatDateTime(r("FLIGHT DATE"), DateFormat.GeneralDate) & """,""" & Format(r("FLIGHT DATE"), "yyyy-MM-dd HH:mm:ss") & """],")
            Else
                html.Append("""FLIGHT_DATE"":[ """",""""],")
            End If


            html.Append("""Origin Airport"": """)


            If Not IsDBNull(r("ORIGIN NAME")) Then
                html.Append(r("ORIGIN NAME"))
                html.Append(" ")
            End If

            If Not IsDBNull(r("ORIGIN CODE")) Then
                html.Append("<br/>")
                If toggleViewLinkOff = False Then
                    html.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ORIGIN_ID").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                End If
                html.Append(r("ORIGIN CODE"))
                If toggleViewLinkOff = False Then
                    html.Append("</a>")
                End If
            Else
                html.Append("")
            End If


            html.Append(""",")


            html.Append("""OriginCity"": """)

            If Not IsDBNull(r("ORIGIN CITY")) Then
                html.Append(r("ORIGIN CITY"))
            End If
            html.Append(""",")


            html.Append("""OriginState"": """)

            If Not IsDBNull(r("ORIGIN STATE")) Then
                html.Append(r("ORIGIN STATE"))
            End If
            html.Append(""",")

            html.Append("""OriginCountry"": """)

            If Not IsDBNull(r("ORIGIN COUNTRY")) Then
                html.Append(r("ORIGIN COUNTRY"))
            End If
            html.Append(""",")

            html.Append("""OriginContinent"": """)

            If Not IsDBNull(r("origin_continent")) Then
                html.Append(r("origin_continent"))
            End If
            html.Append(""",")


            html.Append("""Destination Airport"": """)
            If Not IsDBNull(r("DEST NAME")) Then
                html.Append(r("DEST NAME"))
                html.Append(" ")
            End If

            If Not IsDBNull(r("DEST CODE")) Then
                html.Append("<br/>")
                If toggleViewLinkOff = False Then
                    html.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("DEST_ID").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>" & r("DEST CODE") & "</a> - ")
                End If
                html.Append(r("DEST CODE"))
                If toggleViewLinkOff = False Then
                    html.Append("</a>")
                End If
            Else
                html.Append("")
            End If

            html.Append(""",")




            'ColumnDataNames += "{ title: ""Destination City"", data: ""DestinationCity""}, "
            'ColumnDataNames += "{ title: ""Destination State"", data: ""DestinationState""}, "
            'ColumnDataNames += "{ title: ""Destination Country"", data: ""DestinationCountry""}, "

            html.Append("""DestinationCity"": """)

            If Not IsDBNull(r("DEST CITY")) Then
                html.Append(r("DEST CITY"))
            End If
            html.Append(""",")


            html.Append("""DestinationState"": """)

            If Not IsDBNull(r("DEST STATE")) Then
                html.Append(r("DEST STATE"))
            End If
            html.Append(""",")

            html.Append("""DestinationCountry"": """)

            If Not IsDBNull(r("DEST COUNTRY")) Then
                html.Append(r("DEST COUNTRY"))
            End If
            html.Append(""",")

            html.Append("""DestinationContinent"": """)

            If Not IsDBNull(r("dest_continent")) Then
                html.Append(r("dest_continent"))
            End If
            html.Append(""",")



            If Not IsDBNull(r("FLIGHT TIME")) Then
                html.Append("""Flight Time"": """ & r("FLIGHT TIME") & """,")
            Else
                html.Append("""Flight Time"": """",")
            End If

            If Not IsDBNull(r("DISTANCE")) Then
                html.Append("""Distance"": """ & r("DISTANCE") & """,")
            Else
                html.Append("""Distance"": """",")
            End If

            If Not IsDBNull(r("ESTFUELBURN")) Then
                html.Append("""Est Fuel Burn"": """ & r("ESTFUELBURN") & """,")
            Else
                html.Append("""Est Fuel Burn"": """",")
            End If

            If Not IsDBNull(r("OPERATOR")) Then
                html.Append("""Operator"": [""")
                If toggleViewLinkOff = False Then
                    html.Append("<ul class='cssMenu'><li><a href='#' class='expand_more'>" & r("OPERATOR").ToString & "</a><ul>")
                    html.Append("<li><a class='underline' href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization" & IIf(Airport_ID_OVERALL > 2, "&aport_id=" & Airport_ID_OVERALL.ToString, "&aport_id=0") & "&comp_id=" & r("COMP_ID") & "' title='Select Operator'>Select Operator</a></li>")
                    html.Append("<li><a href='#' onclick=\""javascript:load('DisplayCompanyDetail.aspx?compid=" & r("COMP_ID") & "','','scrollbars=yes,menubar=no,height=900,width=1090,resizable=yes,toolbar=no,location=no,status=no');return false;\"">View Operator Profile</a></li>")
                    html.Append("</ul></li></ul>")
                Else
                    html.Append("<a href='#' class='tiny_text text_underline' onclick=\""javascript:load('DisplayCompanyDetail.aspx?compid=" & r("COMP_ID") & "','','scrollbars=yes,menubar=no,height=900,width=1090,resizable=yes,toolbar=no,location=no,status=no');return false;\"">" & r("OPERATOR") & "</a>")
                End If

                html.Append(""", """ & r("OPERATOR") & """], ")
            Else
                html.Append("""Operator"": ["""",""""],")
            End If

            If Not IsDBNull(r("CITY")) Then
                html.Append("""Operator City"": """ & r("CITY") & """,")
            Else
                html.Append("""Operator City"": """",")
            End If



            If Not IsDBNull(r("EMAIL")) Then
                html.Append("""Email"": [""<a href='mailto:" & r("EMAIL") & "'>" & r("EMAIL") & "</a>"", """ & r("EMAIL") & """],")
            Else
                html.Append("""Email"": ["""", """"],")
            End If

            If Not IsDBNull(r("OFFICE PHONE")) Then
                html.Append("""Office Phone"": """ & r("OFFICE PHONE") & """,")
            Else
                html.Append("""Office Phone"": """",")
            End If


            If Not IsDBNull(r("base_aport_name")) Then
                html.Append("""Base"": """ & r("base_aport_name") & """,")
            Else
                html.Append("""Base"": """",")
            End If

            If Not IsDBNull(r("base_aport_city")) Then
                html.Append("""BaseCity"": """ & r("base_aport_city") & """,")
            Else
                html.Append("""BaseCity"": """",")
            End If

            If Not IsDBNull(r("base_aport_state")) Then
                html.Append("""BaseState"": """ & r("base_aport_state") & """,")
            Else
                html.Append("""BaseState"": """",")
            End If

            If Not IsDBNull(r("base_aport_country")) Then
                html.Append("""BaseCountry"": """ & r("base_aport_country") & """,")
            Else
                html.Append("""BaseCountry"": """",")
            End If

            If Not IsDBNull(r("base_aport_iata_code")) Then
                html.Append("""BaseIATA"": """ & r("base_aport_iata_code") & """,")
            Else
                html.Append("""BaseIATA"": """",")
            End If

            If Not IsDBNull(r("base_aport_icao_code")) Then
                html.Append("""BaseICAO"": """ & r("base_aport_icao_code") & """,")
            Else
                html.Append("""BaseICAO"": """",")
            End If

            If Not IsDBNull(r("amjiqs_cat_desc")) Then
                html.Append("""SizeCategory"": """ & r("amjiqs_cat_desc") & """,")
            Else
                html.Append("""SizeCategory"": """",")
            End If


            'If Not IsDBNull(r("FIRST NAME")) Then
            '  html.append("""FIRST NAME"": """ & r("FIRST NAME") & ""","
            'Else
            '  html.append("""FIRST NAME"": """","
            'End If

            'If Not IsDBNull(r("LAST NAME")) Then
            '  html.append("""LAST NAME"": """ & r("LAST NAME") & ""","
            'Else
            '  html.append("""LAST NAME"": """","
            'End If

            'If Not IsDBNull(r("TITLE")) Then
            '  html.append("""TITLE"": """ & r("TITLE") & ""","
            'Else
            '  html.append("""TITLE"": """","
            'End If

            'If Not IsDBNull(r("CONTACT EMAIL")) Then
            '  html.append("""CONTACT EMAIL"": """ & r("CONTACT EMAIL") & ""","
            'Else
            '  html.append("""CONTACT EMAIL"": """","
            'End If

            'If Not IsDBNull(r("CONTACT OFFICE PHONE")) Then
            '  html.append("""CONTACT OFFICE PHONE"": """ & r("CONTACT OFFICE PHONE") & ""","
            'Else
            '  html.append("""CONTACT OFFICE PHONE"": """","
            'End If

            'If Not IsDBNull(r("CONTACT MOBILE PHONE")) Then
            '  html.append("""CONTACT MOBILE PHONE"": """ & r("CONTACT MOBILE PHONE") & ""","
            'Else
            '  html.append("""CONTACT MOBILE PHONE"": """","
            'End If
            html.Append("}")
        Next


        Return "var flightsDataSet = [ " & html.ToString & " ];"
    End Function

    Public Shared Function ViewAllNotesLink(ByVal companyID As Long, ByVal aircraftID As Long, ByVal CRMSource As String, ByVal linkClass As String) As String
        Dim returnString As String = ""
        returnString = "<a href=""javascript:void(0);"" onclick=""javascript:load('ShowNoteDetails.aspx?"
        If companyID > 0 Then
            returnString += "compid=" & companyID
        Else
            returnString += "acid=" & aircraftID
        End If
        returnString += "&source=" & CRMSource


        returnString += " ','','scrollbars=yes,menubar=no,height=600,width=1160,"
        returnString += "resizable=yes,toolbar=no,location=no,status=no');"" class=""" & linkClass & """>View All</a>"

        Return returnString
    End Function

    Public Shared Function ConvertDataTableToNonDynamicTable(ByVal dt As DataTable) As String
        Dim html As New StringBuilder

        Dim utilizationData As New utilization_functions
        Dim Header As New StringBuilder
        Dim Content As New StringBuilder

        Header.Append("<th>MAKE</th>")
        Header.Append("<th>MODEL</th>")
        Header.Append("<th>MFR YEAR</th>")
        Header.Append("<th>SERNBR</th>")
        Header.Append("<th>REGNBR</th>")
        Header.Append("<th>FLIGHT DATE</th>")
        Header.Append("<th>FLIGHT TIME</th>")
        Header.Append("<th>DISTANCE</th>")
        Header.Append("<th>ESTFUELBURN</th>")
        Header.Append("<th>DEPARTURE DATE</th>")
        Header.Append("<th>DEPARTURE TIME</th>")
        Header.Append("<th>DEPARTURE CODE</th>")
        Header.Append("<th>DEPARTURE NAME</th>")
        Header.Append("<th>DEPARTURE CITY</th>")
        Header.Append("<th>DEPARTURE STATE</th>")
        Header.Append("<th>DEPARTURE COUNTRY</th>")
        Header.Append("<th>DEPARTURE LAT</th>")
        Header.Append("<th>DEPARTURE LONG</th>")

        Header.Append("<th>ARRIVAL DATE</th>")
        Header.Append("<th>ARRIVAL TIME</th>")
        Header.Append("<th>ARRIVAL CODE</th>")
        Header.Append("<th>ARRIVAL AIRPORT</th>")
        Header.Append("<th>ARRIVAL CITY</th>")
        Header.Append("<th>ARRIVAL STATE</th>")
        Header.Append("<th>ARRIVAL COUNTRY</th>")
        Header.Append("<th>ARRIVAL LAT</th>")
        Header.Append("<th>ARRIVAL LONG</th>")
        Header.Append("<th>OPERATOR</th>")
        Header.Append("<th>ADDRESS</th>")
        Header.Append("<th>CITY</th>")
        Header.Append("<th>STATE</th>")
        Header.Append("<th>COUNTRY</th>")
        Header.Append("<th>WEB ADDRESS</th>")
        Header.Append("<th>EMAIL</th>")
        Header.Append("<th>OFFICE PHONE</th>")
        Header.Append("<th>FIRST NAME</th>")
        Header.Append("<th>LAST NAME</th>")
        Header.Append("<th>TITLE</th>")
        Header.Append("<th>CONTACT EMAIL</th>")
        Header.Append("<th>CONTACT OFFICE PHONE</th>")
        Header.Append("<th>CONTACT MOBILE PHONE</th>")
        Header.Append("<th>BASE AIRPORT</th>")
        Header.Append("<th>BASE CITY</th>")
        Header.Append("<th>BASE STATE</th>")
        Header.Append("<th>BASE COUNTRY</th>")
        Header.Append("<th>BASE IATA</th>")
        Header.Append("<th>BASE ICAO</th>")
        Header.Append("<th>BUSINESS TYPE</th>")

        For Each r As DataRow In dt.Rows
            Content.Append("<tr>")
            Content.Append("<td>")
            If Not IsDBNull(r("MAKE")) Then
                Content.Append(r("MAKE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td align=""right"">")
            If Not IsDBNull(r("MODEL")) Then
                Content.Append(r("MODEL").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("MFR YEAR")) Then
                Content.Append(r("MFR YEAR").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("SERNBR")) Then
                Content.Append(r("SERNBR").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("REGNBR")) Then
                Content.Append(r("REGNBR").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("FLIGHT DATE")) Then
                Content.Append(FormatDateTime(r("FLIGHT DATE"), DateFormat.GeneralDate))
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("FLIGHT TIME")) Then
                Content.Append(r("FLIGHT TIME").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("DISTANCE")) Then
                Content.Append(r("DISTANCE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("ESTFUELBURN")) Then
                Content.Append(r("ESTFUELBURN").ToString)
            End If
            Content.Append("</td>")

            ' ADDED IN MSW - 2/25/20---- PER REQUEST
            Content.Append("<td>")
            If Not IsDBNull(r("DepartureDate")) Then
                Content.Append(r("DepartureDate").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("DepartureTime")) Then
                Content.Append(r("DepartureTime").ToString)
            End If
            Content.Append("</td>")


            Content.Append("<td>")
            If Not IsDBNull(r("ORIGIN CODE")) Then
                Content.Append(r("ORIGIN CODE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("ORIGIN NAME")) Then
                Content.Append(r("ORIGIN NAME").ToString)
            End If
            Content.Append("</td>")


            Content.Append("<td>")
            If Not IsDBNull(r("ORIGIN CITY")) Then
                Content.Append(r("ORIGIN CITY").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("ORIGIN STATE")) Then
                Content.Append(r("ORIGIN STATE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("ORIGIN COUNTRY")) Then
                Content.Append(r("ORIGIN COUNTRY").ToString)
            End If
            Content.Append("</td>")


            Content.Append("<td>")
            If Not IsDBNull(r("ORIGIN LAT")) Then
                Content.Append(r("ORIGIN LAT").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("ORIGIN LONG")) Then
                Content.Append(r("ORIGIN LONG").ToString)
            End If
            Content.Append("</td>")


            ' ADDED IN MSW - 2/25/20---- PER REQUEST
            Content.Append("<td>")
            If Not IsDBNull(r("ArrivalDate")) Then
                Content.Append(r("ArrivalDate").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("ArrivalTime")) Then
                Content.Append(r("ArrivalTime").ToString)
            End If
            Content.Append("</td>")


            Content.Append("<td>")
            If Not IsDBNull(r("DEST CODE")) Then
                Content.Append(r("DEST CODE").ToString)
            End If
            Content.Append("</td>")


            Content.Append("<td>")
            If Not IsDBNull(r("DEST NAME")) Then
                Content.Append(r("DEST NAME").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("DEST CITY")) Then
                Content.Append(r("DEST CITY").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("DEST STATE")) Then
                Content.Append(r("DEST STATE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("DEST COUNTRY")) Then
                Content.Append(r("DEST COUNTRY").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("DEST LAT")) Then
                Content.Append(r("DEST LAT").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("DEST LONG")) Then
                Content.Append(r("DEST LONG").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("OPERATOR")) Then
                Content.Append(r("OPERATOR").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("ADDRESS")) Then
                Content.Append(r("ADDRESS").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("CITY")) Then
                Content.Append(r("CITY").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("STATE")) Then
                Content.Append(r("STATE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("COUNTRY")) Then
                Content.Append(r("COUNTRY").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("WEB ADDRESS")) Then
                Content.Append(r("WEB ADDRESS").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("EMAIL")) Then
                Content.Append(r("EMAIL").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("OFFICE PHONE")) Then
                Content.Append(r("OFFICE PHONE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("FIRST NAME")) Then
                Content.Append(r("FIRST NAME").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("LAST NAME")) Then
                Content.Append(r("LAST NAME").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("TITLE")) Then
                Content.Append(r("TITLE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("CONTACT EMAIL")) Then
                Content.Append(r("CONTACT EMAIL").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("CONTACT OFFICE PHONE")) Then
                Content.Append(r("CONTACT OFFICE PHONE").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("CONTACT MOBILE PHONE")) Then
                Content.Append(r("CONTACT MOBILE PHONE").ToString)
            End If
            Content.Append("</td>")



            Content.Append("<td>")
            If Not IsDBNull(r("base_aport_name")) Then
                Content.Append(r("base_aport_name").ToString)
            End If
            Content.Append("</td>")


            Content.Append("<td>")
            If Not IsDBNull(r("base_aport_city")) Then
                Content.Append(r("base_aport_city").ToString)
            End If
            Content.Append("</td>")


            Content.Append("<td>")
            If Not IsDBNull(r("base_aport_state")) Then
                Content.Append(r("base_aport_state").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("base_aport_country")) Then
                Content.Append(r("base_aport_country").ToString)
            End If
            Content.Append("</td>")


            Content.Append("<td>")
            If Not IsDBNull(r("base_aport_iata_code")) Then
                Content.Append(r("base_aport_iata_code").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")
            If Not IsDBNull(r("base_aport_icao_code")) Then
                Content.Append(r("base_aport_icao_code").ToString)
            End If
            Content.Append("</td>")

            Content.Append("<td>")

            If Not IsDBNull(r("cbus_name")) Then
                Content.Append(r("cbus_name").ToString)
            End If
            Content.Append("</td>")
            Content.Append("</tr>")
        Next


        html.Append("<table><thead>" & Header.ToString & "</thead><tbody>" & Content.ToString & "</tbody></table>")
        Return html.ToString
    End Function

    Public Shared Function InsertDashboardModuleList(ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long, sidash_order As Long, sidash_dashb_id As Long) As Boolean
        Dim sQuery = New StringBuilder()
        Dim ResponseCode As Boolean = False
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append(" insert into " & IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") & "Subscription_Install_Dashboard (sidash_sub_id, sidash_login, sidash_seq_no, sidash_order, sidash_dashb_id) ")
            sQuery.Append(" values(@subID, @userLogin, @seqNO, @sidash_order, @sidash_dashb_id)")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "DisplayFunctions.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@subID", subID)
            SqlCommand.Parameters.AddWithValue("@userLogin", userLogin)
            SqlCommand.Parameters.AddWithValue("@seqNo", seqNO)
            SqlCommand.Parameters.AddWithValue("@sidash_order", sidash_order)
            SqlCommand.Parameters.AddWithValue("@sidash_dashb_id", sidash_dashb_id)

            SqlCommand.ExecuteNonQuery()

            ResponseCode = True


            SqlCommand.Dispose()
            SqlCommand = Nothing
        Catch ex As Exception
            Return Nothing

        Finally

            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

        Return ResponseCode

    End Function

    Public Shared Function Update_Subscription_Comp_Contact_ID(ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long, ByVal contact_id As Long) As Boolean
        Dim sQuery = New StringBuilder()
        Dim ResponseCode As Boolean = False
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE = True Then
                sQuery.Append(" UPDATE Subscription_Login ")
            ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append(" UPDATE [Homebase].jetnet_ra.dbo.Subscription_Login  ")
            Else
                sQuery.Append(" UPDATE Subscription_Login ")

            End If

            sQuery.Append(" set sublogin_contact_id = " & contact_id & "  ")
            sQuery.Append(" where sublogin_sub_id = " & subID & " ")
            sQuery.Append(" And sublogin_login = '" & userLogin & "' ")
            sQuery.Append(" and sublogin_contact_id = 0 ")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "DisplayFunctions.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlCommand.ExecuteNonQuery()

            ResponseCode = True


            SqlCommand.Dispose()
            SqlCommand = Nothing
        Catch ex As Exception
            Return Nothing

        Finally

            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

        Return ResponseCode

    End Function

    Public Shared Sub DeleteChosenDashboards(ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long)

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim update_string As String = ""
        Dim temp_id As Long = 0


        Try

            If IsNumeric(subID) Then
                If subID > 0 Then
                    If Not String.IsNullOrEmpty(userLogin) Then
                        update_string = "Delete FROM " & IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") & "Subscription_Install_Dashboard where sidash_sub_id = @subID and sidash_login = @userLogin and sidash_seq_no = @seqNo "

                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                        SqlConn.Open()


                        Dim SqlCommand As New SqlClient.SqlCommand(update_string, SqlConn)
                        SqlCommand.Parameters.AddWithValue("@subID", subID)
                        SqlCommand.Parameters.AddWithValue("@userLogin", userLogin)
                        SqlCommand.Parameters.AddWithValue("@seqNo", seqNO)


                        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "DisplayFunctions.vb", update_string.ToString)

                        SqlCommand.ExecuteNonQuery()

                        SqlCommand.Dispose()
                        SqlCommand = Nothing
                    End If
                End If
            End If

        Catch ex As Exception
        Finally
            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

    End Sub

    Public Shared Function ConvertDataTableToHTML(ByVal dt As DataTable, tableID As Integer) As String

        Dim html As New StringBuilder


        Try
            html.Append("<table id='table_" & tableID.ToString & "' class=""formatTable blue datagrid small""  style=""width:100%"">")
            html.Append("<thead>")
            html.Append("<tr>")
            For i As Integer = 0 To dt.Columns.Count - 1
                If Not InStr(dt.Columns(i).ColumnName.ToUpper, "_ID") > 0 Then
                    html.Append("<th>" & dt.Columns(i).ColumnName & "</th>")
                End If
            Next

            html.Append("</tr>")
            html.Append("</thead>")
            html.Append("<tbody>")
            For i As Integer = 0 To dt.Rows.Count - 1

                html.Append("<tr>")
                For j As Integer = 0 To dt.Columns.Count - 1
                    If Not InStr(dt.Columns(j).ColumnName.ToUpper, "_ID") > 0 Then
                        If IsDBNull(dt.Rows(i)(j)) Then
                            html.Append("<td align=""right"">")
                            html.Append("</td>")
                        ElseIf clsGeneral.clsGeneral.IsDataTypeNumeric(dt.Columns(j)) Then
                            html.Append("<td align=""right"">")
                            html.Append(FormatNumber(dt.Rows(i)(j), 0).ToString())
                            html.Append("</td>")
                        ElseIf IsDate(dt.Rows(i)(j)) Then
                            html.Append("<td data-sort=""" & Format(dt.Rows(i)(j), "yyyy-MM-dd HH:mm:ss") & """>" & FormatDateTime(dt.Rows(i)(j), DateFormat.GeneralDate) & "</td>")
                        Else
                            If InStr(dt.Columns(j).ColumnName.ToUpper, "COMPANY") > 0 Then
                                html.Append("<td>" & WriteDetailsLink(0, dt.Rows(i)("COMPANY_ID"), 0, 0, True, dt.Rows(i)(j).ToString(), "text_underline", "") & "</td>")
                            Else
                                html.Append("<td>" & dt.Rows(i)(j).ToString() & "</td>")
                            End If

                        End If
                    End If
                Next

                html.Append("</tr>")
            Next
            html.Append("</tbody>")
            'html.Append("<tfoot>")
            'html.Append("<tr>")
            'html.Append("<th align=""right""></th>")
            'For i As Integer = 0 To dt.Columns.Count - 2

            '    If Not InStr(dt.Columns(i).ColumnName.ToUpper, "_ID") > 0 Then
            '        html.Append("<th class=""text_align_right""></th>")
            '    End If

            'Next
            'html.Append("</tr>")
            'html.Append("</tfoot>")
            html.Append("</table>")
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in ConvertDataTabletoHTML " + ex.Message
        End Try
        Return html.ToString
    End Function

    Public Shared Function ConvertUpcomingContractsHTML(ByVal dt As DataTable, tableID As Integer) As String

        Dim html As New StringBuilder


        Try
            html.Append("<table id='table_" & tableID.ToString & "' class=""formatTable blue datagrid small""  style=""width:100%"">")
            html.Append("<thead>")
            html.Append("<tr>")
            html.Append("<th width=""5%"">ENDDATE</th>")
            html.Append("<th width=""15%"">COMPANY</th>")
            html.Append("<th width=""7%"">ACTION</th>")
            html.Append("<th width=""33%"">DETAILS</th>")
            html.Append("<th width=""5%"">SERVICE</th>")
            html.Append("<th width=""35%"">LASTNOTE</th>")


            html.Append("</tr>")
            html.Append("</thead>")
            html.Append("<tbody>")
            For Each r As DataRow In dt.Rows

                html.Append("<tr>")

                If Not IsDBNull(r("ENDDATE")) Then
                    html.Append("<td data-sort=""" & Format(r("ENDDATE"), "yyyy-MM-dd HH:mm:ss") & """>" & FormatDateTime(r("ENDDATE"), DateFormat.GeneralDate) & "</td>")
                Else
                    html.Append("<td></td>")
                End If

                If Not IsDBNull(r("COMPANY")) Then
                    html.Append("<td>" & WriteDetailsLink(0, r("COMPANY_ID"), 0, 0, True, r("COMPANY"), "text_underline", "") & "</td>")
                Else
                    html.Append("<td align=""left""></td>")
                End If
                html.Append("<td align=""left"">")
                If Not IsDBNull(r("ACTION")) Then
                    html.Append(r("ACTION"))
                End If

                html.Append("</td>")
                html.Append("<td align=""left"">")
                If Not IsDBNull(r("DETAILS")) Then
                    html.Append(r("DETAILS"))
                End If

                html.Append("</td>")
                html.Append("<td align=""left"">")
                If Not IsDBNull(r("SERVICE")) Then
                    html.Append(r("SERVICE"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("LASTNOTE")) Then
                    html.Append(r("LASTNOTE"))
                End If

                html.Append("</td>")

                html.Append("</tr>")
            Next
            html.Append("</tbody>")

            html.Append("</table>")
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in ConvertDataTabletoHTML " + ex.Message
        End Try
        Return html.ToString
    End Function

    Public Shared Function ContractActionHTML(ByVal dt As DataTable, tableID As Integer) As String

        Dim html As New StringBuilder



        Try
            html.Append("<table id='table_" & tableID.ToString & "' class=""formatTable blue datagrid small""  style=""width:100%"">")
            html.Append("<thead>")
            html.Append("<tr>")


            html.Append("<th>DATE</th>")
            html.Append("<th>SERVICE</th>")
            html.Append("<th>DETAILS</th>")
            html.Append("<th>USERS</th>")
            html.Append("<th>COMPANY</th>")
            html.Append("<th>VALUE</th>")
            html.Append("<th>ACTION</th>")
            html.Append("<th>GROUP</th>")
            html.Append("</tr>")
            html.Append("</thead>")
            html.Append("<tbody>")
            For Each r As DataRow In dt.Rows
                html.Append("<tr>")
                If Not IsDBNull(r("DATE")) Then
                    html.Append("<td data-sort=""" & Format(r("DATE"), "yyyy-MM-dd HH:mm:ss") & """><a href=""javascript:void(0);"" class=""text_underline"" onclick=""javascript:load('/homeTables.aspx?type_of=Company&sub_type_of=" + r("SOURCE").ToString.Trim + "&comp_id=" & r("COMPANY_ID") & "&activityid=" & r("ID") & "&user_id=MVIT&homebase=Y&action=update ','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" & FormatDateTime(r("DATE"), DateFormat.GeneralDate) & "</a></td>")
                Else
                    html.Append("<td align=""right""></td>")
                End If

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("SERVICE")) Then
                    html.Append(r("SERVICE"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("DETAILS")) Then
                    html.Append(r("DETAILS"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("USERS")) Then
                    html.Append(r("USERS"))
                End If

                html.Append("</td>")

                If Not IsDBNull(r("COMPANY")) Then
                    html.Append("<td>" & WriteDetailsLink(0, r("COMPANY_ID"), 0, 0, True, r("COMPANY"), "text_underline", "") & "</td>")
                Else
                    html.Append("<td align=""left""></td>")
                End If


                html.Append("<td align=""right"">")
                If Not IsDBNull(r("VALUE")) Then
                    html.Append(r("VALUE"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("ACTION")) Then
                    html.Append(r("ACTION"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("ENTERPRISE GROUP")) Then
                    html.Append(r("ENTERPRISE GROUP"))
                End If

                html.Append("</td>")


                html.Append("</tr>")
            Next
            html.Append("</tbody>")
            html.Append("<tfoot>")

            html.Append("<tr>")


            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("</tr>")
            html.Append("</tfoot>")
            html.Append("</table>")
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in ContractActionHTML " + ex.Message
        End Try
        Return html.ToString
    End Function

    Public Shared Function MyProspectsHTML(ByVal dt As DataTable, tableID As Integer) As String

        Dim html As New StringBuilder



        Try
            html.Append("<table id='table_" & tableID.ToString & "' class=""formatTable blue datagrid small""  style=""width:100%"">")
            html.Append("<thead>")
            html.Append("<tr>")

            html.Append("<th>EDIT</th>")
            html.Append("<th>COMPANY</th>")
            html.Append("<th>SERVICE</th>")
            html.Append("<th>TYPE</th>")
            html.Append("<th>DETAILS</th>")
            html.Append("<th>START</th>")
            html.Append("<th>TARGET</th>")
            html.Append("<th>NEXT</th>")
            html.Append("<th>BTYPE</th>")
            html.Append("<th>VALUE</th>")
            html.Append("<th>PERCENT</th>")
            html.Append("<th>JETNET</th>")
            html.Append("<th>LASTNOTE</th>")
            html.Append("</tr>")
            html.Append("</thead>")
            html.Append("<tbody>")
            For Each r As DataRow In dt.Rows
                html.Append("<tr>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("cprospect_id")) Then
                    html.Append("<img src=""/images/edit_icon.png"" alt=""Edit"" class=""cursor"" onclick=""javascript:load('/edit_note.aspx?ViewID=18&refreshing=prospect&action=edit&type=prospect&id=" & r("cprospect_id").ToString & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"" /></a>")
                End If

                html.Append("</td>")


                If Not IsDBNull(r("COMPANY")) Then
                    html.Append("<td>" & WriteDetailsLink(0, r("COMP_ID"), 0, 0, True, r("COMPANY"), "text_underline", "") & "</td>")
                Else
                    html.Append("<td align=""left""></td>")
                End If

                'If Not IsDBNull(r("TARGET")) Then
                '    html.Append("<td data-sort=""" & Format(r("TARGET"), "yyyy-MM-dd HH:mm:ss") & """><a href=""javascript:void(0);"" class=""text_underline"" onclick=""javascript:load('/homeTables.aspx?type_of=Company&sub_type_of=" + r("SOURCE").ToString.Trim + "&comp_id=" & r("COMPANY_ID") & "&activityid=" & r("ID") & "&user_id=MVIT&homebase=Y&action=update ','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" & FormatDateTime(r("TARGET"), DateFormat.GeneralDate) & "</a></td>")
                'Else
                '    html.Append("<td align=""right""></td>")
                'End If


                html.Append("<td align=""left"">")
                If Not IsDBNull(r("SERVICE")) Then
                    html.Append(r("SERVICE"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("TYPE")) Then
                    html.Append(r("TYPE"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("DETAILS")) Then
                    html.Append(r("DETAILS"))
                End If

                html.Append("</td>")

                If Not IsDBNull(r("START")) Then
                    html.Append("<td align=""left"" data-sort='" & Format(CDate(r("START")), "yyyy/MM/dd") & "'>")
                    html.Append(FormatDateTime(r("START"), vbShortDate))
                Else
                    html.Append("<td align=""left"">")
                End If

                html.Append("</td>")

                If Not IsDBNull(r("TARGET")) Then
                    html.Append("<td align=""left"" data-sort='" & Format(CDate(r("TARGET")), "yyyy/MM/dd") & "'>")
                    html.Append(FormatDateTime(r("TARGET"), vbShortDate))
                Else
                    html.Append("<td align=""left"">")
                End If

                html.Append("</td>")


                If Not IsDBNull(r("NEXT")) Then
                    html.Append("<td align=""left"" data-sort='" & Format(CDate(r("NEXT")), "yyyy/MM/dd") & "'>")
                    html.Append(FormatDateTime(r("NEXT"), vbShortDate))
                Else
                    html.Append("<td align=""left"">")
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("BTYPE")) Then
                    html.Append(r("BTYPE"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("VALUE")) Then
                    html.Append(r("VALUE"))
                End If

                html.Append("</td>")


                html.Append("<td align=""right"">")
                If Not IsDBNull(r("PERCENT")) Then
                    html.Append(r("PERCENT"))
                End If

                html.Append("</td>")



                html.Append("<td valign=""top"" align=""left"">")
                If Not IsDBNull(r("JETNET")) Then
                    html.Append(UCase(r("JETNET")))
                End If
                html.Append("</td>")

                If Not IsDBNull(r("LASTNOTE")) Then

                    html.Append("<td valign=""top"" align=""left"" data-sort='" & Format(CDate(r("LASTNOTE")), "yyyy/MM/dd") & "'>")

                    ' If Not IsDBNull(r("LASTNOTE_TEXT")) Then
                    '  strBuild.Append("<a href='#' title='" & Left(Trim(r("LASTNOTE_TEXT")), 25) & "'  name='" & Left(Trim(r("LASTNOTE_TEXT")), 25) & "'  alt='" & Left(Trim(r("LASTNOTE_TEXT")), 25) & "'>")
                    'End If

                    html.Append("" & clsGeneral.clsGeneral.TwoPlaceYear(r("LASTNOTE")))

                    If Not IsDBNull(r("LASTNOTE_TEXT")) Then
                        html.Append(" - " & Trim(r("LASTNOTE_TEXT")))
                    End If

                    '  If Not IsDBNull(r("LASTNOTE_TEXT")) Then
                    '  strBuild.Append("</a>")
                    'End If
                    html.Append("</td>")
                Else
                    html.Append("<td valign=""top"" align=""left"" data-sort=''></td>")
                End If


                html.Append("</tr>")
            Next
            html.Append("</tbody>")
            html.Append("<tfoot>")

            html.Append("<tr>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("</tr>")
            html.Append("</tfoot>")
            html.Append("</table>")
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in ContractActionHTML " + ex.Message
        End Try
        Return html.ToString
    End Function

    Public Shared Function My_Demos_Trials_HTML(ByVal dt As DataTable, tableID As Integer, Optional ByVal comp_id As Long = 0, Optional ByVal contact_id As Long = 0) As String

        Dim html As New StringBuilder

        Try
            html.Append("<table id='table_" & tableID.ToString & "' class=""formatTable blue datagrid small""  style=""width:100%"">")
            html.Append("<thead>")
            html.Append("<tr>")

            If tableID <> 100 Then
                html.Append("<th>EDIT</th>")
            End If

            html.Append("<th>ASSIGNEDTO</th>")
            html.Append("<th>SERVICE</th>")
            html.Append("<th>PASSWORD</th>")
            html.Append("<th>INSTALLED</th>")
            html.Append("<th>LASTLOGIN</th>")
            html.Append("<th>EXPIREDON</th>")
            html.Append("<th>STATUS</th>")
            ' html.Append("<th>BTYPE</th>")
            '  html.Append("<th>VALUE</th>")
            '  html.Append("<th>PERCENT</th>")
            '  html.Append("<th>JETNET</th>")
            '   html.Append("<th>LASTNOTE</th>")
            If tableID = 100 Then
                html.Append("<th>CONNECT TRIAL</th>")
            End If

            html.Append("</tr>")
            html.Append("</thead>")
            html.Append("<tbody>")
            For Each r As DataRow In dt.Rows
                html.Append("<tr>")

                If tableID <> 100 Then
                    html.Append("<td align=""left"">")
                    If Not IsDBNull(r("sub_id")) Then
                        html.Append("<img src=""/images/edit_icon.png"" alt=""Edit"" class=""cursor"" onclick=""javascript:load('/" & r("sub_id").ToString & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"" /></a>")
                    End If

                    html.Append("</td>")
                End If

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("ASSIGNEDTO")) Then
                    html.Append(r("ASSIGNEDTO"))
                End If

                html.Append("</td>")


                html.Append("<td align=""left"">")
                If Not IsDBNull(r("SERVICE")) Then
                    html.Append(r("SERVICE"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("PASSWORD")) Then
                    html.Append(r("PASSWORD"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("INSTALLED")) Then

                    If Not IsDBNull(r("sub_id")) And Not IsDBNull(r("sublogin_login")) Then    ' And Not IsDBNull(r("subins_seq_no"))
                        html.Append("<a href='adminSubErrors.aspx?email=demo@jetnet.com&sub_id=" & r("sub_id") & "&login=" & r("sublogin_login") & "' target='_blank'>")
                    End If

                    html.Append(r("INSTALLED"))

                    If Not IsDBNull(r("sub_id")) And Not IsDBNull(r("sublogin_login")) And Not IsDBNull(r("subins_seq_no")) Then
                        html.Append("</a>")
                    End If
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("LASTLOGIN")) Then
                    html.Append(r("LASTLOGIN"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("EXPIREON")) Then
                    html.Append(r("EXPIREON"))
                End If

                html.Append("</td>")

                html.Append("<td align=""left"">")
                If Not IsDBNull(r("STATUS")) Then
                    html.Append(r("STATUS"))
                End If

                html.Append("</td>")


                If tableID = 100 Then
                    If Not IsDBNull(r("sub_id")) And Not IsDBNull(r("sublogin_login")) And Not IsDBNull(r("subins_seq_no")) Then
                        html.Append("<td align=""left""><a href='DisplayContactDetail.aspx?compid=" & comp_id & "&contact_id=" & contact_id & "&trial_connect=Y&sub_id=" & r("sub_id") & "&sub_login=" & r("sublogin_login") & "&sub_seq=" & r("subins_seq_no") & "'>Connect Trial</a></td>")
                    Else
                        html.Append("<td align=""left""><a href='DisplayContactDetail.aspx?compid=" & comp_id & "&contact_id=" & contact_id & "&sub_id=" & contact_id & "&sub_login=" & contact_id & "&sub_seq=" & contact_id & "'>Connect Trial</a></td>")
                    End If
                End If


                html.Append("</tr>")
            Next
            html.Append("</tbody>")
            html.Append("<tfoot>")

            html.Append("<tr>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            html.Append("<th></th>")
            ' html.Append("<th></th>")
            ' html.Append("<th></th>")
            ' html.Append("<th></th>")
            ' html.Append("<th></th>")
            ' html.Append("<th></th>")
            html.Append("</tr>")
            html.Append("</tfoot>")
            html.Append("</table>")
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in ContractActionHTML " + ex.Message
        End Try

        Return html.ToString

    End Function

    Public Shared Function MyActionsHTML(ByVal dt As DataTable, tableID As Integer, ByVal inner_text As String) As String

        '    Dim html As New StringBuilder



        '    Try
        '        html.Append("<table id='table_" & tableID.ToString & "' class=""formatTable blue datagrid small""  style=""width:100%"">")
        '        html.Append("<thead>")
        '        html.Append("<tr>")

        '        If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then

        '        ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then

        '        End If


        '        html.Append(inner_text)

        '        'html.Append("<th>DATE</th>")
        '        'html.Append("<th>DESCRIPTION</th>")
        '        'html.Append("</tr>")
        '        'html.Append("</thead>")
        '        'html.Append("<tbody>")

        '        ' html.Append(inner_text)

        '        'html.Append("</tbody>")
        '        'html.Append("<tfoot>")

        '        'html.Append("<tr>")
        '        'html.Append("<th></th>")
        '        'html.Append("<th></th>")
        '        'html.Append("</tr>")
        '        'html.Append("</tfoot>")
        '        'html.Append("</table>")
        '    Catch ex As Exception

        '        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in ContractActionHTML " + ex.Message
        '    End Try
        '    Return html.ToString
        Return ""
    End Function

    Public Shared Sub DisplayHelpfulHint(TabDisplayName As String, ViewDisplay As Boolean, ViewDisplayName As String, hintText As Label, aclsData_Temp As clsData_Manager_SQL, hintPopupHolder As Panel, hintTextUpdate As Label)
        '  If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
        Dim HintTable As New DataTable
        Dim pageURL As String = ""
        Dim pageEnd As String = ""
        HintTable = aclsData_Temp.Get_Jetnet_Program_Hints("EH", HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, TabDisplayName, ViewDisplay, ViewDisplayName)

        If Not IsNothing(HintTable) Then
            If HintTable.Rows.Count > 0 Then
                'evonot_title, evonot_release_date, evonot_announcement
                hintPopupHolder.Visible = True

                If Not IsDBNull(HintTable.Rows(0).Item("evonot_id")) Then
                    hintTextUpdate.Text = HintTable.Rows(0).Item("evonot_id")
                End If

                If Not IsDBNull(HintTable.Rows(0).Item("evonot_doc_link")) Then
                    If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                        pageURL = Replace(HintTable.Rows(0).Item("evonot_doc_link"), "www.jetnetevolution.com", "www.testjetnetevolution.com")
                        pageURL = "<a href=""" & pageURL & """ target=""new"" class=""helpfulHintLink"">"
                    Else
                        pageURL = "<a href=""" & HintTable.Rows(0).Item("evonot_doc_link") & """ target=""new"" class=""helpfulHintLink"">"
                    End If
                    pageEnd = "</a>"
                End If


                hintText.Text += pageURL & "<img src=""/images/know_more.png"" class=""knowMoreImage"" />" & pageEnd


                If Not IsDBNull(HintTable.Rows(0).Item("evonot_title")) Then
                    If Not String.IsNullOrEmpty(HintTable.Rows(0).Item("evonot_title")) Then
                        hintText.Text += "<h3>" & pageURL & HintTable.Rows(0).Item("evonot_title").ToString & pageEnd & "</h3>"
                    End If
                End If

                If Not IsDBNull(HintTable.Rows(0).Item("evonot_announcement")) Then
                    If Not String.IsNullOrEmpty(HintTable.Rows(0).Item("evonot_announcement")) Then
                        Dim hintParagraph As String = ""
                        hintParagraph = Regex.Replace(HintTable.Rows(0).Item("evonot_announcement").ToString, "ick here", "ick " & pageURL & "<strong>here</strong>" & pageEnd, RegexOptions.IgnoreCase)
                        hintParagraph = ""

                        hintText.Text += "<p>" & Regex.Replace(HintTable.Rows(0).Item("evonot_announcement").ToString, "ick here", "ick " & pageURL & "<strong>here</strong>" & pageEnd, RegexOptions.IgnoreCase) & "</p>"

                    End If
                End If
                hintText.Text += "<br clear=""all"" />"
            End If
        End If


        ' End If
    End Sub

    Public Shared Sub ShowHelpfulHintPage(ByRef pageName As String, ByRef ShowView As Boolean, ByRef viewIdStr As String)
        If HttpContext.Current.Session.Item("isMobile") = False Then
            Dim QueryStringVar As String = ""
            Dim strPageAt As String = UCase(HttpContext.Current.Request.RawUrl.ToString())
            strPageAt = Replace(strPageAt, "/", "")
            Dim pageArray As String() = Split(strPageAt, ".ASPX")
            strPageAt = pageArray(0)



            'If strPageAt.Contains("HOME.ASPX") Then
            '    pageName = "Home"
            'ElseIf strPageAt.Contains("AIRCRAFT_LISTING.ASPX") Then
            '    pageName = "Aircraft"
            '    If Not IsNothing(HttpContext.Current.Request.Item("h")) Then
            '        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("h").ToString) Then
            '            pageName = "History"
            '        End If
            '    End If
            '    If Not IsNothing(HttpContext.Current.Request.Item("e")) Then
            '        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("e").ToString) Then
            '            pageName = "Events"
            '        End If
            '    End If

            'ElseIf strPageAt.Contains("COMPANY_LISTING.ASPX") Then
            '    pageName = "Company"

            'ElseIf strPageAt.Contains("PERFORMANCE_LISTING.ASPX") Then
            '    pageName = "Performance Specs"

            'ElseIf strPageAt.Contains("OPERATING_LISTING.ASPX") Then
            '    pageName = "OperatingCosts"

            'ElseIf strPageAt.Contains("MARKETSUMMARY.ASPX") Then
            '    pageName = "Market Summary"

            'ElseIf strPageAt.Contains("WANTED_LISTING.ASPX") Then
            '    pageName = "Wanted"

            If strPageAt.Contains("VIEW_TEMPLATE") Then
                If Not IsNothing(HttpContext.Current.Request.Item("ViewID")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("ViewID").ToString) Then
                        QueryStringVar = HttpContext.Current.Request.Item("ViewID").ToString
                    End If
                End If
                pageName = ""
                ShowView = True
                viewIdStr = "View[" & QueryStringVar & "]"
                'ElseIf strPageAt.Contains("DISPLAYAIRCRAFTDETAIL.ASPX") Then
                '    pageName = "Aircraft Details"
                '    If Not IsNothing(HttpContext.Current.Request.Item("jid")) Then
                '        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("jid").ToString) Then
                '            pageName = "Aircraft Details History"
                '        End If
                '    End If
                'ElseIf strPageAt.Contains("DISPLAYCOMPANYDETAIL.ASPX") Then
                '    pageName = "Company Details"
                '    If Not IsNothing(HttpContext.Current.Request.Item("jid")) Then
                '        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("jid").ToString) Then

                '            pageName = "Company Details History"
                '        End If
                '    End If
                'ElseIf strPageAt.Contains("DISPLAYCONTACTDETAIL.ASPX") Then
                '    pageName = "Contact Details"
                '    If Not IsNothing(HttpContext.Current.Request.Item("jid")) Then
                '        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("jid").ToString) Then
                '            pageName = "Contact Details History"
                '        End If
                '    End If
                'ElseIf strPageAt.Contains("ASSETINSIGHT.ASPX") Then
                '    pageName = "Asset Insight"
                'ElseIf strPageAt.Contains("FAAFLIGHTDATA.ASPX") Then
                '    pageName = "FAA Flight Data"
                'ElseIf strPageAt.Contains("USERPORTFOLIO.ASPX") Then
                '    pageName = "User Portfolio"
                'ElseIf strPageAt.Contains("AIRCRAFTFINDER.ASPX") Then
                '    pageName = "Aircraft Acquisition View"
                'ElseIf strPageAt.Contains("PDF_CREATOR.ASPX") Then
                '    pageName = "PDF Creator"
                'ElseIf strPageAt.Contains("SEARCHSUMMARY.ASPX") Then
                '    pageName = "Search Summary"
                'ElseIf strPageAt.Contains("EVO_EXPORTER.ASPX") Then
                '    pageName = "Evo Exporter"
                'ElseIf strPageAt.Contains("VIEWTOPDF.ASPX") Then
                '    pageName = "View PDF"
                'ElseIf strPageAt.Contains("FOLDERMAINTENANCE.ASPX") Then
                '    pageName = "Folder Maintenance"
                'ElseIf strPageAt.Contains("PREFERENCES.ASPX") Then
                '    pageName = "Preferences"
                'ElseIf strPageAt.Contains("HELP.ASPX") Then
                '    pageName = "Help"
            Else
                If strPageAt = "NOTIFY" Then
                ElseIf strPageAt = "AIRCRAFT_LISTING" Then
                    pageName = "AIRCRAFT"
                    If Not IsNothing(HttpContext.Current.Request.Item("h")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("h").ToString) Then
                            pageName = "HISTORY"
                        End If
                    End If
                    If Not IsNothing(HttpContext.Current.Request.Item("e")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("e").ToString) Then
                            pageName = "EVENTS"
                        End If
                    End If
                ElseIf strPageAt = "DISPLAYAIRCRAFTDETAIL" Then
                    pageName = "AIRCRAFT DETAIL"
                    If Not IsNothing(HttpContext.Current.Request.Item("jid")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("jid").ToString) Then
                            pageName = "AIRCRAFT DETAIL HISTORY"
                        End If
                    End If
                ElseIf strPageAt = "DISPLAYCOMPANYDETAIL" Then
                    pageName = "COMPANY DETAIL"
                    If Not IsNothing(HttpContext.Current.Request.Item("jid")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("jid").ToString) Then
                            pageName = "COMPANY DETAIL HISTORY"
                        End If
                    End If
                ElseIf strPageAt = "DISPLAYCONTACTDETAIL" Then
                    pageName = "CONTACT DETAIL"
                    If Not IsNothing(HttpContext.Current.Request.Item("jid")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("jid").ToString) Then
                            pageName = "CONTACT DETAIL HISTORY"
                        End If
                    End If
                Else
                    pageName = strPageAt
                End If

            End If
        End If
    End Sub

    Public Shared Function EvolutionDashboardSelectionList(chosenIDs As String) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


            sQuery.Append("select dashb_id, dashb_area, dashb_display_title from Dashboard_Menu with (NOLOCK) where dashb_system ='EVOLUTION' ")

            If chosenIDs <> "" Then
                sQuery.Append(" and dashb_id not in (" & chosenIDs & ") ")
            End If

            If HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
                sQuery.Append(" and dashb_area in ('Aerodex','Both')")
            End If

            sQuery.Append(" and dashb_system <> 'HOMEBASE' ")
            sQuery.Append(" and dashb_id <> 43 ")

            sQuery.Append(" order by dashb_display_title")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "DisplayFunctions.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing
        Catch ex As Exception
            Return Nothing

        Finally

            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

        Return atemptable

    End Function
End Class
