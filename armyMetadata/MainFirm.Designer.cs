namespace armyMetadata
{
    partial class MainFirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFirm));
            this.SoonBtn = new System.Windows.Forms.Button();
            this.ExportTxtBtn = new System.Windows.Forms.Button();
            this.BatchExportBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SoonBtn
            // 
            this.SoonBtn.Image = global::armyMetadata.Properties.Resources.PolSAR_PolSignatures;
            this.SoonBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SoonBtn.Location = new System.Drawing.Point(270, 20);
            this.SoonBtn.Name = "SoonBtn";
            this.SoonBtn.Size = new System.Drawing.Size(66, 55);
            this.SoonBtn.TabIndex = 2;
            this.SoonBtn.Text = "批量修改";
            this.SoonBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SoonBtn.UseVisualStyleBackColor = true;
            this.SoonBtn.Click += new System.EventHandler(this.SoonBtn_Click);
            // 
            // ExportTxtBtn
            // 
            this.ExportTxtBtn.Image = global::armyMetadata.Properties.Resources.CartoGraphy_Export;
            this.ExportTxtBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ExportTxtBtn.Location = new System.Drawing.Point(153, 20);
            this.ExportTxtBtn.Name = "ExportTxtBtn";
            this.ExportTxtBtn.Size = new System.Drawing.Size(66, 55);
            this.ExportTxtBtn.TabIndex = 1;
            this.ExportTxtBtn.Text = "导出文本";
            this.ExportTxtBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExportTxtBtn.UseVisualStyleBackColor = true;
            this.ExportTxtBtn.Click += new System.EventHandler(this.ExportTxtBtn_Click);
            // 
            // BatchExportBtn
            // 
            this.BatchExportBtn.Image = global::armyMetadata.Properties.Resources.Batch_ComplexDataConvert;
            this.BatchExportBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BatchExportBtn.Location = new System.Drawing.Point(39, 19);
            this.BatchExportBtn.Name = "BatchExportBtn";
            this.BatchExportBtn.Size = new System.Drawing.Size(66, 55);
            this.BatchExportBtn.TabIndex = 0;
            this.BatchExportBtn.Text = "批量导入";
            this.BatchExportBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BatchExportBtn.UseVisualStyleBackColor = true;
            this.BatchExportBtn.Click += new System.EventHandler(this.BatchExportBtn_Click);
            // 
            // MainFirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 101);
            this.Controls.Add(this.SoonBtn);
            this.Controls.Add(this.ExportTxtBtn);
            this.Controls.Add(this.BatchExportBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(390, 140);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 140);
            this.Name = "MainFirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DOM后处理程序";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BatchExportBtn;
        private System.Windows.Forms.Button ExportTxtBtn;
        private System.Windows.Forms.Button SoonBtn;
    }
}