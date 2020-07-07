Imports System.Web.Script.Services
Imports System.Web
Imports System.Web.Services
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/JSONresponse.aspx.vb $
'$$Author: Amanda $
'$$Date: 7/06/20 4:20p $
'$$Modtime: 7/06/20 4:21p $
'$$Revision: 13 $
'$$Workfile: JSONresponse.aspx.vb $
'
' ********************************************************************************


Public Class Airport
    Private str_label As String
    Private str_value As String
    'Private str_city As String
    'Private str_state As String
    'Private str_country As String
    'Private str_iata As String
    'Private str_icao As String

    Public Property label() As String
        Get
            Return str_label
        End Get
        Set(ByVal value As String)
            str_label = value
        End Set
    End Property
    Public Property value() As String
        Get
            Return str_value
        End Get
        Set(ByVal value As String)
            str_value = value
        End Set
    End Property
    '  Public Property City() As String
    '    Get
    '      Return str_city
    '    End Get
    '    Set(ByVal value As String)
    '      str_city = value
    '    End Set
    '  End Property
    '  Public Property State() As String
    '    Get
    '      Return str_state
    '    End Get
    '    Set(ByVal value As String)
    '      str_state = value
    '    End Set
    '  End Property
    '  Public Property Country() As String
    '    Get
    '      Return str_country
    '    End Get
    '    Set(ByVal value As String)
    '      str_country = value
    '    End Set
    '  End Property
    '  Public Property Iata() As String
    '    Get
    '      Return str_iata
    '    End Get
    '    Set(ByVal value As String)
    '      str_iata = value
    '    End Set
    '  End Property
    '  Public Property Icao() As String
    '    Get
    '      Return str_icao
    '    End Get
    '    Set(ByVal value As String)
    '      str_icao = value
    '    End Set
    '  End Property
End Class



Partial Public Class JSONresponse

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json,
     UseHttpGet:=True)>
    Public Shared Function Airport()
        If HttpContext.Current.Session.Item("crmUserLogon") = True Then
            Dim localDatalayer As New viewsDataLayer
            Dim resultsTable As New DataTable
            Dim term As String = ""
            term = Trim(HttpContext.Current.Request.Item("term"))

            localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

            resultsTable = localDatalayer.get_airports_by_IATA_or_ICAO_City_Name(term, term, term, term)


            Dim AirportList As New List(Of Airport)
            If Not IsNothing(resultsTable) Then
                If resultsTable.Rows.Count > 0 Then
                    For Each r As DataRow In resultsTable.Rows
                        Dim airportCls As New Airport
                        airportCls.label = r("aport_name")

                        If Not IsDBNull(r("aport_city")) Then
                            airportCls.label += " (" & r("aport_city") & ") "
                        End If
                        If Not IsDBNull(r("aport_iata_code")) Then
                            airportCls.label += " - " & r("aport_iata_code")
                        End If
                        If Not IsDBNull(r("aport_icao_code")) Then
                            airportCls.label += " - " & r("aport_icao_code")
                        End If
                        airportCls.value = r("aport_id")
                        AirportList.Add(airportCls)
                    Next
                End If
            End If

            Return New Script.Serialization.JavaScriptSerializer().Serialize(AirportList)
        Else
            Return ""
        End If
    End Function
    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json,
   UseHttpGet:=True)>
    Public Shared Function AirportIata()
        If HttpContext.Current.Session.Item("crmUserLogon") = True Then
            Dim localDatalayer As New viewsDataLayer
            Dim resultsTable As New DataTable
            Dim term As String = ""
            term = Trim(HttpContext.Current.Request.Item("term"))

            localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

            resultsTable = localDatalayer.get_airports_by_IATA_or_ICAO_City_Name(term, term, term, term)


            Dim AirportList As New List(Of Airport)
            If Not IsNothing(resultsTable) Then
                If resultsTable.Rows.Count > 0 Then
                    For Each r As DataRow In resultsTable.Rows
                        Dim airportCls As New Airport
                        Dim city As String = ""
                        Dim country As String = ""
                        Dim iata As String = ""
                        Dim icao As String = ""

                        If Not IsDBNull(r("aport_city")) Then
                            city = r("aport_city")
                        End If
                        If Not IsDBNull(r("aport_country")) Then
                            country = r("aport_country")
                        End If

                        If Not IsDBNull(r("aport_iata_code")) Then
                            iata = r("aport_iata_code")
                        End If
                        If Not IsDBNull(r("aport_icao_code")) Then
                            icao += r("aport_icao_code")
                        End If

                        airportCls.label = r("aport_name") & " (" & city & IIf(Not String.IsNullOrEmpty(country), IIf(Not String.IsNullOrEmpty(city), ", " & country, country), "") & ") (" & icao & IIf(Not String.IsNullOrEmpty(iata) And Not String.IsNullOrEmpty(icao), "/", "") & iata & ")"
                        airportCls.value = r("aport_latitude_decimal").ToString & "|" & r("aport_longitude_decimal").ToString & "|" & r("aport_id")
                        AirportList.Add(airportCls)
                    Next
                End If
            End If

            Return New Script.Serialization.JavaScriptSerializer().Serialize(AirportList)
        Else
            Return ""
        End If
    End Function




    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json,
      UseHttpGet:=True)>
    Public Shared Function ModelAttributes()
        If HttpContext.Current.Session.Item("crmUserLogon") = True Then
            Dim term As String = ""
            Dim GeneralFunction As New clsGeneral.clsGeneral
            term = Trim(HttpContext.Current.Request.Item("modelID"))
            Dim rowIds As String = Trim(HttpContext.Current.Request.Item("catRow[]"))
            Dim EachRow As Array = Split(rowIds, ",")
            Dim StoredData As New DataTable

            StoredData = GeneralFunction.SelectModelAttribute(term)

            ''Delete all entries in index for this model:
            GeneralFunction.DeleteModelAttribute(term)

            For SingleRowID = 0 To UBound(EachRow)
                'Go through and add each row to the index for this model.
                'EachRow(SingleRowID) = Attribute
                'SingleRowID = Sort
                'ModelID = term
                'Let's add a filter to get the information here:

                Dim valueExisting As String = ""
                Dim defaultExisting As String = "N"
                Dim serStart As String = ""
                Dim serEnd As String = ""
                Dim preExisting As DataRow() = StoredData.Select(" attmod_amod_id = " & term.ToString & " and attmod_att_id = " & EachRow(SingleRowID).ToString, "")
                ' extract and import
                For Each existingRow In preExisting
                    valueExisting = ""
                    defaultExisting = "N"
                    serStart = ""
                    serEnd = ""

                    If Not IsDBNull(existingRow("attmod_standard_equip")) Then
                        If Not String.IsNullOrEmpty(Trim(existingRow("attmod_standard_equip"))) Then
                            defaultExisting = existingRow("attmod_standard_equip")
                        End If
                    End If

                    If Not IsDBNull(existingRow("attmod_value")) Then
                        valueExisting = existingRow("attmod_value")
                    End If

                    If Not IsDBNull(existingRow("attmod_stdeq_start_ser_no_value")) Then
                        serStart = existingRow("attmod_stdeq_start_ser_no_value")
                    End If
                    If Not IsDBNull(existingRow("attmod_stdeq_end_ser_no_value")) Then
                        serEnd = existingRow("attmod_stdeq_end_ser_no_value")
                    End If
                Next

                GeneralFunction.SaveModelAttribute(EachRow(SingleRowID), term, SingleRowID, defaultExisting, valueExisting, serStart, serEnd)


            Next
            Return ""
        Else
            Return ""
        End If
    End Function



    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json,
      UseHttpGet:=True)>
    Public Shared Function DashBoardCreation()
        If HttpContext.Current.Session.Item("crmUserLogon") = True Then
            Dim SubscriptionID As Long = HttpContext.Current.Session.Item("localUser").crmSubSubID
            Dim SubscriptionLogin As String = HttpContext.Current.Session.Item("localUser").crmUserLogin
            Dim SeqNo As Long = HttpContext.Current.Session.Item("localUser").crmSubSeqNo

            Dim rowIds As String = Trim(HttpContext.Current.Request.Item("id[]"))
            Dim EachRow As Array = Split(rowIds, ",")
            Dim StoredData As New DataTable
            'Delete rows currently in table for this user's login.
            DisplayFunctions.DeleteChosenDashboards(SubscriptionID, SubscriptionLogin, SeqNo)

            For SingleRowID = 0 To UBound(EachRow)


                'Go through and add each row to the index for this dashboard.
                'ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long, sidash_order As Long, sidash_dashb_id As Long
                Dim OrderNo As Long = SingleRowID
                Dim DashID As Long = EachRow(SingleRowID)

                DisplayFunctions.InsertDashboardModuleList(SubscriptionID, SubscriptionLogin, SeqNo, OrderNo, DashID)

            Next


            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                DisplayFunctions.InsertDashboardModuleList(SubscriptionID, SubscriptionLogin, SeqNo, 99, 43)
            End If

            Return ""
        Else
            Return ""
        End If
    End Function


    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json,
  UseHttpGet:=True)>
    Public Shared Function toggleAircraftAlert()
        If HttpContext.Current.Session.Item("crmUserLogon") = True Then
            Dim SubscriptionID As Long = HttpContext.Current.Session.Item("localUser").crmSubSubID
            Dim SubscriptionLogin As String = HttpContext.Current.Session.Item("localUser").crmUserLogin
            Dim SeqNo As Long = HttpContext.Current.Session.Item("localUser").crmSubSeqNo
            Dim aclsData_Temp As New clsData_Manager_SQL
            Dim acID As String = Trim(HttpContext.Current.Request.Item("acID"))
            Dim checked As String = Trim(HttpContext.Current.Request.Item("checked"))

            aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

            Dim DoesFolderExist As New DataTable
            Dim FolderID As Long = 0
            DoesFolderExist = clsGeneral.clsGeneral.CheckAircraftAlertFolderExistence(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin)

            If Not IsNothing(DoesFolderExist) Then
                If DoesFolderExist.Rows.Count > 0 Then
                    FolderID = DoesFolderExist.Rows(0).Item("cfolder_id")
                End If
            End If

            If FolderID = 0 Then
                'Folder needs to be created
                Dim usernameString As String = ""

                usernameString = HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName & " " & HttpContext.Current.Session.Item("localUser").crmLocalUserLastName

                If String.IsNullOrEmpty(Trim(usernameString)) Then
                    usernameString = HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress
                End If

                FolderID = aclsData_Temp.Insert_Into_Evolution_Folders(3, "N", "Aircraft Event Alerts", "N", "S", "", "", HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "Y", "", HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress, "", 60, "N", False)
            End If

            If checked = "true" Then
                aclsData_Temp.Insert_Into_Evolution_Folder_Index(FolderID, 0, acID, 0, 0, 0, 0, 0, 0, 0)
            Else 'We need to figure out the index id and then delete it:
                Dim AlertTable As New DataTable
                AlertTable = clsGeneral.clsGeneral.CheckAircraftAlertsOn(acID, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin)
                If Not IsNothing(AlertTable) Then
                    If AlertTable.Rows.Count > 0 Then 'Checking values to display
                        Dim folderIndexIDRemove As Long = AlertTable.Rows(0).Item("cfoldind_id")
                        FolderID = FolderID
                        acID = acID
                        If FolderID > 0 And folderIndexIDRemove > 0 And acID > 0 Then
                            aclsData_Temp.Remove_Evolution_Folder_Index(folderIndexIDRemove, FolderID, acID, 0, 0, 0, 0, 0)
                        End If
                    End If
                End If

            End If

            Return ""
        Else
            Return ""
        End If
    End Function

    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json,
  UseHttpGet:=True)>
    Public Shared Function toggleFolderAlert()
        If HttpContext.Current.Session.Item("crmUserLogon") = True Then
            Dim SubscriptionID As Long = HttpContext.Current.Session.Item("localUser").crmSubSubID
            Dim SubscriptionLogin As String = HttpContext.Current.Session.Item("localUser").crmUserLogin
            Dim SeqNo As Long = HttpContext.Current.Session.Item("localUser").crmSubSeqNo
            Dim aclsData_Temp As New clsData_Manager_SQL
            Dim folderID As String = Trim(HttpContext.Current.Request.Item("folderID"))
            Dim checked As String = Trim(HttpContext.Current.Request.Item("checked"))

            aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

            folderID = Replace(folderID, "folderOff-", "")
            folderID = folderID
            If checked = "true" Then
                'turn autorun on.
                clsGeneral.clsGeneral.UpdateAutoRunFlag("Y", folderID, SubscriptionID, SubscriptionLogin, SeqNo)
            Else 'turn autorun off
                clsGeneral.clsGeneral.UpdateAutoRunFlag("N", folderID, SubscriptionID, SubscriptionLogin, SeqNo)
            End If

            Return ""
        Else
            Return ""
        End If
    End Function


    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json,
 UseHttpGet:=True)>
    Public Shared Function SetSchedule()
        If HttpContext.Current.Session.Item("crmUserLogon") = True Then
            Dim SubscriptionID As Long = HttpContext.Current.Session.Item("localUser").crmSubSubID
            Dim SubscriptionLogin As String = HttpContext.Current.Session.Item("localUser").crmUserLogin
            Dim SeqNo As Long = HttpContext.Current.Session.Item("localUser").crmSubSeqNo
            Dim aclsData_Temp As New clsData_Manager_SQL
            Dim folderID As String = Trim(HttpContext.Current.Request.Item("folderID"))
            Dim mVal As String = Trim(HttpContext.Current.Request.Item("mVal"))
            Dim hVal As String = Trim(HttpContext.Current.Request.Item("hVal"))
            Dim dVal As String = Trim(HttpContext.Current.Request.Item("dVal"))
            Dim miVal As String = Trim(HttpContext.Current.Request.Item("miVal"))
            folderID = folderID.Replace("scheduleChange-", "")
            Dim TotalMinutes As Long = 0

            If IsNumeric(mVal) Then
                TotalMinutes = mVal * 43829
            End If

            If IsNumeric(dVal) Then
                TotalMinutes += dVal * 1440
            End If

            If IsNumeric(hVal) Then
                TotalMinutes += hVal * 60
            End If

            If IsNumeric(miVal) Then
                TotalMinutes += miVal
            End If

            TotalMinutes = TotalMinutes

            clsGeneral.clsGeneral.UpdateScheduleFolder(TotalMinutes, folderID, SubscriptionID, SubscriptionLogin, SeqNo)


            Return ""
        Else
            Return ""
        End If
    End Function

    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json,
 UseHttpGet:=True)>
    Public Shared Function ContactReferenceManipulation()
        If HttpContext.Current.Session.Item("crmUserLogon") = True Then
            Dim SubscriptionID As Long = HttpContext.Current.Session.Item("localUser").crmSubSubID
            Dim SubscriptionLogin As String = HttpContext.Current.Session.Item("localUser").crmUserLogin
            Dim SeqNo As Long = HttpContext.Current.Session.Item("localUser").crmSubSeqNo
            Dim aclsData_Temp As New clsData_Manager_SQL
            Dim referenceID As String = Trim(HttpContext.Current.Request.Item("id"))
            Dim referenceValue As String = ""
            Dim referenceAction As String = Trim(HttpContext.Current.Request.Item("action"))
            Dim aircraftID As String = Trim(HttpContext.Current.Request.Item("acID"))
            Dim jetnetACID As Long = 0
            Dim clientACID As Long = 0


            Dim referenceGroup As String = Trim(HttpContext.Current.Request.Item("data"))
            Dim splitRef As String() = Split(referenceGroup, "|||")

            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

            ' Replace(r("cliact_type"), "'", "\'") & "|" & companyID.ToString & "|" & contactID.ToString & "|" & source.ToString
            Dim contactType As String = ""
            Dim ClientCompanyID As Long = 0
            Dim ClientContactID As Long = 0

            Dim JetnetCompanyID As Long = 0
            Dim JetnetContactID As Long = 0

            Dim recordSource As String = ""

            If UBound(splitRef) >= 0 Then
                contactType = splitRef(0)
            End If
            If UBound(splitRef) >= 3 Then
                recordSource = splitRef(3)
            End If

            If UBound(splitRef) >= 2 Then
                If recordSource = "CLIENT" Then
                    ClientContactID = splitRef(2)
                Else
                    JetnetContactID = splitRef(2)
                End If
            End If

            If UBound(splitRef) >= 1 Then
                If recordSource = "CLIENT" Then
                    ClientCompanyID = splitRef(1)
                Else
                    JetnetCompanyID = splitRef(1)
                End If
            End If

            'What is the action?
            Select Case referenceAction
                Case "remove"
                    '1.) Remove reference by reference ID.
                    referenceID = referenceID
                    aclsData_Temp.Delete_Client_Aircraft_Reference_cliacref_id(referenceID)

                Case Else 'Add
                    '1.) Look at the source. Is it jetnet or client?
                    '2.) If it's CLIENT:
                    Select Case recordSource
                               'a.) Look up corresponding company client record to find jetnet company ID.
                        Case "JETNET"

                            Dim LookupTable As New DataTable
                            LookupTable = aclsData_Temp.CheckforCompanyBy_JETNET_ID(JetnetCompanyID, "")
                            If Not IsNothing(LookupTable) Then
                                If LookupTable.Rows.Count > 0 Then
                                    ClientCompanyID = LookupTable.Rows(0).Item("comp_id")
                                End If
                            End If

                            'Look up Client AC information
                            jetnetACID = aircraftID
                            LookupTable = aclsData_Temp.CHECKFORClient_Aircraft_JETNET_AC(jetnetACID)
                            If Not IsNothing(LookupTable) Then
                                If LookupTable.Rows.Count > 0 Then
                                    clientACID = LookupTable.Rows(0).Item("cliaircraft_id")
                                End If
                            End If

                        Case Else
                            Dim LookupTable As New DataTable
                            ClientCompanyID = ClientCompanyID
                            'a.) We have the information that we need here for company.
                            clientACID = aircraftID
                            LookupTable = aclsData_Temp.Get_Clients_Aircraft(clientACID)
                            If Not IsNothing(LookupTable) Then
                                If LookupTable.Rows.Count > 0 Then
                                    jetnetACID = LookupTable.Rows(0).Item("cliaircraft_jetnet_ac_id")
                                End If
                            End If

                    End Select

                    'We only run this if we're trying to link an actual contact.
                    If ClientContactID > 0 Or JetnetContactID > 0 Then
                        'b.) Look up corresponding contact client record to find jetnet contact ID.
                        Select Case recordSource
                            Case "JETNET"
                                JetnetContactID = JetnetContactID
                                Dim LookupTable As New DataTable
                                LookupTable = aclsData_Temp.GetContactInfo_JETNET_ID(JetnetContactID, "Y")
                                If Not IsNothing(LookupTable) Then
                                    If LookupTable.Rows.Count > 0 Then
                                        ClientContactID = LookupTable.Rows(0).Item("clicontact_id")
                                    End If
                                End If
                            Case Else
                                ClientContactID = ClientContactID
                                'b.) We have the information we need here.

                        End Select
                    End If


                    ' Insert row into database for add.
                    Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference

                    aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = ClientCompanyID
                    aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = contactType
                    aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = ClientContactID
                    aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = jetnetACID

                    aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = clientACID
                    aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = ""
                    aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = "0"
                    aclsInsert_Client_Aircraft_Reference.cliacref_contact_priority = 0

                    If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                        'Response.Write("added")
                    End If

            End Select




            Return ""
        Else
            Return ""
        End If
    End Function

End Class
