﻿Imports System.Data.SqlClient

Imports System.IO

Public Class frmServicesRecord

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select S_ID, RTRIM(ServiceCode),ServiceCreationDate, Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Name), RTRIM(ServiceType), RTRIM(ItemDescription), RTRIM(ProblemDescription), ChargesQuote, AdvanceDeposit, EstimatedRepairDate,RTRIM(Status), RTRIM(Service.Remarks) from Customer,Service where Customer.ID=Service.CustomerID order by ServiceCreationDate", con)
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
        fillServiceCode()
    End Sub

    Private Sub dgw_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles dgw.MouseClick
        Try
            If dgw.Rows.Count > 0 Then
                Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                If lblSet.Text = "Services" Then
                    frmServices.Show()
                    Me.Hide()
                    frmServices.txtID.Text = dr.Cells(0).Value.ToString()
                    frmServices.txtServiceCode.Text = dr.Cells(1).Value.ToString()
                    frmServices.dtpServiceCreationDate.Text = dr.Cells(2).Value.ToString()
                    frmServices.txtCustomerID.Text = dr.Cells(4).Value.ToString()
                    frmServices.txtCID.Text = dr.Cells(3).Value.ToString()
                    frmServices.txtCustomerName.Text = dr.Cells(5).Value.ToString()
                    frmServices.cmbServiceType.Text = dr.Cells(6).Value.ToString()
                    frmServices.txtItemsDescription.Text = dr.Cells(7).Value.ToString()
                    frmServices.txtProblemDescription.Text = dr.Cells(8).Value.ToString()
                    frmServices.txtChargesQuote.Text = dr.Cells(9).Value.ToString()
                    frmServices.txtUpfront.Text = dr.Cells(10).Value.ToString()
                    frmServices.dtpEstimatedRepairDate.Text = dr.Cells(11).Value.ToString()
                    frmServices.cmbStatus.Text = dr.Cells(12).Value.ToString()
                    frmServices.txtRemarks.Text = dr.Cells(13).Value.ToString()
                    frmServices.btnSave.Enabled = False
                    frmServices.btnUpdate.Enabled = True
                    frmServices.btnPrint.Enabled = True
                    frmServices.btnDelete.Enabled = True
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
    Sub fillServiceCode()
        Try
            con = New SqlConnection(cs)
            con.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(ServiceCode) FROM Service", con)
            ds = New DataSet("ds")
            adp.Fill(ds)
            dtable = ds.Tables(0)
            cmbServiceCode.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbServiceCode.Items.Add(drow(0).ToString())
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Sub Reset()
        cmbServiceCode.Text = ""
        txtCustomerName.Text = ""
        fillServiceCode()
        dtpDateFrom.Text = Today
        dtpDateTo.Text = Today
        DateTimePicker2.Text = Today
        DateTimePicker1.Text = Today
        cmbStatus.SelectedIndex = -1
        Getdata()
    End Sub
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs)
        Reset()
    End Sub





    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select S_ID, RTRIM(ServiceCode),ServiceCreationDate, Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Name), RTRIM(ServiceType), RTRIM(ItemDescription), RTRIM(ProblemDescription), ChargesQuote, AdvanceDeposit, EstimatedRepairDate,RTRIM(Status), RTRIM(Service.Remarks) from Customer,Service where Customer.ID=Service.CustomerID and ServiceCreationDate between @d1 and @d2 order by ServiceCreationDate", con)
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

    Private Sub cmbOrderNo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbServiceCode.SelectedIndexChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select S_ID, RTRIM(ServiceCode),ServiceCreationDate, Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Name), RTRIM(ServiceType), RTRIM(ItemDescription), RTRIM(ProblemDescription), ChargesQuote, AdvanceDeposit, EstimatedRepairDate,RTRIM(Status), RTRIM(Service.Remarks) from Customer,Service where Customer.ID=Service.CustomerID and ServiceCode='" & cmbServiceCode.Text & "' order by ServiceCreationDate", con)
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
            If cmbStatus.Text = "" Then
                MessageBox.Show("Please select status", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbStatus.Focus()
                Exit Sub
            End If
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select S_ID, RTRIM(ServiceCode),ServiceCreationDate, Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Name), RTRIM(ServiceType), RTRIM(ItemDescription), RTRIM(ProblemDescription), ChargesQuote, AdvanceDeposit, EstimatedRepairDate,RTRIM(Status), RTRIM(Service.Remarks) from Customer,Service where Customer.ID=Service.CustomerID and ServiceCreationDate between @d1 and @d2 and Status='" & cmbStatus.Text & "' order by ServiceCreationDate", con)
            cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker2.Value.Date
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker1.Value.Date
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8), rdr(9))
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
            cmd = New SqlCommand("Select S_ID, RTRIM(ServiceCode),ServiceCreationDate, Customer.ID,RTRIM(Customer.CustomerID),RTRIM(Name), RTRIM(ServiceType), RTRIM(ItemDescription), RTRIM(ProblemDescription), ChargesQuote, AdvanceDeposit, EstimatedRepairDate,RTRIM(Status), RTRIM(Service.Remarks) from Customer,Service where Customer.ID=Service.CustomerID and Name like '%" & txtCustomerName.Text & "%' order by ServiceCreationDate", con)
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

    Private Sub cmbInvoiceNo_Format(sender As System.Object, e As System.Windows.Forms.ListControlConvertEventArgs) Handles cmbServiceCode.Format
        If (e.DesiredType Is GetType(String)) Then
            e.Value = e.Value.ToString.Trim
        End If
    End Sub

    Private Sub btnExportExcel_Click_1(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        ExportExcel(dgw)
    End Sub
End Class
