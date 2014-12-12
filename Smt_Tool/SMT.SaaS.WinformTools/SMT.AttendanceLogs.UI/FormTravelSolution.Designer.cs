namespace SMT.AttendanceLogs.UI
{
    partial class FormTravelSolution
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label6 = new System.Windows.Forms.Label();
            this.txtsourceSolution = new System.Windows.Forms.TextBox();
            this.BtnCheck1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtSolution = new System.Windows.Forms.TextBox();
            this.BtnCheck2 = new System.Windows.Forms.Button();
            this.BtnStart = new System.Windows.Forms.Button();
            this.labelFromSolutionId = new System.Windows.Forms.Label();
            this.labelToSolutionID = new System.Windows.Forms.Label();
            this.txtSolutionAll = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "复制的源出差方案名";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtsourceSolution
            // 
            this.txtsourceSolution.Location = new System.Drawing.Point(140, 143);
            this.txtsourceSolution.Name = "txtsourceSolution";
            this.txtsourceSolution.Size = new System.Drawing.Size(290, 21);
            this.txtsourceSolution.TabIndex = 2;
            // 
            // BtnCheck1
            // 
            this.BtnCheck1.Location = new System.Drawing.Point(493, 141);
            this.BtnCheck1.Name = "BtnCheck1";
            this.BtnCheck1.Size = new System.Drawing.Size(75, 23);
            this.BtnCheck1.TabIndex = 4;
            this.BtnCheck1.Text = "检查";
            this.BtnCheck1.UseVisualStyleBackColor = true;
            this.BtnCheck1.Click += new System.EventHandler(this.BtnCheck1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 203);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "复制到出差方案名";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TxtSolution
            // 
            this.TxtSolution.Location = new System.Drawing.Point(140, 194);
            this.TxtSolution.Name = "TxtSolution";
            this.TxtSolution.Size = new System.Drawing.Size(290, 21);
            this.TxtSolution.TabIndex = 2;
            // 
            // BtnCheck2
            // 
            this.BtnCheck2.Location = new System.Drawing.Point(493, 194);
            this.BtnCheck2.Name = "BtnCheck2";
            this.BtnCheck2.Size = new System.Drawing.Size(75, 23);
            this.BtnCheck2.TabIndex = 4;
            this.BtnCheck2.Text = "检查";
            this.BtnCheck2.UseVisualStyleBackColor = true;
            this.BtnCheck2.Click += new System.EventHandler(this.BtnCheck2_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(493, 244);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(209, 41);
            this.BtnStart.TabIndex = 4;
            this.BtnStart.Text = "开始同步出差城市分类和补贴";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // labelFromSolutionId
            // 
            this.labelFromSolutionId.AutoSize = true;
            this.labelFromSolutionId.Location = new System.Drawing.Point(606, 146);
            this.labelFromSolutionId.Name = "labelFromSolutionId";
            this.labelFromSolutionId.Size = new System.Drawing.Size(41, 12);
            this.labelFromSolutionId.TabIndex = 5;
            this.labelFromSolutionId.Text = "FromId";
            // 
            // labelToSolutionID
            // 
            this.labelToSolutionID.AutoSize = true;
            this.labelToSolutionID.Location = new System.Drawing.Point(606, 197);
            this.labelToSolutionID.Name = "labelToSolutionID";
            this.labelToSolutionID.Size = new System.Drawing.Size(29, 12);
            this.labelToSolutionID.TabIndex = 5;
            this.labelToSolutionID.Text = "ToId";
            // 
            // txtSolutionAll
            // 
            this.txtSolutionAll.Location = new System.Drawing.Point(6, 27);
            this.txtSolutionAll.Multiline = true;
            this.txtSolutionAll.Name = "txtSolutionAll";
            this.txtSolutionAll.Size = new System.Drawing.Size(496, 94);
            this.txtSolutionAll.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(548, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 43);
            this.button1.TabIndex = 4;
            this.button1.Text = "查询所有出差方案名";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Location = new System.Drawing.Point(119, 406);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(516, 55);
            this.btnCopyAll.TabIndex = 4;
            this.btnCopyAll.Text = "根据源出差方案同步其他公司出差城市分类和补贴";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSolutionAll);
            this.groupBox1.Controls.Add(this.labelToSolutionID);
            this.groupBox1.Controls.Add(this.txtsourceSolution);
            this.groupBox1.Controls.Add(this.labelFromSolutionId);
            this.groupBox1.Controls.Add(this.TxtSolution);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.BtnStart);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.BtnCheck2);
            this.groupBox1.Controls.Add(this.BtnCheck1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(802, 307);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "一对一复制城市分类和补贴";
            // 
            // FormTravelSolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 509);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCopyAll);
            this.Name = "FormTravelSolution";
            this.Text = "FormTravelSolution";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtsourceSolution;
        private System.Windows.Forms.Button BtnCheck1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtSolution;
        private System.Windows.Forms.Button BtnCheck2;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Label labelFromSolutionId;
        private System.Windows.Forms.Label labelToSolutionID;
        private System.Windows.Forms.TextBox txtSolutionAll;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCopyAll;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}