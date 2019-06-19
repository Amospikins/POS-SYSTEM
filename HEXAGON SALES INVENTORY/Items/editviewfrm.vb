Imports System.Data.OleDb
Imports Excel = Microsoft.Office.Interop.Excel
Public Class editviewfrm
    Dim cnn As New OleDb.OleDbConnection
    Dim dtable As DataTable
    Private Sub fillCombo1()
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim da As New OleDbDataAdapter("select Category_Name from Category", cnn)
        da.Fill(ds)
        dtable = ds.Tables(0)
        cbocat.Items.Clear()
        For Each drow As DataRow In dtable.Rows
            cbocat.Items.Add(drow(0).ToString())
        Next
        cnn.Close()
    End Sub
    Private Sub loadData()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim da As New OleDbDataAdapter("select Item_Code as [Item Code], Item_Name as [Item Name], Price, Stocks as [Stock], Reorder_Level as [Reorder Level], Category from Products", cnn)
        da.Fill(dt)
        DataGridView1.DataSource = dt.DefaultView
        cnn.Close()
    End Sub
    Private Sub search()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim da As New OleDbDataAdapter("select Item_Code as [Item Code], Item_Name as [Item Name], Price, Stocks as [Stock], Reorder_Level as [Reorder Level], Category from Products where Item_Name like '" & txtsearch.Text & "%'", cnn)
        da.Fill(dt)
        DataGridView1.DataSource = dt.DefaultView
        cnn.Close()
    End Sub
    Private Sub datatotalStock()
        Try
            Dim GrandTotal As Double = 0
            For i As Integer = 0 To DataGridView1.RowCount - 1

                GrandTotal += Val(DataGridView1.Rows(i).Cells(4).Value)

                'Change the number 2 to your column index number (The first column has a 0 index column)
                'In this example the column index of Price is 2
            Next

            lbltotalitem.Text = Format(GrandTotal, "#,##0")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cnn.Close()
        End Try
    End Sub

    Private Sub editviewfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        txtsearch.Focus()
        loadData()
        fillCombo1()
        datatotalStock()
        cbocat.SelectedIndex = 0
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim f As New New_Item
        Home.PanelHome.Controls.Clear()
        f.TopLevel = False
        f.Size = Home.PanelHome.Size
        f.Visible = True
        Home.PanelHome.Controls.Add(f)
        f.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim a As New Category
        Home.PanelHome.Controls.Clear()
        a.TopLevel = False
        a.Size = Home.PanelHome.Size
        a.Visible = True
        Home.PanelHome.Controls.Add(a)
        a.Show()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        search()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        search()
    End Sub
    Private Sub DataGridView1_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.RowHeaderMouseClick
        Try
            Dim dr As DataGridViewRow = DataGridView1.SelectedRows(0)
            TXTCODE.Text = dr.Cells(0).Value.ToString()
            TXTNAME.Text = dr.Cells(1).Value.ToString()
            TXTPRICE.Text = dr.Cells(2).Value.ToString()
            TXTSTOCKS.Text = dr.Cells(3).Value.ToString()
            TXTREO.Text = dr.Cells(4).Value.ToString()
            cbocat.Text = dr.Cells(5).Value.ToString
            txtsearch.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            If TXTCODE.Text Is String.Empty Or TXTNAME.Text Is String.Empty Or TXTPRICE.Text = String.Empty Or TXTREO.Text = String.Empty Or TXTSTOCKS.Text = String.Empty Or cbocat.SelectedIndex = 0 Then
                MsgBox("Please enter all fields")
                TXTNAME.Focus()
            Else
                Dim cmd As New OleDb.OleDbCommand
                If Not cnn.State = ConnectionState.Open Then
                    ' open connection if not open yet
                    cnn.Open()
                End If
                cmd.Connection = cnn
                cmd.CommandText = "update Products set Item_Code='" & TXTCODE.Text & "', Item_Name='" & TXTNAME.Text & "', Price='" & TXTPRICE.Text & "', Stocks='" & TXTSTOCKS.Text & "', Reorder_Level='" & TXTREO.Text & "', Category ='" & cbocat.Text & "' WHERE Item_Code='" & TXTCODE.Text & "'"
                cmd.ExecuteNonQuery()
                MsgBox("Item Updated Successfully")
                loadData()
                TXTCODE.Clear()
                TXTNAME.Clear()
                TXTPRICE.Clear()
                TXTSTOCKS.Clear()
                TXTREO.Clear()
                cbocat.SelectedIndex = 0

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
       
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
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

    Private Sub TXTPRICE_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TXTPRICE.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TXTPRICE_TextChanged(sender As Object, e As EventArgs) Handles TXTPRICE.TextChanged

    End Sub

    Private Sub TXTSTOCKS_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TXTSTOCKS.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TXTSTOCKS_TextChanged(sender As Object, e As EventArgs) Handles TXTSTOCKS.TextChanged

    End Sub

    Private Sub TXTREO_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TXTREO.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If TXTCODE.Text Is String.Empty Or TXTNAME.Text Is String.Empty Or TXTPRICE.Text = String.Empty Or TXTREO.Text = String.Empty Or TXTSTOCKS.Text = String.Empty Or cbocat.SelectedIndex = 0 Then
                MsgBox("Please enter all fields")
                TXTNAME.Focus()
            Else
                Dim cmd As New OleDb.OleDbCommand
                If Not cnn.State = ConnectionState.Open Then
                    'open connection if not open yet
                    cnn.Open()
                End If
                cmd.Connection = cnn
                cmd.CommandText = "DELETE from Products where Item_Code='" & TXTCODE.Text & "'"
                cmd.ExecuteNonQuery()
                MessageBox.Show("Deleted", "User Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                loadData()
                TXTCODE.Clear()
                TXTNAME.Clear()
                TXTPRICE.Clear()
                TXTSTOCKS.Clear()
                TXTREO.Clear()
                cbocat.SelectedIndex = 0
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class