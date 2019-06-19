Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Data.OleDb

Public Class OutletReport
    Dim cnn As New OleDb.OleDbConnection
    Dim provider As String
    Dim dataFile As String
    Dim connString As String
    Public myConnection As OleDbConnection = New OleDbConnection
    Public dr As OleDbDataReader
    Dim dtable As DataTable
    Private mRow As Integer = 0
    Private newpage As Boolean = True
    Private Sub datatotalStock()
        Try
            Dim GrandTotal As Double = 0
            For i As Integer = 0 To DataGridView1.RowCount - 1

                GrandTotal += Val(DataGridView1.Rows(i).Cells(1).Value)

                'Change the number 2 to your column index number (The first column has a 0 index column)
                'In this example the column index of Price is 2
            Next

            totalamount.Text = "Total Amount: " & Format(GrandTotal, "#,##0")
            lbltransactionno.Text = "Number of Outlets: " & DataGridView1.Rows.Count

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cnn.Close()
        End Try
    End Sub
    Private Sub loadData()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            'Dim sqlText = "select DISTINCT Products.Item_Name as [Item Name], SUM(Qty) as [Quantity], SUM(TotalAmount) as [Total Amount] from TDetails Inner Join Products On TDetails.Item_Code = Products.Item_Code where Dates between @dt and @dh group by Products.Item_Name"
            Dim sqlText = "select DISTINCT Outlet as [Outlets], SUM(TotalAmount) as [Total Amount] from TDetails where Dates between @dt and @dh group by Outlet"
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = report_perior5.DateTimePicker1.Value.Date
            da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = report_perior5.DateTimePicker2.Value.Date
            da.Fill(dt)
            DataGridView1.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub OutletReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        loadData()
        lblperiod.Text = "Period: " & report_perior5.DateTimePicker1.Text & "  to  " & report_perior5.DateTimePicker2.Text
        datatotalStock()
    End Sub
End Class