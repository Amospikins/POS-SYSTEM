Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Data.OleDb
Public Class Stocks_In_Summary
    Dim cnn As New OleDb.OleDbConnection
    Dim provider As String
    Dim dataFile As String
    Dim connString As String
    Public myConnection As OleDbConnection = New OleDbConnection
    Public dr As OleDbDataReader
    Dim dtable As DataTable
    Private mRow As Integer = 0
    Private newpage As Boolean = True
    Private Sub loadData()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            '    Dim da As New OleDbDataAdapter("select Products.Product_Name as [Product], Qty as[Quantity], DateIn as [Date of Addition] from StockIn Inner Join Products On StockIn.Product_ID = Products.Product_ID where DateIn='" & Label3.Text & "'", cnn)
            Dim sqlText = "select DISTINCT Products.Item_Name as [Item Name], SUM(Qty) as [Quantity] from StockIn Inner Join Products On StockIn.Item_Code = Products.Item_Code where DateIn between @dt and @dh group by Products.Item_Name"
            'Dim sqlText = "select DISTINCT Item_Code as [Item Code], SUM(Qty) as [Quantity] from StockIn where DateIn between @dt and @dh group by Item_Code"
            Dim da As New OleDbDataAdapter(sqlText, cnn)
            da.SelectCommand.Parameters.Add("@dt", OleDbType.Date).Value = Stocks_in_Report.DateTimePicker1.Value.Date
            da.SelectCommand.Parameters.Add("@dh", OleDbType.Date).Value = Stocks_in_Report.DateTimePicker2.Value.Date
            da.Fill(dt)
            DataGridView1.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Stocks_In_Summary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        loadData()
        lblperiod.Text = "Period: " & Stocks_in_Report.DateTimePicker1.Text & "  to  " & Stocks_in_Report.DateTimePicker2.Text

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If DataGridView1.RowCount = Nothing Then
            MessageBox.Show("Sorry nothing to export into excel sheet.." & vbCrLf & "Please retrieve data in datagridview", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim rowsTotal, colsTotal As Short
        Dim I, j, iC As Short
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim xlApp As New Excel.Application

        Try
            Dim excelBook As Excel.Workbook = xlApp.Workbooks.Add
            Dim excelWorksheet As Excel.Worksheet = CType(excelBook.Worksheets(1), Excel.Worksheet)
            xlApp.Visible = True

            rowsTotal = DataGridView1.RowCount
            colsTotal = DataGridView1.Columns.Count - 1
            With excelWorksheet
                .Cells.Select()
                .Cells.Delete()
                For iC = 0 To colsTotal
                    .Cells(1, iC + 1).Value = DataGridView1.Columns(iC).HeaderText
                Next
                For I = 0 To rowsTotal - 1
                    For j = 0 To colsTotal
                        .Cells(I + 2, j + 1).value = DataGridView1.Rows(I).Cells(j).Value
                    Next j
                Next I
                .Rows("1:1").Font.FontStyle = "Bold"
                .Rows("1:1").Font.Size = 12

                .Cells.Columns.AutoFit()
                .Cells.Select()
                .Cells.EntireColumn.AutoFit()
                .Cells(1, 1).Select()
            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'RELEASE ALLOACTED RESOURCES
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            xlApp = Nothing
        End Try
    End Sub
    Private Sub PrintDocument1_BeginPrint(sender As Object, e As Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        mRow = 0
        newpage = True
        PrintPreviewDialog1.PrintPreviewControl.StartPage = 0
        PrintPreviewDialog1.PrintPreviewControl.Zoom = 1.0
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Try
            Dim HeaderFont As Font = New Drawing.Font("Times New Roman", 12)
            Dim reportFont As Font = New Drawing.Font("Times New Roman", 12)
            Dim nrmlfnt As Font = New Drawing.Font("Times New Roman", 12)
            Dim drawBrush As New SolidBrush(Color.Black)
            Dim blackpen As New Pen(Color.Black, 1)

            e.Graphics.DrawString("THE HEXAGON PLACE", HeaderFont, drawBrush, 300, 30)
            e.Graphics.DrawString(Label4.Text, reportFont, drawBrush, 310, 49)
            e.Graphics.DrawString(lblperiod.Text & "", nrmlfnt, drawBrush, 170, 70)
            '  e.Graphics.DrawString(totalamount.Text, reportFont, drawBrush, 300, 90)
            ' e.Graphics.DrawString(lbltransactionno.Text & "", nrmlfnt, drawBrush, 300, 110)
            ' sets it to show '...' for long text
            Dim fmt As StringFormat = New StringFormat(StringFormatFlags.LineLimit)
            fmt.LineAlignment = StringAlignment.Center
            fmt.Trimming = StringTrimming.EllipsisCharacter
            Dim y As Int32 = e.MarginBounds.Top
            Dim rc As Rectangle
            Dim x As Int32
            Dim h As Int32 = 0
            Dim row As DataGridViewRow

            ' print the header text for a new page
            '   use a grey bg just like the control
            If newpage Then
                row = DataGridView1.Rows(mRow)
                x = e.MarginBounds.Left
                For Each cell As DataGridViewCell In row.Cells
                    ' since we are printing the control's view,
                    ' skip invidible columns
                    If cell.Visible Then
                        rc = New Rectangle(x, y, cell.Size.Width, cell.Size.Height)



                        ' reused in the data pront - should be a function
                        Select Case DataGridView1.Columns(cell.ColumnIndex).DefaultCellStyle.Alignment
                            Case DataGridViewContentAlignment.BottomRight,
                                 DataGridViewContentAlignment.MiddleRight
                                fmt.Alignment = StringAlignment.Far
                                rc.Offset(-1, 0)
                            Case DataGridViewContentAlignment.BottomCenter,
                                DataGridViewContentAlignment.MiddleCenter
                                fmt.Alignment = StringAlignment.Center
                            Case Else
                                fmt.Alignment = StringAlignment.Near
                                rc.Offset(2, 50)
                        End Select

                        e.Graphics.DrawString(DataGridView1.Columns(cell.ColumnIndex).HeaderText,
                                                   DataGridView1.Font, Brushes.Black, rc, fmt)
                        x += rc.Width
                        h = Math.Max(h, rc.Height)
                    End If
                Next
                y += h

            End If
            newpage = False

            ' now print the data for each row
            Dim thisNDX As Int32
            For thisNDX = mRow To DataGridView1.RowCount - 1
                ' no need to try to print the new row
                If DataGridView1.Rows(thisNDX).IsNewRow Then Exit For

                row = DataGridView1.Rows(thisNDX)
                x = e.MarginBounds.Left
                h = 0

                ' reset X for data
                x = e.MarginBounds.Left

                ' print the data
                For Each cell As DataGridViewCell In row.Cells
                    If cell.Visible Then
                        rc = New Rectangle(x, y, cell.Size.Width, cell.Size.Height)

                        ' pick up any RowPrePaint rule
                        If Convert.ToString(row.Cells(0).Value.ToString) = "NG" Then
                            Using br As New SolidBrush(Color.MistyRose)
                                e.Graphics.FillRectangle(br, rc)
                            End Using
                        End If



                        Select Case DataGridView1.Columns(cell.ColumnIndex).DefaultCellStyle.Alignment
                            Case DataGridViewContentAlignment.BottomRight,
                                 DataGridViewContentAlignment.MiddleRight
                                fmt.Alignment = StringAlignment.Far
                                rc.Offset(-1, 0)
                            Case DataGridViewContentAlignment.BottomCenter,
                                DataGridViewContentAlignment.MiddleCenter
                                fmt.Alignment = StringAlignment.Center
                            Case Else
                                fmt.Alignment = StringAlignment.Near
                                rc.Offset(2, 50)
                        End Select

                        e.Graphics.DrawString(cell.FormattedValue.ToString(),
                                             DataGridView1.Font, Brushes.Black, rc, fmt)

                        x += rc.Width
                        h = Math.Max(h, rc.Height)
                    End If


                Next
                y += h
                ' next row to print
                mRow = thisNDX + 1

                If y + h > e.MarginBounds.Bottom Then
                    e.HasMorePages = True
                    mRow -= 1   'causes last row to rePrint on next page
                    newpage = True
                    Return
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        mRow = 0
        newpage = True

        PrintPreviewDialog1.Document = PrintDocument1
        'optionally reset the first page shown
        PrintPreviewDialog1.PrintPreviewControl.StartPage = 0
        PrintPreviewDialog1.WindowState = FormWindowState.Maximized
        PrintPreviewDialog1.ShowDialog()
    End Sub
End Class