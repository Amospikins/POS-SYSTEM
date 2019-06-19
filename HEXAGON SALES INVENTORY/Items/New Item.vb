Imports System.Data.OleDb
Imports System.Text
Imports System.Security.Cryptography
Public Class New_Item
    Dim cnn As New OleDb.OleDbConnection
    Dim dtable As DataTable
    Public Shared Function GenKey(ByVal maxSize As Integer) As String
        Dim chars As Char() = New Char(61) {}
        chars = "123456789".ToCharArray()
        Dim data As Byte() = New Byte(0) {}
        Dim crypto As New RNGCryptoServiceProvider()
        crypto.GetNonZeroBytes(data)
        data = New Byte(maxSize - 1) {}
        crypto.GetNonZeroBytes(data)
        Dim result As New StringBuilder(maxSize)
        For Each b As Byte In data
            result.Append(chars(b Mod (chars.Length)))
        Next
        Return result.ToString()
    End Function
    Private Sub search()
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        Dim da As New OleDbDataAdapter("select Item_Code as [Item Code], Item_Name as [Item Name], Price, Stocks as [Stock], Reorder_Level as [Reorder Level], Category from Products where Item_Name like '" & txtsearch.Text & "%'", cnn)
        da.Fill(dt)
        DataGridView1.DataSource = dt.DefaultView
        cnn.Close()
    End Sub
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
    Private Sub emptytxt()
        TXTCODE.Text = String.Empty
        TXTNAME.Text = String.Empty
        TXTPRICE.Text = String.Empty
        TXTREO.Text = String.Empty
        TXTSTOCKS.Text = String.Empty
        cbocat.SelectedIndex = 0
    End Sub
    Private Sub New_Item_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        fillCombo1()
        cbocat.SelectedIndex = 0
        loadData()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Dim f As New editviewfrm
        Home.PanelHome.Controls.Clear()
        f.TopLevel = False
        f.Size = Home.PanelHome.Size
        f.Visible = True
        Home.PanelHome.Controls.Add(f)
        f.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TXTCODE.Text = String.Empty & "PC-" & GenKey(4)
    End Sub

    Private Sub BTNADD_Click(sender As Object, e As EventArgs) Handles BTNADD.Click
        Try
            If TXTCODE.Text Is String.Empty Or TXTNAME.Text Is String.Empty Or cbocat.SelectedIndex = 0 Or TXTPRICE.Text = String.Empty Or TXTSTOCKS.Text = String.Empty Or TXTREO.Text = String.Empty Then
                MessageBox.Show("Please enter all fields", "The Hexagon Place", MessageBoxButtons.OK)
                TXTNAME.Focus()
            Else
                Dim cmd As New OleDb.OleDbCommand
                If Not cnn.State = ConnectionState.Open Then
                    'open connection if not open yet
                    cnn.Open()
                End If
                cmd.Connection = cnn
                'add data to table 
                cmd.CommandText = "INSERT INTO Products (Item_Code, Item_Name, Price, Stocks, Reorder_Level, Category) " &
                    " VALUES('" & Me.TXTCODE.Text & "','" & Me.TXTNAME.Text & "','" &
                    Me.TXTPRICE.Text & "','" & Me.TXTSTOCKS.Text & "','" & Me.TXTREO.Text & "','" & Me.cbocat.Text & "')"
                cmd.ExecuteNonQuery()
                cnn.Close()
                MessageBox.Show("Item Added Successfully", "The Hexagon Place", MessageBoxButtons.OK)
                emptytxt()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub TXTNAME_TextChanged(sender As Object, e As EventArgs) Handles TXTNAME.TextChanged

    End Sub

    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        search()
    End Sub

    Private Sub SplitContainer1_Panel1_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel1.Paint

    End Sub

    Private Sub TXTPRICE_TextChanged(sender As Object, e As EventArgs) Handles TXTPRICE.TextChanged

    End Sub

    Private Sub TXTPRICE_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TXTPRICE.KeyPress

    End Sub
End Class