﻿Imports System.Data.SqlClient

Imports System.IO

Public Class frmPaymentRecord_2

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT TC_ID, RTRIM(TransactionID), Date,RTRIM(PaymentMode),Customer.ID, RTRIM(Customer.CustomerID),RTRIM(Name),Amount, RTRIM(Payment_2.Remarks), RTRIM(Check_ID), Check_Date, RTRIM(Bank), SalesMan_ID, SalesMan_Name, SalesMan_Comession, SalesMan_ID_2 from Customer,Payment_2 where Customer.CustomerID=Payment_2.CustomerID and Amount > 0 order by [Date]", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13), rdr(14), rdr(15))
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
                If lblSet.Text = "سندات قبض العملاء" Then
                    Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                    frmPayment_2.Show()
                    Me.Hide()
                    frmPayment_2.txtT_ID.Text = dr.Cells(0).Value.ToString()
                    frmPayment_2.txtTransactionNo.Text = dr.Cells(1).Value.ToString()
                    frmPayment_2.dtpTranactionDate.Text = dr.Cells(2).Value.ToString()
                    frmPayment_2.cmbPaymentMode.Text = dr.Cells(3).Value.ToString()
                    frmPayment_2.txtSup_ID.Text = dr.Cells(4).Value.ToString()
                    frmPayment_2.txtSupplierID.Text = dr.Cells(5).Value.ToString()
                    frmPayment_2.txtSupplierName.Text = dr.Cells(6).Value.ToString()
                    frmPayment_2.txtTransactionAmount.Text = dr.Cells(7).Value.ToString()
                    frmPayment_2.txtRemarks.Text = dr.Cells(8).Value.ToString()
                    frmPayment_2.txtCheck.Text = dr.Cells(9).Value.ToString()
                    frmPayment_2.dtpCheck.Text = dr.Cells(10).Value.ToString()
                    frmPayment_2.txtBank.Text = dr.Cells(11).Value.ToString()
                    frmPayment_2.txtSM_ID.Text = dr.Cells(12).Value.ToString()
                    frmPayment_2.txtSalesman.Text = dr.Cells(13).Value.ToString()
                    frmPayment_2.txtCommissionPer.Text = dr.Cells(14).Value.ToString()
                    frmPayment_2.txtSalesmanID.Text = dr.Cells(15).Value.ToString()

                    frmPayment_2.btnSave.Enabled = False
                    frmPayment_2.GetSupplierBalance()
                    frmPayment_2.btnUpdate.Enabled = True
                    frmPayment_2.btnDelete.Enabled = True
                    frmPayment_2.GetSupplierInfo()
                    frmPayment_2.btnSelection.Enabled = False
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
            cmd = New SqlCommand("SELECT TC_ID, RTRIM(TransactionID), Date,RTRIM(PaymentMode),Customer.ID, RTRIM(Customer.CustomerID),RTRIM(Name),Amount, RTRIM(Payment_2.Remarks), RTRIM(Check_ID), Check_Date, RTRIM(Bank), SalesMan_ID, SalesMan_Name, SalesMan_Comession, SalesMan_ID_2 from Customer,Payment_2 where Customer.CustomerID=Payment_2.CustomerID and Amount > 0  and [Name] like '%" & txtSupplierName.Text & "%' order by [Date]", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13), rdr(14), rdr(15))
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
            cmd = New SqlCommand("SELECT TC_ID, RTRIM(TransactionID), Date,RTRIM(PaymentMode),Customer.ID, RTRIM(Customer.CustomerID),RTRIM(Name),Amount, RTRIM(Payment_2.Remarks), RTRIM(Check_ID), Check_Date, RTRIM(Bank), SalesMan_ID, SalesMan_Name, SalesMan_Comession, SalesMan_ID_2 from Customer,Payment_2 where Customer.CustomerID=Payment_2.CustomerID and Amount > 0  and [Date] between @d1 and @d2 order by [Date]", con)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9), rdr(10), rdr(11), rdr(12), rdr(13), rdr(14), rdr(15))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
