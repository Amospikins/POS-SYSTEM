﻿Public Class Report_Period_Summary

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Summary_Closing.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Report_Period_Summary_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class