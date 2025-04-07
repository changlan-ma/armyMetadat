namespace armyMetadata.Forms
{
    partial class ExportFirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportFirm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OutputFileButton = new DevExpress.XtraEditors.SimpleButton();
            this.InputFileButton = new DevExpress.XtraEditors.SimpleButton();
            this.outputTextBox = new DevExpress.XtraEditors.TextEdit();
            this.inputTextBox = new DevExpress.XtraEditors.TextEdit();
            this.cbLR = new System.Windows.Forms.CheckBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancleBtn = new DevExpress.XtraEditors.SimpleButton();
            this.OkBtn = new DevExpress.XtraEditors.SimpleButton();
            this.ucWaiting1 = new armyMetadata.UserControsl.UcWaiting();
            this.BarProgress = new DevExpress.XtraEditors.ProgressBarControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputTextBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputTextBox.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarProgress.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OutputFileButton);
            this.groupBox1.Controls.Add(this.InputFileButton);
            this.groupBox1.Controls.Add(this.outputTextBox);
            this.groupBox1.Controls.Add(this.inputTextBox);
            this.groupBox1.Controls.Add(this.cbLR);
            this.groupBox1.Controls.Add(this.labelControl3);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(654, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数设置";
            // 
            // OutputFileButton
            // 
            this.OutputFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputFileButton.Location = new System.Drawing.Point(557, 62);
            this.OutputFileButton.Name = "OutputFileButton";
            this.OutputFileButton.Size = new System.Drawing.Size(75, 23);
            this.OutputFileButton.TabIndex = 7;
            this.OutputFileButton.Text = "选择";
            this.OutputFileButton.Click += new System.EventHandler(this.OutputFileButton_Click);
            // 
            // InputFileButton
            // 
            this.InputFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFileButton.Location = new System.Drawing.Point(557, 27);
            this.InputFileButton.Name = "InputFileButton";
            this.InputFileButton.Size = new System.Drawing.Size(75, 23);
            this.InputFileButton.TabIndex = 6;
            this.InputFileButton.Text = "选择";
            this.InputFileButton.Click += new System.EventHandler(this.InputFileButton_Click);
            // 
            // outputTextBox
            // 
            this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextBox.Location = new System.Drawing.Point(102, 64);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.Size = new System.Drawing.Size(439, 20);
            this.outputTextBox.TabIndex = 5;
            // 
            // inputTextBox
            // 
            this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTextBox.Location = new System.Drawing.Point(102, 30);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(439, 20);
            this.inputTextBox.TabIndex = 4;
            // 
            // cbLR
            // 
            this.cbLR.AutoSize = true;
            this.cbLR.Location = new System.Drawing.Point(102, 100);
            this.cbLR.Name = "cbLR";
            this.cbLR.Size = new System.Drawing.Size(86, 18);
            this.cbLR.TabIndex = 3;
            this.cbLR.Text = "是否右对齐";
            this.cbLR.UseVisualStyleBackColor = true;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(22, 100);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "控制参数：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(22, 64);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "输出路径：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "输入路径：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancleBtn);
            this.panel1.Controls.Add(this.OkBtn);
            this.panel1.Controls.Add(this.ucWaiting1);
            this.panel1.Controls.Add(this.BarProgress);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 136);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 88);
            this.panel1.TabIndex = 1;
            // 
            // CancleBtn
            // 
            this.CancleBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancleBtn.Location = new System.Drawing.Point(557, 52);
            this.CancleBtn.Name = "CancleBtn";
            this.CancleBtn.Size = new System.Drawing.Size(75, 23);
            this.CancleBtn.TabIndex = 4;
            this.CancleBtn.Text = "取消";
            this.CancleBtn.Click += new System.EventHandler(this.CancleBtn_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(466, 52);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 3;
            this.OkBtn.Text = "确定";
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // ucWaiting1
            // 
            this.ucWaiting1.Location = new System.Drawing.Point(102, 48);
            this.ucWaiting1.Name = "ucWaiting1";
            this.ucWaiting1.Size = new System.Drawing.Size(238, 30);
            this.ucWaiting1.TabIndex = 2;
            // 
            // BarProgress
            // 
            this.BarProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BarProgress.Location = new System.Drawing.Point(102, 14);
            this.BarProgress.Name = "BarProgress";
            this.BarProgress.Size = new System.Drawing.Size(530, 25);
            this.BarProgress.TabIndex = 1;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(22, 18);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "处理进度：";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 224);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(654, 137);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // ExportFirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 361);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(670, 400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(670, 400);
            this.Name = "ExportFirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出文本";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportFirm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputTextBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputTextBox.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarProgress.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private DevExpress.XtraEditors.TextEdit inputTextBox;
        private System.Windows.Forms.CheckBox cbLR;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton OutputFileButton;
        private DevExpress.XtraEditors.SimpleButton InputFileButton;
        private DevExpress.XtraEditors.TextEdit outputTextBox;
        private DevExpress.XtraEditors.ProgressBarControl BarProgress;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton CancleBtn;
        private DevExpress.XtraEditors.SimpleButton OkBtn;
        private UserControsl.UcWaiting ucWaiting1;
    }
}