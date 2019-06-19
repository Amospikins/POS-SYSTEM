Imports System.Data.OleDb
Public Class Summary_Closing
    Dim cnn As New OleDb.OleDbConnection
    Dim cnn1 As New OleDb.OleDbConnection
    Dim provider As String
    Dim dataFile As String
    Dim connString As String
    Public myConnection As OleDbConnection = New OleDbConnection
    Public dr As OleDbDataReader
    Dim dtable As DataTable
    Private Sub datatotalStock()
        Try
            Dim GrandTotal As Double = 0
            For i As Integer = 0 To DataGridView1.RowCount - 1

                GrandTotal += Val(DataGridView1.Rows(i).Cells(2).Value)

                'Change the number 2 to your column index number (The first column has a 0 index column)
                'In this example the column index of Price is 2
            Next
            totalamount.Text = "Total Amount: " & Format(GrandTotal, "#,##0")
            lbltransactionno.Text = "No. of Transactions: " & DataGridView1.Rows.Count
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cnn.Close()
        End Try
     
    End Sub
    Private Sub grnd()
        Try
            Dim Total As Double = 0
            For i As Integer = 0 To DataGridView2.RowCount - 1

                Total += Val(DataGridView2.Rows(i).Cells(1).Value)

                'Change the number 2 to your column index number (The first column has a 0 index column)
                'In this example the column index of Price is 2
            Next

            Label5.Text = "Total Amount: " & Format(Total, "#,##0")
            Label6.Text = "No. of Staff: " & DataGridView2.Rows.Count

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cnn.Close()
        End Try
    End Sub
    Private Sub datatotalS()
        Try
            Dim GrandTotal As Double = 0
            For i As Integer = 0 To DataGridView3.RowCount - 1

                GrandTotal += Val(DataGridView3.Rows(i).Cells(1).Value)

                'Change the number 2 to your column index number (The first column has a 0 index column)
                'In this example the column index of Price is 2
            Next

            Label8.Text = "Total Amount: " & Format(GrandTotal, "#,##0")
            Label9.Text = "No. of Staff: " & DataGridView3.Rows.Count

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cnn.Close()
        End Try
    End Sub
    Private Sub datatotalStockin()
        Try
            Dim GrandTotal As Double = 0
            For i As Integer = 0 To DataGridView4.RowCount - 1

                GrandTotal += Val(DataGridView4.Rows(i).Cells(2).Value)

                'Change the number 2 to your column index number (The first column has a 0 index column)
                'In this example the column index of Price is 2
            Next

            Label11.Text = "Total Sales: " & Format(GrandTotal, "#,##0")
            Label12.Text = "No of Items: " & DataGridView4.Rows.Count

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cnn.Close()
        End Try
    End Sub
    Private Sub datatotalStockins()
        Try
            Dim GrandTotal As Double = 0
         

            lblstockcount.Text = "No. of Stocks In: " & DataGridView5.Rows.Count

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cnn.Close()
        End Try

    End Sub
    Private Sub loadtransactions()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim sqlText = "select ReceiptNo as [Receipt No], Dates as [Date], TotalAmount as [Total Amount] from Receipt where Dates between @dt and @dh order by Dates"
            ds.Tables.Add(dt)
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker1.Value.Date
            da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker2.Value.Date
            da.Fill(dt)
            DataGridView1.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub loadSalesPeopleData()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            Dim sqlText = "select DISTINCT StaffName as [Staff Name], SUM(TotalAmount) as [Total Amount] from Receipt where Dates between @dt and @dh group by StaffName"
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker1.Value.Date
            da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker2.Value.Date
            da.Fill(dt)
            DataGridView2.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub loadMethodPayment()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            Dim sqlText = "select DISTINCT PaymentMethod as [Staff Name], SUM(TotalAmount) as [Total Amount] from Receipt where Dates between @dt and @dh group by PaymentMethod"
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker1.Value.Date
            da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker2.Value.Date
            da.Fill(dt)
            DataGridView3.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub loadDataItems()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            Dim sqlText = "select DISTINCT Products.Item_Name as [Item Name], SUM(Qty) as [Quantity], SUM(TotalAmount) as [Total Amount] from TDetails Inner Join Products On TDetails.Item_Code = Products.Item_Code where Dates between @dt and @dh group by Products.Item_Name"
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker1.Value.Date
            da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker2.Value.Date
            da.Fill(dt)
            DataGridView4.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub loadDataStocksin()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            Dim sqlText = "select DISTINCT Products.Item_Name as [Item Name], SUM(Qty) as [Quantity] from StockIn Inner Join Products On StockIn.Item_Code = Products.Item_Code where DateIn between @dt and @dh group by Products.Item_Name"
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker1.Value.Date
            da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker2.Value.Date
            da.Fill(dt)
            DataGridView5.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub loadDataclosingstockBYDATE()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim sqlText = "select Item_Code as [Item Code], Item_Name as [Item Name], Stocks as [Stock] from ClosingStock where Dates between @dt and @dh"
        Dim da As New OleDbDataAdapter(sqlText, cnn1)
        da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker1.Value.Date
        da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = Report_Period_Summary.DateTimePicker2.Value.Date
        da.Fill(dt)
        DataGridView6.DataSource = dt.DefaultView
        cnn1.Close()
    End Sub
    Private Sub Summary_Closing_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        cnn1.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\Closing.mdb"
        loadtransactions()
        datatotalStock()
        loadSalesPeopleData()
        grnd()
        loadMethodPayment()
        datatotalS()
        loadDataItems()
        datatotalStockin()
        loadDataStocksin()
        datatotalStockins()
        loadDataclosingstockBYDATE()
        lbldates.Text = "Period: " & Report_Period_Summary.DateTimePicker1.Text & "  to  " & Report_Period_Summary.DateTimePicker2.Text
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)

    End Sub
End Class