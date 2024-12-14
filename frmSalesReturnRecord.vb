Imports System.Data.SqlClient

Imports System.IO

Public Class frmSalesReturnRecord

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT SR_ID, RTRIM(SRNo),SalesReturn.Date,SalesID,RTRIM(InvoiceNo),InvoiceDate, RTRIM(Customer.CustomerID),RTRIM(Name),RTRIM(SalesReturn.GrandTotal) FROM InvoiceInfo,SalesReturn,Customer where InvoiceInfo.Inv_ID=SalesReturn.SalesID and Customer.ID=InvoiceInfo.CustomerID order by SalesReturn.Date", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub frmLogs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Getdata()
    End Sub

    Private Sub dgw_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles dgw.MouseClick
        Try
            If dgw.Rows.Count > 0 Then

                If lblSet.Text = "SR" Then
                    Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                    ' frmSalesReturn.Reset()
                    frmSalesReturn.Show()
                    Me.Hide()
                    frmSalesReturn.txtSRID.Text = dr.Cells(0).Value.ToString()
                    frmSalesReturn.txtSRNO.Text = dr.Cells(1).Value.ToString()
                    frmSalesReturn.dtpSRDate.Text = dr.Cells(2).Value.ToString()
                    frmSalesReturn.txtSalesID.Text = dr.Cells(3).Value.ToString()
                    frmSalesReturn.txtSalesInvoiceNo.Text = dr.Cells(4).Value.ToString()
                    frmSalesReturn.dtpSalesDate.Text = dr.Cells(5).Value.ToString()
                    frmSalesReturn.txtCustomerID.Text = dr.Cells(6).Value.ToString()
                    frmSalesReturn.txtCustomerName.Text = dr.Cells(7).Value.ToString()
                    frmSalesReturn.txtGrandTotal.Text = dr.Cells(8).Value.ToString()
                    frmSalesReturn.btnSave.Enabled = False
                    frmSalesReturn.DataGridView1.Enabled = True
                    frmSalesReturn.btnAdd.Enabled = False
                    frmSalesReturn.btnRemove.Enabled = False
                    frmSalesReturn.lblSet.Text = "Not Allowed"
                    frmSalesReturn.pnlCalc.Enabled = False
                    frmSalesReturn.btnDelete.Enabled = True
                    frmSalesReturn.btnSelection.Enabled = False
                    con = New SqlConnection(cs)
                    con.Open()
                    Dim sql As String = "SELECT RTRIM(ProductCode),RTRIM(ProductName),RTRIM(SalesReturn_Join.Barcode),Price, Qty, DiscountPer, SalesReturn_Join.Discount, VATPer, SalesReturn_Join.VAT,ReturnQty, TotalAmount,ProductID,SalesReturn_Join.CostPrice,Margin FROM SalesReturn_Join INNER JOIN SalesReturn ON SalesReturn_Join.SalesReturnID = SalesReturn.SR_ID INNER JOIN Product ON Product.PID = SalesReturn_Join.ProductID and SR_ID=" & dr.Cells(0).Value & ""
                    cmd = New SqlCommand(sql, con)
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    frmSalesReturn.DataGridView1.Rows.Clear()
                    While (rdr.Read() = True)
                        frmSalesReturn.DataGridView1.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13))
                    End While
                    con.Close()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
    Sub Reset()
        txtCustomerName.Text = ""
        dtpDateFrom.Text = Today
        dtpDateTo.Text = Today
        Getdata()
    End Sub



    Private Sub txtSupplierName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCustomerName.TextChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT SR_ID, RTRIM(SRNo),SalesReturn.Date,SalesID,RTRIM(InvoiceNo),InvoiceDate, RTRIM(Customer.CustomerID),RTRIM(Name),RTRIM(SalesReturn.GrandTotal) FROM InvoiceInfo,SalesReturn,Customer where InvoiceInfo.Inv_ID=SalesReturn.SalesID and Customer.ID=InvoiceInfo.CustomerID  and [Name] like '%" & txtCustomerName.Text & "%' order by SalesReturn.Date", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT SR_ID, RTRIM(SRNo),SalesReturn.Date,SalesID,RTRIM(InvoiceNo),InvoiceDate, RTRIM(Customer.CustomerID),RTRIM(Name),RTRIM(SalesReturn.GrandTotal) FROM InvoiceInfo,SalesReturn,Customer where InvoiceInfo.Inv_ID=SalesReturn.SalesID and Customer.ID=InvoiceInfo.CustomerID and SalesReturn.Date between @d1 and @d2 order by SalesReturn.Date", con)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub btnExportExcel_Click(sender As System.Object, e As System.EventArgs) Handles btnExportExcel.Click
        ExportExcel(dgw)
    End Sub


End Class
