﻿Imports System.Data.SqlClient

Public Class frmTaxReport

    Sub Reset()
        dtpDateFrom.Value = Today
        dtpDateTo.Value = Today
        DateTimePicker1.Value = Today
        DateTimePicker2.Value = Today
    End Sub
    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Cursor = Cursors.Default
        Timer1.Enabled = False
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "Select InvoiceNo,InvoiceDate,Customer.CustomerID,Name,Sum(VAT) FROM InvoiceInfo INNER JOIN Customer ON InvoiceInfo.CustomerID = Customer.ID INNER JOIN Invoice_Product ON InvoiceInfo.Inv_ID = Invoice_Product.InvoiceID where InvoiceDate between @d2 and @d3 group by InvoiceNo,InvoiceDate,Customer.CustomerID,Name having sum(VAT) > 0 order by Name"
            cmd = New SqlCommand(ct)
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d3", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If Not rdr.Read() Then
                MessageBox.Show("عذرًا...لا يوجد سجلات", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select InvoiceNo,InvoiceDate,Customer.CustomerID,Name,Sum(VAT) FROM InvoiceInfo INNER JOIN Customer ON InvoiceInfo.CustomerID = Customer.ID INNER JOIN Invoice_Product ON InvoiceInfo.Inv_ID = Invoice_Product.InvoiceID where InvoiceDate between @d2 and @d3 group by InvoiceNo,InvoiceDate,Customer.CustomerID,Name having sum(VAT) > 0 order by Name", con)
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = dtpDateFrom.Value.Date
            cmd.Parameters.Add("@d3", SqlDbType.DateTime, 30, "Date").Value = dtpDateTo.Value.Date
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("ProductBillingTaxReport.xml")
            Dim rpt As New rptSalesTaxReport
            rpt.SetDataSource(ds)
            rpt.SetParameterValue("p1", dtpDateFrom.Value.Date)
            rpt.SetParameterValue("p2", dtpDateTo.Value.Date)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Try
            Cursor = Cursors.WaitCursor
            Timer1.Enabled = True
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "Select InvoiceNo,InvoiceDate,Customer.CustomerID,Name,Sum(VAT),(ServiceTax) FROM InvoiceInfo1 INNER JOIN Invoice1_Product ON InvoiceInfo1.Inv_ID = Invoice1_Product.InvoiceID INNER JOIN Service ON InvoiceInfo1.ServiceID = Service.S_ID INNER JOIN Customer ON Service.CustomerID = Customer.ID where InvoiceDate between @d2 and @d3 group by InvoiceNo,InvoiceDate,Customer.CustomerID,Name,ServiceTax having (Sum(VAT)+ (ServiceTax)) > 0 order by Name"
            cmd = New SqlCommand(ct)
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker2.Value.Date
            cmd.Parameters.Add("@d3", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker1.Value.Date
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If Not rdr.Read() Then
                MessageBox.Show("عذرًا...لا يوجد سجلات", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("Select InvoiceNo,InvoiceDate,Customer.CustomerID,Name,Sum(VAT),(ServiceTax) FROM InvoiceInfo1 INNER JOIN Invoice1_Product ON InvoiceInfo1.Inv_ID = Invoice1_Product.InvoiceID INNER JOIN Service ON InvoiceInfo1.ServiceID = Service.S_ID INNER JOIN Customer ON Service.CustomerID = Customer.ID where InvoiceDate between @d2 and @d3 group by InvoiceNo,InvoiceDate,Customer.CustomerID,Name,ServiceTax having (Sum(VAT)+ (ServiceTax)) > 0 order by Name", con)
            cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker2.Value.Date
            cmd.Parameters.Add("@d3", SqlDbType.DateTime, 30, "Date").Value = DateTimePicker1.Value.Date
            adp = New SqlDataAdapter(cmd)
            dtable = New DataTable()
            adp.Fill(dtable)
            con.Close()
            ds = New DataSet()
            ds.Tables.Add(dtable)
            ds.WriteXmlSchema("ServiceTaxReport.xml")
            Dim rpt As New rptServiceTaxReport
            rpt.SetDataSource(ds)
            rpt.SetParameterValue("p1", DateTimePicker2.Value.Date)
            rpt.SetParameterValue("p2", DateTimePicker1.Value.Date)
            frmReport.CrystalReportViewer1.ReportSource = rpt
            frmReport.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
