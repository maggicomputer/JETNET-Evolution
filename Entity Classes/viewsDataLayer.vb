Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/viewsDataLayer.vb $
'$$Author: Matt $
'$$Date: 6/09/20 8:38p $
'$$Modtime: 6/09/20 8:00p $
'$$Revision: 21 $
'$$Workfile: viewsDataLayer.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class viewsDataLayer

    Private aError As String
    Private clientConnectString As String
    Private adminConnectString As String

    Private starConnectString As String
    Private cloudConnectString As String
    Private serverConnectString As String



    Dim TOTAL_AVAILABLE_TO_COMPARE As Integer = 50
    Dim array_field_0(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_1(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_2(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_3(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_4(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_5(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_6(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_7(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_8(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_9(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_10(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_11(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_12(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_13(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_14(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_15(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_16(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_17(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_18(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_19(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_20(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_21(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_22(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_23(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_24(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_25(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_26(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_27(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_28(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_29(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_30(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_31(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_32(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_33(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_34(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_field_35(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_ac_id(TOTAL_AVAILABLE_TO_COMPARE) As String
    Dim array_jac_id(TOTAL_AVAILABLE_TO_COMPARE) As String

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

#Region "misc_functions"

    Public Function check_if_picture_exists(ByRef searchCriteria As viewSelectionCriteriaClass) As Boolean

        Dim sQuery = New StringBuilder()
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim bResult As Boolean = False

        Try

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append("SELECT amod_picture_exists_flag FROM aircraft_model WITH(NOLOCK) WHERE amod_id =" + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
                sQuery.Append("SELECT amod_picture_exists_flag FROM aircraft_model WITH(NOLOCK) WHERE amod_id =" + searchCriteria.ViewCriteriaSecondAmodID.ToString)
            ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
                sQuery.Append("SELECT amod_picture_exists_flag FROM aircraft_model WITH(NOLOCK) WHERE amod_id =" + searchCriteria.ViewCriteriaThirdAmodID.ToString)
            ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                sQuery.Append("SELECT amod_picture_exists_flag FROM aircraft_model WITH(NOLOCK) WHERE amod_id =" + searchCriteria.ViewCriteriaMakeAmodID.ToString)
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            Try
                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader()

                If SqlReader.HasRows Then
                    SqlReader.Read()
                    If SqlReader.Item("amod_picture_exists_flag").ToString.ToUpper = "Y" Then
                        bResult = True
                    End If
                End If

            Catch SqlException
                aError = "Error in check_pic_exists ExecuteReader : " & SqlException.Message
            End Try

        Catch ex As Exception

            aError = "Error in check_pic_exists(ByRef searchCriteria As viewSelectionCriteriaClass) As Boolean " + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return bResult

    End Function

    Public Function find_range_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As String
        find_range_by_model = ""

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sql As String = ""

        Try
            'This defaults the search parameter to be SYR if it's empty.
            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportIATA.Trim) Then
                searchCriteria.ViewCriteriaAirportIATA = "SYR"
            End If

            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportICAO.Trim) Then
                searchCriteria.ViewCriteriaAirportICAO = "KSYR"
            End If

            sql = "select amod_range_tanks_full, amod_max_range_miles from aircraft_model where amod_id = " & searchCriteria.ViewCriteriaAmodID

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = sql
            SqlReader = SqlCommand.ExecuteReader()
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            If SqlReader.HasRows Then
                SqlReader.Read()
                searchCriteria.ViewCriteriaAircraftRange = SqlReader("amod_max_range_miles")
                searchCriteria.ViewCriteriaHeliRangeTanksFull = SqlReader("amod_range_tanks_full")
            End If
            SqlReader.Close()

        Catch ex As Exception
            Return Nothing
            Me.class_error = "Error in Search_Airports_ByIATA_orICAO(ByVal field_length As Long, ByVal Search_Query_Param As String) As DataTable: " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

        End Try

    End Function

    Public Sub get_airport_information(ByVal AirportData As DataTable, ByRef string_to_add_to As String, ByRef localCriteria As viewSelectionCriteriaClass, ByRef airport_name_location As String, ByRef orig_lat As Double, ByRef orig_long As Double)
        Dim AirportLocation As String = ""

        If Not IsNothing(AirportData) Then
            If AirportData.Rows.Count > 0 Then

                If Not IsDBNull(AirportData.Rows(0).Item("aport_latitude_decimal")) Then
                    orig_lat = AirportData.Rows(0).Item("aport_latitude_decimal")
                End If

                If Not IsDBNull(AirportData.Rows(0).Item("aport_longitude_decimal")) Then
                    orig_long = AirportData.Rows(0).Item("aport_longitude_decimal")
                End If

                If Not IsDBNull(AirportData.Rows(0).Item("aport_iata_code")) Then
                    localCriteria.ViewCriteriaAirportIATA = AirportData.Rows(0).Item("aport_iata_code").ToString
                End If


                If Not IsDBNull(AirportData.Rows(0).Item("aport_icao_code")) Then
                    localCriteria.ViewCriteriaAirportICAO = AirportData.Rows(0).Item("aport_icao_code").ToString
                End If


                string_to_add_to += "<table cellpadding='2' cellspacing='0' border='0' width='100%' align='left'>"

                If Not IsDBNull(AirportData.Rows(0).Item("aport_name")) Then   ' class='alt_row'
                    '  string_to_add_to += "<tr><td align='left' valign='top'>" + AirportData.Rows(0).Item("aport_name").ToString + "</td></tr>"
                    airport_name_location = AirportData.Rows(0).Item("aport_name").ToString
                    airport_name_location = Replace(airport_name_location, "'", "")

                    localCriteria.ViewCriteriaAirportName = airport_name_location
                End If

                string_to_add_to += "<tr><td align='left' valign='top'>"

                If Not IsDBNull(AirportData.Rows(0).Item("aport_city")) Then
                    string_to_add_to += AirportData.Rows(0).Item("aport_city").ToString + " "
                    localCriteria.ViewCriteriaCity = AirportData.Rows(0).Item("aport_city").ToString
                End If

                If Not IsDBNull(AirportData.Rows(0).Item("aport_state")) Then
                    string_to_add_to += AirportData.Rows(0).Item("aport_state").ToString + ", "
                    localCriteria.ViewCriteriaState = AirportData.Rows(0).Item("aport_state").ToString
                    If Not IsDBNull(AirportData.Rows(0).Item("state_name")) Then
                        localCriteria.ViewCriteriaState = AirportData.Rows(0).Item("state_name").ToString
                    End If



                End If

                If Not IsDBNull(AirportData.Rows(0).Item("aport_country")) Then
                    string_to_add_to += AirportData.Rows(0).Item("aport_country").ToString

                    If InStr(Trim(string_to_add_to), "United States") > 0 Then
                        string_to_add_to = Replace(string_to_add_to, "United States", "U.S.")
                    End If
                    localCriteria.ViewCriteriaCountry = AirportData.Rows(0).Item("aport_country").ToString
                End If

                If Not IsDBNull(AirportData.Rows(0).Item("aport_city")) And Not IsDBNull(AirportData.Rows(0).Item("aport_state")) And Not IsDBNull(AirportData.Rows(0).Item("aport_country")) Then
                    airport_name_location &= " (" & AirportData.Rows(0).Item("aport_city").ToString & ", "
                    airport_name_location &= AirportData.Rows(0).Item("aport_state").ToString & ", "
                    If Trim(AirportData.Rows(0).Item("aport_country")) = "United States" Then
                        airport_name_location &= "U.S." & ") "
                    Else
                        airport_name_location &= AirportData.Rows(0).Item("aport_country").ToString & ") "
                    End If
                ElseIf Not IsDBNull(AirportData.Rows(0).Item("aport_city")) And Not IsDBNull(AirportData.Rows(0).Item("aport_state")) Then
                    airport_name_location &= " (" & AirportData.Rows(0).Item("aport_city").ToString & ", "
                    airport_name_location &= AirportData.Rows(0).Item("aport_state").ToString & ") "
                ElseIf Not IsDBNull(AirportData.Rows(0).Item("aport_country")) And Not IsDBNull(AirportData.Rows(0).Item("aport_state")) Then
                    airport_name_location &= " (" & AirportData.Rows(0).Item("aport_state").ToString & ", "
                    If Trim(AirportData.Rows(0).Item("aport_country")) = "United States" Then
                        airport_name_location &= "U.S." & ") "
                    Else
                        airport_name_location &= AirportData.Rows(0).Item("aport_country").ToString & ") "
                    End If
                ElseIf Not IsDBNull(AirportData.Rows(0).Item("aport_country")) And Not IsDBNull(AirportData.Rows(0).Item("aport_city")) Then
                    airport_name_location &= " (" & AirportData.Rows(0).Item("aport_city").ToString & ", "
                    If Trim(AirportData.Rows(0).Item("aport_country")) = "United States" Then
                        airport_name_location &= "U.S." & ") "
                    Else
                        airport_name_location &= AirportData.Rows(0).Item("aport_country").ToString & ") "
                    End If
                ElseIf Not IsDBNull(AirportData.Rows(0).Item("aport_country")) Then
                    If Trim(AirportData.Rows(0).Item("aport_country")) = "United States" Then
                        airport_name_location &= "U.S." & ") "
                    Else
                        airport_name_location &= AirportData.Rows(0).Item("aport_country").ToString & ") "
                    End If
                ElseIf Not IsDBNull(AirportData.Rows(0).Item("aport_city")) Then
                    airport_name_location &= " (" & AirportData.Rows(0).Item("aport_city").ToString & ") "
                End If


                string_to_add_to += "</td><td align='right'>"
                ' string_to_add_to += "<tr><td align='left' valign='top'><b>Coordinates:</b></td><td align='left' valign='top' nowrap='nowrap'>"

                If Not IsDBNull(AirportData.Rows(0).Item("aport_latitude_decimal")) Then
                    '  string_to_add_to += AirportData.Rows(0).Item("aport_latitude_decimal").ToString
                    localCriteria.ViewCriteriaAirportLatitude = AirportData.Rows(0).Item("aport_latitude_decimal").ToString
                End If

                If Not IsDBNull(AirportData.Rows(0).Item("aport_longitude_decimal")) Then
                    '  string_to_add_to += ", " + AirportData.Rows(0).Item("aport_longitude_decimal").ToString
                    localCriteria.ViewCriteriaAirportLongitude = AirportData.Rows(0).Item("aport_longitude_decimal").ToString
                End If


                ' string_to_add_to += "</td></tr>"

                '    string_to_add_to += "<tr><td align='left' valign='top'>&nbsp;"
                '   string_to_add_to += "</td></tr>"


                '  string_to_add_to += "<tr><td align='left' valign='top'><b>Airport Code:</b>"
                '  string_to_add_to += "</td></tr>"

                'string_to_add_to += "<tr><td align='left' valign='top'>IATA/ICAO:&nbsp;"
                string_to_add_to += localCriteria.ViewCriteriaAirportIATA.ToString & " / " & localCriteria.ViewCriteriaAirportICAO.ToString
                string_to_add_to += "&nbsp;</td></tr>"

                '   string_to_add_to += "<tr><td align='left' valign='top'><b>FAA code:</b></td><td>"
                ' string_to_add_to += localCriteria.viewc.ToString
                '  string_to_add_to += "&nbsp;</td></tr>"


                string_to_add_to += "</table></td></tr><tr><td align='left'>"
            End If
        End If


    End Sub
    Public Function get_range_airports_by_IATA_or_ICAO(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal aport_id As Integer) As DataTable


        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sql As String = ""

        Try
            If aport_id = 0 Then
                'This defaults the search parameter to be SYR if it's empty.
                If String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportIATA.Trim) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportICAO.Trim) Then
                    ' if there are no icao or iata
                    searchCriteria.ViewCriteriaAirportICAO = "KSYR"
                    searchCriteria.ViewCriteriaAirportIATA = "SYR"
                End If

                If String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportIATA.Trim) Then
                    ' if there are no iata 
                End If
            End If


            sql = "SELECT TOP 1 * FROM AIRPORT WITH(NOLOCK)"
            sql += " left outer join STATE WITH(NOLOCK) on state_code = aport_state "
            sql += " WHERE aport_name <> '' AND aport_country <> ''"

            sql += " AND aport_latitude_full <> '' AND aport_longitude_full <> ''"
            sql += " AND aport_max_runway_length IS NOT NULL AND aport_max_runway_length >= " + searchCriteria.ViewCriteriaAircraftFieldLength.ToString

            If aport_id > 0 Then
                sql += " AND aport_id = '" & aport_id & "'"
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportICAO.Trim) Then  ' 4 chars
                sql += " AND aport_icao_code = '" + searchCriteria.ViewCriteriaAirportICAO.Trim + "'"
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportIATA.Trim) Then ' 3 chars
                sql += " AND aport_iata_code = '" + searchCriteria.ViewCriteriaAirportIATA.Trim + "'"
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportName.Trim) Then
                sql += " AND aport_name = '" + searchCriteria.ViewCriteriaAirportIATA.Trim + "'"
            End If

            sql += " ORDER BY aport_name ASC"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = sql
            SqlCommand.CommandTimeout = 60
            SqlCommand.CommandType = CommandType.Text
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

        Catch ex As Exception
            Return Nothing
            Me.class_error = "Error in Search_Airports_ByIATA_orICAO(ByVal field_length As Long, ByVal Search_Query_Param As String) As DataTable: " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

        End Try

        Return temptable

    End Function

    Public Function get_aport_id(ByVal airport_code As String) As DataTable


        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sql As String = ""

        Try

            sql = airport_code

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = sql
            SqlCommand.CommandTimeout = 60
            SqlCommand.CommandType = CommandType.Text
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

        Catch ex As Exception
            Return Nothing
            Me.class_error = "Error in Search_Airports_ByIATA_orICAO(ByVal field_length As Long, ByVal Search_Query_Param As String) As DataTable: " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

        End Try

        Return temptable

    End Function

    Public Function get_airports_by_IATA_or_ICAO_City_Name(ByVal IATA_Code As String, ByVal ICAO_Code As String, ByVal AirportName As String, ByVal AirportCity As String, Optional ByVal aportID As Long = 0) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable

        Try

            'Opening Connection
            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()

            sql = " select * from airport WITH(NOLOCK)"
            sql += " where aport_active_flag='Y'"
            sql += " and (aport_iata_code <> '' "
            sql += " or aport_icao_code <> '') "

            sql += " and aport_latitude_decimal <> 0"

            ''-- CONTROLLED AIRPORTS WILL NOT HAVE A NUMBER IN THEIR IATA CODE
            'sql += " and CHARINDEX('0',aport_iata_code) = 0"
            'sql += " and CHARINDEX('1',aport_iata_code) = 0"
            'sql += " and CHARINDEX('2',aport_iata_code) = 0"
            'sql += " and CHARINDEX('3',aport_iata_code) = 0"
            'sql += " and CHARINDEX('4',aport_iata_code) = 0"
            'sql += " and CHARINDEX('5',aport_iata_code) = 0"
            'sql += " and CHARINDEX('6',aport_iata_code) = 0"
            'sql += " and CHARINDEX('7',aport_iata_code) = 0"
            'sql += " and CHARINDEX('8',aport_iata_code) = 0"
            'sql += " and CHARINDEX('9',aport_iata_code) = 0"


            If aportID > 0 Then
                sql += " and aport_id = @aportID "
            Else
                sql += " and (aport_iata_code like @aport_iata_code "
                sql += " or aport_icao_code like @aport_icao_code "
                sql += " or aport_city like @aport_city"
                sql += " or aport_name like @aport_name)"
            End If

            sql += " ORDER BY aport_name ASC"

            'save to session query debug string.
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

            If aportID > 0 Then
                SqlCommand.Parameters.AddWithValue("aportID", aportID)
            Else
                SqlCommand.Parameters.AddWithValue("aport_iata_code", "%" & IATA_Code & "%")
                SqlCommand.Parameters.AddWithValue("aport_icao_code", "%" & ICAO_Code & "%")

                SqlCommand.Parameters.AddWithValue("aport_city", "%" & AirportCity & "%")
                SqlCommand.Parameters.AddWithValue("aport_name", "%" & AirportName & "%")
            End If

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing


            Return atemptable

        Catch ex As Exception
            Return Nothing
            Me.class_error = "Error in get_airports_by_IATA_or_ICAO_City_Name(ByVal IATA_Code As String, ByVal ICAO_Code As String, ByVal AirportName As String, ByVal AirportCity As String) As DataTable: " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

    End Function
    Public Function Create_Run_Price_History_SPI(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Dim retail_string As String = ""
        Dim selects_guts_asking As String = ""
        Dim selects_guts_sale As String = ""

        Try

            'Opening Connection
            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()

            retail_string &= " (journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) "
            retail_string &= "  AND (journ_subcat_code_part1 = 'WS')"       '-- Whole Sales Only 
            retail_string &= "  AND (journ_internal_trans_flag = 'N')"           '-- No Internals 




            selects_guts_asking &= "  from Aircraft b with (NOLOCK) inner join Journal  with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id"
            selects_guts_asking &= " where ac_asking_price > 0 "
            selects_guts_asking &= " and " & retail_string
            '-- INSERT DATA RANGE HERE
            selects_guts_asking &= " and journ_date >= '" & DateAdd(DateInterval.Month, -searchCriteria.ViewCriteriaTimeSpan, Now()) & "'"
            selects_guts_asking &= " and b.ac_year=a.ac_year and b.ac_amod_id = a.ac_amod_id)"


            selects_guts_sale &= "  from Aircraft b with (NOLOCK) inner join Journal  with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id"
            selects_guts_sale &= " where ac_sale_price > 0 "
            selects_guts_sale &= " and " & retail_string
            '-- INSERT DATA RANGE HERE
            selects_guts_sale &= " and journ_date >= '" & DateAdd(DateInterval.Month, -searchCriteria.ViewCriteriaTimeSpan, Now()) & "'"
            selects_guts_sale &= " and b.ac_year=a.ac_year and b.ac_amod_id = a.ac_amod_id)"


            sql = ""
            sql &= " select distinct amod_make_name, amod_model_name, ac_amod_id, ac_year, "
            sql &= " COUNT(distinct ac_id) as INOP,"
            '-- GET TOTAL SALE TRANSACTIONS
            sql &= " (select count(distinct journ_id) "
            sql &= "  from Aircraft b with (NOLOCK) inner join Journal  with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id"
            sql &= " where " & retail_string

            '-- INSERT DATA RANGE HERE
            sql &= " and journ_date >= '" & DateAdd(DateInterval.Month, -searchCriteria.ViewCriteriaTimeSpan, Now()) & "'"
            sql &= " and b.ac_year=a.ac_year and b.ac_amod_id = a.ac_amod_id) as TOTALRETAILSALES,"
            '-- GET AVERAGE ASKING PRICE

            sql &= " (select (sum(ac_asking_price)/count(distinct journ_id)) "
            sql &= selects_guts_asking
            sql &= " as AVGASKINGPRICE,"

            sql &= " (select (min(ac_asking_price)) "
            sql &= selects_guts_asking
            sql &= " as LOWASKINGPRICE,"

            sql &= " (select (max(ac_asking_price)) "
            sql &= selects_guts_asking
            sql &= " as HIGHASKINGPRICE,"


            '-- GET AVERAGE SALE PRICE
            sql &= " (select (sum(ac_sale_price)/count(distinct journ_id)) "
            sql &= selects_guts_sale
            sql &= " as AVGSALEPRICE,"

            sql &= " (select (min(ac_sale_price)) "
            sql &= selects_guts_sale
            sql &= " as LOWSALEPRICE,"

            sql &= " (select (max(ac_sale_price)) "
            sql &= selects_guts_sale
            sql &= " as HIGHSALEPRICE"


            sql &= " From Aircraft a with (NOLOCK)"
            sql &= " inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id"
            sql &= " where ac_journ_id = 0 "
            sql &= " and ac_lifecycle_stage=3 "
            sql &= " and ac_amod_id = " & searchCriteria.ViewCriteriaAmodID & " "


            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sql &= (Constants.cAndClause + " ((a.ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (a.ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sql &= (Constants.cAndClause + " ((a.ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (a.ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sql &= (Constants.cAndClause + " a.ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sql &= (Constants.cAndClause + " a.ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If
            'End If

            sql &= " group by  amod_make_name, amod_model_name, ac_amod_id, ac_year"
            sql &= " order by amod_make_name, amod_model_name, ac_year"


            'save to session query debug string.
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing


            Return atemptable

        Catch ex As Exception
            Return Nothing
            Me.class_error = "Error in Create_Run_Price_History_SPI() As DataTable: " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

    End Function

    Public Function ListOfActiveAirportsControlled() As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sql As String = ""

        Try

            sql = "select aport_name, aport_iata_code, aport_icao_code, aport_city, aport_state, aport_country, aport_id, "
            sql += " aport_latitude_decimal, aport_longitude_decimal, aport_max_runway_length"
            sql += " from airport WITH(NOLOCK)"
            sql += " where aport_active_flag='Y'"
            sql += " and "
            'Edited 10/27/15 Per instructions below:
            '[9:19:05 AM] Rick Wanner: for example - we would much rather have this be an OR with parens around so the airport would need to have either a iata or icao code
            '[9:19:51 AM] Rick Wanner: we have an airport in california named Camarillo or something like that .... that is one of the busiest in the country that is currently not showing up due to this clause
            sql += "(aport_iata_code <> '' "
            sql += " or aport_icao_code <> '') "

            sql += " and aport_latitude_decimal <> 0"
            '-- CONTROLLED AIRPORTS WILL NOT HAVE A NUMBER IN THEIR IATA CODE
            sql += " and CHARINDEX('0',aport_iata_code) = 0"
            sql += " and CHARINDEX('1',aport_iata_code) = 0"
            sql += " and CHARINDEX('2',aport_iata_code) = 0"
            sql += " and CHARINDEX('3',aport_iata_code) = 0"
            sql += " and CHARINDEX('4',aport_iata_code) = 0"
            sql += " and CHARINDEX('5',aport_iata_code) = 0"
            sql += " and CHARINDEX('6',aport_iata_code) = 0"
            sql += " and CHARINDEX('7',aport_iata_code) = 0"
            sql += " and CHARINDEX('8',aport_iata_code) = 0"
            sql += " and CHARINDEX('9',aport_iata_code) = 0"
            sql += " and aport_max_runway_length > 0 order by aport_name"


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = sql
            SqlCommand.CommandTimeout = 60
            SqlCommand.CommandType = CommandType.Text
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

        Catch ex As Exception
            Return Nothing
            Me.class_error = "Error in ListOfActiveAirportsControlled() As DataTable: " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

        End Try

        Return temptable

    End Function



    Public Sub fill_location_view_type(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long)

        MyDropDownControl.Items.Clear()

        If Not HttpContext.Current.Session.Item("localPreferences").isCommercialOnlyProduct Then
            Select Case (searchCriteria.ViewCriteriaLocationViewType)
                Case Constants.LOCATION_VIEW_BASE
                    MyDropDownControl.Items.Add(New ListItem("Aircraft Base", Constants.LOCATION_VIEW_BASE.ToString))
                    MyDropDownControl.Items.Add(New ListItem("Owners", Constants.LOCATION_VIEW_OWNER.ToString))
                    MyDropDownControl.Items.Add(New ListItem("Operators", Constants.LOCATION_VIEW_OPERATOR.ToString))
                    MyDropDownControl.Items(0).Selected = True ' set to aircraft base
                Case Constants.LOCATION_VIEW_OWNER
                    MyDropDownControl.Items.Add(New ListItem("Aircraft Base", Constants.LOCATION_VIEW_BASE.ToString))
                    MyDropDownControl.Items.Add(New ListItem("Owners", Constants.LOCATION_VIEW_OWNER.ToString))
                    MyDropDownControl.Items.Add(New ListItem("Operators", Constants.LOCATION_VIEW_OPERATOR.ToString))
                    MyDropDownControl.Items(1).Selected = True ' set to owners
                Case Constants.LOCATION_VIEW_OPERATOR
                    MyDropDownControl.Items.Add(New ListItem("Aircraft Base", Constants.LOCATION_VIEW_BASE.ToString))
                    MyDropDownControl.Items.Add(New ListItem("Owners", Constants.LOCATION_VIEW_OWNER.ToString))
                    MyDropDownControl.Items.Add(New ListItem("Operators", Constants.LOCATION_VIEW_OPERATOR.ToString))
                    MyDropDownControl.Items(2).Selected = True ' set to operators
            End Select
        Else
            Select Case (CLng(searchCriteria.ViewCriteriaLocationViewType))
                Case Constants.LOCATION_VIEW_OWNER
                    MyDropDownControl.Items.Add(New ListItem("Owners", Constants.LOCATION_VIEW_OWNER.ToString))
                    MyDropDownControl.Items.Add(New ListItem("Operators", Constants.LOCATION_VIEW_OPERATOR.ToString))
                    MyDropDownControl.Items(0).Selected = True ' set to owners
                Case Constants.LOCATION_VIEW_OPERATOR
                    MyDropDownControl.Items.Add(New ListItem("Owners", Constants.LOCATION_VIEW_OWNER.ToString))
                    MyDropDownControl.Items.Add(New ListItem("Operators", Constants.LOCATION_VIEW_OPERATOR.ToString))
                    MyDropDownControl.Items(1).Selected = True ' set to operators
            End Select
        End If

        maxWidth = (CStr("Aircraft Base").Length * Constants._STARTCHARWIDTH)
        MyDropDownControl.Width = (maxWidth)

    End Sub

    Public Sub views_display_model_pic(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Dim imgDisplayFolder As String = ""

        imgDisplayFolder = HttpContext.Current.Application.Item("crmClientSiteData").ClientFullHostName + HttpContext.Current.Session.Item("ModelPicturesFolderVirtualPath")

        Try

            If check_if_picture_exists(searchCriteria) Then

                If searchCriteria.ViewCriteriaAmodID > -1 Then
                    If searchCriteria.ViewID <> 10 Then
                        htmlOut.Append("<br /><div class=""picture""><img src=""" + imgDisplayFolder.Trim + "/" + searchCriteria.ViewCriteriaAmodID.ToString + ".jpg"" alt=""" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + """  title=""" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + """ height=""205"" width=""300"" border=""1"" style=""height: 205px; width: 300px;""/></div>")
                    Else
                        htmlOut.Append("<div class=""picture_charter""><img src=""" + imgDisplayFolder.Trim + "/" + searchCriteria.ViewCriteriaAmodID.ToString + ".jpg"" alt=""" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + """  title=""" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + """ height=""155"" width=""250"" border=""1"" style=""height: 155px; width: 250px;""/></div>")
                    End If
                ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
                    htmlOut.Append("<br /><div class=""picture""><img src=""" + imgDisplayFolder.Trim + "/" + searchCriteria.ViewCriteriaSecondAmodID.ToString + ".jpg"" alt=""" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaSecondAmodID, False, "") + """  title=""" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaSecondAmodID, False, "") + """ height=""205"" width=""300"" border=""1"" style=""height: 205px; width: 300px;""/></div>")
                ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
                    htmlOut.Append("<br /><div class=""picture""><img src=""" + imgDisplayFolder.Trim + "/" + searchCriteria.ViewCriteriaThirdAmodID.ToString + ".jpg"" alt=""" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaThirdAmodID, False, "") + """  title=""" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaThirdAmodID, False, "") + """ height=""205"" width=""300"" border=""1"" style=""height: 205px; width: 300px;""/></div>")
                End If

            Else
                htmlOut.Append("<br /><div class=""picture""><b>&nbsp;Image&nbsp;or&nbsp;Video&nbsp;not&nbsp;Available&nbsp;</b></div>")
            End If

        Catch ex As Exception

            aError = "Error in views_display_model_pic(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        out_htmlString = htmlOut.ToString()
        htmlOut = Nothing

    End Sub

    Public Function get_comp_id_by_name(ByVal comp_id As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery As String = ""

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery = "Select company.*, "
            sQuery = sQuery & " (select top 1 pnum_number_full from phone_numbers where pnum_comp_id = comp_id and pnum_journ_id = 0 and pnum_contact_id = 0 "
            sQuery = sQuery & " and pnum_type='Office') as comp_phone_office,"
            sQuery = sQuery & " (select top 1 pnum_number_full from phone_numbers where pnum_comp_id = comp_id and pnum_journ_id = 0 and pnum_contact_id = 0 "
            sQuery = sQuery & " and pnum_type='Fax') as comp_phone_fax"
            sQuery = sQuery & " from company "
            sQuery = sQuery & " where comp_journ_id = 0 and comp_id = " & comp_id.ToString & " "

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
                aError = "Error in get_ac_based_on_location load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_ac_based_on_location(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

#End Region

#Region "general_tab_functions"

    Public Function get_my_ac_comparables_transactions(ByRef ac_id As Long, ByVal REPORT_TYPE As String, ByVal note_id As Long, ByVal searchCriteria As viewSelectionCriteriaClass) As DataTable
        Dim atemptable As New DataTable
        Dim atemptable2 As New DataTable
        Dim Query As String = ""

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing

        Dim sSeperator As String = ""
        Dim trans_list As String = ""
        Dim temp_date As String = ""


        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60




            If Trim(REPORT_TYPE) = "TRANS" Then

                Query = Query & " Select clival_clitrans_id FROM client_value_comparables WHERE clival_note_id = " & note_id & " and clival_ac_type = 'C' and clival_clitrans_id > 0 "

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

                SqlCommand.CommandText = Query.ToString
                SqlReader = SqlCommand.ExecuteReader()

                Try
                    atemptable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    aError = "Error in get_my_ac_value_history load datatable" + constrExc.Message
                End Try



                If Not IsNothing(atemptable) Then

                    If atemptable.Rows.Count > 0 Then

                        For Each r As DataRow In atemptable.Rows

                            If Not IsDBNull(r.Item("clival_clitrans_id")) Then
                                If Trim(trans_list) <> "" Then
                                    trans_list = trans_list & ",'" & r.Item("clival_clitrans_id").ToString() & "'"
                                Else
                                    trans_list = trans_list & "'" & r.Item("clival_clitrans_id").ToString() & "'"
                                End If
                            End If


                        Next

                    End If
                End If

                atemptable.Clear()

            End If


            Query = " SELECT clitrans_asking_price as asking_price, clitrans_est_price as take_price, clitrans_sold_price as sold_price, clitrans_date as date_of, clitrans_ser_nbr as ac_details "
            Query = Query & " FROM client_transactions "
            Query = Query & " INNER JOIN client_aircraft_model ON cliamod_id  = clitrans_cliamod_id "

            If Trim(REPORT_TYPE) = "TRANS" Then
                Query = Query & " WHERE clitrans_id in  (" & trans_list & ") "
            Else

                temp_date = DateAdd(DateInterval.Year, -1, Date.Now())
                temp_date = Year(temp_date) & "-" & Month(temp_date) & "-" & Day(temp_date)

                Query = Query & " WHERE clitrans_cliamod_id = " & searchCriteria.ViewCriteriaAmodID & " "
                Query = Query & " and clitrans_date >= '" & temp_date & "' "
            End If


            Query = Query & " order by  clitrans_date asc "

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

            SqlCommand.CommandText = Query.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable2.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_my_ac_value_history load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_my_ac_value_history(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return atemptable2

    End Function

    Public Function get_my_ac_value_make_model(ByRef ac_id As Long, ByVal REPORT_TYPE As String, ByVal note_id As Long, ByVal get_current_date_not_details As Boolean) As DataTable
        Dim atemptable As New DataTable
        Dim Query As String = ""

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing

        Dim sSeperator As String = ""

        Try


            If get_current_date_not_details = True Then
                Query = Query & "  select "
                Query = Query & "  clival_asking_price as asking_price, clival_est_price as take_price, clival_broker_price as broker_price, curdate()  as date_of "
                Query = Query & " from client_value_comparables "
                Query = Query & " inner join client_aircraft_model on clival_cliamod_id  = cliamod_id"
                Query = Query & " where clival_note_id = " & note_id.ToString & " "
                Query = Query & " and clival_type='F' "
                Query = Query & " and clival_ac_type = 'P'   "
                Query = Query & " order by clival_ac_type desc, clival_client_ac_id desc "
            Else
                Query = Query & "  select CONCAT("


                If Trim(REPORT_TYPE) <> "C" Then
                    Query = Query & "cliaircraft_year_mfr, ' '  "
                Else
                    Query = Query & "clival_year_mfr, ' '  "
                End If

                Query = Query & ", cliamod_make_name, ' ', cliamod_model_name "


                If Trim(REPORT_TYPE) <> "C" Then
                    Query = Query & " , ' ', cliaircraft_ser_nbr) as ac_details, cliaircraft_asking_price as asking_price, cliaircraft_est_price as take_price, cliaircraft_broker_price as broker_price "
                Else
                    Query = Query & ", ' ',  clival_ser_nbr) as ac_details, clival_asking_price as asking_price, clival_est_price as take_price, clival_broker_price as broker_price "
                End If

                Query = Query & " from client_value_comparables "

                If Trim(REPORT_TYPE) <> "C" Then
                    Query = Query & " inner join client_aircraft on clival_client_ac_id = cliaircraft_id"
                    Query = Query & " inner join client_aircraft_model on cliamod_id = cliaircraft_cliamod_id "
                Else
                    Query = Query & " inner join client_aircraft_model on clival_cliamod_id  = cliamod_id"
                End If

                Query = Query & " where clival_note_id = " & note_id.ToString & " "
                Query = Query & " and clival_type='F' "
                Query = Query & " order by clival_ac_type desc, clival_client_ac_id desc "
            End If



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = Query.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_my_ac_value_make_model load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_my_ac_value_make_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

    Public Function get_my_ac_value_history_comparables(ByRef ac_id As Long, ByVal REPORT_TYPE As String, ByVal get_details As Boolean, ByVal jetnet_ac_id As Long, ByVal note_id As Long, ByVal completed_or_open As String, Optional ByVal include_snapshot As Boolean = True, Optional ByVal amod_id As Long = 0, Optional ByVal extra_criteria As String = "", Optional ByVal modified_select As String = "N") As DataTable
        Dim atemptable As New DataTable
        Dim Query As String = ""

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing

        Dim sSeperator As String = ""

        Try

            If Trim(LCase(REPORT_TYPE)) = "summary" Then
                Query = Query & " SELECT distinct cliaircraft_asking_price as asking_price, cliaircraft_est_price as take_price, cliaircraft_broker_price as sold_price,  curdate() as date1, cliaircraft_ser_nbr as ac_details "
            ElseIf Trim(LCase(REPORT_TYPE)) = "current" Then

                If Trim(modified_select) = "Y" Then
                    Query = Query & " SELECT distinct 'AC_VALUES' as type_of, 'AC_VALUES' as description, case when cliaircraft_asking_price IS null then 0 else cliaircraft_asking_price end  as asking_price,   case when cliaircraft_est_price IS null then 0 else cliaircraft_est_price end as take_price,   case when cliaircraft_broker_price IS null then 0 else cliaircraft_broker_price end as sale_price,  curdate() as date1, month(curdate()) as month1, year(curdate()) as year1 "
                Else
                    Query = Query & " SELECT distinct cliaircraft_asking_price as asking_price, cliaircraft_est_price as take_price, cliaircraft_broker_price as sold_price,  curdate()  as date_of, CONCAT(cliamod_make_name, ' ', cliamod_model_name, ' ', cliaircraft_ser_nbr) as ac_details "
                End If


            ElseIf Trim(LCase(REPORT_TYPE)) = "trans" Then

                If Trim(modified_select) = "Y" Then
                    Query = Query & " SELECT distinct clitrans_asking_price as asking_price, clitrans_est_price as take_price, clitrans_sold_price as sale_price, 'CLIENTTRANS' as type_of, clitrans_date as date1, month(clitrans_date) as month1, year(clitrans_date) as year1, 0.0 as LOWVALUE, 0.0 as AVGVALUE, 0.0 as HIGHVALUE "
                Else
                    Query = Query & " SELECT distinct clitrans_asking_price as asking_price, clitrans_est_price as take_price, clitrans_sold_price as sold_price, clitrans_date as date_of, CONCAT(clitrans_ser_nbr) as ac_details "
                End If

            ElseIf Trim(LCase(REPORT_TYPE)) = "comparable" Then
                '   Query = Query & " SELECT cliaircraft_asking_price as asking_price, cliaircraft_est_price as take_price, cliaircraft_broker_price as sold_price, cliaircraft_date_purchased  as date_of "
                If Trim(modified_select) = "Y" Then
                    Query = Query & "SELECT distinct clival_asking_price as asking_price, clival_est_price as take_price, clival_broker_price as sale_price, 'CCOMPARE' as type_of, lnote_action_date as date1,  month(lnote_action_date) as month1, year(lnote_action_date) as year1, 0.0 as LOWVALUE, 0.0 as AVGVALUE, 0.0 as HIGHVALUE  "       ' , CONCAT(clival_ser_nbr) as ac_details
                Else
                    Query = Query & "SELECT distinct clival_asking_price as asking_price, clival_est_price as take_price, clival_broker_price as sold_price, lnote_action_date as date_of   "       ' , CONCAT(clival_ser_nbr) as ac_details
                End If

            ElseIf Trim(LCase(REPORT_TYPE)) = "est_value" Then
                If Trim(modified_select) = "Y" Then
                    Query = Query & "SELECT distinct clival_asking_price as asking_price, clival_est_price as take_price, clival_broker_price as sale_price, 'CLIENTESTVAL' as type_of,  clival_entry_date as date1, month(clival_entry_date) as month1, year(clival_entry_date) as year1, 0.0 as LOWVALUE, 0.0 as AVGVALUE, 0.0 as HIGHVALUE   "       ' , CONCAT(clival_ser_nbr) as ac_details
                Else
                    Query = Query & "SELECT distinct clival_asking_price as asking_price, clival_est_price as take_price, clival_broker_price as sold_price, clival_entry_date as date_of   "       ' , CONCAT(clival_ser_nbr) as ac_details
                End If

            ElseIf Trim(LCase(REPORT_TYPE)) = "current_status" Then
                Query = Query & " SELECT distinct cliaircraft_asking_price as asking_price, cliaircraft_est_price as take_price, cliaircraft_broker_price as sold_price,  curdate()  as date_of,  cliaircraft_ser_nbr as ac_details "
            End If


            If get_details Then
                'Query = Query & ", CONCAT("

                'If Trim(REPORT_TYPE) = "CURRENT" Then
                '    Query = Query & "cliaircraft_year_mfr, ' '  "
                'Else
                '    Query = Query & "clival_year_mfr, ' '  "
                'End If

                'Query = Query & ", cliamod_make_name, ' ', cliamod_model_name "


                'If Trim(REPORT_TYPE) = "CURRENT" Then
                '    Query = Query & " , ' ', cliaircraft_ser_nbr) as ac_details  "
                'Else
                '    Query = Query & ", ' ',  clival_ser_nbr) as ac_details "
                'End If


                If Trim(completed_or_open) <> "C" Then
                    If Trim(LCase(REPORT_TYPE)) = "current" Then
                        Query = Query & ", CONCAT('Current Aircraft:', ' <i>', cliaircraft_value_description, '</i>')  as description "
                    ElseIf Trim(LCase(REPORT_TYPE)) = "trans" Then

                        If Trim(modified_select) = "Y" Then
                            Query = Query & ", 'Transaction' as type_of "
                        Else
                            Query = Query & ", clitrans_subject as description "
                        End If

                    ElseIf Trim(LCase(REPORT_TYPE)) = "comparable" Then
                        Query = Query & ",  'Market Snapshot' as description "
                    ElseIf Trim(LCase(REPORT_TYPE)) = "est_value" Then
                        If amod_id > 0 Then
                            Query = Query & ", clival_type as description, clival_jetnet_ac_id as ac_id, clival_aftt_hours, clival_total_landings "
                        Else
                            Query = Query & ", clival_type as description "
                        End If
                    End If
                Else
                    If Trim(LCase(REPORT_TYPE)) = "current" Then
                        Query = Query & ", CONCAT('Current Aircraft:', ' <i>', cliaircraft_value_description, '</i>')  as description "
                    ElseIf Trim(LCase(REPORT_TYPE)) = "trans" Then
                        Query = Query & ", clitrans_subject as description "
                    ElseIf Trim(LCase(REPORT_TYPE)) = "comparable" Then
                        Query = Query & ",  'My Aircraft Value' as description "
                    ElseIf Trim(LCase(REPORT_TYPE)) = "est_value" Then
                        Query = Query & ", clival_type as description "
                    End If
                End If

            End If

            If Trim(modified_select) = "Y" Then
                If ac_id > 0 Or jetnet_ac_id > 0 Then
                    If Trim(LCase(REPORT_TYPE)) = "current" Then
                        '  Query = Query & ", CONCAT('Current Aircraft:', ' <i>', cliaircraft_value_description, '</i>')  as descrip "
                        Query = Query & ", 'Current Aircraft:' as descrip "
                    ElseIf Trim(LCase(REPORT_TYPE)) = "trans" Then
                        Query = Query & ", clitrans_subject as descrip "
                    ElseIf Trim(LCase(REPORT_TYPE)) = "comparable" Then
                        Query = Query & ",  'My Aircraft Value' as descrip "
                    ElseIf Trim(LCase(REPORT_TYPE)) = "est_value" Then
                        Query = Query & ", clival_type as descrip "
                    End If
                End If
            Else

                Query = Query & " , 'CLIENT' as Data_Source, '' as ac_asking "
                Query = Query & " , '" & Trim(LCase(REPORT_TYPE)) & "' as data_type "

                If Trim(LCase(REPORT_TYPE)) = "est_value" Then
                    Query = Query & " , clival_id as clival_id "
                Else
                    Query = Query & " , 0 as clival_id "
                End If

                If Trim(LCase(REPORT_TYPE)) = "est_value" Then
                    Query = Query & " , lnote_id as lnote_id "
                Else
                    Query = Query & " , 0 as lnote_id "
                End If
            End If
            ' for all open 
            If Trim(LCase(REPORT_TYPE)) = "summary" Then
                Query = Query & " FROM  client_aircraft "
                Query = Query & " INNER JOIN client_aircraft_model ON cliaircraft_cliamod_id = cliamod_id "
                If get_details Then
                    Query = Query & " LEFT OUTER JOIN client_value_comparables ON cliaircraft_id = clival_client_ac_id "
                End If
            ElseIf Trim(LCase(REPORT_TYPE)) = "current" Or Trim(LCase(REPORT_TYPE)) = "current_status" Then
                Query = Query & " FROM  client_aircraft "
                Query = Query & " INNER JOIN client_aircraft_model ON cliaircraft_cliamod_id = cliamod_id "
                'If get_details Then
                '    Query = Query & " INNER JOIN client_value_comparables ON cliaircraft_id = clival_client_ac_id "
                ' End If
            ElseIf Trim(LCase(REPORT_TYPE)) = "trans" Then
                Query = Query & " FROM client_transactions "
                '  Query = Query & " INNER JOIN client_aircraft_model ON cliamod_id  = clitrans_cliamod_id "
                If get_details Then
                    Query = Query & " LEFT OUTER JOIN client_aircraft ON cliaircraft_id = clitrans_cliac_id "
                    'Query = Query & " INNER JOIN client_aircraft ON cliaircraft_id = clitrans_cliac_id "
                    'Query = Query & " LEFT OUTER JOIN client_value_comparables ON clival_client_ac_id = cliaircraft_id "
                End If
            ElseIf Trim(LCase(REPORT_TYPE)) = "comparable" Then
                Query = Query & " FROM client_value_comparables "
                If include_snapshot = True Then
                    Query = Query & " INNER JOIN LOCAL_NOTES on lnote_id = clival_note_id and lnote_status in ('V', 'S') "
                Else
                    Query = Query & " INNER JOIN LOCAL_NOTES on lnote_id = clival_note_id and lnote_status in ('V') "
                End If
                '  Query = Query & " INNER JOIN client_aircraft ON clival_client_ac_id = cliaircraft_id "
                '  Query = Query & " INNER JOIN client_aircraft_model ON cliaircraft_cliamod_id = cliamod_id "
            ElseIf Trim(LCase(REPORT_TYPE)) = "est_value" Then
                Query = Query & " FROM client_value_comparables "
                If include_snapshot = True Then
                    Query = Query & " INNER JOIN LOCAL_NOTES on lnote_id = clival_note_id and lnote_status in ('D') "
                Else
                    Query = Query & " INNER JOIN LOCAL_NOTES on lnote_id = clival_note_id and lnote_status in ('D') "
                End If
            End If


            If Trim(LCase(REPORT_TYPE)) = "summary" Then
                Query = Query & " WHERE cliaircraft_id = " & ac_id & " "
            ElseIf Trim(LCase(REPORT_TYPE)) = "current" Or Trim(LCase(REPORT_TYPE)) = "current_status" Then
                Query = Query & " WHERE cliaircraft_id = " & ac_id & " "
            ElseIf Trim(LCase(REPORT_TYPE)) = "trans" Then
                If ac_id > 0 Then
                    Query = Query & " WHERE (clitrans_cliac_id = " & ac_id & " or clitrans_jetnet_ac_id = " & jetnet_ac_id & ") "
                Else
                    Query = Query & " WHERE (clitrans_jetnet_ac_id = " & jetnet_ac_id & ") "
                End If
            ElseIf Trim(LCase(REPORT_TYPE)) = "comparable" Then
                If ac_id = 0 Then
                    If Trim(completed_or_open) <> "C" Then
                        Query = Query & " WHERE (clival_jetnet_ac_id = " & jetnet_ac_id & ") "
                    End If
                Else
                    If Trim(completed_or_open) <> "C" Then
                        Query = Query & " WHERE (clival_client_ac_id =  " & ac_id & " or clival_jetnet_ac_id = " & jetnet_ac_id & ") "
                    Else
                        Query = Query & " WHERE clival_client_ac_id =  " & ac_id & ""  ' if its closed, just get our ac_id
                    End If
                End If


            ElseIf Trim(LCase(REPORT_TYPE)) = "est_value" Then

                If amod_id > 0 Then
                    Query = Query & " WHERE (clival_jetnet_amod_id = " & amod_id & ") "
                ElseIf ac_id = 0 Then
                    If Trim(completed_or_open) <> "C" Then
                        Query = Query & " WHERE (clival_jetnet_ac_id = " & jetnet_ac_id & ") "
                    End If
                Else
                    If Trim(completed_or_open) <> "C" Then
                        Query = Query & " WHERE (clival_client_ac_id =  " & ac_id & " or clival_jetnet_ac_id = " & jetnet_ac_id & ") "
                    Else
                        Query = Query & " WHERE clival_client_ac_id =  " & ac_id & ""  ' if its closed, just get our ac_id
                    End If
                End If

                If Trim(extra_criteria) <> "" Then
                    Query = Query & Trim(extra_criteria)
                End If
            End If


            If Trim(LCase(REPORT_TYPE)) = "current" Then
                ' Query = Query & " AND clival_type = 'F' "
                ' If get_details Then
                '     Query = Query & " AND clival_ac_type = 'P' "
                'End If
            ElseIf Trim(LCase(REPORT_TYPE)) = "trans" Then
                '  Query = Query & " AND clival_type = 'S' "
                '  Query = Query & " AND clival_clitrans_id > 0 "
                '  If get_details Then
                'Query = Query & " AND clival_ac_type = 'C' "
                '  End If
            ElseIf Trim(LCase(REPORT_TYPE)) = "comparable" Then
                '  Query = Query & " AND clival_type = 'S' "



                If Trim(completed_or_open) <> "C" Then
                    If get_details Then
                        '  Query = Query & " AND clival_ac_type = 'C' "
                        '  Query = Query & " and lnote_opportunity_status = 'C' "
                        If include_snapshot = True Then
                            Query = Query & "  and ((lnote_opportunity_status = 'C' and lnote_status = 'V') or lnote_status ='S' )  "
                        Else
                            Query = Query & "  and ((lnote_opportunity_status = 'C' and lnote_status = 'V'))  "
                        End If
                    End If
                Else
                    Query = Query & " and clival_ac_type = 'P' " ' for comparable primary
                End If
            ElseIf Trim(LCase(REPORT_TYPE)) = "est_value" Then
                Query = Query & " and lnote_Status = 'D' "
            End If





            If Trim(LCase(REPORT_TYPE)) = "summary" Then
                Query = Query & " order by cliaircraft_date_purchased asc "
            ElseIf Trim(LCase(REPORT_TYPE)) = "current" Or Trim(LCase(REPORT_TYPE)) = "current_status" Then
                Query = Query & " order by cliaircraft_date_purchased asc "
            ElseIf Trim(LCase(REPORT_TYPE)) = "trans" Then
                Query = Query & " order by  clitrans_date asc "
            ElseIf Trim(LCase(REPORT_TYPE)) = "comparable" Then
                Query = Query & " order by lnote_action_date asc "
            ElseIf Trim(LCase(REPORT_TYPE)) = "est_value" Then
                If amod_id > 0 Then
                    Query = Query & " order by lnote_entry_date asc "
                Else
                    Query = Query & " order by lnote_action_date asc "
                End If
            End If
            '



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = Query.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_my_ac_value_history load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_my_ac_value_history(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

    Public Function get_ac_details_jetnet_record(ByVal ac_id As Long, Optional ByVal modified_select As String = "N") As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If Trim(modified_select) = "Y" Then
                sQuery.Append(" SELECT distinct case when ac_asking_price IS null then 0 else ac_asking_price end  as asking_price, 0 as take_price, 0 as sale_price, GETDATE() as date1,  month(GETDATE()) as month1,  year(GETDATE()) as year1   ")
                ' sQuery.Append(", amod_make_name + ' ' + amod_model_name + ' ' + ac_ser_no  as ac_details ")
                If ac_id > 0 Then
                    sQuery.Append(" , 'Current Record' as descrip ")
                End If
                sQuery.Append(" , 'AC_VALUES' as type_of, 'AC_VALUES' as description, 0 as LOWVALUE , 0 as AVGVALUE,  0 as HIGHVALUE ")
            Else
                sQuery.Append(" SELECT distinct ac_asking_price as asking_price, 0 as take_price, 0 as sold_price, ac_asking,  GETDATE()  as date_of, ")
                sQuery.Append(" amod_make_name + ' ' + amod_model_name + ' ' + ac_ser_no  as ac_details ")
                sQuery.Append(" , 'Current Aircraft:'  as description, 'JETNET' as Data_Source  ")
            End If



            sQuery.Append("  FROM aircraft with (NOLOCK) ")
            sQuery.Append(" INNER JOIN aircraft_model with (NOLOCK) ON ac_amod_id = amod_id ")
            sQuery.Append(" WHERE ac_id = " & ac_id & " and ac_journ_id = 0 ")


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
                aError = "Error in get_ac_details_jetnet_record load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_ac_details_jetnet_record(ac_id) As DataTable" + ex.Message

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

    Public Function get_ac_trans_not_in_client_trans(ByVal ac_id As Long, ByVal ids_to_exclude As String, Optional ByVal modified_select As String = "N") As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()
        Dim AclsData_Temp As New clsData_Manager_SQL

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If Trim(modified_select) = "Y" Then
                sQuery.Append("select  case when ac_asking_price is null then 0.0 else ac_asking_price end as asking_price, 0.0 as take_price ")

                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                    sQuery.Append(AclsData_Temp.add_in_case_for_ac_sale_price("sale_price"))
                Else
                    sQuery.Append(", 0.0 as sale_price")
                End If

                If ac_id > 0 Then
                    sQuery.Append(", journ_subject as descrip ")
                End If

                sQuery.Append(", month(journ_date) as month1, year(journ_date) as year1, journ_date as date1, 'JETNETTRANS' as type_of, 0.0 as LOWVALUE, 0.0 as AVGVALUE, 0.0 as HIGHVALUE ")
            Else
                sQuery.Append("select ac_asking_price  as asking_price, 0.0 as take_price, ac_asking")

                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                    sQuery.Append(AclsData_Temp.add_in_case_for_ac_sale_price("sold_price"))
                Else
                    sQuery.Append(", 0.0 as sold_price")
                End If

                sQuery.Append(", journ_date as date_of, journ_subject as description, 'JETNET' as Data_Source ")
            End If


            sQuery.Append(" from Aircraft with (NOLOCK) ")
            sQuery.Append(" inner join journal with (NOLOCK) on ac_id = journ_ac_id and ac_journ_id = journ_id ")
            sQuery.Append(" WHERE journ_ac_id = " & ac_id & "")
            sQuery.Append(" and journ_subcategory_code  like 'WS%' ")
            sQuery.Append(" and journ_internal_trans_flag='N' ")
            ' sQuery.Append(" and ac_asking='Price' ") 

            sQuery.Append(" and (ac_asking_price > 0 or (ac_sale_price >0 and ac_sale_price_display_flag='Y')) ")

            If Trim(ids_to_exclude) <> "" Then
                sQuery.Append(" and journ_id not in (" & ids_to_exclude & ") ")
            End If


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
                aError = "Error in get_actual_average_days_on_market_info load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_actual_average_days_on_market_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

    Public Sub get_ranges(ByRef low_number As Long, ByRef high_number As Long, ByRef interval_point As Long, ByRef starting_point As Long)

        If high_number - low_number > 100000 And high_number - low_number < 1000000 Then ' one million - count by 250 thousand
            interval_point = 25000
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 1000000 And high_number - low_number < 5000000 Then ' ten million to 50 million count by 5 million
            interval_point = 500000
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 1000000 Then
            interval_point = 100000
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 100000 Then
            interval_point = 10000
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 50000 Then '50k
            interval_point = 10000
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 25000 Then '25k
            interval_point = 5000
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 15000 Then  ' 15k
            interval_point = 2500
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 10000 Then  ' 10k
            interval_point = 1000
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 5000 Then
            interval_point = 500
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 2500 Then
            interval_point = 400
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 1000 Then
            interval_point = 300
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 500 Then
            interval_point = 100
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 200 Then
            interval_point = 50
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 100 Then
            interval_point = 20
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 50 Then
            interval_point = 10
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 10 Then
            interval_point = 5
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        Else
            interval_point = 2
            starting_point = (low_number / interval_point) - 1
            starting_point = starting_point * interval_point
        End If

    End Sub

    Public Function get_journ_ids_from_client_trans(ByVal jetnet_ac_id As Long, ByVal client_ac_id As Long) As String
        get_journ_ids_from_client_trans = ""

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing
        Dim adors As MySql.Data.MySqlClient.MySqlDataReader : adors = Nothing
        Dim Query As String = ""

        Try


            Query = " select distinct clitrans_jetnet_trans_id from client_transactions where (clitrans_cliac_id = " & client_ac_id & " or clitrans_jetnet_ac_id = " & jetnet_ac_id & ")"


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = Query.ToString
            adors = SqlCommand.ExecuteReader


            If adors.HasRows Then

                Do While adors.Read
                    If Not IsDBNull(adors("clitrans_jetnet_trans_id")) Then
                        If Trim(CStr(adors("clitrans_jetnet_trans_id"))) <> "" Then
                            If Trim(get_journ_ids_from_client_trans) <> "" Then
                                get_journ_ids_from_client_trans &= ",'" & Trim(CStr(adors("clitrans_jetnet_trans_id"))) & "'"
                            Else
                                get_journ_ids_from_client_trans &= "'" & Trim(CStr(adors("clitrans_jetnet_trans_id"))) & "'"
                            End If
                        End If
                    End If
                Loop
                adors.Close()

            End If


        Catch ex As Exception
        Finally
            adors = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

        End Try
    End Function

    Public Sub views_analytics_graph_1(ByVal ac_id As Long, ByRef CHART_NAME As DataVisualization.Charting.Chart, ByRef aircraft_history_string As String, ByVal jetnet_ac_id As Long, ByRef google_map_string As String, ByVal COMPLETED_OR_OPEN As String, ByVal note_id As Long, Optional ByRef exists_data As Boolean = False, Optional ByRef estimated_value_label_Text As String = "", Optional ByRef market_checked As Boolean = True, Optional ByRef show_sales_last_months As Integer = 0, Optional ByRef final_datatable As DataTable = Nothing)

        Dim high_number As Integer = 0
        Dim low_number As Integer = 0
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1

        Dim daysonmarket As Integer = 0
        Dim daysonmarket2 As Integer = 0
        Dim days As Integer = 0
        Dim test_string As String = ""

        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim results_table3 As New DataTable
        Dim results_table4 As New DataTable
        Dim results_table5 As New DataTable
        Dim final_table As New DataTable

        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn
        Dim column5 As New DataColumn
        Dim column6 As New DataColumn
        Dim column7 As New DataColumn
        Dim column8 As New DataColumn
        Dim column9 As New DataColumn
        Dim column10 As New DataColumn
        Dim column11 As New DataColumn

        Dim temp_price As Long = 0
        Dim temp_trans_ids As String = ""
        Dim temp_data As String = ""
        Dim temp_take As String = ""
        Dim temp_asking As String = ""
        Dim temp_sold As String = ""
        Dim row_added As Boolean = False
        Dim show_price As Boolean = False
        Dim temp_date_wo_year As String = ""
        Dim color As String = ""
        Dim is_est_value As Boolean = False
        Dim old_date As Date = Now()
        Dim pass_to_show As Boolean = False



        Try

            If Trim(COMPLETED_OR_OPEN) <> "C" Then

                If ac_id = 0 And jetnet_ac_id > 0 Then
                    results_table = get_ac_details_jetnet_record(jetnet_ac_id)
                Else
                    results_table = get_my_ac_value_history_comparables(ac_id, "Current", True, 0, note_id, COMPLETED_OR_OPEN)      ' get current ac primary info
                End If
                results_table2 = get_my_ac_value_history_comparables(ac_id, "Trans", True, jetnet_ac_id, note_id, COMPLETED_OR_OPEN)        ' get transactions for current ac
                results_table3 = get_my_ac_value_history_comparables(ac_id, "Comparable", True, jetnet_ac_id, note_id, COMPLETED_OR_OPEN, market_checked)        ' get other sold ac that were comparables
                results_table5 = get_my_ac_value_history_comparables(ac_id, "est_value", True, jetnet_ac_id, note_id, COMPLETED_OR_OPEN)


                temp_trans_ids = get_journ_ids_from_client_trans(jetnet_ac_id, ac_id)
                results_table4 = get_ac_trans_not_in_client_trans(jetnet_ac_id, temp_trans_ids)
            Else
                results_table = get_my_ac_value_make_model(ac_id, COMPLETED_OR_OPEN, note_id, True)
                results_table2 = get_my_ac_value_history_comparables(ac_id, "Trans", True, jetnet_ac_id, note_id, COMPLETED_OR_OPEN)        ' get transactions for current ac
                results_table3 = get_my_ac_value_history_comparables(ac_id, "Comparable", True, jetnet_ac_id, note_id, COMPLETED_OR_OPEN)        ' get other sold ac that were comparables
                results_table5 = get_my_ac_value_history_comparables(ac_id, "est_value", True, jetnet_ac_id, note_id, COMPLETED_OR_OPEN)

                temp_trans_ids = get_journ_ids_from_client_trans(jetnet_ac_id, ac_id)
                results_table4 = get_ac_trans_not_in_client_trans(jetnet_ac_id, temp_trans_ids)
            End If

            'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
            column.DataType = System.Type.GetType("System.Double")
            column.DefaultValue = 0
            column.Unique = False
            column.ColumnName = "asking_price"
            final_table.Columns.Add(column)

            column2.DataType = System.Type.GetType("System.Double")
            column2.DefaultValue = 0
            column2.Unique = False
            column2.ColumnName = "take_price"
            final_table.Columns.Add(column2)

            column3.DataType = System.Type.GetType("System.Double")
            column3.DefaultValue = 0
            column3.AllowDBNull = True
            column3.Unique = False
            column3.ColumnName = "sold_price"
            final_table.Columns.Add(column3)


            column4.DataType = System.Type.GetType("System.DateTime")
            ' column4.DefaultValue = Date.Now()
            column4.AllowDBNull = True
            column4.Unique = False
            column4.ColumnName = "date_of"
            final_table.Columns.Add(column4)




            'column5.DataType = System.Type.GetType("System.String")
            'column5.DefaultValue = ""
            'column5.Unique = False
            'column5.ColumnName = "ac_details"
            'final_table.Columns.Add(column5)

            column5.DataType = System.Type.GetType("System.String")
            column5.DefaultValue = ""
            column5.Unique = False
            column5.ColumnName = "description"
            final_table.Columns.Add(column5)

            column6.DataType = System.Type.GetType("System.String")
            column6.DefaultValue = ""
            column6.Unique = False
            column6.ColumnName = "ac_details"
            final_table.Columns.Add(column6)

            column7.DataType = System.Type.GetType("System.String")
            column7.DefaultValue = ""
            column7.Unique = False
            column7.ColumnName = "Data_Source"
            final_table.Columns.Add(column7)

            column8.DataType = System.Type.GetType("System.String")
            column8.DefaultValue = ""
            column8.Unique = False
            column8.ColumnName = "ac_asking"
            final_table.Columns.Add(column8)

            column9.DataType = System.Type.GetType("System.String")
            column9.DefaultValue = ""
            column9.Unique = False
            column9.ColumnName = "data_type"
            final_table.Columns.Add(column9)


            column10.DataType = System.Type.GetType("System.String")
            column10.DefaultValue = ""
            column10.Unique = False
            column10.ColumnName = "clival_id"
            final_table.Columns.Add(column10)

            column11.DataType = System.Type.GetType("System.String")
            column11.DefaultValue = ""
            column11.Unique = False
            column11.ColumnName = "lnote_id"
            final_table.Columns.Add(column11)



            For Each drRow As DataRow In results_table.Rows
                final_table.ImportRow(drRow)
            Next

            For Each drRow As DataRow In results_table2.Rows
                final_table.ImportRow(drRow)
            Next

            For Each drRow As DataRow In results_table3.Rows
                final_table.ImportRow(drRow)
            Next

            For Each drRow As DataRow In results_table4.Rows
                final_table.ImportRow(drRow)
            Next


            For Each drRow As DataRow In results_table5.Rows
                final_table.ImportRow(drRow)
            Next



            Dim Filtered_DV As New DataView(final_table)

            For Each drv As DataRowView In Filtered_DV
                Console.WriteLine(vbTab & " {0}", drv("date_of"))
            Next

            Console.WriteLine("<br><br>")


            Filtered_DV.Sort = "date_of asc, description desc"
            final_table = Filtered_DV.ToTable

            For Each drv As DataRowView In Filtered_DV
                Console.WriteLine(vbTab & " {0}", drv("date_of"))
            Next


            ' Dim afiltered_Jetnet As DataRow() = final_table.Select("", "date_of asc")

            If Not IsNothing(CHART_NAME) Then
                CHART_NAME.Series.Clear()
                CHART_NAME.ChartAreas("ChartArea1").AxisY.Title = "Aircraft Value ($k)"

                CHART_NAME.Series.Add("ASKING").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
                CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
                CHART_NAME.Series("ASKING").LabelForeColor = Drawing.Color.Blue
                CHART_NAME.Series("ASKING").Color = Drawing.Color.Blue
                CHART_NAME.Series("ASKING").BorderWidth = 1
                CHART_NAME.Series("ASKING").MarkerSize = 5
                CHART_NAME.Series("ASKING").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
                CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

                CHART_NAME.Series.Add("TAKE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
                CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
                CHART_NAME.Series("TAKE").LabelForeColor = Drawing.Color.Red
                CHART_NAME.Series("TAKE").Color = Drawing.Color.Red
                CHART_NAME.Series("TAKE").BorderWidth = 1
                CHART_NAME.Series("TAKE").MarkerSize = 5
                CHART_NAME.Series("TAKE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
                CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32


                CHART_NAME.Series.Add("SOLD").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
                CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
                CHART_NAME.Series("SOLD").LabelForeColor = Drawing.Color.Green
                CHART_NAME.Series("SOLD").Color = Drawing.Color.Green
                CHART_NAME.Series("SOLD").BorderWidth = 1
                CHART_NAME.Series("SOLD").MarkerSize = 5
                CHART_NAME.Series("SOLD").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
                CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



                CHART_NAME.Width = 300
                CHART_NAME.Height = 300
            End If

            aircraft_history_string = ""
            aircraft_history_string &= "<table border='1' cellpadding='3' cellspacing='0' class='engine'>"
            aircraft_history_string &= "<tr class='dark_blue'><td align='left'><font size='-2' style='font-family: Arial'><b>Date</b></td><td align='left'><font size='-2' style='font-family: Arial'><b>Description</b></td><td align='left' nowrap='nowrap'><font size='-2' style='font-family: Arial'><b>Asking $</b></td><td align='left'><font size='-2' style='font-family: Arial' nowrap='nowrap'><b>Take $</b></td><td align='left' nowrap='nowrap'><font size='-2' style='font-family: Arial'><b>Est/Sold $</b></font></td></tr>"


            '  google_map_string = "['Date', 'Asking', 'Take', 'Sold']"
            ' google_map_string = " data.addRows([ "
            google_map_string = " data1.addColumn('string', 'Serial#'); "
            google_map_string &= " data1.addColumn('number', 'Asking($k)'); "
            google_map_string &= " data1.addColumn('number', 'Take($k)'); "
            google_map_string &= " data1.addColumn('number', 'Est/Sold Value($k)'); "
            google_map_string &= " data1.addColumn('number', 'My AC Asking($k)'); "
            google_map_string &= " data1.addColumn('number', 'My AC Take($k)'); "
            google_map_string &= " data1.addColumn('number', 'My AC Est Value($k)'); "
            google_map_string &= " data1.addRows(["

            If show_sales_last_months > 0 Then
                old_date = DateAdd(DateInterval.Month, -show_sales_last_months, old_date)
            Else
                old_date = DateAdd(DateInterval.Year, -50, old_date)  ' default to show last 50 years  - made 50 years just so i didnt have to changee logic
            End If


            If Not IsNothing(final_table) Then

                If final_table.Rows.Count > 0 Then

                    For Each r As DataRow In final_table.Rows

                        pass_to_show = False
                        If Not IsDBNull(r.Item("date_of")) Then
                            If CDate(r.Item("date_of").ToString) > CDate(old_date) Then
                                pass_to_show = True
                            End If
                        End If


                        If pass_to_show = True Then

                            temp_asking = "null"
                            temp_take = "null"
                            temp_sold = "null"
                            temp_data = ""

                            If color = "alt_row" Then
                                color = ""
                            Else
                                color = "alt_row"
                            End If

                            estimated_value_label_Text = "<table cellpadding='1' cellspacing='0' border='1' width='90%' class=""data_view_grid"">"
                            estimated_value_label_Text &= "<tr class=""header_row"">"
                            estimated_value_label_Text &= "<td>My Aircraft</td>"
                            estimated_value_label_Text &= "<td align='right'>Values</td>"
                            estimated_value_label_Text &= "</tr>"



                            aircraft_history_string &= "<tr class='" & color & "'><td align='left'><font size='-2' style='font-family: Arial'>"

                            If Not IsDBNull(r.Item("data_type")) Then
                                If Trim(r.Item("data_type").ToString) = "est_value" Then
                                    aircraft_history_string &= "<a href='#' onclick=""window.open('edit_note.aspx?action=edit&source=CLIENT&ac_ID=" & ac_id.ToString & "&type=value_analysis&cat_key=17&clival_id=" & r.Item("clival_id") & "&id=" & r.Item("lnote_id") & IIf(note_id > 0, "&NoteID=" & note_id & "&refreshing=view&ViewID=19", "") & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                                End If
                            End If

                            If IsDBNull(r.Item("date_of")) Then
                                If Trim(r.Item("date_of").ToString) = "" Then
                                    temp_date_wo_year = Date.Now.Date
                                    temp_date_wo_year = Month(temp_date_wo_year) & "/" & Day(temp_date_wo_year) & "/" & Right(Trim(Year(temp_date_wo_year)), 2)
                                    aircraft_history_string &= temp_date_wo_year
                                    temp_data = Date.Now.Date
                                Else
                                    temp_date_wo_year = CDate(r.Item("date_of").ToString).Date
                                    temp_date_wo_year = Month(temp_date_wo_year) & "/" & Day(temp_date_wo_year) & "/" & Right(Trim(Year(temp_date_wo_year)), 2)
                                    aircraft_history_string &= temp_date_wo_year
                                    temp_data = CDate(r.Item("date_of").ToString).Date
                                End If
                            Else
                                temp_date_wo_year = CDate(r.Item("date_of").ToString).Date
                                temp_date_wo_year = Month(temp_date_wo_year) & "/" & Day(temp_date_wo_year) & "/" & Right(Trim(Year(temp_date_wo_year)), 2)
                                aircraft_history_string &= temp_date_wo_year
                                temp_data = CDate(r.Item("date_of").ToString).Date
                            End If


                            If Not IsDBNull(r.Item("data_type")) Then
                                If Trim(r.Item("data_type").ToString) = "est_value" Then
                                    aircraft_history_string &= "</a>"
                                End If
                            End If


                            aircraft_history_string &= "&nbsp;</font></td><td align='left'><font size='-2' style='font-family: Arial'>"


                            If Not IsDBNull(r.Item("data_type")) Then
                                If Trim(r.Item("data_type").ToString) = "est_value" Then
                                    aircraft_history_string &= "<a href='#' onclick=""window.open('edit_note.aspx?action=edit&source=CLIENT&ac_ID=" & ac_id.ToString & "&type=value_analysis&cat_key=17&clival_id=" & r.Item("clival_id") & "&id=" & r.Item("lnote_id") & IIf(note_id > 0, "&NoteID=" & note_id & "&refreshing=view&ViewID=19", "") & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                                End If
                            End If

                            is_est_value = False
                            If IsDBNull(r.Item("description")) Then
                                If Trim(r.Item("description").ToString) = "" Then
                                    aircraft_history_string &= ""
                                Else
                                    aircraft_history_string &= r.Item("description").ToString
                                End If
                            ElseIf Len(Trim(r.Item("description").ToString)) = 1 Then
                                is_est_value = True
                                If Trim(r.Item("description").ToString) = "F" Then
                                    aircraft_history_string &= "Full Appraisal"
                                ElseIf Trim(r.Item("description").ToString) = "D" Then
                                    aircraft_history_string &= "Desktop Appraisal"
                                ElseIf Trim(r.Item("description").ToString) = "V" Then
                                    aircraft_history_string &= "VREF"
                                ElseIf Trim(r.Item("description").ToString) = "B" Then
                                    aircraft_history_string &= "Blue Book"
                                ElseIf Trim(r.Item("description").ToString) = "H" Then
                                    aircraft_history_string &= "HeliValue$"
                                End If
                            Else
                                aircraft_history_string &= r.Item("description").ToString
                            End If



                            If Not IsDBNull(r.Item("data_type")) Then
                                If Trim(r.Item("data_type").ToString) = "est_value" Then
                                    aircraft_history_string &= "</a>"
                                End If
                            End If

                            aircraft_history_string &= "&nbsp;</font></td><td align='right'><font size='-2' style='font-family: Arial'>"


                            If Not IsDBNull(r("asking_price")) Then
                                If CDbl(r.Item("asking_price").ToString) > 0 Then


                                    show_price = False
                                    If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                        show_price = True
                                    ElseIf Trim(LCase(r("Data_Source"))) = "client" Then
                                        show_price = True
                                    ElseIf Not IsDBNull(r("ac_asking")) Then
                                        If Trim(r("ac_asking")) = "Price" Then
                                            show_price = True
                                        End If
                                    End If

                                    If show_price = True Then
                                        temp_price = CDbl(r.Item("asking_price").ToString)

                                        estimated_value_label_Text &= "<tr>"
                                        estimated_value_label_Text &= "<td class='alt_color'>Current Asking Price</td><td align='right'>"

                                        estimated_value_label_Text &= "$" & FormatNumber(temp_price, 0)

                                        estimated_value_label_Text &= "&nbsp;</td></tr> "

                                        HttpContext.Current.Session.Item("Current_Asking") = FormatNumber(temp_price, 0)


                                        temp_price = (temp_price / 1000)
                                        temp_asking = temp_price

                                        If high_number = 0 Or CDbl(temp_price) > high_number Then
                                            high_number = CDbl(temp_price)
                                        End If

                                        If low_number = 0 Or CDbl(temp_price) < low_number Then
                                            low_number = CDbl(temp_price)
                                        End If

                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            If Not IsDBNull(r("ac_asking")) And Not IsDBNull(r("Data_Source")) Then
                                                If (Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "") And Trim(r("Data_Source")) = "JETNET" Then
                                                    aircraft_history_string &= "<A href='' alt='Reported Asking Price Displayed with Permission from Source' title='Reported Asking Price Displayed with Permission from Source'>"
                                                End If
                                            End If
                                        End If

                                        aircraft_history_string &= "$" & FormatNumber(temp_price, 0) & "k"

                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            If Not IsDBNull(r("ac_asking")) And Not IsDBNull(r("Data_Source")) Then
                                                If (Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "") And Trim(r("Data_Source")) = "JETNET" Then
                                                    aircraft_history_string &= "</a>"
                                                End If
                                            End If
                                        End If

                                        If Not IsNothing(CHART_NAME) Then
                                            CHART_NAME.Series("ASKING").Points.AddXY(CDate(temp_data).Date, temp_price)
                                        End If
                                        '  temp_asking = "$" & temp_asking & "k"

                                    Else
                                        If Not IsNothing(CHART_NAME) Then
                                            CHART_NAME.Series("ASKING").Points.AddXY(CDate(temp_data).Date, 0)
                                            CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                        End If

                                        HttpContext.Current.Session.Item("Current_Asking") = 0
                                    End If
                                Else
                                    If Not IsNothing(CHART_NAME) Then
                                        CHART_NAME.Series("ASKING").Points.AddXY(CDate(temp_data).Date, 0)
                                        CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                    End If

                                    HttpContext.Current.Session.Item("Current_Asking") = 0
                                End If
                            Else
                                If Not IsNothing(CHART_NAME) Then
                                    CHART_NAME.Series("ASKING").Points.AddXY(CDate(temp_data).Date, 0)
                                    CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                End If

                                HttpContext.Current.Session.Item("Current_Asking") = 0
                            End If

                            aircraft_history_string &= "&nbsp;</font></td><td align='right'><font size='-2' style='font-family: Arial'>"


                            If Not IsDBNull(r("take_price")) Then
                                If CDbl(r.Item("take_price").ToString) > 0 Then

                                    temp_price = CDbl(r.Item("take_price").ToString)


                                    estimated_value_label_Text &= "<tr>"
                                    estimated_value_label_Text &= "<td class='alt_color'>Current Take Price</td><td align='right'>"
                                    estimated_value_label_Text &= "$" & FormatNumber(temp_price, 0)
                                    estimated_value_label_Text &= "&nbsp;</td></tr> "

                                    temp_price = (temp_price / 1000)
                                    temp_take = temp_price

                                    If high_number = 0 Or CDbl(temp_price) > high_number Then
                                        high_number = CDbl(temp_price)
                                    End If

                                    If low_number = 0 Or CDbl(temp_price) < low_number Then
                                        low_number = CDbl(temp_price)
                                    End If

                                    aircraft_history_string &= "$" & FormatNumber(temp_price, 0) & "k"

                                    If Not IsNothing(CHART_NAME) Then
                                        CHART_NAME.Series("TAKE").Points.AddXY(CDate(temp_data).Date, temp_price)
                                    End If

                                    ' temp_take = "$" & temp_take & "k"

                                Else
                                    If Not IsNothing(CHART_NAME) Then
                                        CHART_NAME.Series("TAKE").Points.AddXY(CDate(temp_data).Date, 0)
                                        CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                    End If
                                    estimated_value_label_Text &= "<td class='alt_color'>Current Take Price</td><td align='right'>"
                                    estimated_value_label_Text &= "-"
                                    estimated_value_label_Text &= "&nbsp;</td></tr> "
                                End If
                            Else
                                If Not IsNothing(CHART_NAME) Then
                                    CHART_NAME.Series("TAKE").Points.AddXY(CDate(temp_data).Date, 0)
                                    CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                End If

                                estimated_value_label_Text &= "<td class='alt_color'>Current Take Price</td><td align='right'>"
                                estimated_value_label_Text &= "-"
                                estimated_value_label_Text &= "&nbsp;</td></tr> "
                            End If

                            aircraft_history_string &= "&nbsp;</font></td><td align='right'><font size='-2' style='font-family: Arial'>"


                            If Not IsDBNull(r("sold_price")) Then
                                If CDbl(r.Item("sold_price").ToString) > 0 Then

                                    'double check
                                    show_price = False
                                    If r.Item("Data_Source").ToString = "JETNET" Then
                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            show_price = True
                                        End If
                                    Else
                                        show_price = True ' its client
                                    End If

                                    If show_price = True Then
                                        temp_price = CDbl(r.Item("sold_price").ToString)

                                        estimated_value_label_Text &= "<tr>"
                                        estimated_value_label_Text &= "<td class='alt_color'>Current Estimated Value</td><td align='right'>"

                                        estimated_value_label_Text &= "$" & FormatNumber(temp_price, 0)

                                        estimated_value_label_Text &= "&nbsp;</td></tr> "

                                        HttpContext.Current.Session.Item("Current_Estimated") = FormatNumber(temp_price, 0)

                                        temp_price = (temp_price / 1000)
                                        temp_sold = temp_price

                                        If high_number = 0 Or CDbl(temp_price) > high_number Then
                                            high_number = CDbl(temp_price)
                                        End If

                                        If low_number = 0 Or CDbl(temp_price) < low_number Then
                                            low_number = CDbl(temp_price)
                                        End If


                                        If r.Item("Data_Source").ToString = "JETNET" Then
                                            aircraft_history_string &= "<A href='' alt='Reported Sale Price Displayed with Permission from Source' title='Reported Sale Price Displayed with Permission from Source'>"
                                            aircraft_history_string &= "<p unselectable='on' style='display:inline'>"
                                            aircraft_history_string &= DisplayFunctions.TextToImage("$" & FormatNumber(temp_price, 0) & "k", 7, "", "42", "Reported Sale Price Displayed with Permission from Source")
                                        Else
                                            aircraft_history_string &= "$" & FormatNumber(temp_price, 0) & "k"
                                        End If


                                        If r.Item("Data_Source").ToString = "JETNET" Then
                                            aircraft_history_string &= "</p></a>"
                                        End If


                                        ' temp_sold = "$" & temp_sold & "k"

                                        If Not IsNothing(CHART_NAME) Then
                                            CHART_NAME.Series("SOLD").Points.AddXY(CDate(temp_data).Date, temp_price)
                                        End If
                                    Else
                                        If Not IsNothing(CHART_NAME) Then
                                            CHART_NAME.Series("SOLD").Points.AddXY(CDate(temp_data).Date, 0)
                                            CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                        End If

                                        estimated_value_label_Text &= "<td class='alt_color'>Current Estimated Value</td><td align='right'>"
                                        estimated_value_label_Text &= "-"
                                        estimated_value_label_Text &= "&nbsp;</td></tr> "

                                        HttpContext.Current.Session.Item("Current_Estimated") = 0
                                    End If
                                Else
                                    If Not IsNothing(CHART_NAME) Then
                                        CHART_NAME.Series("SOLD").Points.AddXY(CDate(temp_data).Date, 0)
                                        CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                    End If

                                    estimated_value_label_Text &= "<td class='alt_color'>Current Estimated Value</td><td align='right'>"
                                    estimated_value_label_Text &= "-"
                                    estimated_value_label_Text &= "&nbsp;</td></tr> "

                                    HttpContext.Current.Session.Item("Current_Estimated") = 0

                                End If
                            Else
                                estimated_value_label_Text &= "<td class='alt_color'>Current Estimated Value</td><td align='right'>"
                                estimated_value_label_Text &= "-"
                                estimated_value_label_Text &= "&nbsp;</td></tr> "

                                If Not IsNothing(CHART_NAME) Then
                                    CHART_NAME.Series("SOLD").Points.AddXY(CDate(temp_data).Date, 0)
                                    CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                End If
                                HttpContext.Current.Session.Item("Current_Estimated") = 0
                            End If


                            aircraft_history_string &= "&nbsp;</td></tr>"

                            If temp_asking <> "null" Or temp_take <> "null" Or temp_sold <> "null" Then
                                exists_data = True
                                If Trim(temp_data) = Trim(Date.Now.Date) And is_est_value = False Then
                                    If row_added Then
                                        google_map_string &= ",['" & temp_data & "', null, null, null,  " & temp_asking & ", " & temp_take & ", " & temp_sold & "]"
                                    Else
                                        google_map_string &= "['" & temp_data & "', null, null, null,  " & temp_asking & ", " & temp_take & ", " & temp_sold & "]"
                                    End If
                                    row_added = True
                                Else
                                    If row_added Then
                                        google_map_string &= ",['" & temp_data & "',  " & temp_asking & ", " & temp_take & ", " & temp_sold & ", null, null, null]"
                                    Else
                                        google_map_string &= "['" & temp_data & "',  " & temp_asking & ", " & temp_take & ", " & temp_sold & ", null, null, null]"
                                    End If
                                    row_added = True
                                End If


                            End If

                        End If

                    Next

                End If
            End If

            aircraft_history_string &= "</table>"

            estimated_value_label_Text &= "</table>"


            results_table = Nothing
            results_table2 = Nothing
            results_table3 = Nothing
            final_table = Nothing


            Call get_ranges(low_number, high_number, interval_point, starting_point)


            If Not IsNothing(CHART_NAME) Then
                CHART_NAME.ChartAreas("ChartArea1").AxisY.Maximum = high_number + interval_point
                CHART_NAME.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
                CHART_NAME.ChartAreas("ChartArea1").AxisY.Interval = interval_point
            End If

        Catch ex As Exception

            aError = "Error in views_analytics_graph_1(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef CHART_NAME As DataVisualization.Charting.Chart) " + ex.Message

        Finally

        End Try

    End Sub

    Public Sub views_analytics_graph_market_status(ByVal ac_id As Long, ByRef CHART_NAME As DataVisualization.Charting.Chart, ByVal avg_asking As Long, ByVal avg_take As Long, ByVal avg_sold As Long, ByRef google_map_string As String, ByVal COMPLETED_OR_OPEN As String, Optional ByVal amod_id_no_ac_id As Long = 0)

        Dim high_number As Integer = 0
        Dim low_number As Integer = 0
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1

        Dim daysonmarket As Integer = 0
        Dim daysonmarket2 As Integer = 0
        Dim days As Integer = 0
        Dim test_string As String = ""

        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim results_table3 As New DataTable
        Dim final_table As New DataTable

        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable  
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn
        Dim column5 As New DataColumn

        Dim temp_price As Long = 0
        Dim temp_data As String = ""
        Dim temp_asking As String = "null"
        Dim temp_take As String = "null"
        Dim temp_sold As String = "null"

        ' google_map_string = "['Date', 'Asking', 'Take', 'Estimated Value']"

        google_map_string = " data4.addColumn('string', 'Serial#'); "
        google_map_string &= " data4.addColumn('number', 'Asking'); "
        google_map_string &= " data4.addColumn('number', 'Take'); "
        google_map_string &= " data4.addColumn('number', 'Est Value'); "
        google_map_string &= " data4.addColumn('number', 'My AC Asking'); "
        google_map_string &= " data4.addColumn('number', 'My AC Take'); "
        google_map_string &= " data4.addColumn('number', 'My AC Est Value'); "
        google_map_string &= " data4.addRows(["

        Try

            results_table = get_my_ac_value_history_comparables(ac_id, "current_status", True, 0, 0, COMPLETED_OR_OPEN)      ' get current ac primary info 

            'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
            column.DataType = System.Type.GetType("System.Double")
            column.DefaultValue = 0
            column.Unique = False
            column.ColumnName = "asking_price"
            final_table.Columns.Add(column)

            column2.DataType = System.Type.GetType("System.Double")
            column2.DefaultValue = 0
            column2.Unique = False
            column2.ColumnName = "take_price"
            final_table.Columns.Add(column2)

            column3.DataType = System.Type.GetType("System.Double")
            column3.DefaultValue = 0
            column3.AllowDBNull = True
            column3.Unique = False
            column3.ColumnName = "sold_price"
            final_table.Columns.Add(column3)


            column4.DataType = System.Type.GetType("System.DateTime")
            column4.AllowDBNull = True
            column4.Unique = False
            column4.ColumnName = "date_of"
            final_table.Columns.Add(column4)

            column5.DataType = System.Type.GetType("System.String")
            column5.AllowDBNull = True
            column5.Unique = False
            column5.ColumnName = "ac_details"
            final_table.Columns.Add(column5)

            For Each drRow As DataRow In results_table.Rows
                final_table.ImportRow(drRow)
            Next



            ' Dim afiltered_Jetnet As DataRow() = final_table.Select("", "date_of asc")


            CHART_NAME.Series.Clear()
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Title = "Market Status ($k)"

            CHART_NAME.Series.Add("ASKING").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("ASKING").LabelForeColor = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").Color = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").BorderWidth = 1
            CHART_NAME.Series("ASKING").MarkerSize = 5
            CHART_NAME.Series("ASKING").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32


            CHART_NAME.Series.Add("TAKE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("TAKE").LabelForeColor = Drawing.Color.Red
            CHART_NAME.Series("TAKE").Color = Drawing.Color.Red
            CHART_NAME.Series("TAKE").BorderWidth = 1
            CHART_NAME.Series("TAKE").MarkerSize = 5
            CHART_NAME.Series("TAKE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle


            CHART_NAME.Series.Add("SOLD").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("SOLD").LabelForeColor = Drawing.Color.Green
            CHART_NAME.Series("SOLD").Color = Drawing.Color.Green
            CHART_NAME.Series("SOLD").BorderWidth = 1
            CHART_NAME.Series("SOLD").MarkerSize = 5
            CHART_NAME.Series("SOLD").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle

            'CHART_NAME.Series.Add("ASKING_AVG").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            'CHART_NAME.Series("ASKING_AVG").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            'CHART_NAME.Series("ASKING_AVG").LabelForeColor = Drawing.Color.Blue
            'CHART_NAME.Series("ASKING_AVG").Color = Drawing.Color.Blue
            'CHART_NAME.Series("ASKING_AVG").BorderWidth = 1
            'CHART_NAME.Series("ASKING_AVG").MarkerSize = 6
            'CHART_NAME.Series("ASKING_AVG").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            'CHART_NAME.Series("ASKING_AVG").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

            'CHART_NAME.Series.Add("TAKEAVG").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            'CHART_NAME.Series("TAKEAVG").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            'CHART_NAME.Series("TAKEAVG").LabelForeColor = Drawing.Color.DarkRed
            'CHART_NAME.Series("TAKEAVG").Color = Drawing.Color.DarkRed
            'CHART_NAME.Series("TAKEAVG").BorderWidth = 1
            'CHART_NAME.Series("TAKEAVG").MarkerSize = 7
            'CHART_NAME.Series("TAKEAVG").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross


            'CHART_NAME.Series.Add("SOLDAVG").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            'CHART_NAME.Series("SOLDAVG").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            'CHART_NAME.Series("SOLDAVG").LabelForeColor = Drawing.Color.DarkGreen
            'CHART_NAME.Series("SOLDAVG").Color = Drawing.Color.DarkGreen
            'CHART_NAME.Series("SOLDAVG").BorderWidth = 1
            'CHART_NAME.Series("SOLDAVG").MarkerSize = 7
            'CHART_NAME.Series("SOLDAVG").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross






            CHART_NAME.Width = 300
            CHART_NAME.Height = 300


            If Not IsNothing(final_table) Then

                If final_table.Rows.Count > 0 Then

                    For Each r As DataRow In final_table.Rows

                        temp_asking = "null"
                        temp_take = "null"
                        temp_sold = "null"

                        If Not IsDBNull(r("ac_details")) Then
                            temp_data = r("ac_details")
                        Else
                            temp_data = ""
                        End If


                        If Not IsDBNull(r("asking_price")) Then
                            If CDbl(r.Item("asking_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("asking_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_asking = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If


                                CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, temp_price)
                                ' CHART_NAME.Series("ASKING").Points(CHART_NAME.Series("ASKING").Points.Count - 1).Label = "ASKING"
                            Else
                                CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, 0)
                                CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                ' CHART_NAME.Series("ASKING").Points(CHART_NAME.Series("ASKING").Points.Count).Label = "ASKING"
                            End If
                        Else
                            CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, 0)
                            CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                            ' CHART_NAME.Series("ASKING").Points(CHART_NAME.Series("ASKING").Points.Count).Label = "ASKING"
                        End If

                        If Not IsDBNull(r("take_price")) Then
                            If CDbl(r.Item("take_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("take_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_take = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If

                                CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, temp_price)
                                'CHART_NAME.Series("TAKE").Points(CHART_NAME.Series("TAKE").Points.Count - 1).Label = "TAKE"

                            Else
                                CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, 0)
                                CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                            End If
                        Else
                            CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, 0)
                            CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                        End If




                        If Not IsDBNull(r("sold_price")) Then
                            If CDbl(r.Item("sold_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("sold_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_sold = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If

                                CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, temp_price)
                                ' CHART_NAME.Series("SOLD").Points(CHART_NAME.Series("SOLD").Points.Count - 1).Label = "SOLD"

                            Else
                                CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, 0)
                                CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                            End If
                        Else
                            CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, 0)
                            CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                        End If

                        If amod_id_no_ac_id > 0 Then
                        Else
                            If temp_asking <> "null" Or temp_take <> "null" Or temp_sold <> "null" Then
                                google_map_string &= "['" & temp_data & "',null, null, null, " & temp_asking & ", " & temp_take & ", " & temp_sold & "]"
                            End If
                        End If




                        temp_data = "Market AVG"
                        temp_asking = "null"
                        temp_take = "null"
                        temp_sold = "null"


                        If CDbl(avg_asking) > 0 Then

                            temp_price = CDbl(avg_asking)
                            temp_asking = temp_price

                            If high_number = 0 Or CDbl(temp_price) > high_number Then
                                high_number = CDbl(temp_price)
                            End If

                            If low_number = 0 Or CDbl(temp_price) < low_number Then
                                low_number = CDbl(temp_price)
                            End If

                            CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, temp_price)
                        End If


                        If CDbl(avg_take) > 0 Then

                            temp_price = CDbl(avg_take)
                            temp_take = temp_price

                            If high_number = 0 Or CDbl(temp_price) > high_number Then
                                high_number = CDbl(temp_price)
                            End If

                            If low_number = 0 Or CDbl(temp_price) < low_number Then
                                low_number = CDbl(temp_price)
                            End If

                            CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, temp_price)
                            ' CHART_NAME.Series("TAKEAVG").Points(CHART_NAME.Series("TAKEAVG").Points.Count - 1).Label = "TAKE AVERAGE"
                        End If

                        If CDbl(avg_sold) > 0 Then

                            temp_price = CDbl(avg_sold)
                            temp_sold = temp_price

                            If high_number = 0 Or CDbl(temp_price) > high_number Then
                                high_number = CDbl(temp_price)
                            End If

                            If low_number = 0 Or CDbl(temp_price) < low_number Then
                                low_number = CDbl(temp_price)
                            End If

                            CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, temp_price)
                            'CHART_NAME.Series("SOLDAVG").Points(CHART_NAME.Series("SOLDAVG").Points.Count - 1).Label = "SOLD AVERAGE"
                        End If



                        If temp_asking <> "null" Or temp_take <> "null" Or temp_sold <> "null" Then
                            ' google_map_string &= ",['" & temp_data & "',  " & temp_asking & ", " & temp_take & ", " & temp_sold & "]"
                            If amod_id_no_ac_id > 0 Then ' then it will be the first point
                                google_map_string &= "['" & temp_data & "', " & temp_asking & ", " & temp_take & ", " & temp_sold & ", null, null, null]"
                            Else
                                google_map_string &= ",['" & temp_data & "', " & temp_asking & ", " & temp_take & ", " & temp_sold & ", null, null, null]"
                            End If

                        End If




                    Next

                End If
            End If






            results_table = Nothing
            results_table2 = Nothing
            results_table3 = Nothing
            final_table = Nothing


            Call get_ranges(low_number, high_number, interval_point, starting_point)

            CHART_NAME.ChartAreas("ChartArea1").AxisY.Maximum = high_number + interval_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Interval = interval_point

            CHART_NAME.ChartAreas("ChartArea1").AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.FixedCount
            CHART_NAME.ChartAreas("ChartArea1").AxisX.Interval = 1
            '  CHART_NAME.ChartAreas("ChartArea1").AxisX.IntervalOffset = 0


        Catch ex As Exception

            aError = "Error in views_analytics_graph_market_status(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef CHART_NAME As DataVisualization.Charting.Chart) " + ex.Message

        Finally

        End Try

    End Sub

    Public Function views_analytics_graph_completed_current(ByVal ac_id As Long, ByRef CHART_NAME As DataVisualization.Charting.Chart, ByVal note_id As Long, ByVal searchCriteria As viewSelectionCriteriaClass, ByVal COMPLETED_OR_OPEN As String, ByRef google_map_string As String) As String
        views_analytics_graph_completed_current = ""

        Dim high_number As Integer = 0
        Dim low_number As Integer = 0
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1

        Dim daysonmarket As Integer = 0
        Dim daysonmarket2 As Integer = 0
        Dim days As Integer = 0
        Dim test_string As String = ""

        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim results_table3 As New DataTable
        Dim final_table As New DataTable

        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn

        Dim build_table As String = ""
        Dim temp_price As Long = 0
        Dim temp_asking As String = "null"
        Dim temp_take As String = "null"
        Dim temp_sold As String = "null"
        Dim temp_data As String = ""
        Dim added_row As Boolean = False

        Try



            results_table = get_my_ac_value_make_model(ac_id, COMPLETED_OR_OPEN, note_id, False)      ' get current from current ac table


            'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
            column.DataType = System.Type.GetType("System.Double")
            column.DefaultValue = 0
            column.Unique = False
            column.ColumnName = "asking_price"
            final_table.Columns.Add(column)

            column2.DataType = System.Type.GetType("System.Double")
            column2.DefaultValue = 0
            column2.Unique = False
            column2.ColumnName = "take_price"
            final_table.Columns.Add(column2)

            column3.DataType = System.Type.GetType("System.Double")
            column3.DefaultValue = 0
            column3.AllowDBNull = True
            column3.Unique = False
            column3.ColumnName = "broker_price"
            final_table.Columns.Add(column3)


            column4.DataType = System.Type.GetType("System.String")
            column4.AllowDBNull = True
            column4.Unique = False
            column4.ColumnName = "ac_details"
            final_table.Columns.Add(column4)


            For Each drRow As DataRow In results_table.Rows
                final_table.ImportRow(drRow)
            Next





            CHART_NAME.Series.Clear()
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Title = "Aircraft Value ($k)"

            CHART_NAME.Series.Add("ASKING").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("ASKING").LabelForeColor = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").Color = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").BorderWidth = 1
            CHART_NAME.Series("ASKING").MarkerSize = 5
            CHART_NAME.Series("ASKING").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

            CHART_NAME.Series.Add("TAKE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("TAKE").LabelForeColor = Drawing.Color.Red
            CHART_NAME.Series("TAKE").Color = Drawing.Color.Red
            CHART_NAME.Series("TAKE").BorderWidth = 1
            CHART_NAME.Series("TAKE").MarkerSize = 5
            CHART_NAME.Series("TAKE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            CHART_NAME.Series.Add("SOLD").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("SOLD").LabelForeColor = Drawing.Color.Green
            CHART_NAME.Series("SOLD").Color = Drawing.Color.Green
            CHART_NAME.Series("SOLD").BorderWidth = 1
            CHART_NAME.Series("SOLD").MarkerSize = 5
            CHART_NAME.Series("SOLD").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("SOLD").MarkerImageTransparentColor = Drawing.Color.White



            CHART_NAME.Width = 300
            CHART_NAME.Height = 300

            build_table = "<table border='1' cellpadding='3' cellspacing='0'>"
            build_table &= "<tr class='header_text'><td align='left'><font size='-2'><b>Aircraft</b></td><td align='left'><font size='-2'><b>Asking $</b></td><td align='left'><font size='-2'><b>Take	$</b></td><td align='left'><font size='-2'><b>Estimated Value $</b></font></td></tr>"

            If Not IsNothing(final_table) Then

                If final_table.Rows.Count > 0 Then


                    '  google_map_string = "['Date', 'Asking', 'Take', 'Sold']"


                    google_map_string = " data2.addColumn('string', 'Serial#'); "
                    google_map_string &= " data2.addColumn('number', 'Asking'); "
                    google_map_string &= " data2.addColumn('number', 'Take'); "
                    google_map_string &= " data2.addColumn('number', 'Est Value'); "
                    google_map_string &= " data2.addColumn('number', 'My AC Asking'); "
                    google_map_string &= " data2.addColumn('number', 'My AC Take'); "
                    google_map_string &= " data2.addColumn('number', 'My AC Est Value'); "
                    google_map_string &= " data2.addRows(["

                    For Each r As DataRow In final_table.Rows

                        temp_asking = "null"
                        temp_take = "null"
                        temp_sold = "null"
                        temp_data = r.Item("ac_details").ToString


                        build_table &= "<tr class='small_header_text'><td align='left'><font size='-2'>"

                        build_table &= r.Item("ac_details").ToString

                        build_table &= "&nbsp;</font></td><td align='right'><font size='-2'>"


                        If Not IsDBNull(r("asking_price")) Then
                            If CDbl(r.Item("asking_price").ToString) > 0 Then


                                temp_price = CDbl(r.Item("asking_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_asking = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If
                                CHART_NAME.Series("ASKING").Points.AddXY(temp_data, temp_price)


                                build_table &= "$" & FormatNumber(CDbl(temp_price), 0).ToString & "k"
                            Else
                                CHART_NAME.Series("ASKING").Points.AddXY(temp_data, 0)
                                CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                            End If
                        Else
                            CHART_NAME.Series("ASKING").Points.AddXY(temp_data, 0)
                            CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                        End If

                        build_table &= "&nbsp;</font></td><td align='right'><font size='-2'>"

                        If Not IsDBNull(r("take_price")) Then
                            If CDbl(r.Item("take_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("take_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_take = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If
                                CHART_NAME.Series("TAKE").Points.AddXY(temp_data, temp_price)


                                build_table &= "$" & FormatNumber(CDbl(temp_price), 0).ToString & "k"
                            Else
                                CHART_NAME.Series("TAKE").Points.AddXY(temp_data, 0)
                                CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                            End If
                        Else
                            CHART_NAME.Series("TAKE").Points.AddXY(temp_data, 0)
                            CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                        End If

                        build_table &= "&nbsp;</td><td align='right'><font size='-2'>"

                        If Not IsDBNull(r("broker_price")) Then
                            If CDbl(r.Item("broker_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("broker_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_sold = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If
                                CHART_NAME.Series("SOLD").Points.AddXY(r.Item("ac_details").ToString, temp_price)


                                build_table &= "$" & FormatNumber(CDbl(temp_price), 0).ToString & "k"
                            Else
                                CHART_NAME.Series("SOLD").Points.AddXY(temp_data, 0)
                                CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                            End If
                        Else
                            CHART_NAME.Series("SOLD").Points.AddXY(temp_data, 0)
                            CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                        End If


                        build_table &= "&nbsp;</font></td></tr>"

                        If temp_asking <> "null" Or temp_take <> "null" Or temp_sold <> "null" Then
                            If added_row Then
                                google_map_string &= ",['" & temp_data & "',  " & temp_asking & ", " & temp_take & ", " & temp_sold & ", null, null, null]"
                            Else
                                google_map_string &= "['" & temp_data & "', null, null, null, " & temp_asking & ", " & temp_take & ", " & temp_sold & "]"
                            End If
                            added_row = True
                        End If

                    Next



                End If
            End If

            build_table &= "</table>"

            results_table = Nothing
            results_table2 = Nothing
            results_table3 = Nothing
            final_table = Nothing

            Call get_ranges(low_number, high_number, interval_point, starting_point)


            CHART_NAME.ChartAreas("ChartArea1").AxisY.Maximum = high_number + interval_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Interval = interval_point

            views_analytics_graph_completed_current = build_table

        Catch ex As Exception

            aError = "Error in views_analytics_graph_1(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef CHART_NAME As DataVisualization.Charting.Chart) " + ex.Message

        Finally

        End Try

    End Function

    Public Sub views_analytics_graph_market_status_new2(ByVal ac_id As Long, ByRef CHART_NAME As DataVisualization.Charting.Chart, ByVal avg_asking As Long, ByVal avg_take As Long, ByVal avg_sold As Long, ByVal note_id As Long, ByVal COMPLETED_OR_OPEN As String)
        Dim high_number As Integer = 0
        Dim low_number As Integer = 0
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1

        Dim daysonmarket As Integer = 0
        Dim daysonmarket2 As Integer = 0
        Dim days As Integer = 0
        Dim test_string As String = ""

        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim results_table3 As New DataTable

        Dim final_table As New DataTable

        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn

        Dim temp_price As Long = 0
        Dim temp_data As String = ""

        Try



            results_table = get_my_ac_value_history_comparables(ac_id, "Current", True, 0, note_id, COMPLETED_OR_OPEN)      ' get current ac primary info 


            CHART_NAME.Series.Clear()
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Title = "Aircraft Value ($k)"



            CHART_NAME.Series.Add("ASKING_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("ASKING_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("ASKING_CURRENT").LabelForeColor = Drawing.Color.Blue
            CHART_NAME.Series("ASKING_CURRENT").Color = Drawing.Color.Blue
            CHART_NAME.Series("ASKING_CURRENT").BorderWidth = 1
            CHART_NAME.Series("ASKING_CURRENT").MarkerSize = 6
            CHART_NAME.Series("ASKING_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            CHART_NAME.Series("ASKING_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

            CHART_NAME.Series.Add("TAKE_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("TAKE_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("TAKE_CURRENT").LabelForeColor = Drawing.Color.Red
            CHART_NAME.Series("TAKE_CURRENT").Color = Drawing.Color.Red
            CHART_NAME.Series("TAKE_CURRENT").BorderWidth = 1
            CHART_NAME.Series("TAKE_CURRENT").MarkerSize = 6
            CHART_NAME.Series("TAKE_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            CHART_NAME.Series("TAKE_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32


            CHART_NAME.Series.Add("SOLD_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("SOLD_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("SOLD_CURRENT").LabelForeColor = Drawing.Color.Green
            CHART_NAME.Series("SOLD_CURRENT").Color = Drawing.Color.Green
            CHART_NAME.Series("SOLD_CURRENT").BorderWidth = 1
            CHART_NAME.Series("SOLD_CURRENT").MarkerSize = 6
            CHART_NAME.Series("SOLD_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            CHART_NAME.Series("SOLD_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32


            CHART_NAME.Series("ASKING_CURRENT").Points.AddXY("Avg Comparables", 1)
            CHART_NAME.Series("TAKE_CURRENT").Points.AddXY("Avg Comparables", 2)
            CHART_NAME.Series("SOLD_CURRENT").Points.AddXY("Avg Comparables", 3)



            CHART_NAME.Width = 300
            CHART_NAME.Height = 300



            CHART_NAME.Series.Add("ASKING").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("ASKING").LabelForeColor = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").Color = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").BorderWidth = 1
            CHART_NAME.Series("ASKING").MarkerSize = 5
            CHART_NAME.Series("ASKING").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

            CHART_NAME.Series.Add("TAKE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("TAKE").LabelForeColor = Drawing.Color.Red
            CHART_NAME.Series("TAKE").Color = Drawing.Color.Red
            CHART_NAME.Series("TAKE").BorderWidth = 1
            CHART_NAME.Series("TAKE").MarkerSize = 5
            CHART_NAME.Series("TAKE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            CHART_NAME.Series.Add("SOLD").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("SOLD").LabelForeColor = Drawing.Color.Green
            CHART_NAME.Series("SOLD").Color = Drawing.Color.Green
            CHART_NAME.Series("SOLD").BorderWidth = 1
            CHART_NAME.Series("SOLD").MarkerSize = 5
            CHART_NAME.Series("SOLD").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            CHART_NAME.Series("ASKING_CURRENT").Points.AddXY("My AC", 4)
            CHART_NAME.Series("TAKE_CURRENT").Points.AddXY("My AC", 5)
            CHART_NAME.Series("SOLD_CURRENT").Points.AddXY("My AC", 6)




            results_table = Nothing
            results_table2 = Nothing
            results_table3 = Nothing
            final_table = Nothing

            Call get_ranges(low_number, high_number, interval_point, starting_point)

            CHART_NAME.ChartAreas("ChartArea1").AxisY.Maximum = high_number + interval_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Interval = interval_point

            'CHART_NAME.ChartAreas("ChartArea1").AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.VariableCount
            ''  CHART_NAME.ChartAreas("ChartArea1").AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.FixedCount
            ''  CHART_NAME.ChartAreas("ChartArea1").AxisX.Interval = 1

        Catch ex As Exception

            aError = "Error in views_analytics_graph_1(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef CHART_NAME As DataVisualization.Charting.Chart) " + ex.Message

        Finally

        End Try

    End Sub

    Public Sub views_analytics_graph_2_Survey(ByVal ac_id As Long, ByRef CHART_NAME As DataVisualization.Charting.Chart, ByVal note_id As Long, ByVal searchCriteria As viewSelectionCriteriaClass, ByVal ClientIDSToExclude As String, ByRef google_map_array_list As String, ByVal COMPLETED_OR_OPEN As String, Optional ByVal RunModelOnly As Boolean = False)
        Dim high_number As Integer = 0
        Dim low_number As Integer = 0
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1

        Dim daysonmarket As Integer = 0
        Dim daysonmarket2 As Integer = 0
        Dim days As Integer = 0
        Dim test_string As String = ""

        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim results_table3 As New DataTable

        Dim final_table As New DataTable

        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn

        Dim temp_price As Long = 0
        Dim temp_data As String = ""

        Dim temp_asking As String = ""
        Dim temp_take As String = ""
        Dim temp_sold As String = ""
        Dim temp_asking_my As String = ""
        Dim temp_take_my As String = ""
        Dim temp_sold_my As String = ""

        Dim added_row As Boolean = False

        Try



            results_table = get_my_ac_value_history_comparables(ac_id, "Summary", False, 0, note_id, COMPLETED_OR_OPEN)      ' get current from current ac table 

            '   For Each drRow As DataRow In results_table.Rows
            'final_table.ImportRow(drRow)
            '   Next
            CHART_NAME.Series.Clear()
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Title = "Aircraft Value ($k)"

            CHART_NAME.Series.Add("ASKING_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("ASKING_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("ASKING_CURRENT").LabelForeColor = Drawing.Color.Blue
            CHART_NAME.Series("ASKING_CURRENT").Color = Drawing.Color.Blue
            CHART_NAME.Series("ASKING_CURRENT").BorderWidth = 1
            CHART_NAME.Series("ASKING_CURRENT").MarkerSize = 6
            CHART_NAME.Series("ASKING_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            CHART_NAME.Series("ASKING_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32


            CHART_NAME.Series.Add("SOLD_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("SOLD_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("SOLD_CURRENT").LabelForeColor = Drawing.Color.Green
            CHART_NAME.Series("SOLD_CURRENT").Color = Drawing.Color.Green
            CHART_NAME.Series("SOLD_CURRENT").BorderWidth = 1
            CHART_NAME.Series("SOLD_CURRENT").MarkerSize = 6
            CHART_NAME.Series("SOLD_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            CHART_NAME.Series("SOLD_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32


            CHART_NAME.Series.Add("TAKE_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("TAKE_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("TAKE_CURRENT").LabelForeColor = Drawing.Color.Red
            CHART_NAME.Series("TAKE_CURRENT").Color = Drawing.Color.Red
            CHART_NAME.Series("TAKE_CURRENT").BorderWidth = 1
            CHART_NAME.Series("TAKE_CURRENT").MarkerSize = 6
            CHART_NAME.Series("TAKE_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            CHART_NAME.Series("TAKE_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32





            CHART_NAME.Width = 300
            CHART_NAME.Height = 300


            '  google_map_array_list = "['Serial#', 'Asking', 'Take', 'Estimated Value', 'My AC Asking', 'My AC Take', 'My AC Estimated Value']"

            google_map_array_list = " data5.addColumn('string', 'Serial#'); "
            google_map_array_list &= " data5.addColumn('number', 'Asking'); "
            google_map_array_list &= " data5.addColumn('number', 'Est Value'); "
            google_map_array_list &= " data5.addColumn('number', 'Take'); "
            google_map_array_list &= " data5.addColumn('number', 'My AC Asking'); "
            google_map_array_list &= " data5.addColumn('number', 'My AC Est Value'); "
            google_map_array_list &= " data5.addColumn('number', 'My AC Take'); "

            google_map_array_list &= " data5.addRows(["


            If RunModelOnly = False Then
                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        For Each r As DataRow In results_table.Rows

                            temp_asking = "null"
                            temp_take = "null"
                            temp_sold = "null"

                            temp_data = r.Item("ac_details").ToString


                            If Not IsDBNull(r("asking_price")) Then
                                If CDbl(r.Item("asking_price").ToString) > 0 Then


                                    temp_price = CDbl(r.Item("asking_price").ToString)
                                    temp_price = (temp_price / 1000)
                                    temp_asking = temp_price

                                    If high_number = 0 Or CDbl(temp_price) > high_number Then
                                        high_number = CDbl(temp_price)
                                    End If

                                    If low_number = 0 Or CDbl(temp_price) < low_number Then
                                        low_number = CDbl(temp_price)
                                    End If
                                    CHART_NAME.Series("ASKING_CURRENT").Points.AddXY(temp_data.ToString, temp_price)
                                End If
                            End If


                            If Not IsDBNull(r("take_price")) Then
                                If CDbl(r.Item("take_price").ToString) > 0 Then

                                    temp_price = CDbl(r.Item("take_price").ToString)
                                    temp_price = (temp_price / 1000)
                                    temp_take = temp_price

                                    If high_number = 0 Or CDbl(temp_price) > high_number Then
                                        high_number = CDbl(temp_price)
                                    End If

                                    If low_number = 0 Or CDbl(temp_price) < low_number Then
                                        low_number = CDbl(temp_price)
                                    End If
                                    CHART_NAME.Series("TAKE_CURRENT").Points.AddXY(temp_data.ToString, temp_price)
                                End If
                            End If


                            If Not IsDBNull(r("sold_price")) Then
                                If CDbl(r.Item("sold_price").ToString) > 0 Then


                                    temp_price = CDbl(r.Item("sold_price").ToString)
                                    temp_price = (temp_price / 1000)
                                    temp_sold = temp_price

                                    If high_number = 0 Or CDbl(temp_price) > high_number Then
                                        high_number = CDbl(temp_price)
                                    End If

                                    If low_number = 0 Or CDbl(temp_price) < low_number Then
                                        low_number = CDbl(temp_price)
                                    End If
                                    CHART_NAME.Series("SOLD_CURRENT").Points.AddXY(temp_data.ToString, temp_price)
                                End If
                            End If

                            temp_asking_my = temp_asking
                            temp_take_my = temp_take
                            temp_sold_my = temp_sold

                            '  google_map_array_list &= ",['" & temp_data & "',  " & temp_asking & ", " & temp_take & ", " & temp_sold & ", " & temp_asking_avg & "]"
                            google_map_array_list &= "['" & temp_data & "', null, null, null, " & temp_asking & ", " & temp_sold & ", " & temp_take & "]"
                            added_row = True


                        Next

                    End If
                End If

            End If



            If Not IsNothing(HttpContext.Current.Session.Item("COMPARE_FOR_SALE_TABLE")) Then
                final_table = HttpContext.Current.Session.Item("COMPARE_FOR_SALE_TABLE")
            Else
                Call crmViewDataLayer.views_display_aircraft_forsale(searchCriteria, "", False, Me, True, ClientIDSToExclude, True, final_table, RunModelOnly)
            End If




            CHART_NAME.Series.Add("ASKING").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("ASKING").LabelForeColor = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").Color = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").BorderWidth = 1
            CHART_NAME.Series("ASKING").MarkerSize = 5
            CHART_NAME.Series("ASKING").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

            CHART_NAME.Series.Add("TAKE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("TAKE").LabelForeColor = Drawing.Color.Red
            CHART_NAME.Series("TAKE").Color = Drawing.Color.Red
            CHART_NAME.Series("TAKE").BorderWidth = 1
            CHART_NAME.Series("TAKE").MarkerSize = 5
            CHART_NAME.Series("TAKE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            CHART_NAME.Series.Add("SOLD").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("SOLD").LabelForeColor = Drawing.Color.Green
            CHART_NAME.Series("SOLD").Color = Drawing.Color.Green
            CHART_NAME.Series("SOLD").BorderWidth = 1
            CHART_NAME.Series("SOLD").MarkerSize = 5
            CHART_NAME.Series("SOLD").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            If Not IsNothing(final_table) Then

                If final_table.Rows.Count > 0 Then

                    For Each r As DataRow In final_table.Rows


                        temp_asking = "null"
                        temp_take = "null"
                        temp_sold = "null"


                        temp_data = r.Item("ac_ser_no_full").ToString


                        If Not IsDBNull(r("ac_asking")) Then
                            If Trim(r.Item("ac_asking").ToString) = "Price" Then
                                If IsNumeric(Trim(r.Item("ac_asking_price").ToString)) Then

                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then

                                        temp_price = CDbl(r.Item("ac_asking_price").ToString)
                                        temp_price = (temp_price / 1000)
                                        temp_asking = temp_price

                                        If high_number = 0 Or CDbl(temp_price) > high_number Then
                                            high_number = CDbl(temp_price)
                                        End If

                                        If low_number = 0 Or CDbl(temp_price) < low_number Then
                                            low_number = CDbl(temp_price)
                                        End If
                                        CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, temp_price)
                                    End If
                                End If
                            End If
                        End If


                        If Not IsDBNull(r("ac_take_price")) Then
                            If Trim(r.Item("ac_take_price").ToString) <> "" Then
                                If IsNumeric(Trim(r.Item("ac_take_price").ToString)) Then
                                    If CDbl(r.Item("ac_take_price").ToString) > 0 Then

                                        temp_price = CDbl(r.Item("ac_take_price").ToString)
                                        temp_price = (temp_price / 1000)
                                        temp_take = temp_price

                                        If high_number = 0 Or CDbl(temp_price) > high_number Then
                                            high_number = CDbl(temp_price)
                                        End If

                                        If low_number = 0 Or CDbl(temp_price) < low_number Then
                                            low_number = CDbl(temp_price)
                                        End If
                                        CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, temp_price)
                                    End If
                                End If
                            End If
                        End If


                        If Not IsDBNull(r("sold_price")) Then
                            If CDbl(r.Item("sold_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("sold_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_sold = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If
                                CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, temp_price)
                            End If
                        Else

                        End If


                        ' FILL IN BLANKS------------------------------------------------------------------------------
                        If Not IsDBNull(r("ac_asking")) Then
                            If Trim(r.Item("ac_asking").ToString) = "Price" Then
                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                                        If Not IsDBNull(r("ac_take_price")) Then
                                            If CDbl(r.Item("ac_take_price").ToString) > 0 Then
                                            Else
                                                CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, 0)
                                                CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                            End If
                                        Else
                                            CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, 0)
                                            CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                        End If

                                        If Not IsDBNull(r("sold_price")) Then
                                            If CDbl(r.Item("sold_price").ToString) > 0 Then
                                            Else
                                                CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, 0)
                                                CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                            End If
                                        Else
                                            CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, 0)
                                            CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        If Not IsDBNull(r("ac_take_price")) Then
                            If CDbl(r.Item("ac_take_price").ToString) > 0 Then

                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, 0)
                                        CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, 0)
                                    CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                End If

                                If Not IsDBNull(r("sold_price")) Then
                                    If CDbl(r.Item("sold_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, 0)
                                        CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("SOLD").Points.AddXY(temp_data.ToString, 0)
                                    CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                End If
                            End If
                        End If



                        If Not IsDBNull(r("sold_price")) Then
                            If CDbl(r.Item("sold_price").ToString) > 0 Then

                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, 0)
                                        CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("ASKING").Points.AddXY(temp_data.ToString, 0)
                                    CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                End If

                                If Not IsDBNull(r("ac_take_price")) Then
                                    If CDbl(r.Item("ac_take_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, 0)
                                        CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("TAKE").Points.AddXY(temp_data.ToString, 0)
                                    CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                End If
                            End If
                        End If
                        ' FILL IN BLANKS------------------------------------------------------------------------------

                        If temp_asking <> "null" Or temp_take <> "null" Or temp_sold <> "null" Then

                            'temp_data = temp_data & " - "

                            'If Trim(temp_asking) <> "null" Then
                            '    temp_data = temp_data & " Asking(" & temp_asking & ")"
                            'End If

                            'If Trim(temp_take) <> "null" Then
                            '    temp_data = temp_data & " Take(" & temp_take & ")"
                            'End If

                            'If Trim(temp_sold) <> "null" Then
                            '    temp_data = temp_data & " Est Value(" & temp_sold & ")"
                            'End If


                            '   temp_data = temp_data & vbCrLf & "My AC - Asking(" & temp_asking_my & "), Take(" & temp_take_my & "), Estimated Value(" & temp_sold_my & ")"


                            If added_row Then
                                google_map_array_list &= ",['" & temp_data & "',  " & temp_asking & ", " & temp_sold & ", " & temp_take & ", null, null, null]"       ', null, null, null
                                ' google_map_array_list &= ",['" & temp_data & "',  " & temp_asking & ", " & temp_take & ", " & temp_sold & "," & temp_asking_my & ", " & temp_take_my & ", " & temp_sold_my & "]"
                            Else
                                google_map_array_list &= "['" & temp_data & "',  " & temp_asking & ", " & temp_sold & ", " & temp_take & ", null, null, null]"    '
                                '  google_map_array_list &= "['" & temp_data & "',  " & temp_asking & ", " & temp_take & ", " & temp_sold & ", " & temp_asking_my & ", " & temp_take_my & ", " & temp_sold_my & "]"
                            End If
                            added_row = True

                            '  google_map_array_list &= ",['My AC',  null, null, null, " & temp_asking_my & ", " & temp_take_my & ", " & temp_sold_my & "]"
                        End If

                    Next

                End If
            End If

            ' google_map_array_list &= " ] "


            results_table = Nothing
            results_table2 = Nothing
            results_table3 = Nothing
            final_table = Nothing

            Call get_ranges(low_number, high_number, interval_point, starting_point)

            CHART_NAME.ChartAreas("ChartArea1").AxisY.Maximum = high_number + interval_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Interval = interval_point

            CHART_NAME.ChartAreas("ChartArea1").AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.FixedCount
            CHART_NAME.ChartAreas("ChartArea1").AxisX.Interval = 1


        Catch ex As Exception

            aError = "Error in views_analytics_graph_1(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef CHART_NAME As DataVisualization.Charting.Chart) " + ex.Message

        Finally

        End Try

    End Sub

    Public Function Get_AC_MAKE_MODEL(ByVal ac_id As Long, ByRef make_name As String, ByRef model_name As String, ByRef amod_id As Long, ByRef rest_of As String, ByRef ac_ser_no As String, Optional ByRef year_of As String = "", Optional ByRef aftt_of As String = "") As String
        Get_AC_MAKE_MODEL = ""

        Dim Query As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim fleetinfo As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            Query = "SELECT distinct amod_make_name, amod_model_name, amod_id,  ac_ser_no, ac_reg_no, ac_year, ac_airframe_tot_hrs "
            Query = Query & " FROM Aircraft WITH(NOLOCK)"
            Query = Query & " inner join aircraft_model WITH(NOLOCK) on amod_id = ac_amod_id "
            Query = Query & " WHERE ac_journ_id = 0 and ac_id = " & ac_id

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = Query.ToString
            fleetinfo = SqlCommand.ExecuteReader()

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

            If fleetinfo.HasRows Then

                Do While fleetinfo.Read
                    make_name = fleetinfo("amod_make_name")
                    model_name = fleetinfo("amod_model_name")
                    amod_id = fleetinfo("amod_id")
                    If Not IsDBNull(fleetinfo("ac_year")) Then
                        rest_of = fleetinfo("ac_year") & " "
                        year_of = fleetinfo("ac_year")
                    End If

                    If Not IsDBNull(fleetinfo("ac_airframe_tot_hrs")) Then
                        aftt_of = fleetinfo("ac_airframe_tot_hrs")
                    End If


                    rest_of = rest_of & make_name & " " & model_name

                    If Not IsDBNull(fleetinfo("ac_ser_no")) Then
                        rest_of = rest_of & " S/N " & fleetinfo("ac_ser_no")
                        ac_ser_no = fleetinfo("ac_ser_no")
                    End If



                Loop

            End If

        Catch
        Finally
            fleetinfo = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try
    End Function

    Public Function views_analytics_graph_2(ByVal ac_id As Long, ByRef CHART_NAME As DataVisualization.Charting.Chart, ByVal note_id As Long, ByVal searchCriteria As viewSelectionCriteriaClass, ByVal graph_type As String, ByRef results_table_extra As DataTable, ByRef google_map_string As String, ByVal completed_or_open As String, Optional ByVal less_details As Boolean = False, Optional ByVal RunModelOnly As Boolean = False) As String
        views_analytics_graph_2 = ""

        Dim high_number As Integer = 0
        Dim low_number As Integer = 0
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1

        Dim daysonmarket As Integer = 0
        Dim daysonmarket2 As Integer = 0
        Dim days As Integer = 0
        Dim test_string As String = ""

        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim results_table3 As New DataTable

        Dim final_table As New DataTable

        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn
        Dim column5 As New DataColumn

        Dim temp_price As Long = 0
        Dim temp_data As String = ""
        Dim temp_asking As String = "null"
        Dim temp_take As String = "null"
        Dim temp_sold As String = "null"

        Dim row_added As Boolean = False
        Dim build_table As String = ""



        If Trim(graph_type) = "TRANS" Then
            google_map_string = " data3.addColumn('string', 'Serial#'); "
            google_map_string &= " data3.addColumn('number', 'Asking'); "
            google_map_string &= " data3.addColumn('number', 'Est/Sold Value'); "
            google_map_string &= " data3.addColumn('number', 'Take'); "
            google_map_string &= " data3.addColumn('number', 'My AC Asking'); "
            google_map_string &= " data3.addColumn('number', 'My AC Est Value'); "
            google_map_string &= " data3.addColumn('number', 'My AC Take'); "
            google_map_string &= " data3.addRows(["
        Else
            ' google_map_string = "['Date', 'Asking', 'Take', 'Sold', 'My AC Asking', 'My AC Take', 'My AC Est Value']"
            google_map_string = " data6.addColumn('string', 'Serial#'); "
            google_map_string &= " data6.addColumn('number', 'Asking'); "
            google_map_string &= " data6.addColumn('number', 'Est/Sold Value'); "
            google_map_string &= " data6.addColumn('number', 'Take'); "
            google_map_string &= " data6.addColumn('number', 'My AC Asking'); "
            google_map_string &= " data6.addColumn('number', 'My AC Est Value'); "
            google_map_string &= " data6.addColumn('number', 'My AC Take'); "
            google_map_string &= " data6.addRows(["
        End If



        Try

            If Trim(completed_or_open) <> "C" Then
                If Trim(graph_type) = "TRANS" Then
                    results_table = get_my_ac_value_history_comparables(ac_id, "Current", False, 0, note_id, completed_or_open)      ' get current from current ac table
                    results_table2 = get_my_ac_comparables_transactions(ac_id, graph_type, note_id, searchCriteria)        ' get  transaction id  from comparables  - then select from client trans
                ElseIf Trim(graph_type) = "RECENT" Then
                    'RECENT RETAIL SALES
                    If RunModelOnly = False Then
                        results_table = get_my_ac_value_history_comparables(ac_id, "Current", False, 0, note_id, completed_or_open)      ' get current from current ac table  
                    End If
                End If
            Else
                'If Trim(graph_type) = "TRANS" Then
                results_table = get_my_ac_value_history_comparables(ac_id, "comparable", False, 0, note_id, completed_or_open)
                results_table2 = get_my_ac_comparables_transactions(ac_id, graph_type, note_id, searchCriteria)
                'ElseIf Trim(graph_type) = "RECENT" Then
                '    results_table = get_my_ac_value_history_comparables(ac_id, "comparable", False, 0, note_id, completed_or_open)
                ' End If

            End If

            '   For Each drRow As DataRow In results_table.Rows
            'final_table.ImportRow(drRow)
            '   Next
            '  CHART_NAME.Series.Clear()
            '    CHART_NAME.ChartAreas("ChartArea1").AxisY.Title = "Aircraft Value ($k)"

            'CHART_NAME.Series.Add("ASKING_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            'CHART_NAME.Series("ASKING_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            'CHART_NAME.Series("ASKING_CURRENT").LabelForeColor = Drawing.Color.Blue
            'CHART_NAME.Series("ASKING_CURRENT").Color = Drawing.Color.Blue
            'CHART_NAME.Series("ASKING_CURRENT").BorderWidth = 1
            'CHART_NAME.Series("ASKING_CURRENT").MarkerSize = 6
            'CHART_NAME.Series("ASKING_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            'CHART_NAME.Series("ASKING_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

            'CHART_NAME.Series.Add("TAKE_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            'CHART_NAME.Series("TAKE_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            'CHART_NAME.Series("TAKE_CURRENT").LabelForeColor = Drawing.Color.Red
            'CHART_NAME.Series("TAKE_CURRENT").Color = Drawing.Color.Red
            'CHART_NAME.Series("TAKE_CURRENT").BorderWidth = 1
            'CHART_NAME.Series("TAKE_CURRENT").MarkerSize = 6
            'CHART_NAME.Series("TAKE_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            'CHART_NAME.Series("TAKE_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            'CHART_NAME.Series.Add("SOLD_CURRENT").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            'CHART_NAME.Series("SOLD_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            'CHART_NAME.Series("SOLD_CURRENT").LabelForeColor = Drawing.Color.Green
            'CHART_NAME.Series("SOLD_CURRENT").Color = Drawing.Color.Green
            'CHART_NAME.Series("SOLD_CURRENT").BorderWidth = 1
            'CHART_NAME.Series("SOLD_CURRENT").MarkerSize = 6
            'CHART_NAME.Series("SOLD_CURRENT").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Cross
            'CHART_NAME.Series("SOLD_CURRENT").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            'CHART_NAME.Width = 300
            'CHART_NAME.Height = 300


            'If Not IsNothing(results_table) Then

            '    If results_table.Rows.Count > 0 Then

            '        For Each r As DataRow In results_table.Rows



            '            If Not IsDBNull(r("asking_price")) Then
            '                If CDbl(r.Item("asking_price").ToString) > 0 Then


            '                    temp_price = CDbl(r.Item("asking_price").ToString)
            '                    temp_price = (temp_price / 1000)
            '                    temp_asking = temp_price

            '                    If high_number = 0 Or CDbl(temp_price) > high_number Then
            '                        high_number = CDbl(temp_price)
            '                    End If

            '                    If low_number = 0 Or CDbl(temp_price) < low_number Then
            '                        low_number = CDbl(temp_price)
            '                    End If
            '                    CHART_NAME.Series("ASKING_CURRENT").Points.AddXY(CDate(temp_data).Date, temp_price)
            '                End If
            '            End If


            '            If Not IsDBNull(r("take_price")) Then
            '                If CDbl(r.Item("take_price").ToString) > 0 Then

            '                    temp_price = CDbl(r.Item("take_price").ToString)
            '                    temp_price = (temp_price / 1000)
            '                    temp_take = temp_price

            '                    If high_number = 0 Or CDbl(temp_price) > high_number Then
            '                        high_number = CDbl(temp_price)
            '                    End If

            '                    If low_number = 0 Or CDbl(temp_price) < low_number Then
            '                        low_number = CDbl(temp_price)
            '                    End If
            '                    CHART_NAME.Series("TAKE_CURRENT").Points.AddXY(CDate(temp_data).Date, temp_price)
            '                End If
            '            End If


            '            If Not IsDBNull(r("sold_price")) Then
            '                If CDbl(r.Item("sold_price").ToString) > 0 Then


            '                    temp_price = CDbl(r.Item("sold_price").ToString)
            '                    temp_price = (temp_price / 1000)
            '                    temp_sold = temp_price

            '                    If high_number = 0 Or CDbl(temp_price) > high_number Then
            '                        high_number = CDbl(temp_price)
            '                    End If

            '                    If low_number = 0 Or CDbl(temp_price) < low_number Then
            '                        low_number = CDbl(temp_price)
            '                    End If
            '                    CHART_NAME.Series("SOLD_CURRENT").Points.AddXY(CDate(temp_data).Date, temp_price)
            '                End If
            '            End If

            '            If temp_asking <> "null" Or temp_take <> "null" Or temp_sold <> "null" Then
            '                google_map_string &= ",['" & temp_data & "',  " & temp_asking & ", " & temp_take & ", " & temp_sold & "]"
            '            End If


            '        Next

            '    End If
            'End If











            'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
            column.DataType = System.Type.GetType("System.Double")
            column.DefaultValue = 0
            column.Unique = False
            column.ColumnName = "asking_price"
            final_table.Columns.Add(column)

            column2.DataType = System.Type.GetType("System.Double")
            column2.DefaultValue = 0
            column2.Unique = False
            column2.ColumnName = "take_price"
            final_table.Columns.Add(column2)

            column3.DataType = System.Type.GetType("System.Double")
            column3.DefaultValue = 0
            column3.AllowDBNull = True
            column3.Unique = False
            column3.ColumnName = "sold_price"
            final_table.Columns.Add(column3)



            column4.DataType = System.Type.GetType("System.DateTime")
            column4.AllowDBNull = True
            column4.Unique = False
            column4.ColumnName = "date_of"
            final_table.Columns.Add(column4)

            column5.DataType = System.Type.GetType("System.String")
            column5.AllowDBNull = True
            column5.Unique = False
            column5.ColumnName = "ac_details"
            final_table.Columns.Add(column5)


            For Each drRow As DataRow In results_table.Rows
                final_table.ImportRow(drRow)
            Next


            If Not IsNothing(results_table2) Then
                For Each drRow As DataRow In results_table2.Rows
                    final_table.ImportRow(drRow)
                Next
            End If

            If Not IsNothing(results_table_extra) Then
                For Each drRow As DataRow In results_table_extra.Rows
                    final_table.ImportRow(drRow)
                Next
            End If





            Dim Filtered_DV As New DataView(final_table)


            For Each drv As DataRowView In Filtered_DV
                Console.WriteLine(vbTab & " {0}", drv("date_of"))
            Next


            Console.WriteLine("<br><br>")

            Filtered_DV.Sort = "date_of asc"
            final_table = Filtered_DV.ToTable

            For Each drv As DataRowView In Filtered_DV
                Console.WriteLine(vbTab & " {0}", drv("date_of"))
            Next


            ' Dim afiltered_Jetnet As DataRow() = final_table.Select("", "date_of asc")


            CHART_NAME.Series.Add("ASKING").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("ASKING").LabelForeColor = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").Color = Drawing.Color.Blue
            CHART_NAME.Series("ASKING").BorderWidth = 1
            CHART_NAME.Series("ASKING").MarkerSize = 5
            CHART_NAME.Series("ASKING").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("ASKING").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

            CHART_NAME.Series.Add("TAKE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("TAKE").LabelForeColor = Drawing.Color.Red
            CHART_NAME.Series("TAKE").Color = Drawing.Color.Red
            CHART_NAME.Series("TAKE").BorderWidth = 1
            CHART_NAME.Series("TAKE").MarkerSize = 5
            CHART_NAME.Series("TAKE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("TAKE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            CHART_NAME.Series.Add("SOLD").ChartType = UI.DataVisualization.Charting.SeriesChartType.Point
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            CHART_NAME.Series("SOLD").LabelForeColor = Drawing.Color.Green
            CHART_NAME.Series("SOLD").Color = Drawing.Color.Green
            CHART_NAME.Series("SOLD").BorderWidth = 1
            CHART_NAME.Series("SOLD").MarkerSize = 5
            CHART_NAME.Series("SOLD").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            CHART_NAME.Series("SOLD").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32



            If Not IsNothing(final_table) Then

                If final_table.Rows.Count > 0 Then

                    build_table = "<table border='1' cellpadding='3' cellspacing='0'>"
                    build_table &= "<tr class='header_text'><td align='left'><font size='-2'><b>Date</b></font></td><td align='left'><font size='-2'><b>Aircraft</b></td><td align='left'><font size='-2'><b>Asking $</b></td><td align='left'><font size='-2'><b>Take	$</b></td><td align='left'><font size='-2'><b>Estimated Value $</b></font></td></tr>"


                    For Each r As DataRow In final_table.Rows

                        temp_asking = "null"
                        temp_take = "null"
                        temp_sold = "null"

                        temp_data = CDate(r.Item("date_of").ToString).Date

                        If less_details = True Then
                        Else
                            If Not IsDBNull(r.Item("ac_details")) Then
                                temp_data = temp_data & " (" & r.Item("ac_details") & ") "
                            End If
                        End If


                        If Not IsDBNull(r("asking_price")) Then
                            If CDbl(r.Item("asking_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("asking_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_asking = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If
                                CHART_NAME.Series("ASKING").Points.AddXY(temp_data, temp_price)
                            End If
                        End If


                        If Not IsDBNull(r("take_price")) Then
                            If CDbl(r.Item("take_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("take_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_take = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If
                                CHART_NAME.Series("TAKE").Points.AddXY(temp_data, temp_price)
                            End If
                        End If


                        If Not IsDBNull(r("sold_price")) Then
                            If CDbl(r.Item("sold_price").ToString) > 0 Then

                                temp_price = CDbl(r.Item("sold_price").ToString)
                                temp_price = (temp_price / 1000)
                                temp_sold = temp_price

                                If high_number = 0 Or CDbl(temp_price) > high_number Then
                                    high_number = CDbl(temp_price)
                                End If

                                If low_number = 0 Or CDbl(temp_price) < low_number Then
                                    low_number = CDbl(temp_price)
                                End If
                                CHART_NAME.Series("SOLD").Points.AddXY(temp_data, temp_price)
                            End If
                        Else

                        End If


                        ' FILL IN BLANKS------------------------------------------------------------------------------
                        If Not IsDBNull(r("asking_price")) Then
                            If CDbl(r.Item("asking_price").ToString) > 0 Then
                                If Not IsDBNull(r("take_price")) Then
                                    If CDbl(r.Item("take_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("TAKE").Points.AddXY(temp_data, 0)
                                        CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("TAKE").Points.AddXY(temp_data, 0)
                                    CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                End If

                                If Not IsDBNull(r("sold_price")) Then
                                    If CDbl(r.Item("sold_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("SOLD").Points.AddXY(temp_data, 0)
                                        CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("SOLD").Points.AddXY(temp_data, 0)
                                    CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                End If
                            End If
                        End If


                        If Not IsDBNull(r("take_price")) Then
                            If CDbl(r.Item("take_price").ToString) > 0 Then

                                If Not IsDBNull(r("asking_price")) Then
                                    If CDbl(r.Item("asking_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("ASKING").Points.AddXY(temp_data, 0)
                                        CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("ASKING").Points.AddXY(temp_data, 0)
                                    CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                End If

                                If Not IsDBNull(r("sold_price")) Then
                                    If CDbl(r.Item("sold_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("SOLD").Points.AddXY(temp_data, 0)
                                        CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("SOLD").Points.AddXY(temp_data, 0)
                                    CHART_NAME.Series("SOLD").Points.Item(CHART_NAME.Series("SOLD").Points.Count - 1).IsEmpty = True
                                End If
                            End If
                        End If



                        If Not IsDBNull(r("sold_price")) Then
                            If CDbl(r.Item("sold_price").ToString) > 0 Then

                                If Not IsDBNull(r("asking_price")) Then
                                    If CDbl(r.Item("asking_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("ASKING").Points.AddXY(temp_data, 0)
                                        CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("ASKING").Points.AddXY(temp_data, 0)
                                    CHART_NAME.Series("ASKING").Points.Item(CHART_NAME.Series("ASKING").Points.Count - 1).IsEmpty = True
                                End If

                                If Not IsDBNull(r("take_price")) Then
                                    If CDbl(r.Item("take_price").ToString) > 0 Then
                                    Else
                                        CHART_NAME.Series("TAKE").Points.AddXY(temp_data, 0)
                                        CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                    End If
                                Else
                                    CHART_NAME.Series("TAKE").Points.AddXY(temp_data, 0)
                                    CHART_NAME.Series("TAKE").Points.Item(CHART_NAME.Series("TAKE").Points.Count - 1).IsEmpty = True
                                End If
                            End If
                        End If
                        ' FILL IN BLANKS------------------------------------------------------------------------------



                        If Trim(CDate(r.Item("date_of").ToString).Date) = Trim(Date.Now.Date) Then
                            If row_added Then
                                google_map_string &= ",['" & temp_data & "', null, null, null,  " & temp_asking & ", " & temp_sold & ", " & temp_take & "]"
                            Else
                                google_map_string &= "['" & temp_data & "', null, null, null,  " & temp_asking & ", " & temp_sold & ", " & temp_take & "]"
                            End If
                            row_added = True
                        Else
                            If row_added Then
                                google_map_string &= ",['" & temp_data & "',  " & temp_asking & ", " & temp_sold & ", " & temp_take & ", null, null, null]"
                            Else
                                google_map_string &= "['" & temp_data & "',  " & temp_asking & ", " & temp_sold & ", " & temp_take & ", null, null, null]"
                            End If
                            row_added = True
                        End If


                        build_table &= "<tr class='small_header_text'>"
                        build_table &= "<td align='left'><font size='-2'>" & CDate(r.Item("date_of").ToString).Date & "&nbsp;</font></td>"
                        build_table &= "<td align='left'><font size='-2'>" & r.Item("ac_details") & "&nbsp;</font></td>"
                        If Trim(temp_asking) <> "null" Then
                            build_table &= "<td align='right'><font size='-2'>$" & temp_asking & "k&nbsp;</font></td>"
                        Else
                            build_table &= "<td align='right'><font size='-2'>&nbsp;</font></td>"
                        End If

                        If Trim(temp_take) <> "null" Then
                            build_table &= "<td align='right'><font size='-2'>$" & temp_take & "k&nbsp;</font></td>"
                        Else
                            build_table &= "<td align='right'><font size='-2'>&nbsp;</font></td>"
                        End If


                        If Trim(temp_sold) <> "null" Then
                            build_table &= "<td align='right'><font size='-2'>$" & temp_sold & "k&nbsp;</font></td>"
                        Else
                            build_table &= "<td align='right'><font size='-2'>&nbsp;</font></td>"
                        End If

                        build_table &= "</tr>"



                        row_added = True

                    Next

                    build_table &= "</table>"

                End If
            End If


            results_table = Nothing
            results_table2 = Nothing
            results_table3 = Nothing
            final_table = Nothing

            Call get_ranges(low_number, high_number, interval_point, starting_point)

            CHART_NAME.ChartAreas("ChartArea1").AxisY.Maximum = high_number + interval_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
            CHART_NAME.ChartAreas("ChartArea1").AxisY.Interval = interval_point

            views_analytics_graph_2 = build_table


        Catch ex As Exception

            aError = "Error in views_analytics_graph_1(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef CHART_NAME As DataVisualization.Charting.Chart) " + ex.Message

        Finally

        End Try

    End Function

    Function AIRCRAFT_SUMMARY_SortLabelsValue(ByRef labels, ByRef data, ByVal count, ByVal direction) ' 1 direction is label is used for sort 2 is that data is 
        AIRCRAFT_SUMMARY_SortLabelsValue = ""
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim temp_string As String = ""
        Dim temp_int As Integer = 0
        Dim temp_int_array(count) As Double


        For i = 0 To count
            If direction = 1 Then
                temp_int_array(i) = CDbl(labels(i))
            Else
                temp_int_array(i) = data(i)
            End If
        Next


        For j = 0 To count - 1
            For i = 0 To count - 1
                If temp_int_array(i) > temp_int_array(i + 1) Then

                    temp_int = data(i + 1)
                    data(i + 1) = data(i)
                    data(i) = temp_int

                    temp_string = labels(i + 1)
                    labels(i + 1) = labels(i)
                    labels(i) = temp_string

                    temp_int = temp_int_array(i + 1)
                    temp_int_array(i + 1) = temp_int_array(i)
                    temp_int_array(i) = temp_int

                End If
            Next
        Next

        For j = 0 To count - 1
            If labels(j) = Nothing Then
                labels(j) = ""
            End If
        Next


    End Function

    Function AIRCRAFT_SUMMARY_CreateAndGraphData(ByVal strTitle,
                                 ByVal strBottomTitle,
                                 ByVal strLeftTitle,
                                 ByVal lGraphType,
                                 ByRef aLabels,
                                 ByRef aData,
                                 ByVal lDivBy,
                                 ByVal strXFormatString,
                                 ByVal strYFormatString,
                                 ByVal current_min,
                                 ByVal current_max,
                                   ByVal color, ByRef SPI_QUARTER, Optional ByRef google_string = "") As String
        AIRCRAFT_SUMMARY_CreateAndGraphData = ""
        Dim series_title As String = ""
        Dim text_legend As String = ""
        Dim temp_color As String = ""
        Dim row_added As Boolean = False

        series_title = strTitle ' for the series to be consistent, gets replaced later on 



        If lGraphType = 1 Then '  1=2D-Pie
            SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Pie
        ElseIf lGraphType = 2 Then '  2=3D Pie
            SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Pie
            'rotate
        ElseIf lGraphType = 3 Then '  3=2D Bar
            SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Column
        ElseIf lGraphType = 4 Then '  4=3D Bar
            SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Column
            'rotate
        ElseIf lGraphType = 5 Then  '  5=Gantt
            '   SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.
        ElseIf lGraphType = 6 Then '  6=Line 
            SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Line
        ElseIf lGraphType = 7 Then '  7=Log/lin 
            SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Line

        ElseIf lGraphType = 8 Then '  8=2D Area 
            SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Area
        End If



        SPI_QUARTER.ChartAreas("ChartArea1").AxisY.Title = strLeftTitle  ' passed in 
        SPI_QUARTER.ChartAreas("ChartArea1").AxisX.Title = strBottomTitle



        SPI_QUARTER.Series(series_title).YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
        SPI_QUARTER.Series(series_title).XValueType = UI.DataVisualization.Charting.ChartValueType.String



        '-------------------------------------------------
        '  Graph Types
        '  1=2D-Pie
        '  2=3D Pie
        '  3=2D Bar
        '  4=3D Bar
        '  5=Gantt
        '  6=Line 
        '  7=Log/lin 
        '  8=2D Area 
        '  9=2D Scatter
        ' 10=Polar/Circle Line
        ' 11=High-low-close
        ' 12=Bubble
        ' 13=3D Ribbon 
        ' 14=3D Area 
        ' 15=Log/log
        ' 16=Lin/log
        ' 17=Box-whisker +/- Bar 
        ' 18=Open-high-low-close
        ' 19=Candlestick
        ' 20=3D Survace
        ' 21=3D Scatter

        '
        '
        '
        '
        '
        '  Y Axis (Left)
        '    .
        '   .
        '  .
        ' .               X Axis (Bottom)
        '----------------------------------


        If lGraphType = 7 Then

            SPI_QUARTER.Series(series_title).BorderWidth = 2


            If Right(series_title, 1) = ";" Then

                If color = "Blue" Then
                    SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Blue
                    SPI_QUARTER.Series(series_title).Color = Drawing.Color.Blue
                ElseIf color = "Navy" Then
                    SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Navy
                    SPI_QUARTER.Series(series_title).Color = Drawing.Color.Navy
                ElseIf color = "Light Gray" Then
                    SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.LightSlateGray
                    SPI_QUARTER.Series(series_title).Color = Drawing.Color.LightSlateGray
                ElseIf color = "Gray" Then
                    SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Gray
                    SPI_QUARTER.Series(series_title).Color = Drawing.Color.Gray
                ElseIf color = "Dark Gray" Then
                    SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.DarkSlateGray
                    SPI_QUARTER.Series(series_title).Color = Drawing.Color.DarkSlateGray
                ElseIf color = "Black" Then
                    SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Black
                    SPI_QUARTER.Series(series_title).Color = Drawing.Color.Black
                Else
                    SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Blue ' set to blue for default 
                    SPI_QUARTER.Series(series_title).Color = Drawing.Color.Blue
                End If

                If color = "Blue" Then
                    temp_color = "Blue"
                ElseIf color = "Navy" Then
                    temp_color = "#0000A0"
                ElseIf color = "Light Gray" Then
                    temp_color = "#A0A0A0"
                ElseIf color = "Gray" Then
                    temp_color = "Gray"
                ElseIf color = "Dark Gray" Then
                    temp_color = "#25383C"
                ElseIf color = "Black" Then
                    temp_color = "Black"
                Else
                    temp_color = "Blue"
                End If


                text_legend = "<tr valign='top'><td><table align='center' valign='top' width='50%' border='1'>"
                text_legend += "<tr><td align='left' bgcolor='red'><font size='-2' color='white'>Avg Selling Price:</font></td><td align='left' bgcolor='red'> <font size='-2' color='white'>Red</font></td></tr>"
                text_legend += "<tr><td align='left' bgcolor='" & temp_color & "'><font size='-2' color='white'>Avg Asking Price:</font></td><td align='left' bgcolor='" & temp_color & "'> <font size='-2' color='white'>" & color & "</font></td></tr>"
                text_legend += "</table></td></tr>"
            Else

                SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Red  ' set to blue for default 
                SPI_QUARTER.Series(series_title).Color = Drawing.Color.Red


            End If





        Else
            If color = "Blue" Then
                SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Blue
                SPI_QUARTER.Series(series_title).Color = Drawing.Color.Blue
            ElseIf color = "Navy" Then
                SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Navy
                SPI_QUARTER.Series(series_title).Color = Drawing.Color.Navy
            ElseIf color = "Light Gray" Then
                SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.LightSlateGray
                SPI_QUARTER.Series(series_title).Color = Drawing.Color.LightSlateGray
            ElseIf color = "Gray" Then
                SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Gray
                SPI_QUARTER.Series(series_title).Color = Drawing.Color.Gray
            ElseIf color = "Dark Gray" Then
                SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.DarkSlateGray
                SPI_QUARTER.Series(series_title).Color = Drawing.Color.DarkSlateGray
            ElseIf color = "Black" Then
                SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Black
                SPI_QUARTER.Series(series_title).Color = Drawing.Color.Black
            Else
                SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Blue ' set to blue for default 
                SPI_QUARTER.Series(series_title).Color = Drawing.Color.Blue
            End If
        End If
        '--------------------------
        ' Color
        '  1=Dark Red
        '  2=Red
        '  3=Green
        '  4=Purple
        '  5=Dk Blue
        '  6=Blue
        '  7=Lt Grey
        '  8=Dk Grey
        '  9=Lt Purple
        ' 10=Lt Bue
        ' 11=Lt Green
        ' 12=Lime Green
        ' 13=Yellow
        ' 14=Gold
        ' 15=White 
        ' 16=Blank




        Dim strGraphPath As String = ""
        Dim strGraphURL As String = ""
        Dim lData As Integer = 0
        Dim strLabel As String = ""
        Dim temp_string_start As String = ""
        Dim temp_string_end As String = ""
        Dim temp_string_spot As Integer = 0

        Dim lMax As Double = 0
        Dim lMin As Double = 0

        Dim lCnt1 As Integer = 0

        Dim strHTML As String = ""

        Dim strImageWidth As Integer = 0
        Dim strImageHeight As Integer = 0

        Dim lWidth As Integer = 250
        Dim lHeight As Integer = 250



        ' If CLng(Session("SPGraphWidth")) > 0 Then
        'lWidth = CInt(Session("SPGraphWidth"))
        ' End If

        'If CLng(Session("SPGraphHeight")) > 0 Then
        ' lHeight = CInt(Session("SPGraphHeight"))
        'End If

        strHTML = ""

        If UBound(aData) >= 1 Then '  And Not Session("localMachine")


            If UBound(aData) > 1 Then
                If (UBound(aData) / 2) > 100 Then
                    '   objGraph.YAxisTicks = 100
                Else
                    '  objGraph.YAxisTicks = (UBound(aData) / 2)
                End If
            Else
                '  objGraph.YAxisTicks = 1
            End If


            If current_min > 0 Or current_max > 0 Then
                lMin = current_min
                lMax = current_max
            Else
                lMin = 999999999
                lMax = -999999999
            End If




            For lCnt1 = 1 To UBound(aData)

                If Not IsNothing(aData(lCnt1 - 1)) Then

                    SPI_QUARTER.Series(series_title).Points.AddXY(aLabels(lCnt1 - 1), aData(lCnt1 - 1))


                    If Trim(series_title) = "BEECHJET/400A - Asking vs Selling Price (k)" Then

                        aData(lCnt1 - 1) = Replace(FormatNumber(aData(lCnt1 - 1), 1), ",", "")

                        If row_added Then
                            google_string &= ",['" & aLabels(lCnt1 - 1) & "'," & aData(lCnt1 - 1) & ", null, null, null, null, null ]"
                        Else
                            google_string &= "['" & aLabels(lCnt1 - 1) & "'," & aData(lCnt1 - 1) & ", null, null, null, null, null ]"
                        End If
                        row_added = True
                    ElseIf Trim(series_title) = "BEECHJET/400A - Asking vs Selling Price (k)&nbsp;" Then


                        temp_string_spot = InStr(google_string, aLabels(lCnt1 - 1) & "',")

                        If temp_string_spot > 0 Then
                            temp_string_start = Left(google_string, temp_string_spot + (Len(aLabels(lCnt1 - 1)) + 2) - 1) ' + 2 is for tick comma 
                            temp_string_end = Right(google_string, Len(google_string) - temp_string_spot - (Len(aLabels(lCnt1 - 1)) + 2) + 1)

                            temp_string_spot = InStr(temp_string_end, ", null, null,") ' 13 spots 
                            temp_string_start &= Left(temp_string_end, temp_string_spot - 1)  ' get all to the left, add to left string 
                            temp_string_end = Right(temp_string_end, Len(temp_string_end) - temp_string_spot - 13) ' for 13 spots


                            aData(lCnt1 - 1) = Replace(FormatNumber(aData(lCnt1 - 1), 1), ",", "")

                            google_string = temp_string_start & ", null, " & aData(lCnt1 - 1) & "," & temp_string_end

                        End If

                    End If

                    '  strLabel = aLabels(lCnt1 - 1)
                    '  objGraph.Label(lCnt1) = strLabel & "   "

                    lData = CDbl(aData(lCnt1 - 1))

                    If lDivBy > 0 Then
                        lData = lData \ lDivBy
                    End If



                    If lData > lMax Then
                        lMax = lData
                    End If
                    If lData < lMin Then
                        lMin = lData
                    End If
                End If
            Next ' lCnt1




            Dim temp_counter As Integer = 0
            Dim temp_ten As Double = 10
            Dim temp_max As Double = 0
            Dim temp_min As Integer = 0
            Dim i As Integer = 1
            Dim label_count As Integer = 0
            Dim found As Integer = 0
            Dim found2 As Integer = 0
            label_count = UBound(aLabels)

            If lMax <= 0 Then
                temp_counter = -10
            ElseIf lMax <= 100 Then ' 100 
                temp_counter = 10
            ElseIf lMax <= 1000 Then  ' 1 thousand 
                temp_counter = 100
            ElseIf lMax <= 10000 Then ' ten thousand 
                temp_counter = 1000
            ElseIf lMax <= 100000 Then  ' 100 thousand 
                temp_counter = 10000
            ElseIf lMax <= 1000000 Then ' 1 mill 
                temp_counter = 100000
            ElseIf lMax <= 10000000 Then ' ten mill 
                temp_counter = 1000000
            ElseIf lMax <= 100000000 Then ' one hundred mill
                temp_counter = 10000000
            End If

            temp_max = lMax
            If lMax >= 0 Then
                For i = 1 To 20
                    If lMax >= (temp_counter * i) Then
                        temp_max = i + 1
                    Else
                        i = 20
                    End If
                Next
            Else
                For i = 1 To 20
                    If lMax >= (temp_counter * i * -1) Then
                        temp_max = (i - 1) * -1
                        i = 20
                    End If

                Next
            End If

            temp_min = lMin

            If lMin >= 0 Then
                For i = 1 To 20
                    If lMin <= (temp_counter * i) Then
                        temp_min = i - 1
                        i = 20
                    End If
                Next
            Else
                For i = 1 To 20
                    If lMin >= (temp_counter * i * -1) Then
                        temp_min = (i) * -1
                        i = 20
                    End If

                Next
            End If




            ' even more precision 
            If lMax >= 0 Then
                For i = 1 To 10
                    If (temp_counter + (i * (temp_counter / 10))) >= lMax Then
                        found = (temp_counter + (i * (temp_counter / 10)))
                        i = 10
                    End If
                Next
            End If


            If lMin < 0 Then
                For i = 1 To 10
                    If ((temp_counter * -1) - (i * (temp_counter / 10))) <= lMin Then
                        found2 = ((temp_counter * -1) - (i * (temp_counter / 10)))
                        i = 10
                    End If
                Next
            End If




            ' 10,000    *       2 =         20,000
            If found > 0 Then
                lMax = found
            Else
                lMax = (temp_counter * temp_max)
            End If

            If found2 < 0 Then
                lMin = found2
            Else
                lMin = (temp_counter * temp_min)
            End If


            'If Me.S_3D.Checked = True Then
            '    SPI_QUARTER.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
            '    'SPI_QUARTER.ChartAreas("ChartArea1").Area3DStyle.Perspective = 5
            '    SPI_QUARTER.ChartAreas("ChartArea1").Area3DStyle.Rotation = 10
            'Else
            SPI_QUARTER.Series(series_title).MarkerSize = 5
            SPI_QUARTER.Series(series_title).MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            ' End If




            SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelStyle.Angle = -90
            SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelStyle.Interval = 1
            SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelStyle.IsEndLabelVisible = False
            SPI_QUARTER.ChartAreas("ChartArea1").AxisX.IsLabelAutoFit = True
            '  SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelAutoFitStyle = DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
            ' SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelAutoFitMinFontSize = 5
            'SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelAutoFitMaxFontSize = 5







            SPI_QUARTER.ChartAreas("ChartArea1").AxisY.Minimum = lMin
            SPI_QUARTER.ChartAreas("ChartArea1").AxisY.Maximum = lMax
            '   SPI_QUARTER.ChartAreas("ChartArea1").Area3DStyle.Rotation = 100



            '        If lGraphType = 7 Then
            ' If Right(series_title, 1) = ";" Then
            ' SPI_QUARTER.Series(series_title).Points.FindMinByValue.Label = "Avg Selling Prices"
            ' Else
            '     SPI_QUARTER.Series(series_title).Points.FindMaxByValue.Label = "Avg Asking Prices"
            ' End If
            ' End If




            'If CLng(Session("SPImageWidth")) > 0 Then
            '    strImageWidth = "width='" & CStr(Session("SPImageWidth")) & "' "
            'End If

            ''If Clng(Session("SPImageHeight")) > 0 Then
            ''  strImageHeight="height='" & CStr(Session("SPImageHeight")) & "' "
            ''End If
            'If Me.WD.SelectedValue = "Word" Then
            SPI_QUARTER.Width = 250
            SPI_QUARTER.Height = 250
            'End If



        End If ' If UBound(aData) >= 1 Then

        If text_legend.ToString.Trim <> "" Then
            AIRCRAFT_SUMMARY_CreateAndGraphData = text_legend.ToString.Trim    ' if there is a legend ( for the one line graph) return the legend 
        Else
            AIRCRAFT_SUMMARY_CreateAndGraphData = lMin & "," & lMax
        End If


    End Function ' CreateAndGraphData

    Public Sub views_display_performance_specs(ByVal bisReport As Boolean, ByVal optFormat As String, ByVal bShowLabelColumn As Boolean, ByVal bHasManyAirFrames As Boolean,
                                               ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByRef sharedModelTable As DataTable = Nothing)

        Dim htmlOut As New StringBuilder
        'Dim results_table As New DataTable
        Dim nNumberOfEngines As Integer = 0

        Dim TailBlade As Integer = 0
        Dim MainRotor1Blade As Integer = 0
        Dim MainRotor2Blade As Integer = 0
        Dim TailBladeDiameter As Double = 0
        Dim MainRotor1BladeDiameter As Double = 0
        Dim MainRotor2BladeDiameter As Double = 0
        Dim AntiTorq As String = ""
        Dim highest_engines_count As Integer = 0
        Dim temp_on_condition As String = ""


        Try

            If IsNothing(sharedModelTable) Then
                sharedModelTable = commonEvo.get_view_model_info(searchCriteria, True)
            End If

            ' added in msw 7/28/15
            bHasManyAirFrames = commonEvo.check_for_multi_airframes(sharedModelTable)


            If Not IsNothing(sharedModelTable) Then
                If sharedModelTable.Rows.Count > 0 Then
                    For Each r As DataRow In sharedModelTable.Rows

                        nNumberOfEngines = 0
                        Call commonEvo.GetEngines_For_Spaces(r.Item("amod_id").ToString, nNumberOfEngines, searchCriteria.ViewCriteriaAircraftID)

                        If nNumberOfEngines > highest_engines_count Then
                            highest_engines_count = nNumberOfEngines
                        End If

                    Next
                End If
            End If



            If Not IsNothing(sharedModelTable) Then
                If sharedModelTable.Rows.Count > 0 Then

                    If bShowLabelColumn Then

                        searchCriteria.ViewCriteriaAirframeTypeStr = sharedModelTable.Rows(0).Item("amod_airframe_type_code").ToString.ToUpper

                        If bisReport Then
                            htmlOut.Append("<td valign=""top"" align=""left"" style=""text-align: left; width:155px; margin-left:3px;"">")
                            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"">")
                        Else
                            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                                htmlOut.Append("<td valign=""top"" align=""left"" style=""text-align: left; margin-left:3px;"">")
                            Else
                                htmlOut.Append("<td valign=""top"" align=""left"" style=""text-align: left; width:155px; margin-left:3px;"">")
                            End If

                            htmlOut.Append("<table cellpadding=""0"" cellspacing=""0"">")
                        End If

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'><td nowrap='nowrap'><b>MODEL&nbsp;NAME</b></td></tr>")
                        Else
                            If sharedModelTable.Rows.Count > 0 Then
                                htmlOut.Append("<tr class='performance_header_row'><td>&nbsp;</td></tr>")
                            End If
                        End If

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Fuselage&nbsp;Dimensions</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Length&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Height&nbsp;:</td></tr><tr>")

                        'commented out/changed - msw - down below shows if fixed or rotory, many airframes doesnt matter 
                        'If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                        '  htmlOut.Append("<td class='Label' valign='middle' align='right'>Wing&nbsp;Span&nbsp;:</td></tr>")
                        'ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                        '  htmlOut.Append("<td class='Label' valign='middle' align='right'>Width&nbsp;:</td></tr>")
                        'ElseIf bHasManyAirFrames Then
                        '  htmlOut.Append("<td class='Label' valign='middle' align='right'>[F]&nbsp;Wing&nbsp;Span&nbsp;/&nbsp;[R]&nbsp;Width&nbsp;:</td></tr>")
                        'End If

                        If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") Then
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Wing&nbsp;Span&nbsp;:</td></tr>")
                        ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") Then
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Width&nbsp;:</td></tr>")
                        Else
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Width&nbsp;:</td></tr>")
                        End If

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Cabin&nbsp;Dimensions</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Length&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Height&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Width&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Cabin&nbsp;Volume&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Baggage&nbsp;Volume&nbsp;:</td></tr>")

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Typical&nbsp;Configuration</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Crew&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Passengers&nbsp;:</td></tr>")

                        If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                            htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>Pressurization&nbsp;:</td></tr>")
                        ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                            ' do nothing
                        ElseIf bHasManyAirFrames Then
                            htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>[F]&nbsp;Pressurization&nbsp;:</td></tr>")
                        End If

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Fuel&nbsp;Capacity</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;:</td></tr>")

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Weight</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Ramp&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Takeoff&nbsp;:</td></tr><tr>")

                        If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                            htmlOut.Append("<td nowrap='nowrap' class='Label' valign='middle' align='right'>Zero&nbsp;Fuel&nbsp;:</td></tr><tr>")
                        ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                            If HttpContext.Current.Session.Item("isMobile") Then
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Empty Operating Weight<br />(EOW):</td></tr><tr>")
                            Else
                                htmlOut.Append("<td nowrap='nowrap' class='Label' valign='middle' align='right'>Empty&nbsp;Operating&nbsp;Weight&nbsp;(EOW)&nbsp;:</td></tr><tr>")
                            End If
                        ElseIf bHasManyAirFrames Then
                            htmlOut.Append("<td nowrap='nowrap' class='Label' valign='middle' align='right'>[F]&nbsp;Zero&nbsp;Fuel&nbsp;/&nbsp;[R]&nbsp;Empty&nbsp;Operating&nbsp;Weight&nbsp;:</td></tr><tr>")
                        End If
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Basic&nbsp;Operating&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Landing&nbsp;:</td></tr>")

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Speed</b></td></tr><tr>")

                        If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Vs&nbsp;Clean&nbsp;:</td></tr><tr>")
                        ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                            ' do nothing
                        ElseIf bHasManyAirFrames Then
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>[F]&nbsp;Vs&nbsp;Clean&nbsp;:</td></tr><tr>")
                        End If

                        If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Vso&nbsp;Landing&nbsp;:</td></tr><tr>")
                        ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                            ' do nothing
                        ElseIf bHasManyAirFrames Then
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>[F]&nbsp;Vso&nbsp;Landing&nbsp;:</td></tr><tr>")
                        End If

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Normal&nbsp;Cruise&nbsp;TAS&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Vmo&nbsp;(Max&nbsp;Op)&nbsp;IAS&nbsp;:</td></tr><tr>")

                        If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                            'do nothing
                        ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Vne&nbsp;:</td></tr><tr>")
                        ElseIf bHasManyAirFrames Then
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>[R]&nbsp;Vne&nbsp;:</td></tr><tr>")
                        End If

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>V1&nbsp;Takeoff&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>VFE&nbsp;Max&nbsp;Flap&nbsp;Ext&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right' nowrap='nowrap'>VLE&nbsp;Max&nbsp;Land Gear&nbsp;Ext&nbsp;:</td></tr>")

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>IFR&nbsp;Certification</b></td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>(IFR)&nbsp;:</td></tr>")

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Climb</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Normal&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Engine&nbsp;Out&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Ceiling&nbsp;:</td></tr>")

                        If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                            'do nothing
                        ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                            If HttpContext.Current.Session.Item("isMobile") Then
                                htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>(HOGE)Out of Ground<br />Effect:</td></tr>")
                                htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>(HIGE)In Ground Effect:</td></tr>")
                            Else
                                htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>(HOGE)&nbsp;Out&nbsp;of&nbsp;Ground&nbsp;Effect&nbsp;:</td></tr>")
                                htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>(HIGE)&nbsp;In&nbsp;Ground&nbsp;Effect&nbsp;:</td></tr>")
                            End If

                        ElseIf bHasManyAirFrames Then
                            htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>[R]&nbsp;(HOGE)&nbsp;Out&nbsp;of&nbsp;Ground&nbsp;Effect&nbsp;:</td></tr>")
                            htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>[R]&nbsp;(HIGE)&nbsp;In&nbsp;Ground&nbsp;Effect&nbsp;:</td></tr>")
                        End If

                        If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If

                            htmlOut.Append("<td nowrap='nowrap'><b>Landing&nbsp;Performance</b></td></tr><tr>")
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>FAA&nbsp;Field&nbsp;Length&nbsp;:</td></tr>")

                        ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                            ' do nothing
                        ElseIf bHasManyAirFrames Then
                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If

                            htmlOut.Append("<td nowrap='nowrap'><b>[F]&nbsp;Landing&nbsp;Performance</b></td></tr><tr>")
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>[F]&nbsp;FAA&nbsp;Field&nbsp;Length&nbsp;:</td></tr>")

                        End If

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Takeoff&nbsp;Performance</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>SL&nbsp;ISA&nbsp;BFL&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>5000'&nbsp;+20C&nbsp;BFL&nbsp;:</td></tr>")

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Range</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Range&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Tanks&nbsp;Full&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Seats&nbsp;Full&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Range&nbsp;(4&nbsp;PAX)&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Range&nbsp;(8&nbsp;PAX)&nbsp;:</td></tr>")

                        If bisReport Then
                            htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                        Else
                            htmlOut.Append("<tr class='performance_header_row'>")
                        End If

                        htmlOut.Append("<td nowrap='nowrap'><b>Engines</b></td></tr><tr>")

                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Number&nbsp;of&nbsp;:</td></tr><tr>")
                        htmlOut.Append("<td class='Label' valign='middle' align='right'>Model(s)&nbsp;:")

                        If searchCriteria.ViewCriteriaAircraftID > 0 Then

                        Else
                            'call so it doesnt return names, but it gets count 
                            If highest_engines_count > 0 Then

                                If highest_engines_count > 0 Then
                                    For iLoop As Integer = 1 To highest_engines_count - 1
                                        htmlOut.Append("<br />&nbsp;")
                                    Next
                                Else
                                    htmlOut.Append("<br />&nbsp;")
                                End If

                            Else
                                Call commonEvo.GetEngines_For_Spaces(sharedModelTable.Rows(0).Item("amod_id").ToString, nNumberOfEngines)


                                If nNumberOfEngines > 0 Then
                                    For iLoop As Integer = 1 To nNumberOfEngines - 1
                                        htmlOut.Append("<br />&nbsp;")
                                    Next
                                Else
                                    htmlOut.Append("<br />&nbsp;")
                                End If

                            End If
                        End If

                        htmlOut.Append("</td></tr><tr>")

                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Thrust&nbsp;(per&nbsp;engine)&nbsp;:</td></tr><tr>")
                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Shaft&nbsp;(per&nbsp;engine)&nbsp;:</td></tr><tr>")

                            htmlOut.Append("<td class='Label' valign='middle' align='right'>Common&nbsp;TBO&nbsp;Hours&nbsp;:</td></tr>")

                            If searchCriteria.ViewCriteriaAircraftID > 0 Then
                                htmlOut.Append("<tr><td class='Label' valign='middle' align='right'>On&nbsp;Condition&nbsp;TBO?&nbsp;:</td></tr>")
                            End If

                            If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") Then

                                If bisReport Then
                                    htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                                Else
                                    htmlOut.Append("<tr class='performance_header_row'>")
                                End If

                                htmlOut.Append("<td nowrap='nowrap'><b>Blades</b></td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>MAIN&nbsp;1&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Number of Blades&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Blade Diameter&nbsp;:</td></tr><tr>")

                                htmlOut.Append("<td class='Label' valign='middle' align='right'>MAIN&nbsp;2&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Number of Blades&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Blade Diameter&nbsp;:</td></tr><tr>")

                                htmlOut.Append("<td class='Label' valign='middle' align='right'>TAIL&nbsp;&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Number of Blades&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Blade Diameter&nbsp;:</td></tr><tr>")

                                htmlOut.Append("<td class='Label' valign='middle' align='right'>ANTI TORQUE SYSTEM&nbsp;:</td></tr>")

                            ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And bHasManyAirFrames Then

                                If bisReport Then
                                    htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                                Else
                                    htmlOut.Append("<tr class='performance_header_row'>")
                                End If

                                htmlOut.Append("<td nowrap='nowrap'><b>Blades</b></td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>MAIN&nbsp;1&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Number of Blades&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>Blade Diameter&nbsp;:</td></tr><tr>")

                                htmlOut.Append("<td class='Label' valign='middle' align='right'>[R]MAIN&nbsp;2&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>[R]Number of Blades&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>[R]Blade Diameter&nbsp;:</td></tr><tr>")

                                htmlOut.Append("<td class='Label' valign='middle' align='right'>[R]TAIL&nbsp;&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>[R]Number of Blades&nbsp;:</td></tr><tr>")
                                htmlOut.Append("<td class='Label' valign='middle' align='right'>[R]Blade Diameter&nbsp;:</td></tr><tr>")

                                htmlOut.Append("<td class='Label' valign='middle' align='right'>[R]ANTI TORQUE SYSTEM&nbsp;:</td></tr>")

                            End If


                            htmlOut.Append("</table></td>")

                            '''''
                            '''''' display next rows
                            '''''

                        Else

                            For Each r As DataRow In sharedModelTable.Rows

                            If bisReport Then
                                htmlOut.Append("<td valign=""top"" align=""left"" style=""text-align: left; width:100px; margin-left:3px;"">")
                                htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"">")
                            Else
                                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                                    htmlOut.Append("<td valign=""top"" align=""left"" style=""text-align: left; margin-left:3px;"">")
                                Else
                                    htmlOut.Append("<td valign=""top"" align=""left"" style=""text-align: left; width:100px; margin-left:3px;"">")
                                End If
                                htmlOut.Append("<table cellpadding=""0"" cellspacing=""0"">")
                            End If

                            If bisReport Then

                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")

                                If Not bHasManyAirFrames Then
                                    htmlOut.Append("<td nowrap='nowrap' valign=""middle"" align=""center""><b>" + Replace(r.Item("amod_make_name").ToString, " ", "&nbsp;") + "&nbsp;" + Replace(r.Item("amod_model_name").ToString, " ", "&nbsp;") + "&nbsp;</b></td></tr>")
                                Else
                                    htmlOut.Append("<td nowrap='nowrap' valign=""middle"" align=""center""><b>" + Replace(r.Item("amod_make_name").ToString, " ", "&nbsp;") + "&nbsp;" + Replace(r.Item("amod_model_name").ToString, " ", "&nbsp;") + " [" + r.Item("amod_airframe_type_code").ToString.ToUpper + "]&nbsp;</b></td></tr>")
                                End If
                            Else

                                If sharedModelTable.Rows.Count > 0 Then
                                    htmlOut.Append("<tr class='performance_header_row'>")
                                    If Not bHasManyAirFrames Then
                                        htmlOut.Append("<td nowrap='nowrap' valign=""middle"" align=""center"">" + Replace(r.Item("amod_make_name").ToString, " ", "&nbsp;") + "&nbsp;<a " + DisplayFunctions.WriteModelDetailsLink(CLng(r.Item("amod_id").ToString), "", False) + " class='emphasis_text underline'>" + Replace(r.Item("amod_model_name").ToString, " ", "&nbsp;") + "</a>&nbsp;</td></tr>")
                                    Else
                                        htmlOut.Append("<td nowrap='nowrap' valign=""middle"" align=""center"">" + Replace(r.Item("amod_make_name").ToString, " ", "&nbsp;") + "&nbsp;<a " + DisplayFunctions.WriteModelDetailsLink(CLng(r.Item("amod_id").ToString), "", False) + " class='emphasis_text underline'>" + Replace(r.Item("amod_model_name").ToString, " ", "&nbsp;") + " [" + r.Item("amod_airframe_type_code").ToString.ToUpper + "]</a>&nbsp;</td></tr>")
                                    End If
                                End If

                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If

                            If bisReport Then

                                If Not searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append("<td nowrap='nowrap' valign=""middle"" align=""center""><b>US standard</b></td></tr><tr>")
                                Else
                                    htmlOut.Append("<td nowrap='nowrap' valign=""middle"" align=""center""><b>Metric</b></td></tr><tr>")
                                End If

                            Else

                                htmlOut.Append("<td nowrap='nowrap' valign=""middle"" align=""center"">")

                                If HttpContext.Current.Session.Item("isMobile") Then
                                    If Not searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<b class=""activeLinks""><span class=""activeTog"">Imperial</span><span id=""metricToggle"">Metric</a></b>")
                                    Else
                                        htmlOut.Append("<b class=""activeLinks""><span id=""imperialToggle"">Imperial</span><span class=""activeTog"">Metric</a></b>")
                                    End If
                                Else
                                    If Not searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<b>US standard")
                                    Else
                                        htmlOut.Append("<b>Metric</b>")
                                    End If
                                End If
                                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                                    htmlOut.Append("&nbsp;&nbsp;<a " + DisplayFunctions.WriteModelLink(CLng(r.Item("amod_id").ToString), "", False) + " class='emphasis_text underline displayNoneMobile'>Model&nbsp;Market&nbsp;Summary</a>")
                                End If

                                htmlOut.Append("</td></tr><tr>")

                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                If CDbl(r.Item("amod_fuselage_length").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_fuselage_length").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                End If

                                If CDbl(r.Item("amod_fuselage_height").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_fuselage_height").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                End If

                                If r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("F") Then
                                    If CDbl(r.Item("amod_fuselage_wingspan").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_fuselage_wingspan").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                    End If
                                ElseIf r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("R") Then
                                    If CDbl(r.Item("amod_fuselage_width").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_fuselage_width").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                    End If
                                End If

                            Else
                                If CDbl(r.Item("amod_fuselage_length").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_fuselage_length").ToString), 1, False, True, False) + "&nbsp;(ft)</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(ft)</td></tr><tr>")
                                End If

                                If CDbl(r.Item("amod_fuselage_height").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_fuselage_height").ToString), 1, False, True, False) + "&nbsp;(ft)</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(ft)</td></tr><tr>")
                                End If

                                If r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("F") Then
                                    If CDbl(r.Item("amod_fuselage_wingspan").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_fuselage_wingspan").ToString), 1, False, True, False) + "&nbsp;(ft)</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(ft)</td></tr>")
                                    End If
                                ElseIf r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("R") Then
                                    If CDbl(r.Item("amod_fuselage_width").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_fuselage_width").ToString), 1, False, True, False) + "&nbsp;(ft)</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(ft)</td></tr>")
                                    End If
                                End If

                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If
                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If searchCriteria.ViewCriteriaUseMetricValues Then

                                If Not IsDBNull(r("amod_cabinsize_length_feet")) Then
                                    If CDbl(r.Item("amod_cabinsize_length_feet").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_cabinsize_length_feet").ToString) + (CDbl(r.Item("amod_cabinsize_length_inches").ToString) * 0.083333333)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_cabinsize_height_feet")) Then
                                    If CDbl(r.Item("amod_cabinsize_height_feet").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_cabinsize_height_feet").ToString) + (CDbl(r.Item("amod_cabinsize_height_inches").ToString) * 0.083333333)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_cabinsize_width_feet")) Then
                                    If CDbl(r.Item("amod_cabinsize_width_feet").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_cabinsize_width_feet").ToString) + (CDbl(r.Item("amod_cabinsize_width_inches").ToString) * 0.083333333)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_cabin_volume")) Then
                                    If CDbl(r.Item("amod_cabin_volume").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("CBFT", CDbl(r.Item("amod_cabin_volume").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_baggage_volume")) Then
                                    If CDbl(r.Item("amod_baggage_volume").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("CBFT", CDbl(r.Item("amod_baggage_volume").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr>")
                                End If

                            Else

                                If Not IsDBNull(r("amod_cabinsize_length_feet")) Then
                                    If CDbl(r.Item("amod_cabinsize_length_feet").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_cabinsize_length_feet").ToString), False, True, False) + "'&nbsp;" + FormatNumber(CDbl(r.Item("amod_cabinsize_length_inches").ToString), False, True, False) + "''&nbsp;(ft)(in)</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0'&nbsp;0''&nbsp;(ft)(in)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0'&nbsp;0''&nbsp;(ft)(in)</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_cabinsize_height_feet")) Then
                                    If CDbl(r.Item("amod_cabinsize_height_feet").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_cabinsize_height_feet").ToString), False, True, False) + "'&nbsp;" + FormatNumber(CDbl(r.Item("amod_cabinsize_height_inches").ToString), False, True, False) + "''&nbsp;(ft)(in)</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0'&nbsp;0''&nbsp;(ft)(in)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0'&nbsp;0''&nbsp;(ft)(in)</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_cabinsize_width_feet")) Then
                                    If CDbl(r.Item("amod_cabinsize_width_feet").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_cabinsize_width_feet").ToString), False, True, False) + "'&nbsp;" + FormatNumber(CDbl(r.Item("amod_cabinsize_width_inches").ToString), False, True, False) + "''&nbsp;(ft)(in)</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0'&nbsp;0''&nbsp;(ft)(in)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0'&nbsp;0''&nbsp;(ft)(in)</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_cabin_volume")) Then
                                    If CDbl(r.Item("amod_cabin_volume").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_cabin_volume").ToString), False, True, False) + "&nbsp;(cb ft)</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(cb ft)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'0&nbsp;(cb ft)</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_baggage_volume")) Then
                                    If CDbl(r.Item("amod_baggage_volume").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_baggage_volume").ToString), False, True, False) + "&nbsp;(cb ft)</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(cb ft)</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(cb ft)</td></tr>")
                                End If

                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If

                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            htmlOut.Append("<td valign='middle' align='right'>" + r.Item("amod_number_of_crew").ToString + "&nbsp;</td></tr><tr>")
                            htmlOut.Append("<td valign='middle' align='right'>" + r.Item("amod_number_of_passengers").ToString + "&nbsp;</td></tr>")

                            If r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("PSI", CDbl(r.Item("amod_pressure").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("PSI") + ")</td></tr>")
                                Else
                                    htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_pressure").ToString), 1, False, True, False) + "&nbsp;(psi)</td></tr>")
                                End If
                            ElseIf r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                                ' do nothing 
                            ElseIf bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("PSI", CDbl(r.Item("amod_pressure").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("PSI") + ")</td></tr>")
                                Else
                                    htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_pressure").ToString), 1, False, True, False) + "&nbsp;(psi)</td></tr>")
                                End If
                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If
                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If Not IsDBNull(r("amod_fuel_cap_std_weight")) Then
                                If CDbl(r.Item("amod_fuel_cap_std_weight").ToString) > 0 Then
                                    If searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_fuel_cap_std_weight").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_fuel_cap_std_weight").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                                End If
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                            End If

                            If Not IsDBNull(r("amod_fuel_cap_std_gal")) Then
                                If CDbl(r.Item("amod_fuel_cap_std_gal").ToString) > 0 Then
                                    If searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("GAL", CDbl(r.Item("amod_fuel_cap_std_gal").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("gal") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_fuel_cap_std_gal").ToString), False, True, False) + "&nbsp;(gal)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                                End If
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                            End If

                            If Not IsDBNull(r("amod_fuel_cap_opt_weight")) Then
                                If CDbl(r.Item("amod_fuel_cap_opt_weight").ToString) > 0 Then
                                    If searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_fuel_cap_opt_weight").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_fuel_cap_opt_weight").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                                End If
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                            End If

                            If Not IsDBNull(r("amod_fuel_cap_opt_gal")) Then
                                If CDbl(r.Item("amod_fuel_cap_opt_gal").ToString) > 0 Then
                                    If searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("GAL", CDbl(r.Item("amod_fuel_cap_opt_gal").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("gal") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_fuel_cap_opt_gal").ToString), False, True, False) + "&nbsp;(gal)</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>")
                                End If
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>")
                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If
                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_max_ramp_weight").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr><tr>")
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_max_ramp_weight").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")
                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_max_takeoff_weight").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr><tr>")
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_max_takeoff_weight").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")
                            End If

                            If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_zero_fuel_weight").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_zero_fuel_weight").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")
                                End If
                            ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + IIf(HttpContext.Current.Session.Item("isMobile"), "&nbsp;<br />", "") + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_weight_eow").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + IIf(HttpContext.Current.Session.Item("isMobile"), "&nbsp;<br />", "") + FormatNumber(CDbl(r.Item("amod_weight_eow").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")
                                End If
                            ElseIf bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_zero_fuel_weight").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_zero_fuel_weight").ToString), False, True, False) + "&nbsp;(lbs)")
                                End If
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append(" / " + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_weight_eow").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr><tr>")
                                Else
                                    htmlOut.Append(" / " + FormatNumber(CDbl(r.Item("amod_weight_eow").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")
                                End If
                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_basic_op_weight").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr><tr>")
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_basic_op_weight").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")
                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(r.Item("amod_max_landing_weight").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("lbs") + ")</td></tr>")
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_max_landing_weight").ToString), False, True, False) + "&nbsp;(lbs)</td></tr>")
                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If
                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    If Not IsDBNull(r("amod_stall_vs")) Then
                                        If CDbl(r.Item("amod_stall_vs").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_stall_vs").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    End If
                                Else
                                    If Not IsDBNull(r("amod_stall_vs")) Then
                                        If CDbl(r.Item("amod_stall_vs").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_stall_vs").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                    End If
                                End If

                            ElseIf r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                                ' do nothing 
                            ElseIf bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    If Not IsDBNull(r("amod_stall_vs")) Then
                                        If CDbl(r.Item("amod_stall_vs").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_stall_vs").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    End If
                                Else
                                    If Not IsDBNull(r("amod_stall_vs")) Then
                                        If CDbl(r.Item("amod_stall_vs").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_stall_vs").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                    End If
                                End If
                            End If

                            If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    If Not IsDBNull(r("amod_stall_vso")) Then
                                        If CDbl(r.Item("amod_stall_vso").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_stall_vso").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    End If
                                Else
                                    If Not IsDBNull(r("amod_stall_vso")) Then
                                        If CDbl(r.Item("amod_stall_vso").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_stall_vso").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                    End If
                                End If
                            ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                                ' do nothing 
                            ElseIf bHasManyAirFrames Then
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    If Not IsDBNull(r("amod_stall_vso")) Then
                                        If CDbl(r.Item("amod_stall_vso").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_stall_vso").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    End If
                                Else
                                    If Not IsDBNull(r("amod_stall_vso")) Then
                                        If CDbl(r.Item("amod_stall_vso").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_stall_vso").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                    End If
                                End If
                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_cruis_speed").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_max_speed").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")

                                If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                                    'do nothing
                                ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                                    If Not IsDBNull(r("amod_vne_maxop_speed")) Then
                                        If CDbl(r.Item("amod_vne_maxop_speed").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_vne_maxop_speed").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    End If
                                ElseIf bHasManyAirFrames Then
                                    If Not IsDBNull(r("amod_vne_maxop_speed")) Then
                                        If CDbl(r.Item("amod_vne_maxop_speed").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_vne_maxop_speed").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    End If
                                End If

                                If Not IsDBNull(r("amod_v1_takeoff_speed")) Then
                                    If CDbl(r.Item("amod_v1_takeoff_speed").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_v1_takeoff_speed").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_vfe_max_flap_extended_speed")) Then
                                    If CDbl(r.Item("amod_vfe_max_flap_extended_speed").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_vfe_max_flap_extended_speed").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_vle_max_landing_gear_ext_speed")) Then
                                    If CDbl(r.Item("amod_vle_max_landing_gear_ext_speed").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(r.Item("amod_vle_max_landing_gear_ext_speed").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr>")
                                End If

                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_cruis_speed").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_max_speed").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")

                                If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                                    'do nothing
                                ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                                    If Not IsDBNull(r("amod_vne_maxop_speed")) Then
                                        If CDbl(r.Item("amod_vne_maxop_speed").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_vne_maxop_speed").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                    End If
                                ElseIf bHasManyAirFrames Then
                                    If Not IsDBNull(r("amod_vne_maxop_speed")) Then
                                        If CDbl(r.Item("amod_vne_maxop_speed").ToString) > 0 Then
                                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_vne_maxop_speed").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                        Else
                                            htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                        End If
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                    End If
                                End If

                                If Not IsDBNull(r("amod_v1_takeoff_speed")) Then
                                    If CDbl(r.Item("amod_v1_takeoff_speed").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_v1_takeoff_speed").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_vfe_max_flap_extended_speed")) Then
                                    If CDbl(r.Item("amod_vfe_max_flap_extended_speed").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_vfe_max_flap_extended_speed").ToString), False, True, False) + "&nbsp;(kn)</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                                End If

                                If Not IsDBNull(r("amod_vle_max_landing_gear_ext_speed")) Then
                                    If CDbl(r.Item("amod_vle_max_landing_gear_ext_speed").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_vle_max_landing_gear_ext_speed").ToString), False, True, False) + "&nbsp;(kn)</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr>")
                                End If

                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If
                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If Not IsDBNull(r("amod_ifr_certification")) Then
                                If Not String.IsNullOrEmpty(r.Item("amod_ifr_certification").ToString.Trim) Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + r.Item("amod_ifr_certification").ToString + "&nbsp;</td></tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>-</td></tr>")
                                End If
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>-</td></tr>")
                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If

                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                If CDbl(r.Item("amod_climb_normal_feet").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FPM", CDbl(r.Item("amod_climb_normal_feet").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FPM") + ")</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FPM") + ")</td></tr><tr>")
                                End If

                                If CDbl(r.Item("amod_climb_engout_feet").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FPM", CDbl(r.Item("amod_climb_engout_feet").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FPM") + ")</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FPM") + ")</td></tr><tr>")
                                End If

                                If CDbl(r.Item("amod_ceiling_feet").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_ceiling_feet").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                End If

                            Else
                                If CDbl(r.Item("amod_climb_normal_feet").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_climb_normal_feet").ToString), False, True, False) + "&nbsp;(fpm)</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(fpm)</td></tr><tr>")
                                End If

                                If CDbl(r.Item("amod_climb_engout_feet").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_climb_engout_feet").ToString), False, True, False) + "&nbsp;(fpm)</td></tr><tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(fpm)</td></tr><tr>")
                                End If

                                If CDbl(r.Item("amod_ceiling_feet").ToString) > 0 Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_ceiling_feet").ToString), False, True, False) + "&nbsp;(ft)</td></tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(ft)</td></tr>")
                                End If
                            End If

                            If r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                                ' do nothing 
                            ElseIf r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("R") And Not bHasManyAirFrames Then

                                If Not IsDBNull(r("amod_climb_hoge")) Then
                                    If searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<tr><td valign='middle' align='right'>" + IIf(HttpContext.Current.Session.Item("isMobile"), "&nbsp;<br />", "") + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_climb_hoge").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<tr><td valign='middle' align='right'>" + IIf(HttpContext.Current.Session.Item("isMobile"), "&nbsp;<br />", "") + FormatNumber(CDbl(r.Item("amod_climb_hoge").ToString), False, True, False) + "&nbsp;(ft)</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>")
                                End If

                                If Not IsDBNull(r("amod_climb_hige")) Then
                                    If searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_climb_hige").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_climb_hige").ToString), False, True, False) + "&nbsp;(ft)</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>")
                                End If

                            ElseIf bHasManyAirFrames Then
                                If Not IsDBNull(r("amod_climb_hoge")) Then
                                    If searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_climb_hoge").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_climb_hoge").ToString), False, True, False) + "&nbsp;(ft)</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>")
                                End If

                                If Not IsDBNull(r("amod_climb_hige")) Then
                                    If searchCriteria.ViewCriteriaUseMetricValues Then
                                        htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_climb_hige").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_climb_hige").ToString), False, True, False) + "&nbsp;(ft)</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>")
                                End If
                            End If

                            If r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                                If bisReport Then
                                    htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                                Else
                                    htmlOut.Append("<tr class='performance_header_row'>")
                                End If
                                htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_field_length").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CLng(r.Item("amod_field_length").ToString), False, True, False) + "&nbsp;(ft)</td></tr>")
                                End If

                                searchCriteria.ViewCriteriaAircraftFieldLength = CLng(r.Item("amod_field_length").ToString)
                            ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                                'do nothing 
                            ElseIf bHasManyAirFrames Then
                                If bisReport Then
                                    htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                                Else
                                    htmlOut.Append("<tr class='performance_header_row'>")
                                End If
                                htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_field_length").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CLng(r.Item("amod_field_length").ToString), False, True, False) + "&nbsp;(ft)</td></tr>")
                                End If

                                searchCriteria.ViewCriteriaAircraftFieldLength = CLng(r.Item("amod_field_length").ToString)

                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If
                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_takeoff_ali").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr><tr>")
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CLng(r.Item("amod_takeoff_ali").ToString), False, True, False) + "&nbsp;(ft)</td></tr><tr>")
                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(r.Item("amod_takeoff_500").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("FT") + ")</td></tr>")
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CLng(r.Item("amod_takeoff_500").ToString), False, True, False) + "&nbsp;(ft)</td></tr>")
                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If
                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(r.Item("amod_max_range_miles").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("NM") + ")</td></tr>")
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, FormatNumber(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(r.Item("amod_max_range_miles").ToString)), False, True, False), FormatNumber(CDbl(r.Item("amod_max_range_miles").ToString), False, True, False)) + "&nbsp;" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, "(sm)", "(nm)") + "</td></tr>")
                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(r.Item("amod_range_tanks_full").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("NM") + ")</td></tr>")
                            Else
                                htmlOut.Append("<td valign='middle' align='right'>" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, FormatNumber(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(r.Item("amod_range_tanks_full").ToString)), False, True, False), FormatNumber(CDbl(r.Item("amod_range_tanks_full").ToString), False, True, False)) + "&nbsp;" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, "(sm)", "(nm)") + "</td></tr>")
                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(r.Item("amod_range_seats_full").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("NM") + ")</td></tr>")
                            Else
                                htmlOut.Append("<tr><td valign='middle' align='right'>" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, FormatNumber(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(r.Item("amod_range_seats_full").ToString)), False, True, False), FormatNumber(CDbl(r.Item("amod_range_seats_full").ToString), False, True, False)) + "&nbsp;" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, "(sm)", "(nm)") + "</td></tr>")
                            End If

                            If searchCriteria.ViewCriteriaUseMetricValues Then

                                If Not IsDBNull(r("amod_range_4_passenger")) Then
                                    If CDbl(r.Item("amod_range_4_passenger").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(r.Item("amod_range_4_passenger").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("NM") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>" + Constants.cHyphen + "</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + Constants.cHyphen + "</td></tr>")
                                End If

                                If Not IsDBNull(r("amod_range_8_passenger")) Then
                                    If CDbl(r.Item("amod_range_8_passenger").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(r.Item("amod_range_8_passenger").ToString)), 1, False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("NM") + ")</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>" + Constants.cHyphen + "</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + Constants.cHyphen + "</td></tr>")
                                End If

                            Else

                                If Not IsDBNull(r("amod_range_4_passenger")) Then
                                    If CDbl(r.Item("amod_range_4_passenger").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, FormatNumber(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(r.Item("amod_range_4_passenger").ToString)), False, True, False), FormatNumber(CDbl(r.Item("amod_range_4_passenger").ToString), False, True, False)) + "&nbsp;" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, "(sm)", "(nm)") + "</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>" + Constants.cHyphen + "</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + Constants.cHyphen + "</td></tr>")
                                End If

                                If Not IsDBNull(r("amod_range_8_passenger")) Then
                                    If CDbl(r.Item("amod_range_8_passenger").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, FormatNumber(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(r.Item("amod_range_8_passenger").ToString)), False, True, False), FormatNumber(CDbl(r.Item("amod_range_8_passenger").ToString), False, True, False)) + "&nbsp;" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, "(sm)", "(nm)") + "</td></tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>" + Constants.cHyphen + "</td></tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>" + Constants.cHyphen + "</td></tr>")
                                End If

                            End If

                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If

                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")


                            If highest_engines_count > 0 Then
                                nNumberOfEngines = highest_engines_count
                            Else
                                Call commonEvo.GetEngines_For_Spaces(sharedModelTable.Rows(0).Item("amod_id").ToString, nNumberOfEngines, searchCriteria.ViewCriteriaAircraftID)
                            End If

                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CLng(r.Item("amod_number_of_engines").ToString), False, True, False) + "&nbsp;</td></tr><tr>")
                            htmlOut.Append("<td valign='middle' align='right' nowrap='nowrap'>" + commonEvo.GetEngines(r.Item("amod_id").ToString, nNumberOfEngines, False, searchCriteria.ViewCriteriaAircraftID, temp_on_condition).Trim + "</td></tr><tr>")

                            If searchCriteria.ViewCriteriaUseMetricValues Then

                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(ConversionFunctions.ConvertUSToMetricValue("LBS", r.Item("amod_engine_thrust_lbs").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("LBS") + ")</td></tr><tr>")

                                If Not IsDBNull(r("amod_engine_shaft")) Then
                                    If CDbl(r.Item("amod_engine_shaft").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(ConversionFunctions.ConvertUSToMetricValue("HP", r.Item("amod_engine_shaft").ToString)), False, True, False) + "&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("HP") + ")</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("HP") + ")</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                                End If

                            Else

                                htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_engine_thrust_lbs").ToString), False, True, False) + "&nbsp;(lbs)</td></tr><tr>")

                                If Not IsDBNull(r("amod_engine_shaft")) Then
                                    If CDbl(r.Item("amod_engine_shaft").ToString) > 0 Then
                                        htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_engine_shaft").ToString), False, True, False) + "&nbsp;(hp)</td></tr><tr>")
                                    Else
                                        htmlOut.Append("<td valign='middle' align='right'>0&nbsp;(hp)</td></tr><tr>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                                End If

                            End If

                            htmlOut.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(r.Item("amod_engine_com_tbo_hrs").ToString), False, True, False) + "&nbsp;</td></tr>")



                            If searchCriteria.ViewCriteriaAircraftID > 0 Then
                                htmlOut.Append("<tr><td valign='middle' align='right'>" & temp_on_condition & "&nbsp;</td></tr>")
                            End If


                            If bisReport Then
                                htmlOut.Append("<tr bgcolor='#CCCCCC'>")
                            Else
                                htmlOut.Append("<tr class='performance_header_row'>")
                            End If

                            htmlOut.Append("<td><b>&nbsp;</b></td></tr><tr>")

                            If r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("R") Then

                                If Not (IsDBNull(r.Item("amod_main_rotor_1_blade_count"))) Then
                                    MainRotor1Blade = r.Item("amod_main_rotor_1_blade_count")
                                Else
                                    MainRotor1Blade = 0
                                End If

                                If Not (IsDBNull(r.Item("amod_main_rotor_2_blade_count"))) Then
                                    MainRotor2Blade = r.Item("amod_main_rotor_2_blade_count")
                                Else
                                    MainRotor2Blade = 0
                                End If

                                If Not (IsDBNull(r.Item("amod_main_rotor_1_blade_diameter"))) Then
                                    MainRotor1BladeDiameter = r.Item("amod_main_rotor_1_blade_diameter")
                                Else
                                    MainRotor1BladeDiameter = 0
                                End If

                                If Not (IsDBNull(r.Item("amod_main_rotor_2_blade_diameter"))) Then
                                    MainRotor2BladeDiameter = r.Item("amod_main_rotor_2_blade_diameter")
                                Else
                                    MainRotor2BladeDiameter = 0
                                End If


                                If Not (IsDBNull(r.Item("amod_tail_rotor_blade_count"))) Then
                                    TailBlade = r.Item("amod_tail_rotor_blade_count")
                                Else
                                    TailBlade = 0
                                End If

                                If Not (IsDBNull(r.Item("amod_tail_rotor_blade_diameter"))) Then
                                    TailBladeDiameter = r.Item("amod_tail_rotor_blade_diameter")
                                Else
                                    TailBladeDiameter = 0
                                End If

                                If Not (IsDBNull(r.Item("amod_rotor_anti_torque_system"))) Then
                                    AntiTorq = r.Item("amod_rotor_anti_torque_system")
                                Else
                                    AntiTorq = ""
                                End If

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>") ' for main1 

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;" + FormatNumber(MainRotor1Blade, False, True, False) + "</td></tr><tr>")
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;" + FormatNumber(MainRotor1BladeDiameter, False, True, False) + "</td></tr><tr>")

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr>") ' for main2
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;" + FormatNumber(MainRotor2Blade, False, True, False) + "</td></tr><tr>")
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;" + FormatNumber(MainRotor2BladeDiameter, False, True, False) + "</td></tr><tr>")

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr>") ' for tail
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;" + FormatNumber(TailBlade, False, True, False) + "</td></tr><tr>")
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;" + FormatNumber(TailBladeDiameter, False, True, False) + "</td></tr><tr>")

                                htmlOut.Append("<td valign='top' align='right'>" + IIf(HttpContext.Current.Session.Item("isMobile"), "&nbsp;<br />", "") + "&nbsp;" + AntiTorq.Trim + "</td></tr>")

                            ElseIf r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("F") And bHasManyAirFrames Then

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>") ' for main1 

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>")
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>")

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>") ' for main2
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>")
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>")

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>") ' for tail
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>")
                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr><tr>")

                                htmlOut.Append("<td valign='top' align='right'>&nbsp;</td></tr>")

                            End If

                            htmlOut.Append("</table></td>")
                        Next

                    End If

                End If
            End If

        Catch ex As Exception
            aError = "Error in views_display_performance_specs(ByVal bisReport As Boolean, ByVal optFormat As String, ByVal bShowLabelColumn As Boolean, ByVal bHasManyAirFrames As Boolean, ByRef searchCriteria As viewSelectionCriteriaClass) As String " + ex.Message

        Finally
        End Try

        out_htmlString = htmlOut.ToString()

        htmlOut = Nothing

    End Sub

    Public Sub views_display_operating_costs(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bFirstOne As Boolean, ByRef out_htmlString As String, Optional ByRef SharedModelDatatable As DataTable = Nothing)

        Dim htmlOut As New StringBuilder
        'Dim results_table As New DataTable
        Dim bChangeCurrency As Boolean = True

        Dim sTitle As String = ""
        Dim xIndex As Integer = 0
        Dim sTmpTitle As String = "&nbsp;"


        Dim sCurrencyName As String = ""
        Dim sCurrencySymbol As String = ""
        Dim sCurrencyDate As String = ""

        Dim fuelTotCost As Double = 0.0
        Dim fuelGalCost As Double = 0.0
        Dim fuelAddCost As Double = 0.0
        Dim fuelBurnRate As Double = 0.0

        Dim avgBlockSpeed() As Double = Nothing
        Dim totalCostPerMile() As Double = Nothing

        Dim annualMiles As Integer = 0
        Dim costPerMileFixDir As Double = 0.0
        Dim costPerSeatFixDir As Double = 0.0
        Dim costPerHourFixDir As Double = 0.0

        Dim totalDirCostHour() As Double = Nothing
        Dim annualHrs As Integer = 0
        Dim totalFixedDirect() As Double = Nothing
        Dim totalDirCostYR() As Double = Nothing

        Dim maintTotalCost As Double = 0.0

        Dim maintLaborCost As Double = 0.0
        Dim maintPartsCost As Double = 0.0

        Dim maintLaborCostManHour As Decimal = 0.0
        Dim maintPartsCostManHour As Decimal = 0.0

        Dim maintEngineCost As Double = 0.0
        Dim maintThrustCost As Double = 0.0
        Dim miscFlightTotalCost As Double = 0.0
        Dim miscLandParkCost As Double = 0.0
        Dim miscCrewCost As Double = 0.0
        Dim miscSupplyCost As Double = 0.0
        Dim crewTotalCost As Double = 0.0
        Dim captSalaryCost As Double = 0.0
        Dim coPilotSalaryCost As Double = 0.0
        Dim benefitsCost As Double = 0.0
        Dim hangarCost As Double = 0.0
        Dim insuranceTotalCost As Double = 0.0
        Dim insuranceHullCost As Double = 0.0
        Dim insuranceLiabilityCost As Double = 0.0
        Dim miscTotalCost As Double = 0.0
        Dim miscTrainCost As Double = 0.0
        Dim miscModernCost As Double = 0.0
        Dim miscNavCost As Double = 0.0
        Dim depreciationCost() As Double = Nothing
        Dim fixedTotalCost() As Double = Nothing

        Dim noDepTotalCost As Double = 0.0
        Dim variableTotalCost As Double = 0.0

        Dim costPerHourNoDep As Double = 0.0
        Dim costPerMileNoDep As Double = 0.0
        Dim costPerSeatNoDep As Double = 0.0

        Dim colspan As String = ""

        If HttpContext.Current.Session.Item("homebasefuelPrice") = 0 Then
            HttpContext.Current.Session.Item("homebasefuelPrice") = CDbl(commonEvo.Get_Homebase_Fuel_Price())
        End If

        If CDbl(HttpContext.Current.Session.Item("localfuelPrice")) > 0 Then
            HttpContext.Current.Session.Item("fuelPriceBase") = CDbl(HttpContext.Current.Session.Item("localfuelPrice").ToString)
        ElseIf CDbl(HttpContext.Current.Session.Item("homebasefuelPrice")) > 0 Then
            HttpContext.Current.Session.Item("fuelPriceBase") = CDbl(HttpContext.Current.Session.Item("homebasefuelPrice").ToString)
        End If

        If Not searchCriteria.ViewCriteriaUseMetricValues Then
            If Not searchCriteria.ViewCriteriaUseStatuteMiles Then
                sTitle = "Standard (nm)"
            Else
                sTitle = "Standard"
            End If
        Else
            sTitle = "Metric"
        End If

        If CInt(HttpContext.Current.Session.Item("localPreferences").DefaultCurrency.ToString) <> 9 Then ' 9 = us dollar

            HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate = commonEvo.GetForeignExchangeRate(CInt(HttpContext.Current.Session.Item("localPreferences").DefaultCurrency.ToString), sCurrencyName, sCurrencyDate)

            sTitle += "<br />" + sCurrencyName.Trim

            If Not String.IsNullOrEmpty(sCurrencyDate) Then
                sTmpTitle = "<em>(" + HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString + ") as of " + FormatDateTime(sCurrencyDate, vbShortDate) + "</em>"
            Else
                sTmpTitle = "<em>(" + HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString + ")</em>"
            End If

            If sCurrencyName.ToLower.Contains("euro") Then
                sCurrencySymbol = Constants.cEuroSymbol
            ElseIf sCurrencyName.ToLower.Contains("dollar") Then
                sCurrencySymbol = Constants.cDollarSymbol
            ElseIf sCurrencyName.ToLower.Contains("pound") Then
                sCurrencySymbol = Constants.cPoundSymbol
            Else
                sCurrencySymbol = Constants.cEmptyString
            End If

        Else

            sCurrencySymbol = Constants.cDollarSymbol
            sCurrencyName = "Dollar (US)"
            sCurrencyDate = Now().ToShortDateString

            sTitle += "<br />" + sCurrencyName
            sTmpTitle = "&nbsp;"

        End If

        If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
            bChangeCurrency = True
        End If

        Try

            If IsNothing(SharedModelDatatable) Then
                SharedModelDatatable = commonEvo.get_view_model_info(searchCriteria, True)
            End If

            If Not IsNothing(SharedModelDatatable) Then

                If SharedModelDatatable.Rows.Count > 0 Then

                    ReDim totalDirCostHour(SharedModelDatatable.Rows.Count - 1)
                    ReDim avgBlockSpeed(SharedModelDatatable.Rows.Count - 1)
                    ReDim totalCostPerMile(SharedModelDatatable.Rows.Count - 1)
                    ReDim fixedTotalCost(SharedModelDatatable.Rows.Count - 1)
                    ReDim totalDirCostHour(SharedModelDatatable.Rows.Count - 1)
                    ReDim totalFixedDirect(SharedModelDatatable.Rows.Count - 1)
                    ReDim totalDirCostYR(SharedModelDatatable.Rows.Count - 1)
                    ReDim depreciationCost(SharedModelDatatable.Rows.Count - 1)

                    If HttpContext.Current.Session.Item("isMobile") = True Then
                        If bFirstOne Then
                            htmlOut.Append("</tr><tr>" + vbCrLf)
                        End If
                    End If

                    If SharedModelDatatable.Rows.Count >= 1 Then
                        colspan = " colspan=""" + (SharedModelDatatable.Rows.Count * 2).ToString + """"
                    End If

                    htmlOut.Append("<td valign=""top"" align=""left"" style=""text-align: left;"" >" + vbCrLf)

                    ' inner costs Table
                    htmlOut.Append("<table cellspacing=""0"" id=""innerCostsTbl"" " & IIf(HttpContext.Current.Session.Item("isMobile"), " width=""100%""", "cellpadding=""2"" ") & "><tr class=""noBorder"">" + vbCrLf)

                    If bFirstOne Then
                        htmlOut.Append("<th align=""left"" valign=""middle"" " & IIf(HttpContext.Current.Session.Item("isMobile"), " width=""50%""", " width=""30%""") & "  height='60'><b>DIRECT COSTS PER HOUR</b><br />" + sTmpTitle.Trim + "</th>")
                    Else
                        If HttpContext.Current.Session.Item("isMobile") = True Then
                            htmlOut.Append("<th" + colspan + " valign=""top"" align=""left"" " & IIf(HttpContext.Current.Session.Item("isMobile"), " width=""50%""", " width=""70%""") & ">")
                            If Not searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<b class=""activeLinks""><span class=""activeTog"">Imperial</span><span id=""metricToggle"">Metric</a></b><br clear=""all"" /><div class=""float_right display_inline_block margin-top"">" + sCurrencyName.Trim & "</div>")
                            Else
                                htmlOut.Append("<b class=""activeLinks""><span id=""imperialToggle"">Imperial</span><span class=""activeTog"">Metric</a></b><br clear=""all"" /><div class=""float_right display_inline_block margin-top"">" + sCurrencyName.Trim & "</div>")
                            End If
                            htmlOut.Append("</th>")
                        Else
                            htmlOut.Append("<th" + colspan + " align=""left"" " & IIf(HttpContext.Current.Session.Item("isMobile"), " width=""50%""", " width=""70%""") & " height='60'>" + sTitle.Trim + "</th>")
                        End If
                    End If

                    htmlOut.Append("</tr><tr>" + vbCrLf)

                    For Each r As DataRow In SharedModelDatatable.Rows

                        htmlOut.Append("<td valign=""top"" align=""left"" " & IIf(HttpContext.Current.Session.Item("isMobile") = False, " style=""padding-left:5px;""", "") & ">")

                        ' start direct costs per hour table
                        htmlOut.Append("<table cellspacing=""0"" " & IIf(HttpContext.Current.Session.Item("isMobile"), " width=""100%""", " cellpadding=""2""") & ">" & vbCrLf)

                        fuelTotCost = 0.0
                        fuelGalCost = 0.0
                        fuelAddCost = 0.0
                        fuelBurnRate = 0.0
                        maintTotalCost = 0.0

                        maintLaborCost = 0.0
                        maintPartsCost = 0.0

                        maintLaborCostManHour = 0.0
                        maintPartsCostManHour = 0.0

                        maintEngineCost = 0.0
                        maintThrustCost = 0.0
                        miscFlightTotalCost = 0.0
                        miscLandParkCost = 0.0
                        miscCrewCost = 0.0
                        miscSupplyCost = 0.0

                        If HttpContext.Current.Session.Item("isMobile") = False Then
                            htmlOut.Append("<tr class=""noBorder"">")
                            If bFirstOne Then
                                htmlOut.Append("<td class=""setMobileHeight"">&nbsp;</td>")
                            Else
                                If SharedModelDatatable.Rows.Count > 0 Then
                                    htmlOut.Append("<td colspan=""2"" valign=""middle"" align=""right"" nowrap=""nowrap"" style=""padding-right:3px;"" class=""setMobileHeight""><strong>" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</strong></td>")
                                Else
                                    htmlOut.Append("<td colspan=""2"">&nbsp;</td>")
                                End If
                            End If
                            htmlOut.Append("</tr>")
                        End If

                        htmlOut.Append("<tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><u>Fuel</u></td>") '
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If searchCriteria.ViewCriteriaUseMetricValues Then

                                If Not IsDBNull(r("amod_fuel_gal_cost")) And CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString) = 0 Then
                                    If CDbl(r.Item("amod_fuel_gal_cost").ToString) Then
                                        fuelGalCost = CDbl(ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(r.Item("amod_fuel_gal_cost").ToString)))
                                    End If
                                Else
                                    If CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString) > 0 Then
                                        fuelGalCost = CDbl(ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString)))
                                    End If
                                End If

                                If Not IsDBNull(r("amod_fuel_add_cost")) Then
                                    fuelAddCost = CDbl(ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(r.Item("amod_fuel_add_cost").ToString)))
                                Else
                                    fuelAddCost = CDbl(0)
                                End If

                                If Not IsDBNull(r("amod_fuel_burn_rate")) Then
                                    fuelBurnRate = CDbl(ConversionFunctions.ConvertUSToMetricValue("GAL", CDbl(r.Item("amod_fuel_burn_rate").ToString)))
                                Else
                                    fuelBurnRate = CDbl(0)
                                End If

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    fuelGalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), fuelGalCost))
                                End If

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    fuelAddCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), fuelAddCost))
                                End If

                                fuelTotCost = CDbl((fuelGalCost + fuelAddCost) * CDbl(fuelBurnRate))

                            Else

                                If Not IsDBNull(r("amod_fuel_gal_cost")) And CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString) = 0 Then
                                    If CDbl(r.Item("amod_fuel_gal_cost").ToString) Then
                                        fuelGalCost = CDbl(r.Item("amod_fuel_gal_cost").ToString)
                                    End If
                                Else
                                    If CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString) > 0 Then
                                        fuelGalCost = CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString)
                                    End If
                                End If

                                If Not IsDBNull(r("amod_fuel_add_cost")) Then
                                    fuelAddCost = CDbl(r.Item("amod_fuel_add_cost").ToString)
                                Else
                                    fuelAddCost = CDbl(0)
                                End If

                                If Not IsDBNull(r("amod_fuel_burn_rate")) Then
                                    fuelBurnRate = CDbl(r.Item("amod_fuel_burn_rate").ToString)
                                Else
                                    fuelBurnRate = CDbl(0)
                                End If

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    fuelGalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), fuelGalCost))
                                End If

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    fuelAddCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), fuelAddCost))
                                End If

                                fuelTotCost = CDbl((fuelGalCost + fuelAddCost) * CDbl(fuelBurnRate))

                            End If

                            htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(fuelTotCost, 2, True, False, True) + "</font></td>")

                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            ' when changing from metric to US standard all lables have to change
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Fuel Cost Per " + ConversionFunctions.TranslateUSMetricUnitsLong("GAL") + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Fuel Cost Per Gallon</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If fuelGalCost > 0 Then
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(fuelGalCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If

                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Additive&nbsp;Cost&nbsp;Per&nbsp;" + ConversionFunctions.TranslateUSMetricUnitsLong("GAL") + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Additive&nbsp;Cost&nbsp;Per&nbsp;Gallon</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If fuelAddCost > 0 Then
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(fuelAddCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Burn Rate (" + ConversionFunctions.TranslateUSMetricUnitsLong("GAL") + "s Per Hour)</td>")
                            Else
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Burn Rate (Gallons Per Hour)</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If fuelBurnRate > 0 Then
                                htmlOut.Append("<td align=""right"">" + FormatNumber(fuelBurnRate, 2, True, False, False) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">0</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"" nowrap=""nowrap""><u>Maintenance</u></td>")
                        Else

                            htmlOut.Append("<td>&nbsp;</td>")

                            If Not IsDBNull(r("amod_maint_lab_cost")) Then
                                maintLaborCost = CDbl(r.Item("amod_maint_lab_cost").ToString)
                            Else
                                maintLaborCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_maint_parts_cost")) Then
                                maintPartsCost = CDbl(r.Item("amod_maint_parts_cost").ToString)
                            Else
                                maintPartsCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_maint_labor_cost_man_hours_multiplier")) Then
                                maintLaborCostManHour = CDbl(r.Item("amod_maint_labor_cost_man_hours_multiplier").ToString)
                            Else
                                maintLaborCostManHour = 0.0
                            End If

                            If Not IsDBNull(r("amod_maint_parts_cost_man_hours_multiplier")) Then
                                maintPartsCostManHour = CDbl(r.Item("amod_maint_parts_cost_man_hours_multiplier").ToString)
                            Else
                                maintPartsCostManHour = 0.0
                            End If

                            If (maintLaborCost + maintPartsCost) > 0 Then
                                ' recalculate every time
                                maintTotalCost = CDbl(maintLaborCost + maintPartsCost)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    maintTotalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), maintTotalCost))
                                End If

                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(maintTotalCost, 2, True, False, True) + "</font></td>")
                            Else
                                maintTotalCost = 0.0
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Avg Labor Cost Per Flight Hour</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If maintLaborCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    maintLaborCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), maintLaborCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(maintLaborCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Avg Parts Per Flight Hour Cost</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If maintPartsCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    maintPartsCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), maintPartsCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(maintPartsCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Avg Labor Costs Per Man Hour</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If maintLaborCost > 0 Then

                                maintLaborCostManHour = maintLaborCost * maintLaborCostManHour

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    maintLaborCostManHour = CDec(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), maintLaborCostManHour))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + ConversionFunctions.Truncate(maintLaborCostManHour, 2) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Avg Parts Per Man Hour Cost</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If maintPartsCost > 0 Then

                                maintPartsCostManHour = maintPartsCost * maintPartsCostManHour

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    maintPartsCostManHour = CDec(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), maintPartsCostManHour))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + ConversionFunctions.Truncate(maintPartsCostManHour, 2) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">Engine Overhaul</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If Not IsDBNull(r("amod_engine_ovh_cost")) Then
                                maintEngineCost = CDbl(r.Item("amod_engine_ovh_cost").ToString)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    maintEngineCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), maintEngineCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(maintEngineCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">Thrust Reverse Overhaul</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If Not IsDBNull(r("amod_thrust_rev_ovh_cost")) Then
                                maintThrustCost = CDbl(r.Item("amod_thrust_rev_ovh_cost").ToString)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    maintThrustCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), maintThrustCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(maintThrustCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"" nowrap=""nowrap""><u>Miscellaneous&nbsp;Flight&nbsp;Expenses</u></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If Not IsDBNull(r("amod_land_park_cost")) Then
                                miscLandParkCost = CDbl(r.Item("amod_land_park_cost").ToString)
                            Else
                                miscLandParkCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_crew_exp_cost")) Then
                                miscCrewCost = CDbl(r.Item("amod_crew_exp_cost").ToString)
                            Else
                                miscCrewCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_supplies_cost")) Then
                                miscSupplyCost = CDbl(r.Item("amod_supplies_cost").ToString)
                            Else
                                miscSupplyCost = 0.0
                            End If

                            If (miscLandParkCost + miscCrewCost + miscSupplyCost) > 0 Then
                                ' recalculate every time
                                miscFlightTotalCost = CDbl(miscLandParkCost + miscCrewCost + miscSupplyCost)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    miscFlightTotalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), miscFlightTotalCost))
                                End If

                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(miscFlightTotalCost, 2, True, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"" nowrap=""nowrap"">&nbsp;&nbsp;Landing-Parking Fee</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If miscLandParkCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    miscLandParkCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), miscLandParkCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(miscLandParkCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Crew Expenses</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If miscCrewCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    miscCrewCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), miscCrewCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(miscCrewCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Supplies-Catering</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If miscSupplyCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    miscSupplyCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), miscSupplyCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(miscSupplyCost, 2, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><b>Total Direct Costs</b></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            ' recalculate totals every time
                            totalDirCostHour(xIndex) = 0
                            ' totalDirCostHour = CDbl(fuelTotCost) + CDbl(maintTotalCost) + CDbl(miscFlightTotalCost) + CDbl(maintEngineCost) + CDbl(maintThrustCost) 
                            totalDirCostHour(xIndex) = CDbl(fuelTotCost + maintTotalCost + miscFlightTotalCost + maintEngineCost + maintThrustCost)

                            If totalDirCostHour(xIndex) > 0 Then
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(totalDirCostHour(xIndex), 2, True, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If

                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap""><br />Block&nbsp;Speed&nbsp;" + ConversionFunctions.TranslateUSMetricUnitsLong("SM") + "s&nbsp;Per&nbsp;Hour</td>")
                            Else
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap""><br />Block&nbsp;Speed&nbsp;" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, ConversionFunctions.TranslateStatuteToNauticalMilesLong("SM"), ConversionFunctions.TranslateStatuteToNauticalMilesLong("NM")) + "s&nbsp;Per&nbsp;Hour</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If Not IsDBNull(r("amod_avg_block_speed")) Then
                                avgBlockSpeed(xIndex) = 0
                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    avgBlockSpeed(xIndex) = CDbl(ConversionFunctions.ConvertUSToMetricValue("SM", CDbl(r.Item("amod_avg_block_speed").ToString)))
                                Else
                                    If searchCriteria.ViewCriteriaUseStatuteMiles Then
                                        avgBlockSpeed(xIndex) = CDbl(r.Item("amod_avg_block_speed").ToString)
                                    Else
                                        avgBlockSpeed(xIndex) = CDbl(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(r.Item("amod_avg_block_speed").ToString)))
                                    End If
                                End If
                                htmlOut.Append("<td align=""right""><br />" + FormatNumber(avgBlockSpeed(xIndex), 0, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap"">Total Cost Per " + ConversionFunctions.TranslateUSMetricUnitsLong("SM") + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"" nowrap=""nowrap"">Total Cost Per " + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, ConversionFunctions.TranslateStatuteToNauticalMilesLong("SM"), ConversionFunctions.TranslateStatuteToNauticalMilesLong("NM")) + "</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            ' recalulate everytime
                            ' totalCostPerMile = CDbl(CDbl(totalDirCostHour) / CDbl(avgBlockSpeed))
                            totalCostPerMile(xIndex) = 0

                            If avgBlockSpeed(xIndex) > 0 Then
                                totalCostPerMile(xIndex) = CDbl(totalDirCostHour(xIndex) / avgBlockSpeed(xIndex))
                            End If

                            If totalCostPerMile(xIndex) > 0 Then
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(totalCostPerMile(xIndex), 2, True, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If

                        htmlOut.Append("</tr></table></td>") ' end direct costs per hour table

                        If bFirstOne = False Then

                            xIndex += 1

                        End If

                        If bFirstOne And SharedModelDatatable.Rows.Count > 1 Then
                            Exit For
                        End If

                    Next

                    htmlOut.Append("</tr><tr>" + vbCrLf)

                    If bFirstOne Then
                        htmlOut.Append("<th align=""left"" valign=""middle""><b>ANNUAL FIXED COSTS</b><br />&nbsp;</th>")
                    Else
                        htmlOut.Append("<th" + colspan + " align=""left"">" + sTitle.Trim + "</th>")
                    End If

                    htmlOut.Append("</tr><tr>" + vbCrLf)

                    xIndex = 0

                    '''''
                    '''''' second loop
                    '''''

                    For Each r As DataRow In SharedModelDatatable.Rows

                        htmlOut.Append("<td valign=""top"" align=""left"">" + vbCrLf)
                        ' start annual fixed costs table
                        htmlOut.Append("<table  cellspacing=""0""" & IIf(HttpContext.Current.Session.Item("isMobile"), " width=""100%""", "cellpadding=""2""") & ">" & vbCrLf)

                        crewTotalCost = 0.0
                        captSalaryCost = 0.0
                        coPilotSalaryCost = 0.0
                        benefitsCost = 0.0
                        hangarCost = 0.0
                        insuranceTotalCost = 0.0
                        insuranceHullCost = 0.0
                        insuranceLiabilityCost = 0.0
                        miscTrainCost = 0.0
                        miscModernCost = 0.0
                        miscNavCost = 0.0
                        miscTotalCost = 0.0
                        variableTotalCost = 0.0

                        If HttpContext.Current.Session.Item("isMobile") = False Then
                            htmlOut.Append("<tr valign=""middle"" align=""center"">")
                            If bFirstOne Then
                                htmlOut.Append("<td>&nbsp;</td>")
                            Else
                                If SharedModelDatatable.Rows.Count > 1 Then
                                    htmlOut.Append("<td colspan=""2"" valign=""middle"" align=""right"" nowrap=""nowrap"" style=""padding-right:3px;""><strong>" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</strong></td>")
                                Else
                                    htmlOut.Append("<td colspan=""2"">&nbsp;</td>")
                                End If
                            End If
                            htmlOut.Append("</tr>")
                        End If

                        htmlOut.Append("<tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><u>Crew Salaries</u></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If Not IsDBNull(r("amod_capt_salary_cost")) Then
                                captSalaryCost = CDbl(r.Item("amod_capt_salary_cost").ToString)
                            Else
                                captSalaryCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_cpilot_salary_cost")) Then
                                coPilotSalaryCost = CDbl(r.Item("amod_cpilot_salary_cost").ToString)
                            Else
                                coPilotSalaryCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_crew_benefit_cost")) Then
                                benefitsCost = CDbl(r.Item("amod_crew_benefit_cost").ToString)
                            Else
                                benefitsCost = 0.0
                            End If

                            If (captSalaryCost + coPilotSalaryCost + benefitsCost) > 0 Then
                                ' recalulate everytime
                                ' crewTotalCost = CDbl(System.Math.round(CDbl(captSalaryCost + coPilotSalaryCost + benefitsCost),0))
                                crewTotalCost = CDbl(captSalaryCost + coPilotSalaryCost + benefitsCost)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    crewTotalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), crewTotalCost))
                                End If

                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(crewTotalCost, 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Capt. Salary</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If captSalaryCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    captSalaryCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), captSalaryCost))
                                End If
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(captSalaryCost, 0, False, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Co-pilot Salary</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If coPilotSalaryCost > 0 Then
                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    coPilotSalaryCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), coPilotSalaryCost))
                                End If
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(coPilotSalaryCost, 0, False, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Benefits</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If benefitsCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    benefitsCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), benefitsCost))
                                End If
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(benefitsCost, 0, False, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">Hangar Cost</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If Not IsDBNull(r("amod_hangar_cost")) Then
                                hangarCost = CDbl(r.Item("amod_hangar_cost").ToString)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    hangarCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), hangarCost))
                                End If

                                If (hangarCost > 0) Then
                                    htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(hangarCost, 0, False, False, True) + "</td>")
                                Else
                                    htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><u>Insurance</u></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If Not IsDBNull(r("amod_hull_insurance_cost")) Then
                                insuranceHullCost = CDbl(r.Item("amod_hull_insurance_cost").ToString)
                            Else
                                insuranceHullCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_liability_insurance_cost")) Then
                                insuranceLiabilityCost = CDbl(r.Item("amod_liability_insurance_cost").ToString)
                            Else
                                insuranceLiabilityCost = 0.0
                            End If

                            If (insuranceHullCost + insuranceLiabilityCost) > 0 Then
                                ' recalulate everytime
                                ' insuranceTotalCost = CDbl(System.Math.round(CDbl(insuranceHullCost + insuranceLiabilityCost),0))
                                insuranceTotalCost = CDbl(insuranceHullCost + insuranceLiabilityCost)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    insuranceTotalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), insuranceTotalCost))
                                End If
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(insuranceTotalCost, 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Hull</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If insuranceHullCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    insuranceHullCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), insuranceHullCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(insuranceHullCost, 0, False, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Legal Liability</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If insuranceLiabilityCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    insuranceLiabilityCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), insuranceLiabilityCost))
                                End If
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(insuranceLiabilityCost, 0, False, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><u>Misc. Overhead</u></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If Not IsDBNull(r("amod_misc_train_cost")) Then
                                miscTrainCost = CDbl(r.Item("amod_misc_train_cost").ToString)
                            Else
                                miscTrainCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_misc_modern_cost")) Then
                                miscModernCost = CDbl(r.Item("amod_misc_modern_cost").ToString)
                            Else
                                miscModernCost = 0.0
                            End If

                            If Not IsDBNull(r("amod_misc_naveq_cost")) Then
                                miscNavCost = CDbl(r.Item("amod_misc_naveq_cost").ToString)
                            Else
                                miscNavCost = 0.0
                            End If

                            If (miscTrainCost + miscModernCost + miscNavCost) > 0 Then

                                ' recalulate everytime
                                ' miscTotalCost = CDbl(System.Math.round(CDbl(miscTrainCost + miscModernCost + miscNavCost),0))
                                miscTotalCost = CDbl(miscTrainCost + miscModernCost + miscNavCost)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    miscTotalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), miscTotalCost))
                                End If
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(miscTotalCost, 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Training</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If miscTrainCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    miscTrainCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), miscTrainCost))
                                End If
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(miscTrainCost, 0, False, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Modernization</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If miscModernCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    miscModernCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), miscModernCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(miscModernCost, 0, False, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Nav. Equipment</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If miscNavCost > 0 Then

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    miscNavCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), miscNavCost))
                                End If

                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(miscNavCost, 0, False, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">Depreciation</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            depreciationCost(xIndex) = 0.0

                            If Not IsDBNull(r("amod_deprec_cost")) Then

                                depreciationCost(xIndex) = CDbl(r.Item("amod_deprec_cost").ToString)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    depreciationCost(xIndex) = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), depreciationCost(xIndex)))
                                End If

                                If (depreciationCost(xIndex) > 0) Then
                                    htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(depreciationCost(xIndex), 0, False, False, True) + "</td>")
                                Else
                                    htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><b>Total Fixed Costs</b></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If (crewTotalCost + hangarCost + insuranceTotalCost + miscTotalCost + depreciationCost(xIndex)) > 0 Then
                                ' recalulate everytime
                                fixedTotalCost(xIndex) = 0
                                'fixedTotalCost(xIndex) = CDbl(System.Math.round((crewTotalCost + hangarCost + insuranceTotalCost + miscTotalCost + depreciationCost(xIndex)),0))
                                fixedTotalCost(xIndex) = CDbl(crewTotalCost + hangarCost + insuranceTotalCost + miscTotalCost + depreciationCost(xIndex))
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(fixedTotalCost(xIndex), 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If

                        htmlOut.Append("</tr></table></td>") ' end annual fixed costs table

                        If bFirstOne = False Then

                            xIndex += 1

                        End If

                        If bFirstOne And SharedModelDatatable.Rows.Count > 1 Then
                            Exit For
                        End If

                    Next

                    htmlOut.Append("</tr><tr>" + vbCrLf)

                    If bFirstOne Then
                        htmlOut.Append("<th align=""left"" valign=""middle""><b>ANNUAL&nbsp;BUDGET</b><br />&nbsp;</th>")
                    Else
                        htmlOut.Append("<th" + colspan + " align=""left"">" + sTitle.Trim + "</th>")
                    End If

                    htmlOut.Append("</tr><tr>" + vbCrLf)

                    xIndex = 0

                    '''''
                    '''''' third loop
                    '''''

                    For Each r As DataRow In SharedModelDatatable.Rows

                        htmlOut.Append("<td valign=""top"" align=""left"">" + vbCrLf)

                        ' start annual budget table
                        htmlOut.Append("<table cellspacing=""0"" " & IIf(HttpContext.Current.Session.Item("isMobile"), " width=""100%""", "cellpadding=""2"" ") & ">" & vbCrLf)

                        If HttpContext.Current.Session.Item("isMobile") = False Then
                            htmlOut.Append("<tr valign=""middle"" align=""center"">")
                            If bFirstOne Then
                                htmlOut.Append("<td>&nbsp;</td>")
                            Else
                                If SharedModelDatatable.Rows.Count > 1 Then
                                    htmlOut.Append("<td colspan=""2"" valign=""middle"" align=""right"" nowrap=""nowrap"" style=""padding-right:3px;""><strong>" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</strong></td>")
                                Else
                                    htmlOut.Append("<td colspan=""2"">&nbsp;</td>")
                                End If
                            End If
                            htmlOut.Append("</tr>")
                        End If

                        htmlOut.Append("<tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Number of Seats</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If Not IsDBNull(r("amod_number_of_seats")) Then
                                If (CLng(r.Item("amod_number_of_seats").ToString) > 0) Then
                                    htmlOut.Append("<td align=""right"">" + FormatNumber(CDbl(r.Item("amod_number_of_seats").ToString), 0, False, False, True) + "</td>")
                                Else
                                    htmlOut.Append("<td align=""right"">0</td>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right"">0</td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        annualMiles = 0
                        annualHrs = 0

                        costPerHourFixDir = 0.0
                        costPerMileFixDir = 0.0
                        costPerSeatFixDir = 0.0
                        noDepTotalCost = 0.0
                        costPerHourNoDep = 0.0
                        costPerMileNoDep = 0.0
                        costPerSeatNoDep = 0.0

                        If bFirstOne Then

                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;" + ConversionFunctions.TranslateUSMetricUnitsLong("M") + "s</td>")
                            Else
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, ConversionFunctions.TranslateStatuteToNauticalMilesLong("SM"), ConversionFunctions.TranslateStatuteToNauticalMilesLong("NM")) + "s</td>")
                            End If

                        Else

                            htmlOut.Append("<td>&nbsp;</td>")

                            If Not IsDBNull(r("amod_annual_miles")) Then

                                If searchCriteria.ViewCriteriaUseMetricValues Then
                                    annualMiles = CDbl(ConversionFunctions.ConvertUSToMetricValue("M", CDbl(r.Item("amod_annual_miles").ToString)))
                                Else
                                    If searchCriteria.ViewCriteriaUseStatuteMiles Then
                                        annualMiles = CDbl(CDbl(r.Item("amod_annual_miles").ToString))
                                    Else
                                        annualMiles = CDbl(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(r.Item("amod_annual_miles").ToString)))
                                    End If
                                End If

                                If (annualMiles > 0) Then
                                    htmlOut.Append("<td align=""right"">" + FormatNumber(annualMiles, 0, False, False, True) + "</td>")
                                Else
                                    htmlOut.Append("<td align=""right"">0</td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""right"">0</td>")
                            End If

                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Hours</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If annualMiles > 0 Then
                                ' recalulate everytime
                                If avgBlockSpeed(xIndex) > 0 Then
                                    annualHrs = CDbl(CDbl(annualMiles / avgBlockSpeed(xIndex)))
                                End If
                                htmlOut.Append("<td align=""right""><font color=""red"">" + FormatNumber(annualHrs, 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0</font></td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)
                        If HttpContext.Current.Session.Item("isMobile") = False Then
                            htmlOut.Append("<td>&nbsp;</td>")
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><b>Total Direct Costs</b></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If annualHrs > 0 Then
                                ' recalulate everytime
                                totalDirCostYR(xIndex) = 0.0
                                totalDirCostYR(xIndex) = CDbl(totalDirCostHour(xIndex) * annualHrs)

                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(totalDirCostYR(xIndex), 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><b>Total Fixed Costs</b></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If fixedTotalCost(xIndex) > 0 Then
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(fixedTotalCost(xIndex), 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><b>Total Variable Costs</b></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If Not IsDBNull(r("amod_variable_costs")) Then
                                variableTotalCost = CDbl(r.Item("amod_variable_costs").ToString)

                                If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
                                    variableTotalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString), variableTotalCost))
                                End If

                                If (variableTotalCost > 0) Then
                                    htmlOut.Append("<td align=""right"">" + sCurrencySymbol + FormatNumber(variableTotalCost, 0, False, False, True) + "</td>")
                                Else
                                    htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right"">" + sCurrencySymbol + "0.00</td>")
                            End If
                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)
                        If HttpContext.Current.Session.Item("isMobile") = False Then
                            htmlOut.Append("<td>&nbsp;</td>")
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"" nowrap='nowrap'><u>Total Cost (Fixed &amp; Direct w/Depreciation)</u></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If (totalDirCostYR(xIndex) + fixedTotalCost(xIndex)) > 0 Then
                                ' recalulate everytime
                                totalFixedDirect(xIndex) = 0.0
                                totalFixedDirect(xIndex) = CDbl(totalDirCostYR(xIndex) + fixedTotalCost(xIndex))

                                If totalFixedDirect(xIndex) > 0 Then
                                    htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(totalFixedDirect(xIndex), 0, False, False, True) + "</font></td>")
                                Else
                                    htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/Hour</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If annualHrs > 0 Then
                                ' recalulate everytime
                                If annualHrs > 0 Then
                                    costPerHourFixDir = CDbl(totalFixedDirect(xIndex) / annualHrs)
                                End If

                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(costPerHourFixDir, 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/" + ConversionFunctions.TranslateUSMetricUnitsLong("SM") + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, ConversionFunctions.TranslateStatuteToNauticalMilesLong("SM"), ConversionFunctions.TranslateStatuteToNauticalMilesLong("NM")) + "</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If annualMiles > 0 Then
                                ' recalulate everytime
                                If annualMiles > 0 Then
                                    costPerMileFixDir = CDbl(totalFixedDirect(xIndex) / annualMiles)
                                End If

                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(costPerMileFixDir, 2, True, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/Seat " + ConversionFunctions.TranslateUSMetricUnitsLong("M") + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/Seat " + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, ConversionFunctions.TranslateStatuteToNauticalMilesLong("SM"), ConversionFunctions.TranslateStatuteToNauticalMilesLong("NM")) + "</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If Not IsDBNull(r("amod_number_of_seats")) Then
                                If CDbl(r.Item("amod_number_of_seats").ToString) > 0 Then
                                    ' recalulate everytime
                                    If CDbl(r.Item("amod_number_of_seats").ToString) > 0 Then
                                        costPerSeatFixDir = CDbl(costPerMileFixDir / CDbl(r.Item("amod_number_of_seats").ToString))
                                    End If
                                    htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(costPerSeatFixDir, 2, True, False, True) + "</font></td>")
                                Else
                                    htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If

                        End If

                        htmlOut.Append("</tr><tr>" + vbCrLf)
                        If HttpContext.Current.Session.Item("isMobile") = False Then
                            htmlOut.Append("<td colspan=""2"">&nbsp;</td>")
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left""><u>Total Cost (No Depreciation)</u></td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If (totalFixedDirect(xIndex) - depreciationCost(xIndex)) > 0 Then
                                ' recalulate everytime
                                noDepTotalCost = CDbl(totalFixedDirect(xIndex) - depreciationCost(xIndex))

                                If noDepTotalCost > 0 Then
                                    htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(noDepTotalCost, 0, False, False, True) + "</font></td>")
                                Else
                                    htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/Hour</td>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                            If annualHrs > 0 Then

                                ' recalulate everytime
                                If annualHrs > 0 Then
                                    costPerHourNoDep = CDbl(noDepTotalCost / annualHrs)
                                End If

                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(costPerHourNoDep, 0, False, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If
                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/" + ConversionFunctions.TranslateUSMetricUnitsLong("SM") + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/" + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, ConversionFunctions.TranslateStatuteToNauticalMilesLong("SM"), ConversionFunctions.TranslateStatuteToNauticalMilesLong("NM")) + "</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If annualMiles > 0 Then
                                ' recalulate everytime
                                If annualMiles > 0 Then
                                    costPerMileNoDep = CDbl(noDepTotalCost / annualMiles)
                                End If

                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(costPerMileNoDep, 2, True, False, True) + "</font></td>")
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If

                        End If
                        htmlOut.Append("</tr><tr>" + vbCrLf)

                        If bFirstOne Then
                            If searchCriteria.ViewCriteriaUseMetricValues Then
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/Seat " + ConversionFunctions.TranslateUSMetricUnitsLong("M") + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"">&nbsp;&nbsp;Cost/Seat " + IIf(searchCriteria.ViewCriteriaUseStatuteMiles, ConversionFunctions.TranslateStatuteToNauticalMilesLong("SM"), ConversionFunctions.TranslateStatuteToNauticalMilesLong("NM")) + "</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")

                            If Not IsDBNull(r("amod_number_of_seats")) Then

                                If CInt(r.Item("amod_number_of_seats").ToString) > 0 Then

                                    ' recalulate everytime
                                    If CDbl(r.Item("amod_number_of_seats").ToString) > 0 Then
                                        costPerSeatNoDep = CDbl(costPerMileNoDep / CInt(r.Item("amod_number_of_seats").ToString))
                                    End If

                                    htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + FormatNumber(costPerSeatNoDep, 2, True, False, True) + "</font></td>")
                                Else
                                    htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right""><font color=""red"">" + sCurrencySymbol + "0.00</font></td>")
                            End If

                        End If

                        htmlOut.Append("</tr></table></td>")  ' close annual budget table

                        If bFirstOne = False Then

                            xIndex = xIndex + 1

                        End If

                        If bFirstOne And SharedModelDatatable.Rows.Count > 1 Then
                            Exit For
                        End If

                    Next

                    'close inner table
                    htmlOut.Append("</tr></table></td>")

                End If

            End If

        Catch ex As Exception
            aError = "Error in views_display_operating_costs(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bFirstOne As Boolean) As String " + ex.Message
        Finally
        End Try

        out_htmlString = htmlOut.ToString()

        htmlOut = Nothing

    End Sub

    Public Function get_model_events_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef isCRMViewActive As Boolean, ByVal weekdateValue As String, ByVal eventCategory As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ")

            If weekdateValue = "" Then
                sQuery.Append(" TOP 40 ")
            End If

            sQuery.Append(" priorev_entry_date, ac_mfr_year, ac_ser_no_full, ac_reg_no, ac_id, priorev_subject, priorev_description, amod_make_name FROM Priority_Events WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Priority_Events_category WITH(NOLOCK) ON priorevcat_category_code = priorev_category_code INNER JOIN aircraft WITH(NOLOCK) ON")
            sQuery.Append(" priorev_ac_id = ac_id AND ac_journ_id = 0 INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")


            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next

                sQuery.Append(" WHERE amod_id IN (" + tmpStr.Trim + ")")
            ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then ' NOTE : need to update clause if new market status types are added
                sQuery.Append(" AND priorev_category_code NOT IN ('CA','EXOFF','EXON','MA','OM','OMNS','SALEP','SC','SPTOIM')")
                sQuery.Append(" AND priorevcat_category <> 'Market Status'")
            End If


            If weekdateValue = "" Then
                sQuery.Append(Constants.cAndClause + " priorev_entry_date >= (getdate()-90)")
            Else
                sQuery.Append(Constants.cAndClause + " priorev_entry_date >='" & weekdateValue & "'  ")
            End If

            If eventCategory <> "" Then
                sQuery.Append(Constants.cAndClause + " priorevcat_category = '" & eventCategory & "'  ")
            End If


            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If



            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))

            sQuery.Append(" ORDER BY priorev_id DESC")


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
                aError = "Error in get_model_events_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_model_events_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_model_events(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef isCRMViewActive As Boolean, ByVal eventCategory As String, ByVal eventDate As String, ByVal headerText As String)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Try


            htmlOut.Append("<table border='0' width='100%' cellpadding='2' cellspacing='0'>")
            htmlOut.Append("<tr><td valign='middle' class='header' align='center' colspan='6'>RECENT EVENTS <em>(" & headerText.ToString & ")</em></td></tr>")
            htmlOut.Append("<tr ><td align='left' colspan='6'>")

            results_table = get_model_events_info(searchCriteria, isCRMViewActive, eventDate, eventCategory)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table width='100%' border='0' cellpadding='4' cellspacing='0'>")
                    htmlOut.Append("<tr><td align='center' class='seperator'><strong>Date:</strong></td>")
                    htmlOut.Append("<td align='left' class='seperator'><strong>Status</strong></td><td align='left' class='seperator'><strong>Serial#</strong></td>")
                    htmlOut.Append("<td align='left' class='seperator'><strong>Reg#</strong></td><td align='center' class='seperator'><strong>Year MFR</strong></td>")
                    htmlOut.Append("<td align='left' class='seperator'><strong>Event Description</strong></td></tr>" + vbCrLf)

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        If Not IsDBNull(r("priorev_entry_date")) Then
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><em>" + FormatDateTime(r.Item("priorev_entry_date").ToString, DateFormat.GeneralDate) + "</em></td>")
                        Else
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><em>&nbsp;</em></td>")
                        End If

                        If Not IsDBNull(r("priorev_subject")) Then
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + r.Item("priorev_subject").ToString.Trim + "</td>")
                        Else
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>&nbsp;</td>")
                        End If

                        If Not IsDBNull(r("ac_ser_no_full")) Then
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><a target='_blank' href='DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "'>" + r.Item("ac_ser_no_full").ToString + "</a></td>")
                        Else
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>&nbsp;</td>")
                        End If

                        If Not IsDBNull(r("ac_reg_no")) Then
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + r.Item("ac_reg_no").ToString + "</td>")
                        Else
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>&nbsp;</td>")
                        End If

                        If Not IsDBNull(r("ac_mfr_year")) Then
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" + r.Item("ac_mfr_year").ToString + "</td>")
                        Else
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>&nbsp;</td>")
                        End If

                        If Not IsDBNull(r("priorev_description")) Then
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + Left(r.Item("priorev_description").ToString, 45).ToString + "</td></tr>")
                        Else
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>&nbsp;</td></tr>")
                        End If

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("No Events at this time, for this Make/Model ...")
                End If

            Else
                htmlOut.Append("No Events at this time, for this Make/Model ...")
            End If

            htmlOut.Append("</td></tr></table>" & vbCrLf)

        Catch ex As Exception

            aError = "Error in views_display_model_events(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub


    Public Function get_retail_sales_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal internal_flag As String, ByVal retail_flag As String, ByVal last_date As String, Optional ByVal jetnet_string As String = "", Optional ByVal months_to_Show As Integer = 0, Optional ByVal years_of As String = "", Optional ByVal aftt_within As String = "", Optional ByVal use_only_used As String = "", Optional ByVal extra_criteria As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim YearDateVariable As String = ""
        Dim start_date As String = ""
        Dim AclsData_Temp As New clsData_Manager_SQL

        Try

            'Query = "SELECT TOP 20 journ_id, journ_subcategory_code, journ_date, journ_subject, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, amod_make_name"
            'sQuery.Append(" FROM Journal_Summary WITH(NOLOCK)"
            'sQuery.Append(" WHERE ac_amod_id = " & inModelID
            'sQuery.Append(" AND (journ_date BETWEEN (getdate()-90) AND (getdate()+1))"
            'sQuery.Append(" AND (journ_subcat_code_part1 = 'WS') "
            'sQuery.Append(" AND (journ_subcat_code_part3 NOT IN ('DB','DS','FI','MF','FY','RE','IT','RR')) "
            'sQuery.Append(" AND (journ_subcategory_code NOT LIKE '%IT%')"
            'sQuery.Append(" AND (journ_internal_trans_flag = 'N')"
            'sQuery.Append(MakeAircraftProductCodeClause(session("Product_Code"), False, False)
            'sQuery.Append(" ORDER BY journ_date DESC"
            If Trim(jetnet_string) <> "" Then

                sQuery.Append("SELECT distinct ")
                sQuery.Append(" ac_id,  ac_ser_no_full, emp_program_name,  journ_id, 0 as client_jetnet_trans_id, journ_customer_note, ac_ser_no_sort ")

                If InStr(jetnet_string, "ac_list_date") = 0 Then
                    sQuery.Append(" ,ac_list_date ")
                End If

                If InStr(jetnet_string, "ac_airframe_tot_hrs") = 0 Then
                    sQuery.Append(" ,ac_airframe_tot_hrs ")
                End If

                If InStr(jetnet_string, "ac_asking_price") = 0 Then
                    sQuery.Append(" ,ac_asking_price ")
                End If

                If Trim(jetnet_string) <> "" Then
                    sQuery.Append(", ")
                    sQuery.Append(jetnet_string)
                End If
                sQuery.Append(", ac_sale_price_display_flag, case  when ac_asking IS NULL  then '' else ac_asking end as ac_asking, journ_id")

                sQuery.Append(",ac_engine_1_soh_hrs, ac_engine_2_soh_hrs ")

                sQuery.Append(" FROM aircraft WITH (NOLOCK) ")

                sQuery.Append(" inner JOIN aircraft_reference WITH (NOLOCK) ON aircraft_reference.cref_ac_id = ac_id and cref_journ_id = ac_journ_id ")
                sQuery.Append(" inner JOIN company with (NOLOCK) on  aircraft_reference.cref_comp_id = comp_id and cref_journ_id = comp_journ_id ")
                sQuery.Append(" inner JOIN aircraft_model WITH (NOLOCK) ON aircraft.ac_amod_id = aircraft_model.amod_id ")
                sQuery.Append(" inner JOIN aircraft_contact_type WITH (NOLOCK) ON aircraft_reference.cref_contact_type = aircraft_contact_type.actype_code ")
                sQuery.Append(" INNER Join Engine_Maintenance_Program WITH(NOLOCK) ON aircraft.ac_engine_maintenance_prog_EMP = Engine_Maintenance_Program.emp_id ")

                sQuery.Append(" left outer join contact with (NOLOCK) on cref_contact_id = contact_id and cref_journ_id = contact_journ_id and contact_active_flag='Y' and contact_hide_flag='N' ")
                sQuery.Append(" left outer join Journal on journ_id = ac_journ_id  ")
                sQuery.Append(" left outer join Journal_Category on jcat_subcategory_code  = journ_subcategory_code ")
            Else
                sQuery.Append("SELECT  journ_id, journ_subcategory_code, emp_program_name, journ_date, journ_subject, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, ac_asking_price, ac_ser_no_sort, amod_make_name")
                sQuery.Append(", ac_list_date, ac_airframe_tot_hrs, journ_customer_note ")
                sQuery.Append(",ac_engine_1_soh_hrs, ac_engine_2_soh_hrs ")

                If HttpContext.Current.Session.Item("isMobile") = True Then
                    sQuery.Append(", ac_year, amod_id, amod_model_name, ac_forsale_flag, ac_delivery, jcat_subcategory_name  ")
                End If

                sQuery.Append(", case when ac_sale_price > 0 and ac_sale_price_display_flag = 'Y' ")
                sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())
                sQuery.Append(" then ac_sale_price else '' end as ac_sold_price, ac_airframe_tot_hrs, '' as clitrans_value_description ")
                sQuery.Append(", (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE cref_ac_id =ac_id AND cref_journ_id = ac_journ_id AND ((cref_contact_type = '99') OR (cref_contact_type = '93')) ) as BROKER ")


                sQuery.Append(" , ac_sale_price_display_flag, case  when ac_asking IS NULL  then '' else ac_asking end as ac_asking, ac_status ")
                sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id")
                sQuery.Append(" INNER Join Engine_Maintenance_Program WITH(NOLOCK) ON aircraft.ac_engine_maintenance_prog_EMP = Engine_Maintenance_Program.emp_id ")
                sQuery.Append(" INNER JOIN journal_category WITH(NOLOCK) ON journ_subcategory_code = jcat_subcategory_code")

            End If



            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next

                sQuery.Append(" WHERE amod_id IN (" + tmpStr.Trim + ")")
            ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If Trim(use_only_used) <> "" Then
                sQuery.Append(use_only_used)
            End If

            If Trim(years_of) <> "" And Trim(years_of) <> "0" Then
                sQuery.Append(years_of)
            End If

            If Trim(aftt_within) <> "" And Trim(aftt_within) <> "0" Then
                sQuery.Append(aftt_within)
            End If

            If Trim(extra_criteria) <> "" Then
                sQuery.Append(extra_criteria)
            End If

            'subcat code part3 and date modified/removed per Rick on 4/30/2014
            '  sQuery.Append(" AND ((jcat_category_code = 'AH') and (journ_subcat_code_part1='WS') )") 'AND (journ_subcat_code_part3 NOT IN ('DB','DS','FI','MF','FY','RE','IT','RR'))
            '  sQuery.Append(" and NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) ")

            'added MSW - 5/17/2016 - 
            ' Dim AclsData_Temp As New clsData_Manager_SQL
            'sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())

            sQuery.Append(" AND (journ_subcat_code_part1 = 'WS') ")      '-- Whole Sales Only 


            ' only do if there is no extra ( only do if not from pdf) 
            If Trim(extra_criteria) = "" Or InStr(Trim(extra_criteria), "and ac_asking_price is not NULL") > 0 Then
                If CDbl(months_to_Show) > 0 Then
                    start_date = Date.Now()


                    If Month(start_date) = 12 Or Month(start_date) = 11 Or Month(start_date) = 10 Then '10/16/2016 - 1/1/2017 - 1/1/2014
                        start_date = "1/1/" & (Year(start_date) + 1)
                    ElseIf Month(start_date) = 7 Or Month(start_date) = 8 Or Month(start_date) = 9 Then '8/16/2016 - 10/1/2016 - 10/1/2013
                        start_date = "10/1/" & Year(start_date)
                    ElseIf Month(start_date) = 4 Or Month(start_date) = 5 Or Month(start_date) = 6 Then '5/16/2016 - 7/1/2016 - 7/1/2013
                        start_date = "7/1/" & Year(start_date)
                    Else '1,2,3     '3/16/2016 - 3/1/2016 - 4/1/2013
                        start_date = "4/1/" & Year(start_date)
                    End If

                    YearDateVariable = Year(DateAdd(DateInterval.Month, -CDbl(months_to_Show), CDate(start_date))) & "-" & Month(DateAdd(DateInterval.Month, -CDbl(months_to_Show), CDate(start_date))) & "-" & Day(DateAdd(DateInterval.Month, -CDbl(months_to_Show), CDate(start_date)))
                ElseIf Trim(last_date) = "" Then
                    YearDateVariable = Year(DateAdd(DateInterval.Year, -1, Now())) & "-" & Month(DateAdd(DateInterval.Year, -1, Now())) & "-" & Day(DateAdd(DateInterval.Year, -1, Now()))
                Else
                    YearDateVariable = Year(DateAdd(DateInterval.Year, -1, CDate(last_date))) & "-" & Month(DateAdd(DateInterval.Year, -1, CDate(last_date))) & "-" & Day(DateAdd(DateInterval.Year, -1, CDate(last_date)))
                End If

                sQuery.Append(" AND journ_date >= '" & YearDateVariable & "' ")

                If Trim(last_date) <> "" Then
                    YearDateVariable = Year(DateAdd(DateInterval.Year, 0, CDate(last_date))) & "-" & Month(DateAdd(DateInterval.Year, 0, CDate(last_date))) & "-" & Day(DateAdd(DateInterval.Year, 0, CDate(last_date)))
                    sQuery.Append(" AND journ_date <= '" & YearDateVariable & "' ")
                End If
            End If

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + "  ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + "  ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + "  ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + "  ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If


            If Trim(internal_flag) = "N" Then
                sQuery.Append(" AND  journ_internal_trans_flag = 'N' ")
            End If

            If Trim(retail_flag) = "Y" Then
                sQuery.Append(" AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) ")
            End If


            If Trim(last_date) <> "" Then

            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))





            sQuery.Append(" ORDER BY journ_date DESC")

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
                aError = "Error in get_retail_sales_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_retail_sales_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_recent_retail_sales(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal internal_flag As String, ByVal retail_flag As String, Optional ByVal PreOwnedSales As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim temp_header As String = ""
        Dim temp_status As String = ""
        Dim aftt_number As String = ""
        Dim extra_criteria As String = ""
        Try

            extra_criteria = " AND journ_date >= '" & Year(DateAdd(DateInterval.Month, -CDbl(searchCriteria.ViewCriteriaTimeSpan), Now())) & "-" & Month(DateAdd(DateInterval.Month, -CDbl(searchCriteria.ViewCriteriaTimeSpan), Now())) & "-" & Day(DateAdd(DateInterval.Month, -CDbl(searchCriteria.ViewCriteriaTimeSpan), Now())) & "' "

            results_table = get_retail_sales_info(searchCriteria, internal_flag, retail_flag, "", "", searchCriteria.ViewCriteriaTimeSpan, "", "", IIf(PreOwnedSales, "  AND journ_newac_flag = 'N'  ", ""), extra_criteria)
            htmlOut.Append("<span id=""RetailNewWindowContents"">")
            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    htmlOut.Append("<table width='100%' border='0' cellpadding='3' cellspacing='0' id=""retailSalesCopy"">")
                    htmlOut.Append("<thead><tr><th class='seperator' align='center' width=""35"">SEL</th>")
                    htmlOut.Append("<th class='seperator' align='center'>HIDDEN IDS</th>")
                    htmlOut.Append("<th align='left' class='seperator'><strong>SER#</strong></th><th align='left' class='seperator'><strong>REG #</strong></th>")
                    htmlOut.Append("<th class='seperator' align='center'><strong>YEAR MFR</strong></th>")
                    htmlOut.Append("<th align='center' class='seperator'><strong>DATE</strong></th><th align='left' class='seperator'><strong>TRANSACTION INFO</strong></th>")
                    If HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                        htmlOut.Append("<th class='seperator' align='center'><strong>ASKING ($k)</strong></th>")
                    End If

                    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                        htmlOut.Append("<th class='seperator' align='center'><strong>SALE PRICE ($k)</strong></th>")
                    End If
                    htmlOut.Append("<th class='seperator' align='center'><strong>AFTT</strong></th>")
                    htmlOut.Append("<th class='seperator' align='center'><strong>ENGINE MAINTENANCE PROGRAM</strong></th>")
                    If HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                        htmlOut.Append("<th class='seperator' align='center'><strong>BROKER</strong></th>")
                    End If
                    htmlOut.Append("<th class='seperator' align='center'><strong>ENG 1</br>SOH</strong></th>")
                    htmlOut.Append("<th class='seperator' align='center'><strong>ENG 2</br>SOH</strong></th>")


                    htmlOut.Append("</tr></thead>" + vbCrLf)

                    htmlOut.Append("<tbody>")
                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If
                        htmlOut.Append("<td class='seperator' align='center' width=""35""></td>")
                        htmlOut.Append("<td class='seperator' align='center'>" & r.Item("journ_id").ToString & "</td>")

                        If Not IsDBNull(r("ac_ser_no_full")) Then
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator' data-sort='" & IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort").ToString, "") & "'>")
                            htmlOut.Append(DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, r.Item("journ_id"), True, r.Item("ac_ser_no_full").ToString, "underline", ""))
                            htmlOut.Append("</td>")
                        Else
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator' data-sort=''> </td>")
                        End If

                        If Not IsDBNull(r("ac_reg_no")) Then
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + r.Item("ac_reg_no").ToString + "</td>")
                        Else
                            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                        End If

                        If Not IsDBNull(r("ac_mfr_year")) Then
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" + r.Item("ac_mfr_year").ToString + "</td>")
                        Else
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'> </td>")
                        End If


                        If Not IsDBNull(r("journ_date")) Then
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator' data-sort='" & Format(r.Item("journ_date"), "yyyy/MM/dd") & "'><em>" + FormatDateTime(r.Item("journ_date").ToString, DateFormat.GeneralDate) + "</em></td>")
                        Else
                            htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator' data-sort=''><em> </em></td>")
                        End If

                        htmlOut.Append("<td align='left' valign='top' class='seperator'>")
                        htmlOut.Append("<a " & DisplayFunctions.WriteDetailsLink(r.Item("ac_id").ToString, 0, 0, r.Item("journ_id").ToString, False, "", "underline", "") & ">")
                        'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=" + r.Item("journ_id").ToString + """,""AircraftDetails"");' title='Display Aircraft Details'>")

                        If Not IsDBNull(r("journ_subject")) Then
                            htmlOut.Append(Left(r.Item("journ_subject").ToString, 90).ToString)
                        Else
                            htmlOut.Append(" ")
                        End If

                        If Not IsDBNull(r("journ_customer_note")) Then
                            If Not String.IsNullOrEmpty(r.Item("journ_customer_note")) Then
                                htmlOut.Append(" (<span class=""help_cursor error_text no_text_underline"" title=""" + r.Item("journ_customer_note").ToString + """>Note</span>)")
                            End If
                        End If
                        htmlOut.Append("</a></td>")

                        If HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                            If Not IsDBNull(r("ac_asking")) Then
                                If Trim(r("ac_asking")) = "Price" Then
                                    If Not IsDBNull(r("ac_asking_price")) Then
                                        htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator' >" & FormatNumber((r.Item("ac_asking_price").ToString / 1000), 0) & "</td>")
                                    Else
                                        htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator' > </td>")
                                    End If
                                Else
                                    If Not IsDBNull(r("ac_status")) Then
                                        If Trim(r("ac_status")) = "Not for Sale" Then
                                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>OFFMKT </td>")
                                        Else
                                            '  temp_status = Trim(r.Item("ac_status").ToString)
                                            ' temp_status = Replace(Trim(temp_status), "For Sale", "<A href='' title='For Sale' Name='For Sale'>FS</a>")
                                            ' temp_status = Replace(Trim(temp_status), "Lease", "<A href='' title='Lease' Name='Lease'>LS</a>")
                                            ' temp_status = Replace(Trim(temp_status), "Sale Pending", "<A href='' title='Sale Pending' Name='Sale Pending'>SP</a>")
                                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator' >M/O </td>")
                                        End If
                                    Else
                                        htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator' > </td>")
                                    End If
                                End If
                            Else
                                If Not IsDBNull(r("ac_asking_price")) Then
                                    htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator' >" & FormatNumber((r.Item("ac_asking_price").ToString / 1000), 0) & "</td>")
                                ElseIf Not IsDBNull(r("ac_status")) Then
                                    If Trim(r("ac_status")) = "Not for Sale" Then
                                        htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>OFFMKT </td>")
                                    Else
                                        htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>OFFMKT </td>")
                                    End If
                                Else
                                    htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>OFFMKT </td>")
                                End If
                            End If
                        End If

                        If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                            If Not IsDBNull(r("ac_sold_price")) Then
                                If Trim(r("ac_sold_price")) <> "" And Trim(r("ac_sold_price")) <> "0" Then
                                    htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator' >" & DisplayFunctions.TextToImage(FormatNumber((r.Item("ac_sold_price").ToString / 1000), 0) & "k", 7, "", "42", "Reported Sale Price Displayed with Permission from Source") & "</td>")
                                Else
                                    htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><a href='' onclick=""javascript:load('SendSalesTransaction.aspx?sendSales=true&ModelID=" & searchCriteria.ViewCriteriaAmodID.ToString & "&jID=" & r("journ_id").ToString & "&acid=" & r("ac_id").ToString & "','','scrollbars=yes,menubar=no,height=438,width=800,resizable=yes,toolbar=no,location=no,status=no');return false;"" class='gray_text'>ENTER</a></td>")
                                End If
                            Else
                                htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><a href='' onclick=""javascript:load('SendSalesTransaction.aspx?sendSales=true&ModelID=" & searchCriteria.ViewCriteriaAmodID.ToString & "&jID=" & r("journ_id").ToString & "&acid=" & r("ac_id").ToString & "','','scrollbars=yes,menubar=no,height=438,width=800,resizable=yes,toolbar=no,location=no,status=no');return false;"" class='gray_text'>ENTER</a></td>")
                            End If
                        End If


                        aftt_number = "0"
                        If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
                            If r.Item("ac_airframe_tot_hrs").ToString <> "" Then
                                aftt_number = r.Item("ac_airframe_tot_hrs").ToString
                                aftt_number = Replace(aftt_number, ",", "")

                                If Len(aftt_number) = 6 Then
                                    aftt_number = aftt_number
                                ElseIf Len(aftt_number) = 5 Then
                                    aftt_number = "0" & aftt_number
                                ElseIf Len(aftt_number) = 4 Then
                                    aftt_number = "00" & aftt_number
                                ElseIf Len(aftt_number) = 3 Then
                                    aftt_number = "000" & aftt_number
                                ElseIf Len(aftt_number) = 2 Then
                                    aftt_number = "0000" & aftt_number
                                ElseIf Len(aftt_number) = 1 Then
                                    aftt_number = "00000" & aftt_number
                                End If

                            End If
                        End If


                        If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'  data-sort='" & aftt_number & "'>" & FormatNumber(r.Item("ac_airframe_tot_hrs").ToString, 0) & "</td>")
                        Else
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator' data-sort='" & aftt_number & "'> </td>")
                        End If

                        If Not IsDBNull(r("emp_program_name")) Then
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & r.Item("emp_program_name").ToString & "</td>")
                        Else
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'> </td>")
                        End If


                        If HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                            If Not IsDBNull(r("BROKER")) Then
                                htmlOut.Append("<td align='left' valign='top' class='seperator' >" & r.Item("BROKER").ToString & "</td>")
                            Else
                                htmlOut.Append("<td align='left' valign='top' class='seperator'> </td>")
                            End If
                        End If


                        If Not IsDBNull(r("ac_engine_1_soh_hrs")) Then
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & r.Item("ac_engine_1_soh_hrs").ToString & "</td>")
                        Else
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'> </td>")
                        End If

                        If Not IsDBNull(r("ac_engine_2_soh_hrs")) Then
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & r.Item("ac_engine_2_soh_hrs").ToString & "</td>")
                        Else
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'> </td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next
                    htmlOut.Append("</tbody>")
                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("No Retail Sales at this time, for this Make/Model ...")
                End If

            Else
                htmlOut.Append("No Retail Sales at this time, for this Make/Model ...")
            End If

            htmlOut.Append("</td></tr></table></span><div class=""resizeCWRetail""><div id=""RetailInnerTable"" style=""width: 100%;""></div></div>")

            temp_header = temp_header & ("<table border='0' width='100%' cellpadding='2' cellspacing='0' class=""mobileWidth"">")
            temp_header = temp_header & ("<tr><td valign='middle' class='header' align='center'>RECENT SALES <em>(Last " & searchCriteria.ViewCriteriaTimeSpan.ToString & " Months)</em></td></tr>")
            temp_header = temp_header & ("<tr><td align='left'>")

        Catch ex As Exception

            aError = "Error in views_display_recent_retail_sales(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = temp_header & htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Function FormatUserData(ByVal s_inData, ByVal t_inUserDataType, ByVal b_isDisplay)

        FormatUserData = ""

        If s_inData <> "" Then
            If Not b_isDisplay Then

                ' Clean up any "ALL" or "All" that happens to get through
                FormatUserData = CleanUserData(s_inData, "All,", Constants.cEmptyString, False)
                FormatUserData = CleanUserData(s_inData, "ALL,", Constants.cEmptyString, False)

                Select Case t_inUserDataType
                    Case Constants.gtUSRHTMLSELECTNUM
                        ' take a list from HTML and format it for SQL Clause 123, 346, 789 to 123,346,789 
                        FormatUserData = CleanUserData(FormatUserData, Constants.cMultiDelim, Constants.cCommaDelim, False)
                        Exit Function '- String is ready to be split or used for selection

                    Case Constants.gtUSRMULTISELECT
                        ' change single quote to doubble single quote ie XYZ_ 123'rd,ABC 456's to XYZ_ 123''rd,ABC 456''s
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSingleQuote, Constants.cDoubleSingleQuote, False)

                        ' take a list from HTML and format it for SQL Clause XYZ_ 123''rd, ABC 456''s, DEF789 to 'XYZ_ 123''rd','ABC 456''s','DEF789' 
                        FormatUserData = Constants.cSingleQuote & CleanUserData(FormatUserData, Constants.cCommaDelim, Constants.cValueSeperator, False) & Constants.cSingleQuote

                        ' change underscores back to imbedded commas  ie 'XYZ_ 123''rd','ABC 456''s','DEF789' to 'XYZ, 123''rd','ABC 456''s','DEF789'
                        FormatUserData = CleanUserData(FormatUserData, Constants.cImbedComa, Constants.cCommaDelim, False)

                        Exit Function '- String is ready to be split or used for selection

                    Case Constants.gtUSRHTMLSELECT
                        ' change single quote to doubble single quote ie XYZ_ 123'rd, ABC 456's to XYZ_ 123''rd, ABC 456''s
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSingleQuote, Constants.cDoubleSingleQuote, False)

                        ' take a list from HTML and format it for SQL Clause XYZ_ 123''rd, ABC 456''s, DEF789 to 'XYZ_ 123''rd','ABC 456''s','DEF789' 
                        FormatUserData = Constants.cSingleQuote & CleanUserData(FormatUserData, Constants.cMultiDelim, Constants.cValueSeperator, False) & Constants.cSingleQuote

                        ' change underscores back to imbedded commas  ie 'XYZ_ 123''rd','ABC 456''s','DEF789' to 'XYZ, 123''rd','ABC 456''s','DEF789'
                        FormatUserData = CleanUserData(FormatUserData, Constants.cImbedComa, Constants.cCommaDelim, False)

                        Exit Function '- String is ready to be split or used for selection

                    Case Constants.gtUSRWILDCARD

                        FormatUserData = CleanUserData(FormatUserData, Constants.cWildCard, Constants.cSQLWildCard, False)
                        Exit Function '- String is ready to be used for selection

                    Case Constants.gtUSRRANGE

                        ' Check for Colon Delimiter 
                        FormatUserData = CleanUserData(FormatUserData, Constants.cColonDelim, Constants.cCommaDelim, False)

                        ' Check for SemiColon Delimiter
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSemiColonDelim, Constants.cCommaDelim, False)

                        ' Check for Paste Delimiter
                        FormatUserData = CleanUserData(FormatUserData, Constants.cEmptyString, Constants.cCommaDelim, True)

                        ' change single quote to doubble single quote ie XYZ_ 123'rd, ABC 456's to XYZ_ 123''rd, ABC 456''s
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSingleQuote, Constants.cDoubleSingleQuote, False)

                        ' take a list from range and format it for SQL Clause XYZ_ 123''rd, ABC 456''s, DEF789 to 'XYZ_ 123''rd','ABC 456''s','DEF789' 
                        FormatUserData = Constants.cSingleQuote & CleanUserData(FormatUserData, Constants.cCommaDelim, Constants.cValueSeperator, False) & Constants.cSingleQuote

                        ' change underscores back to imbedded commas  ie 'XYZ_ 123''rd','ABC 456''s','DEF789' to 'XYZ, 123''rd','ABC 456''s','DEF789'
                        FormatUserData = CleanUserData(FormatUserData, Constants.cImbedComa, Constants.cCommaDelim, False)

                        Exit Function '- String is ready to be used for selection

                    Case Constants.gtUSRNUMRANGE

                        ' Check for Colon Delimiter 
                        FormatUserData = CleanUserData(FormatUserData, Constants.cColonDelim, Constants.cCommaDelim, False)

                        ' Check for SemiColon Delimiter
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSemiColonDelim, Constants.cCommaDelim, False)

                        ' Check for Paste Delimiter
                        FormatUserData = CleanUserData(FormatUserData, Constants.cEmptyString, Constants.cCommaDelim, True)

                        Exit Function '- String is ready to be used for selection

                    Case Constants.gtUSRCOMPRANGE

                        If FindItemInData(FormatUserData, Constants.cCommaDelim) Then
                            ' change imbedded commas to underscores  ie JETNET, LLC to JETNET_ LLC
                            FormatUserData = CleanUserData(FormatUserData, Constants.cCommaDelim, Constants.cImbedComa, False)
                        End If

                        ' Check for Colon Delimiter 
                        FormatUserData = CleanUserData(FormatUserData, Constants.cColonDelim, Constants.cCommaDelim, False)

                        ' Check for SemiColon Delimiter
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSemiColonDelim, Constants.cCommaDelim, False)

                        ' Check for Paste Delimiter
                        FormatUserData = CleanUserData(FormatUserData, Constants.cEmptyString, Constants.cCommaDelim, True)

                        ' change single quote to doubble single quote ie XYZ_ 123'rd, ABC 456's to XYZ_ 123''rd, ABC 456''s
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSingleQuote, Constants.cDoubleSingleQuote, False)

                        ' take a list from range and format it for SQL Clause XYZ_ 123''rd, ABC 456''s, DEF789 to 'XYZ_ 123''rd','ABC 456''s','DEF789' 
                        FormatUserData = Constants.cSingleQuote & CleanUserData(FormatUserData, Constants.cCommaDelim, Constants.cValueSeperator, False) & Constants.cSingleQuote

                        ' change underscores back to imbedded commas  ie 'XYZ_ 123''rd','ABC 456''s','DEF789' to 'XYZ, 123''rd','ABC 456''s','DEF789'
                        FormatUserData = CleanUserData(FormatUserData, Constants.cImbedComa, Constants.cCommaDelim, False)

                        Exit Function '- String is ready to be used for selection

                    Case Constants.gtUSRPHONENUM
                        ' remove any dashes from number ie 1-315-123-4567 to 13151234567
                        FormatUserData = CleanUserData(FormatUserData, Constants.cHyphen, Constants.cEmptyString, False)

                        ' Check for wildcards, add them if needed
                        FormatUserData = FormatUserData(FormatUserData, Constants.gtUSRWILDSQL, False)

                        Exit Function '- String is ready to be used for selection

                    Case Constants.gtUSRCOMPNAME

                        ' Format the company name search string          
                        FormatUserData = FormatUserData(CleanWebInputData(FormatUserData, False, False, False, False, False, False, False), Constants.gtUSRWILDSQL, False)

                        Exit Function '- String is ready to be used for selection

                    Case Constants.gtUSRWILDSQL
                        ' change wildcards(*) to SQL wildcards (%)

                        FormatUserData = FormatUserData(FormatUserData, Constants.gtUSRWILDCARD, False)

                        Exit Function '- String is ready to be used for selection

                    Case Constants.gtUSRSAVEPROJECT
                        ' Clean all fields by these rules
                        ' change single quote to doubble single quote ie XYZ_ 123'rd to XYZ_ 123''rd
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSingleQuote, Constants.cDoubleSingleQuote, False)

                        ' take item and format it for SQL Clause XYZ_ 123''rd to 'XYZ_ 123''rd' 
                        FormatUserData = Constants.cSingleQuote & FormatUserData & Constants.cSingleQuote
                        Exit Function '- String is ready to be used for selection

                    Case Constants.gtUSRNUMERICSTR
                        ' Clean all fields by these rules
                        ' change single quote to doubble single quote ie XYZ_ 123'rd to XYZ_ 123''rd
                        FormatUserData = CleanUserData(FormatUserData, Constants.cSingleQuote, Constants.cDoubleSingleQuote, False)

                        ' take item and format it for SQL Clause XYZ_ 123''rd to 'XYZ_ 123''rd' 
                        FormatUserData = Constants.cSingleQuote & FormatUserData & Constants.cSingleQuote

                        ' change underscores back to imbedded commas  ie 'XYZ_ 123''rd' to 'XYZ, 123''rd'
                        FormatUserData = CleanUserData(FormatUserData, Constants.cImbedComa, Constants.cCommaDelim, False)
                        Exit Function '- String is ready to be used for selection

                    Case Else
                        If Not IsNumeric(FormatUserData) Then
                            ' Clean all fields by these rules
                            ' change single quote to doubble single quote ie XYZ_ 123'rd to XYZ_ 123''rd
                            FormatUserData = CleanUserData(FormatUserData, Constants.cSingleQuote, Constants.cDoubleSingleQuote, False)

                            ' take item and format it for SQL Clause XYZ_ 123''rd to 'XYZ_ 123''rd' 
                            FormatUserData = Constants.cSingleQuote & FormatUserData & Constants.cSingleQuote

                            ' change underscores back to imbedded commas  ie 'XYZ_ 123''rd' to 'XYZ, 123''rd'
                            FormatUserData = CleanUserData(FormatUserData, Constants.cImbedComa, Constants.cCommaDelim, False)
                            Exit Function '- String is ready to be used for selection
                        Else
                            FormatUserData = CStr(Trim(FormatUserData))
                            Exit Function '- String is ready to be used for selection
                        End If
                End Select
            Else
                Select Case t_inUserDataType
                    Case Constants.gtUSRHTMLSELECT
                        ' first html encode the data so we take care of weird characters
                        '  FormatUserData = server.HTMLEncode(s_inData)
                        FormatUserData = s_inData
                        ' change imbedded commas to underscores  ie XYZ, 123'rd to XYZ_ 123'rd
                        FormatUserData = CleanUserData(FormatUserData, Constants.cCommaDelim, Constants.cImbedComa, False)

                        Exit Function '- String is ready to be used for display

                    Case Constants.gtUSRWILDCARD
                        FormatUserData = Constants.cSingleQuote & CleanUserData(s_inData, Constants.cSQLWildCard, Constants.cWildCard, False) & Constants.cSingleQuote
                        Exit Function '- String is ready to be used for display

                    Case Constants.gtUSRCOMPNAME

                        ' Replace Comma Delimiter With Colon Delimiter ie XYZ_ 123, ABC_LLC to XYZ_ 123:ABC_LLC
                        FormatUserData = CleanUserData(s_inData, Constants.cCommaDelim, Constants.cColonDelim, False)

                        ' change underscores back to imbedded commas  ie XYZ_ 123: ABC_LLC to XYZ, 123:ABC,LLC
                        FormatUserData = CleanUserData(FormatUserData, Constants.cImbedComa, Constants.cCommaDelim, False)
                        Exit Function '- String is ready to be used for display

                    Case Else
                        ' change underscores back to imbedded commas  ie XYZ, 123'rd to XYZ_ 123'rd
                        FormatUserData = CleanUserData(s_inData, Constants.cImbedComa, Constants.cCommaDelim, False)
                        ' FormatUserData = System HTMLEncode(FormatUserData)

                End Select
            End If
        End If

    End Function

    Function CleanWebInputData(ByVal strInput, ByVal bIsValidate, ByVal bIsAlphaTest, ByVal bIsNumericTest, ByVal bIsAlphaNumericTest, ByVal bAllowSpaces, ByVal bAllowSpecial, ByVal bAllowCRLF)
        ' this function also creates the company name for search criteria

        Dim strWork, iLoop, strResults, iTest, tempChar, bIsCRLF, bIsValidChar, SQLVerChars
        Dim SQLVersionChars As String = ""
        strWork = ""
        iLoop = 1
        strResults = ""
        bIsValidChar = False

        SQLVersionChars = "@@" ' chr(64) & chr(64)

        If Trim(strInput) <> "" And Len(strInput) > 0 Then

            If Not bIsValidate Then ' used to create company name for search criteria
                strInput = CleanUserData(strInput, Constants.cSpaceDelim, Constants.cEmptyString, False)
                strInput = CleanUserData(strInput, Constants.cDot, Constants.cEmptyString, False)
                strInput = CleanUserData(strInput, Constants.cCommaDelim, Constants.cEmptyString, False)
                strInput = CleanUserData(strInput, Constants.cHyphen, Constants.cEmptyString, False)
                strWork = Trim(strInput)
            Else
                strWork = Trim(strInput)
            End If

            If strWork = "" Then
                CleanWebInputData = ""
                Exit Function
            End If

            For iLoop = 1 To Len(strInput)

                iTest = 0
                tempChar = ""
                bIsCRLF = False

                ' special case were user tries to enter SQL Version query
                If Mid(strWork, iLoop, 2) = SQLVersionChars Then
                    CleanWebInputData = "INVALID"
                    Exit Function
                End If

                ' If we are allowing CRLF, check for it and pass it through
                If Mid(strWork, iLoop, 2) = vbCrLf And bAllowCRLF Then
                    If bAllowCRLF Then
                        bIsCRLF = True
                        iLoop = iLoop + 1
                    End If
                Else ' Just check the character

                    tempChar = Mid(strWork, iLoop, 1)

                    On Error Resume Next
                    iTest = CInt(Asc(tempChar))

                    If Err.Number = 0 Then
                        If iTest > 255 Or iTest < 0 Then iTest = 0
                    Else
                        iTest = 0
                    End If
                    Err.Clear()

                End If

                If Not bIsCRLF Then

                    If Not bIsValidate Then ' used to create company name for search criteria and engine name for search criteria ( * keeps any wildcards user has entered )
                        ' UPPERCASE A - Z                       LOWERCASE a-z                           NUMBERS 0-9                         chr(42) = "*"
                        If ((iTest >= 65) And (iTest <= 90)) Or ((iTest >= 97) And (iTest <= 122)) Or ((iTest >= 48) And (iTest <= 57)) Or (iTest = 42) Then
                            strResults = strResults & UCase(tempChar)
                        End If

                    Else ' use function for validate user input data
                        ' only allows type of characters for that input data value

                        If bIsAlphaTest Then ' only allow upper and lower case characters

                            If bAllowSpaces Then
                                If (iTest = 32) Then
                                    strResults = strResults & Chr(32)
                                    bIsValidChar = True
                                End If
                            End If

                            ' UPPERCASE A - Z                      LOWERCASE a-z
                            If ((iTest >= 65) And (iTest <= 90)) Or ((iTest >= 97) And (iTest <= 122)) Then
                                strResults = strResults & tempChar
                                bIsValidChar = True
                            End If

                            If bAllowSpecial Then
                                ' chr(92) = "\"  chr(46) = "." chr(64) = "@" chr(58) = ":" chr(44) = "," chr(94) = "^" chr(126) = "~" chr(47) = "/" chr(43) = "+" chr(39) = "'"
                                ' chr(59) = ";" chr(95) = "_" chr(45) = "-" chr(36) = "$" chr(33) = "!" chr(63) = "?" chr(124) = "|" chr(37) = "%" chr(35) = "#"
                                If (iTest = 92) Or (iTest = 46) Or (iTest = 64) Or (iTest = 58) Or (iTest = 44) Or (iTest = 94) Or (iTest = 126) Or (iTest = 47) Or (iTest = 43) Or (iTest = 39) Or
                                  (iTest = 59) Or (iTest = 95) Or (iTest = 45) Or (iTest = 36) Or (iTest = 33) Or (iTest = 63) Or (iTest = 124) Or (iTest = 37) Or (iTest = 35) Then
                                    strResults = strResults & tempChar
                                    bIsValidChar = True
                                End If
                            End If

                        End If ' bIsAlphaTest 

                        If bIsNumericTest Then ' only allow numerals
                            ' NUMBERS 0-9
                            If ((iTest >= 48) And (iTest <= 57)) Then
                                strResults = strResults & tempChar
                                bIsValidChar = True
                            End If

                        End If 'bIsNumericTest

                        If bIsAlphaNumericTest Then ' allow upper and lower case characters and allow numerals

                            If bAllowSpaces Then
                                If (iTest = 32) Then
                                    strResults = strResults & Chr(32)
                                    bIsValidChar = True
                                End If
                            End If

                            ' UPPERCASE A - Z                      LOWERCASE a-z                         NUMBERS 0-9
                            If ((iTest >= 65) And (iTest <= 90)) Or ((iTest >= 97) And (iTest <= 122)) Or ((iTest >= 48) And (iTest <= 57)) Then
                                strResults = strResults & tempChar
                                bIsValidChar = True
                            End If

                            If bAllowSpecial Then
                                ' chr(92) = "\"  chr(46) = "." chr(64) = "@" chr(58) = ":" chr(44) = "," chr(94) = "^" chr(126) = "~" chr(47) = "/" chr(43) = "+" chr(39) = "'"
                                ' chr(59) = ";" chr(95) = "_" chr(45) = "-" chr(36) = "$" chr(33) = "!" chr(63) = "?" chr(124) = "|" chr(37) = "%" chr(35) = "#"
                                If (iTest = 92) Or (iTest = 46) Or (iTest = 64) Or (iTest = 58) Or (iTest = 44) Or (iTest = 94) Or (iTest = 126) Or (iTest = 47) Or (iTest = 43) Or (iTest = 39) Or
                                  (iTest = 59) Or (iTest = 95) Or (iTest = 45) Or (iTest = 36) Or (iTest = 33) Or (iTest = 63) Or (iTest = 124) Or (iTest = 37) Or (iTest = 35) Then
                                    strResults = strResults & tempChar
                                    bIsValidChar = True
                                End If
                            End If

                        End If ' bIsAlphaNumericTest 

                        If Not bIsValidChar Then

                            CleanWebInputData = "INVALID"
                            Exit Function

                        End If

                    End If ' Not bIsValidate 
                Else
                    strResults = strResults & vbCrLf
                End If ' Not bIsCRLF 

            Next ' iLoop = 1 To Len(strInput)

        End If ' trim(strInput) <> "" and Len(strInput) > 0

        CleanWebInputData = strResults

    End Function

    Function FindItemInData(ByVal inputString, ByVal sDelimiter)

        Dim nPos
        nPos = 0
        FindItemInData = False

        If inputString <> "" Then
            nPos = InStr(1, inputString, sDelimiter, vbBinaryCompare)
        End If

        If nPos > 0 Then
            FindItemInData = True
        End If

    End Function

    Function CleanUserData(ByVal inputString, ByVal sFind, ByVal sReplace, ByVal bIsTextAreaInput) As String
        CleanUserData = ""
        Dim n_loop, n_offset, n_offset1, sTmpData
        Dim sOutputString
        n_loop = 1
        n_offset = 0
        n_offset1 = 0
        sTmpData = ""
        sOutputString = ""

        If inputString <> "" Then

            If Not bIsTextAreaInput Then
                sOutputString = Trim(Replace(inputString, sFind, sReplace))
            Else

                'response.write("CUD(istr): " & inputString & "<br/>")

                Do While n_loop < Len(inputString) + 1

                    ' find first CRLF
                    n_offset = InStr(n_loop, inputString, vbCrLf, vbBinaryCompare)

                    ' find seccond CRLF
                    n_offset1 = InStr(n_offset + 1, inputString, vbCrLf, vbBinaryCompare)

                    ' grab first item from n_loop to n_offset
                    If (n_offset > n_loop) Then
                        sTmpData = Mid(inputString, n_loop, (n_offset - 1))
                        If UCase(sTmpData) <> "GA" And Len(sTmpData) > 0 Then ' clean out any "GA" Garbage also zero length data

                            ' I also need to preserve any commas in the data
                            If FindItemInData(sTmpData, Constants.cCommaDelim) Then
                                ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
                                sTmpData = CleanUserData(sTmpData, Constants.cCommaDelim, Constants.cImbedComa, False)
                            End If

                            ' clean out the EXCEL 03 Character
                            sTmpData = CleanUserData(sTmpData, Constants.EXCEL2003CHAR, Constants.cEmptyString, False)

                            If Trim(sOutputString) = "" Then
                                sOutputString = sTmpData
                            Else
                                sOutputString = sOutputString & sReplace & sTmpData
                            End If

                        End If
                    End If

                    If (n_offset = 0 Or n_offset1 = 0) And n_offset = n_offset1 Then
                        sOutputString = Trim(inputString)
                        Exit Do
                    End If

                    If (n_offset1 > n_loop) Then  ' found second CRLF after our start

                        ' find next CRLF start 1 chars ahead of our first CRLF pair
                        If (n_offset1 > n_offset) Then ' found next CRLF the data is between the two offsets

                            If (n_offset1 - n_offset) > 1 Then ' ok we have at least one char between the two

                                sTmpData = Mid(inputString, n_offset + 2, ((n_offset1 - n_offset) - 2)) ' ok get the data

                                If UCase(sTmpData) <> "GA" And Len(sTmpData) > 0 Then ' clean out any "GA" Garbage also zero length data

                                    ' I also need to preserve any commas in the data
                                    If FindItemInData(sTmpData, Constants.cCommaDelim) Then
                                        ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
                                        sTmpData = CleanUserData(sTmpData, Constants.cCommaDelim, Constants.cImbedComa, False)
                                    End If

                                    ' clean out the EXCEL 03 Character
                                    sTmpData = CleanUserData(sTmpData, Constants.EXCEL2003CHAR, Constants.cEmptyString, False)

                                    If Trim(sOutputString) = "" Then
                                        sOutputString = sTmpData
                                    Else
                                        sOutputString = sOutputString & sReplace & sTmpData
                                    End If

                                End If
                            End If
                        End If

                    Else
                        Exit Do
                    End If

                    ' jump ahead 1 chars to look for the next chunk of data
                    If n_offset1 > 0 Then
                        n_loop = n_offset1
                    End If

                    n_offset = 0
                    n_offset1 = 0
                    sTmpData = ""

                Loop ' While n_loop < Len(inputString) + 1

                If IsNothing(sOutputString) And Trim(sOutputString) = "" Then

                    ' I also need to preserve any commas in the data
                    If FindItemInData(inputString, Constants.cCommaDelim) Then
                        ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
                        inputString = CleanUserData(inputString, Constants.cCommaDelim, Constants.cImbedComa, False)
                    End If

                    ' clean out the EXCEL 03 Character
                    inputString = CleanUserData(inputString, Constants.EXCEL2003CHAR, Constants.cEmptyString, False)

                    ' clean out the CRLF
                    inputString = CleanUserData(inputString, vbCrLf, Constants.cEmptyString, False)

                    sOutputString = Trim(inputString)

                End If

                'response.write("CUD(ostr): " & sOutputString & "<br/>")

            End If ' not bIsTextAreaInput

            CleanUserData = sOutputString

        End If ' inputString <> ""

    End Function

    Function CheckSelectedGeography(ByVal inGeographicZone, ByVal inGeographicSubZone, ByVal inCountry, ByVal inState, ByVal b_isBase)

        Dim sCountry, sState, sWhereClause
        sWhereClause = ""
        CheckSelectedGeography = ""

        If b_isBase Then
            sCountry = "ac_aport_country"
            sState = "ac_aport_state"
        Else
            sCountry = "comp_country"
            sState = "comp_state"
        End If

        'clean input user data if there is data to clean
        If Len(inCountry) > 0 Then
            inCountry = FormatUserData(inCountry, Constants.gtUSRHTMLSELECT, False)
        End If

        If Len(inState) > 0 Then
            inState = FormatUserData(inState, Constants.gtUSRHTMLSELECT, False)
        End If

        Select Case inGeographicZone

            Case "Continent"

                If inGeographicSubZone <> "" Then
                    sWhereClause = MakeRegionWhereClause(inGeographicSubZone, inCountry, inState, b_isBase, True)
                Else
                    If inCountry <> "" And inState = "" Then
                        sWhereClause = Constants.cAndClause & FormatQueryString(sCountry, inCountry, "", "", Constants.gtSQLIN)
                    Else
                        If inState <> "" And inCountry <> "" Then
                            sWhereClause = Constants.cAndClause & Constants.cSingleOpen & FormatQueryString(sCountry, inCountry, "", "", Constants.gtSQLIN)
                        End If
                    End If

                    If inState <> "" And inCountry <> "" Then
                        ' check to see if this state is in this country
                        If InStr(1, inCountry, getCountry(inCountry, inState, True)) = 0 Then
                            sWhereClause = sWhereClause & Constants.cOrClause & FormatQueryString(sState, inState, "", "", Constants.gtSQLIN)
                        Else
                            sWhereClause = sWhereClause & Constants.cAndClause & FormatQueryString(sState, inState, "", "", Constants.gtSQLIN)
                        End If
                        sWhereClause = sWhereClause & Constants.cSingleClose
                    Else
                        If inState <> "" Then
                            sWhereClause = Constants.cAndClause & FormatQueryString(sState, inState, "", "", Constants.gtSQLIN)
                        End If
                    End If
                End If

            Case "Region"
                If inGeographicSubZone <> "" Then
                    sWhereClause = MakeRegionWhereClause(inGeographicSubZone, inCountry, inState, b_isBase, False)
                Else

                    If inCountry <> "" And inState = "" Then
                        sWhereClause = Constants.cAndClause & FormatQueryString(sCountry, inCountry, "", "", Constants.gtSQLIN)
                    Else
                        If inState <> "" And inCountry <> "" Then
                            sWhereClause = Constants.cAndClause & Constants.cSingleOpen & FormatQueryString(sCountry, inCountry, "", "", Constants.gtSQLIN)
                        End If
                    End If

                    If inState <> "" And inCountry <> "" Then
                        ' check to see if this state is in this country
                        If InStr(1, inCountry, getCountry(inCountry, inState, False)) = 0 Then
                            sWhereClause = sWhereClause & Constants.cOrClause & FormatQueryString(sState, inState, "", "", Constants.gtSQLIN)
                        Else
                            sWhereClause = sWhereClause & Constants.cAndClause & FormatQueryString(sState, inState, "", "", Constants.gtSQLIN)
                        End If
                        sWhereClause = sWhereClause & Constants.cSingleClose
                    Else
                        If inState <> "" Then
                            sWhereClause = Constants.cAndClause & FormatQueryString(sState, inState, "", "", Constants.gtSQLIN)
                        End If
                    End If

                End If

        End Select

        If sWhereClause <> "" Then
            CheckSelectedGeography = sWhereClause
        End If

    End Function

    Function getCountry(ByVal s_inCountry, ByVal s_inState, ByVal b_isCountry)

        Dim Query As String = ""
        Dim sGeo_region_name, sGeo_country_name, sState_code, sState_name
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim adoRs As SqlClient.SqlDataReader : adoRs = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim tHeaderString As String = ""
        Dim out_docsViewHeaderText As String = ""

        getCountry = ""

        If b_isCountry Then
            sGeo_region_name = "country_continent_name"
            sGeo_country_name = "country_name"
            sState_code = "state_code"
            sState_name = "state_name"
        Else
            sGeo_region_name = "geographic_region_name"
            sGeo_country_name = "geographic_country_name"
            sState_code = "state_code"
            sState_name = "state_name"
        End If

        If b_isCountry Then
            Query = "SELECT DISTINCT * FROM Country WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON state_country = country_name WHERE state_country = '" & Replace(s_inCountry, Constants.cSingleQuote, Constants.cEmptyString) & "'"
        Else
            Query = "SELECT DISTINCT * FROM Geographic WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON "
            Query = Query & "(geographic_state_code = state_code AND geographic_country_name = state_country) WHERE state_country = '" & Replace(s_inCountry, Constants.cSingleQuote, Constants.cEmptyString) & "'"
        End If ' isCountry

        Query = Query & Constants.cAndClause & FormatQueryString(sState_code, s_inState, "", "", Constants.gtSQLIN)

        If b_isCountry Then
            Query = Query & " ORDER BY country_continent_name, country_name, state_name, state_code"
        Else
            Query = Query & " ORDER BY geographic_region_name, geographic_country_name, state_name, state_code"
        End If ' isCountry

        '   If session("debug") Then
        '   response.write("getCountry(ByVal s_inState, ByVal b_isCountry) : " & Query)
        '   End If


        tHeaderString = ""
        out_docsViewHeaderText = ""

        Try
            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60


            If adoRs.HasRows Then
                If Not (IsDBNull(adoRs("sGeo_country_name"))) Then
                    If Trim(adoRs("Geo_country_name")) <> "" Then
                        If Not (IsDBNull(adoRs("sState_code"))) Then
                            If InStr(1, s_inState, Trim(adoRs("sState_code"))) >= 1 Then
                                getCountry = Trim(adoRs("sGeo_country_name"))
                            End If
                        End If
                    End If
                End If

            End If
            adoRs.Close()


        Catch ex As Exception
        Finally
            adoRs = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

        End Try
    End Function

    Function MakeRegionWhereClause(ByVal inRegion, ByVal selectedCountries, ByVal selectedStates, ByVal b_isBase, ByVal b_isCountry) As String
        MakeRegionWhereClause = ""

        'Dim Query, tQuery, adoRs, sRememberLast, sHolder, sHolder1, bHasAState
        'Dim sCountryList(), sStateList(), nLoop, mLoop, sSelectedStateList()
        'Dim sCountryClause, sStateClause, bHadState, bfirstState, nRememberLastCountryWithState

        'Dim sCountry, sState, bAllCountries, bAllStates
        'Dim sGeo_region_name, sGeo_country_name, sState_code, sState_name

        'If b_isBase Then
        '    sCountry = "ac_aport_country"
        '    sState = "ac_aport_state"
        'Else
        '    sCountry = "comp_country"
        '    sState = "comp_state"
        'End If

        'If b_isCountry Then
        '    sGeo_region_name = "country_continent_name"
        '    sGeo_country_name = "country_name"
        '    sState_code = "state_code"
        '    sState_name = "state_name"
        'Else
        '    sGeo_region_name = "geographic_region_name"
        '    sGeo_country_name = "geographic_country_name"
        '    sState_code = "state_code"
        '    sState_name = "state_name"
        'End If

        'nLoop = 0
        'mLoop = 0
        'sRememberLast = ""
        'nRememberLastCountryWithState = -1
        'sHolder = ""
        'sHolder1 = ""
        'sCountryClause = ""
        'bHadState = False
        'bHasAState = False
        'bfirstState = False
        'bAllCountries = False
        'bAllStates = False

        'If inRegion <> "" And selectedCountries = "" And selectedStates = "" Then
        '    bAllCountries = True
        '    bAllStates = True
        'End If

        'If inRegion <> "" And selectedCountries <> "" And selectedStates = "" Then
        '    bAllStates = True
        'End If

        '' create a record set of countries and states based on real contentents or defined regions

        'If b_isCountry Then
        '    Query = "SELECT DISTINCT * FROM Country WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON state_country = country_name WHERE "
        'Else
        '    Query = "SELECT DISTINCT * FROM Geographic WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON (geographic_state_code = state_code AND geographic_country_name = state_country) WHERE "
        'End If ' isCountry

        'If bAllCountries Then
        '    sHolder = FormatUserData(inRegion, gtUSRHTMLSELECT, False)
        '    Query = Query & FormatQueryString(sGeo_region_name, sHolder, , "", gtSQLIN)
        'Else
        '    sHolder = FormatUserData(inRegion, gtUSRHTMLSELECT, False)
        '    Query = Query & FormatQueryString(sGeo_region_name, sHolder, , "", gtSQLIN)

        '    If selectedCountries <> "" Then
        '        tQuery = FormatQueryString(sGeo_country_name, selectedCountries, , "", gtSQLIN)

        '        If inRegion <> "" Then
        '            Query = Query & cAndClause & tQuery
        '        Else
        '            Query = Query & tQuery
        '        End If ' inRegion <> "" 

        '    End If ' selectedCountries <> ""
        'End If ' bAllCountries

        'If b_isCountry Then
        '    Query = Query & " ORDER BY country_continent_name, country_name, state_name, state_code"
        'Else
        '    Query = Query & " ORDER BY geographic_region_name, geographic_country_name, state_name, state_code"
        'End If ' isCountry

        'Session("objUserConn").CursorLocation = adUseClient
        'adoRs = Session("objUserConn").Execute(Query)

        'If Not (adoRs.BOF And adoRs.EOF) Then

        '    ReDim sCountryList(adoRs.RecordCount - 1)
        '    ReDim sStateList(adoRs.RecordCount - 1)

        '    Do While Not adoRs.EOF

        '        If Not (isNull(adoRs(sGeo_country_name))) And trim(adoRs(sGeo_country_name).value) <> "" Then
        '            sCountryList(nLoop) = FormatUserData(trim(adoRs(sGeo_country_name).value), gtUSRNONE, False)
        '            nLoop = nLoop + 1
        '        End If

        '        If Not bAllStates Or Not b_isCountry Then
        '            If Not (isNull(adoRs(sState_code))) And trim(adoRs(sState_code).value) <> "" Then
        '                ' ok someone has selected a state(s) only use those state(s) 
        '                ' even if there are more state(s) in the record set
        '                If selectedStates <> "" Then
        '                    If InStr(1, selectedStates, trim(adoRs(sState_code).value)) > 0 Then
        '                        sStateList(mLoop) = FormatUserData(trim(adoRs(sState_code).value), gtUSRNONE, False)
        '                    Else
        '                        sCountryList(nLoop - 1) = Empty
        '                    End If
        '                Else
        '                    sStateList(mLoop) = FormatUserData(trim(adoRs(sState_code).value), gtUSRNONE, False)
        '                End If
        '            Else ' if this country has an empty state code
        '                ' and someone has selected states, only display
        '                ' the countries for the selected states.
        '                ' set to Empty to exclude this country from our list
        '                If selectedStates <> "" Then
        '                    sCountryList(nLoop - 1) = Empty
        '                End If
        '            End If
        '        End If

        '        mLoop = mLoop + 1

        '        adoRs.movenext()
        '    Loop

        '    Session("objUserConn").CursorLocation = adUseClient

        '    If adoRs.State = adStateOpen Then
        '        adoRs.Close()
        '    End If

        '    adoRs = Nothing

        '    sRememberLast = ""

        '    ' ok now that we have a list of countries and states
        '    ' generate the in clause

        '    For nLoop = 0 To Ubound(sCountryList)

        '        If Not isEmpty(sCountryList(nLoop)) Then

        '            If nLoop = 0 Then ' this is the first time through
        '                sRememberLast = sCountryList(nLoop)

        '                If Not isEmpty(sStateList(nLoop)) Then ' current country also has a state
        '                    sCountryClause = cAndClause & cDoubleOpen & sCountry & cEq & sCountryList(nLoop)
        '                    sCountryClause = sCountryClause & cAndClause & cSingleOpen & sState & cEq & sStateList(nLoop)
        '                    bfirstState = True ' this is the first state we find
        '                    bHadState = True
        '                    nRememberLastCountryWithState = nLoop
        '                Else ' the current country might have a state check and see               
        '                    For mLoop = nLoop To Ubound(sStateList)
        '                        If Not isEmpty(sStateList(mLoop)) Then
        '                            If sCountryList(mLoop) = sCountryList(nLoop) Then
        '                                bHasAState = True
        '                                Exit For
        '                            End If
        '                        End If
        '                    Next

        '                    If bHasAState Then   ' this country has a state so wrap states with the country
        '                        bHasAState = False
        '                        sCountryClause = cAndClause & cDoubleOpen & sCountry & cEq & sCountryList(nLoop)
        '                    Else                 ' just add the country
        '                        sCountryClause = cAndClause & cSingleOpen & sCountry & cEq & sCountryList(nLoop)
        '                    End If
        '                End If ' not isEmpty(sStateList(nLoop))

        '            Else ' next time through loop

        '                If sRememberLast = sCountryList(nLoop) Then ' same country add another state

        '                    If Not isEmpty(sStateList(nLoop)) Then ' this country has another state
        '                        If bfirstState Then ' we had a state already reset bfirststate flag
        '                            bfirstState = False ' add the state
        '                            sCountryClause = sCountryClause & cOrClause & sState & cEq & sStateList(nLoop)
        '                        Else
        '                            If bHadState Then  ' we had a previous state add the state
        '                                sCountryClause = sCountryClause & cOrClause & sState & cEq & sStateList(nLoop)
        '                            Else               ' add the state as the first and only state
        '                                sCountryClause = sCountryClause & cAndClause & cSingleOpen & sState & cEq & sStateList(nLoop)
        '                            End If
        '                        End If

        '                        bHadState = True
        '                        nRememberLastCountryWithState = nLoop
        '                    End If

        '                Else ' different country check to see if it will have states

        '                    sRememberLast = sCountryList(nLoop)

        '                    If bHadState Then
        '                        ' if the last country had a state close it off
        '                        If Not isEmpty(sStateList(nRememberLastCountryWithState)) Then

        '                            ' I have to look ahead to see if this country might have a state
        '                            ' so I can wrap it right if it does have a state
        '                            ' start looking from current country forward
        '                            For mLoop = nLoop To Ubound(sStateList)
        '                                If Not isEmpty(sStateList(mLoop)) Then
        '                                    If sCountryList(mLoop) = sCountryList(nLoop) Then ' the current country has a state
        '                                        bHasAState = True
        '                                        Exit For
        '                                    End If
        '                                End If
        '                            Next

        '                            If bHasAState Then  ' this country will have a state to add later
        '                                bHasAState = False  ' so close current country and add the next country ready to wrap a state
        '                                sCountryClause = sCountryClause & cDoubleClose & cOrClause & cSingleOpen & sCountry & cEq & sCountryList(nLoop)
        '                            Else                  ' so close current country and add the next country don't have to wrap state
        '                                sCountryClause = sCountryClause & cDoubleClose & cOrClause & sCountry & cEq & sCountryList(nLoop)
        '                            End If
        '                        End If ' not isEmpty(sStateList(nRememberLastCountryWithState))

        '                        nRememberLastCountryWithState = -1  ' reset flags
        '                        bHadState = False
        '                        bfirstState = False
        '                    Else
        '                        ' if the last country did not have a state so
        '                        ' I have to look ahead to see if this country might have a state
        '                        ' so I can wrap it right if it does have a state
        '                        ' start looking from current country forward
        '                        For mLoop = nLoop To Ubound(sStateList)
        '                            If Not isEmpty(sStateList(mLoop)) Then
        '                                If sCountryList(mLoop) = sCountryList(nLoop) Then ' the current country has a state
        '                                    bHasAState = True
        '                                    Exit For
        '                                End If
        '                            End If
        '                        Next

        '                        If bHasAState Then ' this country will have a state to add later
        '                            bHasAState = False
        '                            If sCountryClause = "" Then ' if our clause is empty add current country ready to wrap for state
        '                                sCountryClause = cAndClause & cDoubleOpen & sCountry & cEq & sCountryList(nLoop)
        '                            Else                        ' else add current country don't have to wrap for state
        '                                sCountryClause = sCountryClause & cOrClause & cSingleOpen & sCountry & cEq & sCountryList(nLoop)
        '                            End If
        '                        Else ' this country has no state add country
        '                            sCountryClause = sCountryClause & cOrClause & sCountry & cEq & sCountryList(nLoop)
        '                        End If ' not isEmpty(sStateList(nLoop))
        '                    End If 'bHadState

        '                    ' check and see if this country has a state  
        '                    If Not isEmpty(sStateList(nLoop)) Then  ' add the state to the clause
        '                        sCountryClause = sCountryClause & cAndClause & cSingleOpen & sState & cEq & sStateList(nLoop)
        '                        bHadState = True
        '                        If bfirstState = False Then
        '                            bfirstState = True
        '                        End If
        '                        nRememberLastCountryWithState = nLoop
        '                    End If ' not isEmpty(sStateList(nLoop))

        '                End If ' sRememberLast = sCountryList(nLoop)
        '            End If ' nLoop = 0
        '        End If ' not isEmpty(sCountryList(nLoop))

        '    Next ' nLoop = 0 to Ubound(sCountryList)

        '    If bHadState Or bfirstState Then
        '        sCountryClause = sCountryClause & cDoubleClose & cSingleClose
        '    Else
        '        sCountryClause = sCountryClause & cSingleClose
        '    End If ' bHadState

        '    MakeRegionWhereClause = sCountryClause

        'Else
        '    MakeRegionWhereClause = ""
        'End If

    End Function

    Function FormatQueryString(ByVal sVar1, ByVal sVar2, ByVal sVar3, ByVal sVar4, ByVal t_userSQLClauseType)

        FormatQueryString = "''"

        If sVar1 <> "" And sVar2 <> "" Then
            Select Case t_userSQLClauseType
                Case Constants.gtSQLIN

                    If sVar4 = "" Then
                        ' Check and see if we have one item in the sVar2 list
                        ' if we do make an = clause
                        If UBound(Split(sVar2, Constants.cCommaDelim)) >= 1 Then
                            ' create in clause (sField1 IN ('XYZ, 123''rd','ABC 456''s','DEF789'))"
                            FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cInClause & Constants.cSingleOpen & Trim(sVar2) & Constants.cDoubleClose
                        Else
                            ' create Equal clause (sField1 = 'XYZ, 123''rd')"
                            FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cEq & Trim(sVar2) & Constants.cSingleClose
                        End If
                    Else
                        If sVar4 = "NOWRAP" Then
                            If UBound(Split(sVar2, Constants.cCommaDelim)) >= 1 Then
                                ' create in clause (sField1 IN ('XYZ, 123''rd','ABC 456''s','DEF789'))"
                                FormatQueryString = Trim(sVar1) & Constants.cInClause & Constants.cSingleOpen & Trim(sVar2) & Constants.cSingleClose
                            Else
                                ' create Equal clause (sField1 = 'XYZ, 123''rd')"
                                FormatQueryString = Trim(sVar1) & Constants.cEq & Trim(sVar2)
                            End If
                        Else
                            If sVar4 = "NOT" Then
                                FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cNot & Constants.cLikeClause & Trim(sVar2) & Constants.cSingleClose
                                If UBound(Split(sVar2, Constants.cCommaDelim)) >= 1 Then
                                    ' create in clause (sField1 IN ('XYZ, 123''rd','ABC 456''s','DEF789'))"
                                    FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cNot & Constants.cInClause & Constants.cSingleOpen & Trim(sVar2) & Constants.cDoubleClose
                                Else
                                    ' create Equal clause (sField1 = 'XYZ, 123''rd')"
                                    FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cNotEq & Trim(sVar2) & Constants.cSingleClose
                                End If
                            End If
                        End If
                    End If

                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLAND
                    If sVar4 = "" Then
                        FormatQueryString = Constants.cSingleOpen & Constants.cAndClause & Trim(sVar1) & Constants.cEq & Trim(sVar2) & Constants.cSingleClose
                    Else
                        If sVar4 = "2" Then
                            FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cAndClause & Trim(sVar2) & Constants.cSingleClose
                        End If
                        If sVar4 = "3" Then
                            FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cAndClause & Trim(sVar2) & Constants.cAndClause & Trim(sVar3) & Constants.cSingleClose
                        End If
                    End If
                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLOR
                    If sVar4 = "" Then
                        FormatQueryString = Constants.cSingleOpen & Constants.cOrClause & Trim(sVar1) & Constants.cEq & Trim(sVar2) & Constants.cSingleClose
                    Else
                        If sVar4 = "2" Then
                            FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cOrClause & Trim(sVar2) & Constants.cSingleClose
                        End If
                        If sVar4 = "3" Then
                            FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cOrClause & Trim(sVar2) & Constants.cOrClause & Trim(sVar3) & Constants.cSingleClose
                        End If
                    End If
                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLIKE
                    If sVar4 = "" Then
                        FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cLikeClause & Trim(sVar2) & Constants.cSingleClose
                    Else
                        If sVar4 = "NOWRAP" Then
                            FormatQueryString = Trim(sVar1) & Constants.cLikeClause & Trim(sVar2)
                        Else
                            If sVar4 = "NOT" Then
                                FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cNot & Constants.cLikeClause & Trim(sVar2) & Constants.cSingleClose
                            End If
                        End If
                    End If
                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLBET
                    FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cBetweenClause & Trim(sVar2) & Constants.cAndClause & Trim(sVar3) & Constants.cSingleClose
                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLEQL
                    If sVar4 = "" Then
                        FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cEq & Trim(sVar2) & Constants.cSingleClose
                    Else
                        If sVar4 = "NOWRAP" Then
                            FormatQueryString = Trim(sVar1) & Constants.cEq & Trim(sVar2)
                        Else
                            If sVar4 = "NOT" Then
                                FormatQueryString = Trim(sVar1) & Constants.cNotEq & Trim(sVar2)
                            End If
                        End If
                    End If

                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLLT
                    If sVar4 = "" Then
                        FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cLt & Trim(sVar2) & Constants.cSingleClose
                    Else
                        If sVar4 = "NOWRAP" Then
                            FormatQueryString = Trim(sVar1) & Constants.cLt & Trim(sVar2)
                        Else
                            If sVar4 = "DATE" Then ' (sVar1 < CONVERT(DATETIME, '" & sVar2 & "',102))
                                FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cLt & Constants.cConvertClause & Constants.cSingleOpen & "DATETIME," & Trim(sVar2) & ",102" & Constants.cDoubleClose
                            End If
                        End If
                    End If
                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLGT
                    If sVar4 = "" Then
                        FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cGt & Trim(sVar2) & Constants.cSingleClose
                    Else
                        If sVar4 = "NOWRAP" Then
                            FormatQueryString = Trim(sVar1) & Constants.cGt & Trim(sVar2)
                        Else
                            If sVar4 = "DATE" Then ' (sVar1 > CONVERT(DATETIME, '" & sVar2 & "',102))
                                FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cGt & Constants.cConvertClause & Constants.cSingleOpen & "DATETIME," & Trim(sVar2) & ",102" & Constants.cDoubleClose
                            End If
                        End If
                    End If
                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLLTEQL
                    If sVar4 = "" Then
                        FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cLtEq & Trim(sVar2) & Constants.cSingleClose
                    Else
                        If sVar4 = "NOWRAP" Then
                            FormatQueryString = Trim(sVar1) & Constants.cLtEq & Trim(sVar2)
                        Else
                            If sVar4 = "DATE" Then ' (sVar1 <= CONVERT(DATETIME, '" & sVar2 & "',102))
                                FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cLtEq & Constants.cConvertClause & Constants.cSingleOpen & "DATETIME," & Trim(sVar2) & ",102" & Constants.cDoubleClose
                            End If
                        End If
                    End If
                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLGTEQL
                    If sVar4 = "" Then
                        FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cGtEq & Trim(sVar2) & Constants.cSingleClose
                    Else
                        If sVar4 = "NOWRAP" Then
                            FormatQueryString = Trim(sVar1) & Constants.cGtEq & Trim(sVar2)
                        Else
                            If sVar4 = "DATE" Then ' (sVar1 >= CONVERT(DATETIME, '" & sVar2 & "',102))
                                FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cGtEq & Constants.cConvertClause & Constants.cSingleOpen & "DATETIME," & Trim(sVar2) & ",102" & Constants.cDoubleClose
                            End If
                        End If
                    End If
                    Exit Function '- String is ready to be used for SQL Statement

                Case Constants.gtSQLDATE
                    If sVar4 = "" Then ' (sVar1 = CONVERT(DATETIME, '" & sVar2 & "',102))
                        FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cEq & Constants.cConvertClause & Constants.cSingleOpen & "DATETIME," & Trim(sVar2) & ",102" & Constants.cDoubleClose
                    Else
                        If sVar4 = "RANGE" Then ' (sVar1 >= CONVERT(DATETIME, 'sVar2', 102)) AND (sVar1 <= CONVERT(DATETIME, 'sVar3', 102))
                            FormatQueryString = Constants.cSingleOpen & Trim(sVar1) & Constants.cGtEq & Constants.cConvertClause & Constants.cSingleOpen & "DATETIME," & Trim(sVar2) & ",102" & Constants.cDoubleClose
                            FormatQueryString = FormatQueryString & Constants.cAndClause & Constants.cSingleOpen & Trim(sVar1) & Constants.cLtEq & Constants.cConvertClause & Constants.cSingleOpen & "DATETIME," & Trim(sVar3) & ",102" & Constants.cDoubleClose
                        End If
                    End If
                    Exit Function '- String is ready to be used for SQL Statement
            End Select
        End If

    End Function

    Public Function get_make_model_news_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef isCRMViewActive As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                'If searchCriteria.ViewCriteriaAmodID > -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append("SELECT TOP 20 abinewslnk_date, abinewslnk_title, abinewslnk_description, abinewssrc_name, abinewslnk_web_address")
                sQuery.Append(" FROM ABI_News_Links WITH(NOLOCK) INNER JOIN ABI_News_Source WITH(NOLOCK) ON abinewslnk_source_id = abinewssrc_id WHERE ")

                If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                    Dim tmpStr As String = ""
                    For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                        If String.IsNullOrEmpty(tmpStr) Then
                            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                        Else
                            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                        End If
                    Next

                    sQuery.Append("  abinewslnk_amod_id IN (" + tmpStr.Trim + ")")
                Else
                    sQuery.Append(" abinewslnk_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
                End If



                sQuery.Append(" ORDER BY abinewslnk_date desc")
            Else
                sQuery.Append("SELECT TOP 20 abinewslnk_date, abinewslnk_title, abinewslnk_description, abinewssrc_name, abinewslnk_web_address")
                sQuery.Append(" FROM ABI_News_Links WITH(NOLOCK) INNER JOIN ABI_News_Source WITH(NOLOCK) on abinewslnk_source_id = abinewssrc_id")
                sQuery.Append(" WHERE (abinewslnk_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "')")
                sQuery.Append(" ORDER BY abinewslnk_date desc")
            End If

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
                aError = "Error in get_make_model_news_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_make_model_news_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_make_model_news(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef isCRMViewActive As Boolean)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim fAbinewslnk_web_address As String = ""
        Dim fAbinewslnk_web_address_orig As String = ""

        Try

            htmlOut.Append("<table border='0' width=""100%"" cellpadding='2' cellspacing='0'>")
            htmlOut.Append("<tr><td valign='top' class='header' align='center' colspan='2'>RECENT INDUSTRY NEWS</td></tr>")
            htmlOut.Append("<tr><td align='center' colspan='2'>")

            results_table = get_make_model_news_info(searchCriteria, isCRMViewActive)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table width=""100%"" border='0' cellpadding='4' cellspacing='0'>")

                    For Each r As DataRow In results_table.Rows

                        If Not (IsDBNull(r("abinewslnk_web_address"))) Then
                            If Not String.IsNullOrEmpty(r.Item("abinewslnk_web_address").ToString) Then

                                fAbinewslnk_web_address_orig = r.Item("abinewslnk_web_address").ToString

                                If (fAbinewslnk_web_address.ToLower.Contains("www")) Then
                                    fAbinewslnk_web_address = "<strong><a href='http://" + fAbinewslnk_web_address_orig.Trim + "' target='new'>" + r.Item("abinewslnk_title").ToString + "</a></strong>"
                                Else
                                    fAbinewslnk_web_address = "<strong><a href='" + fAbinewslnk_web_address_orig.Trim + "' target='new'>" + r.Item("abinewslnk_title").ToString + "</a></strong>"
                                End If

                            Else
                                fAbinewslnk_web_address = "<strong>" + r.Item("abinewslnk_title").ToString + "</strong>"
                            End If

                        Else
                            fAbinewslnk_web_address = "<strong>" + r.Item("abinewslnk_title").ToString + "</strong>"
                        End If

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td align='left' valign='top' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='" + fAbinewslnk_web_address_orig.Trim + "' /></td>")
                        htmlOut.Append("<td align='left' valign='top' class='seperator'><em>" + FormatDateTime(r.Item("abinewslnk_date").ToString, DateFormat.GeneralDate) + "</em>  |  " + fAbinewslnk_web_address + "<br />")
                        htmlOut.Append(Left(r.Item("abinewslnk_description").ToString, 300).ToString + " ...</td></tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("No Industry News at this time, for this Make/Model ...")
                End If

            Else
                htmlOut.Append("No Industry News at this time, for this Make/Model ...")
            End If

            htmlOut.Append("</td></tr></table>" & vbCrLf)


        Catch ex As Exception

            aError = "Error in views_display_make_model_news(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_model_wanteds_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef isCRMViewActive As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT TOP 40 Aircraft_Model_Wanted.*, comp_id, comp_name AS interested_party")
            sQuery.Append(" FROM Aircraft_Model_Wanted WITH(NOLOCK), Aircraft_Model WITH(NOLOCK), Company WITH(NOLOCK)")

            sQuery.Append(" WHERE (amwant_amod_id > 0) AND (amwant_amod_id = amod_id) AND (amwant_comp_id = comp_id)")
            sQuery.Append(" AND (amwant_journ_id = comp_journ_id) AND (amwant_journ_id = 0)")
            sQuery.Append(" AND (amwant_verified_date IS NOT NULL) AND (amod_customer_flag = 'Y')")


            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next

                sQuery.Append(" AND (amwant_amod_id IN (" + tmpStr.Trim + ")) ")
            ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" AND (amwant_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + ")")
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(" AND (amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "')")
            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))

            sQuery.Append("ORDER BY amwant_listed_date DESC, amod_make_name, amod_model_name")

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
                aError = "Error in get_model_wanteds_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_model_wanteds_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_model_wanteds(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByRef IsCRMViewActive As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Dim fAmwant_listed_date As String = ""
        Dim fInterested_party As String = ""
        Dim fAmwant_start_year As String = ""
        Dim fAmwant_end_year As String = ""
        Dim fAmwant_notes As String = ""
        Dim fAmwant_id As Long = 0
        Dim fComp_id As Long = 0

        Try

            htmlOut.Append("<table border='0' width=""100%"" cellpadding='2' cellspacing='0'>")

            results_table = get_model_wanteds_info(searchCriteria, IsCRMViewActive)

            If Not IsNothing(results_table) Then

                htmlOut.Append("<tr><td valign='top' class='header' align='center' colspan='2'>WANTED MODELS <em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")
                htmlOut.Append("<tr><td align=""center"" colspan=""2"">")

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table width=""100%"" border='0' cellpadding='4' cellspacing='0'>")

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r("amwant_listed_date")) Then
                            fAmwant_listed_date = r.Item("amwant_listed_date").ToString.Trim
                        Else
                            fAmwant_listed_date = ""
                        End If

                        If Not IsDBNull(r("interested_party")) Then
                            fInterested_party = r.Item("interested_party").ToString.Trim
                        Else
                            fInterested_party = ""
                        End If

                        If Not IsDBNull(r("amwant_start_year")) Then
                            fAmwant_start_year = r.Item("amwant_start_year").ToString.Trim
                        Else
                            fAmwant_start_year = ""
                        End If

                        If Not IsDBNull(r("amwant_end_year")) Then
                            fAmwant_end_year = r.Item("amwant_end_year").ToString.Trim
                        Else
                            fAmwant_end_year = ""
                        End If

                        If Not IsDBNull(r("amwant_notes")) Then
                            fAmwant_notes = r.Item("amwant_notes").ToString.Trim
                        Else
                            fAmwant_notes = ""
                        End If

                        If Not IsDBNull(r("amwant_id")) Then
                            fAmwant_id = CLng(r.Item("amwant_id").ToString)
                        Else
                            fAmwant_id = 0
                        End If

                        If Not IsDBNull(r("comp_id")) Then
                            fComp_id = CLng(r.Item("comp_id").ToString)
                        Else
                            fComp_id = 0
                        End If

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td align='left' valign='top' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='amwantid : " + r.Item("amwant_id").ToString + "' /></td>")
                        htmlOut.Append("<td align='left' valign='top' class='seperator'><em><a href='JavaScript:OpenSmallWindow(""DisplayWantedDetails.asp?id=" + fAmwant_id.ToString + """,""WantedDetail"");'>" + FormatDateTime(fAmwant_listed_date.ToString, DateFormat.ShortDate) + "</a></em> | ")
                        htmlOut.Append("<a target='_blank' href='DisplayCompanyDetail.aspx?compid=" + fComp_id.ToString + "'><strong>" + fInterested_party.Trim + "</strong></a><br />")

                        If Not String.IsNullOrEmpty(fAmwant_start_year) And Not String.IsNullOrEmpty(fAmwant_end_year) Then
                            htmlOut.Append("Year : " + fAmwant_start_year.Trim)
                            htmlOut.Append(" - " + fAmwant_end_year.Trim)
                        ElseIf Not String.IsNullOrEmpty(fAmwant_start_year) And String.IsNullOrEmpty(fAmwant_end_year) Then
                            htmlOut.Append("Year : " + fAmwant_start_year.Trim)
                        ElseIf String.IsNullOrEmpty(fAmwant_start_year) And Not String.IsNullOrEmpty(fAmwant_end_year) Then
                            htmlOut.Append("End Year : " + fAmwant_end_year.Trim)
                        ElseIf String.IsNullOrEmpty(fAmwant_start_year) And String.IsNullOrEmpty(fAmwant_end_year) Then
                            htmlOut.Append("Year : Open")
                        End If

                        If Not String.IsNullOrEmpty(fAmwant_notes) Then
                            htmlOut.Append(" " + Left(fAmwant_notes, 250))
                        End If

                        htmlOut.Append("</td></tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("No Wanteds at this time, for this Make/Model ...")
                End If

            Else
                htmlOut.Append("No Wanteds at this time, for this Make/Model ...")
            End If

            htmlOut.Append("</td></tr></table>" & vbCrLf)


        Catch ex As Exception

            aError = "Error in views_display_model_wanteds(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

#End Region

#Region "forsale_functions"

    Public Function Check_Jetnet_Off_Market_Aircraft(ByVal acID As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand()
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("select count(*) from aircraft with (NOLOCK) where ac_id = @ac_ID and ac_journ_id = 0 and ac_forsale_flag='Y'")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()

            SqlCommand = New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlCommand.Parameters.AddWithValue("ac_ID", acID)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try


        Catch ex As Exception
            Return Nothing

            aError = "Error in Check_Jetnet_Off_Market_Aircraft(ByVal acID As Long) As DataTable " + ex.Message

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


    Public Function get_model_forsale_info2(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal jetnet_string As String, ByVal order_by_string As String, Optional ByVal JetnetExtraCriteria As String = "", Optional ByVal displayEvalue As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        'added MSW - 5/17/2016 - 
        Dim AclsData_Temp As New clsData_Manager_SQL

        Try


            If Trim(order_by_string) = "" Then
                sQuery.Append("SELECT ac_id, ac_reg_no, ac_aport_country, emp_program_name, ac_aport_city, ac_ser_no_full, ac_delivery, ac_delivery_date,  ac_ser_no_sort, ac_year, ac_exclusive_flag, ac_mfr_year, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_interior_moyear, ac_exterior_moyear,")
                sQuery.Append(" ac_list_date, ac_status, ac_asking, ac_asking_price, ac_passenger_count, ac_journ_id, amod_make_name, amod_model_name")

                sQuery.Append(", '' as cliaircraft_value_description ")

                If displayEvalue Then
                    'added 
                    sQuery.Append(", (select afmv_value from ReturnAssetInsighteValue(ac_id)) as EVALUE ")
                    sQuery.Append(", (select AVGMODYREVALUE from ReturnAssetInsightModelYeareValue(ac_id)) as AVGMODYREVALUE ")
                End If

                sQuery.Append(", ac_engine_1_soh_hrs, ac_engine_2_soh_hrs ")

                sQuery.Append(", (select top 1  comp_id  FROM company")
                sQuery.Append(" INNER JOIN aircraft_reference WITH(NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id")
                sQuery.Append(" WHERE cref_ac_id = ac_id AND cref_journ_id = 0 AND cref_contact_type IN ('99','93','00', '08') ORDER BY cref_contact_type DESC, cref_transmit_seq_no asc) AS displaycompany ")


                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                    sQuery.Append(", (select top 1 ac_sale_price From Aircraft b with (NOLOCK) ")
                    sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id ")
                    sQuery.Append(" where(Aircraft.ac_id = b.ac_id) ")
                    sQuery.Append(" and ac_sale_price > 0 and ac_sale_price_display_flag='Y' ")
                    'sQuery.Append("  and journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N'  and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM') ")
                    sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())
                    sQuery.Append(" order by journ_date desc) as  LASTSALEPRICE ")

                    sQuery.Append(", (select top 1 journ_date From Aircraft b with (NOLOCK) ")
                    sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id ")
                    sQuery.Append(" where Aircraft.ac_id = b.ac_id and ac_sale_price > 0 and ac_sale_price_display_flag='Y' ")
                    '  sQuery.Append(" AND journ_internal_trans_flag='N'  and journ_subcat_code_part1='WS' ")
                    ' sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM') ")
                    sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())
                    sQuery.Append(" order by journ_date desc) as  LASTSALEPRICEDATE ")
                Else
                    sQuery.Append(", 0 as  LASTSALEPRICE, NULL as  LASTSALEPRICEDATE ")
                    'sQuery.Append(", ac_est_airframe_hrs  ")
                End If

                sQuery.Append(" , country_continent_name ")

                sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
                sQuery.Append(" inner Join country with (NOLOCK) on country_name = ac_aport_country ")
                sQuery.Append(" INNER Join Engine_Maintenance_Program WITH(NOLOCK) ON ac_engine_maintenance_prog_EMP = Engine_Maintenance_Program.emp_id ")

                If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                    Dim tmpStr As String = ""
                    For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                        If String.IsNullOrEmpty(tmpStr) Then
                            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                        Else
                            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                        End If
                    Next

                    sQuery.Append(" WHERE ac_amod_id IN (" + tmpStr.Trim + ") and ac_journ_id = 0 ")
                ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                    sQuery.Append(" WHERE ac_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " AND ac_journ_id = 0")
                ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                        sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "' AND ac_journ_id = 0")
                    End If
                End If

                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

                If JetnetExtraCriteria <> "" Then
                    sQuery.Append(JetnetExtraCriteria)
                End If

                sQuery.Append(" AND ac_forsale_flag = 'Y'")

                Select Case (searchCriteria.ViewCriteriaSortBy.ToLower)
                    Case "serno"
                        sQuery.Append(" ORDER BY ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

                    Case "aftt"
                        sQuery.Append(" ORDER BY ac_airframe_tot_hrs, ac_ser_no_sort, ac_list_date, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

                    Case "mfryear"
                        sQuery.Append(" ORDER BY ac_mfr_year, ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_year, ac_asking_price desc, ac_asking asc")

                    Case "acyear"
                        sQuery.Append(" ORDER BY ac_year, ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_asking_price desc, ac_asking asc")

                    Case "listdate"
                        sQuery.Append(" ORDER BY ac_list_date, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

                    Case "asking"
                        sQuery.Append(" ORDER BY ac_asking_price desc, ac_asking asc, ac_list_date, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year, ac_year")

                    Case Else
                        sQuery.Append(" ORDER BY ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

                End Select
            Else

                sQuery.Append("SELECT distinct ")
                sQuery.Append(" ac_id, ac_ser_no_sort,  ac_ser_no_full, ")

                If displayEvalue Then
                    'added 
                    sQuery.Append(" (select afmv_value from ReturnAssetInsighteValue(ac_id)) as EVALUE ")
                    sQuery.Append(", (select AVGMODYREVALUE from ReturnAssetInsightModelYeareValue(ac_id)) as AVGMODYREVALUE, ")
                End If


                'If InStr(jetnet_string, " as ac_id") > 0 Then
                '  jetnet_string = jetnet_string
                'ElseIf InStr(jetnet_string, " as ac_ser_no_full") > 0 Then
                '  jetnet_string = jetnet_string
                'End If

                sQuery.Append(jetnet_string)

                sQuery.Append(" FROM aircraft WITH (NOLOCK) ")

                sQuery.Append(" inner JOIN aircraft_reference WITH (NOLOCK) ON aircraft_reference.cref_ac_id = ac_id and cref_journ_id = ac_journ_id ")
                sQuery.Append(" inner JOIN company with (NOLOCK) on  aircraft_reference.cref_comp_id = comp_id and cref_journ_id = comp_journ_id ")
                sQuery.Append(" inner JOIN aircraft_model WITH (NOLOCK) ON aircraft.ac_amod_id = aircraft_model.amod_id ")
                sQuery.Append(" inner JOIN aircraft_contact_type WITH (NOLOCK) ON aircraft_reference.cref_contact_type = aircraft_contact_type.actype_code ")

                sQuery.Append(" left outer join contact with (NOLOCK) on cref_contact_id = contact_id and cref_journ_id = contact_journ_id and contact_active_flag='Y' and contact_hide_flag='N' ")



                If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                    Dim tmpStr As String = ""
                    For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                        If String.IsNullOrEmpty(tmpStr) Then
                            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                        Else
                            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                        End If
                    Next

                    sQuery.Append(" WHERE ac_amod_id IN (" + tmpStr.Trim + ") and ac_journ_id = 0 ")
                ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                    sQuery.Append(" WHERE ac_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " AND ac_journ_id = 0")
                ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                        sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "' AND ac_journ_id = 0")
                    End If
                End If

                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

                sQuery.Append(" AND ac_forsale_flag = 'Y'")

                If JetnetExtraCriteria <> "" Then
                    sQuery.Append(JetnetExtraCriteria)
                End If
                sQuery.Append(" order by " & order_by_string)
            End If

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
                aError = "Error in get_model_forsale_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_model_forsale_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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



    Public Function get_model_forsale_info(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal displayEValues As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()
        Dim AclsData_Temp As New clsData_Manager_SQL
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try


            sQuery.Append("SELECT ac_id, ac_est_airframe_hrs, ac_ser_no_full, emp_program_name, ac_delivery_date, ac_delivery, ac_aport_city, ac_aport_country, ac_ser_no_sort, ac_reg_no, ac_year, ac_mfr_year, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_interior_moyear, ac_exterior_moyear,")
            sQuery.Append(" ac_list_date, ac_status, ac_asking, ac_asking_price, ac_passenger_count, ac_journ_id, amod_make_name, amod_model_name")
            sQuery.Append(" , country_continent_name ")

            If HttpContext.Current.Session.Item("isMobile") = True Then
                sQuery.Append(" ,ac_picture_id,ac_aport_icao_code,ac_aport_iata_code, amod_airframe_type_code, amod_id, ac_forsale_flag, ac_delivery, ac_times_as_of_date, ")
                sQuery.Append(" ac_engine_1_soh_hrs, ac_engine_2_soh_hrs,ac_engine_3_soh_hrs,ac_engine_4_soh_hrs, ")
                sQuery.Append(" ac_last_event")
            End If


            sQuery.Append(", ac_engine_1_soh_hrs, ac_engine_2_soh_hrs ")

            If displayEValues Then
                sQuery.Append(", (select afmv_value from ReturnAssetInsighteValue(ac_id)) as EVALUE ")
                sQuery.Append(", (select AVGMODYREVALUE from ReturnAssetInsightModelYeareValue(ac_id)) as AVGMODYREVALUE ")
            End If

            If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                sQuery.Append(", (select top 1 ac_sale_price From Aircraft b with (NOLOCK) ")
                sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id ")
                sQuery.Append(" where(Aircraft_Flat.ac_id = b.ac_id) ")
                sQuery.Append(" and ac_sale_price > 0 and ac_sale_price_display_flag='Y' ")
                sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())
                sQuery.Append(" order by journ_date desc) as  LASTSALEPRICE ")

                sQuery.Append(", (select top 1 journ_date From Aircraft b with (NOLOCK) ")
                sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id ")
                sQuery.Append(" where Aircraft_Flat.ac_id = b.ac_id and ac_sale_price > 0 and ac_sale_price_display_flag='Y'  ")
                sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())

                sQuery.Append(" order by journ_date desc) as  LASTSALEPRICEDATE ")

            End If


            ' changed msw = 7/28/16 per request
            'sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
            sQuery.Append(" FROM Aircraft_Flat WITH(NOLOCK) ")
            sQuery.Append(" left outer Join country with (NOLOCK) on country_name = ac_aport_country ")

            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next

                sQuery.Append(" WHERE amod_id IN (" + tmpStr.Trim + ") and ac_journ_id = 0 ")
            ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " AND ac_journ_id = 0")
            ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "' AND ac_journ_id = 0")
                End If
            End If
            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If
            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

            sQuery.Append(" AND ac_forsale_flag = 'Y'")

            Select Case (searchCriteria.ViewCriteriaSortBy.ToLower)
                Case "serno"
                    sQuery.Append(" ORDER BY ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

                Case "aftt"
                    sQuery.Append(" ORDER BY ac_airframe_tot_hrs, ac_ser_no_sort, ac_list_date, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

                Case "mfryear"
                    sQuery.Append(" ORDER BY ac_mfr_year, ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_year, ac_asking_price desc, ac_asking asc")

                Case "acyear"
                    sQuery.Append(" ORDER BY ac_year, ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_asking_price desc, ac_asking asc")

                Case "listdate"
                    sQuery.Append(" ORDER BY ac_list_date, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

                Case "asking"
                    sQuery.Append(" ORDER BY ac_asking_price desc, ac_asking asc, ac_list_date, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year, ac_year")

                Case Else
                    sQuery.Append(" ORDER BY ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

            End Select

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
                aError = "Error in get_model_forsale_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_model_forsale_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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


    Public Sub views_display_aircraft_forsale(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal is_extra_criteria As Boolean, Optional ByVal displayEValues As Boolean = False)
        Dim AclsData_Temp As New clsData_Manager_SQL
        AclsData_Temp = New clsData_Manager_SQL
        AclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
        AclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")


        Dim arrFeatCodes() As String = Nothing
        Dim arrStdFeatCodes(,) As String = Nothing

        Dim strOut As New StringBuilder
        Dim htmlOut As New StringBuilder

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Dim bHadStatus As Boolean = False
        Dim cellWidth As Integer = 20

        Dim nFeatureCountForSpan As Integer = 0
        Dim table_height As Integer = 0
        Dim sCompanyPhone As String = ""

        Try

            'strOut.Append("<table id='forSaleOuterTable' cellspacing='0' cellpadding='0' width=""100%"">")

            'If Not searchCriteria.ViewCriteriaIsReport Then
            '  strOut.Append("<tr><td valign='middle' align='center' class='header' style='padding-left:3px;'>")
            '  strOut.Append("&nbsp;&nbsp;Sort List By&nbsp;<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + IIf(is_extra_criteria, "&extra=true", "") + "&sortBy=serno&activetab=1' class='White'><b>Serial&nbsp;#</b></a>&nbsp;or&nbsp;")
            '  strOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + IIf(is_extra_criteria, "&extra=true", "") + "&sortBy=aftt&activetab=1' class='White'><b>AFTT</b></a>&nbsp;or&nbsp;")
            '  strOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + IIf(is_extra_criteria, "&extra=true", "") + "&sortBy=mfryear&activetab=1' class='White'><b>Aircraft&nbsp;MFR&nbsp;Year</b></a>&nbsp;or&nbsp;")
            '  strOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + IIf(is_extra_criteria, "&extra=true", "") + "&sortBy=acyear&activetab=1' class='White'><b>Aircraft&nbsp;DLV&nbsp;Year</b></a>&nbsp;or&nbsp;")
            '  strOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + IIf(is_extra_criteria, "&extra=true", "") + "&sortBy=listdate&activetab=1' class='White'><b>Date&nbsp;Listed</b></a>&nbsp;or&nbsp;")
            '  strOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + IIf(is_extra_criteria, "&extra=true", "") + "&sortBy=asking&activetab=1' class='White'><b>Asking&nbsp;Price</b></a>")
            'Else
            '  strOut.Append("<tr bgcolor='#CCCCCC'><td valign='middle' align='center' class='header' style='padding-left:3px;'><strong>")
            'End If

            results_table = get_model_forsale_info(searchCriteria, displayEValues)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    'If Not searchCriteria.ViewCriteriaIsReport Then
                    'strOut.Append("&nbsp;&nbsp;&nbsp;&nbsp;AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em>") '</td></tr>")
                    'Else
                    '  strOut.Append("AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></strong></td></tr>")
                    'End If

                    'If Not searchCriteria.ViewCriteriaIsReport Then
                    '  htmlOut.Append("<table id='tableCopy' cellpadding='0' cellspacing='0' border='0'>")
                    '  htmlOut.Append("<thead><th>&nbsp;</th>")
                    '  htmlOut.Append("<th class=""text_align_center"">SERIAL<br />NUMBER</th>")
                    'Else
                    htmlOut.Append("<table id='tableCopy' cellpadding='0' cellspacing='0' border='0'>")
                    htmlOut.Append("<thead><tr><th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")
                    htmlOut.Append("<th>HIDDEN ID</th>")
                    'If (searchCriteria.ViewCriteriaNoLocalNotes = False And Not searchCriteria.ViewCriteriaIsReport) Then

                    '  htmlOut.Append("<th class=""text_align_center"">NOTE</th>")

                    'ElseIf (HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) And Not searchCriteria.ViewCriteriaIsReport) Then
                    '  htmlOut.Append("<th class=""text_align_center"">NOTE</th>")
                    'End If
                    htmlOut.Append("<th class=""text_align_center"">SERIAL<br />NUMBER</th>")
                    'End If
                    htmlOut.Append("<th class=""text_align_center"">REG<br />NUMBER</th>")
                    htmlOut.Append("<th class=""text_align_center"" width='20'>YEAR MFR</th>")
                    htmlOut.Append("<th class=""text_align_center"" width='20'>YEAR DLV</th>")

                    htmlOut.Append("<th class=""text_align_center""  width=""210"">OWNER</th>")

                    If is_extra_criteria Or searchCriteria.ViewCriteriaIsReport Then
                        htmlOut.Append("<th class=""text_align_center"">OWNER PHONE</th>")
                    End If

                    If is_extra_criteria Or searchCriteria.ViewCriteriaIsReport Then
                        htmlOut.Append("<th class=""text_align_center"" width=""210"">OPERATOR</th>")
                        htmlOut.Append("<th class=""text_align_center"">OPERATOR PHONE</th>")
                    End If

                    htmlOut.Append("<th class=""text_align_center"" width=""210"">BROKER</th>")

                    If is_extra_criteria Or searchCriteria.ViewCriteriaIsReport Then
                        htmlOut.Append("<th class=""text_align_center"">BROKER PHONE</th>")
                    End If

                    htmlOut.Append("<th class=""text_align_center"">ASKING ($k)</th>")
                    htmlOut.Append("<th class=""text_align_center"">DATE LISTED</th>")

                    If displayEValues Then 'evalues 
                        ' eValue and Model Year Avg eValue
                        htmlOut.Append("<th class=""" & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>" & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & "</th>")
                        htmlOut.Append("<th class=""" & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>MODEL YEAR AVG " & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & "</th>")
                    End If

                    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                        htmlOut.Append("<th class=""text_align_center"">LAST SALE PRICE ($k)</th>")
                        htmlOut.Append("<th class=""text_align_center"">SALE PRICE DATE</th>")
                        htmlOut.Append("<th class=""text_align_center"">EST AFTT</th>")
                    End If





                    htmlOut.Append("<th class=""text_align_center"">AFTT</th>")
                    htmlOut.Append("<th class=""text_align_center"">ENGINE TT</th>")
                    htmlOut.Append("<th>ENG 1<br />SOH</th>")
                    htmlOut.Append("<th>ENG 2<br />SOH</th>")
                    load_standard_ac_features(searchCriteria, arrStdFeatCodes)

                    Dim sNonStandardAcFeature As String = ""
                    display_nonstandard_feature_code_headings(searchCriteria, arrFeatCodes, arrStdFeatCodes, cellWidth, sNonStandardAcFeature)
                    htmlOut.Append(sNonStandardAcFeature)

                    htmlOut.Append("<th class=""text_align_center"" title='Number Of Passengers'>PAX</th>")
                    htmlOut.Append("<th class=""text_align_center"">INT<br />YEAR</th>")

                    'If (HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName)) Then
                    htmlOut.Append("<th class=""text_align_center"">EXT<br />YEAR</th>")
                    'Else
                    '  htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorderNoNotes'><strong>EXT<br />YEAR</strong></th>")
                    'End If

                    htmlOut.Append("<th>ENGINE MAINTENANCE PROGRAM</th>")
                    If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Or HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                        htmlOut.Append("<th><span  class=""help_cursor"" title=""Note indicator. Use the mouse over to see the latest note or click to add a note."">NTE</span></th>")
                    End If

                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                        htmlOut.Append("<th>Status</th>")
                    End If

                    htmlOut.Append("<th>Based</th>")



                    htmlOut.Append("</tr></thead><tbody>")

                    For Each r As DataRow In results_table.Rows

                        ' set the ac_id for this listing
                        searchCriteria.ViewCriteriaAircraftID = CLng(r.Item("ac_id").ToString)

                        'If Not toggleRowColor Then
                        '  htmlOut.Append("<tr class='alt_row'>")
                        '  toggleRowColor = True
                        'Else
                        '  htmlOut.Append("<tr bgcolor='white'>")
                        '  toggleRowColor = False
                        'End If

                        'If (searchCriteria.ViewCriteriaNoLocalNotes = False And Not searchCriteria.ViewCriteriaIsReport) Then

                        '  htmlOut.Append("<td class=""text_align_center"">")  ' Note ICON
                        '  htmlOut.Append("<div style='visibility: hidden;' id='bHasNotesGif" + r.Item("ac_id").ToString + "ID'><a href='javascript:displayLocalAircraftNoteJS(" + r.Item("ac_id").ToString + ",0,0);'><img src='images/Notes.gif' border='0'></a></div>")
                        '  htmlOut.Append("</td>")

                        'ElseIf (HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) And Not searchCriteria.ViewCriteriaIsReport) Then

                        '  htmlOut.Append("<td class=""text_align_center"">")  ' Note ICON
                        '  htmlOut.Append("<div style='visibility: hidden;' id='bHasNotesGif" + r.Item("ac_id").ToString + "ID'><a class='underline' onclick='javascript:callNoteViewImg" + r.Item("ac_id").ToString + "();'><img src='images/Notes.gif' border='0'></a></div>")
                        '  htmlOut.Append("</td>")

                        'End If
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<td></td>")
                        htmlOut.Append("<td>" & CLng(r.Item("ac_id").ToString) & "</td>")
                        htmlOut.Append("<td class=""text_align_center"" nowrap='nowrap' data-sort=""" & IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), "") & """>")  ' SERIAL NUMBER

                        If Not searchCriteria.ViewCriteriaIsReport Then
                            If Not IsDBNull(r("ac_ser_no_full")) Then
                                htmlOut.Append(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_no_full"), "underline", ""))

                                'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                'htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")
                            Else
                                htmlOut.Append("&nbsp;")
                            End If
                        Else
                            If Not IsDBNull(r("ac_ser_no_full")) Then
                                htmlOut.Append(r.Item("ac_ser_no_full").ToString)
                            Else
                                htmlOut.Append(" ")
                            End If
                        End If

                        htmlOut.Append("</td>")
                        htmlOut.Append("<td class=""text_align_center"">") ' REG NBR
                        If Not IsDBNull(r("ac_reg_no")) Then
                            htmlOut.Append(r.Item("ac_reg_no").ToString)
                        Else
                            htmlOut.Append(" ")
                        End If
                        htmlOut.Append("</td>")
                        htmlOut.Append("<td class=""text_align_center"" width='20'>") ' YR MFG

                        If Not IsDBNull(r("ac_mfr_year")) Then
                            If CDbl(r.Item("ac_mfr_year").ToString) = 0 Then
                                htmlOut.Append("0")
                            Else
                                htmlOut.Append(r.Item("ac_mfr_year").ToString)
                            End If
                        Else
                            htmlOut.Append("U")
                        End If

                        htmlOut.Append("</td><td class=""text_align_center"" width='20'>") ' YR DLV

                        If Not IsDBNull(r("ac_year")) Then
                            If CDbl(r.Item("ac_year").ToString) = 0 Then
                                htmlOut.Append("0")
                            Else
                                htmlOut.Append(r.Item("ac_year").ToString)
                            End If
                        Else
                            htmlOut.Append("U")
                        End If

                        htmlOut.Append("</td><td width=""250"">") ' OWNER            

                        searchCriteria.ViewCriteriaGetExclusive = False
                        searchCriteria.ViewCriteriaGetOperator = False

                        Dim ownerDataTable As DataTable = crmViewDataLayer.GetOwnerExclusiveOperatorInformation(searchCriteria)

                        If Not IsNothing(ownerDataTable) Then

                            If ownerDataTable.Rows.Count > 0 Then
                                For Each vr_owner As DataRow In ownerDataTable.Rows

                                    sCompanyPhone = crmViewDataLayer.ReturnCompanyPhoneFax(vr_owner("comp_phone_office"), vr_owner("comp_phone_fax"))

                                    'If String.IsNullOrEmpty(sCompanyPhone) Then
                                    '  sCompanyPhone = "Not listed"
                                    'End If

                                    If Not searchCriteria.ViewCriteriaIsReport Then

                                        If is_extra_criteria Then
                                            htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_owner.Item("comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))
                                            htmlOut.Append(">" + Replace(vr_owner.Item("comp_name").ToString.Trim, "'", "") + "</a></td>") ' OWNER
                                            ' htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");' title='Display company Details'>" + Replace(vr_owner.Item("comp_name").ToString.Trim, "'", "") + "</a></td>") ' OWNER
                                            htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder'>" + sCompanyPhone) ' OWNERPHONE  
                                        Else
                                            htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_owner.Item("comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))

                                            'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");' ") ' OWNER
                                            htmlOut.Append(" title='PH [ " + sCompanyPhone.Replace("<br />", " ") + " ]'>" + Replace(vr_owner.Item("comp_name").ToString.Trim, "'", "") + "</a>") ' OWNERPHONE
                                        End If

                                    Else
                                        htmlOut.Append(vr_owner.Item("comp_name").ToString.Trim + "</td>") ' OWNER
                                        htmlOut.Append("<td>" + sCompanyPhone.Replace("<br />", " ")) ' OWNERPHONE  
                                    End If

                                Next

                            Else

                                If is_extra_criteria Or searchCriteria.ViewCriteriaIsReport Then
                                    htmlOut.Append("</td><td>") ' OWNER OWNERPHONE  
                                Else
                                    'htmlOut.Append("None") ' OWNER
                                End If

                            End If

                        Else

                            If is_extra_criteria Or searchCriteria.ViewCriteriaIsReport Then
                                htmlOut.Append("</td><td>") ' OWNER OWNERPHONE  
                            Else
                                ' htmlOut.Append("None") ' OWNER
                            End If

                        End If

                        ownerDataTable = Nothing

                        If searchCriteria.ViewCriteriaIsReport Or is_extra_criteria Then  ' OPERATOR

                            searchCriteria.ViewCriteriaGetExclusive = False
                            searchCriteria.ViewCriteriaGetOperator = True

                            Dim operatorDataTable As DataTable = crmViewDataLayer.GetOwnerExclusiveOperatorInformation(searchCriteria)

                            If Not IsNothing(operatorDataTable) Then

                                If operatorDataTable.Rows.Count > 0 Then
                                    For Each r_operator As DataRow In operatorDataTable.Rows

                                        sCompanyPhone = crmViewDataLayer.ReturnCompanyPhoneFax(r_operator("comp_phone_office"), r_operator("comp_phone_fax"))

                                        'If String.IsNullOrEmpty(sCompanyPhone) Then
                                        '  sCompanyPhone = "Not listed"
                                        'End If

                                        If is_extra_criteria Then
                                            htmlOut.Append("</td><td>" & DisplayFunctions.WriteDetailsLink(0, r_operator.Item("comp_id").ToString, 0, 0, True, Replace(r_operator.Item("comp_name").ToString.Trim, "'", ""), "underline", "&journid=0") & "</td>")
                                            'htmlOut.Append("</td><td><a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r_operator.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");' title='Display company Details'>" + Replace(r_operator.Item("comp_name").ToString.Trim, "'", "") + "</a></td>") ' OWNER
                                        Else
                                            htmlOut.Append("</td><td>" + Replace(r_operator.Item("comp_name").ToString.Trim, "'", "") + "</td>")
                                        End If

                                        If Not searchCriteria.ViewCriteriaIsReport Then
                                            htmlOut.Append("<td>" + sCompanyPhone)  ' OPERATORPHONE
                                        Else
                                            htmlOut.Append("<td>" + sCompanyPhone.Replace("<br />", " ")) ' OPERATORPHONE  
                                        End If

                                    Next
                                Else
                                    htmlOut.Append("</td><td></td>") ' OPERATOR
                                    htmlOut.Append("<td>") ' OPERATORPHONE         
                                End If
                            Else
                                htmlOut.Append("</td><td></td>") ' OPERATOR
                                htmlOut.Append("<td>") ' OPERATORPHONE         
                            End If

                            operatorDataTable = Nothing

                        End If

                        htmlOut.Append("</td><td width=""250"">") ' BROKER

                        searchCriteria.ViewCriteriaGetExclusive = True
                        searchCriteria.ViewCriteriaGetOperator = False

                        Dim exclusiveDataTable As DataTable = crmViewDataLayer.GetOwnerExclusiveOperatorInformation_Multiple(searchCriteria)

                        If Not IsNothing(exclusiveDataTable) Then

                            sCompanyPhone = ""
                            If exclusiveDataTable.Rows.Count > 0 Then
                                For Each vr_exclusive As DataRow In exclusiveDataTable.Rows

                                    If Trim(sCompanyPhone) <> "" Then
                                        htmlOut.Append(", ")
                                    End If
                                    sCompanyPhone = crmViewDataLayer.ReturnCompanyPhoneFax(vr_exclusive("comp_phone_office"), vr_exclusive("comp_phone_fax"))

                                    'If String.IsNullOrEmpty(sCompanyPhone) Then
                                    '  sCompanyPhone = "Not listed"
                                    'End If

                                    If Not searchCriteria.ViewCriteriaIsReport Then

                                        If is_extra_criteria Then
                                            htmlOut.Append("<a " & DisplayFunctions.WriteDetailsLink(0, vr_exclusive.Item("comp_id").ToString, 0, 0, False, "", "underline", "") & ">")
                                            'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");' title='Display company Details'>")
                                            htmlOut.Append("<font style='color:purple;'>" + Replace(vr_exclusive.Item("comp_name").ToString.Trim, "'", "") + "</font></a></td>") ' BROKER
                                            htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder'>" + sCompanyPhone) ' BROKERPHONE  
                                        Else
                                            htmlOut.Append("<a " & DisplayFunctions.WriteDetailsLink(0, vr_exclusive.Item("comp_id").ToString, 0, 0, False, "", "underline", ""))
                                            'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'") ' BROKER
                                            htmlOut.Append(" title='PH [ " + sCompanyPhone.Replace("<br />", " ") + " ]'><font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></a>") ' BROKERPHONE
                                        End If

                                    Else
                                        htmlOut.Append("<font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></td>")
                                        htmlOut.Append("<td>" + sCompanyPhone) ' BROKERPHONE  
                                    End If

                                Next

                            Else

                                If is_extra_criteria Or searchCriteria.ViewCriteriaIsReport Then
                                    htmlOut.Append("</td><td> ") ' BROKER BROKERPHONE  
                                Else
                                    '  htmlOut.Append("None") ' BROKER
                                End If
                            End If

                        Else

                            If is_extra_criteria Or searchCriteria.ViewCriteriaIsReport Then
                                htmlOut.Append("</td><td> ") ' BROKER BROKERPHONE  
                            Else
                                ' htmlOut.Append("None") ' BROKER
                            End If

                        End If

                        exclusiveDataTable = Nothing

                        htmlOut.Append("</td><td align='right'>") ' ASKING

                        bHadStatus = False
                        If Not IsDBNull(r("ac_Status")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_Status").ToString) Then
                                If r.Item("ac_Status").ToString.ToLower.Trim.Contains("for sale") Then
                                    htmlOut.Append(forsale_status(r.Item("ac_Status").ToString.Trim))
                                    bHadStatus = True
                                End If
                            End If
                        End If

                        If bHadStatus Then
                            htmlOut.Append(" ")
                        End If

                        If Not IsDBNull(r("ac_asking")) Then
                            If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                                        htmlOut.Append("$" + FormatNumber((CDbl(r.Item("ac_asking_price").ToString) / 1000), 0).ToString + "")
                                    End If
                                End If
                            Else
                                htmlOut.Append(forsale_status(r.Item("ac_asking").ToString.Trim))
                            End If
                        End If


                        htmlOut.Append(" </td><td class=""text_align_center""") ' AC LIST DATE

                        Dim dateSort As String = ""
                        If Not IsDBNull(r.Item("ac_list_date")) Then
                            If IsDate(r.Item("ac_list_date").ToString) Then
                                dateSort = Format(r.Item("ac_list_date"), "yyyy/MM/dd")

                                htmlOut.Append(" data-sort='" & dateSort & "'")
                            End If
                        End If

                        htmlOut.Append(">")
                        If Not IsDBNull(r.Item("ac_list_date")) Then
                            If IsDate(r.Item("ac_list_date").ToString) Then
                                htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate)))
                            Else
                                htmlOut.Append(" ")
                            End If
                        Else
                            htmlOut.Append(" ")
                        End If

                        If displayEValues Then 'evalues 
                            ' eValue and Model Year Avg eValue
                            htmlOut.Append("</td><td class=""text_align_center " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>")
                            If Not IsDBNull(r("EVALUE")) Then
                                If IsNumeric(r("EVALUE")) Then
                                    If r("EVALUE") > 0 Then
                                        htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("EVALUE")))
                                    End If
                                End If
                            End If
                            htmlOut.Append("</td>")
                            htmlOut.Append("</td><td class=""text_align_center " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>")
                            If Not IsDBNull(r("AVGMODYREVALUE")) Then
                                If IsNumeric(r("AVGMODYREVALUE")) Then
                                    If r("AVGMODYREVALUE") > 0 Then
                                        htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("AVGMODYREVALUE")))
                                    End If
                                End If
                            End If
                            htmlOut.Append("</td>")
                        End If



                        If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                            htmlOut.Append(" </td><td class=""text_align_center"">") ' AC LIST DATE
                            If Not IsDBNull(r("LASTSALEPRICE")) Then
                                If Not IsDBNull(r("LASTSALEPRICE")) Then
                                    If CDbl(r.Item("LASTSALEPRICE").ToString) > 0 Then
                                        htmlOut.Append(DisplayFunctions.TextToImage("$" & FormatNumber((r.Item("LASTSALEPRICE").ToString / 1000), 0) & "k", 7, "", "42", "Reported Sale Price Displayed with Permission from Source"))
                                    End If
                                End If
                            End If
                            htmlOut.Append(" </td><td class=""text_align_center"">") ' AC LIST DATE

                            If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                    htmlOut.Append("" & FormatDateTime(r.Item("LASTSALEPRICEDATE"), DateFormat.ShortDate))
                                End If
                            End If

                            htmlOut.Append(" </td><td class=""text_align_center"">") ' AC LIST DATE

                            If Not IsDBNull(r("ac_est_airframe_hrs")) Then
                                If Not IsDBNull(r("ac_est_airframe_hrs")) Then
                                    htmlOut.Append("" + (r.Item("ac_est_airframe_hrs").ToString + ""))
                                End If
                            End If
                        End If




                        htmlOut.Append("</td><td class=""text_align_center"">") ' AFTT

                        If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
                            If CDbl(r.Item("ac_airframe_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("0")
                            Else
                                htmlOut.Append(r.Item("ac_airframe_tot_hrs").ToString)
                            End If
                        Else
                            htmlOut.Append("U")
                        End If

                        htmlOut.Append("</td><td class=""text_align_center"">") ' Engine Times

                        If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
                            If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("[0] ")
                            Else
                                htmlOut.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "] ")
                            End If
                        Else
                            htmlOut.Append("[U] ")
                        End If

                        If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
                            If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("[0] ")
                            Else
                                htmlOut.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "] ")
                            End If
                        Else
                            htmlOut.Append("[U] ")
                        End If

                        If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
                            If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("[0] ")
                            Else
                                htmlOut.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "] ")
                            End If
                        End If

                        If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
                            If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
                                htmlOut.Append("[0] ")
                            Else
                                htmlOut.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "] ")
                            End If
                        End If

                        htmlOut.Append("</td>")
                        ''''''''''''''''''
                        htmlOut.Append("<td class=""text_align_center"">") ' AFTT

                        If Not IsDBNull(r("ac_engine_1_soh_hrs")) Then
                            If CDbl(r.Item("ac_engine_1_soh_hrs").ToString) = 0 Then
                                htmlOut.Append("0")
                            Else
                                htmlOut.Append(r.Item("ac_engine_1_soh_hrs").ToString)
                            End If
                        Else
                            htmlOut.Append("")
                        End If


                        htmlOut.Append("</td><td class=""text_align_center"">") ' AFTT

                        If Not IsDBNull(r("ac_engine_2_soh_hrs")) Then
                            If CDbl(r.Item("ac_engine_2_soh_hrs").ToString) = 0 Then
                                htmlOut.Append("0")
                            Else
                                htmlOut.Append(r.Item("ac_engine_2_soh_hrs").ToString)
                            End If
                        Else
                            htmlOut.Append("")
                        End If
                        htmlOut.Append("</td>")
                        Dim sAcFeatureCodes As String = ""

                        display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)


                        htmlOut.Append(sAcFeatureCodes)

                        htmlOut.Append("<td class=""text_align_center"">") ' PASSENGERS

                        If Not IsDBNull(r("ac_passenger_count")) Then
                            If CDbl(r.Item("ac_passenger_count").ToString) = 0 Then
                                htmlOut.Append("0 ")
                            Else
                                htmlOut.Append(r.Item("ac_passenger_count").ToString + " ")
                            End If
                        Else
                            htmlOut.Append("U ")
                        End If

                        htmlOut.Append("</td><td class=""text_align_center"">") ' INT YEAR

                        If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                            htmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)
                            If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
                                htmlOut.Append("/")
                            End If
                            htmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + " ")
                        Else
                            htmlOut.Append(" ")
                        End If

                        htmlOut.Append("</td><td class=""text_align_center"">") ' EXT YEAR

                        If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                            htmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)
                            If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
                                htmlOut.Append("/")
                            End If
                            htmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + " ")
                        Else
                            htmlOut.Append(" ")
                        End If

                        htmlOut.Append("<td class=""text_align_center"">") ' EMP NAME
                        If Not IsDBNull(r("emp_program_name")) Then
                            htmlOut.Append(r.Item("emp_program_name").ToString)
                        Else
                            htmlOut.Append(" ")
                        End If
                        htmlOut.Append("</td>")

                        If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Or HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                            Dim HTML_NOTE As String = ""
                            Dim IconDisplay As String = ""
                            HTML_NOTE = crmViewDataLayer.CheckForNotesForSaleTab(False, "JETNET", r.Item("ac_id"), AclsData_Temp)

                            IconDisplay = HTML_NOTE
                            IconDisplay = Replace(IconDisplay, "class=""float_left""", "class=""text_align_center""")

                            HTML_NOTE = Replace(HTML_NOTE, "img src=""images/document.png""", "span")
                            HTML_NOTE = Replace(HTML_NOTE, "class=""float_left""", "class=""display_none""")
                            HTML_NOTE = Replace(HTML_NOTE, "title='", ">")
                            HTML_NOTE = Replace(HTML_NOTE, "'/>", "</span>")
                            htmlOut.Append("</td><td class=""text_align_center"" title='Most Recent Local Note'>") ' NOTES
                            htmlOut.Append(HTML_NOTE + IconDisplay)

                        End If

                        htmlOut.Append("</td>")

                        'NEW FIELDS 6/12/17

                        'Aircraft Status
                        If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                            htmlOut.Append("<td>") 'STATUS

                            ' Don't show status for Aerodex Users
                            If Not IsDBNull(r.Item("ac_status")) And Not String.IsNullOrEmpty(r.Item("ac_status").ToString) Then
                                htmlOut.Append(UCase(r.Item("ac_status").ToString.Trim + " "))
                            End If
                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            'Aircraft Delivery / Delivery Date
                            If Not IsDBNull(r.Item("ac_delivery")) And Not String.IsNullOrEmpty(r.Item("ac_delivery").ToString) Then
                                If r.Item("ac_delivery").ToString.ToLower.Contains("date") Then
                                    If Not IsDBNull(r.Item("ac_delivery_date")) And Not String.IsNullOrEmpty(r.Item("ac_delivery_date").ToString) Then
                                        htmlOut.Append(UCase(("" + FormatDateTime(r.Item("ac_delivery_date").ToString, DateFormat.ShortDate) + "")))
                                    End If
                                Else
                                    htmlOut.Append(UCase(("" + r.Item("ac_delivery").ToString.Trim + "")))
                                End If
                            End If
                            htmlOut.Append("</td>")
                        End If

                        htmlOut.Append("<td>")
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
                        If Not IsDBNull(r.Item("country_continent_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("country_continent_name")) Then
                                If AportInfo <> "" Then
                                    AportInfo += " - "
                                End If
                                AportInfo += ((" " + Replace(r.Item("country_continent_name").ToString.Trim, "United States", "US") + ""))
                            End If
                        End If

                        htmlOut.Append(AportInfo)
                        htmlOut.Append("</td>") 'BASED 





                        htmlOut.Append("</tr>")

                    Next

                Else
                    htmlOut.Append("<table cellpadding='0' cellspacing='0' border='0'><tbody><tr><td>Nothing For Sale at this time, for this Make/Model ...</td></tr>")
                End If

            Else
                htmlOut.Append("<table cellpadding='0' cellspacing='0' border='0'><tbody><tr><td>Nothing For Sale at this time, for this Make/Model ...</td></tr>")
            End If

            htmlOut.Append("</tbody></table>")

            strOut.Append("<span id=""openNewWindowContents"">" + htmlOut.ToString() + "</span><div class=""resizeCW""><div id=""forSaleInnerTable"" " & IIf(HttpContext.Current.Session.Item("isMobile"), "", "style=""width: 100%;""") & "></div></div>")

        Catch ex As Exception

            aError = "Error in views_display_aircraft_forsale(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = strOut.ToString
        htmlOut = Nothing
        strOut = Nothing
        results_table = Nothing

    End Sub

    Public Function forsale_status(ByVal in_StrToTranslate As String) As String

        Dim alt_tag_start As String = "<a title='" + in_StrToTranslate.Trim.ToUpper + "' style='text-decoration: none' class=""help_cursor"">"
        Dim alt_tag_end As String = "</a>"

        Dim sOutTrans As String = ""

        Select Case (in_StrToTranslate.Trim.ToUpper)

            Case "MAKE OFFER"
                sOutTrans = alt_tag_start + "M/O" + alt_tag_end
            Case "SHARE"
                sOutTrans = alt_tag_start + "Share" + alt_tag_end
            Case "LEASE"
                sOutTrans = alt_tag_start + "LS" + alt_tag_end
            Case "FOR SALE/LEASE"
                sOutTrans = alt_tag_start + "FS/LS" + alt_tag_end
            Case "SALE/LEASE"
                sOutTrans = alt_tag_start + "FS/LS" + alt_tag_end
            Case "LEASE ONLY"
                sOutTrans = alt_tag_start + "LS/O" + alt_tag_end
            Case "SEALED BID"
                sOutTrans = alt_tag_start + "BID" + alt_tag_end
            Case "TRADE"
                sOutTrans = alt_tag_start + "TRD" + alt_tag_end
            Case "FOR SALE/TRADE"
                sOutTrans = alt_tag_start + "FS/TRD" + alt_tag_end
            Case "SALE/TRADE"
                sOutTrans = alt_tag_start + "FS/TRD" + alt_tag_end
            Case "SALE/SHARE"
                sOutTrans = alt_tag_start + "FS/SH" + alt_tag_end
            Case "FOR SALE/SHARE"
                sOutTrans = alt_tag_start + "FS/SH" + alt_tag_end
            Case "NO ENGINES"
                sOutTrans = alt_tag_start + "NO/ENG" + alt_tag_end
            Case "AUCTION"
                sOutTrans = alt_tag_start + "AUC" + alt_tag_end
            Case "UNCONFIRMED"
                sOutTrans = alt_tag_start + "UNC" + alt_tag_end
            Case "CONFIDENTIAL"
                sOutTrans = alt_tag_start + "CONF" + alt_tag_end
            Case "SALE PENDING"
                sOutTrans = alt_tag_start + "SP" + alt_tag_end
            Case "FOR SALE"
                sOutTrans = ""
            Case Else
                sOutTrans = alt_tag_start + in_StrToTranslate.Trim.ToUpper + alt_tag_end

        End Select

        Return sOutTrans

    End Function

    Public Function get_standard_ac_features(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT amfeat_feature_code, kfeat_name, amfeat_seq_no FROM Aircraft_Model_Key_Feature WITH(NOLOCK), Key_Feature WITH(NOLOCK) WHERE amfeat_standard_equip = 'Y' AND")
            sQuery.Append(" ((amfeat_stdeq_start_ser_no_value IS NULL AND amfeat_stdeq_end_ser_no_value IS NULL) OR")
            sQuery.Append(" (amfeat_stdeq_start_ser_no_value = 0 AND amfeat_stdeq_end_ser_no_value = 0)) AND")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" amfeat_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then ' *** might have to generate in clause of models for this make
                sQuery.Append(" amfeat_amod_id = " + searchCriteria.ViewCriteriaMakeAmodID.ToString)
            End If

            sQuery.Append(" AND amfeat_feature_code = kfeat_code ORDER BY amfeat_seq_no, amfeat_feature_code")

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
                aError = "Error in get_standard_ac_features load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_standard_ac_features(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub load_standard_ac_features(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inStdFeatCodes(,) As String)

        Dim results_table As New DataTable
        Dim nCounter As Integer = 0

        Try

            results_table = get_standard_ac_features(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    ReDim inStdFeatCodes(results_table.Rows.Count - 1, 1)

                    For Each r As DataRow In results_table.Rows
                        inStdFeatCodes(nCounter, 0) = r.Item("amfeat_feature_code").ToString.Trim.ToUpper
                        inStdFeatCodes(nCounter, 1) = r.Item("kfeat_name").ToString.Trim
                        nCounter += 1
                    Next

                Else
                    ReDim inStdFeatCodes(0, 0)
                    inStdFeatCodes(0, 0) = ""
                End If

            Else
                ReDim inStdFeatCodes(0, 0)
                inStdFeatCodes(0, 0) = ""
            End If

        Catch ex As Exception

            aError = "Error in GetStandardAircraftFeatures(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inStdFeatCodes As Object) " + ex.Message

        Finally

        End Try

        results_table = Nothing

    End Sub

    Public Function get_nonstandard_ac_features(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal inStdFeatCodes(,) As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT amfeat_feature_code, kfeat_name FROM Aircraft_Model_Key_Feature WITH(NOLOCK), Key_Feature WITH(NOLOCK)")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" WHERE amfeat_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " AND amfeat_feature_code = kfeat_code")
            ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then ' *** might have to generate in clause of models for this make
                sQuery.Append(" WHERE amfeat_amod_id = " + searchCriteria.ViewCriteriaMakeAmodID.ToString + " AND amfeat_feature_code = kfeat_code")
            End If

            If Not IsNothing(inStdFeatCodes) And IsArray(inStdFeatCodes) Then
                If inStdFeatCodes(0, 0) <> "" Then

                    sQuery.Append(" AND amfeat_feature_code NOT IN(")
                    For x As Integer = 0 To UBound(inStdFeatCodes)
                        If x = 0 Then
                            sQuery.Append("'" + inStdFeatCodes(x, 0) + "'")
                        Else
                            sQuery.Append(",'" + inStdFeatCodes(x, 0) + "'")
                        End If
                    Next
                    sQuery.Append(") ORDER BY amfeat_seq_no")

                Else
                    sQuery.Append(" ORDER BY amfeat_seq_no")
                End If
            Else
                sQuery.Append(" ORDER BY amfeat_seq_no")
            End If

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
                aError = "Error in get_nonstandard_ac_features load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_nonstandard_ac_features(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inStdFeatCodes As Object) As DataTable " + ex.Message

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

    Public Sub display_nonstandard_feature_code_headings(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String, ByRef inStdFeatCodes(,) As String, ByVal cellWidth As Integer, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim nCounter As Integer = 0

        Try

            results_table = get_nonstandard_ac_features(searchCriteria, inStdFeatCodes)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    ReDim inFeatCodes(results_table.Rows.Count - 1)

                    For Each r As DataRow In results_table.Rows

                        ' If HttpContext.Current.Session.Item("localPreferences").HasLocalNotes And _
                        '  Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").NotesDatabaseName) And _
                        '  searchCriteria.ViewCriteriaNoLocalNotes = False Then
                        '       htmlOut.Append("<td class='featuresCellNoBorder' style='padding-left:3px; text-align:center; vertical-align: middle; width:" + cellWidth.ToString + "px;' title='" + r.Item("kfeat_name").ToString.Trim + "'><strong>&nbsp;" + r.Item("amfeat_feature_code").ToString.Trim.ToUpper + "&nbsp;<br/>&nbsp;</strong></td>")
                        ' Else
                        htmlOut.Append("<th title='" + r.Item("kfeat_name").ToString.Trim + "'>" + r.Item("amfeat_feature_code").ToString.Trim.ToUpper + "</th>")
                        ' End If

                        inFeatCodes(nCounter) = r.Item("amfeat_feature_code").ToString.Trim.ToUpper
                        nCounter += 1

                    Next

                Else
                    ReDim inFeatCodes(0)
                    inFeatCodes(0) = ""
                End If

            Else
                ReDim inFeatCodes(0)
                inFeatCodes(0) = ""
            End If

        Catch ex As Exception

            aError = "Error in display_nonstandard_feature_code_headings(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String, ByRef inStdFeatCodes(,) As String, ByVal cellWidth As Integer, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_owner_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT TOP 1 * FROM Company WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Aircraft_Reference WITH(NOLOCK) ON (comp_id = cref_comp_id AND comp_journ_id = cref_journ_id)")
            sQuery.Append(" LEFT OUTER JOIN Contact WITH(NOLOCK) ON (cref_contact_id = contact_id AND cref_journ_id = contact_journ_id)")
            sQuery.Append(" WHERE (cref_ac_id = " + searchCriteria.ViewCriteriaAircraftID.ToString + " AND cref_journ_id = " + searchCriteria.ViewCriteriaJournalID.ToString)

            If searchCriteria.ViewCriteriaGetExclusive Then
                sQuery.Append(Constants.cAndClause + "((cref_contact_type = '99') OR (cref_contact_type = '93') OR (cref_transmit_seq_no = 4))")
            ElseIf searchCriteria.ViewCriteriaGetOperator Then
                sQuery.Append(Constants.cAndClause + "(cref_operator_flag  in ('Y', 'O'))")
            Else
                sQuery.Append(Constants.cAndClause + "cref_transmit_seq_no = 1 AND cref_contact_type <> '71'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
            End If

            If searchCriteria.ViewCriteriaJournalID = 0 Then
                sQuery.Append(Constants.cAndClause + "comp_active_flag = 'Y'")
            End If

            sQuery.Append(Constants.cAndClause + "comp_hide_flag = 'N')")
            sQuery.Append(" " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))

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
                aError = "Error in get_owner_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_owner_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_client_ac_features(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConnection As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("SELECT cliafeat_flag, cliafeat_type as clikfeat_type ")
            sQuery.Append(" FROM client_aircraft_key_features ")
            'sQuery.Append(" inner join client_key_features on clikfeat_type = cliafeat_type ")
            '  sQuery.Append(" WHERE kfeat_inactive_date IS NULL ")
            sQuery.Append(" WHERE cliafeat_seq_nbr > 0  ") ' just to have something in the where 

            If Not IsNothing(inFeatCodes) And IsArray(inFeatCodes) Then

                If inFeatCodes(0) <> "" Then

                    sQuery.Append(Constants.cAndClause + "cliafeat_type IN(")

                    For x As Integer = 0 To UBound(inFeatCodes)
                        If x = 0 Then
                            sQuery.Append("'" + inFeatCodes(x) + "'")
                        Else
                            sQuery.Append(",'" + inFeatCodes(x) + "'")
                        End If
                    Next

                    sQuery.Append(")")

                End If

            End If

            sQuery.Append(Constants.cAndClause + "cliafeat_cliac_id = " + searchCriteria.ViewCriteriaAircraftID.ToString + "   ORDER BY cliafeat_seq_nbr")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            MySqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
            MySqlConnection.Open()

            MySqlCommand.Connection = MySqlConnection
            MySqlCommand.CommandTimeout = 1000
            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()


            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_ac_features load datatable " + constrExc.Message
            End Try


        Catch ex As Exception
            MySqlConnection.Dispose()
            MySqlCommand.Dispose()

            Return Nothing

        Finally

            MySqlCommand.Dispose()
            MySqlConnection.Close()
            MySqlConnection.Dispose()

        End Try

        MySqlReader = Nothing
        MySqlCommand = Nothing
        MySqlConnection = Nothing


        Return atemptable

    End Function

    Public Function check_client_model_transactions(ByVal amod_id As Long, ByVal start_date As String, ByVal end_date As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConnection As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append(" select * from client_transactions ")
            sQuery.Append("  inner join client_aircraft_model on cliamod_id = clitrans_cliamod_id  ")
            sQuery.Append("  left outer join client_aircraft on cliaircraft_cliamod_id = cliamod_id and CLITRANS_jetnet_ac_id = cliaircraft_jetnet_ac_id ")
            sQuery.Append("  where cliamod_jetnet_amod_id = " & amod_id & "  ")
            sQuery.Append(" and (clitrans_asking_price > 0 or clitrans_sold_price > 0) ")

            If Trim(start_date) <> "" Then
                If IsDate(start_date) = True Then
                    sQuery.Append("  and clitrans_date >= '" & Year(start_date) & "-" & Month(start_date) & "-" & Day(start_date) & "' ")
                End If
            End If

            If Trim(end_date) <> "" Then
                If IsDate(end_date) = True Then
                    sQuery.Append("  and clitrans_date <= '" & Year(end_date) & "-" & Month(end_date) & "-" & Day(end_date) & "' ")
                End If
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            MySqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
            MySqlConnection.Open()

            MySqlCommand.Connection = MySqlConnection
            MySqlCommand.CommandTimeout = 1000
            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()


            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_ac_features load datatable " + constrExc.Message
            End Try


        Catch ex As Exception
            MySqlConnection.Dispose()
            MySqlCommand.Dispose()

            Return Nothing

        Finally

            MySqlCommand.Dispose()
            MySqlConnection.Close()
            MySqlConnection.Dispose()

        End Try

        MySqlReader = Nothing
        MySqlCommand = Nothing
        MySqlConnection = Nothing


        Return atemptable

    End Function

    Public Function check_client_model_current_market_all(ByVal amod_id As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConnection As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append(" select * from client_aircraft  ")
            sQuery.Append(" inner Join client_aircraft_model on cliaircraft_cliamod_id = cliamod_id  ")
            sQuery.Append("  where cliamod_jetnet_amod_id = " & amod_id & "  ")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            MySqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
            MySqlConnection.Open()

            MySqlCommand.Connection = MySqlConnection
            MySqlCommand.CommandTimeout = 1000
            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()


            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_ac_features load datatable " + constrExc.Message
            End Try


        Catch ex As Exception
            MySqlConnection.Dispose()
            MySqlCommand.Dispose()

            Return Nothing

        Finally

            MySqlCommand.Dispose()
            MySqlConnection.Close()
            MySqlConnection.Dispose()

        End Try

        MySqlReader = Nothing
        MySqlCommand = Nothing
        MySqlConnection = Nothing


        Return atemptable

    End Function
    Public Function get_ac_features(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT afeat_status_flag, afeat_feature_code as kfeat_code FROM Aircraft_Key_Feature WITH(NOLOCK) ")
            sQuery.Append(" WHERE ")

            If Not IsNothing(inFeatCodes) And IsArray(inFeatCodes) Then

                If inFeatCodes(0) <> "" Then

                    sQuery.Append("afeat_feature_code IN(")

                    For x As Integer = 0 To UBound(inFeatCodes)
                        If x = 0 Then
                            sQuery.Append("'" + inFeatCodes(x) + "'")
                        Else
                            sQuery.Append(",'" + inFeatCodes(x) + "'")
                        End If
                    Next

                    sQuery.Append(")")

                End If

            End If

            sQuery.Append(Constants.cAndClause + "afeat_ac_id = " + searchCriteria.ViewCriteriaAircraftID.ToString + " AND afeat_journ_id = " + searchCriteria.ViewCriteriaJournalID.ToString + " ORDER BY afeat_seq_no")

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
                aError = "Error in get_ac_features load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_ac_features(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String) As DataTable " + ex.Message

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

    Public Sub display_ac_feature_codes(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String, ByVal cellWidth As Integer, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim found_spot As Boolean = False

        Try

            'htmlOut.Append("<table id='featureDataTable' cellpadding='2' cellspacing='0' border='0'><tr>")

            If Not IsNothing(inFeatCodes) And IsArray(inFeatCodes) Then

                results_table = get_ac_features(searchCriteria, inFeatCodes)

                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        ' go thro each feature code 
                        For i = 0 To inFeatCodes.Count - 1

                            found_spot = False

                            For Each r As DataRow In results_table.Rows

                                ' added MSW, go through all of the feature codes, find the one it is, and add it under there
                                If Trim(inFeatCodes(i)) = Trim(r("kfeat_code")) Then
                                    If Not IsDBNull(r("afeat_status_flag")) Then
                                        If Not String.IsNullOrEmpty(r.Item("afeat_status_flag").ToString) Then
                                            found_spot = True
                                            htmlOut.Append("<td  style='text-align:center; vertical-align: middle; width:" + cellWidth.ToString + "px;' title='" + r.Item("kfeat_code").ToString.Trim.ToUpper + "'>")
                                            htmlOut.Append("" + r.Item("afeat_status_flag").ToString.Trim.ToUpper + "</td>")
                                            'htmlOut.Append("<table cellpadding='0' cellspacing='0' align='center' height='15'><tr><td align='center' height='15'><strong>&nbsp;" + r.Item("afeat_status_flag").ToString.Trim.ToUpper + "&nbsp;</strong></td></tr></table></td>")
                                        Else
                                            found_spot = True
                                            htmlOut.Append("<td style='text-align:center; vertical-align: middle; width:" + cellWidth.ToString + "px;'></td>")
                                            'htmlOut.Append("<table cellpadding='0' cellspacing='0' align='center'><tr><td align='center'><strong>&nbsp;U&nbsp;</strong></td></tr></table></td>")
                                        End If
                                    End If
                                Else

                                End If

                            Next

                            If found_spot = False Then
                                htmlOut.Append("<td style='text-align:center; vertical-align: middle; width:" + cellWidth.ToString + "px;'>U</td>")
                            End If

                        Next

                    Else
                        htmlOut.Append("<td style='text-align:center; vertical-align: middle;'>No features available for this Make / Model ...</td>")
                    End If

                Else
                    htmlOut.Append("<td style='text-align:center; vertical-align: middle;'>No features available for this Make / Model ...</td>")
                End If

            End If

            ' htmlOut.Append("</tr></table>")

        Catch ex As Exception

            aError = "Error in display_ac_feature_codes(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String, ByVal cellWidth As Integer, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Function VerifyClientFeaturesTable(ByRef TableToVerify As DataTable, ByRef inFeatCodes() As String) As DataTable
        Dim ReturnTable As New DataTable
        ReturnTable = TableToVerify.Copy 'Since this is what's being returned, it needs to be copied no matter if we're editing it or not.

        If TableToVerify.Rows.Count <> inFeatCodes.Length Then
            'Let's copy the reference table to get the structure and the data.

            'First check row count against array.
            'We have verified the client table doesn't have as many feature codes and we need to pad the table to match the array.
            'Loop through the array.
            For x As Integer = 0 To UBound(inFeatCodes)
                'For each feature code in the array, we need to go ahead and search the client feature table to see if it already exists.
                'If it does, we do nothing.
                'If it doesn't, we need to add a blank entry.
                Dim afiltered As DataRow() = ReturnTable.Select("clikfeat_type = '" & inFeatCodes(x).ToString & "'", "")

                If afiltered.Length = 0 Then 'we need to add this one
                    Dim newFeatureRow As DataRow = ReturnTable.NewRow()
                    newFeatureRow("cliafeat_flag") = "U"
                    newFeatureRow("clikfeat_type") = inFeatCodes(x).ToString
                    'Adding the feature in.
                    ReturnTable.Rows.Add(newFeatureRow)
                    ReturnTable.AcceptChanges()
                Else 'It already exists, carry on.
                End If
            Next
        End If
        Return ReturnTable
    End Function

    Public Sub display_client_ac_feature_codes(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String, ByVal cellWidth As Integer, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable

        Try

            'htmlOut.Append("<table id='featureDataTable' cellpadding='2' cellspacing='0' border='0'><tr>")

            If Not IsNothing(inFeatCodes) And IsArray(inFeatCodes) Then

                results_table = get_client_ac_features(searchCriteria, inFeatCodes)

                results_table = VerifyClientFeaturesTable(results_table, inFeatCodes)

                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        For Each r As DataRow In results_table.Rows

                            If Not IsDBNull(r("clikfeat_type")) Then
                                If Not String.IsNullOrEmpty(r.Item("clikfeat_type").ToString) Then
                                    htmlOut.Append("<td  class='forSaleCellBorder' style='text-align:center; vertical-align: middle; width:" + cellWidth.ToString + "px;' title='" + r.Item("clikfeat_type").ToString.Trim.ToUpper + "'>")
                                    'htmlOut.Append("<table cellpadding='0' cellspacing='0' align='center' height='15'><tr><td align='center' height='15'><strong>&nbsp;" + r.Item("cliafeat_flag").ToString.Trim.ToUpper + "&nbsp;</strong></td></tr></table></td>")
                                    htmlOut.Append("" + r.Item("cliafeat_flag").ToString.Trim.ToUpper + "</td>")

                                Else
                                    htmlOut.Append("<td  class='forSaleCellBorder' style='text-align:center; vertical-align: middle; width:" + cellWidth.ToString + "px;'>")
                                    htmlOut.Append("</td>")
                                    'htmlOut.Append("<table cellpadding='0' cellspacing='0' align='center'><tr><td align='center'><strong>&nbsp;U&nbsp;</strong></td></tr></table></td>")
                                End If
                            End If

                        Next

                    Else
                        htmlOut.Append("<td style='text-align:center; vertical-align: middle;'>No features available for this Make / Model ...</td>")
                    End If

                Else
                    htmlOut.Append("<td style='text-align:center; vertical-align: middle;'>No features available for this Make / Model ...</td>")
                End If

            End If

            ' htmlOut.Append("</tr></table>")

        Catch ex As Exception

            aError = "Error in display_ac_feature_codes(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef inFeatCodes() As String, ByVal cellWidth As Integer, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub display_standard_model_features(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal is_use_link As Boolean = True, Optional ByVal DisplayAsTable As Boolean = True)

        Dim arrStdFeatCodes(,) As String = Nothing
        Dim htmlOut As New StringBuilder

        load_standard_ac_features(searchCriteria, arrStdFeatCodes)

        If DisplayAsTable Then
            If is_use_link Then
                htmlOut.Append("<table id='stdModelFeaturesOuterTable' width='100%' cellpadding='0' cellspacing='0' class='module'>")
                htmlOut.Append("<tr><td align='center' valign='middle' class='header'>STANDARD FEATURES</td></tr>")
                htmlOut.Append("<tr><td align='center' valign='top'>")
            Else
                htmlOut.Append("<table id='stdModelFeaturesOuterTable' width='100%' cellpadding='0' cellspacing='0' class='module'>")
                htmlOut.Append("<tr><td align='center' valign='middle' class='header'><font size='1'>STANDARD FEATURES</font></td></tr>")
                htmlOut.Append("<tr><td align='center' valign='top'>")
            End If
        Else
            htmlOut.Append("<strong>STANDARD EQUIPMENT:</strong> ")
        End If

        If Not String.IsNullOrEmpty(arrStdFeatCodes(0, 0).Trim) Then

            If DisplayAsTable Then
                htmlOut.Append("<table id='stdModelFeaturesDataTable' width='100%' cellspacing='0' cellpadding='4' class='leftside_right'>")
            End If

            For I As Integer = 0 To UBound(arrStdFeatCodes)
                If DisplayAsTable Then
                    If is_use_link Then
                        htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'><strong>" + arrStdFeatCodes(I, 0).Trim + "</strong></td><td align='left' valign='middle' class='seperator'>" + arrStdFeatCodes(I, 1).Trim + "</td></tr>")
                    Else
                        htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'><strong><font size='1'>" + arrStdFeatCodes(I, 0).Trim + "</font></strong></td><td align='left' valign='middle' class='seperator'><font size='1'>" + arrStdFeatCodes(I, 1).Trim + "</font></td></tr>")
                    End If
                Else
                    If I > 0 Then
                        htmlOut.Append(", ")
                    End If
                    htmlOut.Append(arrStdFeatCodes(I, 1).Trim) '& ", ")
                    'htmlOut.Append(arrStdFeatCodes(I, 1).Trim)
                End If
            Next

            If DisplayAsTable Then
                htmlOut.Append("</table>")
            End If

        Else
            If DisplayAsTable Then
                If is_use_link Then
                    htmlOut.Append("<table id='stdModelFeaturesDataTable' width='100%' cellspacing='0' cellpadding='2' class='leftside_right'>")
                    htmlOut.Append("<tr><td align='left' valign='top'><strong>No Standard Features for this model</strong></td></tr>")
                    htmlOut.Append("</table>")
                Else
                    htmlOut.Append("<table id='stdModelFeaturesDataTable' width='100%' cellspacing='0' cellpadding='2' class='leftside_right'>")
                    htmlOut.Append("<tr><td align='left' valign='top'><strong><font size='1'>No Standard Features for this model</font></strong></td></tr>")
                    htmlOut.Append("</table>")
                End If
            Else
                htmlOut.Append("No Standard Features for this model")
            End If

        End If

        htmlOut.Append("</td></tr></table>")

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing

    End Sub

#End Region

#Region "spi_functions"

    Public Function views_spi_return_yearQuarter_name(ByVal strYear As Integer, ByVal strQuarter As Integer) As String

        Dim strResults

        strResults = ""

        Select Case strQuarter
            Case 1
                strResults = "1st Quarter (Jan-Feb-Mar), " + strYear.ToString
            Case 2
                strResults = "2nd Quarter (Apr-May-Jun), " + strYear.ToString
            Case 3
                strResults = "3rd Quarter (Jul-Aug-Sep), " + strYear.ToString
            Case 4
                strResults = "4th Quarter (Oct-Nov-Dec), " + strYear.ToString
        End Select

        Return strResults

    End Function

    Public Sub views_spi_sort_labels_value(ByRef labels() As String, ByRef data As String(), ByVal count As Integer, ByVal direction As Integer) ' 1 direction is label is used for sort 2 is that data is 

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim temp_string As String = ""
        Dim temp_int As Integer = 0
        Dim temp_int_array(count) As Double


        For i = 0 To count
            If direction = 1 Then
                temp_int_array(i) = CDbl(labels(i))
            Else
                temp_int_array(i) = data(i)
            End If
        Next


        For j = 0 To count - 1
            For i = 0 To count - 1
                If temp_int_array(i) > temp_int_array(i + 1) Then

                    temp_int = data(i + 1)
                    data(i + 1) = data(i)
                    data(i) = temp_int

                    temp_string = labels(i + 1)
                    labels(i + 1) = labels(i)
                    labels(i) = temp_string

                    temp_int = temp_int_array(i + 1)
                    temp_int_array(i + 1) = temp_int_array(i)
                    temp_int_array(i) = temp_int

                End If
            Next
        Next

        For j = 0 To count - 1
            If labels(j) = Nothing Then
                labels(j) = ""
            End If
        Next


    End Sub


    Public Function views_spi_return_previous_full_quarterly_by_weightclass(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef CHART_TO_GRAPH As System.Web.UI.DataVisualization.Charting.Chart)

        Dim htmlOut As New StringBuilder

        Dim color As String = "Blue"
        Dim strQuery1 As String = ""

        Dim strYearSld As String = ""
        Dim strQuarterSld As String = ""
        Dim strYearQtrName As String = ""

        Dim strMakeAbbrev As String = ""
        Dim lAModID As Integer = 0
        Dim strAModId As String = ""
        Dim strWeightClass As String = ""
        Dim strWeightClassName As String = ""

        Dim strAvgYearMfr As String = ""
        Dim strAvgYearDlv As String = ""
        Dim strAvgAsking As String = ""
        Dim strAvgSelling As String = ""
        Dim strPercent As String = ""
        Dim strVariance As String = ""
        Dim strAvgAFTT As String = ""
        Dim strAvgDOM As String = ""

        Dim dAvgYearMfr As Double = 0
        Dim dAvgYearDlv As Double = 0
        Dim dAvgAsking As Double = 0
        Dim dAvgSelling As Double = 0
        Dim dPercent As Double = 0
        Dim dVariance As Double = 0
        Dim dAvgAFTT As Double = 0
        Dim dAvgDOM As Double = 0

        Dim lRec1 As String = ""
        Dim strHRef As String = ""

        Dim lColSpan As Integer = 0
        Dim lTotRec As Double = 0

        Dim lGraphType As Integer = 0
        Dim strGraphImage As String = ""

        ' Percentage Of Asking Price     
        Dim strTitle1 As String = ""
        Dim strBottomTitle1 As String = ""
        Dim strLeftTitle1 As String = ""
        Dim lCnt1 As Integer = 0
        Dim aData1()
        Dim aLabels1()

        ' Variance Of Asking Price     
        Dim strTitle2 As String = ""
        Dim strBottomTitle2 As String = ""
        Dim strLeftTitle2 As String = ""
        Dim lCnt2 As Integer = 0
        Dim aData2()
        Dim aLabels2()

        Dim strModel As String = ""
        Dim strMake As String = ""
        Dim lYearSld As Integer = 0
        Dim lQuarterSld As Integer = 0

        Dim tmpGraph As String = ""
        Dim strGraphPercentAsking As String = ""
        Dim strGraphVarianceAsking As String = ""
        Dim strHTMLData2 As String = ""

        Dim SqlConn As New SqlClient.SqlConnection
        Dim sqlcommand_final As New SqlClient.SqlCommand
        Dim adors_final As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable

        Dim cssClass As String = ""

        Try


            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            sqlcommand_final.Connection = SqlConn
            sqlcommand_final.CommandType = CommandType.Text
            sqlcommand_final.CommandTimeout = 60
            strGraphPercentAsking = "Percentage of Asking Price (%)<br />Not Enough Data Available"
            strGraphVarianceAsking = "Variance of Asking Price (%)<br />Not Enough Data Available"

            ' Clear All Variables Passed By Reference

            strHTMLData2 = ""

            strQuery1 = "SELECT amod_id As AModId, amod_make_name As Make, amod_make_abbrev As MakeAbbrev, amod_model_name As Model,"
            strQuery1 += " AVG(CAST(ac_mfr_year AS INT)) As dAvgYearMfr,"
            strQuery1 += " AVG(CAST(ac_year AS INT)) As dAvgYearDlv,"
            strQuery1 += " AVG(ac_asking_price) As dAvgAsking,"
            strQuery1 += " AVG(ac_hidden_asking_price) As dAvgAskingHidden,"

            strQuery1 += "  AVG(ac_sale_price) As dAvgSelling, "


            strQuery1 += " ((AVG(ac_sale_price)/AVG(ac_asking_price)) * 100) As dPercent,"
            strQuery1 += " ((1-(AVG(ac_sale_price)/AVG(ac_asking_price))) * 100) As dVariance,"
            strQuery1 += " ((AVG(ac_sale_price)/AVG(ac_hidden_asking_price)) * 100) As dPercentHidden,"
            strQuery1 += " ((1-(AVG(ac_sale_price)/AVG(ac_hidden_asking_price))) * 100) As dVarianceHidden,"
            strQuery1 += " AVG(ac_airframe_tot_hrs) As dAvgAFTT,"
            strQuery1 += " AVG(DateDiff(day,ac_list_date, journ_date)) As dAvgDOM"

            strQuery1 += " FROM Aircraft_Summary_SPI WITH (NOLOCK)"

            strQuery1 += " WHERE (ac_journ_id > 0)"
            'strQuery1 += " AND (ac_lifecycle_stage = 3)"                   '-- In Operation Only
            'strQuery1 += " AND (jcat_used_retail_sales_flag = 'Y')"        '-- Retail Only    
            'strQuery1 += " AND (journ_newac_flag = 'N')"                   '-- Used Sales Only
            'strQuery1 += " AND (journ_subcategory_code LIKE 'WS%')"        '-- Whole Sales Only
            'strQuery1 += " AND (journ_subcategory_code NOT LIKE '%IT%')"   '-- No Internals
            'strQuery1 += " AND (journ_internal_trans_flag = 'N')"          '-- No Internals

            ' added MSW - 5/17/2016
            strQuery1 += "  AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) "
            strQuery1 += "  AND (journ_subcat_code_part1 = 'WS')"       '-- Whole Sales Only 
            strQuery1 += "  AND (journ_internal_trans_flag = 'N')"            '-- No Internals 

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                strQuery1 += (Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                strQuery1 += (Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                strQuery1 += (Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                strQuery1 += (Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If

            If CLng(searchCriteria.ViewCriteriaAmodID) > 0 Then
                strQuery1 += " AND (amod_id <> " + searchCriteria.ViewCriteriaAmodID.ToString + ")"
            End If

            Select Case CLng(searchCriteria.viewCriteriaSPIAirframe)
                Case Is = 1
                    strQuery1 += " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')"
                Case Is = 2
                    strQuery1 += " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')"
                Case Is = 3
                    strQuery1 += " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')"
                Case Is = 4
                    strQuery1 += " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')"
                Case Is = 5
                    strQuery1 += " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))"
            End Select

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaSPIWeightClass) Then

                If InStr(1, searchCriteria.ViewCriteriaSPIWeightClass, ",") = 0 Then
                    strQuery1 += " AND (amod_weight_class = '" + searchCriteria.ViewCriteriaSPIWeightClass + "')"
                Else
                    strQuery1 += " AND (amod_weight_class IN ('" + Replace(searchCriteria.ViewCriteriaSPIWeightClass, ",", "','") + "'))"
                End If

            End If

            strQuery1 += " AND (ac_sale_price IS NOT NULL) AND (ac_sale_price <> 0)"
            strQuery1 += " AND (ac_asking_price IS NOT NULL) AND (ac_asking_price <> 0)"

            strQuery1 += " AND (DATEPART(year,journ_date) >= " + searchCriteria.ViewCriteriaSPIYearSld2 + ")"
            strQuery1 += " AND (DATEPART(quarter,journ_date) = 1)"

            strQuery1 += " GROUP BY amod_id, amod_make_name, amod_make_abbrev, amod_model_name"
            strQuery1 += " ORDER BY amod_make_name, amod_model_name asc"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, strQuery1.ToString)

            sqlcommand_final.CommandText = strQuery1.Trim
            adors_final = sqlcommand_final.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(adors_final)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_manufacturer_companies_info load datatable " + constrExc.Message
            End Try


            strHTMLData2 = "<table id='weightClassDataTable' cellpadding='2' cellspacing='0' border='1' width='100%' class=""salesPriceTable"">"
            strHTMLData2 += "<tr><td align='center' colspan='9' class='headerLine'>"
            strHTMLData2 += "Weight Class Similar To " & searchCriteria.ViewCriteriaAircraftMake.ToString & "&nbsp;/&nbsp;" & searchCriteria.ViewCriteriaAircraftModel.ToString & "&nbsp;(" & searchCriteria.ViewCriteriaSPIWeightClass.ToString & ")&nbsp;ASKING/SALE PRICE SUMMARY<br />"

            '  strHTMLData2 = strHTMLData2 & "Year/Quarter Sold " & HttpContext.Current.Session.Item("SPYearSld2") & " - " & AIRCRAFT_SUMMARY_ReturnYearQuarterName_PDF(HttpContext.Current.Session.Item("SPYearSld2"), 1)

            strHTMLData2 += "</td></tr>"

            strHTMLData2 += "<tr><td align='center' rowspan='2' class=""fieldHeader"">Make<br />Model</td>"
            strHTMLData2 += "<td align='center' colspan='2'  class=""SupfieldHeader"">Avg Year Of</td>"
            strHTMLData2 += "<td align='center' colspan='2'  class=""SupfieldHeader"">Avg Price ($k)</td>"
            strHTMLData2 += "<td align='center' rowspan='2' class=""fieldHeader"">Percent</td>"
            strHTMLData2 += "<td align='center' rowspan='2' class=""fieldHeader"">Variance</td>"
            strHTMLData2 += "<td align='center' colspan='2'  class=""SupfieldHeader"">Average</td></tr>"

            strHTMLData2 += "<tr><td align='center' class=""fieldHeader"">Mfr</td>"
            strHTMLData2 += "<td align='center' class=""fieldHeader"">Delivery</td>"
            strHTMLData2 += "<td align='center' class=""fieldHeader"">Asking</td>"
            strHTMLData2 += "<td align='center' class=""fieldHeader"">Selling</td>"
            strHTMLData2 += "<td align='center' class=""fieldHeader"">AFTT</td>"
            strHTMLData2 += "<td align='center' class=""fieldHeader"">Days<br />On<br />Market</td></tr>"

            If atemptable.Rows.Count > 0 Then


                lTotRec = 1000

                lCnt1 = 0
                lCnt2 = 0

                ReDim aData1(lTotRec)      ' Percentage Of Asking Price
                ReDim aLabels1(lTotRec)

                ReDim aData2(lTotRec)      ' Variance Of Asking Price
                ReDim aLabels2(lTotRec)

                For Each r As DataRow In atemptable.Rows

                    strYearSld = ""
                    strQuarterSld = ""
                    strYearQtrName = ""

                    strModel = ""
                    strMake = ""
                    strMakeAbbrev = ""
                    strAModId = ""

                    strAvgYearMfr = ""
                    strAvgYearDlv = ""
                    strAvgAsking = ""
                    strAvgSelling = ""
                    strPercent = ""
                    strVariance = ""
                    strAvgAFTT = ""
                    strAvgDOM = ""

                    lAModID = 0
                    lYearSld = 0
                    lQuarterSld = 0
                    dAvgYearMfr = 0.0
                    dAvgYearDlv = 0.0
                    dAvgAsking = 0.0
                    dAvgSelling = 0.0
                    dPercent = 0.0
                    dVariance = 0.0
                    dAvgAFTT = 0.0
                    dAvgDOM = 0.0

                    strMake = Trim(r.Item("Make"))
                    strMakeAbbrev = "(" & Trim(r.Item("MakeAbbrev")) & ")"
                    strModel = Trim(r.Item("Model"))
                    lAModID = r.Item("AModId")
                    strAModId = CStr(lAModID)

                    If Not IsDBNull(r.Item("dAvgYearMfr")) Then
                        dAvgYearMfr = r.Item("dAvgYearMfr")
                    Else
                        dAvgYearMfr = 0
                    End If

                    If Not IsDBNull(r.Item("dAvgYearDlv")) Then
                        dAvgYearDlv = r.Item("dAvgYearDlv")
                    Else
                        dAvgYearDlv = 0
                    End If

                    If Not IsDBNull(r.Item("dAvgAsking")) Then
                        dAvgAsking = r.Item("dAvgAsking")
                    ElseIf Not IsDBNull(r.Item("dAvgAskingHidden")) Then
                        dAvgAsking = r.Item("dAvgAskingHidden")
                    Else
                        dAvgAsking = 0
                    End If

                    If Not IsDBNull(r.Item("dAvgSelling")) Then
                        dAvgSelling = r.Item("dAvgSelling")
                    Else
                        dAvgSelling = 0
                    End If

                    If Not IsDBNull(r.Item("dPercent")) Then
                        dPercent = r.Item("dPercent")
                    ElseIf Not IsDBNull(r.Item("dPercentHidden")) Then
                        dPercent = r.Item("dPercentHidden")
                    Else
                        dPercent = 0
                    End If

                    If Not IsDBNull(r.Item("dVariance")) Then
                        dVariance = r.Item("dVariance")
                    ElseIf Not IsDBNull(r.Item("dVarianceHidden")) Then
                        dVariance = r.Item("dVarianceHidden")
                    Else
                        dVariance = 0
                    End If

                    If Not IsDBNull(r.Item("dAvgAFTT")) Then
                        dAvgAFTT = r.Item("dAvgAFTT")
                    Else
                        dAvgAFTT = 0
                    End If

                    If Not IsDBNull(r.Item("dAvgDOM")) Then
                        dAvgDOM = r.Item("dAvgDOM")
                    Else
                        dAvgDOM = 0
                    End If

                    strHRef = strMakeAbbrev & " " & strModel
                    strHTMLData2 = strHTMLData2 & "<tr class=""" & cssClass & """><td align='left' nowrap='nowrap'>" & strHRef & "</td>"

                    If cssClass = "" Then
                        cssClass = "alt_row"
                    Else
                        cssClass = ""
                    End If
                    strHTMLData2 = strHTMLData2 & "<td align='center'>"
                    If dAvgYearMfr > 0 Then
                        strHTMLData2 = strHTMLData2 & CStr(dAvgYearMfr) & "</td>"
                    Else
                        strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
                    End If

                    strHTMLData2 = strHTMLData2 & "<td align='center'>"
                    If dAvgYearDlv > 0 Then
                        strHTMLData2 = strHTMLData2 & CStr(dAvgYearDlv) & "</td>"
                    Else
                        strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
                    End If

                    strHTMLData2 = strHTMLData2 & "<td align='right'>"
                    If dAvgAsking > 0 Then
                        strHTMLData2 = strHTMLData2 & "$" & FormatNumber(dAvgAsking / 1000, 0, True) & "</td>"
                    Else
                        strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
                    End If

                    strHTMLData2 = strHTMLData2 & "<td align='right'>"
                    If dAvgSelling > 0 Then
                        strHTMLData2 = strHTMLData2 & "$" & FormatNumber(dAvgSelling / 1000, 0, True) & "</td>"
                    Else
                        strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
                    End If

                    strHTMLData2 = strHTMLData2 & "<td align='right'>"
                    If dPercent > 0 Then
                        strHTMLData2 = strHTMLData2 & FormatNumber(dPercent, 1, True) & "%</td>"
                    Else
                        strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
                    End If

                    strHTMLData2 = strHTMLData2 & "<td align='right'>"
                    If dAvgAsking > 0 Then
                        strHTMLData2 = strHTMLData2 & FormatNumber(dVariance, 1, True) & "%</td>"
                    ElseIf dAvgAsking = dAvgSelling Then
                        strHTMLData2 = strHTMLData2 & "0.0%</td>"
                    Else
                        strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
                    End If

                    strHTMLData2 = strHTMLData2 & "<td align='right'>"
                    If dAvgAFTT > 0 Then
                        strHTMLData2 = strHTMLData2 & FormatNumber(dAvgAFTT, 0, True) & "</td>"
                    Else
                        strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
                    End If

                    strHTMLData2 = strHTMLData2 & "<td align='right'>"
                    If dAvgDOM > 0 Then
                        strHTMLData2 = strHTMLData2 & FormatNumber(dAvgDOM, 0, True) & "</td>"
                    Else
                        strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
                    End If

                    strHTMLData2 = strHTMLData2 & "</tr>"

                    ' Percentage Of Asking Price  
                    If dAvgAsking > 0 Then
                        lCnt1 = lCnt1 + 1
                        aLabels1(lCnt1 - 1) = strMakeAbbrev & " " & strModel
                        aData1(lCnt1 - 1) = CDbl(FormatNumber(dPercent, 1, True))
                    End If

                    ' Variance Of Asking Price  
                    If dAvgAsking > 0 Then
                        lCnt2 = lCnt2 + 1
                        aLabels2(lCnt2 - 1) = strMakeAbbrev & " " & strModel
                        aData2(lCnt2 - 1) = CDbl(FormatNumber(dVariance, 1, True))
                    End If


                Next

                'Graph(Types)
                '  1=2D-Pie,        2=3D Pie
                '  3=2D Bar,        4=3D Bar
                '  6=Line,          7=Line 
                '  8=Area,          9=Speckle
                ' 10=Circle Line,  13=3D Ribbon 
                ' 14=3D-Area,      15=Line 
                ' 16=Line,         17=+/- Bar


                htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")

                If lCnt1 > 1 Then  '  And Not HttpContext.Current.Session.Item("localMachine")

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250


                    strTitle1 = "Weight Class (" & HttpContext.Current.Session.Item("salesPriceViewWtClsName") & ") - Percentage of Asking Price (%)"
                    strBottomTitle1 = "Make/Model(s)"    ' Y      


                    '      lCnt1 = delete_bad_data_from_graphs(aLabels1, aData1, lCnt1)


                    ReDim Preserve aData1(lCnt1)
                    ReDim Preserve aLabels1(lCnt1)

                    AIRCRAFT_SUMMARY_SortLabelsValue(aLabels1, aData1, lCnt1 - 1, 2)

                    lGraphType = 4
                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle1, strBottomTitle1, strLeftTitle1, lGraphType, aLabels1, aData1, 0, "", "##.0", 0, 0, color, CHART_TO_GRAPH)

                    If tmpGraph.ToString.Length > 2 Then
                        CHART_TO_GRAPH.Titles.Clear()
                        CHART_TO_GRAPH.Titles.Add(strTitle1)
                        CHART_TO_GRAPH.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        CHART_TO_GRAPH.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_W.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        htmlOut.Append("<tr><td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_W.jpg'><img border='0' width='350' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_W.jpg'></td>")
                        CHART_TO_GRAPH.Series.Clear()
                    Else
                        htmlOut.Append("<tr><td align='center'>Percentage of Asking Price (%)<br />Not Enough Data Available</td>")
                    End If
                Else
                    htmlOut.Append("<tr><td align='center'>Percentage of Asking Price (%)<br />Not Enough Data Available</td>")
                End If ' If lCnt1 > 1 Then


                If lCnt2 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    strTitle2 = "Weight Class (" & HttpContext.Current.Session.Item("salesPriceViewWtClsName") & ") - Variance of Asking Price (%)"
                    strBottomTitle2 = "Make/Model(s)"    ' Y

                    ReDim Preserve aData2(lCnt2)
                    ReDim Preserve aLabels2(lCnt2)

                    lGraphType = 4
                    AIRCRAFT_SUMMARY_SortLabelsValue(aLabels2, aData2, lCnt2 - 1, 2)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle2, strBottomTitle2, strLeftTitle2, lGraphType, aLabels2, aData2, 0, "", "##.0", 0, 0, color, CHART_TO_GRAPH)
                    If tmpGraph.ToString.Length > 2 Then
                        CHART_TO_GRAPH.Titles.Clear()
                        CHART_TO_GRAPH.Titles.Add(strTitle2)
                        CHART_TO_GRAPH.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        CHART_TO_GRAPH.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_W_2.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        htmlOut.Append("<td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_W_2.jpg'><img  border='0' width='350'  src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_W_2.jpg'></td></tr>")
                        CHART_TO_GRAPH.Series.Clear()
                    Else
                        htmlOut.Append("<td align='center'>Variance of Asking Price (%)<br />Not Enough Data Available</td></tr>")
                    End If
                Else
                    htmlOut.Append("<td align='center'>Variance of Asking Price (%)<br />Not Enough Data Available</td></tr>")
                End If ' If lCnt2 > 1 Then


            Else

                strHTMLData2 += "<tr><td align='center' colspan='9'>No Records Found</td></tr>"

            End If

            strHTMLData2 += "</table>"

            If Not String.IsNullOrEmpty(htmlOut.ToString) Then
                htmlOut.Append("</table>")
            End If

            adors_final.Close()

            out_htmlString = strHTMLData2.Trim


        Catch ex As Exception
        Finally
            SqlConn.Close()
            SqlConn.Dispose()

        End Try

        Return htmlOut.ToString
        htmlOut = Nothing

    End Function

    Public Function views_spi_return_quarterly_by_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef GRAPH_TO_CHART As System.Web.UI.DataVisualization.Charting.Chart, Optional ByRef google_map_string As String = "")
        views_spi_return_quarterly_by_model = ""

        'views_spi_return_quarterly_by_model(ByVal lModelId, ByRef strHTMLData1, ByRef strGraphVarianceAsking, 
        'ByVal sub_info, ByVal real_make_model_name, ByVal sub_type, ByVal weight_class, 
        'ByVal weight_class_name, ByVal spi_year, ByVal spi_year2, ByVal airframe_type, ByVal color, '
        'ByVal Sqlcommand_final,ByVal adors_final, ByRef GRAPH_TO_CHART, ByRef string_for_spi_start)
        Dim SqlConn As New SqlClient.SqlConnection
        Dim sqlcommand_final As New SqlClient.SqlCommand
        Dim adors_final As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable


        Dim query As String = ""
        Dim strQuery1 As String = ""

        Dim strYearSld As String = ""
        Dim strQuarterSld As String = ""
        Dim strYearQtrName As String = ""
        Dim strAvgYearMfr As String = ""
        Dim strAvgYearDlv As String = ""
        Dim strAvgAsking As String = ""
        Dim strAvgAskingHidden As String = ""
        Dim strAvgSelling As String = ""
        Dim strPercent As String = ""
        Dim strPercentHidden As String = ""
        Dim strVariance As String = ""
        Dim strVarianceHidden As String = ""
        Dim strAvgAFTT As String = ""
        Dim strAvgDOM As String = ""

        Dim lYearSld As String = ""
        Dim lQuarterSld As String = ""
        Dim dAvgYearMfr As Double = 0
        Dim dAvgYearDlv As Double = 0
        Dim dAvgAsking As Double = 0
        Dim dAvgAskingHidden As Double = 0
        Dim dAvgSelling As Double = 0
        Dim dPercent As Double = 0
        Dim dPercentHidden As Double = 0
        Dim dVariance As Double = 0
        Dim dVarianceHidden As Double = 0
        Dim dAvgAFTT As Double = 0
        Dim dAvgDOM As Double = 0

        Dim strHRef1 As String = ""

        Dim lColSpan As String = ""
        Dim lTotRec As Double = 0

        Dim objGraph As String = ""
        Dim lGraphType As String = ""
        Dim strGraphImage As String = ""
        Dim lMaxSets As Double = 0
        Dim lThisSet As Double = 0


        Dim strHTMLData1_2 As String = ""
        Dim strHTMLData1_2_final As String = ""


        ' Percentage Of Asking Price     
        Dim strTitle1 As String = ""
        Dim strBottomTitle1 As String = ""
        Dim strLeftTitle1 As String = ""
        Dim lCnt1 As Integer = 0
        Dim aData1()
        Dim aLabels1()

        ' Variance Of Asking Price     
        Dim strTitle2 As String = ""
        Dim strBottomTitle2 As String = ""
        Dim strLeftTitle2 As String = ""
        Dim lCnt2 As Integer = 0
        Dim aData2()
        Dim aLabels2()

        ' Asking Price     
        Dim strTitle3 As String = ""
        Dim strBottomTitle3 As String = ""
        Dim strLeftTitle3 As String = ""
        Dim lCnt3 As Integer = 0
        Dim aData3()
        Dim aLabels3()

        ' Selling Price     
        Dim strTitle4 As String = ""
        Dim strBottomTitle4 As String = ""
        Dim strLeftTitle4 As String = ""
        Dim lCnt4 As Integer = 0
        Dim aData4()
        Dim aLabels4()

        ' Asking vs Selling Price     
        Dim strTitle5 As String = ""
        Dim strBottomTitle5 As String = ""
        Dim strLeftTitle5 As String = ""
        Dim strRightTitle5 As String = ""
        Dim lCnt5a As Integer = 0
        Dim lCnt5b As Integer = 0
        Dim aData5a()  ' Asking
        Dim aData5b()  ' Selling
        Dim aLabels5a()
        Dim aLabels5b()

        ' AFTT (Airframe Total Time)
        Dim strTitle6 As String = ""
        Dim strBottomTitle6 As String = ""
        Dim strLeftTitle6 As String = ""
        Dim lCnt6 As Integer = 0
        Dim aData6()
        Dim aLabels6()

        ' DOM (Days On Market)
        Dim strTitle7 As String = ""
        Dim strBottomTitle7 As String = ""
        Dim strLeftTitle7 As String = ""
        Dim lCnt7 As Integer = 0
        Dim aData7()
        Dim aLabels7()

        ' AFTT vs Variance
        Dim strTitle8 As String = ""
        Dim strBottomTitle8 As String = ""
        Dim strLeftTitle8 As String = ""
        Dim lCnt8 As Integer = 0
        Dim aData8()
        Dim aLabels8()

        ' DOM vs Variance
        Dim strTitle9 As String = ""
        Dim strBottomTitle9 As String = ""
        Dim strLeftTitle9 As String = ""
        Dim lCnt9 As Integer = 0
        Dim aData9()
        Dim aLabels9()

        ' AFTT vs Selling Price
        Dim strTitle10 As String = ""
        Dim strBottomTitle10 As String = ""
        Dim strLeftTitle10 As String = ""
        Dim lCnt10 As Integer = 0
        Dim aData10()
        Dim aLabels10()

        ' DOM vs Selling Price
        Dim strTitle11 As String = ""
        Dim strBottomTitle11 As String = ""
        Dim strLeftTitle11 As String = ""
        Dim lCnt11 As Integer = 0
        Dim aData11()
        Dim aLabels11()

        Dim tmpGraph As String = ""
        Dim cHyphen As String = "-"

        Dim min As Integer = 0
        Dim max As Integer = 0
        Dim min_max As String = ""
        Dim strHTMLData1 As String = ""
        Dim color As String = "Blue"
        Dim string_for_spi_start As String = ""

        Dim cssClass As String = ""
        Dim YearDateVariable As String = ""
        Dim start_date As String = ""
        Dim strHTMLData1_START As String = ""

        Try

            lGraphType = 4

            strQuery1 = "SELECT DATEPART(year, journ_date) As YearSld, DATEPART(quarter, journ_date) As QuarterSld,"
            strQuery1 += " AVG(CAST(ac_mfr_year AS INT)) As dAvgYearMfr,"
            strQuery1 += " AVG(CAST(ac_year AS INT)) As dAvgYearDlv,"
            strQuery1 += " AVG(ac_asking_price) As dAvgAsking,"
            strQuery1 += " AVG(ac_hidden_asking_price) As dAvgAskingHidden,"

            strQuery1 += "  AVG(ac_sale_price) As dAvgSelling, "


            strQuery1 += " ((AVG(ac_sale_price)/AVG(ac_asking_price)) * 100) As dPercent,"
            strQuery1 += " ((1-(AVG(ac_sale_price)/AVG(ac_asking_price))) * 100) As dVariance,"
            strQuery1 += " ((AVG(ac_sale_price)/AVG(ac_hidden_asking_price)) * 100) As dPercentHidden,"
            strQuery1 += " ((1-(AVG(ac_sale_price)/AVG(ac_hidden_asking_price))) * 100) As dVarianceHidden,"
            strQuery1 += " AVG(ac_airframe_tot_hrs) As dAvgAFTT,"
            strQuery1 += " AVG(DateDiff(day,ac_list_date, journ_date)) As dAvgDOM"

            strQuery1 += " FROM Aircraft_Summary_SPI WITH (NOLOCK)"

            strQuery1 += " WHERE (ac_journ_id > 0)"


            ' added MSW - 5/17/2016
            strQuery1 += "  AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) "
            strQuery1 += "  AND (journ_subcat_code_part1 = 'WS')"       '-- Whole Sales Only 
            strQuery1 += "  AND (journ_internal_trans_flag = 'N')"            '-- No Internals 

            ' If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                strQuery1 += (Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                strQuery1 += (Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                strQuery1 += (Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                strQuery1 += (Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If
            'End If


            If CLng(searchCriteria.ViewCriteriaAmodID) > 0 Then
                strQuery1 += " AND (amod_id = " & CStr(searchCriteria.ViewCriteriaAmodID) & ")"
            End If

            strQuery1 += " AND (ac_sale_price IS NOT NULL) AND (ac_sale_price <> 0) "
            strQuery1 += " AND (ac_asking_price IS NOT NULL) AND (ac_asking_price <> 0) "

            strQuery1 += " " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True)

            If CDbl(searchCriteria.ViewCriteriaTimeSpan) > 0 Then

                start_date = DateAdd(DateInterval.Month, -(searchCriteria.ViewCriteriaTimeSpan), Date.Now())

                If Month(start_date) = 12 Or Month(start_date) = 11 Or Month(start_date) = 10 Then '10/16/2016 - 1/1/2017 - 1/1/2014
                    start_date = "1/1/" & (Year(start_date) + 1)
                ElseIf Month(start_date) = 7 Or Month(start_date) = 8 Or Month(start_date) = 9 Then '8/16/2016 - 10/1/2016 - 10/1/2013
                    start_date = "10/1/" & Year(start_date)
                ElseIf Month(start_date) = 4 Or Month(start_date) = 5 Or Month(start_date) = 6 Then '5/16/2016 - 7/1/2016 - 7/1/2013
                    start_date = "7/1/" & Year(start_date)
                Else '1,2,3     '3/16/2016 - 3/1/2016 - 4/1/2013
                    start_date = "4/1/" & Year(start_date)
                End If

                'YearDateVariable = Year(DateAdd(DateInterval.Month, -CDbl(searchCriteria.ViewCriteriaTimeSpan), Now())) & "-" & Month(DateAdd(DateInterval.Month, -CDbl(searchCriteria.ViewCriteriaTimeSpan), Now())) & "-" & Day(DateAdd(DateInterval.Month, -CDbl(searchCriteria.ViewCriteriaTimeSpan), Now()))

                strQuery1 += " AND journ_date >= '" & start_date & "' "
            End If


            strQuery1 += " AND (DATEPART(year,journ_date) >= " + searchCriteria.ViewCriteriaSPIYearSld1 + ")"

            strQuery1 += " AND (DATEPART(year,journ_date) <= " + searchCriteria.ViewCriteriaSPIYearSld2 + ")"


            strQuery1 += " GROUP BY DATEPART(year, journ_date), DATEPART(quarter, journ_date)"
            strQuery1 += " ORDER BY DATEPART(year, journ_date) ASC, DATEPART(quarter, journ_date) ASC"


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, strQuery1.ToString)

            '  Case Else

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            sqlcommand_final.Connection = SqlConn
            sqlcommand_final.CommandType = CommandType.Text
            sqlcommand_final.CommandTimeout = 60

            sqlcommand_final.CommandText = strQuery1
            adors_final = sqlcommand_final.ExecuteReader()


            strHTMLData1 = "<table id='quarterlyModelDataTable' cellpadding='2' cellspacing='0' width='100%'>"
            strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center' colspan='9'>" & searchCriteria.ViewCriteriaAircraftMake.ToString & "&nbsp;/&nbsp;" & searchCriteria.ViewCriteriaAircraftModel.ToString & " ASKING/SALE PRICE SUMMARY BY QUARTER</td></tr>"

            strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center' rowspan='2'>Year<br />Quarter</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Avg Year Of</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Avg Price ($k)</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center' rowspan='2'>Percent</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center' rowspan='2'>Variance</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Average</td></tr>"

            strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center'>Mfr</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center'>Delivery</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center'>Asking </td>"
            strHTMLData1 = strHTMLData1 & "<td align='center'>Selling</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center'>AFTT</td>"
            strHTMLData1 = strHTMLData1 & "<td align='center'>Days<br />On<br />Market</td></tr>"



            strHTMLData1_START = "<table id='quarterlyModelDataTable' cellpadding='2' cellspacing='0' width='100%' border='1' class=""salesPriceTable"">"
            strHTMLData1_START = strHTMLData1_START & "<tr><td align='center' colspan='9'  class='headerLine'>" & searchCriteria.ViewCriteriaAircraftMake.ToString & "&nbsp;/&nbsp;" & searchCriteria.ViewCriteriaAircraftModel.ToString & " ASKING WITH SOLD PRICE SUMMARY BY QUARTER (LAST " & searchCriteria.ViewCriteriaTimeSpan.ToString & " MONTHS)</td></tr>"


            If adors_final.HasRows Then


                lTotRec = 1000

                lCnt1 = 0
                lCnt2 = 0
                lCnt3 = 0
                lCnt4 = 0
                lCnt5a = 0
                lCnt5b = 0
                lCnt6 = 0
                lCnt7 = 0
                lCnt8 = 0
                lCnt9 = 0
                lCnt10 = 0
                lCnt11 = 0

                ReDim aData1(lTotRec)      ' Percentage Of Asking Price
                ReDim aLabels1(lTotRec)

                ReDim aData2(lTotRec)      ' Variance Of Asking Price
                ReDim aLabels2(lTotRec)

                ReDim aData3(lTotRec)      ' Asking Price
                ReDim aLabels3(lTotRec)

                ReDim aData4(lTotRec)      ' Selling Price
                ReDim aLabels4(lTotRec)

                ReDim aData5a(lTotRec)     ' Asking Price
                ReDim aLabels5a(lTotRec)

                ReDim aData5b(lTotRec)      ' Selling Price
                ReDim aLabels5b(lTotRec)

                ReDim aData6(lTotRec)      ' Avg AFTT
                ReDim aLabels6(lTotRec)

                ReDim aData7(lTotRec)      ' Avg DOM
                ReDim aLabels7(lTotRec)

                ReDim aData8(lTotRec)      ' Avg AFTT vs Variance
                ReDim aLabels8(lTotRec)

                ReDim aData9(lTotRec)      ' Avg DOM vs Variance
                ReDim aLabels9(lTotRec)

                ReDim aData10(lTotRec)     ' Avg AFTT vs Selling Price
                ReDim aLabels10(lTotRec)

                ReDim aData11(lTotRec)     ' Avg DOM vs Selling Price
                ReDim aLabels11(lTotRec)

                strHTMLData1 = strHTMLData1_START

                strHTMLData1 = strHTMLData1 & "<tr><td align='center' rowspan='2' class=""fieldHeader"">Year<br />Quarter</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'  class=""SupfieldHeader"">Avg Year Of</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'  class=""SupfieldHeader"">Avg Price ($k)</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' rowspan='2' class=""fieldHeader"">Percent</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' rowspan='2' class=""fieldHeader"">Variance</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'  class=""SupfieldHeader"">Average</td></tr>"

                strHTMLData1 = strHTMLData1 & "<tr><td align='center' class=""fieldHeader"">Mfr</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' class=""fieldHeader"">Delivery</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' class=""fieldHeader"">Asking</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' class=""fieldHeader"">Selling</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' class=""fieldHeader"">AFTT</td>"
                strHTMLData1 = strHTMLData1 & "<td align='center' class=""fieldHeader"">Days<br />On<br />Market</td></tr>"

                Do While adors_final.Read

                    strYearSld = ""
                    strQuarterSld = ""
                    strYearQtrName = ""

                    strAvgYearMfr = ""
                    strAvgYearDlv = ""
                    strAvgAsking = ""
                    strAvgAskingHidden = ""
                    strAvgSelling = ""
                    strPercent = ""
                    strPercentHidden = ""
                    strVariance = ""
                    strVarianceHidden = ""
                    strAvgAFTT = ""
                    strAvgDOM = ""

                    lYearSld = 0
                    lQuarterSld = 0
                    dAvgYearMfr = 0.0
                    dAvgYearDlv = 0.0
                    dAvgAsking = 0.0
                    dAvgAskingHidden = 0.0
                    dAvgSelling = 0.0
                    dPercent = 0.0
                    dPercentHidden = 0.0
                    dVariance = 0.0
                    dVarianceHidden = 0.0
                    dAvgAFTT = 0.0
                    dAvgDOM = 0.0

                    If Not String.IsNullOrEmpty(adors_final("YearSld")) Then
                        lYearSld = adors_final("YearSld")
                    Else
                        lYearSld = Year(Now())
                    End If

                    If Not String.IsNullOrEmpty(adors_final("QuarterSld")) Then
                        lQuarterSld = adors_final("QuarterSld")
                    Else
                        '     lQuarterSld = Right(Get_Quarter_For_Month_Server(Month(Now())), 1)
                    End If

                    strYearSld = CStr(lYearSld)
                    strQuarterSld = CStr(lQuarterSld)

                    strYearQtrName = strYearSld & cHyphen & "Q" & strQuarterSld

                    If Not IsDBNull(adors_final("dAvgYearMfr")) Then
                        dAvgYearMfr = adors_final("dAvgYearMfr")
                    Else
                        dAvgYearMfr = 0
                    End If

                    If Not IsDBNull(adors_final("dAvgYearDlv")) Then
                        dAvgYearDlv = adors_final("dAvgYearDlv")
                    Else
                        dAvgYearDlv = 0
                    End If

                    If Not IsDBNull(adors_final("dAvgAsking")) Then
                        dAvgAsking = adors_final("dAvgAsking")
                    Else
                        dAvgAsking = 0
                    End If

                    If Not IsDBNull(adors_final("dAvgAskingHidden")) Then
                        dAvgAskingHidden = adors_final("dAvgAskingHidden")
                    Else
                        dAvgAskingHidden = 0
                    End If


                    If dAvgAsking = 0 And dAvgAskingHidden > 0 Then
                        dAvgAsking = dAvgAskingHidden
                    End If

                    If Not IsDBNull(adors_final("dAvgSelling")) Then
                        dAvgSelling = adors_final("dAvgSelling")
                    Else
                        dAvgSelling = 0
                    End If

                    If Not IsDBNull(adors_final("dPercent")) Then
                        dPercent = adors_final("dPercent")
                    Else
                        dPercent = 0
                    End If

                    If Not IsDBNull(adors_final("dPercentHidden")) Then
                        dPercentHidden = adors_final("dPercentHidden")
                    Else
                        dPercentHidden = 0
                    End If

                    If dAvgAsking = 0 And dAvgAskingHidden > 0 Then
                        dPercent = dPercentHidden
                    End If

                    If Not IsDBNull(adors_final("dVariance")) Then
                        dVariance = adors_final("dVariance")
                    Else
                        dVariance = 0
                    End If

                    If Not IsDBNull(adors_final("dVarianceHidden")) Then
                        dVarianceHidden = adors_final("dVarianceHidden")
                    Else
                        dVarianceHidden = 0
                    End If

                    If dAvgAsking = 0 And dAvgAskingHidden > 0 Then
                        dVariance = dVarianceHidden
                    End If

                    If Not IsDBNull(adors_final("dAvgAFTT")) Then
                        dAvgAFTT = adors_final("dAvgAFTT")
                    Else
                        dAvgAFTT = 0
                    End If

                    If Not IsDBNull(adors_final("dAvgDOM")) Then
                        dAvgDOM = adors_final("dAvgDOM")
                    Else
                        dAvgDOM = 0
                    End If

                    '----------------------------- all changed to strHTMLData1_2 to mimick order of table - msw - 8/30/2011

                    strHTMLData1_2 = "<tr class=""" & cssClass & """><td align='left' nowrap='nowrap'>" & strYearQtrName & "</td>"

                    If cssClass = "" Then
                        cssClass = "alt_row"
                    Else
                        cssClass = ""
                    End If
                    strHTMLData1_2 = strHTMLData1_2 & "<td align='center'>"
                    If dAvgYearMfr > 0 Then
                        strHTMLData1_2 = strHTMLData1_2 & CStr(dAvgYearMfr) & "</td>"
                    Else
                        strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
                    End If

                    strHTMLData1_2 = strHTMLData1_2 & "<td align='center'>"
                    If dAvgYearDlv > 0 Then
                        strHTMLData1_2 = strHTMLData1_2 & CStr(dAvgYearDlv) & "</td>"
                    Else
                        strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
                    End If

                    strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
                    If dAvgAsking > 0 Then
                        strHTMLData1_2 = strHTMLData1_2 & "$" & FormatNumber(dAvgAsking / 1000, 0, True) & "</td>"
                    Else
                        strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
                    End If

                    strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
                    If dAvgSelling > 0 Then
                        strHTMLData1_2 = strHTMLData1_2 & "$" & FormatNumber(dAvgSelling / 1000, 0, True) & "</td>"
                    Else
                        strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
                    End If

                    strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
                    If dPercent > 0 Then
                        strHTMLData1_2 = strHTMLData1_2 & FormatNumber(dPercent, 1, True) & "%</td>"
                    Else
                        strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
                    End If

                    strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
                    If dAvgAsking > 0 Then
                        strHTMLData1_2 = strHTMLData1_2 & FormatNumber(dVariance, 1, True) & "%</td>"
                    ElseIf dAvgAsking = dAvgSelling Then
                        strHTMLData1_2 = strHTMLData1_2 & "0.0%</td>"
                    Else
                        strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
                    End If

                    strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
                    If dAvgAFTT > 0 Then
                        strHTMLData1_2 = strHTMLData1_2 & FormatNumber(dAvgAFTT, 0, True) & "</td>"
                    Else
                        strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
                    End If

                    strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
                    If dAvgDOM > 0 Then
                        strHTMLData1_2 = strHTMLData1_2 & FormatNumber(dAvgDOM, 0, True) & "</td>"
                    Else
                        strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
                    End If

                    strHTMLData1_2 = strHTMLData1_2 & "</tr>"

                    '--------------------------------------

                    strHTMLData1_2_final = strHTMLData1_2 & strHTMLData1_2_final


                    ' Percentage Of Asking Price  
                    If dAvgAsking > 0 Then
                        lCnt1 = lCnt1 + 1
                        aLabels1(lCnt1 - 1) = strYearQtrName
                        aData1(lCnt1 - 1) = CDbl(FormatNumber(dPercent, 1, True))
                    End If

                    ' Variance Of Asking Price  
                    If dAvgAsking > 0 Then
                        lCnt2 = lCnt2 + 1
                        aLabels2(lCnt2 - 1) = strYearQtrName
                        aData2(lCnt2 - 1) = CDbl(FormatNumber(dVariance, 1, True))
                    End If

                    ' Asking Price        
                    If dAvgAsking > 0 Then
                        lCnt3 = lCnt3 + 1
                        aLabels3(lCnt3 - 1) = strYearQtrName
                        aData3(lCnt3 - 1) = CDbl(FormatNumber(dAvgAsking / 1000, 1, True))
                    End If

                    ' Selling Price        
                    If dAvgSelling > 0 Then
                        lCnt4 = lCnt4 + 1
                        aLabels4(lCnt4 - 1) = strYearQtrName
                        aData4(lCnt4 - 1) = CDbl(FormatNumber(dAvgSelling / 1000, 1, True))
                    End If

                    ' Asking Price        
                    lCnt5a = lCnt5a + 1
                    aLabels5a(lCnt5a - 1) = strYearQtrName
                    aData5a(lCnt5a - 1) = CDbl(FormatNumber(dAvgAsking / 1000, 1, True))

                    ' Selling Price        
                    lCnt5b = lCnt5b + 1
                    aLabels5b(lCnt5b - 1) = strYearQtrName
                    aData5b(lCnt5b - 1) = CDbl(FormatNumber(dAvgSelling / 1000, 1, True))

                    ' Avg AFTT
                    If dAvgAFTT > 0 Then
                        lCnt6 = lCnt6 + 1
                        aLabels6(lCnt6 - 1) = strYearQtrName
                        aData6(lCnt6 - 1) = CLng(FormatNumber(dAvgAFTT, 0, True))
                    End If

                    ' Avg DOM
                    If dAvgDOM > 0 Then
                        lCnt7 = lCnt7 + 1
                        aLabels7(lCnt7 - 1) = strYearQtrName
                        aData7(lCnt7 - 1) = CLng(FormatNumber(dAvgDOM, 0, True))
                    End If

                    ' Avg AFTT vs Variance
                    If dAvgAFTT > 0 And dAvgAsking > 0 Then
                        lCnt8 = lCnt8 + 1
                        aLabels8(lCnt8 - 1) = FormatNumber(dAvgAFTT, 0, True)
                        aData8(lCnt8 - 1) = CDbl(FormatNumber(dVariance, 1, True))
                    End If

                    ' Avg DOM vs Variance
                    If dAvgDOM > 0 And dAvgAsking > 0 Then
                        lCnt9 = lCnt9 + 1
                        aLabels9(lCnt9 - 1) = FormatNumber(dAvgDOM, 0, True)
                        aData9(lCnt9 - 1) = CDbl(FormatNumber(dVariance, 1, True))
                    End If

                    ' Avg AFTT vs Selling Price
                    If dAvgAFTT > 0 And dAvgSelling > 0 Then
                        lCnt10 = lCnt10 + 1
                        aLabels10(lCnt10 - 1) = FormatNumber(dAvgAFTT, 0, True)
                        aData10(lCnt10 - 1) = CDbl(FormatNumber((dAvgSelling / 1000), 0, True))
                    End If

                    ' Avg DOM vs Selling Price
                    If dAvgDOM > 0 And dAvgSelling > 0 Then
                        lCnt11 = lCnt11 + 1
                        aLabels11(lCnt11 - 1) = FormatNumber(dAvgDOM, 0, True)
                        aData11(lCnt11 - 1) = CDbl(FormatNumber((dAvgSelling / 1000), 0, True))
                    End If



                Loop

                're_order_arrays(aLabels2, aData2)



                '  Graph Types
                '  1=2D-Pie,        2=3D Pie
                '  3=2D Bar,        4=3D Bar
                '  6=Line,          7=Line 
                '  8=Area,          9=Speckle
                ' 10=Circle Line,  13=3D Ribbon 
                ' 14=3D-Area,      15=Line 
                ' 16=Line,         17=+/- Bar







                If lCnt1 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250


                    strTitle1 = searchCriteria.ViewCriteriaAircraftMake & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Percentage of Asking Price (%)"
                    strBottomTitle1 = "Year/Quarter Sold"    ' Y      
                    strLeftTitle1 = "Percentage (%)"

                    ReDim Preserve aData1(lCnt1)
                    ReDim Preserve aLabels1(lCnt1)

                    '       SortLabelsValue(aLabels1, aData1, True)
                    lGraphType = 4
                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle1, strBottomTitle1, strLeftTitle1, lGraphType, aLabels1, aData1, 0, "", "##.0", 0, 0, color, GRAPH_TO_CHART)
                    If tmpGraph.ToString.Length > 2 Then
                        ' If Trim(tmpGraph) <> "" Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle1)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_1.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        ' views_spi_return_quarterly_by_model += ("<tr><td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_1.jpg'><img width='350' border='0' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_1.jpg'></td>")
                        GRAPH_TO_CHART.Series.Clear()
                        'End If

                    Else
                        views_spi_return_quarterly_by_model += "<tr><td align='center'>Percentage of Asking Price (%)<br />Not Enough Data Available</td>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<tr><td align='center'>Percentage of Asking Price (%)<br />Not Enough Data Available</td>"
                End If ' If lCnt1 > 1 Then

                If lCnt2 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle2 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Variance of Asking Price (%)"
                    strBottomTitle2 = "Year/Quarter Sold"    ' Y
                    strLeftTitle2 = "Variance (%)"

                    ReDim Preserve aData2(lCnt2)
                    ReDim Preserve aLabels2(lCnt2)

                    '    SortLabelsValue(aLabels2, aData2, True)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle2, strBottomTitle2, strLeftTitle2, lGraphType, aLabels2, aData2, 0, "", "##.0", 0, 0, color, GRAPH_TO_CHART)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle2)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_2.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        'views_spi_return_quarterly_by_model += ("<td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_2.jpg'><img width='350' border='0' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_2.jpg'></td></tr>")
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<td align='center'>Variance of Asking Price (%)<br />Not Enough Data Available</td></tr>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<td align='center'>Variance of Asking Price (%)<br />Not Enough Data Available</td></tr>"
                End If ' If lCnt2 > 1 Then




                If lCnt5a > 1 And lCnt5b > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 7
                    strTitle5 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Asking vs Selling Price (k)"

                    strBottomTitle5 = "Year Sold-Quarter"    ' Y
                    strRightTitle5 = "Price ($)"
                    strLeftTitle5 = "Price ($)"

                    ReDim Preserve aData5a(lCnt5a)
                    ReDim Preserve aData5b(lCnt5b)
                    ReDim Preserve aLabels5a(lCnt5a)
                    ReDim Preserve aLabels5b(lCnt5b)

                    'If Not HttpContext.Current.Session.Item("localMachine") Then

                    '  objGraph = Server.CreateObject("GSSERVER.GSServerProp")

                    ' If lMaxSets = 2 Then

                    ' SortLabelsWithTwoDataValues(aLabels5a, aLabels5b, aData5a, aData5b, True, 1)

                    '  SetMultiLineGraphTitleStyleLabels(objGraph, lMaxSets, lGraphType, strTitle5, strBottomTitle5, strLeftTitle5, strRightTitle5, aLabels5a, "", "#,###", 2)
                    '   End If

                    google_map_string = " data1.addColumn('string', 'Serial#'); "
                    google_map_string &= " data1.addColumn('number', 'Asking'); "
                    google_map_string &= " data1.addColumn('number', 'Take'); "
                    google_map_string &= " data1.addColumn('number', 'Est/Sold Value'); "
                    google_map_string &= " data1.addColumn('number', 'My AC Asking'); "
                    google_map_string &= " data1.addColumn('number', 'My AC Take'); "
                    google_map_string &= " data1.addColumn('number', 'My AC Est Value'); "
                    google_map_string &= " data1.addRows(["


                    '    SetMultiLineGraphAddData(objGraph, lThisSet, aData5a, 5, 0, "Avg Asking Price")
                    min_max = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle5, strBottomTitle5, strLeftTitle5, lGraphType, aLabels5b, aData5b, 0, "", "", min, max, color, GRAPH_TO_CHART, google_map_string)


                    min = Left(min_max, (InStr(min_max, ",") - 1))
                    max = Right(min_max, (min_max.ToString.Length - InStr(min_max, ",")))
                    '  SetMultiLineGraphAddData(objGraph, lThisSet, aData5b, 1, 0, "Avg Selling Price")


                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle5 & "&nbsp;", strBottomTitle5, strLeftTitle5, lGraphType, aLabels5a, aData5a, 0, "", "", min, max, color, GRAPH_TO_CHART, google_map_string)

                    '       strGraphAskingVsSelling = DrawMultiLineGraphs(objGraph)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle5)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_5.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        ' views_spi_return_quarterly_by_model += ("<tr><td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_5.jpg'><img width='350' border='0' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_5.jpg'><br>")
                        ' views_spi_return_quarterly_by_model += tmpGraph
                        '  views_spi_return_quarterly_by_model += "</td>"
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<tr><td align='center'>Asking vs selling Price (k)<br />Not Enough Data Availabl</td>"
                    End If
                    'Else
                    '    views_spi_return_quarterly_by_model += "<tr><td align='center'>Asking vs Selling Price (k)<br />Not Enough Data Availabl</td>"

                    '    GRAPH_TO_CHART.Series.Clear()
                    ' End If ' If lCnt5a > 1 And lCnt5b > 1 Then      


                End If








                If lCnt3 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle3 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Asking Price (k)"
                    strBottomTitle3 = "Year/Quarter Sold"    ' Y
                    strLeftTitle3 = "Price ($)"

                    ReDim Preserve aData3(lCnt3)
                    ReDim Preserve aLabels3(lCnt3)

                    '   SortLabelsValue(aLabels3, aData3, True)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle3, strBottomTitle3, strLeftTitle3, lGraphType, aLabels3, aData3, 0, "", "#,###", 0, 0, color, GRAPH_TO_CHART)
                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle3)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_3.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        ' views_spi_return_quarterly_by_model += ("<td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_3.jpg'><img width='350' border='0' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_3.jpg'></td></tr>")
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<td align='center'>Asking Price (k)<br />Not Enough Data Available</td></tr>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<td align='center'>Asking Price (k)<br />Not Enough Data Available</td></tr>"
                End If ' If lCnt3 > 1 Then



                If lCnt4 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle4 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Selling Price (k)"
                    strBottomTitle4 = "Year/Quarter Sold"    ' Y
                    strLeftTitle4 = "Price ($)"
                    ReDim Preserve aData4(lCnt4)
                    ReDim Preserve aLabels4(lCnt4)

                    '  SortLabelsValue(aLabels4, aData4, True)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle4, strBottomTitle4, strLeftTitle4, lGraphType, aLabels4, aData4, 0, "", "#,###", 0, 0, color, GRAPH_TO_CHART)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle4)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_4.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        ' views_spi_return_quarterly_by_model += ("<tr><td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_4.jpg'><img width='350' border='0' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_4.jpg'></td>")
                        GRAPH_TO_CHART.Series.Clear()

                    Else
                        views_spi_return_quarterly_by_model += "<tr><td align='center'>Selling Price (k)<br />Not Enough Data Available</td>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<tr><td align='center'>Selling Price (k)<br />Not Enough Data Available</td>"
                End If ' If lCnt4 > 1 Then




                If lCnt6 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle6 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Average Airframe Total Time"
                    strBottomTitle6 = "Year/Quarter Sold"    ' Y


                    ReDim Preserve aData6(lCnt6)
                    ReDim Preserve aLabels6(lCnt6)

                    '  SortLabelsValue(aLabels6, aData6, True)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle6, strBottomTitle6, strLeftTitle6, lGraphType, aLabels6, aData6, 0, "", "#,###", 0, 0, color, GRAPH_TO_CHART)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle6)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_6.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        views_spi_return_quarterly_by_model += ("<td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_6.jpg'><img width='350' border='0'  src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_6.jpg'></td></tr>")
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<td align='center'>Average Airframe Total Time<br />Not Enough Data Available</td></tr>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<td align='center'>Average Airframe Total Time<br />Not Enough Data Available</td></tr>"
                End If ' If lCnt6 > 1 Then


                If lCnt7 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle7 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Average Days On Market"
                    strBottomTitle7 = "Year/Quarter Sold"    ' Y


                    ReDim Preserve aData7(lCnt7)
                    ReDim Preserve aLabels7(lCnt7)

                    ' SortLabelsValue(aLabels7, aData7, True)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle7, strBottomTitle7, strLeftTitle7, lGraphType, aLabels7, aData7, 0, "", "#,###", 0, 0, color, GRAPH_TO_CHART)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle7)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_7.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        views_spi_return_quarterly_by_model += ("<tr><td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_7.jpg'><img width='350' border='0'  src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_7.jpg'></td>")
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<tr><td align='center'>Average Days On Market<br />Not Enough Data Available</td>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<tr><td align='center'>Average Days On Market<br />Not Enough Data Available</td>"
                End If ' If lCnt7 > 1 Then


                If lCnt8 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle8 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Avg AFTT vs Variance (%)"
                    strBottomTitle8 = "Average Airframe Total Time"     ' Y
                    ' X    

                    ReDim Preserve aData8(lCnt8)
                    ReDim Preserve aLabels8(lCnt8)

                    AIRCRAFT_SUMMARY_SortLabelsValue(aLabels8, aData8, lCnt8 - 1, 1)

                    ReDim Preserve aData8(lCnt8)
                    ReDim Preserve aLabels8(lCnt8)
                    '   SortLabelsValue(aLabels8, aData8, True)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle8, strBottomTitle8, strLeftTitle8, lGraphType, aLabels8, aData8, 0, "#,###", "##.0", 0, 0, color, GRAPH_TO_CHART)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle8)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_8.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        views_spi_return_quarterly_by_model += ("<td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_8.jpg'><img width='350' border='0' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_8.jpg'></td></tr>")
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<td align='center'>Avg AFTT vs Variance (%)<br />Not Enough Data Available</td></tr>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<td align='center'>Avg AFTT vs Variance (%)<br />Not Enough Data Available</td></tr>"
                End If ' If lCnt8 > 1 Then


                If lCnt9 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle9 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Avg DOM vs Variance (%)"
                    strBottomTitle9 = "Average Days on Market"     ' Y
                    ' X    

                    ReDim Preserve aData9(lCnt9 - 1)
                    ReDim Preserve aLabels9(lCnt9 - 1)

                    AIRCRAFT_SUMMARY_SortLabelsValue(aLabels9, aData9, lCnt9 - 1, 1)

                    ReDim Preserve aData9(lCnt9)
                    ReDim Preserve aLabels9(lCnt9)
                    '  SortLabelsValue(aLabels9, aData9, True)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle9, strBottomTitle9, strLeftTitle9, lGraphType, aLabels9, aData9, 0, "#,###", "##.0", 0, 0, color, GRAPH_TO_CHART)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle9)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_9.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        views_spi_return_quarterly_by_model += ("<tr><td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_9.jpg'><img width='350' border='0'  src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_9.jpg'></td>")
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<tr><td align='center'>Avg DOM vs Variance (%)<br />Not Enough Data Available</td>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<tr><td align='center'>Avg DOM vs Variance (%)<br />Not Enough Data Available</td>"
                End If ' If lCnt9 > 1 Then


                If lCnt10 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle10 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Avg AFTT vs Selling Price (k)"
                    strBottomTitle10 = "Average Airframe Total Time"     ' Y


                    ReDim Preserve aData10(lCnt10 - 1)
                    ReDim Preserve aLabels10(lCnt10 - 1)

                    AIRCRAFT_SUMMARY_SortLabelsValue(aLabels10, aData10, lCnt10 - 1, 1)

                    ReDim Preserve aData10(lCnt10)
                    ReDim Preserve aLabels10(lCnt10)
                    'SortLabelsValue(aLabels10, aData10)



                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle10, strBottomTitle10, strLeftTitle10, lGraphType, aLabels10, aData10, 0, "#,###", "#,###", 0, 0, color, GRAPH_TO_CHART)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle10)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_10.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        views_spi_return_quarterly_by_model += ("<td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_10.jpg'><img width='350' border='0' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_10.jpg'></td></tr>")
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<td align='center'>AFTT vs Selling Price (k)<br />Not Enough Data Available</td></tr>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<td align='center'>AFTT vs Selling Price (k)<br />Not Enough Data Available</td></tr>"
                End If ' If lCnt10 > 1 Then

                If lCnt11 > 1 Then

                    HttpContext.Current.Session.Item("SPImageWidth") = 250
                    HttpContext.Current.Session.Item("SPImageHeight") = 250

                    lGraphType = 4
                    strTitle11 = searchCriteria.ViewCriteriaAircraftMake.ToString & "/" & searchCriteria.ViewCriteriaAircraftModel.ToString & " - Avg DOM vs Selling Price (k)"
                    strBottomTitle11 = "Average Days on Market"     ' Y



                    ReDim Preserve aData11(lCnt11 - 1)
                    ReDim Preserve aLabels11(lCnt11 - 1)

                    AIRCRAFT_SUMMARY_SortLabelsValue(aLabels11, aData11, lCnt11 - 1, 1)

                    ReDim Preserve aData11(lCnt11)
                    ReDim Preserve aLabels11(lCnt11)

                    ' Array.Sort(aData11, aLabels11)
                    '  Array.Sort(aLabels11)

                    tmpGraph = AIRCRAFT_SUMMARY_CreateAndGraphData(strTitle11, strBottomTitle11, strLeftTitle11, lGraphType, aLabels11, aData11, 0, "#,###", "#,###", 0, 0, color, GRAPH_TO_CHART)

                    If tmpGraph.ToString.Length > 2 Then
                        GRAPH_TO_CHART.Titles.Clear()
                        GRAPH_TO_CHART.Titles.Add(strTitle11)
                        GRAPH_TO_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        GRAPH_TO_CHART.SaveImage(HttpContext.Current.Server.MapPath("") & "\Tempfiles\" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_11.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        views_spi_return_quarterly_by_model += ("<tr><td align='center'><a target='_blank' href='Tempfiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_11.jpg'><img width='350' border='0' src='TempFiles/" & searchCriteria.ViewCriteriaAmodID.ToString & "SPI_QUARTER_11.jpg'></td></tr>")
                        GRAPH_TO_CHART.Series.Clear()
                    Else
                        views_spi_return_quarterly_by_model += "<tr><td align='center'>Avg DOM vs Selling Price (k)<br />Not Enough Data Available</td></tr>"
                    End If
                Else
                    views_spi_return_quarterly_by_model += "<tr><td align='center'>Avg DOM vs Selling Price (k)<br />Not Enough Data Available</td></tr>"
                End If ' If lCnt11 > 1 Then



            Else
                'strHTMLData1 = "<table id='quarterlyModelDataTable' cellpadding='2' cellspacing='0' width='100%'><tr><td align='center' colspan='9'>No Records Found</td></tr>"
                strHTMLData1 = strHTMLData1_START
                strHTMLData1 &= "<tr><td align='center' colspan='9'>No Records Found</td></tr>"
            End If


            strHTMLData1 = strHTMLData1 & strHTMLData1_2_final

            strHTMLData1 = strHTMLData1 & "</table>"


            views_spi_return_quarterly_by_model = "<table width='100%' cellspacing='0' cellpadding='0'>" & views_spi_return_quarterly_by_model & "</table>"

            string_for_spi_start = strHTMLData1

            out_htmlString = string_for_spi_start


            adors_final.Close()

        Catch ex As Exception
        Finally
            SqlConn.Close()
            SqlConn.Dispose()
        End Try
    End Function

    Public Sub views_spi_display_report(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef SPI_QUARTER As System.Web.UI.DataVisualization.Charting.Chart, ByRef google_string As String, ByVal try_new_chart As String, ByRef sHtmlSPIReport_bottom As String, ByVal sales_string As String, ByRef middle_text As String, Optional ByRef sale_prices As String = "")

        Dim htmlOut As New StringBuilder
        Dim page_count As Integer = 0
        Dim space_spot As Integer = 0
        Dim text_from_first As String = ""
        Dim color As String = "Blue"
        Dim temp_string As String = ""
        Dim graphs1 As String = ""
        Dim graphs2 As String = ""

        Try
            Dim sHtmlSPISalePrices As String = ""
            views_spi_return_sale_price_table_by_model(searchCriteria, sHtmlSPISalePrices)
            If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                htmlOut.Append(sHtmlSPISalePrices)
                views_spi_return_sale_price_table_by_model(searchCriteria, sale_prices, True) ' call without links 
            End If

            If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                If Trim(sales_string) <> "" Then
                    htmlOut.Append("</td></tr><tr><td width=""100%"" valign=""top"">")
                    htmlOut.Append(sales_string)
                End If
            End If



            Dim sHtmlSPIQuarterModel As String = ""
            graphs1 = views_spi_return_quarterly_by_model(searchCriteria, sHtmlSPIQuarterModel, SPI_QUARTER, google_string)

            Dim sHtmlSPIQuarterWeightClass As String = ""
            graphs2 = views_spi_return_previous_full_quarterly_by_weightclass(searchCriteria, sHtmlSPIQuarterWeightClass, SPI_QUARTER)

            'htmlOut.Append(sHtmlSPIQuarterModel)
            middle_text = sHtmlSPIQuarterModel

            '  If Trim(try_new_chart) = "Y" Then


            '  Else
            sHtmlSPIReport_bottom = graphs1
            sHtmlSPIReport_bottom = sHtmlSPIReport_bottom & sHtmlSPIQuarterWeightClass
            sHtmlSPIReport_bottom = sHtmlSPIReport_bottom & graphs2
            ' End If

        Catch ex As Exception

        End Try

        out_htmlString = htmlOut.ToString
        htmlOut = Nothing

    End Sub

    Public Sub views_spi_return_sale_price_table_by_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef returnString As String, Optional ByVal nolink As Boolean = False)
        Dim SalesTable As New DataTable
        Dim cssClass As String = ""
        Dim temp_sale As String = ""
        Dim temp_counter As Integer = 0
        Dim start_string As String = ""
        SalesTable = get_spi_sale_prices(searchCriteria, "N", "Y", "", searchCriteria.ViewCriteriaTimeSpan)
        If Not IsNothing(SalesTable) Then
            If SalesTable.Rows.Count > 0 Then
                returnString = "<table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""salesPriceTable"" border=""1"">"
                returnString += "<tr><td align=""left"" valign=""top"" colspan='7' class=""headerLine"">" & searchCriteria.ViewCriteriaAircraftMake.Trim & " / " & searchCriteria.ViewCriteriaAircraftModel.Trim & " Reported Sales Prices (Last " & searchCriteria.ViewCriteriaTimeSpan.ToString & " Months)</td></tr>"
                returnString += "<tr class=""fieldHeader"">"

                'Ser #
                returnString += "<td align='center' valign='top'>Serial #</td>"

                'Reg #
                returnString += "<td align='center' valign='top'>Reg#</td>"

                'Transaction Date
                returnString += "<td align='center' valign='top'>Date</td>"

                'Transaction Info
                returnString += "<td align='left' valign='top'>Transaction</td>"

                'Year MFR
                returnString += "<td align='center' valign='top'>Year MFR</td>"

                'Asking Price
                returnString += "<td align='right' valign='top'>Asking ($k)</td>"

                'Sale Price
                returnString += "<td align='right' valign='top'>Sold ($k)</td>"

                returnString += "</tr>"
                start_string = returnString

                For Each r As DataRow In SalesTable.Rows

                    If nolink = True Then
                        If temp_counter = 36 Then
                            returnString += "XXXX" & start_string
                            temp_counter = 0
                        End If
                        temp_counter = temp_counter + 1
                    End If


                    returnString += "<tr class=""" & cssClass & """>"

                    'Ser #
                    returnString += "<td align='center' valign='top'>"
                    If nolink = True Then
                        If Not IsDBNull(r("ac_ser_no_full")) Then
                            returnString += r("ac_ser_no_full").ToString
                        End If
                    Else
                        If Not IsDBNull(r("ac_ser_no_full")) Then
                            returnString += DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_no_full").ToString, "", "")
                        End If
                    End If

                    returnString += "</td>"

                    'Reg #
                    returnString += "<td align='center' valign='top'>"
                    If Not IsDBNull(r("ac_reg_no")) Then
                        returnString += r("ac_reg_no").ToString
                    End If
                    returnString += "</td>"

                    'Transaction Date
                    returnString += "<td align='center' valign='top'>"
                    If Not IsDBNull(r("journ_date")) Then
                        returnString += clsGeneral.clsGeneral.FormatDateShorthand(r("journ_date").ToString)
                    End If
                    returnString += "</td>"

                    'Transaction Info
                    returnString += "<td align='left' valign='top'>"

                    'Subject
                    If nolink = True Then
                        If Not IsDBNull(r("journ_subject")) Then
                            returnString += r("journ_subject").ToString
                        End If
                    Else
                        If Not IsDBNull(r("journ_subject")) Then
                            returnString += DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, r("journ_id"), True, r("journ_subject").ToString, "", "")
                        End If
                    End If

                    'Optional Note
                    If Not IsDBNull(r("journ_customer_note")) Then
                        If Not String.IsNullOrEmpty(r.Item("journ_customer_note")) Then
                            returnString += "&nbsp;&nbsp;(<span class=""help_cursor no_text_underline"" title=""" + r.Item("journ_customer_note").ToString + """>Note</span>)"
                        End If
                    End If

                    'Optional NEW AC SALE
                    If Not IsDBNull(r("journ_newac_flag")) Then
                        If Not String.IsNullOrEmpty(r.Item("journ_newac_flag")) Then
                            If Trim(r.Item("journ_newac_flag")) = "Y" Then
                                returnString += "&nbsp;&nbsp;(<span class=""help_cursor no_text_underline"" title='New Aircraft Sale'>SOLD NEW</span>)"
                            End If
                        End If
                    End If


                    returnString += "</td>"

                    'MFR Year
                    returnString += "<td align='center' valign='top'>"
                    If Not IsDBNull(r("ac_mfr_year")) Then
                        returnString += r("ac_mfr_year").ToString
                    End If
                    returnString += "</td>"

                    'Asking
                    returnString += "<td align='right' valign='top'>"
                    If Not IsDBNull(r("ac_asking_price")) Then
                        If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                            If (Trim(r("ac_asking").ToString.ToUpper) = "MAKE OFFER" Or Trim(r("ac_forsale_flag").ToString.ToUpper) = "N") Then
                                returnString += "<span alt='Reported Asking Price Displayed with Permission from Source' title='Reported Asking Price Displayed with Permission from Source' class='help_cursor text_underline'>"
                                returnString += "$" & FormatNumber(CDbl(r.Item("ac_asking_price")) / 1000, 0).ToString & ""
                                returnString += "</span>"
                            Else
                                returnString += "$" & FormatNumber(CDbl(r.Item("ac_asking_price")) / 1000, 0).ToString & ""
                            End If

                        End If
                    End If
                    returnString += "</td>"




                    'Sale
                    returnString += "<td align='right' valign='top'>"
                    temp_sale = ""
                    If Not IsDBNull(r.Item("ac_sold_price")) Then
                        If (r.Item("ac_sold_price").ToString / 1000) < 1000 Then
                            temp_sale = DisplayFunctions.TextToImage("$" & FormatNumber((r.Item("ac_sold_price").ToString / 1000), 0) & "k", 9, "", "35", "Reported Sale Price Displayed with Permission from Source")
                        Else
                            temp_sale = DisplayFunctions.TextToImage("$" & FormatNumber((r.Item("ac_sold_price").ToString / 1000), 0) & "k", 9, "", "43", "Reported Sale Price Displayed with Permission from Source")
                        End If
                    Else
                    End If

                    If InStr(temp_sale, "img") > 0 Then
                        returnString += "<p unselectable='on' style='display:inline'>"
                        If Not IsDBNull(r("ac_sold_price")) Then
                            If CInt(r("ac_sold_price")) <> 0 Then
                                returnString += "<span alt='Reported Sale Price Displayed with Permission from Source' title='Reported Sale Price Displayed with Permission from Source' class='help_cursor error_text text_underline'>"
                                If (r.Item("ac_sold_price").ToString / 1000) < 1000 Then
                                    returnString += DisplayFunctions.TextToImage("$" & FormatNumber((r.Item("ac_sold_price").ToString / 1000), 0) & "k", 9, "", "35", "Reported Sale Price Displayed with Permission from Source")
                                Else
                                    returnString += DisplayFunctions.TextToImage("$" & FormatNumber((r.Item("ac_sold_price").ToString / 1000), 0) & "k", 9, "", "43", "Reported Sale Price Displayed with Permission from Source")
                                End If
                                returnString += "</span>"
                            Else
                                returnString += "&nbsp;"
                            End If
                        End If
                        returnString += "</p>"
                    Else
                        returnString += temp_sale
                    End If


                    returnString += "</td>"
                    returnString += "</tr>"

                    If cssClass = "" Then
                        cssClass = "alt_row"
                    Else
                        cssClass = ""
                    End If
                Next
                returnString += "</table>"
            End If
        End If



    End Sub

    Public Function get_spi_sale_prices(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal internal_flag As String, ByVal retail_flag As String, ByVal last_date As String, ByVal months_to_Show As Integer) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim YearDateVariable As String = ""
        Dim aclsData_Temp As New clsData_Manager_SQL

        Try



            sQuery.Append("SELECT  journ_id, journ_subcategory_code, journ_date, journ_subject, ac_ser_no_full, ac_forsale_flag, ac_reg_no, ac_mfr_year, ac_id, ac_asking_price, ac_ser_no_sort, amod_make_name")

            sQuery.Append(", ac_sale_price as ac_sold_price ")

            sQuery.Append(", ac_list_date, ac_airframe_tot_hrs, journ_customer_note, journ_newac_flag ")
            sQuery.Append(", ac_sale_price_display_flag, case  when ac_asking IS NULL  then '' else ac_asking end as ac_asking ")
            sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id")
            sQuery.Append(" INNER JOIN journal_category WITH(NOLOCK) ON journ_subcategory_code = jcat_subcategory_code")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            sQuery.Append(" AND (jcat_category_code = 'AH')  ")

            sQuery.Append(aclsData_Temp.add_in_wholesale_non_internal_retail_string())

            'Not sure if this should stay, but keeping for now.
            sQuery.Append(" AND (ac_sale_price_display_flag = 'Y') ")

            If CDbl(months_to_Show) > 0 Then
                YearDateVariable = Year(DateAdd(DateInterval.Month, -CDbl(months_to_Show), Now())) & "-" & Month(DateAdd(DateInterval.Month, -CDbl(months_to_Show), Now())) & "-" & Day(DateAdd(DateInterval.Month, -CDbl(months_to_Show), Now()))
            ElseIf Trim(last_date) = "" Then
                YearDateVariable = Year(DateAdd(DateInterval.Year, -1, Now())) & "-" & Month(DateAdd(DateInterval.Year, -1, Now())) & "-" & Day(DateAdd(DateInterval.Year, -1, Now()))
            End If

            sQuery.Append(" AND journ_date >= '" & YearDateVariable & "' ")

            If Trim(last_date) <> "" Then
                YearDateVariable = Year(DateAdd(DateInterval.Year, 0, CDate(last_date))) & "-" & Month(DateAdd(DateInterval.Year, 0, CDate(last_date))) & "-" & Day(DateAdd(DateInterval.Year, 0, CDate(last_date)))
                sQuery.Append(" AND journ_date <= '" & YearDateVariable & "' ")
            End If


            If Trim(internal_flag) = "N" Then
                sQuery.Append(" AND  journ_internal_trans_flag = 'N' ")
            End If

            If Trim(retail_flag) = "Y" Then
                sQuery.Append(" AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) ")
            End If

            ' If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If
            'End If


            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))





            sQuery.Append(" ORDER BY journ_date DESC")

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
                aError = "Error in get_retail_sales_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_retail_sales_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub get_model_weight_class_type_code(ByVal amod_id As Long, ByRef weight_class As String, ByRef amod_type_code As Integer)
        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim localAdoRs2 As System.Data.SqlClient.SqlDataReader : localAdoRs2 = Nothing
        Dim Query As String = ""
        Dim temp_type_code As String = ""

        Try
            SqlConn.ConnectionString = clientConnectString

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 60

            Query = "SELECT amod_weight_class, amod_type_code FROM aircraft_model WITH(NOLOCK) WHERE amod_id = " + amod_id.ToString

            SqlCommand.CommandText = Query

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

            localAdoRs2 = SqlCommand.ExecuteReader()
            If localAdoRs2.HasRows Then
                localAdoRs2.Read()
                weight_class = localAdoRs2.Item("amod_weight_class")
                temp_type_code = localAdoRs2.Item("amod_type_code")
            End If

            If Trim(temp_type_code) = "E" Then
                amod_type_code = 1
            ElseIf Trim(temp_type_code) = "J" Then
                amod_type_code = 2
            ElseIf Trim(temp_type_code) = "T" Then
                amod_type_code = 3
            ElseIf Trim(temp_type_code) = "P" Then
                amod_type_code = 4
            Else
                amod_type_code = 5
            End If


        Catch ex As Exception
        Finally
            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try
    End Sub

#End Region

#Region "unused_functions"

    ' aka : excel column input parse function
    'Function CleanUserData(ByVal inputString, ByVal sFind, ByVal sReplace, ByVal bIsTextAreaInput)

    '  Dim cCommaDelim As String = ","
    '  Dim cColonDelim As String = ":"
    '  Dim cSemiColonDelim As String = ";"
    '  Dim cImbedComa As String = "_"
    '  Dim EXCEL2003CHAR As String = Chr(160)
    '  Dim cEmptyString As String = ""

    '  Dim n_loop, n_offset, n_offset1, sTmpData
    '  Dim sOutputString
    '  n_loop = 1
    '  n_offset = 0
    '  n_offset1 = 0
    '  sTmpData = ""
    '  sOutputString = ""

    '  If inputString <> "" Then
    '    If Not bIsTextAreaInput Then
    '      sOutputString = Trim(Replace(inputString, sFind, sReplace))
    '    Else
    '      ' find first CR
    '      ' commented out seems the starting CRLF has disapeared???
    '      'n_loop = (InStr(1, inputString, CRLF, vbBinaryCompare))
    '      ' remove it by starting after it
    '      If n_loop > 0 Then
    '        Do While n_loop < Len(inputString) + 1

    '          ' find first CR
    '          n_offset = InStr(n_loop, inputString, vbCrLf, vbBinaryCompare)
    '          n_offset1 = InStr(n_offset + 1, inputString, vbCrLf, vbBinaryCompare)

    '          If (n_offset = 0 Or n_offset1 = 0) And n_offset = n_offset1 Then
    '            sOutputString = Trim(inputString)
    '            Exit Do
    '          End If

    '          If (n_offset1 > n_loop) Then  ' found first CRLF after our start

    '            ' find next CRLF start 1 chars ahead of our first CRLF pair
    '            If (n_offset1 > n_offset) Then ' found next CRLF the data is between the two offsets
    '              If (n_offset1 - n_offset) > 1 Then ' ok we have at least one char between the two
    '                sTmpData = Mid(inputString, n_offset + 2, ((n_offset1 - n_offset) - 2)) ' ok get the data
    '                If UCase(sTmpData) <> "GA" And Len(sTmpData) > 0 Then ' clean out any "GA" Garbage also zero length data

    '                  ' I also need to preserve any commas in the data
    '                  If FindItemInData(sTmpData, cCommaDelim) Then
    '                    ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
    '                    sTmpData = CleanUserData(sTmpData, cCommaDelim, cImbedComa, False)
    '                  End If

    '                  ' clean out the EXCEL 03 Character
    '                  sTmpData = CleanUserData(sTmpData, EXCEL2003CHAR, cEmptyString, False)

    '                  sOutputString = sOutputString & sTmpData & sReplace
    '                End If
    '              End If
    '            End If

    '          Else
    '            Exit Do
    '          End If

    '          ' jump ahead 1 chars to look for the next chunk of data
    '          If n_offset1 > 0 Then
    '            n_loop = n_offset1
    '          End If

    '          n_offset = 0
    '          n_offset1 = 0
    '          sTmpData = ""
    '        Loop

    '        If Not IsNothing(sOutputString) And sOutputString <> "" Then
    '          ' chop off the last comma if there is one
    '          If Right(sOutputString, 1) = cCommaDelim Or _
    '            Right(sOutputString, 1) = cColonDelim Or _
    '            Right(sOutputString, 1) = cSemiColonDelim Then
    '            sOutputString = Trim(Mid(sOutputString, 1, Len(sOutputString) - 1)) ' remove the last comma, colon, or semicolon
    '          End If
    '        Else

    '          ' I also need to preserve any commas in the data
    '          If FindItemInData(inputString, cCommaDelim) Then
    '            ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
    '            inputString = CleanUserData(inputString, cCommaDelim, cImbedComa, False)
    '          End If

    '          ' clean out the EXCEL 03 Character
    '          inputString = CleanUserData(inputString, EXCEL2003CHAR, cEmptyString, False)

    '          ' clean out the CRLF
    '          inputString = CleanUserData(inputString, vbCrLf, cEmptyString, False)

    '          sOutputString = Trim(inputString)

    '        End If

    '      Else
    '        sOutputString = Trim(inputString)
    '      End If 'n_loop > 0

    '    End If ' not bIsTextAreaInput

    '    ' chop off the last comma if there is one
    '    If Right(sOutputString, 1) = cCommaDelim Or _
    '      Right(sOutputString, 1) = cColonDelim Or _
    '      Right(sOutputString, 1) = cSemiColonDelim Then
    '      sOutputString = Trim(Mid(sOutputString, 1, Len(sOutputString) - 1)) ' remove the last comma, colon, or semicolon
    '    End If

    '    CleanUserData = sOutputString

    '  End If ' inputString <> ""

    'End Function

#End Region

#Region "Compare View Functions"

    Public Function Get_JETNET_AC_ID_FROM_CLIENT(ByVal real_ac_id As String, ByVal is_reverse As Boolean) As Long
        Get_JETNET_AC_ID_FROM_CLIENT = 0
        Dim atemptable As New DataTable

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing
        Dim sQuery As String = ""

        Try

            If is_reverse Then
                sQuery = "select cliaircraft_id from client_aircraft where cliaircraft_jetnet_ac_id = " & real_ac_id
            Else
                sQuery = "select cliaircraft_jetnet_ac_id from client_aircraft where cliaircraft_id = " & real_ac_id
            End If


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in Get_CRM_VIEW_Prospects load datatable" + constrExc.Message
            End Try


            If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                    For Each r As DataRow In atemptable.Rows

                        If is_reverse Then
                            Get_JETNET_AC_ID_FROM_CLIENT = r("cliaircraft_id")
                        Else
                            Get_JETNET_AC_ID_FROM_CLIENT = r("cliaircraft_jetnet_ac_id")
                        End If

                    Next
                End If
            End If

            atemptable.Dispose()

        Catch ex As Exception
            Return Nothing
            aError = "Error in Get_CRM_VIEW_Prospects()" + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Function

    Public Function Get_Compare_Query(ByVal sQuery As String, ByVal title_of_query As String) As DataTable
        Dim atemptable As New DataTable

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing

        Try

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & title_of_query & " (" & Date.Now & ")</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString
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
                aError = "Error in Get_CRM_VIEW_Prospects load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing
            aError = "Error in Get_CRM_VIEW_Prospects()" + ex.Message
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

    Public Function Create_Distinct_Export_Type(ByVal type_of_export_check As CheckBoxList, ByRef session_for_types As String) As DataTable
        Dim atemptable As New DataTable

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing
        Dim sQuery As String = ""
        Dim type_of_export As String = ""

        Try


            If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then   '  Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString
            Else
                SqlConn.ConnectionString = HttpContext.Current.Application.Item("crmMasterDatabase")
            End If


            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60


            For Each li In type_of_export_check.Items
                If li.Selected Then
                    Dim type As String = li.Value

                    If Trim(type_of_export) <> "" Then
                        type_of_export = type_of_export & ",'" & type & "'"
                    Else
                        type_of_export = type_of_export & "'" & type & "'"
                    End If


                End If
            Next

            session_for_types = type_of_export


            sQuery = " select distinct cliexp_sub_group from client_export "

            sQuery &= "  where cliexp_type in (" & type_of_export & ") "

            sQuery &= " order by cliexp_sub_group asc, cliexp_sort asc, cliexp_type asc"



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in Get_CRM_VIEW_Prospects load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing
            aError = "Error in Get_CRM_VIEW_Prospects()" + ex.Message
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

#End Region

#Region "CRM VIEW FUNCTIONS"


    Public Function Get_CRM_VIEW_Prospects(ByVal sQuery As String) As DataTable
        Dim atemptable As New DataTable

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing

        Try

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString
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
                aError = "Error in Get_CRM_VIEW_Prospects load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing
            aError = "Error in Get_CRM_VIEW_Prospects()" + ex.Message
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

    Public Function Get_CRM_VIEW_Function(ByVal sQuery As String, ByVal query_name As String) As DataTable
        Dim atemptable As New DataTable

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & query_name & "</b><br />" + sQuery.ToString

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
                aError = "Error in Get_CRM_VIEW_Function load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing
            aError = "Error in Get_CRM_VIEW_Function()" + ex.Message
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


#End Region

    Public Sub run_view_19_actions(ByVal NOTE_ID As Long, ByRef real_ac_id As Long, ByRef make_name As String, ByRef model_name As String, ByRef temp_amod_id As Long, ByRef rest_of_ac_string As String, ByRef chart_htmlString As String, ByRef session_client_ac_id As Long, ByRef model_reports_header_text As String, ByRef COMPLETED_OR_OPEN As String, ByRef localCriteria As viewSelectionCriteriaClass, ByRef make_model_label As String, ByRef bread As String, ByRef TabContainer1 As AjaxControlToolkit.TabContainer, ByRef label_behind As String, ByRef page_title As String, ByRef last_save As String, ByRef jetnet_Ac_id As Long, ByRef edit_label_text As String, ByRef sold_time_drop As DropDownList, ByRef show_estimates As CheckBox, ByRef filter_years As DropDownList, ByRef aftt_search As DropDownList, ByRef use_jetnet_data As CheckBox, ByVal page1 As Page, ByRef used_of_used As CheckBox, ByRef year_of As String, ByRef aftt_of As String, Optional ByVal runModelOnly As Boolean = False)

        Dim temp_ac_id As Long = 0

        If Not IsNothing(TabContainer1.ActiveTab) Then
            model_reports_header_text = "Selection: " & TabContainer1.ActiveTab.HeaderText.ToString
        Else
            model_reports_header_text = ""
        End If

        If Not IsNothing(page1) Then
            If Not page1.IsPostBack Then
                label_behind = get_valuation_details(NOTE_ID, COMPLETED_OR_OPEN, session_client_ac_id, jetnet_Ac_id, last_save, edit_label_text, sold_time_drop, show_estimates, filter_years, aftt_search, use_jetnet_data, used_of_used)
            End If
        Else
            label_behind = get_valuation_details(NOTE_ID, COMPLETED_OR_OPEN, session_client_ac_id, jetnet_Ac_id, last_save, edit_label_text, sold_time_drop, show_estimates, filter_years, aftt_search, use_jetnet_data, used_of_used)
        End If


        temp_ac_id = session_client_ac_id


        real_ac_id = Get_JETNET_AC_ID_FROM_CLIENT(temp_ac_id, False)



        Get_AC_MAKE_MODEL(real_ac_id, make_name, model_name, temp_amod_id, rest_of_ac_string, "", year_of, aftt_of)
        localCriteria.ViewCriteriaAircraftID = real_ac_id
        localCriteria.ViewCriteriaAircraftMake = make_name
        localCriteria.ViewCriteriaAircraftModel = model_name
        localCriteria.ViewCriteriaAmodID = temp_amod_id



        make_model_label = rest_of_ac_string
        If runModelOnly = False Then
            bread = "Market Value Analysis: " & rest_of_ac_string
            page_title = "Market Value Analysis: " & rest_of_ac_string
        Else
            bread = "Market Value Analysis: " & make_name & " " & model_name
            page_title = "Market Value Analysis: " & make_name & " " & model_name
        End If
        chart_htmlString = ""


    End Sub

    Public Sub Build_Compare_View(ByVal temp_amod_id As Long, ByVal make_name As String, ByVal model_name As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal rep_id As String, ByVal NOTE_ID As Long, ByRef TabContainer1 As AjaxControlToolkit.TabContainer, ByRef tabs_container As AjaxControlToolkit.TabContainer, ByRef localCriteria As viewSelectionCriteriaClass, ByRef session_client_Ac_id As Long, ByRef reports_text As String, ByRef compare_view_current_label As Label, ByRef compare_view_sold_label As Label, ByRef make_model_label As String, ByRef bread As String, ByRef client_Ac_id As String, ByRef model_reports_header_text As String, ByRef label_behind As String, ByRef last_save As String, ByVal uses_links As Boolean, ByRef page_title As String, ByRef session_jetnet_ac_id As String, ByRef edit_label_text As String, ByRef sold_time_drop As DropDownList, ByRef show_estimates As CheckBox, ByRef filter_years As DropDownList, ByRef aftt_search As DropDownList, ByRef use_jetnet_data As CheckBox, ByVal page1 As Page, ByRef used_of_used As CheckBox, ByRef year_of_current As String, ByRef aftt_of_current As String, Optional ByVal RunModelOnValueOnly As Boolean = False)

        Dim header_string_title As String = ""
        Dim header_string As String = ""
        Dim header_string_text As String = ""
        Dim temp_ac_id As Long = 0
        Dim type_of_search As String = ""
        Dim results_table As New DataTable
        Dim ac_ser_no As String = ""
        Dim rest_of_ac_string As String = ""
        Dim query As String = ""
        Dim htmlOut As New StringBuilder
        Dim tempStr As String = ""
        Dim sHtmlMarketStatus As String = ""
        Dim CompanyLocation As String = ""
        Dim CompanyTitle As String = ""
        Dim timeline_search As String = ""
        Dim sort_by_search As String = ""
        Dim order_by_text As String = ""
        Dim year_text As String = ""
        Dim year_special As String = ""
        Dim imgDisplayFolder As String = ""
        Dim PictureTable As New DataTable
        Dim count_of_records As Long = 0
        Dim include_not_text As String = ""
        Dim temp_prospect As String = ""
        Dim toggleRowColor As Boolean = False

        Dim current_row As Integer = 0
        Dim print_array(TOTAL_AVAILABLE_TO_COMPARE) As String
        Dim i As Integer = 0
        Dim table_start_add As String = ""
        Dim h As Integer = 0
        Dim k As Integer = 0
        Dim select_string As String = ""

        Dim foundChild As New DropDownList
        Dim dimsearch_alt_comp_name As Boolean = False 'this is a special case in which we have to look and see if the alt name has been set to true. This sets as it iterates to true if checked so 
        'we can use it later on the comp name textbox
        Dim DoNotIncludeOverdue As Boolean = False 'this is yet another special case where we look to see if Do Not Include Overdue has been checked
        Dim DoNotIncludeRelationships As Boolean = False
        Dim db_fields(100) As String
        Dim name_fields(100) As String
        Dim desc_fields(100) As String
        Dim jetnet_trans_id(100) As String
        Dim client_trans_id(100) As String
        Dim field_array_count As Integer = 0
        Dim real_ac_id As Long = 0
        Dim jetnet_ac_id_query As String = ""
        Dim chart_htmlString As String = ""
        Dim default_spot_count As Integer = 0
        Dim COMPLETED_OR_OPEN As String = "O"
        Dim insert_string As String = ""
        Dim jetnet_Ac_id As Long = 0


        Try




            'Call set_up_dynamic_text()


            '   Me.update_compare.Visible = True
            'Me.update_compare2.Visible = True


            ' '' I NEED TO DO THIS ON A SAVE
            'For Each c As Control In trends_tab.Controls
            '    If TypeOf c Is TextBox Then
            '        Dim temporaryTextBox As TextBox = c
            '        Dim comparedIDString As String = c.ID.ToString

            '        If temporaryTextBox.Text <> "" Then

            '            If temporaryTextBox.ID = "TEXTBOX3" Then
            '                current_row = current_row
            '            End If

            '        End If


            '        temporaryTextBox.Dispose()
            '    End If
            'Next

            'tabs_container.ActiveTabIndex = 6


            For i = 0 To 10
                array_field_0(i) = ""
                array_field_1(i) = ""
                array_field_2(i) = ""
                array_field_3(i) = ""
                array_field_4(i) = ""
                array_field_5(i) = ""
                array_field_6(i) = ""
                array_field_7(i) = ""
                array_field_8(i) = ""
                array_field_9(i) = ""
                array_field_10(i) = ""
                array_field_11(i) = ""
                array_field_12(i) = ""
                array_field_13(i) = ""
                array_field_14(i) = ""
                array_field_15(i) = ""
                array_field_16(i) = ""
                array_field_17(i) = ""
                array_field_18(i) = ""
                array_field_19(i) = ""
                array_field_20(i) = ""
                array_field_21(i) = ""
                array_field_22(i) = ""
                array_field_23(i) = ""
                array_field_24(i) = ""
                array_field_25(i) = ""
                array_field_26(i) = ""
                array_field_27(i) = ""
                array_field_28(i) = ""
                array_field_29(i) = ""
                array_field_30(i) = ""
                array_field_31(i) = ""
                array_field_32(i) = ""
                array_field_33(i) = ""
                array_field_34(i) = ""
                array_field_35(i) = ""
            Next



            If TabContainer1.ActiveTabIndex = 2 Then
                Call get_valuation_items_into_array(NOTE_ID, db_fields, name_fields, field_array_count, select_string, count_of_records, "active", "P", temp_ac_id, desc_fields, default_spot_count, jetnet_trans_id, client_trans_id, rep_id, COMPLETED_OR_OPEN, localCriteria, aclsData_Temp)

                If CDbl(temp_ac_id) = 0 Then
                    Call get_valuation_details(NOTE_ID, COMPLETED_OR_OPEN, temp_ac_id, jetnet_Ac_id, last_save, edit_label_text, sold_time_drop, show_estimates, filter_years, aftt_search, use_jetnet_data, used_of_used)
                    ' insert it 
                    Call aclsData_Temp.Insert_Primary_Comparable_Record(NOTE_ID, temp_ac_id, jetnet_Ac_id)
                    ' re-run
                    Call get_valuation_items_into_array(NOTE_ID, db_fields, name_fields, field_array_count, select_string, count_of_records, "active", "P", temp_ac_id, desc_fields, default_spot_count, jetnet_trans_id, client_trans_id, rep_id, COMPLETED_OR_OPEN, localCriteria, aclsData_Temp)
                End If
                real_ac_id = temp_ac_id
                session_client_Ac_id = real_ac_id
                field_array_count = 0 ' to reset so that no need to do anything else
                Call get_valuation_items_into_array(NOTE_ID, db_fields, name_fields, field_array_count, select_string, count_of_records, "active", "C", temp_ac_id, desc_fields, default_spot_count, jetnet_trans_id, client_trans_id, rep_id, COMPLETED_OR_OPEN, localCriteria, aclsData_Temp)
            ElseIf TabContainer1.ActiveTabIndex = 6 Then
                Call get_valuation_items_into_array(NOTE_ID, db_fields, name_fields, field_array_count, select_string, count_of_records, "sold", "P", temp_ac_id, desc_fields, default_spot_count, jetnet_trans_id, client_trans_id, rep_id, COMPLETED_OR_OPEN, localCriteria, aclsData_Temp)
                real_ac_id = temp_ac_id
                session_client_Ac_id = real_ac_id
                field_array_count = 0 ' to reset so that no need to do anything else
                Call get_valuation_items_into_array(NOTE_ID, db_fields, name_fields, field_array_count, select_string, count_of_records, "sold", "C", temp_ac_id, desc_fields, default_spot_count, jetnet_trans_id, client_trans_id, rep_id, COMPLETED_OR_OPEN, localCriteria, aclsData_Temp)
            End If



            If TabContainer1.ActiveTabIndex = 2 Then
                Call add_text_to_table(compare_view_current_label, db_fields, name_fields, field_array_count, select_string, count_of_records, desc_fields, default_spot_count, TabContainer1.ActiveTabIndex, jetnet_trans_id, client_trans_id, NOTE_ID, False, COMPLETED_OR_OPEN, aclsData_Temp, localCriteria, rep_id)
                compare_view_current_label.Visible = True
            ElseIf TabContainer1.ActiveTabIndex = 6 Then
                Call add_text_to_table(compare_view_sold_label, db_fields, name_fields, field_array_count, select_string, count_of_records, desc_fields, default_spot_count, TabContainer1.ActiveTabIndex, jetnet_trans_id, client_trans_id, NOTE_ID, False, COMPLETED_OR_OPEN, aclsData_Temp, localCriteria, rep_id)
                compare_view_sold_label.Visible = True
            End If

            If TabContainer1.ActiveTabIndex = 2 Then
                CommonAircraftFunctions.Build_String_To_HTML(compare_view_current_label.Text, HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & NOTE_ID.ToString & "_market_comparables_current.xls")
            ElseIf TabContainer1.ActiveTabIndex = 6 Then
                CommonAircraftFunctions.Build_String_To_HTML(compare_view_sold_label.Text, HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & NOTE_ID.ToString & "_market_comparables_sold.xls")
            End If

            If TabContainer1.ActiveTabIndex = 2 Then
                Call add_text_to_table(compare_view_current_label, db_fields, name_fields, field_array_count, select_string, count_of_records, desc_fields, default_spot_count, TabContainer1.ActiveTabIndex, jetnet_trans_id, client_trans_id, NOTE_ID, True, COMPLETED_OR_OPEN, aclsData_Temp, localCriteria, rep_id)
            ElseIf TabContainer1.ActiveTabIndex = 6 Then
                Call add_text_to_table(compare_view_sold_label, db_fields, name_fields, field_array_count, select_string, count_of_records, desc_fields, default_spot_count, TabContainer1.ActiveTabIndex, jetnet_trans_id, client_trans_id, NOTE_ID, True, COMPLETED_OR_OPEN, aclsData_Temp, localCriteria, rep_id)
            End If

            'If uses_links = True Then
            If TabContainer1.ActiveTabIndex = 2 Then
                compare_view_current_label.Text = "<a href='/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & NOTE_ID.ToString & "_market_comparables_current.xls' target='blank'>Export To Excel</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br>" & compare_view_current_label.Text
            ElseIf TabContainer1.ActiveTabIndex = 6 Then
                compare_view_sold_label.Text = "<a href='/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & NOTE_ID.ToString & "_market_comparables_sold.xls' target='blank'>Export To Excel</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br>" & compare_view_sold_label.Text
            End If
            '  End If


            Call run_view_19_actions(NOTE_ID, real_ac_id, make_name, model_name, temp_amod_id, rest_of_ac_string, chart_htmlString, session_client_Ac_id, model_reports_header_text, COMPLETED_OR_OPEN, localCriteria, make_model_label, bread, TabContainer1, label_behind, page_title, last_save, session_jetnet_ac_id, edit_label_text, sold_time_drop, show_estimates, filter_years, aftt_search, use_jetnet_data, page1, used_of_used, year_of_current, aftt_of_current, RunModelOnValueOnly)


            make_model_label = rest_of_ac_string
            bread = "Market Value Analysis: " & rest_of_ac_string
            page_title = "Market Value Analysis: " & rest_of_ac_string


            '  Me.view_reports_label.Text = "Displays a side by side of view of aircraft for sale that have been designated as 'comparable' to my selected aircraft.   The graph below shows my current aircraft value(s) with respect to the value of other individual aircraft that have specifically been identified as comparable to mine."

            ' If TabContainer1.ActiveTabIndex = 1 Then
            'Call Build_Analytics_Charts(localCriteria, chart_htmlString, NOTE_ID, "Current Market", Nothing)
            ' Me.view_reports_label.Text &= chart_htmlString
            ' End If
            ' set_up_ac_model_pic(real_ac_id, temp_amod_id, make_name, model_name, ac_ser_no, type_of_search, rest_of_ac_string, imgDisplayFolder)


        Catch ex As Exception

        Finally

        End Try


        ' Call turn_on_off_tabs_compare_view(NOTE_ID)


    End Sub

    Public Function get_valuation_details(ByVal NOTE_ID As Long, ByRef completed_or_open As String, ByRef session_client_ac_id As Long, ByRef session_jetnet_ac_id As Long, ByRef LAST_SAVE_DATE As String, ByRef edit_label_text As String, ByRef sold_time_drop As DropDownList, ByRef show_estimates As CheckBox, ByRef filter_years As DropDownList, ByRef aftt_search As DropDownList, ByRef use_jetnet_data As CheckBox, ByRef used_of_used As CheckBox) As String
        get_valuation_details = ""

        Dim Query As String = ""
        Dim results_table As New DataTable
        Dim results_table_inner As New DataTable
        Dim temp_string As String = ""
        Dim action_date As String = ""
        Dim temp_note As String = ""
        Dim ac_info As String = ""
        Dim comp_info As String = ""
        Dim contact_info As String = ""
        Dim Query2 As String = ""
        Dim string_open_closed As String = ""

        Query = ""
        Query = Query & " SELECT lnote_id, lnote_action_date, lnote_note, lnote_opportunity_status, "
        Query = Query & " lnote_client_ac_id, lnote_jetnet_ac_id, lnote_client_comp_id, lnote_client_contact_id, "
        Query = Query & " cliamod_make_name, cliamod_model_name, cliaircraft_year_mfr, cliaircraft_ser_nbr, "
        Query = Query & " clicomp_name, clicomp_address1, clicomp_address2, clicomp_city, clicomp_state, "
        Query = Query & " clicomp_zip_code, clicomp_country, clicontact_first_name, clicontact_last_name, "
        Query = Query & " clicontact_title, clicontact_email_address " ', clicomp_email_address, clicomp_web_address "

        Query = Query & ", lnote_value_sold_time, lnote_value_show_estimates, lnote_value_filter_years, lnote_value_filter_aftt, lnote_value_use_jetnet, lnote_value_used_ac_only"

        Query = Query & " FROM local_notes "
        Query = Query & " inner join client_aircraft on lnote_client_ac_id=cliaircraft_id "
        Query = Query & " inner join client_aircraft_model on cliaircraft_cliamod_id = cliamod_id "
        Query = Query & " left outer join client_company on lnote_client_comp_id=clicomp_id "
        Query = Query & " left outer join client_contact on lnote_client_contact_id=clicontact_id "
        Query = Query & " WHERE lnote_id = " & NOTE_ID & " "

        Try

            results_table = Get_Compare_Query(Query, "GET VALUATION DETAILS")

            If Not IsNothing(results_table) Then

                '  If Trim(HttpContext.Current.Session.Item("Show_Estimated")) = "Y" Then
                'temp_string &= "<div style='height:75px; width:355px; overflow: auto;'>"
                ' Else
                '   temp_string &= "<div style='height:200px; width:355px; overflow: auto;'>"
                ' End If


                temp_string &= "<table cellpadding=""3"" cellspacing=""5"" bgcolor='#E8E8E8'>"


                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Trim(ac_info) = "" Then
                            session_client_ac_id = r("lnote_client_ac_id")
                            session_jetnet_ac_id = r("lnote_jetnet_ac_id")
                        End If

                        If Not IsDBNull(r("lnote_action_date")) Then
                            action_date = Trim(r("lnote_action_date"))
                        End If



                        If Not IsDBNull(r("lnote_note")) Then
                            If Trim(r("lnote_note")) <> "" Then
                                temp_note = "<br>" & Trim(r("lnote_note"))
                            End If
                        End If

                        '------------------- AC INFO SECTION----------------------
                        'If Not IsDBNull(r("lnote_client_ac_id")) Then
                        ' action_date = Trim(r("lnote_client_ac_id"))
                        ' End If
                        ac_info = "<b>Aircraft: </b>"
                        If Not IsDBNull(r("cliaircraft_year_mfr")) Then
                            ac_info &= Trim(r("cliaircraft_year_mfr")) & " "
                        End If

                        If Not IsDBNull(r("cliamod_make_name")) Then
                            ac_info &= Trim(r("cliamod_make_name")) & " "
                        End If

                        If Not IsDBNull(r("cliamod_model_name")) Then
                            ac_info &= Trim(r("cliamod_model_name")) & " "
                        End If

                        If Not IsDBNull(r("cliaircraft_ser_nbr")) Then
                            ac_info &= ", Ser# " & Trim(r("cliaircraft_ser_nbr"))
                        End If

                        '------------------- AC INFO SECTION----------------------


                        '--------------- COMP INFO-------------------------
                        If Not IsDBNull(r("clicomp_name")) Then
                            comp_info = "<table width='100%'><tr valign='top'><td><b>Company/Customer: </b><br>"



                            comp_info &= Trim(r("clicomp_name")) & "<Br>"


                            If Not IsDBNull(r("clicomp_address1")) Then
                                If Trim(r("clicomp_address1")) <> "" Then
                                    comp_info &= Trim(r("clicomp_address1")) & "<Br>"
                                End If
                            End If

                            If Not IsDBNull(r("clicomp_address2")) Then
                                If Trim(r("clicomp_address2")) <> "" Then
                                    comp_info &= Trim(r("clicomp_address2")) & "<Br>"
                                End If
                            End If

                            If Not IsDBNull(r("clicomp_city")) Then
                                comp_info &= Trim(r("clicomp_city")) & " "
                            End If

                            If Not IsDBNull(r("clicomp_state")) Then
                                If Not IsDBNull(r("clicomp_city")) Then
                                    comp_info &= ", "
                                End If
                                comp_info &= Trim(r("clicomp_state")) & " "
                                If Not IsDBNull(r("clicomp_zip_code")) Then
                                    comp_info &= Trim(r("clicomp_zip_code")) & " "
                                End If
                            End If

                            If Not IsDBNull(r("clicomp_country")) Then
                                comp_info &= Trim(r("clicomp_country")) & ""
                            End If

                            If Not IsDBNull(r("clicomp_state")) Then
                                comp_info &= "<br>"
                            ElseIf Not IsDBNull(r("clicomp_city")) Then
                                comp_info &= "<br>"
                            ElseIf Not IsDBNull(r("clicomp_country")) Then
                                comp_info &= "<br>"
                            End If



                            If Not IsDBNull(r("lnote_client_comp_id")) Then
                                If r("lnote_client_comp_id") > 0 Then
                                    Query2 = " select  clipnum_number from client_phone_numbers where clipnum_comp_id = " & r("lnote_client_comp_id") & " and clipnum_contact_id = 0 and clipnum_type = 'Office' "
                                    results_table_inner = Get_Compare_Query(Query2, "GET VALUATION DETAILS - COMPANY PHONE")
                                    If Not IsNothing(results_table_inner) Then
                                        If results_table_inner.Rows.Count > 0 Then
                                            For Each x As DataRow In results_table_inner.Rows
                                                comp_info &= "<b>Office: </b>" & x.Item("clipnum_number") & "<Br>"
                                            Next
                                        End If
                                    End If
                                End If
                            End If

                            comp_info &= "</td></tr></table>"
                        End If
                        '--------------- COMP INFO-------------------------


                        '------------- CONTACT INFO-------------------------

                        contact_info = ""

                        If Not IsDBNull(r("clicontact_first_name")) Then
                            contact_info &= Trim(r("clicontact_first_name")) & " "
                        End If

                        If Not IsDBNull(r("clicontact_last_name")) Then
                            contact_info &= Trim(r("clicontact_last_name")) & "<br>"
                        End If

                        If Not IsDBNull(r("clicontact_title")) Then
                            contact_info &= Trim(r("clicontact_title")) & "<br>"
                        End If

                        If Not IsDBNull(r("clicontact_email_address")) Then
                            contact_info &= Trim(r("clicontact_email_address")) & "<br>"
                        End If

                        If Not IsDBNull(r("lnote_client_contact_id")) Then
                            If r("lnote_client_contact_id") > 0 Then
                                Query2 = " select  clipnum_number from client_phone_numbers where  clipnum_contact_id = " & r("lnote_client_contact_id") & " and clipnum_comp_id = 0 and clipnum_type = 'Office' "
                                results_table_inner = Get_Compare_Query(Query2, "GET VALUATION DETAILS - CONTACT PHONE")
                                If Not IsNothing(results_table_inner) Then
                                    If results_table_inner.Rows.Count > 0 Then
                                        For Each x As DataRow In results_table_inner.Rows
                                            comp_info &= "<b>Office: </b>" & x.Item("clipnum_number") & "<Br>"
                                        Next
                                    End If
                                End If
                            End If
                        End If

                        If Trim(contact_info) <> "" Then
                            contact_info = "<table width='100%'><tr valign='top'><td><b>Contact: </b><br>" & contact_info
                            contact_info &= "</td></tr></table>"
                        End If

                        '------------- CONTACT INFO-------------------------





                        '" , , , , "
                        ' , , , , , "
                        '  , , , , "
                        '   "


                        If Not IsDBNull(r("lnote_opportunity_status")) Then
                            string_open_closed = Trim(r("lnote_opportunity_status"))
                            completed_or_open = string_open_closed
                        End If


                        If Not IsDBNull(r("lnote_action_date")) Then
                            If Trim(completed_or_open) <> "T" Then
                                action_date = "<b>Last Update: </b>" & Trim(r("lnote_action_date"))
                            Else
                                action_date = ""
                            End If
                            If Trim(completed_or_open) = "C" Then
                                LAST_SAVE_DATE = Trim(r("lnote_action_date"))
                            End If
                        End If


                        If Not IsDBNull(r("lnote_value_sold_time")) Then
                            If Trim(r("lnote_value_sold_time")) <> "" Then
                                sold_time_drop.SelectedValue = CDbl(Trim(r("lnote_value_sold_time")))
                            End If
                        End If


                        If Not IsDBNull(r("lnote_value_show_estimates")) Then
                            If Trim(r("lnote_value_show_estimates")) = "Y" Then
                                show_estimates.Checked = True
                            Else
                                show_estimates.Checked = False
                            End If
                        Else
                            show_estimates.Checked = False
                        End If

                        If Not IsDBNull(r("lnote_value_filter_years")) Then
                            If Trim(r("lnote_value_filter_years")) <> "" Then
                                filter_years.SelectedValue = CDbl(Trim(r("lnote_value_filter_years")))
                            End If
                        End If

                        If Not IsDBNull(r("lnote_value_filter_aftt")) Then
                            If Trim(r("lnote_value_filter_aftt")) <> "" Then
                                aftt_search.SelectedValue = CDbl(Trim(r("lnote_value_filter_aftt")))
                            End If
                        End If

                        If Not IsDBNull(r("lnote_value_use_jetnet")) Then
                            If Trim(r("lnote_value_use_jetnet")) = "Y" Then
                                use_jetnet_data.Checked = True
                            Else
                                use_jetnet_data.Checked = False
                            End If
                        Else
                            use_jetnet_data.Checked = False
                        End If

                        If Not IsDBNull(r("lnote_value_used_ac_only")) Then
                            If Trim(r("lnote_value_used_ac_only")) = "Y" Then
                                used_of_used.Checked = True
                            Else
                                used_of_used.Checked = False
                            End If
                        Else
                            used_of_used.Checked = False
                        End If



                    Next
                End If




                temp_string &= "<tr valign='top'>"
                ' temp_string &= "<td align='left'>" & ac_info & "</td>"




                If Trim(string_open_closed) = "O" Then
                    temp_string &= "<td align='left' class=""date"">" & action_date & "&nbsp;</td>"
                    string_open_closed = "<b>Status:</b> Open"
                    '   Me.update_compare2.Visible = True
                    temp_string &= "</tr><tr><td align='left'>"
                ElseIf Trim(string_open_closed) = "C" Then
                    temp_string &= "<td align='left' class=""date"">" & action_date & "&nbsp;</td>"
                    string_open_closed = "<b>Status:</b> Closed"
                    '  Me.update_compare2.Visible = False
                    temp_string &= "</tr><tr><td align='left'>"
                ElseIf Trim(string_open_closed) = "T" Then
                    ' string_open_closed = "<b>Status:</b> Temporary<br>"
                    '   string_open_closed = "This view shows the current Market Value Analysis for this aircraft. If you would like to save this active analysis associated with a specific customer or close the analysis and save if for historical reference then click on the Edit link below."
                    string_open_closed = "If you would like to save this analysis for a specific customer or for historical reference then click on the Edit link below"
                    temp_string &= "<td align='left' colspan='2' width='99%'>"
                End If

                temp_string &= string_open_closed & "</td>"


                temp_string &= "</tr>"



                If Trim(comp_info) <> "" Then
                    temp_string &= "<tr valign='top'>"
                    temp_string &= "<td align='left' colspan='2'>" & comp_info & "</td>"
                    temp_string &= "</tr>"
                End If


                If Trim(contact_info) <> "" Then
                    temp_string &= "<tr valign='top'>"
                    temp_string &= "<td align='left' colspan='2'>" & contact_info & "</td>"
                    temp_string &= "</tr>"
                End If


                temp_string &= "<tr valign='top'>"
                temp_string &= "<td align='left' colspan='2'>" & temp_note & "</td>"
                temp_string &= "</tr>"



                temp_string &= "</table>"
                ' temp_string &= "</div>"


                edit_label_text = "<table cellspacing='0' cellpadding='0'>"
                edit_label_text &= "<tr valign='top'>"
                edit_label_text &= "<td align='left' colspan='2'>"
                edit_label_text &= "<a href='#' onclick=""window.open('/edit_note.aspx?action=edit&type=valuation&id=" & NOTE_ID & "&refreshing=view&nWin=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                '<a href='edit_note.aspx?action=edit&type=valuation&id=" & NOTE_ID & "' target='_blank'>
                edit_label_text &= "Edit Analysis</a></td>"
                edit_label_text &= "</tr></table>"


            End If

            get_valuation_details = temp_string

        Catch ex As Exception
        Finally
        End Try


    End Function

    Public Sub add_text_to_table(ByRef temp_label As Label, ByRef db_fields As Array, ByRef name_fields As Array, ByRef field_array_count As Integer, ByRef select_string As String, ByRef count_of_records As Integer, ByRef desc_fields As Array, ByVal default_spot_count As Integer, ByVal tab1 As Integer, ByRef jetnet_trans_id As Array, ByRef client_trans_id As Array, ByVal note_id As Long, ByVal uses_links As Boolean, ByVal COMPLETED_OR_OPEN As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal localCriteria As viewSelectionCriteriaClass, ByVal rep_id As String)

        Dim temp_text As String = ""
        Dim array_spot As Integer = 0
        Dim i As Integer = 0
        Dim temp_bold_string As String = "<b>"
        Dim temp_bold_string_end As String = "</b>"
        Dim TemporaryTable As New DataTable
        Dim temp_value_id As Long = 0
        Dim htmlStandardFeatures As String = ""
        Dim SoldComparablesTab As Boolean = False
        Dim MarketComparablesTab As Boolean = False

        'In order to tell what we're displaying,
        'Either the sold market comparables tab (historical/view_documents_label1)
        'Or the Market Comparables tab (present/compare_view_sold_label)
        'We're going to check and see what label we're writing to.

        If temp_label.ID.ToString.ToUpper = "VIEW_DOCUMENTS_LABEL1" Then
            SoldComparablesTab = True
        Else
            MarketComparablesTab = True
        End If

        Try


            temp_text = "<table border='1' cellspacing='0' cellpadding='5'>"
            temp_text &= "<tr bgcolor='gray'><td bgcolor='gray'><font color='white'>Comparing</font></td>"

            For array_spot = 0 To count_of_records - 1
                temp_text &= "<td bgcolor='gray'>"
                'temp_text &= "<a href='#' style='color: #FFFFFF' onclick=""javascript:load('edit.aspx?action=edit&type=aircraft&ac_ID=" & array_ac_id(array_spot) & "&source=CLIENT','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"" alt='" & desc_fields(array_spot) & "' title='" & desc_fields(array_spot) & "'/>"



                If uses_links = True Then
                    If tab1 = 1 Then
                        If Trim(COMPLETED_OR_OPEN) <> "C" Then
                            temp_text &= "<a href='#' alt='" & desc_fields(array_spot) & "' title='" & desc_fields(array_spot) & "' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & array_ac_id(array_spot) & "&source=CLIENT&from=view&viewNOTEID=" & Trim(note_id) & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                        End If
                    Else
                        ' if its sold - 
                        If Trim(COMPLETED_OR_OPEN) <> "C" Then
                            If array_spot = 0 Then
                                temp_text &= "<a href='#' alt='" & desc_fields(array_spot) & "' title='" & desc_fields(array_spot) & "' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & array_ac_id(array_spot) & "&source=CLIENT&from=view&viewNOTEID=" & Trim(note_id) & "&activetab=2','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                            Else
                                ' trans link
                                If SoldComparablesTab = True Then
                                    temp_text &= "<a href='#' alt='" & desc_fields(array_spot) & "' title='" & desc_fields(array_spot) & "' onclick=""window.open('/edit.aspx?action=edit&type=transaction&cli_trans=" & client_trans_id(array_spot) & "&trans=" & jetnet_trans_id(array_spot) & "&autoCheckTransaction=true&ac_ID=" & array_ac_id(array_spot) & "&source=CLIENT&from=view&viewNOTEID=" & Trim(note_id) & "&activetab=5&market=" & MarketComparablesTab.ToString & "&sold=" & SoldComparablesTab.ToString & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                                Else
                                    temp_text &= "<a href='#' alt='" & desc_fields(array_spot) & "' title='" & desc_fields(array_spot) & "' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & array_ac_id(array_spot) & "&source=CLIENT&from=view&viewNOTEID=" & Trim(note_id) & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                                End If
                            End If
                        Else
                            ' if its completed, no link
                        End If
                    End If
                End If

                ' temp_text &= "<a href='#' style='color: #FFFFFF' onclick=""javascript:load('edit.aspx?action=edit&type=aircraft&ac_ID=" & array_ac_id(array_spot) & "&source=CLIENT','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"" alt='" & desc_fields(array_spot) & "' title='" & desc_fields(array_spot) & "'/>"
                '   temp_text &= "<a href='#' style='color: #FFFFFF' onclick=""javascript:load('edit.aspx?action=edit&type=aircraft&ac_ID=" & array_ac_id(array_spot) & "&source=CLIENT','scrollbars=yes,menubar=no,height=500,width=1200,resizable=yes,toolbar=no,location=no,status=no');return false;"" alt='" & desc_fields(array_spot) & "' title='" & desc_fields(array_spot) & "'/>"
                '   temp_text &= "<a href='http://crmwebclient/edit.aspx?action=edit&type=aircraft&ac_ID=" & array_ac_id(array_spot) & "&source=CLIENT' style='color: #FFFFFF' target='_blank' alt='" & desc_fields(array_spot) & "' title='" & desc_fields(array_spot) & "'>"


                temp_text &= "<font color='white'>"
                If array_spot = 0 Then
                    temp_text &= temp_bold_string & array_field_0(array_spot) & temp_bold_string_end
                Else
                    temp_text &= array_field_0(array_spot)
                End If

                temp_text &= "</font>"

                If Trim(COMPLETED_OR_OPEN) <> "C" Then
                    temp_text &= "</a>"
                End If



                If Trim(COMPLETED_OR_OPEN) <> "C" And array_spot > 0 Then  ' if its still open and not the original comparable ac
                    Try

                        If tab1 = 1 Then
                            TemporaryTable = aclsData_Temp.Find_Client_Analysis_Note(array_ac_id(array_spot), Trim(note_id), "F", 0)
                        Else
                            TemporaryTable = aclsData_Temp.Find_Client_Analysis_Note(array_ac_id(array_spot), Trim(note_id), "S", 0)
                        End If


                        If Not IsNothing(TemporaryTable) Then
                            If TemporaryTable.Rows.Count > 0 Then
                                temp_value_id = TemporaryTable.Rows(0).Item("clival_id")
                            End If
                        End If


                    Catch ex As Exception
                    Finally
                        If Not IsNothing(TemporaryTable) Then
                            TemporaryTable.Dispose()
                        End If
                    End Try

                    If uses_links = True Then
                        If tab1 = 2 Then ' changed from 1 
                            temp_text &= "&nbsp;<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & IIf(Not IsNothing(note_id), note_id, "") & "&activetab=9&id=" & temp_value_id.ToString & "&type_of=remove&compare_ac_id=" & array_ac_id(array_spot).ToString & "&sold_current=F&from_spot=1' alt='Delete this client aircraft as a current market comparable' title='Delete this client aircraft as a current market comparable'>"
                        Else
                            ' &jac_id=" & jetnet_Ac_id_ifclient & "

                            If array_ac_id(array_spot) = "0" Or array_ac_id(array_spot) = "" Then
                                temp_text &= "&nbsp;<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & IIf(Not IsNothing(note_id), note_id, "") & "&activetab=9&id=" & temp_value_id.ToString & "&type_of=remove&compare_ac_id=" & array_ac_id(array_spot).ToString & "&sold_current=S&trans_id=" & client_trans_id(array_spot).ToString & "&from_spot=6&jac_id=" & array_jac_id(array_spot) & "' alt='Delete this client aircraft as a Sold comparable' title='Delete this client aircraft as a Sold comparable'>"
                            Else
                                temp_text &= "&nbsp;<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & IIf(Not IsNothing(note_id), note_id, "") & "&activetab=9&id=" & temp_value_id.ToString & "&type_of=remove&compare_ac_id=" & array_ac_id(array_spot).ToString & "&sold_current=S&trans_id=" & client_trans_id(array_spot).ToString & "&from_spot=6' alt='Delete this client aircraft as a Sold comparable' title='Delete this client aircraft as a Sold comparable'>"
                            End If
                        End If


                        temp_text &= "<img src='images/delete_icon.png' width='12' title='" & desc_fields(array_spot) & "'>"
                        temp_text &= "</a>"
                    End If

                End If




                temp_text &= "</td>"
            Next
            temp_text &= "</tr>"



            ' 5 is for how many automatic fields we have added to front
            For i = 1 To field_array_count - 1
                temp_text &= "<tr valign='top'>"




                temp_text &= "<td bgcolor='gray'><font color='white'>" & name_fields(i)

                If Trim(COMPLETED_OR_OPEN) <> "C" Then
                    'If i >= default_spot_count Then

                    '  If Trim(Request("rep_id")) = "" Or Trim(Request("rep_id")) = "0" Then
                    '    If tab1 = 2 Then
                    '      temp_text &= " <a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & Trim(Request("noteID")) & "&activetab=8&type_of=remove&field_name=" & name_fields(i) & "&sold_current=S'>"
                    '    Else
                    '      temp_text &= " <a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & Trim(Request("noteID")) & "&activetab=8&type_of=remove&field_name=" & name_fields(i) & "&sold_current=F'>"
                    '    End If 

                    '    temp_text &= "(-)"
                    '    temp_text &= "</a> "
                    '  End If
                    'End If

                End If

                temp_text &= "</font></td>"


                For array_spot = 0 To count_of_records - 1

                    If Trim(db_fields(i)) <> "" Then

                        If array_spot = 0 Then
                            temp_bold_string = "<b>"
                            temp_bold_string_end = "</b>"
                        Else
                            temp_bold_string = ""
                            temp_bold_string_end = ""
                        End If

                        If i = 0 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_0(array_spot) & temp_bold_string_end
                        ElseIf i = 1 Then
                            'temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_1(array_spot) & temp_bold_string_end   ' this is for year mfr
                            If Trim(array_field_1(array_spot)) <> "" And IsNumeric(Trim(array_field_1(array_spot))) Then
                                If CDbl(array_field_1(array_spot) / 1000) > 0 Then
                                    temp_text &= "<td align='right' width='175'>" & temp_bold_string & "$" & FormatNumber(CDbl(array_field_1(array_spot) / 1000), 0) & "k" & temp_bold_string_end 'asking 
                                Else
                                    temp_text &= "<td align='right' width='175'>-" & temp_bold_string_end 'est/sold
                                End If
                            ElseIf Trim(array_field_1(array_spot)) <> "" Then
                                temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_1(array_spot) & temp_bold_string_end
                            Else
                                temp_text &= "<td align='right' width='175'>-" & temp_bold_string_end
                            End If
                        ElseIf i = 2 Then
                            'temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_2(array_spot) & temp_bold_string_end  'reg 
                            If Trim(array_field_2(array_spot)) <> "" And IsNumeric(Trim(array_field_2(array_spot))) Then
                                If CDbl(array_field_2(array_spot) / 1000) > 0 Then
                                    temp_text &= "<td align='right' width='175'>" & temp_bold_string & "$" & FormatNumber(CDbl(array_field_2(array_spot) / 1000), 0) & "k" & temp_bold_string_end  'take
                                Else
                                    temp_text &= "<td align='right' width='175'>-" & temp_bold_string_end 'est/sold
                                End If
                            ElseIf Trim(array_field_2(array_spot)) <> "" Then
                                temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_2(array_spot) & temp_bold_string_end
                            Else
                                temp_text &= "<td align='right' width='175'>-" & temp_bold_string_end
                            End If

                        ElseIf i = 3 Then
                            If Trim(array_field_3(array_spot)) <> "" And IsNumeric(Trim(array_field_3(array_spot))) Then
                                If CDbl(array_field_3(array_spot) / 1000) > 0 Then
                                    temp_text &= "<td align='right' width='175'>" & temp_bold_string & "$" & FormatNumber(CDbl(array_field_3(array_spot) / 1000), 0) & "k" & temp_bold_string_end 'est/sold
                                Else
                                    temp_text &= "<td align='right' width='175'>-" & temp_bold_string_end 'est/sold
                                End If
                            ElseIf Trim(array_field_3(array_spot)) <> "" Then
                                temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_3(array_spot) & temp_bold_string_end
                            Else
                                temp_text &= "<td align='right' width='175'>-" & temp_bold_string_end
                            End If
                        ElseIf i = 4 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_4(array_spot) & temp_bold_string_end
                        ElseIf i = 5 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_5(array_spot) & temp_bold_string_end
                        ElseIf i = 6 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_6(array_spot) & temp_bold_string_end
                            'If (Trim(name_fields(i)) = "Date" And array_field_6(array_spot) = "" And array_spot = 0) Then
                            '  temp_text &= "<td align='right' width='175'>" & temp_bold_string & Date.Now.Month & "/" & Date.Now.Day & "/" & Date.Now.Year & temp_bold_string_end
                            'Else
                            '  temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_6(array_spot) & temp_bold_string_end
                            'End If  
                        ElseIf i = 7 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_7(array_spot) & temp_bold_string_end
                        ElseIf i = 8 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_8(array_spot) & temp_bold_string_end
                        ElseIf i = 9 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_9(array_spot) & temp_bold_string_end
                        ElseIf i = 10 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_10(array_spot) & temp_bold_string_end
                        ElseIf i = 11 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_11(array_spot) & temp_bold_string_end
                        ElseIf i = 12 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_12(array_spot) & temp_bold_string_end
                        ElseIf i = 13 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_13(array_spot) & temp_bold_string_end
                        ElseIf i = 14 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_14(array_spot) & temp_bold_string_end
                        ElseIf i = 15 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_15(array_spot) & temp_bold_string_end
                        ElseIf i = 16 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_16(array_spot) & temp_bold_string_end
                        ElseIf i = 17 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_17(array_spot) & temp_bold_string_end
                        ElseIf i = 18 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_18(array_spot) & temp_bold_string_end
                        ElseIf i = 19 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_19(array_spot) & temp_bold_string_end
                        ElseIf i = 20 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_20(array_spot) & temp_bold_string_end
                        ElseIf i = 21 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_21(array_spot) & temp_bold_string_end
                        ElseIf i = 22 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_22(array_spot) & temp_bold_string_end
                        ElseIf i = 23 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_23(array_spot) & temp_bold_string_end
                        ElseIf i = 24 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_24(array_spot) & temp_bold_string_end
                        ElseIf i = 25 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_25(array_spot) & temp_bold_string_end
                        ElseIf i = 26 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_26(array_spot) & temp_bold_string_end
                        ElseIf i = 27 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_27(array_spot) & temp_bold_string_end
                        ElseIf i = 28 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_28(array_spot) & temp_bold_string_end
                        ElseIf i = 29 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_29(array_spot) & temp_bold_string_end
                        ElseIf i = 30 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_30(array_spot) & temp_bold_string_end
                        ElseIf i = 31 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_31(array_spot) & temp_bold_string_end
                        ElseIf i = 32 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_32(array_spot) & temp_bold_string_end
                        ElseIf i = 33 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_33(array_spot) & temp_bold_string_end
                        ElseIf i = 34 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_34(array_spot) & temp_bold_string_end
                        ElseIf i = 35 Then
                            temp_text &= "<td align='right' width='175'>" & temp_bold_string & array_field_35(array_spot) & temp_bold_string_end
                        End If

                        temp_text &= "</td>"
                    End If
                Next

                temp_text &= "</tr>"

            Next



            temp_text &= "</table>"

            If (Trim(rep_id) = "" Or Trim(rep_id) = "0") And tab1 = 2 Then ' switched from tab 1 
                temp_text &= ("<br><br><table id='modelForsaleViewBottomTable' width=""70%"" cellpadding=""4"" cellspacing=""0"">")
                temp_text &= ("<tr><td align=""left"" valign=""top"">")

                display_standard_model_features(localCriteria, htmlStandardFeatures)
                temp_text &= (htmlStandardFeatures)

                temp_text &= ("</td></tr></table>")
            End If


            temp_label.Text = temp_text
        Catch ex As Exception

        End Try
    End Sub

    Public Sub get_valuation_items_into_array(ByVal NOTE_ID As Long, ByRef db_fields As Array, ByRef name_fields As Array, ByRef field_array_count As Integer, ByRef select_string As String, ByRef count_of_records As Integer, ByVal REPORT_TYPE As String, ByVal primary_or_compare As String, ByRef temp_ac_id As Long, ByRef desc_string As Array, ByRef default_spot_count As Integer, ByRef jetnet_trans_id As Array, ByRef client_trans_id As Array, ByVal report_using_id As Long, ByVal COMPLETED_OR_OPEN As String, ByRef localCriteria As viewSelectionCriteriaClass, ByVal aclsData_Temp As clsData_Manager_SQL)

        Dim Query As String = ""
        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim custom_order_by As String = ""
        Dim custom_select_string As String = ""
        Dim temp1_org As String = ""
        Dim temp1 As String = ""
        Dim order_by_string As String = ""
        Dim temp_value As String = ""
        Dim i As Integer = 0
        Dim sNonStandardAcFeature As String = ""
        Dim arrStdFeatCodes(,) As String = Nothing
        Dim arrFeatCodes() As String = Nothing
        Dim Query2 As String = ""
        Dim primary_custom_fields As String = ""




        If Trim(COMPLETED_OR_OPEN) <> "C" Then
            If (Trim(primary_or_compare) = "P" Or Trim(LCase(REPORT_TYPE)) = "active") Then


                name_fields(0) = "AC_DETAILS"
                db_fields(0) = "CONCAT(cliamod_make_name, ' ', cliamod_model_name  , ' ', cliaircraft_ser_nbr) as ac_details "

                ' name_fields(1) = "Year Mfr"
                ' db_fields(1) = "cliaircraft_year_mfr"

                '  name_fields(2) = "Registration Number"
                '  db_fields(2) = "cliaircraft_reg_nbr"

                name_fields(1) = "Asking Price"
                db_fields(1) = "cliaircraft_asking_price"

                name_fields(2) = "Take Price"
                db_fields(2) = "cliaircraft_est_price"

                name_fields(3) = "Estimated Value"
                db_fields(3) = "cliaircraft_broker_price"



                If (Trim(LCase(REPORT_TYPE)) = "sold") Then
                    field_array_count = 4



                    If (report_using_id = -1 Or report_using_id = 0) Then

                        field_array_count = 10

                        name_fields(4) = "Date Listed"
                        db_fields(4) = "cliaircraft_date_listed"       ' THIS FIELD WILL ALWAYS BE NULL ON AN OPEN PRIMARY

                        name_fields(5) = "Year Mfr"
                        db_fields(5) = "cliaircraft_year_mfr"

                        name_fields(6) = "Registration Number"
                        db_fields(6) = "cliaircraft_reg_nbr"

                        name_fields(7) = "Date"
                        db_fields(7) = "'' as Date"

                        name_fields(8) = "Purchaser"
                        db_fields(8) = "'' as Purchaser"

                        name_fields(9) = "Seller"
                        db_fields(9) = "'' as Seller"
                    Else
                        'name_fields(4) = "Date"
                        'db_fields(4) = "clival_date_purchased"       ' THIS FIELD WILL ALWAYS BE NULL ON AN OPEN PRIMARY 
                    End If

                Else

                    field_array_count = 4
                End If






            ElseIf (Trim(LCase(REPORT_TYPE)) = "sold") Then


                name_fields(0) = "AC_DETAILS"
                db_fields(0) = "CONCAT(clitrans_ser_nbr, ' ' , cliamod_make_name, ' ', cliamod_model_name) as ac_details "

                name_fields(1) = "Asking Price"
                db_fields(1) = "clitrans_asking_price"

                name_fields(2) = "Take Price"
                db_fields(2) = "clitrans_est_price"

                name_fields(3) = "Sold Value"
                db_fields(3) = "clitrans_sold_price"


                If report_using_id = -1 Or report_using_id = 0 Then

                    field_array_count = 10

                    name_fields(4) = "Date Listed"
                    db_fields(4) = "clitrans_date_listed"

                    name_fields(5) = "Year Mfr"
                    db_fields(5) = "clitrans_year_mfr"

                    name_fields(6) = "Registration Number"
                    db_fields(6) = "clitrans_reg_nbr"

                    name_fields(7) = "Date"
                    db_fields(7) = "clitrans_date"

                    name_fields(8) = "Purchaser"
                    db_fields(8) = "(select distinct clitcomp_name from client_Transactions_aircraft_reference inner join client_transactions_company on clitcomp_id = clitcref_client_comp_id where clitcref_client_Trans_id = clitrans_id and  clitcref_contact_type = '96' LIMIT 1) as Purchaser"

                    name_fields(9) = "Seller"
                    db_fields(9) = "(select distinct clitcomp_name from client_Transactions_aircraft_reference inner join client_transactions_company on clitcomp_id = clitcref_client_comp_id where clitcref_client_Trans_id = clitrans_id and  clitcref_contact_type = '95' LIMIT 1) as Seller"
                Else
                    field_array_count = 4
                End If


            End If
        ElseIf Trim(COMPLETED_OR_OPEN) = "C" Then
            '--------------------------------------- CLOSED----------------------------------------------
            If (Trim(primary_or_compare) = "P" Or Trim(LCase(REPORT_TYPE)) = "active") Then
                field_array_count = 4

                name_fields(0) = "AC_DETAILS"
                db_fields(0) = "CONCAT(cliamod_make_name, ' ', cliamod_model_name  , ' ', clival_ser_nbr) as ac_details "

                '  name_fields(1) = "Year Mfr"
                '  db_fields(1) = "clival_year_mfr"

                '  name_fields(2) = "Registration Number"
                '  db_fields(2) = "clival_reg_nbr"

                name_fields(1) = "Asking Price"
                db_fields(1) = "clival_asking_price"

                name_fields(2) = "Take Price"
                db_fields(2) = "clival_est_price"

                name_fields(3) = "Estimated Value"
                db_fields(3) = "clival_broker_price"


            ElseIf (Trim(LCase(REPORT_TYPE)) = "sold") Then
                field_array_count = 5

                name_fields(0) = "AC_DETAILS"
                db_fields(0) = "CONCAT(clitrans_ser_nbr, ' ' , cliamod_make_name, ' ', cliamod_model_name) as ac_details "

                '  name_fields(1) = "Year Mfr"
                '  db_fields(1) = "clitrans_year_mfr"

                '  name_fields(2) = "Registration Number"
                '  db_fields(2) = "clitrans_reg_nbr"

                name_fields(1) = "Asking Price"
                db_fields(1) = "clitrans_asking_price"

                name_fields(2) = "Take Price"
                db_fields(2) = "clitrans_est_price"

                name_fields(3) = "Estimated Value"
                db_fields(3) = "clitrans_sold_price"

                name_fields(4) = "Date"
                db_fields(4) = "clitrans_date"
            End If
            '--------------------------------------- CLOSED----------------------------------------------
        End If


        If (report_using_id = -1 And Trim(LCase(REPORT_TYPE)) = "active") Or (report_using_id = 0 And Trim(LCase(REPORT_TYPE)) = "active") Then

            name_fields(field_array_count) = "Year MFR"
            db_fields(field_array_count) = "cliaircraft_year_mfr"

            name_fields(field_array_count + 1) = "Year DLV"
            db_fields(field_array_count + 1) = "cliaircraft_year_dlv"

            name_fields(field_array_count + 2) = "Owner"
            db_fields(field_array_count + 2) = "(SELECT clicomp_id FROM client_company INNER JOIN client_aircraft_reference ON (clicomp_id = cliacref_comp_id) "
            db_fields(field_array_count + 2) &= " LEFT OUTER JOIN client_contact ON (cliacref_contact_id = clicontact_id )"
            db_fields(field_array_count + 2) &= " WHERE (cliacref_cliac_id = cliaircraft_id) and (cliacref_contact_type = '00') LIMIT 1 ) as Owner"


            name_fields(field_array_count + 3) = "Broker"
            db_fields(field_array_count + 3) = "(SELECT clicomp_id FROM client_company INNER JOIN client_aircraft_reference ON (clicomp_id = cliacref_comp_id) "
            db_fields(field_array_count + 3) &= " LEFT OUTER JOIN client_contact ON (cliacref_contact_id = clicontact_id )"
            db_fields(field_array_count + 3) &= " WHERE (cliacref_cliac_id = cliaircraft_id) and ((cliacref_contact_type = '99') OR (cliacref_contact_type = '93')) LIMIT 1 ) as Broker "

            name_fields(field_array_count + 4) = "Date Listed"
            db_fields(field_array_count + 4) = "cliaircraft_date_listed"

            name_fields(field_array_count + 5) = "AFTT"
            db_fields(field_array_count + 5) = "cliaircraft_airframe_total_hours"

            name_fields(field_array_count + 6) = "Engine TT"
            db_fields(field_array_count + 6) = "(select cliacep_engine_1_ttsn_hours from client_aircraft_engine where cliacep_cliac_id = clival_client_ac_id) as ENGINETT " ' "cliacep_engine_1_ttsn_hours"

            name_fields(field_array_count + 7) = "INT Year"
            db_fields(field_array_count + 7) = "cliaircraft_interior_month_year"

            name_fields(field_array_count + 8) = "EXT Year"
            db_fields(field_array_count + 8) = "cliaircraft_exterior_month_year"

            'name_fields(field_array_count + 9) = "Date Listed"
            ' db_fields(field_array_count + 9) = "cliaircraft_date_listed"

            field_array_count = field_array_count + 9
        End If





        For i = 0 To field_array_count - 1
            temp1 = db_fields(i)
            temp1_org = temp1

            If Trim(temp1) <> "" Then
                If InStr(Trim(temp1), " as ") > 0 Then
                    temp1 = Right(temp1, (Len(temp1) - InStr(Trim(temp1), " as ") - 3))
                End If

                If i = 0 Then
                    select_string = temp1_org
                ElseIf i = 1 Then
                    select_string &= ", " & temp1_org
                    order_by_string = temp1 & " asc "
                Else
                    select_string &= ", " & temp1_org
                    order_by_string &= ", " & temp1 & " asc "
                End If

                db_fields(i) = Trim(temp1) ' which, if changed for "as" will fix for dbfields showing
            End If
        Next

        If (report_using_id = -1 And Trim(LCase(REPORT_TYPE)) = "active") Or (report_using_id = 0 And Trim(LCase(REPORT_TYPE)) = "active") Then
            If localCriteria.ViewCriteriaAmodID > 0 Then
                temp_value = localCriteria.ViewCriteriaAmodID

                name_fields(i) = ""
                sNonStandardAcFeature = ""

                If Trim(temp_value) <> "0" Then
                    load_standard_ac_features(localCriteria, arrStdFeatCodes)

                    display_nonstandard_feature_code_headings(localCriteria, arrFeatCodes, arrStdFeatCodes, 30, sNonStandardAcFeature)

                    For i = 0 To UBound(arrFeatCodes)
                        name_fields(field_array_count) = arrFeatCodes(i)
                        select_string &= ", (select cliafeat_flag from client_aircraft_key_features where cliafeat_cliac_id = cliaircraft_id and cliafeat_type = '" & arrFeatCodes(i) & "') as '" & arrFeatCodes(i) & "' "
                        order_by_string &= ", '" & arrFeatCodes(i) & "' asc "
                        db_fields(field_array_count) = arrFeatCodes(i)

                        field_array_count = field_array_count + 1
                    Next
                End If
            End If
        End If



        default_spot_count = field_array_count


        If Trim(COMPLETED_OR_OPEN) <> "C" Then
            If report_using_id > 0 Then
                If (Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold") Then
                    Call crmViewDataLayer.get_valuation_fields_from_layout(NOTE_ID, db_fields, name_fields, field_array_count, custom_select_string, REPORT_TYPE, primary_or_compare, report_using_id, aclsData_Temp, custom_order_by, primary_custom_fields, True)
                Else
                    Call crmViewDataLayer.get_valuation_fields_from_layout(NOTE_ID, db_fields, name_fields, field_array_count, custom_select_string, REPORT_TYPE, primary_or_compare, report_using_id, aclsData_Temp, custom_order_by, primary_custom_fields, False)
                End If
            Else
                ' Call get_valuation_fields(NOTE_ID, db_fields, name_fields, field_array_count, select_string, REPORT_TYPE, primary_or_compare)
            End If
        Else
            Call get_valuation_fields(NOTE_ID, db_fields, name_fields, field_array_count, select_string, REPORT_TYPE, primary_or_compare, COMPLETED_OR_OPEN)
        End If








        Query = Query & " SELECT DISTINCT "

        Query = Query & Replace(select_string, " asc ", "") & ", clival_client_ac_id, clival_jetnet_ac_id "
        If Trim(custom_select_string) <> "" Then
            Query = Query & ", " & Trim(custom_select_string)
        End If

        If (Trim(LCase(REPORT_TYPE)) = "active" Or Trim(primary_or_compare) = "P") And Trim(COMPLETED_OR_OPEN) <> "C" Then
            Query = Query & ", cliaircraft_value_description "
        Else
            Query = Query & ", clival_value_description "

            ' if its a closed sold record, or its an open sold comparable record
            If (Trim(COMPLETED_OR_OPEN) = "C" And Trim(LCase(REPORT_TYPE)) = "sold" And Trim(primary_or_compare) = "C") Or (Trim(COMPLETED_OR_OPEN) <> "C" And Trim(LCase(REPORT_TYPE)) = "sold" And Trim(primary_or_compare) = "C") Then

                Query = Query & ", clitrans_jetnet_trans_id, clitrans_id "
            End If
        End If


        Query = Query & " FROM client_value_comparables "

        If Trim(COMPLETED_OR_OPEN) <> "C" Then
            If (Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold") Then
                Query = Query & " INNER JOIN client_aircraft ON clival_client_ac_id = cliaircraft_id "
                Query = Query & " INNER JOIN client_aircraft_model ON cliaircraft_cliamod_id = cliamod_id "

                'Query = Query & " LEFT OUTER JOIN client_transactions ON clitrans_id = clival_clitrans_id"
                'Query = Query & " LEFT OUTER JOIN client_transactions_aircraft_reference ON client_transactions_aircraft_reference.clitcref_client_trans_id = client_transactions.clitrans_id and clitcref_contact_type = '96' "
                'Query = Query & " LEFT OUTER JOIN client_transactions_company on clitcref_client_comp_id = client_transactions_company.clitcomp_id and clitcref_client_trans_id = clitcomp_trans_id  "
                'Query = Query & " LEFT OUTER JOIN client_transactions_contact on clitcref_client_contact_id = client_transactions_contact.clitcontact_id and clitcref_client_trans_id = clitcontact_trans_id  "

                'Query = Query & " left outer join company  WITH(NOLOCK) on comp_id = yr_comp_id and comp_journ_id = yr_journ_id and yr_contact_type = '00' "
                'Query = Query & " left outer join contact  WITH(NOLOCK) on contact_id = yr_contact_id  and contact_comp_id = yr_comp_id and contact_journ_id = yr_journ_id "

                Query = Query & " left outer join client_aircraft_reference on cliacref_cliac_id = cliaircraft_id  and cliacref_contact_type = '00' "
                Query = Query & " left outer join client_company on cliacref_comp_id = clicomp_id "
                Query = Query & " Left outer join client_contact on cliacref_contact_id = clicontact_id and clicontact_status = 'Y'"



            ElseIf Trim(LCase(REPORT_TYPE)) = "active" Then
                Query = Query & " INNER JOIN client_aircraft ON clival_client_ac_id = cliaircraft_id "
                Query = Query & " INNER JOIN client_aircraft_model ON cliaircraft_cliamod_id = cliamod_id "
            ElseIf Trim(LCase(REPORT_TYPE)) = "sold" Then
                ' If Trim(primary_or_compare) = "P" Then
                ' Query = Query & " INNER JOIN client_aircraft ON clival_client_ac_id = cliaircraft_id "
                'End If
                Query = Query & " INNER JOIN client_transactions ON clitrans_id = clival_clitrans_id"
                Query = Query & " INNER JOIN client_aircraft_model ON cliamod_id  = clitrans_cliamod_id "
                Query = Query & " LEFT OUTER JOIN client_transactions_aircraft_reference ON client_transactions_aircraft_reference.clitcref_client_trans_id = client_transactions.clitrans_id and clitcref_contact_type = '96' "
                Query = Query & " LEFT OUTER JOIN client_transactions_company on clitcref_client_comp_id = client_transactions_company.clitcomp_id and clitcref_client_trans_id = clitcomp_trans_id  "
                Query = Query & " LEFT OUTER JOIN client_transactions_contact on clitcref_client_contact_id = client_transactions_contact.clitcontact_id and clitcref_client_trans_id = clitcontact_trans_id  "
            End If
        ElseIf Trim(COMPLETED_OR_OPEN) = "C" Then

            If (Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold") Then
                Query = Query & " INNER JOIN client_aircraft ON clival_client_ac_id = cliaircraft_id "
                Query = Query & " INNER JOIN client_aircraft_model ON cliaircraft_cliamod_id = cliamod_id "
            ElseIf Trim(LCase(REPORT_TYPE)) = "active" Then
                Query = Query & " INNER JOIN client_aircraft ON clival_client_ac_id = cliaircraft_id "
                Query = Query & " INNER JOIN client_aircraft_model ON cliaircraft_cliamod_id = cliamod_id "
            ElseIf Trim(LCase(REPORT_TYPE)) = "sold" Then
                Query = Query & " INNER JOIN client_transactions ON clitrans_id = clival_clitrans_id"
                Query = Query & " INNER JOIN client_aircraft_model ON cliamod_id  = clitrans_cliamod_id "
            End If
        End If




        Query = Query & " WHERE clival_note_id = " & NOTE_ID & " "

        If Trim(COMPLETED_OR_OPEN) <> "C" Then
            If Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold" Then
                Query = Query & " AND clival_type = 'F' "
            ElseIf Trim(LCase(REPORT_TYPE)) = "active" Then
                Query = Query & " AND clival_type = 'F' "
            Else
                Query = Query & " AND clival_type = 'S' "
            End If
        Else
            If Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold" Then
                Query = Query & " AND clival_type = 'F' "
            ElseIf Trim(LCase(REPORT_TYPE)) = "active" Then
                Query = Query & " AND clival_type = 'F' "
            Else
                Query = Query & " AND clival_type = 'S' "
            End If
        End If
        Query = Query & " AND clival_ac_type = '" & primary_or_compare & "' "
        Query = Query & " order by clival_ac_type desc, clival_client_ac_id desc, " & order_by_string

        If Trim(custom_order_by) <> "" Then
            Query = Query & ", " & Trim(custom_order_by)
        End If


        Try

            results_table = Get_Compare_Query(Query, "GET COMPARE AC VIEW QUERY")

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Trim(primary_or_compare) = "P" Then
                            temp_ac_id = r("clival_client_ac_id")
                        End If

                        If (Trim(COMPLETED_OR_OPEN) = "C" And Trim(LCase(REPORT_TYPE)) = "sold" And Trim(primary_or_compare) = "C") Or (Trim(COMPLETED_OR_OPEN) <> "C" And Trim(LCase(REPORT_TYPE)) = "sold" And Trim(primary_or_compare) = "C") Then
                            jetnet_trans_id(count_of_records) = r("clitrans_jetnet_trans_id")
                            client_trans_id(count_of_records) = r("clitrans_id")
                        End If



                        ' set all variables = "" so no nulls

                        array_ac_id(count_of_records) = r("clival_client_ac_id")
                        array_jac_id(count_of_records) = r("clival_jetnet_ac_id")

                        array_field_0(count_of_records) = ""
                        array_field_1(count_of_records) = ""
                        array_field_2(count_of_records) = ""
                        array_field_3(count_of_records) = ""
                        array_field_4(count_of_records) = ""
                        array_field_5(count_of_records) = ""
                        array_field_6(count_of_records) = ""
                        array_field_7(count_of_records) = ""
                        array_field_8(count_of_records) = ""
                        array_field_9(count_of_records) = ""
                        array_field_10(count_of_records) = ""
                        array_field_11(count_of_records) = ""
                        array_field_12(count_of_records) = ""
                        array_field_13(count_of_records) = ""
                        array_field_14(count_of_records) = ""
                        array_field_15(count_of_records) = ""
                        array_field_16(count_of_records) = ""
                        array_field_17(count_of_records) = ""
                        array_field_18(count_of_records) = ""
                        array_field_19(count_of_records) = ""
                        array_field_20(count_of_records) = ""
                        array_field_21(count_of_records) = ""
                        array_field_22(count_of_records) = ""
                        array_field_23(count_of_records) = ""
                        array_field_24(count_of_records) = ""
                        array_field_25(count_of_records) = ""
                        array_field_26(count_of_records) = ""
                        array_field_27(count_of_records) = ""
                        array_field_28(count_of_records) = ""
                        array_field_29(count_of_records) = ""
                        array_field_30(count_of_records) = ""
                        array_field_31(count_of_records) = ""
                        array_field_32(count_of_records) = ""
                        array_field_33(count_of_records) = ""
                        array_field_34(count_of_records) = ""
                        array_field_35(count_of_records) = ""

                        If Trim(COMPLETED_OR_OPEN) <> "C" Then
                            If Trim(LCase(REPORT_TYPE)) = "active" Or Trim(primary_or_compare) = "P" Then
                                If Not IsDBNull(r("cliaircraft_value_description")) Then
                                    desc_string(count_of_records) = r("cliaircraft_value_description")
                                Else
                                    desc_string(count_of_records) = ""
                                End If
                            Else
                                If Not IsDBNull(r("clival_value_description")) Then
                                    desc_string(count_of_records) = r("clival_value_description")
                                Else
                                    desc_string(count_of_records) = ""
                                End If
                            End If
                        Else
                            If Not IsDBNull(r("clival_value_description")) Then
                                desc_string(count_of_records) = r("clival_value_description")
                            Else
                                desc_string(count_of_records) = ""
                            End If
                        End If




                        For i = 0 To field_array_count - 1

                            If Trim(db_fields(i)) <> "" Then

                                ' find the datee field, fill it in 
                                '   If Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold" Then 
                                'End If

                                If Not IsDBNull(r("" & db_fields(i) & "")) Then
                                    temp_value = r("" & db_fields(i) & "")
                                Else
                                    temp_value = ""
                                End If

                                If Trim(db_fields(i)) = "Owner" Or Trim(db_fields(i)) = "Broker" Then
                                    If IsNumeric(Trim(temp_value)) Then
                                        ' temp_value = get_company_name_by_id(temp_value, True) 'look up
                                        temp_value = crmViewDataLayer.get_client_company_name_by_id(temp_value, True) 'look up
                                    End If
                                End If

                                If i = 0 Then
                                    If Not IsDBNull(r("ac_details")) Then
                                        array_field_0(count_of_records) = Trim(r("ac_details"))
                                    End If
                                Else
                                    add_to_array(i, count_of_records, temp_value)
                                End If
                            End If


                        Next



                        count_of_records = count_of_records + 1
                    Next

                End If

            End If

        Catch ex As Exception
        Finally
            results_table2 = Nothing
        End Try
    End Sub

    Public Sub add_to_array(ByVal i As Integer, ByVal count_of_records As Integer, ByVal temp_value As String)

        If i = 1 Then
            array_field_1(count_of_records) = Trim(temp_value)
        ElseIf i = 2 Then
            array_field_2(count_of_records) = Trim(temp_value)
        ElseIf i = 3 Then
            array_field_3(count_of_records) = Trim(temp_value)
        ElseIf i = 4 Then
            array_field_4(count_of_records) = Trim(temp_value)
        ElseIf i = 5 Then
            array_field_5(count_of_records) = Trim(temp_value)
        ElseIf i = 6 Then
            array_field_6(count_of_records) = Trim(temp_value)
        ElseIf i = 7 Then
            array_field_7(count_of_records) = Trim(temp_value)
        ElseIf i = 8 Then
            array_field_8(count_of_records) = Trim(temp_value)
        ElseIf i = 9 Then
            array_field_9(count_of_records) = Trim(temp_value)
        ElseIf i = 10 Then
            array_field_10(count_of_records) = Trim(temp_value)
        ElseIf i = 11 Then
            array_field_11(count_of_records) = Trim(temp_value)
        ElseIf i = 12 Then
            array_field_12(count_of_records) = Trim(temp_value)
        ElseIf i = 13 Then
            array_field_13(count_of_records) = Trim(temp_value)
        ElseIf i = 14 Then
            array_field_14(count_of_records) = Trim(temp_value)
        ElseIf i = 15 Then
            array_field_15(count_of_records) = Trim(temp_value)
        ElseIf i = 16 Then
            array_field_16(count_of_records) = Trim(temp_value)
        ElseIf i = 17 Then
            array_field_17(count_of_records) = Trim(temp_value)
        ElseIf i = 18 Then
            array_field_18(count_of_records) = Trim(temp_value)
        ElseIf i = 19 Then
            array_field_19(count_of_records) = Trim(temp_value)
        ElseIf i = 20 Then
            array_field_20(count_of_records) = Trim(temp_value)
        ElseIf i = 21 Then
            array_field_21(count_of_records) = Trim(temp_value)
        ElseIf i = 22 Then
            array_field_22(count_of_records) = Trim(temp_value)
        ElseIf i = 23 Then
            array_field_23(count_of_records) = Trim(temp_value)
        ElseIf i = 24 Then
            array_field_24(count_of_records) = Trim(temp_value)
        ElseIf i = 25 Then
            array_field_25(count_of_records) = Trim(temp_value)
        ElseIf i = 26 Then
            array_field_26(count_of_records) = Trim(temp_value)
        ElseIf i = 27 Then
            array_field_27(count_of_records) = Trim(temp_value)
        ElseIf i = 28 Then
            array_field_28(count_of_records) = Trim(temp_value)
        ElseIf i = 29 Then
            array_field_29(count_of_records) = Trim(temp_value)
        ElseIf i = 30 Then
            array_field_30(count_of_records) = Trim(temp_value)
        ElseIf i = 31 Then
            array_field_31(count_of_records) = Trim(temp_value)
        ElseIf i = 32 Then
            array_field_32(count_of_records) = Trim(temp_value)
        ElseIf i = 33 Then
            array_field_33(count_of_records) = Trim(temp_value)
        ElseIf i = 34 Then
            array_field_34(count_of_records) = Trim(temp_value)
        ElseIf i = 35 Then
            array_field_35(count_of_records) = Trim(temp_value)
        End If

    End Sub

    Public Sub get_valuation_fields(ByVal NOTE_ID As Long, ByRef db_fields As Array, ByRef name_fields As Array, ByRef field_count As Integer, ByRef select_string As String, ByVal REPORT_TYPE As String, ByVal primary_or_compare As String, ByVal COMPLETED_OR_OPEN As String)


        Dim Query As String = ""
        Dim results_table As New DataTable
        Dim results_table_inner As New DataTable
        Dim temp_string As String = ""
        Dim temp_note As String = ""
        Dim Query2 As String = ""


        Query = ""
        Query = Query & " select * from client_value_fields where clivalfld_val_id = " & NOTE_ID


        If Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold" Then

        ElseIf Trim(LCase(REPORT_TYPE)) = "active" Then
            Query = Query & " and clivalfld_db_name <> '' "
        Else
            Query = Query & " and clivalfld_trans_db_name <> '' "
        End If


        Query = Query & " order by clivalfld_order asc "

        Try
            'If IsNothing(localDatalayer) Then
            '    localDatalayer = New viewsDataLayer
            'End If

            results_table = Get_Compare_Query(Query, "GET VALUATION DETAILS")

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows



                        If Not IsDBNull(r("clivalfld_name")) Then
                            name_fields(field_count) = Trim(r("clivalfld_name"))
                        End If


                        If Trim(COMPLETED_OR_OPEN) <> "C" Then

                            If Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold" Then
                                If Not IsDBNull(r("clivalfld_db_name")) Then
                                    If Trim(r("clivalfld_db_name")) <> "" Then
                                        db_fields(field_count) = Trim(r("clivalfld_db_name"))
                                    ElseIf Trim(name_fields(field_count)) = "Date" Then
                                        db_fields(field_count) = ""
                                    Else
                                        db_fields(field_count) = ""
                                    End If
                                Else
                                    db_fields(field_count) = ""
                                End If
                            ElseIf Trim(LCase(REPORT_TYPE)) = "active" Then
                                If Not IsDBNull(r("clivalfld_db_name")) Then
                                    db_fields(field_count) = Trim(r("clivalfld_db_name"))
                                End If
                            Else
                                If Not IsDBNull(r("clivalfld_trans_db_name")) Then
                                    db_fields(field_count) = Trim(r("clivalfld_trans_db_name"))
                                End If
                            End If
                        ElseIf Trim(COMPLETED_OR_OPEN) = "C" Then
                            If Trim(primary_or_compare) = "P" And Trim(LCase(REPORT_TYPE)) = "sold" Then
                                If Not IsDBNull(r("clivalfld_closed_db_name")) Then
                                    If Trim(r("clivalfld_closed_db_name")) <> "" Then
                                        db_fields(field_count) = Trim(r("clivalfld_closed_db_name"))
                                    ElseIf Trim(name_fields(field_count)) = "Date" Then
                                        db_fields(field_count) = ""
                                    Else
                                        db_fields(field_count) = ""
                                    End If
                                Else
                                    db_fields(field_count) = ""
                                End If
                            ElseIf Trim(LCase(REPORT_TYPE)) = "active" Then
                                If Not IsDBNull(r("clivalfld_closed_db_name")) Then
                                    db_fields(field_count) = Trim(r("clivalfld_closed_db_name"))
                                End If
                            Else
                                If Not IsDBNull(r("clivalfld_trans_db_name")) Then
                                    db_fields(field_count) = Trim(r("clivalfld_trans_db_name"))
                                End If
                            End If
                        End If


                        If Trim(db_fields(field_count)) <> "" Then
                            If field_count = 0 Then
                                select_string = db_fields(field_count) & " asc "
                            Else
                                If Trim(select_string) <> "" Then
                                    select_string = select_string & ", " & db_fields(field_count) & " asc "
                                Else
                                    select_string = db_fields(field_count) & " asc "
                                End If
                            End If
                        End If

                        field_count = field_count + 1

                    Next
                End If

            End If


        Catch ex As Exception
        Finally
        End Try


    End Sub

    Function get_company_name_by_id(ByVal comp_id As Long, Optional ByVal no_columns As Boolean = False) As String
        get_company_name_by_id = ""

        Dim CompanyLocation As String = ""
        Dim CompanyTitle As String = ""
        Dim results_table As New DataTable

        CompanyLocation = ""
        CompanyTitle = ""

        results_table = get_comp_id_by_name(comp_id)

        If Not IsNothing(results_table) Then
            If results_table.Rows.Count > 0 Then



                For Each r As DataRow In results_table.Rows


                    CompanyTitle = IIf(Not IsDBNull(r("comp_name")), r("comp_name") & vbNewLine, vbNewLine)
                    CompanyTitle += IIf(Not IsDBNull(r("comp_address1")), r("comp_address1") & " ", "")
                    CompanyTitle += IIf(Not IsDBNull(r("comp_address2")), r("comp_address2") & vbNewLine, vbNewLine)
                    CompanyLocation += IIf(Not IsDBNull(r("comp_city")), r("comp_city") & ", ", "")
                    CompanyLocation += IIf(Not IsDBNull(r("comp_state")), r("comp_state") & " ", " ")
                    CompanyLocation += IIf(Not IsDBNull(r("comp_country")), r("comp_country") & " ", " ")

                    CompanyLocation = Replace(CompanyLocation, "United States", "U.S.")
                    CompanyTitle += IIf(Not IsDBNull(r("comp_city")), r("comp_city") & ", ", "")
                    CompanyTitle += IIf(Not IsDBNull(r("comp_state")), r("comp_state") & " ", " ")

                    CompanyTitle += IIf(Not IsDBNull(r("comp_country")), r("comp_country") & vbNewLine & vbNewLine, vbNewLine & vbNewLine)
                    CompanyTitle += IIf(Not IsDBNull(r("comp_phone_office")), "Office: " & r("comp_phone_office") & vbNewLine, "")
                    CompanyTitle += IIf(Not IsDBNull(r("comp_phone_fax")), "Fax: " & r("comp_phone_fax") & vbNewLine & vbNewLine, vbNewLine & vbNewLine)
                    CompanyTitle += IIf(Not IsDBNull(r("comp_email_address")), "Email: " & r("comp_email_address") & vbNewLine, "")
                    CompanyTitle += IIf(Not IsDBNull(r("comp_web_address")), "Website: " & r("comp_web_address") & vbNewLine, "")

                    If no_columns = False Then
                        get_company_name_by_id += "<td align=""left"" valign=""top"">"
                    End If

                    If Not IsDBNull(r("comp_name")) Then
                        get_company_name_by_id += "<span><span class='label'><span class='magnify_bullet' title='" & CompanyTitle & "'>" & DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, Trim(r("comp_name")), "", "") & " <span class='tiny'>" & CompanyLocation & "</span></span></span>"
                    End If
                    If no_columns = False Then
                        get_company_name_by_id += "</td>"
                    End If


                Next

            Else
                If no_columns = False Then
                    get_company_name_by_id += "<td align=""left"" valign=""top"">"
                    get_company_name_by_id += "&nbsp;</td>"
                End If

            End If
        Else
            If no_columns = False Then
                get_company_name_by_id += "<td align=""left"" valign=""top"">"
                get_company_name_by_id += "&nbsp;</td>"
            End If
        End If







    End Function



    Public Function Get_Subscription_Team(ByVal sub_id As Long, ByVal name_string As String) As Boolean
        Get_Subscription_Team = False

        Dim atemptable As New DataTable

        Dim sQuery As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            sQuery = " select * from subscription_teams with (NOLOCK)"
            sQuery &= " where subteam_sub_id =  " & sub_id & " "
            sQuery &= " and subteam_name='" & name_string & "'"


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase")
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "ErrorGet_Subscription_Team" + constrExc.Message
            End Try


            If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                    For Each r As DataRow In atemptable.Rows

                        Get_Subscription_Team = True

                    Next
                End If
            End If

            atemptable.Dispose()

        Catch ex As Exception
            aError = "Error in Get_Subscription_Team()" + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Function


End Class