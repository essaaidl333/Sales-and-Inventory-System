Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Public Class frmActivation

    Private Sub frmActivation_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            Dim i As System.Management.ManagementObject
            Dim searchInfo_Processor As New System.Management.ManagementObjectSearcher("Select * from Win32_Processor")
            For Each i In searchInfo_Processor.Get()
                txtHardwareID.Text = i("ProcessorID").ToString
            Next
            Dim searchInfo_Board As New System.Management.ManagementObjectSearcher("Select * from Win32_BaseBoard")
            For Each i In searchInfo_Board.Get()
                txtSerialNo.Text = i("SerialNumber").ToString
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error!")
            End
        End Try
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Try
            If txtActivationID.Text = "" Then
                MessageBox.Show("Please enter activation id", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtActivationID.Focus()
                Exit Sub
            End If
            Dim st As String = (txtHardwareID.Text) + (txtSerialNo.Text)
            TextBox1.Text = Encryption.MakePassword(st, 659)
            If txtActivationID.Text = TextBox1.Text Then
                con = New SqlConnection(cs)
                con.Open()
                Dim cb1 As String = "insert into Activation(HardwareID,SerialNo,ActivationID,ActivationTime) VALUES (@d1,@d2,@d3,@d4)"
                cmd = New SqlCommand(cb1)
                cmd.Connection = con
                cmd.Parameters.AddWithValue("@d1", Encrypt(txtHardwareID.Text.Trim))
                cmd.Parameters.AddWithValue("@d2", Encrypt(txtSerialNo.Text.Trim))
                cmd.Parameters.AddWithValue("@d3", Encrypt(txtActivationID.Text.Trim()))
                cmd.Parameters.AddWithValue("@d4", DateTimePicker1.Text.Trim())
                cmd.ExecuteReader()
                con.Close()
                MessageBox.Show("Successfully activated", "Software", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'frmLogin.Show()
                Me.Hide()
                End
            Else
                MessageBox.Show("Invalid activation id...Please contact software provider for buying full licence" & vbCrLf & "Contact us at :" & vbCrLf & "Ahmad Hamdan" & vbCrLf & "Email-ahmedhemdan366@gmail.com" & vbCrLf & "Mobile No. 0096550440071", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ' Me.Close()
        End
    End Sub


    Public Sub DeleteAct()
        Try
            Dim Act As String = 0
            con = New SqlConnection(cs)
            con.Open()
            Dim cl As String = "delete from Activation"
            cmd = New SqlCommand(cl)
            cmd.Connection = con
            ' cmd.Parameters.AddWithValue(txtActivationID.Text)
            rdr = cmd.ExecuteReader()
            MessageBox.Show("عذراً : تم أنتهاء صلاحية هذه النسخة ! لتجديد الاشتراك تواصل مباشرة مع المهندس / ساجي بن سعيد على الرقم التالي 0597849490")
            Exit Sub
            If con.State = ConnectionState.Open Then
                con.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub


End Class
