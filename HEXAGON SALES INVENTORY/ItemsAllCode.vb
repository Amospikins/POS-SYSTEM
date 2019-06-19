Imports System.Data.OleDb
Module ItemsAllCode
    Dim cnn As New OleDb.OleDbConnection
    Dim dtable As DataTable
    Public dr As OleDbDataReader
    Public Role As String
    Public FirstName As String
    Public LastName As String
    Public Sub connecting()
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
    End Sub

    Public Sub savenewuser()
        Try
            'add data to table 
            Dim cmd As New OleDb.OleDbCommand("INSERT INTO Users(FirstName, LastName, Address, MobileNo, Designation, UserName, Passwords) VALUES(@a1,@a2,@a3,@a4,@a5,@a6,@a7)", cnn)
            cmd.Parameters.AddWithValue("a1", usersss.TextBox1.Text)
            cmd.Parameters.AddWithValue("a2", usersss.TextBox2.Text)
            cmd.Parameters.AddWithValue("a3", usersss.TextBox3.Text)
            cmd.Parameters.AddWithValue("a4", usersss.TextBox4.Text)
            cmd.Parameters.AddWithValue("a5", usersss.textbox5.Text)
            cmd.Parameters.AddWithValue("a6", usersss.TextBox6.Text)
            cmd.Parameters.AddWithValue("a7", usersss.TextBox7.Text)

            cnn.Open()
            cmd.ExecuteNonQuery()
            cnn.Close()
            MessageBox.Show("User Created Successfully", "The Hexagon Place", MessageBoxButtons.OK)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Sub LOGIN()
        Try
            If LoginForm1.UsernameTextBox.Text = String.Empty Or LoginForm1.PasswordTextBox.Text = String.Empty Then
                MsgBox("Please enter all field")
                LoginForm1.UsernameTextBox.Focus()
            Else

                Dim uname As String
                Dim pword As String


                uname = LoginForm1.UsernameTextBox.Text
                pword = LoginForm1.PasswordTextBox.Text
                cnn.Open()
                Dim str As String
                str = "SELECT * FROM Users WHERE (UserName like '" & uname & "')"
                Dim cmd As OleDbCommand = New OleDbCommand(str, cnn)
                dr = cmd.ExecuteReader

                If dr.Read() Then

                    If pword = dr("Passwords") Then
                        MsgBox("Login Successful")
                        FirstName = dr("FirstName")
                        LastName = dr("LastName")
                        Role = dr("Designation")
                        If Role = "ADMIN" Then
                            Home.PanelHome.Controls.Clear()
                            Home.ToolStripButton1.Enabled = True
                            Home.ToolStripButton2.Enabled = True
                            Home.ToolStripButton3.Enabled = True
                            Home.ToolStripButton4.Enabled = True
                            Home.ToolStripButton5.Enabled = True
                            Home.ToolStripButton6.Enabled = True
                            Home.ToolStripButton7.Enabled = True
                            Home.ToolStripButton8.Enabled = True
                            Home.ToolStripButton9.Enabled = True
                            Home.ToolStripButton10.Enabled = True
                            Home.ToolStripButton11.Enabled = True
                            Home.ToolStripButton12.Enabled = True
                        ElseIf Role = "CASHIER" Then
                            Home.ToolStripButton2.Enabled = True
                            Home.ToolStripButton7.Enabled = True
                            Home.ToolStripButton8.Enabled = True
                            'Home.ToolStripButton6.Enabled = True
                        ElseIf Role = "ACCOUNTANT" Then
                            Home.ToolStripButton3.Enabled = True
                            Home.ToolStripButton8.Enabled = True
                        ElseIf Role = "AUDITOR" Then
                            Home.ToolStripButton3.Enabled = True
                            Home.ToolStripButton8.Enabled = True
                        ElseIf Role = "SUPERVISOR" Then
                            Home.ToolStripButton8.Enabled = True
                            Home.ToolStripButton3.Enabled = True
                            Home.ToolStripButton6.Enabled = True
                            Home.ToolStripButton7.Enabled = True
                            Home.ToolStripButton10.Enabled = True
                        ElseIf Role = "HEAD BAR MAN" Then
                            Home.ToolStripButton8.Enabled = True
                            Home.ToolStripButton3.Enabled = True
                            Home.ToolStripButton6.Enabled = True
                            Home.ToolStripButton7.Enabled = True
                            Home.ToolStripButton10.Enabled = True
                        ElseIf Role = "MANAGER" Then
                            Home.ToolStripButton3.Enabled = True
                            Home.ToolStripButton8.Enabled = True
                            '  Home.ToolStripButton10.Enabled = True
                        ElseIf Role = "BAR MAN" Then
                            Home.ToolStripButton2.Enabled = True
                            Home.ToolStripButton7.Enabled = True
                            Home.ToolStripButton8.Enabled = True

                        End If
                        Home.lblUsername.Text = "Hello, " & FirstName & " " & LastName & vbNewLine & Role
                        Home.btnLog.Text = "LOG OUT"
                        LoginForm1.UsernameTextBox.Clear()
                        LoginForm1.PasswordTextBox.Clear()
                        LoginForm1.Close()
                    Else
                        MsgBox("Wrong Username or Password")
                    End If
                End If
                cnn.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Public Sub again()
        Try
            If EditLogin.TextBox1.Text = String.Empty Then
                MsgBox("Please enter password")
                EditLogin.TextBox1.Focus()
            Else

                Dim pword As String
                Dim uname As String

                uname = EditLogin.TextBox1.Text
                pword = EditLogin.TextBox2.Text
                '   confirm = pword
                cnn.Open()
                Dim str As String
                str = "SELECT * FROM Users WHERE (UserName like '" & uname & "')"
                Dim cmd As OleDbCommand = New OleDbCommand(str, cnn)
                dr = cmd.ExecuteReader

                If dr.Read() Then
                    role = dr("Designation")
                    If pword = dr("Passwords") Then
                        MsgBox("Successful")

                        Home.PanelHome.Controls.Clear()
                        point_of_sale.txtsearchCode.Enabled = True
                        point_of_sale.btnretrieve.Enabled = True
                        point_of_sale.DataGridView3.Enabled = True
                        point_of_sale.Button7.Enabled = True
                        point_of_sale.Button9.Enabled = True
                        point_of_sale.btnprocess.Enabled = False
                        point_of_sale.Button11.Enabled = True
                    Else
                        MsgBox("wrong values")
                        Exit Sub
                    End If
                    cnn.Close()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Module
