
Public Class EditLogin





    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        again()

    End Sub

    Private Sub EditLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connecting()
    End Sub

    Private Sub ButtonSpecAny1_Click(sender As Object, e As EventArgs) Handles ButtonSpecAny1.Click
        Me.Close()
    End Sub
End Class