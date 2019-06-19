Imports System.Data.OleDb
Imports System.Text
Public Class frmOutlet
    Dim cnn As New OleDb.OleDbConnection
    Private Sub frmOutlet_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
    End Sub

    Private Sub BTNADD_Click(sender As Object, e As EventArgs) Handles BTNADD.Click
        Try
            If TXTNAME.Text Is String.Empty Then
                MessageBox.Show("Please Complete All Fields", "The Hexagon Place", MessageBoxButtons.OK)
                TXTNAME.Focus()
            Else
                Dim cmd As New OleDb.OleDbCommand
                If Not cnn.State = ConnectionState.Open Then
                    'open connection if not open yet
                    cnn.Open()
                End If
                cmd.Connection = cnn
                'add data to table 
                cmd.CommandText = "INSERT INTO Outlets (Outlet) " & _
                    " VALUES('" & Me.TXTNAME.Text & "')"
                cmd.ExecuteNonQuery()
                cnn.Close()
                MessageBox.Show("Outlet Created Successfully", "The Hexagon Place", MessageBoxButtons.OK)
                TXTNAME.Clear()
                ' cbodesg.SelectedIndex = 0
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class