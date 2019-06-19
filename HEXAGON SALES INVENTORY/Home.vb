Public Class Home

    Dim stock As New stocksin
    Dim view As New Views_Select
    Dim staf As New Staff
    Dim outlet As New frmOutlet

    Dim user As New frmselect

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim edit As New editviewfrm
        PanelHome.Controls.Clear()
        edit.TopLevel = False
        edit.Size = PanelHome.Size
        edit.Visible = True
        PanelHome.Controls.Add(edit)
        edit.Show()
    End Sub

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PanelHome.Controls.Clear()
        ToolStripButton1.Enabled = False
        ToolStripButton2.Enabled = False
        ToolStripButton3.Enabled = False
        ToolStripButton4.Enabled = False
        ToolStripButton5.Enabled = False
        ToolStripButton6.Enabled = False
        ToolStripButton7.Enabled = False
        ToolStripButton8.Enabled = True
        ToolStripButton9.Enabled = False
        ToolStripButton10.Enabled = False
        ToolStripButton11.Enabled = False
        ToolStripButton12.Enabled = False
        btnLog.Text = "LOG IN"
        lblDate.Text = Date.Now.ToShortTimeString & " " & Date.Now.ToLongDateString
        Timer1.Start()
    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

        PanelHome.Controls.Clear()
        stock.TopLevel = False
        stock.Size = PanelHome.Size
        stock.Visible = True
        PanelHome.Controls.Add(stock)
        stock.Show()
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        PanelHome.Controls.Clear()
        stock.TopLevel = False
        stock.Size = PanelHome.Size
        stock.Visible = True
        PanelHome.Controls.Add(stock)
        stock.Show()
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim Rep As New Reports
        PanelHome.Controls.Clear()
        Rep.TopLevel = False
        Rep.Size = PanelHome.Size
        Rep.Visible = True
        PanelHome.Controls.Add(Rep)
        Rep.Show()
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click

        PanelHome.Controls.Clear()
        view.TopLevel = False
        view.Size = PanelHome.Size
        view.Visible = True
        PanelHome.Controls.Add(view)
        view.Show()
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        PanelHome.Controls.Clear()
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        PanelHome.Controls.Clear()
        CLOSING_STOCKS.ShowDialog()
    End Sub

    Private Sub ToolStripButton8_Click(sender As Object, e As EventArgs) Handles ToolStripButton8.Click
        PanelHome.Controls.Clear()
        End
    End Sub

    Private Sub ToolStripButton9_Click(sender As Object, e As EventArgs) Handles ToolStripButton9.Click
        PanelHome.Controls.Clear()
        staf.TopLevel = False
        staf.Size = PanelHome.Size
        staf.Visible = True
        PanelHome.Controls.Add(staf)
        staf.Show()
    End Sub

    Private Sub ToolStripButton10_Click(sender As Object, e As EventArgs) Handles ToolStripButton10.Click
        Dim damage As New Damages
        PanelHome.Controls.Clear()
        damage.TopLevel = False
        damage.Size = PanelHome.Size
        damage.Visible = True
        PanelHome.Controls.Add(damage)
        damage.Show()
    End Sub

    Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
        Dim pos As New point_of_sale
        PanelHome.Controls.Clear()
        pos.TopLevel = False
        pos.Size = PanelHome.Size
        pos.Visible = True
        PanelHome.Controls.Add(pos)

        pos.Show()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblDate.Text = Date.Now.ToShortTimeString & " " & Date.Now.ToLongDateString
    End Sub

    Private Sub ToolStripButton11_Click(sender As Object, e As EventArgs) Handles ToolStripButton11.Click
        PanelHome.Controls.Clear()
        outlet.TopLevel = False
        outlet.Size = PanelHome.Size
        outlet.Visible = True
        PanelHome.Controls.Add(outlet)
        outlet.Show()
    End Sub

    Private Sub ToolStripButton12_Click(sender As Object, e As EventArgs) Handles ToolStripButton12.Click
        PanelHome.Controls.Clear()
        user.TopLevel = False
        user.Size = PanelHome.Size
        user.Visible = True
        PanelHome.Controls.Add(user)
        user.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLog.Click
        If btnLog.Text = "LOG OUT" Then
            If MessageBox.Show("Are you Sure you want to Log Out?", "Hexagon Sales Inventory", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                PanelHome.Controls.Clear()
                ToolStripButton1.Enabled = False
                ToolStripButton2.Enabled = False
                ToolStripButton3.Enabled = False
                ToolStripButton4.Enabled = False
                ToolStripButton5.Enabled = False
                ToolStripButton6.Enabled = False
                ToolStripButton7.Enabled = False
                ToolStripButton9.Enabled = False
                ToolStripButton10.Enabled = False
                ToolStripButton11.Enabled = False
                ToolStripButton12.Enabled = False
                lblUsername.Text = String.Empty
                btnLog.Text = "LOG IN"
            End If

        Else
            LoginForm1.ShowDialog()
        End If

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
