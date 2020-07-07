Imports System.IO
Partial Public Class edit
  Inherits System.Web.UI.Page
  Public aclsData_Temp As New clsData_Manager_SQL 'Test DataObject!!
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""

#Region "Loop Contacts Function Events for Contact Reference, Company Edit"
  Private Sub Contact_Reference_Edit_Template1_loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean) Handles Contact_Reference_Edit_Template1.loop_contacts, Aircraft_Edit_Transactions_Tab1.loop_contacts
    Try
      loop_contacts(idnum_new, jetnet_id, contact_id, skip, inactive)
    Catch ex As Exception
      error_string = "edit.aspx.vb - Contact_Reference_Edit_Template1_loop_contacts() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Company_Edit_Template1_loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean) Handles Company_Edit_Template1.loop_contacts
    Try
      loop_contacts(idnum_new, jetnet_id, contact_id, skip, inactive)
    Catch ex As Exception
      error_string = "edit.aspx.vb - Company_Edit_Template1_loop_contacts() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Aircraft_Edit_Template1_loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean, ByVal client_ac_id As Integer, ByVal jetnet_ac_id As Integer) Handles Aircraft_Edit_Template1.loop_contacts
    Try
      loop_contacts_ac_add(idnum_new, jetnet_id, contact_id, skip, inactive, client_ac_id, jetnet_ac_id)
    Catch ex As Exception
      error_string = "edit.aspx.vb - loop_contacts_ac_add() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Contact_Edit_Template1_loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean) Handles Contact_Edit_Template1.loop_contacts
    Try
      loop_contacts(idnum_new, jetnet_id, contact_id, skip, inactive)
    Catch ex As Exception
      error_string = "edit.aspx.vb - Contact_Edit_Template1_loop_contacts() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Function loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal skip_one As Integer, ByVal add_ref_here As Boolean, ByVal inactive As String)
    loop_contacts = ""

    Dim status As String = "Y"

    If inactive = True Then
      status = "N"
    End If

    'After that's cleared, we need to take all of the contacts
    Try
      'This is where we have to add the contacts that were already linked with this company. 
      'Make sure to use the jetnet_id id. This is important because we're using jetnet ID to get the existing contacts. 

      aTempTable = aclsData_Temp.GetContacts(jetnet_id, "JETNET", "Y", 0)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          'This loops through all of the contacts. 
          For Each r As DataRow In aTempTable.Rows

            If r("contact_id") <> skip_one Then
              Dim aclsClient_Contact As New clsClient_Contact
              aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
              aclsClient_Contact.clicontact_sirname = IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname"), "")
              aclsClient_Contact.clicontact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
              aclsClient_Contact.clicontact_middle_initial = IIf(Not IsDBNull(r("contact_middle_initial")), r("contact_middle_initial"), "")
              aclsClient_Contact.clicontact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
              aclsClient_Contact.clicontact_suffix = IIf(Not IsDBNull(r("contact_suffix")), r("contact_suffix"), "")
              aclsClient_Contact.clicontact_title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")
              aclsClient_Contact.clicontact_email_address = IIf(Not IsDBNull(r("contact_email_address")), r("contact_email_address"), "")
              aclsClient_Contact.clicontact_date_updated = Now()
              aclsClient_Contact.clicontact_jetnet_contact_id = r("contact_id")
              aclsClient_Contact.clicontact_comp_id = idnum_new
              aclsClient_Contact.clicontact_status = status
              Dim contact_id_new As Integer


              'This attempts to insert this contact record. 
              If aclsData_Temp.Insert_Client_Contact(aclsClient_Contact) = True Then
                'not done yet. Now we have to get the phone numbers based on the contact and insert them.
                'First we need to get the contact id of what we just inserted.. 
                'Have to get the new contact ID 
                aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(r("contact_id"), "Y")
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable2.Rows
                      contact_id_new = q("contact_id")
                    Next 'this loops through contact ID record
                  Else 'rows = 0 
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("edit.aspx.vb - loop_contacts() - " & error_string)
                  End If
                  display_error()
                End If

                'Inserting new contact phone numbers. 
                aTempTable2 = aclsData_Temp.GetContact_PhoneNumbers(r("contact_id"))
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable2.Rows
                      Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
                      aclsClient_Phone_Numbers.clipnum_type = q("pnum_type")
                      aclsClient_Phone_Numbers.clipnum_number = q("pnum_number")
                      aclsClient_Phone_Numbers.clipnum_comp_id = idnum_new
                      aclsClient_Phone_Numbers.clipnum_contact_id = contact_id_new
                      If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                        '  Response.Write("insert contact phone Number<br />")
                      Else
                        'Response.Write("Update Client Contact Fail")
                      End If
                    Next 'This loops through new contact phone numbers


                  Else ' rows = 0
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("edit.aspx.vb - loop_contacts() - " & error_string)
                  End If
                  display_error()
                End If

                'Get all the AC information.
                If add_ref_here = True Then
                  aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_JETNET_contactID(r("contact_id"))
                  If Not IsNothing(aTempTable2) Then
                    If aTempTable2.Rows.Count > 0 Then
                      For Each q As DataRow In aTempTable2.Rows
                        Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference

                        aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new
                        aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = IIf(Not IsDBNull(q("acref_contact_type")), q("acref_contact_type"), "")
                        aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id_new
                        aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = q("acref_ac_id")
                        aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = IIf(Not IsDBNull(q("acref_operator_flag")), q("acref_operator_flag"), "")
                        aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                        aclsInsert_Client_Aircraft_Reference.cliacref_business_type = IIf(Not IsDBNull(q("acref_business_type")), q("acref_business_type"), "")
                        aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                        aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                        If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                        Else
                          If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            LogError("edit.aspx.vb - loop_contacts() - " & error_string)
                          End If
                          display_error()
                        End If
                      Next
                    End If
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("edit.aspx.vb - loop_contacts() - " & error_string)
                    End If
                    display_error()
                  End If
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("edit.aspx.vb - loop_contacts() - " & error_string)
                End If
                display_error()
              End If 'if client is inserted
            End If 'end skip
          Next ' This loops through all the contacts. 


        End If
      End If
    Catch ex As Exception
      error_string = "edit.aspx.vb - loop_contacts() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
#End Region
#Region "Loop Contacts for AC Add"
  Function loop_contacts_ac_add(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal skip_one As Integer, ByVal add_ref_here As Boolean, ByVal inactive As String, ByVal client_ac_id As Integer, ByVal jetnet_ac_id As Integer)
    loop_contacts_ac_add = ""

    Dim status As String = "Y"

    If inactive = True Then
      status = "N"
    End If

    'After that's cleared, we need to take all of the contacts
    Try
      'This is where we have to add the contacts that were already linked with this company. 
      'Make sure to use the jetnet_id id. This is important because we're using jetnet ID to get the existing contacts. 

      aTempTable = aclsData_Temp.GetContacts(jetnet_id, "JETNET", "Y", 0)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          'This loops through all of the contacts. 
          For Each r As DataRow In aTempTable.Rows

            If r("contact_id") <> skip_one Then
              Dim aclsClient_Contact As New clsClient_Contact
              aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
              If Not IsDBNull(r("contact_sirname")) Then
                aclsClient_Contact.clicontact_sirname = r("contact_sirname")
              End If
              If Not IsDBNull(r("contact_first_name")) Then
                aclsClient_Contact.clicontact_first_name = r("contact_first_name")
              End If
              If Not IsDBNull(r("contact_middle_initial")) Then
                aclsClient_Contact.clicontact_middle_initial = r("contact_middle_initial")
              End If
              If Not IsDBNull(r("contact_last_name")) Then
                aclsClient_Contact.clicontact_last_name = r("contact_last_name")
              End If
              If Not IsDBNull(r("contact_suffix")) Then
                aclsClient_Contact.clicontact_suffix = r("contact_suffix")
              End If
              If Not IsDBNull(r("contact_title")) Then
                aclsClient_Contact.clicontact_title = r("contact_title")
              End If
              If Not IsDBNull(r("contact_email_address")) Then
                aclsClient_Contact.clicontact_email_address = r("contact_email_address")
              End If
              aclsClient_Contact.clicontact_date_updated = Now()
              aclsClient_Contact.clicontact_jetnet_contact_id = r("contact_id")
              aclsClient_Contact.clicontact_comp_id = idnum_new
              aclsClient_Contact.clicontact_status = status
              Dim contact_id_new As Integer

              aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(r("contact_id"), "Y")
              'If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                For Each q As DataRow In aTempTable2.Rows
                  contact_id_new = q("contact_id")
                Next 'this loops through contact ID record
              Else 'rows = 0 
                'This attempts to insert this contact record. 
                If aclsData_Temp.Insert_Client_Contact(aclsClient_Contact) = True Then

                  'not done yet. Now we have to get the phone numbers based on the contact and insert them.
                  'First we need to get the contact id of what we just inserted.. 
                  'Have to get the new contact ID 
                  aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(r("contact_id"), "Y")
                  If Not IsNothing(aTempTable2) Then
                    If aTempTable2.Rows.Count > 0 Then
                      For Each q As DataRow In aTempTable2.Rows
                        contact_id_new = q("contact_id")
                      Next 'this loops through contact ID record
                    Else 'rows = 0 
                    End If
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("edit.aspx.vb - loop_contacts_ac_add() - " & error_string)
                    End If
                    display_error()
                  End If

                  'Inserting new contact phone numbers. 
                  aTempTable2 = aclsData_Temp.GetContact_PhoneNumbers(r("contact_id"))
                  If Not IsNothing(aTempTable2) Then
                    If aTempTable2.Rows.Count > 0 Then
                      For Each q As DataRow In aTempTable2.Rows
                        Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
                        If Not IsDBNull(q("pnum_type")) Then
                          aclsClient_Phone_Numbers.clipnum_type = q("pnum_type")
                        End If
                        If Not IsDBNull(q("pnum_number")) Then
                          aclsClient_Phone_Numbers.clipnum_number = q("pnum_number")
                        End If
                        aclsClient_Phone_Numbers.clipnum_comp_id = idnum_new
                        aclsClient_Phone_Numbers.clipnum_contact_id = contact_id_new
                        If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                          '  Response.Write("insert contact phone Number<br />")
                        Else
                          'Response.Write("Update Client Contact Fail")
                        End If
                      Next 'This loops through new contact phone numbers


                    Else ' rows = 0
                    End If
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("edit.aspx.vb - loop_contacts_ac_add() - " & error_string)
                    End If
                    display_error()
                  End If

                End If
              End If
              'Get all the AC information.
              If add_ref_here = True Then
                aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_JETNET_contactID(r("contact_id"))
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable2.Rows
                      Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
                      aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new


                      aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new
                      'Changed october 21st to stop duplicate trans entries
                      aTempTable = aclsData_Temp.Get_Aircraft_Reference_Client_JETNET_acID_SORT(q("acref_ac_id"), idnum_new, contact_id_new)

                      '' check the state of the DataTable
                      If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count = 0 Then
                          If jetnet_ac_id = CInt(q("acref_ac_id")) Then


                            aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = client_ac_id
                            aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
                            If Not IsDBNull(q("acref_contact_type")) Then
                              aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = q("acref_contact_type")
                            End If
                            aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id_new
                            If Not IsDBNull(q("acref_operator_flag")) Then
                              aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = q("acref_operator_flag")
                            End If
                            aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                            If Not IsDBNull(q("acref_business_type")) Then
                              aclsInsert_Client_Aircraft_Reference.cliacref_business_type = q("acref_business_type")
                            End If
                            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                            'Response.Write(aclsInsert_Client_Aircraft_Reference.ClassInfo(aclsInsert_Client_Aircraft_Reference) & "<Hr />")
                            If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                            Else
                              If aclsData_Temp.class_error <> "" Then
                                error_string = aclsData_Temp.class_error
                                LogError("edit.aspx.vb - loop_contacts_ac_add() - " & error_string)
                              End If
                              display_error()
                            End If
                          Else

                            aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = 0
                            aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = q("acref_ac_id")
                            If Not IsDBNull(q("acref_contact_type")) Then
                              aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = q("acref_contact_type")
                            End If
                            aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id_new
                            If Not IsDBNull(q("acref_operator_flag")) Then
                              aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = q("acref_operator_flag")
                            End If
                            aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                            If Not IsDBNull(q("acref_business_type")) Then
                              aclsInsert_Client_Aircraft_Reference.cliacref_business_type = q("acref_business_type")
                            End If
                            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                            'Response.Write(aclsInsert_Client_Aircraft_Reference.ClassInfo(aclsInsert_Client_Aircraft_Reference) & "<Hr />")
                            If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                            Else
                              If aclsData_Temp.class_error <> "" Then
                                error_string = aclsData_Temp.class_error
                                LogError("edit.aspx.vb - loop_contacts_ac_add() - " & error_string)
                              End If
                              display_error()
                            End If
                          End If
                        End If
                      End If

                    Next
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("edit.aspx.vb - loop_contacts_ac_add() - " & error_string)
                  End If
                  display_error()
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("edit.aspx.vb - loop_contacts_ac_add() - " & error_string)
                End If
                display_error()
              End If 'if client is inserted

            End If 'end skip
          Next ' This loops through all the contacts. 


        End If
      End If
    Catch ex As Exception
      error_string = "edit.aspx.vb - loop_contacts_ac_add() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
#End Region
#Region "Get Insert AC for Contacts/Company"
  Private Sub Company_Edit_Template1_get_insert_ac(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer) Handles Company_Edit_Template1.get_insert_ac
    Try
      get_insert_ac(jetnet_id, idnum_new, get_previous_ac, insert_ac, new_client_ac)
    Catch ex As Exception
      error_string = "edit.aspx.vb - Company_Edit_Template1_get_insert_ac() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub Aircraft_Edit_Template1_get_insert_ac(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer, ByVal jetnet_ac_id As Integer) Handles Aircraft_Edit_Template1.get_insert_ac
    Try
      get_insert_ac_AC_CLIENT_INSERT(jetnet_id, idnum_new, get_previous_ac, insert_ac, new_client_ac, jetnet_ac_id)
    Catch ex As Exception
      error_string = "edit.aspx.vb - Aircraft_Edit_Template1_get_insert_ac() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Contact_Edit_Template1_get_insert_ac(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer) Handles Contact_Edit_Template1.get_insert_ac
    Try
      get_insert_ac(jetnet_id, idnum_new, get_previous_ac, insert_ac, new_client_ac)
    Catch ex As Exception
      error_string = "edit.aspx.vb - Contact_Edit_Template1_get_insert_ac() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Function get_insert_ac(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer)
    get_insert_ac = ""
    Try
      If get_previous_ac = True Then
        'Get the AC's from the old DB
      End If

      If insert_ac = True Then
        'Insert the AC information
      End If
      aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_JETNET_compID(jetnet_id)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable2.Rows
            Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference


            aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new


            aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = new_client_ac


            aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = CStr(q("acref_contact_type"))
            aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
            aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = CStr(q("acref_ac_id"))
            aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = CStr(q("acref_operator_flag"))
            aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
            aclsInsert_Client_Aircraft_Reference.cliacref_business_type = CStr(IIf(Not IsDBNull(q("acref_business_type")), q("acref_business_type"), ""))

            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

            If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("edit.aspx.vb - get_insert_ac() - " & error_string)
              End If
              display_error()
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("edit.aspx.vb - get_insert_ac() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "edit.aspx.vb - get_insert_ac() - " & ex.Message
      LogError(error_string)
    End Try
  End Function


  Function get_insert_ac_AC_CLIENT_INSERT(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer, ByVal jetnet_ac_id As Integer)
    get_insert_ac_AC_CLIENT_INSERT = ""
    Try
      If get_previous_ac = True Then
        'Get the AC's from the old DB
      End If

      If insert_ac = True Then
        'Insert the AC information
      End If
      aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_JETNET_compID(jetnet_id)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          ' Response.Write("Company # " & jetnet_id & "<br />")
          For Each q As DataRow In aTempTable2.Rows
            'Response.Write(CStr(q("acref_id")) & " - " & idnum_new & "!!!<br />")
            Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
            aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new

            'Changed october 21st to stop duplicate trans entries
            aTempTable = aclsData_Temp.Get_Aircraft_Reference_Client_JETNET_acID_SORT(q("acref_ac_id"), idnum_new, 0)

            '' check the state of the DataTable
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count = 0 Then
                If jetnet_ac_id = CInt(q("acref_ac_id")) Then
                  aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = new_client_ac
                  aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0

                  aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = CStr(q("acref_contact_type"))
                  aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
                  aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = CStr(q("acref_operator_flag"))
                  aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                  aclsInsert_Client_Aircraft_Reference.cliacref_business_type = CStr(q("acref_business_type"))

                  aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                  aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

                  ' Response.Write(aclsInsert_Client_Aircraft_Reference.ClassInfo(aclsInsert_Client_Aircraft_Reference) & "<Hr />")

                  If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("edit.aspx.vb - get_insert_ac_AC_CLIENT_INSERT() - " & error_string)
                    End If
                    display_error()
                  End If
                Else

                  aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = 0
                  aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = q("acref_ac_id")

                  aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = CStr(q("acref_contact_type"))
                  aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
                  aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = CStr(q("acref_operator_flag"))
                  aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                  aclsInsert_Client_Aircraft_Reference.cliacref_business_type = CStr(q("acref_business_type"))

                  aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                  aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

                  'Response.Write(aclsInsert_Client_Aircraft_Reference.ClassInfo(aclsInsert_Client_Aircraft_Reference) & "<Hr />")

                  If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("edit.aspx.vb - get_insert_ac_AC_CLIENT_INSERT() - " & error_string)
                    End If
                    display_error()
                  End If
                End If
              End If
            End If

          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("edit.aspx.vb - get_insert_ac_AC_CLIENT_INSERT() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "edit.aspx.vb - get_insert_ac_AC_CLIENT_INSERT() - " & ex.Message
      LogError(error_string)
    End Try
  End Function

#End Region
#Region "Page Events"
  Private Sub export_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
      aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      aclsData_Temp.class_error = ""

      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      End If
      'Setting up the display - decide what control to show
      Select Case Trim(Request("action"))
        Case "cyfolder", "aifolder", "ctfolder", "trfolder", "folder"
          Select Case Trim(Request("action"))
            Case "cyfolder"
              titleh.Text = "Company Folder Management - Marketplace Manager"
            Case "aifolder"
              titleh.Text = "Aircraft Folder Management - Marketplace Manager"
            Case "trfolder"
              titleh.Text = "Transaction Folder Management - Marketplace Manager"
            Case "ctfolder"
              titleh.Text = "Contact Folder Management - Marketplace Manager"
            Case "folder"
              titleh.Text = "Add a Folder - Marketplace Manager"
          End Select
          Submenu_Edit_Template1.Visible = True
        Case "preference"
          Preference_Edit_Template1.Visible = True
          titleh.Text = "Preferences Edit - Marketplace Manager"
        Case "user"
          User_Edit_Template1.Visible = True
          titleh.Text = "User Edit - Marketplace Manager"
        Case "view_logs"
          titleh.Text = "Event Logs - Marketplace Manager"
          ViewLogs1.Visible = True
        Case "reference"
          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
            If Not IsNothing(Trim(Request("listing"))) Then
              If Not String.IsNullOrEmpty(Trim(Request("listing"))) Then
                Session.Item("Listing") = Trim(Request("listing"))
              End If
            End If
            If Not IsNothing(Trim(Request("source"))) Then
              If Not String.IsNullOrEmpty(Trim(Request("source"))) Then
                Session.Item("ListingSource") = Trim(Request("source"))
              End If
            End If
            If Not IsNothing(Trim(Request("itemID"))) Then
              If IsNumeric(Trim(Request("itemID"))) Then
                If Trim(Request("itemID")) > 0 Then
                  Session.Item("ListingID") = Trim(Request("itemID"))
                End If
              End If
            End If
          End If

          Contact_Reference_Edit_Template1.Visible = True
          titleh.Text = "Contact References - Marketplace Manager"
        Case "quick"
            ContactQuickEntry1.Visible = True
            titleh.Text = "Contact Quick Entry - Marketplace Manager"
        Case "checkforcreation"
            If Trim(Request("type")) = "company" Then
              Company_Edit_Template1.Visible = True
              titleh.Text = "Check for Company - Marketplace Manager"
            End If
        Case Else
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
              If Not IsNothing(Trim(Request("Listing"))) Then
                If IsNumeric(Trim(Request("Listing"))) Then
                  If Trim(Request("Listing")) > 0 Then
                    Session.Item("Listing") = Trim(Request("Listing"))
                  End If
                End If
              End If
            End If

            Select Case CInt(Session.Item("Listing"))
              Case 2
                If Trim(Request("type")) = "company" Then
                  Company_Edit_Template1.Visible = True
                  Session.Item("Listing") = 1
                  titleh.Text = "Company Maintenance - Marketplace Manager"
                Else
                  Contact_Edit_Template1.Visible = True
                  titleh.Text = "Contact Maintenance - Marketplace Manager"
                End If
              Case 3
                Dim s As String = Trim(Request("type"))
                Select Case Trim(Request("type"))
                  Case "aircraft"
                    Aircraft_Edit_Template1.Visible = True
                    titleh.Text = "Aircraft Maintenance - Marketplace Manager"
                  Case "engine"
                    Aircraft_Edit_Engine_Tab1.Visible = True
                    titleh.Text = "Aircraft Engine Maintenance - Marketplace Manager"
                  Case "avionics"
                    Aircraft_Edit_Avionics_Tab1.Visible = True
                    titleh.Text = "Aircraft Avionics Maintenance - Marketplace Manager"
                  Case "propeller"
                    Aircraft_Edit_Propeller_Tab1.Visible = True
                    titleh.Text = "Aircraft Propeller Maintenance - Marketplace Manager"
                  Case "details"
                    Aircraft_Edit_Details_Tabs1.Visible = True
                    titleh.Text = "Aircraft Details Maintenance - Marketplace Manager"
                  Case "apu"
                    Aircraft_Edit_Details_Tabs1.Visible = True
                    titleh.Text = "Aircraft APU Maintenance - Marketplace Manager"
                  Case "usage"
                    Aircraft_Edit_Details_Tabs1.Visible = True
                    titleh.Text = "Aircraft Usage Maintenance - Marketplace Manager"
                  Case "transaction"
                    Aircraft_Edit_Transactions_Tab1.Visible = True
                    titleh.Text = "Aircraft Transaction Maintenance - Marketplace Manager"
                  Case "features"
                    Aircraft_Edit_Features_Tab1.Visible = True
                    titleh.Text = "Aircraft Features Maintenance - Marketplace Manager"
                End Select
              Case Else
                If Trim(Request("type")) = "company" Then
                  Company_Edit_Template1.Visible = True
                  Session.Item("Listing") = 1
                  ''Contact_Reference_Edit_Template1.Visible = True
                  titleh.Text = "Company Maintenance - Marketplace Manager"
                ElseIf Trim(Request("type")) = "contact" Then
                  Contact_Edit_Template1.Visible = True
                  titleh.Text = "Contact Maintenance - Marketplace Manager"
                ElseIf Trim(Request("type")) = "aircraft" Then
                  Aircraft_Edit_Template1.Visible = True
                  titleh.Text = "Aircraft Maintenance - Marketplace Manager"
                ElseIf Trim(Request("type")) = "transaction" Then
                  Aircraft_Edit_Transactions_Tab1.Visible = True
                  titleh.Text = "Aircraft Transaction Maintenance - Marketplace Manager"
                End If
            End Select
      End Select
    Catch ex As Exception
      error_string = "edit.aspx.vb - Page Load() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Phone Function to Fill Phone Numbers from Company/Contact Edit"
  Sub fill_phone_contact(ByVal type1 As String, ByVal type2 As String, ByVal type3 As String, ByVal type4 As String, ByVal type5 As String, ByVal type6 As String)
    Try
      If Not Page.IsPostBack Then
        'Filling CBO Boxes
        Fill_Cbo(type1, Contact_Edit_Template1)
        Fill_Cbo(type2, Contact_Edit_Template1)
        Fill_Cbo(type3, Contact_Edit_Template1)
        Fill_Cbo(type4, Contact_Edit_Template1)
        Fill_Cbo(type5, Contact_Edit_Template1)
        Fill_Cbo(type6, Contact_Edit_Template1)
      End If
    Catch ex As Exception
      error_string = "edit.aspx.vb - fill_phone_contact() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Fill_Cbo(ByVal z As Object, ByVal q As Control)
    Try
      Dim x As DropDownList = CType(FindControlRecursive(q, z.ToString), DropDownList)

      x.Items.Add(New ListItem("Select One", ""))
      x.Items.Add(New ListItem("Office", "Office"))
      x.Items.Add(New ListItem("Fax", "Fax"))
      x.Items.Add(New ListItem("Residence", "Residence"))
      x.Items.Add(New ListItem("Mobile", "Mobile"))
      x.Items.Add(New ListItem("Residential Fax", "Residential Fax"))
      x.Items.Add(New ListItem("Pager", "Pager"))
      x.Items.Add(New ListItem("Sales", "Sales"))
      x.Items.Add(New ListItem("Toll Free", "Toll Free"))
      x.Items.Add(New ListItem("Charter", "Charter"))
      x.Items.Add(New ListItem("Airport Admin", "Airport Admin"))
      x.Items.Add(New ListItem("Parts", "Parts"))
      x.Items.Add(New ListItem("Customs", "Customs"))
      x.Items.Add(New ListItem("Tower", "Tower"))
      x.Items.Add(New ListItem("Line Service", "Line Service"))
      x.Items.Add(New ListItem("Hangar", "Hangar"))
      x.Items.Add(New ListItem("Hangar Fax", "Hangar Fax"))
      x.Items.Add(New ListItem("Other", "Other"))
    Catch ex As Exception
      error_string = "edit.aspx.vb - Fill_Cbo() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Company_Edit_Template1_Fill_Phone(ByVal type1 As String, ByVal type2 As String, ByVal type3 As String, ByVal type4 As String, ByVal type5 As String, ByVal type6 As String, ByVal con As Control) Handles Company_Edit_Template1.Fill_Phone, ContactQuickEntry1.Fill_Phone, Contact_Edit_Template1.Fill_Phone
    Try
      fill_phone(type1, type2, type3, type4, type5, type6, con)
    Catch ex As Exception
      error_string = "edit.aspx.vb - Fill_Phone() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  'Private Sub Contact_Edit_Template1_Fill_Phone(ByVal type1 As String, ByVal type2 As String, ByVal type3 As String, ByVal type4 As String, ByVal type5 As String, ByVal type6 As String) Handles Contact_Edit_Template1.Fill_Phone
  '    Try
  '        fill_phone_contact(type1, type2, type3, type4, type5, type6)
  '    Catch ex As Exception
  '        error_string = "edit.aspx.vb - Contact_Edit_Template1_Fill_Phone() - " & ex.Message
  '        LogError(error_string)
  '    End Try
  'End Sub
  Sub fill_phone(ByVal type1 As String, ByVal type2 As String, ByVal type3 As String, ByVal type4 As String, ByVal type5 As String, ByVal type6 As String, ByVal con As Control)
    Try
      If Not Page.IsPostBack Then
        'Filling CBO Boxes
        Fill_Cbo(type1, con)
        Fill_Cbo(type2, con)
        Fill_Cbo(type3, con)
        Fill_Cbo(type4, con)
        Fill_Cbo(type5, con)
        Fill_Cbo(type6, con)
      End If
    Catch ex As Exception
      error_string = "edit.aspx.vb - fill_phone() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Functions Needed to be Consolidated"
  Public Function FindControlRecursive(ByVal ctrl As Control, ByVal controlID As String) As Control
    Try
      If String.Compare(ctrl.ID, controlID, True) = 0 Then ' We found the control! 
        Return ctrl
      Else ' Recurse through ctrl's Controls collections 
        For Each child As Control In ctrl.Controls
          Dim lookFor As Control = FindControlRecursive(child, controlID)
          If lookFor IsNot Nothing Then
            Return lookFor ' We found the control 
          End If
        Next
      End If ' If we reach here, control was not found 
      Return Nothing
    Catch ex As Exception
      error_string = "edit.aspx.vb - loop_contacts_ac_add() - " & ex.Message
      LogError(error_string)
    End Try
    Return Nothing
  End Function

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
