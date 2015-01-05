namespace ReCheckUI
{
    partial class ReCheckForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnReCheck = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCurMonth = new System.Windows.Forms.TextBox();
            this.btnGetCheckRd = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtMsgUdOrg = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSelectDoubleAcount = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.BtnAcountIn = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnGetActualmoney = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.btnChangeActualmoney = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.cmbAcountType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMoneyReslut = new System.Windows.Forms.TextBox();
            this.dataGridCompany = new System.Windows.Forms.DataGridView();
            this.CheckColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridSequence = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.dataGridErrAcData = new System.Windows.Forms.DataGridView();
            this.btnCompany = new System.Windows.Forms.Button();
            this.btnSelectErrData = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.btbSelect = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPerson = new System.Windows.Forms.TextBox();
            this.btnSaveLog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSequence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridErrAcData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReCheck
            // 
            this.btnReCheck.Location = new System.Drawing.Point(361, 11);
            this.btnReCheck.Name = "btnReCheck";
            this.btnReCheck.Size = new System.Drawing.Size(75, 23);
            this.btnReCheck.TabIndex = 0;
            this.btnReCheck.Text = "重新结算";
            this.btnReCheck.UseVisualStyleBackColor = true;
            this.btnReCheck.Click += new System.EventHandler(this.btnReCheck_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "预算年月";
            // 
            // txtCurMonth
            // 
            this.txtCurMonth.Location = new System.Drawing.Point(72, 12);
            this.txtCurMonth.Name = "txtCurMonth";
            this.txtCurMonth.Size = new System.Drawing.Size(100, 21);
            this.txtCurMonth.TabIndex = 2;
            // 
            // btnGetCheckRd
            // 
            this.btnGetCheckRd.Location = new System.Drawing.Point(532, 11);
            this.btnGetCheckRd.Name = "btnGetCheckRd";
            this.btnGetCheckRd.Size = new System.Drawing.Size(188, 23);
            this.btnGetCheckRd.TabIndex = 3;
            this.btnGetCheckRd.Text = "通过流水更新总账（功能可用）";
            this.btnGetCheckRd.UseVisualStyleBackColor = true;
            this.btnGetCheckRd.Click += new System.EventHandler(this.btnGetCheckRd_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(18, 52);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(822, 451);
            this.txtMsg.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "公司";
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(223, 12);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(100, 21);
            this.txtCompany.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(668, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "科目";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(718, 14);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(100, 21);
            this.txtSubject.TabIndex = 8;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 11);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(229, 23);
            this.dataGridView1.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(14, 131);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(863, 473);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtMsg);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.btnSaveLog);
            this.tabPage1.Controls.Add(this.btnReCheck);
            this.tabPage1.Controls.Add(this.btnGetCheckRd);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(855, 447);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtMsgUdOrg);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.btnSelectDoubleAcount);
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Controls.Add(this.BtnAcountIn);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(855, 447);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtMsgUdOrg
            // 
            this.txtMsgUdOrg.Location = new System.Drawing.Point(514, 55);
            this.txtMsgUdOrg.Multiline = true;
            this.txtMsgUdOrg.Name = "txtMsgUdOrg";
            this.txtMsgUdOrg.Size = new System.Drawing.Size(301, 360);
            this.txtMsgUdOrg.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(328, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(190, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "更新总账记录所属人组织架构";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSelectDoubleAcount
            // 
            this.btnSelectDoubleAcount.Location = new System.Drawing.Point(21, 15);
            this.btnSelectDoubleAcount.Name = "btnSelectDoubleAcount";
            this.btnSelectDoubleAcount.Size = new System.Drawing.Size(111, 23);
            this.btnSelectDoubleAcount.TabIndex = 10;
            this.btnSelectDoubleAcount.Text = "查询重复总账科目";
            this.btnSelectDoubleAcount.UseVisualStyleBackColor = true;
            this.btnSelectDoubleAcount.Click += new System.EventHandler(this.btnSelectDoubleAcount_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(3, 55);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(354, 467);
            this.dataGridView2.TabIndex = 0;
            // 
            // BtnAcountIn
            // 
            this.BtnAcountIn.Location = new System.Drawing.Point(183, 15);
            this.BtnAcountIn.Name = "BtnAcountIn";
            this.BtnAcountIn.Size = new System.Drawing.Size(112, 23);
            this.BtnAcountIn.TabIndex = 0;
            this.BtnAcountIn.Text = "合并重复总账科目";
            this.BtnAcountIn.UseVisualStyleBackColor = true;
            this.BtnAcountIn.Click += new System.EventHandler(this.BtnAcountIn_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnGetActualmoney);
            this.tabPage3.Controls.Add(this.dataGridView3);
            this.tabPage3.Controls.Add(this.btnChangeActualmoney);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(855, 447);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnGetActualmoney
            // 
            this.btnGetActualmoney.Location = new System.Drawing.Point(24, 25);
            this.btnGetActualmoney.Name = "btnGetActualmoney";
            this.btnGetActualmoney.Size = new System.Drawing.Size(156, 23);
            this.btnGetActualmoney.TabIndex = 13;
            this.btnGetActualmoney.Text = "查询非正常实际额度科目";
            this.btnGetActualmoney.UseVisualStyleBackColor = true;
            this.btnGetActualmoney.Click += new System.EventHandler(this.btnGetActualmoney_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(6, 65);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.Size = new System.Drawing.Size(354, 467);
            this.dataGridView3.TabIndex = 12;
            // 
            // btnChangeActualmoney
            // 
            this.btnChangeActualmoney.Location = new System.Drawing.Point(186, 25);
            this.btnChangeActualmoney.Name = "btnChangeActualmoney";
            this.btnChangeActualmoney.Size = new System.Drawing.Size(146, 23);
            this.btnChangeActualmoney.TabIndex = 11;
            this.btnChangeActualmoney.Text = "重置非正常实际额度科目";
            this.btnChangeActualmoney.UseVisualStyleBackColor = true;
            this.btnChangeActualmoney.Click += new System.EventHandler(this.btnChangeActualmoney_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.cmbAcountType);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.txtMoneyReslut);
            this.tabPage4.Controls.Add(this.dataGridCompany);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.dataGridSequence);
            this.tabPage4.Controls.Add(this.btnExport);
            this.tabPage4.Controls.Add(this.dataGridErrAcData);
            this.tabPage4.Controls.Add(this.btnCompany);
            this.tabPage4.Controls.Add(this.btnSelectErrData);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(855, 447);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // cmbAcountType
            // 
            this.cmbAcountType.FormattingEnabled = true;
            this.cmbAcountType.Items.AddRange(new object[] {
            "请选择处理的费用类型",
            "公司",
            "部门",
            "个人"});
            this.cmbAcountType.Location = new System.Drawing.Point(692, 297);
            this.cmbAcountType.Name = "cmbAcountType";
            this.cmbAcountType.Size = new System.Drawing.Size(146, 20);
            this.cmbAcountType.TabIndex = 8;
            this.cmbAcountType.SelectedIndexChanged += new System.EventHandler(this.cmbAcountType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(686, 490);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "流水剩余额度";
            // 
            // txtMoneyReslut
            // 
            this.txtMoneyReslut.Location = new System.Drawing.Point(688, 505);
            this.txtMoneyReslut.Name = "txtMoneyReslut";
            this.txtMoneyReslut.Size = new System.Drawing.Size(150, 21);
            this.txtMoneyReslut.TabIndex = 6;
            // 
            // dataGridCompany
            // 
            this.dataGridCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCompany.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheckColumn});
            this.dataGridCompany.Location = new System.Drawing.Point(6, 29);
            this.dataGridCompany.Name = "dataGridCompany";
            this.dataGridCompany.RowTemplate.Height = 23;
            this.dataGridCompany.Size = new System.Drawing.Size(385, 233);
            this.dataGridCompany.TabIndex = 5;
            // 
            // CheckColumn
            // 
            this.CheckColumn.HeaderText = "Column1";
            this.CheckColumn.Name = "CheckColumn";
            this.CheckColumn.Width = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 273);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "部门负数科目流水帐列表";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(675, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "总账为负数的科目列表";
            // 
            // dataGridSequence
            // 
            this.dataGridSequence.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSequence.Location = new System.Drawing.Point(14, 297);
            this.dataGridSequence.Name = "dataGridSequence";
            this.dataGridSequence.RowTemplate.Height = 23;
            this.dataGridSequence.Size = new System.Drawing.Size(661, 229);
            this.dataGridSequence.TabIndex = 3;
            this.dataGridSequence.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridSequence_CellClick);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(691, 399);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(147, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "导出问题科目流水";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dataGridErrAcData
            // 
            this.dataGridErrAcData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridErrAcData.Location = new System.Drawing.Point(434, 29);
            this.dataGridErrAcData.Name = "dataGridErrAcData";
            this.dataGridErrAcData.RowTemplate.Height = 23;
            this.dataGridErrAcData.Size = new System.Drawing.Size(406, 233);
            this.dataGridErrAcData.TabIndex = 1;
            this.dataGridErrAcData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridErrAcData_CellClick);
            this.dataGridErrAcData.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridErrAcData_RowEnter);
            // 
            // btnCompany
            // 
            this.btnCompany.Location = new System.Drawing.Point(691, 341);
            this.btnCompany.Name = "btnCompany";
            this.btnCompany.Size = new System.Drawing.Size(149, 23);
            this.btnCompany.TabIndex = 0;
            this.btnCompany.Text = "查询公司";
            this.btnCompany.UseVisualStyleBackColor = true;
            this.btnCompany.Click += new System.EventHandler(this.btnCompany_Click);
            // 
            // btnSelectErrData
            // 
            this.btnSelectErrData.Location = new System.Drawing.Point(691, 370);
            this.btnSelectErrData.Name = "btnSelectErrData";
            this.btnSelectErrData.Size = new System.Drawing.Size(149, 23);
            this.btnSelectErrData.TabIndex = 0;
            this.btnSelectErrData.Text = "查出可用额度为负的科目";
            this.btnSelectErrData.UseVisualStyleBackColor = true;
            this.btnSelectErrData.Click += new System.EventHandler(this.btnSelectErrData_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(358, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "部门";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(394, 12);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(100, 21);
            this.txtDepartment.TabIndex = 6;
            // 
            // btbSelect
            // 
            this.btbSelect.Location = new System.Drawing.Point(718, 41);
            this.btbSelect.Name = "btbSelect";
            this.btbSelect.Size = new System.Drawing.Size(100, 23);
            this.btbSelect.TabIndex = 11;
            this.btbSelect.Text = "查询";
            this.btbSelect.UseVisualStyleBackColor = true;
            this.btbSelect.Click += new System.EventHandler(this.btbSelect_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(500, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "员工";
            // 
            // txtPerson
            // 
            this.txtPerson.Location = new System.Drawing.Point(550, 14);
            this.txtPerson.Name = "txtPerson";
            this.txtPerson.Size = new System.Drawing.Size(100, 21);
            this.txtPerson.TabIndex = 8;
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.Location = new System.Drawing.Point(765, 11);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(75, 23);
            this.btnSaveLog.TabIndex = 0;
            this.btnSaveLog.Text = "保存日志";
            this.btnSaveLog.UseVisualStyleBackColor = true;
            this.btnSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // ReCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 616);
            this.Controls.Add(this.btbSelect);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtPerson);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCompany);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCurMonth);
            this.Controls.Add(this.label1);
            this.Name = "ReCheckForm";
            this.Text = "Main";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSequence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridErrAcData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurMonth;
        private System.Windows.Forms.Button btnGetCheckRd;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnSelectDoubleAcount;
        private System.Windows.Forms.Button BtnAcountIn;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnGetActualmoney;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button btnChangeActualmoney;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dataGridErrAcData;
        private System.Windows.Forms.Button btnSelectErrData;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridSequence;
        private System.Windows.Forms.DataGridView dataGridCompany;
        private System.Windows.Forms.Button btnCompany;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckColumn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMoneyReslut;
        private System.Windows.Forms.ComboBox cmbAcountType;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtMsgUdOrg;
        private System.Windows.Forms.Button btbSelect;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPerson;
        private System.Windows.Forms.Button btnSaveLog;
    }
}

