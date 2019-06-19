Imports System.Data.OleDb
Public Class stocksin
    Dim cnn As New OleDb.OleDbConnection
    Private Sub loadData()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim da As New OleDbDataAdapter("select Item_Code as [Item Code], Item_Name as [Item Name], Price, Stocks as [Stock], Reorder_Level as [Reorder Level] from Products", cnn)
        da.Fill(dt)
        DataGridView1.DataSource = dt.DefaultView
        cnn.Close()
    End Sub
    Private Sub search()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim da As New OleDbDataAdapter("select Item_Code as [Item Code], Item_Name as [Item Name], Price, Stocks as [Stock], Reorder_Level as [Reorder Level] from Products where Item_Name like '" & txtsearch.Text & "%'", cnn)
        da.Fill(dt)
        DataGridView1.DataSource = dt.DefaultView
        cnn.Close()
    End Sub
    Private Sub StockOutUpdate()
        Try
            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                'open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            'add data to table 
            cmd.CommandText = "INSERT INTO StockIn (Item_Code, Qty, DateIn) " & _
                " VALUES('" & Me.TXTCODE.Text & "','" & _
                Me.TXTSTOCSIN.Text & "','" & Date.Today & "')"
            cmd.ExecuteNonQuery()
            cnn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub storeOutupdateproduct()
        Try

            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                ' open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            cmd.CommandText = "update Products set Stocks='" & TXTTOTAL.Text & "' WHERE Item_Code='" & TXTCODE.Text & "'"
            cmd.ExecuteNonQuery()
            MsgBox("Stock Added Successfully")
            loadData()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub stocksin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        loadData()
    End Sub

    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        search()
    End Sub

    Private Sub DataGridView1_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.RowHeaderMouseClick
        Try
            Dim dr As DataGridViewRow = DataGridView1.SelectedRows(0)
            TXTCODE.Text = dr.Cells(0).Value.ToString()
            TXTNAME.Text = dr.Cells(1).Value.ToString()
            TXTSTOCKS.Text = dr.Cells(3).Value.ToString()
            TXTTOTAL.Text = TXTSTOCKS.Text
            txtsearch.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub TXTSTOCSIN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TXTSTOCSIN.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TXTSTOCSIN.TextChanged
        TXTTOTAL.Text = Val(TXTSTOCSIN.Text) + Val(TXTSTOCKS.Text)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        StockOutUpdate()
        storeOutupdateproduct()
        TXTSTOCSIN.Clear()
        TXTCODE.Clear()
        TXTNAME.Clear()
        TXTSTOCKS.Clear()
        TXTTOTAL.Clear()
        txtsearch.Clear()

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class