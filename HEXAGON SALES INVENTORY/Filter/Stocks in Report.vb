Public Class Stocks_in_Report

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Stocks_In_Summary.ShowDialog()
        Me.Close()
    End Sub
End Class