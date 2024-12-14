Imports System.Data.SqlClient

Imports System.IO

Public Class frmPaymentRecord_3

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT TCD_ID, RTRIM(TransactionID), Date,Customer.ID, RTRIM(Customer.CustomerID),RTRIM(Name),Amount, RTRIM(Payment_3.Remarks), SalesMan_ID, SalesMan_Name, SalesMan_Comession, SalesMan_ID_2 from Customer,Payment_3 where Customer.CustomerID=Payment_3.CustomerID and Amount > 0 order by [Date]", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11))
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
                If lblSet.Text = "اشعار خصم - دائن" Then
                    Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                    frmPayment_3.Show()
                    Me.Hide()
                    frmPayment_3.txtT_ID.Text = dr.Cells(0).Value.ToString()
                    frmPayment_3.txtTransactionNo.Text = dr.Cells(1).Value.ToString()
                    frmPayment_3.dtpTranactionDate.Text = dr.Cells(2).Value.ToString()
                    frmPayment_3.txtSup_ID.Text = dr.Cells(3).Value.ToString()
                    frmPayment_3.txtSupplierID.Text = dr.Cells(4).Value.ToString()
                    frmPayment_3.txtSupplierName.Text = dr.Cells(5).Value.ToString()
                    frmPayment_3.txtTransactionAmount.Text = dr.Cells(6).Value.ToString()
                    frmPayment_3.txtRemarks.Text = dr.Cells(7).Value.ToString()
                    frmPayment_3.txtSM_ID.Text = dr.Cells(8).Value.ToString()
                    frmPayment_3.txtSalesman.Text = dr.Cells(9).Value.ToString()
                    frmPayment_3.txtCommissionPer.Text = dr.Cells(10).Value.ToString()
                    frmPayment_3.txtSalesmanID.Text = dr.Cells(11).Value.ToString()

                    frmPayment_3.btnSave.Enabled = False
                    frmPayment_3.GetSupplierBalance()
                    frmPayment_3.btnUpdate.Enabled = True
                    frmPayment_3.btnDelete.Enabled = True
                    frmPayment_3.GetSupplierInfo()
                    frmPayment_3.btnSelection.Enabled = False
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
        txtSupplierName.Text = ""
        dtpDateFrom.Text = Today
        dtpDateTo.Text = Today
        Getdata()
    End Sub
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub


    Private Sub txtSupplierName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSupplierName.TextChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT TCD_ID, RTRIM(TransactionID), Date,Customer.ID, RTRIM(Customer.CustomerID),RTRIM(Name),Amount, RTRIM(Payment_3.Remarks), SalesMan_ID, SalesMan_Name, SalesMan_Comession, SalesMan_ID_2 from Customer,Payment_3 where Customer.CustomerID=Payment_3.CustomerID and Amount > 0  and [Name] like '%" & txtSupplierName.Text & "%' order by [Date]", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub btnExportExcel_Click(sender As System.Object, e As System.EventArgs) Handles btnExportExcel.Click
        ExportExcel(dgw)
    End Sub

    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT TCD_ID, RTRIM(TransactionID), Date,Customer.ID, RTRIM(Customer.CustomerID),RTRIM(Name),Amount, RTRIM(Payment_3.Remarks), SalesMan_ID, SalesMan_Name, SalesMan_Comession, SalesMan_ID_2 from Customer,Payment_3 where Customer.CustomerID=Payment_3.CustomerID and Amount > 0  and [Date] between @d1 and @d2 order by [Date]", con)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
