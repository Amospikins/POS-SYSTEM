Public Class Views_Select

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        transactionlist.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Sales_People_List.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        items_list.ShowDialog()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        payment_method_list.ShowDialog()
    End Sub
End Class