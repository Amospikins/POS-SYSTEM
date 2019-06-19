Imports System.Data.OleDb
Imports System.Text
Imports System
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.IO
Public Class point_of_sale
    ReadOnly ValidChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789, "
    Dim cnn As New OleDb.OleDbConnection
    Dim provider As String
    Dim dataFile As String
    Dim connString As String
    Public myConnection As OleDbConnection = New OleDbConnection
    Public dr As OleDbDataReader
    Dim dtable As DataTable
    Public Const eClear As String = Chr(27) + "@"
    Public Const eCentre As String = Chr(27) + Chr(97) + "1"
    Public Const eLeft As String = Chr(27) + Chr(97) + "0"
    Public Const eRight As String = Chr(27) + Chr(97) + "2"
    Public Const eDrawer As String = eClear + Chr(27) + "p" + Chr(0) + ".}"
    Public Const eCut As String = Chr(27) + "i" + vbCrLf
    Public Const eSmlText As String = Chr(27) + "!" + Chr(1)
    Public Const eNmlText As String = Chr(27) + "!" + Chr(0)
    Public Const eInit As String = eNmlText + Chr(13) + Chr(27) + "c6" + Chr(1) + Chr(27) + "R3" + vbCrLf
    Public Const eBigCharOn As String = Chr(27) + "!" + Chr(56)
    Public Const eBigCharOff As String = Chr(27) + "!" + Chr(0)
    Private prn As New RawPrinterHelper
    Private PrinterName As String = "POS-58"

    Private Sub searchdata()
        Try

            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            Dim da As New OleDbDataAdapter("select Dates, Item_Code, Item_Name, Qty as [Quantity], Price, TotalAmount as [Total] from TDetails where ReceiptNo like'" & txtsearchCode.Text & "%'", cnn)
            da.Fill(dt)
            DataGridView3.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub loadData()
        Try

            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            Dim da As New OleDbDataAdapter("select Item_Name as [Item Name], Stocks as [Stock], Price from Products", cnn)
            da.Fill(dt)
            DataGridView2.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub search()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            Dim da As New OleDbDataAdapter("select Item_Name as [Item Name], Price, Stocks as [Stock] from Products where Item_Name like '" & txtsearch.Text & "%'", cnn)
            da.Fill(dt)
            DataGridView2.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub search1()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            ds.Tables.Add(dt)
            Dim da As New OleDbDataAdapter("select Item_Name as [Item Name], Price, Stocks as [Stock] from Products where Item_Name like '" & txtsearch.Text & "%'", cnn)
            da.Fill(dt)
            DataGridView2.DataSource = dt.DefaultView
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub resetting()
        TXTCODE.Clear()
        TXTNAME.Clear()
        TXTPRICE.Clear()
        TXTSTOCKS.Clear()
        txtsearch.Clear()
        txttotal.Text = 0
        txtqty.Text = 1
    End Sub
    Private Sub fillCombo()
        Dim ds As New DataSet
        Dim dt As New DataTable
        If Role = "CASHIER" Then
            Dim da As New OleDbDataAdapter("select FirstName from Users WHERE FirstName='" & FirstName & "'", cnn)
            da.Fill(ds)
            dtable = ds.Tables(0)
            cbowaiter.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cbowaiter.Items.Add(drow(0).ToString())
            Next
            cnn.Close()
        Else
            Dim da As New OleDbDataAdapter("select Staff_Name from Staff", cnn)
            da.Fill(ds)
            dtable = ds.Tables(0)
            cbowaiter.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cbowaiter.Items.Add(drow(0).ToString())
            Next
            cnn.Close()
        End If
    End Sub
    Private Sub fillCombo2()
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim da As New OleDbDataAdapter("select Outlet from Outlets", cnn)
        da.Fill(ds)
        dtable = ds.Tables(0)
        cboOutlet.Items.Clear()
        For Each drow As DataRow In dtable.Rows
            cboOutlet.Items.Add(drow(0).ToString())
        Next
        cnn.Close()
    End Sub
    Private Sub returditem()
         Try
            myConnection.Open()
            Dim str As String
            Dim holder As Integer
            str = "SELECT Stocks FROM Products WHERE Item_Code='" & TXTCODE.Text & "'"
            Dim cmds As OleDbCommand = New OleDbCommand(str, myConnection)
            dr = cmds.ExecuteReader
            If dr.Read() Then
                If dr(0) Then
                    holder = dr(0)

                End If
            End If
            myConnection.Close()
            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                ' open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            cmd.CommandText = "update Products set Stocks='" & holder + Val(txtqty.Text) & "' WHERE Item_Code='" & TXTCODE.Text & "'"
            cmd.ExecuteNonQuery()
            txtamountdue.Text = Val(txtamountdue.Text) - Val(txttotal.Text)
            loadData()
            resetting()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub edititem()
        Try
            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                ' open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            cmd.CommandText = "update Receipt set Dates='" & DateTimePicker1.Value.Date & "', Times='" & TimeOfDay & "', TotalAmount='" & txtamountdue.Text & "', PaymentMethod='" & cbomodeofpayment.Text & "', StaffName='" & cbowaiter.Text & "', Outlet='" & cboOutlet.Text & "' WHERE ReceiptNo like'" & txtsearchCode.Text & "'"
            cmd.ExecuteNonQuery()

            loadData()
            resetting()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub retrievewaiter()
        Try
            myConnection.Open()
            Dim str As String
            str = "SELECT * FROM Receipt WHERE ReceiptNo like'" & txtsearchCode.Text & "'"
            Dim cmds As OleDbCommand = New OleDbCommand(str, myConnection)
            dr = cmds.ExecuteReader
            If dr.Read() Then
                '   txtcashpaid.Text = dr("CashPaid")
                ' txtchange.Text = dr("Change")
                cbowaiter.Text = dr("StaffName")
                cbomodeofpayment.Text = dr("PaymentMethod")
                cboOutlet.Text = dr("Outlet")
            End If
            myConnection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub deleteTDetails()
        Try
            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                'open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            cmd.CommandText = "DELETE from TDetails where ReceiptNo='" & txtsearchCode.Text & "'"
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Try
          
            Dim cmd As New OleDb.OleDbCommand
            Dim Item_Code As String
            Dim Quantity As String
            Dim Price As String
            Dim Tot As String
            Dim item As String
            If Not cnn.State = ConnectionState.Open Then
                'open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            For x As Integer = 0 To DataGridView1.Rows.Count - 1
                Item_Code = DataGridView1.Rows(x).Cells(1).Value
                item = DataGridView1.Rows(x).Cells(2).Value
                Quantity = DataGridView1.Rows(x).Cells(3).Value
                Price = DataGridView1.Rows(x).Cells(4).Value
                Tot = DataGridView1.Rows(x).Cells(5).Value
                'add data to table 
                cmd.CommandText = "INSERT INTO TDetails (ReceiptNo, Item_Code, Qty, Price, TotalAmount, Dates, Item_Name, Outlet) " & _
                    " VALUES('" & txtsearchCode.Text & "','" & Item_Code & "','" & _
                    Quantity & "','" & Price & "','" & Tot & "','" & DateTimePicker1.Value.Date & "','" & item & "','" & cboOutlet.Text & "')"
                cmd.ExecuteNonQuery()
            Next
            cnn.Close()
            MsgBox("Transaction Edited Successfully")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub deleteTdetailsandReceipt()
        Try
            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                'open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            cmd.CommandText = "DELETE from TDetails where ReceiptNo='" & txtsearchCode.Text & "'"
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Try
            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                'open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            cmd.CommandText = "DELETE from Receipt where ReceiptNo like'" & txtsearchCode.Text & "'"
            cmd.ExecuteNonQuery()
            cnn.Close()
            MsgBox("Transaction Voided Successfully")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub process()
        Try
            Dim cmd As New OleDb.OleDbCommand
            If Not cnn.State = ConnectionState.Open Then
                'open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            'add data to table 
            cmd.CommandText = "INSERT INTO Receipt (ReceiptNo, Dates, Times, TotalAmount, StaffName, PaymentMethod, Outlet) " &
                " VALUES('" & lblReceiptId.Text & "','" & DateTimePicker1.Value.Date & "','" & TimeOfDay & "','" & txtamountdue.Text & "','" & cbowaiter.Text & "','" & cbomodeofpayment.Text & "','" & cboOutlet.Text & "')"
            cmd.ExecuteNonQuery()
            cnn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub receiptId()
        Try
            myConnection.Open()
            Dim str As String
            Dim holder As Integer
            Dim dd As DataGridViewRow = DataGridView1.SelectedRows(0)
            str = "SELECT MAX(ReceiptNo) FROM Receipt"
            Dim cmds As OleDbCommand = New OleDbCommand(str, myConnection)
            dr = cmds.ExecuteReader
            If dr.Read() Then
                If dr(0) Then
                    holder = dr(0)

                End If
            End If
            myConnection.Close()
            Dim cmd As New OleDb.OleDbCommand
            Dim Item_Code As String
            Dim Quantity As String
            Dim Price As String
            Dim Tot As String
            Dim item As String
            If Not cnn.State = ConnectionState.Open Then
                'open connection if not open yet
                cnn.Open()
            End If
            cmd.Connection = cnn
            For x As Integer = 0 To DataGridView1.Rows.Count - 1
                Item_Code = DataGridView1.Rows(x).Cells(1).Value
                item = DataGridView1.Rows(x).Cells(2).Value
                Quantity = DataGridView1.Rows(x).Cells(3).Value
                Price = DataGridView1.Rows(x).Cells(4).Value
                Tot = DataGridView1.Rows(x).Cells(5).Value
                'add data to table 
                cmd.CommandText = "INSERT INTO TDetails (ReceiptNo, Item_Code, Qty, Price, TotalAmount, Dates, Item_Name, Outlet) " & _
                    " VALUES('" & holder & "','" & Item_Code & "','" & _
                    Quantity & "','" & Price & "','" & Tot & "','" & DateTimePicker1.Value.Date & "','" & item & "','" & cboOutlet.Text & "')"
                cmd.ExecuteNonQuery()
            Next
            cnn.Close()
            MsgBox("Transaction Successfully")
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
            cmd.CommandText = "update Products set Stocks='" & TXTNEWSTOCK.Text & "' WHERE Item_Code='" & TXTCODE.Text & "'"
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnrem_Click(sender As Object, e As EventArgs) Handles btnrem.Click
        Try
            If DataGridView1.SelectedRows.Count > 0 Then
                For Each row As DataGridViewRow In DataGridView1.SelectedRows


                    TXTCODE.Text = row.Cells(1).Value
                    TXTNAME.Text = row.Cells(2).Value
                    txtqty.Text = row.Cells(3).Value
                    TXTPRICE.Text = row.Cells(4).Value
                    txttotal.Text = (row.Cells(5).Value)
                    returditem()
                    'you may want to add a confirmation message, and if the user confirms delete
                    DataGridView1.Rows.Remove(row)
                Next
            Else
                MessageBox.Show("Select 1 row before you hit Delete")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        txtsearch.Focus()

    End Sub
    Private Sub receiptidgeneration()
        myConnection.Open()
        Dim str As String
        Dim holder As Integer
        str = "SELECT MAX(ReceiptNo) FROM Receipt"
        Dim cmds As OleDbCommand = New OleDbCommand(str, myConnection)
        dr = cmds.ExecuteReader
        If dr.Read() Then
            If dr(0) Then
                holder = dr(0)
                lblReceiptId.Text = holder + 1
            End If
        End If
        myConnection.Close()
    End Sub
    Private Sub point_of_sale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        provider = "Provider=Microsoft.Jet.Oledb.4.0; Data Source="
        dataFile = Application.StartupPath & "\DB.mdb" ' Change it to your Access Database location
        connString = provider & dataFile
        myConnection.ConnectionString = connString
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
        txtsearch.Focus()
        loadData()
        'txtcashpaid.Enabled = False
        '    txtchange.Enabled = False
        fillCombo()
        fillCombo2()
        Button1.Enabled = False
        Button8.Enabled = False
        Button3.Enabled = False
        cbomodeofpayment.SelectedIndex = 0
        cbowaiter.SelectedIndex = 0
        txtsearchCode.Enabled = False
        btnretrieve.Enabled = False
        DataGridView3.Enabled = False
        Button7.Enabled = False
        Button9.Enabled = False
        receiptidgeneration()
        Button5.Enabled = False
        Button10.Enabled = False
        Button11.Enabled = False
        Button12.Enabled = False
        cboOutlet.SelectedIndex = 0
        txtsearch.Focus()
    End Sub

    Public Sub PrintHeader()
        Dim dateholder As Date = Date.Today
        Dim timeholder As Date = TimeOfDay
        Print(eCentre + "THE HEXAGON PLACE")
        Print("2a, Golf Course Road GRA")
        Print(eCentre + "Benin City, Nigeria")
        Print(eCentre + "Contact No: 07068609724")
        Print(eBigCharOn + "Receipt")
        Print(eNmlText + "Transaction no: #" + lblReceiptId.Text)
        Print(vbNewLine)
        Print(eCentre + eSmlText + dateholder + "      " + timeholder)
        Print(eLeft + eBigCharOff + "--------------------------------")

    End Sub
    Public Sub PrintBody()
        Dim cmd As New OleDb.OleDbCommand
        Dim Item_Code As String
        Dim Quantity As String
        Dim Price As String
        Dim Tot As String
        Dim item As String

        Print(eSmlText + "Qty " + " Item Name   " + "   Price  " + "Total Amount")
        Print("------------------------------------------")
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            Item_Code = DataGridView1.Rows(x).Cells(1).Value
            item = DataGridView1.Rows(x).Cells(2).Value
            Quantity = DataGridView1.Rows(x).Cells(3).Value
            Price = DataGridView1.Rows(x).Cells(4).Value
            Tot = DataGridView1.Rows(x).Cells(5).Value

            'add data to table 
            Print(eRight + eSmlText + Quantity + " " + Microsoft.VisualBasic.Left(item, 20) + "  " + Price.ToString & "  " + Tot)
        Next
        Print("------------------------------------------")


        Print(eLeft + eNmlText + "Total Amount:  " + txtamountdue.Text)
        Print("Served by: " + cbowaiter.Text)
        Print("Outlet: " + cboOutlet.Text)
        Print("Operator: " + FirstName)
        Print(eSmlText + "------------------------------------------")
        Print(vbNewLine)

    End Sub

    Public Sub PrintFooter()
        Print(eCentre + "Thank You For Your Support!" + eLeft)
        Print(vbLf + vbLf + vbLf + vbLf + vbLf + eCut + eDrawer)
    End Sub

    Public Sub PrintFooter1()
        Print(eCentre + "MARCHANT COPY!" + eLeft)
        Print(vbLf + vbLf + vbLf + vbLf + vbLf + eCut + eDrawer)
    End Sub

    Private Sub txtsearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtsearch.KeyDown

    End Sub

    Private Sub txtsearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtsearch.KeyPress
        e.Handled = Not (ValidChars.IndexOf(e.KeyChar) > -1 _
                OrElse e.KeyChar = Convert.ToChar(Keys.Back))
    End Sub


    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged

        search()
        myConnection.Open()
        Dim str As String
        str = "SELECT * FROM Products WHERE (Item_Name like '" & txtsearch.Text & "')"
        Dim cmd As OleDbCommand = New OleDbCommand(str, myConnection)
        dr = cmd.ExecuteReader
        While dr.Read()
            TXTCODE.Text = dr("Item_Code").ToString
            TXTNAME.Text = dr("Item_Name").ToString
            TXTPRICE.Text = dr("Price").ToString
            TXTSTOCKS.Text = dr("Stocks").ToString
        End While
        myConnection.Close()
        If txtsearch.Text = String.Empty Then
            txttotal.Text = 0
        End If
        TXTNEWSTOCK.Text = Val(TXTSTOCKS.Text) - Val(txtqty.Text)
        If TXTNEWSTOCK.Text < 1 Then
            TXTNEWSTOCK.Text = 0
        End If
    End Sub
    Private Sub totals()
        For Each row As DataGridViewRow In Me.DataGridView1.Rows
            If row.Cells(1).Value = TXTCODE.Text Then
                row.Cells(3).Value = row.Cells(3).Value + Val(txtqty.Text)
            Else
                row.Cells(0).Value = Date.Now
                row.Cells(1).Value = TXTCODE.Text
                row.Cells(2).Value = TXTNAME.Text
                row.Cells(3).Value = txtqty.Text
                row.Cells(4).Value = TXTPRICE.Text
                row.Cells(5).Value = txttotal.Text
            End If
        Next
    End Sub

    Private Sub btnaddcart_Click(sender As Object, e As EventArgs) Handles btnaddcart.Click
        If TXTCODE.Text = String.Empty Then
            MsgBox("Please Select a Product")
        Else
            Try

                Dim rnum As Integer = DataGridView1.Rows.Add()
                If DataGridView1.Rows.Item(rnum).Cells("Item_Code").Value = TXTCODE.Text Then
                    DataGridView1.Rows.Item(rnum).Cells("Quantity").Value = DataGridView1.Rows.Item(rnum).Cells("Quantity").Value + Val(txtqty.Text)
                Else
                    DataGridView1.Rows.Item(rnum).Cells("Dates").Value = Date.Now
                    DataGridView1.Rows.Item(rnum).Cells("Item_Code").Value = TXTCODE.Text
                    DataGridView1.Rows.Item(rnum).Cells("Item_Name").Value = TXTNAME.Text
                    DataGridView1.Rows.Item(rnum).Cells("Quantity").Value = txtqty.Text
                    DataGridView1.Rows.Item(rnum).Cells("Price").Value = TXTPRICE.Text
                    DataGridView1.Rows.Item(rnum).Cells("Total").Value = txttotal.Text
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Try
                Dim AmountDue As Double = 0
                For i As Integer = 0 To DataGridView1.RowCount - 1
                    AmountDue += Val(DataGridView1.Rows(i).Cells(5).Value)
                Next
                txtamountdue.Text = AmountDue
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            storeOutupdateproduct()
            resetting()
            loadData()
            txtsearch.Focus()
        End If
    End Sub

    Private Sub DataGridView2_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView2.RowHeaderMouseClick
        Try
            Dim dr As DataGridViewRow = DataGridView2.SelectedRows(0)
            txtsearch.Text = dr.Cells(0).Value.ToString()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        txtsearch.Focus()
    End Sub

    Private Sub txtqty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtqty.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txtqty_Leave(sender As Object, e As EventArgs) Handles txtqty.Leave
        If Val(txtqty.Text) < 1 Then
            txtqty.Text = 1
        End If
    End Sub

    Private Sub txtqty_TextChanged(sender As Object, e As EventArgs) Handles txtqty.TextChanged
        txttotal.Text = Val(txtqty.Text) * Val(TXTPRICE.Text)
        If txtqty.Text = String.Empty Then
            txttotal.Text = Val(TXTPRICE.Text)
        End If
        TXTNEWSTOCK.Text = Val(TXTSTOCKS.Text) - Val(txtqty.Text)
        If TXTNEWSTOCK.Text < 1 Then
            TXTNEWSTOCK.Text = 0
        End If

    End Sub

    Private Sub TXTPRICE_TextChanged(sender As Object, e As EventArgs) Handles TXTPRICE.TextChanged
        txttotal.Text = Val(TXTPRICE.Text)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        StartPrint()

        If prn.PrinterIsOpen = True Then
            PrintHeader()

            PrintBody()

            PrintFooter()

            EndPrint()
        End If
        Button8.Enabled = True
        Button8.Focus()
        Button3.Enabled = True

        ' Dim Found As Boolean = False
        'If DataGridView1.Rows.Count > 0 Then

        '  For Each row As DataGridViewRow In DataGridView1.SelectedRows

        '        If Convert.ToString(row.Cells(1).Value) = TXTCODE.Text Then

        ' row.Cells(3).Value = Convert.ToString(1 + Convert.ToInt16(row.Cells(3).Value))
        ' Found = True
        ' If Not Found Then

        '        DataGridView1.Rows.Add(TXTCODE.Text, TXTNAME.Text)
        '       End If
        '      End If
        '     Next
        '    End If

    End Sub

    Private Sub btnprocess_Click(sender As Object, e As EventArgs) Handles btnprocess.Click
        If cboOutlet.SelectedIndex = 0 Or cbomodeofpayment.SelectedIndex = 0 Or cbowaiter.Text = "Select" Or DataGridView1.Rows.Count < 1 Then
            MessageBox.Show("Please Select Payment type or Waiter/Waitress, also make sure you have added items to cart", "Error", MessageBoxButtons.OK)
        Else
            process()
            receiptId()
            btnprocess.Enabled = False
            '  Button2.Enabled = False
            ' Button3.Enabled = False
            btnaddcart.Enabled = False
            btnrem.Enabled = False
            Button1.Enabled = True
            '  Button8.Enabled = True
            DataGridView1.Enabled = False
            Button1.Focus()
        End If

        'Processor.txtamountdue.Text = Val(txtamountdue.Text)
        'Processor.ShowDialog()
    End Sub

    Private Sub txtamountdue_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbomodeofpayment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbomodeofpayment.SelectedIndexChanged
        If cbomodeofpayment.SelectedIndex = 1 Then
            ' txtcashpaid.Enabled = True
        Else
            ' txtcashpaid.Enabled = False

        End If

    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If DataGridView1.Rows.Count > 0 And btnprocess.Enabled = True Then
            MessageBox.Show("Please Remove Item From Cart before creating new Transaction", "Sales Assistant", MessageBoxButtons.OK)
        Else
            DataGridView1.Rows.Clear()
            txtamountdue.Text = String.Empty
            resetting()
            ' txtcashpaid.Enabled = False
            'txtchange.Enabled = False
            Button1.Enabled = False
            Button8.Enabled = False
            Button3.Enabled = False
            Button7.Enabled = False
            Button9.Enabled = False
            cbomodeofpayment.SelectedIndex = 0
            cbowaiter.SelectedIndex = 0
            cboOutlet.SelectedIndex = 0
            '  txtcashpaid.Clear()
            'txtchange.Clear()
            loadData()
            btnprocess.Enabled = True
            '     Button2.Enabled = True
            '    Button3.Enabled = True
            btnaddcart.Enabled = True
            btnrem.Enabled = True
            DataGridView1.Enabled = True
            TXTNEWSTOCK.Clear()
            lblReceiptId.Text = String.Empty
            receiptidgeneration()
            txtsearchCode.Clear()
            txtsearchCode.Enabled = False
            DataGridView3.Enabled = False
            btnretrieve.Enabled = False
            Button5.Enabled = False
            Button10.Enabled = False
            txtsearch.Focus()
        End If

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnretrieve.Click
        Try
            Dim sourceGrid As DataGridView = Me.DataGridView3
            Dim targetGrid As DataGridView = Me.DataGridView1
            'Copy all rows and cells.
            Dim targetRows = New List(Of DataGridViewRow)
            For Each sourceRow As DataGridViewRow In sourceGrid.Rows
                If (Not sourceRow.IsNewRow) Then
                    Dim targetRow = CType(sourceRow.Clone(), DataGridViewRow)
                    For Each cell As DataGridViewCell In sourceRow.Cells
                        targetRow.Cells(cell.ColumnIndex).Value = cell.Value
                    Next
                    targetRows.Add(targetRow)
                End If
            Next
            'Clear target columns and then clone all source columns.
            targetGrid.Columns.Clear()
            For Each column As DataGridViewColumn In sourceGrid.Columns
                targetGrid.Columns.Add(CType(column.Clone(), DataGridViewColumn))
            Next
            'It's recommended to use the AddRange method (if available)
            'when adding multiple items to a collection.
            targetGrid.Rows.AddRange(targetRows.ToArray())
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Try
            Dim AmountDue As Double = 0
            For i As Integer = 0 To DataGridView1.RowCount - 1
                AmountDue += Val(DataGridView1.Rows(i).Cells(5).Value)
            Next
            txtamountdue.Text = AmountDue
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        retrievewaiter()
        txtsearchCode.Enabled = False
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If cboOutlet.SelectedIndex = 0 Or cbomodeofpayment.SelectedIndex = 0 Or cbowaiter.SelectedIndex = 0 Or DataGridView1.Rows.Count < 1 Then
            MessageBox.Show("Please Select Payment type or Waiter/Waitress, also make sure you have added items to cart", "Error", MessageBoxButtons.OK)
        Else
            edititem()
            deleteTDetails()
            txtsearchCode.Enabled = False
            btnretrieve.Enabled = False
            DataGridView3.Enabled = False
            Button7.Enabled = False
            Button8.Enabled = False
            btnprocess.Enabled = False
            '  txtamountdue.Text = String.Empty
            resetting()
            ' txtcashpaid.Enabled = False
            'txtchange.Enabled = False
            Button1.Enabled = False
            loadData()
            '   Button2.Enabled = True
            '   Button3.Enabled = True
            btnaddcart.Enabled = True
            btnrem.Enabled = True
            DataGridView1.Enabled = False
            TXTNEWSTOCK.Clear()
            Button9.Enabled = False
            Button5.Enabled = True
            Button10.Enabled = True
            Button12.Enabled = True
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        txtsearchCode.Enabled = True
        btnretrieve.Enabled = True
        DataGridView3.Enabled = True
        Button7.Enabled = True
        Button9.Enabled = True
        btnprocess.Enabled = False
        Button11.Enabled = True
    End Sub

    Private Sub txtsearchCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtsearchCode.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txtsearchCode_TextChanged(sender As Object, e As EventArgs) Handles txtsearchCode.TextChanged
        searchdata()
    End Sub


    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        StartPrint()

        If prn.PrinterIsOpen = True Then
            PrintHeader()

            PrintBody()

            PrintFooter1()

            EndPrint()
        End If
        Button3.Focus()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If DataGridView1.Rows.Count > 0 Then
            MessageBox.Show("Please Remove Item From Cart before Voiding Receipts", "Sales Assistant", MessageBoxButtons.OK)
        Else
            deleteTdetailsandReceipt()
            DataGridView1.Rows.Clear()
            txtsearchCode.Clear()
            btnretrieve.Enabled = False
            DataGridView3.Enabled = False
            Button7.Enabled = False
            Button9.Enabled = False
            Button5.Enabled = False
            btnaddcart.Enabled = False
            Button10.Enabled = False
            txtamountdue.Text = String.Empty
        End If
    End Sub

    REM REFUND AND VOID PRINTING

    Public Sub StartPrint()
        prn.OpenPrint(PrinterName)
    End Sub
    Public Sub PrintHeader2()
        Dim dateholder As Date = Date.Today
        Dim timeholder As Date = TimeOfDay
        Print(eCentre + "THE HEXAGON PLACE")
        Print("2a, Golf Course Road GRA")
        Print(eCentre + "Benin City, Nigeria")
        Print(eCentre + "Contact No: 07068609724")
        Print(eBigCharOn + "Receipt")
        Print(eNmlText + "Transaction no: #" + txtsearchCode.Text)
        Print(eNmlText + "Editted Copy")
        Print(vbNewLine)
        Print(eCentre + eSmlText + dateholder + "      " + timeholder)
        Print(eLeft + eBigCharOff + "--------------------------------")

    End Sub
    Public Sub PrintBody2()
        Dim cmd As New OleDb.OleDbCommand
        Dim Item_Code As String
        Dim Quantity As String
        Dim Price As String
        Dim Tot As String
        Dim item As String


        Print(eSmlText + "Qty " + " Item Name   " + "   Price  " + "Total Amount")
        Print("------------------------------------------")
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            Item_Code = DataGridView1.Rows(x).Cells(1).Value
            item = DataGridView1.Rows(x).Cells(2).Value
            Quantity = DataGridView1.Rows(x).Cells(3).Value
            Price = DataGridView1.Rows(x).Cells(4).Value
            Tot = DataGridView1.Rows(x).Cells(5).Value

            'add data to table 
            Print(eRight + eSmlText + Quantity + " " + Microsoft.VisualBasic.Left(item, 20) + "  " + Price.ToString & "  " + Tot)
        Next
        Print("------------------------------------------")

        Print(eLeft + eNmlText + "Total Amount:  " + txtamountdue.Text)
        Print("Served by: " + cbowaiter.Text)
        Print("Outlet: " + cboOutlet.Text)
        Print("Operator: " + FirstName)
        Print(eSmlText + "------------------------------------------")
        Print(vbNewLine)

    End Sub

    Public Sub PrintFooter2()
        Print(eCentre + "Thank You For Your Support!" + eLeft)
        Print(vbLf + vbLf + vbLf + vbLf + vbLf + eCut + eDrawer)
    End Sub

    Public Sub PrintFooter3()
        Print(eCentre + "MARCHANT COPY!" + eLeft)
        Print(vbLf + vbLf + vbLf + vbLf + vbLf + eCut + eDrawer)
    End Sub

    Public Sub PrintFooter4()
        Print(eCentre + "SERVER COPY!" + eLeft)
        Print(vbLf + vbLf + vbLf + vbLf + vbLf + eCut + eDrawer)
    End Sub

    Public Sub Print(Line As String)
        prn.SendStringToPrinter(PrinterName, Line + vbLf)
    End Sub

    Public Sub PrintDashes2()
        Print(eLeft + eNmlText + "-".PadRight(42, "-"))
    End Sub

    Public Sub EndPrint()
        prn.ClosePrint()
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        StartPrint()

        If prn.PrinterIsOpen = True Then
            PrintHeader2()

            PrintBody2()

            PrintFooter2()

            EndPrint()
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        StartPrint()

        If prn.PrinterIsOpen = True Then
            PrintHeader2()

            PrintBody2()

            PrintFooter3()

            EndPrint()
        End If
    End Sub
    Public Sub PrintHeader3()
        Dim dateholder As Date = Date.Today
        Dim timeholder As Date = TimeOfDay
        Print(eCentre + "THE HEXAGON PLACE")
        Print("2a, Golf Course Road GRA")
        Print(eCentre + "Benin City, Nigeria")
        Print(eCentre + "Contact No: 07068609724")
        Print(eBigCharOn + "Receipt")
        Print(eNmlText + "Transaction no: #" + txtsearchCode.Text)
        Print(eNmlText + "Reprint")
        Print(vbNewLine)
        Print(eCentre + eSmlText + dateholder + "      " + timeholder)
        Print(eLeft + eBigCharOff + "--------------------------------")

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        StartPrint()

        If prn.PrinterIsOpen = True Then
            PrintHeader3()

            PrintBody2()

            PrintFooter3()

            EndPrint()
        End If
    End Sub

    Private Sub cbowaiter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbowaiter.SelectedIndexChanged

    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        fillCombo2()
        fillCombo()
        cbowaiter.SelectedIndex = 0
        cboOutlet.SelectedIndex = 0
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        StartPrint()

        If prn.PrinterIsOpen = True Then
            PrintHeader()

            PrintBody()

            PrintFooter4()

            EndPrint()
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        StartPrint()

        If prn.PrinterIsOpen = True Then
            PrintHeader2()

            PrintBody2()

            PrintFooter4()

            EndPrint()
        End If
    End Sub

    Private Sub txttotal_TextChanged(sender As Object, e As EventArgs) Handles txttotal.TextChanged

    End Sub
End Class