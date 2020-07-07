' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/commonEvo.vb $
'$$Author: Mike $
'$$Date: 6/16/20 11:55a $
'$$Modtime: 6/16/20 11:09a $
'$$Revision: 31 $
'$$Workfile: commonEvo.vb $
'
' ********************************************************************************


Public Class searchMaintenanceItems

  Private _item1 As String = ""
  Private _date1 As String = ""
  Private _time1 As String = ""
  Private _value1 As String = ""
  Private _chk1 As String = ""

  Private _item2 As String = ""
  Private _date2 As String = ""
  Private _time2 As String = ""
  Private _value2 As String = ""
  Private _chk2 As String = ""

  Public Sub New()

    _item1 = ""
    _date1 = ""
    _time1 = ""
    _value1 = ""
    _chk1 = ""

    _item2 = ""
    _date2 = ""
    _time2 = ""
    _value2 = ""
    _chk2 = ""

  End Sub

  Public Property Maintenance_item1() As String
    Get
      Return _item1
    End Get
    Set(ByVal value As String)
      _item1 = value
    End Set
  End Property

  Public Property Maintenance_date1() As String
    Get
      Return _date1
    End Get
    Set(ByVal value As String)
      _date1 = value
    End Set
  End Property

  Public Property Maintenance_time1() As String
    Get
      Return _time1
    End Get
    Set(ByVal value As String)
      _time1 = value
    End Set
  End Property

  Public Property Maintenance_value1() As String
    Get
      Return _value1
    End Get
    Set(ByVal value As String)
      _value1 = value
    End Set
  End Property

  Public Property Maintenance_chk1() As String
    Get
      Return _chk1
    End Get
    Set(ByVal value As String)
      _chk1 = value
    End Set
  End Property

  Public Property Maintenance_item2() As String
    Get
      Return _item2
    End Get
    Set(ByVal value As String)
      _item2 = value
    End Set
  End Property

  Public Property Maintenance_date2() As String
    Get
      Return _date2
    End Get
    Set(ByVal value As String)
      _date2 = value
    End Set
  End Property

  Public Property Maintenance_time2() As String
    Get
      Return _time2
    End Get
    Set(ByVal value As String)
      _time2 = value
    End Set
  End Property

  Public Property Maintenance_value2() As String
    Get
      Return _value2
    End Get
    Set(ByVal value As String)
      _value2 = value
    End Set
  End Property

  Public Property Maintenance_chk2() As String
    Get
      Return _chk2
    End Get
    Set(ByVal value As String)
      _chk2 = value
    End Set
  End Property

End Class

Public Class commonEvo

#Region "client_make_model_array_functions"

  Public Shared Function CreateClientStringFromArray(ByVal s_inArray As Object, ByVal n_inLength As Long, ByVal n_inDimensions As Integer) As String

    ' ok take the input array and create one string to pass to client
    Dim sTemp As New StringBuilder
    Dim outString As String = ""

    Try

      If Not IsNothing(s_inArray) And IsArray(s_inArray) Then

        For x As Integer = 0 To n_inLength

          If n_inDimensions > 0 Then ' ok multi-dimension array add record seperator

            For y As Integer = 0 To n_inDimensions
              ' take the data and place on one string

              If (y = n_inDimensions) Then

                ' append record seperator
                If x < n_inLength Then
                  sTemp.Append(Constants.cSvrDataSeperator + s_inArray(x, y).ToString + Constants.cSvrRecordSeperator)
                Else
                  sTemp.Append(Constants.cSvrDataSeperator + s_inArray(x, y).ToString)
                End If

              Else

                ' append data seperator
                If y > 0 Then
                  sTemp.Append(Constants.cSvrDataSeperator + s_inArray(x, y).ToString)
                Else
                  sTemp.Append(s_inArray(x, y).ToString)
                End If

              End If
            Next

          Else ' single dimension array just collapse

            sTemp.Append(Constants.cSvrDataSeperator + s_inArray(x).ToString)

          End If

        Next

      End If

    Catch ex As Exception

    End Try

    If Not String.IsNullOrEmpty(sTemp.ToString) Then
      ' compleated String : format (length of array, dimensions of array, colapsed array data) 
      outString = n_inLength.ToString + Constants.cSvrStringSeperator + n_inDimensions.ToString + Constants.cSvrStringSeperator + sTemp.ToString
    End If

    Return outString
    sTemp = Nothing

  End Function

  Public Shared Function ReturnAmodIDForItemIndex(ByVal n_inIndex As Long) As Long

    Dim n_outIndex As Long = -1

    Try

      If IsNumeric(n_inIndex) Then

        If Not IsNothing(HttpContext.Current.Session.Item("AirframeArray")) And IsArray(HttpContext.Current.Session.Item("AirframeArray")) Then
          n_outIndex = HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_MODEL_ID)
        End If

      End If

    Catch ex As Exception

    End Try

    Return n_outIndex

  End Function

  Public Shared Function ReturnModelDataFromIndex(ByVal n_inIndex As Long, ByRef sAirFrame As String,
                                                  ByRef sAirType As String, ByRef sMake As String,
                                                  ByRef sModel As String, ByRef sUsage As String,
                                                  Optional ByRef sMfrName As String = "", Optional ByRef sSize As String = "",
                                                  Optional ByRef amod_id_list As String = "") As Boolean

    Dim bResults = False

    Try

      If IsNumeric(n_inIndex) Then
        If Not IsNothing(HttpContext.Current.Session.Item("AirframeArray")) And IsArray(HttpContext.Current.Session.Item("AirframeArray")) Then
          sAirFrame = HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_FRAME)
          sAirType = HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_TYPE)
          sMake = HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_MAKE)
          sModel = HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_MODEL)
          sUsage = HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_USAGE)
          sMfrName = HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_MFRNAME)
          sSize = HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_SIZE)


          If Trim(amod_id_list) <> "" Then
            amod_id_list &= ","
          End If

          amod_id_list &= HttpContext.Current.Session.Item("AirframeArray")(n_inIndex, Constants.AIRFRAME_MODEL_ID)
          'sAirType 

          bResults = True
        End If
      End If

    Catch ex As Exception

    End Try

    Return bResults

  End Function

  Public Shared Function FindIndexForItemByAmodID(ByVal n_inAmodID As Long) As Long

    Dim tValue As String = ""
    Dim n_outIndex As Long = -1

    Try

      If Not IsNothing(HttpContext.Current.Session.Item("AirframeAmodArray")) And IsArray(HttpContext.Current.Session.Item("AirframeAmodArray")) Then

        For xLoop As Integer = 0 To UBound(HttpContext.Current.Session.Item("AirframeAmodArray"))

          tValue = HttpContext.Current.Session.Item("AirframeAmodArray")(xLoop, 0)

          If CLng(tValue) = CLng(n_inAmodID) Then
            n_outIndex = HttpContext.Current.Session.Item("AirframeAmodArray")(xLoop, 1)
            Exit For
          End If

        Next

      End If

    Catch ex As Exception

    End Try

    Return n_outIndex

  End Function

  Public Shared Function FindIndexForFirstItem(ByVal n_inItem As String, ByVal inByTypeMakeModel As Integer, Optional ByVal s_inItem2 As String = "", Optional ByVal extraValueToMatch As Integer = -1) As Long

    Dim tLookUp As String = ""
    Dim tLookUpExtraMatch As String = ""

    Dim tLookUpIndex As Long = 0
    Dim n_outIndex As Long = -1

    Try

      If Not IsNothing(HttpContext.Current.Session.Item("AirframeArray")) And IsArray(HttpContext.Current.Session.Item("AirframeArray")) Then

        For xLoop As Integer = 0 To UBound(HttpContext.Current.Session.Item("AirframeArray"))

          If extraValueToMatch > -1 And Not String.IsNullOrEmpty(s_inItem2.Trim) Then

            tLookUp = HttpContext.Current.Session.Item("AirframeArray")(xLoop, inByTypeMakeModel)
            tLookUpExtraMatch = HttpContext.Current.Session.Item("AirframeArray")(xLoop, extraValueToMatch)

            If Not IsNumeric(n_inItem) And Not IsNumeric(s_inItem2) Then

              If ((tLookUp.ToLower.Trim = n_inItem.ToLower.Trim) And (tLookUpExtraMatch.ToLower.Trim = s_inItem2.ToLower.Trim)) Then
                n_outIndex = HttpContext.Current.Session.Item("AirframeArray")(xLoop, Constants.AIRFRAME_INDEX)
                Exit For
              End If

            Else

              If (CLng(tLookUp) = CLng(n_inItem) And CLng(tLookUpExtraMatch) = CLng(s_inItem2)) Then
                n_outIndex = HttpContext.Current.Session.Item("AirframeArray")(xLoop, Constants.AIRFRAME_INDEX)
                Exit For
              End If

            End If ' Not isNumeric(n_inItem)

          Else

            tLookUp = HttpContext.Current.Session.Item("AirframeArray")(xLoop, inByTypeMakeModel)

            If Not IsNumeric(n_inItem) Then

              If tLookUp.ToLower.Trim = n_inItem.ToLower.Trim Then
                n_outIndex = HttpContext.Current.Session.Item("AirframeArray")(xLoop, Constants.AIRFRAME_INDEX)
                Exit For
              End If

            Else

              If CLng(tLookUp) = CLng(n_inItem) Then
                n_outIndex = HttpContext.Current.Session.Item("AirframeArray")(xLoop, Constants.AIRFRAME_INDEX)
                Exit For
              End If

            End If ' Not isNumeric(n_inItem)

          End If

        Next ' xLoop

      End If ' Not isEmpty(session("AirframeArray")) 

    Catch ex As Exception

    End Try

    Return n_outIndex

  End Function

  Public Shared Function ReturnYachtModelIDForItemIndex(ByVal n_inIndex As Long) As Long

    Dim n_outIndex As Long = -1

    Try

      If IsNumeric(n_inIndex) Then

        If Not IsNothing(HttpContext.Current.Session.Item("YachtArray")) And IsArray(HttpContext.Current.Session.Item("YachtArray")) Then
          n_outIndex = HttpContext.Current.Session.Item("YachtArray")(n_inIndex, Constants.LOCYACHT_MODEL_ID)
        End If

      End If

    Catch ex As Exception

    End Try

    Return n_outIndex

  End Function

  Public Shared Function ReturnYachtModelDataFromIndex(ByVal n_inIndex As Long, ByRef sMotor As String, ByRef sCategory As String, ByRef sBrand As String, ByRef sModel As String) As Boolean

    Dim bResults = False

    Try

      If IsNumeric(n_inIndex) Then
        If Not IsNothing(HttpContext.Current.Session.Item("YachtArray")) And IsArray(HttpContext.Current.Session.Item("YachtArray")) Then
          sMotor = HttpContext.Current.Session.Item("YachtArray")(n_inIndex, Constants.LOCYACHT_MOTOR)
          sCategory = HttpContext.Current.Session.Item("YachtArray")(n_inIndex, Constants.LOCYACHT_CATEGORY)
          sBrand = HttpContext.Current.Session.Item("YachtArray")(n_inIndex, Constants.LOCYACHT_BRAND)
          sModel = HttpContext.Current.Session.Item("YachtArray")(n_inIndex, Constants.LOCYACHT_MODEL)
          bResults = True
        End If
      End If

    Catch ex As Exception

    End Try

    Return bResults

  End Function

  Public Shared Function FindYachtIndexForItemByModelID(ByVal n_inModelID As Long) As Long

    Dim tValue As String = ""
    Dim n_outIndex As Long = -1

    Try

      If Not IsNothing(HttpContext.Current.Session.Item("YachtYmodArray")) And IsArray(HttpContext.Current.Session.Item("YachtYmodArray")) Then

        For xLoop As Integer = 0 To UBound(HttpContext.Current.Session.Item("YachtYmodArray"))

          tValue = HttpContext.Current.Session.Item("YachtYmodArray")(xLoop, 0)

          If CLng(tValue) = CLng(n_inModelID) Then
            n_outIndex = HttpContext.Current.Session.Item("YachtYmodArray")(xLoop, 1)
            Exit For
          End If

        Next

      End If

    Catch ex As Exception

    End Try

    Return n_outIndex

  End Function

  Public Shared Function FindYachtIndexForFirstItem(ByVal n_inItem As String, ByVal inByCategoryBrandModel As Integer, Optional ByVal s_inItem2 As String = "", Optional ByVal extraValueToMatch As Integer = -1) As Long

    Dim tLookUp As String = ""
    Dim tLookUpExtraMatch As String = ""

    Dim tLookUpIndex As Long = 0
    Dim n_outIndex As Long = -1

    Try

      If Not IsNothing(HttpContext.Current.Session.Item("YachtArray")) And IsArray(HttpContext.Current.Session.Item("YachtArray")) Then

        For xLoop As Integer = 0 To UBound(HttpContext.Current.Session.Item("YachtArray"))

          If extraValueToMatch > -1 And Not String.IsNullOrEmpty(s_inItem2.Trim) Then

            tLookUp = HttpContext.Current.Session.Item("YachtArray")(xLoop, inByCategoryBrandModel)
            tLookUpExtraMatch = HttpContext.Current.Session.Item("YachtArray")(xLoop, extraValueToMatch)

            If Not IsNumeric(n_inItem) And Not IsNumeric(s_inItem2) Then

              If ((tLookUp.ToLower.Trim = n_inItem.ToLower.Trim) And (tLookUpExtraMatch.ToLower.Trim = s_inItem2.ToLower.Trim)) Then
                n_outIndex = HttpContext.Current.Session.Item("YachtArray")(xLoop, Constants.LOCYACHT_INDEX)
                Exit For
              End If

            Else

              If (CLng(tLookUp) = CLng(n_inItem) And CLng(tLookUpExtraMatch) = CLng(s_inItem2)) Then
                n_outIndex = HttpContext.Current.Session.Item("YachtArray")(xLoop, Constants.LOCYACHT_INDEX)
                Exit For
              End If

            End If ' Not isNumeric(n_inItem)

          Else

            tLookUp = HttpContext.Current.Session.Item("YachtArray")(xLoop, inByCategoryBrandModel)

            If Not IsNumeric(n_inItem) Then

              If tLookUp.ToLower.Trim = n_inItem.ToLower.Trim Then
                n_outIndex = HttpContext.Current.Session.Item("YachtArray")(xLoop, Constants.LOCYACHT_INDEX)
                Exit For
              End If

            Else

              If CLng(tLookUp) = CLng(n_inItem) Then
                n_outIndex = HttpContext.Current.Session.Item("YachtArray")(xLoop, Constants.LOCYACHT_INDEX)
                Exit For
              End If

            End If ' Not isNumeric(n_inItem)

          End If

        Next ' xLoop
      End If ' Not isEmpty(session("YachtArray")) 

    Catch ex As Exception

    End Try

    Return n_outIndex

  End Function

  Public Shared Sub fillMakeModelDropDown(ByRef MyDropDownControl As DropDownList, ByRef MyListBoxControl As ListBox, ByRef maxWidth As Long, ByRef htmlOutput As String, ByVal inAmodID As String, ByVal bAddAll As Boolean, ByVal bIsWanted As Boolean, ByVal bIsMultiSelect As Boolean, ByVal bAddBlankFirstItem As Boolean, ByVal bIsForSale As Boolean, ByVal bIsSubscriber As Boolean)

    Dim results_table As New DataTable

    Dim fAmod_make_name As String = ""
    Dim fAmod_model_name As String = ""
    Dim fAmod_id As String = ""
    Dim tmpAmodArr() As String = Nothing

    Dim sMakeModelName As String = ""

    If Not bIsSubscriber Then
      If bIsMultiSelect Then
        MyListBoxControl.Items.Clear()
      Else
        MyDropDownControl.Items.Clear()
      End If
    End If

    If String.IsNullOrEmpty(inAmodID) Then
      inAmodID = "-1"
    End If

    Try

      results_table = Get_MakesModels_ByProductCode(bIsForSale)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If bAddBlankFirstItem Then
            If bIsMultiSelect Then
              MyListBoxControl.Items.Add(New ListItem("", ""))
            Else
              MyDropDownControl.Items.Add(New ListItem("", ""))
            End If
          End If

          If Not bIsMultiSelect Then
            If bIsWanted Then
              MyDropDownControl.Items.Add(New ListItem("Please Select One", ""))
            ElseIf bAddAll Then
              MyDropDownControl.Items.Add(New ListItem("All", ""))
            End If
          End If

          If bIsMultiSelect Then
            tmpAmodArr = inAmodID.Split(Constants.cCommaDelim)
          End If

          For Each r As DataRow In results_table.Rows

            If Not (IsDBNull(r("amod_make_name"))) Then
              fAmod_make_name = r.Item("amod_make_name").ToString
            End If

            If Not (IsDBNull(r("amod_model_name"))) Then
              fAmod_model_name = r.Item("amod_model_name").ToString
            End If

            If Not (IsDBNull(r("amod_id"))) Then
              fAmod_id = r.Item("amod_id").ToString
            End If

            If Not bIsSubscriber Then
              sMakeModelName = fAmod_make_name.Trim + " / " + fAmod_model_name.Trim
            Else
              sMakeModelName = fAmod_make_name.Trim + "&nbsp;/&nbsp;" + fAmod_model_name.Trim
            End If

            If (sMakeModelName.Length * Constants._STARTCHARWIDTH) > maxWidth Then
              maxWidth = (sMakeModelName.Length * Constants._STARTCHARWIDTH)
            End If

            If bIsMultiSelect Then
              If bIsSubscriber Then
                If commonEvo.inMyArray(tmpAmodArr, fAmod_id) Then
                  htmlOutput &= "<em>" + sMakeModelName + "</em><br />" & vbCrLf
                End If
              Else
                MyListBoxControl.Items.Add(New ListItem(sMakeModelName, fAmod_id))
              End If
            Else
              MyDropDownControl.Items.Add(New ListItem(sMakeModelName, fAmod_id))
            End If

          Next

          If Not bIsSubscriber Then

            If bIsMultiSelect Then

              If Not String.IsNullOrEmpty(inAmodID) Then
                ' set selected values
                For i As Integer = 0 To MyListBoxControl.Items.Count - 1

                  If commonEvo.inMyArray(tmpAmodArr, MyListBoxControl.Items(i).Value.ToUpper) Then
                    MyListBoxControl.Items(i).Selected = True
                  End If

                Next

              End If

              MyListBoxControl.Width = (maxWidth)

            Else

              If inAmodID <> "-1" Then
                MyDropDownControl.SelectedValue = inAmodID.ToString.Trim
              End If

              MyDropDownControl.Width = (maxWidth)

            End If

          End If

        End If
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />fillMakeModelDropDown(ByRef MyDropDownControl As DropDownList, ByRef MyListBoxControl As ListBox, ByRef maxWidth As Long, ByRef htmlOutput As String, ByVal inAmodID As String, ByVal bAddAll As Boolean, ByVal bIsWanted As Boolean, ByVal bIsMultiSelect As Boolean, ByVal bIsPreferences As Boolean, ByVal bIsForSale As Boolean, ByVal bIsSubscriber As Boolean)</b><br />" + ex.Message

    Finally


    End Try


  End Sub

#End Region

#Region "product_code_selection_query_functions"

  Public Shared Function GenerateAirframeSelectionQuery(ByRef evoSubScriptionCls As clsSubscriptionClass) As String

    Dim sQuery As New StringBuilder
    Dim sTempClause As String = ""
    Dim sSelectionClause As String = ""


    For nloop As Integer = 0 To UBound(evoSubScriptionCls.ProductCode)

      Select Case evoSubScriptionCls.ProductCode(nloop)
        Case eProductCodeTypes.H
          sTempClause += "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y' AND amod_product_helicopter_flag = 'Y')"

        Case eProductCodeTypes.B, eProductCodeTypes.S, eProductCodeTypes.I
          Select Case evoSubScriptionCls.Tierlevel
            Case eTierLevelTypes.JETS
              sTempClause += "(amod_type_code IN ('J','E') AND amod_customer_flag = 'Y'"

            Case eTierLevelTypes.TURBOS
              sTempClause += "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'"

            Case Else
              sTempClause += "(amod_customer_flag = 'Y'"

          End Select

          sTempClause &= Constants.cAndClause + "amod_product_business_flag = 'Y')"

        Case eProductCodeTypes.R
          sTempClause += "(amod_customer_flag = 'Y' AND amod_product_regional_flag = 'Y')"

        Case eProductCodeTypes.C
          Select Case evoSubScriptionCls.Tierlevel
            Case eTierLevelTypes.JETS
              sTempClause += "(amod_type_code IN ('J','E') AND amod_customer_flag = 'Y'"

            Case eTierLevelTypes.TURBOS
              sTempClause += "(amod_type_code IN ('T') AND amod_customer_flag = 'Y'"

            Case Else
              sTempClause += "(amod_customer_flag = 'Y'"

          End Select

          sTempClause += Constants.cAndClause + "amod_product_commercial_flag = 'Y')"

        Case eProductCodeTypes.P
          sTempClause += "(amod_customer_flag = 'Y' AND amod_product_airbp_flag = 'Y')"

        Case eProductCodeTypes.A
          ' sTempClause += "(amod_customer_flag = 'Y' AND amod_product_abi_flag = 'Y')"

        Case eProductCodeTypes.Y
          ' sTempClause += "(amod_customer_flag = 'Y' AND amod_product_yacht_flag = 'Y')"

      End Select

      If UBound(evoSubScriptionCls.ProductCode) > 0 And nloop <= UBound(evoSubScriptionCls.ProductCode) Then
        If (evoSubScriptionCls.ProductCode(nloop) <> eProductCodeTypes.Y) And (evoSubScriptionCls.ProductCode(nloop) <> eProductCodeTypes.NULL) Then
          If String.IsNullOrEmpty(sSelectionClause) Then
            sSelectionClause = sTempClause
          Else
            sSelectionClause += Constants.cOrClause + sTempClause    ' add or clauses for each item
          End If
        End If
        sTempClause = ""
      Else
        sSelectionClause = sTempClause
        sTempClause = ""
      End If

    Next

    If Not String.IsNullOrEmpty(sSelectionClause) Then
      sQuery.Append("SELECT DISTINCT amod_id, amod_airframe_type_code, amod_type_code, amod_make_name, amod_make_abbrev, amod_model_name,")
      sQuery.Append(" amod_product_business_flag, amod_product_commercial_flag, amod_product_helicopter_flag, amod_product_airbp_flag, amod_manufacturer_common_name, amod_jniq_size")
      sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK) WHERE ")
      sQuery.Append(sSelectionClause)
      sQuery.Append(" ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_make_abbrev, amod_id, amod_model_name")
    Else ' default to business if selection string is blank
      sQuery.Append("SELECT DISTINCT amod_id, amod_airframe_type_code, amod_type_code, amod_make_name, amod_make_abbrev, amod_model_name,")
      sQuery.Append(" amod_product_business_flag, amod_product_commercial_flag, amod_product_helicopter_flag, amod_product_airbp_flag, amod_manufacturer_common_name, amod_jniq_size")
      sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK) WHERE ")
      sQuery.Append("(amod_customer_flag = 'Y' AND amod_product_business_flag = 'Y')")
      sQuery.Append(" ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_make_abbrev, amod_id, amod_model_name")
    End If


    Return sQuery.ToString.Trim

    sQuery = Nothing

  End Function

  Public Shared Function GenerateProductCodeSelectionQuery(ByRef evoSubScriptionCls As clsSubscriptionClass, ByVal bNoAndClause As Boolean,
                                                           ByVal bUseModelUsageCode As Boolean, Optional ByVal bExcludeCommercial As Boolean = False) As String

    Dim sSelectionClause As String = ""
    Dim sTmpClause As String = ""

    Dim nloop As Integer = 0
    Dim bSingleProduct As Boolean = True

    Try

      If IsArray(evoSubScriptionCls.ProductCode) And Not IsNothing(evoSubScriptionCls.ProductCode) Then

        ' loop through the inUserProductCode and create the Where Clause  
        For nloop = 0 To UBound(evoSubScriptionCls.ProductCode)

          Select Case evoSubScriptionCls.ProductCode(nloop)
            Case eProductCodeTypes.H

              If bUseModelUsageCode Then
                sTmpClause += "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y' AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y')"
              Else
                sTmpClause += "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y' AND amod_airframe_type_code = 'R' AND ac_product_helicopter_flag = 'Y')"
              End If

            Case eProductCodeTypes.B, eProductCodeTypes.S, eProductCodeTypes.I

              Select Case evoSubScriptionCls.Tierlevel
                Case eTierLevelTypes.JETS
                  sTmpClause += "(amod_type_code IN ('J','E') AND amod_customer_flag = 'Y'"

                Case eTierLevelTypes.TURBOS
                  sTmpClause += "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'"

                Case Else
                  sTmpClause += "(amod_customer_flag = 'Y'"

              End Select

              If bUseModelUsageCode Then
                sTmpClause += Constants.cAndClause + "amod_airframe_type_code = 'F' AND amod_product_business_flag = 'Y')"
              Else
                sTmpClause += Constants.cAndClause + "amod_airframe_type_code = 'F' AND ac_product_business_flag = 'Y')"
              End If

            Case eProductCodeTypes.C

              If Not bExcludeCommercial Then

                Select Case evoSubScriptionCls.Tierlevel
                  Case eTierLevelTypes.JETS
                    sTmpClause += "(amod_type_code IN ('J','E') AND amod_customer_flag = 'Y'"

                  Case eTierLevelTypes.TURBOS
                    sTmpClause += "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'"

                  Case Else
                    sTmpClause += "(amod_customer_flag = 'Y'"

                End Select

                If bUseModelUsageCode Then
                  sTmpClause += Constants.cAndClause + "amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')"
                Else
                  sTmpClause += Constants.cAndClause + "amod_airframe_type_code = 'F' AND ac_product_commercial_flag = 'Y')"
                End If

              Else
                Exit For
              End If

            Case eProductCodeTypes.R

              If bUseModelUsageCode Then
                sTmpClause += "(amod_customer_flag = 'Y' AND amod_product_regional_flag = 'Y')"
              Else
                sTmpClause += "(amod_customer_flag = 'Y' AND ac_product_regional_flag = 'Y')"
              End If

            Case eProductCodeTypes.P

              If bUseModelUsageCode Then
                sTmpClause += "(amod_customer_flag = 'Y' AND amod_product_airbp_flag = 'Y')"
              Else
                sTmpClause += "(amod_customer_flag = 'Y' AND ac_product_airbp_flag = 'Y')"
              End If

            Case eProductCodeTypes.A

              If bUseModelUsageCode Then
                sTmpClause += "(amod_customer_flag = 'Y' AND amod_product_abi_flag = 'Y')"
              Else
                sTmpClause += "(amod_customer_flag = 'Y' AND ac_product_abi_flag = 'Y')"
              End If

            Case eProductCodeTypes.Y

              'If bUseModelUsageCode Then
              '  sTmpClause += "(amod_customer_flag = 'Y' AND amod_product_yacht_flag = 'Y')"
              'Else
              '  sTmpClause += "(amod_customer_flag = 'Y' AND amod_product_yacht_flag = 'Y')"
              'End If

          End Select

          If UBound(evoSubScriptionCls.ProductCode) >= 1 And nloop <= UBound(evoSubScriptionCls.ProductCode) Then
            If Not ((evoSubScriptionCls.ProductCode(nloop) = eProductCodeTypes.Y) Or (evoSubScriptionCls.ProductCode(nloop) = eProductCodeTypes.NULL)) Then

              If Not String.IsNullOrEmpty(sSelectionClause) Then
                sSelectionClause += Constants.cOrClause + sTmpClause ' add or clauses for each item
              Else
                sSelectionClause = sTmpClause
              End If

            End If

            bSingleProduct = False
            sTmpClause = ""

          Else
            sSelectionClause = sTmpClause
            sTmpClause = ""

          End If

        Next

        If Not bSingleProduct Then
          If bNoAndClause Then
            sSelectionClause = Constants.cSingleSpace + Constants.cSingleOpen + sSelectionClause + Constants.cSingleClose
          Else
            sSelectionClause = Constants.cAndClause + Constants.cSingleOpen + sSelectionClause + Constants.cSingleClose
          End If
        Else
          If bNoAndClause Then
            sSelectionClause = Constants.cSingleSpace + sSelectionClause
          Else
            sSelectionClause = Constants.cAndClause + sSelectionClause
          End If
        End If
      Else
        ' if for some reason we dont have a product code array on users subscription default to
        If bNoAndClause Then
          If bUseModelUsageCode Then
            sSelectionClause = Constants.cSingleSpace + "(amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F' AND amod_product_business_flag = 'Y')"
          Else
            sSelectionClause = Constants.cSingleSpace + "(amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F' AND ac_product_business_flag = 'Y')"
          End If
        Else
          If bUseModelUsageCode Then
            sSelectionClause = Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F' AND amod_product_business_flag = 'Y')"
          Else
            sSelectionClause = Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F' AND ac_product_business_flag = 'Y')"
          End If
        End If
      End If

    Catch ex As Exception

    End Try

    Return sSelectionClause.Trim

  End Function

  Public Shared Function MakeCompanyProductCodeClause(ByRef evoSubScriptionCls As clsSubscriptionClass, ByVal bNoAndClause As Boolean) As String

    Dim sSelectionClause As String = ""
    Dim sTmpClause As String = ""
    Dim bSingleProduct As Boolean = True

    Try

      If IsArray(evoSubScriptionCls.ProductCode) And Not IsNothing(evoSubScriptionCls.ProductCode) Then

        For nloop As Integer = 0 To UBound(evoSubScriptionCls.ProductCode)

          Select Case evoSubScriptionCls.ProductCode(nloop)
            Case eProductCodeTypes.H
              sTmpClause += "comp_product_helicopter_flag = 'Y'"
            Case eProductCodeTypes.B, eProductCodeTypes.S, eProductCodeTypes.I
              sTmpClause += "comp_product_business_flag = 'Y'"
            Case eProductCodeTypes.C
              sTmpClause += "comp_product_commercial_flag = 'Y'"
            Case eProductCodeTypes.R
              sTmpClause += "comp_product_regional_flag = 'Y'"
            Case eProductCodeTypes.P
              sTmpClause += "comp_product_airbp_flag = 'Y'"
            Case eProductCodeTypes.A
              sTmpClause += "comp_product_abi_flag = 'Y'"
            Case eProductCodeTypes.Y
              sTmpClause += "comp_product_yacht_flag = 'Y'"
          End Select

          If UBound(evoSubScriptionCls.ProductCode) >= 1 And nloop <= UBound(evoSubScriptionCls.ProductCode) Then
            If Not evoSubScriptionCls.ProductCode(nloop) = eProductCodeTypes.NULL Then
              If Not String.IsNullOrEmpty(sSelectionClause) Then
                sSelectionClause += Constants.cOrClause + sTmpClause ' add or clauses for each item
              Else
                sSelectionClause = sTmpClause
              End If
            End If
            bSingleProduct = False
            sTmpClause = ""
          Else
            sSelectionClause = sTmpClause
            sTmpClause = ""
          End If

        Next

        If Not bSingleProduct Then
          If bNoAndClause Then
            sSelectionClause = Constants.cSingleSpace + Constants.cSingleOpen + sSelectionClause + Constants.cSingleClose
          Else
            sSelectionClause = Constants.cAndClause + Constants.cSingleOpen + sSelectionClause + Constants.cSingleClose
          End If
        Else
          If bNoAndClause Then
            sSelectionClause = Constants.cSingleSpace + sSelectionClause
          Else
            sSelectionClause = Constants.cAndClause + sSelectionClause
          End If
        End If

      Else

        If bNoAndClause Then
          sSelectionClause = Constants.cSingleSpace + "comp_product_business_flag = 'Y'"
        Else
          sSelectionClause = Constants.cAndClause + "comp_product_business_flag = 'Y'"
        End If

      End If

    Catch ex As Exception

    End Try

    Return sSelectionClause.Trim

  End Function

  Public Shared Function MakeMarketProductCodeClause(ByRef evoSubScriptionCls As clsSubscriptionClass, ByVal bNoAndClause As Boolean) As String

    Dim sSelectionClause As String = ""
    Dim sTmpClause As String = ""
    Dim bSingleProduct As Boolean = True

    Try

      If IsArray(evoSubScriptionCls.ProductCode) And Not IsNothing(evoSubScriptionCls.ProductCode) Then

        For nloop As Integer = 0 To UBound(evoSubScriptionCls.ProductCode)

          Select Case evoSubScriptionCls.ProductCode(nloop)
            Case eProductCodeTypes.H
              sTmpClause += "mtrend_product_type = '" + Constants.PRODUCT_TYPE_H + "'"
            Case eProductCodeTypes.B, eProductCodeTypes.S, eProductCodeTypes.I
              sTmpClause += "mtrend_product_type = '" + Constants.PRODUCT_TYPE_B + "'"
            Case eProductCodeTypes.C
              sTmpClause += "mtrend_product_type = '" + Constants.PRODUCT_TYPE_C + "'"
            Case eProductCodeTypes.R
              sTmpClause += "mtrend_product_type = '" + Constants.PRODUCT_TYPE_R + "'"
            Case eProductCodeTypes.P
              sTmpClause += "mtrend_product_type = '" + Constants.PRODUCT_TYPE_P + "'"
            Case eProductCodeTypes.A
              sTmpClause += "mtrend_product_type = '" + Constants.PRODUCT_TYPE_A + "'"
            Case eProductCodeTypes.Y
              'sTmpClause += "mtrend_product_type = '" + Constants.PRODUCT_TYPE_Y + "'"
          End Select

          If UBound(evoSubScriptionCls.ProductCode) >= 1 And nloop <= UBound(evoSubScriptionCls.ProductCode) Then
            If Not ((evoSubScriptionCls.ProductCode(nloop) = eProductCodeTypes.Y) Or (evoSubScriptionCls.ProductCode(nloop) = eProductCodeTypes.NULL)) Then
              If Not String.IsNullOrEmpty(sSelectionClause) Then
                sSelectionClause += Constants.cOrClause + sTmpClause ' add or clauses for each item
              Else
                sSelectionClause = sTmpClause
              End If
            End If
            bSingleProduct = False
            sTmpClause = ""
          Else
            sSelectionClause = sTmpClause
            sTmpClause = ""
          End If

        Next

        If Not bSingleProduct Then
          If bNoAndClause Then
            sSelectionClause = Constants.cSingleSpace + Constants.cSingleOpen + sSelectionClause + Constants.cSingleClose
          Else
            sSelectionClause = Constants.cAndClause + Constants.cSingleOpen + sSelectionClause + Constants.cSingleClose
          End If
        Else
          If bNoAndClause Then
            sSelectionClause = Constants.cSingleSpace + sSelectionClause
          Else
            sSelectionClause = Constants.cAndClause + sSelectionClause
          End If
        End If

      Else

        If bNoAndClause Then
          sSelectionClause = Constants.cSingleSpace + "mtrend_product_type = 'B'"
        Else
          sSelectionClause = Constants.cAndClause + "mtrend_product_type = 'B'"
        End If

      End If

    Catch ex As Exception

    End Try

    Return sSelectionClause.Trim

  End Function

  Public Shared Function BuildProductCodeCheckWhereClause(ByVal bHasHeli As Boolean, ByVal bHasBus As Boolean, ByVal bHasCom As Boolean, ByVal bHasReg As Boolean, ByVal bHasYht As Boolean, ByVal bNoAndClause As Boolean, ByVal bUseModelCode As Boolean) As String

    Dim sQuery As StringBuilder = New StringBuilder()

    Dim sTmpClause As String = ""

    Dim sHelicopterClause As String = ""
    Dim sBusinessClause As String = ""
    Dim sCommercialClause As String = ""
    Dim sSeperator As String = ""

    If bNoAndClause Then
      sSeperator = Constants.cSingleSpace
    Else
      sSeperator = Constants.cAndClause
    End If

    If bUseModelCode Then
      sHelicopterClause = "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y' AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y')"
    Else
      sHelicopterClause = "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y' AND amod_airframe_type_code = 'R' AND ac_product_helicopter_flag = 'Y')"
    End If

    Select Case HttpContext.Current.Session.Item("localPreferences").Tierlevel
      Case eTierLevelTypes.JETS
        sTmpClause = "(amod_type_code IN ('J','E') AND amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'"

      Case eTierLevelTypes.TURBOS
        sTmpClause = "(amod_type_code IN ('T','P') AND amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'"

      Case Else
        sTmpClause = "(amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'"

    End Select

    If bUseModelCode Then
      sBusinessClause = sTmpClause + Constants.cAndClause + "amod_product_business_flag = 'Y')"
    Else
      sBusinessClause = sTmpClause + Constants.cAndClause + "ac_product_business_flag = 'Y')"
    End If

    If bUseModelCode Then
      sCommercialClause = sTmpClause + Constants.cAndClause + "amod_product_commercial_flag = 'Y')"
    Else
      sCommercialClause = sTmpClause + Constants.cAndClause + "ac_product_commercial_flag = 'Y')"
    End If

    ' just start with h, b, c

    If Not bHasHeli And Not bHasBus And Not bHasCom Then ' error user has to select "AT LEAST ONE"
      sQuery.Append(sSeperator + Constants.cSingleOpen + sHelicopterClause + Constants.cOrClause + sBusinessClause + Constants.cOrClause + sCommercialClause + Constants.cSingleClose)
    End If

    If Not bHasHeli And Not bHasBus And bHasCom Then
      sQuery.Append(sSeperator + sCommercialClause)
    End If

    If Not bHasHeli And bHasBus And Not bHasCom Then
      sQuery.Append(sSeperator + sBusinessClause)
    End If

    If Not bHasHeli And bHasBus And bHasCom Then
      sQuery.Append(sSeperator + Constants.cSingleOpen + sBusinessClause + Constants.cOrClause + sCommercialClause + Constants.cSingleClose)
    End If

    If bHasHeli And Not bHasBus And Not bHasCom Then
      sQuery.Append(sSeperator + sHelicopterClause)
    End If

    If bHasHeli And Not bHasBus And bHasCom Then
      sQuery.Append(sSeperator + Constants.cSingleOpen + sHelicopterClause + Constants.cOrClause + sCommercialClause + Constants.cSingleClose)
    End If

    If bHasHeli And bHasBus And Not bHasCom Then
      sQuery.Append(sSeperator + Constants.cSingleOpen + sHelicopterClause + Constants.cOrClause + sBusinessClause + Constants.cSingleClose)
    End If

    If bHasHeli And bHasBus And bHasCom Then
      sQuery.Append(sSeperator + Constants.cSingleOpen + sHelicopterClause + Constants.cOrClause + sBusinessClause + Constants.cOrClause + sCommercialClause + Constants.cSingleClose)
    End If

    Return sQuery.ToString

    sQuery = Nothing

  End Function

  Public Shared Function BuildCompanyProductCodeCheckWhereClause(ByVal bHasHeli As Boolean, ByVal bHasBus As Boolean, ByVal bHasCom As Boolean, ByVal bHasReg As Boolean, ByVal bHasYht As Boolean, ByVal bNoAndClause As Boolean) As String

    Dim sQuery As StringBuilder = New StringBuilder()
    Dim sSeperator As String = ""


    If bNoAndClause Then
      sSeperator = Constants.cSingleSpace
    Else
      sSeperator = Constants.cAndClause
    End If

    'H B C Y
    '0 0 0 0
    If Not bHasHeli And Not bHasBus And Not bHasCom And Not bHasYht Then ' error user has to select "AT LEAST ONE"
      sQuery.Append(sSeperator + "(comp_product_helicopter_flag = 'Y' OR comp_product_business_flag = 'Y' OR comp_product_commercial_flag = 'Y' OR comp_product_yacht_flag = 'Y')")
    End If

    'H B C Y
    '0 0 0 1
    If Not bHasHeli And Not bHasBus And Not bHasCom And bHasYht Then
      sQuery.Append(sSeperator + "comp_product_yacht_flag = 'Y'")
    End If

    'H B C Y
    '0 0 1 0
    If Not bHasHeli And Not bHasBus And bHasCom And Not bHasYht Then
      sQuery.Append(sSeperator + "comp_product_commercial_flag = 'Y'")
    End If

    'H B C Y
    '0 0 1 1
    If Not bHasHeli And Not bHasBus And bHasCom And bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_commercial_flag = 'Y' OR comp_product_yacht_flag = 'Y')")
    End If

    'H B C Y
    '0 1 0 0
    If Not bHasHeli And bHasBus And Not bHasCom And Not bHasYht Then
      sQuery.Append(sSeperator + "comp_product_business_flag = 'Y'")
    End If

    'H B C Y
    '0 1 0 1
    If Not bHasHeli And bHasBus And Not bHasCom And bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_business_flag = 'Y' OR comp_product_yacht_flag = 'Y')")
    End If

    'H B C Y
    '0 1 1 0
    If Not bHasHeli And bHasBus And bHasCom And Not bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_business_flag = 'Y' OR comp_product_commercial_flag = 'Y')")
    End If

    'H B C Y
    '0 1 1 1
    If Not bHasHeli And bHasBus And bHasCom And bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_business_flag = 'Y' OR comp_product_commercial_flag = 'Y' OR comp_product_yacht_flag = 'Y')")
    End If

    'H B C Y
    '1 0 0 0
    If bHasHeli And Not bHasBus And Not bHasCom And Not bHasYht Then
      sQuery.Append(sSeperator + "comp_product_helicopter_flag = 'Y'")
    End If

    'H B C Y
    '1 0 0 1
    If bHasHeli And Not bHasBus And Not bHasCom And bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_helicopter_flag = 'Y' OR comp_product_yacht_flag = 'Y')")
    End If

    'H B C Y
    '1 0 1 0
    If bHasHeli And Not bHasBus And bHasCom And Not bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_helicopter_flag = 'Y' OR comp_product_commercial_flag = 'Y')")
    End If

    'H B C Y
    '1 0 1 1
    If bHasHeli And Not bHasBus And bHasCom And bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_helicopter_flag = 'Y' OR comp_product_commercial_flag = 'Y' OR comp_product_yacht_flag = 'Y')")
    End If

    'H B C Y
    '1 1 0 0
    If bHasHeli And bHasBus And Not bHasCom And Not bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_helicopter_flag = 'Y' OR comp_product_business_flag = 'Y')")
    End If

    'H B C Y
    '1 1 0 1
    If bHasHeli And bHasBus And Not bHasCom And bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_helicopter_flag = 'Y' OR comp_product_business_flag = 'Y' OR comp_product_yacht_flag = 'Y')")
    End If

    'H B C Y
    '1 1 1 0
    If bHasHeli And bHasBus And bHasCom And Not bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_helicopter_flag = 'Y' OR comp_product_business_flag = 'Y' OR comp_product_commercial_flag = 'Y')")
    End If

    'H B C Y
    '1 1 1 1
    If bHasHeli And bHasBus And bHasCom And bHasYht Then
      sQuery.Append(sSeperator + "(comp_product_helicopter_flag = 'Y' OR comp_product_business_flag = 'Y' OR comp_product_commercial_flag = 'Y' OR comp_product_yacht_flag = 'Y')")
    End If

    Return sQuery.ToString

    sQuery = Nothing

  End Function

  Public Shared Function BuildMarketProductCodeCheckWhereClause(ByVal bHasHeli As Boolean, ByVal bHasBus As Boolean, ByVal bHasCom As Boolean, ByVal bHasReg As Boolean, ByVal bHasYht As Boolean, ByVal bNoAndClause As Boolean) As String

    Dim sQuery As StringBuilder = New StringBuilder()
    Dim sSeperator As String = ""
    ' just start with h, b, c

    If bNoAndClause Then
      sSeperator = Constants.cSingleSpace
    Else
      sSeperator = Constants.cAndClause
    End If

    If Not bHasHeli And Not bHasBus And Not bHasCom Then ' error user has to select "AT LEAST ONE"
      sQuery.Append(sSeperator + "(mtrend_product_type = 'H' OR mtrend_product_type = 'B' OR mtrend_product_type = 'C')")
    End If

    If Not bHasHeli And Not bHasBus And bHasCom Then
      sQuery.Append(sSeperator + "mtrend_product_type = 'C'")
    End If

    If Not bHasHeli And bHasBus And Not bHasCom Then
      sQuery.Append(sSeperator + "mtrend_product_type = 'B'")
    End If

    If Not bHasHeli And bHasBus And bHasCom Then
      sQuery.Append(sSeperator + "(mtrend_product_type = 'B' OR mtrend_product_type = 'C')")
    End If

    If bHasHeli And Not bHasBus And Not bHasCom Then
      sQuery.Append(sSeperator + "mtrend_product_type = 'H'")
    End If

    If bHasHeli And Not bHasBus And bHasCom Then
      sQuery.Append(sSeperator + "(mtrend_product_type = 'H' OR mtrend_product_type = 'C')")
    End If

    If bHasHeli And bHasBus And Not bHasCom Then
      sQuery.Append(sSeperator + "(mtrend_product_type = 'H' OR mtrend_product_type = 'B')")
    End If

    If bHasHeli And bHasBus And bHasCom Then
      sQuery.Append(sSeperator + "(mtrend_product_type = 'H' OR mtrend_product_type = 'B' OR mtrend_product_type = 'C')")
    End If

    Return sQuery.ToString

    sQuery = Nothing

  End Function

#End Region

#Region "helper_functions"

  Public Shared Function cleanTempFilesDirectory() As Boolean

    If CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString) = 0 Or String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim) Then Return False

    Try

      Dim sFileNames As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath"))
      Dim fileList As String() = System.IO.Directory.GetFiles(sFileNames, HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + "_" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + "*.*")

      For Each f As String In fileList
        System.IO.File.Delete(f)
      Next

      Return True
    Catch ex As Exception
      Return False
    End Try

  End Function

  Public Shared Function inMyArray(ByRef theArray() As String, ByVal strFind As String) As Boolean

    Dim bResults As Boolean = False

    Dim theArrayValue As String = ""
    Dim lCnt1 As Integer = 0

    Try

      If Not IsNothing(theArray) And IsArray(theArray) Then

        Select Case theArray.Length

          Case Is > 0 ' loop through the array see if an array item matches strFind

            Do

              If IsNothing(theArray(lCnt1)) Then
                Exit Do
              Else
                theArrayValue = theArray(lCnt1).ToString.Trim
              End If

              If (IsNumeric(theArrayValue) And IsNumeric(strFind)) Then
                If CLng(theArrayValue.ToString) = CLng(strFind.ToString) Then
                  bResults = True
                End If
              Else
                If theArrayValue.ToString.ToUpper.Trim = strFind.ToString.ToUpper.Trim Then
                  bResults = True
                End If
              End If

              lCnt1 += 1

            Loop Until (bResults = True) Or (lCnt1 = theArray.Length)

          Case Is = 0 ' there is one item in the array see if this matches strFind

            If IsNothing(theArray(lCnt1)) Then
              Exit Select
            Else
              theArrayValue = theArray(lCnt1).ToString.Trim
            End If

            If (IsNumeric(theArrayValue) And IsNumeric(strFind)) Then
              If CLng(theArrayValue.ToString) = CLng(strFind.ToString) Then
                bResults = True
              End If
            Else
              If theArrayValue.ToString.ToUpper.Trim = strFind.ToString.ToUpper.Trim Then
                bResults = True
              End If
            End If

          Case Is < 0 ' there are no items in the array return false

            bResults = False

        End Select

      End If 'isArray(theArray) and not IsEmpty(theArray)

    Catch ex As Exception

      bResults = False

    End Try

    Return bResults

  End Function

  Public Shared Function scrubEmailString(ByVal inEmailString As String) As String

    Dim strResults As String = inEmailString.ToLower.Trim

    'If strResults.Contains("script") Or strResults.Contains("a href") Or strResults.Contains("html") Or strResults.Contains("iframe") Or _
    '   strResults.Contains(".php?") Or strResults.Contains("http") Or strResults.Contains("sys") Then

    '  Return "INVALID"
    '  Exit Function

    'End If

    ' scrub data for common sql key words and remove them
    strResults = strResults.Replace("alter", "")
    strResults = strResults.Replace("select", "")
    strResults = strResults.Replace("where", "")
    strResults = strResults.Replace("from", "")
    strResults = strResults.Replace("delete", "")
    strResults = strResults.Replace("update", "")
    strResults = strResults.Replace("convert", "")
    strResults = strResults.Replace("sysobjects", "")
    strResults = strResults.Replace("char", "")
    strResults = strResults.Replace("cast", "")
    strResults = strResults.Replace("varchar", "")
    strResults = strResults.Replace("version", "")

    strResults = strResults.Replace("@@", "")

    ' scrub data for common HTML key words and remove them
    strResults = strResults.Replace("http://", "")
    strResults = strResults.Replace("https://", "")
    strResults = strResults.Replace("a href", "")
    strResults = strResults.Replace("/a>", "")
    strResults = strResults.Replace("/a", "")
    strResults = strResults.Replace("iframe", "")
    strResults = strResults.Replace("/iframe>", "")

    strResults = strResults.Replace("/script>", "")
    strResults = strResults.Replace("/script", "")
    strResults = strResults.Replace(" script", "")
    strResults = strResults.Replace("script", "")

    strResults = strResults.Replace("/html>", "")
    strResults = strResults.Replace("html", "")
    strResults = strResults.Replace(" html", "")
    strResults = strResults.Replace("html>", "")

    strResults = strResults.Replace(".php?", "")

    Return strResults

  End Function

  Public Shared Function getBrowserCapabilities(ByRef clientBrowser As HttpBrowserCapabilities) As String

    Dim osString As String = ""
    Dim browserString As String = ""
    Dim s As String = ""

    Dim sPad As Char = Constants.cSingleSpace

    osString = clientBrowser.Platform.Trim.PadRight(7, sPad)
    browserString = clientBrowser.Browser.Trim + Constants.cSingleSpace + clientBrowser.Version.Trim

    'With clientBrowser                          1234567 1234567890123456789012345
    '  s &= "Browser Capabilities" & vbCrLf      winnt   internetexplorer 11.0
    '  s &= "Type = " & .Type & vbCrLf
    '  s &= "Name = " & .Browser & vbCrLf
    '  s &= "Version = " & .Version & vbCrLf
    '  s &= "Major Version = " & .MajorVersion & vbCrLf
    '  s &= "Minor Version = " & .MinorVersion & vbCrLf
    '  s &= "Platform = " & .Platform & vbCrLf
    '  s &= "Is Beta = " & .Beta & vbCrLf
    '  s &= "Is Crawler = " & .Crawler & vbCrLf
    '  s &= "Is AOL = " & .AOL & vbCrLf
    '  s &= "Is Win16 = " & .Win16 & vbCrLf
    '  s &= "Is Win32 = " & .Win32 & vbCrLf
    '  s &= "Supports Frames = " & .Frames & vbCrLf
    '  s &= "Supports Tables = " & .Tables & vbCrLf
    '  s &= "Supports Cookies = " & .Cookies & vbCrLf
    '  s &= "Supports VBScript = " & .VBScript & vbCrLf
    '  s &= "Supports JavaScript = " & _
    '      .EcmaScriptVersion.ToString() & vbCrLf
    '  s &= "Supports Java Applets = " & .JavaApplets & vbCrLf
    '  s &= "Supports ActiveX Controls = " & .ActiveXControls & _
    '      vbCrLf
    '  s &= "Supports JavaScript Version = " & _
    '      clientBrowser("JavaScriptVersion") & vbCrLf
    'End With

    Return LCase(osString + Constants.cSingleSpace + browserString)

  End Function

  Public Shared Function get_crm_client_info(ByVal crm_user_email_address As String, ByVal sFirstName As String, ByVal sLastName As String, ByRef outErrorString As String) As Long

    ' look up client database info from CRM client_register_master table
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing

    Dim cliuser_id As Long = 0

    Dim sQuery As String = "SELECT cliuser_id FROM client_user WHERE cliuser_active_flag = 'Y' and cliuser_login = '" + crm_user_email_address.Trim.ToLower + "'"

    Try

      Try

        MySqlConn.ConnectionString = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn.ToString()
        MySqlConn.Open()

        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60
        MySqlCommand.CommandText = sQuery

        MySqlReader = MySqlCommand.ExecuteReader()

        If MySqlReader.HasRows Then

          MySqlReader.Read()

          If Not (IsDBNull(MySqlReader("cliuser_id"))) Then
            cliuser_id = MySqlReader.Item("cliuser_id")
          End If

          MySqlReader.Close()
          MySqlReader.Dispose()
        Else

          MySqlReader.Close()
          MySqlReader = Nothing

          ' insert a new user into CRM user table ...  

          sQuery = "INSERT INTO client_user (cliuser_first_name, cliuser_last_name, cliuser_login, cliuser_email_address, cliuser_active_flag) VALUES"
          sQuery &= "('" + sFirstName + "','" + sLastName + "','" + crm_user_email_address + "','" + crm_user_email_address + "','Y')"

          MySqlCommand.CommandText = sQuery
          MySqlCommand.ExecuteNonQuery()

          ' get the CRM client id and return it ...

          sQuery = "SELECT cliuser_id FROM client_user WHERE cliuser_active_flag = 'Y' and cliuser_login = '" + crm_user_email_address.Trim.ToLower + "'"
          MySqlCommand.CommandText = sQuery

          MySqlReader = MySqlCommand.ExecuteReader()

          If MySqlReader.HasRows Then

            MySqlReader.Read()

            If Not (IsDBNull(MySqlReader("cliuser_id"))) Then
              cliuser_id = MySqlReader.Item("cliuser_id")
            End If

            MySqlReader.Close()
            MySqlReader.Dispose()

          Else

            MySqlReader.Close()
            MySqlReader = Nothing

          End If

        End If 'MySqlReader.HasRows 

      Catch MySqlException

        outErrorString = "Error in CRM user Lookup " + crm_user_email_address + " | " + MySqlException.Message.Trim

        MySqlConn.Dispose()
        MySqlCommand.Dispose()

      Finally

        MySqlConn.Close()
        MySqlCommand.Dispose()
        MySqlConn.Dispose()

      End Try

    Catch ex As Exception

      outErrorString = "Error in CRM user Lookup " + crm_user_email_address + " | " + ex.Message.Trim

    End Try

    Return cliuser_id

  End Function

  Public Shared Function GenerateFileName(ByVal s_filename As String, ByVal s_filetype As String, ByVal b_replace_filetype As Boolean) As String

    Dim s_seperator As String = "_"
    Dim s_tmpFileName As String = ""

    Dim s_day As String = ""
    Dim s_month As String = ""
    Dim s_year As String = ""
    Dim n_hour As Integer = 0
    Dim s_minute As String = ""
    Dim s_second As String = ""
    Dim s_msecond As String = ""
    Dim s_ampm As String = ""

    If Not b_replace_filetype Then

      s_day = Now().Day.ToString
      s_month = Now().Month.ToString
      s_year = Now().Year.ToString
      n_hour = CInt(Now().Hour.ToString)
      s_minute = Now().Minute.ToString
      s_second = Now().Second.ToString
      s_msecond = Now().Millisecond.ToString

      Select Case n_hour

        Case 0
          s_ampm = "AM"
          n_hour = 12
        Case 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11
          s_ampm = "AM"
        Case 12
          s_ampm = "PM"
        Case 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23
          s_ampm = "PM"
          n_hour = n_hour - 12

      End Select

      s_tmpFileName = s_month + s_seperator + s_day + s_seperator + s_year +
                    s_seperator + n_hour.ToString + s_seperator + s_minute + s_seperator + s_second + s_seperator + s_msecond +
                    s_seperator + s_ampm

      If Not String.IsNullOrEmpty(s_filename) Then
        s_tmpFileName = s_filename + s_seperator + s_tmpFileName
      End If

      If Not String.IsNullOrEmpty(s_filetype) Then
        s_tmpFileName += s_filetype
      End If

    Else

      Dim pos As Integer = s_filename.IndexOf(".")

      If pos > 0 Then
        'strip off old extension and put new one on
        s_tmpFileName = s_filename.Remove(pos, (s_filename.Length - pos))
        s_tmpFileName += s_filetype
      Else
        s_tmpFileName = s_filename + s_filetype
      End If

    End If

    Return s_tmpFileName

  End Function

  Public Shared Function DeterminePageSize(ByVal nTotalRecords As Long, ByVal nUserPageSize As Long) As Long

    ' we should always have one page
    Dim nPageSize As Long = 1

    ' only have to determine page size if page size is less then the total records
    If nUserPageSize < nTotalRecords Then

      nPageSize = nTotalRecords Mod nUserPageSize

      If nPageSize = 0 Then  ' no remainder even record count vs page size
        nPageSize = nTotalRecords / nUserPageSize
      ElseIf nPageSize > 0 Then

        If nPageSize > (nUserPageSize / 2) Then
          nPageSize = Math.Round(nTotalRecords / nUserPageSize)
        Else
          nPageSize = Math.Round(nTotalRecords / nUserPageSize) + 1
        End If

      End If

    End If

    Return nPageSize

  End Function

  Public Shared Function ParseUserInputData(ByVal sInputString As String, ByVal sFind As String, ByVal sReplace As String, ByVal bIsTextAreaInput As Boolean) As String

    ' parse "cut and paste" column into a "sReplace value" delimited string

    Dim n_loop As Integer = 0
    Dim n_offset As Integer = 0
    Dim n_offset1 As Integer = 0
    Dim CRLF = Chr(13) + Chr(10)

    Dim sTmpData As String = ""
    Dim sOutputString As String = ""

    If Not String.IsNullOrEmpty(sInputString.Trim) Then

      If Not bIsTextAreaInput Then
        sOutputString = sInputString.Replace(sFind, sReplace).Trim
      Else

        Do While n_loop < sInputString.Length + 1

          ' find first CRLF pair
          n_offset = sInputString.IndexOfAny(CRLF, n_loop)

          ' find second CRLF pair
          n_offset1 = sInputString.IndexOfAny(CRLF, n_offset + 1)

          If n_offset > 0 And n_offset1 = -1 Then
            n_offset1 = sInputString.Length
          End If

          ' grab first item from n_loop to n_offset
          If (n_offset > n_loop) Then

            sTmpData = sInputString.Substring(n_loop, n_offset)

            ' clean out any "GA" Garbage also zero length data
            If Not sTmpData.ToUpper.Contains("GA") And sTmpData.Length > 0 Then

              ' I also need to preserve any commas in the data
              If sTmpData.Contains(Constants.cCommaDelim) Then
                ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
                sTmpData = sTmpData.Replace(Constants.cCommaDelim, Constants.cImbedComa)
              End If

              ' clean out the EXCEL 03 Character
              sTmpData = sTmpData.Replace(Constants.EXCEL2003CHAR, Constants.cEmptyString)

              If String.IsNullOrEmpty(sOutputString.Trim) Then
                sOutputString = sTmpData
              Else
                sOutputString += sReplace + sTmpData.Trim
              End If

            End If
          End If

          sTmpData = ""

          ' if one or other offset = -1 and both offsets are equal return input string
          If (n_offset = -1 Or n_offset1 = -1) And n_offset = n_offset1 Then
            sOutputString = sInputString.Trim
            Exit Do
          End If

          If (n_offset1 > n_loop) Then  ' found second CRLF after our start 

            ' find next CRLF start 1 chars ahead of our first CRLF pair
            If (n_offset1 > n_offset) Then ' found next CRLF the data is between the two offsets
              If (n_offset1 - n_offset) > 1 Then ' ok we have at least one char between the two

                sTmpData = sInputString.Substring(n_offset + 1, ((n_offset1 - n_offset) - 1)) ' ok get the data

                ' clean out any "GA" Garbage also zero length data
                If Not sTmpData.ToUpper.Contains("GA") And sTmpData.Length > 0 Then

                  ' I also need to preserve any commas in the data
                  If sTmpData.Contains(Constants.cCommaDelim) Then
                    ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
                    sTmpData = sTmpData.Replace(Constants.cCommaDelim, Constants.cImbedComa)
                  End If

                  ' clean out the EXCEL 03 Character
                  sTmpData = sTmpData.Replace(Constants.EXCEL2003CHAR, Constants.cEmptyString)

                  If String.IsNullOrEmpty(sOutputString.Trim) Then
                    sOutputString = sTmpData
                  Else
                    sOutputString += sReplace + sTmpData.Trim
                  End If

                End If
              End If
            End If

          Else
            Exit Do
          End If

          ' jump ahead n_offset1 to look for the next chunk of data
          If n_offset1 > 0 Then
            n_loop = n_offset1
          End If

          n_offset = 0
          n_offset1 = 0
          sTmpData = ""

        Loop ' While n_loop < sInputString.Length + 1

        If Not String.IsNullOrEmpty(sOutputString.Trim) Then

          ' chop off the last comma if there is one
          If Right(sOutputString, 1) = Constants.cCommaDelim Or
            Right(sOutputString, 1) = Constants.cColonDelim Or
            Right(sOutputString, 1) = Constants.cSemiColonDelim Then
            sOutputString = sOutputString.Substring(1, sOutputString.Length - 1)
          End If

        Else

          ' I also need to preserve any commas in the data
          If sInputString.Contains(Constants.cCommaDelim) Then
            ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
            sInputString = sInputString.Replace(Constants.cCommaDelim, Constants.cImbedComa)
          End If

          ' clean out the EXCEL 03 Character
          sInputString = sInputString.Replace(Constants.EXCEL2003CHAR, Constants.cEmptyString)

          ' clean out the CRLF
          sInputString = sInputString.Replace(CRLF, Constants.cEmptyString)

          sOutputString = sInputString.Trim

        End If

      End If ' not bIsTextAreaInput

      ' chop off the last comma if there is one
      If Right(sOutputString, 1) = Constants.cCommaDelim Or
         Right(sOutputString, 1) = Constants.cColonDelim Or
         Right(sOutputString, 1) = Constants.cSemiColonDelim Then
        sOutputString = sOutputString.Substring(1, sOutputString.Length - 1)
      End If

    End If ' Not String.IsNullOrEmpty(sInputString.Trim)

    Return sOutputString

  End Function

  Public Shared Function CheckForIataIcaoCode(ByVal sInCodes As String, ByRef sOutCodes As String, ByVal b_isIata As Boolean) As String

    Dim sWhereClause As String = ""

    Dim sHoldCodes As String = ""
    Dim sIataIcao As String = ""
    Dim sArrayItem As String = ""
    Dim codesArray() As String = Nothing

    Dim nloop As Integer = 0

    If b_isIata Then
      sIataIcao = "ac_aport_iata_code"
    Else
      sIataIcao = "ac_aport_icao_code"
    End If

    If Not String.IsNullOrEmpty(sInCodes.Trim) Then '        

      sInCodes = sInCodes.Replace(Constants.cSpaceDelim, Constants.cEmptyString) ' Remove All Spaces

      sHoldCodes = "'" + sInCodes.Replace(",", "','") + "'"

      If b_isIata Then
        sOutCodes = ParseUserInputData(sInCodes, Constants.cEmptyString, Constants.cCommaDelim, True)
      Else
        sOutCodes = ParseUserInputData(sInCodes, Constants.cEmptyString, Constants.cCommaDelim, True)
      End If

      codesArray = sHoldCodes.Split(Constants.cCommaDelim)

      If UBound(codesArray) > 0 Then

        sWhereClause = Constants.cAndClause + Constants.cSingleOpen

        For nloop = 0 To UBound(codesArray)
          ' change wildcards(*) to SQL wildcards (%)
          sArrayItem = codesArray(nloop).Replace(Constants.cWildCard, Constants.cSQLWildCard)

          sWhereClause += sIataIcao + Constants.cLikeClause + sArrayItem

          If UBound(codesArray) >= 1 And nloop < UBound(codesArray) Then
            sWhereClause += Constants.cOrClause   ' add or clauses for each item
          End If

        Next

        sWhereClause += Constants.cSingleClose

      Else
        sWhereClause = Constants.cAndClause + sIataIcao + Constants.cInClause + "(" + sHoldCodes + ")"
      End If

    End If

    Return sWhereClause

  End Function

  Public Shared Function CheckForZipCode(ByVal sInZipCodes As String) As String

    Dim regularZipArray() As String = Nothing
    Dim less5ZipArray() As String = Nothing
    Dim zipCodeArray() As String = Nothing

    Dim nloop As Integer = 0

    Dim sWhereClause As String = ""
    Dim sHoldPlusFourZip As String = ""
    Dim sHoldRegularZip As String = ""
    Dim sHoldLess5Zip As String = ""
    Dim sHold5Zip As String = ""
    Dim sTempCode As String = ""
    Dim sHoldCanadianZip As String = ""
    Dim sHoldZipCode As String = ""
    Dim sArrayItem As String = ""

    If Not String.IsNullOrEmpty(sInZipCodes.Trim) Then '         sHoldSerial = "'" + sHoldSerial.Replace(",", "','") + "'"

      'sHoldZipCode = Replace(FormatUserData(sInZipCodes, gtUSRRANGE, False), cSingleQuote, cEmptyString) ' clean off single quotes

      'session("companyZipCode") = Replace(FormatUserData(CleanUserData(sInZipCodes, cEmptyString, cCommaDelim, True), gtUSRRANGE, False), cSingleQuote, cEmptyString) ' clean off single quotes

      'zipCodeArray = SplitUserData(sHoldZipCode, cCommaDelim) ' split apart data

      'If UBound(zipCodeArray) > 0 Then ' process data

      '  ' first loop through zip code array and seperate out any +4(5) us zip codes
      '  For nloop = 0 To UBound(zipCodeArray)

      '    sArrayItem = zipCodeArray(nloop)

      '    If InStr(1, sArrayItem, cHyphen) > 0 Then ' has Hyphen must be +4(5) zip

      '      If Trim(sHoldPlusFourZip) = "" Then
      '        sHoldPlusFourZip = cSingleQuote & sArrayItem & cSingleQuote
      '      Else
      '        sHoldPlusFourZip = sHoldPlusFourZip & cCommaDelim & cSingleQuote & sArrayItem & cSingleQuote
      '      End If

      '    Else

      '      If Trim(sHoldRegularZip) = "" Then
      '        sHoldRegularZip = sArrayItem
      '      Else
      '        sHoldRegularZip = sHoldRegularZip & cCommaDelim & sArrayItem
      '      End If
      '    End If
      '  Next

      '  ' next loop through Regular zip code array and split out 5char zip - for in clause
      '  ' less than 5 char - for like cause
      '  regularZipArray = SplitUserData(sHoldRegularZip, cCommaDelim)

      '  If IsArray(regularZipArray) And Not isEmpty(regularZipArray) Then
      '    For nloop = 0 To UBound(regularZipArray)

      '      sArrayItem = Replace(Replace(Trim(regularZipArray(nloop)), cSingleQuote, cEmptyString), cWildCard, cEmptyString)

      '      If Len(sArrayItem) = 7 And InStr(1, sArrayItem, cEmptySpace) > 0 Then

      '        If Trim(sHoldCanadianZip) = "" Then
      '          sHoldCanadianZip = cSingleQuote & sArrayItem & cSingleQuote
      '        Else
      '          sHoldCanadianZip = sHoldCanadianZip & cCommaDelim & cSingleQuote & sArrayItem & cSingleQuote
      '        End If

      '      ElseIf Len(sArrayItem) = 5 Then

      '        If Trim(sHold5Zip) = "" Then
      '          sHold5Zip = cSingleQuote & sArrayItem & cSingleQuote
      '        Else
      '          sHold5Zip = sHold5Zip & cCommaDelim & cSingleQuote & sArrayItem & cSingleQuote
      '        End If

      '      Else

      '        If Trim(sHoldLess5Zip) = "" Then
      '          sHoldLess5Zip = sArrayItem
      '        Else
      '          sHoldLess5Zip = sHoldLess5Zip & cCommaDelim & sArrayItem
      '        End If

      '      End If

      '    Next
      '  End If

      '  'add sql Wildcards if none entered
      '  less5ZipArray = SplitUserData(sHoldLess5Zip, cCommaDelim)
      '  sHoldLess5Zip = "" ' clean out temp string

      '  If IsArray(less5ZipArray) And Not isEmpty(less5ZipArray) Then
      '    For nloop = 0 To UBound(less5ZipArray)

      '      If InStr(1, sArrayItem, cSQLWildCard) > 0 Then
      '        sTempCode = FormatQueryString("comp_zip_code", cSingleQuote & less5ZipArray(nloop) & cSingleQuote, , cEmptyString, gtSQLIKE)
      '      Else
      '        sTempCode = FormatQueryString("comp_zip_code", cSingleQuote & less5ZipArray(nloop) & cSQLWildCard & cSingleQuote, , cEmptyString, gtSQLIKE)
      '      End If

      '      If Trim(sHoldLess5Zip) = "" Then
      '        sHoldLess5Zip = sTempCode
      '      Else
      '        sHoldLess5Zip = sHoldLess5Zip & cOrClause & sTempCode
      '      End If

      '    Next
      '  End If

      '  'if session("debug") then
      '  '  response.write("sHoldLess5Zip: " & sHoldLess5Zip & "<br />")
      '  '  response.write("sHold5Zip: " & sHold5Zip & "<br />")
      '  '  response.write("sHoldPlusFourZip: " & sHoldPlusFourZip & "<br />")
      '  '  response.write("sHoldCanadianZip: " & sHoldCanadianZip & "<br />")
      '  'end if

      '  sWhereClause = cAndClause & cSingleOpen

      '  If Trim(sHoldLess5Zip) <> "" Then
      '    sWhereClause = sWhereClause & sHoldLess5Zip
      '  End If

      '  If Trim(sHold5Zip) <> "" Then
      '    If Trim(sHoldLess5Zip) <> "" Then
      '      sWhereClause = sWhereClause & cOrClause & FormatQueryString("SUBSTRING(comp_zip_code,1,5)", sHold5Zip, , cEmptyString, gtSQLIN)
      '    Else
      '      sWhereClause = sWhereClause & FormatQueryString("SUBSTRING(comp_zip_code,1,5)", sHold5Zip, , cEmptyString, gtSQLIN)
      '    End If
      '  End If

      '  If Trim(sHoldPlusFourZip) <> "" Then
      '    If Trim(sHoldLess5Zip) <> "" Or Trim(sHold5Zip) <> "" Then
      '      sWhereClause = sWhereClause & cOrClause & FormatQueryString("comp_zip_code", sHoldPlusFourZip, , cEmptyString, gtSQLIN)
      '    Else
      '      sWhereClause = sWhereClause & FormatQueryString("comp_zip_code", sHoldPlusFourZip, , cEmptyString, gtSQLIN)
      '    End If
      '  End If

      '  If Trim(sHoldCanadianZip) <> "" Then
      '    If Trim(sHoldLess5Zip) <> "" Or Trim(sHold5Zip) <> "" Or Trim(sHoldPlusFourZip) <> "" Then
      '      sWhereClause = sWhereClause & cOrClause & FormatQueryString("SUBSTRING(comp_zip_code,1,7)", sHoldCanadianZip, , cEmptyString, gtSQLIN)
      '    Else
      '      sWhereClause = sWhereClause & FormatQueryString("SUBSTRING(comp_zip_code,1,7)", sHoldCanadianZip, , cEmptyString, gtSQLIN)
      '    End If
      '  End If

      '  sWhereClause = sWhereClause & cSingleClose

      'Else ' single zipcode 

      '  If InStr(1, zipCodeArray(0), cHyphen) > 0 Then ' has Hyphen must be +4(5) zip
      '    sWhereClause = cAndClause & FormatQueryString("comp_zip_code", cSingleQuote & zipCodeArray(0) & cSingleQuote, , cEmptyString, gtSQLIN)
      '  Else

      '    Dim bHadWildCard
      '    bHadWildCard = False

      '    If InStr(1, sArrayItem, cWildCard) > 0 Then ' preserve wildcard if needed
      '      bHadWildCard = True
      '    End If

      '    ' strip off any single quotes or wildcards that get through
      '    sArrayItem = Replace(Replace(Trim(zipCodeArray(0)), cSingleQuote, cEmptyString), cWildCard, cEmptyString)

      '    If Len(sArrayItem) = 7 And InStr(1, sArrayItem, cEmptySpace) > 0 Then
      '      sWhereClause = cAndClause & FormatQueryString("SUBSTRING(comp_zip_code,1,7)", cSingleQuote & sArrayItem & cSingleQuote, , cEmptyString, gtSQLIN)
      '    ElseIf Len(sArrayItem) = 5 Then
      '      sWhereClause = cAndClause & FormatQueryString("SUBSTRING(comp_zip_code,1,5)", cSingleQuote & sArrayItem & cSingleQuote, , cEmptyString, gtSQLIN)
      '    Else
      '      If bHadWildCard Then
      '        sWhereClause = cAndClause & FormatQueryString("comp_zip_code", cSingleQuote & Replace(sArrayItem, cWildCard, cSQLWildCard) & cSingleQuote, , cEmptyString, gtSQLIKE)
      '      Else
      '        sWhereClause = cAndClause & FormatQueryString("comp_zip_code", cSingleQuote & sArrayItem & cSQLWildCard & cSingleQuote, , cEmptyString, gtSQLIKE)
      '      End If
      '    End If
      '  End If

      'End If

    End If

    Return sWhereClause

  End Function

#End Region

#Region "document_functions"

  Public Shared Function Get_Document_File_Name(ByVal nAircraftID As Long,
                                              ByVal nAircraftJournalID As Long,
                                              ByVal nAircraftJournSeqNo As Integer,
                                              ByVal sDocType As String,
                                              ByVal sDocExtension As String) As String

    Dim sDestinationFileName As String = ""
    Dim sDirName As String = ""
    Dim sDestinationPath As String = ""
    Dim i As Integer = 0

    ' IDENTIFY THE SUBDIRECTORY WHERE THE DOCUMENT IS BASE ON THE ACID

    Select Case Len(nAircraftID.ToString.Trim)
      Case 1, 2, 3  ' THE AIRCRAFT ID MUST BE LESS THAN 1000 SO JUST SET THE DIRECTORY
        sDirName = "0-999"
      Case 4 ' AIRCRAFT ID MUST BE IN THE THOUSANDS
        sDirName = Left(nAircraftID.ToString.Trim, 1)
        For i = 1 To Len(nAircraftID.ToString.Trim) - 1
          sDirName = sDirName + "0"
        Next
        sDirName = sDirName + "-" + Left(nAircraftID.ToString, 1) + "999"
      Case 5 ' AIRCRAFT ID MUST BE IN THE TENS OF THOUSANDS
        sDirName = Left(nAircraftID.ToString.Trim, 2)
        For i = 1 To Len(nAircraftID.ToString.Trim) - 2
          sDirName = sDirName + "0"
        Next
        sDirName = sDirName + "-" + Left(nAircraftID.ToString, 2) + "999"
      Case 6  ' AIRCRAFT ID MUST BE IN THE HUNDREDS OF THOUSANDS
        sDirName = Left(nAircraftID.ToString.Trim, 3)
        For i = 1 To Len(nAircraftID.ToString.Trim) - 3
          sDirName = sDirName + "0"
        Next
        sDirName = sDirName + "-" + Left(nAircraftID.ToString.Trim, 3) + "999"
      Case 7  ' AIRCRAFT ID MUST BE IN THE MILLIONS
        sDirName = Left(nAircraftID.ToString.Trim, 4)
        For i = 1 To Len(nAircraftID.ToString.Trim) - 4
          sDirName = sDirName + "0"
        Next
        sDirName = sDirName + Constants.cHyphen + Left(nAircraftID.ToString.Trim, 4) + "999"
      Case Else ' RETURN A DIRECTORY NAME OF "0" IF THE NUMBER IS BIGGER THAN 7
        sDirName = "0"
    End Select

    If Not String.IsNullOrEmpty(sDocType) Then

      ' ASSIGN THE FILE NAME BASED ON AC ID AND JOURN ID AND Extension
      ' IF A SEQUENCE NUMBER IS PASSED THEN ADD THIS TO THE FILE NAME
      ' AS WELL

      If CLng(nAircraftJournSeqNo) > 0 Then
        sDestinationFileName = nAircraftID.ToString + Constants.cHyphen + nAircraftJournalID.ToString + Constants.cHyphen + nAircraftJournSeqNo.ToString + sDocExtension
      Else
        sDestinationFileName = nAircraftID.ToString + Constants.cHyphen + nAircraftJournalID.ToString + sDocExtension
      End If

      ' ASSIGN THE DIRECTORY TO BE STORED BASED ON THE FILE TYPE
      ' THE ASSIGN DOCUMENT DIRECTORY FUNCTION IS PASSED AN AIRCRAFT ID
      ' AND RETURNS A SUBDIRECTORY GROUPED INTO THOUSANDS WHERE THE DOCUMENT WILL BE STORED

      If HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNET12") Then
        ' depending on the nDocType sDestinationPath will change
        Select Case (sDocType.ToUpper)

          Case "FAAPDF"
            sDestinationPath = HttpContext.Current.Session.Item("FAAPDFFolderVirtualPath") + Constants.cSingleForwardSlash + sDirName
          Case "NTSB"
            sDestinationPath = HttpContext.Current.Session.Item("NTSBFolderVirtualPath") + Constants.cSingleForwardSlash + sDirName
          Case "337"
            sDestinationPath = HttpContext.Current.Session.Item("337FolderVirtualPath") + Constants.cSingleForwardSlash + sDirName

        End Select
      Else
        sDestinationPath = HttpContext.Current.Session.Item("DocumentFolderVirtualPath") + Constants.cSingleForwardSlash + sDocType.ToUpper + Constants.cSingleForwardSlash + sDirName
      End If

    End If

    ' CREATE THE FULL FILE NAME
    Return Trim(sDestinationPath + "/" + sDestinationFileName)

  End Function

  Public Shared Function GetDocumentDisplayPath(ByVal nAircraftID As Long,
                                                ByVal nAircraftJournalID As Long,
                                                ByVal nSequenceNo As Integer,
                                                ByRef outDocTitle As String) As String

    ' DISPLAY AIRCRAFT DOCUMENTS ASSOCIATED WITH THE JOURNAL ENTRY
    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing

    Dim sQuery As String = ""

    outDocTitle = ""

    Dim hDocumentFilePath As String = ""

    Dim fAdoc_journ_seq_no As Integer = 0
    Dim fAdoc_doc_type As String = ""
    Dim fAdoc_journ_id As Long = 0
    Dim fAdoc_hide_flag As String = ""
    Dim fAdoc_doc_date As String = ""
    Dim fDoctype_subdir_name As String = ""
    Dim fDoctype_file_extension As String = ""
    Dim fAdoctype_description As String = ""

    sQuery = "SELECT adoc_doc_date, adoc_journ_seq_no, adoc_doc_type, adoc_journ_id, adoc_hide_flag, doctype_subdir_name, doctype_file_extension, doctype_description"
    sQuery &= " FROM Aircraft_Document WITH(NOLOCK) INNER JOIN Document_Type WITH(NOLOCK) ON (doctype_description = adoc_doc_type)"
    sQuery &= " WHERE adoc_ac_id = " + nAircraftID.ToString + " AND adoc_journ_id = " + nAircraftJournalID.ToString

    If nSequenceNo > 0 Then
      sQuery &= " AND adoc_journ_seq_no = " + nSequenceNo.ToString
    End If

    sQuery &= " ORDER BY adoc_journ_seq_no"

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not IsDBNull(lDataReader("adoc_doc_date")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_doc_date").ToString) Then
          fAdoc_doc_date = lDataReader.Item("adoc_doc_date").ToString.Trim
        Else
          fAdoc_doc_date = ""
        End If

        If Not IsDBNull(lDataReader("adoc_journ_seq_no")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_journ_seq_no").ToString) Then
          fAdoc_journ_seq_no = CInt(lDataReader.Item("adoc_journ_seq_no").ToString.Trim)
        Else
          fAdoc_journ_seq_no = 0
        End If

        If Not IsDBNull(lDataReader("adoc_doc_type")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_doc_type").ToString) Then
          fAdoc_doc_type = lDataReader.Item("adoc_doc_type").ToString.Trim
        Else
          fAdoc_doc_type = ""
        End If

        If Not IsDBNull(lDataReader("adoc_journ_id")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_journ_id").ToString) Then
          fAdoc_journ_id = CLng(lDataReader.Item("adoc_journ_id").ToString.Trim)
        Else
          fAdoc_journ_id = 0
        End If

        If Not IsDBNull(lDataReader("adoc_hide_flag")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_hide_flag").ToString) Then
          fAdoc_hide_flag = lDataReader.Item("adoc_hide_flag").ToString.ToUpper.Trim
        Else
          fAdoc_hide_flag = ""
        End If

        If Not IsDBNull(lDataReader("doctype_subdir_name")) And Not String.IsNullOrEmpty(lDataReader.Item("doctype_subdir_name").ToString) Then
          fDoctype_subdir_name = lDataReader.Item("doctype_subdir_name").ToString.Trim
        Else
          fDoctype_subdir_name = ""
        End If

        If Not IsDBNull(lDataReader("doctype_file_extension")) And Not String.IsNullOrEmpty(lDataReader.Item("doctype_file_extension").ToString) Then
          fDoctype_file_extension = lDataReader.Item("doctype_file_extension").ToString.Trim
        Else
          fDoctype_file_extension = ""
        End If

        If Not IsDBNull(lDataReader("doctype_description")) And Not String.IsNullOrEmpty(lDataReader.Item("doctype_description").ToString) Then
          fAdoctype_description = lDataReader.Item("doctype_description").ToString.Trim
        Else
          fAdoctype_description = "Unknown"
        End If

        If fAdoc_hide_flag <> "Y" Then

          ' GET THE FILE NAME FOR THE ELECTRONIC DOCUMENT
          hDocumentFilePath = Get_Document_File_Name(nAircraftID, fAdoc_journ_id, fAdoc_journ_seq_no, fDoctype_subdir_name, fDoctype_file_extension)

        End If ' fAdoc_hide_flag <> "Y" 

        outDocTitle = fAdoctype_description

        lDataReader.Close()

      End If

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return ""

    Finally

      SqlCommand.Dispose()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return hDocumentFilePath

  End Function

  Public Shared Function displayTransactionDocuments(ByVal nAircraftID As Long,
                                                     ByVal nAircraftJournalID As Long,
                                                     ByVal nSequenceNo As Integer,
                                                     ByVal isDisplay As Boolean,
                                                     ByVal isDetails As Boolean,
                                                     ByVal isJFWAFW As Boolean,
                                                     ByVal isView As Boolean,
                                                     ByRef out_html As String) As Boolean

    ' DISPLAY THE LIST OF AIRCRAFT DOCUMENTS ASSOCIATED WITH THE JOURNAL ENTRY
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""
    Dim sHtmlOut As StringBuilder = New StringBuilder()

    Dim hDocumentFile As String = ""

    Dim fAdoc_general_note As String = ""
    Dim fAdoc_journ_seq_no As Integer = 0
    Dim fAdoc_doc_type As String = ""
    Dim fAdoc_journ_id As Long = 0
    Dim fAdoc_hide_flag As String = ""
    Dim fAdoc_doc_date As String = ""
    Dim fDoctype_subdir_name As String = ""
    Dim fDoctype_file_extension As String = ""
    Dim fAdoc_onbehalf_comp_id As Long = 0
    Dim fAdoc_onbehalf_text As String = ""
    Dim fAdoc_infavor_comp_id As Long = 0
    Dim fAdoc_infavor_text As String = ""

    sQuery = "SELECT adoc_doc_date, adoc_onbehalf_comp_id, adoc_onbehalf_text, adoc_infavor_comp_id, adoc_infavor_text,"
    sQuery &= " adoc_general_note, adoc_journ_seq_no, adoc_doc_type, adoc_journ_id, adoc_hide_flag, doctype_subdir_name, doctype_file_extension"
    sQuery &= " FROM Aircraft_Document WITH(NOLOCK) INNER JOIN Document_Type WITH(NOLOCK) ON (doctype_description = adoc_doc_type)"
    sQuery &= " WHERE adoc_ac_id = " + nAircraftID.ToString + " AND adoc_journ_id = " + nAircraftJournalID.ToString

    If nSequenceNo > 0 Then
      sQuery &= " AND adoc_journ_seq_no = " + nSequenceNo.ToString
    End If

    sQuery &= " ORDER BY adoc_journ_seq_no"

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.Trim

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        If Not isView Then

          Do While lDataReader.Read()

            If Not IsDBNull(lDataReader.Item("adoc_general_note")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_general_note").ToString) Then
              fAdoc_general_note = lDataReader.Item("adoc_general_note").ToString.Trim
            Else
              fAdoc_general_note = ""
            End If

            If Not IsDBNull(lDataReader.Item("adoc_doc_date")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_doc_date").ToString) Then
              fAdoc_doc_date = FormatDateTime(lDataReader.Item("adoc_doc_date").ToString.Trim, DateFormat.GeneralDate)
            Else
              fAdoc_doc_date = ""
            End If

            If Not IsDBNull(lDataReader.Item("adoc_onbehalf_comp_id")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_onbehalf_comp_id").ToString) Then
              fAdoc_onbehalf_comp_id = CLng(lDataReader.Item("adoc_onbehalf_comp_id").ToString.Trim)
            Else
              fAdoc_onbehalf_comp_id = 0
            End If

            If Not IsDBNull(lDataReader.Item("adoc_onbehalf_text")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_onbehalf_text").ToString) Then
              fAdoc_onbehalf_text = lDataReader.Item("adoc_onbehalf_text").ToString.Trim
            Else
              fAdoc_onbehalf_text = ""
            End If

            If Not IsDBNull(lDataReader.Item("adoc_infavor_comp_id")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_infavor_comp_id").ToString) Then
              fAdoc_infavor_comp_id = CLng(lDataReader.Item("adoc_infavor_comp_id").ToString.Trim)
            Else
              fAdoc_infavor_comp_id = 0
            End If

            If Not IsDBNull(lDataReader.Item("adoc_infavor_text")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_infavor_text").ToString) Then
              fAdoc_infavor_text = lDataReader.Item("adoc_infavor_text").ToString.Trim
            Else
              fAdoc_infavor_text = ""
            End If

            If Not IsDBNull(lDataReader.Item("adoc_journ_seq_no")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_journ_seq_no").ToString) Then
              fAdoc_journ_seq_no = CInt(lDataReader.Item("adoc_journ_seq_no").ToString.Trim)
            Else
              fAdoc_journ_seq_no = 0
            End If

            If Not IsDBNull(lDataReader.Item("adoc_doc_type")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_doc_type").ToString) Then
              fAdoc_doc_type = lDataReader.Item("adoc_doc_type").ToString.Trim
            Else
              fAdoc_doc_type = ""
            End If

            If Not IsDBNull(lDataReader.Item("adoc_journ_id")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_journ_id").ToString) Then
              fAdoc_journ_id = CLng(lDataReader.Item("adoc_journ_id").ToString.Trim)
            Else
              fAdoc_journ_id = 0
            End If

            If Not IsDBNull(lDataReader.Item("adoc_hide_flag")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_hide_flag").ToString) Then
              fAdoc_hide_flag = lDataReader.Item("adoc_hide_flag").ToString.Trim
            Else
              fAdoc_hide_flag = ""
            End If

            If Not IsDBNull(lDataReader.Item("doctype_subdir_name")) And Not String.IsNullOrEmpty(lDataReader.Item("doctype_subdir_name").ToString) Then
              fDoctype_subdir_name = lDataReader.Item("doctype_subdir_name").ToString.Trim
            Else
              fDoctype_subdir_name = ""
            End If

            If Not IsDBNull(lDataReader.Item("doctype_file_extension")) And Not String.IsNullOrEmpty(lDataReader.Item("doctype_file_extension").ToString) Then
              fDoctype_file_extension = lDataReader.Item("doctype_file_extension").ToString.Trim
            Else
              fDoctype_file_extension = ""
            End If

            If fAdoc_hide_flag.ToUpper <> "Y" Then

              ' GET THE FILE NAME FOR THE ELECTRONIC DOCUMENT
              hDocumentFile = commonEvo.Get_Document_File_Name(nAircraftID, fAdoc_journ_id, fAdoc_journ_seq_no, fDoctype_subdir_name, fDoctype_file_extension)

              If Not String.IsNullOrEmpty(hDocumentFile) Then

                If System.IO.File.Exists(HttpContext.Current.Server.MapPath(hDocumentFile)) Then
                  If isDisplay Then
                    sHtmlOut.Append("<a class='underline' onclick=""javascript:load('DocumentDetails.asp?inACID=" + nAircraftID.ToString + "&inJournID=" + nAircraftJournalID.ToString + "&inSeqNo=" + fAdoc_journ_seq_no.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Document Details'>" + fAdoc_doc_type.ToString + "</a>")
                    sHtmlOut.Append("&nbsp;&nbsp;<a class='underline' onclick=""javascript:load('DocumentDetails.asp?inACID=" + nAircraftID.ToString + "&inJournID=" + nAircraftJournalID.ToString + "&inSeqNo=" + fAdoc_journ_seq_no.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Document Details'><img align='absmiddle' src='images/DocumentSM.gif' border='0' title='" + fAdoc_doc_type + "' alt='" + fAdoc_doc_type + "' /></a><br />")
                  ElseIf isDetails Then
                    sHtmlOut.Append(fAdoc_doc_type + "&nbsp;&nbsp;")
                    sHtmlOut.Append("<a href='" + hDocumentFile + "' target='_new'><img align='absmiddle' src='images/DocumentSM.gif' border='0' title='" + fAdoc_doc_type + "' alt='" + fAdoc_doc_type + "' /></a><br />")
                  Else
                    If Not isJFWAFW Then
                      sHtmlOut.Append(fAdoc_doc_type + "&nbsp;&nbsp;")
                      sHtmlOut.Append("<a href='" + hDocumentFile + "' target='_new'><img align='absmiddle' src='images/DocumentSM.gif' border='0' title='" + fAdoc_doc_type + "' alt='" + fAdoc_doc_type + "' /></a><br />")
                    Else
                      sHtmlOut.Append(fAdoc_doc_type + "&nbsp;&nbsp;")
                      sHtmlOut.Append("<img align='absmiddle' src='images/DocumentSM_Display.GIF' border='0' title='" + fAdoc_doc_type + "' alt='" + fAdoc_doc_type + "' /><br />")
                    End If
                  End If
                Else
                  sHtmlOut.Append(fAdoc_doc_type + "&nbsp;&nbsp;&lt;Not&nbsp;On&nbsp;File&gt;<br />") ' + HttpContext.Current.Server.MapPath(hDocumentFile))
                End If ' Document_Exists(hDocumentFile) 

                If nAircraftJournalID > 0 Then
                  If Not String.IsNullOrEmpty(fAdoc_general_note) Then
                    sHtmlOut.Append("&nbsp;Note&nbsp;:&nbsp;" + fAdoc_general_note.Trim + "<br />")
                  End If
                End If

                ' we have an infavor company id or infavor text is not blank
                If fAdoc_infavor_comp_id > 0 Or Not String.IsNullOrEmpty(fAdoc_infavor_text) Then

                  If Not String.IsNullOrEmpty(fAdoc_doc_date) Then

                    sHtmlOut.Append("&nbsp;(Filed on " + fAdoc_doc_date)

                    If fAdoc_onbehalf_comp_id > 0 Then

                      sHtmlOut.Append(" on behalf of ")
                      sHtmlOut.Append(get_company_name_fromID(fAdoc_onbehalf_comp_id, nAircraftJournalID, False, True, ""))
                      sHtmlOut.Append(")<br />")

                    ElseIf fAdoc_onbehalf_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_onbehalf_text) Then

                      sHtmlOut.Append(" on behalf of " + fAdoc_onbehalf_text)
                      sHtmlOut.Append(")<br />")

                    ElseIf Not String.IsNullOrEmpty(fAdoc_onbehalf_text) Then

                      sHtmlOut.Append(" on behalf of " + fAdoc_onbehalf_text)
                      sHtmlOut.Append(")<br />")

                    End If
                  End If
                End If

              Else
                sHtmlOut.Append(fAdoc_doc_type + "&nbsp;:&nbsp;&lt;Not&nbsp;On&nbsp;File&gt;<br />")
              End If ' hDocumentFile <> "" 

            Else
              sHtmlOut.Append("&nbsp;")
            End If ' fAdoc_hide_flag <> "Y" 

          Loop ' while lDataReader.HasRows 

        Else

          lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("adoc_general_note")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_general_note").ToString) Then
            fAdoc_general_note = lDataReader.Item("adoc_general_note").ToString.Trim
          Else
            fAdoc_general_note = ""
          End If

          If Not IsDBNull(lDataReader.Item("adoc_doc_date")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_doc_date").ToString) Then
            fAdoc_doc_date = lDataReader.Item("adoc_doc_date").ToString.Trim
          Else
            fAdoc_doc_date = ""
          End If

          If Not IsDBNull(lDataReader.Item("adoc_onbehalf_comp_id")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_onbehalf_comp_id").ToString) Then
            fAdoc_onbehalf_comp_id = CLng(lDataReader.Item("adoc_onbehalf_comp_id").ToString.Trim)
          Else
            fAdoc_onbehalf_comp_id = 0
          End If

          If Not IsDBNull(lDataReader.Item("adoc_onbehalf_text")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_onbehalf_text").ToString) Then
            fAdoc_onbehalf_text = lDataReader.Item("adoc_onbehalf_text").ToString.Trim
          Else
            fAdoc_onbehalf_text = ""
          End If

          If Not IsDBNull(lDataReader.Item("adoc_infavor_comp_id")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_infavor_comp_id").ToString) Then
            fAdoc_infavor_comp_id = CLng(lDataReader.Item("adoc_infavor_comp_id").ToString.Trim)
          Else
            fAdoc_infavor_comp_id = 0
          End If

          If Not IsDBNull(lDataReader.Item("adoc_infavor_text")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_infavor_text").ToString) Then
            fAdoc_infavor_text = lDataReader.Item("adoc_infavor_text").ToString.Trim
          Else
            fAdoc_infavor_text = ""
          End If

          If Not IsDBNull(lDataReader.Item("adoc_journ_seq_no")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_journ_seq_no").ToString) Then
            fAdoc_journ_seq_no = CInt(lDataReader.Item("adoc_journ_seq_no").ToString.Trim)
          Else
            fAdoc_journ_seq_no = 0
          End If

          If Not IsDBNull(lDataReader.Item("adoc_doc_type")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_doc_type").ToString) Then
            fAdoc_doc_type = lDataReader.Item("adoc_doc_type").ToString.Trim
          Else
            fAdoc_doc_type = ""
          End If

          If Not IsDBNull(lDataReader.Item("adoc_journ_id")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_journ_id").ToString) Then
            fAdoc_journ_id = CLng(lDataReader.Item("adoc_journ_id").ToString.Trim)
          Else
            fAdoc_journ_id = 0
          End If

          If Not IsDBNull(lDataReader.Item("adoc_hide_flag")) And Not String.IsNullOrEmpty(lDataReader.Item("adoc_hide_flag").ToString) Then
            fAdoc_hide_flag = lDataReader.Item("adoc_hide_flag").ToString.Trim
          Else
            fAdoc_hide_flag = ""
          End If

          If Not IsDBNull(lDataReader.Item("doctype_subdir_name")) And Not String.IsNullOrEmpty(lDataReader.Item("doctype_subdir_name").ToString) Then
            fDoctype_subdir_name = lDataReader.Item("doctype_subdir_name").ToString.Trim
          Else
            fDoctype_subdir_name = ""
          End If

          If Not IsDBNull(lDataReader.Item("doctype_file_extension")) And Not String.IsNullOrEmpty(lDataReader.Item("doctype_file_extension").ToString) Then
            fDoctype_file_extension = lDataReader.Item("doctype_file_extension").ToString.Trim
          Else
            fDoctype_file_extension = ""
          End If

          If fAdoc_hide_flag.ToUpper <> "Y" Then

            ' GET THE FILE NAME FOR THE ELECTRONIC DOCUMENT
            hDocumentFile = commonEvo.Get_Document_File_Name(nAircraftID, fAdoc_journ_id, fAdoc_journ_seq_no, fDoctype_subdir_name, fDoctype_file_extension)

            If Not String.IsNullOrEmpty(hDocumentFile) Then

              If System.IO.File.Exists(HttpContext.Current.Server.MapPath(hDocumentFile)) Then
                sHtmlOut.Append("<a href='" + hDocumentFile + "' target='_new'><img align='absmiddle' src='images/DocumentSM.gif' border='0' title='" + fAdoc_doc_type + "' alt='" + fAdoc_doc_type + "' /></a>")
              Else
                sHtmlOut.Append("<img align='absmiddle' src='images/DocumentSM_Display.GIF' border='0' title='" + fAdoc_doc_type + "' alt='" + fAdoc_doc_type + "' />")
              End If ' Document_Exists(hDocumentFile) 

            Else
              sHtmlOut.Append("<img align='absmiddle' src='images/DocumentSM_Display.GIF' border='0' title='" + fAdoc_doc_type + "' alt='" + fAdoc_doc_type + "' />")
            End If ' hDocumentFile <> "" 		  

          Else
            sHtmlOut.Append("&nbsp;")
          End If ' fAdoc_hide_flag <> "Y" 

        End If ' isView

        lDataReader.Close()

      End If

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return False

    Finally

      SqlConnection.Close()
      SqlCommand.Dispose()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    lDataReader = Nothing
    SqlCommand = Nothing

    out_html = sHtmlOut.ToString

    Return True

    sHtmlOut = Nothing

  End Function

#End Region

#Region "client_side_array_functions"

  Public Shared Sub fillAirframeArray(ByRef out_htmlString As String)

    Dim arrTypeMakeModel(,) As String = Nothing
    Dim arrTypeMakeModelAmodIndex(,) As String = Nothing

    Dim results_table As New DataTable

    Dim sAircraft_type As String = ""
    Dim sAircraft_frameType As String = ""
    Dim sAircraft_make As String = ""
    Dim sAircraft_make_abbrev As String = ""
    Dim sAircraft_model As String = ""
    Dim sAircraft_modelID As Long = 0
    Dim sAircraft_usage As String = ""
    Dim sAircraft_mfrName As String = ""
    Dim sAircraft_jniqSize As String = ""

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("AirframeArray")) Then

        results_table = get_make_model_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrTypeMakeModel(results_table.Rows.Count - 1, Constants.serverAIRFRAMEARRAY_DIM)
            ReDim arrTypeMakeModelAmodIndex(results_table.Rows.Count - 1, 1)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("amod_type_code"))) Then
                sAircraft_type = r.Item("amod_type_code").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_airframe_type_code"))) Then
                sAircraft_frameType = r.Item("amod_airframe_type_code").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_make_name"))) Then
                sAircraft_make = r.Item("amod_make_name").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_make_abbrev"))) Then
                sAircraft_make_abbrev = r.Item("amod_make_abbrev").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_model_name"))) Then
                sAircraft_model = r.Item("amod_model_name").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_id"))) Then
                sAircraft_modelID = CLng(r.Item("amod_id").ToString)
              End If

              ' look for models based on user product code

              If Not (IsDBNull(r("amod_product_business_flag"))) Then
                If r.Item("amod_product_business_flag").ToString.Trim.ToUpper = "Y" Then
                  If String.IsNullOrEmpty(sAircraft_usage) Then
                    sAircraft_usage = Constants.PRODUCT_TYPE_B
                  Else
                    sAircraft_usage = sAircraft_usage + "," + Constants.PRODUCT_TYPE_B
                  End If
                End If
              End If

              If Not (IsDBNull(r("amod_product_commercial_flag"))) Then
                If r.Item("amod_product_commercial_flag").ToString.Trim.ToUpper = "Y" Then
                  If String.IsNullOrEmpty(sAircraft_usage) Then
                    sAircraft_usage = Constants.PRODUCT_TYPE_C
                  Else
                    sAircraft_usage = sAircraft_usage + "," + Constants.PRODUCT_TYPE_C
                  End If
                End If
              End If

              If Not (IsDBNull(r("amod_product_helicopter_flag"))) Then
                If r.Item("amod_product_helicopter_flag").ToString.Trim.ToUpper = "Y" Then
                  If String.IsNullOrEmpty(sAircraft_usage) Then
                    sAircraft_usage = Constants.PRODUCT_TYPE_H
                  Else
                    sAircraft_usage = sAircraft_usage + "," + Constants.PRODUCT_TYPE_H
                  End If
                End If
              End If

              If Not (IsDBNull(r("amod_product_airbp_flag"))) Then
                If r.Item("amod_product_airbp_flag").ToString.Trim.ToUpper = "Y" Then
                  If String.IsNullOrEmpty(sAircraft_usage) Then
                    sAircraft_usage = Constants.PRODUCT_TYPE_P
                  Else
                    sAircraft_usage = sAircraft_usage + "," + Constants.PRODUCT_TYPE_P
                  End If
                End If
              End If

              If Not (IsDBNull(r("amod_manufacturer_common_name"))) Then
                sAircraft_mfrName = r.Item("amod_manufacturer_common_name").ToString.ToUpper.Replace(", INC.", "").Trim
              End If

              If Not (IsDBNull(r("amod_jniq_size"))) Then
                sAircraft_jniqSize = r.Item("amod_jniq_size").ToString.Trim
              End If

              arrTypeMakeModel(nCounter, 0) = nCounter
              arrTypeMakeModel(nCounter, 1) = sAircraft_type
              arrTypeMakeModel(nCounter, 2) = sAircraft_make
              arrTypeMakeModel(nCounter, 3) = sAircraft_make_abbrev
              arrTypeMakeModel(nCounter, 4) = sAircraft_model
              arrTypeMakeModel(nCounter, 5) = sAircraft_modelID
              arrTypeMakeModel(nCounter, 6) = sAircraft_usage
              arrTypeMakeModel(nCounter, 7) = sAircraft_frameType
              arrTypeMakeModel(nCounter, 8) = sAircraft_mfrName
              arrTypeMakeModel(nCounter, 9) = sAircraft_jniqSize


              arrTypeMakeModelAmodIndex(nCounter, 0) = sAircraft_modelID
              arrTypeMakeModelAmodIndex(nCounter, 1) = nCounter

              nCounter += 1
              sAircraft_usage = ""

            Next

            If IsArray(arrTypeMakeModel) And Not IsNothing(arrTypeMakeModel) And UBound(arrTypeMakeModel) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrTypeMakeModel, UBound(arrTypeMakeModel), Constants.serverAIRFRAMEARRAY_DIM)
              HttpContext.Current.Session.Item("AirframeArray") = arrTypeMakeModel
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("AirframeArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("AirframeArray"), Array), UBound(HttpContext.Current.Session.Item("AirframeArray")), Constants.serverAIRFRAMEARRAY_DIM)
        End If

      End If

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("AirframeAmodArray")) Then
        If IsArray(arrTypeMakeModelAmodIndex) And Not IsNothing(arrTypeMakeModelAmodIndex) Then
          HttpContext.Current.Session.Item("AirframeAmodArray") = arrTypeMakeModelAmodIndex
        End If
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillAirframeArray(ByRef out_htmlString As String) " + ex.Message
    Finally

    End Try

    arrTypeMakeModel = Nothing
    arrTypeMakeModelAmodIndex = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillAircraftTypeLableArray(ByRef out_htmlString As String)
    Dim arrAircraftMakeTypeLabel(,) As String = Nothing

    Dim results_table As New DataTable

    Dim sAFMT_airframetype As String = ""
    Dim sAFMT_airframemaketype As String = ""
    Dim sAFMT_code As String = ""
    Dim sAFMT_description As String = ""

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("AircraftTypeLableArray")) Then

        results_table = get_aircraft_type_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrAircraftMakeTypeLabel(results_table.Rows.Count - 1, Constants.serverAIRMAKELABLEARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("afmt_airframetype"))) Then
                sAFMT_airframetype = Trim(r.Item("afmt_airframetype").ToString)
              End If

              If Not (IsDBNull(r("afmt_airframemaketype"))) Then
                sAFMT_airframemaketype = Trim(r.Item("afmt_airframemaketype").ToString)
              End If

              If Not (IsDBNull(r("afmt_code"))) Then
                sAFMT_code = Trim(r.Item("afmt_code").ToString)
              End If

              If Not (IsDBNull(r("afmt_description"))) Then
                sAFMT_description = Trim(r.Item("afmt_description").ToString)
              End If

              arrAircraftMakeTypeLabel(nCounter, 0) = nCounter
              arrAircraftMakeTypeLabel(nCounter, 1) = sAFMT_airframetype
              arrAircraftMakeTypeLabel(nCounter, 2) = sAFMT_airframemaketype
              arrAircraftMakeTypeLabel(nCounter, 3) = sAFMT_code
              arrAircraftMakeTypeLabel(nCounter, 4) = sAFMT_description

              nCounter += 1

            Next

            If IsArray(arrAircraftMakeTypeLabel) And Not IsNothing(arrAircraftMakeTypeLabel) Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrAircraftMakeTypeLabel, UBound(arrAircraftMakeTypeLabel), Constants.serverAIRMAKELABLEARRAY_DIM)
              HttpContext.Current.Session.Item("AircraftTypeLableArray") = arrAircraftMakeTypeLabel
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("AircraftTypeLableArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("AircraftTypeLableArray"), Array), UBound(HttpContext.Current.Session.Item("AircraftTypeLableArray")), Constants.serverAIRMAKELABLEARRAY_DIM)
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillAircraftTypeLableArray(ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    arrAircraftMakeTypeLabel = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillDefaultAirframeArray(ByRef out_htmlString As String)

    Dim arrTypeMakeModel(,) As String = Nothing

    Dim results_table As New DataTable

    Dim sAircraft_type As String = ""
    Dim sAircraft_frameType As String = ""
    Dim sAircraft_make As String = ""
    Dim sAircraft_make_abbrev As String = ""
    Dim sAircraft_model As String = ""
    Dim sAircraft_modelID As Long = 0
    Dim sAircraft_usage As String = ""
    Dim sAircraft_mfrName As String = ""
    Dim sAircraft_jniqSize As String = ""

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("DefaultAirframeArray")) Then

        results_table = get_default_make_model_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrTypeMakeModel(results_table.Rows.Count - 1, Constants.serverAIRFRAMEARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("amod_type_code"))) Then
                sAircraft_type = r.Item("amod_type_code").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_airframe_type_code"))) Then
                sAircraft_frameType = r.Item("amod_airframe_type_code").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_make_name"))) Then
                sAircraft_make = r.Item("amod_make_name").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_make_abbrev"))) Then
                sAircraft_make_abbrev = r.Item("amod_make_abbrev").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_model_name"))) Then
                sAircraft_model = r.Item("amod_model_name").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_id"))) Then
                sAircraft_modelID = CLng(r.Item("amod_id").ToString)
              End If

              ' look for models based on user product code

              If Not (IsDBNull(r("amod_product_business_flag"))) Then
                If r.Item("amod_product_business_flag").ToString.Trim.ToUpper = "Y" Then
                  If String.IsNullOrEmpty(sAircraft_usage) Then
                    sAircraft_usage = Constants.PRODUCT_TYPE_B
                  Else
                    sAircraft_usage = sAircraft_usage + "," + Constants.PRODUCT_TYPE_B
                  End If
                End If
              End If

              If Not (IsDBNull(r("amod_product_commercial_flag"))) Then
                If r.Item("amod_product_commercial_flag").ToString.Trim.ToUpper = "Y" Then
                  If String.IsNullOrEmpty(sAircraft_usage) Then
                    sAircraft_usage = Constants.PRODUCT_TYPE_C
                  Else
                    sAircraft_usage = sAircraft_usage + "," + Constants.PRODUCT_TYPE_C
                  End If
                End If
              End If

              If Not (IsDBNull(r("amod_product_helicopter_flag"))) Then
                If r.Item("amod_product_helicopter_flag").ToString.Trim.ToUpper = "Y" Then
                  If String.IsNullOrEmpty(sAircraft_usage) Then
                    sAircraft_usage = Constants.PRODUCT_TYPE_H
                  Else
                    sAircraft_usage = sAircraft_usage + "," + Constants.PRODUCT_TYPE_H
                  End If
                End If
              End If

              If Not (IsDBNull(r("amod_product_airbp_flag"))) Then
                If r.Item("amod_product_airbp_flag").ToString.Trim.ToUpper = "Y" Then
                  If String.IsNullOrEmpty(sAircraft_usage) Then
                    sAircraft_usage = Constants.PRODUCT_TYPE_P
                  Else
                    sAircraft_usage = sAircraft_usage + "," + Constants.PRODUCT_TYPE_P
                  End If
                End If
              End If

              If Not (IsDBNull(r("amod_manufacturer_common_name"))) Then
                sAircraft_mfrName = r.Item("amod_manufacturer_common_name").ToString.ToUpper.Replace(", INC.", "").Trim
              End If

              If Not (IsDBNull(r("amod_jniq_size"))) Then
                sAircraft_jniqSize = r.Item("amod_jniq_size").ToString.Trim
              End If

              arrTypeMakeModel(nCounter, 0) = nCounter
              arrTypeMakeModel(nCounter, 1) = sAircraft_type
              arrTypeMakeModel(nCounter, 2) = sAircraft_make
              arrTypeMakeModel(nCounter, 3) = sAircraft_make_abbrev
              arrTypeMakeModel(nCounter, 4) = sAircraft_model
              arrTypeMakeModel(nCounter, 5) = sAircraft_modelID
              arrTypeMakeModel(nCounter, 6) = sAircraft_usage
              arrTypeMakeModel(nCounter, 7) = sAircraft_frameType
              arrTypeMakeModel(nCounter, 8) = sAircraft_mfrName
              arrTypeMakeModel(nCounter, 9) = sAircraft_jniqSize

              nCounter += 1
              sAircraft_usage = ""

            Next ' r As DataRow In results_table.Rows

            HttpContext.Current.Session.Item("UserDefaultFlag") = True

            If IsArray(arrTypeMakeModel) And Not IsNothing(arrTypeMakeModel) And UBound(arrTypeMakeModel) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrTypeMakeModel, UBound(arrTypeMakeModel), Constants.serverAIRFRAMEARRAY_DIM)
              HttpContext.Current.Session.Item("DefaultAirframeArray") = arrTypeMakeModel
            End If

          Else
            HttpContext.Current.Session.Item("UserDefaultFlag") = False
          End If ' results_table.Rows.Count > 0

        Else
          HttpContext.Current.Session.Item("UserDefaultFlag") = False
        End If ' Not IsNothing(results_table)

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("DefaultAirframeArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("DefaultAirframeArray"), Array), UBound(HttpContext.Current.Session.Item("DefaultAirframeArray")), Constants.serverAIRFRAMEARRAY_DIM)
        Else
          HttpContext.Current.Session.Item("UserDefaultFlag") = False
        End If ' Not IsNothing(HttpContext.Current.Session.Item("DefaultAirframeArray"))

      End If 'IsNothing(HttpContext.Current.Session.Item("DefaultAirframeArray"))

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillDefaultAirframeArray(ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    arrTypeMakeModel = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillContinentArray(ByRef out_htmlString As String)

    Dim arrContinent(,) As String = Nothing

    Dim results_table As New DataTable

    Dim sCountryName As String = ""
    Dim sCountry_continent_name As String = ""
    Dim sState_code As String = ""
    Dim sState_name As String = ""
    Dim sState_timezone_id As Integer = 0
    Dim sCountry_Active As String = ""

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("ContinentArray")) Then

        results_table = get_continent_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrContinent(results_table.Rows.Count - 1, Constants.serverRGNARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("country_name"))) Then
                sCountryName = HttpContext.Current.Server.HtmlEncode(r.Item("country_name").ToString.Trim)
              End If

              If Not (IsDBNull(r("country_continent_name"))) Then
                sCountry_continent_name = HttpContext.Current.Server.HtmlEncode(r.Item("country_continent_name").ToString.Trim)
              End If

              If Not (IsDBNull(r("state_code"))) Then
                sState_code = r.Item("state_code").ToString.Trim
              End If

              If Not (IsDBNull(r("state_name"))) Then
                sState_name = r.Item("state_name").ToString.Trim
              End If

              If Not (IsDBNull(r("state_timezone_id"))) Then
                sState_timezone_id = CInt(r.Item("state_timezone_id").ToString)
              End If

              If Not (IsDBNull(r("country_active_flag"))) Then
                sCountry_Active = r.Item("country_active_flag").ToString.Trim
              End If

              arrContinent(nCounter, 0) = sCountry_continent_name
              arrContinent(nCounter, 1) = sCountryName
              arrContinent(nCounter, 2) = sState_code
              arrContinent(nCounter, 3) = sState_name
              arrContinent(nCounter, 4) = sState_timezone_id.ToString
              arrContinent(nCounter, 5) = sCountry_Active

              sCountry_continent_name = ""
              sCountryName = ""
              sState_name = ""
              sState_code = ""
              sState_timezone_id = 0
              sCountry_Active = ""

              nCounter += 1

            Next

            If IsArray(arrContinent) And Not IsNothing(arrContinent) And UBound(arrContinent) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrContinent, UBound(arrContinent), Constants.serverRGNARRAY_DIM)
              HttpContext.Current.Session.Item("ContinentArray") = arrContinent
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("ContinentArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("ContinentArray"), Array), UBound(HttpContext.Current.Session.Item("ContinentArray")), Constants.serverRGNARRAY_DIM)
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillContinentArray(ByRef out_htmlString As String) " + ex.Message
    Finally

    End Try

    arrContinent = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillRegionArray(ByRef out_htmlString As String)

    Dim arrRegion(,) As String = Nothing

    Dim results_table As New DataTable

    Dim sGeographic_region_name As String = ""
    Dim sGeographic_country_name As String = ""
    Dim sState_code As String = ""
    Dim sState_name As String = ""
    Dim sState_timezone_id As Integer = 0

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("RegionArray")) Then

        results_table = get_region_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrRegion(results_table.Rows.Count - 1, Constants.serverRGNARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("geographic_region_name"))) Then
                sGeographic_region_name = HttpContext.Current.Server.HtmlEncode(r.Item("geographic_region_name").ToString.Trim)
              End If

              If Not (IsDBNull(r("geographic_country_name"))) Then
                sGeographic_country_name = HttpContext.Current.Server.HtmlEncode(r.Item("geographic_country_name").ToString.Trim)
              End If

              If Not (IsDBNull(r("state_code"))) Then
                sState_code = r.Item("state_code").ToString.Trim
              End If

              If Not (IsDBNull(r("state_name"))) Then
                sState_name = r.Item("state_name").ToString.Trim
              End If

              If Not (IsDBNull(r("state_timezone_id"))) Then
                sState_timezone_id = CInt(r.Item("state_timezone_id").ToString)
              End If

              arrRegion(nCounter, 0) = sGeographic_region_name
              arrRegion(nCounter, 1) = sGeographic_country_name
              arrRegion(nCounter, 2) = sState_code
              arrRegion(nCounter, 3) = sState_name
              arrRegion(nCounter, 4) = sState_timezone_id.ToString
              arrRegion(nCounter, 5) = "Y"

              nCounter += 1

              sGeographic_region_name = ""
              sGeographic_country_name = ""
              sState_name = ""
              sState_code = ""
              sState_timezone_id = 0

            Next

            If IsArray(arrRegion) And Not IsNothing(arrRegion) And UBound(arrRegion) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrRegion, UBound(arrRegion), Constants.serverRGNARRAY_DIM)
              HttpContext.Current.Session.Item("RegionArray") = arrRegion
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("RegionArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("RegionArray"), Array), UBound(HttpContext.Current.Session.Item("RegionArray")), Constants.serverRGNARRAY_DIM)
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillRegionArray(ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    arrRegion = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillTimeZoneArray(ByRef out_htmlString As String)

    Dim arrTimeZone(,) As String = Nothing

    Dim results_table As New DataTable

    Dim fTzone_id As String = ""
    Dim fTzone_name As String = ""
    Dim fTzone_name_short As String = ""

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("TimeZoneArray")) Then

        results_table = get_timezone_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrTimeZone(results_table.Rows.Count - 1, Constants.serverTZARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("tzone_id"))) Then
                fTzone_id = r.Item("tzone_id").ToString.Trim
              End If

              If Not (IsDBNull(r("tzone_name"))) Then
                fTzone_name = r.Item("tzone_name").ToString.Trim
              End If

              If Not (IsDBNull(r("tzone_name_short"))) Then
                fTzone_name_short = r.Item("tzone_name_short").ToString.Trim
              End If

              arrTimeZone(nCounter, 0) = fTzone_id
              arrTimeZone(nCounter, 1) = fTzone_name
              arrTimeZone(nCounter, 2) = fTzone_name_short

              nCounter += 1

            Next

            If IsArray(arrTimeZone) And Not IsNothing(arrTimeZone) And UBound(arrTimeZone) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrTimeZone, UBound(arrTimeZone), Constants.serverTZARRAY_DIM)
              HttpContext.Current.Session.Item("TimeZoneArray") = arrTimeZone
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("TimeZoneArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("TimeZoneArray"), Array), UBound(HttpContext.Current.Session.Item("TimeZoneArray")), Constants.serverTZARRAY_DIM)
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillTimeZoneArray(ByRef out_htmlString As String) " + ex.Message
    Finally

    End Try

    arrTimeZone = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillEventCategoryArray(ByRef out_htmlString As String)

    Dim arrEventCategory(,) As String = Nothing

    Dim results_table As New DataTable

    Dim fPriorevcat_category As String = ""
    Dim fPriorevcat_category_name As String = ""
    Dim fPriorevcat_category_code As String = ""
    Dim added_extra_columns1 As Boolean = False

    Dim nCounter As Long = 0

    Try

      ' only fill array once 
      If IsNothing(HttpContext.Current.Session.Item("EventCategoryArray")) Then

        results_table = get_event_category_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ' CHANGED THIS LINE TO ALLOW 3 EXTRA LINES
            'ReDim arrEventCategory(results_table.Rows.Count - 1, Constants.serverEVENTCATARRAY_DIM)
            ReDim arrEventCategory(results_table.Rows.Count + 2, Constants.serverEVENTCATARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("priorevcat_category"))) Then
                fPriorevcat_category = r.Item("priorevcat_category").ToString.Trim
              End If

              If Not (IsDBNull(r("priorevcat_category_name"))) Then
                fPriorevcat_category_name = r.Item("priorevcat_category_name").ToString.Trim
              End If

              If Not (IsDBNull(r("priorevcat_category_code"))) Then
                fPriorevcat_category_code = r.Item("priorevcat_category_code").ToString.Trim
              End If

              arrEventCategory(nCounter, 0) = fPriorevcat_category
              arrEventCategory(nCounter, 1) = fPriorevcat_category_name
              arrEventCategory(nCounter, 2) = fPriorevcat_category_code

              If Trim(fPriorevcat_category) = "Aircraft Information" And added_extra_columns1 = False Then
                added_extra_columns1 = True
                nCounter += 1
                arrEventCategory(nCounter, 0) = fPriorevcat_category
                arrEventCategory(nCounter, 1) = "Aircraft Back In Service"
                arrEventCategory(nCounter, 2) = "SC^Aircraft Back In Service"

                nCounter += 1
                arrEventCategory(nCounter, 0) = fPriorevcat_category
                arrEventCategory(nCounter, 1) = "Written Off"
                arrEventCategory(nCounter, 2) = "SC^Written Off"

                nCounter += 1
                arrEventCategory(nCounter, 0) = fPriorevcat_category
                arrEventCategory(nCounter, 1) = "Withdrawn From Use"
                arrEventCategory(nCounter, 2) = "SC^Withdrawn From Use"
              End If


              nCounter += 1

            Next

            If IsArray(arrEventCategory) And Not IsNothing(arrEventCategory) And UBound(arrEventCategory) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrEventCategory, UBound(arrEventCategory), Constants.serverEVENTCATARRAY_DIM)
              HttpContext.Current.Session.Item("EventCategoryArray") = arrEventCategory
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("EventCategoryArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("EventCategoryArray"), Array), UBound(HttpContext.Current.Session.Item("EventCategoryArray")), Constants.serverEVENTCATARRAY_DIM)
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillEventCategoryArray(ByRef out_htmlString As String) " + ex.Message
    Finally

    End Try

    arrEventCategory = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillYachtArray(ByRef out_htmlString As String)

    Dim arrCategoryBrandModel(,) As String = Nothing
    Dim arrCategoryBrandModelYmodIndex(,) As String = Nothing

    Dim results_table As New DataTable

    Dim sYacht_category As String = ""
    Dim sYacht_motor_type As String = ""
    Dim sYacht_brand As String = ""
    Dim sYacht_brand_abbrev As String = ""
    Dim sYacht_model As String = ""
    Dim sYacht_modelID As Long = 0

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("YachtArray")) Then

        results_table = get_yacht_brand_model_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrCategoryBrandModel(results_table.Rows.Count - 1, Constants.serverYACHTARRAY_DIM)
            ReDim arrCategoryBrandModelYmodIndex(results_table.Rows.Count - 1, 1)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("ym_category_size"))) Then
                sYacht_category = r.Item("ym_category_size").ToString.Trim
              End If

              If Not (IsDBNull(r("ym_motor_type"))) Then
                sYacht_motor_type = r.Item("ym_motor_type").ToString.Trim
              End If

              If Not (IsDBNull(r("ym_brand_name"))) Then
                sYacht_brand = r.Item("ym_brand_name").ToString.Trim
              End If

              If Not (IsDBNull(r("ym_brand_abbrev"))) Then
                sYacht_brand_abbrev = r.Item("ym_brand_abbrev").ToString.Trim
              Else
                If Not String.IsNullOrEmpty(sYacht_brand.Trim) Then
                  sYacht_brand_abbrev = sYacht_brand.Substring(0, 2).ToUpper
                Else
                  sYacht_brand_abbrev = Trim(sYacht_motor_type.ToUpper + sYacht_category.ToUpper)
                End If
              End If

              If Not (IsDBNull(r("ym_model_name"))) Then
                sYacht_model = r.Item("ym_model_name").ToString.Trim
              End If

              If Not (IsDBNull(r("ym_model_id"))) Then
                sYacht_modelID = CLng(r.Item("ym_model_id").ToString)
              End If

              arrCategoryBrandModel(nCounter, 0) = nCounter
              arrCategoryBrandModel(nCounter, 1) = sYacht_category
              arrCategoryBrandModel(nCounter, 2) = sYacht_brand
              arrCategoryBrandModel(nCounter, 3) = sYacht_brand_abbrev
              arrCategoryBrandModel(nCounter, 4) = sYacht_model
              arrCategoryBrandModel(nCounter, 5) = sYacht_modelID
              arrCategoryBrandModel(nCounter, 6) = sYacht_motor_type

              arrCategoryBrandModelYmodIndex(nCounter, 0) = sYacht_modelID
              arrCategoryBrandModelYmodIndex(nCounter, 1) = nCounter

              nCounter += 1

            Next

            If IsArray(arrCategoryBrandModel) And Not IsNothing(arrCategoryBrandModel) And UBound(arrCategoryBrandModel) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrCategoryBrandModel, UBound(arrCategoryBrandModel), Constants.serverYACHTARRAY_DIM)
              HttpContext.Current.Session.Item("YachtArray") = arrCategoryBrandModel
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("YachtArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("YachtArray"), Array), UBound(HttpContext.Current.Session.Item("YachtArray")), Constants.serverYACHTARRAY_DIM)
        End If

      End If

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("YachtYmodArray")) Then
        If IsArray(arrCategoryBrandModelYmodIndex) And Not IsNothing(arrCategoryBrandModelYmodIndex) Then
          HttpContext.Current.Session.Item("YachtYmodArray") = arrCategoryBrandModelYmodIndex
        End If
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillYachtArray(ByRef out_htmlString As String) " + ex.Message
    Finally

    End Try

    arrCategoryBrandModel = Nothing
    arrCategoryBrandModelYmodIndex = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillYachtCategoryLableArray(ByRef out_htmlString As String)
    Dim arrYachtCategoryTypeLabel(,) As String = Nothing

    Dim results_table As New DataTable

    Dim sYCBM_motortype As String = ""
    Dim sYCBM_category As String = ""
    Dim sYCBM_code As String = ""
    Dim sYCBM_description As String = ""

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("YachtCategoryLableArray")) Then

        results_table = get_yacht_category_info()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrYachtCategoryTypeLabel(results_table.Rows.Count - 1, Constants.serverYACHTLABLEARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("ycbm_motortype"))) Then
                sYCBM_motortype = Trim(r.Item("ycbm_motortype").ToString)
              End If

              If Not (IsDBNull(r("ycbm_category"))) Then
                sYCBM_category = Trim(r.Item("ycbm_category").ToString)
              End If

              If Not (IsDBNull(r("ycbm_code"))) Then
                sYCBM_code = Trim(r.Item("ycbm_code").ToString)
              End If

              If Not (IsDBNull(r("ycbm_description"))) Then
                sYCBM_description = Trim(r.Item("ycbm_description").ToString)
              End If

              arrYachtCategoryTypeLabel(nCounter, 0) = nCounter
              arrYachtCategoryTypeLabel(nCounter, 1) = sYCBM_motortype
              arrYachtCategoryTypeLabel(nCounter, 2) = sYCBM_category
              arrYachtCategoryTypeLabel(nCounter, 3) = sYCBM_code
              arrYachtCategoryTypeLabel(nCounter, 4) = sYCBM_description

              nCounter += 1

            Next

            If IsArray(arrYachtCategoryTypeLabel) And Not IsNothing(arrYachtCategoryTypeLabel) Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrYachtCategoryTypeLabel, UBound(arrYachtCategoryTypeLabel), Constants.serverYACHTLABLEARRAY_DIM)
              HttpContext.Current.Session.Item("YachtCategoryLableArray") = arrYachtCategoryTypeLabel
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("YachtCategoryLableArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("YachtCategoryLableArray"), Array), UBound(HttpContext.Current.Session.Item("YachtCategoryLableArray")), Constants.serverYACHTLABLEARRAY_DIM)
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillYachtCategoryLableArray(ByRef out_htmlString As String) " + ex.Message
    Finally

    End Try

    arrYachtCategoryTypeLabel = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillMfrNamesArray(ByRef out_htmlString As String)

    Dim arrMfrNames(,) As String = Nothing
    Dim arrTemp(,) As String = Nothing

    Dim results_table As New DataTable

    Dim amod_manufacturer_common_name As String = ""
    Dim amod_product_helicopter_flag As String = ""
    Dim amod_product_business_flag As String = ""
    Dim amod_product_commercial_flag As String = ""

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("AircraftMfrNamesArray")) Then

        results_table = get_mfr_names()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrTemp(results_table.Rows.Count - 1, Constants.serverMFRNAMESARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("amod_manufacturer_common_name"))) Then
                amod_manufacturer_common_name = r.Item("amod_manufacturer_common_name").ToString.ToUpper.Replace(", INC.", "").Trim
              End If

              If Not (IsDBNull(r("amod_product_helicopter_flag"))) Then
                amod_product_helicopter_flag = IIf(r.Item("amod_product_helicopter_flag").ToString.Trim.ToUpper.Contains("Y"), "H", "")
              End If

              If Not (IsDBNull(r("amod_product_business_flag"))) Then
                amod_product_business_flag = IIf(r.Item("amod_product_business_flag").ToString.Trim.ToUpper.Contains("Y"), "B", "")
              End If

              If Not (IsDBNull(r("amod_product_commercial_flag"))) Then
                amod_product_commercial_flag = IIf(r.Item("amod_product_commercial_flag").ToString.Trim.ToUpper.Contains("Y"), "C", "")
              End If

              If nCounter > 0 Then

                ' if the current code matches previous code then check if we need to add another flag
                If arrTemp(nCounter - 1, 1).Contains(amod_manufacturer_common_name) Then

                  If String.IsNullOrEmpty(arrTemp(nCounter - 1, 2)) And arrTemp(nCounter - 1, 2) <> amod_product_helicopter_flag Then
                    arrTemp(nCounter - 1, 2) = amod_product_helicopter_flag
                  End If

                  If String.IsNullOrEmpty(arrTemp(nCounter - 1, 3)) And arrTemp(nCounter - 1, 3) <> amod_product_business_flag Then
                    arrTemp(nCounter - 1, 3) = amod_product_business_flag
                  End If

                  If String.IsNullOrEmpty(arrTemp(nCounter - 1, 4)) And arrTemp(nCounter - 1, 4) <> amod_product_commercial_flag Then
                    arrTemp(nCounter - 1, 4) = amod_product_commercial_flag
                  End If

                Else

                  arrTemp(nCounter, 0) = nCounter
                  arrTemp(nCounter, 1) = amod_manufacturer_common_name
                  arrTemp(nCounter, 2) = amod_product_helicopter_flag
                  arrTemp(nCounter, 3) = amod_product_business_flag
                  arrTemp(nCounter, 4) = amod_product_commercial_flag

                  nCounter += 1

                End If

              Else

                arrTemp(nCounter, 0) = nCounter
                arrTemp(nCounter, 1) = amod_manufacturer_common_name
                arrTemp(nCounter, 2) = amod_product_helicopter_flag
                arrTemp(nCounter, 3) = amod_product_business_flag
                arrTemp(nCounter, 4) = amod_product_commercial_flag

                nCounter += 1

              End If

            Next

            ReDim arrMfrNames(nCounter - 1, Constants.serverMFRNAMESARRAY_DIM)

            nCounter = 0

            For x As Integer = 0 To UBound(arrTemp)

              If Not IsNothing(arrTemp(x, 0)) Then

                arrMfrNames(nCounter, 0) = x
                arrMfrNames(nCounter, 1) = arrTemp(x, 1)
                arrMfrNames(nCounter, 2) = arrTemp(x, 2)
                arrMfrNames(nCounter, 3) = arrTemp(x, 3)
                arrMfrNames(nCounter, 4) = arrTemp(x, 4)

                nCounter += 1

              End If

            Next

            If Not IsNothing(arrMfrNames) And IsArray(arrMfrNames) And UBound(arrMfrNames) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrMfrNames, UBound(arrMfrNames), Constants.serverMFRNAMESARRAY_DIM)
              HttpContext.Current.Session.Item("AircraftMfrNamesArray") = arrMfrNames
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("AircraftMfrNamesArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("AircraftMfrNamesArray"), Array), UBound(HttpContext.Current.Session.Item("AircraftMfrNamesArray")), Constants.serverMFRNAMESARRAY_DIM)
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillTimeZoneArray(ByRef out_htmlString As String) " + ex.Message
    Finally

    End Try

    arrMfrNames = Nothing
    results_table = Nothing

  End Sub

  Public Shared Sub fillAircraftSizeArray(ByRef out_htmlString As String)

    Dim arrAircraftSize(,) As String = Nothing
    Dim arrTemp(,) As String = Nothing

    Dim results_table As New DataTable

    Dim amjiqs_cat_code As String = ""
    Dim amjiqs_cat_desc As String = ""
    Dim amod_product_helicopter_flag As String = ""
    Dim amod_product_business_flag As String = ""
    Dim amod_product_commercial_flag As String = ""

    Dim nCounter As Long = 0

    Try

      ' only fill array once
      If IsNothing(HttpContext.Current.Session.Item("AircraftSizeArray")) Then

        results_table = get_aircraft_size()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            ReDim arrTemp(results_table.Rows.Count - 1, Constants.serverSIZECATARRAY_DIM)

            For Each r As DataRow In results_table.Rows

              If Not (IsDBNull(r("amjiqs_cat_code"))) Then
                amjiqs_cat_code = r.Item("amjiqs_cat_code").ToString.Trim
              End If

              If Not (IsDBNull(r("amjiqs_cat_desc"))) Then
                amjiqs_cat_desc = r.Item("amjiqs_cat_desc").ToString.Trim
              End If

              If Not (IsDBNull(r("amod_product_helicopter_flag"))) Then
                amod_product_helicopter_flag = IIf(r.Item("amod_product_helicopter_flag").ToString.Trim.ToUpper.Contains("Y"), "H", "")
              End If

              If Not (IsDBNull(r("amod_product_business_flag"))) Then
                amod_product_business_flag = IIf(r.Item("amod_product_business_flag").ToString.Trim.ToUpper.Contains("Y"), "B", "")
              End If

              If Not (IsDBNull(r("amod_product_commercial_flag"))) Then
                amod_product_commercial_flag = IIf(r.Item("amod_product_commercial_flag").ToString.Trim.ToUpper.Contains("Y"), "C", "")
              End If

              If nCounter > 0 Then

                ' if the current code matches previous code then check if we need to add another flag
                If arrTemp(nCounter - 1, 1).Contains(amjiqs_cat_code) Then

                  If String.IsNullOrEmpty(arrTemp(nCounter - 1, 3)) And arrTemp(nCounter - 1, 3) <> amod_product_helicopter_flag Then
                    arrTemp(nCounter - 1, 3) = amod_product_helicopter_flag
                  End If

                  If String.IsNullOrEmpty(arrTemp(nCounter - 1, 4)) And arrTemp(nCounter - 1, 4) <> amod_product_business_flag Then
                    arrTemp(nCounter - 1, 4) = amod_product_business_flag
                  End If

                  If String.IsNullOrEmpty(arrTemp(nCounter - 1, 5)) And arrTemp(nCounter - 1, 5) <> amod_product_commercial_flag Then
                    arrTemp(nCounter - 1, 5) = amod_product_commercial_flag
                  End If

                Else

                  arrTemp(nCounter, 0) = nCounter
                  arrTemp(nCounter, 1) = amjiqs_cat_code
                  arrTemp(nCounter, 2) = amjiqs_cat_desc
                  arrTemp(nCounter, 3) = amod_product_helicopter_flag
                  arrTemp(nCounter, 4) = amod_product_business_flag
                  arrTemp(nCounter, 5) = amod_product_commercial_flag

                  nCounter += 1

                End If

              Else

                arrTemp(nCounter, 0) = nCounter
                arrTemp(nCounter, 1) = amjiqs_cat_code
                arrTemp(nCounter, 2) = amjiqs_cat_desc
                arrTemp(nCounter, 3) = amod_product_helicopter_flag
                arrTemp(nCounter, 4) = amod_product_business_flag
                arrTemp(nCounter, 5) = amod_product_commercial_flag

                nCounter += 1

              End If

            Next

            ReDim arrAircraftSize(nCounter - 1, Constants.serverSIZECATARRAY_DIM)

            nCounter = 0

            For x As Integer = 0 To UBound(arrTemp)

              If Not IsNothing(arrTemp(x, 0)) Then

                arrAircraftSize(nCounter, 0) = x
                arrAircraftSize(nCounter, 1) = arrTemp(x, 1)
                arrAircraftSize(nCounter, 2) = arrTemp(x, 2)
                arrAircraftSize(nCounter, 3) = arrTemp(x, 3)
                arrAircraftSize(nCounter, 4) = arrTemp(x, 4)
                arrAircraftSize(nCounter, 5) = arrTemp(x, 5)

                nCounter += 1

              End If

            Next

            If IsArray(arrAircraftSize) And Not IsNothing(arrAircraftSize) And UBound(arrAircraftSize) > 0 Then
              out_htmlString = commonEvo.CreateClientStringFromArray(arrAircraftSize, UBound(arrAircraftSize), Constants.serverSIZECATARRAY_DIM)
              HttpContext.Current.Session.Item("AircraftSizeArray") = arrAircraftSize
            End If

          End If

        End If

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("AircraftSizeArray")) Then
          out_htmlString = commonEvo.CreateClientStringFromArray(CType(HttpContext.Current.Session.Item("AircraftSizeArray"), Array), UBound(HttpContext.Current.Session.Item("AircraftSizeArray")), Constants.serverSIZECATARRAY_DIM)
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fillTimeZoneArray(ByRef out_htmlString As String) " + ex.Message
    Finally

    End Try

    arrAircraftSize = Nothing
    results_table = Nothing

  End Sub

  Public Shared Function get_mfr_names() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT amod_manufacturer_common_name, amod_product_helicopter_flag, amod_product_business_flag, amod_product_commercial_flag")
      sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK) WHERE (")
      sQuery.Append(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), True, True))
      sQuery.Append(") ORDER BY amod_manufacturer_common_name ASC")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnMfrNames load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnMfrNames() As DataTable " + ex.Message

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

  Public Shared Function get_aircraft_size() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT amjiqs_cat_code, amjiqs_cat_desc, amod_product_helicopter_flag, amod_product_business_flag, amod_product_commercial_flag FROM Aircraft_Model_JIQ_Size WITH(NOLOCK)")
      sQuery.Append(" LEFT OUTER JOIN Aircraft_Model WITH(NOLOCK) ON amod_jniq_size = amjiqs_cat_code WHERE (")
      sQuery.Append(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), True, True))
      sQuery.Append(") GROUP BY amjiqs_cat_desc, amjiqs_cat_code, amod_product_helicopter_flag, amod_product_business_flag, amod_product_commercial_flag")
      sQuery.Append(" ORDER BY amjiqs_cat_desc, amjiqs_cat_code")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "viewTypeMakeModel.ascx.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnAcSize load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnAcSize() As DataTable " + ex.Message

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

  Public Shared Function get_continent_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT country_name, country_continent_name, country_active_flag, state_code, state_name, state_timezone_id")
      sQuery.Append(" FROM Country WITH(NOLOCK)")
      sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON (state_country = country_name And state_active_flag = 'Y')")
      sQuery.Append(" ORDER BY country_continent_name, country_name, state_code")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_continent_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_continent_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_continent_info() As DataTable " + ex.Message

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

  Public Shared Function get_region_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT geographic_id, geographic_region_name, geographic_country_name, state_name, state_code, state_timezone_id")
      sQuery.Append(" FROM Geographic WITH(NOLOCK)")
      sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON (geographic_state_code = state_code AND geographic_country_name = state_country)")
      sQuery.Append(" AND state_active_flag = 'Y'")
      sQuery.Append(" ORDER BY geographic_region_name, geographic_country_name, state_code")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_region_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_region_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_region_info() As DataTable " + ex.Message

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

  Public Shared Function get_timezone_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT tzone_id, tzone_name, tzone_name_short FROM Timezone WITH(NOLOCK) ORDER BY tzone_sort_num")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_timezone_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_timezone_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_timezone_info() As DataTable " + ex.Message

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

  Public Shared Function get_event_category_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bAerodexFlag As Boolean = HttpContext.Current.Session.Item("localPreferences").AerodexFlag.ToString.ToLower()

    Try

      sQuery.Append("SELECT priorevcat_category, priorevcat_category_name, priorevcat_category_code")
      sQuery.Append(" FROM Priority_Events_Category WITH(NOLOCK)")

      If bAerodexFlag Then
        sQuery.Append(" WHERE priorevcat_category <> 'Market Status' and priorevcat_category_code not in ('NNESC', 'NSCPCT','RNESC','RSCPCT') ")
      End If

      sQuery.Append(" ORDER BY priorevcat_category, priorevcat_category_name, priorevcat_category_code")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_event_category_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_event_category_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_event_category_info() As DataTable " + ex.Message

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

  Public Shared Function get_make_model_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append(GenerateAirframeSelectionQuery(HttpContext.Current.Session.Item("localPreferences")))

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_make_model_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_make_model_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_make_model_info() As DataTable " + ex.Message

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

  Public Shared Function get_default_make_model_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()
    Dim sDefaultModels As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      ' first get the list of "default models" from this subscription
      sQuery.Append("SELECT subins_default_models FROM Subscription_Install WHERE (subins_session_guid = '" + HttpContext.Current.Session.Item("localPreferences").SessionGUID.ToString + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_default_make_model_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.Default)

      Try
        atemptable.Load(SqlReader)

        If Not IsNothing(atemptable) Then
          If atemptable.Rows.Count > 0 Then
            For Each r As DataRow In atemptable.Rows
              If Not IsDBNull(r("subins_default_models")) Then
                sDefaultModels = r.Item("subins_default_models").ToString.Trim
              End If
            Next
          End If
        End If

        If Not String.IsNullOrEmpty(sDefaultModels) Then

          ' clean up previous results
          sQuery = Nothing
          sQuery = New StringBuilder
          atemptable = Nothing
          atemptable = New DataTable

          ' now use the list of selected models to generate a make/model query
          sQuery.Append("SELECT DISTINCT amod_id, amod_airframe_type_code, amod_type_code, amod_make_name, amod_make_abbrev, amod_model_name,")
          sQuery.Append(" amod_product_business_flag, amod_product_commercial_flag, amod_product_helicopter_flag, amod_product_airbp_flag, amod_manufacturer_common_name, amod_jniq_size")
          sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK) WHERE amod_id IN (" + sDefaultModels + ")")
          sQuery.Append(" ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_make_abbrev, amod_id, amod_model_name")

          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_default_make_model_info() As DataTable</b><br />" + sQuery.ToString

          SqlCommand.CommandText = sQuery.ToString
          SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

          Try
            atemptable.Load(SqlReader)
          Catch constrExc As System.Data.ConstraintException
            Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_default_make_model_info load datatable(2) " + constrExc.Message
          End Try

        Else
          atemptable = Nothing
        End If

      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_default_make_model_info load datatable(1) " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_default_make_model_info() As DataTable " + ex.Message

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

  Public Shared Function get_aircraft_type_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM Airframe_Make_Type WITH(NOLOCK) ORDER BY afmt_code")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_type_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_aircraft_type_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_aircraft_type_info() As DataTable " + ex.Message

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

  Public Shared Function get_yacht_brand_model_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT ym_model_id, ym_motor_type, ym_category_size, ym_brand_name, ym_brand_abbrev, ym_model_name, ycs_seqnbr")
      sQuery.Append(" FROM Yacht_Model INNER JOIN Yacht_Category_Size WITH (NOLOCK) ON ym_category_size = ycs_category_size AND ym_motor_type = ycs_motor_type")
      sQuery.Append(" WHERE ym_brand_name <> 'JETNET'")
      sQuery.Append(" GROUP BY ycs_seqnbr, ym_motor_type, ym_category_size, ym_brand_name, ym_brand_abbrev, ym_model_id, ym_model_name")
      sQuery.Append(" ORDER BY ycs_seqnbr, ym_motor_type, ym_category_size, ym_brand_name, ym_brand_abbrev, ym_model_id, ym_model_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_yacht_brand_model_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_yacht_brand_model_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_yacht_brand_model_info() As DataTable " + ex.Message

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

  Public Shared Function get_yacht_category_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT ycbm_motortype, ycbm_category, ycbm_code, ycbm_description, ycs_seqnbr FROM Yacht_Hull_Category_Type")
      sQuery.Append(" INNER JOIN Yacht_Category_Size WITH (NOLOCK) ON ycbm_category = ycs_category_size AND ycbm_motortype = ycs_motor_type")
      sQuery.Append(" ORDER BY ycs_seqnbr")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_yacht_category_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_yacht_category_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_yacht_category_info() As DataTable " + ex.Message

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

  Public Shared Function get_model_permissions(ByVal amod_id As Integer, ByVal bus_flag As Boolean, ByVal comm_flag As Boolean, ByVal heli_flag As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append(" select amod_product_business_flag, amod_product_commercial_flag, amod_product_helicopter_flag ")
      sQuery.Append(" from Aircraft_Model with (NOLOCK) ")
      sQuery.Append(" where amod_id = " & amod_id & " ")

      If bus_flag = True And comm_flag = True And heli_flag = True Then
        sQuery.Append(" and (amod_product_business_flag = 'Y' or amod_product_commercial_flag = 'Y' or amod_product_helicopter_flag = 'Y') ")
      ElseIf bus_flag = True And comm_flag = True Then
        sQuery.Append(" and (amod_product_business_flag = 'Y' or amod_product_commercial_flag = 'Y') ")
      ElseIf bus_flag = True And heli_flag = True Then
        sQuery.Append(" and (amod_product_business_flag = 'Y' or amod_product_helicopter_flag = 'Y') ")
      ElseIf comm_flag = True And heli_flag = True Then
        sQuery.Append(" and (amod_product_commercial_flag = 'Y' or amod_product_helicopter_flag = 'Y') ")
      ElseIf bus_flag = True Then
        sQuery.Append(" and (amod_product_business_flag = 'Y') ")
      ElseIf comm_flag = True Then
        sQuery.Append(" and (amod_product_commercial_flag = 'Y') ")
      ElseIf heli_flag = True Then
        sQuery.Append(" and (amod_product_helicopter_flag = 'Y') ")
      End If

      sQuery.Append(Constants.cSingleSpace + GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_model_permissions() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_model_permissions load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_model_permissions() As DataTable " + ex.Message

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

  Public Shared Function check_user_model_permissions(ByVal bus_flag As Boolean, ByVal comm_flag As Boolean, ByVal heli_flag As Boolean, ByVal amod_id As Integer) As Boolean

    Dim results_table As New DataTable
    Dim can_see_model As Boolean = False


    Try
      results_table = get_model_permissions(amod_id, bus_flag, comm_flag, heli_flag)

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then
          can_see_model = True
        End If
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in check_user_model_permissions " + ex.Message

    Finally

    End Try

    results_table = Nothing

    Return can_see_model

  End Function

#End Region

#Region "preferences_aspx_functions"

  Public Shared Function CheckUniquePhoneNumber(ByRef MySesState As HttpSessionState, ByVal inPhoneNumber As String) As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim bResult As Boolean = False

    Try

      sQuery = "SELECT DISTINCT subins_sub_id, subins_login, subins_seq_no, subins_cell_number FROM Subscription_Install WITH(NOLOCK) WHERE (subins_cell_number = '" + inPhoneNumber.Trim + "')"

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("subins_cell_number")) Then
            If Not String.IsNullOrEmpty(lDataReader.Item("subins_cell_number").ToString) Then
              If lDataReader.Item("subins_cell_number").ToString.Trim = inPhoneNumber.Trim Then

                If Not (CLng(lDataReader.Item("subins_sub_id").ToString) = CLng(MySesState.Item("localUser").crmSubSubID.ToString) And
                  lDataReader.Item("subins_login").ToString.Trim = MySesState.Item("localUser").crmUserLogin.ToString.Trim And
                  CInt(lDataReader.Item("subins_seq_no").ToString) = CInt(MySesState.Item("localUser").crmSubSeqNo.ToString)) Then
                  bResult = True
                End If

                Exit Do

              End If
            End If
          End If

        Loop

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    Return bResult

  End Function

  Public Shared Function Get_Default_User_View(ByVal nViewID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As StringBuilder = New StringBuilder()

    Dim sViewName As String = ""

    If Not CBool(HttpContext.Current.Session.Item("localPreferences").AerodexFlag) Then
      sQuery.Append("SELECT * FROM Evolution_Views WITH(NOLOCK) WHERE evoview_id = " + nViewID.ToString + " ORDER BY evoview_seq_no")
    ElseIf CBool(HttpContext.Current.Session.Item("localPreferences").AerodexFlag) Then
      sQuery.Append("SELECT * FROM Evolution_Views WITH(NOLOCK) WHERE evoview_aerodex_flag = 'Y' AND evoview_id = " + nViewID.ToString + " ORDER BY evoview_seq_no")
    End If

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, GetType(commonEvo).FullName, sQuery.ToString)

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not (IsDBNull(lDataReader("evoview_title"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("evoview_title").ToString) Then
            sViewName = lDataReader("evoview_title").ToString
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException
      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, GetType(commonEvo).FullName, SqlException.Message)

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return "view ID : " + nViewID.ToString

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    Return sViewName

  End Function

  Public Shared Function Get_Default_User_View_Name(ByVal nViewID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As StringBuilder = New StringBuilder()

    Dim sViewName As String = ""

    If Not CBool(HttpContext.Current.Session.Item("localPreferences").AerodexFlag) Then
      sQuery.Append("SELECT * FROM Evolution_Views WITH(NOLOCK) WHERE evoview_id = " + nViewID.ToString + " ORDER BY evoview_seq_no")
    ElseIf CBool(HttpContext.Current.Session.Item("localPreferences").AerodexFlag) Then
      sQuery.Append("SELECT * FROM Evolution_Views WITH(NOLOCK) WHERE evoview_aerodex_flag = 'Y' AND evoview_id = " + nViewID.ToString + " ORDER BY evoview_seq_no")
    End If

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, GetType(commonEvo).FullName, sQuery.ToString)

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not (IsDBNull(lDataReader("evoview_title"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("evoview_title").ToString) Then
            sViewName = lDataReader("evoview_title").ToString
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException
      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, GetType(commonEvo).FullName, SqlException.Message)

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return "view ID : " + nViewID.ToString

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    Return sViewName

  End Function

  Public Shared Function Get_Default_User_Background(ByVal nBackgroundID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As StringBuilder = New StringBuilder()

    Dim sBackgroundName As String = ""

    sQuery.Append("SELECT * FROM Evolution_Backgrounds WITH (NOLOCK) WHERE (evoback_active_flag = 'Y') AND evoback_id = " + nBackgroundID.ToString)

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not (IsDBNull(lDataReader("evoback_title"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("evoback_title").ToString) Then
            sBackgroundName = lDataReader("evoback_title").ToString.Replace(" ", "&nbsp;")
          End If
        End If
        lDataReader.Close()
      Else
        sBackgroundName = "Random"
      End If


    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return sBackgroundName

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    Return sBackgroundName

  End Function

  Public Shared Function CheckForProject(ByRef sDefaultProjectName As String) As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim bResult As Boolean = False

    Try

      sQuery = "SELECT sissc_subject, sissc_tab FROM Subscription_Install_Saved_Search_Criteria WITH(NOLOCK)"
      sQuery &= " WHERE sissc_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString
      sQuery &= " AND sissc_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString + "'"
      sQuery &= " AND sissc_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString
      sQuery &= " AND sissc_default_flag = 'Y'"

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        bResult = True

        lDataReader.Read()

        If Not IsDBNull(lDataReader.Item("sissc_tab")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("sissc_tab").ToString) Then
            sDefaultProjectName = lDataReader.Item("sissc_tab").ToString.Trim.Replace("History", "Transactions") + " Tab: "
          End If
        End If

        If Not IsDBNull(lDataReader.Item("sissc_subject")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("sissc_subject").ToString) Then
            sDefaultProjectName &= lDataReader.Item("sissc_subject").ToString.Trim
          End If
        End If

      End If

      lDataReader.Close()


    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    Return bResult

  End Function

  Public Shared Function ReturnSMSProviderName(ByVal inProviderID As Integer) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim strResult As String = "&nbsp;"

    Try

      sQuery = "SELECT DISTINCT smstxtcar_carrier FROM SMS_Text_Message_Carrier WITH(NOLOCK) WHERE (smstxtcar_id = " + inProviderID.ToString
      sQuery &= " AND smstxtcar_active_flag = 'Y')"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText + "<br /><br />ReturnSMSProviderName() As string</b><br />" + sQuery.ToString

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not IsDBNull(lDataReader.Item("smstxtcar_carrier")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("smstxtcar_carrier").ToString) Then
            strResult = lDataReader.Item("smstxtcar_carrier").ToString.Trim
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    Return strResult

  End Function

  Public Shared Function Get_SMS_Providers() As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT smstxtcar_id, smstxtcar_country, smstxtcar_carrier FROM SMS_Text_Message_Carrier WITH(NOLOCK) WHERE")
      sQuery.Append(" smstxtcar_active_flag = 'Y' ORDER BY smstxtcar_country, smstxtcar_carrier")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText + "<br /><br />Get_SMS_Providers() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

    Catch ex As Exception
      Return Nothing

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

  Public Shared Function getUserAutoLogonCookies(ByVal sCookieName As String, ByRef bNoCookie As Boolean) As Boolean

    Dim nCount As Integer
    Dim tmpAutoLogonFlag As Boolean = False

    nCount = 0

    bNoCookie = False

    If Not IsNothing(HttpContext.Current.Request.Cookies.Item(sCookieName)) Then
      If HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Count > 0 Then
        For nCount = 0 To HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Count - 1
          If Not IsNothing(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.GetKey(0)) Then
            If nCount = 0 Then ' only get first cookie
              tmpAutoLogonFlag = IIf(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Item(0).ToString.ToLower.Trim = "true", True, False)
            End If
          Else
            tmpAutoLogonFlag = IIf(HttpContext.Current.Request.Cookies(sCookieName).Value.ToString.ToLower.Trim = "true", True, False)
          End If
        Next ' nCount	
      End If ' if Request.cookies(sCookieName).Count > 0 then
    Else
      bNoCookie = True
    End If

    Return tmpAutoLogonFlag

  End Function

  Public Shared Function getUserShowBlankACFields(ByVal sCookieName As String, ByRef bNoCookie As Boolean) As Boolean

    Dim nCount As Integer
    Dim tmpShowCondensedFormat As Boolean = False

    nCount = 0

    bNoCookie = False

    If Not IsNothing(HttpContext.Current.Request.Cookies.Item(sCookieName)) Then
      If HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Count > 0 Then
        For nCount = 0 To HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Count - 1
          If Not IsNothing(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.GetKey(0)) Then
            If nCount = 0 Then ' only get first cookie
              tmpShowCondensedFormat = IIf(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Item(0).ToString.ToLower.Trim = "y", True, False)
            End If
          Else
            tmpShowCondensedFormat = IIf(HttpContext.Current.Request.Cookies(sCookieName).Value.ToLower.Trim = "y", True, False)
          End If
        Next ' nCount	
      End If ' if Request.cookies(sCookieName).Count > 0 then
    Else
      bNoCookie = True
    End If

    Return tmpShowCondensedFormat

  End Function

  Public Shared Function getUserValuesCookie(ByVal sCookieName As String, ByRef bNoCookie As Boolean) As Boolean

    Dim nCount As Integer
    Dim tmpShowValues As Boolean = False

    nCount = 0

    bNoCookie = False

    If Not IsNothing(HttpContext.Current.Request.Cookies.Item(sCookieName)) Then
      If HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Count > 0 Then
        For nCount = 0 To HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Count - 1
          If Not IsNothing(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.GetKey(0)) Then
            If nCount = 0 Then ' only get first cookie
              tmpShowValues = IIf(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Item(0).ToString.ToLower.Trim = "true", True, False)
            End If
          Else
            tmpShowValues = IIf(HttpContext.Current.Request.Cookies(sCookieName).Value.ToLower.Trim = "true", True, False)
          End If
        Next ' nCount	
      End If ' if Request.cookies(sCookieName).Count > 0 then
    Else
      bNoCookie = True
    End If

    Return tmpShowValues

  End Function
#End Region

#Region "create_location_where_clause_functions"

  Public Shared Function get_continent_region_country(ByVal s_inCountry As String, ByVal s_inState As String, ByVal b_isCountry As Boolean) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlReader As System.Data.SqlClient.SqlDataReader : SqlReader = Nothing

    Dim sQuery As New StringBuilder

    Dim sGeo_region_name As String = ""
    Dim sGeo_country_name As String = ""
    Dim sState_code As String = ""
    Dim sState_name As String = ""

    Dim getCountryString As String = ""

    Try

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

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      If b_isCountry Then
        sQuery.Append("SELECT DISTINCT * FROM Country WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON state_country = country_name WHERE state_active_flag = 'Y' AND state_country = '" + s_inCountry.Trim + "'")
      Else
        sQuery.Append("SELECT DISTINCT * FROM Geographic WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON ")
        sQuery.Append("(geographic_state_code = state_code AND geographic_country_name = state_country) WHERE state_active_flag = 'Y' AND state_country = '" + s_inCountry.Trim + "'")
      End If ' isCountry

      sQuery.Append(Constants.cAndClause + sState_name + " IN ('" + s_inState.Trim + "')")

      If b_isCountry Then
        sQuery.Append(" ORDER BY country_continent_name, country_name, state_name, state_code")
      Else
        sQuery.Append(" ORDER BY geographic_region_name, geographic_country_name, state_name, state_code")
      End If ' isCountry

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader(sGeo_country_name)) Then
          If Not String.IsNullOrEmpty(SqlReader.Item(sGeo_country_name).ToString.Trim) Then
            If Not IsDBNull(SqlReader(sState_code)) Then

              If s_inState.ToString.Contains(SqlReader.Item(sState_name).ToString.ToUpper) Then
                getCountryString = SqlReader.Item(sGeo_country_name).ToString.Trim
              End If

            End If
          End If
        End If

      End If

      SqlReader.Close()
      SqlReader = Nothing

    Catch SqlException

      SqlCommand.Dispose()
      SqlConn.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

    sQuery = Nothing

    Return getCountryString

  End Function

  Public Shared Function make_continent_region_where_clause(ByVal inRegion As String, ByVal selectedCountries As String, ByVal selectedStates As String, ByVal b_isBase As Boolean, ByVal b_isCountry As Boolean) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlReader As System.Data.SqlClient.SqlDataReader : SqlReader = Nothing

    Dim sQuery As New StringBuilder
    Dim sWhereClause As String = ""

    Dim sCountryList() As String = Nothing
    Dim sStateList() As String = Nothing

    Dim bHasAState As Boolean = False
    Dim bHadState As Boolean = False
    Dim bfirstState As Boolean = False

    Dim bAllCountries As Boolean = False
    Dim bAllStates As Boolean = False

    Dim sCountryClause As String = ""

    Dim sRememberLastCountry As String = ""
    Dim nRememberLastCountryWithState As Integer = -1

    Dim nLoop As Integer = 0
    Dim mLoop As Integer = 0

    Dim sCountry As String = ""
    Dim sState As String = ""

    Dim sGeo_region_name As String = ""
    Dim sGeo_country_name As String = ""
    Dim sState_code As String = ""
    Dim sState_name As String = ""

    Dim atemptable As New DataTable

    Try

      If b_isBase Then
        sCountry = "ac_aport_country"
        sState = "ac_aport_state"
      Else
        sCountry = "comp_country"
        sState = "comp_state"
      End If

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

      If Not String.IsNullOrEmpty(inRegion.Trim) And String.IsNullOrEmpty(selectedCountries.Trim) And String.IsNullOrEmpty(selectedStates.Trim) Then
        bAllCountries = True
        bAllStates = True
      End If

      If Not String.IsNullOrEmpty(inRegion.Trim) And Not String.IsNullOrEmpty(selectedCountries.Trim) And String.IsNullOrEmpty(selectedStates.Trim) Then
        bAllStates = True
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      ' create a record set of countries and states based on real contentents or defined regions
      If b_isCountry Then
        sQuery.Append("SELECT DISTINCT * FROM Country WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON state_country = country_name WHERE" + IIf(Not bAllStates, " state_active_flag = 'Y' AND", ""))
      Else
        sQuery.Append("SELECT DISTINCT * FROM Geographic WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON (geographic_state_code = state_code AND geographic_country_name = state_country) WHERE" + IIf(Not bAllStates, " state_active_flag = 'Y' AND", ""))
      End If ' isCountry

      If bAllCountries Then
        sQuery.Append(" " + sGeo_region_name + " IN ('" + inRegion.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
      Else
        sQuery.Append(" " + sGeo_region_name + " IN ('" + inRegion.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")

        If Not String.IsNullOrEmpty(selectedCountries.Trim) Then

          If Not String.IsNullOrEmpty(inRegion.Trim) Then
            sQuery.Append(Constants.cAndClause + sGeo_country_name + " IN ('" + selectedCountries.Trim + "')")
          Else
            sQuery.Append(" " + sGeo_country_name + " IN ('" + selectedCountries.Trim + "')")
          End If ' inRegion <> "" 

        End If ' selectedCountries <> ""

      End If ' bAllCountries

      If b_isCountry Then
        sQuery.Append(" ORDER BY country_continent_name, country_name, state_name, state_code")
      Else
        sQuery.Append(" ORDER BY geographic_region_name, geographic_country_name, state_name, state_code")
      End If ' isCountry

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in MakeRegionWhereClause load datatable " + constrExc.Message
      End Try

      SqlReader.Close()
      SqlReader = Nothing

      If Not IsNothing(atemptable) Then

        If atemptable.Rows.Count > 0 Then

          ReDim sCountryList(atemptable.Rows.Count - 1)
          ReDim sStateList(atemptable.Rows.Count - 1)

          For Each r As DataRow In atemptable.Rows

            If Not IsDBNull(r.Item(sGeo_country_name)) Then
              If Not String.IsNullOrEmpty(r.Item(sGeo_country_name).ToString.Trim) Then
                sCountryList(nLoop) = r.Item(sGeo_country_name).ToString.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote) ' change any single "ticks" to "double single ticks"
                nLoop += 1
              End If
            End If

            If Not bAllStates Or Not b_isCountry Then

              If Not IsDBNull(r.Item(sState_name)) And Not String.IsNullOrEmpty(r.Item(sState_name).ToString.Trim) Then

                ' ok someone has selected a state(s) only use those state(s) 
                ' even if there are more state(s) in the record set

                If Not String.IsNullOrEmpty(selectedStates.Trim) Then
                  If selectedStates.Trim.ToUpper.Contains(r.Item(sState_name).ToString.ToUpper) Then
                    sStateList(mLoop) = r.Item(sState_code).ToString.ToUpper
                  Else
                    sCountryList(nLoop - 1) = ""
                  End If
                Else
                  sStateList(mLoop) = r.Item(sState_code).ToString.ToUpper
                End If

              Else ' if this country has an empty state code

                ' and someone has selected states, only display
                ' the countries for the selected states.
                ' set to Empty to exclude this country from our list
                If Not String.IsNullOrEmpty(selectedStates.Trim) Then
                  sCountryList(nLoop - 1) = ""
                End If

                sStateList(mLoop) = ""

              End If

            Else

              sStateList(mLoop) = ""

            End If

            mLoop += 1

          Next

          ' ok now that we have a list of countries and states
          ' generate the in clause

          For x As Integer = 0 To sCountryList.Length - 1

            If Not String.IsNullOrEmpty(sCountryList(x).Trim) Then

              If x = 0 Then ' this is the first time through

                sRememberLastCountry = sCountryList(x).Trim

                If Not String.IsNullOrEmpty(sStateList(x).Trim) Then ' current country also has a state

                  sCountryClause = Constants.cAndClause + Constants.cDoubleOpen + sCountry + Constants.cEq + Constants.cSingleQuote + sCountryList(x).Trim + Constants.cSingleQuote
                  sCountryClause += Constants.cAndClause + Constants.cSingleOpen + sState + Constants.cEq + Constants.cSingleQuote + sStateList(x).Trim + Constants.cSingleQuote

                  bfirstState = True ' this is the first state we find
                  bHadState = True
                  nRememberLastCountryWithState = x

                Else ' the current country might have a state check and see               

                  For y As Integer = x To sStateList.Length - 1
                    If Not String.IsNullOrEmpty(sStateList(y).Trim) Then
                      If sCountryList(y) = sCountryList(x) Then
                        bHasAState = True
                        Exit For
                      End If
                    End If
                  Next

                  If bHasAState Then   ' this country has a state so wrap states with the country
                    bHasAState = False
                    sCountryClause = Constants.cAndClause + Constants.cDoubleOpen + sCountry + Constants.cEq + Constants.cSingleQuote + sCountryList(x).Trim + Constants.cSingleQuote
                  Else                 ' just add the country
                    sCountryClause = Constants.cAndClause + Constants.cSingleOpen + sCountry + Constants.cEq + Constants.cSingleQuote + sCountryList(x).Trim + Constants.cSingleQuote
                  End If

                End If ' not isEmpty(sStateList(nLoop))

              Else ' next time through loop

                If sRememberLastCountry.ToLower.Trim.Contains(sCountryList(x).ToLower.Trim) Then ' same country add another state

                  If Not String.IsNullOrEmpty(sStateList(x).Trim) Then ' this country has another state
                    If bfirstState Then ' we had a state already reset bfirststate flag
                      bfirstState = False ' add the state
                      sCountryClause += Constants.cOrClause + sState + Constants.cEq + Constants.cSingleQuote + sStateList(x).Trim + Constants.cSingleQuote
                    Else
                      If bHadState Then  ' we had a previous state add the state
                        sCountryClause += Constants.cOrClause + sState + Constants.cEq + Constants.cSingleQuote + sStateList(x).Trim + Constants.cSingleQuote
                      Else               ' add the state as the first and only state
                        sCountryClause += Constants.cAndClause + Constants.cSingleOpen + sState + Constants.cEq + Constants.cSingleQuote + sStateList(x).Trim + Constants.cSingleQuote
                      End If
                    End If

                    bHadState = True
                    nRememberLastCountryWithState = x
                  End If

                Else ' different country check to see if it will have states

                  sRememberLastCountry = sCountryList(x)

                  If bHadState Then
                    ' if the last country had a state close it off
                    If Not String.IsNullOrEmpty(sStateList(nRememberLastCountryWithState).Trim) Then

                      ' I have to look ahead to see if this country might have a state
                      ' so I can wrap it right if it does have a state
                      ' start looking from current country forward
                      For y As Integer = x To sStateList.Length - 1
                        If Not String.IsNullOrEmpty(sStateList(y).Trim) Then
                          If sCountryList(y) = sCountryList(x) Then
                            bHasAState = True
                            Exit For
                          End If
                        End If
                      Next

                      If bHasAState Then  ' this country will have a state to add later
                        bHasAState = False  ' so close current country and add the next country ready to wrap a state
                        sCountryClause += Constants.cDoubleClose + Constants.cOrClause + Constants.cSingleOpen + sCountry + Constants.cEq + Constants.cSingleQuote + sCountryList(x).Trim + Constants.cSingleQuote
                      Else                  ' so close current country and add the next country don't have to wrap state
                        sCountryClause += Constants.cDoubleClose + Constants.cOrClause + sCountry + Constants.cEq + Constants.cSingleQuote + sCountryList(x).Trim + Constants.cSingleQuote
                      End If
                    End If ' not isEmpty(sStateList(nRememberLastCountryWithState))

                    nRememberLastCountryWithState = -1  ' reset flags
                    bHadState = False
                    bfirstState = False
                  Else
                    ' if the last country did not have a state so
                    ' I have to look ahead to see if this country might have a state
                    ' so I can wrap it right if it does have a state
                    ' start looking from current country forward
                    For y As Integer = x To sStateList.Length - 1
                      If Not String.IsNullOrEmpty(sStateList(y).Trim) Then
                        If sCountryList(y) = sCountryList(x) Then
                          bHasAState = True
                          Exit For
                        End If
                      End If
                    Next

                    If bHasAState Then ' this country will have a state to add later
                      bHasAState = False
                      If String.IsNullOrEmpty(sCountryClause.Trim) Then ' if our clause is empty add current country ready to wrap for state
                        sCountryClause = Constants.cAndClause + Constants.cDoubleOpen + sCountry + Constants.cEq + Constants.cSingleQuote + sCountryList(x).Trim + Constants.cSingleQuote
                      Else                        ' else add current country don't have to wrap for state
                        sCountryClause += Constants.cOrClause + Constants.cSingleOpen + sCountry + Constants.cEq + Constants.cSingleQuote + sCountryList(x).Trim + Constants.cSingleQuote
                      End If
                    Else ' this country has no state add country
                      sCountryClause += Constants.cOrClause + sCountry + Constants.cEq + Constants.cSingleQuote + sCountryList(x).Trim + Constants.cSingleQuote
                    End If ' bHasAState

                  End If 'bHadState

                  ' check and see if this country has a state  
                  If Not String.IsNullOrEmpty(sStateList(x).Trim) Then  ' add the state to the clause

                    sCountryClause += Constants.cAndClause + Constants.cSingleOpen + sState + Constants.cEq + Constants.cSingleQuote + sStateList(x).Trim + Constants.cSingleQuote
                    bHadState = True

                    If bfirstState = False Then
                      bfirstState = True
                    End If

                    nRememberLastCountryWithState = x
                  End If ' not isEmpty(sStateList(x))

                End If ' sRememberLast = sCountryList(x)

              End If ' nLoop = 0

            End If ' not isEmpty(sCountryList(nLoop))

          Next

          If bHadState Or bfirstState Then
            sCountryClause += Constants.cDoubleClose + Constants.cSingleClose
          Else
            sCountryClause += Constants.cSingleClose
          End If ' bHadState

          sWhereClause += sCountryClause

        End If

      End If

    Catch ex As Exception

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

    sQuery = Nothing
    atemptable = Nothing

    Return sWhereClause

  End Function

  Public Shared Function check_selected_geography_from_dropdowns(ByVal inGeographicZone As String, ByVal inGeographicSubZone As String, ByVal inCountry As String, ByVal inState As String, ByVal b_isBase As Boolean) As String

    Dim sWhereClause As String = ""

    Dim sCountry As String = ""
    Dim sState As String = ""

    If b_isBase Then
      sCountry = "ac_aport_country"
      sState = "state_name" '"ac_aport_state"
    Else
      sCountry = "comp_country"
      sState = "state_name" '"comp_state"
    End If

    'clean input user data if there is data to clean
    If Not String.IsNullOrEmpty(inCountry) Then
      inCountry.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote) ' change any single "ticks" to "double single ticks"
      inCountry = inCountry.Replace(Constants.cCommaDelim, Constants.cValueSeperator)
    End If

    If Not String.IsNullOrEmpty(inState) Then
      inState = inState.Replace(Constants.cCommaDelim, Constants.cValueSeperator)
    End If

    Select Case inGeographicZone.Trim.ToLower

      Case "continent"

        If Not String.IsNullOrEmpty(inGeographicSubZone.Trim) Then
          sWhereClause = make_continent_region_where_clause(inGeographicSubZone, inCountry, inState, b_isBase, True)
        Else

          If Not String.IsNullOrEmpty(inCountry.Trim) And String.IsNullOrEmpty(inState.Trim) Then
            sWhereClause = Constants.cAndClause + sCountry + " IN ('" + inCountry.Trim + "')"
          Else

            If Not String.IsNullOrEmpty(inState.Trim) And Not String.IsNullOrEmpty(inCountry.Trim) Then
              sWhereClause = Constants.cAndClause + Constants.cSingleOpen + sCountry + " IN ('" + inCountry.Trim + "')"
            End If

          End If

          If Not String.IsNullOrEmpty(inState.Trim) And Not String.IsNullOrEmpty(inCountry.Trim) Then

            ' check to see if this state is in this country
            If InStr(1, inCountry, get_continent_region_country(inCountry, inState, True)) = 0 Then
              sWhereClause += Constants.cOrClause + sState + " IN ('" + inState.Trim + "')" + Constants.cSingleClose
            Else
              sWhereClause += Constants.cAndClause + sState + " IN ('" + inState.Trim + "')" + Constants.cSingleClose
            End If

          Else

            If Not String.IsNullOrEmpty(inState.Trim) Then
              sWhereClause = Constants.cAndClause + sState + " IN ('" + inState.Trim + "')"
            End If

          End If

        End If

      Case "region"

        If Not String.IsNullOrEmpty(inGeographicSubZone.Trim) Then
          sWhereClause = make_continent_region_where_clause(inGeographicSubZone, inCountry, inState, b_isBase, False)
        Else

          If Not String.IsNullOrEmpty(inCountry.Trim) And String.IsNullOrEmpty(inState.Trim) Then
            sWhereClause = Constants.cAndClause + sCountry + " IN ('" + inCountry.Trim + "')"
          Else

            If Not String.IsNullOrEmpty(inState.Trim) And Not String.IsNullOrEmpty(inCountry.Trim) Then
              sWhereClause = Constants.cAndClause + Constants.cSingleOpen + sCountry + " IN ('" + inCountry.Trim + "')"
            End If

          End If

          If Not String.IsNullOrEmpty(inState.Trim) And Not String.IsNullOrEmpty(inCountry.Trim) Then

            ' check to see if this state is in this country
            If InStr(1, inCountry, get_continent_region_country(inCountry, inState, False)) = 0 Then
              sWhereClause += Constants.cOrClause + sState + " IN ('" + inState.Trim + "')" + Constants.cSingleClose
            Else
              sWhereClause += Constants.cAndClause + sState + " IN ('" + inState.Trim + "')" + Constants.cSingleClose
            End If

          Else

            If Not String.IsNullOrEmpty(inState.Trim) Then
              sWhereClause = Constants.cAndClause + sState + " IN ('" + inState.Trim + "')"
            End If

          End If

        End If

    End Select

    Return sWhereClause

  End Function

  Public Shared Function TranslateTimeZone(ByVal s_inTimeZone As String) As String

    Const TIMEZONE_INDEX = 0
    Const TIMEZONE_NAME = 1
    'Const TIMEZONE_SHORT = 2

    Dim sOutputString As String = ""
    Dim zoneArray() As String = Nothing

    If Not String.IsNullOrEmpty(s_inTimeZone.Trim) And (IsArray(HttpContext.Current.Session.Item("TimeZoneArray")) And Not IsNothing(HttpContext.Current.Session.Item("TimeZoneArray"))) Then

      zoneArray = s_inTimeZone.Split(Constants.cCommaDelim)

      For x As Integer = 0 To UBound(HttpContext.Current.Session.Item("TimeZoneArray"))

        If zoneArray.Length = 1 Then

          If CInt(HttpContext.Current.Session.Item("TimeZoneArray")(x, TIMEZONE_INDEX)) = CInt(zoneArray(0)) Then
            sOutputString = HttpContext.Current.Session.Item("TimeZoneArray")(x, TIMEZONE_NAME)
          End If

        Else

          For y As Integer = 0 To zoneArray.Length - 1

            If CInt(HttpContext.Current.Session.Item("TimeZoneArray")(x, TIMEZONE_INDEX)) = CInt(zoneArray(y)) Then

              If String.IsNullOrEmpty(sOutputString) Then
                sOutputString = HttpContext.Current.Session.Item("TimeZoneArray")(x, TIMEZONE_NAME)
              Else
                sOutputString += Constants.cCommaDelim + HttpContext.Current.Session.Item("TimeZoneArray")(x, TIMEZONE_NAME)
              End If

            End If ' session("TimeZoneArray")(nloop, TIMEZONE_INDEX) = zoneArray(nloop)
          Next ' xloop

        End If ' UBound(zoneArray) = 0

      Next ' nloop = 0 to UBound(zoneArray)

    End If ' s_inTimeZone <> ""

    Return sOutputString

  End Function

#End Region

#Region "eula_functions"

  Public Shared Function Check_Subscription_Eula(ByVal eulaID As Long) As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlReader As System.Data.SqlClient.SqlDataReader : SqlReader = Nothing

    Dim sQuery As New StringBuilder
    Dim bReturnResult As Boolean = False

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      ' use HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID when not on EVO
      If HttpContext.Current.Session.Item("jetnetWebHostType") = eWebHostTypes.CRM Then
        sQuery.Append("SELECT * FROM Subscription_EULA_Log WHERE (seulal_sub_id = " + HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID.ToString.Trim + ")")
      Else ' use HttpContext.Current.Session.Item("localUser").crmSubSubID when on EVO
        sQuery.Append("SELECT * FROM Subscription_EULA_Log WHERE (seulal_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim + ")")
      End If

      sQuery.Append(Constants.cAndClause + "(seulal_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim + ")")
      sQuery.Append(Constants.cAndClause + "(seulal_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString.Trim + ")")

      ' use HttpContext.Current.Session.Item("CRMJetnetUserName") when not on EVO
      If HttpContext.Current.Session.Item("jetnetWebHostType") = eWebHostTypes.CRM Then
        sQuery.Append(Constants.cAndClause + "(seulal_login = '" + HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim + "')")
      Else ' use HttpContext.Current.Session.Item("localUser").crmUserLogin when on EVO
        sQuery.Append(Constants.cAndClause + "(seulal_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")
      End If

      sQuery.Append(Constants.cAndClause + "(seulal_seula_id = " + eulaID.ToString + ")")

      sQuery.Append(Constants.cAndClause + "(seulal_agreed_to_flag = 'Y')")

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then
        SqlReader.Read()
        If Not IsDBNull(SqlReader("seulal_agreed_to_flag")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("seulal_agreed_to_flag").ToString) Then
            bReturnResult = IIf(SqlReader.Item("seulal_agreed_to_flag").ToString.ToUpper.Contains("Y"), True, False)
          End If
        End If
      End If

      SqlReader.Close()
      SqlReader = Nothing

    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

    Return bReturnResult

  End Function

  Public Shared Sub Get_Current_Eula(ByRef eulaID As Long, ByRef eulaDate As String, ByRef eulaText As String)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlReader As System.Data.SqlClient.SqlDataReader : SqlReader = Nothing

    Dim sQuery As New StringBuilder

    Try

      eulaID = 0
      eulaDate = ""
      eulaText = ""

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      sQuery.Append("SELECT TOP 1 seula_id, seula_license_message, seula__entry_date FROM Subscription_EULA")

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()
        If Not IsDBNull(SqlReader("seula_id")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("seula_id").ToString) Then
            eulaID = CLng(SqlReader.Item("seula_id").ToString)
          End If
        End If

        If Not IsDBNull(SqlReader("seula__entry_date")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("seula__entry_date").ToString) Then
            eulaDate = SqlReader.Item("seula__entry_date").ToString.Trim
          End If
        End If

        If Not IsDBNull(SqlReader("seula_license_message")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("seula_license_message").ToString) Then
            eulaText = SqlReader.Item("seula_license_message").ToString.Trim
          End If
        End If

      End If

      SqlReader.Close()
      SqlReader = Nothing

    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

  End Sub

  Public Shared Sub Update_Subscription_Eula(ByVal sUpdateEulaFlag As String)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    Dim sQuery As New StringBuilder
    Dim results_table As New DataTable

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      results_table = crmWebClient.clsSubscriptionClass.getSessionSubscriptionInfo()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          Dim nEulaID As Long = 0
          Dim sEulaDate As String = ""
          Dim sEulaText As String = ""
          Dim subEmail As String = ""

          Get_Current_Eula(nEulaID, sEulaDate, sEulaText)

          For Each r As DataRow In results_table.Rows

            sEulaText = sEulaText.Replace("[DATETIME]", Now.ToShortDateString)
            sEulaText = sEulaText.Replace("[COMPANYNAME]", get_company_name_fromID(CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), 0, False, True, ""))

            Dim tempName = get_contact_info_fromID(CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString), 0, False, False, False, subEmail)

            If Not String.IsNullOrEmpty(tempName) Then
              Dim nIndex As Integer = tempName.IndexOf("<br />")
              sEulaText = sEulaText.Replace("[CONTACTNAME]", tempName.Substring(0, nIndex).Trim)
            Else ' set generic user name
              sEulaText = sEulaText.Replace("[CONTACTNAME]", HttpContext.Current.Application.Item("crmClientSiteData").webSiteHostName(HttpContext.Current.Session.Item("jetnetWebHostType")).ToString + " USER")
            End If

            sQuery.Append("INSERT INTO Subscription_EULA_Log (seulal_comp_id, seulal_contact_id, seulal_sub_id, seulal_login, seulal_seq_no, seulal_email_address, seulal_tcpip, seulal_seula_id, seulal_license_message, seulal_agreed_to_flag, seulal_host_name, seulal_app_name")
            sQuery.Append(") VALUES (" + HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString.Trim + ", ")   ' company id        
            sQuery.Append(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString.ToString.Trim + ", ")                 ' contact id
            sQuery.Append(r.Item("sub_id").ToString.Trim + ", ")                         ' sub id

            sQuery.Append("'" + r.Item("subins_login").ToString.Trim + "', ")             'login

            sQuery.Append(r.Item("subins_seq_no").ToString.Trim + ", ") ' sequence no

            sQuery.Append("'" + subEmail.Trim + "', ") ' email address
            sQuery.Append("'" + HttpContext.Current.Request.UserHostAddress + "', ") ' ip address
            sQuery.Append(nEulaID.ToString.Trim + ", ")  ' eula id
            sQuery.Append("'" + sEulaText.Replace("'", "''").Trim + "', ")  ' eula message
            sQuery.Append("'" + sUpdateEulaFlag.Trim + "', ")  ' agreed to
            sQuery.Append("'" + HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim + "', ")  'hostname 
            sQuery.Append("'" + r.Item("serfreqan_appname").ToString.Trim + "')") ' app name


            SqlCommand.CommandText = sQuery.ToString

            SqlCommand.ExecuteNonQuery()

          Next

        End If

      End If

    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      results_table = Nothing

    End Try

  End Sub

#End Region

  Public Shared Function TranslateAcWeightClass(ByVal inAcWeightClass As String) As String

    Dim tmpCodeName As String = "Unknown"

    Select Case inAcWeightClass.ToUpper.Trim
      Case "V"
        tmpCodeName = "Very Light Jet"
      Case "L"
        tmpCodeName = "Light"
      Case "M"
        tmpCodeName = "Medium"
      Case "H"
        tmpCodeName = "Heavy"
    End Select

    Return tmpCodeName

  End Function

  Public Shared Function TranslateAcSizes(ByVal inAcSize As String) As String

    Dim tmpCodeName As String = "Unknown"

    Select Case inAcSize.ToUpper.Trim
      Case "ABJ"
        tmpCodeName = "Airline Business Jet"
      Case "ALJ"
        tmpCodeName = "Airliner Jet Converted"
      Case "ALTP"
        tmpCodeName = "Airliner Turbo-Prop Converted"
      Case "LGJ"
        tmpCodeName = "Large Jet"
      Case "LGLR"
        tmpCodeName = "Large Long-Range Jet"
      Case "LGULR"
        tmpCodeName = "Large Ultra Long-Range Jet"
      Case "LJ"
        tmpCodeName = "Light Jet"
      Case "MJ"
        tmpCodeName = "Mid-Size Jet"
      Case "MEP"
        tmpCodeName = "Multi-Engine Piston"
      Case "METP"
        tmpCodeName = "Multi-Engine Turbo-Prop"
      Case "PJ"
        tmpCodeName = "Personal Jet"
      Case "SEP"
        tmpCodeName = "Single-Engine Piston"
      Case "SETP"
        tmpCodeName = "Single-Engine Turbo-Prop"
      Case "SLJ"
        tmpCodeName = "Super Light Jet"
      Case "SMJ"
        tmpCodeName = "Super Mid-Size Jet"
      Case "VLJ"
        tmpCodeName = "Very Light Jet"
    End Select

    Return tmpCodeName

  End Function

  Public Shared Function GetBusinessTypes(ByVal in_CompanyID As Long, ByVal in_CompanyJournalID As Long)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing

    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As StringBuilder = New StringBuilder()

    sQuery.Append("SELECT cbus_name FROM Company_Business_Type WITH(NOLOCK) INNER JOIN Business_Type_Reference WITH(NOLOCK) ON cbus_type = bustypref_type")
    sQuery.Append(" WHERE bustypref_comp_id = " + in_CompanyID.ToString + " AND bustypref_journ_id = " + in_CompanyJournalID.ToString)

    If Not HttpContext.Current.Session.Item("localPreferences").isYachtOnlyProduct Then
      sQuery.Append(" AND cbus_aircraft_flag = 'Y'")
    Else
      sQuery.Append(" AND cbus_yacht_flag = 'Y'")
    End If

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not String.IsNullOrEmpty(htmlOut.ToString) Then
            htmlOut.Append("<br />")
          End If

          If Not IsDBNull(lDataReader.Item("cbus_name")) Then
            If Not String.IsNullOrEmpty(lDataReader.Item("cbus_name").ToString) Then
              htmlOut.Append(lDataReader.Item("cbus_name").ToString)
            End If
          End If

        Loop

      End If
      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    If String.IsNullOrEmpty(htmlOut.ToString) Then
      htmlOut.Append("&lt;Unknown&gt;")
    End If

    Return htmlOut.ToString.Trim

  End Function

  Public Shared Function GetCompanyBusinessTypeName(ByVal in_BusinessType As String) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As New StringBuilder

    Dim strResult As String = "UNKNOWN"

    Try

      sQuery.Append("SELECT cbus_name FROM Company_Business_Type WITH(NOLOCK) WHERE cbus_type = '" + in_BusinessType.Trim + "'")

      If Not HttpContext.Current.Session.Item("localPreferences").isYachtOnlyProduct Then
        sQuery.Append(" AND cbus_aircraft_flag = 'Y'")
      Else
        sQuery.Append(" AND cbus_yacht_flag = 'Y'")
      End If

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not IsDBNull(lDataReader.Item("cbus_name")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("cbus_name").ToString) Then
            strResult = lDataReader.Item("cbus_name").ToString.Trim
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    Return strResult
    sQuery = Nothing

  End Function

  Public Shared Function Get_Aircraft_Model_Info(ByVal amod_id As Integer, ByVal bJustMakeName As Boolean, ByRef nModelProduct As String, Optional ByRef sAirframeModelType As String = "") As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As StringBuilder = New StringBuilder()
    Dim sMakeModelName As String = ""

    nModelProduct = Constants.PRODUCT_CODE_ALL.ToString

    sQuery.Append("SELECT amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, amod_product_business_flag, amod_product_helicopter_flag, amod_product_commercial_flag FROM Aircraft_Model WITH(NOLOCK) WHERE (amod_id = " + amod_id.ToString + ")")

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not (IsDBNull(lDataReader("amod_airframe_type_code"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("amod_airframe_type_code").ToString) Then
            sAirframeModelType = lDataReader("amod_airframe_type_code").ToString
          End If
        End If

        If Not (IsDBNull(lDataReader("amod_type_code"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("amod_type_code").ToString) Then
            sAirframeModelType += ":" + lDataReader("amod_type_code").ToString
          End If
        End If

        If Not (IsDBNull(lDataReader("amod_make_name"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("amod_make_name").ToString) Then
            sMakeModelName = lDataReader("amod_make_name").ToString
          End If
        End If

        If Not (IsDBNull(lDataReader("amod_model_name"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("amod_model_name").ToString) Then
            If Not bJustMakeName Then
              sMakeModelName &= "&nbsp;/&nbsp;" + lDataReader("amod_model_name").ToString
            End If
          End If
        End If

        If Not (IsDBNull(lDataReader("amod_product_business_flag"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("amod_product_business_flag").ToString) Then
            If IIf(lDataReader("amod_product_business_flag").ToString.ToLower.Trim = "y", True, False) Then
              nModelProduct = Constants.PRODUCT_CODE_BUSINESS.ToString
            End If
          End If
        End If

        If Not (IsDBNull(lDataReader("amod_product_helicopter_flag"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("amod_product_helicopter_flag").ToString) Then
            If IIf(lDataReader("amod_product_helicopter_flag").ToString.ToLower.Trim = "y", True, False) Then
              If nModelProduct <> Constants.PRODUCT_CODE_ALL.ToString Then
                nModelProduct = nModelProduct & Constants.cCommaDelim & Constants.PRODUCT_CODE_HELICOPTERS.ToString
              Else
                nModelProduct = Constants.PRODUCT_CODE_HELICOPTERS.ToString
              End If
            End If
          End If
        End If

        If Not (IsDBNull(lDataReader("amod_product_commercial_flag"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("amod_product_commercial_flag").ToString) Then
            If IIf(lDataReader("amod_product_commercial_flag").ToString.ToLower.Trim = "y", True, False) Then
              If nModelProduct <> Constants.PRODUCT_CODE_ALL.ToString Then
                nModelProduct = nModelProduct & Constants.cCommaDelim & Constants.PRODUCT_CODE_COMMERCIAL.ToString
              Else
                nModelProduct = Constants.PRODUCT_CODE_COMMERCIAL.ToString
              End If
            End If
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return sMakeModelName

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    Return sMakeModelName

  End Function

  Public Shared Function Get_Yacht_Model_Info(ByVal ymod_id As Integer, ByVal bJustBrandName As Boolean) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As StringBuilder = New StringBuilder()
    Dim sMakeModelName As String = ""


    sQuery.Append("SELECT ym_brand_name, ym_model_name FROM Yacht_Model WITH(NOLOCK) WHERE (ym_model_id = " + ymod_id.ToString + ")")

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not (IsDBNull(lDataReader("ym_brand_name"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("ym_brand_name").ToString) Then
            sMakeModelName = lDataReader("ym_brand_name").ToString
          End If
        End If

        If Not (IsDBNull(lDataReader("ym_model_name"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("ym_model_name").ToString) Then
            If Not bJustBrandName Then
              sMakeModelName &= "&nbsp;/&nbsp;" + lDataReader("ym_model_name").ToString
            End If
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return sMakeModelName

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    Return sMakeModelName

  End Function

  Public Shared Function GetReferenceType(ByVal inRefCode As String) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim strResult As String = "&nbsp;"

    Try

      sQuery = "SELECT actype_name FROM Aircraft_Contact_Type WITH(NOLOCK) WHERE actype_code = '" + inRefCode.Trim + "'"

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not IsDBNull(lDataReader.Item("actype_name")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("actype_name").ToString) Then
            strResult = lDataReader.Item("actype_name").ToString.Trim
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    Return strResult

  End Function

  Public Shared Function Get_MakesModels_ByProductCode(ByVal bIsForSale As Boolean) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try '

      If Not bIsForSale Then

        sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name, amod_id, amod_max_range_miles, amod_range_tanks_full, amod_range_seats_full, amod_airframe_type_code, atype_code, atype_name")
        sQuery.Append(" FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_type WITH(NOLOCK) ON atype_code = amod_type_code")
        sQuery.Append(" WHERE")
        sQuery.Append(Constants.cSingleSpace + GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), True, True))

      Else

        sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_id, atype_code, atype_name")
        sQuery.Append(" FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_type WITH(NOLOCK) ON atype_code = amod_type_code")
        sQuery.Append(" INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft WITH(NOLOCK) ON amod_id = ac_amod_id")
        sQuery.Append(" WHERE ac_journ_id = 0")
        sQuery.Append(Constants.cSingleSpace + GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        sQuery.Append(Constants.cAndClause + "ac_forsale_flag = 'Y'")

      End If

      sQuery.Append(" ORDER BY amod_make_name, amod_model_name, amod_id")


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText + "<br /><br />Get_MakesModels_ByProductCode() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

    Catch ex As Exception
      Return Nothing

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

  Public Shared Function Get_MakesModels_ByType(ByVal bGetAcDetails As Boolean, ByVal nAmodID As Long, ByVal acType As Integer, ByVal amod_make_name As String) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If Not bGetAcDetails Then

        sQuery.Append("SELECT DISTINCT amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, amod_id")
        sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK) WHERE amod_customer_flag = 'Y' AND (amod_product_helicopter_flag = 'Y' OR amod_product_business_flag = 'Y')")

        If nAmodID > 0 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + nAmodID.ToString)
        End If

        If Not String.IsNullOrEmpty(amod_make_name.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + amod_make_name.Trim + "'")
        End If

        Select Case acType
          Case Constants.VIEW_EXECUTIVE
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
          Case Constants.VIEW_JETS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
          Case Constants.VIEW_TURBOPROPS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
          Case Constants.VIEW_PISTONS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
          Case Constants.VIEW_HELICOPTERS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
        End Select

        sQuery.Append(Constants.cAndClause + "EXISTS (SELECT TOP 1 ac_reg_no FROM Aircraft AS a WITH(NOLOCK) WHERE a.ac_amod_id = amod_id AND (a.ac_product_helicopter_flag = 'Y' OR a.ac_product_business_flag = 'Y') AND a.ac_journ_id = 0 AND a.ac_reg_no <> '' AND a.ac_reg_no IS NOT NULL AND LEN(a.ac_reg_no) > 3)")

        sQuery.Append(" ORDER BY amod_make_name, amod_model_name, amod_id")

      Else

        sQuery.Append("SELECT DISTINCT amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, amod_id, amod_manufacturer, amod_number_of_engines, ac_mfr_year, ac_ser_no_full, ac_reg_no, ac_id")
        sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK) INNER JOIN Aircraft WITH(NOLOCK) ON amod_id = ac_amod_id WHERE ac_journ_id = 0 AND amod_customer_flag = 'Y' AND (ac_product_helicopter_flag = 'Y' OR ac_product_business_flag = 'Y')")
        sQuery.Append(" AND ac_reg_no <> '' AND ac_reg_no IS NOT NULL AND LEN(ac_reg_no) > 3")

        If nAmodID > 0 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + nAmodID.ToString)
        End If

        If Not String.IsNullOrEmpty(amod_make_name.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + amod_make_name.Trim + "'")
        End If

        Select Case acType
          Case Constants.VIEW_EXECUTIVE
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
          Case Constants.VIEW_JETS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
          Case Constants.VIEW_TURBOPROPS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
          Case Constants.VIEW_PISTONS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
          Case Constants.VIEW_HELICOPTERS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
        End Select

        sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id ASC")

      End If



      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText + "<br /><br />Get_MakesModels_ByProductCode() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

    Catch ex As Exception
      Return Nothing

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

  Public Shared Function hasCompanyEvents(ByVal inCompanyID As Long) As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim bResult As Boolean = False

    sQuery = "SELECT count(*) as EventCount FROM Priority_Events WITH(NOLOCK) WHERE priorev_comp_id = " + inCompanyID.ToString

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        If Not IsDBNull(lDataReader.Item("EventCount")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("EventCount").ToString) Then
            If IsNumeric(lDataReader.Item("EventCount").ToString) Then
              If CLng(lDataReader.Item("EventCount").ToString) > 0 Then
                bResult = True
              End If
            End If
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return bResult

  End Function

  Public Shared Function hasAircraftEvents(ByVal nAircraftID As Long) As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim bResult As Boolean = False

    sQuery = "SELECT count(*) as EventCount FROM Aircraft WITH(NOLOCK), Priority_Events WITH(NOLOCK), Aircraft_Model WITH(NOLOCK)"
    sQuery += " WHERE ac_amod_id = amod_id AND priorev_ac_id = ac_id AND ac_journ_id = 0 AND ac_id = " + nAircraftID.ToString
    sQuery += " " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        If Not IsDBNull(lDataReader.Item("EventCount")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("EventCount").ToString) Then
            If IsNumeric(lDataReader.Item("EventCount").ToString) Then
              If CLng(lDataReader.Item("EventCount").ToString) > 0 Then
                bResult = True
              End If
            End If
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return bResult

  End Function

  Public Shared Function Get_Homebase_Fuel_Price() As Double

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlReader As System.Data.SqlClient.SqlDataReader : SqlReader = Nothing

    Dim sQuery As String = ""
    Dim tempFuel_cost As Double = 0.0

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 90


      If HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNET12") Then

        Select Case HttpContext.Current.Session.Item("jetnetWebSiteType")
          Case eWebSiteTypes.LIVE
            sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'LIVE'"
          Case eWebSiteTypes.TEST
            sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'TEST'"
        End Select

      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVO.COM") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVO1.COM") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO1'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVO2.COM") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO2'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETTEST.COM") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'TEST'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETBETA.COM") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'BETA'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVOLUTION.COM") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALHOST") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("NEWEVONET") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNET.COM") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'LIVE'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Trim.Contains("YACHT") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Trim.Contains("ADMIN") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO'"
      ElseIf HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Trim.Contains("CRM") Then
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO'"
      Else
        sQuery = "SELECT * FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'EVO'"
      End If

      SqlCommand.CommandText = sQuery

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then
        SqlReader.Read()
        If Not IsDBNull(SqlReader("evo_config_fuel_cost")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("evo_config_fuel_cost").ToString) Then
            tempFuel_cost = CDbl(SqlReader.Item("evo_config_fuel_cost").ToString)
          End If
        End If
      End If

      SqlReader.Close()
      SqlReader = Nothing

    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

    Return tempFuel_cost

  End Function

  Public Shared Function GetAircraftInfo(ByVal inAircraftID As Long, ByVal bGetAmodID As Boolean, Optional ByVal bUseProductCodeFilter As Boolean = True, Optional ByVal is_from_pdf As Boolean = False) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection

    Dim sQuery = New StringBuilder()
    Dim sOutString As StringBuilder = New StringBuilder()

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConnection.Open()

      sQuery.Append("SELECT amod_make_name, amod_model_name, ac_mfr_year, ac_ser_no_full, ac_reg_no, ac_amod_id")
      sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE ac_id = " + inAircraftID.ToString + " AND ac_journ_id = 0")

      If bUseProductCodeFilter Then
        sQuery.Append(" " + GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      End If

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not bGetAmodID Then

          If is_from_pdf = True Then

            If Not (IsDBNull(lDataReader("amod_make_name"))) Then
              sOutString.Append(lDataReader.Item("amod_make_name").ToString.Trim & " ")
            End If

            If Not (IsDBNull(lDataReader("amod_model_name"))) Then
              sOutString.Append(lDataReader.Item("amod_model_name").ToString.Trim)
            End If

            If Not (IsDBNull(lDataReader("ac_ser_no_full"))) Then
              sOutString.Append(" SN#: " & lDataReader.Item("ac_ser_no_full").ToString.Trim)
            End If

            If Not (IsDBNull(lDataReader("ac_reg_no"))) Then
              sOutString.Append(" REG#: " & lDataReader.Item("ac_reg_no").ToString.Trim)
            End If

          Else

            If Not (IsDBNull(lDataReader("amod_make_name"))) Then
              sOutString.Append(lDataReader.Item("amod_make_name").ToString.Trim + Constants.cSvrDataSeperator)
            End If

            If Not (IsDBNull(lDataReader("amod_model_name"))) Then
              sOutString.Append(lDataReader.Item("amod_model_name").ToString.Trim + Constants.cSvrDataSeperator)
            End If

            If Not (IsDBNull(lDataReader("ac_ser_no_full"))) Then
              sOutString.Append(lDataReader.Item("ac_ser_no_full").ToString.Trim + Constants.cSvrDataSeperator)
            End If

            If Not (IsDBNull(lDataReader("ac_reg_no"))) Then
              sOutString.Append(lDataReader.Item("ac_reg_no").ToString.Trim)
            End If

          End If


        Else

          If Not (IsDBNull(lDataReader("ac_amod_id"))) Then
            sOutString.Append(lDataReader.Item("ac_amod_id").ToString.Trim)
          End If

        End If

      End If

      lDataReader.Close()
      lDataReader.Dispose()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return ""

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    Return (sOutString.ToString)

    sQuery = Nothing
    sOutString = Nothing

  End Function

  Public Shared Function GetAllAircraftInfo_dataTable(ByVal inAircraftID As Long, ByVal inJournalID As Long, ByVal bUseProductCodeFilter As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, amod_id, amod_manufacturer, amod_number_of_engines, ac_mfr_year, ac_ser_no_full, ac_reg_no, ac_year, ac_id")
      sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE ac_id = " + inAircraftID.ToString + " AND ac_journ_id = " + inJournalID.ToString)

      If bUseProductCodeFilter Then
        sQuery.Append(" " + GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      End If

      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim) Then
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      Else
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      End If

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      Return Nothing

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Shared Function GetAircraftInfo_dataTable(ByVal inAircraftID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT Aircraft.*, amod_make_name, amod_model_name, aport_latitude_decimal, aport_longitude_decimal ")
      sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" LEFT OUTER JOIN Airport ON (ac_aport_iata_code = aport_iata_code or ac_aport_icao_code = aport_icao_code) ")
      sQuery.Append(" WHERE ac_id = " + inAircraftID.ToString + " AND ac_journ_id = 0")
      sQuery.Append(" " + GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      Return Nothing

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Shared Function GetAircraftHistoricalInfo_dataTable(ByVal inAircraftID As Long, ByVal inJournalID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bAerodexFlag As Boolean = HttpContext.Current.Session.Item("localPreferences").AerodexFlag.ToString.ToLower()
    Dim excludeONOFFmarket As String = ""

    Try

      sQuery.Append("SELECT * FROM Journal WITH(NOLOCK) INNER JOIN Journal_Category WITH(NOLOCK) ON journ_subcategory_code = jcat_subcategory_code")
      sQuery.Append(" AND jcat_category_code = 'AH' AND SUBSTRING(journ_subcategory_code, 3, 6) <> 'CORR'")

      If bAerodexFlag Then excludeONOFFmarket = ",'OM','MA'"

      sQuery.Append(" AND journ_subcategory_code NOT IN ('IN','DM','EXOFF','EXON'" + excludeONOFFmarket + ")")

      If inJournalID > 0 Then
        sQuery.Append(" WHERE journ_id = " + inJournalID.ToString)
      Else
        sQuery.Append(" WHERE journ_ac_id = " + inAircraftID.ToString)
      End If

      sQuery.Append(" ORDER BY journ_date DESC, journ_id DESC")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      Return Nothing

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Shared Function GetYachtInfo(ByVal inYachtID As Long, ByVal bGetYmodID As Boolean) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection

    Dim sQuery = New StringBuilder()
    Dim sOutString As StringBuilder = New StringBuilder()

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConnection.Open()

      sQuery.Append("SELECT ym_brand_name, ym_model_name, yt_year_mfr, yt_radio_call_sign, ym_mfr_comp_id, yt_model_id, yt_hull_mfr_nbr, yt_yacht_name")
      sQuery.Append(" FROM Yacht WITH(NOLOCK) INNER JOIN Yacht_Model WITH(NOLOCK) ON yt_model_id = ym_model_id")
      sQuery.Append(" WHERE yt_id = " + inYachtID.ToString + " AND yt_journ_id = 0")

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not bGetYmodID Then

          If Not (IsDBNull(lDataReader("ym_brand_name"))) Then
            sOutString.Append(lDataReader.Item("ym_brand_name").ToString.Trim + Constants.cSvrDataSeperator)
          End If

          If Not (IsDBNull(lDataReader("ym_model_name"))) Then
            sOutString.Append(lDataReader.Item("ym_model_name").ToString.Trim + Constants.cSvrDataSeperator)
          End If

          If Not (IsDBNull(lDataReader("ym_mfr_comp_id"))) Then
            sOutString.Append(lDataReader.Item("ym_mfr_comp_id").ToString.Trim + Constants.cSvrDataSeperator)
          End If

          If Not (IsDBNull(lDataReader("yt_hull_mfr_nbr"))) Then
            sOutString.Append(lDataReader.Item("yt_hull_mfr_nbr").ToString.Trim + Constants.cSvrDataSeperator)
          End If

          If Not (IsDBNull(lDataReader("yt_yacht_name"))) Then
            sOutString.Append(lDataReader.Item("yt_yacht_name").ToString.Trim)
          End If

        Else

          If Not (IsDBNull(lDataReader("yt_model_id"))) Then
            sOutString.Append(lDataReader.Item("yt_model_id").ToString.Trim)
          End If

        End If

      End If

      lDataReader.Close()
      lDataReader.Dispose()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return ""

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    Return (sOutString.ToString)

    sQuery = Nothing
    sOutString = Nothing

  End Function

  Public Shared Function GetEngines(ByVal inModelID As Long, ByVal nMAXEngines As Integer, Optional ByVal bJustNames As Boolean = False, Optional ByVal ac_id As Integer = 0, Optional ByRef engine_on_condition As String = "") As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection

    Dim sQuery = New StringBuilder()
    Dim sOutString As StringBuilder = New StringBuilder()

    Dim nCurrentNumber As Integer = 0
    Dim sSeparator As String = ""

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConnection.Open()

      sQuery.Append("SELECT ameng_engine_name, ameng_seq_no")

      If ac_id > 0 Then
        sQuery.Append(",  em_on_condition_flag ")
      End If

      sQuery.Append(" FROM Aircraft_Model_Engine WITH(NOLOCK) ")
      If ac_id > 0 Then
        sQuery.Append("  inner join aircraft with (NOLOCK) on ac_amod_id = ameng_amod_id and ac_journ_id = 0 and ac_engine_name = ameng_engine_name  ")
        sQuery.Append("  inner Join Engine_Models with (NOLOCK) on em_engine_name = ameng_engine_name And em_active_flag = 'Y' ")
      End If

      sQuery.Append("   WHERE ameng_amod_id = " + inModelID.ToString)

      If ac_id > 0 Then
        sQuery.Append(" and ac_id = " & ac_id & " ")
      End If

      sQuery.Append(" GROUP BY ameng_seq_no, ameng_engine_name ")

      If ac_id > 0 Then
        sQuery.Append(",  em_on_condition_flag ")
      End If

      sQuery.Append("   ORDER BY ameng_seq_no, ameng_engine_name")

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If bJustNames Then
            sOutString.Append(sSeparator + lDataReader.Item("ameng_engine_name").ToString.Trim)
            sSeparator = ","
          Else
            sOutString.Append(sSeparator + lDataReader.Item("ameng_engine_name").ToString.Trim + Constants.cHTMLnbsp)
            sSeparator = "<br />"
            nCurrentNumber += 1
          End If

          If ac_id > 0 Then
            If Not IsDBNull(lDataReader.Item("em_on_condition_flag")) Then
              engine_on_condition = lDataReader.Item("em_on_condition_flag")
            End If
          End If

        Loop
      End If

      ' if we count of engines dont match add spacers so table data row height matches
      If ((nMAXEngines > 0) And (nCurrentNumber <> nMAXEngines)) Then
        For nLoop As Integer = nCurrentNumber To nMAXEngines
          If nLoop < nMAXEngines Then
            sOutString.Append("<br />&nbsp;")
          Else
            Exit For
          End If
        Next
      End If

      sOutString.Append("&nbsp;")  ' add a space, either at the end. or blank 

      lDataReader.Close()
      lDataReader.Dispose()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return ""

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    Return (sOutString.ToString)

    sQuery = Nothing
    sOutString = Nothing

  End Function

  Public Shared Sub GetEngines_For_Spaces(ByVal inModelID As Integer, ByRef nMAXEngine_spaces As Integer, Optional ByVal ac_id As Long = 0)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection

    Dim sQuery = New StringBuilder()
    Dim sOutString As StringBuilder = New StringBuilder()

    Dim nCurrentNumber As Integer = 0
    Dim sSeparator As String = ""

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConnection.Open()

      sQuery.Append("SELECT ameng_engine_name, ameng_seq_no")
      sQuery.Append(" FROM Aircraft_Model_Engine WITH(NOLOCK) ")
      If ac_id > 0 Then
        sQuery.Append("  inner join aircraft with (NOLOCK) on ac_amod_id = ameng_amod_id And ac_journ_id = 0 And ac_engine_name = ameng_engine_name  ")
      End If

      sQuery.Append("   WHERE ameng_amod_id = " & inModelID.ToString & " ")

      If ac_id > 0 Then
        sQuery.Append(" And ac_id = " & ac_id & " ")
      End If
      sQuery.Append(" GROUP BY ameng_seq_no, ameng_engine_name ORDER BY ameng_seq_no, ameng_engine_name")

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then
        Do While lDataReader.Read()
          nCurrentNumber += 1
        Loop
      End If

      nMAXEngine_spaces = nCurrentNumber

      lDataReader.Close()
      lDataReader.Dispose()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    sOutString = Nothing

  End Sub

  Public Shared Function GetNumberOfEngines(ByVal inModelID) As Integer
    'Dim adoRs, Query

    'GetNumberOfEngines = CDbl(0)

    'Query = "SELECT count(ameng_engine_name) as EngineCount, ameng_seq_no"
    'Query = Query & " FROM Aircraft_Model_Engine WITH(NOLOCK) WHERE ameng_amod_id = " & CStr(inModelID)
    'Query = Query & " GROUP BY ameng_seq_no ORDER BY ameng_seq_no"

    'adoRS = Session("objUserConn").execute(Query)

    'If Not isnull(adoRs) Then

    '  If Not (adoRs.bof And adoRs.eof) Then

    '    If CDbl(trim(adoRS("EngineCount").value)) > CDbl(GetNumberOfEngines) Then
    '      GetNumberOfEngines = CDbl(trim(adoRS("EngineCount").value))
    '    End If

    '  End If 'not (adoRs.bof and adoRs.eof)

    '  adoRs.close()

    'End If ' not isnull(adoRs)

    'adoRs = Nothing
    Return 0

  End Function

  Public Shared Function check_for_multi_airframes(ByRef in_DataTable As DataTable) As Boolean

    ' check and see if we have multi-airframe types bHasMultiTypes
    Dim tmpAirframeCode As String = ""
    Dim tstAirframeCode As String = ""
    Dim bGotOne As Boolean = False

    Dim bResult As Boolean = False

    If Not IsNothing(in_DataTable) Then

      If in_DataTable.Rows.Count > 0 Then

        For Each r As DataRow In in_DataTable.Rows

          If Not IsDBNull(r.Item("amod_airframe_type_code")) Then

            If Not String.IsNullOrEmpty(r.Item("amod_airframe_type_code").ToString) Then

              tmpAirframeCode = r.Item("amod_airframe_type_code").ToString.ToUpper

              If (tstAirframeCode.ToUpper <> tmpAirframeCode) And Not bGotOne Then
                tstAirframeCode = tmpAirframeCode
                bGotOne = True
              Else ' we have a airframe , see if this is the same as the one we have or
                ' look untill we find a different one, or end
                If (tstAirframeCode.ToUpper <> tmpAirframeCode) And bGotOne Then
                  bResult = True
                  Exit For
                End If

              End If

            End If

          End If

        Next

      End If

    End If

    Return bResult

  End Function

  Public Shared Function GetForeignExchangeRate(ByVal in_CurrencyID As Integer, ByRef out_CurrencyName As String, ByRef out_CurrencyDate As String) As Double

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing

    Dim sQuery As String = ""
    Dim nTempExchangeRate As Double = CDbl(1)

    out_CurrencyName = ""
    out_CurrencyDate = ""

    sQuery = "SELECT currency_exchange_rate, currency_name, currency_exchange_rate_date FROM Currency WITH(NOLOCK) WHERE currency_id = " + in_CurrencyID.ToString

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not IsDBNull(lDataReader.Item("currency_exchange_rate")) Then
          If CDbl(lDataReader.Item("currency_exchange_rate").ToString) > 0 Then
            nTempExchangeRate = CDbl(lDataReader.Item("currency_exchange_rate").ToString)
          End If
        End If

        If Not IsDBNull(lDataReader.Item("currency_name")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("currency_name").ToString) Then
            out_CurrencyName = lDataReader.Item("currency_name").ToString.Trim
          End If
        End If

        If Not IsDBNull(lDataReader.Item("currency_exchange_rate_date")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("currency_exchange_rate_date").ToString) Then
            out_CurrencyDate = lDataReader.Item("currency_exchange_rate_date").ToString.Trim
          End If
        End If

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return nTempExchangeRate

  End Function

  Public Shared Function get_company_phone(ByVal in_CompanyID As Long, ByVal bGetFirst2Numbers As Boolean) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing

    Dim sCompanyPhone As String = ""

    Dim sQuery As New StringBuilder

    If bGetFirst2Numbers Then
      sQuery.Append("SELECT TOP 2 pnum_number_full, pnum_type FROM Phone_Numbers WITH(NOLOCK) INNER JOIN Phone_Type WITH(NOLOCK) ON ptype_name = pnum_type")
      sQuery.Append(" WHERE pnum_comp_id = " + in_CompanyID.ToString)
      sQuery.Append(" AND pnum_journ_id = 0 AND pnum_contact_id = 0 AND pnum_hide_customer = 'N'")
      sQuery.Append(" ORDER BY ptype_seq_no ASC")
    Else
      sQuery.Append("SELECT pnum_number_full, pnum_type FROM Phone_Numbers WITH(NOLOCK) INNER JOIN Phone_Type WITH(NOLOCK) ON ptype_name = pnum_type")
      sQuery.Append(" WHERE pnum_comp_id = " + in_CompanyID.ToString)
      sQuery.Append(" AND pnum_journ_id = 0 AND pnum_contact_id = 0 AND pnum_hide_customer = 'N'")
      sQuery.Append(" ORDER BY ptype_seq_no ASC")
    End If

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not String.IsNullOrEmpty(sCompanyPhone.Trim) Then
            sCompanyPhone += "<br />"
          End If

          If Not IsDBNull(lDataReader.Item("pnum_type")) Then
            If Not String.IsNullOrEmpty(lDataReader.Item("pnum_type").ToString.Trim) Then
              sCompanyPhone += lDataReader.Item("pnum_type").ToString.Trim + " : "
            End If
          End If
          If Not IsDBNull(lDataReader.Item("pnum_number_full")) Then
            If Not String.IsNullOrEmpty(lDataReader.Item("pnum_number_full").ToString.Trim) Then
              sCompanyPhone += lDataReader.Item("pnum_number_full").ToString.Trim
            End If
          End If

        Loop

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return sCompanyPhone

  End Function

  Public Shared Function get_contact_phone(ByVal in_CompanyID As Long, ByVal in_ContactID As Long, ByVal bGetFirstNumber As Boolean) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing

    Dim sContactPhone As String = ""

    Dim sQuery As New StringBuilder

    If Not bGetFirstNumber Then
      sQuery.Append("SELECT pnum_number_full, pnum_type FROM Phone_Numbers WITH(NOLOCK) INNER JOIN Phone_Type WITH(NOLOCK) ON ptype_name = pnum_type")
      sQuery.Append(" WHERE pnum_comp_id = " + in_CompanyID.ToString + " AND pnum_contact_id = " + in_ContactID.ToString)
      sQuery.Append(" AND pnum_journ_id = 0 AND pnum_hide_customer = 'N'")
    Else
      sQuery.Append("SELECT TOP 1 pnum_number_full FROM Phone_Numbers WITH(NOLOCK) INNER JOIN Phone_Type WITH(NOLOCK) ON ptype_name = pnum_type")
      sQuery.Append(" WHERE pnum_comp_id = " + in_CompanyID.ToString + " AND pnum_contact_id = " + in_ContactID.ToString)
      sQuery.Append(" AND pnum_journ_id = 0 AND pnum_hide_customer = 'N'")
      sQuery.Append(" ORDER BY ptype_seq_no ASC")
    End If

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not bGetFirstNumber Then

            If Not String.IsNullOrEmpty(sContactPhone) Then
              sContactPhone += "<br />"
            End If

            If Not IsDBNull(lDataReader.Item("pnum_type")) Then
              If Not String.IsNullOrEmpty(lDataReader.Item("pnum_type").ToString.Trim) Then
                sContactPhone = lDataReader.Item("pnum_type").ToString.Trim + " : "
              End If
            End If
            If Not IsDBNull(lDataReader.Item("pnum_number_full")) Then
              If Not String.IsNullOrEmpty(lDataReader.Item("pnum_number_full").ToString.Trim) Then
                sContactPhone += lDataReader.Item("pnum_number_full").ToString.Trim
              End If
            End If

          Else

            If Not IsDBNull(lDataReader.Item("pnum_number_full")) Then
              If Not String.IsNullOrEmpty(lDataReader.Item("pnum_number_full").ToString.Trim) Then
                sContactPhone += lDataReader.Item("pnum_number_full").ToString.Trim
              End If
            End If

          End If


        Loop

      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return sContactPhone

  End Function

  Public Shared Function get_company_name_fromID(ByVal nCompanyID As Long,
                                               ByVal nCompanyJournalID As Long,
                                               ByVal bIsDisplay As Boolean,
                                               ByVal bIgnoreHidden As Boolean,
                                               ByRef sExtraCompanyInfo As String) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection

    Dim sQuery As New StringBuilder()
    Dim sOutString As New StringBuilder()

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      sQuery.Append("SELECT DISTINCT comp_name, comp_city, comp_country FROM Company WITH(NOLOCK)")
      sQuery.Append(" WHERE (comp_id = " + nCompanyID.ToString + " AND comp_journ_id = " + nCompanyJournalID.ToString)

      If nCompanyJournalID = 0 Then
        sQuery.Append(" AND comp_active_flag = 'Y'")
      End If

      If Not bIgnoreHidden Then
        sQuery.Append(" AND comp_hide_flag = 'N'")
      End If

      sQuery.Append(")")

      If HttpContext.Current.Session.Item("jetnetWebHostType") <> eWebHostTypes.YACHT Then '
        sQuery.Append(" " + MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
      End If

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not (IsDBNull(lDataReader("comp_name"))) Then

            If bIsDisplay Then
              sOutString.Append("<a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + nCompanyID.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Company Details'>" + lDataReader.Item("comp_name").ToString.Trim + "</a>")
            Else
              sOutString.Append(lDataReader.Item("comp_name").ToString.Trim)
            End If

          End If

          If Not IsNothing(sExtraCompanyInfo) And String.IsNullOrEmpty(sExtraCompanyInfo) Then

            If Not (IsDBNull(lDataReader("comp_city"))) Then
              If Not String.IsNullOrEmpty(lDataReader.Item("comp_city").ToString.Trim) Then
                If String.IsNullOrEmpty(sExtraCompanyInfo) Then
                  sExtraCompanyInfo = lDataReader.Item("comp_city").ToString.Trim
                Else
                  sExtraCompanyInfo += ":" + lDataReader.Item("comp_city").ToString.Trim
                End If
              Else
                sExtraCompanyInfo = "N/A"
              End If
            Else
              sExtraCompanyInfo = "N/A"
            End If

            If Not (IsDBNull(lDataReader("comp_country"))) Then
              If Not String.IsNullOrEmpty(lDataReader.Item("comp_country").ToString.Trim) Then
                If String.IsNullOrEmpty(sExtraCompanyInfo) Then
                  sExtraCompanyInfo = lDataReader.Item("comp_country").ToString.Trim
                Else
                  sExtraCompanyInfo += ":" + lDataReader.Item("comp_country").ToString.Trim
                End If
              End If
            End If

          End If

        Loop ' lDataReader.HasRows

      Else

        lDataReader.Close()

        If nCompanyJournalID = 0 Then
          SqlCommand.CommandText = sQuery.ToString.Replace(" AND comp_active_flag = 'Y'", "")
        End If

        lDataReader = SqlCommand.ExecuteReader()

        If lDataReader.HasRows Then

          Do While lDataReader.Read()

            If Not (IsDBNull(lDataReader("comp_name"))) Then

              If bIsDisplay Then
                sOutString.Append("<a class=""underline"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + nCompanyID.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display INACTIVE Company Details"">" + lDataReader.Item("comp_name").ToString.Trim + "</a>&nbsp;<em>(Inactive)</em>)")
              Else
                sOutString.Append(lDataReader.Item("comp_name").ToString.Trim + "&nbsp;<em>(Inactive)</em>")
              End If

            End If

            If Not IsNothing(sExtraCompanyInfo) And String.IsNullOrEmpty(sExtraCompanyInfo) Then

              If Not (IsDBNull(lDataReader("comp_city"))) Then
                If Not String.IsNullOrEmpty(lDataReader.Item("comp_city").ToString.Trim) Then
                  If String.IsNullOrEmpty(sExtraCompanyInfo) Then
                    sExtraCompanyInfo = lDataReader.Item("comp_city").ToString.Trim
                  Else
                    sExtraCompanyInfo += ":" + lDataReader.Item("comp_city").ToString.Trim
                  End If
                Else
                  sExtraCompanyInfo = "N/A"
                End If
              Else
                sExtraCompanyInfo = "N/A"
              End If

              If Not (IsDBNull(lDataReader("comp_country"))) Then
                If Not String.IsNullOrEmpty(lDataReader.Item("comp_country").ToString.Trim) Then
                  If String.IsNullOrEmpty(sExtraCompanyInfo) Then
                    sExtraCompanyInfo = lDataReader.Item("comp_country").ToString.Trim
                  Else
                    sExtraCompanyInfo += ":" + lDataReader.Item("comp_country").ToString.Trim
                  End If
                End If
              End If

            End If

          Loop ' lDataReader.HasRows

        Else
          sOutString.Append("COMPANY NAME NOT FOUND")
        End If

      End If

    Catch SqlException

      sOutString.Append("COMPANY NAME NOT FOUND")

    End Try

    lDataReader.Close()
    lDataReader.Dispose()
    SqlCommand.Dispose()
    SqlConnection.Close()
    SqlConnection.Dispose()

    Return sOutString.ToString

    sOutString = Nothing

  End Function

  Public Shared Function get_company_info_fromID(ByVal inCompanyID As Long, ByVal inJournalID As Long, ByVal bShowPhone As Boolean, ByVal bIsDisplay As Boolean, ByRef pictureLink As String, ByRef siteLink As String, Optional ByVal bOnlyDataTable As Boolean = False, Optional ByRef compDataTable As DataTable = Nothing) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection

    Dim sQuery As StringBuilder = New StringBuilder()
    Dim sOutString As StringBuilder = New StringBuilder()

    Try

      Try

        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then

          SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
          sQuery.Append("SELECT DISTINCT comp_id, comp_name, comp_name_alt_type, comp_name_alt, comp_address1, comp_address2, comp_city, comp_state, comp_country, comp_zip_code, comp_web_address, comp_email_address, comp_fractowr_notes")

          sQuery.Append(" FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Company WITH(NOLOCK) WHERE (comp_id = " + inCompanyID.ToString + " AND comp_journ_id = " + inJournalID.ToString)

          sQuery.Append(" AND comp_hide_flag = 'N')")

        Else

          SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

          sQuery.Append("SELECT DISTINCT comp_id, comp_name, comp_name_alt_type, comp_name_alt, comp_address1, comp_address2, comp_city, comp_state, comp_country, comp_zip_code, comp_web_address, comp_email_address, comp_fractowr_notes")
          sQuery.Append(" FROM Company WITH(NOLOCK) WHERE (comp_id = " + inCompanyID.ToString + " AND comp_journ_id = " + inJournalID.ToString)

          If inJournalID = 0 Then
            sQuery.Append(" AND comp_active_flag = 'Y'")
          End If

          sQuery.Append(" AND comp_hide_flag = 'N')")

        End If

        SqlConnection.Open()

        If HttpContext.Current.Session.Item("jetnetWebHostType") <> eWebHostTypes.YACHT Then '
          sQuery.Append(" " + MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
        End If

        SqlCommand.Connection = SqlConnection
        SqlCommand.CommandTimeout = 1000
        SqlCommand.CommandText = sQuery.ToString

        lDataReader = SqlCommand.ExecuteReader()

        If bOnlyDataTable Then

          Try
            compDataTable.Load(lDataReader)
          Catch constrExc As System.Data.ConstraintException
            Dim rowsErr As System.Data.DataRow() = compDataTable.GetErrors()
          End Try

          Return ""

        End If


        If lDataReader.HasRows Then

          Do While lDataReader.Read()

            'Dim tmpID As Long = CLng(lDataReader.Item("comp_id"))
            'Dim imgFolder As String = HttpContext.Current.Server.MapPath("../../photos") + "\company\" + tmpID.ToString
            'Dim imgDisplayFolder As String = "~/photos/company/" + tmpID.ToString

            'Dim file1 As String = imgFolder + ".gif"
            'Dim file2 As String = imgFolder + ".jpg"

            'If System.IO.File.Exists(file1) Then
            'pictureLink = imgDisplayFolder + ".gif"
            'ElseIf System.IO.File.Exists(file2) Then
            'pictureLink = imgDisplayFolder + ".jpg"
            'End If

            If Not (IsDBNull(lDataReader("comp_name"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_name").ToString.Trim) Then

              Dim sCompanyName As String = Replace(lDataReader.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp)

              If bIsDisplay Then
                sOutString.Append("<a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + lDataReader.Item("comp_id").ToString + "&journid=" + inJournalID.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Company Details'>" + sCompanyName)
                sOutString.Append("</a><br />")
              Else
                sOutString.Append(sCompanyName + "&nbsp;<br />")
              End If

            End If

            If Not IsDBNull(lDataReader("comp_name_alt_type")) And Not IsDBNull(lDataReader("comp_name_alt")) Then
              If Not String.IsNullOrEmpty(lDataReader.Item("comp_name_alt_type").ToString.Trim) And Not String.IsNullOrEmpty(lDataReader.Item("comp_name_alt").ToString.Trim) Then
                sOutString.Append(lDataReader.Item("comp_name_alt_type").ToString.Trim + Constants.cSingleSpace + lDataReader.Item("comp_name_alt").ToString.Trim + "<br />")
              End If
            Else
              If Not IsDBNull(lDataReader("comp_name_alt")) Then
                If Not String.IsNullOrEmpty(lDataReader.Item("comp_name_alt").ToString.Trim) Then
                  sOutString.Append(lDataReader.Item("comp_name_alt").ToString.Trim + "<br />")
                End If
              End If
            End If

            If Not (IsDBNull(lDataReader("comp_address1"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_address1").ToString.Trim) Then
              sOutString.Append(lDataReader.Item("comp_address1").ToString.Trim + "<br />")
            End If

            If Not (IsDBNull(lDataReader("comp_address2"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_address2").ToString.Trim) Then
              sOutString.Append(lDataReader.Item("comp_address2").ToString.Trim + "<br />")
            End If

            If Not (IsDBNull(lDataReader("comp_city"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_city").ToString.Trim) Then
              sOutString.Append(lDataReader.Item("comp_city").ToString.Trim)
            End If

            If Not (IsDBNull(lDataReader("comp_state"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_state").ToString.Trim) Then
              If Not (IsDBNull(lDataReader("comp_city"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_city").ToString.Trim) Then
                sOutString.Append(", " + lDataReader.Item("comp_state").ToString.Trim)
              Else
                sOutString.Append(lDataReader.Item("comp_state").ToString.Trim)
              End If
            End If

            If Not (IsDBNull(lDataReader("comp_zip_code"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_zip_code").ToString.Trim) Then
              sOutString.Append(Constants.cHTMLnbsp + lDataReader.Item("comp_zip_code").ToString.Trim)
            End If

            If Not (IsDBNull(lDataReader("comp_country"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_country").ToString.Trim) Then
              If (lDataReader.Item("comp_country").ToString.ToUpper <> "UNITED STATES") Then
                sOutString.Append(Constants.cHTMLnbsp + lDataReader.Item("comp_country").ToString.Trim + "<br />")
              Else
                sOutString.Append("<br />")
              End If
            End If

            If Not IsDBNull(lDataReader("comp_email_address")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_email_address").ToString.Trim) Then
              If bIsDisplay Then
                sOutString.Append("<a href='mailto:" + lDataReader.Item("comp_email_address").ToString.Trim + "' title='Send Email to Company'>" + lDataReader.Item("comp_email_address").ToString.Trim + "</a><br />")
              Else
                sOutString.Append(lDataReader.Item("comp_email_address").ToString.Trim + "<br />")
              End If
            End If

            If Not (IsDBNull(lDataReader("comp_web_address"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_web_address").ToString.Trim) Then

              If bIsDisplay Then
                If lDataReader.Item("comp_web_address").ToString.Trim.ToLower.Contains("www") Then
                  siteLink = "<a href=""http://" + lDataReader.Item("comp_web_address").ToString.Trim + """ target=""new"">" + lDataReader.Item("comp_web_address").ToString.Trim + "</a><br />"
                Else
                  siteLink = "<a href=""" + lDataReader.Item("comp_web_address").ToString.Trim + """ target=""new"">" + lDataReader.Item("comp_web_address").ToString.Trim + "</a><br />"
                End If
                sOutString.Append(siteLink)
              Else
                sOutString.Append(lDataReader.Item("comp_web_address").ToString.Trim + "<br />")
              End If

            End If

            If bShowPhone Then
              sOutString.Append(commonEvo.get_company_phone(CLng(lDataReader.Item("comp_id").ToString), False))
            End If

            If Not (IsDBNull(lDataReader("comp_fractowr_notes"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_fractowr_notes").ToString.Trim) Then
              sOutString.Append("<br /><em>" + lDataReader.Item("comp_fractowr_notes").ToString.Trim + "</em><br />")
            End If

          Loop ' lDataReader.HasRows

        End If

        lDataReader.Close()
        lDataReader.Dispose()

      Catch SqlException

        SqlConnection.Dispose()
        SqlCommand.Dispose()

        sQuery = Nothing

        Return ""

      Finally

        SqlCommand.Dispose()
        SqlConnection.Close()
        SqlConnection.Dispose()

      End Try

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_company_info_fromID(ByVal inCompanyID As Long, ByVal inJournalID As Long, ByVal bShowPhone As Boolean, ByVal bIsDisplay As Boolean, ByRef pictureLink As String, ByRef siteLink As String) As String</b><br />" + ex.Message
    End Try

    Return (sOutString.ToString)

    sQuery = Nothing
    sOutString = Nothing

  End Function

  Public Shared Function get_company_info_from_datarow(ByVal currentRow As DataRow, ByVal inJournalID As Long, ByVal bShowPhone As Boolean, ByVal bIsDisplay As Boolean, ByRef pictureLink As String, ByRef siteLink As String) As String

    Dim sOutString As StringBuilder = New StringBuilder()

    Try

      If Not IsNothing(currentRow) Then

        'Dim tmpID As Long = CLng(lDataReader.Item("comp_id"))
        'Dim imgFolder As String = HttpContext.Current.Server.MapPath("../../photos") + "\company\" + tmpID.ToString
        'Dim imgDisplayFolder As String = "~/photos/company/" + tmpID.ToString

        'Dim file1 As String = imgFolder + ".gif"
        'Dim file2 As String = imgFolder + ".jpg"

        'If System.IO.File.Exists(file1) Then
        'pictureLink = imgDisplayFolder + ".gif"
        'ElseIf System.IO.File.Exists(file2) Then
        'pictureLink = imgDisplayFolder + ".jpg"
        'End If

        If Not (IsDBNull(currentRow.Item("comp_name"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_name").ToString.Trim) Then

          Dim sCompanyName As String = Replace(currentRow.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp)

          If bIsDisplay Then
            sOutString.Append("<a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + currentRow.Item("comp_id").ToString + "&journid=" + inJournalID.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Company Details'>" + sCompanyName)
            sOutString.Append("</a><br />")
          Else
            sOutString.Append(sCompanyName + "<br />")
          End If

        End If

        If Not IsDBNull(currentRow("comp_name_alt_type")) And Not IsDBNull(currentRow("comp_name_alt")) Then
          If Not String.IsNullOrEmpty(currentRow.Item("comp_name_alt_type").ToString.Trim) And Not String.IsNullOrEmpty(currentRow.Item("comp_name_alt").ToString.Trim) Then
            sOutString.Append(currentRow.Item("comp_name_alt_type").ToString.Trim + Constants.cSingleSpace + currentRow.Item("comp_name_alt").ToString.Trim + "<br />")
          End If
        Else
          If Not IsDBNull(currentRow("comp_name_alt")) Then
            If Not String.IsNullOrEmpty(currentRow.Item("comp_name_alt").ToString.Trim) Then
              sOutString.Append(currentRow.Item("comp_name_alt").ToString.Trim + "<br />")
            End If
          End If
        End If

        If Not (IsDBNull(currentRow("comp_address1"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_address1").ToString.Trim) Then
          sOutString.Append(currentRow.Item("comp_address1").ToString.Trim + "<br />")
        End If

        If Not (IsDBNull(currentRow("comp_address2"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_address2").ToString.Trim) Then
          sOutString.Append(currentRow.Item("comp_address2").ToString.Trim + "<br />")
        End If

        If Not (IsDBNull(currentRow("comp_city"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_city").ToString.Trim) Then
          sOutString.Append(currentRow.Item("comp_city").ToString.Trim)
        End If

        If Not (IsDBNull(currentRow("comp_state"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_state").ToString.Trim) Then
          If Not (IsDBNull(currentRow("comp_city"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_city").ToString.Trim) Then
            sOutString.Append(", " + currentRow.Item("comp_state").ToString.Trim)
          Else
            sOutString.Append(currentRow.Item("comp_state").ToString.Trim)
          End If
        End If

        If Not (IsDBNull(currentRow("comp_zip_code"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_zip_code").ToString.Trim) Then
          sOutString.Append(Constants.cHTMLnbsp + currentRow.Item("comp_zip_code").ToString.Trim)
        End If

        If Not (IsDBNull(currentRow("comp_country"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_country").ToString.Trim) Then
          If (currentRow.Item("comp_country").ToString.ToUpper <> "UNITED STATES") Then
            sOutString.Append(Constants.cHTMLnbsp + currentRow.Item("comp_country").ToString.Trim + "<br />")
          Else
            sOutString.Append("<br />")
          End If
        End If

        If Not IsDBNull(currentRow("comp_email_address")) And Not String.IsNullOrEmpty(currentRow.Item("comp_email_address").ToString.Trim) Then
          If bIsDisplay Then
            sOutString.Append("<a href='mailto:" + currentRow.Item("comp_email_address").ToString.Trim + "' title='Send Email to Company'>" + currentRow.Item("comp_email_address").ToString.Trim + "</a><br />")
          Else
            sOutString.Append(currentRow.Item("comp_email_address").ToString.Trim + "<br />")
          End If
        End If

        If Not (IsDBNull(currentRow("comp_web_address"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_web_address").ToString.Trim) Then

          If bIsDisplay Then
            If currentRow.Item("comp_web_address").ToString.Trim.ToLower.Contains("www") Then
              siteLink = "<a href='http://" + currentRow.Item("comp_web_address").ToString.Trim + "' target='new'>" + currentRow.Item("comp_web_address").ToString.Trim + "</a><br />"
            Else
              siteLink = "<a href='" + currentRow.Item("comp_web_address").ToString.Trim + "' target='new'>" + currentRow.Item("comp_web_address").ToString.Trim + "</a><br />"
            End If
          Else
            sOutString.Append(currentRow.Item("comp_web_address").ToString.Trim + "<br />")
          End If

        End If

        If bShowPhone Then
          sOutString.Append(commonEvo.get_company_phone(CLng(currentRow.Item("comp_id").ToString), False))
        End If

        If Not (IsDBNull(currentRow("comp_fractowr_notes"))) And Not String.IsNullOrEmpty(currentRow.Item("comp_fractowr_notes").ToString.Trim) Then
          sOutString.Append("<br /><em>" + currentRow.Item("comp_fractowr_notes").ToString.Trim + "</em><br />")
        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_company_info_from_datarow(currentRow as datarow As Long, ByVal bShowPhone As Boolean, ByVal bIsDisplay As Boolean, ByRef pictureLink As String, ByRef siteLink As String) As String</b><br />" + ex.Message
    End Try

    Return (sOutString.ToString)

    sOutString = Nothing

  End Function

  Public Shared Function get_contact_info_fromID(ByVal inCompanyID As Long, ByVal inContactID As Long, ByVal inJournalID As Long, ByVal bShowPhone As Boolean, ByVal bIsDisplay As Boolean, ByVal bIsChiefPilot As Boolean, ByRef sEmail As String) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection

    Dim sQuery As StringBuilder = New StringBuilder()
    Dim sOutString As StringBuilder = New StringBuilder()

    sEmail = ""

    Try

      Try

        sQuery.Append("SELECT * FROM Contact WITH(NOLOCK) WHERE contact_id = " + inContactID.ToString + " AND contact_journ_id = " + inJournalID.ToString)
        sQuery.Append(" AND contact_hide_flag = 'N'")

        If inJournalID = 0 Then
          sQuery.Append(" AND contact_active_flag  = 'Y'")
        End If

        sQuery.Append(" ORDER BY contact_acpros_seq_no, contact_last_name, contact_first_name")

        SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        SqlConnection.Open()
        SqlCommand.Connection = SqlConnection
        SqlCommand.CommandTimeout = 1000
        SqlCommand.CommandText = sQuery.ToString

        lDataReader = SqlCommand.ExecuteReader()

        If lDataReader.HasRows Then

          Do While lDataReader.Read()

            If bIsDisplay Then
              sOutString.Append("<a class=""underline"" onclick=""javascript:load('DisplayContactDetail.aspx?compid=" + inCompanyID.ToString + "&conid=" + lDataReader.Item("contact_id").ToString + "&JournID=" + inJournalID.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Show Contact Details'>" + lDataReader.Item("contact_sirname").ToString.Trim + Constants.cSingleSpace + lDataReader.Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)
            Else
              sOutString.Append(lDataReader.Item("contact_sirname").ToString.Trim + Constants.cSingleSpace + lDataReader.Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)
            End If

            If Not IsDBNull(lDataReader.Item("contact_middle_initial")) And Not String.IsNullOrEmpty(lDataReader.Item("contact_middle_initial").ToString) Then
              sOutString.Append(lDataReader.Item("contact_middle_initial").ToString.Trim + ". ")
            End If

            If bIsDisplay Then

              If Not (IsDBNull(lDataReader.Item("contact_last_name"))) And Not String.IsNullOrEmpty(lDataReader.Item("contact_last_name").ToString) Then
                sOutString.Append(lDataReader.Item("contact_last_name").ToString.Trim)
              End If

              If Not IsDBNull(lDataReader.Item("contact_suffix")) And Not String.IsNullOrEmpty(lDataReader.Item("contact_suffix").ToString) Then
                sOutString.Append(Constants.cSingleSpace + lDataReader.Item("contact_suffix").ToString.Trim)
              End If

              sOutString.Append("</a><br />")

            Else

              If Not (IsDBNull(lDataReader.Item("contact_last_name"))) And Not String.IsNullOrEmpty(lDataReader.Item("contact_last_name").ToString) Then
                sOutString.Append(lDataReader.Item("contact_last_name").ToString.Trim)
              End If

              If Not (IsDBNull(lDataReader.Item("contact_suffix"))) And Not String.IsNullOrEmpty(lDataReader.Item("contact_suffix").ToString) Then
                sOutString.Append(Constants.cSingleSpace + lDataReader.Item("contact_suffix").ToString.Trim)
              End If

              sOutString.Append("<br />")

            End If

            If Not (IsDBNull(lDataReader.Item("contact_title"))) And Not String.IsNullOrEmpty(lDataReader.Item("contact_title").ToString) Then

              If bIsChiefPilot And Not lDataReader.Item("contact_title").ToString.ToLower.Contains(LCase("Chief Pilot")) Then
                sOutString.Append(lDataReader.Item("contact_title").ToString.Trim + "/Chief Pilot<br />")
              Else
                sOutString.Append(lDataReader.Item("contact_title").ToString.Trim + "<br />")
              End If

            End If

            If Not (IsDBNull(lDataReader.Item("contact_email_address"))) And Not String.IsNullOrEmpty(lDataReader.Item("contact_email_address").ToString) Then
              If bIsDisplay Then
                sOutString.Append("<a href='mailto:" + lDataReader.Item("contact_email_address").ToString.Trim + "'>" + lDataReader.Item("contact_email_address").ToString.Trim + "</a><br />")
              Else
                sOutString.Append(lDataReader.Item("contact_email_address").ToString.Trim + "<br />")
                sEmail = lDataReader.Item("contact_email_address").ToString.Trim
              End If
            End If

            If bShowPhone Then
              sOutString.Append(commonEvo.get_contact_phone(inCompanyID, CLng(lDataReader.Item("contact_id").ToString), False))
            End If

          Loop ' lDataReader.HasRows

        End If

        lDataReader.Close()
        lDataReader.Dispose()

      Catch SqlException

        SqlConnection.Dispose()
        SqlCommand.Dispose()

        sQuery = Nothing

        Return ""

      Finally

        SqlCommand.Dispose()
        SqlConnection.Close()
        SqlConnection.Dispose()

      End Try

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_contact_info_fromID(ByVal inCompanyID As Long, ByVal inJournalID As Long, ByVal bShowPhone As Boolean, ByVal bIsDisplay As Boolean, ByRef pictureLink As String, ByRef siteLink As String) As String</b><br />" + ex.Message
    End Try

    Return (sOutString.ToString)

    sQuery = Nothing
    sOutString = Nothing

  End Function

  Public Shared Function get_contact_info_fromID_returnDatatable(ByVal inCompanyID As Long, ByVal inContactID As Long, ByVal inJournalID As Long, ByVal bShowHidden As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM Contact WITH(NOLOCK) LEFT OUTER JOIN contact_pictures WITH(NOLOCK) ON contact_id = conpic_contact_id AND conpic_hide_flag = 'N'")
      sQuery.Append(" WHERE contact_id = " + inContactID.ToString + " AND contact_journ_id = " + inJournalID.ToString)

      If Not bShowHidden Then
        sQuery.Append(" AND contact_hide_flag = 'N'")
      End If

      If inJournalID = 0 Then
        sQuery.Append(" AND contact_active_flag = 'Y'")
      End If

      sQuery.Append(" ORDER BY contact_acpros_seq_no, contact_last_name, contact_first_name")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in get_contact_info_fromID_returnDatatable load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_contact_info_fromID_returnDatatable(ByVal inCompanyID As Long, ByVal inJournalID As Long) As DataTable</b><br />" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

    sQuery = Nothing

  End Function

  Public Shared Function isChiefPilot(ByVal inContactID As Long,
                                       ByVal nAircraftID As Long,
                                       ByVal nAircraftJournalID As Long) As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim bResult As Boolean = False

    sQuery = "SELECT cref_contact_type FROM Aircraft_Reference WITH(NOLOCK)"
    sQuery += " WHERE cref_contact_id = " + inContactID.ToString
    sQuery += " AND cref_ac_id = " + nAircraftID.ToString
    sQuery += " AND cref_journ_id = " + nAircraftJournalID.ToString
    sQuery += " AND cref_contact_type = '44'"

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then
        bResult = True
      End If

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return bResult

  End Function

  Public Shared Function SafeSqlLikeClauseLiteral(ByVal inputSQL As String) As String

    ' Make the following replacements:
    ' '  becomes  ''
    ' [  becomes  [[]
    ' %  becomes  [%]
    ' _  becomes  [_]

    Dim sTmp As String = inputSQL

    sTmp = inputSQL.Replace("'", "''")
    sTmp += sTmp.Replace("[", "[[]")
    sTmp += sTmp.Replace("%", "[%]")
    sTmp += sTmp.Replace("_", "[_]")

    Return sTmp

  End Function

  Public Shared Function GetExclusiveBrokerCompany(ByVal in_AircraftID As Long, ByVal in_AircraftJournalID As Long, Optional ByRef companyIDstr As String = "") As String

    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As New StringBuilder

    sQuery.Append("SELECT DISTINCT comp_name, comp_id, cref_transmit_seq_no FROM Company WITH(NOLOCK)")
    sQuery.Append(" INNER JOIN Aircraft_Reference WITH(NOLOCK) ON (comp_id = cref_comp_id AND comp_journ_id = cref_journ_id)")
    sQuery.Append(" WHERE (cref_ac_id = " + in_AircraftID.ToString + " AND cref_journ_id = " + in_AircraftJournalID.ToString)
    sQuery.Append(" AND cref_contact_type IN ('93','98','99')")

    If in_AircraftJournalID = 0 Then
      sQuery.Append(" AND comp_active_flag = 'Y'")
    End If

    sQuery.Append(" AND comp_hide_flag = 'N')")
    sQuery.Append(" ORDER BY cref_transmit_seq_no")

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not IsDBNull(lDataReader.Item("comp_name")) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("comp_name").ToString) Then
            htmlOut.Append(lDataReader.Item("comp_name").ToString.Trim)
            If String.IsNullOrEmpty(companyIDstr) Then
              companyIDstr = lDataReader.Item("comp_id").ToString.Trim
            Else
              companyIDstr += Constants.cCommaDelim + lDataReader.Item("comp_id").ToString.Trim
            End If
          End If
        End If

      End If


    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    If String.IsNullOrEmpty(htmlOut.ToString) Then
      htmlOut.Append("&lt;Unknown&gt;")
    End If

    Return htmlOut.ToString.Trim

  End Function

  Public Shared Function DoesCompanyHaveShareRelationships(ByVal in_CompanyRefID As Long) As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim bResult As Boolean = False

    sQuery = "SELECT * FROM Share_Reference WITH(NOLOCK) WHERE sref_cref_id = " + in_CompanyRefID.ToString

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      If lDataReader.HasRows Then
        bResult = True
      End If

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return bResult

  End Function

  Public Shared Sub set_ranges_dates(ByVal current_date As String, ByRef start_date As String, ByRef end_date As String)

    If Trim(start_date) = "" Then
      start_date = CDate(current_date)
    ElseIf CDate(current_date) < CDate(start_date) Then
      start_date = CDate(current_date)
    End If

    If Trim(end_date) = "" Then
      end_date = CDate(current_date)
    ElseIf CDate(current_date) > CDate(end_date) Then
      end_date = CDate(current_date)
    End If



  End Sub

  Public Shared Sub make_ticks_string(ByRef start_date As String, ByRef end_date As String, ByRef ticks_horizontal As String)

    Dim first_month As String = ""
    Dim move_date As Date
    Dim last_month As String = ""
    Dim i As Integer = 0
    Dim months_inbetween As Integer = 0

    first_month = Month(start_date) & "/1/" & Year(start_date)
    move_date = CDate(first_month)
    last_month = Month(end_date) & "/1/" & Year(end_date)

    months_inbetween = DateDiff(DateInterval.Month, CDate(first_month), CDate(last_month))

    first_month = Replace(first_month, "/1/", "/")
    last_month = Replace(last_month, "/1/", "/")

    ticks_horizontal = "new Date(" & Year(first_month) & "," & (Month(first_month) - 1) & ", 1)"

    For i = 1 To months_inbetween + 1 ' no doign -1 so it includes the last month - do plus 1 so that it gets the month after 
      ticks_horizontal &= ", new Date(" & Year(DateAdd(DateInterval.Month, i, move_date)) & "," & (Month(DateAdd(DateInterval.Month, i, move_date)) - 1) & ", 1)"
    Next



  End Sub

  Public Shared Sub make_ticks_string2(ByRef start_date As String, ByRef end_date As String, ByRef ticks_horizontal As String, ByVal date_of_string As String)

    Dim first_month As String = ""
    Dim move_date As Date
    Dim last_month As String = ""
    Dim i As Integer = 0
    Dim months_inbetween As Integer = 0
    Dim split_months_string() As String
    Dim missed_count As Integer = 0
    Dim display_this As Boolean = False
    Dim k As Integer = 0

    split_months_string = Split(date_of_string, ",")

    first_month = Month(start_date) & "/1/" & Year(start_date)
    move_date = CDate(first_month)
    last_month = Month(end_date) & "/1/" & Year(end_date)

    months_inbetween = DateDiff(DateInterval.Month, CDate(first_month), CDate(last_month))

    first_month = Replace(first_month, "/1/", "/")
    last_month = Replace(last_month, "/1/", "/")

    ticks_horizontal = "new Date(" & Year(first_month) & "," & (Month(first_month) - 1) & ", 1)"

    For i = 1 To months_inbetween + 1 ' no doign -1 so it includes the last month - do plus 1 so that it gets the month after 
      display_this = False

      If i = 1 Then ' then display 
        display_this = True
      ElseIf i = months_inbetween + 1 Then ' then display
        display_this = True
      ElseIf missed_count = 3 Then
        display_this = True
        missed_count = 0
      Else

        ' go thro all of the items in the array, if there is one that is in this year and month, then display, otherwise, add to missed counter
        For k = 0 To UBound(split_months_string) - 1
          If Year(CDate(split_months_string(k))) = Year((DateAdd(DateInterval.Month, i, move_date))) And Month(CDate(split_months_string(k))) = Month((DateAdd(DateInterval.Month, i, move_date))) Then
            display_this = True
          End If
        Next

        If display_this = False Then
          missed_count = missed_count + 1
        End If
      End If

      If display_this = True Then
        ticks_horizontal &= ", new Date(" & Year(DateAdd(DateInterval.Month, i, move_date)) & "," & (Month(DateAdd(DateInterval.Month, i, move_date)) - 1) & ", 1)"
      End If

    Next



  End Sub

  Public Shared Sub set_ranges_for_vsCharts(ByRef low_number As Double, ByRef high_number As Double, ByRef interval_point As Double, ByRef starting_point As Double, ByRef ending_point As Double, Optional ByRef ticks_string As String = "")
    Dim temp_amount_in As Integer = 0
    Dim seperating_interval As Double = 0.0


    If Math.Abs(high_number - low_number) > 100000000 Then
      interval_point = 50000000
    ElseIf Math.Abs(high_number - low_number) > 10000000 Then
      interval_point = 5000000
    ElseIf Math.Abs(high_number - low_number) > 1000000 Then
      interval_point = 500000
    ElseIf Math.Abs(high_number - low_number) > 500000 Then
      interval_point = 250000
    ElseIf Math.Abs(high_number - low_number) > 100000 Then
      interval_point = 125000
    ElseIf Math.Abs(high_number - low_number) > 80000 Then
      interval_point = 25000
    ElseIf Math.Abs(high_number - low_number) > 50000 Then
      interval_point = 10000
    ElseIf Math.Abs(high_number - low_number) > 30000 Then
      interval_point = 8000
    ElseIf Math.Abs(high_number - low_number) > 15000 Then
      interval_point = 5000
    ElseIf Math.Abs(high_number - low_number) > 10000 Then
      interval_point = 3000
    ElseIf Math.Abs(high_number - low_number) > 8000 Then
      interval_point = 2000
    ElseIf Math.Abs(high_number - low_number) > 5000 Then
      interval_point = 1000
    ElseIf Math.Abs(high_number - low_number) > 1000 Then
      interval_point = 500
    ElseIf Math.Abs(high_number - low_number) > 500 Then
      interval_point = 100
    ElseIf Math.Abs(high_number - low_number) > 150 Then
      interval_point = 75
    ElseIf Math.Abs(high_number - low_number) > 100 Then
      interval_point = 50
    ElseIf Math.Abs(high_number - low_number) > 50 Then
      interval_point = 25
    ElseIf Math.Abs(high_number - low_number) > 25 Then
      interval_point = 5
    ElseIf Math.Abs(high_number - low_number) > 10 Then
      interval_point = 2
    End If

    seperating_interval = interval_point

    Select Case (CInt((CInt(high_number).ToString.Length + CInt(low_number).ToString.Length) / 2))

      Case 1    ' 0 - 9           ones
        ' seperating_interval = 1
        starting_point = 0
        ending_point = high_number + 1
      Case 2    ' 0 - 99          tens
        'seperating_interval = 5
        starting_point = (CLng(low_number / seperating_interval) - 1) * seperating_interval
        ending_point = (CLng(high_number / seperating_interval) + 1) * seperating_interval
      Case 3    ' 0 - 999         hundreds
        'seperating_interval = 50
        starting_point = (CLng(low_number / seperating_interval) - 1) * seperating_interval
        ending_point = (CLng(high_number / seperating_interval) + 1) * seperating_interval
      Case 4    ' 0 - 9999        thousands
        'seperating_interval = 100
        starting_point = (CLng(low_number / seperating_interval) - 1) * seperating_interval
        ending_point = (CLng(high_number / seperating_interval) + 1) * seperating_interval
      Case 5    ' 0 - 99,999      10 thousands
        'seperating_interval = 1000
        starting_point = (CLng(low_number / seperating_interval) - 1) * seperating_interval
        ending_point = (CLng(high_number / seperating_interval) + 1) * seperating_interval
      Case 6    ' 0 - 999,999     millions
        ' seperating_interval = 10000
        starting_point = (CLng(low_number / seperating_interval) - 1) * seperating_interval
        ending_point = (CLng(high_number / seperating_interval) + 1) * seperating_interval
      Case 7    ' 0 - 9,999,999   10 millions
        ' seperating_interval = 100000
        starting_point = (CLng(low_number / seperating_interval) - 1) * seperating_interval
        ending_point = (CLng(high_number / seperating_interval) + 1) * seperating_interval
      Case 8    ' 0 - 99,999,999  100 millions
        'seperating_interval = 1000000
        starting_point = (CLng(low_number / seperating_interval) - 1) * seperating_interval
        ending_point = (CLng(high_number / seperating_interval) + 1) * seperating_interval
      Case 9    ' 0 - 999,999,999  billions
        ' seperating_interval = 10000000
        starting_point = (CLng(low_number / seperating_interval) - 1) * seperating_interval
        ending_point = (CLng(high_number / seperating_interval) + 1) * seperating_interval
    End Select

    ' added these two in for a case where the high number was 44,000 and the low was 10,000.
    ' it was creating a ending_point of 56,000 and a starting_point of 0
    ' if the ending_point (56,000) - the high number (44,000) > 8,000 interval, then take 8k off of the end point
    If (ending_point - high_number) > seperating_interval Then
      ending_point = ending_point - seperating_interval
    End If

    ' if starting point (0) + seperating interval (8,000) is still less than the low number, 10,000 then take 
    If (starting_point + seperating_interval) < low_number Then
      starting_point = starting_point + seperating_interval
    End If


    If starting_point > 0 Then
      If CDbl(starting_point) > CDbl(seperating_interval) Then
        temp_amount_in = CDbl(starting_point / seperating_interval)
        starting_point = CInt(seperating_interval * temp_amount_in)
      End If
    End If

    If Trim(ticks_string) <> "" Then
      Dim i As Integer = 0
      ticks_string = ""
      ticks_string = "" & starting_point
      For i = 1 To 25
        If (starting_point + (i * interval_point)) >= ending_point Then
          ticks_string &= ", " & starting_point + (interval_point * i)
          Exit For
        Else
          ticks_string &= ", " & starting_point + (interval_point * i)
        End If
      Next
    End If



  End Sub

  Public Shared Function get_yacht_view_model_info(ByRef searchCriteria As yachtViewSelectionCriteria, ByVal bGetAllFields As Boolean) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If Not bGetAllFields Then
        sQuery.Append("SELECT DISTINCT ym_model_id, ym_motor_type, ym_category_size, ym_brand_name, ym_brand_abbrev, ym_model_name, yt_radio_call_sign, yt_yacht_name_search, ycs_seqnbr")
        sQuery.Append(" FROM Yacht_Model INNER JOIN Yacht_Category_Size WITH (NOLOCK) ON ym_category_size = ycs_category_size AND ym_motor_type = ycs_motor_type")
      Else
        sQuery.Append("SELECT * FROM Yacht_Model INNER JOIN Yacht_Category_Size WITH (NOLOCK) ON ym_category_size = ycs_category_size AND ym_motor_type = ycs_motor_type")
      End If

      sQuery.Append(" WHERE ym_brand_name <> 'JETNET'")

      If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
          If String.IsNullOrEmpty(tmpStr) Then
            tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
          Else
            tmpStr += Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
          End If
        Next

        sQuery.Append(Constants.cAndClause + "ym_model_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "ym_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Then
        sQuery.Append(Constants.cAndClause + "ym_brand_name IN ('" + searchCriteria.YachtViewCriteriaYachtBrand.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then
        sQuery.Append(Constants.cAndClause + "ym_brand_name = '" + searchCriteria.YachtViewCriteriaYachtBrand.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtCategory.Trim) Then
        sQuery.Append(Constants.cAndClause + "ym_category_size = '" + searchCriteria.YachtViewCriteriaYachtCategory.Trim + "'")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_yacht_view_model_info(ByRef searchCriteria As yachtViewSelectionCriteria, ByVal bGetAllFields As Boolean) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_yacht_view_model_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_yacht_view_model_info(ByRef searchCriteria As yachtViewSelectionCriteria, ByVal bGetAllFields As Boolean) As DataTable" + ex.Message

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

  Public Shared Function get_view_model_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetAllFields As Boolean) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim hadItem As Boolean = False

    Try

      If Not bGetAllFields Then
        sQuery.Append("SELECT amod_make_name, amod_model_name, amod_manufacturer, amod_airframe_type_code, amod_weight_class, amod_start_year, amod_end_year, amod_ser_no_prefix, amod_ser_no_start, amod_ser_no_end,")
        sQuery.Append(" amod_ser_no_suffix, amod_start_price, amod_end_price, amod_description, amod_type_code, amod_product_commercial_flag, amod_product_helicopter_flag, atype_name, ambc_name")
        sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN Aircraft_Type WITH(NOLOCK) ON amod_type_code = atype_code")
        sQuery.Append(" INNER JOIN Aircraft_Model_Body_Config WITH(NOLOCK) ON amod_body_config = ambc_type")
      Else
        sQuery.Append("SELECT * FROM Aircraft_Model WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN Aircraft_Type WITH(NOLOCK) ON amod_type_code = atype_code")
        sQuery.Append(" INNER JOIN Aircraft_Model_Body_Config WITH(NOLOCK) ON amod_body_config = ambc_type")
      End If

      sQuery.Append(" WHERE ")

      If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
          If String.IsNullOrEmpty(tmpStr) Then
            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
          Else
            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
          End If
        Next

        sQuery.Append("amod_id IN (" + tmpStr.Trim + ")")
        hadItem = True
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        hadItem = True
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
        hadItem = True
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
        hadItem = True
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append("amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        hadItem = True
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append("amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        hadItem = True
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append("amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
        hadItem = True
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(IIf(hadItem, Constants.cAndClause, " ") + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
          hadItem = True
        Else
          sQuery.Append(IIf(hadItem, Constants.cAndClause, " ") + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
          hadItem = True
        End If
      End If

      If IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(IIf(hadItem, Constants.cAndClause, " ") + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), True, True))
        Else
          sQuery.Append(IIf(hadItem, Constants.cAndClause, " ") + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, True, True))
        End If

      End If

      sQuery.Append(" ORDER BY amod_make_name, amod_model_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Get_Model_Info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Model_Info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Model_Info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Shared Function get_fleet_market_summary_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bUseCharterQuery As Boolean, Optional ByVal number_of_months_divide As Long = 0) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If Not bUseCharterQuery Then

        sQuery.Append("SELECT ac_id, ac_country_of_registration, ac_ownership_type, ac_lifecycle_stage, ac_forsale_flag, ac_exclusive_flag, ac_airframe_tot_Hrs, ")
        sQuery.Append(" ac_lease_flag, ac_asking, ac_asking_price, 0 as sold_price, ac_list_date, ac_mfr_year, DATEDIFF(d,ac_list_date,getdate()) AS daysonmarket, ac_airframe_tot_landings ")

        If number_of_months_divide > 0 Then
          ' -- GET SALES PER MONTH FOR LAST 6 MONTHS - DIVIDE THIS INTO THE TOTAL FOR SALE TO GET ABSORPTION RATE.
          sQuery.Append(", (SELECT COUNT(J1.journ_id)")
          sQuery.Append(" FROM Journal AS J1 WITH (NOLOCK)")
          sQuery.Append(" INNER JOIN Aircraft AS A1 WITH (NOLOCK) ON A1.ac_id = J1.journ_ac_id AND A1.ac_journ_id = J1.journ_id")

          If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
            If searchCriteria.ViewCriteriaAmodIDArray.Length > 0 Then
              sQuery.Append("  WHERE A1.ac_amod_id in ( ")

              For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                If x = 0 Then
                  sQuery.Append(searchCriteria.ViewCriteriaAmodIDArray(x))
                Else
                  sQuery.Append(Constants.cCommaDelim & searchCriteria.ViewCriteriaAmodIDArray(x))
                End If
              Next
              sQuery.Append("  ) ")

            Else
              sQuery.Append("  WHERE A1.ac_amod_id = Aircraft_Flat.amod_id ")
            End If
          Else
            sQuery.Append("  WHERE A1.ac_amod_id = Aircraft_Flat.amod_id ")
          End If




          '-- Whole Sale, Used, Retail Only Last Year
          sQuery.Append(" AND (J1.journ_subcat_code_part1 IN ('WS'))  ")
          sQuery.Append(" AND (J1.journ_subcat_code_part2 NOT IN ('CC','DS','FY','MF'))  ")
          sQuery.Append(" AND (J1.journ_subcat_code_part3 NOT IN ('CC','DB','DS','FI','FY','IT','LS','MF','RE','RM')) ")
          sQuery.Append(" AND (J1.journ_internal_trans_flag = 'N') ")
          sQuery.Append(" AND (J1.journ_newac_flag = 'N')")
          sQuery.Append(" AND (J1.journ_subcategory_code NOT LIKE '%CORR%')  ")
          sQuery.Append(" AND (J1.journ_date >= DATEADD(MONTH,-" & number_of_months_divide & ",GETDATE()))")
          '    sql += " )/" & number_of_months_divide & " As SalesPerMonth "

          sQuery.Append("  and")
          sQuery.Append(" ( (J1.journ_date > (select top 1 journ_date from View_Aircraft_History_Flat af2 with (NOLOCK) ")
          sQuery.Append(" Where af2.ac_id = A1.ac_id and (af2.journ_newac_flag = 'Y' or ac_previously_owned_flag = 'Y') ")
          sQuery.Append(" order by af2.journ_date asc))")
          sQuery.Append(" or ")
          sQuery.Append(" (J1.journ_date = (select top 1 journ_date from View_Aircraft_History_Flat af2 with (NOLOCK) ")
          sQuery.Append(" where af2.ac_id = A1.ac_id And af2.ac_journ_id > Aircraft_Flat.ac_journ_id ")
          sQuery.Append(" and (af2.journ_newac_flag = 'Y' or ac_previously_owned_flag = 'Y') ")
          sQuery.Append(" order by af2.journ_date asc))")
          sQuery.Append(" )")

          sQuery.Append(" ) As SalesPerTimeframe ")
        End If

        sQuery.Append(" FROM Aircraft_Flat WITH(NOLOCK)")
        sQuery.Append(" WHERE ac_journ_id = 0")

        If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
          Dim tmpStr As String = ""

          ' flatten out amodID array ...
          For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
            If String.IsNullOrEmpty(tmpStr) Then
              tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
            Else
              tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
            End If
          Next

          sQuery.Append(Constants.cAndClause + "amod_id IN (" + tmpStr.Trim + ")")
        ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
        ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
        ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
        End If

        Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
          Case Constants.VIEW_EXECUTIVE
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
          Case Constants.VIEW_JETS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
          Case Constants.VIEW_TURBOPROPS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
          Case Constants.VIEW_PISTONS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
          Case Constants.VIEW_HELICOPTERS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
        End Select

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
          If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
            sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
          End If
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

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
        End If

      Else

        sQuery.Append("SELECT ac_id, ac_country_of_registration, ac_ownership_type, ac_lifecycle_stage, ac_forsale_flag, ac_exclusive_flag,ac_airframe_tot_Hrs,")
        sQuery.Append(" ac_lease_flag, ac_asking, ac_asking_price, ac_list_date, ac_mfr_year, datediff(day, ac_list_date,getdate()) AS daysonmarket, ac_airframe_tot_landings ")
        sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON amod_id = ac_amod_id WHERE ac_journ_id = 0")


        If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
          Dim tmpStr As String = ""

          ' flatten out amodID array ...
          For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
            If String.IsNullOrEmpty(tmpStr) Then
              tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
            Else
              tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
            End If
          Next

          sQuery.Append(Constants.cAndClause + "amod_id IN (" + tmpStr.Trim + ")")
        ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
        ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
        ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
        End If

        Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
          Case Constants.VIEW_EXECUTIVE
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
          Case Constants.VIEW_JETS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
          Case Constants.VIEW_TURBOPROPS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
          Case Constants.VIEW_PISTONS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
          Case Constants.VIEW_HELICOPTERS
            sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
        End Select

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

        sQuery.Append(" AND EXISTS (SELECT NULL FROM aircraft_reference WITH(NOLOCK)")
        sQuery.Append(" WHERE cref_ac_id = ac_id AND cref_journ_id = ac_journ_id")
        sQuery.Append(" AND (cref_contact_type IN ('94','33') OR cref_business_type = 'CH'))")

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
          If searchCriteria.ViewCriteriaWeightClass.Contains(Constants.cCommaDelim) Then
            sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
          End If
        End If

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fleet_market_summary_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bUseCharterQuery As Boolean) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_fleet_market_summary_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_fleet_market_summary_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bUseCharterQuery As Boolean) As DataTable " + ex.Message

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

  Public Shared Sub FindDataAsOfDate()

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim asOfDateMax As Date = Nothing
    Dim asOfDate As Date = Nothing

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 120

      sQuery.Append("SELECT MAX(ac_upd_date) AS MaxDate FROM Aircraft WITH(NOLOCK)")

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then
        SqlReader.Read()
        If Not IsDBNull(SqlReader.Item("MaxDate")) Then
          If IsDate(SqlReader.Item("MaxDate").ToString) Then
            asOfDateMax = CDate(FormatDateTime(SqlReader.Item("MaxDate").ToString, vbShortDate))
          End If
        End If
        SqlReader.Close()
      End If

      sQuery = New StringBuilder
      sQuery.Append("SELECT MAX(comp_upd_date) AS MaxDate FROM Company WITH(NOLOCK)")

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then
        SqlReader.Read()
        If Not IsDBNull(SqlReader.Item("MaxDate")) Then
          If IsDate(SqlReader.Item("MaxDate").ToString) Then
            asOfDate = CDate(FormatDateTime(SqlReader.Item("MaxDate").ToString, vbShortDate))
          End If
        End If
        SqlReader.Close()
      End If

      If asOfDate > asOfDateMax Then asOfDateMax = asOfDate

      sQuery = New StringBuilder
      sQuery.Append("SELECT MAX(contact_update_date) AS MaxDate FROM Contact WITH(NOLOCK)")

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then
        SqlReader.Read()
        If Not IsDBNull(SqlReader.Item("MaxDate")) Then
          If IsDate(SqlReader.Item("MaxDate").ToString) Then
            asOfDate = CDate(FormatDateTime(SqlReader.Item("MaxDate").ToString, vbShortDate))
          End If
        End If
        SqlReader.Close()
      End If

      If asOfDate > asOfDateMax Then asOfDateMax = asOfDate

      sQuery = New StringBuilder
      sQuery.Append("SELECT MAX(priorev_entry_date) AS MaxDate FROM Priority_Events WITH(NOLOCK)")

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then
        SqlReader.Read()
        If Not IsDBNull(SqlReader.Item("MaxDate")) Then
          If IsDate(SqlReader.Item("MaxDate").ToString) Then
            asOfDate = CDate(FormatDateTime(SqlReader.Item("MaxDate").ToString, vbShortDate))
          End If
        End If
        SqlReader.Close()
      End If

      If asOfDate > asOfDateMax Then asOfDateMax = asOfDate

      If IsDate(asOfDateMax) And Not String.IsNullOrEmpty(asOfDateMax.ToString.Trim) Then
        HttpContext.Current.Session.Item("DataAsOfDate") = asOfDateMax
      Else
        HttpContext.Current.Session.Item("DataAsOfDate") = Now()
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in FindDataAsOfDate()</b><br />" + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Shared Function isDealerCompany(ByVal inCompanyID As Long, ByVal inJournalID As Long, Optional ByVal ac_id As Long = 0) As Boolean

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sQuery As New StringBuilder

    Dim bResult As Boolean = False

    sQuery.Append("SELECT comp_account_type FROM Company WITH(NOLOCK) ")

    If ac_id > 0 Then
      sQuery.Append(" inner join Aircraft_Reference with (NOLOCK) on cref_ac_id = " & ac_id & " and cref_journ_id = 0 and cref_comp_id = comp_id  ")    ' and cref_contact_type = '97' 
    End If

    sQuery.Append(" WHERE comp_id = " + inCompanyID.ToString + " And comp_journ_id = " + inJournalID.ToString)

    If inJournalID = 0 Then
      sQuery.Append(" And comp_active_flag = 'Y'")
    End If

    sQuery.Append(" AND comp_hide_flag = 'N' ")

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      If SqlReader.HasRows Then

        SqlReader.Read()

        ' if there is an AC ID then 
        If ac_id > 0 Then   ' IF THERE IS ANY COMP ACCOUNT TYPE THEN CONSIDER IT RELATED 
          If Not IsDBNull(SqlReader("comp_account_type")) Then
            If Not String.IsNullOrEmpty(SqlReader("comp_account_type").ToString.Trim) Then
              bResult = True
            End If
          End If
        Else
          If Not IsDBNull(SqlReader("comp_account_type")) Then
            If Not String.IsNullOrEmpty(SqlReader("comp_account_type").ToString.Trim) Then
              If SqlReader("comp_account_type").ToString.Trim.ToUpper.Contains("DB") Then
                bResult = True
              End If
            End If
          End If
        End If




        SqlReader.Close()

      End If

    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

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

  Public Shared Function returnUserFolders(ByVal bJustTabs As Boolean, ByVal bIsAdmin As Boolean, Optional ByVal sTabName As String = "", Optional ByVal nItemID As Long = 0) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT " + IIf(bJustTabs, "DISTINCT cfttpe_name", "cfolder_id, cfttpe_name, cfolder_name, contact_first_name, contact_last_name, cfolder_method, cfolder_share"))
      sQuery.Append(" FROM Client_Folder WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Client_Folder_Type WITH (NOLOCK) ON cfolder_cftype_id = cftype_id")
      sQuery.Append(" INNER JOIN Subscription_Install WITH (NOLOCK) ON subins_sub_id = cfolder_sub_id AND subins_login = cfolder_login AND subins_seq_no = cfolder_seq_no")
      sQuery.Append(" INNER JOIN Contact WITH (NOLOCK) ON subins_contact_id = contact_id AND contact_journ_id = 0")
      sQuery.Append(" WHERE")

      If nItemID > 0 Then
        sQuery.Append(" cfolder_id = " + nItemID.ToString.Trim + Constants.cAndClause)
      Else


        If Not String.IsNullOrEmpty(sTabName.Trim) And Not sTabName.ToLower.Contains("all") Then
          sQuery.Append(" lower(cfttpe_name) = '" + sTabName.Trim.ToLower + "'" + Constants.cAndClause)
        End If

        If bIsAdmin Then
          sQuery.Append(" ( cfolder_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + " )")
        Else
          sQuery.Append(" ( cfolder_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
          sQuery.Append(" AND cfolder_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "'")
          sQuery.Append(" AND cfolder_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + " )")
        End If

        If bJustTabs Then
          sQuery.Append(" ORDER BY cfttpe_name")
        Else
          sQuery.Append(" ORDER BY cfttpe_name, cfolder_name")
        End If

      End If

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonEvo.vb.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnUserFolders load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnUserFolders(ByVal bJustTabs As Boolean, ByVal bIsAdmin As Boolean, Optional ByVal sTabName As String = "", Optional ByVal nItemID As Long = 0) As DataTable " + ex.Message

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

  Public Shared Function returnUserTemplates(ByVal bJustTabs As Boolean, ByVal bIsAdmin As Boolean, Optional ByVal sTabName As String = "", Optional ByVal nItemID As Long = 0) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try


      sQuery.Append("SELECT " + IIf(bJustTabs, "DISTINCT sise_tab", "sise_id, sise_tab, sise_subject, contact_first_name, contact_last_name, sise_export_type, sise_share_flag"))
      sQuery.Append(" FROM Subscription_Install_Saved_Exports WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Subscription_Install WITH (NOLOCK) ON subins_sub_id = sise_sub_id AND subins_login = sise_login AND subins_seq_no = sise_seq_no")
      sQuery.Append(" INNER JOIN Contact WITH (NOLOCK) ON subins_contact_id = contact_id AND contact_journ_id = 0")
      sQuery.Append(" WHERE")

      If nItemID > 0 Then
        sQuery.Append(" sise_id = " + nItemID.ToString.Trim + Constants.cAndClause)
      Else

        If Not String.IsNullOrEmpty(sTabName.Trim) And Not sTabName.ToLower.Contains("all") Then
          sQuery.Append(" lower(sise_tab) = '" + sTabName.Trim.ToLower + "'" + Constants.cAndClause)
        End If

        If bIsAdmin Then
          sQuery.Append(" ( sise_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + " )")
        Else
          sQuery.Append(" ( sise_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
          sQuery.Append(" AND sise_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "'")
          sQuery.Append(" AND sise_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + " )")
        End If

        If bJustTabs Then
          sQuery.Append(" ORDER BY sise_tab")
        Else
          sQuery.Append(" ORDER BY sise_tab, sise_subject")
        End If

      End If

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonEvo.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnUserTemplates load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnUserTemplates(ByVal bJustTabs As Boolean, ByVal bIsAdmin As Boolean, Optional ByVal sTabName As String = "", Optional ByVal nItemID As Long = 0) As DataTable " + ex.Message

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

  Public Shared Function returnAirportFolderName(ByVal bJustDefult As Boolean, Optional ByVal bShared As Boolean = False, Optional ByVal nFolderType As Integer = 0, Optional ByVal nItemID As Long = 0) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT cfolder_id, cfttpe_name, cfolder_name, contact_first_name, contact_last_name, cfolder_method, cfolder_share, cfolder_default_flag")
      sQuery.Append(" FROM Client_Folder WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Client_Folder_Type WITH (NOLOCK) ON cfolder_cftype_id = cftype_id")
      sQuery.Append(" INNER JOIN Subscription_Install WITH (NOLOCK) ON subins_sub_id = cfolder_sub_id AND subins_login = cfolder_login AND subins_seq_no = cfolder_seq_no")
      sQuery.Append(" INNER JOIN Contact WITH (NOLOCK) ON subins_contact_id = contact_id AND contact_journ_id = 0")
      sQuery.Append(" WHERE")

      If nItemID > 0 Then
        sQuery.Append(" cfolder_id = " + nItemID.ToString.Trim + Constants.cAndClause) 'cfolder_cftype_id
      Else
        sQuery.Append(" ( cfolder_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
        sQuery.Append(Constants.cAndClause + "cfolder_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "'")
        sQuery.Append(Constants.cAndClause + "cfolder_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + " )")
      End If

      sQuery.Append(IIf(nFolderType > 0, Constants.cAndClause + "cfolder_cftype_id = " + nFolderType.ToString, ""))
      sQuery.Append(IIf(bJustDefult, Constants.cAndClause + "cfolder_default_flag = 'Y'", ""))
      sQuery.Append(IIf(bShared, Constants.cAndClause + "cfolder_share = 'Y'", ""))

      sQuery.Append(" ORDER BY cfolder_name")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonEvo.vb.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnUserFolders load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnUserFolders(ByVal bJustTabs As Boolean, ByVal bIsAdmin As Boolean, Optional ByVal sTabName As String = "", Optional ByVal nItemID As Long = 0) As DataTable " + ex.Message

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

  Public Shared Function returnAirportFolderContents(ByVal nFolderID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT aport_name, aport_city, aport_state, aport_country, aport_iata_code, aport_icao_code, aport_id")
      sQuery.Append(" FROM Client_Folder_Index WITH(NOLOCK) INNER JOIN Airport WITH(NOLOCK) ON cfoldind_jetnet_aport_id = aport_id")
      sQuery.Append(" WHERE ( cfoldind_cfolder_id = " + nFolderID.ToString + " )")
      sQuery.Append(" ORDER BY aport_name")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonEvo.vb.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnAirportFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnAirportFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

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

  Public Shared Sub model_operational_trends_graph(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, ByRef valuePDF As Boolean, ByRef PER_MONTH As System.Web.UI.DataVisualization.Charting.Chart)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable
    Dim high_number As Integer = 0
    Dim low_number As Integer = 100000000
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1
    Dim has_data As Boolean = False

    Try

      results_table = model_operational_trends(amod_id)

      ' check to see if there is something for last month, if there is not, then go back a month
      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then
          For Each r As DataRow In results_table.Rows
            For x As Integer = 5 To 5
              If Not IsDBNull(r.Item("YEAR" + x.ToString + "VAL")) Then
                has_data = True
              End If
            Next
          Next
        End If
      End If


      If has_data = False Then
        results_table.Clear()
        results_table = model_operational_trends(amod_id, 2)
      End If


      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If Not IsNothing(PER_MONTH) Then
            PER_MONTH.Series.Clear()
            PER_MONTH.Series.Add("PER_MONTH").ChartType = UI.DataVisualization.Charting.SeriesChartType.Column
            PER_MONTH.ChartAreas("ChartArea1").AxisY.Title = "# of Aircraft"

            PER_MONTH.Series("PER_MONTH").Color = Drawing.Color.Blue
            PER_MONTH.Series("PER_MONTH").BorderWidth = 1
            PER_MONTH.Series("PER_MONTH").MarkerSize = 5
            PER_MONTH.Series("PER_MONTH").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
            PER_MONTH.BorderlineWidth = 10
            PER_MONTH.Series("PER_MONTH").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
          End If

          If graphID = 33 Then
            scriptOut.Append(" data33.addColumn('string', 'Year');" + vbCrLf)
            scriptOut.Append(" data33.addColumn('number', 'In Operation');" + vbCrLf)

            scriptOut.Append(" data33.addRows([" + vbCrLf)
          Else
            scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
            scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)
            scriptOut.Append(" data.addColumn('string', 'Year');" + vbCrLf)
            scriptOut.Append(" data.addColumn('number', 'In Operation');" + vbCrLf)

            scriptOut.Append(" data.addRows([" + vbCrLf)
          End If

          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  





          For Each r As DataRow In results_table.Rows

            For x As Integer = 1 To 5

              scriptOut.Append(IIf(x > 1, ",['", "['"))



              If Not IsDBNull(r.Item("YEAR" + x.ToString + "YEAR")) Then
                If Not String.IsNullOrEmpty(r.Item("YEAR" + x.ToString + "YEAR").ToString.Trim) Then

                  If CLng(r.Item("YEAR" + x.ToString + "YEAR").ToString) > 0 Then
                    scriptOut.Append(r.Item("YEAR" + x.ToString + "YEAR").ToString + "'")
                  Else
                    scriptOut.Append("0'")
                  End If

                Else
                  scriptOut.Append("0'")
                End If

              Else
                scriptOut.Append("0'")
              End If

              If Not IsDBNull(r.Item("YEAR" + x.ToString + "VAL")) Then
                If Not String.IsNullOrEmpty(r.Item("YEAR" + x.ToString + "VAL").ToString.Trim) Then

                  If CLng(r.Item("YEAR" + x.ToString + "VAL").ToString) > 0 Then
                    scriptOut.Append("," + r.Item("YEAR" + x.ToString + "VAL").ToString + "]")
                  Else
                    scriptOut.Append(",0]")
                  End If

                  If Not IsNothing(PER_MONTH) Then
                    If CDbl(r.Item("YEAR" + x.ToString + "VAL")) > high_number Then
                      high_number = r.Item("YEAR" + x.ToString + "VAL")
                    End If
                    If CDbl(r.Item("YEAR" + x.ToString + "VAL")) < low_number Then
                      low_number = r.Item("YEAR" + x.ToString + "VAL")
                    End If


                    PER_MONTH.Series("PER_MONTH").Points.AddXY(r.Item("YEAR" + x.ToString + "YEAR").ToString, r.Item("YEAR" + x.ToString + "VAL"))
                  End If


                Else
                  scriptOut.Append(",0]")
                  If Not IsNothing(PER_MONTH) Then
                    PER_MONTH.Series("PER_MONTH").Points.AddXY(r.Item("YEAR" + x.ToString + "YEAR").ToString, "0")
                  End If
                End If

              Else
                scriptOut.Append(",0]")
                If Not IsNothing(PER_MONTH) Then
                  PER_MONTH.Series("PER_MONTH").Points.AddXY(r.Item("YEAR" + x.ToString + "YEAR").ToString, "0")
                End If
              End If

            Next

          Next


          If graphID = 33 Then

          Else
            scriptOut.Append("]);" + vbCrLf)

            scriptOut.Append("var options = { " + vbCrLf)
            scriptOut.Append("  chartArea:{width:'80%',height:'75%'}," + vbCrLf)
            scriptOut.Append("  hAxis: { title: 'Year'," + vbCrLf)
            scriptOut.Append("           textStyle: { color: 'black', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
            scriptOut.Append("           titleTextStyle: { color: 'black', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
            scriptOut.Append("         }," + vbCrLf)
            scriptOut.Append("  vAxis: { title: 'In Operation'," + vbCrLf)
            scriptOut.Append("           textStyle: { color: 'black', fontSize: 14, bold: true }," + vbCrLf)
            scriptOut.Append("           titleTextStyle: { color: 'black', fontSize: 16, bold: true }" + vbCrLf)
            scriptOut.Append("        }," + vbCrLf)
            scriptOut.Append("  smoothLine:true," + vbCrLf)
            scriptOut.Append("  legend:'none'," + vbCrLf)
            scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange']" + vbCrLf)
            scriptOut.Append("};" + vbCrLf)


            scriptOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
            scriptOut.Append(" chart.draw(data, options);" + vbCrLf)

            If Not IsNothing(PER_MONTH) Then

              If high_number > 500 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = 800
              ElseIf high_number > 400 And high_number > 0 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = 500
              ElseIf high_number > 300 And high_number > 0 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = 400
              ElseIf high_number > 200 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = 300
              ElseIf high_number > 100 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = 200
              Else
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = 500
              End If

              If high_number - low_number > 500 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Interval = 200
              ElseIf high_number - low_number > 300 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Interval = 100
              ElseIf high_number - low_number > 200 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Interval = 50
              ElseIf high_number - low_number > 100 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Interval = 25
              Else
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Interval = 20
              End If


              If low_number > 500 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 500
              ElseIf low_number > 400 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 400
              ElseIf low_number > 300 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 300
              ElseIf low_number > 200 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 200
              ElseIf low_number > 100 Then
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 100
              Else
                PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 0
              End If



              PER_MONTH.ChartAreas("ChartArea1").AxisX.Interval = 1
            End If




          End If


          If graphID = 33 Then

          ElseIf valuePDF Then
            scriptOut.Append("document.getElementById('ctl00_ContentPlaceHolder1_png20').innerHTML = '<img src=""' + chart.getImageURI() + '"" >'" + vbCrLf)
          End If

          If graphID = 33 Then
          Else
            scriptOut.Append("}" + vbCrLf)
          End If


        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""modeloperationalTrendsTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>In Operation Aircraft (Last 5 Years)</strong>")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:295px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""modeloperationalTrendsTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>In Operation Aircraft (Last 5 Years)</strong>")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No In Operation Aircraft Data at this time, for this Make/Model ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_operational_trends_graph(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Shared Function model_operational_trends(ByVal amod_id As Long, Optional ByVal move_back_months As Integer = 0) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()
    Dim temp_date As String = ""

    Try

      If move_back_months > 0 Then
        temp_date = FormatDateTime(DateAdd(DateInterval.Month, -move_back_months, Date.Now()), DateFormat.ShortDate)
      Else
        temp_date = FormatDateTime(DateAdd(DateInterval.Month, -1, Date.Now()), DateFormat.ShortDate)
      End If

      sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id,")
      sQuery.Append(" YEAR('" & temp_date & "')-4 as YEAR1YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR('" & temp_date & "')-4 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR1VAL,")
      sQuery.Append(" YEAR('" & temp_date & "')-3 as YEAR2YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR('" & temp_date & "')-3 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR2VAL,")
      sQuery.Append(" YEAR('" & temp_date & "')-2 as YEAR3YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR('" & temp_date & "')-2 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR3VAL,")
      sQuery.Append(" YEAR('" & temp_date & "')-1 as YEAR4YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR('" & temp_date & "')-1 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR4VAL,")
      sQuery.Append(" YEAR('" & temp_date & "') as YEAR5YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR('" & temp_date & "') AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR5VAL")
      sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK)")
      sQuery.Append(" WHERE amod_id = " + amod_id.ToString)



      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonEvo.vb", sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_user_interest(ByVal amod_id As Long) As DataTable: " + ex.Message

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

  Public Shared Function model_utilization_percentage(ByVal amod_id As Long) As String

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Dim nFAA_ac_count As Long = 0
    Dim nInOp_ac_count As Long = 0
    Dim nAnswer As Long = 0

    Dim htmlOut As New StringBuilder

    Try

      sQuery.Append(" SELECT COUNT(distinct ac_id) AS tcount,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year = YEAR(getdate()) AND mtrend_month = 1 AND mtrend_amod_id = ac_amod_id) AS INOP")
      sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ffd_ac_id = ac_id AND ac_journ_id = 0")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON amod_id = ac_amod_id")
      sQuery.Append(" WHERE ffd_date >= GETDATE()-730 AND ac_amod_id = " + amod_id.ToString) ' GETDATE()-365        
      sQuery.Append(" GROUP BY ac_amod_id")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonEvo.vb", sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      If temptable.Rows.Count > 0 Then

        For Each r As DataRow In temptable.Rows

          If CLng(r.Item("tcount").ToString) > 0 Then

            nFAA_ac_count = CLng(r.Item("tcount").ToString)

          End If

          If CLng(r.Item("INOP").ToString) > 0 Then

            nInOp_ac_count = CLng(r.Item("INOP").ToString)

          End If

        Next

      End If

      If nInOp_ac_count > 0 Then
        nAnswer = FormatNumber(System.Math.Round(CDbl((nFAA_ac_count / nInOp_ac_count) * 100), 2), 1, False, False, False)
      End If

      htmlOut.Append("Based on flight activity of <strong>" + nAnswer.ToString + "%</strong> of in operation aircraft.")

    Catch ex As Exception

      Return ""
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_utilization_percentage(ByVal amod_id As Long) As Long : " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return htmlOut.ToString

    htmlOut = Nothing

  End Function

  'Public Shared Function DisplayMarketStatusBlocks(ByVal localCriteria As viewSelectionCriteriaClass, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal wordReport As Boolean, ByVal ReportColor As String, ByVal wordWidth As String) As String
  '  Dim ReturnString As String = ""
  '  Dim strHTML As String = ""
  '  Dim GetMarketStatus As String = ""
  '  Dim ViewPDFRef As New viewtopdf_aspx
  '  Dim forSaleAvLow, forSaleAvg, ForSaleAvgHigh, mfrLowfs, mfrAvgFs, mfrHighFs, lowDays, highDays, disDays, afttLowFs, afttAvgFs, afttHighFS As Double
  '  Dim totalInOpCount, ACForSale, acExclusiveCount, acLease, AcExclusiveSale, perforSale, perInOp As Double


  '  'List of things this function needs to do:
  '  '0.) Query information.
  '  '1.) Market Summary Block.
  '  '2.) Absorption Rate Gauge
  '  '3.) Market Composition Block.
  '  '4.) Average Asking.


  '  'GetFleetInfo(inModelID, False)

  '  ReturnString += "<div class=""Simplistic marketSummary""> "
  '  ' ReturnString += "<span class='mainHeading'><strong>MakeModelName</strong> Market Trends</span>"
  '  ReturnString += "<div class=""Box marginTop"">"

  '  'Block 1.)
  '  ReturnString += CreateBlockOne(ReportColor, totalInOpCount, ACForSale, acExclusiveCount, acLease, AcExclusiveSale, perforSale, perInOp)
  '  'End block one

  '  ReturnString += "</div>"


  '  ReturnString += "<div class=""Box marginTop"">"

  '  'Block 2.)
  '  ReturnString += CreateBlockTwo(wordReport, wordWidth, ReportColor, forSaleAvLow, forSaleAvg, ForSaleAvgHigh, mfrLowfs, mfrAvgFs, mfrHighFs, lowDays, highDays, disDays, afttLowFs, afttAvgFs, afttHighFS)
  '  'End Block 2.

  '  ReturnString += "</div>"
  '  ReturnString += "<br clear=""all"" />"

  '  ReturnString += "<div class=""Box gaugeImage"">"

  '  'Create Gauge 1
  '  ReturnString += "<span>ABSORPTION RATE</span>"
  '  ReturnString += "<img src='' width='320' align='center'  />"
  '  'End Gauge 1

  '  ReturnString += "</div>"

  '  ReturnString += "<div class=""Box gaugeImage"">"

  '  'Create Gauge 2
  '  ReturnString += "<span>AVERAGE ASKING</span>"
  '  ReturnString += "<img src='' width='320' align='center'  />"
  '  'End Gauge 2

  '  ReturnString += "</div>"


  '  ReturnString += "</td>"
  '  ReturnString += "</tr>"


  '  Return ReturnString

  'End Function
  'Public Shared Function CreateBlockTwo(ByVal WordReport As Boolean, ByVal WordWidth As String, ByVal ReportColor As String, ByVal forSaleAvLow As Double, ByVal forSaleAvg As Double, ByVal ForSaleAvgHigh As Double, ByVal mfrLowfs As Double, ByVal mfrAvgFs As Double, ByVal mfrHighFs As Double, ByVal lowDays As Double, ByVal highDays As Double, ByVal disDays As Double, ByVal afttLowFs As Double, ByVal afttAvgFs As Double, ByVal afttHighFS As Double)
  '  Dim returnString As String = ""
  '  If WordReport Then
  '    returnString += "<table id='lifeCycleTable'  cellspacing='0' cellpadding='0' width='" & WordWidth & "' class='formatTable " & ReportColor & "'><thead>" & vbCrLf
  '  Else
  '    returnString += "<table id='lifeCycleTable'  cellspacing='0' cellpadding='0' class='formatTable " & ReportColor & " large' width='100%'><thead>" & vbCrLf
  '  End If

  '  returnString += "<tr><th valign='top' align='left' class='upperCase'>Market<br/>Composition&nbsp;</th><th valign='top' class='upperCase right'>Low&nbsp;</th><th valign='top' class='upperCase right'>Avg&nbsp;</th><th valign='top' class='upperCase right'>High&nbsp;</th></tr>" & vbCrLf
  '  returnString += "</thead><tbody>"
  '  returnString += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>Asking Price:&nbsp;</td>"
  '  returnString += "<td align='right'>" & forSaleAvLow & "</td>"
  '  returnString += "<td align='right'>$" & FormatNumber(forSaleAvg, 0) & "k</td>"  '  " &  & "
  '  returnString += "<td align='right'>" & ForSaleAvgHigh & "</td>"
  '  returnString += "</tr>"

  '  returnString += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>MFR Year:&nbsp;</td>"
  '  returnString += "<td align='right'>" & mfrLowfs & "</td>"
  '  returnString += "<td align='right'>" & mfrAvgFs & "</td>"
  '  returnString += "<td align='right'>" & mfrHighFs & "</td>"
  '  returnString += "</tr>"

  '  returnString += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>Days on Market:&nbsp;</td>"
  '  returnString += "<td align='right'>" & FormatNumber(lowDays, 0) & "</td>"
  '  returnString += "<td align='right'>" & FormatNumber(disDays, 0) & "</td>"
  '  returnString += "<td align='right'>" & FormatNumber(highDays, 0) & "</td>"
  '  returnString += "</tr>"

  '  returnString += "<tr><td valign='top' align='left' nowrap='nowrap' class='upperCase'>Airframe Time:&nbsp;</td>"
  '  returnString += "<td align='right'>" & FormatNumber(afttLowFs, 0) & "</td>"
  '  returnString += "<td align='right'>" & FormatNumber(afttAvgFs, 0) & "</td>"
  '  returnString += "<td align='right'>" & FormatNumber(afttHighFS, 0) & "</td>"
  '  returnString += "</tr>"


  '  returnString += "</tbody></table>"

  '  Return returnString
  'End Function
  'Public Shared Function CreateBlockOne(ByVal reportColor As String, ByVal totalInOpCount As Double, ByVal ACForSale As Double, ByVal acExclusiveCount As Double, ByVal acLease As Double, ByVal AcExclusiveSale As Double, ByVal perForSale As Double, ByVal perInOp As Double)
  '  Dim returnString As String = ""

  '  returnString += "<table id='marketPlaceStatusTable' cellspacing='0' cellpadding='0' class='formatTable " & reportColor & "  large' width='100%'><thead>"
  '  returnString += "<tr><th valign='top' align='center' colspan='2' class='center upperCase'>Market Summary</th></tr>" & vbCrLf
  '  returnString += "</thead><tbody>"

  '  If CLng(totalInOpCount) > 0 Then
  '    returnString += "<tr><td valign='top' align='left' class='upperCase'>In Operation:&nbsp;</td><td align='left'>" & FormatNumber(totalInOpCount, 0, True, False, True) & "</td></tr>" & vbCrLf
  '  Else
  '    returnString += "<tr><td valign='top' align='left' class='upperCase'>In Operation:&nbsp;</td><td align='left'>0</td></tr>" & vbCrLf
  '  End If

  '  If CLng(ACForSale) > 0 Then
  '    If Not HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
  '      returnString += "<tr><td valign='top' align='left' class='upperCase'>For Sale:&nbsp;</td><td align='left'>" & FormatNumber(ACForSale, 0, True, False, True) & "&nbsp;<span class='tiny'>(" & FormatNumber(perForSale, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
  '    Else
  '      returnString += "<tr><td valign='top' align='left' class='upperCase'>For Sale:&nbsp;</td><td align='left'>" & FormatNumber(ACForSale, 0, True, False, True) & " &nbsp;<span class='tiny'>(" & FormatNumber(perForSale, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
  '    End If
  '  Else
  '    returnString += "<tr><td valign='top' align='left' class='upperCase'>For Sale:&nbsp;</td><td align='left'>0 <span class='tiny'>(0% of In Operation)</span></td></tr>" & vbCrLf
  '  End If

  '  If Not HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
  '    ' THIS IS FOR ON EXCLUSIVE %
  '    If CLng(acexclusivesale) > 0 Then
  '      returnString += "<tr><td valign='top' align='left' class='upperCase'>On Exclusive:&nbsp;</td><td align='left'>" & FormatNumber(acexclusivesale, 0, True, False, True) & " <span class='tiny'>(" & FormatNumber(PerForSale, 1) & "% of For Sale)</span></td></tr>" & vbCrLf
  '    Else
  '      returnString += "<tr><td valign='top' align='left' class='upperCase'>On Exclusive:&nbsp;</td><td align='left'>(0% of For Sale on Exclusive)</span></td></tr>" & vbCrLf
  '    End If

  '  End If

  '  If CLng(acLease) > 0 Then
  '    returnString += "<tr><td valign='top' align='left' class='upperCase'>Leased:&nbsp;</td><td align='left'>" & FormatNumber(acLease, 0, True, False, True) & " <span class='tiny'>(" & FormatNumber(perInOp, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
  '  Else
  '    returnString += "<tr><td valign='top' align='left' class='upperCase'>Leased:&nbsp;</td><td align='left'>0 <span class='tiny'>(0% of In Operation)</span></td></tr>" & vbCrLf
  '  End If

  '  returnString += "</tbody></table>"
  '  Return returnString
  'End Function

  Public Shared Function getViewCustomerNotesDataTable(ByVal inCompanyID As Long, limit As Integer) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT " + IIf(limit > 0, "TOP " + limit.ToString + " ", "") + "* FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "View_Customer_Notes WITH(NOLOCK)")
      sQuery.Append(" WHERE journ_comp_id = " + inCompanyID.ToString + " ORDER BY journ_date DESC")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
      Return Nothing

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

End Class
