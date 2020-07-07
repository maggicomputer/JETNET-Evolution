Partial Public Class crmAdministration
  Inherits System.Web.UI.Page
  ' create a new class from clsData_Manager
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try


      Master.TypeOfListing = 9
      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
      Else
        Response.Redirect("home.aspx")
      End If


    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - Page Load() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
    'End If

  End Sub

  Private Sub Fix_Serial_Sorts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Fix_Serial_Sorts.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Fix_Jetnet_Based_Client_Aircraft_Without_Sort()
      If counter <> "" Then
        attention.Text = "<h1>CHANGE LOG</h1><p align='left'><u>The following Bad Serial # Sorts have been cleaned:</u><br /><br /> " & counter & "</p>"
        attention.Visible = True

      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  Fix_Serial_Sorts_Click() - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - Fix_Jetnet_Based_Client_Aircraft_Without_Sort() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Private Sub Synch_Feature_Codes_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles Synch_Feature_Codes.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Synchronize_Jetnet_Feature_Codes()
      If counter <> "" Then
        attention.Text = "<h1>CHANGE LOG</h1><p align='left'><u>Feature Code Changes:</u><br /><br /> " & counter & "</p>"
        attention.Visible = True

      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  Synch_Feature_Codes() - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - Synch_Feature_Codes() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub



  Private Sub orphaned_aircraft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles orphaned_aircraft.Click
    'This gets rid of all of the orphaned AC records. 
    Try
      Dim counter As String = Master.aclsData_Temp.Clean_Orphaned_Aircraft_Table_Rows()
      If counter <> "" Then
        attention.Text = "<h1>CHANGE LOG</h1><p align='left'><u>Orphaned Aircraft Records have been cleaned using the following queries:</u><br /><br /> " & counter & "</p>"
        attention.Visible = True
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  orphaned_aircraft_Click() - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - orphaned_aircraft_Click() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub


  Private Sub orphaned_contact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles orphaned_contact.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Clean_Orphaned_Contacts_Phones_References()
      If counter <> "" Then
        attention.Text = "<h1>CHANGE LOG</h1><p align='left'><u>Orphaned Contact/Phone/Reference Records have been cleaned using the following queries:</u><br /><br /> " & counter & "</p>"
        attention.Visible = True
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  orphaned_contact_Click() - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - orphaned_contact_Click() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub

  Private Sub orphaned_notes_folders_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles orphaned_notes_folders.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Clean_Orphaned_Notes_Folders()
      If counter <> "" Then
        attention.Text = "<h1>CHANGE LOG</h1><p align='left'><u>Orphaned Notes/Folder Index Records have been cleaned using the following queries:</u><br /><br /> " & counter & "</p>"
        attention.Visible = True
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  orphaned_notes_folders_Click() - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - orphaned_notes_folders_Click() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub

  Private Sub fix_notes_models_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fix_notes_models.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Fix_Client_Notes_Models()
      If counter <> "" Then
        attention.Text = "<h1>CHANGE LOG</h1>" & counter
        attention.Visible = True

      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  Fix_Client_Notes_Models() - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - Fix_Client_Notes_Models() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub

  Private Sub client_aircraft_bad_matches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles client_aircraft_bad_matches.Click
    Try
      Dim counter As String = Master.aclsData_Temp.List_Bad_Client_AC_Matches
      If counter <> "" Then
        attention.Text = "<h1>RESULTS</h1><p>These Aircraft have a bad link:</p>" & counter
        attention.Visible = True

      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  Fix_Client_Notes_Models() - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - Fix_Client_Notes_Models() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub

  Protected Sub potential_orphaned_client_records_Click(ByVal sender As Object, ByVal e As EventArgs) Handles potential_orphaned_client_records.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Identify_Potential_Orphaned_Client_Aircraft_Records
      If counter <> "" Then
        attention.Text = "<h1>RESULTS</h1><p>These Aircraft have a jetnet aircraft ID of 0 and could potentially be orphaned:</p>" & counter
        attention.Visible = True

      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  Identify_Potential_Orphaned_Client_Aircraft_Records - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - Identify_Potential_Orphaned_Client_Aircraft_Records - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Private Sub load_client_maint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles load_client_maint.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Create_Aircraft_Maintenance(0)
      If counter <> "" Then
        attention.Text = "<h1>RESULTS</h1><p>Consolidate:</p>" & counter
        attention.Visible = True

      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  Consolidate_Aircraft_Fixes - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - Consolidate_Aircraft_Fixes - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub 
  Private Sub consolidate_AC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles consolidate_AC.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Consolidate_Aircraft_Fixes
      If counter <> "" Then
        attention.Text = "<h1>RESULTS</h1><p>Consolidate:</p>" & counter
        attention.Visible = True

      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  Consolidate_Aircraft_Fixes - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - Consolidate_Aircraft_Fixes - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub

  Private Sub fixTransactionRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fixTransactionRecords.Click
    Try
      'Fixing Client Transactions
      '1.	For each client transaction
      'a.	Select the JETNET AC ID from the jetnet transaction based on the clitrans_jetnet_trans_id 
      'b.	Fix Client Transactions with No JETNET AC ID - Update the client transaction to have the correct clitrans_jetnet_ac_id
      'c.	Client Transactions with Wrong Client AC ID - For the JETNET AC ID – see if we have a corresponding client aircraft record by matching on select cliaircraft_id, cliaircraft_cliamot from client_aircaft where cliaircraft_jetnet_ac_id = XXX
      'i.	If we get a record then copy the client aircraft id and client model id on to the client transaction record.
      'ii.	If we don’t then make sure that the client aircraft id and client model are 0 on the client transaction.


      Dim counter As String = Master.aclsData_Temp.Fix_Client_Transaction_Records()
      If counter <> "" Then
        attention.Text = "<h1>RESULTS</h1>" & counter
        attention.Visible = True
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  fixTransactionRecords_Click - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - fixTransactionRecords_Click - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub

  Private Sub fixClientAircraftTransactionRecords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fixClientAircraftTransactionRecords.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Fix_Client_Transaction_ClientAC_References_Records()
      If counter <> "" Then
        attention.Text = "<h1>RESULTS</h1>" & counter
        attention.Visible = True
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  fixClientAircraftTransactionRecords_Click - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - fixClientAircraftTransactionRecords_Click - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub

  Private Sub fixTransactionCategories_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fixTransactionCategories.Click
    Try
      Dim counter As String = Master.aclsData_Temp.Fix_Client_Transaction_Categories()
      If counter <> "" Then
        attention.Text = "<h1>RESULTS</h1>" & counter
        attention.Visible = True
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("crmAdministration.aspx.vb  fixTransactionCategories_Click - " & Master.error_string)
        End If
        Master.display_error()
      End If
    Catch ex As Exception
      Master.error_string = "crmAdministration.aspx.vb - fixTransactionCategories_Click - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
End Class