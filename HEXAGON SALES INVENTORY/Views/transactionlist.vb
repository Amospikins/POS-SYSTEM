Imports System.Data.OleDb
Public Class transactionlist
    Dim cnn As New OleDb.OleDbConnection
    Dim provider As String
    Dim dataFile As String
    Dim connString As String
    Public myConnection As OleDbConnection = New OleDbConnection
    Public dr As OleDbDataReader
    Dim dtable As DataTable
    Private Sub loadData()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim sqlText = "select ReceiptNo as [Receipt No], Dates as [Date], Times as [Time], TotalAmount as [Total Amount], StaffName as [Staff Name], PaymentMethod as [Payment Method], CashPaid as [Cash Paid], Change from Receipt where Dates between @dt and @dh order by Dates"
            ds.Tables.Add(dt)
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = DateTimePicker1.Value.Date
            da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = DateTimePicker2.Value.Date
            da.Fill(dt)
            DataGridView1.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub loadData1()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim sqlText = "select ReceiptNo as [Receipt No], Dates as [Date], Times as [Time], TotalAmount as [Total Amount], StaffName as [Staff Name], PaymentMethod as [Payment Method], CashPaid as [Cash Paid], Change from Receipt order by ReceiptNo"
            ds.Tables.Add(dt)
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.Fill(dt)
            DataGridView1.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub datatotalStock()
        Try
            Dim GrandTotal As Double = 0
            For i As Integer = 0 To DataGridView1.RowCount - 1

                GrandTotal += Val(DataGridView1.Rows(i).Cells(3).Value)

                'Change the number 2 to your column index number (The first column has a 0 index column)
                'In this example the column index of Price is 2
            Next

            totalamount.Text = "Total Amount: " & Format(GrandTotal, "#,##0")
            lbltransactionno.Text = "Number of Transactions: " & DataGridView1.Rows.Count

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cnn.Close()
        End Try
    End Sub
    Private Sub transactionlist_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        loadData1()
        datatotalStock()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        loadData()
        datatotalStock()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        loadData1()
        datatotalStock()
    End Sub
End Class