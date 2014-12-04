namespace SmtPortalSetUp
{
    partial class Form_Second
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Second));
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.btnSourcePath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtMessagebox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNew = new System.Windows.Forms.TextBox();
            this.TxtOld = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelAlet = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listOld = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listNew = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(491, 25);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(348, 21);
            this.txtFilePath.TabIndex = 0;
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(845, 22);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnSelectPath.Size = new System.Drawing.Size(52, 23);
            this.btnSelectPath.TabIndex = 1;
            this.btnSelectPath.Text = "选择";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(489, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "请指定安装的路径：";
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Silver;
            this.btnNext.Location = new System.Drawing.Point(762, 315);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(135, 41);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "开始";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(14, 25);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(399, 21);
            this.txtSourcePath.TabIndex = 0;
            // 
            // btnSourcePath
            // 
            this.btnSourcePath.Location = new System.Drawing.Point(419, 23);
            this.btnSourcePath.Name = "btnSourcePath";
            this.btnSourcePath.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnSourcePath.Size = new System.Drawing.Size(52, 23);
            this.btnSourcePath.TabIndex = 1;
            this.btnSourcePath.Text = "选择";
            this.btnSourcePath.UseVisualStyleBackColor = true;
            this.btnSourcePath.Click += new System.EventHandler(this.btnSourcePath_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(237, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "请选择安装文件的路径：";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(18, 398);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(886, 16);
            this.progressBar.TabIndex = 6;
            // 
            // txtMessagebox
            // 
            this.txtMessagebox.Location = new System.Drawing.Point(16, 420);
            this.txtMessagebox.Multiline = true;
            this.txtMessagebox.Name = "txtMessagebox";
            this.txtMessagebox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessagebox.Size = new System.Drawing.Size(888, 143);
            this.txtMessagebox.TabIndex = 7;
            this.txtMessagebox.DoubleClick += new System.EventHandler(this.txtMessagebox_DoubleClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(317, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "将需要替换的域名，数据库访问连接字符串增加到替换栏中";
            // 
            // txtNew
            // 
            this.txtNew.Location = new System.Drawing.Point(167, 99);
            this.txtNew.Name = "txtNew";
            this.txtNew.Size = new System.Drawing.Size(575, 21);
            this.txtNew.TabIndex = 0;
            // 
            // TxtOld
            // 
            this.TxtOld.Location = new System.Drawing.Point(167, 68);
            this.TxtOld.Name = "TxtOld";
            this.TxtOld.Size = new System.Drawing.Size(575, 21);
            this.TxtOld.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "需要替换的内容";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "原安装文件中需替换的内容";
            // 
            // labelAlet
            // 
            this.labelAlet.ForeColor = System.Drawing.Color.Red;
            this.labelAlet.Location = new System.Drawing.Point(20, 359);
            this.labelAlet.Name = "labelAlet";
            this.labelAlet.Size = new System.Drawing.Size(884, 36);
            this.labelAlet.TabIndex = 8;
            this.labelAlet.Text = "label5";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Silver;
            this.button1.Location = new System.Drawing.Point(764, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "增加一项";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listOld
            // 
            this.listOld.FormattingEnabled = true;
            this.listOld.ItemHeight = 12;
            this.listOld.Location = new System.Drawing.Point(9, 31);
            this.listOld.Name = "listOld";
            this.listOld.Size = new System.Drawing.Size(418, 148);
            this.listOld.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "替换前值";
            // 
            // listNew
            // 
            this.listNew.FormattingEnabled = true;
            this.listNew.ItemHeight = 12;
            this.listNew.Location = new System.Drawing.Point(433, 31);
            this.listNew.Name = "listNew";
            this.listNew.Size = new System.Drawing.Size(447, 148);
            this.listNew.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(431, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "替换后值";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Silver;
            this.btnClear.Location = new System.Drawing.Point(764, 96);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(131, 30);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listNew);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.listOld);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(14, 126);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(880, 183);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "替换栏";
            // 
            // Form_Second
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(916, 575);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelAlet);
            this.Controls.Add(this.txtMessagebox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSourcePath);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.txtSourcePath);
            this.Controls.Add(this.TxtOld);
            this.Controls.Add(this.txtNew);
            this.Controls.Add(this.txtFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_Second";
            this.Load += new System.EventHandler(this.Form_Second_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Button btnSourcePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtMessagebox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNew;
        private System.Windows.Forms.TextBox TxtOld;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelAlet;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listOld;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listNew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

