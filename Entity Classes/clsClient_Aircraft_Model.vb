Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Aircraft_Model
' Purpose: Configure all properties and methods for clsClient_Aircraft_Model
' Parameters: None
' Return: None
' Change Log
'           04/15/2010 - Created By: Tom Jones


Public Class clsClient_Aircraft_Model



'**************************************************
' Private variable declarations
'**************************************************
Private intcliamod_id As Integer
Private strcliamod_airframe_type As String
Private strcliamod_make_type As String
Private strcliamod_make_name As String
Private strcliamod_model_name As String
  Private strcliamod_manufacturer_name As String
  Private intcliamod_jetnet_amod_id As Integer


'**************************************************
' Setters and getters
'**************************************************
Public Property cliamod_id As Integer
    Get
       Return intcliamod_id
    End Get
    Set (ByVal Value As Integer)
        intcliamod_id = Value
    End Set
  End Property

Public Property cliamod_airframe_type As String
    Get
       Return strcliamod_airframe_type
    End Get
    Set (ByVal Value As String)
        strcliamod_airframe_type = Value
    End Set
End Property


Public Property cliamod_make_type As String
    Get
       Return strcliamod_make_type
    End Get
    Set (ByVal Value As String)
        strcliamod_make_type = Value
    End Set
End Property


Public Property cliamod_make_name As String
    Get
       Return strcliamod_make_name
    End Get
    Set (ByVal Value As String)
        strcliamod_make_name = Value
    End Set
End Property


Public Property cliamod_model_name As String
    Get
       Return strcliamod_model_name
    End Get
    Set (ByVal Value As String)
        strcliamod_model_name = Value
    End Set
End Property


Public Property cliamod_manufacturer_name As String
    Get
       Return strcliamod_manufacturer_name
    End Get
    Set (ByVal Value As String)
        strcliamod_manufacturer_name = Value
    End Set
End Property

  Public Property cliamod_jetnet_amod_id() As Integer
    Get
      Return intcliamod_jetnet_amod_id
    End Get
    Set(ByVal Value As Integer)
      intcliamod_jetnet_amod_id = Value
    End Set
  End Property
'**************************************************
' Constructors
'**************************************************


'   Default Constructor
Public Sub New()
    cliamod_id = 0
cliamod_airframe_type = ""
cliamod_make_type = ""
cliamod_make_name = ""
cliamod_model_name = ""
    cliamod_manufacturer_name = ""
    cliamod_jetnet_amod_id = 0
End Sub


'   Parameter Based Constructor
  Public Sub New(ByVal acliamod_id As Integer, ByVal acliamod_airframe_type As String, ByVal acliamod_make_type As String, ByVal acliamod_make_name As String, ByVal acliamod_model_name As String, ByVal acliamod_manufacturer_name As String, ByVal acliamod_jetnet_amod_id As Integer)
    cliamod_id = acliamod_id
    cliamod_airframe_type = acliamod_airframe_type
    cliamod_make_type = acliamod_make_type
    cliamod_make_name = acliamod_make_name
    cliamod_model_name = acliamod_model_name
    cliamod_manufacturer_name = acliamod_manufacturer_name
    cliamod_jetnet_amod_id = acliamod_jetnet_amod_id
  End Sub




' ***********************************************************************
' Methods
' ***********************************************************************


' Method name: ClassInfo
' Purpose: to generate a string with all assigned parameters
' Parameters: Client_Aircraft_Model
' Return: String with all assigned parameters
' Change Log
'           04/15/2010 - Created By: Tom Jones
Public Function ClassInfo(byval aClient_Aircraft_Model as clsClient_Aircraft_Model) as string
Try
Dim ClassInformation as String


      ClassInformation = " cliamod_id = " & aClient_Aircraft_Model.cliamod_id & vbNewLine & " cliamod_airframe_type = " & aClient_Aircraft_Model.cliamod_airframe_type & vbNewLine & " cliamod_make_type = " & aClient_Aircraft_Model.cliamod_make_type & vbNewLine & " cliamod_make_name = " & aClient_Aircraft_Model.cliamod_make_name & vbNewLine & " cliamod_model_name = " & aClient_Aircraft_Model.cliamod_model_name & vbNewLine & " cliamod_manufacturer_name = " & aClient_Aircraft_Model.cliamod_manufacturer_name & vbNewLine & " cliamod_jetnet_amod_id = " & aClient_Aircraft_Model.cliamod_jetnet_amod_id


' return the string
Return ClassInformation
Catch ex As Exception
MsgBox("Error occured in classInfo. Class: clsClient_Aircraft_Model. Error:" & ex.Message)
Return Nothing
End Try
End Function


End Class
