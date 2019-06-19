Imports System.Data.OleDb
Imports System.Text
Public Class Staff
    Dim cnn As New OleDb.OleDbConnection
    Private Sub Staff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        cbodesg.SelectedIndex = 0
        loadData()
    End Sub
    Private Sub loadData()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim da As New OleDbDataAdapter("select Staff_ID as [Staff ID], Staff_Name as [Staff Name], DESIGNATION from Staff", cnn)
        da.Fill(dt)
        DataGridView1.DataSource = dt.DefaultView
        cnn.Close()
    End Sub
    Private Sub search()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim da As New OleDbDataAdapter("select Staff_ID as [Staff ID], Staff_Name as [Staff Name], DESIGNATION from Staff like '" & txtsearch.Text & "%'", cnn)
        da.Fill(dt)
        DataGridView1.DataSource = dt.DefaultView
        cnn.Close()
    End Sub

    Private Sub BTNADD_Click(sender As Object, e As EventArgs) Handles BTNADD.Click
        Try
            If TXTNAME.Text Is String.Empty Or cbodesg.SelectedIndex = 0 Then
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
                cmd.CommandText = "INSERT INTO Staff (Staff_Name, DESIGNATION) " &
                    " VALUES('" & Me.TXTNAME.Text & "','" & Me.cbodesg.Text & "')"
                cmd.ExecuteNonQuery()
                cnn.Close()
                MessageBox.Show("Staff Added Successfully", "The Hexagon Place", MessageBoxButtons.OK)
                TXTNAME.Clear()
                cbodesg.SelectedIndex = 0
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub DataGridView1_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.RowHeaderMouseClick
        Try
            Dim dr As DataGridViewRow = DataGridView1.SelectedRows(0)
            TextBox1.Text = dr.Cells(0).Value.ToString()
            TXTNAME.Text = dr.Cells(1).Value.ToString()
            cbodesg.Text = dr.Cells(2).Value.ToString()
            txtsearch.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                    'open connection if not open yet
                    cnn.Open()
                End If
                cmd.Connection = cnn
            cmd.CommandText = "DELETE from Staff where Staff_ID='" & TextBox1.Text & "'"
            cmd.ExecuteNonQuery()
                MessageBox.Show("Deleted", "User Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                loadData()
            TXTNAME.Clear()
            txtsearch.Clear()
            cbodesg.SelectedIndex = 0
            TextBox1.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
End Class