namespace SMT.AttendanceLogs.UI
{
    partial class TravelEditForm
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSerch = new System.Windows.Forms.Button();
            this.dtBussnissTrip = new System.Windows.Forms.DataGridView();
            this.dtTravel = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.txtCheckState = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labMsg = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtBussnissTrip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTravel)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(71, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 0;
            this.txtName.Text = "王力";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "姓名";
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(402, 13);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(200, 21);
            this.dtFrom.TabIndex = 2;
            this.dtFrom.Value = new System.DateTime(2014, 1, 1, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "出发日期";
            // 
            // btnSerch
            // 
            this.btnSerch.Location = new System.Drawing.Point(418, 59);
            this.btnSerch.Name = "btnSerch";
            this.btnSerch.Size = new System.Drawing.Size(184, 34);
            this.btnSerch.TabIndex = 4;
            this.btnSerch.Text = "查询";
            this.btnSerch.UseVisualStyleBackColor = true;
            this.btnSerch.Click += new System.EventHandler(this.btnSerch_Click);
            // 
            // dtBussnissTrip
            // 
            this.dtBussnissTrip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtBussnissTrip.Location = new System.Drawing.Point(14, 99);
            this.dtBussnissTrip.Name = "dtBussnissTrip";
            this.dtBussnissTrip.RowTemplate.Height = 23;
            this.dtBussnissTrip.Size = new System.Drawing.Size(875, 138);
            this.dtBussnissTrip.TabIndex = 5;
            this.dtBussnissTrip.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtBussnissTrip_CellClick);
            // 
            // dtTravel
            // 
            this.dtTravel.AllowUserToAddRows = false;
            this.dtTravel.AllowUserToDeleteRows = false;
            this.dtTravel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtTravel.Location = new System.Drawing.Point(14, 278);
            this.dtTravel.Name = "dtTravel";
            this.dtTravel.RowTemplate.Height = 23;
            this.dtTravel.Size = new System.Drawing.Size(875, 192);
            this.dtTravel.TabIndex = 6;
            this.dtTravel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtTravel_CellClick);
            this.dtTravel.SelectionChanged += new System.EventHandler(this.dtTravel_SelectionChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "出差申请";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "出差申请明细";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(704, 479);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(185, 36);
            this.btnEdit.TabIndex = 8;
            this.btnEdit.Text = "修改";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // txtCheckState
            // 
            this.txtCheckState.Location = new System.Drawing.Point(402, 488);
            this.txtCheckState.Name = "txtCheckState";
            this.txtCheckState.Size = new System.Drawing.Size(136, 21);
            this.txtCheckState.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 491);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(365, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "出差申请审核状态(0 未提交，1 审核中，2审核通过，3审核不通过)";
            // 
            // labMsg
            // 
            this.labMsg.AutoSize = true;
            this.labMsg.Location = new System.Drawing.Point(400, 70);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new System.Drawing.Size(0, 12);
            this.labMsg.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(705, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(184, 34);
            this.button1.TabIndex = 4;
            this.button1.Text = "处理出差";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TravelEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 524);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labMsg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtTravel);
            this.Controls.Add(this.dtBussnissTrip);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSerch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCheckState);
            this.Controls.Add(this.txtName);
            this.Name = "TravelEditForm";
            this.Text = "TravelEditForm";
            ((System.ComponentModel.ISupportInitialize)(this.dtBussnissTrip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTravel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSerch;
        private System.Windows.Forms.DataGridView dtBussnissTrip;
        private System.Windows.Forms.DataGridView dtTravel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.TextBox txtCheckState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labMsg;
        private System.Windows.Forms.Button button1;
    }
}