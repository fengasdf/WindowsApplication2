Public Class Form1



    Public socketi As Net.Sockets.Socket
    Dim Th As Threading.Thread

    Public Sub Task()


        While 1
            Dim str As New Text.StringBuilder
            Dim str2 As String = Nothing
            Dim str3 As String = Nothing
            Dim by(100) As Byte
            Dim ch(100) As Char
            Dim ch1(100) As Char
            Dim err As Int32 = socketi.Receive(by)
            If err > 0 Then
                'str = BitConverter.ToString(by)
                '///////////////////////////////////
                'Dim pos As Int32 = 0
                'Dim first As Int32 = 0
                'Dim second As Int32 = 0
                'Dim SSize As Int32 = str.Length
                'While second <= SSize And first <= SSize And str.Chars(first) <> "0"c And str.Chars(second) <> "0"c
                '    ch(pos) = str.Chars(first)
                '    pos = pos + 1
                '    ch(pos) = str.Chars(second)
                '    pos = pos + 1
                '    ch(pos) = " "c
                '    pos = pos + 1
                '    first += 3
                '    second += 3
                'End While

                str.Append(System.Text.ASCIIEncoding.ASCII.GetString(by, 0, by.Length()))
                '////////////////////////////////
                Dim str1 As String = TextBox4.Text
                str3 = str.ToString()
                TextBox4.Text = str1 & str.ToString()
                str1 = TextBox4.Text
                TextBox4.Text = str1 & vbNewLine
                '/////////////////////////////////
                Dim size As Int32 = str3.Length
                Dim i As Int32 = 0
                Dim j As Int32 = 0
                While i < size；
                    If str3.Chars(i) = "="c Then
                        Exit While
                    End If
                    i = i + 1
                End While
                i += 1
                While i < size
                    If str3.Chars(i) = " "c Then
                        Exit While
                    End If
                    ch(j) = str3.Chars(i)
                    j += 1
                    i += 1
                End While
                '///////////////////////
                j = 0
                While i < size
                    If str3.Chars(i) = "="c Then
                        Exit While
                    End If
                    i = i + 1
                End While
                i += 1
                While i < size
                    ch1(j) = str3.Chars(i)
                    j += 1
                    i += 1
                End While
                Dim text1 As String = ch
                Dim text2 As String = ch1
                TextBox5.Text = text1
                TextBox6.Text = text2
            End If
        End While
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim flag As Int32 = 1
        Dim Ip As String = Nothing
        Dim Port As String = Nothing
        Ip = TextBox1.Text
        While Ip Is ""
            MsgBox("请输入Ip地址", MsgBoxStyle.OkOnly, Me.Text)
            Return
        End While

        Port = TextBox2.Text
        While Port Is ""
            MsgBox("请输入Port地址", MsgBoxStyle.OkOnly, Me.Text)
            Return
        End While
        '///////////////////////////////////////////////////////////链接代码
        Dim LocalEndPoint As New Net.IPEndPoint(Net.IPAddress.Parse(Ip), CInt(Port))

        Dim client As Net.Sockets.Socket = Nothing
        client = New Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)

        client.Connect(LocalEndPoint)
        socketi = client
        ' This is how you can determine whether a socket is still connected.
        Dim blockingState As Boolean = client.Blocking
        Try
            Dim tmp(0) As Byte

            client.Blocking = False
            client.Send(tmp, 0, 0)
            MsgBox("Connected!")
        Catch ie As Net.Sockets.SocketException
            ' 10035 == WSAEWOULDBLOCK
            flag = 0
            If ie.NativeErrorCode.Equals(10035) Then
                MsgBox("Still Connected, but the Send would block")
            Else
                MsgBox("Disconnected: error code {0}!", ie.NativeErrorCode)
            End If

        Finally
            client.Blocking = blockingState
        End Try

        MsgBox("Connected: {0}", client.Connected)
        If flag <> 1 Then
            Return
        End If
        Dim newTheread As Threading.Thread = New Threading.Thread(AddressOf Task)
        Th = newTheread
        newTheread.Start()




    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim str As String = Nothing
        str = TextBox3.Text
        If str Is "" Then
            MsgBox("请输入数据", MsgBoxStyle.Information)
            Return
        End If
        Dim encText As New System.Text.UTF8Encoding()
        Dim btText() As Byte
        btText = encText.GetBytes(str)
        socketi.Send(btText)
        Dim str1 As String = TextBox4.Text
        TextBox4.Text = str1 & str & vbNewLine


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        socketi.Close()
        Try
            Th.Abort()

        Catch ex As Exception
            Return
        End Try

        MsgBox("连接断开")
    End Sub


End Class
