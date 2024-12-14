Imports System.Data.SqlClient

Imports System.IO

Public Class frmSalesInvoiceRecord

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select Inv_ID, RTRIM(InvoiceNo), InvoiceDate,SM_ID, RTRIM(Salesman_ID),RTRIM(Salesman.Name),Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Customer.Name),RTRIM(Customer.ContactNo), GrandTotal, TotalPaid, Balance, RTRIM(InvoiceInfo.Remarks) from Customer,InvoiceInfo,Salesman where Customer.ID=InvoiceInfo.CustomerID and Salesman.SM_ID=InvoiceInfo.SalesmanID order by InvoiceDate Desc", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub frmLogs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Getdata()
        fillInvoiceNo()
    End Sub

    Private Sub dgw_RowPostPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles dgw.RowPostPaint
        Dim strRowNumber As String = (e.RowIndex + 1).ToString()
        Dim size As SizeF = e.Graphics.MeasureString(strRowNumber, Me.Font)
        If dgw.RowHeadersWidth < Convert.ToInt32((size.Width + 20)) Then
            dgw.RowHeadersWidth = Convert.ToInt32((size.Width + 20))
        End If
        Dim b As Brush = SystemBrushes.ControlText
        e.Graphics.DrawString(strRowNumber, Me.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))

    End Sub
    Sub fillInvoiceNo()
        Try
            con = New SqlConnection(cs)
            con.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(InvoiceNo) FROM InvoiceInfo", con)
            ds = New DataSet("ds")
            adp.Fill(ds)
            dtable = ds.Tables(0)
            cmbInvoiceNo.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbInvoiceNo.Items.Add(drow(0).ToString())
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Sub Reset()
        cmbInvoiceNo.Text = ""
        txtCustomerName.Text = ""
        txtSalesman.Text = ""
        fillInvoiceNo()
        dtpDateFrom.Text = Today
        dtpDateTo.Text = Today
        DateTimePicker2.Text = Today
        DateTimePicker1.Text = Today
        Getdata()
    End Sub
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub



    Private Sub btnExportExcel_Click(sender As System.Object, e As System.EventArgs) Handles btnExportExcel.Click
        ExportExcel(dgw)
    End Sub

    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select Inv_ID, RTRIM(InvoiceNo), InvoiceDate,SM_ID, RTRIM(Salesman_ID),RTRIM(Salesman.Name),Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Customer.Name),RTRIM(Customer.ContactNo), GrandTotal, TotalPaid, Balance, RTRIM(InvoiceInfo.Remarks) from Customer,InvoiceInfo,Salesman where Customer.ID=InvoiceInfo.CustomerID and Salesman.SM_ID=InvoiceInfo.SalesmanID and InvoiceDate between @d1 and @d2 order by InvoiceDate Desc", con)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbOrderNo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbInvoiceNo.SelectedIndexChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select Inv_ID, RTRIM(InvoiceNo), InvoiceDate,SM_ID, RTRIM(Salesman_ID),RTRIM(Salesman.Name),Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Customer.Name),RTRIM(Customer.ContactNo), GrandTotal, TotalPaid, Balance, RTRIM(InvoiceInfo.Remarks) from Customer,InvoiceInfo,Salesman where Customer.ID=InvoiceInfo.CustomerID and Salesman.SM_ID=InvoiceInfo.SalesmanID and InvoiceNo='" & cmbInvoiceNo.Text & "' order by InvoiceDate Desc", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select Inv_ID, RTRIM(InvoiceNo), InvoiceDate,SM_ID, RTRIM(Salesman_ID),RTRIM(Salesman.Name),Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Customer.Name),RTRIM(Customer.ContactNo), GrandTotal, TotalPaid, Balance, RTRIM(InvoiceInfo.Remarks) from Customer,InvoiceInfo,Salesman where Customer.ID=InvoiceInfo.CustomerID and Salesman.SM_ID=InvoiceInfo.SalesmanID and InvoiceDate between @d1 and @d2 and Balance > 0 order by InvoiceDate Desc", con)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker2.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker1.Value.Date
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtCustomerName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCustomerName.TextChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select Inv_ID, RTRIM(InvoiceNo), InvoiceDate,SM_ID, RTRIM(Salesman_ID),RTRIM(Salesman.Name),Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Customer.Name),RTRIM(Customer.ContactNo), GrandTotal, TotalPaid, Balance, RTRIM(InvoiceInfo.Remarks) from Customer,InvoiceInfo,Salesman where Customer.ID=InvoiceInfo.CustomerID and Salesman.SM_ID=InvoiceInfo.SalesmanID and Customer.Name like '%" & txtCustomerName.Text & "%' order by InvoiceDate Desc", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbInvoiceNo_Format(sender As System.Object, e As System.Windows.Forms.ListControlConvertEventArgs) Handles cmbInvoiceNo.Format
        If (e.DesiredType Is GetType(String)) Then
            e.Value = e.Value.ToString.Trim
        End If
    End Sub

    Private Sub txtSalesman_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSalesman.TextChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select Inv_ID, RTRIM(InvoiceNo), InvoiceDate,SM_ID, RTRIM(Salesman_ID),RTRIM(Salesman.Name),Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Customer.Name),RTRIM(Customer.ContactNo), GrandTotal, TotalPaid, Balance, RTRIM(InvoiceInfo.Remarks) from Customer,InvoiceInfo,Salesman where Customer.ID=InvoiceInfo.CustomerID and Salesman.SM_ID=InvoiceInfo.SalesmanID and Salesman.Name like '%" & txtSalesman.Text & "%' order by InvoiceDate Desc", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub dgw_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dgw.MouseDoubleClick
        Try
            If dgw.Rows.Count > 0 Then
                If lblSet.Text = "Sales Invoice" Then
                    Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                    frmPOS.Show()
                    Me.Hide()
                    frmPOS.txtID.Text = dr.Cells(0).Value.ToString()
                    frmPOS.txtInvoiceNo.Text = dr.Cells(1).Value.ToString()
                    frmPOS.dtpInvoiceDate.Text = dr.Cells(2).Value.ToString()
                    frmPOS.txtSM_ID.Text = dr.Cells(3).Value.ToString()
                    frmPOS.txtSalesmanID.Text = dr.Cells(4).Value.ToString()
                    frmPOS.txtSalesman.Text = dr.Cells(5).Value.ToString()
                    frmPOS.txtCustomerID.Text = dr.Cells(7).Value.ToString()
                    frmPOS.txtCID.Text = dr.Cells(6).Value.ToString()
                    frmPOS.txtCustomerName.Text = dr.Cells(8).Value.ToString()
                    frmPOS.txtContactNo.Text = dr.Cells(9).Value.ToString()
                    frmPOS.txtGrandTotal.Text = dr.Cells(10).Value.ToString()
                    frmPOS.txtTotalPayment.Text = dr.Cells(11).Value.ToString()
                    frmPOS.txtPaymentDue.Text = dr.Cells(12).Value.ToString()
                    frmPOS.txtRemarks.Text = dr.Cells(13).Value.ToString()
                    'frmPOS.TextBox1.Text = dr.Cells(14).Value.ToString()
                    frmPOS.btnSave.Enabled = False
                    frmPOS.Button2.Enabled = False

                    frmPOS.btnUpdate.Enabled = True
                    frmPOS.btnPrint.Enabled = True
                    frmPOS.btnDelete.Enabled = True
                    frmPOS.lblSet.Text = "Not Allowed"
                    frmPOS.btnAdd.Enabled = False
                    frmPOS.txtCustomerName.ReadOnly = True
                    frmPOS.txtContactNo.ReadOnly = True
                    con = New SqlConnection(cs)
                    con.Open()
                    Dim sql As String = "SELECT RTRIM(ProductCode),RTRIM(ProductName),RTRIM(Invoice_Product.Barcode), Invoice_Product.CostPrice, Invoice_Product.SellingPrice, Invoice_Product.Margin, Invoice_Product.Qty, Invoice_Product.Amount, Invoice_Product.DiscountPer, Invoice_Product.Discount, Invoice_Product.VATPer, Invoice_Product.VAT, Invoice_Product.TotalAmount,Product.PID from InvoiceInfo,Invoice_Product,Product where InvoiceInfo.Inv_ID=Invoice_Product.InvoiceID and Product.PID=Invoice_Product.ProductID and InvoiceInfo.Inv_ID=@d1"
                    cmd = New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@d1", Val(dr.Cells(0).Value))
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    frmPOS.DataGridView1.Rows.Clear()
                    While (rdr.Read() = True)
                        frmPOS.DataGridView1.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
                    End While
                    con.Close()
                    con = New SqlConnection(cs)
                    con.Open()
                    Dim sql1 As String = "SELECT RTRIM(PaymentMode),Invoice_Payment.TotalPaid,PaymentDate from InvoiceInfo,Invoice_Payment where InvoiceInfo.Inv_ID=Invoice_Payment.InvoiceID and InvoiceInfo.Inv_ID=@d1"
                    cmd = New SqlCommand(sql1, con)
                    cmd.Parameters.AddWithValue("@d1", Val(dr.Cells(0).Value))
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    frmPOS.DataGridView2.Rows.Clear()
                    While (rdr.Read() = True)
                        frmPOS.DataGridView2.Rows.Add(rdr(0), rdr(1), rdr(2))
                    End While
                    con.Close()
                    con = New SqlConnection(cs)
                    con.Open()
                    Dim ct As String = "select RTRIM(CustomerType) from Customer where ID=" & dr.Cells(3).Value & ""
                    cmd = New SqlCommand(ct)
                    cmd.Connection = con
                    rdr = cmd.ExecuteReader()
                    If rdr.Read Then
                        frmPOS.txtCustomerType.Text = rdr.GetValue(0)
                        If Not rdr Is Nothing Then
                            rdr.Close()
                        End If
                        Exit Sub
                    End If
                    con.Close()
                End If
                If lblSet.Text = "Sales Return" Then
                    Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                    frmSalesReturn.Show()
                    Me.Hide()
                    frmSalesReturn.txtSalesID.Text = dr.Cells(0).Value.ToString()
                    frmSalesReturn.txtSalesInvoiceNo.Text = dr.Cells(1).Value.ToString()
                    frmSalesReturn.dtpSalesDate.Text = dr.Cells(2).Value.ToString()
                    frmSalesReturn.txtCustomerID.Text = dr.Cells(7).Value.ToString()
                    frmSalesReturn.txtcust_ID.Text = dr.Cells(6).Value.ToString()
                    frmSalesReturn.txtCustomerName.Text = dr.Cells(8).Value.ToString()
                    frmSalesReturn.txtGrandTotal.Text = dr.Cells(10).Value.ToString()
                    con = New SqlConnection(cs)
                    con.Open()
                    Dim sql As String = "SELECT RTRIM(ProductCode),RTRIM(ProductName),RTRIM(Invoice_Product.Barcode), Invoice_Product.SellingPrice, Invoice_Product.Qty, Invoice_Product.Amount, Invoice_Product.DiscountPer, Invoice_Product.Discount, Invoice_Product.VATPer, Invoice_Product.VAT, Invoice_Product.TotalAmount,Product.PID,Invoice_Product.CostPrice,Margin from InvoiceInfo,Invoice_Product,Product where InvoiceInfo.Inv_ID=Invoice_Product.InvoiceID and Product.PID=Invoice_Product.ProductID and InvoiceInfo.Inv_ID=@d1"
                    cmd = New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@d1", Val(dr.Cells(0).Value))
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    frmSalesReturn.DataGridView2.Rows.Clear()
                    While (rdr.Read() = True)
                        frmSalesReturn.DataGridView2.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
                    End While
                    con.Close()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub dgw_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgw.CellContentClick

    End Sub
End Class
