﻿Imports System.Data.SqlClient
Public Class frmRegistration
    Dim st1 As String
    Sub Reset()
        txtContactNo.Text = ""
        txtEmailID.Text = ""
        txtName.Text = ""
        txtPassword.Text = ""
        txtUserID.Text = ""
        cmbUserType.SelectedIndex = -1
        chkActive.Checked = True
        txtUserID.Focus()
        btnSave.Enabled = True
        btnUpdate.Enabled = False
        btnDelete.Enabled = False
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim obj As frmMainMenu = DirectCast(Application.OpenForms("frmMainMenu"), frmMainMenu)
        obj.lblUser.Text = lblUser.Text
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtUserID.Text = "" Then
            MessageBox.Show("الرجاء كتابة كود المستخدم", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUserID.Focus()
            Return
        End If
        If cmbUserType.Text = "" Then
            MessageBox.Show("الرجاء اختيار صلاحية المستخدم", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbUserType.Focus()
            Return
        End If
        If txtPassword.Text = "" Then
            MessageBox.Show("الرجاء كتابة كلمة السر", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPassword.Focus()
            Return
        End If
        If txtName.Text = "" Then
            MessageBox.Show("الرجاء كتابة اسم المستخدم", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()
            Return
        End If
        If txtContactNo.Text = "" Then
            MessageBox.Show("الرجاء كتابة رقم الاتصال.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContactNo.Focus()
            Return
        End If
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select userid from registration where userid=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", txtUserID.Text)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                MessageBox.Show("كود المستخدم موجود مسبقا", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                txtUserID.Text = ""
                txtUserID.Focus()
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If
            con.Close()
            If chkActive.Checked = True Then
                st1 = "Yes"
            Else
                st1 = "No"
            End If
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "insert into Registration(userid, UserType, Password, Name, ContactNo, EmailID,JoiningDate,Active) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7,@d8)"
            cmd = New SqlCommand(cb)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", txtUserID.Text)
            cmd.Parameters.AddWithValue("@d2", cmbUserType.Text)
            cmd.Parameters.AddWithValue("@d3", Encrypt(txtPassword.Text.Trim()))
            cmd.Parameters.AddWithValue("@d4", txtName.Text)
            cmd.Parameters.AddWithValue("@d5", txtContactNo.Text)
            cmd.Parameters.AddWithValue("@d6", txtEmailID.Text)
            cmd.Parameters.AddWithValue("@d7", Now)
            cmd.Parameters.AddWithValue("@d8", st1)
            cmd.ExecuteReader()
            con.Close()
            Dim st As String = "added the new user '" & txtUserID.Text & "'"
            LogFunc(lblUser.Text, st)
            MessageBox.Show("تم التسجيل بنجاح", "المستخدمين", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnSave.Enabled = False
            Getdata()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If MessageBox.Show("هل انت متأكد أنك تريد حذف بيانات هذا المستخدم?", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                DeleteRecord()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub DeleteRecord()

        Try
            If txtUserID.Text = "admin" Or txtUserID.Text = "Admin" Then
                MessageBox.Show("لا يمكن حذف حساب المسؤول", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            Dim RowsAffected As Integer = 0
            con = New SqlConnection(cs)
            con.Open()
            Dim cq As String = "delete from Registration where userid='" & txtUserID.Text & "'"
            cmd = New SqlCommand(cq)
            cmd.Connection = con
            RowsAffected = cmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                Dim st As String = "deleted the user '" & txtUserID.Text & "'"
                LogFunc(lblUser.Text, st)
                MessageBox.Show("تم الحذف بنجاح", "السجلات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Getdata()
                Reset()
            Else
                MessageBox.Show("لا يوجد سجلات", "عذرًا ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Reset()
            End If
            If con.State = ConnectionState.Open Then
                con.Close()

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
            If txtUserID.Text = "" Then
                MessageBox.Show("الرجاء كتابة كود المستخدم", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtUserID.Focus()
                Return
            End If
            If cmbUserType.Text = "" Then
                MessageBox.Show("الرجاء اختيار صلاحية المستخدم", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbUserType.Focus()
                Return
            End If
            If txtPassword.Text = "" Then
                MessageBox.Show("الرجاء كتابة كلمة السر", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPassword.Focus()
                Return
            End If
            If txtName.Text = "" Then
                MessageBox.Show("الرجاء كتابة اسم المستخدم", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtName.Focus()
                Return
            End If
            If txtContactNo.Text = "" Then
                MessageBox.Show("الرجاء كتابة رقم الاتصال", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtContactNo.Focus()
                Return
            End If
            If chkActive.Checked = True Then
                st1 = "Yes"
            Else
                st1 = "No"
            End If
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "update registration set userid=@d1, usertype=@d2,password=@d3,name=@d4,contactno=@d5,emailid=@d6,Active=@d8 where userid=@d7"
            cmd = New SqlCommand(cb)
            cmd.Connection = con
            cmd.Parameters.AddWithValue("@d1", txtUserID.Text)
            cmd.Parameters.AddWithValue("@d2", cmbUserType.Text)
            cmd.Parameters.AddWithValue("@d3", Encrypt(txtPassword.Text.Trim()))
            cmd.Parameters.AddWithValue("@d4", txtName.Text)
            cmd.Parameters.AddWithValue("@d5", txtContactNo.Text)
            cmd.Parameters.AddWithValue("@d6", txtEmailID.Text)
            cmd.Parameters.AddWithValue("@d7", TextBox1.Text)
            cmd.Parameters.AddWithValue("@d8", st1)
            cmd.ExecuteReader()
            con.Close()
            Dim st As String = "updated the user '" & txtUserID.Text & "' details"
            LogFunc(lblUser.Text, st)
            MessageBox.Show("تم التعديل بنجاح", "معلومات المستخدم", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnUpdate.Enabled = False
            Getdata()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub
    Public Sub Getdata()
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = New SqlCommand("SELECT RTRIM(userid), RTRIM(UserType), RTRIM(Password), RTRIM(Name), RTRIM(EmailID), RTRIM(ContactNo),RTRIM(Active),JoiningDate from Registration order by JoiningDate", con)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            dgw.Rows.Clear()
            While (rdr.Read() = True)
                dgw.Rows.Add(rdr(0), rdr(1), Decrypt(rdr(2)), rdr(3), rdr(4), rdr(5), rdr(6), rdr(7))
            End While
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Reset()
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

    Private Sub frmRegistration_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Getdata()
    End Sub

    Private Sub dgw_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgw.MouseClick
        Try
            If dgw.Rows.Count > 0 Then
                Dim dr As DataGridViewRow = dgw.SelectedRows(0)
                txtUserID.Text = dr.Cells(0).Value.ToString()
                TextBox1.Text = dr.Cells(0).Value.ToString()
                cmbUserType.Text = dr.Cells(1).Value.ToString()
                txtPassword.Text = dr.Cells(2).Value.ToString()
                txtName.Text = dr.Cells(3).Value.ToString()
                txtContactNo.Text = dr.Cells(5).Value.ToString()
                txtEmailID.Text = dr.Cells(4).Value.ToString()
                If dr.Cells(6).Value.ToString() = "Yes" Then
                    chkActive.Checked = True
                Else
                    chkActive.Checked = False
                End If
                btnUpdate.Enabled = True
                btnDelete.Enabled = True
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCheckAvailability_Click(sender As System.Object, e As System.EventArgs) Handles btnCheckAvailability.Click
        If txtUserID.Text = "" Then
            MessageBox.Show("الرجاء كتابة كود المستخدم", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUserID.Focus()
            Return
        End If
        Try
            con = New SqlConnection(cs)
            con.Open()
            Dim ct As String = "select userid from registration where userid=@d1"
            cmd = New SqlCommand(ct)
            cmd.Parameters.AddWithValue("@d1", txtUserID.Text)
            cmd.Connection = con
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                MessageBox.Show("كود المستخدم غير متاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            Else
                MessageBox.Show("كود المستخدم متاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
