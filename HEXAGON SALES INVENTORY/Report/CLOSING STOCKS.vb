Imports System.Data.OleDb

Public Class CLOSING_STOCKS
    Dim cnn As New OleDb.OleDbConnection
    Dim cnn1 As New OleDb.OleDbConnection

    Private Sub loadDataclosingstock()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim da As New OleDbDataAdapter("select Item_Code as [Item Code], Item_Name as [Item Name], Price, Stocks as [Stock] from Products", cnn)
        da.Fill(dt)
        DataGridView7.DataSource = dt.DefaultView
        cnn.Close()
    End Sub

    Private Sub CLOSING_STOCKS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        cnn1.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\Closing.mdb"
        loadDataclosingstock()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Dim cmd As New OleDb.OleDbCommand
            Dim ProductID As String
            Dim ProductName As String
            Dim Stocks As String

            If Not cnn1.State = ConnectionState.Open Then
                'open connection if not open yet
                cnn1.Open()
            End If
            cmd.Connection = cnn1
            For x As Integer = 0 To DataGridView7.Rows.Count - 1
                ProductID = DataGridView7.Rows(x).Cells(0).Value
                ProductName = DataGridView7.Rows(x).Cells(1).Value
                Stocks = DataGridView7.Rows(x).Cells(3).Value
                'add data to table 
                cmd.CommandText = "INSERT INTO ClosingStock (Item_Code, Item_Name, Stocks, Dates) " & _
                    " VALUES('" & ProductID & "','" & ProductName & "','" & Stocks & "','" & _
                    DateTimePicker1.Value.Date & "')"
                cmd.ExecuteNonQuery()
            Next
            cnn1.Close()
            MsgBox("Stock Closed Successfully")
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class