Imports System.Data.OleDb
Imports System.Text
Imports System.Security.Cryptography
Public Class Category
    Dim cnn As New OleDb.OleDbConnection

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
    Private Sub Category_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cnn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" & Application.StartupPath & "\DB.mdb"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TXTCODE.Text = String.Empty & "CAT-" & GenKey(4)
    End Sub

    Private Sub BTNADD_Click(sender As Object, e As EventArgs) Handles BTNADD.Click
        Try
            If TXTCODE.Text Is String.Empty Or TXTNAME.Text Is String.Empty Then
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
                cmd.CommandText = "INSERT INTO Category(CategoryID, Category_Name) " & _
                    " VALUES('" & Me.TXTCODE.Text & "','" & Me.TXTNAME.Text & "')"
                cmd.ExecuteNonQuery()
                cnn.Close()
                MessageBox.Show("Category Created Successfully", "The Hexagon Place", MessageBoxButtons.OK)
                TXTCODE.Text = String.Empty
                TXTNAME.Text = String.Empty
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim f As New editviewfrm
        Home.PanelHome.Controls.Clear()
        f.TopLevel = False
        f.Size = Home.PanelHome.Size
        f.Visible = True
        Home.PanelHome.Controls.Add(f)
        f.Show()
    End Sub
End Class