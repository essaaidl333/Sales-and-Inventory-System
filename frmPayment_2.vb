Imports System.Data.SqlClient
Imports System.IO

Public Class frmPayment_2
    Dim str As String
    Dim OBType As String
    Dim num1, num2, num3, num4 As Decimal
    Dim i As Integer = 0

    Sub GetSupplierBalance()
        Try
            Try
                num1 = 0
                con = New SqlConnection(cs)
                con.Open()
                Dim sql As String = "SELECT isNULL(Sum(Credit),0)-IsNull(Sum(Debit),0) from LedgerBook where PartyID=@d1 group By PartyID"
                cmd = New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@d1", txtSupplierID.Text)
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                If (rdr.Read() = True) Then
                    num1 = rdr.GetValue(0)
                End If
                con.Close()
                lblBalance.Text = num1
                If Val(lblBalance.Text) >= 0 Then
                    str = "دائن"
                ElseIf Val(lblBalance.Text < 0) Then
                    str = "مدين"
                End If
                lblBalance.Text = Math.Abs(Val(lblBalance.Text))
                lblBalance.Text = (lblBalance.Text & " " & str).ToString()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub
    Private Function GenerateID() As String
        con = New SqlConnection(cs)
        Dim value As String = "0000"
        Try
            ' Fetch the latest ID from the database
            con.Open()
            cmd = New SqlCommand("SELECT TOP 1 TC_ID FROM Payment_2 ORDER BY TC_ID DESC", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows Then
                rdr.Read()
                value = rdr.Item("TC_ID")
            End If
            rdr.Close()
            ' Increase the ID by 1
            value += 1
            ' Because incrementing a string with an integer removes 0's
            ' we need to replace them. If necessary.
            If value <= 9 Then 'Value is between 0 and 10
                value = "000" & value
            ElseIf value <= 99 Then 'Value is between 9 and 100
                value = "00" & value
            ElseIf value <= 999 Then 'Value is between 999 and 1000
                value = "0" & value
            End If
        Catch ex As Exception
            ' If an error occurs, check the connection state and close it if necessary.
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            value = "0000"
        End Try
        Return value
    End Function
    Private Sub CountValue()
        con = New SqlConnection(cs)
        con.Open()
        Dim sql As String = ("SELECT Count(TC_ID) FROM Payment_2 where Amount=0")
        cmd = New SqlCommand(sql)
        cmd.Connection = con
        If (IsDBNull(cmd.ExecuteScalar)) Then
            i = 0
        Else
            i = cmd.ExecuteScalar
        End If
        cmd.Dispose()
        con.Close()
        con.Dispose()
    End Sub
    Sub auto()
        Try
            CountValue()
            txtT_ID.Text = GenerateID()
            txtTransactionNo.Text = "TC-" & (Val(GenerateID()) - Val(i))
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Sub Reset()
        txtAddress.Text = ""
        txtCity.Text = ""
        txtContactNo.Text = ""
        txtRemarks.Text = ""
        txtSupplierID.Text = ""
        txtCheck.Text = ""
        txtBank.Text = ""
        '' txtSalesman.Text = ""
        '' txtSalesmanID.Text = ""
        '' txtSM_ID.Text = ""
        ''txtCommissionPer.Text = ""
        txtSupplierName.Text = ""
        txtTransactionAmount.Text = ""
        cmbPaymentMode.SelectedIndex = 0
        dtpTranactionDate.Value = Today
        dtpCheck.Value = Today
        lblBalance.Text = "0.000"
        btnSave.Enabled = True
        btnDelete.Enabled = False
        btnUpdate.Enabled = False
        btnSelection.Enabled = True
        auto()
    End Sub

    Sub GetSupplierInfo()
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim sql As String = "SELECT CustomerID,Name,Address,City,ContactNo from Customer where ID=@d1"
            cmd = New SqlCommand(sql, con)
            cmd.Parameters.AddWithValue("@d1", Val(txtSup_ID.Text))
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If (rdr.Read() = True) Then
                txtSupplierID.Text = rdr.GetValue(0)
                txtSupplierName.Text = rdr.GetValue(1)
                txtAddress.Text = rdr.GetValue(2)
                txtCity.Text = rdr.GetValue(3)
                txtContactNo.Text = rdr.GetValue(4)
            End If
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Private Sub DeleteRecord()
        Try
            Dim RowsAffected As Integer = 0
            con = New SqlConnection(cs)
            con.Open()
            Dim cq As String = "delete from Payment_2 where TC_ID=@d1"
            cmd = New SqlCommand(cq)
            cmd.Parameters.AddWithValue("@d1", Val(txtT_ID.Text))
            cmd.Connection = con
            RowsAffected = cmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                SupplierLedgerDelete(txtTransactionNo.Text)
                LedgerDelete(txtTransactionNo.Text, "سند قبض")
                LogFunc(lblUser.Text, "deleted the Customer payment record having transaction No. '" & txtTransactionNo.Text & "'")
                MessageBox.Show("تم الحذف بنجاح", "سندات قبض العملاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Reset()
            Else
                MessageBox.Show("لا يوجد سجلات", "عذرًا", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Reset()
            End If
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub


    Private Sub txtTotalPaid_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtTransactionAmount.KeyPress
        'Dim keyChar = e.KeyChar

        'If Char.IsControl(keyChar) Then
        '    'Allow all control characters.
        'ElseIf Char.IsDigit(keyChar) OrElse keyChar = "."c Then
        '    Dim text = Me.txtTransactionAmount.Text
        '    Dim selectionStart = Me.txtTransactionAmount.SelectionStart
        '    Dim selectionLength = Me.txtTransactionAmount.SelectionLength

        '    text = text.Substring(0, selectionStart) & keyChar & text.Substring(selectionStart + selectionLength)

        If Integer.TryParse(text, New Integer) AndAlso text.Length > 16 Then
                'Reject an integer that is longer than 16 digits.
                e.Handled = True
            ElseIf Double.TryParse(text, New Double) AndAlso text.IndexOf("."c) < text.Length - 3 Then
                'Reject a real number with two many decimal places.
                e.Handled = False
            End If
        'Else
        '    'Reject all other characters.
        '    e.Handled = True
        'End If
    End Sub


    Private Sub btnSelection_Click(sender As System.Object, e As System.EventArgs) Handles btnSelection.Click
        'Me.Reset()
        frmCustomerRecord3.lblSet.Text = "سندات قبض العملاء"
        frmCustomerRecord3.Reset()
        frmCustomerRecord3.ShowDialog()
    End Sub


    Private Sub txtTransactionAmount_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles txtTransactionAmount.Validating
        'If Val(txtTransactionAmount.Text) > Val(lblBalance.Text) Then
        '    MessageBox.Show("مبلغ السند لا يجب أن يكون أكبر من رصيد العميل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    txtTransactionAmount.Focus()
        '    Exit Sub
        'End If
    End Sub

    Private Sub btnNew_Click(sender As System.Object, e As System.EventArgs) Handles btnNew.Click
        Reset()
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        If Len(Trim(txtSupplierID.Text)) = 0 Then
            MessageBox.Show("الرجاء إدراج رقم العميل", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSupplierID.Focus()
            Exit Sub
        End If
        If Len(Trim(txtTransactionAmount.Text)) = 0 Then
            MessageBox.Show("الرجاء كتابة المبلغ المدفوع", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTransactionAmount.Focus()
            Exit Sub
        End If
        If Val(txtTransactionAmount.Text) = 0 Then
            MessageBox.Show("مبلغ السند يجب أن يكون أكبر من صفر", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTransactionAmount.Focus()
            Exit Sub
        End If
        If Len(Trim(txtSalesmanID.Text)) = 0 Then
            MessageBox.Show("الرجاء اختيار المندوب الذي حصل الشيك", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSalesman.Focus()
            Exit Sub
        End If
        'If Val(txtCommissionPer.Text) = 0 Then
        '    MessageBox.Show("مبلغ نسبة المندوب يجب أن يكون أكبر من صفر", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    txtCommissionPer.Focus()
        '    Exit Sub
        'End If
        'If Val(txtTransactionAmount.Text) > Val(lblBalance.Text) Then
        '    MessageBox.Show("مبلغ السند يجب أن لا يكون أكبر من رصيد العميل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    txtTransactionAmount.Focus()
        '    Exit Sub
        'End If
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "insert into Payment_2(TC_ID, TransactionID, Date, PaymentMode, CustomerID, Amount,Remarks,Check_ID,Check_Date,Bank,SalesMan_ID,SalesMan_Name,SalesMan_Comession,SalesMan_ID_2) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7,@d8,@d9,@d10,@d11,@d12,@d13,@d14)"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d1", Val(txtT_ID.Text))
            cmd.Parameters.AddWithValue("@d2", txtTransactionNo.Text)
            cmd.Parameters.AddWithValue("@d3", dtpTranactionDate.Value.Date)
            cmd.Parameters.AddWithValue("@d4", cmbPaymentMode.Text)
            cmd.Parameters.AddWithValue("@d5", txtSupplierID.Text)
            cmd.Parameters.AddWithValue("@d6", Val(txtTransactionAmount.Text))
            cmd.Parameters.AddWithValue("@d7", txtRemarks.Text)
            cmd.Parameters.AddWithValue("@d8", txtCheck.Text)
            cmd.Parameters.AddWithValue("@d9", dtpCheck.Value.Date)
            cmd.Parameters.AddWithValue("@d10", txtBank.Text)
            cmd.Parameters.AddWithValue("@d11", txtSM_ID.Text)
            cmd.Parameters.AddWithValue("@d12", txtSalesman.Text)
            cmd.Parameters.AddWithValue("@d13", txtCommissionPer.Text)
            cmd.Parameters.AddWithValue("@d14", txtSalesmanID.Text)
            cmd.Connection = con
            cmd.ExecuteNonQuery()
            con.Close()
            If cmbPaymentMode.SelectedIndex = 0 Then
                LedgerSave(dtpTranactionDate.Value.Date, "نقدا", txtTransactionNo.Text, "سند قبض", 0, Val(txtTransactionAmount.Text), txtSupplierID.Text, "")
            End If
            If cmbPaymentMode.SelectedIndex = 1 Then
                LedgerSave(dtpTranactionDate.Value.Date, "شيك رقم" & txtCheck.Text.Trim & "", txtTransactionNo.Text, "سند قبض", 0, Val(txtTransactionAmount.Text), txtSupplierID.Text, "")
            End If
            If cmbPaymentMode.SelectedIndex = 2 Then
                LedgerSave(dtpTranactionDate.Value.Date, "حوالة بنكية رقم" & txtCheck.Text.Trim & "", txtTransactionNo.Text, "سند قبض", 0, Val(txtTransactionAmount.Text), txtSupplierID.Text, "")
            End If

            'If cmbPaymentMode.SelectedIndex = 0 Then
            'SupplierLedgerSave(dtpTranactionDate.Value.Date, "نقدا", txtTransactionNo.Text, "سند قبض", Val(txtTransactionAmount.Text), 0, txtSupplierID.Text)
            'End If
            'If cmbPaymentMode.SelectedIndex = 1 Then
            'SupplierLedgerSave(dtpTranactionDate.Value.Date, "شيك", txtTransactionNo.Text, "سند قبض", Val(txtTransactionAmount.Text), 0, txtSupplierID.Text)
            'End If
            LogFunc(lblUser.Text, "added the new payment having transaction No. '" & txtTransactionNo.Text & "'")
            MessageBox.Show("تم الحفظ بنجاح", "سندات قبض العملاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnSave.Enabled = False
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click

        If Len(Trim(txtSupplierID.Text)) = 0 Then
            MessageBox.Show("الرجاء إدراج رقم العميل", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSupplierID.Focus()
            Exit Sub
        End If
        If Len(Trim(txtTransactionAmount.Text)) = 0 Then
            MessageBox.Show("الرجاء كتابة المبلغ المدفوع", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTransactionAmount.Focus()
            Exit Sub
        End If
        If Val(txtTransactionAmount.Text) = 0 Then
            MessageBox.Show("مبلغ السند يجب أن يكون أكبر من صفر", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTransactionAmount.Focus()
            Exit Sub
        End If
        'If Val(txtTransactionAmount.Text) > Val(lblBalance.Text) Then
        '    MessageBox.Show("مبلغ السند يجب أن لا يكون أكبر من رصيد العميل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    txtTransactionAmount.Focus()
        '    Exit Sub
        'End If
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "Update Payment_2 set TransactionID=@d2, Date=@d3, PaymentMode=@d4, CustomerID=@d5, Amount=@d6,Remarks=@d7,Check_ID=@d8,Check_Date=@d9,Bank=@d10,SalesMan_ID=@d11,SalesMan_Name=@d12,SalesMan_Comession=@d13,SalesMan_ID_2=@d14 where TC_ID=@d1"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d1", Val(txtT_ID.Text))
            cmd.Parameters.AddWithValue("@d2", txtTransactionNo.Text)
            cmd.Parameters.AddWithValue("@d3", dtpTranactionDate.Value.Date)
            cmd.Parameters.AddWithValue("@d4", cmbPaymentMode.Text)
            cmd.Parameters.AddWithValue("@d5", txtSupplierID.Text)
            cmd.Parameters.AddWithValue("@d6", Val(txtTransactionAmount.Text))
            cmd.Parameters.AddWithValue("@d7", txtRemarks.Text)
            cmd.Parameters.AddWithValue("@d8", txtCheck.Text)
            cmd.Parameters.AddWithValue("@d9", dtpCheck.Value.Date)
            cmd.Parameters.AddWithValue("@d10", txtBank.Text)
            cmd.Parameters.AddWithValue("@d11", txtSM_ID.Text)
            cmd.Parameters.AddWithValue("@d12", txtSalesman.Text)
            cmd.Parameters.AddWithValue("@d13", txtCommissionPer.Text)
            cmd.Parameters.AddWithValue("@d14", txtSalesmanID.Text)
            cmd.Connection = con
            cmd.ExecuteNonQuery()
            con.Close()
            If cmbPaymentMode.SelectedIndex = 0 Then
                LedgerUpdate(dtpTranactionDate.Value.Date, "نقدا", 0, Val(txtTransactionAmount.Text), txtSupplierID.Text, txtTransactionNo.Text, "سند قبض")
            End If
            If cmbPaymentMode.SelectedIndex = 1 Then
                LedgerUpdate(dtpTranactionDate.Value.Date, "شيك رقم" & txtCheck.Text.Trim & "", 0, Val(txtTransactionAmount.Text), txtSupplierID.Text, txtTransactionNo.Text, "سند قبض")
            End If
            If cmbPaymentMode.SelectedIndex = 2 Then
                LedgerUpdate(dtpTranactionDate.Value.Date, "حوالة بنكية رقم" & txtCheck.Text.Trim & "", 0, Val(txtTransactionAmount.Text), txtSupplierID.Text, txtTransactionNo.Text, "سند قبض")
            End If
            LogFunc(lblUser.Text, "updated Customer payment record having transaction No. '" & txtTransactionNo.Text & "'")
            MessageBox.Show("تم التعديل بنجاح", "سندات قبض العملاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnUpdate.Enabled = False
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub cmbPaymentMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPaymentMode.SelectedIndexChanged
        If cmbPaymentMode.SelectedIndex = 0 Then
            txtCheck.Enabled = False
            dtpCheck.Enabled = False
            txtBank.Enabled = False

        Else
            txtCheck.Enabled = True
            dtpCheck.Enabled = True
            txtBank.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        frmSalesmanRecord_2.lblSet.Text = ""
        frmSalesmanRecord_2.Reset()
        frmSalesmanRecord_2.ShowDialog()

    End Sub



    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        Try
            If MessageBox.Show("هل أنت متأكد أنك تريد حذف هذا السند من السجلات?", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                DeleteRecord()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Me.Reset()
        frmPaymentRecord_2.lblSet.Text = "سندات قبض العملاء"
        frmPaymentRecord_2.Reset()
        frmPaymentRecord_2.ShowDialog()
    End Sub


    Private Sub txtCommissionPer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCommissionPer.KeyPress
        Dim keyChar = e.KeyChar

        If Char.IsControl(keyChar) Then
            'Allow all control characters.
        ElseIf Char.IsDigit(keyChar) OrElse keyChar = "."c Then
            Dim text = Me.txtCommissionPer.Text
            Dim selectionStart = Me.txtCommissionPer.SelectionStart
            Dim selectionLength = Me.txtCommissionPer.SelectionLength

            text = text.Substring(0, selectionStart) & keyChar & text.Substring(selectionStart + selectionLength)

            If Integer.TryParse(text, New Integer) AndAlso text.Length > 16 Then
                'Reject an integer that is longer than 16 digits.
                e.Handled = True
            ElseIf Double.TryParse(text, New Double) AndAlso text.IndexOf("."c) < text.Length - 3 Then
                'Reject a real number with two many decimal places.
                e.Handled = False
            End If
        Else
            'Reject all other characters.
            e.Handled = True
        End If

    End Sub
End Class
