' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'

'$$Archive: /commonWebProject/Entity Classes/user_portfolio_view.vb $
'$$Author: Amanda $
'$$Date: 6/16/20 4:02p $
'$$Modtime: 6/16/20 3:50p $

'$$Revision: 33 $
'$$Workfile: user_portfolio_view.vb $

'
' ********************************************************************************



<System.Serializable()> Public Class userPortfolioDataLayer

    Private aError As String

    Private clientConnectString As String
    Private adminConnectString As String

    Private starConnectString As String
    Private cloudConnectString As String
    Private serverConnectString As String

    Sub New()




        aError = ""
        clientConnectString = ""
        adminConnectString = ""

        starConnectString = ""
        cloudConnectString = ""
        serverConnectString = ""

    End Sub

    Public Property class_error() As String
        Get
            class_error = aError
        End Get
        Set(ByVal value As String)
            aError = value
        End Set
    End Property


#Region "database_connection_strings"

    Public Property adminConnectStr() As String
        Get
            adminConnectStr = adminConnectString
        End Get
        Set(ByVal value As String)
            adminConnectString = value
        End Set
    End Property

    Public Property clientConnectStr() As String
        Get
            clientConnectStr = clientConnectString
        End Get
        Set(ByVal value As String)
            clientConnectString = value
        End Set
    End Property

    Public Property starConnectStr() As String
        Get
            starConnectStr = starConnectString
        End Get
        Set(ByVal value As String)
            starConnectString = value
        End Set
    End Property

    Public Property cloudConnectStr() As String
        Get
            cloudConnectStr = cloudConnectString
        End Get
        Set(ByVal value As String)
            cloudConnectString = value
        End Set
    End Property

    Public Property serverConnectStr() As String
        Get
            serverConnectStr = serverConnectString
        End Get
        Set(ByVal value As String)
            serverConnectString = value
        End Set
    End Property

#End Region

#Region "FILL_FUNCTIONS"

    Public Sub get_folder_list(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef folderDropdown As DropDownList, cfolderShare As String)


        Dim FoldersTable As New DataTable
        Dim StaticTable As New DataTable
        Dim TypeOfFolder As Integer = 0

        Const brokerItem As String = "Aircraft Brokered"
        Const ownedItem As String = "Aircraft Owned"
        Const operateItem As String = "Aircraft Operated"
        Const manageItem As String = "Managed Aircraft"

        Dim aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.JETNET_DB = clientConnectString

        Try
            'Fill up the Aircraft Folder List

            folderDropdown.Items.Clear()
            folderDropdown.Items.Add(New ListItem("", 0))

            ' check for and add "static" options based on 
            StaticTable = returnStaticItems(CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString))

            If Not IsNothing(StaticTable) Then
                If StaticTable.Rows.Count > 0 Then
                    For Each r As DataRow In StaticTable.Rows

                        If Not IsDBNull(r("BROKER")) Then
                            If IsNumeric(r("BROKER").ToString) Then
                                If CLng(r("BROKER").ToString) > 0 Then

                                    If (brokerItem.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                                        maxWidth = (brokerItem.Length * crmWebClient.Constants._STARTCHARWIDTH)
                                    End If

                                    folderDropdown.Items.Add(New ListItem(brokerItem, 99999))

                                End If
                            End If
                        End If

                        If Not IsDBNull(r("OPERATE")) Then
                            If IsNumeric(r("OPERATE").ToString) Then
                                If CLng(r("OPERATE").ToString) > 0 Then

                                    If (operateItem.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                                        maxWidth = (operateItem.Length * crmWebClient.Constants._STARTCHARWIDTH)
                                    End If

                                    folderDropdown.Items.Add(New ListItem(operateItem, 88888))

                                End If
                            End If
                        End If

                        If Not IsDBNull(r("OWN")) Then
                            If IsNumeric(r("OWN").ToString) Then
                                If CLng(r("OWN").ToString) > 0 Then

                                    If (ownedItem.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                                        maxWidth = (ownedItem.Length * crmWebClient.Constants._STARTCHARWIDTH)
                                    End If

                                    folderDropdown.Items.Add(New ListItem(ownedItem, 77777))

                                End If
                            End If
                        End If

                        If Not IsDBNull(r("MANAGE")) Then
                            If IsNumeric(r("MANAGE").ToString) Then
                                If CLng(r("MANAGE").ToString) > 0 Then

                                    If (manageItem.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                                        maxWidth = (manageItem.Length * crmWebClient.Constants._STARTCHARWIDTH)
                                    End If

                                    folderDropdown.Items.Add(New ListItem(manageItem, 66666))

                                End If
                            End If
                        End If

                    Next

                    folderDropdown.SelectedValue = searchCriteria.ViewCriteriaFolderID

                End If
            End If

            TypeOfFolder = 3
            FoldersTable = aclsData_Temp.GetEvolutionFolderssBySubscription(0, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, cfolderShare, TypeOfFolder, Nothing, "")

            If Not IsNothing(FoldersTable) Then
                If FoldersTable.Rows.Count > 0 Then
                    For Each r As DataRow In FoldersTable.Rows
                        If Not IsDBNull(r("cfolder_data")) Then

                            If Not IsDBNull(r.Item("cfolder_name")) And Not String.IsNullOrEmpty(r.Item("cfolder_name").ToString.Trim) Then

                                If (r.Item("cfolder_name").ToString.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                                    maxWidth = (r.Item("cfolder_name").ToString.Length * crmWebClient.Constants._STARTCHARWIDTH)
                                End If

                                folderDropdown.Items.Add(New ListItem(r.Item("cfolder_name").ToString, r.Item("cfolder_id").ToString))

                                If CLng(r.Item("cfolder_id").ToString) = searchCriteria.ViewCriteriaFolderID Then
                                    folderDropdown.SelectedValue = searchCriteria.ViewCriteriaFolderID
                                End If

                            End If

                        End If

                    Next
                End If
            End If

            If searchCriteria.ViewCriteriaFolderID = 0 Then
                folderDropdown.SelectedValue = ""
            End If

            folderDropdown.Width = (maxWidth)

        Catch ex As Exception

        Finally

            FoldersTable.Dispose()
            FoldersTable = Nothing

        End Try

    End Sub

    Public Function returnStaticItems(ByVal nCompanyID As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT comp_id,")
            sQuery.Append(" SUM(CASE WHEN cref_contact_type in ('99','93','38') THEN 1 ELSE 0 END) AS [BROKER],")
            sQuery.Append(" SUM(CASE WHEN cref_operator_flag in ('Y','O') then 1 ELSE 0 END) AS OPERATE,")
            sQuery.Append(" SUM(CASE WHEN cref_contact_type in ('00','97','08') THEN 1 ELSE 0 END) AS OWN,")
            sQuery.Append(" SUM(CASE WHEN cref_contact_type in ('31','94','17','18') THEN 1 ELSE 0 END) AS MANAGE")
            sQuery.Append(" FROM View_Aircraft_Company_Flat WITH(NOLOCK)")
            sQuery.Append(" WHERE comp_id  = " + nCompanyID.ToString)
            sQuery.Append(" AND ac_journ_id = 0")
            sQuery.Append(" GROUP BY comp_id")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "user_portfolio_view.vb", sQuery.ToString)

            SqlConn.ConnectionString = adminConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnStaticItems load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnStaticItems(ByVal nCompanyID As Integer) As DataTable " + ex.Message

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

    Public Function returnFolderContents(ByVal nFolderID As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT * FROM Client_Folder WITH(NOLOCK) WHERE (cfolder_id = " + nFolderID.ToString + " )")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "user_portfolio_view.vb", sQuery.ToString)

            SqlConn.ConnectionString = adminConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

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


    Public Function return_re_select_aircraft_list(ForceUncheckNotes As Boolean) As String

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As String = ""
        Dim i As Integer = 0

        Dim acListOut As New StringBuilder

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 90

            sQuery = HttpContext.Current.Session.Item("MasterAircraft")

            If InStr(Trim(sQuery.ToString), "from View_Aircraft_Flat") > 0 Then
                sQuery = "select distinct ac_id " & Right(Trim(sQuery.ToString), Len(Trim(sQuery.ToString)) - InStr(Trim(sQuery.ToString), "from View_Aircraft_Flat") + 1)
            End If

            If InStr(Trim(sQuery), " order by") > 0 Then
                sQuery = Left(Trim(sQuery), InStr(Trim(sQuery), " order by") - 1)
            End If


            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader()

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
            End Try

            _recordSet.Close()
            _recordSet = Nothing
            Dim CountNotes As Integer = 0
            If _dataTable.Rows.Count > 0 Then

                For Each r As DataRow In _dataTable.Rows

                    If Not IsDBNull(r.Item("ac_id")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_id").ToString.Trim) Then
                            CountNotes += 1
                            If String.IsNullOrEmpty(acListOut.ToString().Trim) Then
                                acListOut.Append(r.Item("ac_id").ToString.Trim)
                            Else
                                acListOut.Append(Constants.cCommaDelim + r.Item("ac_id").ToString.Trim)
                            End If

                        End If
                    End If

                Next

            End If ' _dataTable.Rows.Count > 0 Then
            If CountNotes > 5000 Then
                ForceUncheckNotes = True
            End If
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in return_aircraft_list(ByRef folderTable As DataTable) As String " + ex.Message

        Finally

        End Try

        Return acListOut.ToString()


    End Function

    Public Function returnStaticAircraftList(ByVal nFolderID As Long, ByVal nCompanyID As Long, Optional ByVal company_id_list As String = "", Optional ByVal show_type As String = "") As String

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()
        Dim i As Integer = 0

        Dim acListOut As New StringBuilder

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 90


            If Trim(company_id_list) <> "" Then
                company_id_list = company_id_list
            Else
                company_id_list = nCompanyID
            End If

            Select Case nFolderID
                Case 11111
                    sQuery.Append("SELECT ac_id FROM Aircraft_Flat WITH(NOLOCK)")

                    If Trim(show_type) = "" Then

                    End If


                    If Trim(show_type) = "operated" Then
                        sQuery.Append(" WHERE ac_id in (SELECT DISTINCT ac_id from View_Aircraft_Company_Flat WITH(NOLOCK) WHERE comp_id in (" & company_id_list & ") AND ac_journ_id = 0 AND cref_operator_flag IN('Y','O'))")
                    ElseIf Trim(show_type) = "own_operated" Then
                        sQuery.Append(" WHERE ac_id in (SELECT DISTINCT ac_id from View_Aircraft_Company_Flat WITH(NOLOCK) WHERE comp_id in (" & company_id_list & ") AND ac_journ_id = 0 AND (cref_contact_type IN('00','97','08') or cref_operator_flag IN('Y','O')))")
                    ElseIf Trim(show_type) = "brokered" Then
                        sQuery.Append(" WHERE ac_id in (SELECT DISTINCT ac_id from View_Aircraft_Company_Flat WITH(NOLOCK) WHERE comp_id in (" & company_id_list & ") AND ac_journ_id = 0 AND (cref_contact_type IN('99')))")
                    ElseIf Trim(show_type) = "managed" Then
                        sQuery.Append(" WHERE ac_id in (SELECT DISTINCT ac_id from View_Aircraft_Company_Flat WITH(NOLOCK) WHERE comp_id in (" & company_id_list & ") AND ac_journ_id = 0 AND (cref_contact_type IN('31')))")
                    End If


                    sQuery.Append(" AND ac_journ_id = 0")
                Case 99999
                    ' -- BROKER      
                    sQuery.Append("SELECT ac_id FROM Aircraft_Flat WITH(NOLOCK)")
                    sQuery.Append(" WHERE ac_id in (SELECT DISTINCT ac_id from View_Aircraft_Company_Flat WITH(NOLOCK) WHERE comp_id in (" & company_id_list & ") AND ac_journ_id = 0 AND cref_contact_type IN('99','93','38'))")
                    sQuery.Append(" AND ac_journ_id = 0")
                Case 88888
                    '-- OPERATE
                    sQuery.Append("SELECT ac_id FROM Aircraft_Flat WITH(NOLOCK)")
                    sQuery.Append(" WHERE ac_id in (SELECT DISTINCT ac_id from View_Aircraft_Company_Flat WITH(NOLOCK) WHERE comp_id in (" & company_id_list & ") AND ac_journ_id = 0 AND cref_operator_flag IN('Y','O'))")
                    sQuery.Append(" AND ac_journ_id = 0")
                Case 77777

                    '-- OWN
                    sQuery.Append("SELECT ac_id FROM Aircraft_Flat WITH(NOLOCK)")
                    sQuery.Append(" WHERE ac_id in (SELECT DISTINCT ac_id from View_Aircraft_Company_Flat WITH(NOLOCK) WHERE comp_id in (" & company_id_list & ") AND ac_journ_id = 0 AND cref_contact_type IN('00','97','08'))")
                    sQuery.Append(" AND ac_journ_id = 0")
                Case 66666

                    '-- MANAGE
                    sQuery.Append("SELECT ac_id FROM Aircraft_Flat WITH(NOLOCK)")
                    sQuery.Append(" WHERE ac_id in (SELECT DISTINCT ac_id from View_Aircraft_Company_Flat WITH(NOLOCK) WHERE comp_id in (" & company_id_list & ") AND ac_journ_id = 0 AND cref_contact_type IN('31','94','17','18'))")
                    sQuery.Append(" AND ac_journ_id = 0")

            End Select


            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader()

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                For Each r As DataRow In _dataTable.Rows

                    If Not IsDBNull(r.Item("ac_id")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_id").ToString.Trim) Then

                            If String.IsNullOrEmpty(acListOut.ToString().Trim) Then
                                acListOut.Append(r.Item("ac_id").ToString.Trim)
                            Else
                                acListOut.Append(Constants.cCommaDelim + r.Item("ac_id").ToString.Trim)
                            End If

                        End If
                    End If

                Next

            End If ' _dataTable.Rows.Count > 0 Then

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in return_aircraft_list(ByRef folderTable As DataTable) As String " + ex.Message

        Finally

        End Try

        Return acListOut.ToString()


    End Function

    Public Function returnAircraftList(ByRef folderTable As DataTable, ByRef uncheckNotes As Boolean) As String

        Dim dataString As String = ""

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()
        Dim i As Integer = 0

        Dim acListOut As New StringBuilder

        Const searchPhrase As String = "THEREALSEARCHQUERY"

        Try

            If Not IsNothing(folderTable) Then

                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                If folderTable.Rows.Count > 0 Then

                    For Each r As DataRow In folderTable.Rows

                        If Not IsDBNull(r.Item("cfolder_data")) Then
                            If Not String.IsNullOrEmpty(r.Item("cfolder_data").ToString.Trim) Then

                                dataString = r.Item("cfolder_data").ToString

                            End If
                        End If

                    Next


                    Dim tmpArray(1) As String

                    If Not String.IsNullOrEmpty(dataString.Trim) Then

                        If dataString.Trim.ToUpper.Contains(searchPhrase) Then

                            Dim pos1 As Integer = 0

                            pos1 = dataString.IndexOf(searchPhrase)

                            tmpArray(0) = dataString.Substring(pos1 + searchPhrase.Length + 1, dataString.Length - (pos1 + searchPhrase.Length + 1))

                        Else
                            If InStr(dataString, "!~!ac_id=") > 0 Then
                                tmpArray = Split(dataString, "!~!ac_id=")
                            Else
                                tmpArray = Split(dataString, "ac_id=")
                            End If

                            If InStr(tmpArray(1), "ac_id=") > 0 Then
                                tmpArray(1) = Replace(tmpArray(1), "ac_id=", "")
                            End If

                            Return tmpArray(1).Trim
                        End If

                    End If

                    If dataString.Trim.ToUpper.Contains(searchPhrase) Then

                        sQuery.Append(tmpArray(0).Replace(Constants.cDoubleSingleQuote, Constants.cSingleQuote))

                    Else

                        Return tmpArray(2).Trim

                    End If

                    SqlCommand.CommandText = sQuery.ToString
                    _recordSet = SqlCommand.ExecuteReader()

                    Try
                        _dataTable.Load(_recordSet)
                    Catch constrExc As System.Data.ConstraintException
                        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                    End Try

                    _recordSet.Close()
                    _recordSet = Nothing
                    Dim CountNotesItems As Integer = 0

                    If _dataTable.Rows.Count > 0 Then

                        For Each r As DataRow In _dataTable.Rows

                            If Not IsDBNull(r.Item("ac_id")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_id").ToString.Trim) Then

                                    If String.IsNullOrEmpty(acListOut.ToString().Trim) Then
                                        acListOut.Append(r.Item("ac_id").ToString.Trim)
                                    Else
                                        acListOut.Append(Constants.cCommaDelim + r.Item("ac_id").ToString.Trim)
                                    End If
                                    CountNotesItems += 1
                                End If
                            End If

                        Next


                    End If ' _dataTable.Rows.Count > 0 Then

                    If CountNotesItems > 5000 Then
                        uncheckNotes = True
                    End If

                End If ' folderTable.Rows.Count > 0 Then

            End If ' Not IsNothing(folderTable) Then

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in return_aircraft_list(ByRef folderTable As DataTable) As String " + ex.Message

        Finally

        End Try

        Return acListOut.ToString()

    End Function
#End Region

#Region "TAB_FUNCTIONS"
    Public Function display_tab0_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByVal summarize_field As String = "", Optional ByVal Show_Notes As Boolean = True) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Dim temp_label2 As New Label
        Dim temp_label3 As New Label


        Dim ResultsTable As New DataTable
        Dim aclsData_Temp = New clsData_Manager_SQL
        If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
            aclsData_Temp.JETNET_DB = clientConnectString
        End If


        Try


            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90


                If Trim(summarize_field) = "" Then
                    sQuery.Append("SELECT ac_id, amod_make_name as MAKE,")
                    sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
                    sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
                    sQuery.Append(" acs_name as LIFECYCLE, acot_name as OWNERSHIP,")
                    sQuery.Append(" (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = 0 AND cref_contact_type IN('00','17','08') AND comp_active_flag = 'Y')) AS OWNER,")
                    sQuery.Append(" (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = 0 AND cref_operator_flag IN('Y','O') AND comp_active_flag = 'Y')) AS OPERATOR,")
                    sQuery.Append(" ac_aport_name as BASEAPORT,")
                    sQuery.Append(" ac_aport_country as BASECOUNTRY,")
                    sQuery.Append(" ac_est_airframe_hrs as ESTAIRFRAMEHRS")
                    sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
                    sQuery.Append(aclist)
                    sQuery.Append(") AND ac_journ_id = 0 ")
                    sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                    sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")
                Else
                    Dim AFTTQuery As String = ""
                    If summarize_field = "ac_airframe_tot_hrs" Then
                        sQuery.Append("SELECT COUNT(DISTINCT ac_id) as tcount, ")
                        AFTTQuery = "case "

                        Dim CeilingAFTT As Long = 15000

                        For x As Integer = 0 To CeilingAFTT Step 1000
                            If x = CeilingAFTT Then
                                AFTTQuery += " when ac_airframe_tot_hrs >= " & x & " and ac_airframe_tot_hrs <= " & x + 1000 & " then '" & x & " - " & (x + 1000) & "' "
                            Else
                                If x = 0 Then
                                    AFTTQuery += " when ac_airframe_tot_hrs >= 1 and ac_airframe_tot_hrs < 1000 then '1 - 999' "
                                Else
                                    AFTTQuery += " when ac_airframe_tot_hrs >= " & x & " and ac_airframe_tot_hrs < " & x + 1000 & " then '" & x & " - " & (x + 1000) - 1 & "' "
                                End If
                            End If
                        Next

                        AFTTQuery += " end "
                        sQuery.Append(AFTTQuery & " as 'Summarized'")
                    Else

                        sQuery.Append("SELECT distinct " & Trim(summarize_field) & " as 'Summarized', count(distinct ac_id) as tcount  ")

                    End If

                    sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) ")

                    If Trim(summarize_field) = "acuse_name" Then
                        sQuery.Append(" inner Join  Aircraft_Useage with (NOLOCK) on acuse_code = ac_use_code")
                    End If


                    sQuery.Append(" WHERE ac_id IN (")
                    sQuery.Append(aclist)
                    sQuery.Append(") And ac_journ_id = 0 ")
                    sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))


                    If summarize_field = "ac_airframe_tot_hrs" Then
                        sQuery.Append(" GROUP BY ( " & AFTTQuery & " ) ORDER BY cast( replace( ( " & AFTTQuery & " ), ' - ', '') as float) asc ")
                    Else
                        sQuery.Append(" group by " & Trim(summarize_field))
                        sQuery.Append(" order by count(distinct ac_id) desc ")
                    End If

                End If


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab0_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        AllowExport = False
                    End If


                    Dim sSeparator As String = ""
                    htmlOut.Append(" var tab0DataSet  = [ ")

                    For Each r As DataRow In _dataTable.Rows



                        If Trim(summarize_field) = "" Then
                            If count > 0 Then
                                htmlOut.Append(",")
                            End If
                            htmlOut.Append("{")
                            htmlOut.Append("""check"": """",") 'Checkbox row. 
                            htmlOut.Append("""id"": """ + r.Item("ac_id").ToString.Trim + """,") 'hidden ID


                            htmlOut.Append("""note"": """)

                            If Show_Notes = True Then
                                htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                            Else
                                htmlOut.Append("")
                            End If


                            '       If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
                            'And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then

                            '           aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
                            '           DisplayFunctions.DisplayLocalItems(aclsData_Temp, r.Item("ac_id"), 0, 0, temp_label2, temp_label3, False, True, False, True, 5, False, False, "JETNET", Nothing, True)


                            '       ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                            '           ResultsTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(r.Item("ac_id"), "A", True, False, False, True)
                            '           If Not IsNothing(ResultsTable) Then
                            '               If ResultsTable.Rows.Count > 0 Then

                            '                   If Not IsDBNull(ResultsTable.Rows(0).Item("lnote_note")) Then
                            '                       If Trim(ResultsTable.Rows(0).Item("lnote_note")) <> "" Then
                            '                           htmlOut.Append("Y")
                            '                       End If
                            '                   End If

                            '                   ' htmlOut.Append("<i class=""fa-thumb-tack"" alt='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "'></i>")
                            '               End If
                            '           End If
                            '       End If

                            htmlOut.Append(""",")


                            htmlOut.Append("""make"": """)

                            If Not IsDBNull(r.Item("MAKE")) Then
                                If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MAKE").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append(""",") 'make

                            htmlOut.Append("""model"": """)
                            If Not IsDBNull(r.Item("MODEL")) Then
                                If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MODEL").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append(""",") 'model

                            htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("MAKE") & " " & r("MODEL") & " S/N #" & r("SERNO") & """>" & r("SERNO").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")


                            htmlOut.Append("""reg"": """)

                            If Not IsDBNull(r.Item("REGNO")) Then
                                If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
                                    htmlOut.Append(r.Item("REGNO").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""mfryear"": """)

                            If Not IsDBNull(r.Item("MFRYEAR")) Then
                                If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""dlvyear"": """)

                            If Not IsDBNull(r.Item("DLVYEAR")) Then
                                If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""lifecycle"": """)

                            If Not IsDBNull(r.Item("LIFECYCLE")) Then
                                If Not String.IsNullOrEmpty(r.Item("LIFECYCLE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("LIFECYCLE").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""ownership"": """)

                            If Not IsDBNull(r.Item("OWNERSHIP")) Then
                                If Not String.IsNullOrEmpty(r.Item("OWNERSHIP").ToString.Trim) Then
                                    Select Case UCase(Trim(r.Item("OWNERSHIP")))
                                        Case "FRACTIONAL OWNERSHIP PROGRAM"
                                            htmlOut.Append("Fractional")
                                        Case "SHARED OWNERSHIP"
                                            htmlOut.Append("Shared")
                                        Case Else
                                            htmlOut.Append("Whole")
                                    End Select
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""owner"": """)

                            If Not IsDBNull(r.Item("OWNER")) Then
                                If Not String.IsNullOrEmpty(r.Item("OWNER").ToString.Trim) Then
                                    htmlOut.Append(r.Item("OWNER").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""operator"": """)

                            If Not IsDBNull(r.Item("OPERATOR")) Then
                                If Not String.IsNullOrEmpty(r.Item("OPERATOR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("OPERATOR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""baseaport"": """)

                            If Not IsDBNull(r.Item("BASEAPORT")) Then
                                If Not String.IsNullOrEmpty(r.Item("BASEAPORT").ToString.Trim) Then
                                    htmlOut.Append(r.Item("BASEAPORT").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""basecountry"": """)

                            If Not IsDBNull(r.Item("BASECOUNTRY")) Then
                                If Not String.IsNullOrEmpty(r.Item("BASECOUNTRY").ToString.Trim) Then
                                    htmlOut.Append(r.Item("BASECOUNTRY").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""estairframehrs"": """)

                            If Not IsDBNull(r.Item("ESTAIRFRAMEHRS")) Then
                                If Not String.IsNullOrEmpty(r.Item("ESTAIRFRAMEHRS").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ESTAIRFRAMEHRS").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("""")
                            htmlOut.Append("}")
                            count += 1
                        Else

                            If count > 0 Then
                                htmlOut.Append(",")
                            End If
                            htmlOut.Append("{")
                            htmlOut.Append("""check"": """",") 'Checkbox row. 
                            htmlOut.Append("""id"": """ & count & """,") 'hidden ID


                            htmlOut.Append("""Summarized"": ")

                            If Not IsDBNull(r.Item("Summarized")) Then
                                If Not String.IsNullOrEmpty(r.Item("Summarized").ToString.Trim) Then


                                    If summarize_field = "ac_airframe_tot_hrs" Then
                                        If r.Item("Summarized").ToString.ToLower = "unknown" Then
                                            htmlOut.Append("[""Unknown"",0]")
                                        Else
                                            htmlOut.Append("[""" & r.Item("Summarized").ToString.Trim & """," & count.ToString & "]")
                                        End If
                                    Else
                                        If r.Item("Summarized").ToString.ToLower = "unknown" Then
                                            htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                                        Else
                                            htmlOut.Append("[""" & r.Item("Summarized").ToString.Trim & """,""" & r.Item("Summarized").ToString.Trim & """]")
                                        End If
                                    End If

                                Else
                                    If summarize_field = "ac_airframe_tot_hrs" Then
                                        htmlOut.Append("[""Unknown"",0]")
                                    Else
                                        htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                                    End If
                                End If
                            Else
                                If summarize_field = "ac_airframe_tot_hrs" Then
                                    htmlOut.Append("[""Unknown"",0]")
                                Else
                                    htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                                End If
                            End If


                            htmlOut.Append(",")

                            htmlOut.Append("""Total"": """)

                            If Not IsDBNull(r.Item("tcount")) Then
                                If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then
                                    htmlOut.Append(r.Item("tcount").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append("""")
                            htmlOut.Append("}")
                            count += 1
                        End If



                    Next
                    htmlOut.Append("];")
                End If ' _dataTable.Rows.Count > 0 Then

            End If ' Not IsNothing(folderTable) Then


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab0_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally


        End Try

        Return htmlOut
        htmlOut = Nothing
    End Function


    Public Function AircraftBuildNote_Portfolio(ByVal ID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal typeOfNote As String) As String
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
        Try
            'If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag Then 'make sure the display is correct on the listing page
            If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                '  If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag = True Then
                ResultsTable = aclsData_Temp.AIRCRAFT_LISTING_DUAL_Notes_LIMIT(typeOfNote, ID, "A", "JETNET", Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()))
                If Not IsNothing(ResultsTable) Then
                    If ResultsTable.Rows.Count > 0 Then
                        ReturnString = "<i class=""fa-thumb-tack"" alt='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "'></i>"
                    End If
                Else
                    Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " BuildNote()Ser: " & Replace(aclsData_Temp.class_error, "'", "''"), Nothing, 0, 0, 0, 0, 0)
                End If
                ' End If
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                If typeOfNote = "AC" Or typeOfNote = "YACHT" Then
                    ResultsTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(ID, "A", aircraft, company, Yacht, True)
                    If Not IsNothing(ResultsTable) Then
                        If ResultsTable.Rows.Count > 0 Then
                            ReturnString = "<i class=""fa-thumb-tack"" alt='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "'></i>"
                        End If
                    Else
                        Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " BuildNote()Cl: " & Replace(aclsData_Temp.class_error, "'", "''"), Nothing, 0, 0, 0, 0, 0)
                    End If
                End If
            End If
            'End If
            ' End If

        Catch ex As Exception
            commonLogFunctions.forceLogError("ERROR", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " (" + ex.ToString.Trim + "):")
        End Try
        ReturnString = clsGeneral.clsGeneral.PrepForJS(ReturnString)
        Return ReturnString

    End Function
    'Public Sub display_tab0_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

    '  Dim htmlOut As New StringBuilder
    '  Dim toggleRowColor As Boolean = False

    '  Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    '  Dim SqlConn As New System.Data.SqlClient.SqlConnection
    '  Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    '  Dim _dataTable As New DataTable
    '  Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    '  Dim sQuery As New StringBuilder()

    '  Try

    '    out_htmlString = ""

    '    If Not String.IsNullOrEmpty(aclist) Then

    '      SqlConn.ConnectionString = adminConnectString

    '      SqlConn.Open()

    '      SqlCommand.Connection = SqlConn
    '      SqlCommand.CommandType = System.Data.CommandType.Text
    '      SqlCommand.CommandTimeout = 90

    '      sQuery.Append("SELECT ac_id, amod_make_name as MAKE,")
    '      sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
    '      sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
    '      sQuery.Append(" acs_name as LIFECYCLE, acot_name as OWNERSHIP,")
    '      sQuery.Append(" (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = 0 AND cref_contact_type IN('00','17','08') AND comp_active_flag = 'Y')) AS OWNER,")
    '      sQuery.Append(" (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = 0 AND cref_operator_flag IN('Y','O') AND comp_active_flag = 'Y')) AS OPERATOR,")
    '      sQuery.Append(" ac_aport_name as BASEAPORT,")
    '      sQuery.Append(" ac_aport_country as BASECOUNTRY,")
    '      sQuery.Append(" ac_est_airframe_hrs as ESTAIRFRAMEHRS")
    '      sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
    '      sQuery.Append(aclist)
    '      sQuery.Append(") AND ac_journ_id = 0 ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

    '      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab0_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

    '      SqlCommand.CommandText = sQuery.ToString
    '      _recordSet = SqlCommand.ExecuteReader()

    '      Try
    '        _dataTable.Load(_recordSet)
    '      Catch constrExc As System.Data.ConstraintException
    '        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
    '      End Try

    '      _recordSet.Close()
    '      _recordSet = Nothing

    '      If _dataTable.Rows.Count > 0 Then

    '        htmlOut.Append("<table id=""tab0_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
    '        htmlOut.Append("<thead><tr>")
    '        htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

    '        If isMobileDisplay Then
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th></th>")
    '        Else
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th>MAKE</th>")
    '          htmlOut.Append("<th>MODEL</th>")
    '          htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
    '          htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
    '          htmlOut.Append("<th>MFRYEAR</th>")
    '          htmlOut.Append("<th>DLVYEAR</th>")
    '          htmlOut.Append("<th>LIFECYCLE</th>")
    '          htmlOut.Append("<th>OWNERSHIP</th>")
    '          htmlOut.Append("<th>OWNER</th>")
    '          htmlOut.Append("<th>OPERATOR</th>")
    '          htmlOut.Append("<th>BASEAPORT</th>")
    '          htmlOut.Append("<th>BASECOUNTRY</th>")
    '          htmlOut.Append("<th>ESTAIRFRAMEHRS</th>")

    '        End If

    '        htmlOut.Append("</tr></thead><tbody>")

    '        Dim sSeparator As String = ""

    '        For Each r As DataRow In _dataTable.Rows

    '          htmlOut.Append("<tr>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MAKE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MAKE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MODEL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MODEL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")

    '          If Not IsDBNull(r.Item("SERNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
    '              htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
    '              htmlOut.Append(r.Item("SERNO").ToString.Trim)
    '              htmlOut.Append("</a>")
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("REGNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
    '              htmlOut.Append(r.Item("REGNO").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MFRYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("DLVYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("LIFECYCLE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("LIFECYCLE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("LIFECYCLE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OWNERSHIP")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OWNERSHIP").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OWNERSHIP").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OWNER")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OWNER").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OWNER").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OPERATOR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OPERATOR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OPERATOR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("BASEAPORT")) Then
    '            If Not String.IsNullOrEmpty(r.Item("BASEAPORT").ToString.Trim) Then
    '              htmlOut.Append(r.Item("BASEAPORT").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("BASECOUNTRY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("BASECOUNTRY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("BASECOUNTRY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ESTAIRFRAMEHRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ESTAIRFRAMEHRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ESTAIRFRAMEHRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("</tr>")

    '        Next

    '      End If ' _dataTable.Rows.Count > 0 Then

    '      htmlOut.Append("</tbody></table>")
    '      'htmlOut.Append("<div id=""tab0_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
    '      htmlOut.Append("<div id=""tab0_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

    '    End If ' Not IsNothing(folderTable) Then

    '    out_htmlString = htmlOut.ToString

    '  Catch ex As Exception

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab0_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

    '  Finally

    '    htmlOut = Nothing

    '  End Try

    'End Sub

    'Public Sub display_tab1_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByVal displayEValues As Boolean = False)

    '  Dim htmlOut As New StringBuilder
    '  Dim toggleRowColor As Boolean = False

    '  Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    '  Dim SqlConn As New System.Data.SqlClient.SqlConnection
    '  Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    '  Dim _dataTable As New DataTable
    '  Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    '  Dim sQuery As New StringBuilder()

    '  Try

    '    out_htmlString = ""

    '    If Not String.IsNullOrEmpty(aclist) Then

    '      SqlConn.ConnectionString = adminConnectString

    '      SqlConn.Open()

    '      SqlCommand.Connection = SqlConn
    '      SqlCommand.CommandType = System.Data.CommandType.Text
    '      SqlCommand.CommandTimeout = 90

    '      sQuery.Append("SELECT * ")
    '      If displayEValues Then
    '        sQuery.Append(", (select afmv_value from ReturnAssetInsighteValue(ac_id)) as EVAL ")
    '        sQuery.Append(", (select AVGMODYREVALUE from ReturnAssetInsightModelYeareValue(ac_id)) as AVGMODYRE ")
    '      Else
    '        sQuery.Append(", NULL as EVAL ")
    '        sQuery.Append(", NULL as AVGMODYRE ")
    '      End If

    '      sQuery.Append(" FROM Aircraft_Flat with (NOLOCK) WHERE ac_id IN (")
    '      sQuery.Append(aclist)
    '      sQuery.Append(") AND ac_journ_id = 0 ORDER BY ac_ser_no_sort ASC")

    '      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab1_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

    '      SqlCommand.CommandText = sQuery.ToString
    '      _recordSet = SqlCommand.ExecuteReader()

    '      Try
    '        _dataTable.Load(_recordSet)
    '      Catch constrExc As System.Data.ConstraintException
    '        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
    '      End Try

    '      _recordSet.Close()
    '      _recordSet = Nothing

    '      If _dataTable.Rows.Count > 0 Then

    '        htmlOut.Append("<table id=""tab1_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
    '        htmlOut.Append("<thead><tr>")
    '        htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

    '        If isMobileDisplay Then
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th></th>")
    '        Else
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th>MAKE</th>")
    '          htmlOut.Append("<th>MODEL</th>")
    '          htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
    '          htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
    '          htmlOut.Append("<th>YEAR MFR</th>")
    '          htmlOut.Append("<th>YEAR DLV</th>")
    '          htmlOut.Append("<th>STATUS</th>")
    '          htmlOut.Append("<th>ASKING</th>")
    '          If displayEValues Then
    '            htmlOut.Append("<th class=""" + HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS.ToString + """>" + Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) + "</th>")
    '            htmlOut.Append("<th class=""" + HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS.ToString + """>MODEL YEAR AVG " + Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) + "</th>")
    '          End If
    '          htmlOut.Append("<th>DATE LISTED</th>")
    '          htmlOut.Append("<th>AFTT</th>")
    '          htmlOut.Append("<th>ENGINE&nbsp;TT</th>")
    '          htmlOut.Append("<th title='Number Of Passengers'>PAX</th>")
    '          htmlOut.Append("<th>INT<br />YEAR</th>")
    '          htmlOut.Append("<th>EXT<br />YEAR</th>")
    '          htmlOut.Append("<th>BASED</th>")
    '        End If

    '        htmlOut.Append("</tr></thead><tbody>")

    '        Dim sSeparator As String = ""

    '        For Each r As DataRow In _dataTable.Rows

    '          htmlOut.Append("<tr>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("amod_make_name")) Then
    '            If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString.Trim) Then
    '              htmlOut.Append(r.Item("amod_make_name").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("amod_model_name")) Then
    '            If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then
    '              htmlOut.Append(r.Item("amod_model_name").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")

    '          If Not IsDBNull(r.Item("ac_ser_no_full")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString.Trim) Then
    '              htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
    '              htmlOut.Append(r.Item("ac_ser_no_full").ToString.Trim)
    '              htmlOut.Append("</a>")
    '            End If
    '          End If

    '          htmlOut.Append("</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ac_reg_no")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ac_reg_no").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r("ac_mfr_year")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_mfr_year")) Then
    '              If CDbl(r.Item("ac_mfr_year").ToString) = 0 Then
    '                htmlOut.Append("0")
    '              Else
    '                htmlOut.Append(r.Item("ac_mfr_year").ToString)
    '              End If
    '            End If
    '          Else
    '            htmlOut.Append("U")
    '          End If

    '          htmlOut.Append("</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r("ac_year")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_year")) Then
    '              If CDbl(r.Item("ac_year").ToString) = 0 Then
    '                htmlOut.Append("0")
    '              Else
    '                htmlOut.Append(r.Item("ac_year").ToString)
    '              End If
    '            End If
    '          Else
    '            htmlOut.Append("U")
    '          End If

    '          htmlOut.Append("</td>")

    '          If Not IsDBNull(r.Item("ac_forsale_flag")) Then

    '            If r.Item("ac_forsale_flag").ToString.Trim.ToUpper.Contains("Y") Then

    '              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_status").ToString.Trim + "</td>")

    '              If Not IsDBNull(r.Item("ac_asking")) Then
    '                If r.Item("ac_asking").ToString.Trim.ToLower.Contains("price") Then
    '                  If Not IsDBNull(r.Item("ac_asking_price")) Then

    '                    Dim tmpPrice As Long = 0
    '                    If IsNumeric(r.Item("ac_asking_price").ToString) Then
    '                      If CLng(r.Item("ac_asking_price").ToString) > 0 Then
    '                        tmpPrice = CLng(r.Item("ac_asking_price").ToString) / 1000
    '                      End If
    '                    End If

    '                    htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap""  data-sort=""" + tmpPrice.ToString + """>$" + FormatNumber(tmpPrice, 0, False, False, True) + "k</td>")
    '                  Else
    '                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""0"">&nbsp;</td>")
    '                  End If
    '                Else
    '                  htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_asking").ToString.Trim + "</td>")
    '                End If
    '              Else
    '                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '              End If

    '            Else
    '              If Not IsDBNull(r.Item("ac_status")) Then
    '                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_status").ToString.Trim + "</td>")
    '                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '              Else
    '                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '              End If
    '            End If

    '          Else
    '            If Not IsDBNull(r.Item("ac_status")) Then
    '              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_status").ToString.Trim + "</td>")
    '              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '            Else
    '              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '            End If
    '          End If
    '          If displayEValues Then
    '            htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"" class=""" + HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS.ToString + """>")

    '            If Not IsNothing(_dataTable.Columns("EVAL")) Then
    '              If Not IsDBNull(r.Item("EVAL")) Then
    '                If Not String.IsNullOrEmpty(r.Item("EVAL").ToString.Trim) Then
    '                  htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("EVAL")))
    '                End If
    '              End If
    '            Else
    '              htmlOut.Append("&nbsp;")
    '            End If

    '            htmlOut.Append("</td>")

    '            htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"" class=""" + HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS.ToString + """>")

    '            If Not IsNothing(_dataTable.Columns("AVGMODYRE")) Then
    '              If Not IsDBNull(r.Item("AVGMODYRE")) Then
    '                If Not String.IsNullOrEmpty(r.Item("AVGMODYRE").ToString.Trim) Then
    '                  htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("AVGMODYRE")))
    '                End If
    '              End If
    '            Else
    '              htmlOut.Append("&nbsp;")
    '            End If

    '            htmlOut.Append("</td>")
    '          End If
    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap""")

    '          Dim dateSort As String = ""
    '          If Not IsDBNull(r.Item("ac_list_date")) Then
    '            If IsDate(r.Item("ac_list_date").ToString) Then
    '              dateSort = Format(r.Item("ac_list_date"), "yyyy/MM/dd")
    '            End If
    '          End If

    '          htmlOut.Append("data-sort=""" + dateSort + """>")

    '          If Not IsDBNull(r.Item("ac_list_date")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_list_date").ToString.Trim) Then
    '              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate)))
    '            Else
    '              htmlOut.Append("&nbsp;")
    '            End If
    '          Else
    '            htmlOut.Append("&nbsp;")
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ac_airframe_tot_hrs")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_airframe_tot_hrs").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ac_airframe_tot_hrs").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
    '            If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
    '              htmlOut.Append("[0]&nbsp;")
    '            Else
    '              htmlOut.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "]&nbsp;")
    '            End If
    '          Else
    '            htmlOut.Append("[U]&nbsp;")
    '          End If

    '          If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
    '            If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
    '              htmlOut.Append("[0]&nbsp;")
    '            Else
    '              htmlOut.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "]&nbsp;")
    '            End If
    '          Else
    '            htmlOut.Append("[U]&nbsp;")
    '          End If

    '          If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
    '            If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
    '              htmlOut.Append("[0]&nbsp;")
    '            Else
    '              htmlOut.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "]&nbsp;")
    '            End If
    '          End If

    '          If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
    '            If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
    '              htmlOut.Append("[0]&nbsp;")
    '            Else
    '              htmlOut.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "]&nbsp;")
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsNothing(_dataTable.Columns("ac_passenger_count")) Then
    '            If Not IsDBNull(r.Item("ac_passenger_count")) Then
    '              If Not String.IsNullOrEmpty(r.Item("ac_passenger_count").ToString.Trim) Then
    '                htmlOut.Append(r.Item("ac_passenger_count").ToString.Trim)
    '              End If
    '            End If
    '          Else
    '            htmlOut.Append("&nbsp;")
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsNothing(_dataTable.Columns("ac_interior_moyear")) Then

    '            If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
    '              htmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)

    '              If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
    '                htmlOut.Append("/")
    '              End If
    '              htmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
    '            Else
    '              htmlOut.Append("&nbsp;")
    '            End If
    '          Else
    '            htmlOut.Append("&nbsp;")
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsNothing(_dataTable.Columns("ac_exterior_moyear")) Then

    '            If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
    '              htmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)

    '              If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
    '                htmlOut.Append("/")

    '              End If
    '              htmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + "&nbsp;")
    '            Else
    '              htmlOut.Append("&nbsp;")
    '            End If
    '          Else
    '            htmlOut.Append("&nbsp;")
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsNothing(_dataTable.Columns("ac_aport_city")) And Not IsNothing(_dataTable.Columns("ac_aport_country")) Then

    '            Dim AportInfo As String = ""
    '            If Not IsDBNull(r.Item("ac_aport_city")) Then
    '              If Not String.IsNullOrEmpty(r.Item("ac_aport_city")) Then
    '                AportInfo = (("" + r.Item("ac_aport_city").ToString.Trim + ""))
    '              End If
    '            End If
    '            If Not IsDBNull(r.Item("ac_aport_country")) Then
    '              If Not String.IsNullOrEmpty(r.Item("ac_aport_country")) Then
    '                If AportInfo <> "" Then
    '                  AportInfo += ", "
    '                End If
    '                AportInfo += ((" " + Replace(r.Item("ac_aport_country").ToString.Trim, "United States", "US") + ""))
    '              End If
    '            End If

    '            htmlOut.Append(AportInfo + "</td>")

    '          Else
    '            htmlOut.Append("&nbsp;</td>")
    '          End If


    '          htmlOut.Append("</tr>")

    '        Next

    '      End If ' _dataTable.Rows.Count > 0 Then

    '      htmlOut.Append("</tbody></table>")
    '      'htmlOut.Append("<div id=""tab1_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
    '      htmlOut.Append("<div id=""tab1_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

    '    End If ' Not IsNothing(folderTable) Then

    '    out_htmlString = htmlOut.ToString

    '  Catch ex As Exception

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab1_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

    '  Finally

    '    htmlOut = Nothing

    '  End Try

    'End Sub

    Public Function display_tab1_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByVal displayEValues As Boolean = False, Optional ByVal show_Notes As Boolean = False) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()
        Dim count As Integer = 0
        Try
            Dim aclsData_Temp = New clsData_Manager_SQL
            If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                aclsData_Temp.JETNET_DB = clientConnectString
            End If


            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT * ")
                If displayEValues Then
                    sQuery.Append(", (select afmv_value from ReturnAssetInsighteValue(ac_id)) as EVAL ")
                    sQuery.Append(", (select AVGMODYREVALUE from ReturnAssetInsightModelYeareValue(ac_id)) as AVGMODYRE ")
                Else
                    sQuery.Append(", NULL as EVAL ")
                    sQuery.Append(", NULL as AVGMODYRE ")
                End If

                sQuery.Append(" FROM Aircraft_Flat with (NOLOCK) WHERE ac_forsale_flag = 'Y' and ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 ")
                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                sQuery.Append("ORDER BY ac_ser_no_sort ASC")

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab1_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        AllowExport = False
                    End If

                    htmlOut.Append(" var tab1DataSet  = [ ")
                    Dim sSeparator As String = ""

                    For Each r As DataRow In _dataTable.Rows

                        If count > 0 Then
                            htmlOut.Append(",")
                        End If

                        htmlOut.Append("{")
                        htmlOut.Append("""check"": """",") 'Checkbox row.
                        htmlOut.Append("""id"": """) 'id row
                        htmlOut.Append(r.Item("ac_id").ToString.Trim)
                        htmlOut.Append(""",")


                        htmlOut.Append("""note"": """)

                        If show_Notes = True Then
                            htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                        Else
                            htmlOut.Append("")
                        End If

                        htmlOut.Append(""",")


                        htmlOut.Append("""make"": """) 'make row

                        If Not IsDBNull(r.Item("amod_make_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("amod_make_name").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""model"": """) 'model row
                        If Not IsDBNull(r.Item("amod_model_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("amod_model_name").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("amod_make_name") & " " & r("amod_model_name") & " S/N #" & r("ac_ser_no_full") & """>" & r("ac_ser_no_full").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")
                        htmlOut.Append("""reg"": """)

                        If Not IsDBNull(r.Item("ac_reg_no")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString.Trim) Then
                                htmlOut.Append(r.Item("ac_reg_no").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""mfryear"": """)

                        If Not IsDBNull(r.Item("ac_mfr_year")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_mfr_year").ToString.Trim) Then
                                htmlOut.Append(r.Item("ac_mfr_year").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""dlvyear"": """)

                        If Not IsDBNull(r.Item("ac_year")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_year").ToString.Trim) Then
                                htmlOut.Append(r.Item("ac_year").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        If Not IsDBNull(r.Item("ac_forsale_flag")) Then

                            If r.Item("ac_forsale_flag").ToString.Trim.ToUpper.Contains("Y") Then

                                htmlOut.Append("""status"": ""<span class=\""green_text\"">")
                                htmlOut.Append(r.Item("ac_status").ToString.Trim)
                                htmlOut.Append("</span>"",")

                                htmlOut.Append("")
                                If Not IsDBNull(r.Item("ac_asking")) Then
                                    If r.Item("ac_asking").ToString.Trim.ToLower.Contains("price") Then
                                        If Not IsDBNull(r.Item("ac_asking_price")) Then

                                            Dim tmpPrice As Long = 0
                                            If IsNumeric(r.Item("ac_asking_price").ToString) Then
                                                If CLng(r.Item("ac_asking_price").ToString) > 0 Then
                                                    tmpPrice = CLng(r.Item("ac_asking_price").ToString) / 1000
                                                End If
                                            End If

                                            htmlOut.Append("""asking"": [""<span class=\""green_text\"">$" + FormatNumber(tmpPrice, 0, False, False, True) + "k</span>"",""" & tmpPrice.ToString & """],")
                                        Else
                                            htmlOut.Append("""asking"": ["""",""0""],")
                                        End If
                                    Else
                                        htmlOut.Append("""asking"": ["""",""0""],")
                                    End If
                                Else
                                    htmlOut.Append("""asking"": ["""",""0""],")
                                End If

                            Else
                                If Not IsDBNull(r.Item("ac_status")) Then
                                    htmlOut.Append("""status"": """)
                                    htmlOut.Append(r.Item("ac_status").ToString.Trim)
                                    htmlOut.Append(""",")
                                    htmlOut.Append("""asking"": ["""",""0""],")
                                Else
                                    htmlOut.Append("""status"": """",")
                                    htmlOut.Append("""asking"": ["""",""0""],")
                                End If
                            End If

                        Else
                            If Not IsDBNull(r.Item("ac_status")) Then
                                htmlOut.Append("""status"": """)
                                htmlOut.Append(r.Item("ac_status").ToString.Trim)
                                htmlOut.Append(""",")
                                htmlOut.Append("""asking"": ["""",""0""],")
                            Else
                                htmlOut.Append("""status"": """",")
                                htmlOut.Append("""asking"": ["""",""0""],")
                            End If
                        End If


                        If displayEValues Then
                            htmlOut.Append("""eval"": """)

                            If Not IsNothing(_dataTable.Columns("EVAL")) Then
                                If Not IsDBNull(r.Item("EVAL")) Then
                                    If Not String.IsNullOrEmpty(r.Item("EVAL").ToString.Trim) Then
                                        htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("EVAL")))
                                    End If
                                End If
                            Else
                                htmlOut.Append("&nbsp;")
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""avgmod"": """)
                            If Not IsNothing(_dataTable.Columns("AVGMODYRE")) Then
                                If Not IsDBNull(r.Item("AVGMODYRE")) Then
                                    If Not String.IsNullOrEmpty(r.Item("AVGMODYRE").ToString.Trim) Then
                                        htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("AVGMODYRE")))
                                    End If
                                End If
                            Else
                                htmlOut.Append("&nbsp;")
                            End If

                            htmlOut.Append(""",")
                        End If

                        htmlOut.Append("""listdate"": [")
                        Dim dateSort As String = ""
                        If Not IsDBNull(r.Item("ac_list_date")) Then
                            If IsDate(r.Item("ac_list_date").ToString) Then
                                dateSort = Format(r.Item("ac_list_date"), "yyyy/MM/dd")
                            End If
                        End If

                        If Not IsDBNull(r.Item("ac_list_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_list_date").ToString.Trim) Then

                                If Not IsDBNull(r.Item("ac_forsale_flag")) Then
                                    If r.Item("ac_forsale_flag").ToString.Trim.ToUpper.Contains("Y") Then
                                        htmlOut.Append("""<span class=\""green_text\"">" & CommonAircraftFunctions.trim_out_year_start(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate)) & "</span>"",""" & dateSort & """")
                                    Else
                                        htmlOut.Append("""" & CommonAircraftFunctions.trim_out_year_start(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate)) & """,""" & dateSort & """")
                                    End If
                                Else
                                    htmlOut.Append("""" & CommonAircraftFunctions.trim_out_year_start(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate)) & """,""" & dateSort & """")
                                End If

                            Else
                                htmlOut.Append(""""",""0""")
                            End If
                        Else
                            htmlOut.Append(""""",""""")
                        End If

                        htmlOut.Append("],")

                        htmlOut.Append("""tothrs"": """)

                        If Not IsDBNull(r.Item("ac_airframe_tot_hrs")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_airframe_tot_hrs").ToString.Trim) Then
                                htmlOut.Append(r.Item("ac_airframe_tot_hrs").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""tothrs1"": """)

                        If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
                            If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("\[0]&nbsp;")
                            Else
                                htmlOut.Append("\[" + r.Item("ac_engine_1_tot_hrs").ToString + "\]&nbsp;")
                            End If
                        Else
                            htmlOut.Append("\[U\]&nbsp;")
                        End If

                        If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
                            If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("\[0\]&nbsp;")
                            Else
                                htmlOut.Append("\[" + r.Item("ac_engine_2_tot_hrs").ToString + "\]&nbsp;")
                            End If
                        Else
                            htmlOut.Append("\[U\]&nbsp;")
                        End If

                        If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
                            If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("\[0\]&nbsp;")
                            Else
                                htmlOut.Append("\[" + r.Item("ac_engine_3_tot_hrs").ToString + "\]&nbsp;")
                            End If
                        End If

                        If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
                            If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("\[0\]&nbsp;")
                            Else
                                htmlOut.Append("\[" + r.Item("ac_engine_4_tot_hrs").ToString + "\]&nbsp;")
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""PAX"": """)

                        If Not IsNothing(_dataTable.Columns("ac_passenger_count")) Then
                            If Not IsDBNull(r.Item("ac_passenger_count")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_passenger_count").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_passenger_count").ToString.Trim)
                                End If
                            End If
                        Else
                            htmlOut.Append("&nbsp;")
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""intyear"": """)

                        If Not IsNothing(_dataTable.Columns("ac_interior_moyear")) Then

                            If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                                htmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)

                                If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
                                    htmlOut.Append("/")
                                End If
                                htmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
                            Else
                                htmlOut.Append("&nbsp;")
                            End If
                        Else
                            htmlOut.Append("&nbsp;")
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""extyear"": """)

                        If Not IsNothing(_dataTable.Columns("ac_exterior_moyear")) Then

                            If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                                htmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)

                                If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
                                    htmlOut.Append("/")

                                End If
                                htmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + "&nbsp;")
                            Else
                                htmlOut.Append("&nbsp;")
                            End If
                        Else
                            htmlOut.Append("&nbsp;")
                        End If

                        htmlOut.Append(""",")


                        htmlOut.Append("""based"": """)

                        If Not IsNothing(_dataTable.Columns("ac_aport_city")) And Not IsNothing(_dataTable.Columns("ac_aport_country")) Then

                            Dim AportInfo As String = ""
                            If Not IsDBNull(r.Item("ac_aport_city")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_city")) Then
                                    AportInfo = (("" + r.Item("ac_aport_city").ToString.Trim + ""))
                                End If
                            End If
                            If Not IsDBNull(r.Item("ac_aport_country")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_country")) Then
                                    If AportInfo <> "" Then
                                        AportInfo += ", "
                                    End If
                                    AportInfo += ((" " + Replace(r.Item("ac_aport_country").ToString.Trim, "United States", "US") + ""))
                                End If
                            End If

                            htmlOut.Append(AportInfo)

                        Else
                            htmlOut.Append("&nbsp;")
                        End If

                        htmlOut.Append("""")
                        htmlOut.Append("}")
                        count += 1
                    Next
                    htmlOut.Append("];")

                End If ' _dataTable.Rows.Count > 0 Then


            End If ' Not IsNothing(folderTable) Then


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab1_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally



        End Try
        Return htmlOut
        htmlOut = Nothing

    End Function
    Public Function display_tab2_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByVal summarize_field As String = "", Optional ByRef datatable_dropdown As DataTable = Nothing, Optional ByRef Show_Notes As Boolean = False) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Try

            Dim aclsData_Temp = New clsData_Manager_SQL
            If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                aclsData_Temp.JETNET_DB = clientConnectString
            End If

            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90


                If Trim(summarize_field) = "" Then
                    sQuery.Append("SELECT ac_id, amod_make_name AS MAKE,")
                    sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year AS MODYEAR,")
                    sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
                    sQuery.Append(" ac_est_airframe_hrs AS ESTAIRFRAMEHOURS, amp_program_name AS AIRFRAMEMAINTPROGRAM,")
                    sQuery.Append(" amtp_program_name AS AIRFRAMETRACKPROGRAM, ac_maintained AS MAINTAINED,")
                    sQuery.Append(" ac_engine_name AS ENGINEMODELNAME, emp_program_name AS ENGINEMAINTPROGRAM,")
                    sQuery.Append(" ac_engine_1_tot_hrs AS ENG1HRS, ac_engine_2_tot_hrs AS ENG2HRS,")
                    sQuery.Append(" ac_engine_1_soh_hrs AS ENG1SOHHRS, ac_engine_2_soh_hrs AS ENG2SOHHRS,")
                    sQuery.Append(" ac_apu_model_name AS APUMODELNAME, (SELECT TOP 1 emp_name FROM Engine_Maintenance_Program WITH (NOLOCK) where emp_code = ac_apu_maint_prog) AS APUPROGRAMNAME,")
                    sQuery.Append(" ac_interior_moyear AS INTERIORDATE, ac_interior_doneby_name AS INTERIORDONEBY,")
                    sQuery.Append(" ac_exterior_moyear AS EXTERIORDATE, ac_exterior_doneby_name AS EXTERIORDONEBY,")
                    sQuery.Append(" ac_damage_history_notes AS DAMAGE, ac_airframe_tot_hrs AS LASTREPORTEDHRS,")
                    sQuery.Append(" ac_airframe_tot_landings AS LASTREPORTEDCYCLES, ac_times_as_of_date AS LASTREPORTEDDATE")
                    sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
                    sQuery.Append(aclist)
                    sQuery.Append(") AND ac_journ_id = 0 ")
                    sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                    sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")
                Else
                    sQuery.Append("SELECT distinct " & Trim(summarize_field) & " as 'Summarized', count(distinct ac_id) as tcount ")
                    sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
                    sQuery.Append(aclist)
                    sQuery.Append(") AND ac_journ_id = 0 ")
                    sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                    sQuery.Append(" group by " & Trim(summarize_field) & "  ORDER BY count(distinct ac_id) desc ")
                End If


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab2_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing


                If Not IsNothing(datatable_dropdown) And Not IsNothing(_dataTable) Then
                    datatable_dropdown = _dataTable
                End If

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        AllowExport = False
                    End If

                    htmlOut.Append(" var tab2DataSet  = [ ")

                    Dim sSeparator As String = ""

                    For Each r As DataRow In _dataTable.Rows


                        If Trim(summarize_field) = "" Then

                            If count > 0 Then
                                htmlOut.Append(",")
                            End If

                            htmlOut.Append("{")
                            htmlOut.Append("""check"": """",") 'Checkbox row.
                            htmlOut.Append("""id"": """) 'id row
                            htmlOut.Append(r.Item("ac_id").ToString.Trim)
                            htmlOut.Append(""",")


                            htmlOut.Append("""note"": """)

                            If Show_Notes = True Then
                                htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                            Else
                                htmlOut.Append("")
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""make"": """) 'make row

                            If Not IsDBNull(r.Item("MAKE")) Then
                                If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MAKE").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append(""",")

                            htmlOut.Append("""model"": """) 'model row
                            If Not IsDBNull(r.Item("MODEL")) Then
                                If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MODEL").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append(""",")

                            htmlOut.Append("""modyear"": """) 'model year row

                            If Not IsDBNull(r.Item("MODYEAR")) Then
                                If Not String.IsNullOrEmpty(r.Item("MODYEAR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MODYEAR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("MAKE") & " " & r("MODEL") & " S/N #" & r("SERNO") & """>" & r("SERNO").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")
                            htmlOut.Append("""reg"": """)

                            If Not IsDBNull(r.Item("REGNO")) Then
                                If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
                                    htmlOut.Append(r.Item("REGNO").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")


                            htmlOut.Append("""estairframe"": """) 'ESTAIRFRAMEHOURS

                            If Not IsDBNull(r.Item("ESTAIRFRAMEHOURS")) Then
                                If Not String.IsNullOrEmpty(r.Item("ESTAIRFRAMEHOURS").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ESTAIRFRAMEHOURS").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""airmaint"": """) 'Air maintenantance

                            If Not IsDBNull(r.Item("AIRFRAMEMAINTPROGRAM")) Then
                                If Not String.IsNullOrEmpty(r.Item("AIRFRAMEMAINTPROGRAM").ToString.Trim) Then
                                    htmlOut.Append(r.Item("AIRFRAMEMAINTPROGRAM").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""airtrack"": """) 'Air maintenantance
                            If Not IsDBNull(r.Item("AIRFRAMETRACKPROGRAM")) Then
                                If Not String.IsNullOrEmpty(r.Item("AIRFRAMETRACKPROGRAM").ToString.Trim) Then
                                    htmlOut.Append(r.Item("AIRFRAMETRACKPROGRAM").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")


                            htmlOut.Append("""maintained"": """) 'Maintained

                            If Not IsDBNull(r.Item("MAINTAINED")) Then
                                If Not String.IsNullOrEmpty(r.Item("MAINTAINED").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MAINTAINED").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""enginemodel"": """) 'Maintained

                            If Not IsDBNull(r.Item("ENGINEMODELNAME")) Then
                                If Not String.IsNullOrEmpty(r.Item("ENGINEMODELNAME").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ENGINEMODELNAME").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""enginemaint"": """)

                            If Not IsDBNull(r.Item("ENGINEMAINTPROGRAM")) Then
                                If Not String.IsNullOrEmpty(r.Item("ENGINEMAINTPROGRAM").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ENGINEMAINTPROGRAM").ToString.Trim)
                                End If
                            End If


                            htmlOut.Append(""",")


                            htmlOut.Append("""eng1"": """)

                            If Not IsDBNull(r.Item("ENG1HRS")) Then
                                If Not String.IsNullOrEmpty(r.Item("ENG1HRS").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ENG1HRS").ToString.Trim)
                                End If
                            End If


                            htmlOut.Append(""",")

                            htmlOut.Append("""eng2"": """)

                            If Not IsDBNull(r.Item("ENG2HRS")) Then
                                If Not String.IsNullOrEmpty(r.Item("ENG2HRS").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ENG2HRS").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""eng1so"": """)

                            If Not IsDBNull(r.Item("ENG1SOHHRS")) Then
                                If Not String.IsNullOrEmpty(r.Item("ENG1SOHHRS").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ENG1SOHHRS").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""eng2so"": """)

                            If Not IsDBNull(r.Item("ENG2SOHHRS")) Then
                                If Not String.IsNullOrEmpty(r.Item("ENG2SOHHRS").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ENG2SOHHRS").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")


                            htmlOut.Append("""apumodel"": """)

                            If Not IsDBNull(r.Item("APUMODELNAME")) Then
                                If Not String.IsNullOrEmpty(r.Item("APUMODELNAME").ToString.Trim) Then
                                    htmlOut.Append(r.Item("APUMODELNAME").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""apuprogram"": """)

                            If Not IsDBNull(r.Item("APUPROGRAMNAME")) Then
                                If Not String.IsNullOrEmpty(r.Item("APUPROGRAMNAME").ToString.Trim) Then
                                    htmlOut.Append(r.Item("APUPROGRAMNAME").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""interiordate"": """)

                            If Not IsDBNull(r.Item("INTERIORDATE")) Then
                                If Not String.IsNullOrEmpty(r.Item("INTERIORDATE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("INTERIORDATE").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""interiordone"": """)

                            If Not IsDBNull(r.Item("INTERIORDONEBY")) Then
                                If Not String.IsNullOrEmpty(r.Item("INTERIORDONEBY").ToString.Trim) Then
                                    htmlOut.Append(r.Item("INTERIORDONEBY").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")


                            htmlOut.Append("""exteriordate"": """)

                            If Not IsDBNull(r.Item("EXTERIORDATE")) Then
                                If Not String.IsNullOrEmpty(r.Item("EXTERIORDATE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("EXTERIORDATE").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")


                            htmlOut.Append("""exteriordone"": """)

                            If Not IsDBNull(r.Item("EXTERIORDONEBY")) Then
                                If Not String.IsNullOrEmpty(r.Item("EXTERIORDONEBY").ToString.Trim) Then
                                    htmlOut.Append(r.Item("EXTERIORDONEBY").ToString.Trim)
                                End If
                            End If


                            htmlOut.Append(""",")

                            htmlOut.Append("""lastreportedhrs"": """)


                            If Not IsDBNull(r.Item("LASTREPORTEDHRS")) Then
                                If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDHRS").ToString.Trim) Then
                                    htmlOut.Append(r.Item("LASTREPORTEDHRS").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""lastreportedcycles"": """)

                            If Not IsDBNull(r.Item("LASTREPORTEDCYCLES")) Then
                                If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDCYCLES").ToString.Trim) Then
                                    htmlOut.Append(r.Item("LASTREPORTEDCYCLES").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""lastreporteddate"": """)

                            If Not IsDBNull(r.Item("LASTREPORTEDDATE")) Then
                                If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDDATE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("LASTREPORTEDDATE").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""damage"": """)

                            If Not IsDBNull(r.Item("DAMAGE")) Then
                                If Not String.IsNullOrEmpty(r.Item("DAMAGE").ToString.Trim) Then
                                    htmlOut.Append(PrepForJS(r.Item("DAMAGE").ToString.Trim))
                                End If
                            End If


                            htmlOut.Append("""")

                            htmlOut.Append("}")
                            count += 1
                        Else

                            If count > 0 Then
                                htmlOut.Append(",")
                            End If
                            htmlOut.Append("{")
                            htmlOut.Append("""check"": """",") 'Checkbox row. 
                            htmlOut.Append("""id"": """ & count & """,") 'hidden ID
                            htmlOut.Append("""Summarized"": ")

                            If Not IsDBNull(r.Item("Summarized")) Then
                                If Not String.IsNullOrEmpty(r.Item("Summarized").ToString.Trim) Then
                                    If r.Item("Summarized").ToString.ToLower = "unknown" Then
                                        htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                                    Else
                                        htmlOut.Append("[""" & r.Item("Summarized").ToString.Trim & """,""" & r.Item("Summarized").ToString.Trim & """]")
                                    End If

                                Else
                                    htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                                End If
                            Else
                                htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                            End If

                            htmlOut.Append(",")

                            htmlOut.Append("""Total"": """)

                            If Not IsDBNull(r.Item("tcount")) Then
                                If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then
                                    htmlOut.Append(r.Item("tcount").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("""")

                            'htmlOut.Append("""")
                            htmlOut.Append("}")
                            count += 1
                        End If



                    Next
                    htmlOut.Append("];")

                End If ' _dataTable.Rows.Count > 0 Then

                'htmlOut.Append("</tbody></table>")
                ''htmlOut.Append("<div id=""tab2_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
                'htmlOut.Append("<div id=""tab2_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

            End If ' Not IsNothing(folderTable) Then

            ' out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab2_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            '  htmlOut = Nothing

        End Try

        Return htmlOut
        htmlOut = Nothing
    End Function

    Public Shared Function PrepForJS(ByVal Text As Object) As String
        PrepForJS = ""
        If Not IsNothing(Text) Then
            Dim illegalChars As Char() = "&/\#,+()$~%..'"":*?<>{}"
            Dim str As String = Text.ToString
            Dim first As New System.Text.StringBuilder

            For Each ch As Char In str
                If Array.IndexOf(illegalChars, ch) = -1 Then
                    first.Append(ch)
                Else
                    first.Append("\" + ch)
                End If
            Next
            Return Replace(first.ToString, Environment.NewLine, "")
        End If
    End Function
    'Public Sub display_tab2_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

    '  Dim htmlOut As New StringBuilder
    '  Dim toggleRowColor As Boolean = False

    '  Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    '  Dim SqlConn As New System.Data.SqlClient.SqlConnection
    '  Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    '  Dim _dataTable As New DataTable
    '  Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    '  Dim sQuery As New StringBuilder()

    '  Try

    '    out_htmlString = ""

    '    If Not String.IsNullOrEmpty(aclist) Then

    '      SqlConn.ConnectionString = adminConnectString

    '      SqlConn.Open()

    '      SqlCommand.Connection = SqlConn
    '      SqlCommand.CommandType = System.Data.CommandType.Text
    '      SqlCommand.CommandTimeout = 90

    '      sQuery.Append("SELECT ac_id, amod_make_name AS MAKE,")
    '      sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year AS MODYEAR,")
    '      sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
    '      sQuery.Append(" ac_est_airframe_hrs AS ESTAIRFRAMEHOURS, amp_program_name AS AIRFRAMEMAINTPROGRAM,")
    '      sQuery.Append(" amtp_program_name AS AIRFRAMETRACKPROGRAM, ac_maintained AS MAINTAINED,")
    '      sQuery.Append(" ac_engine_name AS ENGINEMODELNAME, emp_program_name AS ENGINEMAINTPROGRAM,")
    '      sQuery.Append(" ac_engine_1_tot_hrs AS ENG1HRS, ac_engine_2_tot_hrs AS ENG2HRS,")
    '      sQuery.Append(" ac_engine_1_soh_hrs AS ENG1SOHHRS, ac_engine_2_soh_hrs AS ENG2SOHHRS,")
    '      sQuery.Append(" ac_apu_model_name AS APUMODELNAME, (SELECT TOP 1 emp_name FROM Engine_Maintenance_Program WITH (NOLOCK) where emp_code = ac_apu_maint_prog) AS APUPROGRAMNAME,")
    '      sQuery.Append(" ac_interior_moyear AS INTERIORDATE, ac_interior_doneby_name AS INTERIORDONEBY,")
    '      sQuery.Append(" ac_exterior_moyear AS EXTERIORDATE, ac_exterior_doneby_name AS EXTERIORDONEBY,")
    '      sQuery.Append(" ac_damage_history_notes AS DAMAGE, ac_airframe_tot_hrs AS LASTREPORTEDHRS,")
    '      sQuery.Append(" ac_airframe_tot_landings AS LASTREPORTEDCYCLES, ac_times_as_of_date AS LASTREPORTEDDATE")
    '      sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
    '      sQuery.Append(aclist)
    '      sQuery.Append(") AND ac_journ_id = 0 ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

    '      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab2_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

    '      SqlCommand.CommandText = sQuery.ToString
    '      _recordSet = SqlCommand.ExecuteReader()

    '      Try
    '        _dataTable.Load(_recordSet)
    '      Catch constrExc As System.Data.ConstraintException
    '        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
    '      End Try

    '      _recordSet.Close()
    '      _recordSet = Nothing

    '      If _dataTable.Rows.Count > 0 Then

    '        htmlOut.Append("<table id=""tab2_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
    '        htmlOut.Append("<thead><tr>")
    '        htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

    '        If isMobileDisplay Then
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th></th>")
    '        Else
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th>MAKE</th>")
    '          htmlOut.Append("<th>MODEL</th>")
    '          htmlOut.Append("<th>MODYEAR</th>")
    '          htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
    '          htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
    '          htmlOut.Append("<th>ESTAIRFRAMEHOURS</th>")
    '          htmlOut.Append("<th>AIRFRAMEMAINTPROGRAM</th>")
    '          htmlOut.Append("<th>AIRFRAMETRACKPROGRAM</th>")
    '          htmlOut.Append("<th>MAINTAINED</th>")
    '          htmlOut.Append("<th>ENGINEMODELNAME</th>")
    '          htmlOut.Append("<th>ENGINEMAINTPROGRAM</th>")
    '          htmlOut.Append("<th>ENG1HRS</th>")
    '          htmlOut.Append("<th>ENG2HRS</th>")
    '          htmlOut.Append("<th>ENG1SOHHRS</th>")
    '          htmlOut.Append("<th>ENG2SOHHRS</th>")
    '          htmlOut.Append("<th>APUMODELNAME</th>")
    '          htmlOut.Append("<th>APUPROGRAMNAME</th>")
    '          htmlOut.Append("<th>INTERIORDATE</th>")
    '          htmlOut.Append("<th>INTERIORDONEBY</th>")
    '          htmlOut.Append("<th>EXTERIORDATE</th>")
    '          htmlOut.Append("<th>EXTERIORDONEBY</th>")
    '          htmlOut.Append("<th>DAMAGE</th>")
    '          htmlOut.Append("<th>LASTREPORTEDHRS</th>")
    '          htmlOut.Append("<th>LASTREPORTEDCYCLES</th>")
    '          htmlOut.Append("<th>LASTREPORTEDDATE</th>")

    '        End If

    '        htmlOut.Append("</tr></thead><tbody>")

    '        Dim sSeparator As String = ""

    '        For Each r As DataRow In _dataTable.Rows

    '          htmlOut.Append("<tr>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MAKE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MAKE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MODEL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MODEL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MODYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MODYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MODYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")

    '          If Not IsDBNull(r.Item("SERNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
    '              htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
    '              htmlOut.Append(r.Item("SERNO").ToString.Trim)
    '              htmlOut.Append("</a>")
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("REGNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
    '              htmlOut.Append(r.Item("REGNO").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ESTAIRFRAMEHOURS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ESTAIRFRAMEHOURS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ESTAIRFRAMEHOURS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("AIRFRAMEMAINTPROGRAM")) Then
    '            If Not String.IsNullOrEmpty(r.Item("AIRFRAMEMAINTPROGRAM").ToString.Trim) Then
    '              htmlOut.Append(r.Item("AIRFRAMEMAINTPROGRAM").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("AIRFRAMETRACKPROGRAM")) Then
    '            If Not String.IsNullOrEmpty(r.Item("AIRFRAMETRACKPROGRAM").ToString.Trim) Then
    '              htmlOut.Append(r.Item("AIRFRAMETRACKPROGRAM").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MAINTAINED")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MAINTAINED").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MAINTAINED").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENGINEMODELNAME")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENGINEMODELNAME").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENGINEMODELNAME").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENGINEMAINTPROGRAM")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENGINEMAINTPROGRAM").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENGINEMAINTPROGRAM").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENG1HRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENG1HRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENG1HRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENG2HRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENG2HRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENG2HRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENG1SOHHRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENG1SOHHRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENG1SOHHRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENG2SOHHRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENG2SOHHRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENG2SOHHRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("APUMODELNAME")) Then
    '            If Not String.IsNullOrEmpty(r.Item("APUMODELNAME").ToString.Trim) Then
    '              htmlOut.Append(r.Item("APUMODELNAME").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("APUPROGRAMNAME")) Then
    '            If Not String.IsNullOrEmpty(r.Item("APUPROGRAMNAME").ToString.Trim) Then
    '              htmlOut.Append(r.Item("APUPROGRAMNAME").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("INTERIORDATE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("INTERIORDATE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("INTERIORDATE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("INTERIORDONEBY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("INTERIORDONEBY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("INTERIORDONEBY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("EXTERIORDATE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("EXTERIORDATE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("EXTERIORDATE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("EXTERIORDONEBY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("EXTERIORDONEBY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("EXTERIORDONEBY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("DAMAGE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("DAMAGE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("DAMAGE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("LASTREPORTEDHRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDHRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("LASTREPORTEDHRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("LASTREPORTEDCYCLES")) Then
    '            If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDCYCLES").ToString.Trim) Then
    '              htmlOut.Append(r.Item("LASTREPORTEDCYCLES").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("LASTREPORTEDDATE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDDATE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("LASTREPORTEDDATE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("</tr>")

    '        Next

    '      End If ' _dataTable.Rows.Count > 0 Then

    '      htmlOut.Append("</tbody></table>")
    '      'htmlOut.Append("<div id=""tab2_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
    '      htmlOut.Append("<div id=""tab2_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

    '    End If ' Not IsNothing(folderTable) Then

    '    out_htmlString = htmlOut.ToString

    '  Catch ex As Exception

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab2_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

    '  Finally

    '    htmlOut = Nothing

    '  End Try

    'End Sub

    'Public Sub display_tab3_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

    '  Dim htmlOut As New StringBuilder
    '  Dim toggleRowColor As Boolean = False

    '  Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    '  Dim SqlConn As New System.Data.SqlClient.SqlConnection
    '  Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    '  Dim _dataTable As New DataTable
    '  Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    '  Dim sQuery As New StringBuilder()

    '  Try

    '    out_htmlString = ""

    '    If Not String.IsNullOrEmpty(aclist) Then

    '      SqlConn.ConnectionString = adminConnectString

    '      SqlConn.Open()

    '      SqlCommand.Connection = SqlConn
    '      SqlCommand.CommandType = System.Data.CommandType.Text
    '      SqlCommand.CommandTimeout = 90

    '      sQuery.Append("SELECT ac_id, amod_make_name as MAKE,")
    '      sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MODYEAR,")
    '      sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
    '      sQuery.Append(" ac_est_airframe_hrs as ESTAIRFRAMEHOURS, amp_program_name as AIRFRAMEMAINTPROGRAM,")
    '      sQuery.Append(" amtp_program_name AS AIRFRAMETRACKPROGRAM, ac_maintained as MAINTAINED,")
    '      sQuery.Append(" ac_engine_name as ENGINEMODELNAME, emp_program_name as ENGINEMAINTPROGRAM,")
    '      sQuery.Append(" ac_engine_1_tot_hrs as ENG1HRS, ac_engine_2_tot_hrs as ENG2HRS,")
    '      sQuery.Append(" ac_engine_1_soh_hrs as ENG1SOHHRS, ac_engine_2_soh_hrs as ENG2SOHHRS,")
    '      sQuery.Append(" ac_apu_model_name as APUMODELNAME, (select top 1 emp_name from Engine_Maintenance_Program wtih (NOLOCK) where emp_code = ac_apu_maint_prog) as APUPROGRAMNAME,")
    '      sQuery.Append(" ac_interior_moyear as INTERIORDATE, ac_interior_doneby_name as INTERIORDONEBY,")
    '      sQuery.Append(" ac_exterior_moyear as EXTERIORDATE, ac_exterior_doneby_name as EXTERIORDONEBY,")
    '      sQuery.Append(" ac_damage_history_notes as DAMAGE, ac_airframe_tot_hrs AS LASTREPORTEDHRS,")
    '      sQuery.Append(" ac_airframe_tot_landings AS LASTREPORTEDCYCLES, ac_times_as_of_date LASTREPORTEDDATE")
    '      sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
    '      sQuery.Append(aclist)
    '      sQuery.Append(") AND ac_journ_id = 0 ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

    '      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab3_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

    '      SqlCommand.CommandText = sQuery.ToString
    '      _recordSet = SqlCommand.ExecuteReader()

    '      Try
    '        _dataTable.Load(_recordSet)
    '      Catch constrExc As System.Data.ConstraintException
    '        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
    '      End Try

    '      _recordSet.Close()
    '      _recordSet = Nothing

    '      If _dataTable.Rows.Count > 0 Then

    '        htmlOut.Append("<table id=""tab3_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
    '        htmlOut.Append("<thead><tr>")
    '        htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

    '        If isMobileDisplay Then
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th></th>")
    '        Else
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th>MAKE</th>")
    '          htmlOut.Append("<th>MODEL</th>")
    '          htmlOut.Append("<th>MODYEAR</th>")
    '          htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
    '          htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
    '          htmlOut.Append("<th>ESTAIRFRAMEHOURS</th>")
    '          htmlOut.Append("<th>AIRFRAMEMAINTPROGRAM</th>")
    '          htmlOut.Append("<th>AIRFRAMETRACKPROGRAM</th>")
    '          htmlOut.Append("<th>MAINTAINED</th>")
    '          htmlOut.Append("<th>ENGINEMODELNAME</th>")
    '          htmlOut.Append("<th>ENGINEMAINTPROGRAM</th>")
    '          htmlOut.Append("<th>ENG1HRS</th>")
    '          htmlOut.Append("<th>ENG2HRS</th>")
    '          htmlOut.Append("<th>ENG1SOHHRS</th>")
    '          htmlOut.Append("<th>ENG2SOHHRS</th>")
    '          htmlOut.Append("<th>APUMODELNAME</th>")
    '          htmlOut.Append("<th>APUPROGRAMNAME</th>")
    '          htmlOut.Append("<th>INTERIORDATE</th>")
    '          htmlOut.Append("<th>INTERIORDONEBY</th>")
    '          htmlOut.Append("<th>EXTERIORDATE</th>")
    '          htmlOut.Append("<th>EXTERIORDONEBY</th>")
    '          htmlOut.Append("<th>DAMAGE</th>")
    '          htmlOut.Append("<th>LASTREPORTEDHRS</th>")
    '          htmlOut.Append("<th>LASTREPORTEDCYCLES</th>")
    '          htmlOut.Append("<th>LASTREPORTEDDATE</th>")

    '        End If

    '        htmlOut.Append("</tr></thead><tbody>")

    '        Dim sSeparator As String = ""

    '        For Each r As DataRow In _dataTable.Rows

    '          htmlOut.Append("<tr>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MAKE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MAKE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MODEL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MODEL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MODYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MODYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MODYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")

    '          If Not IsDBNull(r.Item("SERNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
    '              htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
    '              htmlOut.Append(r.Item("SERNO").ToString.Trim)
    '              htmlOut.Append("</a>")
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("REGNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
    '              htmlOut.Append(r.Item("REGNO").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ESTAIRFRAMEHOURS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ESTAIRFRAMEHOURS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ESTAIRFRAMEHOURS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("AIRFRAMEMAINTPROGRAM")) Then
    '            If Not String.IsNullOrEmpty(r.Item("AIRFRAMEMAINTPROGRAM").ToString.Trim) Then
    '              htmlOut.Append(r.Item("AIRFRAMEMAINTPROGRAM").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("AIRFRAMETRACKPROGRAM")) Then
    '            If Not String.IsNullOrEmpty(r.Item("AIRFRAMETRACKPROGRAM").ToString.Trim) Then
    '              htmlOut.Append(r.Item("AIRFRAMETRACKPROGRAM").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MAINTAINED")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MAINTAINED").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MAINTAINED").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENGINEMODELNAME")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENGINEMODELNAME").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENGINEMODELNAME").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENGINEMAINTPROGRAM")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENGINEMAINTPROGRAM").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENGINEMAINTPROGRAM").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENG1HRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENG1HRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENG1HRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENG2HRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENG2HRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENG2HRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENG1SOHHRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENG1SOHHRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENG1SOHHRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ENG2SOHHRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ENG2SOHHRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ENG2SOHHRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("APUMODELNAME")) Then
    '            If Not String.IsNullOrEmpty(r.Item("APUMODELNAME").ToString.Trim) Then
    '              htmlOut.Append(r.Item("APUMODELNAME").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("APUPROGRAMNAME")) Then
    '            If Not String.IsNullOrEmpty(r.Item("APUPROGRAMNAME").ToString.Trim) Then
    '              htmlOut.Append(r.Item("APUPROGRAMNAME").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("INTERIORDATE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("INTERIORDATE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("INTERIORDATE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("INTERIORDONEBY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("INTERIORDONEBY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("INTERIORDONEBY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("EXTERIORDATE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("EXTERIORDATE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("EXTERIORDATE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("EXTERIORDONEBY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("EXTERIORDONEBY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("EXTERIORDONEBY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("DAMAGE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("DAMAGE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("DAMAGE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("LASTREPORTEDHRS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDHRS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("LASTREPORTEDHRS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("LASTREPORTEDCYCLES")) Then
    '            If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDCYCLES").ToString.Trim) Then
    '              htmlOut.Append(r.Item("LASTREPORTEDCYCLES").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("LASTREPORTEDDATE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("LASTREPORTEDDATE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("LASTREPORTEDDATE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("</tr>")

    '        Next

    '      End If ' _dataTable.Rows.Count > 0 Then

    '      htmlOut.Append("</tbody></table>")
    '      'htmlOut.Append("<div id=""tab3_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
    '      htmlOut.Append("<div id=""tab3_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

    '    End If ' Not IsNothing(folderTable) Then

    '    out_htmlString = htmlOut.ToString

    '  Catch ex As Exception

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab3_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

    '  Finally

    '    htmlOut = Nothing

    '  End Try

    'End Sub

    'Public Sub display_tab3_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByRef GetTotalAircraftNumber As Long = 0)

    '  Dim htmlOut As New StringBuilder
    '  Dim toggleRowColor As Boolean = False

    '  Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    '  Dim SqlConn As New System.Data.SqlClient.SqlConnection
    '  Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    '  Dim _dataTable As New DataTable
    '  Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    '  Dim sQuery As New StringBuilder()

    '  Try

    '    out_htmlString = ""

    '    If Not String.IsNullOrEmpty(aclist) Then

    '      SqlConn.ConnectionString = adminConnectString

    '      SqlConn.Open()

    '      SqlCommand.Connection = SqlConn
    '      SqlCommand.CommandType = System.Data.CommandType.Text
    '      SqlCommand.CommandTimeout = 90

    '      sQuery.Append("select distinct amod_make_name as MAKE,ac_ser_no_sort, amod_model_name as MODEL, ac_ser_no_full as SERNO, ac_reg_no as REGNO, ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
    '      sQuery.Append(" replace(replace(replace(STUFF(( ")
    '      sQuery.Append(" select distinct (' ' + kfeat_name + ', ')")
    '      sQuery.Append(" from Aircraft_Key_Feature with (NOLOCK)")
    '      sQuery.Append(" inner join Key_Feature with (NOLOCK) on Aircraft_Key_Feature.afeat_feature_code = kfeat_code and kfeat_code not in ('DAM') and kfeat_area not IN('Maintenance')")
    '      sQuery.Append(" and afeat_status_flag ='Y'")
    '      sQuery.Append(" where   Aircraft_Key_Feature.afeat_ac_id  = ac_id ")
    '      sQuery.Append(" group by kfeat_name  ")
    '      sQuery.Append(" FOR XML PATH('')),1,1,''), ")
    '      sQuery.Append(" '<afeat_feature_code>', ''), '</afeat_feature_code>', ''), 'afeat_feature_code>', '') as FEATURES, ac_id")
    '      sQuery.Append(" from View_Aircraft_Flat with (NOLOCK) ")

    '      sQuery.Append(" WHERE ac_id IN (")
    '      sQuery.Append(aclist)
    '      sQuery.Append(") AND ac_journ_id = 0 ")

    '      sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))


    '      sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

    '      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab3_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

    '      SqlCommand.CommandText = sQuery.ToString
    '      _recordSet = SqlCommand.ExecuteReader()

    '      Try
    '        _dataTable.Load(_recordSet)
    '      Catch constrExc As System.Data.ConstraintException
    '        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
    '      End Try

    '      _recordSet.Close()
    '      _recordSet = Nothing

    '      If _dataTable.Rows.Count > 0 Then
    '        GetTotalAircraftNumber = _dataTable.Rows.Count
    '        htmlOut.Append("<table id=""tab3_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
    '        htmlOut.Append("<thead><tr>")
    '        htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

    '        If isMobileDisplay Then
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th></th>")
    '        Else
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th>MAKE</th>")
    '          htmlOut.Append("<th>MODEL</th>")
    '          htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
    '          htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
    '          htmlOut.Append("<th>MFR YEAR</th>")
    '          htmlOut.Append("<th>DLV YEAR</th>")
    '          htmlOut.Append("<th align=""left"">FEATURES</th>")


    '        End If

    '        htmlOut.Append("</tr></thead><tbody>")

    '        Dim sSeparator As String = ""

    '        For Each r As DataRow In _dataTable.Rows

    '          htmlOut.Append("<tr>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MAKE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MAKE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MODEL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MODEL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")


    '          If Not IsDBNull(r.Item("SERNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
    '              htmlOut.Append(DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, True, r.Item("SERNO").ToString.Trim, "text_underline", ""))
    '            End If
    '          End If


    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("REGNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
    '              htmlOut.Append(r.Item("REGNO").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MFRYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("DLVYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("FEATURES")) Then
    '            If Not String.IsNullOrEmpty(r.Item("FEATURES").ToString.Trim) Then
    '              If Right(Trim(r.Item("FEATURES")), 1) = "," Then
    '                htmlOut.Append(Left(r.Item("FEATURES").ToString.Trim, r.Item("FEATURES").ToString.Trim.Length - 1))
    '              Else
    '                htmlOut.Append(r.Item("FEATURES").ToString.Trim)
    '              End If

    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("</tr>")

    '        Next

    '      End If ' _dataTable.Rows.Count > 0 Then

    '      htmlOut.Append("</tbody></table>")
    '      'htmlOut.Append("<div id=""tab3_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
    '      htmlOut.Append("<div id=""tab3_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

    '    End If ' Not IsNothing(folderTable) Then

    '    out_htmlString = htmlOut.ToString

    '  Catch ex As Exception

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab3_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

    '  Finally

    '    htmlOut = Nothing

    '  End Try

    'End Sub



    Public Function display_tab3_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByVal summarize_field As Integer = 0, Optional ByVal Show_Notes As Boolean = True, Optional features_dropdown As DropDownList = Nothing, Optional features_dropdown_button As Button = Nothing) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Dim temp_label2 As New Label
        Dim temp_label3 As New Label


        Dim ResultsTable As New DataTable
        Dim aclsData_Temp = New clsData_Manager_SQL
        If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
            aclsData_Temp.JETNET_DB = clientConnectString
        End If


        Try


            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                If summarize_field = -1 Then

                    sQuery.Append(" select distinct acatt_area, acatt_name,acatt_id")
                    sQuery.Append(" , count(distinct ac_id) as tcount ")
                    sQuery.Append(" from Aircraft_Attribute_Index with (NOLOCK)")
                    sQuery.Append(" inner join Aircraft_Attribute with (NOLOCK) on acattind_acatt_id = acatt_id")
                    sQuery.Append(" inner join Aircraft with (NOLOCK) on acattind_ac_id = ac_id And ac_journ_id = 0")
                    sQuery.Append(" left outer join Aircraft_Attribute_Model with (NOLOCK) on ac_amod_id = attmod_amod_id and acattind_acatt_id = attmod_att_id")
                    sQuery.Append(" where acattind_journ_id = 0")
                    sQuery.Append(" and acattind_status_flag ='Y'")

                    sQuery.Append(" and ac_id IN (")
                    sQuery.Append(aclist)
                    sQuery.Append(") AND ac_journ_id = 0 ")

                    sQuery.Append(" group by acatt_area,acatt_name, acatt_id")
                    sQuery.Append(" order by acatt_Area, acatt_name ")

                Else

                    sQuery.Append("SELECT ac_id, amod_make_name as MAKE,")
                    sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
                    sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
                    sQuery.Append(" acs_name as LIFECYCLE, acot_name as OWNERSHIP,")
                    sQuery.Append(" (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = 0 AND cref_contact_type IN('00','17','08') AND comp_active_flag = 'Y')) AS OWNER,")
                    sQuery.Append(" (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = 0 AND cref_operator_flag IN('Y','O') AND comp_active_flag = 'Y')) AS OPERATOR,")
                    sQuery.Append(" ac_aport_name as BASEAPORT,")
                    sQuery.Append(" ac_aport_country as BASECOUNTRY,")
                    sQuery.Append(" ac_est_airframe_hrs as ESTAIRFRAMEHRS ")


                    If summarize_field > 0 Then
                        sQuery.Append(" from Aircraft_Attribute_Index with (NOLOCK) ")
                        sQuery.Append(" inner Join Aircraft_Attribute with (NOLOCK) on acattind_acatt_id = acatt_id ")
                        sQuery.Append(" inner Join Aircraft_Flat with (NOLOCK) on acattind_ac_id = ac_id And ac_journ_id = 0 ")
                        sQuery.Append(" left outer join Aircraft_Attribute_Model with (NOLOCK) on amod_id = attmod_amod_id And acattind_acatt_id = attmod_att_id ")
                    Else
                        sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) ")
                    End If


                    sQuery.Append(" WHERE ac_id IN (")
                    sQuery.Append(aclist)
                    sQuery.Append(") AND ac_journ_id = 0 ")


                    If summarize_field > 0 Then
                        sQuery.Append(" and acattind_status_flag ='Y'")
                        sQuery.Append(" AND acatt_id = " & summarize_field.ToString & " ")
                    End If

                    sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                    sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

                End If

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab0_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        AllowExport = False
                    End If


                    Dim sSeparator As String = ""
                    htmlOut.Append(" var tab3DataSet  = [ ")

                    For Each r As DataRow In _dataTable.Rows

                        If count > 0 Then
                            htmlOut.Append(",")
                        End If

                        htmlOut.Append("{")

                        If summarize_field = -1 Then
                            htmlOut.Append("""check"": """",") 'Checkbox row. 
                            htmlOut.Append("""id"": """ + r.Item("acatt_id").ToString.Trim + """,") 'hidden ID

                            Dim jsClick As String = ""

                            jsClick = "javascript: $(\'#" & features_dropdown.ClientID & "\').val(\'" + r.Item("acatt_id").ToString.Trim + "\');$(\'#" & features_dropdown_button.ClientID & "\').click();"
                            htmlOut.Append("""Summarized"": ""<a href=\'javascript:void(0);\' onclick=\""" & jsClick & "\"">" + r.Item("acatt_name").ToString.Trim + """,") 'Checkbox row. 
                            htmlOut.Append("""Total"": """ + r.Item("tcount").ToString.Trim + """") 'hidden ID

                        Else

                            htmlOut.Append("""check"": """",") 'Checkbox row. 
                            htmlOut.Append("""id"": """ + r.Item("ac_id").ToString.Trim + """,") 'hidden ID


                            htmlOut.Append("""note"": """)

                            If Show_Notes = True Then
                                htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                            Else
                                htmlOut.Append("")
                            End If



                            htmlOut.Append(""",")


                            htmlOut.Append("""make"": """)

                            If Not IsDBNull(r.Item("MAKE")) Then
                                If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MAKE").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append(""",") 'make

                            htmlOut.Append("""model"": """)
                            If Not IsDBNull(r.Item("MODEL")) Then
                                If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MODEL").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append(""",") 'model

                            htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("MAKE") & " " & r("MODEL") & " S/N #" & r("SERNO") & """>" & r("SERNO").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")


                            htmlOut.Append("""reg"": """)

                            If Not IsDBNull(r.Item("REGNO")) Then
                                If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
                                    htmlOut.Append(r.Item("REGNO").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""mfryear"": """)

                            If Not IsDBNull(r.Item("MFRYEAR")) Then
                                If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""dlvyear"": """)

                            If Not IsDBNull(r.Item("DLVYEAR")) Then
                                If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""lifecycle"": """)

                            If Not IsDBNull(r.Item("LIFECYCLE")) Then
                                If Not String.IsNullOrEmpty(r.Item("LIFECYCLE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("LIFECYCLE").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""ownership"": """)

                            If Not IsDBNull(r.Item("OWNERSHIP")) Then
                                If Not String.IsNullOrEmpty(r.Item("OWNERSHIP").ToString.Trim) Then
                                    Select Case UCase(Trim(r.Item("OWNERSHIP")))
                                        Case "FRACTIONAL OWNERSHIP PROGRAM"
                                            htmlOut.Append("Fractional")
                                        Case "SHARED OWNERSHIP"
                                            htmlOut.Append("Shared")
                                        Case Else
                                            htmlOut.Append("Whole")
                                    End Select
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""owner"": """)

                            If Not IsDBNull(r.Item("OWNER")) Then
                                If Not String.IsNullOrEmpty(r.Item("OWNER").ToString.Trim) Then
                                    htmlOut.Append(r.Item("OWNER").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""operator"": """)

                            If Not IsDBNull(r.Item("OPERATOR")) Then
                                If Not String.IsNullOrEmpty(r.Item("OPERATOR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("OPERATOR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""baseaport"": """)

                            If Not IsDBNull(r.Item("BASEAPORT")) Then
                                If Not String.IsNullOrEmpty(r.Item("BASEAPORT").ToString.Trim) Then
                                    htmlOut.Append(r.Item("BASEAPORT").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""basecountry"": """)

                            If Not IsDBNull(r.Item("BASECOUNTRY")) Then
                                If Not String.IsNullOrEmpty(r.Item("BASECOUNTRY").ToString.Trim) Then
                                    htmlOut.Append(r.Item("BASECOUNTRY").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""estairframehrs"": """)

                            If Not IsDBNull(r.Item("ESTAIRFRAMEHRS")) Then
                                If Not String.IsNullOrEmpty(r.Item("ESTAIRFRAMEHRS").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ESTAIRFRAMEHRS").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("""")
                        End If

                        htmlOut.Append("}")
                        count += 1


                    Next
                    htmlOut.Append("];")
                End If ' _dataTable.Rows.Count > 0 Then

            End If ' Not IsNothing(folderTable) Then


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab0_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally


        End Try

        Return htmlOut
        htmlOut = Nothing
    End Function

    'Public Function display_tab3_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByRef GetTotalAircraftNumber As Long = 0, Optional ByRef Show_Notes As Boolean = False) As StringBuilder

    '    Dim htmlOut As New StringBuilder
    '    Dim toggleRowColor As Boolean = False

    '    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    '    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    '    Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    '    Dim _dataTable As New DataTable
    '    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
    '    Dim count As Integer = 0
    '    Dim sQuery As New StringBuilder()

    '    Try
    '        Dim aclsData_Temp = New clsData_Manager_SQL
    '        If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
    '           And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
    '            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
    '        ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
    '            aclsData_Temp.JETNET_DB = clientConnectString
    '        End If

    '        If Not String.IsNullOrEmpty(aclist) Then



    '            SqlConn.ConnectionString = clientConnectStr

    '            SqlConn.Open()

    '            SqlCommand.Connection = SqlConn
    '            SqlCommand.CommandType = System.Data.CommandType.Text
    '            SqlCommand.CommandTimeout = 90

    '            sQuery.Append("select distinct amod_make_name as MAKE,ac_ser_no_sort, amod_model_name as MODEL, ac_ser_no_full as SERNO, ac_reg_no as REGNO, ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
    '            sQuery.Append(" replace(replace(replace(STUFF(( ")
    '            sQuery.Append(" select distinct (' ' + kfeat_name + ', ')")
    '            sQuery.Append(" from Aircraft_Key_Feature with (NOLOCK)")
    '            sQuery.Append(" inner join Key_Feature with (NOLOCK) on Aircraft_Key_Feature.afeat_feature_code = kfeat_code and kfeat_code not in ('DAM') and kfeat_area not IN('Maintenance')")
    '            sQuery.Append(" and afeat_status_flag ='Y'")
    '            sQuery.Append(" where   Aircraft_Key_Feature.afeat_ac_id  = ac_id ")
    '            sQuery.Append(" group by kfeat_name  ")
    '            sQuery.Append(" FOR XML PATH('')),1,1,''), ")
    '            sQuery.Append(" '<afeat_feature_code>', ''), '</afeat_feature_code>', ''), 'afeat_feature_code>', '') as FEATURES, ac_id")
    '            sQuery.Append(" from View_Aircraft_Flat with (NOLOCK) ")

    '            sQuery.Append(" WHERE ac_id IN (")
    '            sQuery.Append(aclist)
    '            sQuery.Append(") AND ac_journ_id = 0 ")

    '            sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))


    '            sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

    '            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab3_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

    '            SqlCommand.CommandText = sQuery.ToString
    '            _recordSet = SqlCommand.ExecuteReader()

    '            Try
    '                _dataTable.Load(_recordSet)
    '            Catch constrExc As System.Data.ConstraintException
    '                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
    '            End Try

    '            _recordSet.Close()
    '            _recordSet = Nothing

    '            If _dataTable.Rows.Count > 0 Then
    '                GetTotalAircraftNumber = _dataTable.Rows.Count

    '                If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
    '                    AllowExport = False
    '                End If

    '                Dim sSeparator As String = ""

    '                htmlOut.Append(" var tab3DataSet  = [ ")
    '                For Each r As DataRow In _dataTable.Rows

    '                    If count > 0 Then
    '                        htmlOut.Append(",")
    '                    End If
    '                    htmlOut.Append("{")
    '                    htmlOut.Append("""check"": """",") 'Checkbox row.
    '                    htmlOut.Append("""id"": """ + r.Item("ac_id").ToString.Trim + """,") 'hidden ID

    '                    htmlOut.Append("""note"": """)

    '                    If Show_Notes = True Then
    '                        htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
    '                    Else
    '                        htmlOut.Append("")
    '                    End If

    '                    htmlOut.Append(""",")


    '                    htmlOut.Append("""make"": """)

    '                    If Not IsDBNull(r.Item("MAKE")) Then
    '                        If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
    '                            htmlOut.Append(r.Item("MAKE").ToString.Trim)
    '                        End If
    '                    End If
    '                    htmlOut.Append(""",") 'make

    '                    htmlOut.Append("""model"": """)
    '                    If Not IsDBNull(r.Item("MODEL")) Then
    '                        If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
    '                            htmlOut.Append(r.Item("MODEL").ToString.Trim)
    '                        End If
    '                    End If
    '                    htmlOut.Append(""",") 'model

    '                    htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("MAKE") & " " & r("MODEL") & " S/N #" & r("SERNO") & """>" & r("SERNO").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")


    '                    htmlOut.Append("""reg"": """)

    '                    If Not IsDBNull(r.Item("REGNO")) Then
    '                        If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
    '                            htmlOut.Append(r.Item("REGNO").ToString.Trim)
    '                        End If
    '                    End If

    '                    htmlOut.Append(""",")

    '                    htmlOut.Append("""mfryear"": """)

    '                    If Not IsDBNull(r.Item("MFRYEAR")) Then
    '                        If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
    '                            htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
    '                        End If
    '                    End If

    '                    htmlOut.Append(""",")

    '                    htmlOut.Append("""dlvyear"": """)

    '                    If Not IsDBNull(r.Item("DLVYEAR")) Then
    '                        If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
    '                            htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
    '                        End If
    '                    End If

    '                    htmlOut.Append(""",")
    '                    htmlOut.Append("""features"": """)



    '                    If Not IsDBNull(r.Item("FEATURES")) Then
    '                        If Not String.IsNullOrEmpty(r.Item("FEATURES").ToString.Trim) Then
    '                            If Right(Trim(r.Item("FEATURES")), 1) = "," Then
    '                                htmlOut.Append(Left(r.Item("FEATURES").ToString.Trim, r.Item("FEATURES").ToString.Trim.Length - 1))
    '                            Else
    '                                htmlOut.Append(r.Item("FEATURES").ToString.Trim)
    '                            End If

    '                        End If
    '                    End If

    '                    htmlOut.Append("""")
    '                    htmlOut.Append("}")
    '                    count += 1
    '                Next
    '                htmlOut.Append("];")

    '            End If ' _dataTable.Rows.Count > 0 Then

    '            'htmlOut.Append("</tbody></table>")
    '            ''htmlOut.Append("<div id=""tab3_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
    '            'htmlOut.Append("<div id=""tab3_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

    '        End If ' Not IsNothing(folderTable) Then



    '    Catch ex As Exception

    '        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab3_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

    '    Finally



    '    End Try

    '    Return htmlOut
    '    htmlOut = Nothing

    'End Function
    Public Function display_tab4_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByRef Show_Notes As Boolean = False) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Try
            Dim aclsData_Temp = New clsData_Manager_SQL
            If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                aclsData_Temp.JETNET_DB = clientConnectString
            End If
            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT DISTINCT Aircraft.ac_id, Aircraft_Model.amod_make_name, Aircraft_Model.amod_model_name,")
                sQuery.Append(" Aircraft.ac_ser_no_sort, Aircraft.ac_ser_no_full, Aircraft.ac_reg_no, base_aport_name,")
                sQuery.Append(" COUNT(ffd_unique_flight_id) AS FLTS12MONTHS,")
                sQuery.Append(" COUNT(ffd_unique_flight_id)/12 AS FLTSPERMONTH,")
                sQuery.Append(" SUM(CONVERT(DECIMAL(18,4),ffd_flight_time))/60 AS TOTALFLIGHTTIMEHRS,")
                sQuery.Append(" SUM((ffd_flight_time * Aircraft_Model.amod_fuel_burn_rate)/60) AS TOTALFUELBURN")
                sQuery.Append(" FROM Aircraft WITH (NOLOCK)")
                sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON amod_id = ac_amod_id")
                sQuery.Append(" LEFT OUTER JOIN view_flights WITH (NOLOCK) ON Aircraft.ac_id = view_flights.ac_id AND ac_journ_id = 0 AND (CONVERT(DATE, ffd_date, 0) >= GETDATE()-367)")
                sQuery.Append(" WHERE Aircraft.ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 ")
                ' sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                sQuery.Append(" and (")


                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
                    sQuery.Append(" Aircraft.ac_product_business_flag = 'Y' ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                    sQuery.Append(" or ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                    sQuery.Append(" Aircraft.ac_product_commercial_flag = 'Y' ")
                End If

                If (HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True) Or (HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True) Then
                    sQuery.Append(" or ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                    sQuery.Append(" Aircraft.ac_product_helicopter_flag = 'Y'")
                End If

                sQuery.Append(") ")


                sQuery.Append(" GROUP BY Aircraft_Model.amod_make_name, Aircraft_Model.amod_model_name, Aircraft.ac_ser_no_full, base_aport_name, Aircraft.ac_reg_no, Aircraft.ac_ser_no_sort, Aircraft.ac_id")
                sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab4_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        AllowExport = False
                    End If

                    Dim sSeparator As String = ""

                    htmlOut.Append(" var tab4DataSet  = [ ")


                    For Each r As DataRow In _dataTable.Rows

                        If count > 0 Then
                            htmlOut.Append(",")
                        End If

                        htmlOut.Append("{")
                        htmlOut.Append("""check"": """",") 'Checkbox row.
                        htmlOut.Append("""id"": """) 'id row
                        htmlOut.Append(r.Item("ac_id").ToString.Trim)
                        htmlOut.Append(""",")
                        htmlOut.Append("""note"": """)

                        If Show_Notes = True Then
                            htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                        Else
                            htmlOut.Append("")
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""make"": """) 'make row

                        If Not IsDBNull(r.Item("amod_make_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("amod_make_name").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""model"": """) 'model row
                        If Not IsDBNull(r.Item("amod_model_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("amod_model_name").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("amod_make_name") & " " & r("amod_model_name") & " S/N #" & r("ac_ser_no_full") & """>" & r("ac_ser_no_full").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")
                        htmlOut.Append("""reg"": """)

                        If Not IsDBNull(r.Item("ac_reg_no")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString.Trim) Then
                                htmlOut.Append(r.Item("ac_reg_no").ToString.Trim)
                            End If
                        End If


                        htmlOut.Append(""",")


                        htmlOut.Append("""base"": """)

                        If Not IsDBNull(r.Item("base_aport_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("base_aport_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("base_aport_name").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""flts12months"": """)

                        If Not IsDBNull(r.Item("FLTS12MONTHS")) Then
                            If Not String.IsNullOrEmpty(r.Item("FLTS12MONTHS").ToString.Trim) Then
                                htmlOut.Append(FormatNumber(r.Item("FLTS12MONTHS"), 0).ToString.Trim)
                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""fltspermonths"": """)

                        If Not IsDBNull(r.Item("FLTSPERMONTH")) Then
                            If Not String.IsNullOrEmpty(r.Item("FLTSPERMONTH").ToString.Trim) Then
                                htmlOut.Append(FormatNumber(r.Item("FLTSPERMONTH"), 0).ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""totalflighttimehrs"": """)
                        If Not IsDBNull(r.Item("TOTALFLIGHTTIMEHRS")) Then
                            If Not String.IsNullOrEmpty(r.Item("TOTALFLIGHTTIMEHRS").ToString.Trim) Then
                                htmlOut.Append(FormatNumber(r.Item("TOTALFLIGHTTIMEHRS"), 0).ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""totalfuelburn"": """)
                        If Not IsDBNull(r.Item("TOTALFUELBURN")) Then
                            If Not String.IsNullOrEmpty(r.Item("TOTALFUELBURN").ToString.Trim) Then
                                htmlOut.Append(FormatNumber(r.Item("TOTALFUELBURN"), 0).ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("""")
                        htmlOut.Append("}")
                        count += 1
                    Next
                    htmlOut.Append("];")

                End If ' _dataTable.Rows.Count > 0 Then

            End If ' Not IsNothing(folderTable) Then



        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab4_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            'htmlOut = Nothing

        End Try
        Return htmlOut
        htmlOut = Nothing

    End Function

    Public Function display_tab5_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByRef Show_Notes As Boolean = False) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Try
            Dim aclsData_Temp = New clsData_Manager_SQL
            If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                aclsData_Temp.JETNET_DB = clientConnectString
            End If

            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT DISTINCT ac_id, amod_make_name as MAKE,")
                sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
                sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
                sQuery.Append(" comp_name AS OPERATOR, comp_city AS OPCITY,")
                sQuery.Append(" comp_state AS OPSTATE, comp_country AS OPCOUNTRY,")
                sQuery.Append(" comp_web_address as COMPWEBADDRESS, comp_email_address as COMPEMAIL, comp_phone_office as COMPOFFICEPHONE,")
                sQuery.Append(" contact_first_name as CONTACTFIRSTNAME, contact_last_name AS CONTACTLASTNAME,")
                sQuery.Append(" contact_title as CONTACTTITLE, contact_email_address as CONTACTEMAIL,")
                sQuery.Append(" contact_phone_office as CONTACTOFFICEPHONE, contact_phone_mobile as CONTACTMOBILEPHONE,")
                sQuery.Append(" comp_id AS COMPID, contact_id as CONTACTID")
                sQuery.Append(" FROM View_Aircraft_Company_Flat WITH (NOLOCK) WHERE ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 AND cref_operator_flag IN('Y','O') ")
                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab5_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        AllowExport = False
                    End If

                    Dim sSeparator As String = ""

                    htmlOut.Append(" var tab5DataSet  = [ ")
                    For Each r As DataRow In _dataTable.Rows

                        If count > 0 Then
                            htmlOut.Append(",")
                        End If
                        htmlOut.Append("{")
                        htmlOut.Append("""check"": """",") 'Checkbox row.
                        htmlOut.Append("""id"": """ + r.Item("ac_id").ToString.Trim + """,") 'hidden ID

                        htmlOut.Append("""note"": """)

                        If Show_Notes = True Then
                            htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                        Else
                            htmlOut.Append("")
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""make"": """)

                        If Not IsDBNull(r.Item("MAKE")) Then
                            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                                htmlOut.Append(r.Item("MAKE").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",") 'make

                        htmlOut.Append("""model"": """)
                        If Not IsDBNull(r.Item("MODEL")) Then
                            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                                htmlOut.Append(r.Item("MODEL").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",") 'model

                        htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("MAKE") & " " & r("MODEL") & " S/N #" & r("SERNO") & """>" & r("SERNO").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")


                        htmlOut.Append("""reg"": """)

                        If Not IsDBNull(r.Item("REGNO")) Then
                            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
                                htmlOut.Append(r.Item("REGNO").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""mfryear"": """)

                        If Not IsDBNull(r.Item("MFRYEAR")) Then
                            If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
                                htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""dlvyear"": """)

                        If Not IsDBNull(r.Item("DLVYEAR")) Then
                            If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
                                htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""operator"": """)
                        If Not IsDBNull(r.Item("OPERATOR")) Then
                            If Not String.IsNullOrEmpty(r.Item("OPERATOR").ToString.Trim) Then
                                If Not IsDBNull(r.Item("COMPID")) Then
                                    htmlOut.Append(Replace(Replace(DisplayFunctions.WriteDetailsLink(0, r.Item("COMPID"), 0, 0, True, Replace(r.Item("OPERATOR").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp), "text_underline", ""), """", "\"""), "'", "\'"))
                                End If
                            End If
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""opcity"": """)

                        If Not IsDBNull(r.Item("OPCITY")) Then
                            If Not String.IsNullOrEmpty(r.Item("OPCITY").ToString.Trim) Then
                                htmlOut.Append(r.Item("OPCITY").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""opstate"": """)

                        If Not IsDBNull(r.Item("OPSTATE")) Then
                            If Not String.IsNullOrEmpty(r.Item("OPSTATE").ToString.Trim) Then
                                htmlOut.Append(r.Item("OPSTATE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")


                        htmlOut.Append("""opcountry"": """)

                        If Not IsDBNull(r.Item("OPCOUNTRY")) Then
                            If Not String.IsNullOrEmpty(r.Item("OPCOUNTRY").ToString.Trim) Then
                                htmlOut.Append(r.Item("OPCOUNTRY").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""opwebaddress"": """)

                        If Not IsDBNull(r.Item("COMPWEBADDRESS")) Then
                            If Not String.IsNullOrEmpty(r.Item("COMPWEBADDRESS").ToString.Trim) Then
                                htmlOut.Append(r.Item("COMPWEBADDRESS").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""opemail"": """)


                        If Not IsDBNull(r.Item("COMPEMAIL")) Then
                            If Not String.IsNullOrEmpty(r.Item("COMPEMAIL").ToString.Trim) Then
                                htmlOut.Append(r.Item("COMPEMAIL").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",")
                        htmlOut.Append("""opofficephone"": """)

                        If Not IsDBNull(r.Item("COMPOFFICEPHONE")) Then
                            If Not String.IsNullOrEmpty(r.Item("COMPOFFICEPHONE").ToString.Trim) Then
                                htmlOut.Append(r.Item("COMPOFFICEPHONE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""contactname"": """)
                        If Not IsDBNull(r.Item("COMPID")) And Not IsDBNull(r.Item("CONTACTID")) Then
                            htmlOut.Append(Replace(Replace("<a class=""underline"" " & DisplayFunctions.WriteDetailsLink(0, r.Item("COMPID"), r.Item("CONTACTID"), 0, False, "", "", "") & ">", """", "\"""), "'", "\'"))
                            If Not IsDBNull(r.Item("CONTACTFIRSTNAME")) Then
                                If Not String.IsNullOrEmpty(r.Item("CONTACTFIRSTNAME").ToString.Trim) Then
                                    htmlOut.Append(r.Item("CONTACTFIRSTNAME").ToString.Trim)
                                End If
                            End If

                            If Not IsDBNull(r.Item("CONTACTLASTNAME")) Then
                                If Not String.IsNullOrEmpty(r.Item("CONTACTLASTNAME").ToString.Trim) Then
                                    htmlOut.Append(Constants.cSingleSpace + r.Item("CONTACTLASTNAME").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</a>")
                        End If
                        htmlOut.Append(""",")
                        htmlOut.Append("""contacttitle"": """)

                        If Not IsDBNull(r.Item("CONTACTTITLE")) Then
                            If Not String.IsNullOrEmpty(r.Item("CONTACTTITLE").ToString.Trim) Then
                                htmlOut.Append(r.Item("CONTACTTITLE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""contactemail"": """)

                        If Not IsDBNull(r.Item("CONTACTEMAIL")) Then
                            If Not String.IsNullOrEmpty(r.Item("CONTACTEMAIL").ToString.Trim) Then
                                htmlOut.Append(r.Item("CONTACTEMAIL").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""contactofficephone"": """)

                        If Not IsDBNull(r.Item("CONTACTOFFICEPHONE")) Then
                            If Not String.IsNullOrEmpty(r.Item("CONTACTOFFICEPHONE").ToString.Trim) Then
                                htmlOut.Append(r.Item("CONTACTOFFICEPHONE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""contactmobilephone"": """)

                        If Not IsDBNull(r.Item("CONTACTMOBILEPHONE")) Then
                            If Not String.IsNullOrEmpty(r.Item("CONTACTMOBILEPHONE").ToString.Trim) Then
                                htmlOut.Append(r.Item("CONTACTMOBILEPHONE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("""")
                        htmlOut.Append("}")
                        count += 1
                    Next
                    htmlOut.Append("];")

                End If ' _dataTable.Rows.Count > 0 Then


            End If ' Not IsNothing(folderTable) Then



        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab5_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally


        End Try

        Return htmlOut
        htmlOut = Nothing


    End Function
    'Public Sub display_tab5_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

    '  Dim htmlOut As New StringBuilder
    '  Dim toggleRowColor As Boolean = False

    '  Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    '  Dim SqlConn As New System.Data.SqlClient.SqlConnection
    '  Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    '  Dim _dataTable As New DataTable
    '  Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    '  Dim sQuery As New StringBuilder()

    '  Try

    '    out_htmlString = ""

    '    If Not String.IsNullOrEmpty(aclist) Then

    '      SqlConn.ConnectionString = adminConnectString

    '      SqlConn.Open()

    '      SqlCommand.Connection = SqlConn
    '      SqlCommand.CommandType = System.Data.CommandType.Text
    '      SqlCommand.CommandTimeout = 90

    '      sQuery.Append("SELECT DISTINCT ac_id, amod_make_name as MAKE,")
    '      sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
    '      sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
    '      sQuery.Append(" comp_name AS OPERATOR, comp_city AS OPCITY,")
    '      sQuery.Append(" comp_state AS OPSTATE, comp_country AS OPCOUNTRY,")
    '      sQuery.Append(" comp_web_address as COMPWEBADDRESS, comp_email_address as COMPEMAIL, comp_phone_office as COMPOFFICEPHONE,")
    '      sQuery.Append(" contact_first_name as CONTACTFIRSTNAME, contact_last_name AS CONTACTLASTNAME,")
    '      sQuery.Append(" contact_title as CONTACTTITLE, contact_email_address as CONTACTEMAIL,")
    '      sQuery.Append(" contact_phone_office as CONTACTOFFICEPHONE, contact_phone_mobile as CONTACTMOBILEPHONE,")
    '      sQuery.Append(" comp_id AS COMPID, contact_id as CONTACTID")
    '      sQuery.Append(" FROM View_Aircraft_Company_Flat WITH (NOLOCK) WHERE ac_id IN (")
    '      sQuery.Append(aclist)
    '      sQuery.Append(") AND ac_journ_id = 0 AND cref_operator_flag IN('Y','O') ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")


    '      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab5_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

    '      SqlCommand.CommandText = sQuery.ToString
    '      _recordSet = SqlCommand.ExecuteReader()

    '      Try
    '        _dataTable.Load(_recordSet)
    '      Catch constrExc As System.Data.ConstraintException
    '        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
    '      End Try

    '      _recordSet.Close()
    '      _recordSet = Nothing

    '      If _dataTable.Rows.Count > 0 Then

    '        htmlOut.Append("<table id=""tab5_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
    '        htmlOut.Append("<thead><tr>")
    '        htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

    '        If isMobileDisplay Then
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th></th>")
    '        Else
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th>MAKE</th>")
    '          htmlOut.Append("<th>MODEL</th>")
    '          htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
    '          htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
    '          htmlOut.Append("<th>MFRYEAR</th>")
    '          htmlOut.Append("<th>DLVYEAR</th>")
    '          htmlOut.Append("<th>OPERATOR</th>")
    '          htmlOut.Append("<th>OPCITY</th>")
    '          htmlOut.Append("<th>OPSTATE</th>")
    '          htmlOut.Append("<th>OPCOUNTRY</th>")
    '          htmlOut.Append("<th>OPWEBADDRESS</th>")
    '          htmlOut.Append("<th>OPEMAIL</th>")
    '          htmlOut.Append("<th>OPOFFICEPHONE</th>")
    '          htmlOut.Append("<th>CONTACTNAME</th>")
    '          htmlOut.Append("<th>CONTACTTITLE</th>")
    '          htmlOut.Append("<th>CONTACTEMAIL</th>")
    '          htmlOut.Append("<th>CONTACTOFFICEPHONE</th>")
    '          htmlOut.Append("<th>CONTACTMOBILEPHONE</th>")

    '        End If

    '        htmlOut.Append("</tr></thead><tbody>")

    '        Dim sSeparator As String = ""

    '        For Each r As DataRow In _dataTable.Rows

    '          htmlOut.Append("<tr>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MAKE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MAKE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MODEL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MODEL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")

    '          If Not IsDBNull(r.Item("SERNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
    '              htmlOut.Append(DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, True, r.Item("SERNO").ToString.Trim, "text_underline", ""))
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("REGNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
    '              htmlOut.Append(r.Item("REGNO").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MFRYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("DLVYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OPERATOR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OPERATOR").ToString.Trim) Then
    '              If Not IsDBNull(r.Item("COMPID")) Then
    '                htmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r.Item("COMPID"), 0, 0, True, Replace(r.Item("OPERATOR").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp), "text_underline", ""))
    '              End If
    '            End If
    '          End If
    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OPCITY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OPCITY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OPCITY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OPSTATE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OPSTATE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OPSTATE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OPCOUNTRY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OPCOUNTRY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OPCOUNTRY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("COMPWEBADDRESS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("COMPWEBADDRESS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("COMPWEBADDRESS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("COMPEMAIL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("COMPEMAIL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("COMPEMAIL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("COMPOFFICEPHONE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("COMPOFFICEPHONE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("COMPOFFICEPHONE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
    '          If Not IsDBNull(r.Item("COMPID")) And Not IsDBNull(r.Item("CONTACTID")) Then
    '            htmlOut.Append("<a class=""underline"" " & DisplayFunctions.WriteDetailsLink(0, r.Item("COMPID"), r.Item("CONTACTID"), 0, False, "", "", "") & ">")
    '            If Not IsDBNull(r.Item("CONTACTFIRSTNAME")) Then
    '              If Not String.IsNullOrEmpty(r.Item("CONTACTFIRSTNAME").ToString.Trim) Then
    '                htmlOut.Append(r.Item("CONTACTFIRSTNAME").ToString.Trim)
    '              End If
    '            End If

    '            If Not IsDBNull(r.Item("CONTACTLASTNAME")) Then
    '              If Not String.IsNullOrEmpty(r.Item("CONTACTLASTNAME").ToString.Trim) Then
    '                htmlOut.Append(Constants.cSingleSpace + r.Item("CONTACTLASTNAME").ToString.Trim)
    '              End If
    '            End If

    '            htmlOut.Append("</a>")
    '          End If
    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("CONTACTTITLE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTTITLE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTTITLE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("CONTACTEMAIL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTEMAIL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTEMAIL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("CONTACTOFFICEPHONE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTOFFICEPHONE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTOFFICEPHONE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("CONTACTMOBILEPHONE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTMOBILEPHONE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTMOBILEPHONE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("</tr>")

    '        Next

    '      End If ' _dataTable.Rows.Count > 0 Then

    '      htmlOut.Append("</tbody></table>")
    '      'htmlOut.Append("<div id=""tab5_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
    '      htmlOut.Append("<div id=""tab5_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

    '    End If ' Not IsNothing(folderTable) Then

    '    out_htmlString = htmlOut.ToString

    '  Catch ex As Exception

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab5_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

    '  Finally

    '    htmlOut = Nothing





    '  End Try

    'End Sub
    Public Function display_tab6_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByRef Show_Notes As Boolean = False) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False


        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()


        Try
            Dim aclsData_Temp = New clsData_Manager_SQL
            If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                aclsData_Temp.JETNET_DB = clientConnectString
            End If

            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT DISTINCT ac_id, amod_make_name as MAKE,")
                sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MFRYEAR, ac_year AS DLVYEAR,")
                sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
                sQuery.Append(" acot_name AS OWNERSHIP,")
                sQuery.Append(" CASE WHEN cref_owner_percent IS NULL OR cref_owner_percent = 0 THEN 100 ELSE cref_owner_percent END AS PERCENTOWNED,")
                sQuery.Append(" comp_name AS OWNER, comp_city AS OWNERCITY,")
                sQuery.Append(" comp_state AS OWNERSTATE, comp_country AS OWNERCOUNTRY, country_continent_name,")
                sQuery.Append(" comp_web_address as COMPWEBADDRESS, comp_email_address as COMPEMAIL, comp_phone_office as COMPOFFICEPHONE,")
                sQuery.Append(" contact_first_name as CONTACTFIRSTNAME, contact_last_name AS CONTACTLASTNAME,")
                sQuery.Append(" contact_title as CONTACTTITLE, contact_email_address as CONTACTEMAIL,")
                sQuery.Append(" contact_phone_office as CONTACTOFFICEPHONE, contact_phone_mobile as CONTACTMOBILEPHONE,")
                sQuery.Append(" comp_id AS COMPID, contact_id as CONTACTID")
                sQuery.Append(" FROM View_Aircraft_Company_Flat WITH (NOLOCK) WHERE ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 AND cref_contact_type IN('00','08','97') ")
                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab6_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        AllowExport = False
                    End If

                    Dim sSeparator As String = ""

                    htmlOut.Append(" var tab6DataSet  = [ ")
                    For Each r As DataRow In _dataTable.Rows

                        If count > 0 Then
                            htmlOut.Append(",")
                        End If
                        htmlOut.Append("{")
                        htmlOut.Append("""check"": """",") 'Checkbox row.
                        htmlOut.Append("""id"": """ + r.Item("ac_id").ToString.Trim + """,") 'hidden ID

                        htmlOut.Append("""note"": """)

                        If Show_Notes = True Then
                            htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                        Else
                            htmlOut.Append("")
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""make"": """)

                        If Not IsDBNull(r.Item("MAKE")) Then
                            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                                htmlOut.Append(r.Item("MAKE").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",") 'make

                        htmlOut.Append("""model"": """)
                        If Not IsDBNull(r.Item("MODEL")) Then
                            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                                htmlOut.Append(r.Item("MODEL").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",") 'model

                        htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("MAKE") & " " & r("MODEL") & " S/N #" & r("SERNO") & """>" & r("SERNO").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")


                        htmlOut.Append("""reg"": """)

                        If Not IsDBNull(r.Item("REGNO")) Then
                            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
                                htmlOut.Append(r.Item("REGNO").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""mfryear"": """)

                        If Not IsDBNull(r.Item("MFRYEAR")) Then
                            If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
                                htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""dlvyear"": """)

                        If Not IsDBNull(r.Item("DLVYEAR")) Then
                            If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
                                htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""ownership"": """)

                        If Not IsDBNull(r.Item("OWNERSHIP")) Then
                            If Not String.IsNullOrEmpty(r.Item("OWNERSHIP").ToString.Trim) Then
                                Select Case UCase(Trim(r.Item("OWNERSHIP")))
                                    Case "FRACTIONAL OWNERSHIP PROGRAM"
                                        htmlOut.Append("Fractional")
                                    Case "SHARED OWNERSHIP"
                                        htmlOut.Append("Shared")
                                    Case Else
                                        htmlOut.Append("Whole")
                                End Select

                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""percentowned"": """)


                        If Not IsDBNull(r.Item("PERCENTOWNED")) Then
                            If Not String.IsNullOrEmpty(r.Item("PERCENTOWNED").ToString.Trim) Then
                                htmlOut.Append(r.Item("PERCENTOWNED").ToString.Trim + "%")
                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""owner"": """)


                        If Not IsDBNull(r.Item("OWNER")) Then
                            If Not String.IsNullOrEmpty(r.Item("OWNER").ToString.Trim) Then

                                htmlOut.Append(Replace(Replace(DisplayFunctions.WriteDetailsLink(0, r.Item("COMPID"), 0, 0, True, Replace(r.Item("OWNER").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp), "text_underline", ""), """", "\"""), "'", "\'"))

                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""ownercity"": """)


                        If Not IsDBNull(r.Item("OWNERCITY")) Then
                            If Not String.IsNullOrEmpty(r.Item("OWNERCITY").ToString.Trim) Then
                                htmlOut.Append(r.Item("OWNERCITY").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""ownerstate"": """)


                        If Not IsDBNull(r.Item("OWNERSTATE")) Then
                            If Not String.IsNullOrEmpty(r.Item("OWNERSTATE").ToString.Trim) Then
                                htmlOut.Append(r.Item("OWNERSTATE").ToString.Trim)
                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""ownercountry"": """)


                        If Not IsDBNull(r.Item("OWNERCOUNTRY")) Then
                            If Not String.IsNullOrEmpty(r.Item("OWNERCOUNTRY").ToString.Trim) Then
                                htmlOut.Append(r.Item("OWNERCOUNTRY").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""ownerwebaddress"": """)


                        If Not IsDBNull(r.Item("COMPWEBADDRESS")) Then
                            If Not String.IsNullOrEmpty(r.Item("COMPWEBADDRESS").ToString.Trim) Then
                                htmlOut.Append(r.Item("COMPWEBADDRESS").ToString.Trim)
                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""owneremail"": """)


                        If Not IsDBNull(r.Item("COMPEMAIL")) Then
                            If Not String.IsNullOrEmpty(r.Item("COMPEMAIL").ToString.Trim) Then
                                htmlOut.Append(r.Item("COMPEMAIL").ToString.Trim)
                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""ownerofficephone"": """)


                        If Not IsDBNull(r.Item("COMPOFFICEPHONE")) Then
                            If Not String.IsNullOrEmpty(r.Item("COMPOFFICEPHONE").ToString.Trim) Then
                                htmlOut.Append(r.Item("COMPOFFICEPHONE").ToString.Trim)
                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""contactname"": """)


                        If Not IsDBNull(r.Item("COMPID")) And Not IsDBNull(r.Item("CONTACTID")) Then
                            htmlOut.Append(Replace(Replace("<a class=""underline"" " & DisplayFunctions.WriteDetailsLink(0, r.Item("COMPID"), r.Item("CONTACTID"), 0, False, "", "", "") & ">", """", "\"""), "'", "\'"))
                            If Not IsDBNull(r.Item("CONTACTFIRSTNAME")) Then
                                If Not String.IsNullOrEmpty(r.Item("CONTACTFIRSTNAME").ToString.Trim) Then
                                    htmlOut.Append(r.Item("CONTACTFIRSTNAME").ToString.Trim)
                                End If
                            End If

                            If Not IsDBNull(r.Item("CONTACTLASTNAME")) Then
                                If Not String.IsNullOrEmpty(r.Item("CONTACTLASTNAME").ToString.Trim) Then
                                    htmlOut.Append(Constants.cSingleSpace + r.Item("CONTACTLASTNAME").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</a>")
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""contacttitle"": """)


                        If Not IsDBNull(r.Item("CONTACTTITLE")) Then
                            If Not String.IsNullOrEmpty(r.Item("CONTACTTITLE").ToString.Trim) Then
                                htmlOut.Append(r.Item("CONTACTTITLE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""contactemail"": """)


                        If Not IsDBNull(r.Item("CONTACTEMAIL")) Then
                            If Not String.IsNullOrEmpty(r.Item("CONTACTEMAIL").ToString.Trim) Then
                                htmlOut.Append(r.Item("CONTACTEMAIL").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""contactofficephone"": """)

                        If Not IsDBNull(r.Item("CONTACTOFFICEPHONE")) Then
                            If Not String.IsNullOrEmpty(r.Item("CONTACTOFFICEPHONE").ToString.Trim) Then
                                htmlOut.Append(r.Item("CONTACTOFFICEPHONE").ToString.Trim)
                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""contactmobilephone"": """)

                        If Not IsDBNull(r.Item("CONTACTMOBILEPHONE")) Then
                            If Not String.IsNullOrEmpty(r.Item("CONTACTMOBILEPHONE").ToString.Trim) Then
                                htmlOut.Append(r.Item("CONTACTMOBILEPHONE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("""")
                        htmlOut.Append("}")
                        count += 1
                    Next
                    htmlOut.Append("];")

                End If ' _dataTable.Rows.Count > 0 Then



            End If ' Not IsNothing(folderTable) Then



        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab6_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally



        End Try
        Return htmlOut
        htmlOut = Nothing

    End Function
    'Public Sub display_tab6_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

    '  Dim htmlOut As New StringBuilder
    '  Dim toggleRowColor As Boolean = False


    '  Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    '  Dim SqlConn As New System.Data.SqlClient.SqlConnection
    '  Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    '  Dim _dataTable As New DataTable
    '  Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    '  Dim sQuery As New StringBuilder()

    '  Try

    '    out_htmlString = ""

    '    If Not String.IsNullOrEmpty(aclist) Then

    '      SqlConn.ConnectionString = adminConnectString

    '      SqlConn.Open()

    '      SqlCommand.Connection = SqlConn
    '      SqlCommand.CommandType = System.Data.CommandType.Text
    '      SqlCommand.CommandTimeout = 90

    '      sQuery.Append("SELECT DISTINCT ac_id, amod_make_name as MAKE,")
    '      sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MFRYEAR, ac_year AS DLVYEAR,")
    '      sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")
    '      sQuery.Append(" CASE WHEN acot_name = 'Shared Ownership' THEN 'Shared' WHEN acot_name = 'Fractional Ownership Program' THEN 'Fractional' ELSE 'Whole' END AS OWNERSHIP,")
    '      sQuery.Append(" CASE WHEN cref_owner_percent IS NULL OR cref_owner_percent = 0 THEN 100 ELSE cref_owner_percent END AS PERCENTOWNED,")
    '      sQuery.Append(" comp_name AS OWNER, comp_city AS OWNERCITY,")
    '      sQuery.Append(" comp_state AS OWNERSTATE, comp_country AS OWNERCOUNTRY, country_continent_name,")
    '      sQuery.Append(" comp_web_address as COMPWEBADDRESS, comp_email_address as COMPEMAIL, comp_phone_office as COMPOFFICEPHONE,")
    '      sQuery.Append(" contact_first_name as CONTACTFIRSTNAME, contact_last_name AS CONTACTLASTNAME,")
    '      sQuery.Append(" contact_title as CONTACTTITLE, contact_email_address as CONTACTEMAIL,")
    '      sQuery.Append(" contact_phone_office as CONTACTOFFICEPHONE, contact_phone_mobile as CONTACTMOBILEPHONE,")
    '      sQuery.Append(" comp_id AS COMPID, contact_id as CONTACTID")
    '      sQuery.Append(" FROM View_Aircraft_Company_Flat WITH (NOLOCK) WHERE ac_id IN (")
    '      sQuery.Append(aclist)
    '      sQuery.Append(") AND ac_journ_id = 0 AND cref_contact_type IN('00','08','97') ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")


    '      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab6_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

    '      SqlCommand.CommandText = sQuery.ToString
    '      _recordSet = SqlCommand.ExecuteReader()

    '      Try
    '        _dataTable.Load(_recordSet)
    '      Catch constrExc As System.Data.ConstraintException
    '        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
    '      End Try

    '      _recordSet.Close()
    '      _recordSet = Nothing

    '      If _dataTable.Rows.Count > 0 Then

    '        htmlOut.Append("<table id=""tab6_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
    '        htmlOut.Append("<thead><tr>")
    '        htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

    '        If isMobileDisplay Then
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th></th>")
    '        Else
    '          htmlOut.Append("<th></th>")
    '          htmlOut.Append("<th>MAKE</th>")
    '          htmlOut.Append("<th>MODEL</th>")
    '          htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
    '          htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
    '          htmlOut.Append("<th>MFRYEAR</th>")
    '          htmlOut.Append("<th>DLVYEAR</th>")
    '          htmlOut.Append("<th>OWNERSHIP</th>")
    '          htmlOut.Append("<th>PERCENTOWNED</th>")
    '          htmlOut.Append("<th>OWNER</th>")
    '          htmlOut.Append("<th>OWNERCITY</th>")
    '          htmlOut.Append("<th>OWNERSTATE</th>")
    '          htmlOut.Append("<th>OWNERCOUNTRY</th>")
    '          htmlOut.Append("<th>OWNERWEBADDRESS</th>")
    '          htmlOut.Append("<th>OWNEREMAIL</th>")
    '          htmlOut.Append("<th>OWNEROFFICEPHONE</th>")
    '          htmlOut.Append("<th>CONTACTNAME</th>")
    '          htmlOut.Append("<th>CONTACTTITLE</th>")
    '          htmlOut.Append("<th>CONTACTEMAIL</th>")
    '          htmlOut.Append("<th>CONTACTOFFICEPHONE</th>")
    '          htmlOut.Append("<th>CONTACTMOBILEPHONE</th>")

    '        End If

    '        htmlOut.Append("</tr></thead><tbody>")

    '        Dim sSeparator As String = ""

    '        For Each r As DataRow In _dataTable.Rows

    '          htmlOut.Append("<tr>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MAKE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MAKE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MODEL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MODEL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")

    '          If Not IsDBNull(r.Item("SERNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
    '              htmlOut.Append(DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, True, r.Item("SERNO").ToString.Trim, "text_underline", ""))
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("REGNO")) Then
    '            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
    '              htmlOut.Append(r.Item("REGNO").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("MFRYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("DLVYEAR")) Then
    '            If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
    '              htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OWNERSHIP")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OWNERSHIP").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OWNERSHIP").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("PERCENTOWNED")) Then
    '            If Not String.IsNullOrEmpty(r.Item("PERCENTOWNED").ToString.Trim) Then
    '              htmlOut.Append(r.Item("PERCENTOWNED").ToString.Trim + "%")
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OWNER")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OWNER").ToString.Trim) Then

    '              htmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r.Item("COMPID"), 0, 0, True, Replace(r.Item("OWNER").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp), "text_underline", ""))

    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OWNERCITY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OWNERCITY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OWNERCITY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OWNERSTATE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OWNERSTATE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OWNERSTATE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("OWNERCOUNTRY")) Then
    '            If Not String.IsNullOrEmpty(r.Item("OWNERCOUNTRY").ToString.Trim) Then
    '              htmlOut.Append(r.Item("OWNERCOUNTRY").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("COMPWEBADDRESS")) Then
    '            If Not String.IsNullOrEmpty(r.Item("COMPWEBADDRESS").ToString.Trim) Then
    '              htmlOut.Append(r.Item("COMPWEBADDRESS").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("COMPEMAIL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("COMPEMAIL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("COMPEMAIL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("COMPOFFICEPHONE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("COMPOFFICEPHONE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("COMPOFFICEPHONE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayContactDetail.aspx?compid=" + r.Item("COMPID").ToString + "&jid=0&conid=" + r.Item("CONTACTID").ToString + """,""ContactDetails"");' title=""Display Contact Details"">")

    '          If Not IsDBNull(r.Item("CONTACTFIRSTNAME")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTFIRSTNAME").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTFIRSTNAME").ToString.Trim)
    '            End If
    '          End If

    '          If Not IsDBNull(r.Item("CONTACTLASTNAME")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTLASTNAME").ToString.Trim) Then
    '              htmlOut.Append(Constants.cSingleSpace + r.Item("CONTACTLASTNAME").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</a></td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("CONTACTTITLE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTTITLE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTTITLE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("CONTACTEMAIL")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTEMAIL").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTEMAIL").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("CONTACTOFFICEPHONE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTOFFICEPHONE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTOFFICEPHONE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("CONTACTMOBILEPHONE")) Then
    '            If Not String.IsNullOrEmpty(r.Item("CONTACTMOBILEPHONE").ToString.Trim) Then
    '              htmlOut.Append(r.Item("CONTACTMOBILEPHONE").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("</tr>")

    '        Next

    '      End If ' _dataTable.Rows.Count > 0 Then

    '      htmlOut.Append("</tbody></table>")
    '      'htmlOut.Append("<div id=""tab6_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
    '      htmlOut.Append("<div id=""tab6_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

    '    End If ' Not IsNothing(folderTable) Then

    '    out_htmlString = htmlOut.ToString

    '  Catch ex As Exception

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab6_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

    '  Finally

    '    htmlOut = Nothing

    '  End Try

    'End Sub
    Public Function display_tab7_results_table(ByRef AllowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByRef chartingString As String = "", Optional searchUpdate As UpdatePanel = Nothing) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Try

            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT DISTINCT amod_id, amod_make_name as MAKE, amod_model_name AS MODEL, COUNT(DISTINCT ac_id) AS NUMAIRCRAFT")
                sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 ")
                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                sQuery.Append(" GROUP BY amod_id, amod_make_name, amod_model_name ORDER BY amod_make_name, amod_model_name, amod_id")

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab7_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        AllowExport = False
                    End If

                    Dim sSeparator As String = ""
                    htmlOut.Append(" var tab7DataSet  = [ ")
                    For Each r As DataRow In _dataTable.Rows

                        If count > 0 Then
                            htmlOut.Append(",")
                        End If
                        htmlOut.Append("{")
                        htmlOut.Append("""check"": """",") 'Checkbox row.
                        htmlOut.Append("""id"": """ + r.Item("amod_id").ToString.Trim + """,") 'hidden ID


                        htmlOut.Append("""make"": """)

                        If Not IsDBNull(r.Item("MAKE")) Then
                            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                                htmlOut.Append(r.Item("MAKE").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",") 'make

                        htmlOut.Append("""model"": """)
                        If Not IsDBNull(r.Item("MODEL")) Then
                            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                                htmlOut.Append(r.Item("MODEL").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",") 'model



                        htmlOut.Append("""numaircraft"": """)

                        If Not IsDBNull(r.Item("NUMAIRCRAFT")) Then
                            If Not String.IsNullOrEmpty(r.Item("NUMAIRCRAFT").ToString.Trim) Then
                                htmlOut.Append(r.Item("NUMAIRCRAFT").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("""")
                        htmlOut.Append("}")
                        count += 1
                    Next
                    htmlOut.Append("];")

                End If ' _dataTable.Rows.Count > 0 Then



            End If ' Not IsNothing(folderTable) Then

            chartingString = DrawTopModelsBarChart(aclist, _dataTable, searchUpdate)



        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab7_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally



        End Try
        Return htmlOut
        htmlOut = Nothing

    End Function


    Public Function display_tab8_results_table(ByRef allowExport As Boolean, ByVal aclist As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByRef chartingString As String = "", Optional ByRef comp_id As Long = 0, Optional ByVal show_type As String = "", Optional ByVal comp_ids_string As String = "", Optional ByRef Show_Notes As Boolean = False) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Try
            Dim aclsData_Temp = New clsData_Manager_SQL
            If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                aclsData_Temp.JETNET_DB = clientConnectString
            End If


            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 600

                If Trim(comp_ids_string) <> "" Or Trim(comp_id) > 0 Then
                    sQuery.Append("SELECT ac_id, amod_make_name as MAKE,")
                Else
                    sQuery.Append("SELECT distinct ac_id, amod_make_name as MAKE,")
                End If

                sQuery.Append(" amod_model_name AS MODEL, ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
                sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO,")

                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True And HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                    sQuery.Append("  Aircraft_Flat.ac_asking_price as ASKING_PRICE, Aircraft_Flat.ac_list_date as LIST_DATE, Aircraft_Flat.ac_sale_price as SALE_PRICE , ")
                ElseIf HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = False And HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                    sQuery.Append("  Aircraft_Flat.ac_asking_price as ASKING_PRICE, Aircraft_Flat.ac_list_date as LIST_DATE, 0 as SALE_PRICE, ")
                Else
                    sQuery.Append("  0 as ASKING_PRICE, '' as LIST_DATE, 0 as SALE_PRICE, ")
                    'if aerodex get nothing, and shouldnt ever be spi true and aerodex true 
                End If



                sQuery.Append(" journ_date as TRANS_DATE, journ_subject as DESCRIPTION,")
                sQuery.Append(" acs_name as LIFECYCLE, acot_name as OWNERSHIP,")

                If Trim(comp_ids_string) <> "" Or Trim(comp_id) > 0 Then
                    sQuery.Append(" (SELECT TOP 1 actype_name   FROM Aircraft_Company_Flat WITH(NOLOCK)   ")
                    sQuery.Append(" WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = ac_journ_id  ")
                    sQuery.Append(" AND comp_id = cref_comp_id  )) AS RELATIONSHIP, ")
                End If

                sQuery.Append(" (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = ac_journ_id AND cref_contact_type IN('95'))) AS SELLER,")
                sQuery.Append(" (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE (cref_ac_id = Aircraft_Flat.ac_id AND cref_journ_id = ac_journ_id AND cref_contact_type IN('96'))) AS PURCHASER,")
                sQuery.Append(" ac_aport_name as BASEAPORT,")
                sQuery.Append(" ac_aport_country as BASECOUNTRY,")
                sQuery.Append(" ac_est_airframe_hrs as ESTAIRFRAMEHRS")
                sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) ")
                sQuery.Append(" inner join Aircraft_Reference with (NOLOCK) on cref_ac_id = ac_id and cref_journ_id = ac_journ_id ")
                sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id ")
                sQuery.Append(" WHERE ac_journ_id > 0")    '




                If Trim(comp_ids_string) <> "" Then
                    sQuery.Append(" AND cref_comp_id in (" & Trim(comp_ids_string) & ") ")
                ElseIf comp_id > 0 Then
                    sQuery.Append(" AND cref_comp_id = " & comp_id & " ")
                ElseIf Trim(aclist) <> "" Then ' if there is no comp id - then check if there is an ac list 
                    sQuery.Append(" and ac_id IN (" & aclist & ") ")
                End If

                ' sQuery.Append(" AND journ_date >= (getdate() - 365) ")
                sQuery.Append(" and  journ_subcat_code_part1 not in ('OM','MA','MS') ")

                If Trim(show_type) = "operated" Then
                    sQuery.Append(" AND cref_operator_flag IN('Y','O')  ")
                ElseIf Trim(show_type) = "own_operated" Then
                    sQuery.Append(" AND (cref_contact_type IN('95','96') or cref_operator_flag IN('Y','O')) ")
                ElseIf Trim(show_type) = "brokered" Then
                    sQuery.Append(" AND cref_contact_type IN('99','2P','IV')  ")
                ElseIf Trim(show_type) = "managed" Then
                    sQuery.Append(" AND cref_contact_type IN('31') ")
                End If

                'sQuery.Append(" and cref_contact_type in ('99','93','38','95','96', '2X', 'IV', '2P') ")


                sQuery.Append(" and  journ_subcat_code_part1 not in ('OM','MA','MS') ")
                sQuery.Append(" and  journ_internal_trans_flag='N' ")

                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab8_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing




                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        allowExport = False
                    End If

                    Dim sSeparator As String = ""
                    htmlOut.Append(" var tab8DataSet  = [ ")
                    For Each r As DataRow In _dataTable.Rows

                        If count > 0 Then
                            htmlOut.Append(",")
                        End If
                        htmlOut.Append("{")
                        htmlOut.Append("""check"": """",") 'Checkbox row.
                        htmlOut.Append("""id"": """ + r.Item("ac_id").ToString.Trim + """,") 'hidden ID

                        htmlOut.Append("""note"": """)

                        If Show_Notes = True Then
                            htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                        Else
                            htmlOut.Append("")
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""make"": """)

                        If Not IsDBNull(r.Item("MAKE")) Then
                            If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                                htmlOut.Append(r.Item("MAKE").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",") 'make

                        htmlOut.Append("""model"": """)
                        If Not IsDBNull(r.Item("MODEL")) Then
                            If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                                htmlOut.Append(r.Item("MODEL").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append(""",") 'model

                        htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("MAKE") & " " & r("MODEL") & " S/N #" & r("SERNO") & """>" & r("SERNO").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")


                        htmlOut.Append("""reg"": """)

                        If Not IsDBNull(r.Item("REGNO")) Then
                            If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
                                htmlOut.Append(r.Item("REGNO").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""mfryear"": """)

                        If Not IsDBNull(r.Item("MFRYEAR")) Then
                            If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
                                htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""dlvyear"": """)

                        If Not IsDBNull(r.Item("DLVYEAR")) Then
                            If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
                                htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""TRANS_DATE"": """)

                        If Not IsDBNull(r.Item("TRANS_DATE")) Then
                            If Not String.IsNullOrEmpty(r.Item("TRANS_DATE").ToString.Trim) Then
                                htmlOut.Append(FormatDateTime(r.Item("TRANS_DATE").ToString.Trim, DateFormat.ShortDate))
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""DESCRIPTION"": """)

                        If Not IsDBNull(r.Item("DESCRIPTION")) Then
                            If Not String.IsNullOrEmpty(r.Item("DESCRIPTION").ToString.Trim) Then
                                htmlOut.Append(Replace(r.Item("DESCRIPTION").ToString.Trim, """", ""))
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""ASKING_PRICE"": """)

                        If Not IsDBNull(r.Item("ASKING_PRICE")) Then
                            If Not String.IsNullOrEmpty(r.Item("ASKING_PRICE").ToString.Trim) Then
                                htmlOut.Append(r.Item("ASKING_PRICE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")



                        htmlOut.Append("""LIST_DATE"": """)

                        If Not IsDBNull(r.Item("LIST_DATE")) Then
                            If Not String.IsNullOrEmpty(r.Item("LIST_DATE").ToString.Trim) Then
                                htmlOut.Append(FormatDateTime(r.Item("LIST_DATE").ToString.Trim, DateFormat.ShortDate))
                            End If
                        End If

                        htmlOut.Append(""",")



                        htmlOut.Append("""SALE_PRICE"": """)

                        If Not IsDBNull(r.Item("SALE_PRICE")) Then
                            If Not String.IsNullOrEmpty(r.Item("SALE_PRICE").ToString.Trim) Then
                                htmlOut.Append(r.Item("SALE_PRICE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")


                        'If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True And HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                        '  htmlOut.Append("""ASKING_PRICE"": """)

                        '  If Not IsDBNull(r.Item("ASKING_PRICE")) Then
                        '    If Not String.IsNullOrEmpty(r.Item("ASKING_PRICE").ToString.Trim) Then
                        '      htmlOut.Append(r.Item("ASKING_PRICE").ToString.Trim)
                        '    End If
                        '  End If

                        '  htmlOut.Append(""",")



                        '  htmlOut.Append("""LIST_DATE"": """)

                        '  If Not IsDBNull(r.Item("LIST_DATE")) Then
                        '    If Not String.IsNullOrEmpty(r.Item("LIST_DATE").ToString.Trim) Then
                        '      htmlOut.Append(r.Item("LIST_DATE").ToString.Trim)
                        '    End If
                        '  End If

                        '  htmlOut.Append(""",")



                        '  htmlOut.Append("""SALE_PRICE"": """)

                        '  If Not IsDBNull(r.Item("SALE_PRICE")) Then
                        '    If Not String.IsNullOrEmpty(r.Item("SALE_PRICE").ToString.Trim) Then
                        '      htmlOut.Append(r.Item("SALE_PRICE").ToString.Trim)
                        '    End If
                        '  End If

                        '  htmlOut.Append(""",")
                        'ElseIf HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = False And HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                        '  htmlOut.Append("""ASKING_PRICE"": """)

                        '  If Not IsDBNull(r.Item("ASKING_PRICE")) Then
                        '    If Not String.IsNullOrEmpty(r.Item("ASKING_PRICE").ToString.Trim) Then
                        '      htmlOut.Append(r.Item("ASKING_PRICE").ToString.Trim)
                        '    End If
                        '  End If

                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""LIST_DATE"": """)

                        '  If Not IsDBNull(r.Item("LIST_DATE")) Then
                        '    If Not String.IsNullOrEmpty(r.Item("LIST_DATE").ToString.Trim) Then
                        '      htmlOut.Append(r.Item("LIST_DATE").ToString.Trim)
                        '    End If
                        '  End If

                        '  htmlOut.Append(""",") 
                        'End If






                        htmlOut.Append("""lifecycle"": """)

                        If Not IsDBNull(r.Item("LIFECYCLE")) Then
                            If Not String.IsNullOrEmpty(r.Item("LIFECYCLE").ToString.Trim) Then
                                htmlOut.Append(r.Item("LIFECYCLE").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""ownership"": """)

                        If Not IsDBNull(r.Item("OWNERSHIP")) Then
                            If Not String.IsNullOrEmpty(r.Item("OWNERSHIP").ToString.Trim) Then
                                htmlOut.Append(r.Item("OWNERSHIP").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")


                        If Trim(comp_ids_string) <> "" Or Trim(comp_id) > 0 Then
                            htmlOut.Append("""RELATIONSHIP"": """)

                            If Not IsDBNull(r.Item("RELATIONSHIP")) Then
                                If Not String.IsNullOrEmpty(r.Item("RELATIONSHIP").ToString.Trim) Then
                                    htmlOut.Append(r.Item("RELATIONSHIP").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")
                        End If

                        htmlOut.Append("""seller"": """)

                        If Not IsDBNull(r.Item("SELLER")) Then
                            If Not String.IsNullOrEmpty(r.Item("SELLER").ToString.Trim) Then
                                htmlOut.Append(r.Item("SELLER").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""purchaser"": """)

                        If Not IsDBNull(r.Item("PURCHASER")) Then
                            If Not String.IsNullOrEmpty(r.Item("PURCHASER").ToString.Trim) Then
                                htmlOut.Append(r.Item("PURCHASER").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("""")

                        'htmlOut.Append("""baseaport"": """)

                        'If Not IsDBNull(r.Item("BASEAPORT")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("BASEAPORT").ToString.Trim) Then
                        '    htmlOut.Append(r.Item("BASEAPORT").ToString.Trim)
                        '  End If
                        'End If

                        'htmlOut.Append(""",")

                        'htmlOut.Append("""basecountry"": """)

                        'If Not IsDBNull(r.Item("BASECOUNTRY")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("BASECOUNTRY").ToString.Trim) Then
                        '    htmlOut.Append(r.Item("BASECOUNTRY").ToString.Trim)
                        '  End If
                        'End If

                        'htmlOut.Append(""",")

                        'htmlOut.Append("""estairframehrs"": """)

                        'If Not IsDBNull(r.Item("ESTAIRFRAMEHRS")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("ESTAIRFRAMEHRS").ToString.Trim) Then
                        '    htmlOut.Append(r.Item("ESTAIRFRAMEHRS").ToString.Trim)
                        '  End If
                        'End If

                        'htmlOut.Append("""")
                        htmlOut.Append("}")
                        count += 1
                    Next
                    htmlOut.Append("];")

                Else
                    htmlOut.Append(" var tab8DataSet  = [ ")
                    htmlOut.Append("{")
                    htmlOut.Append("""check"": """",") 'Checkbox row.
                    htmlOut.Append("""id"": """",") 'hidden ID
                    htmlOut.Append("""make"": """)
                    htmlOut.Append("NO TRANSACTIONS FOUND"",") 'make 
                    htmlOut.Append("""model"": """)
                    htmlOut.Append(""",") 'model 
                    htmlOut.Append("""ser"": [""""],")
                    htmlOut.Append("""reg"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""mfryear"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""dlvyear"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""TRANS_DATE"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""DESCRIPTION"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""ASKING_PRICE"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""LIST_DATE"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""SALE_PRICE"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""lifecycle"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""ownership"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""RELATIONSHIP"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""seller"": """)
                    htmlOut.Append(""",")
                    htmlOut.Append("""purchaser"": """)
                    htmlOut.Append("""")
                    htmlOut.Append("}")
                    htmlOut.Append("];")

                End If ' _dataTable.Rows.Count > 0 Then

            End If ' Not IsNothing(folderTable) Then 

            chartingString = htmlOut.ToString

            '    chartingString = DrawHistoryChart(aclist, _dataTable)



        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab7_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally



        End Try
        Return htmlOut
        htmlOut = Nothing

    End Function


    Public Function display_tab9_results_table(ByRef allowExport As Boolean, ByVal aclist As String, ByVal isMobileDisplay As Boolean, ByRef chartingString As String, ByRef comp_id As Long, ByVal show_type As String, ByVal comp_ids_string As String, ByVal field_to_sum As String, ByRef summary_table As DataTable, Optional ByRef Show_Notes As Boolean = False) As StringBuilder

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Try
            Dim aclsData_Temp = New clsData_Manager_SQL
            If ((Not HttpContext.Current.Session.Item("localUser").crmDemoUserFlag) And (HttpContext.Current.Session.Item("localUser").crmEnableNotes) _
               And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag) And (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetServerNotesDatabase"))) Then
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                aclsData_Temp.JETNET_DB = clientConnectString
            End If


            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = clientConnectStr

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 600

                'sQuery.Append(" select distinct ac_aport_iata_code as 'Base Airport IATA Code', ac_aport_icao_code as 'Base Airport ICAO Code', ")
                'sQuery.Append(" ac_aport_faaid_code as 'Base FAA ID Code', ac_aport_name as 'Base Airport Name',  ")
                'sQuery.Append(" ac_aport_city as 'Base Airport City', ac_aport_state_name as 'Base Airport State Name', ")
                'sQuery.Append(" ac_aport_country as 'Base Airport Country',   ac_country_continent_name as 	'Base Continent', ")
                'sQuery.Append(" ac_country_of_registration as 'Country of Registration',")


                If Trim(field_to_sum) = "" Then
                    sQuery.Append(" select distinct ac_aport_iata_code, ac_aport_icao_code,  ")
                    sQuery.Append(" ac_aport_faaid_code, ac_aport_name ,  ")
                    sQuery.Append(" ac_aport_city, ac_aport_state_name , ")
                    sQuery.Append(" ac_aport_country ,   ac_country_continent_name, ")
                    sQuery.Append(" ac_country_of_registration,")
                    sQuery.Append(" amod_make_name as 'MAKE', amod_model_name as 'MODEL', ac_ser_no_full as 'SERNO', ac_ser_no_sort, ")
                    sQuery.Append(" ac_reg_no as 'REGNO', ac_mfr_year as 'MFRYEAR', ac_year as 'DLVYEAR', ac_id ")
                    sQuery.Append(" From View_Aircraft_Company_Flat WITH(NOLOCK) ")
                    sQuery.Append(" where ac_journ_id = 0 ")

                    If Trim(aclist) <> "" Then
                        sQuery.Append(" and ac_id in (" & Trim(aclist) & ")")
                    End If
                    sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

                Else
                    sQuery.Append(" select distinct " & field_to_sum & " as 'Location', count(distinct ac_id) as tcount  ")
                    sQuery.Append(" From View_Aircraft_Company_Flat WITH(NOLOCK) ")
                    sQuery.Append(" where ac_journ_id = 0 ")

                    If Trim(aclist) <> "" Then
                        sQuery.Append(" and ac_id in (" & Trim(aclist) & ")")
                    End If

                    sQuery.Append(" group by " & field_to_sum & "  ")
                    sQuery.Append(" order by count(distinct ac_id) desc  ")
                End If




                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_tab8_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If Not IsNothing(summary_table) And Not IsNothing(_dataTable) Then
                    summary_table = _dataTable
                End If

                If _dataTable.Rows.Count > 0 Then

                    If _dataTable.Rows.Count >= HttpContext.Current.Session.Item("localUser").crmMaxClientExport Then
                        allowExport = False
                    End If

                    Dim sSeparator As String = ""
                    htmlOut.Append(" var tab9DataSet  = [ ")

                    For Each r As DataRow In _dataTable.Rows

                        If Trim(field_to_sum) = "" Then
                            If count > 0 Then
                                htmlOut.Append(",")
                            End If
                            htmlOut.Append("{")
                            htmlOut.Append("""check"": """",") 'Checkbox row.
                            htmlOut.Append("""id"": """ + r.Item("ac_id").ToString.Trim + """,") 'hidden ID

                            htmlOut.Append("""note"": """)

                            If Show_Notes = True Then
                                htmlOut.Append(AircraftBuildNote_Portfolio(r.Item("ac_id"), aclsData_Temp, "AC"))
                            Else
                                htmlOut.Append("")
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""make"": """)

                            If Not IsDBNull(r.Item("MAKE")) Then
                                If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MAKE").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append(""",") 'make

                            htmlOut.Append("""model"": """)
                            If Not IsDBNull(r.Item("MODEL")) Then
                                If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MODEL").ToString.Trim)
                                End If
                            End If
                            htmlOut.Append(""",") 'model

                            htmlOut.Append("""ser"": [""" & Replace(Replace("<a " & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, False, "", "", "") & " title=""" & r("MAKE") & " " & r("MODEL") & " S/N #" & r("SERNO") & """>" & r("SERNO").ToString & "</a>", """", "\"""), "'", "\'") & """,   """ & r("ac_ser_no_sort").ToString & """],")


                            htmlOut.Append("""reg"": """)

                            If Not IsDBNull(r.Item("REGNO")) Then
                                If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
                                    htmlOut.Append(r.Item("REGNO").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""mfryear"": """)

                            If Not IsDBNull(r.Item("MFRYEAR")) Then
                                If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""dlvyear"": """)

                            If Not IsDBNull(r.Item("DLVYEAR")) Then
                                If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
                                    htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")



                            htmlOut.Append("""BASE_IATA"": """)

                            If Not IsDBNull(r.Item("ac_aport_iata_code")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_iata_code").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_aport_iata_code").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""BASE_ICAO"": """)

                            If Not IsDBNull(r.Item("ac_aport_icao_code")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_icao_code").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_aport_icao_code").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""FAA_ID"": """)

                            If Not IsDBNull(r.Item("ac_aport_faaid_code")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_faaid_code").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_aport_faaid_code").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""APORT_NAME"": """)

                            If Not IsDBNull(r.Item("ac_aport_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_aport_name").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")


                            htmlOut.Append("""APORT_CITY"": """)

                            If Not IsDBNull(r.Item("ac_aport_city")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_city").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_aport_city").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""APORT_STATE"": """)

                            If Not IsDBNull(r.Item("ac_aport_state_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_state_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_aport_state_name").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""APORT_COUNTRY"": """)

                            If Not IsDBNull(r.Item("ac_aport_country")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_country").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_aport_country").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""APORT_CONTINENT"": """)

                            If Not IsDBNull(r.Item("ac_country_continent_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_country_continent_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_country_continent_name").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append(""",")

                            htmlOut.Append("""REG_COUNTRY"": """)

                            If Not IsDBNull(r.Item("ac_country_of_registration")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_country_of_registration").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ac_country_of_registration").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("""")


                            'htmlOut.Append("""")
                            htmlOut.Append("}")
                            count += 1

                        Else

                            If count > 0 Then
                                htmlOut.Append(",")
                            End If
                            htmlOut.Append("{")
                            htmlOut.Append("""check"": """",") 'Checkbox row. 
                            htmlOut.Append("""id"": """ & count & """,") 'hidden ID
                            htmlOut.Append("""Location"":")

                            If Not IsDBNull(r.Item("Location")) Then
                                If Not String.IsNullOrEmpty(r.Item("Location").ToString.Trim) Then
                                    If r.Item("Location").ToString.ToLower = "unknown" Then
                                        htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                                    Else
                                        htmlOut.Append("[""" & r.Item("Location").ToString.Trim & """,""" & r.Item("Location").ToString.Trim & """]")
                                    End If
                                Else
                                    htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                                End If
                            Else
                                htmlOut.Append("[""Unknown"",""ZZZZZUnknown""]")
                            End If


                            htmlOut.Append(",") 'make


                            htmlOut.Append("""Total"": """)

                            If Not IsDBNull(r.Item("tcount")) Then
                                If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then
                                    htmlOut.Append(r.Item("tcount").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("""")


                            'htmlOut.Append("""")
                            htmlOut.Append("}")
                            count += 1

                        End If




                    Next
                    htmlOut.Append("];")

                Else

                    If Trim(field_to_sum) = "" Then
                        htmlOut.Append(" var tab9DataSet  = [ ")
                        htmlOut.Append("{")
                        htmlOut.Append("""check"": """",") 'Checkbox row.
                        htmlOut.Append("""id"": """",") 'hidden ID
                        htmlOut.Append("""make"": """)
                        htmlOut.Append("NO TRANSACTIONS FOUND"",") 'make 
                        htmlOut.Append("""model"": """)
                        htmlOut.Append(""",") 'model 
                        htmlOut.Append("""ser"": [""""],")
                        htmlOut.Append("""reg"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""mfryear"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""dlvyear"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""BASE_IATA"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""BASE_ICAO"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""FAA_ID"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""APORT_NAME"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""APORT_CITY"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""APORT_STATE"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""APORT_COUNTRY"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""APORT_CONTINENT"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""REG_COUNTRY"": """)
                        htmlOut.Append("""")
                        htmlOut.Append("}")
                        htmlOut.Append("];")
                    Else
                        htmlOut.Append(" var tab9DataSet  = [ ")
                        htmlOut.Append("{")
                        htmlOut.Append("""check"": """",") 'Checkbox row.
                        htmlOut.Append("""id"": """",") 'hidden ID  
                        htmlOut.Append("""LOCATION"": """)
                        htmlOut.Append(""",")
                        htmlOut.Append("""COUNT"": """)
                        htmlOut.Append("""")
                        htmlOut.Append("}")
                        htmlOut.Append("];")
                    End If


                End If ' _dataTable.Rows.Count > 0 Then

            End If ' Not IsNothing(folderTable) Then 

            chartingString = htmlOut.ToString

            '    chartingString = DrawHistoryChart(aclist, _dataTable)



        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab7_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally



        End Try
        Return htmlOut
        htmlOut = Nothing

    End Function

#End Region

#Region "CHART_FUNCTIONS"

    Public Function DrawTopModelsBarChart(ByVal acList As String, ByVal dataModels As DataTable, Optional searchUpdate As UpdatePanel = Nothing) As String
        Dim whole As Integer = 0
        Dim sharedOw As Integer = 0
        Dim frac As Integer = 0
        Dim chartResults As String = ""
        Dim chartString As String = ""
        Dim ResultsTable As New DataTable
        Dim chartInside As String = ""
        Dim counter As Integer = 0
        chartString = " function drawTopModelsBar() {" & vbNewLine

        chartString += " var data = new google.visualization.DataTable();" & vbNewLine
        chartString += "data.addColumn('string', 'Type');" & vbNewLine
        chartString += "data.addColumn('number', '# of Aircraft');" & vbNewLine


        chartString += "data.addRows([" & vbNewLine
        If dataModels.Rows.Count > 0 Then
            Dim afiltered As DataRow() = dataModels.Select("", "NUMAIRCRAFT DESC")

            For Each r In afiltered
                counter += 1
                If counter <= 25 Then
                    If chartInside <> "" Then
                        chartInside += ", " & vbNewLine
                    End If
                    chartInside += "['" & r("MAKE").ToString & " " & r("MODEL").ToString & "'," & r("NUMAIRCRAFT").ToString & "]"
                End If
            Next
        End If
        chartString += chartInside
        chartString += "]);" & vbNewLine

        chartString += "var options = {colors: ['#078fd7'],"
        chartString += "chartArea:{top:20, width:'95%',height:'65%'},"
        'chartString += "width: 950,"
        chartString += "fontSize: 10,"
        'chartString += "height: 480,"
        chartString += "legend:  'none',"
        chartString += " hAxis: { slantedText: true, slantedTextAngle: 90 }"
        chartString += "  };"


        chartString += "var chart = new google.visualization.ColumnChart(" & vbNewLine
        chartString += "document.getElementById('top_models_graphs'));" & vbNewLine

        chartString += "chart.draw(data, options);" & vbNewLine
        chartString += "}; drawTopModelsBar();" & vbNewLine
        If Not IsNothing(searchUpdate) Then
            PageResize(5, searchUpdate, "drawTopModelsBar", "top_models_graphs")
        End If
        Return chartString
    End Function

    Public Function DrawHistoryChart(ByVal acList As String, ByVal dataModels As DataTable) As String
        Dim whole As Integer = 0
        Dim sharedOw As Integer = 0
        Dim frac As Integer = 0
        Dim chartResults As String = ""
        Dim chartString As String = ""
        Dim ResultsTable As New DataTable
        Dim chartInside As String = ""
        Dim counter As Integer = 0
        chartString = " function drawHistoryBar() {" & vbNewLine

        chartString += " var data = new google.visualization.DataTable();" & vbNewLine
        chartString += "data.addColumn('string', 'Type');" & vbNewLine
        chartString += "data.addColumn('number', '# of Aircraft');" & vbNewLine


        chartString += "data.addRows([" & vbNewLine
        If dataModels.Rows.Count > 0 Then
            Dim afiltered As DataRow() = dataModels.Select("", "NUMAIRCRAFT DESC")

            For Each r In afiltered
                counter += 1
                If counter <= 25 Then
                    If chartInside <> "" Then
                        chartInside += ", " & vbNewLine
                    End If
                    chartInside += "['" & r("MAKE").ToString & " " & r("MODEL").ToString & "'," & r("NUMAIRCRAFT").ToString & "]"
                End If
            Next
        End If
        chartString += chartInside
        chartString += "]);" & vbNewLine

        chartString += "var options = {"
        chartString += "chartArea:{top:20, width:'95%',height:'65%'},"
        chartString += "width: 950,"
        chartString += "fontSize: 10,"
        chartString += "height: 480,"
        chartString += "legend:  'none',"
        chartString += " hAxis: { slantedText: true, slantedTextAngle: 90 }"
        chartString += "  };"


        chartString += "var chart = new google.visualization.ColumnChart(" & vbNewLine
        chartString += "document.getElementById('history_graphs'));" & vbNewLine

        chartString += "chart.draw(data, options);" & vbNewLine
        chartString += "}; drawHistoryBar();" & vbNewLine

        Return chartString
    End Function

    Public Function DrawOwnerContinentPieChart(ByVal acList As String, ByVal operatorFlag As Boolean, ByVal ownerFlag As Boolean, searchUpdate As UpdatePanel) As String
        Dim whole As Integer = 0
        Dim sharedOw As Integer = 0
        Dim frac As Integer = 0
        Dim chartResults As String = ""
        Dim chartString As String = " function drawChart" & IIf(operatorFlag, "Operator", "Owner") & "Continent() {"
        Dim ResultsTable As New DataTable
        ResultsTable = GetContinents(acList, operatorFlag, ownerFlag)
        chartString += " var data = google.visualization.arrayToDataTable(["
        chartString += " ['Country', '#'],"

        If Not IsNothing(ResultsTable) Then
            If ResultsTable.Rows.Count > 0 Then
                For Each r As DataRow In ResultsTable.Rows
                    If chartResults <> "" Then
                        chartResults += ", "
                    End If
                    chartResults += " ['" & r("country_continent_name") & "',     " & r("tcount") & "]"
                Next
            End If
        End If

        chartString += chartResults
        chartString += " ]);"

        chartString += " var options = {"
        chartString += " chartArea:{width:'95%',height:'95%'}, legendFontSize:12"
        chartString += " };"

        chartString += " var chart = new google.visualization.PieChart(document.getElementById('" & IIf(operatorFlag, "operator", "owner") & "_continent_chart'));"

        chartString += " chart.draw(data, options);"
        chartString += " }; drawChart" & IIf(operatorFlag, "Operator", "Owner") & "Continent();"
        PageResize(IIf(operatorFlag, 107, 108), searchUpdate, "drawChart" & IIf(operatorFlag, "Operator", "Owner") & "Continent", IIf(operatorFlag, "Operator", "owner") & "_continent_chart")
        Return chartString
    End Function
    Public Function DrawOperatorBusinessType(ByVal acList As String, searchUpdate As UpdatePanel) As String
        Dim tempTable As New DataTable
        Dim chartString As String = " function drawChartOperatorBusiness() {"
        Dim chartInside As String = ""
        tempTable = GetOperatorBusinessType(acList)
        chartString += " var data = google.visualization.arrayToDataTable(["
        chartString += " ['Certification Type', '#'],"


        For Each r As DataRow In tempTable.Rows
            If chartInside <> "" Then
                chartInside += ", "
            End If
            chartInside += " ['" & r("cbus_name") & "',     " & r("tcount").ToString & "]"
        Next
        chartString += chartInside
        chartString += " ]);"

        chartString += " var options = {"
        chartString += " chartArea:{width:'95%',height:'95%'}, legendFontSize:12"
        chartString += " };"

        chartString += " var chart = new google.visualization.PieChart(document.getElementById('operator_business_chart'));"

        chartString += " chart.draw(data, options);"
        chartString += " }; drawChartOperatorBusiness();"
        PageResize(100, searchUpdate, "drawChartOperatorBusiness", "operator_business_chart")
        Return chartString
    End Function
    Public Function DrawOperatorCertifications(ByVal acList As String) As String
        Dim tempTable As New DataTable
        Dim chartString As String = " function drawChartOperatorCert() {"
        Dim chartInside As String = ""
        tempTable = GetOperatorCertifications(acList)
        chartString += " var data = google.visualization.arrayToDataTable(["
        chartString += " ['Certification Type', '#'],"

        For Each r As DataRow In tempTable.Rows
            If chartInside <> "" Then
                chartInside += ", "
            End If
            chartInside += " ['" & r("ccerttype_type") & "',     " & r("tcount").ToString & "]"
        Next
        chartString += chartInside
        chartString += " ]);"

        chartString += " var options = {"
        chartString += " chartArea:{width:'95%',height:'95%'}, legendFontSize:12"
        chartString += " };"

        chartString += " var chart = new google.visualization.PieChart(document.getElementById('operator_cert_chart'));"

        chartString += " chart.draw(data, options);"
        chartString += " }; drawChartOperatorCert();"
        Return chartString
    End Function
    Public Function DrawOwnershipPieChart(ByVal acList As String, searchUpdate As UpdatePanel) As String
        Dim whole As Integer = 0
        Dim sharedOw As Integer = 0
        Dim frac As Integer = 0
        Dim chartString As String = " function drawChartOwnership() {"
        display_ownership_composition_results(acList, whole, frac, sharedOw)
        chartString += " var data = google.visualization.arrayToDataTable(["
        chartString += " ['Ownership', '#'],"
        chartString += " ['Whole',     " & whole.ToString & "],"
        chartString += " ['Shared',      " & sharedOw.ToString & "],"
        chartString += " ['Fractional',  " & frac.ToString & "],"
        chartString += " ]);"

        chartString += " var options = {"
        chartString += " chartArea:{width:'95%',height:'95%'}, legendFontSize:13"
        chartString += " };"

        chartString += " var chart = new google.visualization.PieChart(document.getElementById('ownership_pie_chart'));"

        chartString += " chart.draw(data, options);"
        chartString += " }; drawChartOwnership();"

        PageResize(110, searchUpdate, "drawChartOwnership", "ownership_pie_chart")

        Return chartString
    End Function
    Public Function get_portfolio_passenger_bar_chart_info(ByVal aclist As String, ByVal summarize_field As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim AFTTQuery As String = ""
        Try



            If summarize_field = "ac_airframe_tot_hrs" Then
                sQuery.Append("SELECT COUNT(DISTINCT ac_id) AS NUMAIRCRAFT, ")
                AFTTQuery = "case "

                Dim CeilingAFTT As Long = 15000

                For x As Integer = 0 To CeilingAFTT Step 1000
                    If x = CeilingAFTT Then
                        AFTTQuery += " when ac_airframe_tot_hrs >= " & x & " and ac_airframe_tot_hrs <= " & x + 1000 & " then '" & x & " - " & (x + 1000) & "' "
                    Else
                        If x = 0 Then
                            AFTTQuery += " when ac_airframe_tot_hrs >= 1 and ac_airframe_tot_hrs < 1000 then '1 - 999' "
                        Else
                            AFTTQuery += " when ac_airframe_tot_hrs >= " & x & " and ac_airframe_tot_hrs < " & x + 1000 & " then '" & x & " - " & (x + 1000) - 1 & "' "
                        End If
                    End If
                Next

                AFTTQuery += " end "
                sQuery.Append(AFTTQuery & " as 'SUMMARIZED'")
            Else
                sQuery.Append("SELECT DISTINCT " & summarize_field & " AS 'SUMMARIZED', COUNT(DISTINCT ac_id) AS NUMAIRCRAFT")
            End If



            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) ")

            If Trim(summarize_field) = "acuse_name" Then
                sQuery.Append(" inner Join  Aircraft_Useage with (NOLOCK) on acuse_code = ac_use_code")
            End If

            sQuery.Append(" WHERE ac_id IN (")
            sQuery.Append(aclist)
            sQuery.Append(") And ac_journ_id = 0 ")
            sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

            If summarize_field = "ac_airframe_tot_hrs" Then
                sQuery.Append(" GROUP BY ( " & AFTTQuery & " ) ORDER BY cast( replace( ( " & AFTTQuery & " ), ' - ', '') as float) asc ")
            Else
                sQuery.Append(" GROUP BY " & summarize_field & " ORDER BY " & summarize_field & " asc, COUNT(DISTINCT ac_id)")
            End If


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_portfolio_aircraft_age_bar_chart_info(ByVal aclist As String) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_portfolio_aircraft_age_bar_chart_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_portfolio_aircraft_age_bar_chart_info(ByVal aclist As String) As DataTable " + ex.Message

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

    Public Function get_portfolio_aircraft_age_bar_chart_info(ByVal aclist As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT ac_mfr_year AS YEARMFR, COUNT(DISTINCT ac_id) AS NUMAIRCRAFT")
            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
            sQuery.Append(aclist)
            sQuery.Append(") And ac_journ_id = 0 ")
            sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

            sQuery.Append(" GROUP BY ac_mfr_year ORDER BY ac_mfr_year, COUNT(DISTINCT ac_id)")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_portfolio_aircraft_age_bar_chart_info(ByVal aclist As String) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_portfolio_aircraft_age_bar_chart_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_portfolio_aircraft_age_bar_chart_info(ByVal aclist As String) As DataTable " + ex.Message

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

    Public Sub display_portfolio_aircraft_age_bar_chart(ByVal aclist As String, ByVal graphID As Integer, ByVal searchUpdate As UpdatePanel, Optional ByVal summarize_field As String = "", Optional summarize_text As String = "")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim x As Integer = 0


        Try

            If Trim(summarize_field) = "" Then
                results_table = get_portfolio_aircraft_age_bar_chart_info(aclist)
            Else
                results_table = get_portfolio_passenger_bar_chart_info(aclist, summarize_field)
            End If


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
                    htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
                    htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
                    htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                    htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
                    htmlOut.Append("data.addColumn('string', 'Label');" + vbCrLf)
                    htmlOut.Append("data.addColumn('number', 'Number of Aircraft');" + vbCrLf)
                    '  If Trim(summarize_field) = "" Then

                    '  Else
                    '      htmlOut.Append("data.addColumn('number', 'Summarized');" + vbCrLf)
                    '  End If

                    htmlOut.Append("data.addRows(" + CStr(results_table.Rows.Count) + ");" + vbCrLf)

                    For Each r As DataRow In results_table.Rows

                        If Trim(summarize_field) = "" Then
                            If Not IsDBNull(r.Item("NUMAIRCRAFT")) Then
                                If Not String.IsNullOrEmpty(r.Item("NUMAIRCRAFT").ToString.Trim) Then
                                    If CLng(r.Item("NUMAIRCRAFT").ToString.Trim) > 0 Then
                                        htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + r.Item("YEARMFR").ToString + "');" + vbCrLf)
                                        htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("NUMAIRCRAFT").ToString + ");" + vbCrLf)
                                    Else
                                        htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + r.Item("YEARMFR").ToString + "');" + vbCrLf)
                                        htmlOut.Append("data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                                    End If
                                    x += 1
                                End If
                            End If
                        Else
                            If Not IsDBNull(r.Item("NUMAIRCRAFT")) Then
                                If Not String.IsNullOrEmpty(r.Item("NUMAIRCRAFT").ToString.Trim) Then
                                    If Not IsDBNull(r.Item("SUMMARIZED")) Then
                                        If Not String.IsNullOrEmpty(r.Item("SUMMARIZED")) Then
                                            If Not (r.Item("SUMMARIZED").ToString.ToLower) = "unknown" Then
                                                If CLng(r.Item("NUMAIRCRAFT").ToString.Trim) > 0 Then
                                                    htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + r.Item("SUMMARIZED").ToString + "');" + vbCrLf)
                                                    htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("NUMAIRCRAFT").ToString + ");" + vbCrLf)
                                                Else
                                                    htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + r.Item("SUMMARIZED").ToString + "');" + vbCrLf)
                                                    htmlOut.Append("data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                                                End If
                                                x += 1
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If

                    Next

                    htmlOut.Append("var chart = new google.visualization.ColumnChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
                    htmlOut.Append("chart.draw(data, {chartArea:{width:'85%',height:'70%'},colors: ['#078fd7'], title:'', slantedText:'true', slantedTextAngle:60, legend:'none', legendFontSize:12, tooltipFontSize:9});" + vbCrLf)

                    htmlOut.Append("};drawVisualization" + graphID.ToString + "();" + vbCrLf)


                    PageResize(graphID, searchUpdate, "", "")

                    htmlOut.Append("</script>" + vbCrLf)

                End If

            End If

            'This needs to be called from javascript
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab0barChart", htmlOut.ToString, False)
            'htmlOut = New StringBuilder
            'htmlOut.Append("<div class=""Box"">")
            'If Not String.IsNullOrEmpty(htmlOut.ToString.Trim) Then
            '  htmlOut.Append("<table width=""100%"" cellpadding=""2"" cellspacing=""0"">")
            '  htmlOut.Append("<tr><td valign=""middle"" align=""center""><span class=""subHeader"">AGE OF FLEET BY MFR YEAR</span></td></tr>")
            '  htmlOut.Append("<tr><td valign=""top"" align=""left""><div id=""visualization" + graphID.ToString + """ style=""text-align:center; width:100%; height:295px;""></div></td></tr></table>")
            'Else
            '  htmlOut.Append("<table width=""100%"" cellpadding=""2"" cellspacing=""0"">")
            '  htmlOut.Append("<tr><td valign=""middle"" align=""center""><span class=""subHeader"">AGE OF FLEET BY MFR YEAR</span></td></tr>")
            '  htmlOut.Append("<tr><td valign=""top"" align=""left""><div style=""text-align:center; width:100%; height:295px;"">No Data to display</div></td></tr></table>")
            'End If
            'htmlOut.Append("</div>")
        Catch ex As Exception

            aError = "Error in display_portfolio_aircraft_age_bar_chart(ByVal aclist As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        'out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub PageResize(graphID As Integer, searchUpdate As UpdatePanel, functionName As String, chartIDDiv As String)
        Dim htmlOut As New StringBuilder
        htmlOut.Append("$(window).resize(function() {" + vbCrLf)
        htmlOut.Append("if(this.resizeTO) clearTimeout(this.resizeTO);" + vbCrLf)
        htmlOut.Append("this.resizeTO = setTimeout(function() {" + vbCrLf)
        htmlOut.Append("$(this).trigger('resizeEnd');" + vbCrLf)
        htmlOut.Append("}, 800);" + vbCrLf)
        htmlOut.Append("});" + vbCrLf)

        '//redraw graph when window resize is completed  
        htmlOut.Append("$(window).on('resizeEnd', function() {")
        If Not String.IsNullOrEmpty(chartIDDiv) Then
            htmlOut.Append("$('#" + chartIDDiv + "').empty();" + vbCrLf)
        Else
            htmlOut.Append("$('#visualization" + graphID.ToString + "').empty();" + vbCrLf)
        End If
        If Not String.IsNullOrEmpty(functionName) Then
            htmlOut.Append("   " + functionName.ToString + "();" + vbCrLf)
        Else
            htmlOut.Append("   drawVisualization" + graphID.ToString + "();" + vbCrLf)
        End If

        htmlOut.Append("});" + vbCrLf)

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "refreshGraph" & graphID.ToString, htmlOut.ToString, True)
    End Sub



    Public Function get_portfolio_maint_bar_chart_info(ByVal aclist As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            sQuery.Append("SELECT")

            sQuery.Append(" ( CASE ") 'WHEN ac_est_airframe_hrs < 1 THEN '0'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 1 AND ac_est_airframe_hrs < 1000 THEN '1 - 999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 1000 AND ac_est_airframe_hrs < 2000 THEN '1000 - 1999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 2000 AND ac_est_airframe_hrs < 3000 THEN '2000 - 2999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 3000 AND ac_est_airframe_hrs < 4000 THEN '3000 - 3999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 4000 AND ac_est_airframe_hrs < 5000 THEN '4000 - 4999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 5000 AND ac_est_airframe_hrs < 6000 THEN '5000 - 5999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 6000 AND ac_est_airframe_hrs < 7000 THEN '6000 - 6999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 7000 AND ac_est_airframe_hrs < 8000 THEN '7000 - 7999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 8000 AND ac_est_airframe_hrs < 9000 THEN '8000 - 8999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 9000 AND ac_est_airframe_hrs < 10000 THEN '9000 - 9999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 10000 AND ac_est_airframe_hrs < 11000 THEN '11000 - 10999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 11000 AND ac_est_airframe_hrs <= 12000 THEN '11000 - 12000' END ) ")
            'sQuery.Append(" WHEN ac_est_airframe_hrs IS NULL THEN 'NA' END )")

            sQuery.Append("AS AFTT, COUNT(DISTINCT ac_id) AS NUMAIRCRAFT")
            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
            sQuery.Append(aclist)
            sQuery.Append(") AND ac_journ_id = 0 ")
            sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

            sQuery.Append(" GROUP BY")

            sQuery.Append(" ( CASE ") 'WHEN ac_est_airframe_hrs < 1 THEN '0'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 1 AND ac_est_airframe_hrs < 1000 THEN '1 - 999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 1000 AND ac_est_airframe_hrs < 2000 THEN '1000 - 1999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 2000 AND ac_est_airframe_hrs < 3000 THEN '2000 - 2999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 3000 AND ac_est_airframe_hrs < 4000 THEN '3000 - 3999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 4000 AND ac_est_airframe_hrs < 5000 THEN '4000 - 4999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 5000 AND ac_est_airframe_hrs < 6000 THEN '5000 - 5999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 6000 AND ac_est_airframe_hrs < 7000 THEN '6000 - 6999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 7000 AND ac_est_airframe_hrs < 8000 THEN '7000 - 7999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 8000 AND ac_est_airframe_hrs < 9000 THEN '8000 - 8999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 9000 AND ac_est_airframe_hrs < 10000 THEN '9000 - 9999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 10000 AND ac_est_airframe_hrs < 11000 THEN '11000 - 10999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 11000 AND ac_est_airframe_hrs <= 12000 THEN '11000 - 12000' END )")
            '   sQuery.Append(" WHEN ac_est_airframe_hrs IS NULL THEN 'NA' END )")

            sQuery.Append(" ORDER BY CAST( REPLACE(")

            sQuery.Append(" ( CASE ") 'WHEN ac_est_airframe_hrs < 1 THEN '0'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 1 AND ac_est_airframe_hrs < 1000 THEN '1 - 999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 1000 AND ac_est_airframe_hrs < 2000 THEN '1000 - 1999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 2000 AND ac_est_airframe_hrs < 3000 THEN '2000 - 2999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 3000 AND ac_est_airframe_hrs < 4000 THEN '3000 - 3999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 4000 AND ac_est_airframe_hrs < 5000 THEN '4000 - 4999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 5000 AND ac_est_airframe_hrs < 6000 THEN '5000 - 5999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 6000 AND ac_est_airframe_hrs < 7000 THEN '6000 - 6999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 7000 AND ac_est_airframe_hrs < 8000 THEN '7000 - 7999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 8000 AND ac_est_airframe_hrs < 9000 THEN '8000 - 8999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 9000 AND ac_est_airframe_hrs < 10000 THEN '9000 - 9999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 10000 AND ac_est_airframe_hrs < 11000 THEN '11000 - 10999'")
            sQuery.Append(" WHEN ac_est_airframe_hrs >= 11000 AND ac_est_airframe_hrs <= 12000 THEN '11000 - 12000' END )")
            ' sQuery.Append(" WHEN ac_est_airframe_hrs IS NULL THEN 'NA' END )")

            sQuery.Append(", ' - ', '') AS FLOAT) ASC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_portfolio_maint_bar_chart_info(ByVal aclist As String) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_portfolio_maint_bar_chart_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_portfolio_maint_bar_chart_info(ByVal aclist As String) As DataTable " + ex.Message

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

    Public Function get_portfolio_pie_chart_maint_info_1(ByVal aclist As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT")
            sQuery.Append(" ( CASE WHEN amp_program_name = 'Confirmed not on any maintenance program' THEN 'Not on Program'")
            sQuery.Append(" WHEN amp_program_name = 'Confirmed to be on a maintenance program' THEN 'On Program'")
            sQuery.Append(" WHEN amp_program_name = 'Confirmed to be on a Factory maintenance program' THEN 'Factory Program'")
            sQuery.Append(" WHEN amp_program_name = 'JSSI Airframe Program' THEN 'JSSI'")
            sQuery.Append(" WHEN amp_program_name = 'Equalized Maintenance Program' THEN 'Equalized Maintenance'")
            sQuery.Append(" WHEN amp_program_name = 'Unknown' THEN 'Not Reported'")
            sQuery.Append(" WHEN left(amp_program_name,3) = 'MPP' THEN 'MPP'")

            sQuery.Append(" ELSE amp_program_name END ) AS AMPPROGRAM, COUNT(DISTINCT ac_id) AS NUMAIRCRAFT")

            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
            sQuery.Append(aclist)
            sQuery.Append(") AND ac_journ_id = 0 ")
            sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

            sQuery.Append(" GROUP BY amp_program_name")
            sQuery.Append(" ORDER BY AMPPROGRAM")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)


            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_portfolio_pie_chart_maint_info_1 load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_portfolio_pie_chart_maint_info_1(ByVal aclist As String) As DataTable " + ex.Message

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

    Public Function get_portfolio_pie_chart_maint_info_2(ByVal aclist As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT")
            sQuery.Append(" ( CASE WHEN left(amtp_program_name,4) = 'CAMP' THEN 'CAMP'")
            sQuery.Append(" WHEN left(amtp_program_name,4) = 'CAMS' THEN 'CAMS'")
            sQuery.Append(" WHEN left(amtp_program_name,5) = 'G-CMP' THEN 'G-CMP'")
            sQuery.Append(" WHEN left(amtp_program_name,5) = 'MTrax' THEN 'MTrax'")
            sQuery.Append(" WHEN left(amtp_program_name,5) = 'CIMMS' THEN 'CIMMS'")
            sQuery.Append(" WHEN left(amtp_program_name,6) = 'CESCOM' THEN 'CESCOM'")
            sQuery.Append(" WHEN amtp_program_name = 'Flightdocs Maintenance Tracker' THEN 'Flightdocs'")
            sQuery.Append(" WHEN amtp_program_name = 'AvTrak GlobalNet' THEN 'AvTrak'")
            sQuery.Append(" WHEN amtp_program_name = 'Confirmed to be on a maintenance tracking program' THEN 'Confirmed'")
            sQuery.Append(" WHEN amtp_program_name = 'Confirmed not on a maintenance tracking program' THEN 'Not On'")
            sQuery.Append(" WHEN amtp_program_name = 'Unknown' then 'Not Reported'")
            sQuery.Append(" ELSE amtp_program_name END ) AS AMTPPROGRAM, COUNT(DISTINCT ac_id) AS NUMAIRCRAFT")

            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
            sQuery.Append(aclist)
            sQuery.Append(") AND ac_journ_id = 0 AND ac_lifecycle_stage = 3 ")
            sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))


            sQuery.Append(" GROUP BY amtp_program_name")
            sQuery.Append(" ORDER BY AMTPPROGRAM")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_portfolio_pie_chart_maint_info_2(ByVal aclist As String) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_portfolio_pie_chart_maint_info_2 load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_portfolio_pie_chart_maint_info_2(ByVal aclist As String) As DataTable " + ex.Message

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
    Public Function get_portfolio_pie_chart_maint_info_3(ByVal aclist As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT")
            sQuery.Append(" emp_program_name, COUNT(DISTINCT ac_id) AS NUMAIRCRAFT")

            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
            sQuery.Append(aclist)
            sQuery.Append(") AND ac_journ_id = 0 AND ac_lifecycle_stage = 3 ")
            sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))


            sQuery.Append(" GROUP BY emp_program_name")
            sQuery.Append(" ORDER BY emp_program_name asc ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_portfolio_pie_chart_maint_info_3(ByVal aclist As String) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_portfolio_pie_chart_maint_info_3 load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_portfolio_pie_chart_maint_info_3(ByVal aclist As String) As DataTable " + ex.Message

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
    Public Sub display_dropdown_bar_chart(ByVal results_table As DataTable, ByVal graphID As Integer, ByVal searchUpdate As UpdatePanel, ByVal summarize_field As String)

        Dim htmlOut As New StringBuilder

        Dim x As Integer = 0

        Dim temp_field As String = ""
        Dim acCount As Long = 0
        Dim rowCount As Long = 0
        Try


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
                    htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
                    htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
                    htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                    htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
                    htmlOut.Append("data.addColumn('string', 'Label');" + vbCrLf)
                    htmlOut.Append("data.addColumn('number', 'Number of Aircraft');" + vbCrLf)

                    htmlOut.Append("data.addRows(" + IIf(results_table.Rows.Count >= 50, "50", results_table.Rows.Count.ToString) + ");" + vbCrLf)


                    For Each r As DataRow In results_table.Rows
                        If x < 50 Then
                            If Not IsDBNull(r.Item("Summarized")) Then

                                If Not String.IsNullOrEmpty(r.Item("Summarized").ToString.Trim) Then
                                    If Not (r.Item("SUMMARIZED").ToString.ToLower) = "unknown" Then
                                        temp_field = r.Item("Summarized").ToString.Trim


                                        If Not IsDBNull(r.Item("tcount")) Then

                                            If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                                                If CLng(r.Item("tcount").ToString.Trim) > 0 Then
                                                    acCount = CLng(r.Item("tcount").ToString.Trim)

                                                End If
                                            End If
                                        End If


                                        If acCount > 0 Then
                                            htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + temp_field.Trim + "');" + vbCrLf)
                                            htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + acCount.ToString + ");" + vbCrLf)
                                        Else
                                            htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + temp_field.Trim + "');" + vbCrLf)
                                            htmlOut.Append("data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                                        End If

                                        x += 1

                                    End If
                                End If
                            End If
                            temp_field = ""
                            acCount = 0
                        End If
                    Next

                    htmlOut.Append("var chart = new google.visualization.ColumnChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
                    htmlOut.Append("chart.draw(data, {chartArea:{width:'75%',height:'65%'}, title:'', slantedText:'true',legend:  'none', tooltipFontSize:9});" + vbCrLf)

                    htmlOut.Append("};drawVisualization2();" + vbCrLf)
                    PageResize(graphID, searchUpdate, "", "")
                    htmlOut.Append("</script>" + vbCrLf)

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "maintBarChart", htmlOut.ToString, False)



                End If
            End If

        Catch ex As Exception

            aError = "Error in display_portfolio_maint_bar_chart(ByVal aclist As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try


        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub display_portfolio_maint_bar_chart(ByVal aclist As String, ByVal graphID As Integer, ByVal searchUpdate As UpdatePanel)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim x As Integer = 0

        Dim aftt As String = ""
        Dim acCount As Long = 0
        Dim rowCount As Long = 0
        Try

            results_table = get_portfolio_maint_bar_chart_info(aclist)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
                    htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
                    htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
                    htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                    htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
                    htmlOut.Append("data.addColumn('string', 'Label');" + vbCrLf)
                    htmlOut.Append("data.addColumn('number', 'Number of Aircraft');" + vbCrLf)

                    htmlOut.Append("data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("AFTT")) Then

                            If Not String.IsNullOrEmpty(r.Item("AFTT").ToString.Trim) Then

                                aftt = r.Item("AFTT").ToString.Trim


                                If Not IsDBNull(r.Item("NUMAIRCRAFT")) Then

                                    If Not String.IsNullOrEmpty(r.Item("NUMAIRCRAFT").ToString.Trim) Then

                                        If CLng(r.Item("NUMAIRCRAFT").ToString.Trim) > 0 Then
                                            acCount = CLng(r.Item("NUMAIRCRAFT").ToString.Trim)
                                        End If


                                    End If
                                End If
                            End If
                        End If

                        If acCount > 0 Then
                            htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + aftt.Trim + "');" + vbCrLf)
                            htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + acCount.ToString + ");" + vbCrLf)
                        Else
                            htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + aftt.Trim + "');" + vbCrLf)
                            htmlOut.Append("data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                        End If

                        x += 1

                        aftt = ""
                        acCount = 0

                    Next

                    htmlOut.Append("var chart = new google.visualization.ColumnChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
                    htmlOut.Append("chart.draw(data, {chartArea:{width:'75%',height:'65%'},colors: ['#078fd7'], title:'', slantedText:'true',legend:  'none', tooltipFontSize:9});" + vbCrLf)

                    htmlOut.Append("};drawVisualization2();" + vbCrLf)
                    PageResize(graphID, searchUpdate, "", "")
                    htmlOut.Append("</script>" + vbCrLf)

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "maintBarChart", htmlOut.ToString, False)



                End If
            End If

        Catch ex As Exception

            aError = "Error in display_portfolio_maint_bar_chart(ByVal aclist As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try


        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub display_portfolio_generic_bar_chart(ByVal aclist As String, ByVal graphID As Integer, ByVal searchUpdate As UpdatePanel, genericMapString As String, Optional ByRef graphWidth As String = "85%", Optional ByVal OnSelection As Boolean = False, Optional ByVal selectedString As String = "", Optional clickSelection As String = "", Optional slantText As Boolean = False, Optional graphHeight As String = "45%")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try


            htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
            'htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
            'htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)

            htmlOut.Append("google.charts.setOnLoadCallback(function() {" + vbCrLf)

            If OnSelection Then
                '// The select handler. Call the chart's getSelection() method
                htmlOut.Append("function selectHandler" + graphID.ToString + "() {" + vbCrLf)
                htmlOut.Append("var selectedItem = chart" + graphID.ToString + ".getSelection()[0];" + vbCrLf)
                htmlOut.Append("if (selectedItem) {" + vbCrLf)
                htmlOut.Append(selectedString)
                htmlOut.Append("}" + vbCrLf)
                htmlOut.Append("};" + vbCrLf)
            End If

            htmlOut.Append("var chart" + graphID.ToString + " = new google.visualization.ColumnChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
            htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)

            htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)

            htmlOut.Append(genericMapString)

            If OnSelection Then
                '// Listen for the 'select' event, and call my function selectHandler() when
                '// the user selects something on the chart.
                htmlOut.Append("google.visualization.events.addListener(chart" + graphID.ToString + ", 'select', selectHandler" + graphID.ToString + ");" + vbCrLf)


                'htmlOut.Append("google.visualization.events.addListener(chart" + graphID.ToString + ", 'click', function(e) {" + vbCrLf)
                'htmlOut.Append("selection = e.targetID.split('#');" + vbCrLf)
                'htmlOut.Append(" if(selection[0].indexOf('hAxis') > -1) {" + vbCrLf)
                'htmlOut.Append(clickSelection + vbCrLf)
                'htmlOut.Append("}" + vbCrLf)
                'htmlOut.Append("});" + vbCrLf)
            End If

            htmlOut.Append("chart" + graphID.ToString + ".draw(data, {chartArea:  {width:'" & graphWidth & "',height:'" & graphHeight & "'},colors: ['#078fd7'], title:'', hAxis: { " & IIf(slantText, "slantedText: true, slantedTextAngle: 90,", "") & " textStyle : {fontSize: 9}} ,vAxis: { textStyle : {fontSize: 9}} , legend:  'none', tooltipFontSize:10});" + vbCrLf)

            htmlOut.Append("};drawVisualization" & graphID.ToString & "();" + vbCrLf)

            htmlOut.Append("});")
            PageResize(graphID, searchUpdate, "", "")


            htmlOut.Append("</script>" + vbCrLf)

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "genericBarChart-" & graphID.ToString, htmlOut.ToString, False)



        Catch ex As Exception

            aError = "Error in display_portfolio_generic_bar_chart(ByVal aclist As String, ByVal graphID As Integer, ByVal searchUpdate As UpdatePanel, genericMapString As String) " + ex.Message

        Finally

        End Try


        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub display_portfolio_pie_chart_us_international(ByVal aclist As String, ByVal graphID As Integer, ByVal searchUpdate As UpdatePanel, usCount As Double, internationalCount As Double)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim x As Integer = 0

        Dim ampName As String = ""
        Dim acCount As Long = 0

        Try



            htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
            htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
            htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
            htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
            ' htmlOut.Append("var data = new google.visualization.DataTable([['LABEL', 'VALUE'],['US', " & usCount.ToString & "], ['INTERNATIONAL', " & internationalCount.ToString & "]]);" + vbCrLf)
            htmlOut.Append("var data = google.visualization.arrayToDataTable([" + vbCrLf)
            htmlOut.Append("['Label', 'Value']," + vbCrLf)
            htmlOut.Append("['US',     " & usCount.ToString & "]," + vbCrLf)
            htmlOut.Append("['INTERNATIONAL',      " & internationalCount.ToString & "]" + vbCrLf)
            htmlOut.Append("]);" + vbCrLf)

            htmlOut.Append("var chart = new google.visualization.PieChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)


            htmlOut.Append("chart.draw(data, {chartArea:{width:'90%',height:'90%'}, colors: ['#078fd7','#dc3912', 'green','blue'],legend:'none', legendFontSize:10 });" + vbCrLf)


            htmlOut.Append("};drawVisualization" + graphID.ToString + "();" + vbCrLf)

            PageResize(graphID, searchUpdate, "", "")

            htmlOut.Append("</script>" + vbCrLf)

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chart_us_reg", htmlOut.ToString, False)



        Catch ex As Exception

            aError = "Error in display_portfolio_pie_chart_maint_info_1(ByVal aclist As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        'out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub display_portfolio_pie_chart_maint_info_1(ByVal aclist As String, ByVal graphID As Integer, ByVal searchUpdate As UpdatePanel)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim x As Integer = 0

        Dim ampName As String = ""
        Dim acCount As Long = 0

        Try

            results_table = get_portfolio_pie_chart_maint_info_1(aclist)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
                    htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
                    htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
                    htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                    htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
                    htmlOut.Append("data.addColumn('string', 'Label');" + vbCrLf)
                    htmlOut.Append("data.addColumn('number', 'Value');" + vbCrLf)
                    htmlOut.Append("data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("AMPPROGRAM")) Then

                            If Not String.IsNullOrEmpty(r.Item("AMPPROGRAM").ToString.Trim) Then

                                ampName = r.Item("AMPPROGRAM").ToString.Trim

                            End If
                        Else
                            ampName = "NA"
                        End If

                        If Not IsDBNull(r.Item("NUMAIRCRAFT")) Then

                            If Not String.IsNullOrEmpty(r.Item("NUMAIRCRAFT").ToString.Trim) Then

                                If CLng(r.Item("NUMAIRCRAFT").ToString.Trim) > 0 Then
                                    acCount = CLng(r.Item("NUMAIRCRAFT").ToString.Trim)
                                End If

                            End If

                        End If

                        If acCount > 0 Then
                            htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + ampName.Trim + "');" + vbCrLf)
                            htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + acCount.ToString + ");" + vbCrLf)
                        Else
                            htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + ampName.Trim + "');" + vbCrLf)
                            htmlOut.Append("data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                        End If

                        x += 1

                        ampName = ""
                        acCount = 0

                    Next

                    htmlOut.Append("var chart = new google.visualization.PieChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)

                    If results_table.Rows.Count > 35 Then  ' 1/720 slice visibility threshold
                        htmlOut.Append("chart.draw(data, {chartArea:{width:'85%',height:'65%'}, sliceVisibilityThreshold:'0', pieResidueSliceLabel:'Other', legend:'left', legendFontSize:12 });" + vbCrLf)
                    Else
                        htmlOut.Append("chart.draw(data, {chartArea:{width:'85%',height:'65%'}, legend:'left', legendFontSize:12 });" + vbCrLf)
                    End If

                    htmlOut.Append("};drawVisualization3();" + vbCrLf)

                    PageResize(graphID, searchUpdate, "", "")

                    htmlOut.Append("</script>" + vbCrLf)

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chart_maint_info1", htmlOut.ToString, False)

                End If

            End If


        Catch ex As Exception

            aError = "Error in display_portfolio_pie_chart_maint_info_1(ByVal aclist As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        'out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub display_portfolio_pie_chart_maint_info_2(ByVal aclist As String, ByVal graphID As Integer, ByVal searchUpdate As UpdatePanel)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim x As Integer = 0

        Dim trkName As String = ""
        Dim acCount As Long = 0

        Try

            'changed MSW per request 2/4/2020
            results_table = get_portfolio_pie_chart_maint_info_3(aclist)
            ' results_table = get_portfolio_pie_chart_maint_info_2(aclist)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
                    htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
                    htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
                    htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                    htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
                    htmlOut.Append("data.addColumn('string', 'Label');" + vbCrLf)
                    htmlOut.Append("data.addColumn('number', 'Value');" + vbCrLf)
                    htmlOut.Append("data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)

                    For Each r As DataRow In results_table.Rows

                        'If Not IsDBNull(r.Item("AMTPPROGRAM")) Then

                        '    If Not String.IsNullOrEmpty(r.Item("AMTPPROGRAM").ToString.Trim) Then

                        '        trkName = r.Item("AMTPPROGRAM").ToString.Trim

                        '    End If
                        'Else
                        '    trkName = "NA"
                        'End If 

                        If Not IsDBNull(r.Item("emp_program_name")) Then

                            If Not String.IsNullOrEmpty(r.Item("emp_program_name").ToString.Trim) Then

                                trkName = r.Item("emp_program_name").ToString.Trim

                            End If
                        Else
                            trkName = "NA"
                        End If




                        If Not IsDBNull(r.Item("NUMAIRCRAFT")) Then

                            If Not String.IsNullOrEmpty(r.Item("NUMAIRCRAFT").ToString.Trim) Then

                                If CLng(r.Item("NUMAIRCRAFT").ToString.Trim) > 0 Then
                                    acCount = CLng(r.Item("NUMAIRCRAFT").ToString.Trim)
                                End If

                            End If

                        End If

                        If acCount > 0 Then
                            htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + trkName.Trim + "');" + vbCrLf)
                            htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + acCount.ToString + ");" + vbCrLf)
                        Else
                            htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + trkName.Trim + "');" + vbCrLf)
                            htmlOut.Append("data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                        End If

                        x += 1

                        trkName = ""
                        acCount = 0

                    Next

                    htmlOut.Append("var chart = new google.visualization.PieChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)

                    If results_table.Rows.Count > 35 Then  ' 1/720 slice visibility threshold
                        htmlOut.Append("chart.draw(data, {chartArea:{width:'85%',height:'65%'}, sliceVisibilityThreshold:'0', pieResidueSliceLabel:'Other', legend:'left', legendFontSize:12 });" + vbCrLf)
                    Else
                        htmlOut.Append("chart.draw(data, {chartArea:{width:'85%',height:'65%'}, legend:'left', legendFontSize:12 });" + vbCrLf)
                    End If

                    htmlOut.Append("};drawVisualization4();" + vbCrLf)
                    PageResize(graphID, searchUpdate, "", "")
                    htmlOut.Append("</script>" + vbCrLf)
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chart_maint_info_2", htmlOut.ToString, False)

                End If

            End If

        Catch ex As Exception

            aError = "Error in display_portfolio_pie_chart_maint_info_2(ByVal aclist As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        'out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub


    Public Function BuildFeaturesGauge(ByVal searchCriteria As viewSelectionCriteriaClass, ByVal featuresGaugePanel As Panel, ByVal aclist As String, ByVal totalAircraft As Long) As String
        Dim GaugeString As String = ""
        Dim results_Table As New DataTable
        results_Table = GetTopFeatures(aclist)


        'gaugeString = ""
        GaugeString &= vbNewLine & "  function initGauge_Features() { "
        Dim counter As Integer = 1
        If Not IsNothing(results_Table) Then
            If results_Table.Rows.Count > 0 Then
                For Each r As DataRow In results_Table.Rows
                    If counter < 9 Then
                        Dim variablePicture As New HtmlGenericControl
                        Dim variableBox As New HtmlGenericControl

                        Dim variableLabel As New Label
                        If Not IsNothing(featuresGaugePanel.FindControl("features" & counter.ToString & "image")) Then
                            variablePicture = featuresGaugePanel.FindControl("features" & counter.ToString & "image")
                        End If
                        If Not IsNothing(featuresGaugePanel.FindControl("Box" & counter.ToString & "")) Then
                            variableBox = featuresGaugePanel.FindControl("Box" & counter.ToString & "")
                        End If
                        If Not IsNothing(featuresGaugePanel.FindControl("featuresText" & counter.ToString & "")) Then
                            variableLabel = featuresGaugePanel.FindControl("featuresText" & counter.ToString & "")
                            If Not IsDBNull(r("kfeat_name")) Then
                                variableLabel.Text = r("kfeat_name")
                            End If
                        End If
                        'kfeat_name, kfeat_area, Aircraft_Key_Feature.afeat_feature_code, count(distinct ac_id) as tcount 

                        GaugeString &= vbNewLine & " var gauge = new RadialGauge({ renderTo:  'features" & counter.ToString & "',"
                        GaugeString &= vbNewLine & " width: 220, height: 220, units: false,"
                        GaugeString &= vbNewLine & " fontTitleSize: ""34"","
                        GaugeString &= vbNewLine & " fontTitle:""Arial"","
                        GaugeString &= vbNewLine & "colorTitle:  '#4f5050',"

                        GaugeString &= vbNewLine & " title: """ & FormatNumber((r("tcount") / totalAircraft) * 100, 0) & "%"", "
                        GaugeString &= vbNewLine & "  startAngle: 90, SweepAngle: 180, valueBox: false, ticksAngle: 180, exactTicks: true, "
                        GaugeString &= vbNewLine & "  minValue: 0,  maxValue: 100,"
                        GaugeString &= vbNewLine & " majorTicks: false, minorTicks: 0,strokeTicks: false,"
                        GaugeString &= vbNewLine & " colorUnits: ""#000000"","
                        GaugeString &= vbNewLine & " fontUnitsSize: ""30"","
                        GaugeString &= vbNewLine & "highlights: false,animation: false,"
                        GaugeString &= vbNewLine & "barWidth: 25,"
                        GaugeString &= vbNewLine & "barProgress: true,"
                        GaugeString &= vbNewLine & "colorBarProgress:  '#078fd7',"
                        GaugeString &= vbNewLine & "needle: false,"
                        GaugeString &= vbNewLine & "colorBar:  '#eee',"
                        GaugeString &= vbNewLine & "colorStrokeTicks: '#fff',"
                        GaugeString &= vbNewLine & "numbersMargin: -18,"
                        GaugeString &= vbNewLine & "  colorPlate: ""rgba(0,0,0,0)""," 'Make background transparent.
                        GaugeString &= vbNewLine & "    borderShadowWidth: 0,"
                        GaugeString &= vbNewLine & "    borders: false,"
                        GaugeString &= vbNewLine & "  value: " & (r("tcount") / totalAircraft) * 100 & ","
                        GaugeString &= vbNewLine & "}).draw();"
                        'GaugeString &= vbNewLine & "  var canvas = document.getElementById(""features" & counter.ToString & """);"
                        'GaugeString &= vbNewLine & "  var img = canvas.toDataURL(""image/png"");"

                        'GaugeString &= vbNewLine & " document.getElementById('" & variablePicture.ClientID & "').innerHTML = '<img src=""' + img + '"">';"


                        variableBox.Visible = True

                        counter += 1
                    End If
                Next
            End If
        End If
        GaugeString &= " };initGauge_Features();"

        Return GaugeString
    End Function
    Public Function BuildSelectedFeatureGauge(ByVal searchCriteria As viewSelectionCriteriaClass, ByVal aclist As String, ByVal totalAircraft As Long, acattID As Integer, selectedFeatureLabel As Label, aircraftCompositionFeatureLabel As Label) As String
        Dim GaugeString As String = ""
        Dim results_Table As New DataTable
        results_Table = GetFeaturesDropdown(aclist, True, acattID)


        GaugeString = ""
        GaugeString &= vbNewLine & "  function initGauge_Features() { "

        If Not IsNothing(results_Table) Then
            If results_Table.Rows.Count > 0 Then

                selectedFeatureLabel.Text = "AIRCRAFT WITH " & results_Table.Rows(0).Item("acatt_name")
                GaugeString &= vbNewLine & " var gauge = new RadialGauge({ renderTo:  'featuresGaugeSelected',"
                GaugeString &= vbNewLine & " width: 260, height: 220, units: false,"
                GaugeString &= vbNewLine & " fontTitleSize: ""34"","
                GaugeString &= vbNewLine & " fontTitle:""Arial"","
                GaugeString &= vbNewLine & "colorTitle:  '#4f5050',"

                GaugeString &= vbNewLine & " title: """ & FormatNumber((results_Table.Rows(0).Item("tcount") / totalAircraft) * 100, 0) & "%"", "
                GaugeString &= vbNewLine & "  startAngle: 90, SweepAngle: 180, valueBox: false, ticksAngle: 180, exactTicks: true, "
                GaugeString &= vbNewLine & "  minValue: 0,  maxValue: 100,"
                GaugeString &= vbNewLine & " majorTicks: false, minorTicks: 0,strokeTicks: false,"
                GaugeString &= vbNewLine & " colorUnits: ""#000000"","
                GaugeString &= vbNewLine & " fontUnitsSize: ""30"","
                GaugeString &= vbNewLine & "highlights: false,animation: false,"
                GaugeString &= vbNewLine & "barWidth: 25,"
                GaugeString &= vbNewLine & "barProgress: true,"
                GaugeString &= vbNewLine & "colorBarProgress:  '#078fd7',"
                GaugeString &= vbNewLine & "needle: false,"
                GaugeString &= vbNewLine & "colorBar:  '#eee',"
                GaugeString &= vbNewLine & "colorStrokeTicks: '#fff',"
                GaugeString &= vbNewLine & "numbersMargin: -18,"
                GaugeString &= vbNewLine & "  colorPlate: ""rgba(0,0,0,0)""," 'Make background transparent.
                GaugeString &= vbNewLine & "    borderShadowWidth: 0,"
                GaugeString &= vbNewLine & "    borders: false,"
                GaugeString &= vbNewLine & "  value: " & (results_Table.Rows(0).Item("tcount") / totalAircraft) * 100 & ","
                GaugeString &= vbNewLine & "}).draw();"


                aircraftCompositionFeatureLabel.Text = "<table class=""formatTable blue""><tr class=""noBorder""><td valign=""middle""><div class=""subHeader remove_padding"">Aircraft with " & results_Table.Rows(0).Item("acatt_name") & "</div></td><td valign=""middle"">" & results_Table.Rows(0).Item("tcount").ToString & "</td></tr><tr class=""noBorder""><td valign=""middle""><div class=""subHeader remove_padding"">Total Aircraft</div></td><td valign=""middle"">" & totalAircraft.ToString & "</td></tr></table>"

            End If
        End If
        GaugeString &= "};initGauge_Features();"

        Return GaugeString
    End Function

#End Region

#Region "INFO_FUNCTIONS"
    Public Function GetOperatorBusinessType(ByVal acList As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Try

            If acList <> "" Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()


                sQuery.Append("select distinct View_Aircraft_Company_Flat.cbus_name, count(distinct View_Aircraft_Company_Flat.comp_id) as tcount ")
                sQuery.Append(" from View_Aircraft_Company_Flat with (NOLOCK)  ")
                sQuery.Append(" where ")

                sQuery.Append(" ac_id IN (")
                sQuery.Append(acList)
                sQuery.Append(")")

                sQuery.Append(" and ( cref_operator_flag IN ('Y', 'O'))  ")

                sQuery.Append(" AND amod_customer_flag = 'Y' and (")


                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
                    sQuery.Append(" ac_product_business_flag = 'Y' ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                    sQuery.Append(" or ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                    sQuery.Append(" ac_product_commercial_flag = 'Y' ")
                End If

                If (HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True) Or (HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True) Then
                    sQuery.Append(" or ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                    sQuery.Append(" ac_product_helicopter_flag = 'Y'")
                End If

                sQuery.Append(")")


                sQuery.Append(" group by  View_Aircraft_Company_Flat.cbus_name ")



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetOperatorBusinessType = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function
    Public Function GetOperatorCertifications(ByVal acList As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Try

            If acList <> "" Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                sQuery.Append(" select distinct case when ccerttype_type is NULL then 'None Specified' else ccerttype_type end as ccerttype_type, ")
                sQuery.Append(" count(distinct View_Aircraft_Company_Flat.comp_id) as tcount")
                sQuery.Append(" from View_Aircraft_Company_Flat with (NOLOCK) ")
                sQuery.Append(" left outer join Company_Certification with (NOLOCK) on ccert_comp_id = View_Aircraft_Company_Flat.comp_id and ")
                sQuery.Append("ccert_journ_id=View_Aircraft_Company_Flat.ac_journ_id")
                sQuery.Append(" left outer join Company_Certification_Type with (NOLOCK) on ccerttype_id = ccert_type_id")
                sQuery.Append(" where ")
                sQuery.Append(" ac_id IN (")
                sQuery.Append(acList)
                sQuery.Append(")")

                sQuery.Append(" and  ( cref_operator_flag IN ('Y', 'O')) ")

                sQuery.Append(" AND amod_customer_flag = 'Y' and (")


                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
                    sQuery.Append(" ac_product_business_flag = 'Y' ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                    sQuery.Append(" or ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                    sQuery.Append(" ac_product_commercial_flag = 'Y' ")
                End If

                If (HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True) Or (HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True) Then
                    sQuery.Append(" or ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                    sQuery.Append(" ac_product_helicopter_flag = 'Y'")
                End If

                sQuery.Append(")")


                sQuery.Append(" group by  ccerttype_type ")



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetOperatorCertifications = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function
    Public Function GetContinents(ByVal acList As String, ByVal operatorFlag As Boolean, ByVal ownerFlag As Boolean) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Try

            If acList <> "" Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                sQuery.Append("SELECT country_continent_name, COUNT(distinct ac_id) AS tcount ")
                sQuery.Append(" FROM View_Aircraft_Company_Flat WITH (NOLOCK) WHERE ac_id IN (")
                sQuery.Append(acList)
                sQuery.Append(") AND ac_journ_id = 0 ")

                If ownerFlag Then
                    sQuery.Append(" AND cref_contact_type IN('00','08','97') ")
                Else
                    sQuery.Append(" and ( ( cref_operator_flag IN ('Y', 'O') ")
                    sQuery.Append(" or cref_contact_type in ('Y','O') ) ) ")
                End If

                sQuery.Append(" and (")


                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
                    sQuery.Append(" ac_product_business_flag = 'Y' ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                    sQuery.Append(" or ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                    sQuery.Append(" ac_product_commercial_flag = 'Y' ")
                End If

                If (HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True) Or (HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True) Then
                    sQuery.Append(" or ")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                    sQuery.Append(" ac_product_helicopter_flag = 'Y'")
                End If

                sQuery.Append(")")

                sQuery.Append(" AND not (country_continent_name IS NULL) ")

                sQuery.Append(" GROUP BY country_continent_name")
                sQuery.Append(" ORDER BY country_continent_name")

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetContinents = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function
    Public Sub display_composition_results(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()

        Dim avgyear As Integer = 0
        Dim avgyearcount As Integer = 0
        Dim allhigh As Integer = 0
        Dim alllow As Integer = 0
        Dim all_aftt_low As Long = 0
        Dim all_aftt_high As Long = 0
        Dim us_reg As Integer = 0
        Dim th_stage As Integer = 0

        Try

            out_htmlString = ""

            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = adminConnectString

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT ac_id, ac_country_of_registration, ac_ownership_type, ac_lifecycle_stage, ac_forsale_flag, ac_exclusive_flag, ac_airframe_tot_Hrs, ")
                sQuery.Append(" ac_lease_flag, ac_asking, ac_asking_price, 0 as sold_price, ac_list_date, ac_mfr_year, DATEDIFF(d,ac_list_date,getdate()) AS daysonmarket")
                sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 ")
                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_composition_results(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    For Each r As DataRow In _dataTable.Rows

                        If Not IsDBNull(r("ac_mfr_year")) Then
                            If IsNumeric(r("ac_mfr_year").ToString) Then

                                If CInt(r("ac_mfr_year").ToString) > 0 Then

                                    If allhigh = 0 Or CInt(r.Item("ac_mfr_year").ToString) > allhigh Then
                                        allhigh = CInt(r.Item("ac_mfr_year").ToString)
                                    End If

                                    If alllow = 0 Or CInt(r.Item("ac_mfr_year").ToString) < alllow Then
                                        alllow = CInt(r.Item("ac_mfr_year").ToString)
                                    End If

                                    avgyear += CInt(r.Item("ac_mfr_year").ToString)
                                    avgyearcount += 1

                                End If
                            End If
                        End If

                        If Not IsDBNull(r("ac_airframe_tot_Hrs")) Then
                            If IsNumeric(r("ac_airframe_tot_Hrs")) Then
                                If CInt(r("ac_airframe_tot_Hrs").ToString) > 0 Then
                                    If CInt(r("ac_airframe_tot_Hrs")) > CInt(all_aftt_high) Then
                                        all_aftt_high = CInt(r("ac_airframe_tot_Hrs"))
                                    End If

                                    If CInt(r("ac_airframe_tot_Hrs")) < CInt(all_aftt_low) Or all_aftt_low = 0 Then
                                        all_aftt_low = CInt(r("ac_airframe_tot_Hrs"))
                                    End If
                                End If
                            End If
                        End If

                        If r("ac_lifecycle_stage") = "3" Then
                            If Not IsDBNull(r("ac_country_of_registration")) Then
                                If Trim(r("ac_country_of_registration")) = "United States" Then
                                    us_reg = us_reg + 1
                                End If
                            End If
                        End If

                        If r.Item("ac_lifecycle_stage").ToString = "3" And (r.Item("ac_ownership_type").ToString.ToUpper = "S" Or r.Item("ac_ownership_type").ToString.ToUpper = "F" Or r.Item("ac_ownership_type").ToString.ToUpper = "W") Then
                            th_stage += 1
                        End If

                    Next

                End If ' _dataTable.Rows.Count > 0 Then

                If avgyear > 0 And avgyearcount > 0 Then
                    avgyear = CLng(avgyear / avgyearcount)
                End If

                htmlOut.Append("<div class=""Box""><table id=""compositionTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue removeBorderSpacing"">")

                htmlOut.Append("<tr class=""noBorder""><td valign=""top"" align=""center"" colspan=""2""><span class=""subHeader"">COMPOSITION</span></td></tr>")

                htmlOut.Append("<tr><td valign=""top"" align=""left"" nowrap=""nowrap"">MFR Year Range:&nbsp;</td><td align=""right"">" + alllow.ToString + " - " + allhigh.ToString + "</td></tr>")

                htmlOut.Append("<tr><td valign=""top"" align=""left"">AFTT Range:&nbsp;</td><td align=""right"">" + FormatNumber(all_aftt_low, 0).ToString + " - " + FormatNumber(all_aftt_high, 0).ToString + "</td></tr>")

                htmlOut.Append("<tr><td valign=""top"" align=""left"">US/International:&nbsp;</td><td align=""right"">" + IIf(us_reg > 0, us_reg.ToString + " / " + (th_stage - us_reg).ToString, "") + "</td></tr>")

                htmlOut.Append("</table></div>")


            End If ' Not String.IsNullOrEmpty(aclist)

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_composition_results(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub
    Public Sub display_value_composition_results(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False, Optional ByVal displayEValues As Boolean = False, Optional ByRef value_summary_box As String = "", Optional ByRef display_Only_ForSale As Boolean = False, Optional ByRef us_Reg As Double = 0, Optional ByRef th_stage As Double = 0, Optional chartNumber As Integer = 0, Optional toggleMarketItems As Boolean = False, Optional ByRef acCount As Integer = 0, Optional ToggleFeatureCount As Boolean = False, Optional SelectedFeatureID As Integer = 0)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()

        Dim avgyear As Integer = 0
        Dim avgyearcount As Integer = 0
        Dim allhigh As Integer = 0
        Dim alllow As Integer = 0
        Dim all_aftt_low As Long = 0
        Dim all_aftt_high As Long = 0
        Dim all_aftt_avg As Long = 0
        Dim all_aftt_count As Long = 0

        'asking
        Dim forsaleavghigh As Double = 0
        Dim forsaleavlow As Double = 0
        Dim avg_asking As Double = 0
        Dim avg_asking_count As Long = 0
        'dom
        Dim highDom As Long = 0
        Dim lowDom As Long = 0
        Dim avgDom As Long = 0
        Dim domCount As Long = 0

        Dim landings_high As Long = 0
        Dim landings_low As Long = 0
        Dim landings_avg As Long = 0
        Dim landings_count As Long = 0


        'evalues
        Dim evalues_avg As Double = 0
        Dim evalues_high As Double = 0
        Dim evalues_low As Double = 0

        Dim ac_exclusive_sale As Long = 0
        Dim ac_lease As Long = 0
        Dim ac_for_sale As Long = 0

        Dim forSalePer As Double = 0
        Dim excPer As Double = 0
        Dim leasePer As Double = 0
        'Dim acCount As Double = 0
        Try

            out_htmlString = ""

            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = adminConnectString

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT ac_id, ac_country_of_registration, ac_ownership_type, ac_lifecycle_stage, ac_forsale_flag, ac_exclusive_flag, ac_airframe_tot_Hrs, ")
                sQuery.Append(" ac_lease_flag, ac_asking, ac_asking_price, 0 as sold_price, ac_list_date, ac_mfr_year, DATEDIFF(d,ac_list_date,getdate()) AS daysonmarket, ac_airframe_tot_landings ")


                If ToggleFeatureCount And SelectedFeatureID > 0 Then
                    sQuery.Append(" from Aircraft_Attribute_Index with (NOLOCK)")
                    sQuery.Append(" inner join Aircraft_Attribute with (NOLOCK) on acattind_acatt_id = acatt_id")
                    sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on acattind_ac_id = ac_id And ac_journ_id = 0")
                    sQuery.Append(" left outer join Aircraft_Attribute_Model with (NOLOCK) on amod_id = attmod_amod_id and acattind_acatt_id = attmod_att_id")
                Else
                    sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) ")
                End If

                sQuery.Append(" WHERE ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 ")

                If ToggleFeatureCount Then
                    If SelectedFeatureID > 0 Then
                        sQuery.Append(" and acattind_journ_id = 0")
                        sQuery.Append(" and acattind_status_flag ='Y'")
                        sQuery.Append(" and acatt_id = " & SelectedFeatureID.ToString)
                    End If
                End If

                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                If display_Only_ForSale = True Then
                    sQuery.Append(" and ac_forsale_flag = 'Y' ")
                End If



                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_composition_results(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    For Each r As DataRow In _dataTable.Rows
                        acCount += 1
                        ' added MSw - 3/23/20
                        If Not IsDBNull(r("ac_airframe_tot_landings")) Then
                            If IsNumeric(r("ac_airframe_tot_landings").ToString) Then

                                If CInt(r("ac_airframe_tot_landings").ToString) > 0 Then
                                    If landings_high = 0 Or CInt(r.Item("ac_airframe_tot_landings").ToString) > landings_high Then
                                        landings_high = CInt(r.Item("ac_airframe_tot_landings").ToString)
                                    End If

                                    If landings_low = 0 Or CInt(r.Item("ac_airframe_tot_landings").ToString) < landings_low Then
                                        landings_low = CInt(r.Item("ac_airframe_tot_landings").ToString)
                                    End If

                                    landings_avg += CInt(r.Item("ac_airframe_tot_landings").ToString)
                                    landings_count += 1
                                End If
                            End If
                        End If



                        If Not IsDBNull(r("daysonmarket")) Then
                            If IsNumeric(r("daysonmarket").ToString) Then

                                If CInt(r("daysonmarket").ToString) > 0 Then

                                    If highDom = 0 Or CInt(r.Item("daysonmarket").ToString) > highDom Then
                                        highDom = CInt(r.Item("daysonmarket").ToString)
                                    End If

                                    If lowDom = 0 Or CInt(r.Item("daysonmarket").ToString) < lowDom Then
                                        lowDom = CInt(r.Item("daysonmarket").ToString)
                                    End If

                                    avgDom += CInt(r.Item("daysonmarket").ToString)
                                    domCount += 1


                                End If
                            End If
                        End If

                        If Not IsDBNull(r("ac_mfr_year")) Then
                            If IsNumeric(r("ac_mfr_year").ToString) Then

                                If CInt(r("ac_mfr_year").ToString) > 0 Then

                                    If allhigh = 0 Or CInt(r.Item("ac_mfr_year").ToString) > allhigh Then
                                        allhigh = CInt(r.Item("ac_mfr_year").ToString)
                                    End If

                                    If alllow = 0 Or CInt(r.Item("ac_mfr_year").ToString) < alllow Then
                                        alllow = CInt(r.Item("ac_mfr_year").ToString)
                                    End If

                                    avgyear += CInt(r.Item("ac_mfr_year").ToString)
                                    avgyearcount += 1


                                End If
                            End If
                        End If


                        If r.Item("ac_forsale_flag").ToString.ToUpper = "Y" Then
                            ac_for_sale += 1
                            If Not IsDBNull(r("ac_asking_price")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_asking_price").ToString) Then

                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then

                                        If forsaleavghigh = 0 Or CDbl(r.Item("ac_asking_price").ToString) > forsaleavghigh Then
                                            forsaleavghigh = CDbl(r.Item("ac_asking_price").ToString)
                                        End If

                                        If forsaleavlow = 0 Or (CDbl(r.Item("ac_asking_price").ToString) < forsaleavlow) Then
                                            forsaleavlow = CDbl(r.Item("ac_asking_price").ToString)
                                        End If

                                        avg_asking = avg_asking + CDbl(r.Item("ac_asking_price").ToString)
                                        avg_asking_count = avg_asking_count + 1

                                    End If

                                End If
                            End If
                        End If

                        If Not IsDBNull(r("ac_airframe_tot_Hrs")) Then
                            If IsNumeric(r("ac_airframe_tot_Hrs")) Then
                                If CInt(r("ac_airframe_tot_Hrs").ToString) > 0 Then
                                    If CInt(r("ac_airframe_tot_Hrs")) > CInt(all_aftt_high) Then
                                        all_aftt_high = CInt(r("ac_airframe_tot_Hrs"))
                                    End If

                                    If CInt(r("ac_airframe_tot_Hrs")) < CInt(all_aftt_low) Or all_aftt_low = 0 Then
                                        all_aftt_low = CInt(r("ac_airframe_tot_Hrs"))
                                    End If

                                    all_aftt_avg = all_aftt_avg + CInt(r("ac_airframe_tot_Hrs"))
                                    all_aftt_count = all_aftt_count + 1
                                End If
                            End If
                        End If

                        If r("ac_lifecycle_stage") = "3" Then
                            If Not IsDBNull(r("ac_country_of_registration")) Then
                                If Trim(r("ac_country_of_registration")) = "United States" Then
                                    us_Reg = us_Reg + 1
                                End If
                            End If
                        End If

                        If r.Item("ac_lifecycle_stage").ToString = "3" And (r.Item("ac_ownership_type").ToString.ToUpper = "S" Or r.Item("ac_ownership_type").ToString.ToUpper = "F" Or r.Item("ac_ownership_type").ToString.ToUpper = "W") Then
                            th_stage += 1
                        End If


                        If Not IsDBNull(r("ac_exclusive_flag")) Then
                            If r.Item("ac_exclusive_flag").ToString.ToUpper = "Y" Then
                                ac_exclusive_sale += 1
                            End If
                        End If

                        If Not IsDBNull(r("ac_lease_flag")) Then
                            If r.Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                                ac_lease += 1
                            End If
                        End If

                    Next

                End If ' _dataTable.Rows.Count > 0 Then


                ' added MSw - 3/23/20
                If landings_avg > 0 And landings_count > 0 Then
                    landings_avg = (landings_avg / landings_count)
                End If



                If avgyear > 0 And avgyearcount > 0 Then
                    avgyear = CLng(avgyear / avgyearcount)
                End If

                If (forsaleavlow > 0) Then
                    forsaleavlow = CDbl(forsaleavlow / 1000)
                End If

                If (forsaleavghigh > 0) Then
                    forsaleavghigh = CDbl(forsaleavghigh / 1000)
                End If

                If (domCount > 0) Then
                    avgDom = CDbl(avgDom / domCount)
                End If

                If all_aftt_count > 0 Then
                    all_aftt_avg = CDbl(all_aftt_avg / all_aftt_count)
                End If


                If avg_asking_count > 0 Then
                    avg_asking = CDbl(avg_asking / avg_asking_count)
                    avg_asking = CDbl(avg_asking / 1000)
                End If

                If (ac_for_sale > 0 And th_stage > 0) Then

                    forSalePer = System.Math.Round(CDbl(ac_for_sale / th_stage * 100), 1)
                    excPer = System.Math.Round(CDbl(ac_exclusive_sale / ac_for_sale * 100), 1)
                    leasePer = System.Math.Round(CDbl(ac_lease / th_stage * 100), 1)

                ElseIf th_stage > 0 And ac_lease > 0 Then
                    leasePer = System.Math.Round(CDbl(ac_lease / th_stage * 100), 1)
                End If


                If displayEValues Then
                    'Look up these three variables - evalues_low, evalues_high, evalues_avg
                    Dim CurrentTable As New DataTable
                    Dim utilization_Vfunctions As New utilization_view_functions
                    utilization_Vfunctions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                    utilization_Vfunctions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                    utilization_Vfunctions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                    utilization_Vfunctions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                    utilization_Vfunctions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


                    CurrentTable = valueControl.GetEValuesMaxMinCurrent("", 0, "", "", "", "", "", "", "", "", aclist)
                    If Not IsNothing(CurrentTable) Then
                        If CurrentTable.Rows.Count > 0 Then
                            If Not IsDBNull(CurrentTable.Rows(0).Item("AVGVALUE")) Then
                                evalues_avg = CurrentTable.Rows(0).Item("AVGVALUE")
                            End If
                            If Not IsDBNull(CurrentTable.Rows(0).Item("HIGHVALUE")) Then
                                evalues_high = CurrentTable.Rows(0).Item("HIGHVALUE")
                            End If
                            If Not IsDBNull(CurrentTable.Rows(0).Item("AVGVALUE")) Then
                                evalues_low = CurrentTable.Rows(0).Item("LOWVALUE")
                            End If

                        End If
                    End If

                End If

                ' htmlOut.Append("<div class= ""Box"">")
                htmlOut.Append(DisplayFunctions.BuildViewMarketCompositionBox("", "blue", "$" & FormatNumber(forsaleavlow, 0).ToString & "k", "$" & FormatNumber(avg_asking, 0).ToString & "k", "$" & FormatNumber(forsaleavghigh, 0).ToString & "k", alllow, avgyear, allhigh, lowDom, avgDom, highDom, all_aftt_low, all_aftt_avg, all_aftt_high, displayEValues, evalues_low, evalues_avg, evalues_high, landings_high, landings_low, landings_avg, us_Reg, th_stage, display_Only_ForSale, acCount, chartNumber, toggleMarketItems, ToggleFeatureCount))
                ' htmlOut.Append("</div>")

                'value_summary_box = "<div class=""Box"">"
                value_summary_box += DisplayFunctions.BuildViewMarketSummaryBox("", "blue", th_stage, ac_for_sale, ac_exclusive_sale, ac_lease, forSalePer, excPer, leasePer, us_Reg, th_stage)
                'value_summary_box += "</div>"

            End If ' Not String.IsNullOrEmpty(aclist)

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_composition_results(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub
    Public Sub display_ownership_composition_results(ByVal aclist As String, ByRef w_owner As Integer, ByRef f_owner As Integer, ByRef s_owner As Integer)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()

        Try

            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = adminConnectString

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT ac_id, ac_country_of_registration, ac_ownership_type, ac_lifecycle_stage, ac_forsale_flag, ac_exclusive_flag, ac_airframe_tot_Hrs, ")
                sQuery.Append(" ac_lease_flag, ac_asking, ac_asking_price, 0 as sold_price, ac_list_date, ac_mfr_year, DATEDIFF(d,ac_list_date,getdate()) AS daysonmarket")
                sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 ")
                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_composition_results(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

                If _dataTable.Rows.Count > 0 Then

                    For Each r As DataRow In _dataTable.Rows


                        If Not IsDBNull(r("ac_ownership_type")) Then
                            If r("ac_ownership_type") = "W" Then 'And r("ac_lifecycle_stage") = 3 Then
                                w_owner = w_owner + 1
                            End If
                        End If

                        If Not IsDBNull(r("ac_ownership_type")) Then
                            If r("ac_ownership_type") = "F" Then 'And r("ac_lifecycle_stage") = 3 Then
                                f_owner = f_owner + 1
                            End If
                        End If

                        If Not IsDBNull(r("ac_ownership_type")) Then
                            If r("ac_ownership_type") = "S" Then 'And r("ac_lifecycle_stage") = 3 Then
                                s_owner = s_owner + 1
                            End If
                        End If

                    Next

                End If ' _dataTable.Rows.Count > 0 Then


            End If ' Not String.IsNullOrEmpty(aclist)


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_ownership_composition_results(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message


        End Try

    End Sub
    Public Sub BuildUtilizationSummaryTable(ByVal SummaryTable As DataTable, ByVal labelToDisplay As Label, ByVal folderID As Long, ByVal folderName As String)

        If Not IsNothing(SummaryTable) Then
            If SummaryTable.Rows.Count > 0 Then

                Dim differenceOfDate As Long = DateDiff(DateInterval.Day, CDate(DateAdd(DateInterval.Month, -12, Now())), CDate(Now()))
                Dim tflights As Long = 0
                Dim tflightHrs As Long = 0
                Dim tflightBurn As Long = 0
                Dim avgDayFlight As Long = 0
                Dim avgDayHours As Long = 0
                Dim avgDayFuel As Long = 0

                'If differenceOfDate > 0 Then
                If Not IsDBNull(SummaryTable.Rows(0).Item("tflights")) Then
                    If SummaryTable.Rows(0).Item("tflights") > 0 Then
                        avgDayFlight = SummaryTable.Rows(0).Item("tflights") / differenceOfDate
                        tflights = SummaryTable.Rows(0).Item("tflights")
                    End If
                End If
                If Not IsDBNull(SummaryTable.Rows(0).Item("TotalFlightTimeHrs")) Then
                    If SummaryTable.Rows(0).Item("TotalFlightTimeHrs") > 0 Then
                        avgDayHours = SummaryTable.Rows(0).Item("TotalFlightTimeHrs") / differenceOfDate
                        tflightHrs = SummaryTable.Rows(0).Item("TotalFlightTimeHrs")
                    End If
                End If
                If Not IsDBNull(SummaryTable.Rows(0).Item("TotalFuelBurn")) Then
                    If SummaryTable.Rows(0).Item("TotalFuelBurn") > 0 Then
                        avgDayFuel = SummaryTable.Rows(0).Item("TotalFuelBurn") / differenceOfDate
                        tflightBurn = SummaryTable.Rows(0).Item("TotalFuelBurn")
                    End If
                End If
                'Else
                '  If Not IsDBNull(SummaryTable.Rows(0).Item("tflights")) Then
                '    avgDayFlight = SummaryTable.Rows(0).Item("tflights")
                '  End If
                '  If Not IsDBNull(SummaryTable.Rows(0).Item("TotalFlightTimeHrs")) Then
                '    avgDayHours = SummaryTable.Rows(0).Item("TotalFlightTimeHrs")
                '  End If
                '  If Not IsDBNull(SummaryTable.Rows(0).Item("TotalFuelBurn")) Then
                '    avgDayFuel = SummaryTable.Rows(0).Item("TotalFuelBurn")
                '  End If
                'End If

                labelToDisplay.Text = "<div class=""Box""><table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""formatTable blue utilizationSummaryTable"">"
                labelToDisplay.Text += "<thead><tr><th width=""114""></th><th class=""right""><strong>TOTAL</strong></th><th class=""right""><strong>AVG/MO</strong></th><th class=""right""><strong>AVG/DAY</strong></th></tr></thead>"

                labelToDisplay.Text += "<tbody><tr><td><strong># Flights</strong></td><td class=""right"">" & FormatNumber(tflights, 0).ToString & "</td><td class=""right"">" & FormatNumber((avgDayFlight * 30.5), 0).ToString & "</td><td class=""right"">" & FormatNumber(avgDayFlight, 0).ToString & "</td></tr>"

                labelToDisplay.Text += "<tr><td><strong>Flight Hours</strong></td><td class=""right"">" & FormatNumber(tflightHrs, 0).ToString & "</td><td class=""right"">" & FormatNumber((avgDayHours * 30.5), 0).ToString & "</td><td class=""right"">" & FormatNumber(avgDayHours, 0).ToString & "</td></tr>"

                labelToDisplay.Text += "<tr><td><strong>Est Fuel (GAL)</strong></td><td class=""right"">" & FormatNumber(tflightBurn, 0).ToString & "</td><td class=""right"">" & FormatNumber((avgDayFuel * 30.5), 0).ToString & "</td><td class=""right"">" & FormatNumber(avgDayFuel, 0).ToString & "</td></tr>"

                If (folderID = 11111 And folderName = "") Or (folderID = 1) Then
                    labelToDisplay.Text += "</tbody></table><br /><div class=""clearfix""></div></div>"
                Else
                    labelToDisplay.Text += "</tbody></table><br /><a href=""javascript:void(0);"" onclick=""setFlightActivityView('" & folderID.ToString & "','" & folderName & "');"" class=""float_right"">View Flight Activity Details</a><div class=""clearfix""></div></div>"
                End If


            End If
        End If
    End Sub

    Public Function GetTopFeatures(ByVal acList As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Try

            If acList <> "" Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()

                sQuery.Append(" select distinct kfeat_name, kfeat_area, Aircraft_Key_Feature.afeat_feature_code, count(distinct ac_id) as tcount ")
                sQuery.Append(" from View_Aircraft_Flat with (NOLOCK)  ")
                sQuery.Append(" inner join Aircraft_Key_Feature on ac_id = Aircraft_Key_Feature.afeat_ac_id and ac_journ_id = Aircraft_Key_Feature.afeat_journ_id and afeat_status_flag='Y' ")
                sQuery.Append(" inner join Key_Feature with (NOLOCK) on Aircraft_Key_Feature.afeat_feature_code = kfeat_code and kfeat_code not in ('DAM') and kfeat_area not in ('Maintenance') and kfeat_model_dependent_flag = 'N' ")


                sQuery.Append(" WHERE ac_id IN (")
                sQuery.Append(acList)
                sQuery.Append(") AND ac_journ_id = 0 ")

                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))


                sQuery.Append(" group by kfeat_name, kfeat_area, Aircraft_Key_Feature.afeat_feature_code ")
                sQuery.Append(" order by tcount desc ")




                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetTopFeatures = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function

    Public Function BuildFeaturesChart(ByVal acList As String, ByRef chartVisibility As Boolean, featuresDropdownSelection As Integer, ByRef featuresTitle As String) As String
        Dim TempTable As New DataTable

        Dim MapString As String = ""
        Dim x As Integer = 0

        If featuresDropdownSelection = 0 Then
            featuresTitle = "Summary Level Features"
            TempTable = GetFeaturesBarChart(acList)
        Else
            featuresTitle = "Feature Profile"
            TempTable = GetFeaturesDropdown(acList, True, 0)
        End If

        If Not IsNothing(TempTable) Then
            If TempTable.Rows.Count > 0 Then

                MapString += "data.addColumn('string', 'Feature');" + vbCrLf
                MapString += "data.addColumn('number', 'Total Count');" + vbCrLf
                'MapString += "data.addColumn('number', 'ID');" + vbCrLf
                MapString += "data.addRows(" & TempTable.Rows.Count & ");" + vbCrLf
                For Each r As DataRow In TempTable.Rows
                    If Not IsDBNull(r("acatt_name")) Then
                        If Not String.IsNullOrEmpty(r("acatt_name")) Then

                            MapString += "data.setCell(" + x.ToString + ", 0, '" + r("acatt_name") + "');" + vbCrLf
                            MapString += "data.setCell(" + x.ToString + ", 1, " + r("tcount").ToString + ");" + vbCrLf
                            'MapString += "data.setCell(" + x.ToString + ", 2, " + r("acatt_id").ToString + ");" + vbCrLf
                            x += 1
                        End If
                    End If
                Next
            Else
                chartVisibility = False
            End If
        Else
            chartVisibility = False
        End If

        Return MapString
    End Function
    Public Function GetFeaturesDescription(acattID As Integer) As String
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Dim FeatureDescription As String = ""
        Try

            'Opening Connection
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
            SqlConn.Open()



            sQuery.Append(" select acatt_description from Aircraft_Attribute with (NOLOCK)  where acatt_glossary='Y' and acatt_description <> '' ")
            sQuery.Append(" and acatt_id = " & acattID.ToString)

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                TempTable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
            End Try

            If Not IsNothing(TempTable) Then
                If TempTable.Rows.Count > 0 Then
                    FeatureDescription = TempTable.Rows(0).Item("acatt_description")
                End If
            End If
            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception
            GetFeaturesDescription = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return FeatureDescription
    End Function
    Public Function GetFeaturesDropdown(ByVal acList As String, addCount As Boolean, acattID As Integer) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Try

            If acList <> "" Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()


                sQuery.Append(" select distinct acatt_area, acatt_name,acatt_id")

                If addCount Then
                    sQuery.Append(" , count(distinct ac_id) as tcount ")
                End If

                sQuery.Append(" from Aircraft_Attribute_Index with (NOLOCK)")
                sQuery.Append(" inner join Aircraft_Attribute with (NOLOCK) on acattind_acatt_id = acatt_id")
                sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on acattind_ac_id = ac_id And ac_journ_id = 0")
                sQuery.Append(" left outer join Aircraft_Attribute_Model with (NOLOCK) on amod_id = attmod_amod_id and acattind_acatt_id = attmod_att_id")
                sQuery.Append(" where acattind_journ_id = 0")
                sQuery.Append(" and acattind_status_flag ='Y'")

                If acattID > 0 Then
                    sQuery.Append(" and acatt_id = " & acattID.ToString)
                End If

                sQuery.Append(" and ac_id IN (")
                sQuery.Append(acList)
                sQuery.Append(") AND ac_journ_id = 0 ")


                If acattID = 0 Then
                    sQuery.Append(" group by acatt_area,acatt_name, acatt_id")
                    sQuery.Append(" order by acatt_name ")
                Else
                    sQuery.Append(" group by acatt_area,acatt_name, acatt_id")
                    sQuery.Append(" order by acatt_Area, acatt_name ")
                End If



                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetFeaturesDropdown = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function
    Public Function GetFeaturesBarChart(ByVal acList As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim sQuery As New StringBuilder
        Try

            If acList <> "" Then

                'Opening Connection
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
                SqlConn.Open()


                sQuery.Append(" select distinct acatt_area, acatt_block, acatt_abbrev, acatt_name,acatt_id,count(*) as tcount ")
                sQuery.Append(" from Aircraft_Attribute_Index with (NOLOCK) ")
                sQuery.Append(" inner Join Aircraft_Attribute with (NOLOCK) on acattind_acatt_id = acatt_id ")
                sQuery.Append(" inner Join Aircraft with (NOLOCK) on acattind_ac_id = ac_id And ac_journ_id = 0 ")
                sQuery.Append(" left outer join Aircraft_Attribute_Model with (NOLOCK) on ac_amod_id = attmod_amod_id And acattind_acatt_id = attmod_att_id ")
                sQuery.Append(" where acattind_journ_id = 0 ")
                sQuery.Append(" And acattind_status_flag ='Y' ")
                sQuery.Append(" And acatt_summary_level_flag = 'Y' ")

                sQuery.Append(" and ac_id IN (")
                sQuery.Append(acList)
                sQuery.Append(") AND ac_journ_id = 0 ")

                sQuery.Append(" group by acatt_area, acatt_block, acatt_abbrev, acatt_name, acatt_id ")
                sQuery.Append(" order by acatt_name ")


                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

                Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    TempTable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If




        Catch ex As Exception
            GetFeaturesBarChart = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try
        Return TempTable
    End Function
#End Region





End Class
