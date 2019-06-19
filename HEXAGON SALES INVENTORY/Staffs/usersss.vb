Public Class usersss
    Private Sub usersss_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connecting()
        textbox5.SelectedIndex = 0
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = String.Empty Or TextBox2.Text = String.Empty Or TextBox3.Text = String.Empty Or TextBox4.Text = String.Empty Or textbox5.SelectedIndex = 0 Or TextBox6.Text = String.Empty Or TextBox7.Text = String.Empty Then
            MessageBox.Show("Please Complete All Fields", "The Hexagon Place", MessageBoxButtons.OK)
            TextBox1.Focus()
        ElseIf TextBox7.Text <> TextBox8.Text Then
            MessageBox.Show("Passwords not the same", "The Hexagon Place", MessageBoxButtons.OK)
            TextBox7.Clear()
            TextBox8.Clear()
            TextBox7.Focus()
        Else
            savenewuser()
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            textbox5.SelectedIndex = 0
            TextBox6.Clear()
            TextBox7.Clear()
            TextBox8.Clear()

        End If
    End Sub
    Private Sub ButtonSpecAny1_Click(sender As Object, e As EventArgs) Handles ButtonSpecAny1.Click
        Me.Close()
    End Sub
    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

    End Sub
End Class