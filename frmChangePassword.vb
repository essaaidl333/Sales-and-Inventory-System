﻿Imports System.Data.SqlClient
Public Class frmChangePassword
    Declare Function Wow64DisableWow64FsRedirection Lib "kernel32" (ByRef oldvalue As Long) As Boolean
    Declare Function Wow64EnableWow64FsRedirection Lib "kernel32" (ByRef oldvalue As Long) As Boolean

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim RowsAffected As Integer = 0
            If Len(Trim(UserID.Text)) = 0 Then
                MessageBox.Show("الرجاء كتابة اسم المستخدم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                UserID.Focus()
                Exit Sub
            End If
            If Len(Trim(OldPassword.Text)) = 0 Then
                MessageBox.Show("الرجاء كتابة كلمة السر القديمة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                OldPassword.Focus()
                Exit Sub
            End If
            If Len(Trim(NewPassword.Text)) = 0 Then
                MessageBox.Show("الرجاء كتابة كلمة السر الجديدة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                NewPassword.Focus()
                Exit Sub
            End If
            If Len(Trim(ConfirmPassword.Text)) = 0 Then
                MessageBox.Show("الرجاء تأكيد كلمة السر الجديدة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ConfirmPassword.Focus()
                Exit Sub
            End If
            If NewPassword.TextLength < 5 Then
                MessageBox.Show("كلمة السر يجب الا تقل عن خمسة حروف أو أرقام", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                NewPassword.Text = ""
                ConfirmPassword.Text = ""
                NewPassword.Focus()
                Exit Sub
            ElseIf NewPassword.Text <> ConfirmPassword.Text Then
                MessageBox.Show("كلمة السر غير مطابقة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                NewPassword.Text = ""
                OldPassword.Text = ""
                ConfirmPassword.Text = ""
                OldPassword.Focus()
                Exit Sub
            ElseIf OldPassword.Text = NewPassword.Text Then
                MessageBox.Show("كلمة السر الجديدة يجب أن تكون مختلفة عن القديمة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                NewPassword.Text = ""
                ConfirmPassword.Text = ""
                NewPassword.Focus()
                Exit Sub
            End If
            con = New SqlConnection(cs)
            con.Open()
            Dim co As String = "update Registration set password =@d1 where userid=@d2 and password =@d3"
            cmd = New SqlCommand(co)
            cmd.Parameters.AddWithValue("@d1", Encrypt(NewPassword.Text))
            cmd.Parameters.AddWithValue("@d2", UserID.Text)
            cmd.Parameters.AddWithValue("@d3", Encrypt(OldPassword.Text))
            cmd.Connection = con
            RowsAffected = cmd.ExecuteNonQuery()
            If RowsAffected > 0 Then
                Dim st As String = "تم تغيير كلمة السر بنجاح"
                LogFunc(UserID.Text, st)
                frmCustomDialog5.ShowDialog()
                Me.Hide()
                frmLogin.Show()
                frmLogin.UserID.Text = ""
                frmLogin.Password.Text = ""
                frmLogin.UserID.Focus()
            Else

                MessageBox.Show("خطأ في اسم المستخدم أو كلمة السر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                UserID.Text = ""
                NewPassword.Text = ""
                OldPassword.Text = ""
                ConfirmPassword.Text = ""
                UserID.Focus()
            End If
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Sub OSKeyboard()
        Dim old As Long
        If Environment.Is64BitOperatingSystem Then
            If Wow64DisableWow64FsRedirection(old) Then
                Process.Start("osk.exe")
                Wow64EnableWow64FsRedirection(old)
            End If
        Else
            Process.Start("osk.exe")
        End If
    End Sub
    Private Sub frmChangePassword1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        frmLogin.Show()
        frmLogin.UserID.Text = ""
        frmLogin.Password.Text = ""
        frmLogin.UserID.Focus()
    End Sub

    Private Sub frmChangePassword_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Panel1.Location = New Point(Me.ClientSize.Width / 2 - Panel1.Size.Width / 2, Me.ClientSize.Height / 2 - Panel1.Size.Height / 2)
        Panel1.Anchor = AnchorStyles.None
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Hide()
        frmLogin.Show()
        frmLogin.UserID.Text = ""
        frmLogin.Password.Text = ""
        frmLogin.UserID.Focus()
    End Sub

    Private Sub btnKeyboard_Click(sender As System.Object, e As System.EventArgs) Handles btnKeyboard.Click
        OSKeyboard()
    End Sub

End Class