Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Aircraft_Reference
' Purpose: Configure all properties and methods for clsClient_Aircraft_Reference
' Parameters: None
' Return: None
' Change Log
'           3/19/2010 - Created By: Tom Jones


Public Class clsClient_Aircraft_Reference



  '**************************************************
  ' Private variable declarations
  '**************************************************
  Private intcliacref_id As Integer
  Private intcliacref_cliac_id As Integer
  Private intcliacref_comp_id As Integer
  Private intcliacref_contact_id As Integer
  Private strcliacref_contact_type As String
  Private strcliacref_owner_percentage As String
  Private strcliacref_jetnet_ac_id As String
    Private strcliacref_date_fraction_purchased As Nullable(Of System.DateTime)
    Private strcliacref_date_fraction_expires As Nullable(Of System.DateTime)
  Private strcliacref_business_type As String
  Private strcliacref_operator_flag As String
  Private strcliacref_jetnet_contact_type As String
  Private intcliacref_contact_priority As Integer


  '**************************************************
  ' Setters and getters
  '**************************************************
  Public Property cliacref_id() As Integer
    Get
      Return intcliacref_id
    End Get
    Set(ByVal Value As Integer)
      intcliacref_id = Value
    End Set
  End Property


  Public Property cliacref_cliac_id() As Integer
    Get
      Return intcliacref_cliac_id
    End Get
    Set(ByVal Value As Integer)
      intcliacref_cliac_id = Value
    End Set
  End Property


  Public Property cliacref_comp_id() As Integer
    Get
      Return intcliacref_comp_id
    End Get
    Set(ByVal Value As Integer)
      intcliacref_comp_id = Value
    End Set
  End Property


  Public Property cliacref_contact_id() As Integer
    Get
      Return intcliacref_contact_id
    End Get
    Set(ByVal Value As Integer)
      intcliacref_contact_id = Value
    End Set
  End Property


  Public Property cliacref_contact_type() As String
    Get
      Return strcliacref_contact_type
    End Get
    Set(ByVal Value As String)
      strcliacref_contact_type = Value
    End Set
  End Property


  Public Property cliacref_owner_percentage() As String
    Get
      Return strcliacref_owner_percentage
    End Get
        Set(ByVal Value As String)
            If cliacref_owner_percentage = "" Then
                strcliacref_owner_percentage = "0"
            Else
                strcliacref_owner_percentage = Value
            End If
        End Set
  End Property


  Public Property cliacref_jetnet_ac_id() As String
    Get
      Return strcliacref_jetnet_ac_id
    End Get
    Set(ByVal Value As String)
      strcliacref_jetnet_ac_id = Value
    End Set
  End Property


    Public Property cliacref_date_fraction_purchased() As Nullable(Of System.DateTime)
        Get
            Return strcliacref_date_fraction_purchased
        End Get
        Set(ByVal Value As Nullable(Of System.DateTime))
            strcliacref_date_fraction_purchased = Value
        End Set
    End Property


    Public Property cliacref_date_fraction_expires() As Nullable(Of System.DateTime)
        Get
            Return strcliacref_date_fraction_expires
        End Get
        Set(ByVal Value As Nullable(Of System.DateTime))
            strcliacref_date_fraction_expires = Value
        End Set
    End Property


  Public Property cliacref_business_type() As String
    Get
      Return strcliacref_business_type
    End Get
    Set(ByVal Value As String)
      strcliacref_business_type = Value
    End Set
  End Property


  Public Property cliacref_operator_flag() As String
    Get
      Return strcliacref_operator_flag
    End Get
    Set(ByVal Value As String)
      strcliacref_operator_flag = Value
    End Set
  End Property

  Public Property cliacref_jetnet_contact_type() As String
    Get
      Return strcliacref_jetnet_contact_type
    End Get
    Set(ByVal Value As String)
      strcliacref_jetnet_contact_type = Value
    End Set
  End Property

  Public Property cliacref_contact_priority() As Integer
    Get
      Return intcliacref_contact_priority
    End Get
    Set(ByVal Value As Integer)
      intcliacref_contact_priority = Value
    End Set
  End Property

  '**************************************************
  ' Constructors
  '**************************************************


  '   Default Constructor
  Public Sub New()
    cliacref_id = 0
    cliacref_cliac_id = 0
    cliacref_comp_id = 0
    cliacref_contact_id = 0
    cliacref_contact_type = ""
    cliacref_owner_percentage = ""
    cliacref_jetnet_ac_id = ""
        'cliacref_date_fraction_purchased = ""
        'cliacref_date_fraction_expires = ""
    cliacref_business_type = ""
    cliacref_operator_flag = ""
    cliacref_jetnet_contact_type = ""
    cliacref_contact_priority = 0
  End Sub


  '   Parameter Based Constructor
    Public Sub New(ByVal acliacref_id As Integer, ByVal acliacref_cliac_id As Integer, ByVal acliacref_comp_id As Integer, ByVal acliacref_contact_id As Integer, ByVal acliacref_contact_type As String, ByVal acliacref_owner_percentage As String, ByVal acliacref_jetnet_ac_id As String, ByVal acliacref_date_fraction_purchased As Nullable(Of System.DateTime), ByVal acliacref_date_fraction_expires As Nullable(Of System.DateTime), ByVal acliacref_business_type As String, ByVal acliacref_operator_flag As String, ByVal astrcliacref_jetnet_contact_type As String, ByVal acliacref_contact_priority As Integer)
        cliacref_id = acliacref_id
        cliacref_cliac_id = acliacref_cliac_id
        cliacref_comp_id = acliacref_comp_id
        cliacref_contact_id = acliacref_contact_id
        cliacref_contact_type = acliacref_contact_type
        cliacref_owner_percentage = acliacref_owner_percentage
        cliacref_jetnet_ac_id = acliacref_jetnet_ac_id
        cliacref_date_fraction_purchased = acliacref_date_fraction_purchased
        cliacref_date_fraction_expires = acliacref_date_fraction_expires
        cliacref_business_type = acliacref_business_type
        cliacref_operator_flag = acliacref_operator_flag
        cliacref_jetnet_contact_type = astrcliacref_jetnet_contact_type
        cliacref_contact_priority = acliacref_contact_priority
    End Sub




  ' ***********************************************************************
  ' Methods
  ' ***********************************************************************


  ' Method name: ClassInfo
  ' Purpose: to generate a string with all assigned parameters
  ' Parameters: Client_Aircraft_Reference
  ' Return: String with all assigned parameters
  ' Change Log
  '           3/19/2010 - Created By: Tom Jones
  Public Function ClassInfo(ByVal aClient_Aircraft_Reference As clsClient_Aircraft_Reference) As String
    Try
      Dim ClassInformation As String


      ClassInformation = " cliacref_id = " & aClient_Aircraft_Reference.cliacref_id & vbNewLine & " cliacref_cliac_id = " & aClient_Aircraft_Reference.cliacref_cliac_id & vbNewLine & " cliacref_comp_id = " & aClient_Aircraft_Reference.cliacref_comp_id & vbNewLine & " cliacref_contact_id = " & aClient_Aircraft_Reference.cliacref_contact_id & vbNewLine & " cliacref_contact_type = " & aClient_Aircraft_Reference.cliacref_contact_type & vbNewLine & " cliacref_owner_percentage = " & aClient_Aircraft_Reference.cliacref_owner_percentage & vbNewLine & " cliacref_jetnet_ac_id = " & aClient_Aircraft_Reference.cliacref_jetnet_ac_id & vbNewLine & " cliacref_date_fraction_purchased = " & aClient_Aircraft_Reference.cliacref_date_fraction_purchased & vbNewLine & " cliacref_date_fraction_expires = " & aClient_Aircraft_Reference.cliacref_date_fraction_expires & vbNewLine & " cliacref_business_type = " & aClient_Aircraft_Reference.cliacref_business_type & vbNewLine & " cliacref_operator_flag = " & aClient_Aircraft_Reference.cliacref_operator_flag & vbNewLine & " cliacref_jetnet_contact_type = " & aClient_Aircraft_Reference.cliacref_jetnet_contact_type & vbNewLine & " cliacref_contact_priority: " & aClient_Aircraft_Reference.cliacref_contact_priority


      ' return the string
      Return ClassInformation
    Catch ex As Exception
      MsgBox("Error occured in classInfo. Class: clsClient_Aircraft_Reference. Error:" & ex.Message)
      Return Nothing
    End Try
  End Function


End Class
