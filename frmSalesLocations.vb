Public Class frmSalesLocations

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        'frmCustomerRecord6.Reset()
        ' frmCustomerRecord6.ShowDialog()
        Dim frmm As New frmCustomerRecord6
        frmm.lblSet.Text = "Customer Entry"
        frmm.Getdata()
        frmm.Show()
    End Sub

    Private Sub frmSalesLocations_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class