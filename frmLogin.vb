Imports System.Data.SqlClient
Imports System.Net.Mail

Public Class frmLogin
    Dim frm As New frmMainMenu
    Declare Function Wow64DisableWow64FsRedirection Lib "kernel32" (ByRef oldvalue As Long) As Boolean
    Declare Function Wow64EnableWow64FsRedirection Lib "kernel32" (ByRef oldvalue As Long) As Boolean
    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If Len(Trim(UserID.Text)) = 0 Then
            MessageBox.Show("Please enter user id", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            UserID.Focus()
            Exit Sub
        End If
        If Len(Trim(Password.Text)) = 0 Then
            MessageBox.Show("Please enter password", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Password.Focus()
            Exit Sub
        End If
        Try
            con = New SqlConnection(cs)
            con.Open()
            cmd = con.CreateCommand()

            cmd.CommandText = "SELECT RTRIM(UserID),RTRIM(Password) FROM Registration where UserID = @d1 and Password=@d2 and Active='Yes'"

            cmd.Parameters.AddWithValue("@d1", UserID.Text)
            cmd.Parameters.AddWithValue("@d2", Password.Text)
            rdr = cmd.ExecuteReader()
            If rdr.Read() Then
                con = New SqlConnection(cs)
                con.Open()
                cmd = con.CreateCommand()
                MessageBox.Show("uYUYVOIERYV")
                cmd.CommandText = "SELECT UserID FROM Registration where UserID=@d3 and Password=@d4"
                cmd.Parameters.AddWithValue("@d3", UserID.Text)
                cmd.Parameters.AddWithValue("@d4", Password.Text)
                rdr = cmd.ExecuteReader()
                If rdr.Read() Then
                    TextBox1.Text = rdr.GetValue(0).ToString.Trim
                End If
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If


                con = New SqlConnection(cs)
                con.Open()
                cmd = con.CreateCommand()
                cmd.CommandText = "SELECT usertype FROM Registration where UserID=@d3 and Password=@d4"
                cmd.Parameters.AddWithValue("@d3", UserID.Text)
                cmd.Parameters.AddWithValue("@d4", Password.Text)
                rdr = cmd.ExecuteReader()
                If rdr.Read() Then
                    UserType.Text = rdr.GetValue(0).ToString.Trim
                End If
                If (rdr IsNot Nothing) Then
                    rdr.Close()
                End If
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If




                If UserType.Text = "Admin" Then
                    frm.MasterEntryToolStripMenuItem.Enabled = True
                    frm.المستخدمينToolStripMenuItem.Enabled = True
                    frm.LogsToolStripMenuItem.Enabled = True
                    frm.استرجاعنسخةاحتياطيةToolStripMenuItem.Enabled = True
                    frm.CustomerToolStripMenuItem.Enabled = True
                    frm.SupplierToolStripMenuItem.Enabled = True
                    frm.ProductToolStripMenuItem.Enabled = True
                    frm.StockToolStripMenuItem.Enabled = True
                    frm.ServiceToolStripMenuItem.Enabled = True
                    frm.جردالمخزونToolStripMenuItem.Enabled = True
                    frm.BillingToolStripMenuItem.Enabled = True
                    frm.QuotationToolStripMenuItem.Enabled = True
                    frm.RecordToolStripMenuItem.Enabled = True
                    frm.ReportsToolStripMenuItem.Enabled = True
                    frm.VoucherToolStripMenuItem.Enabled = True
                    frm.SalesmanToolStripMenuItem.Enabled = True
                    frm.SendSMSToolStripMenuItem.Enabled = True
                    frm.SalesReturnToolStripMenuItem.Enabled = True
                    frm.PaymentToolStripMenuItem.Enabled = True
                    frm.ToolStripMenuItemSetting.Enabled = True
                    frm.ToolStripMenuItem2.Enabled = True
                    frm.lblUser.Text = UserID.Text
                    frm.TextBox1.Text = TextBox1.Text
                    frm.lblUserType.Text = UserType.Text
                    '###############33333333

                    If frmSplash.DateTimePicker1.Value <= frmSplash.DateTimePicker2.Value Then
                        'MsgBox(" النسخة منشطة")
                        Dim st As String = "Successfully logged in"
                        LogFunc(UserID.Text, st)
                        Me.Hide()
                        frm.Show()

                    End If
                End If

                'End If
                If UserType.Text = "Sales Person" Then
                    frm.MasterEntryToolStripMenuItem.Enabled = False
                    frm.المستخدمينToolStripMenuItem.Enabled = False
                    frm.LogsToolStripMenuItem.Enabled = False
                    frm.استرجاعنسخةاحتياطيةToolStripMenuItem.Enabled = False
                    frm.CustomerToolStripMenuItem.Enabled = True
                    frm.SupplierToolStripMenuItem.Enabled = False
                    frm.ProductToolStripMenuItem.Enabled = False
                    frm.StockToolStripMenuItem.Enabled = False
                    frm.ServiceToolStripMenuItem.Enabled = True
                    frm.جردالمخزونToolStripMenuItem.Enabled = True
                    frm.BillingToolStripMenuItem.Enabled = True
                    frm.QuotationToolStripMenuItem.Enabled = True
                    frm.RecordToolStripMenuItem.Enabled = False
                    frm.ReportsToolStripMenuItem.Enabled = False
                    frm.VoucherToolStripMenuItem.Enabled = False
                    frm.SalesmanToolStripMenuItem.Enabled = False
                    frm.SendSMSToolStripMenuItem.Enabled = False
                    frm.SalesReturnToolStripMenuItem.Enabled = True
                    frm.PaymentToolStripMenuItem.Enabled = False
                    frm.lblUser.Text = UserID.Text
                    frm.TextBox1.Text = TextBox1.Text
                    frm.lblUserType.Text = UserType.Text
                    ' Dim st As String = "Successfully logged in"
                    ' LogFunc(UserID.Text, st)
                    '  Me.Hide()
                    ' frm.Show()
                    '###
                    If frmSplash.DateTimePicker1.Value <= frmSplash.DateTimePicker2.Value Then
                        'MsgBox(" النسخة منشطة")
                        Dim st As String = "Successfully logged in"
                        LogFunc(UserID.Text, st)
                        Me.Hide()
                        frm.Show()

                    End If
                End If
                If UserType.Text = "Inventory Manager" Then
                    frm.MasterEntryToolStripMenuItem.Enabled = False
                    frm.المستخدمينToolStripMenuItem.Enabled = False
                    frm.LogsToolStripMenuItem.Enabled = False
                    frm.استرجاعنسخةاحتياطيةToolStripMenuItem.Enabled = False
                    frm.CustomerToolStripMenuItem.Enabled = False
                    frm.SupplierToolStripMenuItem.Enabled = False
                    frm.ProductToolStripMenuItem.Enabled = True
                    frm.StockToolStripMenuItem.Enabled = True
                    frm.ServiceToolStripMenuItem.Enabled = False
                    frm.جردالمخزونToolStripMenuItem.Enabled = True
                    frm.BillingToolStripMenuItem.Enabled = False
                    frm.QuotationToolStripMenuItem.Enabled = False
                    frm.RecordToolStripMenuItem.Enabled = False
                    frm.ReportsToolStripMenuItem.Enabled = False
                    frm.VoucherToolStripMenuItem.Enabled = False
                    frm.SalesmanToolStripMenuItem.Enabled = False
                    frm.SendSMSToolStripMenuItem.Enabled = False
                    frm.PaymentToolStripMenuItem.Enabled = False
                    frm.lblUser.Text = UserID.Text
                    frm.TextBox1.Text = TextBox1.Text
                    frm.lblUserType.Text = UserType.Text
                    ' Dim st As String = "Successfully logged in"
                    'LogFunc(UserID.Text, st)
                    'Me.Hide()
                    'frm.Show()
                    '#####
                    If frmSplash.DateTimePicker1.Value <= frmSplash.DateTimePicker2.Value Then
                        'MsgBox(" النسخة منشطة")
                        Dim st As String = "Successfully logged in"
                        LogFunc(UserID.Text, st)
                        Me.Hide()
                        frm.Show()

                    End If
                End If
                '#################3
                If UserType.Text = "accountant" Then
                    frm.MasterEntryToolStripMenuItem.Enabled = False
                    frm.المستخدمينToolStripMenuItem.Enabled = False
                    frm.LogsToolStripMenuItem.Enabled = False
                    frm.استرجاعنسخةاحتياطيةToolStripMenuItem.Enabled = False
                    frm.CustomerToolStripMenuItem.Enabled = False
                    frm.SupplierToolStripMenuItem.Enabled = False
                    frm.ProductToolStripMenuItem.Enabled = False
                    frm.StockToolStripMenuItem.Enabled = False
                    frm.ServiceToolStripMenuItem.Enabled = True
                    frm.جردالمخزونToolStripMenuItem.Enabled = True
                    frm.BillingToolStripMenuItem.Enabled = False
                    frm.QuotationToolStripMenuItem.Enabled = True
                    frm.RecordToolStripMenuItem.Enabled = True
                    frm.ReportsToolStripMenuItem.Enabled = True
                    frm.VoucherToolStripMenuItem.Enabled = False
                    frm.SalesmanToolStripMenuItem.Enabled = False
                    frm.SendSMSToolStripMenuItem.Enabled = False
                    frm.PaymentToolStripMenuItem.Enabled = True
                    frm.lblUser.Text = UserID.Text
                    frm.TextBox1.Text = TextBox1.Text
                    frm.lblUserType.Text = UserType.Text
                    ' Dim st As String = "Successfully logged in"
                    'LogFunc(UserID.Text, st)
                    'Me.Hide()
                    'frm.Show()
                    '#####
                    If frmSplash.DateTimePicker1.Value <= frmSplash.DateTimePicker2.Value Then
                        'MsgBox(" النسخة منشطة")
                        Dim st As String = "Successfully logged in"
                        LogFunc(UserID.Text, st)
                        Me.Hide()
                        frm.Show()

                    End If
                End If
                '#####
            Else
                MsgBox("Login is Failed...Try again !", MsgBoxStyle.Critical, "Login Denied")
                UserID.Text = ""
                Password.Text = ""
                UserID.Focus()
            End If
            cmd.Dispose()
            con.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        End
    End Sub


    Private Sub LoginForm1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Panel1.Location = New Point(Me.ClientSize.Width / 2 - Panel1.Size.Width / 2, Me.ClientSize.Height / 2 - Panel1.Size.Height / 2)
        Panel1.Anchor = AnchorStyles.None
        fillUsers()
        UserID.SelectedIndex = 0

    End Sub
    Sub fillUsers()
        Try
            con = New SqlConnection(cs)
            con.Open()
            adp = New SqlDataAdapter()
            adp.SelectCommand = New SqlCommand("SELECT distinct (UserID) FROM Registration", con)
            ds = New DataSet("ds")
            adp.Fill(ds)
            dtable = ds.Tables(0)
            UserID.Items.Clear()
            For Each drow As DataRow In dtable.Rows
                UserID.Items.Add(drow(0).ToString())
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub btnChangePassword_Click(sender As System.Object, e As System.EventArgs) Handles btnChangePassword.Click
        Me.Hide()
        frmChangePassword.Show()
        frmChangePassword.UserID.Text = ""
        frmChangePassword.OldPassword.Text = ""
        frmChangePassword.NewPassword.Text = ""
        frmChangePassword.ConfirmPassword.Text = ""
        frmChangePassword.UserID.Focus()
    End Sub

    Private Sub btnChangePassword_MouseHover(sender As System.Object, e As System.EventArgs) Handles btnChangePassword.MouseHover
        ToolTip1.IsBalloon = True
        ToolTip1.UseAnimation = True
        ToolTip1.ToolTipTitle = ""
        ToolTip1.SetToolTip(btnChangePassword, "Change Password")
    End Sub

    Private Sub btnRecoveryPassword_MouseHover(sender As System.Object, e As System.EventArgs) Handles btnRecoveryPassword.MouseHover
        ToolTip1.IsBalloon = True
        ToolTip1.UseAnimation = True
        ToolTip1.ToolTipTitle = ""
        ToolTip1.SetToolTip(btnRecoveryPassword, "Password Recovery")
    End Sub

    Private Sub btnKeyboard_MouseHover(sender As System.Object, e As System.EventArgs) Handles btnKeyboard.MouseHover
        ToolTip1.IsBalloon = True
        ToolTip1.UseAnimation = True
        ToolTip1.ToolTipTitle = ""
        ToolTip1.SetToolTip(btnKeyboard, "OnScreen Keyboard")
    End Sub

    Private Sub btnKeyboard_Click(sender As System.Object, e As System.EventArgs) Handles btnKeyboard.Click
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

    Private Sub btnRecoveryPassword_Click(sender As System.Object, e As System.EventArgs) Handles btnRecoveryPassword.Click
        Me.Hide()
        frmRecoveryPassword.Show()
        frmRecoveryPassword.txtEmailID.Text = ""
        frmRecoveryPassword.txtEmailID.Focus()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim smtp1 As String = "smtp.office365.com"
        Dim smtp As New SmtpClient
        Dim mail As New MailMessage

        mail.From = New MailAddress("essaaidl333@gmail")
        mail.To.Add("essaaidl77146@gmail.com")
        mail.Body = "hgjh"


        mail.Subject = ""
        smtp.EnableSsl = True
        smtp.Port = "587"
        smtp.Host = "smtp.gmail.com"
        smtp.Credentials = New Net.NetworkCredential("essaaidl333@gmail", "zxnionhnyrcweyna")
        smtp.Send(mail)
        smtp.Dispose()

    End Sub
End Class
