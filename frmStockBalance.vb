Imports System.Data.SqlClient
Imports System.IO

Imports Microsoft.SqlServer.Management.Smo
Imports System.Globalization

Public Class frmStockBalance
    Dim Filename As String
    Dim i1, i2 As Integer

    Private Sub frmStockBalance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Getdata()
        DataGridView1.ClearSelection()
        DataGridView1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        DataGridView1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

    End Sub

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(Product.ProductCode),RTRIM(ProductName),RTRIM(Temp_Stock.Barcode),CostPrice,SellingPrice,Discount,VAT,Qty from Temp_Stock,Product where Product.PID=Temp_Stock.ProductID and Qty > 0 order by ProductCode", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            DataGridView1.Rows.Clear()
            While (rdr.Read() = True)
                DataGridView1.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7))
            End While
            For Each r As DataGridViewRow In Me.DataGridView1.Rows
                con = New SqlConnection(cs)
                con.Open()
                Dim ct As String = "select ReorderPoint from Product where ProductCode=@d1"
                cmd = New SqlCommand(ct)
                cmd.Connection = con
                cmd.Parameters.AddWithValue("@d1", r.Cells(0).Value.ToString())
                rdr = cmd.ExecuteReader()
                If (rdr.Read()) Then

                    i1 = rdr.GetValue(0)
                End If
                con.Close()
                con = New SqlConnection(cs)
                con.Open()
                Dim ct1 As String = "select sum(Qty) from Product,Temp_Stock where Product.PID=Temp_Stock.ProductID and ProductCode=@d1"
                cmd = New SqlCommand(ct1)
                cmd.Connection = con
                cmd.Parameters.AddWithValue("@d1", r.Cells(0).Value.ToString())
                rdr = cmd.ExecuteReader()
                If (rdr.Read()) Then
                    i2 = rdr.GetValue(0)
                End If
                con.Close()
                If i2 < i1 Then
                    r.DefaultCellStyle.BackColor = Color.Red
                End If
            Next
            con.Close()
            DataGridView1.ClearSelection()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        ExportExcel(DataGridView1)

    End Sub

    Private Sub txtProductName_TextChanged(sender As Object, e As EventArgs) Handles txtProductName.TextChanged
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(Product.ProductCode),RTRIM(ProductName),RTRIM(Temp_Stock.Barcode),CostPrice,SellingPrice,Discount,VAT,Qty from Temp_Stock,Product where Product.PID=Temp_Stock.ProductID and qty > 0 and ProductName like '%" & txtProductName.Text & "%' order by ProductName", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            DataGridView1.Rows.Clear()
            While (rdr.Read() = True)
                DataGridView1.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7))
            End While
            For Each r As DataGridViewRow In Me.DataGridView1.Rows
                con = New SqlConnection(cs)
                con.Open()
                Dim ct As String = "select ReorderPoint from Product where ProductCode=@d1"
                cmd = New SqlCommand(ct)
                cmd.Connection = con
                cmd.Parameters.AddWithValue("@d1", r.Cells(0).Value.ToString())
                rdr = cmd.ExecuteReader()
                If (rdr.Read()) Then

                    i1 = rdr.GetValue(0)
                End If
                con.Close()
                con = New SqlConnection(cs)
                con.Open()
                Dim ct1 As String = "select sum(Qty) from Product,Temp_Stock where Product.PID=Temp_Stock.ProductID and ProductCode=@d1"
                cmd = New SqlCommand(ct1)
                cmd.Connection = con
                cmd.Parameters.AddWithValue("@d1", r.Cells(0).Value.ToString())
                rdr = cmd.ExecuteReader()
                If (rdr.Read()) Then
                    i2 = rdr.GetValue(0)
                End If
                con.Close()
                If i2 < i1 Then
                    r.DefaultCellStyle.BackColor = Color.Red
                End If
            Next
            con.Close()
            DataGridView1.ClearSelection()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Sub Reset()
        txtProductName.Text = ""
        Getdata()
    End Sub

End Class