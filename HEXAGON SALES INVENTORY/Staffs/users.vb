Imports System.Data.OleDb
Public Class users
    Dim cnn As New OleDb.OleDbConnection
    Dim dtable As DataTable
    Private Sub users_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        cbodesg.SelectedIndex = 0
    End Sub

    Private Sub BTNADD_Click(sender As Object, e As EventArgs) Handles BTNADD.Click
        If TXTFIRSTNAME.Text Is String.Empty Or TXTLASTNAME.Text = String.Empty Or TXTADD.Text = String.Empty Or TXTNUMBER.Text = String.Empty Or cbodesg.SelectedIndex = 0 Then
            MessageBox.Show("Please Complete All Fields", "The Hexagon Place", MessageBoxButtons.OK)
            TXTFIRSTNAME.Focus()
        Else
            Try
                Dim cmd As New OleDb.OleDbCommand("INSERT INTO Users (FirstName, LastName, Address, MobileNo, Designation, UserName, Password) VALUES(@a1,@a2,@a3,@a4,@a5,@a6)", cnn)
                cmd.Parameters.AddWithValue("a1", TXTFIRSTNAME.Text)
                cmd.Parameters.AddWithValue("a2", TXTLASTNAME.Text)
                cmd.Parameters.AddWithValue("a3", TXTADD.Text)
                cmd.Parameters.AddWithValue("a4", TXTNUMBER.Text)
                cmd.Parameters.AddWithValue("a5", cbodesg.SelectedText)
                cmd.Parameters.AddWithValue("a6", TXTUSERNAME.Text)

                cnn.Open()
                cmd.ExecuteNonQuery()
                cnn.Close()
                MessageBox.Show("User Added Successfully", "The Hexagon Place", MessageBoxButtons.OK)
                TXTFIRSTNAME.Clear()
                TXTLASTNAME.Clear()
                TXTADD.Clear()
                TXTNUMBER.Clear()
                cbodesg.SelectedIndex = 0

            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If
    End Sub

    Private Sub TXTNUMBER_TextChanged(sender As Object, e As EventArgs) Handles TXTNUMBER.TextChanged

    End Sub
End Class