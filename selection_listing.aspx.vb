
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/selection_listing.aspx.vb $
'$$Author: Mike $
'$$Date: 11/16/19 3:06p $
'$$Modtime: 11/16/19 3:01p $
'$$Revision: 3 $
'$$Workfile: selection_listing.aspx.vb $
'
' ********************************************************************************

Partial Public Class selection_listing
  Inherits System.Web.UI.Page



  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    Try



      ' TO BE MOVED OUT OF THIS SELECTION WHEN DONE - THE PAGE WILL BE DYNAMIC 
      ' First spot it going to be your sub headers
      ' Second spot is going to be your description 
      ' Third spot will be link 
      ' 4th column will be a sub_type if it is summed up by it
      If Trim(Request("area")) = "avionics" And Trim(Request("item_name")) <> "" Then
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = " avitem_item_name as 'Item Name', avitem_Description as 'Description', avitem_web_address as 'Web Address',  avitem_name as 'Name', avitem_mfr_name as 'Mfr Name', avitem_research_description as 'Research Description', avitem_upgrade_cost as 'Upgrade Cost', avitem_upgrade_downtime  as 'Upgrade Downtime', avitem_id as 'ID' "
        HttpContext.Current.Session.Item("Selection_Listing_Table") = "Avionics_Item"
        HttpContext.Current.Session.Item("Selection_Listing_Where") = "avitem_name='" & Trim(Request("item_name")) & "'"
        HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
        HttpContext.Current.Session.Item("Selection_Listing_Order") = " order by avitem_item_name asc "
      ElseIf Trim(Request("area")) = "avionics" Then
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = " avitem_item_name as 'Item Name', avitem_Description as 'Description', avitem_web_address as 'Web Address',  avitem_name as 'Name', avitem_mfr_name as 'Mfr Name', avitem_research_description as 'Research Description', avitem_upgrade_cost as 'Upgrade Cost', avitem_upgrade_downtime  as 'Upgrade Downtime', avitem_id as 'ID' "
        HttpContext.Current.Session.Item("Selection_Listing_Table") = "Avionics_Item"
        HttpContext.Current.Session.Item("Selection_Listing_Where") = " "
        HttpContext.Current.Session.Item("Selection_Listing_Group") = "  "
        HttpContext.Current.Session.Item("Selection_Listing_Order") = " order by avitem_name asc, avitem_item_name asc "
      Else

      End If

      Response.Write(Build_Dynamic_Listing())



    Catch ex As Exception

    End Try
  End Sub

  Public Shared Function Build_Dynamic_Listing() As String

    Dim data_Table As New DataTable
    Dim temp_fields As String = ""
    Dim temp_table As String = ""
    Dim temp_where As String = ""
    Dim fields_array(100) As String
    Dim field_names_array(100) As String
    Dim field_as_name As String = ""
    Dim field_count As Long = 0
    Dim htmlout As New StringBuilder
    Dim comp_functions As New CompanyFunctions
    Dim last_sub_type As String = ""
    Dim found_id As Boolean = False
    Dim t As Integer = 0
    Dim starting_space As String = ""
    Dim aclsData_Temp As New clsData_Manager_SQL


    Try

      If Trim(HttpContext.Current.Request.Item("viewType")) = "dynamic" Then
      Else
        htmlout.Append(comp_functions.NEW_build_style_page_full_spec(False, False, 998))
      End If


      temp_fields = HttpContext.Current.Session.Item("Selection_Listing_Fields")

      temp_fields = Replace(UCase(Trim(temp_fields)), "DISTINCT", "")
      temp_fields = Replace(UCase(Trim(temp_fields)), "SELECT", "")

      If Trim(HttpContext.Current.Request.Item("replace_top")) <> "" Then
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "TOP " & Trim(HttpContext.Current.Request.Item("replace_top")), "")
      End If

      If InStr(Trim(temp_fields), ",") > 0 Then
        fields_array = Split(Trim(temp_fields), ",")
        field_count = UBound(fields_array)
      End If

      data_Table = aclsData_Temp.Run_Selection_Listing_Query()

      If Not IsNothing(data_Table) Then
        If data_Table.Rows.Count > 0 Then


          If Trim(HttpContext.Current.Request.Item("display")) = "table" And Trim(HttpContext.Current.Request.Item("area")) = "avionics" Then
            htmlout.Append("<table cellpadding='3' cellspacing='0' width='90%'>")
            htmlout.Append("<tr valign='top'>")
            htmlout.Append("<td align='left' colspan='10'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER_NOALIGN") & "'>Avionics " & Trim(HttpContext.Current.Request.Item("item_name")) & "</font></td>")
            htmlout.Append("</tr>")
            htmlout.Append("<tr valign='top'>")
          ElseIf Trim(HttpContext.Current.Request.Item("display")) = "listing" And Trim(HttpContext.Current.Request.Item("area")) = "avionics" And Trim(HttpContext.Current.Request.Item("item_name")) = "" Then
            htmlout.Append("<table cellpadding='0' cellspacing='0' width='50%'>")
            htmlout.Append("<tr valign='top'>")
            htmlout.Append("<td align='left' colspan='10'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER_NOALIGN") & "'><font size='+2'>Avionics " & Trim(HttpContext.Current.Request.Item("item_name")) & "</font></font></td>")
            htmlout.Append("</tr>")
          ElseIf Trim(HttpContext.Current.Request.Item("display")) = "listing" And Trim(HttpContext.Current.Request.Item("area")) = "avionics" And Trim(HttpContext.Current.Request.Item("item_name")) <> "" Then
            htmlout.Append("<table cellpadding='0' cellspacing='0' width='50%'>")
          ElseIf Trim(HttpContext.Current.Request.Item("viewType")) = "dynamic" Then
            '   htmlout.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
            '   htmlout.Append("<tr><td align=""left"" valign=""top"">")
            '   htmlout.Append("<table id='tableCopy' cellpadding='0' cellspacing='0' border='0'>")
            '  htmlout.Append("<thead><tr><th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

            htmlout.Append("&nbsp;<font size='-6'><table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module""><tr><td align=""left"" valign=""top""><span id=""openNewWindowContents""><table id='tableCopy' cellpadding='0' cellspacing='0' border='0' align='center'><thead> <th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")


          Else
            htmlout.Append("<table cellpadding='0' cellspacing='0' width='50%'>")
          End If


          For i = 0 To field_count
            field_as_name = fields_array(i)
            If Left(field_as_name, 1) = " " Then
              field_as_name = Right(field_as_name, Len(field_as_name) - 1)
            End If

            field_as_name = Right(Trim(field_as_name), Len(Trim(field_as_name)) - InStr(Trim(LCase(field_as_name)), " as ") - 3) ' MAKE SURE ITS A LOWER CASE " as "
            field_as_name = Replace(Trim(field_as_name), "'", "")
            field_names_array(i) = Trim(field_as_name)

            If Trim(HttpContext.Current.Request.Item("viewType")) = "dynamic" Then
              htmlout.Append("<th class=""text_align_center"">" & field_names_array(i) & "</th>")
            ElseIf Trim(HttpContext.Current.Request.Item("display")) = "table" Then
              htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>" & field_names_array(i) & "</font></td>")
            End If
          Next

          If Trim(HttpContext.Current.Request.Item("display")) = "table" And Trim(HttpContext.Current.Request.Item("viewType")) <> "dynamic" Then
            htmlout.Append("</tr>")
          End If

          If Trim(HttpContext.Current.Request.Item("viewType")) = "dynamic" Then
            htmlout.Append("</thead><tbody>")
          End If

          For Each r As DataRow In data_Table.Rows


            If Trim(HttpContext.Current.Request.Item("display")) = "table" Then
              htmlout.Append("<tr valign='top'>")
            End If

            If Trim(HttpContext.Current.Request.Item("viewType")) = "dynamic" Then
              htmlout.Append("<td></td>")
            End If



            For i = 0 To field_count


              If Trim(HttpContext.Current.Request.Item("display")) = "listing" Then
                ' set this = 3 so that we will get the sub type 
                If Trim(last_sub_type) = "" Or Trim(last_sub_type) <> Trim(r.Item(field_names_array(3))) Then

                  last_sub_type = Trim(r.Item(field_names_array(3)))

                  htmlout.Append("<tr valign='top'>")

                  If Not IsDBNull(r.Item(field_names_array(3))) Then
                    htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER_NOALIGN") & "'>" & Trim(r.Item(field_names_array(3))) & "</font></td>")
                  Else
                    htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER_NOALIGN") & "'>&nbsp;</font></td>")
                  End If
                  htmlout.Append("</tr>")
                End If

                last_sub_type = Trim(r.Item(field_names_array(3)))
              End If


              If Trim(HttpContext.Current.Request.Item("display")) = "table" Then

                If Trim(HttpContext.Current.Request.Item("viewType")) = "dynamic" Then
                  If Not IsDBNull(r.Item(field_names_array(i))) Then

                    If IsDate(r.Item(field_names_array(i))) = True Then

                      htmlout.Append("<td class=""text_align_center""")
                      Dim dateSort As String = ""
                      If Not IsDBNull(r.Item(field_names_array(i))) Then
                        dateSort = Format(r.Item(field_names_array(i)), "yyyy/MM/dd")
                      End If

                      htmlout.Append(" data-sort=""" & dateSort & """>") ' AC LIST DATE
                      htmlout.Append("<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & Trim(r.Item(field_names_array(i))) & "</font></td>")

                    ElseIf IsNumeric(r.Item(field_names_array(i))) Then
                      htmlout.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & Trim(r.Item(field_names_array(i))) & "</font></td>")
                    Else
                      htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & Trim(r.Item(field_names_array(i))) & "</font></td>")
                    End If
                  Else
                    htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>&nbsp;</font></td>")
                  End If
                Else
                  If Not IsDBNull(r.Item(field_names_array(i))) Then
                    htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & Trim(r.Item(field_names_array(i))) & "</font></td>")
                  Else
                    htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>&nbsp;</font></td>")
                  End If
                End If


              ElseIf Trim(HttpContext.Current.Request.Item("display")) = "listing" Then

                If i = 0 Then
                  htmlout.Append("<tr valign='top'>")

                  found_id = False
                  If Trim(HttpContext.Current.Request.Item("homebase")) = "Y" Then
                    For t = 0 To field_count
                      If Trim(field_names_array(t)) = "ID" Then
                        found_id = True
                        Exit For
                      End If
                    Next
                  End If


                  If found_id = True Then
                    If Not IsDBNull(r.Item(field_names_array(i))) And Not IsDBNull(r.Item(field_names_array(t))) Then
                      htmlout.Append("<td align='left'><A href='maintenance.aspx?avionics=Y&homebase=Y&id=" & Trim(r.Item(field_names_array(t))) & "'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>" & Trim(r.Item(field_names_array(i))) & "</font></a></td>")
                    Else
                      htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>&nbsp;</font></td>")
                    End If
                  Else
                    If Not IsDBNull(r.Item(field_names_array(i))) Then
                      htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>" & Trim(r.Item(field_names_array(i))) & "</font></td>")
                    Else
                      htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>&nbsp;</font></td>")
                    End If
                  End If



                  htmlout.Append("</tr>")
                ElseIf i = 1 And field_count > 2 Then
                  htmlout.Append("<tr valign='top'>")
                  If Not IsDBNull(r.Item(field_names_array(i))) Then
                    htmlout.Append("<td align='left'><br/><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & Trim(r.Item(field_names_array(i))) & "</font>")
                  Else
                    htmlout.Append("<td align='left'><br/><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>&nbsp;</font>")
                  End If

                ElseIf i = 2 Then

                  If Not IsDBNull(r.Item(field_names_array(i))) Then
                    If InStr(Trim(r.Item(field_names_array(i))), "http://") = 0 Then
                      htmlout.Append("<A href='http://" & Trim(r.Item(field_names_array(i))) & "' target='_blank'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>Learn More</font></a><br/>&nbsp;</td>")
                    Else
                      htmlout.Append("<A href='" & Trim(r.Item(field_names_array(i))) & "' target='_blank'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>Learn More</font></a><br/>&nbsp;</td>")
                    End If
                  Else
                    htmlout.Append("<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>&nbsp;<br/>&nbsp;</font></td>")
                  End If

                  htmlout.Append("</tr>")
                Else
                  '  htmlout.Append("<tr valign='top'>")
                  ' If Not IsDBNull(r.Item(field_names_array(i))) Then
                  'htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & Trim(r.Item(field_names_array(i))) & "</font></td>")
                  ' Else
                  '  htmlout.Append("<td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>&nbsp;</font></td>")
                  ' End If
                  'htmlout.Append("</tr>")
                End If
              Else

              End If




            Next




            If Trim(HttpContext.Current.Request.Item("display")) = "table" Then
              htmlout.Append("</tr>")
            End If


          Next

          If Trim(HttpContext.Current.Request.Item("viewType")) = "dynamic" Then
            htmlout.Append("</tbody>")
          End If

          htmlout.Append("</table>")

          If Trim(HttpContext.Current.Request.Item("viewType")) = "dynamic" Then
            htmlout.Append("</span><div id=""forSaleInnerTable"" style=""width: 960px;""></div><br clear=""all"" /><p><strong>STANDARD EQUIPMENT:</strong> No Standard Features for this model</td></tr></table></p>")
          End If

        End If
      End If

    Catch ex As Exception
      Return ""
    End Try

    Return htmlout.ToString

  End Function

End Class