Imports System.Data.SqlClient
Imports System.IO

Public Class frmPurchaseReturn
    Dim str As String
    Dim st As String
    Dim num1, num2, num3, num4, num5, num6, num7, num8, num9, num10, num11 As Decimal
    Private Function GenerateID() As String
        con = New SqlConnection(cs)
        Dim value As String = "0000"
        Try
            ' Fetch the latest ID from the database
            con.Open()
            cmd = New SqlCommand("SELECT TOP 1 PR_ID FROM PurchaseReturn ORDER BY PR_ID DESC", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows Then
                rdr.Read()
                value = rdr.Item("PR_ID")
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
    Sub Reset()
        txtPRNO.Text = ""
        txtPRID.Text = ""
        dtpPRDate.Text = Today
        dtpPurchaseDate.Text = Today
        txtPurchaseID.Text = ""
        txtPurchaseInvoiceNo.Text = ""
        txtDiscPer.Text = "0.000"
        txtDisc.Text = "0.000"
        txtVatPer.Text = "0.000"
        txtVATAmt.Text = "0.000"
        txtSubTotal.Text = ""
        txtTotal.Text = ""
        txtSupplierID.Text = ""
        txtSupplierName.Text = ""
        txtSup_ID.Text = ""
        txtGrandTotal.Text = ""
        txtRoundOff.Text = "0.000"
        btnSave.Enabled = True
        btnDelete.Enabled = False
        DataGridView1.Enabled = True
        btnAdd.Enabled = True
        pnlCalc.Enabled = True
        btnRemove.Enabled = False
        DataGridView1.Rows.Clear()
        DataGridView2.Rows.Clear()
        btnSelection.Enabled = True
        lblSet.Text = ""
        Clear()
        auto()
    End Sub
    Sub Compute()
        num6 = (Val(txtSubTotal.Text) * Val(txtDiscPer.Text)) / 100
        num6 = Math.Round(num6, 2)
        txtDisc.Text = num6
        num7 = Val(txtSubTotal.Text) - num6
        num8 = (num7 * Val(txtVatPer.Text)) / 100
        num8 = Math.Round(num8, 2)
        txtVATAmt.Text = num8
        num1 = num7 + Val(txtVATAmt.Text)
        num1 = Math.Round(num1, 2)
        txtTotal.Text = num1
        num2 = Math.Round(num1, 1)
        num3 = num2 - num1
        num3 = Math.Round(num3, 2)
        txtRoundOff.Text = num3
        num4 = Val(txtTotal.Text) + Val(txtRoundOff.Text)
        num4 = Math.Round(num4, 2)
        txtGrandTotal.Text = num4

    End Sub

    Sub auto()
        Try
            txtPRID.Text = GenerateID()
            txtPRNO.Text = "PR-" + GenerateID()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Private Sub btnSelection_Click(sender As System.Object, e As System.EventArgs) Handles btnSelection.Click
        frmPurchaseRecord.lblSet.Text = "PR"
        frmPurchaseRecord.Reset()
        frmPurchaseRecord.ShowDialog()
    End Sub



    Private Sub DataGridView2_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles DataGridView2.MouseClick
        Try
            If DataGridView2.Rows.Count > 0 Then
                Clear()
                Dim dr As DataGridViewRow = DataGridView2.SelectedRows(0)
                txtProductID.Text = dr.Cells(0).Value.ToString()
                txtProductCode.Text = dr.Cells(1).Value.ToString()
                txtProductName.Text = dr.Cells(2).Value.ToString()
                txtBarcode.Text = dr.Cells(3).Value.ToString()
                txtQty.Text = dr.Cells(4).Value.ToString()
                txtPricePerQty.Text = dr.Cells(5).Value.ToString()
                txtReturnQty.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Function SubTotal() As Double
        Dim sum As Double = 0
        Try
            For Each r As DataGridViewRow In Me.DataGridView1.Rows
                sum = sum + r.Cells(7).Value
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return sum
    End Function
    Sub Clear()
        txtProductName.Text = ""
        txtBarcode.Text = ""
        txtProductCode.Text = ""
        txtQty.Text = ""
        txtPricePerQty.Text = ""
        txtTotalAmount.Text = ""
        txtReturnQty.Text = ""
        btnAdd.Enabled = True
        btnRemove.Enabled = False
    End Sub
    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        Try
            If txtProductName.Text = "" Then
                MessageBox.Show("الرجاء إدراج اسم الصنف", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtProductName.Focus()
                Exit Sub
            End If
            If txtBarcode.Text = "" Then
                MessageBox.Show("الرجاء كتابة الباركود", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBarcode.Focus()
                Exit Sub
            End If
            If txtQty.Text = "" Then
                MessageBox.Show("الرجاء كتابة الكمية", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtQty.Focus()
                Exit Sub
            End If
            If Val(txtQty.Text = 0) Then
                MessageBox.Show("الكمية يجب أن تكون أكبر من صفر", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtQty.Focus()
                Exit Sub
            End If
            If txtPricePerQty.Text = "" Then
                MessageBox.Show("الرجاء كتابة سعر الوحدة", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPricePerQty.Focus()
                Exit Sub
            End If

            If Val(txtPricePerQty.Text = 0) Then
                MessageBox.Show("سعر الوحدة يجب أن يكون أكبر من صفر", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPricePerQty.Focus()
                Exit Sub
            End If
            If txtReturnQty.Text = "" Then
                MessageBox.Show("الرجاء كتابة الكمية المرتجعة", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtReturnQty.Focus()
                Exit Sub
            End If
            If Val(txtReturnQty.Text) = 0 Then
                MessageBox.Show("الكمية المرتجعة يجب أن تكون أكبر من صفر", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtReturnQty.Focus()
                Exit Sub
            End If
            If Val(txtReturnQty.Text) > Val(txtQty.Text) Then
                MessageBox.Show("الكمية المرتجعة لا يجب أن تكون أكبر من كمية الشراء", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtReturnQty.Text = ""
                txtReturnQty.Focus()
                Exit Sub
            End If
            If DataGridView1.Rows.Count = 0 Then
                DataGridView1.Rows.Add(Val(txtProductID.Text), txtProductCode.Text, txtProductName.Text, txtBarcode.Text, Val(txtQty.Text), Val(txtPricePerQty.Text), Val(txtReturnQty.Text), Val(txtTotalAmount.Text))
                Dim k As Double = 0
                k = SubTotal()
                k = Math.Round(k, 2)
                txtSubTotal.Text = k
                Compute()
                Clear()
                Exit Sub
            End If
            For Each row As DataGridViewRow In DataGridView1.Rows
                If txtBarcode.Text = row.Cells(3).Value Then
                    MessageBox.Show("الباركود هذا مدرج بالفعل في شبكة البيانات", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtBarcode.Focus()
                    Exit Sub
                End If
                If txtBarcode.Text = row.Cells(3).Value And txtProductID.Text = row.Cells(0).Value Then
                    MessageBox.Show("هذا السجل مضاف بالفعل في شبكة البيانات", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtBarcode.Focus()
                    Exit Sub
                End If
            Next
            DataGridView1.Rows.Add(Val(txtProductID.Text), txtProductCode.Text, txtProductName.Text, txtBarcode.Text, Val(txtQty.Text), Val(txtPricePerQty.Text), Val(txtReturnQty.Text), Val(txtTotalAmount.Text))
            Dim k1 As Double = 0
            k1 = SubTotal()
            k1 = Math.Round(k1, 2)
            txtSubTotal.Text = k1
            Clear()
            Compute()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click
        Try
            For Each row As DataGridViewRow In DataGridView1.SelectedRows
                DataGridView1.Rows.Remove(row)
            Next
            Dim k As Double = 0
            k = SubTotal()
            k = Math.Round(k, 2)
            txtSubTotal.Text = k
            Compute()
            btnRemove.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtAddVATAmt_TextChanged(sender As System.Object, e As System.EventArgs)
        Compute()
    End Sub

    Private Sub txtAddVatPer_TextChanged(sender As System.Object, e As System.EventArgs)
        Compute()
    End Sub

    Private Sub txtVATAmt_TextChanged(sender As System.Object, e As System.EventArgs) 
        Compute()
    End Sub

    Private Sub txtVatPer_TextChanged(sender As System.Object, e As System.EventArgs) 
        Compute()
    End Sub

    Private Sub txtDisc_TextChanged(sender As System.Object, e As System.EventArgs) 
        Compute()
    End Sub

    Private Sub txtDiscPer_TextChanged(sender As System.Object, e As System.EventArgs) 
        Compute()
    End Sub

    Private Sub txtSubTotal_TextChanged(sender As System.Object, e As System.EventArgs) 
        Compute()
    End Sub

    Private Sub txtTotal_TextChanged(sender As System.Object, e As System.EventArgs) 
        Compute()
    End Sub

    Private Sub txtRoundOff_TextChanged(sender As System.Object, e As System.EventArgs) 
        Compute()
    End Sub

    Private Sub txtRetuenQty_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtReturnQty.TextChanged
        Dim i As Double = 0
        i = CDbl(Val(txtReturnQty.Text) * Val(txtPricePerQty.Text))
        i = Math.Round(i, 2)
        txtTotalAmount.Text = i
    End Sub

    Private Sub frmPurchaseReturn_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub txtQty_TextChanged(sender As Object, e As EventArgs) Handles txtQty.TextChanged

    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Try
            If Len(Trim(txtPurchaseInvoiceNo.Text)) = 0 Then
                MessageBox.Show("الرجاء إدراج بيانات فاتورة الشراء", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPurchaseInvoiceNo.Focus()
                Exit Sub
            End If
            If Len(Trim(txtSupplierID.Text)) = 0 Then
                MessageBox.Show("الرجاء استرداد رقم المورد", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtSupplierID.Focus()
                Exit Sub
            End If

            If DataGridView1.Rows.Count = 0 Then
                MessageBox.Show("الرجاء إدراج بيانات الأصناف المرتجعة في شبكة البيانات", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            If Len(Trim(txtDiscPer.Text)) = 0 Then
                MessageBox.Show("الرجاء إدراج نسبة الخصم %", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDiscPer.Focus()
                Exit Sub
            End If
            If Len(Trim(txtRoundOff.Text)) = 0 Then
                MessageBox.Show("الرجاء كتابة التقريب", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtRoundOff.Focus()
                Exit Sub
            End If
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select PurchaseID from PurchaseReturn where PurchaseID=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", Val(txtPurchaseID.Text))
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                MessageBox.Show("هذا الصنف في فاتورة الشراء هذه بالفعل تم إرجاعه", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If
            con.Close()
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "insert into  PurchaseReturn(PR_ID, PRNo, Date,PurchaseID, SubTotal, DiscPer, Discount, VATPer, VATAmt, Total, RoundOff, GrandTotal) VALUES (@d1,@d2,@d3,@d5,@d6,@d7,@d8,@d9,@d10,@d13,@d14,@d15)"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d1", Val(txtPRID.Text))
            cmd.Parameters.AddWithValue("@d2", txtPRNO.Text)
            cmd.Parameters.AddWithValue("@d3", dtpPRDate.Value.Date)
            cmd.Parameters.AddWithValue("@d5", Val(txtPurchaseID.Text))
            cmd.Parameters.AddWithValue("@d6", Val(txtSubTotal.Text))
            cmd.Parameters.AddWithValue("@d7", Val(txtDiscPer.Text))
            cmd.Parameters.AddWithValue("@d8", Val(txtDisc.Text))
            cmd.Parameters.AddWithValue("@d9", Val(txtVatPer.Text))
            cmd.Parameters.AddWithValue("@d10", Val(txtVATAmt.Text))
            cmd.Parameters.AddWithValue("@d13", Val(txtTotal.Text))
            cmd.Parameters.AddWithValue("@d14", Val(txtRoundOff.Text))
            cmd.Parameters.AddWithValue("@d15", Val(txtGrandTotal.Text))
            cmd.Connection = con
            cmd.ExecuteNonQuery()
            con.Close()
            con = New SqlConnection(cs)
            con.Open()
            Dim cb1 As String = "insert into PurchaseReturn_Join(PurchaseReturnID,ProductID,Barcode,Qty, Price,ReturnQty,TotalAmount)VALUES (" & txtPRID.Text & ",@d1,@d2,@d3,@d4,@d5,@d6)"
            cmd = New SqlCommand(cb1)
            cmd.Connection = con
            ' Prepare command for repeated execution
            cmd.Prepare()
            ' Data to be inserted
            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow Then
                    cmd.Parameters.AddWithValue("@d1", Val(row.Cells(0).Value))
                    cmd.Parameters.AddWithValue("@d2", row.Cells(3).Value)
                    cmd.Parameters.AddWithValue("@d3", Val(row.Cells(4).Value))
                    cmd.Parameters.AddWithValue("@d4", Val(row.Cells(5).Value))
                    cmd.Parameters.AddWithValue("@d5", Val(row.Cells(6).Value))
                    cmd.Parameters.AddWithValue("@d6", Val(row.Cells(7).Value))
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                End If
            Next
            con.Close()
            For Each row As DataGridViewRow In DataGridView1.Rows
                con = New SqlConnection(cs)
                con.Open()
                Dim cb2 As String = "Update Temp_Stock set Qty=Qty - " & Val(row.Cells(6).Value) & " where ProductID=@d1 and Barcode=@d2"
                cmd = New SqlCommand(cb2)
                cmd.Connection = con
                cmd.Parameters.AddWithValue("@d1", Val(row.Cells(0).Value))
                cmd.Parameters.AddWithValue("@d2", row.Cells(3).Value)
                cmd.ExecuteReader()
                con.Close()
            Next
            SupplierLedgerSave(dtpPRDate.Value.Date, txtSupplierName.Text, txtPRNO.Text, "مردودات مشتريات", Val(txtGrandTotal.Text), 0, txtSupplierID.Text)
            LedgerSave(dtpPRDate.Value.Date, txtSupplierName.Text, txtPRNO.Text, "مردودات مشتريات", Val(txtGrandTotal.Text), 0, txtSupplierID.Text, "")
            LedgerSave(dtpPRDate.Value.Date, "نقدا", txtPRNO.Text, "مردودات مشتريات من " & txtSupplierName.Text & "", 0, Val(txtGrandTotal.Text), txtSupplierID.Text, "")
            LogFunc(lblUser.Text, "added the new Purchase return record having PR No. '" & txtPRNO.Text & "'")
            MessageBox.Show("تم الحفظ بنجاح", "السجلات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnSave.Enabled = False
            con.Close()
            RefreshRecords()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtQty_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtQty.KeyPress
        Dim keyChar = e.KeyChar

        If Char.IsControl(keyChar) Then
            'Allow all control characters.
        ElseIf Char.IsDigit(keyChar) OrElse keyChar = "."c Then
            Dim text = Me.txtQty.Text
            Dim selectionStart = Me.txtQty.SelectionStart
            Dim selectionLength = Me.txtQty.SelectionLength

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
    Private Sub DeleteRecord()

        Try
           
            Dim RowsAffected As Integer = 0
            con = New SqlConnection(cs)
            con.Open()
            Dim cq As String = "delete from PurchaseReturn where PR_ID=@d1"
            cmd = New SqlCommand(cq)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", Val(txtPRID.Text))
            RowsAffected = cmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                For Each row As DataGridViewRow In DataGridView1.Rows
                    con = New SqlConnection(cs)
                    con.Open()
                    Dim cb2 As String = "Update Temp_Stock set Qty=Qty + " & Val(row.Cells(6).Value) & " where ProductID=@d1 and Barcode=@d2"
                    cmd = New SqlCommand(cb2)
                    cmd.Connection = con
                    cmd.Parameters.AddWithValue("@d1", Val(row.Cells(0).Value))
                    cmd.Parameters.AddWithValue("@d2", row.Cells(3).Value)
                    cmd.ExecuteReader()
                    con.Close()
                Next
                LedgerDelete(txtPRNO.Text, "مردودات مشتريات من " & txtSupplierName.Text & "")
                LedgerDelete(txtPRNO.Text, "مردودات مشتريات")
                SupplierLedgerDelete(txtPRNO.Text)
                Dim st As String = "deleted the Purchase Return record having PR No. '" & txtPRNO.Text & "'"
                LogFunc(lblUser.Text, st)
                MessageBox.Show("تم الحذف بنجاح", "السجلات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Reset()
                RefreshRecords()
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
    Private Sub txtReturnQty_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtReturnQty.KeyPress
        'If (e.KeyChar < Chr(48) Or e.KeyChar > Chr(57)) And e.KeyChar <> Chr(8) Then
        '    e.Handled = True
        'End If
        Dim keyChar = e.KeyChar

        If Char.IsControl(keyChar) Then
            'Allow all control characters.
        ElseIf Char.IsDigit(keyChar) OrElse keyChar = "."c Then
            Dim text = Me.txtReturnQty.Text
            Dim selectionStart = Me.txtReturnQty.SelectionStart
            Dim selectionLength = Me.txtReturnQty.SelectionLength

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

    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        Try
            If MessageBox.Show("هل أنت متأكد أنك تريد بالفعل حذف هذا السجل?", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                DeleteRecord()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNew_Click(sender As System.Object, e As System.EventArgs) Handles btnNew.Click
        Reset()

    End Sub

    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Me.Reset()
        frmPurchaseReturnRecord.lblSet.Text = "PR"
        frmPurchaseReturnRecord.Reset()
        frmPurchaseReturnRecord.ShowDialog()
    End Sub

    Private Sub DataGridView1_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles DataGridView1.MouseClick
        If DataGridView1.Rows.Count > 0 Then
            If lblSet.Text = "Not allowed" Then
                btnRemove.Enabled = False
            Else
                btnRemove.Enabled = True
            End If
        End If
    End Sub

    Private Sub DataGridView1_RowPostPaint(sender As Object, e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DataGridView1.RowPostPaint
        Dim strRowNumber As String = (e.RowIndex + 1).ToString()
        Dim size As SizeF = e.Graphics.MeasureString(strRowNumber, Me.Font)
        If DataGridView1.RowHeadersWidth < Convert.ToInt32((size.Width + 20)) Then
            DataGridView1.RowHeadersWidth = Convert.ToInt32((size.Width + 20))
        End If
        Dim b As Brush = SystemBrushes.ControlText
        e.Graphics.DrawString(strRowNumber, Me.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))

    End Sub

    Private Sub DataGridView2_RowPostPaint(sender As Object, e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DataGridView2.RowPostPaint
        Dim strRowNumber As String = (e.RowIndex + 1).ToString()
        Dim size As SizeF = e.Graphics.MeasureString(strRowNumber, Me.Font)
        If DataGridView2.RowHeadersWidth < Convert.ToInt32((size.Width + 20)) Then
            DataGridView2.RowHeadersWidth = Convert.ToInt32((size.Width + 20))
        End If
        Dim b As Brush = SystemBrushes.ControlText
        e.Graphics.DrawString(strRowNumber, Me.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))

    End Sub
End Class
