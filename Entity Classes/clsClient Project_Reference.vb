Option Strict On
Option Explicit On

Public Class clsClient_Project_Reference
    ' ***********************************************************************
    ' Class Comments Section
    ' ***********************************************************************
    ' Class name: clsClient_Project_Reference
    ' Purpose: Configure all properties and methods for clsClient_Project_Reference
    ' Parameters: None
    ' Return: None
    ' Change Log
    '           05/03/2011 - Created By: Amanda Vaughn

    '--
    '-- Table structure for table `client_projects`
    '--
    '    DROP TABLE IF EXISTS `client_project_reference`;
    'CREATE TABLE `client_project_reference` (
    '  `clipref_id` bigint(20) NOT NULL auto_increment,
    '  `clipref_cliproj_id` int(11) NOT NULL,
    '  `clipref_exp_id` int(11) NOT NULL,
    '  `clipref_sort_order` smallint(6) NOT NULL,
    '  PRIMARY KEY  (`clipref_id`)
    ') ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;



    '**************************************************
    ' Private variable declarations
    '**************************************************
  Private intclipref_id As Integer
  Private intclipref_cliproj_id As Integer
  Private intclipref_exp_id As Integer
  Private intclipref_sort_order As Integer
  Private strclipref_source As String

    '**************************************************
    ' Setters and getters
    '**************************************************
    'integers
    Public Property clipref_id() As Integer
        Get
            Return intclipref_id
        End Get
        Set(ByVal Value As Integer)
            intclipref_id = Value
        End Set
    End Property
    Public Property clipref_cliproj_id() As Integer
        Get
            Return intclipref_cliproj_id
        End Get
        Set(ByVal Value As Integer)
            intclipref_cliproj_id = Value
        End Set
    End Property
    Public Property clipref_exp_id() As Integer
        Get
            Return intclipref_exp_id
        End Get
        Set(ByVal Value As Integer)
            intclipref_exp_id = Value
        End Set
    End Property
    Public Property clipref_sort_order() As Integer
        Get
            Return intclipref_sort_order
        End Get
        Set(ByVal Value As Integer)
            intclipref_sort_order = Value
        End Set
  End Property
  Public Property clipref_source() As String
    Get
      Return strclipref_source
    End Get
    Set(ByVal Value As String)
      strclipref_source = Value
    End Set
  End Property


    '**************************************************
    ' Constructors
    '**************************************************


    '   Default Constructor
    Public Sub New()
        intclipref_id = 0
        intclipref_cliproj_id = 0
        intclipref_exp_id = 0
        intclipref_sort_order = 0
    End Sub

    'Parameter Based Constructor
    Public Sub New(ByVal aclipref_id As Integer, ByVal aclipref_cliproj_id As Integer, ByVal aclipref_exp_id As Integer, ByVal aclipref_sort_order As Integer)
        clipref_id = aclipref_id
        clipref_cliproj_id = aclipref_cliproj_id
        clipref_exp_id = aclipref_exp_id
        clipref_sort_order = aclipref_sort_order
    End Sub

    ' ***********************************************************************
    ' Methods
    ' Method name: ClassInfo
    ' Purpose: to generate a string with all assigned parameters
    ' Parameters: Client_Project_Reference
    ' Return: String with all assigned parameters
    ' Change Log
    '           05/3/2010 - Created By: Amanda Vaughn
    Public Function ClassInfo(ByVal aClient_Project_Reference As clsClient_Project_Reference) As String
        Try
            Dim ClassInformation As String
            ClassInformation = "<b>Client Project Class Reference Information:</b>" & vbNewLine
            ClassInformation = ClassInformation & " clipref_id = " & aClient_Project_Reference.clipref_id & vbNewLine
            ClassInformation = ClassInformation & " clipref_cliproj_id= " & aClient_Project_Reference.clipref_cliproj_id & vbNewLine
            ClassInformation = ClassInformation & " clipref_exp_id = " & aClient_Project_Reference.clipref_exp_id & vbNewLine
            ClassInformation = ClassInformation & " clipref_sort_order = " & aClient_Project_Reference.clipref_sort_order & vbNewLine
            ' return the string
            Return ClassInformation
        Catch ex As Exception
            MsgBox("Error occured in classInfo. Class: clsClient_Project_Reference. Error:" & ex.Message)
            Return Nothing
        End Try
    End Function
End Class
