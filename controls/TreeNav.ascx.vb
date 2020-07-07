Imports System.IO
Partial Public Class TreeNav
  Inherits System.Web.UI.UserControl
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Public Event Clicked_Me(ByVal sender As Object, ByVal type As Integer, ByVal parent As Integer, ByVal text As String, ByVal isSubNode As Boolean, ByVal cfolderMethod As String)
  Public Event Searched_Me(ByVal sender As Object, ByVal table As DataTable)
  Dim error_string As String = ""

#Region "Page Events"

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    ' If Me.Visible Then
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      masterPage.aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
      masterPage.aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

      Make_TreeList()
    Catch ex As Exception
      error_string = "TreeNav.ascx.vb - Page_Load() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
    'End If

  End Sub

  Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Me.Visible Then
      Dim masterPage As main_site = DirectCast(Page.Master, main_site)
      Try

        '---------------------------------------------End Database Connection Stuff---------------------------------------------
        If Not Page.IsPostBack Then
          If Not IsNothing(Session.Item("show_hidden")) Then
            If Not String.IsNullOrEmpty(Session.Item("show_hidden").ToString) Then
              If Session.Item("show_hidden") = True Or Session.Item("show_hidden") = False Then
                show_hidden.Checked = Session.Item("show_hidden")
              End If
            End If
          End If
        End If



      Catch ex As Exception
        error_string = "TreeNav.ascx.vb - Page_Load() - " & ex.Message
        masterPage.LogError(error_string)
      End Try
    End If

  End Sub
#End Region
#Region "Function that makes the treeview nodes"
  Public Sub Make_TreeView()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      Dim og As Boolean = show_hidden.Checked


      If Session.Item("localUser").crmEvo = True Then 'If an EVO user
        With left_nav_tv
          .Nodes.Clear()
          Dim node As New TreeNode

          node.Text = "Company"
          'node.Value = 1
          node.ImageUrl = "../images/expanded.jpg"
          node.NavigateUrl = "../listing.aspx"
          .Nodes.Add(node)


          node = New TreeNode
          node.Text = "Contact"
          'node.Value = 2
          node.ImageUrl = "../images/expanded.jpg"
          node.NavigateUrl = "../listing_contact.aspx"
          .Nodes.Add(node)


          node = New TreeNode
          node.Text = "Aircraft"
          'node.Value = 3
          node.ImageUrl = "../images/expanded.jpg"
          node.NavigateUrl = "../listing_air.aspx"
          .Nodes.Add(node)


          node = New TreeNode
          node.Text = "Market Activity"
          'node.Value = 10
          node.NavigateUrl = "../market.aspx"
          node.ImageUrl = "../images/expanded.jpg"
          .Nodes.Add(node)


          node = New TreeNode
          node.Text = "Transactions"
          'node.Value = 8
          node.NavigateUrl = "../listing_transaction.aspx"
          node.ImageUrl = "../images/expanded.jpg"
          .Nodes.Add(node)

          node = New TreeNode
          node.Text = "Wanteds"
          'node.Value = 12
          node.NavigateUrl = "../listing_wanted.aspx"
          node.ImageUrl = "../images/expanded.jpg"
          .Nodes.Add(node)

          node = New TreeNode
          node.Text = "Model Summary"
          'node.Value = 13
          node.NavigateUrl = "../view_template.aspx?ViewID=1&noMaster=false"
          node.ImageUrl = "../images/expanded.jpg"
          node.Target = "new"
          .Nodes.Add(node)

          node = New TreeNode
          node.Text = "Perf. Specs"
          'node.Value = 13
          node.NavigateUrl = "../performance_specs.aspx"
          node.ImageUrl = "../images/expanded.jpg"
          .Nodes.Add(node)

          node = New TreeNode
          node.Text = "Operating Costs"
          'node.Value = 13
          node.NavigateUrl = "../op_costs.aspx"
          node.ImageUrl = "../images/expanded.jpg"
          .Nodes.Add(node)
        End With
        show_hidden.Visible = False

      Else
        Dim Folder_Hold As New DataTable
        Dim Folder_Search As New DataTable
        Dim atemptable3 As New DataTable
        Dim Folder_NonShare_Hold As New DataTable
        Dim Folder_NonShare_Search As New DataTable
        Dim atemptable4 As New DataTable
        Folder_NonShare_Hold = masterPage.aclsData_Temp.Get_Client_Folders_NonSharedForLeftNav(CInt(Session.Item("localUser").crmLocalUserID), "N")
        Folder_NonShare_Search = Folder_NonShare_Hold
        atemptable4 = Folder_NonShare_Hold.Clone

        Folder_Hold = masterPage.aclsData_Temp.Get_Client_Folders_Shared_ForNAV("Y")
        Folder_Search = Folder_Hold
        atemptable3 = Folder_Hold.Clone
        aTempTable = masterPage.aclsData_Temp.Get_Client_Folder_Type
        Dim pagename As String = ""
        Dim node, subnode, node_add As New TreeNode
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            With left_nav_tv
              .Nodes.Clear()

              For Each r As DataRow In aTempTable.Rows

                If HttpContext.Current.Session.Item("isMobile") <> True Or Session.Item("Listing") = r("cftype_id") Then
                  node = New TreeNode
                  node.Text = r("cfttpe_name")
                  'node.NavigateUrl = r("cftype_url")
                  node.Value = r("cftype_id")
                  node.ImageUrl = "../images/expanded.jpg"
                  If HttpContext.Current.Session.Item("isMobile") = True Then
                    node.NavigateUrl = "../mobile_listing.aspx?type=" & r("cftype_id")
                  End If
                  If r("cftype_id") = "5" And HttpContext.Current.Session.Item("isMobile") <> True Then
                    If Application.Item("crmClientSiteData").crmClientHostName = "WWW.JETADVISORSCRM.COM" Then
                      Dim counter As Integer
                      Dim aError As String = ""
                      aTempTable = masterPage.aclsData_Temp.GetClient_JobSeeker_status("P", aError)
                      If Not IsNothing(aTempTable) Then
                        counter = aTempTable.Rows.Count
                      Else
                        'display_error()
                      End If
                      subnode = New TreeNode
                      If counter <> 0 Then
                        subnode.Text = "<span style='color:crimson;'>Pending (" & counter & ")</span>"
                      Else
                        subnode.Text = "<span>Pending (" & counter & ")</span>"
                      End If
                      subnode.Value = "1"
                      subnode.ImageUrl = "../images/final.jpg"
                      node.ChildNodes.Add(subnode)
                      subnode = New TreeNode
                      subnode.Text = "Pilots"
                      subnode.Value = "2"
                      subnode.ImageUrl = "../images/final.jpg"
                      node.ChildNodes.Add(subnode)
                      subnode = New TreeNode
                      subnode.Text = "Mechanics"
                      subnode.Value = "3"
                      subnode.ImageUrl = "../images/final.jpg"
                      node.ChildNodes.Add(subnode)
                      .Nodes.Add(node)
                    End If
                    '.Nodes.Add(node)
                  ElseIf r("cftype_id") = "7" Then
                    If Session.Item("localSubscription").crmDocumentsFlag = True Then
                      node = New TreeNode
                      node.Text = "Documents"
                      node.Value = r("cftype_id")
                      node.ImageUrl = "../images/expanded.jpg"
                      'node.NavigateUrl = "../listing_document.aspx"
                      .Nodes.Add(node)
                    End If
                  ElseIf r("cftype_id") <> "5" Then

                    If r("cftype_id") = "4" Then
                      node_add = New TreeNode
                      node_add.Text = "Model Summary"
                      'node.Value = 13
                      node_add.NavigateUrl = "../view_template.aspx?ViewID=1&noMaster=false"
                      node_add.ImageUrl = "../images/expanded.jpg"
                      node_add.Target = "new"
                      .Nodes.Add(node_add)

                    End If
                    'End If

                    If r("cftype_id") = "4" Then

                      node_add = New TreeNode
                      node_add.Text = "Perf. Specs"
                      'node.Value = 13
                      node_add.NavigateUrl = "../performance_specs.aspx"
                      node_add.ImageUrl = "../images/expanded.jpg"
                      .Nodes.Add(node_add)

                      node_add = New TreeNode
                      node_add.Text = "Operating Costs"
                      'node.Value = 13
                      node_add.NavigateUrl = "../op_costs.aspx"
                      node_add.ImageUrl = "../images/expanded.jpg"
                      .Nodes.Add(node_add)
                    End If
                    atemptable3 = New DataTable
                    atemptable3 = Folder_Hold.Clone
                    'These Folders Are Not Shared. 
                    Folder_Search = Folder_Hold
                    Dim afiltered_Client As DataRow() = Folder_Search.Select("cfolder_cftype_id = '" & r("cftype_id") & "'", "")
                    ' extract and import
                    For Each atmpDataRow_Client In afiltered_Client
                      atemptable3.ImportRow(atmpDataRow_Client)
                    Next

                    If Not IsNothing(atemptable3) Then
                      If atemptable3.Rows.Count > 0 Then
                        For Each m As DataRow In atemptable3.Rows
                          Dim vis As Boolean = True
                          If show_hidden.Checked = False Then
                            If m("cfolder_hide_flag") = "Y" Then
                              vis = False
                            End If
                          End If
                          If vis = False Then
                          Else
                            subnode = New TreeNode
                            subnode.ImageToolTip = IIf(m("cfolder_method").ToString = "A", "ACTIVE", "")
                            subnode.Text = m("cfolder_name").ToString 'IIf(Len(m("cfolder_name")) > 17, Left(m("cfolder_name").ToString, 12) & "...", m("cfolder_name").ToString)
                            subnode.Value = m("cfolder_id")
                            subnode.ToolTip = m("cfolder_name").ToString
                            If HttpContext.Current.Session.Item("isMobile") = True Then
                              subnode.NavigateUrl = "../mobile_listing.aspx?sub=" & m("cfolder_id") & "&type=" & r("cftype_id")
                            End If

                            subnode.ImageUrl = "../" & DisplayFunctions.ReturnFolderImage(m("cfolder_method").ToString, m("cfolder_hide_flag").ToString, m("cfolder_share").ToString)

                            node.ChildNodes.Add(subnode)
                          End If
                        Next
                      End If
                    Else
                      If masterPage.aclsData_Temp.class_error <> "" Then
                        error_string = masterPage.aclsData_Temp.class_error
                        masterPage.LogError("TreeNav.ascx.vb - MakeTreeView() - " & error_string)
                      End If
                      masterPage.display_error()
                    End If

                    atemptable4 = New DataTable
                    atemptable4 = Folder_Hold.Clone
                    'These Folders Are Not Shared. 
                    Folder_NonShare_Search = Folder_NonShare_Hold
                    afiltered_Client = Folder_NonShare_Search.Select("cfolder_cftype_id = '" & r("cftype_id") & "'", "")
                    ' extract and import
                    For Each atmpDataRow_Client In afiltered_Client
                      atemptable4.ImportRow(atmpDataRow_Client)
                    Next


                    If Not IsNothing(atemptable4) Then
                      If atemptable4.Rows.Count > 0 Then
                        For Each m As DataRow In atemptable4.Rows
                          Dim vis As Boolean = True
                          If show_hidden.Checked = False Then
                            If m("cfolder_hide_flag") = "Y" Then
                              vis = False
                            End If
                          End If
                          If vis = False Then
                          Else
                            If UCase(m("cfolder_name")) = "MY AIRCRAFT" Or UCase(m("cfolder_name")) = "MY COMPANIES" Or UCase(m("cfolder_name")) = "MY CONTACTS" Then
                              Dim aTempTable_MyAC As New DataTable
                              aTempTable_MyAC = masterPage.aclsData_Temp.Get_Client_Folders_MyAircraft(UCase(m("cfolder_name")), CInt(Session.Item("localUser").crmLocalUserID), True)
                              If Not IsNothing(aTempTable_MyAC) Then
                                If aTempTable_MyAC.Rows.Count > 0 Then
                                  subnode = New TreeNode
                                  subnode.Text = m("cfolder_name")
                                  subnode.Value = m("cfolder_id")
                                  If HttpContext.Current.Session.Item("isMobile") = True Then
                                    subnode.NavigateUrl = "../mobile_listing.aspx?sub=" & m("cfolder_id") & "&type=" & r("cftype_id")
                                  End If
                                  subnode.ImageUrl = "../images/" & Server.HtmlDecode(Trim(Replace(LCase(m("cfolder_name")), "my", ""))) & "_folder.png"
                                  node.ChildNodes.Add(subnode)
                                End If
                              Else
                                If masterPage.aclsData_Temp.class_error <> "" Then
                                  error_string = masterPage.aclsData_Temp.class_error
                                  masterPage.LogError("TreeNav.ascx.vb - MakeTreeView() - " & error_string)
                                End If
                                masterPage.display_error()
                              End If
                            Else
                              subnode = New TreeNode
                              subnode.ImageToolTip = IIf(m("cfolder_method").ToString = "A", "ACTIVE", "")

                              subnode.Text = m("cfolder_name").ToString 'IIf(Len(m("cfolder_name")) > 17, Left(m("cfolder_name").ToString, 12) & "...", m("cfolder_name").ToString)
                              subnode.ToolTip = m("cfolder_name").ToString
                              subnode.Value = m("cfolder_id")
                              If HttpContext.Current.Session.Item("isMobile") = True Then
                                subnode.NavigateUrl = "../mobile_listing.aspx?sub=" & m("cfolder_id") & "&type=" & r("cftype_id")
                              End If


                              subnode.ImageUrl = "../" & DisplayFunctions.ReturnFolderImage(m("cfolder_method").ToString, m("cfolder_hide_flag").ToString, m("cfolder_share").ToString)


                              node.ChildNodes.Add(subnode)
                            End If
                          End If
                        Next
                      End If
                    Else
                      If masterPage.aclsData_Temp.class_error <> "" Then
                        error_string = masterPage.aclsData_Temp.class_error
                        masterPage.LogError("TreeNav.ascx.vb - MakeTreeView() - " & error_string)
                      End If
                      masterPage.display_error()
                    End If
                    .Nodes.Add(node)
                  Else
                    'response.Write("table empty")
                  End If
                End If
              Next

            End With
          Else
            '  Response.Write("rows zero")
          End If
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("TreeNav.ascx.vb - MakeTreeView() - " & error_string)
          End If
          masterPage.display_error()
        End If
      End If
      left_nav_tv.ExpandAll()
    Catch ex As Exception
      error_string = "TreeNav.ascx.vb - MakeTreeView() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub

  ''' <summary>
  ''' Returns folder image based on type.
  ''' </summary>
  ''' <param name="cfolder_method"></param>
  ''' <param name="cfolder_hide_flag"></param>
  ''' <param name="cfolder_share"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared Function ReturnFolderImageClass(ByVal cfolder_method As String, ByVal cfolder_hide_flag As String, ByVal cfolder_share As String) As String
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
      ReturnURL = Method_File & "shared_folder" & hidden_File '& ".png"
    Else
      ReturnURL = Method_File & "regular_folder" & hidden_File '& ".png"
    End If
    Return ReturnURL
  End Function

  Public Sub Make_TreeList()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      Dim og As Boolean = show_hidden.Checked

      Dim Folder_Hold As New DataTable
      Dim Folder_Search As New DataTable
      Dim atemptable3 As New DataTable
      Dim Folder_NonShare_Hold As New DataTable
      Dim Folder_NonShare_Search As New DataTable
      Dim atemptable4 As New DataTable
      Dim counter As Integer
      Dim aError As String = ""
      Folder_NonShare_Hold = masterPage.aclsData_Temp.Get_Client_Folders_NonSharedForLeftNav(CInt(Session.Item("localUser").crmLocalUserID), "N")
      Folder_NonShare_Search = Folder_NonShare_Hold
      atemptable4 = Folder_NonShare_Hold.Clone

      Folder_Hold = masterPage.aclsData_Temp.Get_Client_Folders_Shared_ForNAV("Y")
      Folder_Search = Folder_Hold
      atemptable3 = Folder_Hold.Clone
      aTempTable = masterPage.aclsData_Temp.Get_Client_Folder_Type
      Dim pagename As String = ""
      Dim node, subnode, node_add As New ListItem
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          orderedTreeList.Items.Clear()
          With orderedTreeList
            For Each r As DataRow In aTempTable.Rows
              subnode = New ListItem
              node = New ListItem
              If HttpContext.Current.Session.Item("isMobile") <> True Or Session.Item("Listing") = r("cftype_id") Then
                node = New ListItem
                node.Value = r("cftype_id") & "|0|"
                node.Attributes.Add("class", "expanded")

                If Not IsDBNull(r("cfttpe_name")) Then
                  If r("cfttpe_name").ToString.ToLower = "transactions" Then
                    node.Text = "History/Transactions"
                  Else
                    node.Text = r("cfttpe_name").ToString
                  End If
                End If

                If r("cftype_id") <> "5" And r("cftype_id") <> "7" Then
                  .Items.Add(node)
                End If

                If r("cftype_id") = "5" And HttpContext.Current.Session.Item("isMobile") <> True Then
                  If Application.Item("crmClientSiteData").crmClientHostName = "WWW.JETADVISORSCRM.COM" Then
                    .Items.Add(node)
                    aTempTable = masterPage.aclsData_Temp.GetClient_JobSeeker_status("P", aError)
                    If Not IsNothing(aTempTable) Then
                      counter = aTempTable.Rows.Count
                    End If

                    subnode = New ListItem
                    If counter <> 0 Then
                      subnode.Text = "<span style='color:crimson;'>Pending (" & counter & ")</span>"
                    Else
                      subnode.Text = "<span>Pending (" & counter & ")</span>"
                    End If

                    subnode.Value = "5|1|"
                    subnode.Attributes.Add("class", "subNode")
                    .Items.Add(subnode)

                    subnode = New ListItem
                    subnode.Text = "Pilots"
                    subnode.Value = "5|2|"
                    subnode.Attributes.Add("class", "subNode")
                    .Items.Add(subnode)

                    subnode = New ListItem
                    subnode.Text = "Mechanics"
                    subnode.Value = "5|3|"
                    subnode.Attributes.Add("class", "subNode")
                    .Items.Add(subnode)
                    subnode = New ListItem
                  End If

                ElseIf r("cftype_id") = "7" Then
                  If Session.Item("localSubscription").crmDocumentsFlag = True Then
                    subnode = New ListItem
                    subnode.Text = "Documents"
                    subnode.Value = "7|" & r("cftype_id") & "|"
                    subnode.Attributes.Add("parent", 7)
                    subnode.Attributes.Add("class", "expanded")
                    .Items.Add(subnode)
                  End If
                ElseIf r("cftype_id") <> "5" Then

                  'If r("cftype_id") = "4" Then
                  '  subnode = New ListItem
                  '  subnode.Text = "Model Market Summary"
                  '  subnode.Value = "13|0|"
                  '  .Items.Add(subnode)

                  '  subnode = New ListItem
                  '  subnode.Text = "Performance Specs"
                  '  subnode.Value = "14|0|"
                  '  .Items.Add(subnode)

                  '  subnode = New ListItem
                  '  subnode.Text = "Operating Costs"
                  '  subnode.Value = "15|0|"
                  '  .Items.Add(subnode)
                  'End If

                  atemptable3 = New DataTable
                  atemptable3 = Folder_Hold.Clone
                  'These Folders Are Not Shared. 
                  Folder_Search = Folder_Hold
                  Dim afiltered_Client As DataRow() = Folder_Search.Select("cfolder_cftype_id = '" & r("cftype_id") & "'", "")
                  ' extract and import
                  For Each atmpDataRow_Client In afiltered_Client
                    atemptable3.ImportRow(atmpDataRow_Client)
                  Next

                  If Not IsNothing(atemptable3) Then
                    If atemptable3.Rows.Count > 0 Then
                      For Each m As DataRow In atemptable3.Rows
                        Dim vis As Boolean = True
                        If show_hidden.Checked = False Then
                          If m("cfolder_hide_flag") = "Y" Then
                            vis = False
                          End If
                        End If
                        If vis = False Then
                        Else
                          subnode = New ListItem
                          subnode.Attributes.Add("title", IIf(m("cfolder_method").ToString = "A", "ACTIVE", ""))
                          subnode.Text = m("cfolder_name").ToString
                          subnode.Value = m("cfolder_cftype_id") & "|" & m("cfolder_id") & "|" & m("cfolder_method").ToString
                          subnode.Attributes.Add("class", "subnode " & ReturnFolderImageClass(m("cfolder_method").ToString, m("cfolder_hide_flag").ToString, m("cfolder_share").ToString))

                          .Items.Add(subnode)
                        End If
                      Next
                    End If
                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("TreeNav.ascx.vb - MakeTreeView() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If

                  atemptable4 = New DataTable
                  atemptable4 = Folder_Hold.Clone
                  'These Folders Are Not Shared. 
                  Folder_NonShare_Search = Folder_NonShare_Hold
                  afiltered_Client = Folder_NonShare_Search.Select("cfolder_cftype_id = '" & r("cftype_id") & "'", "")
                  ' extract and import
                  For Each atmpDataRow_Client In afiltered_Client
                    atemptable4.ImportRow(atmpDataRow_Client)
                  Next


                  If Not IsNothing(atemptable4) Then
                    If atemptable4.Rows.Count > 0 Then
                      For Each m As DataRow In atemptable4.Rows
                        Dim vis As Boolean = True
                        If show_hidden.Checked = False Then
                          If m("cfolder_hide_flag") = "Y" Then
                            vis = False
                          End If
                        End If
                        If vis = False Then
                        Else
                          If UCase(m("cfolder_name")) = "MY AIRCRAFT" Or UCase(m("cfolder_name")) = "MY COMPANIES" Or UCase(m("cfolder_name")) = "MY CONTACTS" Then
                            Dim aTempTable_MyAC As New DataTable
                            aTempTable_MyAC = masterPage.aclsData_Temp.Get_Client_Folders_MyAircraft(UCase(m("cfolder_name")), CInt(Session.Item("localUser").crmLocalUserID), True)
                            If Not IsNothing(aTempTable_MyAC) Then
                              If aTempTable_MyAC.Rows.Count > 0 Then
                                subnode = New ListItem
                                subnode.Text = m("cfolder_name")
                                subnode.Value = m("cfolder_cftype_id") & "|" & m("cfolder_id") & "|"


                                Select Case UCase(m("cfolder_name"))
                                  Case "MY AIRCRAFT"
                                    subnode.Attributes.Add("class", "myaircraft_folder subnode")
                                  Case "MY COMPANIES"
                                    subnode.Attributes.Add("class", "mycompanies_folder subnode")
                                  Case "MY CONTACTS"
                                    subnode.Attributes.Add("class", "mycontact_folder subnode")
                                End Select
                                .Items.Add(subnode)
                              End If
                            Else
                              If masterPage.aclsData_Temp.class_error <> "" Then
                                error_string = masterPage.aclsData_Temp.class_error
                                masterPage.LogError("TreeNav.ascx.vb - MakeTreeView() - " & error_string)
                              End If
                              masterPage.display_error()
                            End If
                          Else
                            subnode = New ListItem


                            subnode.Text = m("cfolder_name").ToString
                            subnode.Attributes.Add("class", "subnode " & ReturnFolderImageClass(m("cfolder_method").ToString, m("cfolder_hide_flag").ToString, m("cfolder_share").ToString))
                            subnode.Value = m("cfolder_cftype_id") & "|" & m("cfolder_id") & "|" & m("cfolder_method").ToString


                            .Items.Add(subnode)
                          End If
                        End If
                      Next
                    End If
                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("TreeNav.ascx.vb - MakeTreeView() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If

                Else
                  'response.Write("table empty")
                End If
              End If
            Next

          End With
        End If
      End If
    Catch ex As Exception
      error_string = "TreeNav.ascx.vb - MakeTreeView() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Treenode events"
  Private Sub left_nav_tv_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles left_nav_tv.SelectedNodeChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      left_nav_tv.ExpandAll()

      Session.Item("Subnode") = ""
      Session.Item("SubnodeMethod") = ""
      Session.Remove("Subnode")
      Session.Item("Results") = ""
      Session.Remove("Results")
      Session.Item("DaySelected") = "false"
      Session.Remove("DayPilotCalendar1_startDate")
      Dim node As TreeNode
      node = left_nav_tv.SelectedNode
      If Not IsNothing(node.Parent) Then
        RaiseEvent Clicked_Me(e, node.Value, node.Parent.Value, node.Text, True, IIf(node.ImageToolTip = "ACTIVE", "A", ""))
        masterPage.IsSubNode = True
      Else
        If node.Value = 4 Then
          Session.Item("DayPilotCalendar1_startDate") = ""
        End If
        RaiseEvent Clicked_Me(e, node.Value, node.Value, node.Text, True, IIf(node.ImageToolTip = "ACTIVE", "A", ""))
        masterPage.IsSubNode = False
      End If
    Catch ex As Exception
      error_string = "TreeNav.ascx.vb - left_nav_tv_SelectedNodeChanged() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region


  Private Sub show_hidden_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles show_hidden.CheckedChanged

    If show_hidden.Checked = True Then
      Session.Item("show_hidden") = True
    Else
      Session.Item("show_hidden") = False
    End If
    Make_TreeView()
  End Sub


  Private Sub orderedTreeList_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs) Handles orderedTreeList.Click
    Dim selectedLI As New ListItem
    Dim SplitValue As String()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    selectedLI = sender.Items(e.Index)
    SplitValue = Split(selectedLI.Value, "|")
    ' Response.Write(selectedLI.Value & " " & selectedLI.Text & " " & selectedLI.Attributes("parent"))
    Session.Item("Subnode") = ""
    Session.Item("SubnodeMethod") = ""
    Session.Remove("Subnode")
    Session.Item("Results") = ""
    Session.Remove("Results")
    Session.Item("DaySelected") = "false"
    Session.Remove("DayPilotCalendar1_startDate")
    If UBound(SplitValue) = 2 Then


      ' Response.Write(SplitValue(0) & " " & SplitValue(1) & " " & SplitValue(2))
      If SplitValue(1) > 0 Then
        RaiseEvent Clicked_Me(e, CLng(SplitValue(1)), CLng(SplitValue(0)), selectedLI.Text, True, IIf(SplitValue(2) = "A", "A", ""))
        masterPage.IsSubNode = True
      Else
        If SplitValue(0) = 4 Then
          Session.Item("DayPilotCalendar1_startDate") = ""
        End If
        RaiseEvent Clicked_Me(e, CLng(SplitValue(0)), CLng(SplitValue(0)), selectedLI.Text, True, IIf(SplitValue(2) = "A", "A", ""))
        masterPage.IsSubNode = False
      End If
    End If

  End Sub
End Class