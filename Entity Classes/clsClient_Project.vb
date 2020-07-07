Option Strict On
Option Explicit On

Public Class clsClient_Project
    ' ***********************************************************************
    ' Class Comments Section
    ' ***********************************************************************
    ' Class name: clsClient_Project
    ' Purpose: Configure all properties and methods for clsClient_Project
    ' Parameters: None
    ' Return: None
    ' Change Log
    '           05/03/2011 - Created By: Amanda Vaughn

    '--
    '-- Table structure for table `client_projects`
    '--
    'DROP TABLE IF EXISTS `client_projects`;
    'CREATE TABLE `client_projects` (
    '  `cliproj_id` int(11) NOT NULL auto_increment,
    '  `cliproj_name` varchar(120) default NULL,
    '  `cliproj_description` text,
    '  `cliproj_user_id` int(11) default NULL,
    '  `cliproj_shared` char(1) default 'N',
    '  `cliproj_action_date` datetime default NULL,
    '  `cliproj_type` char(1) default 'E',
    '  `cliproj_source` varchar(15) default NULL,
    '  PRIMARY KEY  (`cliproj_id`)
    ') ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;


    '**************************************************
    ' Private variable declarations
    '**************************************************
    Private dtcliproj_action_date As Nullable(Of System.DateTime)
    Private intcliproj_id As Integer
    Private intcliproj_user_id As Nullable(Of Integer)
    Private intcliproj_source As Integer
    Private intcliproj_name As String
    Private strcliproj_description As String
    Private strcliproj_shared As String
  Private strcliproj_type As String
  Private strcliproj_jetnet_model As Integer
  Private strcliproj_client_model As Integer
  Private strcliproj_model_default As String
  Private strcliproj_market_default As String
    '**************************************************
    ' Setters and getters
    '**************************************************
    'dates
    Public Property cliproj_action_date() As Nullable(Of System.DateTime)
        Get
            Return dtcliproj_action_date
        End Get
        Set(ByVal Value As Nullable(Of System.DateTime))
            dtcliproj_action_date = Value
        End Set
    End Property
    'integers
    Public Property cliproj_id() As Integer
        Get
            Return intcliproj_id
        End Get
        Set(ByVal Value As Integer)
            intcliproj_id = Value
        End Set
    End Property
    Public Property cliproj_user_id() As Nullable(Of Integer)
        Get
            Return intcliproj_user_id
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliproj_user_id = Value
        End Set
    End Property
    Public Property cliproj_source() As Integer
        Get
            Return intcliproj_source
        End Get
        Set(ByVal Value As Integer)
            intcliproj_source = Value
        End Set
    End Property
    'strings
    Public Property cliproj_name() As String
        Get
            Return intcliproj_name
        End Get
        Set(ByVal Value As String)
            intcliproj_name = Value
        End Set
    End Property
    Public Property cliproj_description() As String
        Get
            Return strcliproj_description
        End Get
        Set(ByVal Value As String)
            strcliproj_description = Value
        End Set
    End Property
    Public Property cliproj_shared() As String
        Get
            Return strcliproj_shared
        End Get
        Set(ByVal Value As String)
            strcliproj_shared = Value
        End Set
  End Property
  Public Property cliproj_jetnet_model() As Integer
    Get
      Return strcliproj_jetnet_model
    End Get
    Set(ByVal Value As Integer)
      strcliproj_jetnet_model = Value
    End Set
  End Property
  Public Property cliproj_client_model() As Integer
    Get
      Return strcliproj_client_model
    End Get
    Set(ByVal Value As Integer)
      strcliproj_client_model = Value
    End Set
  End Property
    Public Property cliproj_type() As String
        Get
            Return strcliproj_type
        End Get
        Set(ByVal Value As String)
            strcliproj_type = Value
        End Set
    End Property
  Public Property cliproj_model_default() As String
    Get
      Return strcliproj_model_default
    End Get
    Set(ByVal Value As String)
      strcliproj_model_default = Value
    End Set
  End Property
  Public Property cliproj_market_default() As String
    Get
      Return strcliproj_market_default
    End Get
    Set(ByVal Value As String)
      strcliproj_market_default = Value
    End Set
  End Property




    '**************************************************
    ' Constructors
    '**************************************************


    '   Default Constructor
    Public Sub New()
        cliproj_action_date = Now()
        cliproj_id = 0
        'cliproj_user_id nullable
        cliproj_name = ""
        cliproj_description = ""
        cliproj_shared = ""
        cliproj_type = ""
        cliproj_source = 0
    End Sub

    'Parameter Based Constructor
    Public Sub New(ByVal acliproj_action_date As Date, ByVal acliproj_id As Integer, ByVal acliproj_user_id As Integer, ByVal acliproj_name As String, ByVal acliproj_description As String, ByVal acliproj_shared As String, ByVal acliproj_type As String, ByVal acliproj_source As Integer)
        cliproj_action_date = acliproj_action_date
        cliproj_id = acliproj_id
        cliproj_user_id = acliproj_user_id
        cliproj_name = acliproj_name
        cliproj_description = acliproj_description
        cliproj_shared = acliproj_shared
        cliproj_type = acliproj_type
        cliproj_source = acliproj_source
    End Sub

    ' ***********************************************************************
    ' Methods
    ' Method name: ClassInfo
    ' Purpose: to generate a string with all assigned parameters
    ' Parameters: Client_Project
    ' Return: String with all assigned parameters
    ' Change Log
    '           05/3/2010 - Created By: Amanda Vaughn
    Public Function ClassInfo(ByVal aClient_Project As clsClient_Project) As String
        Try
            Dim ClassInformation As String
            ClassInformation = "<b>Client Project Class Information:</b><br />"
            ClassInformation = ClassInformation & " cliproj_id = " & aClient_Project.cliproj_id & "<br />"
            ClassInformation = ClassInformation & " cliproj_action_date= " & aClient_Project.cliproj_action_date & "<br />"
            ClassInformation = ClassInformation & " cliproj_user_id = " & aClient_Project.cliproj_user_id & "<br />"
            ClassInformation = ClassInformation & " cliproj_name = " & aClient_Project.cliproj_name & "<br />"
            ClassInformation = ClassInformation & " cliproj_description = " & aClient_Project.cliproj_description & "<br />"
            ClassInformation = ClassInformation & " cliproj_shared = " & aClient_Project.cliproj_shared & "<br />"
            ClassInformation = ClassInformation & " cliproj_type = " & aClient_Project.cliproj_type & "<br />"
            ClassInformation = ClassInformation & " cliproj_source = " & aClient_Project.cliproj_source & "<br />"
            ' return the string
            Return ClassInformation
        Catch ex As Exception
            MsgBox("Error occured in classInfo. Class: clsClient_Project. Error:" & ex.Message)
            Return Nothing
        End Try
    End Function
End Class
