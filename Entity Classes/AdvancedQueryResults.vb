
Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/AdvancedQueryResults.vb $
'$$Author: Amanda $
'$$Date: 5/18/20 4:36p $
'$$Modtime: 5/18/20 11:57a $
'$$Revision: 6 $
'$$Workfile: AdvancedQueryResults.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class AdvancedQueryResults
    Private aError As String
    Private pId As Long = 0
    Private pFieldName As String
    Private pOper As String
    Private pDataType As String
    Private pVal As String
    Private pFieldDisplay As String
    Private pSpecialConsideration As Boolean
    Private pCommasAsDelimiters As Boolean
    Private pCompanyContactSearch As Boolean

    ''' Extra Note Added: 1/31/2014: This class is going to be worked out on the yacht side, then used to replace on the aircraft side
    ''' When it's working. I'm not replacing it yet, because it's not functional.
    ''' It will replace the class: 
    ''' Public Class QueryStringValues
    ''' It will also take the IterateThroughChildren function and any other it needs to build the advanced query string, so that way
    ''' they can be used on both pages.
    ''' 
    ''' Notes: This is a class I use to parse through the advanced search. What basically happens is as follows:
    ''' I create an arraylist of this type of class and as I'm looping through the controls on the page, I fill in a class, if there's a search
    ''' control that needs one. Then I add it to the list and repeat through each control.
    ''' Please see the notes for IterateThroughChildren for more information.

    Sub New()
        aError = ""

        pId = 0
        pFieldName = ""
        pOper = ""
        pDataType = ""
        pVal = ""
        pFieldDisplay = ""
        pSpecialConsideration = False
        pCommasAsDelimiters = True
        pCompanyContactSearch = False
    End Sub

    Public Property class_error() As String
        Get
            class_error = aError
        End Get
        Set(ByVal value As String)
            aError = value
        End Set
    End Property

    Public Property FieldName() As String
        Get
            Return pFieldName
        End Get
        Set(ByVal value As String)
            pFieldName = value
        End Set
    End Property
    Public Property FieldDisplay() As String
        Get
            Return pFieldDisplay
        End Get
        Set(ByVal value As String)
            pFieldDisplay = value
        End Set
    End Property
    Public Property OperatorChoice() As String
        Get
            Return pOper
        End Get
        Set(ByVal value As String)
            pOper = value
        End Set
    End Property
    Public Property DataType() As String
        Get
            Return pDataType
        End Get
        Set(ByVal value As String)
            pDataType = value
        End Set
    End Property
    Public Property SearchValue() As String
        Get
            Return pVal
        End Get
        Set(ByVal value As String)
            pVal = value
        End Set
    End Property
    Public Property SpecialConsideration() As Boolean
        Get
            Return pSpecialConsideration
        End Get
        Set(ByVal value As Boolean)
            pSpecialConsideration = value
        End Set
    End Property
    Public Property CommasAsDelimiters() As Boolean
        Get
            Return pCommasAsDelimiters
        End Get
        Set(ByVal value As Boolean)
            pCommasAsDelimiters = value
        End Set
    End Property
    Public Property CompanyContactSearch() As Boolean
        Get
            Return pCompanyContactSearch
        End Get
        Set(ByVal value As Boolean)
            pCompanyContactSearch = value
        End Set
    End Property

    Public Shared foundChild As New DropDownList
    Public Shared search_alt_comp_name As Boolean = False 'this is a special case in which we have to look and see if the alt name has been set to true. This sets as it iterates to true if checked so 
    'we can use it later on the comp name textbox
    Public Shared DoNotIncludeOverdue As Boolean = False 'this is yet another special case where we look to see if Do Not Include Overdue has been checked
    Public Shared DoNotIncludeRelationships As Boolean = False
    Public Shared acmaintItem_chk As Boolean = False
    Public Shared acmaintItem_chk1 As Boolean = False
    Public Shared FinalAttributeList As String = ""

    Public Shared Sub IterateThroughChildren(ByVal parent As Control, ByVal tabPanelID As String, ByRef Query_Class_Array As System.Collections.ArrayList)

        Try

            Dim AttributeList As String = ""
            Dim OperatorChosen As String = ""

            For Each c As Control In parent.Controls

                If TypeOf c Is TextBox Then

                    Dim temporaryTextBox As TextBox = c
                    Dim comparedIDString As String = "COMPARE_" & c.ID.ToString

                    If foundChild.ID.ToString = comparedIDString Then
                        If foundChild.SelectedValue <> "" Then
                            OperatorChosen = foundChild.SelectedValue
                        End If
                        'Testing, setting the operator in session
                        If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & comparedIDString)) Then
                            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & comparedIDString)) Then
                                HttpContext.Current.Session.Item("Advanced-" & comparedIDString) = OperatorChosen
                            End If
                        Else
                            If OperatorChosen <> "" And temporaryTextBox.Text <> "" Then
                                HttpContext.Current.Session.Item("Advanced-" & comparedIDString) = OperatorChosen
                            End If
                        End If
                    End If


                    If Trim(temporaryTextBox.Text) <> "" Then
                        If OperatorChosen <> "" Then

                            Dim QueryData As New AdvancedQueryResults
                            QueryData.CommasAsDelimiters = True
                            QueryData.FieldName = UnescapeSpecialCharactersInSearchIDs(temporaryTextBox.ID) 'Ar(2).ToString
                            QueryData.OperatorChoice = OperatorChosen.ToString
                            QueryData.DataType = temporaryTextBox.ValidationGroup 'Ar(0).ToString
                            QueryData.SearchValue = temporaryTextBox.Text
                            QueryData.SearchValue = QueryData.SearchValue.TrimEnd()
                            QueryData.SearchValue = QueryData.SearchValue.TrimStart()

                            If QueryData.FieldName = "contact_last_name" Then
                                QueryData.SearchValue = Replace(QueryData.SearchValue, "'", "&apos;")
                                QueryData.CommasAsDelimiters = False
                            End If

                            QueryData.SearchValue = clsGeneral.clsGeneral.StripChars(QueryData.SearchValue, False) 'added in a special check to remove some special characters.



                            If UCase(tabPanelID) = "COMPANY/CONTACT" Then
                                QueryData.CompanyContactSearch = True
                            End If

                            'This has been added in
                            'Almost as a check.
                            'What it's going to do is this: 
                            'If the data type is either numeric, date or year
                            'it is going to check and see if you have a : in your statement
                            'if you have an : in your statement
                            'it's going to force you to use a BETWEEN.
                            If UCase(QueryData.DataType) = "DATE" Or UCase(QueryData.DataType) = "NUMERIC" Or UCase(QueryData.DataType) = "YEAR" Then
                                If InStr(QueryData.SearchValue, ":") > 0 Or InStr(QueryData.SearchValue, ";") > 0 Then
                                    QueryData.OperatorChoice = "Between"
                                End If
                            End If

                            'Note: In search fields where we use a between and we recommend a ":" and we still just recommend the “:” but also handle as ";" in case they mistype?
                            If UCase(QueryData.OperatorChoice) = "BETWEEN" Then
                                QueryData.SearchValue = Replace(Trim(temporaryTextBox.Text), ";", ":") '.ToString
                            End If

                            QueryData.FieldDisplay = temporaryTextBox.ToolTip
                            QueryData.SpecialConsideration = False
                            If temporaryTextBox.ID = "comp_name" Then 'this is a special case in which we have to look and see if the alt name has been set to true.
                                If search_alt_comp_name Then
                                    QueryData.SpecialConsideration = True
                                End If
                            End If
                            'This is a special case on the engine tab where you either check the do not include overdue or not.
                            If temporaryTextBox.ID = "engine_ac_hours" Then
                                If DoNotIncludeOverdue Then
                                    QueryData.SpecialConsideration = True
                                End If
                            End If

                            Query_Class_Array.Add(QueryData)

                        End If
                    End If
                    'Testing
                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID)) Then
                            HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID) = temporaryTextBox.Text
                        End If
                    Else
                        If temporaryTextBox.Text <> "" Then
                            HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID) = temporaryTextBox.Text
                        End If
                    End If

                    temporaryTextBox.Dispose()
                ElseIf TypeOf c Is CheckBox Then
                    'There are two checkbox fields that we care about on the company advanced search/engine page.
                    Dim temporaryCheckbox As CheckBox = c
                    'If temporaryCheckbox.Checked Then
                    If temporaryCheckbox.ID = "comp_alt_name" Then
                        search_alt_comp_name = temporaryCheckbox.Checked 'True
                    End If

                    If temporaryCheckbox.ID = "EngineNoOverdue" Then
                        DoNotIncludeOverdue = temporaryCheckbox.Checked 'True
                    End If

                    If temporaryCheckbox.ID = "comp_not_in_selected" Then
                        DoNotIncludeRelationships = temporaryCheckbox.Checked ' True
                    End If

                    If temporaryCheckbox.ID = "acmaint_chk" Then
                        acmaintItem_chk = temporaryCheckbox.Checked ' True
                    End If

                    If temporaryCheckbox.ID = "acmaint_chk1" Then
                        acmaintItem_chk1 = temporaryCheckbox.Checked ' True
                    End If

                    If temporaryCheckbox.ID = "comp_active_flag" Then
                        If temporaryCheckbox.Checked Then
                            Dim QueryData As New AdvancedQueryResults
                            QueryData.CommasAsDelimiters = True
                            QueryData.FieldName = temporaryCheckbox.ID
                            QueryData.OperatorChoice = "Equals"
                            QueryData.DataType = temporaryCheckbox.ValidationGroup

                            If UCase(tabPanelID) = "COMPANY/CONTACT" Then
                                QueryData.CompanyContactSearch = True
                            End If

                            QueryData.SearchValue = "Y"
                            QueryData.FieldDisplay = temporaryCheckbox.ToolTip

                            Query_Class_Array.Add(QueryData)
                        End If
                    End If
                    ' End If

                    'These are the attributes in the last tab panel.
                    'These fill a temporary variable called Attribute list, which is then added to finalattributelist.
                    'This builds it's own QueryData manually in the BuildDynamicString function after we're done iterating.
                    'This is protected from going out to live by checking for local/test website types.
                    'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                    If InStr(temporaryCheckbox.ID, "Attribute_") > 0 Then
                        If temporaryCheckbox.Checked Then
                            'Attribute here
                            AttributeList = Replace(temporaryCheckbox.ID, "Attribute_", "") & ","
                        End If
                    End If
                    'End If

                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & temporaryCheckbox.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & temporaryCheckbox.ID)) Then
                            HttpContext.Current.Session.Item("Advanced-" & temporaryCheckbox.ID) = temporaryCheckbox.Checked.ToString
                        End If
                    Else
                        If temporaryCheckbox.Checked Then
                            HttpContext.Current.Session.Item("Advanced-" & temporaryCheckbox.ID) = temporaryCheckbox.Checked.ToString
                        End If
                    End If

                ElseIf TypeOf c Is ListBox Then
                    'These are the company advanced search listboxes. 

                    Dim TemporaryListbox As ListBox = c
                    Dim SelectedString As String = ""
                    SelectedString = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(TemporaryListbox, True, 0, False)
                    OperatorChosen = "Equals"

                    If TemporaryListbox.ID = "cref_contact_type" Then
                        SelectedString = SelectedString.Replace("'Y','O'", "'Y'&#45;'O'")
                        SelectedString = SelectedString.Replace("'00','97','17','08','16'", "'00'&#45;'97'&#45;'17'&#45;'08'&#45;'16'")
                        SelectedString = SelectedString.Replace("'93','98','99','38','2X'", "'93'&#45;'98'&#45;'99'&#45;'38'&#45;'2X'")
                    End If

                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & TemporaryListbox.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & TemporaryListbox.ID)) Then
                            HttpContext.Current.Session.Item("Advanced-" & TemporaryListbox.ID) = Replace(Replace(SelectedString, "'", ""), ",", "##")
                        End If
                    Else
                        If OperatorChosen <> "" And SelectedString <> "" Then
                            HttpContext.Current.Session.Item("Advanced-" & TemporaryListbox.ID) = Replace(Replace(SelectedString, "'", ""), ",", "##")
                        End If
                    End If

                    If TemporaryListbox.ID = "cref_contact_type" Then
                        SelectedString = Replace(SelectedString, "&#45;", ",")
                        HttpContext.Current.Session.Item("Advanced-" & TemporaryListbox.ID) = Replace(HttpContext.Current.Session.Item("Advanced-" & TemporaryListbox.ID), "&#45;", ",")
                    End If

                    If SelectedString <> "" Then

                        Dim QueryData As New AdvancedQueryResults
                        QueryData.CommasAsDelimiters = True
                        QueryData.FieldName = UnescapeSpecialCharactersInSearchIDs(TemporaryListbox.ID)
                        QueryData.OperatorChoice = OperatorChosen.ToString
                        QueryData.DataType = TemporaryListbox.ValidationGroup


                        If UCase(tabPanelID) = "COMPANY/CONTACT" Then
                            QueryData.CompanyContactSearch = True
                        End If

                        'Time to loop through listbox to get values.
                        QueryData.SearchValue = SelectedString
                        QueryData.FieldDisplay = TemporaryListbox.ToolTip

                        If TemporaryListbox.ID = "cref_contact_type" Then
                            If DoNotIncludeRelationships Then
                                QueryData.SpecialConsideration = True
                            End If
                        End If

                        Query_Class_Array.Add(QueryData)
                    End If

                ElseIf TypeOf c Is DropDownList Then
                    Dim temporaryDropdownList As DropDownList = c
                    Dim comparedIDString As String = "COMPARE_" & c.ID.ToString

                    If Not IsNothing(foundChild.ID) Then
                        If foundChild.ID.ToString = comparedIDString Then

                            If foundChild.SelectedValue <> "" Then
                                OperatorChosen = foundChild.SelectedValue
                            End If

                            'Setting the operator in session
                            If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & comparedIDString)) Then
                                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & comparedIDString)) Then
                                    HttpContext.Current.Session.Item("Advanced-" & comparedIDString) = OperatorChosen
                                End If
                            Else
                                If OperatorChosen <> "" And temporaryDropdownList.Text.ToString <> "" Then
                                    HttpContext.Current.Session.Item("Advanced-" & comparedIDString) = OperatorChosen
                                End If
                            End If


                            If temporaryDropdownList.SelectedValue <> "" Then
                                If OperatorChosen <> "" Then

                                    Dim QueryData As New AdvancedQueryResults
                                    QueryData.CommasAsDelimiters = False

                                    If temporaryDropdownList.ID = "engine_mfr_name_static" Then
                                        QueryData.FieldName = "(SELECT top 1 e2.em_mfr_name FROM Aircraft_Model am2 WITH (NOLOCK) INNER JOIN Aircraft a2 WITH (NOLOCK) ON ac_amod_id = amod_id INNER JOIN Engine_Models e2 WITH (NOLOCK) ON e2.em_engine_name = a2.ac_engine_name WHERE am2.amod_customer_flag = 'Y' AND am2.amod_product_business_flag = 'Y' AND am2.amod_type_code = 'J' AND a2.ac_journ_id = 0 AND a2.ac_lifecycle_stage = 3 AND a2.ac_id = View_Aircraft_Flat.ac_id)"
                                    Else
                                        QueryData.FieldName = UnescapeSpecialCharactersInSearchIDs(temporaryDropdownList.ID) 'Ar(2).ToString
                                    End If

                                    QueryData.OperatorChoice = OperatorChosen.ToString
                                    QueryData.DataType = temporaryDropdownList.ValidationGroup 'Ar(0).ToString
                                    QueryData.SearchValue = temporaryDropdownList.SelectedValue 'temporaryDropdownList.Text.ToString
                                    QueryData.FieldDisplay = temporaryDropdownList.ToolTip

                                    If UCase(tabPanelID) = "COMPANY/CONTACT" Then
                                        QueryData.CompanyContactSearch = True
                                    End If

                                    HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID) = temporaryDropdownList.SelectedValue 'temporaryDropdownList.Text.ToString
                                    Query_Class_Array.Add(QueryData)

                                End If

                            End If

                            'Testing
                            If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID)) Then
                                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID)) Then
                                    HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID) = temporaryDropdownList.Text.ToString
                                End If
                            Else
                                If temporaryDropdownList.Text.ToString <> "" Then
                                    HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID) = temporaryDropdownList.Text.ToString
                                End If
                            End If

                            temporaryDropdownList.Dispose()
                        Else
                            foundChild = c
                        End If
                    Else
                        foundChild = c
                    End If

                End If

                If TypeOf c Is DropDownList Then
                    foundChild = c
                End If

                If c.Controls.Count > 0 Then
                    IterateThroughChildren(c, tabPanelID, Query_Class_Array)
                End If
            Next
            'Appending the checked attribute to the final list of attributes that are publicly shared.
            FinalAttributeList += AttributeList
        Catch ex As Exception
            ' Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (" & ErrorReportingTypeString & "):  " & ex.Message)
        End Try

    End Sub


    ''' <summary>
    ''' Sets up the dynamic query string generation
    ''' Added error reporting.
    ''' </summary>
    ''' <param name="AircraftTextStringDisplay"></param>
    ''' <param name="FinancialInstitution"></param>
    ''' <param name="FinancialDate"></param>
    ''' <param name="totalcounthold"></param>
    ''' <param name="counter"></param>
    ''' <param name="DoNotSearchPrevRegNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks> 
    Public Shared Function BuildDynamicString(ByRef aclsDataTemp As clsData_Manager_SQL, ByRef Query_Class_Array As System.Collections.ArrayList,
                                              ByRef advanced_search As AjaxControlToolkit.TabContainer, ByRef AircraftTextStringDisplay As String,
                                              ByRef FinancialInstitution As TextBox, ByRef FinancialDate As TextBox, ByRef static_folder_ac_ids As TextBox,
                                              ByRef static_folder As TextBox, ByVal totalcounthold As Long, ByVal counter As Long, ByRef DoNotSearchPrevRegNo As Boolean,
                                              ByRef ErrorReportingTypeString As String, Optional ByRef FinancialDocType As TextBox = Nothing, Optional ByRef srchMaintenanceItems As searchMaintenanceItems = Nothing, Optional ByRef check_notin_company As Boolean = False, Optional ByRef amod_id_list As String = "")
        Dim DynamicQueryString As String = ""
        Dim ReturnedFullQueryString As String = ""
        Dim DynamicCompanyQueryString As String = ""
        Dim DoNotAppendAnd As Boolean = False
        Dim split_temp_hours_low As String = ""
        Dim split_temp_hours_high As String = ""

        'Clear this session variable.
        'This is just testing for now.
        HttpContext.Current.Session.Item("MasterAircraftCompany") = ""
        Try
            Dim TemporaryTable As New DataTable
            'We need to clear the final attribute variable
            FinalAttributeList = ""

            'First we check for each tab panel.
            For Each tabpanel In advanced_search.Controls.OfType(Of AjaxControlToolkit.TabPanel)()
                ' If tabpanel.ID.ToString <> "company_contact" Then
                'then we build the class array for the advanced search
                IterateThroughChildren(tabpanel, tabpanel.HeaderText.ToString, Query_Class_Array)
                ' End If
            Next

            'After we loop through the Advanced Search Controls, we still have one more query class to add to the array.
            'This one is for the attributes. Since this is built differently (it builds a variable called FinalAttributeList as it loops along 
            'creates an in clause variable) we have to add it manually at the end after all the others.
            'This is protected however, to only show up on test.
            'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            Dim QueryData As New AdvancedQueryResults
            QueryData.CommasAsDelimiters = True
            QueryData.FieldName = "attributes"
            QueryData.OperatorChoice = "Equals"
            QueryData.DataType = "String"

            QueryData.SearchValue = FinalAttributeList.TrimEnd(",")
            QueryData.FieldDisplay = "List of Attributes"

            Query_Class_Array.Add(QueryData)
            ' End If





            'This is going through the class arraylist that's built


            totalcounthold = Query_Class_Array.Count
            For Each Query As AdvancedQueryResults In Query_Class_Array

                DoNotAppendAnd = False 'This operator is set for financial 
                DynamicQueryString = "" 'We're going to clear the query string here
                'Near the bottom we will append it 
                'To either the full Returned query string or both the full returned query string and the dynamic company string ac_aport_iata_code  ac_aport_icao_code
                'based on what we're dealing with.
                If Query.FieldName <> "" And Query.SearchValue <> "" And Query.FieldName <> "ym_mfr_comp_id" And Query.FieldName <> "SwapAttributeType" And Query.FieldName <> "yt_engine_manufacturer" And Query.FieldName <> "yt_engine_model" Then
                    If Query.FieldName <> "comp_name" And Query.FieldName <> "ac_engine_name_search" Then
                        Query.SearchValue = clsGeneral.clsGeneral.CleanUserData(Query.SearchValue, Constants.cEmptyString, Constants.cCommaDelim, True)
                    End If

                    If Query.FieldName = "FinancialInstitution" Then
                        DoNotAppendAnd = True
                        TemporaryTable = aclsDataTemp.Get_Financial_Institution_Primary_Group(Query.SearchValue.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote))
                        If Not IsNothing(TemporaryTable) Then
                            If TemporaryTable.Rows.Count = 1 Then
                                FinancialInstitution.Text = TemporaryTable.Rows(0).Item("fipg_comp_id_in_clause")
                            End If
                        End If
                        TemporaryTable.Dispose()
                        TemporaryTable = Nothing
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        totalcounthold -= 1
                    ElseIf Query.FieldName = "adoc_doc_date" Then
                        DoNotAppendAnd = True
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        FinancialDate.Text = Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, True, Query.FieldName, Query.CommasAsDelimiters)
                        totalcounthold -= 1
                    ElseIf Query.FieldName = "adoc_doc_type" Then
                        DoNotAppendAnd = True
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        FinancialDocType.Text = Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                        totalcounthold -= 1
                    ElseIf Query.FieldName = "maintenance_item" Then

                        DoNotAppendAnd = True
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        srchMaintenanceItems.Maintenance_item1 = Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                        srchMaintenanceItems.Maintenance_chk1 = acmaintItem_chk.ToString
                        totalcounthold -= 1

                    ElseIf Query.FieldName = "acmaint_date" Then

                        DoNotAppendAnd = True
                        srchMaintenanceItems.Maintenance_date1 = Query.SearchValue.Replace("'", "")
                        totalcounthold -= 1

                    ElseIf Query.FieldName = "acmaint_time" Then

                        DoNotAppendAnd = True
                        srchMaintenanceItems.Maintenance_time1 = Query.SearchValue.Replace("'", "")
                        totalcounthold -= 1

                    ElseIf Query.FieldName = "acmaint_value" Then

                        DoNotAppendAnd = True
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        srchMaintenanceItems.Maintenance_value1 = Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                        totalcounthold -= 1

                    ElseIf Query.FieldName = "maintenance_item1" Then

                        DoNotAppendAnd = True
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        srchMaintenanceItems.Maintenance_item2 = Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                        srchMaintenanceItems.Maintenance_chk2 = acmaintItem_chk.ToString
                        totalcounthold -= 1

                    ElseIf Query.FieldName = "acmaint_date1" Then

                        DoNotAppendAnd = True
                        srchMaintenanceItems.Maintenance_date2 = Query.SearchValue.Replace("'", "")
                        totalcounthold -= 1

                    ElseIf Query.FieldName = "acmaint_time1" Then

                        DoNotAppendAnd = True
                        srchMaintenanceItems.Maintenance_time2 = Query.SearchValue.Replace("'", "")
                        totalcounthold -= 1

                    ElseIf Query.FieldName = "acmaint_value1" Then

                        DoNotAppendAnd = True
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        srchMaintenanceItems.Maintenance_value2 = Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                        totalcounthold -= 1

                    ElseIf Query.FieldName = "attributes" Then
                        'We need to take our field value list and split it. It's longs seperated by commas. 
                        'Each ID seperated needs to be looked up and added to the query.
                        Dim RunningAttributeNames As String = "" 'This variable is used for displaying the attribute names in the search list.
                        'That way we don't have to refer to them as their IDs.
                        Dim AppendableQueryString As String = "" 'I basically want to keep the attributes queries put into this seperate appendable query string
                        'So that way when we're done looping, we can seperate them a little bit and end them to the main query.
                        'Plus it will be a little easier to debug.
                        Dim TemporarySplitArray() As String = Split(Query.SearchValue, ",")
                        For Each mSel As String In TemporarySplitArray
                            Dim temporaryID As Long = 0
                            Dim resultsTable As New DataTable
                            temporaryID = CLng(mSel)
                            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                                resultsTable = GetYachtTopicQueryByID(temporaryID)
                            Else
                                resultsTable = aclsDataTemp.GetTopicQueryByID(temporaryID)
                            End If

                            If Not IsNothing(resultsTable) Then
                                If resultsTable.Rows.Count > 0 Then
                                    'We've got a query result, so let's see what it is.
                                    If Not IsDBNull(resultsTable.Rows(0).Item("MYQUERY")) Then
                                        Dim queryResult As String = ""
                                        queryResult = resultsTable.Rows(0).Item("MYQUERY").ToString
                                        If Not String.IsNullOrEmpty(queryResult) Then
                                            If AppendableQueryString <> "" Then
                                                AppendableQueryString += " and "
                                            End If

                                            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                                                AppendableQueryString += " yt_id in (" & queryResult & ")"
                                            Else
                                                AppendableQueryString += " ac_id in (" & queryResult & ")"
                                            End If
                                        End If
                                    End If
                                    If Not IsDBNull(resultsTable.Rows(0).Item("TOPIC")) Then
                                        If RunningAttributeNames <> "" Then
                                            RunningAttributeNames += ", "
                                        End If
                                        RunningAttributeNames += resultsTable.Rows(0).Item("TOPIC").ToString
                                    End If
                                End If
                            End If

                            resultsTable.Dispose()
                        Next

                        If RunningAttributeNames <> "" Then
                            AircraftTextStringDisplay += "Selected Attribute List: " & RunningAttributeNames & "<br />"
                        End If

                        If AppendableQueryString <> "" Then
                            AppendableQueryString = "(" & AppendableQueryString & ")"
                            DynamicQueryString += AppendableQueryString
                        End If
                        counter += 1
                    ElseIf Query.FieldName = "int_ext_generic_data_description" Then
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"

                        DynamicQueryString += " ac_id in (select distinct adet_ac_id from aircraft_details with (NOLOCK) "
                        DynamicQueryString += " where adet_data_type in ('Interior','Exterior') and adet_data_description like '%" & Query.SearchValue & "%' and adet_journ_id=0)"

                        counter += 1
                    ElseIf Query.FieldName = "cref_comp_id" Then

                        Dim folderIDString As String = BuildOperatorsCompanyID(Query.SearchValue, aclsDataTemp)

                        If folderIDString <> "" Then
                            DynamicQueryString += " comp_id in (" & folderIDString & ")"
                        End If

                    ElseIf Query.FieldName = "cref_business_type" Then

                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        DynamicQueryString += " (EXISTS (SELECT NULL FROM Business_Type_Reference "
                        DynamicQueryString += " WITH(NOLOCK) where (bustypref_comp_id = comp_id and bustypref_journ_id = 0) "
                        DynamicQueryString += " and bustypref_type "

                        DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "bustypref_type", Query.CommasAsDelimiters)


                        DynamicQueryString += "  ))"

                        counter += 1
                    ElseIf Query.FieldName = "yt_compliance_type" Then
                        'This text string display gets set during the search click. It does that so that we can get the actual text of the dropdown instead of the ID.
                        'If we displayed the ID, it doesn't do a lot of good. Telling the user the compliance type equals 3 isn't really so great, but saying that it's COLREGS is much easier
                        'To understand.
                        ' AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Replace(Query.SearchValue, ":", " and "), "*", "%") & "<br />"

                        If Trim(HttpContext.Current.Session.Item("IS_YACHT_HISTORY")) = "Y" Then
                            DynamicQueryString += " yt_id in (select distinct yc_yt_id from Yacht_Compliance with (NOLOCK) where yc_journ_id > 0 and yc_cert_id " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "yc_cert_id", Query.CommasAsDelimiters) & ")"
                        Else
                            DynamicQueryString += " yt_id in (select distinct yc_yt_id from Yacht_Compliance with (NOLOCK) where yc_journ_id = 0 and yc_cert_id " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "yc_cert_id", Query.CommasAsDelimiters) & ")"
                        End If



                    ElseIf Query.FieldName = "yt_confidential_notes" Or Query.FieldName = "yt_charter_availability" Or Query.FieldName = "yt_charter_duration" Then
                        Query.OperatorChoice = "Includes"
                        Dim tempString As String = ""

                        Query.SearchValue = Replace(Query.SearchValue, "'", "")
                        Dim arraySplit As Array = Split(Query.SearchValue, ",")

                        If UBound(arraySplit) > 0 Then
                            For arraySplitCount = 0 To UBound(arraySplit)
                                If tempString <> "" Then
                                    tempString += ","
                                End If

                                tempString += "*" & arraySplit(arraySplitCount) & "*"
                            Next
                            Query.CommasAsDelimiters = True
                        Else
                            tempString = Query.SearchValue
                            DynamicQueryString += " yt_confidential_notes "
                        End If

                        Query.SearchValue = tempString

                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Replace(Query.SearchValue, ":", " and "), "*", "%") & "<br />"

                        DynamicQueryString += clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "yt_confidential_notes", Query.CommasAsDelimiters)

                    ElseIf Query.FieldName = "yt_home_port_id" Or Query.FieldName = "yt_lying_port_id" Or Query.FieldName = "yt_port_registered_id" Then
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        DynamicQueryString += " ( " & Query.FieldName & " in (select yp_id from Yacht_Port where yp_world_port_id "

                        DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "bustypref_type", True)


                        DynamicQueryString += "  ))"


                    ElseIf Query.FieldName = "avionics_generic_data_description" Then
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        DynamicQueryString += " ( "
                        DynamicQueryString += " ac_id in (select distinct av_ac_id from aircraft_avionics with (NOLOCK) "
                        DynamicQueryString += " where av_description like '%" & Query.SearchValue & "%' and av_ac_journ_id=0)"

                        DynamicQueryString += " or ac_id in (select distinct adet_ac_id from aircraft_details with (NOLOCK) "
                        DynamicQueryString += " where adet_data_type in ('Addl Cockpit Equipment') and adet_data_description like '%" & Query.SearchValue & "%' and adet_journ_id=0)"

                        DynamicQueryString += " ) "
                        counter += 1
                    ElseIf Query.FieldName = "eqp_main_generic_data_description" Then
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"

                        DynamicQueryString += " ac_id in (select distinct adet_ac_id from aircraft_details with (NOLOCK) "
                        DynamicQueryString += " where adet_data_type in ('Equipment','Maintenance') and adet_data_description like '%" & Query.SearchValue & "%' and adet_journ_id=0)"

                        counter += 1
                    ElseIf Query.FieldName = "generic_data_description" Then
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        DynamicQueryString += " ( "

                        DynamicQueryString += " ac_id in (select distinct adet_ac_id from aircraft_details with (NOLOCK) "
                        DynamicQueryString += " where adet_data_description like '%" & Query.SearchValue & "%' and adet_journ_id=0)"

                        DynamicQueryString += " or ac_id in (select distinct av_ac_id from aircraft_avionics with (NOLOCK) where (av_description like '%" & Query.SearchValue & "%' and av_ac_journ_id=0))"
                        DynamicQueryString += " ) "
                        counter += 1
                    ElseIf Query.FieldName = "amod_number_of_passengers" Then
                        AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                        DynamicQueryString += "( "

                        'a.	and ((ac_passenger_count > 10)
                        'b.	or (ac_passenger_Count is NULL and amod_number_of_passengers > 10))

                        DynamicQueryString += " ( "
                        If InStr(Query.SearchValue, "*") = 0 Then
                            DynamicQueryString += " ac_passenger_count " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_passenger_count", Query.CommasAsDelimiters)
                        Else
                            DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_passenger_count", Query.CommasAsDelimiters)
                        End If
                        DynamicQueryString += " ) "

                        DynamicQueryString += " or ( ac_passenger_Count is NULL and "

                        'Regular Amod # of passengers
                        If InStr(Query.SearchValue, "*") = 0 Then
                            DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                        Else
                            DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                        End If

                        DynamicQueryString += " ) "
                        DynamicQueryString += " )"
                        counter += 1

                    Else

                        If Query.FieldName = "comp_zip_code" Then
                            If InStr(Query.SearchValue, "-") > 0 Then
                                Query.OperatorChoice = "Equals"
                            End If
                        End If

                        If Query.FieldName = "ac_reg_no_search" Then
                            'AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                            DynamicQueryString += "( "
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Replace(Query.SearchValue, "-", ""), Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Replace(Query.SearchValue, "-", ""), Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If

                            If DoNotSearchPrevRegNo = False Then
                                AircraftTextStringDisplay += "Previous Reg #" & " " & Query.OperatorChoice & " " & Replace(Replace(Query.SearchValue, ":", " and "), "*", "") & "<br />"

                                If InStr(Query.SearchValue, "*") = 0 Then
                                    DynamicQueryString += " or ac_prev_reg_no " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_prev_reg_no", Query.CommasAsDelimiters)
                                Else
                                    DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_prev_reg_no", Query.CommasAsDelimiters)
                                End If

                            End If
                            DynamicQueryString += " )"
                            counter += 1
                        ElseIf Query.FieldName = "ac_engine_prop_ser_from" Or Query.FieldName = "ac_propeller_prop_ser_from" Then
                            Dim TypeString As String = "Engine"
                            If Query.FieldName = "ac_propeller_prop_ser_from" Then
                                TypeString = "Prop"
                            End If

                            If InStr(Query.SearchValue, ":") > 0 Then
                                Query.OperatorChoice = "Between"
                            End If
                            AircraftTextStringDisplay += TypeString & " Ser# " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                            TypeString = LCase(TypeString)

                            DynamicQueryString += " ("

                            DynamicQueryString += " ac_" & TypeString & "_1_ser_no " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_" & TypeString & "_1_ser_no", Query.CommasAsDelimiters)
                            DynamicQueryString += " OR ac_" & TypeString & "_2_ser_no " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_" & TypeString & "_2_ser_no", Query.CommasAsDelimiters)
                            DynamicQueryString += " OR ac_" & TypeString & "_3_ser_no " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_" & TypeString & "_3_ser_no", Query.CommasAsDelimiters)
                            DynamicQueryString += " OR ac_" & TypeString & "_4_ser_no " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_" & TypeString & "_4_ser_no", Query.CommasAsDelimiters)


                            DynamicQueryString += " ) "

                            counter += 1
                        ElseIf Query.FieldName = "propeller_soh" Or Query.FieldName = "propeller_snew" Then
                            Dim TempFieldName As String = Replace(Query.FieldName, "propeller_", "")
                            If InStr(Query.SearchValue, ":") > 0 Then
                                Query.OperatorChoice = "Between"
                            End If
                            AircraftTextStringDisplay += "Propeller " & IIf(UCase(TempFieldName) = "SOH", UCase(TempFieldName), "TTSN") & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"

                            DynamicQueryString += " ("

                            DynamicQueryString += " ac_prop_1_" & TempFieldName & "_hrs " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_prop_1_" & TempFieldName & "_hrs", Query.CommasAsDelimiters)
                            DynamicQueryString += " OR ac_prop_2_" & TempFieldName & "_hrs " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_prop_2_" & TempFieldName & "_hrs", Query.CommasAsDelimiters)
                            DynamicQueryString += " OR ac_prop_3_" & TempFieldName & "_hrs " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_prop_3_" & TempFieldName & "_hrs", Query.CommasAsDelimiters)
                            DynamicQueryString += " OR ac_prop_4_" & TempFieldName & "_hrs " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ac_prop_4_" & TempFieldName & "_hrs", Query.CommasAsDelimiters)

                            DynamicQueryString += " ) "

                            counter += 1

                        ElseIf Query.FieldName = "engine_ac_hours" Then

                            AircraftTextStringDisplay += "Engine Times Within " & Query.SearchValue & " Hours of Next Overhaul " & IIf(Query.SpecialConsideration, " (Overdue not included)", "") & "<br />"

                            If Query.SpecialConsideration = False Then
                                DynamicQueryString += " ("
                                DynamicQueryString += " (ac_engine_1_soh_hrs IS NOT NULL AND ac_engine_1_tbo_hrs - ac_engine_1_soh_hrs <= " & Query.SearchValue & ") "
                                DynamicQueryString += " OR (ac_engine_2_soh_hrs IS NOT NULL AND ac_engine_2_tbo_hrs - ac_engine_2_soh_hrs <= " & Query.SearchValue & ") "

                                DynamicQueryString += " OR (ac_engine_3_soh_hrs IS NOT NULL AND ac_engine_3_tbo_hrs - ac_engine_3_soh_hrs <= " & Query.SearchValue & ") "
                                DynamicQueryString += " OR (ac_engine_4_soh_hrs IS NOT NULL AND ac_engine_4_tbo_hrs - ac_engine_4_soh_hrs <= " & Query.SearchValue & ") "
                                DynamicQueryString += " OR (ac_engine_1_soh_hrs IS NULL AND ac_engine_1_tbo_hrs - ac_engine_1_tot_hrs <= " & Query.SearchValue & ") "
                                DynamicQueryString += " OR (ac_engine_2_soh_hrs IS NULL AND ac_engine_2_tbo_hrs - ac_engine_2_tot_hrs <= " & Query.SearchValue & ") "
                                DynamicQueryString += " OR (ac_engine_3_soh_hrs IS NULL AND ac_engine_3_tbo_hrs - ac_engine_3_tot_hrs <= " & Query.SearchValue & ") "
                                DynamicQueryString += " OR (ac_engine_4_soh_hrs IS NULL AND ac_engine_4_tbo_hrs - ac_engine_4_tot_hrs <= " & Query.SearchValue & ") "
                                DynamicQueryString += " )"
                            ElseIf Query.SpecialConsideration = True Then
                                DynamicQueryString += " ("
                                DynamicQueryString += " (ac_engine_1_soh_hrs IS NOT NULL AND ac_engine_1_tbo_hrs - ac_engine_1_soh_hrs <= " & Query.SearchValue & " "
                                DynamicQueryString += " AND ac_engine_1_tbo_hrs - ac_engine_1_soh_hrs > 0) "
                                DynamicQueryString += " OR (ac_engine_2_soh_hrs IS NOT NULL AND ac_engine_2_tbo_hrs - ac_engine_2_soh_hrs <= " & Query.SearchValue & " "
                                DynamicQueryString += " AND ac_engine_2_tbo_hrs - ac_engine_2_soh_hrs > 0) "
                                DynamicQueryString += " OR (ac_engine_3_soh_hrs IS NOT NULL AND ac_engine_3_tbo_hrs - ac_engine_3_soh_hrs <= " & Query.SearchValue & " "
                                DynamicQueryString += " AND ac_engine_3_tbo_hrs - ac_engine_3_soh_hrs > 0) "
                                DynamicQueryString += " OR (ac_engine_4_soh_hrs IS NOT NULL AND ac_engine_4_tbo_hrs - ac_engine_4_soh_hrs <= " & Query.SearchValue & " "
                                DynamicQueryString += " AND ac_engine_4_tbo_hrs - ac_engine_4_soh_hrs > 0) "
                                DynamicQueryString += " OR (ac_engine_1_soh_hrs IS NULL AND ac_engine_1_tbo_hrs - ac_engine_1_tot_hrs <= " & Query.SearchValue & " "
                                DynamicQueryString += " AND ac_engine_1_tbo_hrs - ac_engine_1_tot_hrs > 0) "
                                DynamicQueryString += " OR (ac_engine_2_soh_hrs IS NULL AND ac_engine_2_tbo_hrs - ac_engine_2_tot_hrs <= " & Query.SearchValue & " "
                                DynamicQueryString += " AND ac_engine_2_tbo_hrs - ac_engine_2_tot_hrs > 0) "
                                DynamicQueryString += " OR (ac_engine_3_soh_hrs IS NULL AND ac_engine_3_tbo_hrs - ac_engine_3_tot_hrs <= " & Query.SearchValue & " "
                                DynamicQueryString += " AND ac_engine_3_tbo_hrs - ac_engine_3_tot_hrs > 0) "
                                DynamicQueryString += " OR (ac_engine_4_soh_hrs IS NULL AND ac_engine_4_tbo_hrs - ac_engine_4_tot_hrs <= " & Query.SearchValue & " "
                                DynamicQueryString += " AND ac_engine_4_tbo_hrs - ac_engine_4_tot_hrs > 0)"
                                DynamicQueryString += " )"
                            End If

                            counter += 1


                        ElseIf Query.FieldName = "engine_soh_hours" Then

                            If Trim(Query.SearchValue) <> "" Then


                                If Trim(Query.OperatorChoice) <> "" Then
                                    If Trim(Query.OperatorChoice) = "Equals" Then
                                        Query.OperatorChoice = " = "
                                    ElseIf Trim(Query.OperatorChoice) = "Greater Than" Then
                                        Query.OperatorChoice = " > "
                                    ElseIf Trim(Query.OperatorChoice) = "Less Than" Then
                                        Query.OperatorChoice = " < "
                                    ElseIf Trim(Query.OperatorChoice) = "Between" Then
                                        Query.OperatorChoice = "Between"
                                    End If
                                Else
                                    Query.OperatorChoice = " = "
                                End If

                                If InStr(Query.SearchValue, ":") > 0 Then
                                    Query.OperatorChoice = "Between"

                                    split_temp_hours_low = Left(Trim(Query.SearchValue), InStr(Trim(Query.SearchValue), ":", CompareMethod.Text) - 1)
                                    split_temp_hours_high = Right(Trim(Query.SearchValue), Len(Trim(Query.SearchValue)) - InStr(Trim(Query.SearchValue), ":", CompareMethod.Text))
                                End If

                                If Trim(Query.OperatorChoice) = "Between" Then
                                    DynamicQueryString += " ("
                                    DynamicQueryString += " (ac_engine_1_soh_hrs <= " & split_temp_hours_high & " and ac_engine_1_soh_hrs >= " & split_temp_hours_low & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_2_soh_hrs <= " & split_temp_hours_high & " and ac_engine_2_soh_hrs >= " & split_temp_hours_low & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_3_soh_hrs <= " & split_temp_hours_high & " and ac_engine_3_soh_hrs >= " & split_temp_hours_low & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_4_soh_hrs <= " & split_temp_hours_high & " and ac_engine_4_soh_hrs >= " & split_temp_hours_low & ") "
                                    DynamicQueryString += " )"
                                Else
                                    DynamicQueryString += " ("
                                    DynamicQueryString += " (ac_engine_1_soh_hrs " & Query.OperatorChoice & " " & Query.SearchValue & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_2_soh_hrs " & Query.OperatorChoice & " " & Query.SearchValue & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_3_soh_hrs " & Query.OperatorChoice & " " & Query.SearchValue & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_4_soh_hrs " & Query.OperatorChoice & " " & Query.SearchValue & ") "
                                    DynamicQueryString += " )"
                                End If
                            End If

                        ElseIf Query.FieldName = "engine_shi_hours" Then


                            If Trim(Query.SearchValue) <> "" Then


                                If Trim(Query.OperatorChoice) <> "" Then
                                    If Trim(Query.OperatorChoice) = "Equals" Then
                                        Query.OperatorChoice = " = "
                                    ElseIf Trim(Query.OperatorChoice) = "Greater Than" Then
                                        Query.OperatorChoice = " > "
                                    ElseIf Trim(Query.OperatorChoice) = "Less Than" Then
                                        Query.OperatorChoice = " < "
                                    ElseIf Trim(Query.OperatorChoice) = "Between" Then
                                        Query.OperatorChoice = "Between"
                                    End If
                                Else
                                    Query.OperatorChoice = " = "
                                End If

                                If InStr(Query.SearchValue, ":") > 0 Then
                                    Query.OperatorChoice = "Between"

                                    split_temp_hours_low = Left(Trim(Query.SearchValue), InStr(Trim(Query.SearchValue), ":", CompareMethod.Text) - 1)
                                    split_temp_hours_high = Right(Trim(Query.SearchValue), Len(Trim(Query.SearchValue)) - InStr(Trim(Query.SearchValue), ":", CompareMethod.Text))
                                End If

                                If Trim(Query.OperatorChoice) = "Between" Then
                                    DynamicQueryString += " ("
                                    DynamicQueryString += " (ac_engine_1_shi_hrs <= " & split_temp_hours_high & " and ac_engine_1_shi_hrs >= " & split_temp_hours_low & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_2_shi_hrs <= " & split_temp_hours_high & " and ac_engine_2_shi_hrs >= " & split_temp_hours_low & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_3_shi_hrs <= " & split_temp_hours_high & " and ac_engine_3_shi_hrs >= " & split_temp_hours_low & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_4_shi_hrs <= " & split_temp_hours_high & " and ac_engine_4_shi_hrs >= " & split_temp_hours_low & ") "
                                    DynamicQueryString += " )"
                                Else
                                    DynamicQueryString += " ("
                                    DynamicQueryString += " (ac_engine_1_shi_hrs " & Query.OperatorChoice & " " & Query.SearchValue & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_2_shi_hrs " & Query.OperatorChoice & " " & Query.SearchValue & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_3_shi_hrs " & Query.OperatorChoice & " " & Query.SearchValue & ") "
                                    DynamicQueryString += " or "
                                    DynamicQueryString += " (ac_engine_4_shi_hrs " & Query.OperatorChoice & " " & Query.SearchValue & ") "
                                    DynamicQueryString += " )"
                                End If
                            End If


                        ElseIf Query.FieldName = "comp_name" Or Query.FieldName = "ac_engine_name_search" Then
                            If Query.FieldName = "comp_name" Then
                                Query.FieldName = "comp_name_search"
                            ElseIf Query.FieldName = "ac_engine_name_search" Then
                                Query.OperatorChoice = "Begins With"
                                Query.SearchValue = Replace(Query.SearchValue, ",", ";")
                            End If
                            Dim TempNameHold As String = ""
                            Dim TempCompHold As String = ""
                            'We're going to prep the value for this function by replacing commas with ;
                            TempCompHold = Replace(Query.SearchValue, ",", "_")
                            TempCompHold = clsGeneral.clsGeneral.CleanUserData(TempCompHold, Constants.cEmptyString, Constants.cCommaDelim, True)
                            TempCompHold = Replace(TempCompHold, ",", ";")

                            TempNameHold = clsGeneral.clsGeneral.FilterCompanyNameForCompanyAircraftSearch(TempCompHold)


                            Query.SearchValue = TempNameHold 'clsGeneral.clsGeneral.Get_Name_Search_String(Query.SearchValue)

                            DynamicQueryString += "( "
                            If Query.FieldName = "ac_engine_name_search" And InStr(Query.SearchValue, "*") = 0 Then   ' added MSW - 5/1/19
                                DynamicQueryString += " upper(replace(ac_engine_name_search, ' ', ''))  " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            ElseIf InStr(Query.SearchValue, "*") = 0 Then
                                ' ADDED MSW - 4/13/20 - if there is a comma and its comp name or alt name 
                                If (Query.FieldName = "comp_name_search") And InStr(Query.SearchValue, ",") > 0 Then
                                    DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                Else
                                    DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                End If
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If

                            AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                            If Query.FieldName = "comp_name_search" Then
                                If Query.SpecialConsideration = True Then
                                    AircraftTextStringDisplay += "Company Alternate Name" & " " & Query.OperatorChoice & " " & Replace(Replace(Query.SearchValue, ":", " and "), "*", "") & "<br />"

                                    If InStr(Query.SearchValue, "*") = 0 Then
                                        If (Query.FieldName = "comp_name_search") And InStr(Query.SearchValue, ",") > 0 Then ' ADDED MSW - 4/13/2020
                                            DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "comp_altname_search", Query.CommasAsDelimiters)
                                        Else
                                            DynamicQueryString += " or comp_altname_search " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "comp_altname_search", Query.CommasAsDelimiters)
                                        End If
                                    Else
                                        DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "comp_altname_search", Query.CommasAsDelimiters)
                                    End If

                                End If
                            End If

                            DynamicQueryString += " )"
                            counter += 1
                        ElseIf Query.FieldName = "comp_zip_code" Then

                            'display search text
                            AircraftTextStringDisplay += "Zip Code: " & " " & Query.OperatorChoice & " " & Replace(Replace(Query.SearchValue, ":", " and "), "*", "") & "<br />"

                            Query.OperatorChoice = "="
                            DynamicQueryString += " " & clsGeneral.clsGeneral.ZipCodePrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, True)


                        ElseIf Query.FieldName = "comp_phone_office" Then
                            'search company phone office
                            DynamicQueryString += "( "
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If
                            'display search text
                            AircraftTextStringDisplay += "Phone Number: " & " " & Query.OperatorChoice & " " & Replace(Replace(Query.SearchValue, ":", " and "), "*", "") & "<br />"

                            'search company phone fax
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " or comp_phone_fax " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "comp_phone_fax", Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "comp_phone_fax", Query.CommasAsDelimiters)
                            End If

                            'search company phone mobile
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " or comp_phone_mobile " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "comp_phone_mobile", Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "comp_phone_mobile", Query.CommasAsDelimiters)
                            End If

                            'search contact phone office
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " or contact_phone_office " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_phone_office", Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_phone_office", Query.CommasAsDelimiters)
                            End If

                            'search contact phone fax
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " or contact_phone_fax " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_phone_fax", Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_phone_fax", Query.CommasAsDelimiters)
                            End If

                            'search contact phone mobile
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " or contact_phone_mobile " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_phone_mobile", Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_phone_mobile", Query.CommasAsDelimiters)
                            End If

                            DynamicQueryString += " )"
                            counter += 1
                        ElseIf Query.FieldName = "comp_email_address" Then
                            'search company email address
                            DynamicQueryString += "( "
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If

                            'search contact email address
                            AircraftTextStringDisplay += "Email Address: " & " " & Query.OperatorChoice & " " & Replace(Replace(Query.SearchValue, ":", " and "), "*", "") & "<br />"

                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " or contact_email_address " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_email_address", Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " or " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_email_address", Query.CommasAsDelimiters)
                            End If

                            DynamicQueryString += " )"
                            counter += 1

                        ElseIf Query.FieldName = "contact_title" Then
                            DynamicQueryString += " " & " contact_id in ( " ' changed from comp_id - Msw - 2/20/19

                            DynamicQueryString += "select distinct contact_id from Contact " ' changed from contact_comp_id to contact_id - Msw 2/20/19
                            DynamicQueryString += "where contact_title in (SELECT ctitlegref_title_name FROM Contact_Title_Group_Reference WHERE(ctitlegref_group_name " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "ctitlegref_group_name", Query.CommasAsDelimiters) & "))"
                            DynamicQueryString += " and contact_journ_id = 0 and contact_active_flag='Y' and contact_hide_flag='N') "

                            'DynamicQueryString += " " & " contact_title IN (SELECT ctitlegref_title_name FROM Contact_Title_Group_Reference WHERE(ctitlegref_group_name " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, "contact_title", Query.CommasAsDelimiters) & "))"
                            AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                            counter += 1
                        ElseIf Query.FieldName = "cref_contact_type" Or Query.FieldName = "yr_contact_type" Then

                            DynamicQueryString += " ( "
                            Dim HasOperators As Boolean = False

                            If Query.FieldName = "yr_contact_type" And check_notin_company = True Then
                                DynamicQueryString += "  (NOT EXISTS (SELECT NULL FROM Yacht_Reference WITH (NOLOCK) WHERE (yr_yt_id = yt_id) AND (yr_journ_id = yt_journ_id) AND "
                            ElseIf Query.SpecialConsideration Then
                                DynamicQueryString += " (NOT EXISTS (SELECT NULL FROM Aircraft_Reference WITH (NOLOCK) WHERE (cref_ac_id = ac_id) AND (cref_journ_id = ac_journ_id) AND "
                            End If
                            DynamicQueryString += " ( "
                            If InStr(Query.SearchValue, "'Y'") > 0 Then
                                HasOperators = True
                                Query.SearchValue = Replace(Query.SearchValue, "''Y''", "") 'replace that option from the search value since we're using it to custom add operator flag.
                                DynamicQueryString += " cref_operator_flag IN ('Y', 'O') "
                                If Query.SearchValue <> "" Then
                                    DynamicQueryString += " or "
                                End If
                            End If

                            If InStr(Query.SearchValue, "'I'") > 0 Then
                                Query.SearchValue = Replace(Query.SearchValue, "''I''", "'00','97','17','08','16'") 'replace that option from the search value since we're using it to custom add operator flag.
                                DynamicQueryString += "comp_contact_address_flag = 'Y'"
                                If Not String.IsNullOrEmpty(Query.SearchValue.Trim) Then
                                    DynamicQueryString += Constants.cAndClause
                                End If
                            End If

                            If Not String.IsNullOrEmpty(Query.SearchValue.Trim) Then
                                If Query.FieldName = "yr_contact_type" Then
                                    AircraftTextStringDisplay += Query.FieldDisplay & " " & IIf(Query.SpecialConsideration, "NOT ", "") & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & IIf(HasOperators, " , Operators", "") & "<br />"
                                End If

                                If InStr(Query.SearchValue, "*") = 0 Then
                                    DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                Else
                                    DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                End If
                            ElseIf HasOperators Then
                                If Query.FieldName = "yr_contact_type" Then
                                    AircraftTextStringDisplay += Query.FieldDisplay & " " & IIf(Query.SpecialConsideration, "NOT ", "") & Query.OperatorChoice & "  Operators" & "<br />"
                                End If
                            End If

                            DynamicQueryString += " ) "

                            If Query.SpecialConsideration Or (Query.FieldName = "yr_contact_type" And check_notin_company = True) Then
                                DynamicQueryString += " )) "
                            End If


                            DynamicQueryString += " ) "
                            counter += 1
                        ElseIf Query.FieldName = "ac_id" Then
                            'First we need to run a small check,
                            'If this folder is static, proceed with saved IDs in textbox (this verifies that the id's are current, rather than what's cached.
                            If static_folder.Text = "true" Then
                                Query.SearchValue = static_folder_ac_ids.Text
                                If Not IsNothing(HttpContext.Current.Session.Item("Advanced-ac_id")) Then
                                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-ac_id")) Then
                                        'Reset the Static ID Session in just this case.
                                        HttpContext.Current.Session.Item("Advanced-ac_id") = static_folder_ac_ids.Text
                                    End If
                                End If
                            End If
                            'proceed like normal:
                            AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"

                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If

                            counter += 1
                        ElseIf Query.FieldName = "foreign_ac_maintained" Or Query.FieldName = "us_ac_maintained" Then
                            Query.FieldName = "ac_maintained"


                            If Query.SearchValue.ToUpper.Contains("IS NULL") Or Query.SearchValue.ToUpper.Contains("BLANK") Then

                                If Not Query.SearchValue.ToUpper.Contains("BLANK") Then
                                    AircraftTextStringDisplay += Query.FieldDisplay + " " + Query.SearchValue + "<br />"
                                    DynamicQueryString += " " + Query.FieldName + " " + Query.SearchValue
                                Else
                                    AircraftTextStringDisplay += Query.FieldDisplay + " = '' <br />"
                                    DynamicQueryString += " " + Query.FieldName + " = ''"
                                End If
                                counter += 1

                            Else

                                AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"

                                If InStr(Query.SearchValue, "*") = 0 Then
                                    DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                Else
                                    DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                End If
                                counter += 1
                            End If

                        ElseIf Query.FieldName = "comp_address1" Then
                            AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"

                            DynamicQueryString += " ("
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If

                            DynamicQueryString += " or "

                            Query.FieldName = "comp_address2"
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If

                            DynamicQueryString += " )"
                            counter += 1
                        ElseIf Query.FieldName = "ac_ser_no_full" Then
                            AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"
                            DynamicQueryString += "( "
                            'regular:
                            '((ac_ser_no_full = '5116') 
                            'OR (ac_ser_no = '5116') OR 
                            '(ac_ser_no_value = 5116) OR 

                            'alternate:
                            '(ac_alt_ser_no_full = '5116') OR 
                            '(ac_alt_ser_no = '5116') OR 
                            '(ac_alt_ser_no_value = 5116) 
                            'Regular Search Fields:
                            'Fields where it doesn't matter if it's numeric or not:
                            '1) ac ser no_full:
                            Dim TemporaryOnlyDigitsAndCommas As String = ""
                            TemporaryOnlyDigitsAndCommas = Regex.Replace(Query.SearchValue, "([^0-9,])", "")

                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If
                            DynamicQueryString += " or "
                            Query.FieldName = "ac_ser_no"

                            '2) ac_ser_no
                            If InStr(Query.SearchValue, "*") = 0 Then
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            Else
                                DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If

                            'We need to check on something
                            'If the TemporaryOnlyDigitsAndCommas is the same as the search value, then we can go ahead and complete this search
                            'This means the search didn't have anything except letters and commas
                            If TemporaryOnlyDigitsAndCommas = Query.SearchValue Then
                                DynamicQueryString += " or "
                                '3) Field where it needs to be numeric = ac_ser_no_value
                                Query.FieldName = "ac_ser_no_value"
                                Query.DataType = "Numeric"
                                'Query.OperatorChoice = "Equals"
                                DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, TemporaryOnlyDigitsAndCommas, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                            End If


                            'Change back to string:
                            Query.DataType = "String"
                            ' Query.OperatorChoice = "Begins With"

                            If Query.SpecialConsideration = False Then
                                DynamicQueryString += " or "
                                'Search alternate serial no.
                                Query.FieldName = "ac_alt_ser_no_full"
                                '1) ac_alt_ser_no_full:
                                If InStr(Query.SearchValue, "*") = 0 Then
                                    DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                Else
                                    DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                End If
                                DynamicQueryString += " or "
                                Query.FieldName = "ac_alt_ser_no"

                                '2) ac_alt_ser_no
                                If InStr(Query.SearchValue, "*") = 0 Then
                                    DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                Else
                                    DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                End If

                                'If the TemporaryOnlyDigitsAndCommas is the same as the search value, then we can go ahead and complete this search
                                'This means the search didn't have anything except letters and commas
                                If TemporaryOnlyDigitsAndCommas = Query.SearchValue Then
                                    DynamicQueryString += " or "
                                    '3) Field where it needs to be numeric = ac_alt_ser_no_value
                                    Query.FieldName = "ac_alt_ser_no_value"
                                    Query.DataType = "Numeric"
                                    ' Query.OperatorChoice = "Equals"
                                    DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, TemporaryOnlyDigitsAndCommas, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)

                                    Query.DataType = "String"
                                    ' Query.OperatorChoice = "Begins With"
                                End If
                            End If

                            DynamicQueryString += " ) "
                            counter += 1


                        Else

                            If Not (Query.FieldName = "acmaint_date" Or Query.FieldName = "acmaint_time" Or Query.FieldName = "acmaint_date1" Or Query.FieldName = "acmaint_time1") Then

                                ' make adjustment for "blank / null value"
                                If Query.SearchValue.ToUpper.Contains("IS NULL") Or Query.SearchValue.ToUpper.Contains("BLANK") Then

                                    If Not Query.SearchValue.ToUpper.Contains("BLANK") Then
                                        AircraftTextStringDisplay += Query.FieldDisplay + " " + Query.SearchValue + "<br />"

                                        DynamicQueryString += " " + Query.FieldName + " " + Query.SearchValue
                                    Else
                                        AircraftTextStringDisplay += Query.FieldDisplay + " = '' <br />"

                                        DynamicQueryString += " " + Query.FieldName + " = ''"

                                    End If

                                    counter += 1

                                Else

                                    AircraftTextStringDisplay += Query.FieldDisplay & " " & Query.OperatorChoice & " " & Replace(Query.SearchValue, ":", " and ") & "<br />"

                                    If InStr(Query.SearchValue, "*") = 0 Then
                                        DynamicQueryString += " " & Query.FieldName & " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                    Else
                                        DynamicQueryString += " " & clsGeneral.clsGeneral.PrepQueryString(Query.OperatorChoice, Query.SearchValue, Query.DataType, False, Query.FieldName, Query.CommasAsDelimiters)
                                    End If
                                    counter += 1

                                End If

                            End If

                        End If


                    End If
                    If DoNotAppendAnd = False Then
                        If totalcounthold <> counter Then
                            DynamicQueryString += " and "
                        End If
                    End If

                End If

                'This is appending the information to the full query string so that way we can go ahead and 
                'return it 
                ReturnedFullQueryString += DynamicQueryString

                If Query.CompanyContactSearch Then
                    'If this comes from a designated tab for company/contact, we need to store it seperately.
                    If Not DynamicQueryString.ToLower.Contains("lbfractionalprogram") Then
                        DynamicCompanyQueryString += DynamicQueryString
                    End If
                End If

            Next

            ' COMMENTED OUT , THOUGH SHOULD WORK - MSW - 12/3/19
            'If Trim(amod_id_list) <> "" Then
            '    If Trim(ReturnedFullQueryString) <> "" Then
            '        ReturnedFullQueryString &= " and "
            '    End If
            '    ReturnedFullQueryString &= " amod_id in (" & Trim(amod_id_list) & ") "
            'End If

            'We need to trim both the full query string
            ReturnedFullQueryString = Trim(ReturnedFullQueryString)
            'As well as the company one.
            DynamicCompanyQueryString = Trim(DynamicCompanyQueryString)

            'Trims the last and from the dynamically generated query string in case an extra one was added. 
            Dim MyChar() As Char = {"a", "n", "d"}
            ReturnedFullQueryString = ReturnedFullQueryString.TrimEnd(MyChar)
            'As well as doing it on the company side.
            DynamicCompanyQueryString = DynamicCompanyQueryString.TrimEnd(MyChar)

            'Adding a space to the end.
            If ReturnedFullQueryString <> "" Then
                ReturnedFullQueryString += " "
            End If
            'Doing the same on the dynamic Company query string.
            If DynamicCompanyQueryString <> "" Then
                DynamicCompanyQueryString += " "
            End If

            HttpContext.Current.Session.Item("MasterAircraftCompany") = DynamicCompanyQueryString

        Catch ex As Exception
            'aError = (System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (" & ErrorReportingTypeString & "): " & ex.Message)
        End Try
        Return ReturnedFullQueryString
    End Function

    Public Shared Function BuildOperatorsCompanyID(ByVal CompanyFolderID As Long, ByVal aclsDataTemp As clsData_Manager_SQL) As String
        Dim companyTable As New DataTable
        Dim companyList As String = ""
        Dim util_functions As New utilization_functions

        util_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        util_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        util_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        util_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        util_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


        companyTable = util_functions.get_data_from_client_folder(CompanyFolderID, "cfoldind_jetnet_comp_id")
        companyList = ""

        If Not IsNothing(companyTable) Then
            If companyTable.Rows.Count > 0 Then
                For Each r As DataRow In companyTable.Rows
                    If Trim(companyList) = "" Then
                        companyList = r("cfoldind_jetnet_comp_id")
                    Else
                        companyList = companyList & ", " & r("cfoldind_jetnet_comp_id")
                    End If

                Next
            Else
                'This is probably an active folder, let's check:
                companyTable = aclsDataTemp.GetEvolutionFolderssBySubscription(CompanyFolderID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "A")
                If Not IsNothing(companyTable) Then
                    If companyTable.Rows.Count > 0 Then
                        If Not IsDBNull(companyTable.Rows(0).Item("cfolder_data")) Then
                            Dim cfolderQuery As String() = Split(companyTable.Rows(0).Item("cfolder_data"), "THEREALSEARCHQUERY")
                            If UBound(cfolderQuery) >= 1 Then
                                Dim ToSplitBy As String = "WHERE  CREF_JOURN_ID = 0"
                                If InStr(UCase(cfolderQuery(1)), "FROM VIEW_AIRCRAFT_COMPANY_FLAT") = 0 Then
                                    ToSplitBy = "WHERE  COMP_JOURN_ID = 0"
                                End If

                                Dim queryToBreak As String() = Split(UCase(cfolderQuery(1)), ToSplitBy)
                                If UBound(queryToBreak) >= 1 Then
                                    Dim queryToRun As String = ""
                                    Dim queryWhere As String = ""
                                    Dim querySelect As String = "select distinct comp_id "
                                    Dim queryFrom As String = ""
                                    If InStr(queryToBreak(0), "FROM VIEW_AIRCRAFT_COMPANY_FLAT") = 0 Then
                                        queryFrom = "  From Company WITH(NOLOCK) "
                                        queryFrom += " LEFT OUTER JOIN State WITH(NOLOCK) on state_code = comp_state and state_country=comp_country"
                                        queryFrom += " LEFT OUTER JOIN Contact WITH(NOLOCK) ON (comp_id = contact_comp_id AND comp_journ_id = contact_journ_id and contact_hide_flag = 'N' and contact_active_flag = 'Y')"
                                        If InStr(queryToBreak(0), "INNER JOIN COUNTRY") Then
                                            queryFrom += " INNER JOIN Country WITH(NOLOCK) on comp_country = country_name "
                                        End If

                                        If InStr(queryToBreak(0), "INNER JOIN COUNTRY") Then
                                            queryFrom += " INNER JOIN Country WITH(NOLOCK) on comp_country = country_name "
                                        End If

                                        If InStr(queryToBreak(0), "INNER JOIN COUNTRY") Then
                                            queryFrom += " INNER JOIN Country WITH(NOLOCK) on comp_country = country_name "
                                        End If

                                        queryWhere = " WHERE comp_journ_id = 0 "
                                    Else
                                        queryFrom = " From View_Aircraft_Company_Flat "
                                        queryWhere = " WHERE CREF_JOURN_ID = 0"
                                    End If

                                    If InStr(queryToBreak(0), "INNER JOIN COMPANY_AIRCRAFT_COUNT WITH(NOLOCK)") > 0 Then
                                        Dim queryFleetChoice As String() = Split(queryToBreak(0), "INNER JOIN COMPANY_AIRCRAFT_COUNT WITH(NOLOCK)")
                                        If UBound(queryFleetChoice) > 0 Then
                                            queryFrom += " INNER JOIN COMPANY_AIRCRAFT_COUNT WITH(NOLOCK) " + queryFleetChoice(1)
                                        End If
                                    End If


                                    Dim querySplitOrder As String() = Split(UCase(queryToBreak(1)), " ORDER BY ")
                                    If UBound(querySplitOrder) >= 0 Then
                                        queryWhere += Replace(querySplitOrder(0), "''", "'")
                                        queryToRun = querySelect + queryFrom + queryWhere
                                        queryToRun = queryToRun
                                        'We need to run this query.
                                        Dim FolderIDData As DataTable = util_functions.RunActiveFolderCompanyID(queryToRun)
                                        If Not IsNothing(FolderIDData) Then
                                            If FolderIDData.Rows.Count > 0 Then
                                                companyList = ""
                                                For Each r As DataRow In FolderIDData.Rows
                                                    If companyList <> "" Then
                                                        companyList += ","
                                                    End If
                                                    companyList += r("comp_id").ToString
                                                Next
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return companyList
    End Function
    Public Shared Function EscapeSpecialCharactersInSearchIDs(ByVal IDString As String) As String
        Dim returnString As String = ""

        returnString = Replace(IDString, ")", "_RightParentheses_")
        returnString = Replace(returnString, "(", "_LeftParentheses_")
        returnString = Replace(returnString, "'", "_SQuote_")
        Return returnString
    End Function

    Public Shared Function UnescapeSpecialCharactersInSearchIDs(ByVal IDString As String) As String
        Dim returnString As String = ""

        returnString = Replace(IDString, "_RightParentheses_", ")")
        returnString = Replace(returnString, "_LeftParentheses_", "(")
        returnString = Replace(returnString, "_SQuote_", "'")
        Return returnString
    End Function
    ''' <summary>
    ''' This is a function that builds the dynamic region where string. Basically it takes your country, region and state string and
    ''' builds a dynamic set of OR's that are applicable to the search you're trying to do. This only happens if it's a region search though.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function BuildRegionWhereString(ByVal stateFieldName As String, ByVal CountryFieldName As String, ByRef aclsData_temp As clsData_Manager_SQL, ByVal StateNameString As String, ByVal CountriesNameString As String, ByVal RegionNameString As String) As String
        Dim TempBaseStateHold As String = Replace(StateNameString, "'", "")
        Dim StateArray As Array = Split(TempBaseStateHold, ",")
        Dim WhereClause As String = ""
        Dim TemporaryCountryHold As String = ""
        Dim CountriesArray As Array = Split(Replace(CountriesNameString, "'", ""), ",")
        For MultipleSelectionCount = 0 To UBound(StateArray)
            Dim TemporaryCountry As String = ""
            Dim TempReturn As New DataTable
            Dim TempStateName As String = Trim(StateArray(MultipleSelectionCount))
            TempReturn = aclsData_temp.Get_Jetnet_Country_By_State(RegionNameString, TempStateName)

            If Not IsNothing(TempReturn) Then
                If TempReturn.Rows.Count > 0 Then
                    If Not IsDBNull(TempReturn.Rows(0).Item("geographic_country_name")) Then
                        If WhereClause <> "" Then
                            WhereClause += " or "
                        End If
                        WhereClause += "(" & CountryFieldName & " = '" & TempReturn.Rows(0).Item("geographic_country_name").ToString & "' and " & stateFieldName & " = '" & TempStateName & "')"

                        'loop through the countries to get rid of the one we don't need.
                        For countryCount = 0 To UBound(CountriesArray)
                            If Trim(CountriesArray(countryCount)) = TempReturn.Rows(0).Item("geographic_country_name").ToString Then
                                CountriesArray(countryCount) = "" 'Clear the country
                            End If
                        Next
                    End If
                End If
            End If
        Next


        For countryCount = 0 To UBound(CountriesArray)
            Dim TempCountry As New DataTable
            If CountriesArray(countryCount) <> "" Then
                'If they haven't picked an applicable state, like for instance if they picked United States but didn't select the state allowed (Alaska for instance) then 
                'This ignores that selection. These are the countries that have no state picked - so we check the db to make sure they actually don't have states before we add them to the clause.
                TempCountry = aclsData_temp.Get_Jetnet_State_By_Country(RegionNameString, Trim(CountriesArray(countryCount)))
                If Not IsNothing(TempCountry) Then
                    If TempCountry.Rows.Count = 0 Then
                        If TemporaryCountryHold <> "" Then
                            TemporaryCountryHold += ","
                        End If
                        TemporaryCountryHold += "'" & Trim(CountriesArray(countryCount)) & "'"
                    End If
                End If
            End If
        Next

        If TemporaryCountryHold <> "" Then
            WhereClause += " or (" & CountryFieldName & " in (" & TemporaryCountryHold & "))"
        End If

        Return WhereClause
    End Function


#Region "Dealing with Attributes Tab"
    'Needs to be moved to the CLS Data Manager next to the AC one
    Public Shared Function GetYachtTopicQueryByID(ByVal topicID As Long) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable

        Try

            sql = " select yttop_id as TOPID, yttop_area as AREA, yttop_name as TOPIC, yttop_reference_id as REFID, "
            sql += " yttop_query as MYQUERY from yacht_topic a with (NOLOCK) "
            sql += " where yttop_id = @topicID "
            sql += " order by yttop_name"


            'save to session query debug string.
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "AdvancedQueryResults.vb", sql.ToString)

            'Opening Connection
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


            Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

            If topicID > 0 Then
                SqlCommand.Parameters.AddWithValue("topicID", topicID)
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
            'Me.class_error = "Error in GetYachtTopicQueryByID(ByVal topicID As Long) As DataTable: " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing
        End Try

    End Function
    Public Shared Function BuildAttributeTextAndDropdown(ByRef tempContainer As Panel, ByVal MainContent As String, ByVal ac_advanced_search As String, ByVal tempPanelID As String, ByVal MeRef As Page, ByVal OnListingPage As Boolean, Optional ByVal TempUpdatePanel As UpdatePanel = Nothing) As Integer
        Dim tempLabel_Pre As New Label
        Dim tempLabel_End As New Label
        Dim tempDropdown As New DropDownList
        Dim DropdownValue As Integer = 1
        Dim productName As String = "Evolution"
        Dim productType As String = "aircraft"

        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            productType = "yacht"
            productName = "YachtSpot"
        End If

        tempLabel_Pre.Text = "The following represents an index of " & productType & " attributes organized by Area "

        tempContainer.Controls.AddAt(0, tempLabel_Pre)

        tempDropdown.Items.Add(New ListItem("Equals", "Equals"))
        tempDropdown.ID = "COMPARE_SwapAttributeType"
        tempDropdown.CssClass = "display_none"
        tempContainer.Controls.AddAt(1, tempDropdown)

        'tempDropdown = New DropDownList

        'tempDropdown.Items.Add(New ListItem("Alphabetically", "1"))
        'tempDropdown.Items.Add(New ListItem("By Area", "2"))
        'tempDropdown.ID = "SwapAttributeType"
        'tempDropdown.Attributes.Add("onchange", "SwapAttribute(this,0)")
        'If OnListingPage Then
        '  tempDropdown.SelectedValue = 2
        '  DropdownValue = 2
        'Else
        '  tempDropdown.SelectedValue = 1
        '  DropdownValue = 1
        'End If

        'tempDropdown.CssClass = "margin_4"

        'If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & tempDropdown.ID)) Then
        '  If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & tempDropdown.ID)) Then
        '    tempDropdown.SelectedValue = HttpContext.Current.Session.Item("Advanced-" & tempDropdown.ID)
        '    DropdownValue = CInt(HttpContext.Current.Session.Item("Advanced-" & tempDropdown.ID))
        '    'calling the swap attribute function if this session variable is set.
        '    If Not OnListingPage Then
        '      Dim SwapList As StringBuilder = New StringBuilder()

        '      SwapList.Append("$(function(){SwapAttribute($('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_SwapAttributeType')," & DropdownValue & ")});")
        '      System.Web.UI.ScriptManager.RegisterStartupScript(MeRef, IIf(Not IsNothing(TempUpdatePanel), TempUpdatePanel.GetType, MeRef.GetType()), "SwapList", SwapList.ToString, True)
        '    End If
        '  End If
        'End If



        'tempContainer.Controls.AddAt(2, tempDropdown)

        If OnListingPage Then
            If Not MeRef.IsPostBack Then
                tempLabel_End.Text = " found within " & productName & ". Check the boxes of the attributes that apply to your search needs."
            Else
                tempLabel_End.Text = " associated with the " & HttpContext.Current.Session.Item("localUser").crmLatestRecordCount.ToString & " " & productType & " found in your search."
            End If
            tempLabel_End.Text += " Check the boxes of the attributes that apply to your search needs."
        Else
            tempLabel_End.Text = " found within " & productName & ". Check the boxes of the attributes that apply to your search needs and click ""Find " & DisplayFunctions.ConvertToTitleCase(productType) & """ to select the " & productType & " of interest.<br /><br />"
        End If


        tempContainer.Controls.AddAt(2, tempLabel_End)
        tempContainer.CssClass = "padding"

        'If Not MeRef.ClientScript.IsClientScriptBlockRegistered("SwapAttributeList") Then
        '  Dim SwapAttributeList As StringBuilder = New StringBuilder()
        '  SwapAttributeList.Append("<script type=""text/javascript"">")
        '  SwapAttributeList.Append("function SwapAttribute(dropdown, valueKnown){")
        '  SwapAttributeList.Append("var useThis;")
        '  SwapAttributeList.Append("if (valueKnown == 0) {")
        '  SwapAttributeList.Append("useThis = Number(dropdown.value);")
        '  SwapAttributeList.Append("} else { ")
        '  SwapAttributeList.Append("useThis = Number(valueKnown);")
        '  SwapAttributeList.Append("}")
        '  SwapAttributeList.Append("if (useThis == 2) { ")

        '  SwapAttributeList.Append("$('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_area_display').css('display','block');")
        '  SwapAttributeList.Append("$('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_letter_display').css('display','none');")
        '  SwapAttributeList.Append(" } else if (useThis == 1) { ")
        '  'SwapAttributeList.Append("alert(""2 if"");")
        '  SwapAttributeList.Append("$('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_letter_display').css('display','block');")
        '  SwapAttributeList.Append("$('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_area_display').css('display','none');")

        '  SwapAttributeList.Append("}")

        '  SwapAttributeList.Append("};")
        '  'SwapAttributeList.Append("$(document).ready(function() {")

        '  '' SwapAttributeList.Append("SwapAttribute($('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_SwapAttributeType')," & DropdownValue & ")")
        '  'SwapAttributeList.Append("});")
        '  SwapAttributeList.Append("</script>")
        'Dim s As New Object

        'If Not IsNothing(TempUpdatePanel) Then
        '  s = TryCast(TempUpdatePanel.GetType, System.Type)
        'Else
        '  s = TryCast(MeRef.GetType, System.Type)
        'End If

        'System.Web.UI.ScriptManager.RegisterStartupScript(MeRef, s, "SwapAttributeList", SwapAttributeList.ToString, False)
        'End If

        Return DropdownValue

    End Function

    Public Shared Sub DealWithAttributeTab(ByVal MainContentClientID As String, ByVal ContainingPanel As Panel, ByRef AttrTab As AjaxControlToolkit.TabPanel, ByRef advancedSearchTabContainer As AjaxControlToolkit.TabContainer, ByRef meRef As Page, ByRef aclsData_Temp As clsData_Manager_SQL, ByVal attentionLabel As Label)
        ' If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        'Adding the attributes panel.
        AttrTab.Visible = True
        Dim DropDownSelection As Integer = 2
        Dim TempTable As New DataTable
        'This panel is completely custom, it does not come from the database.
        'It is a panel that allows the ability to integrate filtering into the aircraft search area.
        'It will display two topic lists. One organized by alphabet. One organized by area.
        'The important thing to note is that when this is built, you really need to have it rebuilt after the search.
        'Your search results could alter the list of topics.

        If attentionLabel.Text = "" Then
            Dim TemporaryContainer As New Panel
            DropDownSelection = AdvancedQueryResults.BuildAttributeTextAndDropdown(TemporaryContainer, MainContentClientID, advancedSearchTabContainer.ID, AttrTab.ID, meRef, True)
            ContainingPanel.Controls.Add(TemporaryContainer)

            'TemporaryContainer = New Panel
            'TemporaryContainer.ID = "letter_display"
            'If DropDownSelection = 2 Then
            '  TemporaryContainer.Attributes.Add("style", "display:none;")
            'Else
            '  TemporaryContainer.Attributes.Remove("style")
            'End If

            'TempTable = AdvancedQueryResults.BuildTopicAreaPanel(TemporaryContainer, True, False, "LETTER", "TOPIC", MainContentClientID, advancedSearchTabContainer.ID, AttrTab.ID, meRef, True, aclsData_Temp)
            'ContainingPanel.Controls.Add(TemporaryContainer)

            TemporaryContainer = New Panel
            TemporaryContainer.ID = "area_display"
            'If DropDownSelection = 1 Then
            '  TemporaryContainer.Attributes.Add("style", "display:none;")
            'Else
            TemporaryContainer.Attributes.Remove("style")
            'End If
            AdvancedQueryResults.BuildTopicAreaPanel(TemporaryContainer, False, True, "AREA", "TOPIC", MainContentClientID, advancedSearchTabContainer.ID, AttrTab.ID, meRef, True, aclsData_Temp)
            ContainingPanel.Controls.Add(TemporaryContainer)
            AttrTab.Controls.Add(ContainingPanel)
        Else
            Dim newLabel As New Label
            newLabel.ForeColor = Drawing.Color.Red
            newLabel.Font.Bold = True
            newLabel.Text = "<br /><p align=""center"">No attributes were found for this search.</p>"
            AttrTab.Controls.Add(newLabel)
        End If

        ' End If
    End Sub


    Public Shared Function BuildTopicAreaPanel(ByRef displayPanel As Panel, ByVal DisplayLetter As Boolean, ByVal DisplayArea As Boolean, ByVal displayFieldOne As String, ByVal displayFieldTwo As String, ByVal MainContent As String, ByVal ac_advanced_search As String, ByVal tempPanelID As String, ByVal MeRef As Page, ByVal DisplayOnListing As Boolean, ByVal aclsDataTemp As clsData_Manager_SQL) As DataTable
        Dim ResultsTable As New DataTable
        Dim DisplayTable As New Table
        Dim DisplayTR As New TableRow
        Dim DisplayTD As New TableCell

        Dim DisplayMiniTable As New Table
        Dim DisplayMiniTR As New TableRow
        Dim DisplayMiniTD As New TableCell
        Dim DisplayLabel As New Label
        Dim displayCheck As New CheckBox

        Dim HoldMainArea As String = ""
        Dim letter As String = ""
        Dim topicCount As Integer = 0
        Dim cssClass As String = ""
        Dim areaString As String = ""
        Dim ItemUnderCount As Integer = 0

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Added on 8/28/15.
        'Added to save the attributes tables in session.
        'Both the letters and the area.
        Dim UseSessionMainTable As Boolean = False
        Dim SessionAreaTable As New DataTable
        Dim SessionLetterTable As New DataTable

        'Is this going to be a main (full) table load? Or is this going to be a listing (after search, partial) load?
        If DisplayOnListing = False Then
            'This is always a main full table load
            UseSessionMainTable = True
        Else
            'If the string is null or empty on the where clause - we can't use it to filter the attributes table.
            'This case happens when we're on the listing page before a search happens and the attributes are filtered. 
            If ((String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterAircraftWhere")) And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO) Or (String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterYachtWhere")) And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT)) Then
                'This is a full main table load as well.
                UseSessionMainTable = True
            End If
        End If

        'Here's the next important step. We need to figure out if we already have the session datatables in session or if we need
        'To load them up.
        'However we really only need to check the applicable datatable (letter/area) depending on what we're calling.
        If UseSessionMainTable Then 'If this isn't set (not pulling data from session), we don't need to really do all of this work.
            If DisplayLetter Then 'Letter datatable display only.
                If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable) Then
                    If TypeOf HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable Is DataTable Then
                        If HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable.rows.count > 0 Then
                            'We're going to cast this. We possibly don't need this, but with session objects, I'd rather be safe and get the type correct even though we're checking
                            SessionLetterTable = DirectCast(HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable, DataTable)
                        End If
                    End If
                End If

                If SessionLetterTable.Rows.Count = 0 Then
                    'We need to fill this session datatable up and save it.
                    If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                        SessionLetterTable = aclsDataTemp.GetYachtTopicListQueryByLetter(areaString, letter, IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtFrom"), ""))
                    Else
                        SessionLetterTable = aclsDataTemp.GetTopicListQueryByLetter(areaString, letter, IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftFrom"), ""))
                    End If
                    HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable = SessionLetterTable
                End If
            Else
                'Display Area. 
                If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable) Then
                    If TypeOf HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable Is DataTable Then
                        If HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable.rows.count > 0 Then
                            'We're going to cast this. We possibly don't need this, but with session objects, I'd rather be safe and get the type correct even though we're checking
                            SessionAreaTable = DirectCast(HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable, DataTable)
                        End If
                    End If
                End If

                If SessionAreaTable.Rows.Count = 0 Then
                    'We need to fill this session datatable up and save it.
                    If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                        SessionAreaTable = aclsDataTemp.GetYachtTopicListQueryByArea("", IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtFrom"), ""))
                    Else
                        SessionAreaTable = aclsDataTemp.GetTopicListQueryByArea("", IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftFrom"), ""))
                    End If
                    HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable = SessionAreaTable
                End If
            End If
        End If

        If DisplayLetter Then
            'Here we go ahead and make sure we need to use the main table, 
            'But also that there are rows in the main table.
            If UseSessionMainTable And SessionLetterTable.Rows.Count > 0 Then
                ResultsTable = SessionLetterTable
            Else
                If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                    ResultsTable = aclsDataTemp.GetYachtTopicListQueryByLetter(areaString, letter, IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtFrom"), ""))
                Else
                    ResultsTable = aclsDataTemp.GetTopicListQueryByLetter(areaString, letter, IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftFrom"), ""))
                End If
            End If
        ElseIf DisplayArea Then
            'Here we go ahead and make sure we need to use the main table, 
            'But also that there are rows in the main table.
            If UseSessionMainTable And SessionAreaTable.Rows.Count > 0 Then
                ResultsTable = SessionAreaTable
            Else
                If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                    ResultsTable = aclsDataTemp.GetYachtTopicListQueryByArea("", IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtFrom"), ""))
                Else
                    ResultsTable = aclsDataTemp.GetTopicListQueryByArea("", IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftFrom"), ""))
                End If
            End If
        End If

        'Dispose session temporary datatables.
        SessionAreaTable.Dispose()
        SessionLetterTable.Dispose()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Not IsNothing(ResultsTable) Then
            If ResultsTable.Rows.Count > 0 Then

                DisplayTable.CellPadding = 3
                DisplayTable.CellSpacing = 0
                DisplayTable.CssClass = "data_aircraft_grid medium_text div_clear"
                DisplayTable.Width = Unit.Percentage(100)

                For Each r As DataRow In ResultsTable.Rows

                    If HoldMainArea <> r(displayFieldOne).ToString Then

                        If ItemUnderCount = 2 Then
                            DisplayMiniTR.Controls.Add(DisplayMiniTD)
                            DisplayMiniTD = New TableCell
                            DisplayMiniTD.Width = Unit.Percentage(33)
                            DisplayMiniTD.CssClass = "override_borders"
                            DisplayMiniTD.HorizontalAlign = HorizontalAlign.Left
                            DisplayMiniTD.VerticalAlign = VerticalAlign.Top
                        End If

                        ItemUnderCount = 0

                        If cssClass = "" Then
                            cssClass = "dataListGray"
                        Else
                            cssClass = ""
                        End If

                        If HoldMainArea <> "" Then
                            DisplayMiniTR.Controls.Add(DisplayMiniTD)
                            DisplayMiniTD = New TableCell
                            DisplayMiniTable.Controls.Add(DisplayMiniTR)
                            DisplayMiniTR = New TableRow
                            DisplayTD.Controls.Add(DisplayMiniTable)
                            DisplayMiniTable = New Table
                            DisplayTR.Controls.Add(DisplayTD)
                            DisplayTable.Controls.Add(DisplayTR)
                        End If


                        topicCount = 0

                        DisplayTR = New TableRow
                        DisplayTD = New TableCell
                        DisplayTD.CssClass = cssClass & " override_borders"
                        DisplayTD.HorizontalAlign = HorizontalAlign.Left
                        DisplayTD.VerticalAlign = VerticalAlign.Top
                        DisplayTD.Text = "<span class=""upper_header medium_text""><b>" & r(displayFieldOne).ToString & "</b></span>"
                        DisplayTR.Controls.Add(DisplayTD)
                        DisplayTable.Controls.Add(DisplayTR)

                        DisplayTR = New TableRow
                        DisplayTD = New TableCell

                        'setting up to house mini table.
                        DisplayTD.CssClass = cssClass & " override_borders"
                        DisplayTD.HorizontalAlign = HorizontalAlign.Left
                        DisplayTD.VerticalAlign = VerticalAlign.Top

                        DisplayMiniTable = New Table
                        DisplayMiniTable.Width = Unit.Percentage(100)
                    End If


                    If topicCount = 3 Then

                        DisplayMiniTable.Controls.Add(DisplayMiniTR)
                        DisplayMiniTR = New TableRow
                        topicCount = 0
                    End If

                    DisplayMiniTD = New TableCell
                    DisplayMiniTD.CssClass = "override_borders padding_topic_list"""
                    DisplayMiniTD.Width = Unit.Percentage(33)
                    DisplayMiniTD.HorizontalAlign = HorizontalAlign.Left
                    DisplayMiniTD.VerticalAlign = VerticalAlign.Top

                    DisplayLabel = New Label
                    'Changed this display to not display the count if we're using the main attributes table (full before AC search)
                    DisplayLabel.Text = "&nbsp;" & r(displayFieldTwo).ToString & IIf(UseSessionMainTable = False, " (" & r("tcount").ToString & ")", "")
                    displayCheck = New CheckBox
                    displayCheck.ID = IIf(DisplayOnListing = False, "___", "") & IIf(DisplayArea, "Attribute", "Ignore") & "_" & r("TOPID").ToString

                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & displayCheck.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & displayCheck.ID)) Then
                            displayCheck.Checked = HttpContext.Current.Session.Item("Advanced-" & displayCheck.ID)
                        End If
                    End If

                    'displayCheck.Attributes.Add("onchange", "checkOther($('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_" & IIf(DisplayOnListing = False, "___", "") & IIf(DisplayLetter, "Ignore", "Attribute") & "_" & r("TOPID").ToString & "'),$('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_" & IIf(DisplayOnListing = False, "___", "") & IIf(DisplayLetter, "Attribute", "Ignore") & "_" & r("TOPID").ToString & "'))")

                    DisplayMiniTD.Controls.Add(displayCheck)
                    DisplayMiniTD.Controls.Add(DisplayLabel)

                    DisplayMiniTR.Controls.Add(DisplayMiniTD)

                    HoldMainArea = r(displayFieldOne).ToString


                    topicCount += 1
                    ItemUnderCount += 1
                Next

                'finish up in case the last one hasn't been adedd.
                DisplayMiniTR.Controls.Add(DisplayMiniTD)
                DisplayMiniTable.Controls.Add(DisplayMiniTR)
                DisplayTD.Controls.Add(DisplayMiniTable)
                DisplayTR.Controls.Add(DisplayTD)
                DisplayTable.Controls.Add(DisplayTR)
                displayPanel.Controls.Add(DisplayTable)


                'If Not MeRef.ClientScript.IsClientScriptBlockRegistered("CheckOther") Then
                '  Dim SwapAttributeList As StringBuilder = New StringBuilder()
                '  SwapAttributeList.Append("<script type=""text/javascript"">")
                '  SwapAttributeList.Append("function checkOther(otherCheckBox,boxChecked){")
                '  SwapAttributeList.Append(" if (boxChecked.prop('checked') == true) { ")
                '  SwapAttributeList.Append("otherCheckBox.prop('checked', true); ")
                '  SwapAttributeList.Append(" } else { ")
                '  SwapAttributeList.Append("otherCheckBox.prop('checked',false);")
                '  SwapAttributeList.Append("}")
                '  SwapAttributeList.Append("};")

                '  SwapAttributeList.Append("</script>")
                '  System.Web.UI.ScriptManager.RegisterStartupScript(MeRef, MeRef.GetType(), "CheckOther", SwapAttributeList.ToString, False)
                'End If
            End If
        End If
        Return ResultsTable
    End Function



    ''' <summary>
    '''This sub is going to set a cache variable called CachedAttributeDataset. This application variable is going to
    '''store two datatables (as a dataset). The first table will be all the attributes sorted as Letter.
    '''The second datatable will be the attributes stored as area.
    '''It automatically expires in one day
    '''So that way it will refill itself.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub FillCachedAttributeDataset(ByRef aclsData_Temp As clsData_Manager_SQL)
        If IsNothing(HttpContext.Current.Cache("CachedAttributeDataset")) Then
            HttpContext.Current.Cache.Insert("CachedAttributeDataset", CreateAttributeCacheDataset(aclsData_Temp), Nothing, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration)
        End If
    End Sub

    Public Shared Function CreateAttributeCacheDataset(ByRef aclsData_Temp As clsData_Manager_SQL) As DataSet
        Dim AttributeDataSet As New DataSet
        'Dim Letter_Datatable As New DataTable
        Dim Area_Datatable As New DataTable

        'Datatable #1 is the Attribute Letter Datatable. We need to set a unique table names
        'If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        '  Letter_Datatable = GetFullYachtTopicListQueryByLetter()
        'ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        '  Letter_Datatable = GetFullTopicListQueryByLetter()
        'End If
        'Letter_Datatable.TableName = "LETTER"

        'Datatable #2 is Attribute Area datatable. We will need to set the table name. 


        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            Area_Datatable = GetFullYachtTopicListQueryByArea()
        ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
            Area_Datatable = GetFullTopicListQueryByArea()
        End If
        Area_Datatable.TableName = "AREA"

        'Adding to the dataset.
        'If Not IsNothing(Letter_Datatable) Then
        '  AttributeDataSet.Tables.Add(Letter_Datatable)
        'Else
        '  AttributeDataSet.Tables.Add(New DataTable)
        'End If

        If Not IsNothing(Area_Datatable) Then
            AttributeDataSet.Tables.Add(Area_Datatable)
        Else
            AttributeDataSet.Tables.Add(New DataTable)
        End If

        Return AttributeDataSet

    End Function


    Public Shared Function GetFullTopicListQueryByArea() As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable


        Try

            sql = "select distinct actop_id as TOPID,actop_area as AREA, actop_name as TOPIC from aircraft_topic with (NOLOCK)"
            sql += " group by actop_id, actop_area,actop_name"
            sql += " order by actop_area,actop_name"

            'save to session query debug string.
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "AdvancedQueryResults.vb", sql.ToString)

            'Opening Connection
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


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
            ' Me.class_error = "Error in GetTopicListQueryByArea() As DataTable" + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

    End Function

    '  -- ******************************************
    '-- TOPIC LIST QUERY - ALL TOPICS- BY LETTER
    'select distinct actop_id as TOPID, left(actop_name,1) as LETTER, actop_name as TOPIC
    'group by actop_id, left(actop_name,1),actop_name
    'order by actop_name
    'Public Shared Function GetFullTopicListQueryByLetter() As DataTable
    '  Dim sql As String = ""
    '  Dim SqlConn As New SqlClient.SqlConnection
    '  Dim SqlReader As SqlClient.SqlDataReader
    '  Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    '  Dim atemptable As New DataTable

    '  Try

    '    sql = "select distinct actop_id as TOPID, left(actop_name,1) as LETTER, actop_name as TOPIC from aircraft_topic with (NOLOCK)"
    '    sql += " group by actop_id, left(actop_name,1),actop_name"
    '    sql += " order by actop_name"

    '    'save to session query debug string.
    '    clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "AdvancedQueryResults.vb", sql.ToString)

    '    'Opening Connection
    '    SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    '    SqlConn.Open()


    '    Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

    '    SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

    '    Try
    '      atemptable.Load(SqlReader)
    '    Catch constrExc As System.Data.ConstraintException
    '      Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
    '    End Try

    '    SqlCommand.Dispose()
    '    SqlCommand = Nothing

    '    Return atemptable

    '  Catch ex As Exception
    '    Return Nothing
    '    'Me.class_error = "Error in GetTopicListQueryByLetter() As DataTable: " + ex.Message
    '  Finally
    '    SqlReader = Nothing

    '    SqlConn.Dispose()
    '    SqlConn.Close()
    '    SqlConn = Nothing
    '  End Try

    'End Function


    '  -- ******************************************
    '-- TOPIC LIST QUERY - FOR ALL Yacht - BY LETTER
    'Public Shared Function GetFullYachtTopicListQueryByLetter() As DataTable
    '  Dim sql As String = ""
    '  Dim SqlConn As New SqlClient.SqlConnection
    '  Dim SqlReader As SqlClient.SqlDataReader
    '  Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    '  Dim atemptable As New DataTable

    '  Try

    '    sql = " select distinct yttop_id as TOPID, left(yttop_name,1) as LETTER, yttop_name as TOPIC, COUNT(*) as tcount from yacht_topic with (NOLOCK)"
    '    sql += " group by yttop_id, left(yttop_name,1),yttop_name"
    '    sql += " order by yttop_name"

    '    'save to session query debug string.
    '    clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "AdvancedQueryResults.vb", sql.ToString)

    '    'Opening Connection
    '    SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    '    SqlConn.Open()


    '    Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


    '    SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

    '    Try
    '      atemptable.Load(SqlReader)
    '    Catch constrExc As System.Data.ConstraintException
    '      Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
    '    End Try

    '    SqlCommand.Dispose()
    '    SqlCommand = Nothing

    '    Return atemptable

    '  Catch ex As Exception
    '    Return Nothing
    '    'Me.class_error = "Error in GetFullYachtTopicListQueryByLetter(ByVal letter As String, ByVal appendedWhereClause As String, ByVal AppendedFromClause As String) As DataTable: " + ex.Message
    '  Finally
    '    SqlReader = Nothing

    '    SqlConn.Dispose()
    '    SqlConn.Close()
    '    SqlConn = Nothing
    '  End Try

    'End Function

    Public Shared Function GetFullYachtTopicListQueryByArea() As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable


        Try

            sql = "select distinct yttop_id as TOPID, yttop_area as AREA, yttop_name as TOPIC, COUNT(*) as tcount from yacht_topic with (NOLOCK)"
            sql += " group by yttop_id, yttop_area,yttop_name"
            sql += " order by yttop_area,yttop_name"

            'save to session query debug string.
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "AdvancedQueryResults.vb", sql.ToString)

            'Opening Connection
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


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
            'Me.class_error = "Error in GetFullYachtTopicListQueryByArea() As DataTable: " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

    End Function

    Public Shared Function BuildInitialCachedCheckboxesOnHomePage(ByRef CachedTable As DataTable, ByRef displayPanel As Panel, ByVal DisplayLetter As Boolean, ByVal DisplayArea As Boolean, ByVal displayFieldOne As String, ByVal displayFieldTwo As String, ByVal MainContent As String, ByVal ac_advanced_search As String, ByVal tempPanelID As String, ByVal MeRef As Page, ByVal DisplayOnListing As Boolean, ByRef aclsDataTemp As clsData_Manager_SQL, ByRef main_home_update_panel As UpdatePanel) As DataTable
        Dim DisplayTable As New Table
        Dim DisplayTR As New TableRow
        Dim DisplayTD As New TableCell
        Dim UseSessionMainTable = True
        Dim DisplayMiniTable As New Table
        Dim DisplayMiniTR As New TableRow
        Dim DisplayMiniTD As New TableCell
        Dim DisplayLabel As New Label
        Dim displayCheck As New CheckBox

        Dim HoldMainArea As String = ""
        Dim letter As String = ""
        Dim topicCount As Integer = 0
        Dim cssClass As String = ""
        Dim areaString As String = ""
        Dim ItemUnderCount As Integer = 0

        If Not IsNothing(CachedTable) Then
            If CachedTable.Rows.Count > 0 Then

                DisplayTable.CellPadding = 3
                DisplayTable.CellSpacing = 0
                DisplayTable.CssClass = "data_aircraft_grid medium_text div_clear"
                DisplayTable.Width = Unit.Percentage(100)

                For Each r As DataRow In CachedTable.Rows

                    If HoldMainArea <> r(displayFieldOne).ToString Then

                        If ItemUnderCount = 2 Then
                            DisplayMiniTR.Controls.Add(DisplayMiniTD)
                            DisplayMiniTD = New TableCell
                            DisplayMiniTD.Width = Unit.Percentage(33)
                            DisplayMiniTD.CssClass = "override_borders"
                            DisplayMiniTD.HorizontalAlign = HorizontalAlign.Left
                            DisplayMiniTD.VerticalAlign = VerticalAlign.Top
                        End If

                        ItemUnderCount = 0

                        If cssClass = "" Then
                            cssClass = "dataListGray"
                        Else
                            cssClass = ""
                        End If

                        If HoldMainArea <> "" Then
                            DisplayMiniTR.Controls.Add(DisplayMiniTD)
                            DisplayMiniTD = New TableCell
                            DisplayMiniTable.Controls.Add(DisplayMiniTR)
                            DisplayMiniTR = New TableRow
                            DisplayTD.Controls.Add(DisplayMiniTable)
                            DisplayMiniTable = New Table
                            DisplayTR.Controls.Add(DisplayTD)
                            DisplayTable.Controls.Add(DisplayTR)
                        End If


                        topicCount = 0


                        DisplayTD = New TableCell
                        DisplayTD.CssClass = cssClass & " override_borders"
                        DisplayTD.HorizontalAlign = HorizontalAlign.Left
                        DisplayTD.VerticalAlign = VerticalAlign.Top
                        DisplayTD.Text = "<span class=""upper_header medium_text""><b>" & r(displayFieldOne).ToString & "</b></span>"

                        DisplayTR = New TableRow
                        DisplayTR.ID = IIf(DisplayOnListing = False, "___", "") & IIf(DisplayArea, "AttributeTR", "IgnoreTR") & "_" & r(displayFieldOne).ToString
                        DisplayTR.CssClass = "display_none"

                        DisplayTR.Controls.Add(DisplayTD)
                        DisplayTable.Controls.Add(DisplayTR)

                        DisplayTR = New TableRow
                        DisplayTD = New TableCell

                        'setting up to house mini table.
                        DisplayTD.CssClass = cssClass & " override_borders"
                        DisplayTD.HorizontalAlign = HorizontalAlign.Left
                        DisplayTD.VerticalAlign = VerticalAlign.Top

                        DisplayTD.ID = IIf(DisplayOnListing = False, "___", "") & IIf(DisplayArea, "AttributeTD", "IgnoreTD") & "_" & r(displayFieldOne).ToString
                        DisplayTD.CssClass = "display_none"



                        DisplayMiniTable = New Table
                        DisplayMiniTable.Width = Unit.Percentage(100)
                    End If


                    If topicCount = 3 Then

                        DisplayMiniTable.Controls.Add(DisplayMiniTR)
                        DisplayMiniTR = New TableRow

                        topicCount = 0
                    End If

                    DisplayMiniTD = New TableCell
                    DisplayMiniTD.CssClass = "override_borders padding_topic_list"""
                    DisplayMiniTD.Width = Unit.Percentage(33)
                    DisplayMiniTD.HorizontalAlign = HorizontalAlign.Left
                    DisplayMiniTD.ID = IIf(DisplayOnListing = False, "___", "") & IIf(DisplayArea, "AttributeCell", "IgnoreCell") & "_" & r("TOPID").ToString
                    DisplayMiniTD.CssClass = "display_none"

                    DisplayMiniTD.VerticalAlign = VerticalAlign.Top

                    DisplayLabel = New Label
                    'Changed this display to not display the count if we're using the main attributes table (full before AC search)
                    DisplayLabel.Text = "&nbsp;" & r(displayFieldTwo).ToString
                    displayCheck = New CheckBox
                    displayCheck.ID = IIf(DisplayOnListing = False, "___", "") & IIf(DisplayArea, "Attribute", "Ignore") & "_" & r("TOPID").ToString

                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & displayCheck.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & displayCheck.ID)) Then
                            displayCheck.Checked = HttpContext.Current.Session.Item("Advanced-" & displayCheck.ID)
                        End If
                    End If

                    'displayCheck.Attributes.Add("onchange", "checkOther($('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_" & "___" & IIf(DisplayLetter, "Ignore", "Attribute") & "_" & r("TOPID").ToString & "'),$('#" & MainContent & "_" & ac_advanced_search & "_" & tempPanelID & "_" & "___" & IIf(DisplayLetter, "Attribute", "Ignore") & "_" & r("TOPID").ToString & "'))")

                    DisplayMiniTD.Controls.Add(displayCheck)
                    DisplayMiniTD.Controls.Add(DisplayLabel)

                    DisplayMiniTR.Controls.Add(DisplayMiniTD)

                    HoldMainArea = r(displayFieldOne).ToString


                    topicCount += 1
                    ItemUnderCount += 1
                Next

                'finish up in case the last one hasn't been adedd.
                DisplayMiniTR.Controls.Add(DisplayMiniTD)
                DisplayMiniTable.Controls.Add(DisplayMiniTR)
                DisplayTD.Controls.Add(DisplayMiniTable)
                DisplayTR.Controls.Add(DisplayTD)
                DisplayTable.Controls.Add(DisplayTR)
                displayPanel.Controls.Add(DisplayTable)


                'If Not MeRef.ClientScript.IsClientScriptBlockRegistered("CheckOther") Then
                '  Dim SwapAttributeList As StringBuilder = New StringBuilder()
                '  SwapAttributeList.Append("<script type=""text/javascript"">")
                '  SwapAttributeList.Append("function checkOther(otherCheckBox,boxChecked){")
                '  SwapAttributeList.Append(" if (boxChecked.prop('checked') == true) { ")
                '  SwapAttributeList.Append("otherCheckBox.prop('checked', true); ")
                '  SwapAttributeList.Append(" } else { ")
                '  SwapAttributeList.Append("otherCheckBox.prop('checked',false);")
                '  SwapAttributeList.Append("}")
                '  SwapAttributeList.Append("};")

                '  SwapAttributeList.Append("</script>")
                '  System.Web.UI.ScriptManager.RegisterStartupScript(MeRef, main_home_update_panel.GetType, "CheckOther", SwapAttributeList.ToString, False)
                'End If
            End If
        End If
        Return CachedTable
    End Function

    Public Shared Sub LoopThroughHomeIndexTabToTurnOnCheckboxes(ByVal DisplayOnListing As Boolean, ByVal DisplayLetter As Boolean, ByVal DisplayArea As Boolean, ByRef aclsDataTemp As clsData_Manager_SQL, ByVal DisplayFieldOne As String, ByRef index_tab As AjaxControlToolkit.TabPanel)

        'let's just try a test here:
        Dim SessionAreaTable As New DataTable
        'Dim SessionLetterTable As New DataTable
        Dim UseSessionMainTable As Boolean = False
        Dim ResultsTable As New DataTable
        'Dim letter As String = ""
        Dim areaString As String = ""
        If DisplayOnListing = False Then
            'This is always a main full table load
            UseSessionMainTable = True
        Else
            'If the string is null or empty on the where clause - we can't use it to filter the attributes table.
            'This case happens when we're on the listing page before a search happens and the attributes are filtered. 
            If ((String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterAircraftWhere")) And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO) Or (String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterYachtWhere")) And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT)) Then
                'This is a full main table load as well.
                UseSessionMainTable = True
            End If
        End If

        'Here's the next important step. We need to figure out if we already have the session datatables in session or if we need
        'To load them up.
        'However we really only need to check the applicable datatable (letter/area) depending on what we're calling.
        If UseSessionMainTable Then 'If this isn't set (not pulling data from session), we don't need to really do all of this work.
            If DisplayLetter Then 'Letter datatable display only.
                'If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable) Then
                '  If TypeOf HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable Is DataTable Then
                '    If HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable.rows.count > 0 Then
                '      'We're going to cast this. We possibly don't need this, but with session objects, I'd rather be safe and get the type correct even though we're checking
                '      SessionLetterTable = DirectCast(HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable, DataTable)
                '    End If
                '  End If
                'End If

                'If SessionLetterTable.Rows.Count = 0 Then
                '  'We need to fill this session datatable up and save it.
                '  If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                '    SessionLetterTable = aclsDataTemp.GetYachtTopicListQueryByLetter(areaString, letter, IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtFrom"), ""))
                '  Else
                '    SessionLetterTable = aclsDataTemp.GetTopicListQueryByLetter(areaString, letter, IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftFrom"), ""))
                '  End If
                '  HttpContext.Current.Session.Item("localUser").crmUserAttributeLetterDatatable = SessionLetterTable
                'End If
            Else
                'Display Area. 
                If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable) Then
                    If TypeOf HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable Is DataTable Then
                        If HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable.rows.count > 0 Then
                            'We're going to cast this. We possibly don't need this, but with session objects, I'd rather be safe and get the type correct even though we're checking
                            SessionAreaTable = DirectCast(HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable, DataTable)
                        End If
                    End If
                End If

                If SessionAreaTable.Rows.Count = 0 Then
                    'We need to fill this session datatable up and save it.
                    If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                        SessionAreaTable = aclsDataTemp.GetYachtTopicListQueryByArea("", IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtFrom"), ""))
                    Else
                        SessionAreaTable = aclsDataTemp.GetTopicListQueryByArea("", IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftFrom"), ""))
                    End If
                    HttpContext.Current.Session.Item("localUser").crmUserAttributeAreaDatatable = SessionAreaTable
                End If
            End If
        End If

        If DisplayLetter Then
            ''Here we go ahead and make sure we need to use the main table, 
            ''But also that there are rows in the main table.
            'If UseSessionMainTable And SessionLetterTable.Rows.Count > 0 Then
            '  ResultsTable = SessionLetterTable
            'Else
            '  If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            '    ResultsTable = aclsDataTemp.GetYachtTopicListQueryByLetter(areaString, letter, IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtFrom"), ""))
            '  Else
            '    ResultsTable = aclsDataTemp.GetTopicListQueryByLetter(areaString, letter, IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftFrom"), ""))
            '  End If
            'End If
        ElseIf DisplayArea Then
            'Here we go ahead and make sure we need to use the main table, 
            'But also that there are rows in the main table.
            If UseSessionMainTable And SessionAreaTable.Rows.Count > 0 Then
                ResultsTable = SessionAreaTable
            Else
                If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                    ResultsTable = aclsDataTemp.GetYachtTopicListQueryByArea("", IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterYachtFrom"), ""))
                Else
                    ResultsTable = aclsDataTemp.GetTopicListQueryByArea("", IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftWhere"), ""), IIf(DisplayOnListing, HttpContext.Current.Session.Item("MasterAircraftFrom"), ""))
                End If
            End If
        End If

        'Dispose session temporary datatables.
        SessionAreaTable.Dispose()
        'SessionLetterTable.Dispose()


        If Not IsNothing(ResultsTable) Then
            If ResultsTable.Rows.Count > 0 Then
                For Each r As DataRow In ResultsTable.Rows
                    Dim DisplayCell As New TableCell
                    Dim DisplayHeaderCell As New TableCell
                    Dim DisplayTR As New TableRow
                    Dim TemporaryID As String = IIf(DisplayOnListing = False, "___", "") & IIf(DisplayArea, "AttributeCell", "IgnoreCell") & "_" & r("TOPID").ToString
                    Dim TemporaryCellID As String = IIf(DisplayOnListing = False, "___", "") & IIf(DisplayArea, "AttributeTD", "IgnoreTD") & "_" & r(DisplayFieldOne).ToString
                    Dim TemporaryRowID As String = IIf(DisplayOnListing = False, "___", "") & IIf(DisplayArea, "AttributeTR", "IgnoreTR") & "_" & r(DisplayFieldOne).ToString
                    If Not IsNothing(index_tab.FindControl(TemporaryID)) Then
                        DisplayCell = index_tab.FindControl(TemporaryID)
                        DisplayCell.CssClass = ""

                        If Not IsNothing(index_tab.FindControl(TemporaryCellID)) Then
                            DisplayHeaderCell = index_tab.FindControl(TemporaryCellID)
                            DisplayHeaderCell.CssClass = ""
                        End If
                        If Not IsNothing(index_tab.FindControl(TemporaryRowID)) Then
                            DisplayTR = index_tab.FindControl(TemporaryRowID)
                            DisplayTR.CssClass = ""
                        End If
                    End If

                Next
            End If
        End If
    End Sub

#End Region
End Class


