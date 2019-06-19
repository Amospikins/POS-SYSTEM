Public Class Report_Period3

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Payment_Method_Report.ShowDialog()
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class