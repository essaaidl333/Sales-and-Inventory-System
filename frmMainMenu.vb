Imports System.Data.SqlClient
Imports System.IO

Imports Microsoft.SqlServer.Management.Smo
Imports System.Globalization

Public Class frmMainMenu
    Dim Filename As String
    Dim i1, i2 As Integer

    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.ShowDialog()
        ' frmSalesLocations.ShowDialog()

    End Sub
    Sub Backup()
        Try
            Dim dt As DateTime = Today
            Dim destdir As String = "INV_DB " & System.DateTime.Now.ToString("dd-MM-yyyy_h-mm-ss") & ".bak"
            Dim objdlg As New SaveFileDialog
            objdlg.FileName = destdir
            objdlg.ShowDialog()
            Filename = objdlg.FileName
            Cursor = Cursors.WaitCursor
            Timer2.Enabled = True
            con = New SqlConnection(cs)
            con.Open()
            Dim cb As String = "backup database INV_DB to disk='" & Filename & "'with init,stats=10"
            cmd = New SqlCommand(cb)
            cmd.Connection = con
            cmd.ExecuteReader()
            con.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub BackupToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        Cursor = Cursors.Default
        Timer2.Enabled = False
    End Sub

    Private Sub RestoreToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub RegistrationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub LogsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LogsToolStripMenuItem.Click
        frmLogs.Reset()
        frmLogs.lblUser.Text = lblUser.Text
        frmLogs.ShowDialog()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Dim dt As DateTime = Today
        lblDateTime.Text = dt.ToString("dd/MM/yyyy")
        lblTime.Text = TimeOfDay.ToString("h:mm:ss tt")
    End Sub

    Private Sub CalculatorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CalculatorToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("Calc.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub NotepadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NotepadToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("Notepad.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub WordpadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles WordpadToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("wordpad.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MSWordToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MSWordToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("winword.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub TaskManagerToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TaskManagerToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start("TaskMgr.exe")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SystemInfoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SystemInfoToolStripMenuItem.Click
        frmSystemInfo.ShowDialog()
    End Sub
    Sub LogOut()
        frmPurchaseEntry.Hide()
        frmProduct.Hide()
        Dim st As String = "Successfully logged out"
        LogFunc(lblUser.Text, st)
        Me.Hide()
        frmLogin.Show()
        frmLogin.UserID.Text = ""
        frmLogin.Password.Text = ""
        frmLogin.UserID.Focus()
    End Sub
    Private Sub LogoutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LogoutToolStripMenuItem.Click
        Try
            If MessageBox.Show("هل تريد تبديل المستخدم الحالي?", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                If MessageBox.Show("هل تريد عمل نسخة احتياطية من البرنامج قبل الخروج?", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Backup()
                    LogOut()
                Else
                    LogOut()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmMainMenu_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'e.Cancel = True
        Application.Exit()
        End

    End Sub

    Private Sub CompanyInfoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CompanyInfoToolStripMenuItem.Click
        frmCompany.lblUser.Text = lblUser.Text
        frmCompany.Reset()
        frmCompany.ShowDialog()
    End Sub

    Private Sub CustomerToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CustomerToolStripMenuItem.Click
        frmCustomer.lblUser.Text = lblUser.Text
        frmCustomer.Reset()
        frmCustomer.ShowDialog()
    End Sub

    Private Sub CategoryToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub SubCategoryToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SupplierToolStripMenuItem.Click
        frmSupplier.lblUser.Text = lblUser.Text
        frmSupplier.Reset()
        frmSupplier.ShowDialog()
    End Sub

    Private Sub CustomerToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles CustomerToolStripMenuItem1.Click
        frmCustomerRecord1.Reset()
        frmCustomerRecord1.ShowDialog()
    End Sub

    Private Sub SupplierToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles SupplierToolStripMenuItem1.Click
        frmSupplierRecord.Reset()
        frmSupplierRecord.ShowDialog()
    End Sub

    Private Function HandleRegistry() As Boolean
        Dim firstRunDate As Date
        Dim st As Date = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\InventorySoft2", "Set", Nothing)
        firstRunDate = st
        If firstRunDate = Nothing Then
            firstRunDate = System.DateTime.Today.Date
            My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\InventorySoft2", "Set", firstRunDate)
        ElseIf (Now - firstRunDate).Days > 7 Then
            Return False
        End If
        Return True
    End Function
    Private Sub frmMainMenu_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Dim result As Boolean = HandleRegistry()
        'If result = False Then 'something went wrong
        'MessageBox.Show("Trial expired" & vbCrLf & "for purchasing the full version of software call us at +919827858191", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End
        'End If
        MessageBox.Show(TextBox1.Text)
        If lblUserType.Text = "Admin" Then
            MasterEntryToolStripMenuItem.Enabled = True
            المستخدمينToolStripMenuItem.Enabled = True
            LogsToolStripMenuItem.Enabled = True
            استرجاعنسخةاحتياطيةToolStripMenuItem.Enabled = True
            CustomerToolStripMenuItem.Enabled = True
            SupplierToolStripMenuItem.Enabled = True
            ProductToolStripMenuItem.Enabled = True
            StockToolStripMenuItem.Enabled = True
            ServiceToolStripMenuItem.Enabled = True
            جردالمخزونToolStripMenuItem.Enabled = True
            BillingToolStripMenuItem.Enabled = True
            QuotationToolStripMenuItem.Enabled = True
            RecordToolStripMenuItem.Enabled = True
            ReportsToolStripMenuItem.Enabled = True
            VoucherToolStripMenuItem.Enabled = True
            SalesmanToolStripMenuItem.Enabled = True
            SendSMSToolStripMenuItem.Enabled = True
            SalesReturnToolStripMenuItem.Enabled = True
            PaymentToolStripMenuItem.Enabled = True
            ToolStripMenuItemSetting.Enabled = True
            ToolStripMenuItem2.Enabled = True
        End If
        If lblUserType.Text = "Sales Person" Then
            MasterEntryToolStripMenuItem.Enabled = False
            المستخدمينToolStripMenuItem.Enabled = False
            LogsToolStripMenuItem.Enabled = False
            استرجاعنسخةاحتياطيةToolStripMenuItem.Enabled = False
            CustomerToolStripMenuItem.Enabled = True
            SupplierToolStripMenuItem.Enabled = False
            ProductToolStripMenuItem.Enabled = False
            StockToolStripMenuItem.Enabled = False
            ServiceToolStripMenuItem.Enabled = True
            جردالمخزونToolStripMenuItem.Enabled = True
            BillingToolStripMenuItem.Enabled = True
            QuotationToolStripMenuItem.Enabled = True
            RecordToolStripMenuItem.Enabled = False
            ReportsToolStripMenuItem.Enabled = False
            VoucherToolStripMenuItem.Enabled = False
            SalesmanToolStripMenuItem.Enabled = False
            SendSMSToolStripMenuItem.Enabled = False
            SalesReturnToolStripMenuItem.Enabled = True
            PaymentToolStripMenuItem.Enabled = False
            ToolStripMenuItemSetting.Enabled = False
        End If
        If lblUserType.Text = "Inventory Manager" Then
            MasterEntryToolStripMenuItem.Enabled = False
            المستخدمينToolStripMenuItem.Enabled = False
            LogsToolStripMenuItem.Enabled = False
            استرجاعنسخةاحتياطيةToolStripMenuItem.Enabled = False
            CustomerToolStripMenuItem.Enabled = False
            SupplierToolStripMenuItem.Enabled = False
            ProductToolStripMenuItem.Enabled = True
            StockToolStripMenuItem.Enabled = True
            ServiceToolStripMenuItem.Enabled = False
            جردالمخزونToolStripMenuItem.Enabled = True
            BillingToolStripMenuItem.Enabled = False
            QuotationToolStripMenuItem.Enabled = False
            RecordToolStripMenuItem.Enabled = False
            ReportsToolStripMenuItem.Enabled = False
            VoucherToolStripMenuItem.Enabled = False
            SalesmanToolStripMenuItem.Enabled = False
            SendSMSToolStripMenuItem.Enabled = False
            PaymentToolStripMenuItem.Enabled = False
            ToolStripMenuItemSetting.Enabled = False
        End If
        '###############33
        If lblUserType.Text = "accountant" Then
            MasterEntryToolStripMenuItem.Enabled = False
            المستخدمينToolStripMenuItem.Enabled = False
            LogsToolStripMenuItem.Enabled = False
            استرجاعنسخةاحتياطيةToolStripMenuItem.Enabled = False
            CustomerToolStripMenuItem.Enabled = False
            SupplierToolStripMenuItem.Enabled = False
            ProductToolStripMenuItem.Enabled = False
            StockToolStripMenuItem.Enabled = False
            ServiceToolStripMenuItem.Enabled = True
            جردالمخزونToolStripMenuItem.Enabled = True
            BillingToolStripMenuItem.Enabled = False
            QuotationToolStripMenuItem.Enabled = True
            RecordToolStripMenuItem.Enabled = True
            ReportsToolStripMenuItem.Enabled = True
            VoucherToolStripMenuItem.Enabled = False
            SalesmanToolStripMenuItem.Enabled = False
            SendSMSToolStripMenuItem.Enabled = False
            PaymentToolStripMenuItem.Enabled = True
            ToolStripMenuItemSetting.Enabled = False
        End If

        Me.DockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingMdi   '--->Requires:  "Me.IsMdiContainer = True"
        Me.IsMdiContainer = True    'Must be defined at design-time or at run-time.

        frmStockBalance.Reset()
        'frmStockBalance.lblUser.Text = lblUser.Text
        frmStockBalance.Show(DockPanel1)
        frmStockBalance.MdiParent = Me
        frmStockBalance.Focus()


    End Sub

    Private Sub StockToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles StockToolStripMenuItem1.Click
        frmPurchaseRecord.Reset()
        frmPurchaseRecord.ShowDialog()
    End Sub

    Private Sub StockInToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub btnExportExcel_Click(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub txtProductName_TextChanged(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs)
        Reset()
    End Sub

    Private Sub ContactsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ContactsToolStripMenuItem.Click
        frmContacts.lblUser.Text = lblUser.Text
        frmContacts.Reset()
        frmContacts.ShowDialog()
    End Sub

    Private Sub IndividualToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs)
        frmProductRecord.Reset()
        frmProductRecord.ShowDialog()
    End Sub

    Private Sub ProductToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProductToolStripMenuItem.Click
    End Sub

    Private Sub ProductToolStripMenuItem2_Click(sender As System.Object, e As System.EventArgs) Handles ProductToolStripMenuItem2.Click
        frmProductRecord.Reset()
        frmProductRecord.ShowDialog()
    End Sub

    Private Sub ServiceToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles ServiceToolStripMenuItem1.Click
        frmServicesRecord.Reset()
        frmServicesRecord.ShowDialog()
    End Sub

    Private Sub QuotationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles QuotationToolStripMenuItem.Click
        frmQuotation.lblUser.Text = lblUser.Text
        frmQuotation.lblUserType.Text = lblUserType.Text
        frmQuotation.Reset()
        frmQuotation.ShowDialog()
    End Sub

    Private Sub QuotationToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles QuotationToolStripMenuItem1.Click
        frmQuotationRecord1.Reset()
        frmQuotationRecord1.ShowDialog()
    End Sub

    Private Sub ProductsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles POSToolStripMenuItem.Click
        'frmPOS.lblUser.Text = lblUser.Text
        'frmPOS.lblUserType.Text = lblUserType.Text
        'frmPOS.Reset()
        'frmPOS.ShowDialog()


        frmPOS.lblUser.Text = lblUser.Text
        frmPOS.TextBox2.Text = TextBox1.Text



        frmPOS.lblUserType.Text = lblUserType.Text
        frmPOS.Show(DockPanel1)
        frmPOS.Reset()
        frmPOS.MdiParent = Me
        frmPOS.Focus()

    End Sub

    Private Sub BillingProductsServiceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BillingProductsServiceToolStripMenuItem.Click
        frmServiceBillingRecord.Reset()
        frmServiceBillingRecord.ShowDialog()
    End Sub

    Private Sub SMSSettingToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SMSSettingToolStripMenuItem.Click
        frmSMSSetting.Reset()
        frmSMSSetting.ShowDialog()
    End Sub

    Private Sub SalesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SalesToolStripMenuItem.Click
        frmSalesReport.Reset()
        frmSalesReport.ShowDialog()
    End Sub

    Private Sub ServiceBillingToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ServiceBillingToolStripMenuItem.Click
        frmServiceDoneReport.Reset()
        frmServiceDoneReport.ShowDialog()
    End Sub

    Private Sub StockInAndStockOutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles StockInAndStockOutToolStripMenuItem.Click
        frmStockInAndOutReport.ShowDialog()
    End Sub

    Private Sub PurchaseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PurchaseToolStripMenuItem.Click
        frmPurchaseReport.Reset()
        frmPurchaseReport.ShowDialog()
    End Sub

    Private Sub ProfitAndLossToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProfitAndLossToolStripMenuItem.Click
        frmProfitAndLossReport.Reset()
        frmProfitAndLossReport.ShowDialog()
    End Sub

    Private Sub VoucherToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles VoucherToolStripMenuItem.Click
        frmVoucher.Reset()
        frmVoucher.lblUser.Text = lblUser.Text
        frmVoucher.ShowDialog()
    End Sub

    Private Sub ExpenditureToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExpenditureToolStripMenuItem.Click
        frmVoucherReport.Reset()
        frmVoucherReport.ShowDialog()
    End Sub

    Private Sub CreditorsAndDebtorsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CreditorsAndDebtorsToolStripMenuItem.Click
        frmDebtorsReport.ShowDialog()
    End Sub

    Private Sub SQLServerSettingToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SQLServerSettingToolStripMenuItem.Click
        frmSqlServerSetting.Reset()
        frmSqlServerSetting.lblSet.Text = "Main Form"
        frmSqlServerSetting.ShowDialog()
    End Sub

    Private Sub PurchaseDaybookToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PurchaseDaybookToolStripMenuItem.Click
        frmPurchaseDaybook.Reset()
        frmPurchaseDaybook.ShowDialog()
    End Sub

    Private Sub GeneralLedgerToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GeneralLedgerToolStripMenuItem.Click
        frmGeneralLedger.Reset()
        frmGeneralLedger.ShowDialog()
    End Sub

    Private Sub GeneralDaybookToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GeneralDaybookToolStripMenuItem.Click
        frmGeneralDayBook.Reset()
        frmGeneralDayBook.ShowDialog()
    End Sub

    Private Sub PaymentToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PaymentToolStripMenuItem.Click
        frmPayment.lblUser.Text = lblUser.Text
        frmPayment.Reset()
        frmPayment.ShowDialog()
    End Sub

    Private Sub PaymentsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PaymentsToolStripMenuItem.Click
        frmPaymentRecord.Reset()
        frmPaymentRecord.ShowDialog()
    End Sub

    Private Sub TrialBalanceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TrialBalanceToolStripMenuItem.Click
        frmTrialBalance.Reset()
        frmTrialBalance.ShowDialog()
    End Sub

    Private Sub SupplierLedgerToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SupplierLedgerToolStripMenuItem.Click
        frmSupplierLedger.Reset()
        frmSupplierLedger.ShowDialog()
    End Sub

    Private Sub CustomerLedgerToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CustomerLedgerToolStripMenuItem.Click
        frmCustomerLedger.Reset()
        frmCustomerLedger.ShowDialog()
    End Sub

    Private Sub SMSToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SMSToolStripMenuItem.Click
        frmSMS.Reset()
        frmSMS.lblUser.Text = lblUser.Text
        frmSMS.ShowDialog()
    End Sub


    Private Sub SalesmanToolStripMenuItem3_Click(sender As System.Object, e As System.EventArgs)
        frmSalesman.Reset()
        frmSalesman.lblUser.Text = lblUser.Text
        frmSalesman.ShowDialog()
    End Sub

    Private Sub SalesmanLedgerToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SalesmanLedgerToolStripMenuItem.Click
        frmSalesmanLedger.Reset()
        frmSalesmanLedger.ShowDialog()
    End Sub

    Private Sub SalesmanCommissionToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SalesmanCommissionToolStripMenuItem.Click
        frmSalesmanCommmissionReport.Reset()
        frmSalesmanCommmissionReport.ShowDialog()
    End Sub

    Private Sub TaxToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TaxToolStripMenuItem.Click
        frmTaxReport.Reset()
        frmTaxReport.ShowDialog()
    End Sub

    Private Sub SendSMSToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SendSMSToolStripMenuItem.Click
        frmSendSMS.lblUser.Text = lblUser.Text
        frmSendSMS.Reset()
        frmSendSMS.ShowDialog()
    End Sub

    Private Sub CreditTermsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CreditTermsToolStripMenuItem.Click
        frmCreditTermsReport.Reset()
        frmCreditTermsReport.ShowDialog()
    End Sub

    Private Sub CreditTermsStatementsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CreditTermsStatementsToolStripMenuItem.Click
        frmCreditTermsStatementsReport.Reset()
        frmCreditTermsStatementsReport.ShowDialog()
    End Sub

    Private Sub SalesmanToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SalesmanToolStripMenuItem.Click
        frmSalesman.Reset()
        frmSalesman.lblUser.Text = lblUser.Text
        frmSalesman.ShowDialog()
    End Sub

    Private Sub SalesmanToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles SalesmanToolStripMenuItem1.Click
        frmSalesmanRecord.Reset()
        frmSalesmanRecord.lblSet.Text = ""
        frmSalesmanRecord.ShowDialog()
    End Sub

    Private Sub BarcodeLabelPrintingToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BarcodeLabelPrintingToolStripMenuItem.Click
        frmBarcodeLabelPrinting.Reset()
        frmBarcodeLabelPrinting.ShowDialog()
    End Sub

    Private Sub PurchaseEntryToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PurchaseEntryToolStripMenuItem.Click
        frmPurchaseEntry.lblUser.Text = lblUser.Text
        frmPurchaseEntry.lblUserType.Text = lblUserType.Text
        frmPurchaseEntry.Reset()
        frmPurchaseEntry.ShowDialog()
    End Sub

    Private Sub PurchaseReturnToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PurchaseReturnToolStripMenuItem.Click
        frmPurchaseReturn.lblUser.Text = lblUser.Text
        frmPurchaseReturn.lblUserType.Text = lblUserType.Text
        frmPurchaseReturn.Reset()
        frmPurchaseReturn.ShowDialog()
    End Sub

    Private Sub EmailSettingToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EmailSettingToolStripMenuItem.Click
        frmEmailSetting.Reset()
        frmEmailSetting.ShowDialog()
    End Sub

    Private Sub ServiceCreationToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ServiceCreationToolStripMenuItem.Click
        frmServices.lblUser.Text = lblUser.Text
        frmServices.lblUserType.Text = lblUserType.Text
        frmServices.Reset()
        frmServices.ShowDialog()
    End Sub

    Private Sub ServiceBillingToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles ServiceBillingToolStripMenuItem1.Click
        frmServiceBilling.lblUser.Text = lblUser.Text
        frmServiceBilling.lblUserType.Text = lblUserType.Text
        frmServiceBilling.Reset()
        frmServiceBilling.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Try
            If MessageBox.Show("هل أنت متأكد أنك تريد إغلاق البرنامج?", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Application.Exit()
                End
            Else
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        frmPayment_2.lblUser.Text = lblUser.Text
        frmPayment_2.Reset()
        frmPayment_2.ShowDialog()
    End Sub

    Private Sub اشعــــارخصــمدائـــنToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles اشعــــارخصــمدائـــنToolStripMenuItem.Click
        frmPayment_3.lblUser.Text = lblUser.Text
        frmPayment_3.Reset()
        frmPayment_3.ShowDialog()

    End Sub

    Private Sub ترميزمراكزالبيعToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs)

    End Sub

    Private Sub المساعدةToolStripMenuItem_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub ToolStripMenuItemSetting_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSetting.Click
        frmSetting.Show()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
    End Sub

    Private Sub الارباحToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles الارباحToolStripMenuItem.Click
        frmOverallReport.Reset()
        frmOverallReport.ShowDialog()
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
    End Sub

    Private Sub الارباحوالخسائرToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles الارباحوالخسائرToolStripMenuItem.Click
        'frmProfitAndLossReport.Reset()
        frmProfitAndLossReport.ShowDialog()
    End Sub


    Private Sub SalesReturnToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SalesReturnToolStripMenuItem.Click
        frmSalesReturn.lblUser.Text = lblUser.Text
        frmSalesReturn.lblUserType.Text = lblUserType.Text
        frmSalesReturn.Reset()
        frmSalesReturn.ShowDialog()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub ترميزالفئاتالرئيسيةToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ترميزالفئاتالرئيسيةToolStripMenuItem.Click
        frmCategory.lblUser.Text = lblUser.Text
        frmCategory.Reset()
        frmCategory.ShowDialog()

    End Sub

    Private Sub ترميزالفئاتالفرعيةToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ترميزالفئاتالفرعيةToolStripMenuItem.Click
        frmSubCategory.lblUser.Text = lblUser.Text
        frmSubCategory.Reset()
        frmSubCategory.ShowDialog()

    End Sub

    Private Sub ترميزمراكزالبيعToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ترميزمراكزالبيعToolStripMenuItem1.Click
        frmContacts_2.lblUser.Text = lblUser.Text
        frmContacts_2.Reset()
        frmContacts_2.ShowDialog()

    End Sub

    Private Sub نسخاحتياطيToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles نسخاحتياطيToolStripMenuItem.Click
        Backup()

    End Sub

    Private Sub استرجاعنسخةاحتياطيةToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles استرجاعنسخةاحتياطيةToolStripMenuItem.Click
        Try
            With OpenFileDialog1
                .Filter = ("DB Backup File|*.bak;")
                .FilterIndex = 4
            End With
            'Clear the file name
            OpenFileDialog1.FileName = ""

            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                Cursor = Cursors.WaitCursor
                Timer2.Enabled = True
                SqlConnection.ClearAllPools()
                con = New SqlConnection(cs)
                con.Open()
                Dim cb As String = "USE Master ALTER DATABASE INV_DB SET Single_User WITH Rollback Immediate Restore database INV_DB FROM disk='" & OpenFileDialog1.FileName & "' WITH REPLACE ALTER DATABASE INV_DB SET Multi_User "
                cmd = New SqlCommand(cb)
                cmd.Connection = con
                cmd.ExecuteReader()
                con.Close()
                Dim st As String = "Sucessfully performed the restore"
                LogFunc(lblUser.Text, st)
                MessageBox.Show("تمت بنجاح ", "استعادة قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub المستخدمينToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles المستخدمينToolStripMenuItem.Click
        frmRegistration.lblUser.Text = lblUser.Text
        frmRegistration.Reset()
        frmRegistration.ShowDialog()

    End Sub

    Private Sub اشعـــارخصـــممديــنToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles اشعـــارخصـــممديــنToolStripMenuItem.Click

    End Sub

    Private Sub جردالمخزونToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles جردالمخزونToolStripMenuItem.Click
        frmCurrentStock.Reset()
        frmCurrentStock.ShowDialog()

    End Sub

    Private Sub شاشةالأصنافToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles شاشةالأصنافToolStripMenuItem.Click
        'frmProduct.lblUser.Text = lblUser.Text
        'frmProduct.lblUserType.Text = lblUserType.Text
        'frmProduct.Reset()
        'frmProduct.ShowDialog()

        frmProduct.lblUser.Text = lblUser.Text
        frmProduct.lblUserType.Text = lblUserType.Text
        frmProduct.Show(DockPanel1)
        frmProduct.Reset()
        frmProduct.MdiParent = Me
        frmProduct.Focus()

    End Sub

    Private Sub استيرادتصديرالأصناففيالأكسلToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles استيرادتصديرالأصناففيالأكسلToolStripMenuItem.Click
        'frmExportImportExcel_ProductsRecord.Reset()
        'frmExportImportExcel_ProductsRecord.ShowDialog()

        frmExportImportExcel_ProductsRecord.Show(DockPanel1)
        frmExportImportExcel_ProductsRecord.Reset()
        frmExportImportExcel_ProductsRecord.MdiParent = Me
        frmExportImportExcel_ProductsRecord.Focus()

    End Sub

    Private Sub StockToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StockToolStripMenuItem.Click

    End Sub

    Private Sub frmMainMenu_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Q Then
            'شاشة المبيعات
            frmPOS.lblUser.Text = lblUser.Text
            frmPOS.lblUserType.Text = lblUserType.Text
            frmPOS.Reset()
            frmPOS.ShowDialog()
        End If
    End Sub
End Class