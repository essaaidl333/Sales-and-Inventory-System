Imports System.Data.SqlClient

Imports System.IO

Public Class frmCurrentStock

    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT PID, RTRIM(Product.ProductCode),RTRIM(ProductName),RTRIM(Temp_Stock.Barcode),(CostPrice),(SellingPrice),(Discount),(VAT),Qty from Temp_Stock,Product where Product.PID=Temp_Stock.ProductID and Qty > 0  order by ProductCode", con)
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

        dgw.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dgw.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dgw.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dgw.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dgw.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Private Sub dgw_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles dgw.MouseClick
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
        txtProductName.Text = ""
        txtBarcode.Text = ""
        Getdata()
    End Sub
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub btnExportExcel_Click(sender As System.Object, e As System.EventArgs) Handles btnExportExcel.Click
        ExportExcel(dgw)
    End Sub

    Private Sub txtProductName_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtProductName.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                con = New SqlConnection(cs)
                con.Open()
                cmd = New SqlCommand("SELECT PID, RTRIM(Product.ProductCode),RTRIM(ProductName),RTRIM(Temp_Stock.Barcode),(CostPrice),(SellingPrice),(Discount),(VAT),Qty from Temp_Stock,Product where Product.PID=Temp_Stock.ProductID and Qty > 0 and ProductName like '%" & txtProductName.Text & "%' order by ProductCode", con)
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                dgw.Rows.Clear()
                While (rdr.Read() = True)
                    dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
                End While
                con.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtBarcode_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtBarcode.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                con = New SqlConnection(cs)
                con.Open()
                cmd = New SqlCommand("SELECT PID, RTRIM(Product.ProductCode),RTRIM(ProductName),RTRIM(Temp_Stock.Barcode),(CostPrice),(SellingPrice),(Discount),(VAT),Qty from Temp_Stock,Product where Product.PID=Temp_Stock.ProductID and Qty > 0 and Temp_Stock.Barcode like '%" & txtBarcode.Text & "%' order by ProductCode", con)
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                dgw.Rows.Clear()
                While (rdr.Read() = True)
                    dgw.Rows.Add(rdr(0), rdr(1), rdr(2), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7), rdr(8))
                End While
                con.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgw_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgw.CellContentClick

    End Sub

    Private Sub dgw_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dgw.MouseDoubleClick
        Try
            If dgw.Rows.Count > 0 Then
                Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                If lblSet.Text = "Billing" Then
                    frmPOS.Show()
                    Me.Hide()
                    frmPOS.txtProductID.Text = dr.Cells(0).Value.ToString()
                    frmPOS.txtProductCode.Text = dr.Cells(1).Value.ToString()
                    frmPOS.txtProductName.Text = dr.Cells(2).Value.ToString()
                    frmPOS.txtBarcode.Text = dr.Cells(3).Value.ToString()
                    frmPOS.txtCostPrice.Text = dr.Cells(4).Value.ToString()
                    frmPOS.txtSellingPrice.Text = dr.Cells(5).Value.ToString()
                    Dim num As Double
                    num = Val(dr.Cells(5).Value) - Val(dr.Cells(4).Value)
                    num = Math.Round(num, 2)
                    frmPOS.txtMargin.Text = num
                    frmPOS.txtDiscountPer.Text = dr.Cells(6).Value.ToString()
                    frmPOS.txtVAT.Text = dr.Cells(7).Value.ToString()
                    frmPOS.txtQty.Focus()
                    lblSet.Text = ""
                End If
                If lblSet.Text = "Billing1" Then
                    frmServiceBilling.Show()
                    Me.Hide()
                    frmServiceBilling.txtProductID.Text = dr.Cells(0).Value.ToString()
                    frmServiceBilling.txtProductCode.Text = dr.Cells(1).Value.ToString()
                    frmServiceBilling.txtProductName.Text = dr.Cells(2).Value.ToString()
                    frmServiceBilling.txtBarcode.Text = dr.Cells(3).Value.ToString()
                    frmServiceBilling.txtCostPrice.Text = dr.Cells(4).Value.ToString()
                    frmServiceBilling.txtSellingPrice.Text = dr.Cells(5).Value.ToString()
                    Dim num As Double
                    num = Val(dr.Cells(5).Value) - Val(dr.Cells(4).Value)
                    num = Math.Round(num, 2)
                    frmServiceBilling.txtMargin.Text = num
                    frmServiceBilling.txtDiscountPer.Text = dr.Cells(6).Value.ToString()
                    frmServiceBilling.txtVAT.Text = dr.Cells(7).Value.ToString()
                    frmServiceBilling.txtQty.Focus()
                    lblSet.Text = ""
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
End Class
