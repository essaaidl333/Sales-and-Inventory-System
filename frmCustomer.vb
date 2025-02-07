﻿Imports System.Data.SqlClient
Imports System.IO

Public Class frmCustomer
    Dim s As String
    Dim Photoname As String = ""
    Dim IsImageChanged As Boolean = False
    Sub Reset()
        txtCustomerName.Text = ""
        txtAddress.Text = ""
        txtRemarks.Text = ""
        txtCustomerName.Text = ""
        txtCustomerID.Text = ""
        txtContactNo.Text = ""
        txtOpeningBalance.Text = ""
        cmbOpeningBalanceType.SelectedIndex = 0
        txtEmailID.Text = ""
        txtZipCode.Text = ""
        rbMale.Checked = False
        rbFemale.Checked = False
        txtCity.Text = ""
        txtCustomerName.Focus()
        btnSave.Enabled = True
        btnUpdate.Enabled = False
        btnDelete.Enabled = False
        Picture.Image = My.Resources.photo
        auto()
        cmbState.Text = ""
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
        frmCustomerRecord2.Reset()
    End Sub
    Private Function GenerateID() As String
        con = New SqlConnection(cs)
        Dim value As String = "0000"
        Try
            ' Fetch the latest ID from the database
            con.Open()
            cmd = New SqlCommand("SELECT TOP 1 ID FROM Customer ORDER BY ID DESC", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If rdr.HasRows Then
                rdr.Read()
                value = rdr.Item("ID")
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
    Sub auto()
        Try
            txtID.Text = GenerateID()
            txtCustomerID.Text = "C-" + GenerateID()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Len(Trim(txtCustomerName.Text)) = 0 Then
            MessageBox.Show("الرجاء كتابة اسم العميل", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCustomerName.Focus()
            Exit Sub
        End If
        If ((rbMale.Checked = False) And (rbFemale.Checked = False)) Then
            MessageBox.Show("الرجاء اختيار النوع", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If Len(Trim(txtAddress.Text)) = 0 Then
            MessageBox.Show("الرجاء كتابة العنوان", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAddress.Focus()
            Exit Sub
        End If
        'If Len(Trim(txtCity.Text)) = 0 Then
        '    MessageBox.Show("الرجاء كتابة اسم المدينة", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    txtCity.Focus()
        '    Exit Sub
        'End If
        If Len(Trim(txtContactNo.Text)) = 0 Then
            MessageBox.Show("الرجاء كتابة رقم الهاتف", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContactNo.Focus()
            Exit Sub
        End If

        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select RTRIM(ContactNo) from Customer where ContactNo=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", txtContactNo.Text)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()

            If rdr.Read() Then
                MessageBox.Show("بيانات هذا العميل تم تسجيلها من قبل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If
            con.Close()
            If (rbMale.Checked = True) Then
                s = rbMale.Text
            End If
            If (rbFemale.Checked = True) Then
                s = rbFemale.Text
            End If
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "insert into Customer(ID, CustomerID, [Name], Gender, Address, City, ContactNo, EmailID,Remarks,State,ZipCode,Photo,OpeningBalance,OpeningBalanceType,CustomerType) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7,@d8,@d9,@d10,@d11,@d12,@d13,@d14,'Regular')"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d1", Val(txtID.Text))
            cmd.Parameters.AddWithValue("@d2", txtCustomerID.Text)
            cmd.Parameters.AddWithValue("@d3", txtCustomerName.Text)
            cmd.Parameters.AddWithValue("@d4", s)
            cmd.Parameters.AddWithValue("@d5", txtAddress.Text)
            cmd.Parameters.AddWithValue("@d6", txtCity.Text)
            cmd.Parameters.AddWithValue("@d7", txtContactNo.Text)
            cmd.Parameters.AddWithValue("@d8", txtEmailID.Text)
            cmd.Parameters.AddWithValue("@d9", txtRemarks.Text)
            cmd.Parameters.AddWithValue("@d10", cmbState.Text)
            cmd.Parameters.AddWithValue("@d11", txtZipCode.Text)
            cmd.Parameters.AddWithValue("@d13", Val(txtOpeningBalance.Text))
            cmd.Parameters.AddWithValue("@d14", cmbOpeningBalanceType.Text)
            cmd.Connection = con
            Dim ms As New MemoryStream()
            Dim bmpImage As New Bitmap(Picture.Image)
            bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim data As Byte() = ms.GetBuffer()
            Dim p As New SqlParameter("@d12", SqlDbType.Image)
            p.Value = data
            cmd.Parameters.Add(p)
            cmd.ExecuteNonQuery()
            LogFunc(lblUser.Text, "added the new Customer having Customer id '" & txtCustomerID.Text & "'")
            btnSave.Enabled = False
            fillState()
            con.Close()
            If cmbOpeningBalanceType.SelectedIndex = 0 And Val(txtOpeningBalance.Text) > 0 Then
                LedgerSave(Today, txtCustomerName.Text, txtCustomerID.Text, "الرصيد الافتتاحي", Val(txtOpeningBalance.Text), 0, txtCustomerID.Text, "")
                'SupplierLedgerSave(Today, txtCustomerName.Text, txtCustomerID.Text, "الرصيد الافتتاحي", 0, Val(txtOpeningBalance.Text), txtCustomerID.Text)
            End If
            If cmbOpeningBalanceType.SelectedIndex = 1 And Val(txtOpeningBalance.Text) > 0 Then
                LedgerSave(Today, txtCustomerName.Text, txtCustomerID.Text, "الرصيد الافتتاحي", 0, Val(txtOpeningBalance.Text), txtCustomerID.Text, "")
                'SupplierLedgerSave(Today, txtCustomerName.Text, txtCustomerID.Text, "الرصيد الافتتاحي", Val(txtOpeningBalance.Text), 0, txtCustomerID.Text)
            End If
            MessageBox.Show("تم الحفظ بنجاح", "سجلات العملاء", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If Trim(txtCustomerID.Text) = "C-0001" Then
                MessageBox.Show("لا يمكن حذف بيانات العميل النقدي", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                'txtCity.Focus()
                Exit Sub
            End If

            If MessageBox.Show("هل أنت متأكد أنك تريد حذف سجل هذا العميل?", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                DeleteRecord()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub DeleteRecord()
        Try
            Dim RowsAffected As Integer = 0
            con = New SqlConnection(cs)
            con.Open()
            Dim cl As String = "SELECT Customer.ID FROM Customer INNER JOIN InvoiceInfo ON Customer.ID = InvoiceInfo.CustomerID where Customer.ID=@d1"
            cmd = New SqlCommand(cl)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", Val(txtID.Text))
            rdr = cmd.ExecuteReader()
            If rdr.Read Then
                MessageBox.Show("لا يمكن حذف هذا العميل ... يوجد سجلات مبيعات بأسمه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Not rdr Is Nothing Then
                    rdr.Close()
                End If
                Exit Sub
            End If
            con.Close()
            con = New SqlConnection(cs)
            con.Open()
            Dim cl1 As String = "SELECT Customer.ID FROM Customer INNER JOIN Quotation ON Customer.ID = Quotation.CustomerID where Customer.ID=@d1"
            cmd = New SqlCommand(cl1)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", Val(txtID.Text))
            rdr = cmd.ExecuteReader()
            If rdr.Read Then
                MessageBox.Show("لا يمكن حذف هذا العميل ... يوجد عروض أسعار بأسمه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Not rdr Is Nothing Then
                    rdr.Close()
                End If
                Exit Sub
            End If
            con.Close()
            con = New SqlConnection(cs)
            con.Open()
            Dim cl2 As String = "SELECT Customer.ID FROM Customer INNER JOIN Service ON Customer.ID = Service.CustomerID where Customer.ID=@d1"
            cmd = New SqlCommand(cl2)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", Val(txtID.Text))
            rdr = cmd.ExecuteReader()
            If rdr.Read Then
                MessageBox.Show("لا يمكن حذف هذا العميل يوجد فواتير خدمات بأسمه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If Not rdr Is Nothing Then
                    rdr.Close()
                End If
                Exit Sub
            End If
            con.Close()

            con = New SqlConnection(cs)
            con.Open()
            Dim cq As String = "delete from Customer where ID =" & txtID.Text & ""
            cmd = New SqlCommand(cq)
            cmd.Connection = con
            RowsAffected = cmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                LogFunc(lblUser.Text, "deleted the Customer record having Customer id '" & txtCustomerID.Text & "'")
                MessageBox.Show("تم الحذف بنجاح", "سجلات العملاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Reset()
                fillState()
            Else
                MessageBox.Show("لا يوجد سجلات", "عذراً", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Reset()
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Len(Trim(txtCustomerName.Text)) = 0 Then
            MessageBox.Show("الرجاء كتابة اسم العميل", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCustomerName.Focus()
            Exit Sub
        End If
        If ((rbMale.Checked = False) And (rbFemale.Checked = False)) Then
            MessageBox.Show("الرجاء اختيار النوع", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If Len(Trim(txtAddress.Text)) = 0 Then
            MessageBox.Show("الرجاء كتابة العنوان", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAddress.Focus()
            Exit Sub
        End If
        If Trim(txtCustomerID.Text) = "C-0001" Then
            MessageBox.Show("لا يمكن تعديل بيانات العميل النقدي", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'txtCity.Focus()
            Exit Sub
        End If
        'If cmbState.Text = "" Then
        '    MessageBox.Show("الرجاء كتابة اسم المنطقة", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    cmbState.Focus()
        '    Return
        'End If
        If Len(Trim(txtContactNo.Text)) = 0 Then
            MessageBox.Show("الرجاء كتابة رقم الهاتف.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContactNo.Focus()
            Exit Sub
        End If

        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim cb1 As String = "update LedgerBook set [Name]=@d3 where PartyID=@d1 and Name=@d2"
            cmd = New SqlCommand(cb1)
            cmd.Parameters.AddWithValue("@d1", txtCustomerID.Text)
            cmd.Parameters.AddWithValue("@d2", txtCustName.Text)
            cmd.Parameters.AddWithValue("@d3", txtCustomerName.Text)
            cmd.Connection = con
            cmd.ExecuteNonQuery()
            con.Close()
            If (rbMale.Checked = True) Then
                s = rbMale.Text
            End If
            If (rbFemale.Checked = True) Then
                s = rbFemale.Text
            End If
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "update Customer set CustomerID=@d2,[Name]=@d3,Gender=@d4, Address=@d5,City=@d6, ContactNo=@d7, EmailID=@d8,Remarks=@d9,State=@d10,ZipCode=@d11,Photo=@d12,CustomerType='Regular',OpeningBalance=@d13,OpeningBalanceType=@d14 where ID=@d1"
            cmd = New SqlCommand(cb)
            cmd.Parameters.AddWithValue("@d2", txtCustomerID.Text)
            cmd.Parameters.AddWithValue("@d3", txtCustomerName.Text)
            cmd.Parameters.AddWithValue("@d4", s)
            cmd.Parameters.AddWithValue("@d5", txtAddress.Text)
            cmd.Parameters.AddWithValue("@d6", txtCity.Text)
            cmd.Parameters.AddWithValue("@d7", txtContactNo.Text)
            cmd.Parameters.AddWithValue("@d8", txtEmailID.Text)
            cmd.Parameters.AddWithValue("@d9", txtRemarks.Text)
            cmd.Parameters.AddWithValue("@d10", cmbState.Text)
            cmd.Parameters.AddWithValue("@d11", txtZipCode.Text)
            cmd.Parameters.AddWithValue("@d13", Val(txtOpeningBalance.Text))
            cmd.Parameters.AddWithValue("@d14", cmbOpeningBalanceType.Text)
            cmd.Connection = con
            Dim ms As New MemoryStream()
            Dim bmpImage As New Bitmap(Picture.Image)
            bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim data As Byte() = ms.GetBuffer()
            Dim p As New SqlParameter("@d12", SqlDbType.Image)
            p.Value = data
            cmd.Parameters.Add(p)
            cmd.Parameters.AddWithValue("@d1", Val(txtID.Text))
            cmd.ExecuteNonQuery()
            con.Close()
            If cmbOpeningBalanceType.SelectedIndex = 0 Then
                LedgerUpdate(Today, "الرصيد الافتتاحي", Val(txtOpeningBalance.Text), 0, txtCustomerID.Text, txtCustomerID.Text, "الرصيد الافتتاحي")
            End If
            If cmbOpeningBalanceType.SelectedIndex = 1 Then
                LedgerUpdate(Today, "الرصيد الافتتاحي", 0, Val(txtOpeningBalance.Text), txtCustomerID.Text, txtCustomerID.Text, "الرصيد الافتتاحي")
            End If

            LogFunc(lblUser.Text, "updated the Customer having Customer id '" & txtCustomerID.Text & "'")
            MessageBox.Show("تم التعديل بنجاح", "سجلات العملاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnUpdate.Enabled = False
            fillState()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Reset()
    End Sub


    Private Sub BStartCapture_Click(sender As System.Object, e As System.EventArgs) Handles BStartCapture.Click
        Dim k As New frmCamera
        k.ShowDialog()
        If TempFileNames2.Length > 0 Then

            Picture.Image = Image.FromFile(TempFileNames2)
            Photoname = TempFileNames2
            IsImageChanged = True
        End If
    End Sub

    Private Sub btnGetData_Click(sender As System.Object, e As System.EventArgs) Handles btnGetData.Click
        Dim frm As New frmCustomerRecord
        frm.lblSet.Text = "Customer Entry"
        frm.Getdata()
        frm.ShowDialog()
    End Sub
    Sub fillState()
        Try
            con = New SqlConnection(cs)
            con.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct RTRIM(State) FROM Customer order by 1", con)
            ds = New DataSet("ds")
            adp.Fill(ds)
            dtable = ds.Tables(0)
            cmbState.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                cmbState.Items.Add(drow(0).ToString())
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Browse_Click(sender As System.Object, e As System.EventArgs) Handles Browse.Click
        Try
            With OpenFileDialog1
                .Filter = ("Images |*.png; *.bmp; *.jpg;*.jpeg; *.gif;")
                .FilterIndex = 4
            End With
            'Clear the file name
            OpenFileDialog1.FileName = ""
            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                Picture.Image = Image.FromFile(OpenFileDialog1.FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Sub BRemove_Click(sender As System.Object, e As System.EventArgs) Handles BRemove.Click
        Picture.Image = My.Resources.photo
    End Sub

    Private Sub frmCustomer_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillState()
    End Sub

    Private Sub cmbState_Format(sender As System.Object, e As System.Windows.Forms.ListControlConvertEventArgs) Handles cmbState.Format
        If (e.DesiredType Is GetType(String)) Then
            e.Value = e.Value.ToString.Trim
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtCustomerID.Text <> "" Then
            If cmbOpeningBalanceType.SelectedIndex = 0 And Val(txtOpeningBalance.Text) > 0 Then
                LedgerSave(Today, txtCustomerName.Text, txtCustomerID.Text, "الرصيد الافتتاحي", Val(txtOpeningBalance.Text), 0, txtCustomerID.Text, "")
                'SupplierLedgerSave(Today, txtCustomerName.Text, txtCustomerID.Text, "الرصيد الافتتاحي", 0, Val(txtOpeningBalance.Text), txtCustomerID.Text)
            End If
            If cmbOpeningBalanceType.SelectedIndex = 1 And Val(txtOpeningBalance.Text) > 0 Then
                LedgerSave(Today, txtCustomerName.Text, txtCustomerID.Text, "الرصيد الافتتاحي", 0, Val(txtOpeningBalance.Text), txtCustomerID.Text, "")
                'SupplierLedgerSave(Today, txtCustomerName.Text, txtCustomerID.Text, "الرصيد الافتتاحي", Val(txtOpeningBalance.Text), 0, txtCustomerID.Text)
            End If
            MessageBox.Show("تم الحفظ بنجاح", "سجلات العملاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Button1.Enabled = True
        End If
    End Sub


End Class
