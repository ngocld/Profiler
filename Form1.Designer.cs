namespace Profiler
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.grvListTrace = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCreateTrace = new System.Windows.Forms.Button();
            this.txtConnection = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cboTrace = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblInfoSource = new System.Windows.Forms.Label();
            this.upd_NumberFile = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.rbView = new System.Windows.Forms.RadioButton();
            this.rbExcel = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDirLogSQL = new System.Windows.Forms.TextBox();
            this.tabReport = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbReport = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtCustomReport = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnExportReport = new System.Windows.Forms.Button();
            this.picHelp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.grvListTrace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upd_NumberFile)).BeginInit();
            this.tabReport.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
            this.SuspendLayout();
            // 
            // grvListTrace
            // 
            this.grvListTrace.AllowUserToAddRows = false;
            this.grvListTrace.AllowUserToDeleteRows = false;
            this.grvListTrace.AllowUserToResizeColumns = false;
            this.grvListTrace.AllowUserToResizeRows = false;
            this.grvListTrace.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grvListTrace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvListTrace.Location = new System.Drawing.Point(33, 158);
            this.grvListTrace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grvListTrace.Name = "grvListTrace";
            this.grvListTrace.ReadOnly = true;
            this.grvListTrace.RowHeadersVisible = false;
            this.grvListTrace.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grvListTrace.Size = new System.Drawing.Size(837, 97);
            this.grvListTrace.TabIndex = 0;
            this.grvListTrace.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvListTrace_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection SQL Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 129);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Trace Logs";
            // 
            // btnCreateTrace
            // 
            this.btnCreateTrace.Location = new System.Drawing.Point(484, 106);
            this.btnCreateTrace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCreateTrace.Name = "btnCreateTrace";
            this.btnCreateTrace.Size = new System.Drawing.Size(123, 35);
            this.btnCreateTrace.TabIndex = 4;
            this.btnCreateTrace.Text = "Create Trace";
            this.btnCreateTrace.UseVisualStyleBackColor = true;
            this.btnCreateTrace.Click += new System.EventHandler(this.btnCreateTrace_Click);
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(220, 26);
            this.txtConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(514, 26);
            this.txtConnection.TabIndex = 5;
            this.txtConnection.Enter += new System.EventHandler(this.txtConnection_Enter);
            this.txtConnection.Leave += new System.EventHandler(this.txtConnection_Leave);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(354, 106);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(122, 35);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "View Trace";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cboTrace
            // 
            this.cboTrace.FormattingEnabled = true;
            this.cboTrace.Location = new System.Drawing.Point(216, 297);
            this.cboTrace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboTrace.Name = "cboTrace";
            this.cboTrace.Size = new System.Drawing.Size(422, 28);
            this.cboTrace.TabIndex = 19;
            this.cboTrace.SelectedIndexChanged += new System.EventHandler(this.cboTrace_SelectedIndexChanged);
            this.cboTrace.Leave += new System.EventHandler(this.cboTrace_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 302);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "Source data";
            // 
            // lblInfoSource
            // 
            this.lblInfoSource.AutoSize = true;
            this.lblInfoSource.Location = new System.Drawing.Point(213, 338);
            this.lblInfoSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfoSource.Name = "lblInfoSource";
            this.lblInfoSource.Size = new System.Drawing.Size(166, 20);
            this.lblInfoSource.TabIndex = 25;
            this.lblInfoSource.Text = "Information File Log ...";
            // 
            // upd_NumberFile
            // 
            this.upd_NumberFile.Location = new System.Drawing.Point(654, 298);
            this.upd_NumberFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.upd_NumberFile.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.upd_NumberFile.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upd_NumberFile.Name = "upd_NumberFile";
            this.upd_NumberFile.Size = new System.Drawing.Size(82, 26);
            this.upd_NumberFile.TabIndex = 26;
            this.upd_NumberFile.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(742, 303);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 20);
            this.label5.TabIndex = 27;
            this.label5.Text = "(No. Extend)";
            // 
            // rbView
            // 
            this.rbView.AutoSize = true;
            this.rbView.Location = new System.Drawing.Point(218, 392);
            this.rbView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbView.Name = "rbView";
            this.rbView.Size = new System.Drawing.Size(64, 24);
            this.rbView.TabIndex = 28;
            this.rbView.TabStop = true;
            this.rbView.Text = "Grid";
            this.rbView.UseVisualStyleBackColor = true;
            // 
            // rbExcel
            // 
            this.rbExcel.AutoSize = true;
            this.rbExcel.Location = new System.Drawing.Point(303, 392);
            this.rbExcel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbExcel.Name = "rbExcel";
            this.rbExcel.Size = new System.Drawing.Size(72, 24);
            this.rbExcel.TabIndex = 29;
            this.rbExcel.TabStop = true;
            this.rbExcel.Text = "Excel";
            this.rbExcel.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(69, 402);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 20);
            this.label7.TabIndex = 31;
            this.label7.Text = "Report Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 69);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(167, 20);
            this.label8.TabIndex = 32;
            this.label8.Text = "Path Logs SQL Server";
            // 
            // txtDirLogSQL
            // 
            this.txtDirLogSQL.Location = new System.Drawing.Point(220, 66);
            this.txtDirLogSQL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDirLogSQL.Name = "txtDirLogSQL";
            this.txtDirLogSQL.Size = new System.Drawing.Size(514, 26);
            this.txtDirLogSQL.TabIndex = 33;
            // 
            // tabReport
            // 
            this.tabReport.Controls.Add(this.tabPage1);
            this.tabReport.Controls.Add(this.tabPage2);
            this.tabReport.Location = new System.Drawing.Point(218, 443);
            this.tabReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabReport.Name = "tabReport";
            this.tabReport.SelectedIndex = 0;
            this.tabReport.Size = new System.Drawing.Size(519, 380);
            this.tabReport.TabIndex = 34;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbReport);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(511, 347);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Template";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbReport
            // 
            this.lbReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbReport.FormattingEnabled = true;
            this.lbReport.ItemHeight = 20;
            this.lbReport.Location = new System.Drawing.Point(9, 9);
            this.lbReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbReport.Name = "lbReport";
            this.lbReport.Size = new System.Drawing.Size(486, 320);
            this.lbReport.TabIndex = 0;
            this.lbReport.DoubleClick += new System.EventHandler(this.lbReport_DoubleClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtCustomReport);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(511, 347);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Custom";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtCustomReport
            // 
            this.txtCustomReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCustomReport.Location = new System.Drawing.Point(9, 9);
            this.txtCustomReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCustomReport.Multiline = true;
            this.txtCustomReport.Name = "txtCustomReport";
            this.txtCustomReport.Size = new System.Drawing.Size(486, 326);
            this.txtCustomReport.TabIndex = 35;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(70, 451);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 20);
            this.label9.TabIndex = 35;
            this.label9.Text = "Report Name";
            // 
            // btnExportReport
            // 
            this.btnExportReport.Location = new System.Drawing.Point(408, 832);
            this.btnExportReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Size = new System.Drawing.Size(122, 35);
            this.btnExportReport.TabIndex = 36;
            this.btnExportReport.Text = "View Report";
            this.btnExportReport.UseVisualStyleBackColor = true;
            this.btnExportReport.Click += new System.EventHandler(this.btnExportReport_Click);
            // 
            // picHelp
            // 
            this.picHelp.Location = new System.Drawing.Point(872, 5);
            this.picHelp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picHelp.Name = "picHelp";
            this.picHelp.Size = new System.Drawing.Size(52, 52);
            this.picHelp.TabIndex = 37;
            this.picHelp.TabStop = false;
            this.picHelp.Click += new System.EventHandler(this.picHelp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 895);
            this.Controls.Add(this.picHelp);
            this.Controls.Add(this.btnExportReport);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tabReport);
            this.Controls.Add(this.txtDirLogSQL);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.rbExcel);
            this.Controls.Add(this.rbView);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.upd_NumberFile);
            this.Controls.Add(this.lblInfoSource);
            this.Controls.Add(this.cboTrace);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.txtConnection);
            this.Controls.Add(this.btnCreateTrace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grvListTrace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Analyze query SQL FPT.iHRP v1.3.1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grvListTrace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upd_NumberFile)).EndInit();
            this.tabReport.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grvListTrace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCreateTrace;
        private System.Windows.Forms.TextBox txtConnection;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cboTrace;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblInfoSource;
        private System.Windows.Forms.NumericUpDown upd_NumberFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbView;
        private System.Windows.Forms.RadioButton rbExcel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDirLogSQL;
        private System.Windows.Forms.TabControl tabReport;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lbReport;
        private System.Windows.Forms.TextBox txtCustomReport;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnExportReport;
        private System.Windows.Forms.PictureBox picHelp;
    }
}

