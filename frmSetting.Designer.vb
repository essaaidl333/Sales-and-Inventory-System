<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetting
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnDeleteAllLogs = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnDeleteAllLogs
        '
        Me.btnDeleteAllLogs.BackColor = System.Drawing.Color.Maroon
        Me.btnDeleteAllLogs.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDeleteAllLogs.Font = New System.Drawing.Font("Tajawal", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteAllLogs.ForeColor = System.Drawing.Color.White
        Me.btnDeleteAllLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDeleteAllLogs.Location = New System.Drawing.Point(152, 67)
        Me.btnDeleteAllLogs.Name = "btnDeleteAllLogs"
        Me.btnDeleteAllLogs.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.btnDeleteAllLogs.Size = New System.Drawing.Size(164, 65)
        Me.btnDeleteAllLogs.TabIndex = 1
        Me.btnDeleteAllLogs.Text = "حذف التنشيط"
        Me.btnDeleteAllLogs.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tajawal", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(462, 24)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "تحذير :: الرجاء عدم الضغط على الزر ,,,, سيتم حذف تنشيط النسخه"
        '
        'frmSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(461, 206)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnDeleteAllLogs)
        Me.MaximizeBox = False
        Me.Name = "frmSetting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnDeleteAllLogs As Button
    Friend WithEvents Label1 As Label
End Class
