
Public Class LoginForm1

    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        UsernameTextBox.Focus()
        LOGIN()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        UsernameTextBox.Focus()
        Me.Close()
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connecting()
        UsernameTextBox.Focus()
    End Sub

    Private Sub ButtonSpecAny1_Click(sender As Object, e As EventArgs) Handles ButtonSpecAny1.Click
        UsernameTextBox.Focus()
        Me.Close()
    End Sub

    Private Sub KryptonHeader1_Paint(sender As Object, e As PaintEventArgs) Handles KryptonHeader1.Paint

    End Sub
End Class
