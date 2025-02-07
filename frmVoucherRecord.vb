﻿Imports System.Data.SqlClient


Public Class frmVoucherRecord

    Sub fillVoucherNo()
        Try
            Dim CN As New SqlConnection(cs)
            CN.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(VoucherNo) FROM Voucher", CN)
            ds = New DataSet("ds")
            adp.Fill(ds)
            dtable = ds.Tables(0)
            cmbVoucherNo.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbVoucherNo.Items.Add(drow(0).ToString())
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Sub GetData()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select RTRIM(Voucher.Id) as [Voucher ID], RTRIM(VoucherNo) as [Voucher No.],Convert(DateTime,Date,103) as [Voucher Date], RTRIM(Name) as [Name],RTRIM(Details) as [Details],RTRIM(Voucher.GrandTotal) as [Grand Total] from Voucher order by Date", con)
            Dim myDA As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim myDataSet As DataSet = New DataSet()
            myDA.Fill(myDataSet, "Voucher")
            dgw.DataSource = myDataSet.Tables("Voucher").DefaultView
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub frmLogs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetData()
        fillVoucherNo()
    End Sub
    Sub Reset()
        cmbVoucherNo.Text = ""
        dtpDateFrom.Text = Today
        dtpDateTo.Text = Today
        GetData()
    End Sub
    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub




    Private Sub btnExportExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportExcel.Click
        ExportExcel(dgw)
    End Sub

    Private Sub dgw_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgw.MouseClick
        Try
            If dgw.Rows.Count > 0 Then
                Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                Me.Hide()
                frmVoucher.Show()
                ' or simply use column name instead of index
                'dr.Cells["id"].Value.ToString();
                frmVoucher.txtVoucherID.Text = dr.Cells(0).Value.ToString()
                frmVoucher.txtVoucherNo.Text = dr.Cells(1).Value.ToString()
                frmVoucher.dtpDate.Text = dr.Cells(2).Value.ToString()
                frmVoucher.txtName.Text = dr.Cells(3).Value.ToString()
                frmVoucher.txtDetails.Text = dr.Cells(4).Value.ToString()
                frmVoucher.txtGrandTotal.Text = dr.Cells(5).Value.ToString()
                frmVoucher.btnSave.Enabled = False
                frmVoucher.btnDelete.Enabled = True
                frmVoucher.btnUpdate.Enabled = True
                frmVoucher.btnPrint.Enabled = True
                frmVoucher.btnRemove.Enabled = False
                con = New SqlConnection(cs)
                con.Open()
                Dim sql As String = "Select RTRIM(Particulars),RTRIM(Amount),RTRIM(Note) from Voucher,Voucher_OtherDetails where Voucher.Id=Voucher_OtherDetails.VoucherID and Voucher.ID=" & dr.Cells(0).Value & ""
                cmd = New SqlCommand(sql, con)
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                frmVoucher.DataGridView1.Rows.Clear()
                While (rdr.Read() = True)
                    frmVoucher.DataGridView1.Rows.Add(rdr(0), rdr(1), rdr(2))
                End While
                con.Close()
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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select RTRIM(Voucher.Id) as [Voucher ID], RTRIM(VoucherNo) as [Voucher No.],Convert(DateTime,Date,103) as [Voucher Date], RTRIM(Name) as [Name],RTRIM(Details) as [Details],RTRIM(Voucher.GrandTotal) as [Grand Total] from Voucher where Date between @d1 and @d2 order by Date", con)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value
            Dim myDA As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim myDataSet As DataSet = New DataSet()
            myDA.Fill(myDataSet, "Voucher")
            dgw.DataSource = myDataSet.Tables("Voucher").DefaultView
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbBillNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVoucherNo.SelectedIndexChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select RTRIM(Voucher.Id) as [Voucher ID], RTRIM(VoucherNo) as [Voucher No.],Convert(DateTime,Date,103) as [Voucher Date], RTRIM(Name) as [Name],RTRIM(Details) as [Details],RTRIM(Voucher.GrandTotal) as [Grand Total] from Voucher where VoucherNo='" & cmbVoucherNo.Text & "' order by Date", con)
            Dim myDA As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim myDataSet As DataSet = New DataSet()
            myDA.Fill(myDataSet, "Voucher")
            dgw.DataSource = myDataSet.Tables("Voucher").DefaultView
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
