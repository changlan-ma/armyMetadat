
namespace armyMetadata.Forms
{
    partial class EditeMetedataFirm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditeMetedataFirm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OkBtn = new CCWin.SkinControl.SkinButton();
            this.outputBtn = new CCWin.SkinControl.SkinButton();
            this.inputBtn = new CCWin.SkinControl.SkinButton();
            this.inputTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.outputTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataCon_txtbox = new System.Windows.Forms.TextBox();
            this.dataName_txtbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgressBar1 = new CCWin.SkinControl.SkinProgressBar();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.OkBtn);
            this.groupBox1.Controls.Add(this.outputBtn);
            this.groupBox1.Controls.Add(this.inputBtn);
            this.groupBox1.Controls.Add(this.inputTextbox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.outputTextbox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dataCon_txtbox);
            this.groupBox1.Controls.Add(this.dataName_txtbox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(8, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(534, 135);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数设置";
            // 
            // OkBtn
            // 
            this.OkBtn.BackColor = System.Drawing.Color.Transparent;
            this.OkBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.OkBtn.DownBack = null;
            this.OkBtn.Location = new System.Drawing.Point(469, 91);
            this.OkBtn.MouseBack = null;
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.NormlBack = null;
            this.OkBtn.Size = new System.Drawing.Size(59, 23);
            this.OkBtn.TabIndex = 10;
            this.OkBtn.Text = "运行";
            this.OkBtn.UseVisualStyleBackColor = false;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // outputBtn
            // 
            this.outputBtn.BackColor = System.Drawing.Color.Transparent;
            this.outputBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.outputBtn.DownBack = null;
            this.outputBtn.Location = new System.Drawing.Point(469, 50);
            this.outputBtn.MouseBack = null;
            this.outputBtn.Name = "outputBtn";
            this.outputBtn.NormlBack = null;
            this.outputBtn.Size = new System.Drawing.Size(59, 23);
            this.outputBtn.TabIndex = 9;
            this.outputBtn.Text = "选择";
            this.outputBtn.UseVisualStyleBackColor = false;
            this.outputBtn.Click += new System.EventHandler(this.outputBtn_Click);
            // 
            // inputBtn
            // 
            this.inputBtn.BackColor = System.Drawing.Color.Transparent;
            this.inputBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.inputBtn.DownBack = null;
            this.inputBtn.Location = new System.Drawing.Point(469, 15);
            this.inputBtn.MouseBack = null;
            this.inputBtn.Name = "inputBtn";
            this.inputBtn.NormlBack = null;
            this.inputBtn.Size = new System.Drawing.Size(59, 23);
            this.inputBtn.TabIndex = 8;
            this.inputBtn.Text = "选择";
            this.inputBtn.UseVisualStyleBackColor = false;
            this.inputBtn.Click += new System.EventHandler(this.inputBtn_Click);
            // 
            // inputTextbox
            // 
            this.inputTextbox.Location = new System.Drawing.Point(91, 17);
            this.inputTextbox.Name = "inputTextbox";
            this.inputTextbox.Size = new System.Drawing.Size(372, 21);
            this.inputTextbox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "输出路径：";
            // 
            // outputTextbox
            // 
            this.outputTextbox.Location = new System.Drawing.Point(91, 52);
            this.outputTextbox.Name = "outputTextbox";
            this.outputTextbox.Size = new System.Drawing.Size(372, 21);
            this.outputTextbox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "输入路径：";
            // 
            // dataCon_txtbox
            // 
            this.dataCon_txtbox.Location = new System.Drawing.Point(290, 92);
            this.dataCon_txtbox.Name = "dataCon_txtbox";
            this.dataCon_txtbox.Size = new System.Drawing.Size(174, 21);
            this.dataCon_txtbox.TabIndex = 3;
            // 
            // dataName_txtbox
            // 
            this.dataName_txtbox.Location = new System.Drawing.Point(91, 92);
            this.dataName_txtbox.Name = "dataName_txtbox";
            this.dataName_txtbox.Size = new System.Drawing.Size(133, 21);
            this.dataName_txtbox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据内容：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据项：";
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Back = null;
            this.ProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.ProgressBar1.BarBack = null;
            this.ProgressBar1.BarRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.ProgressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ProgressBar1.ForeColor = System.Drawing.Color.Red;
            this.ProgressBar1.Location = new System.Drawing.Point(8, 270);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.ProgressBar1.Size = new System.Drawing.Size(534, 22);
            this.ProgressBar1.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox1.Location = new System.Drawing.Point(8, 165);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(534, 105);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // EditeMetedataFirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 300);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 300);
            this.Name = "EditeMetedataFirm";
            this.Text = "元数据批量修改";
            this.TitleCenter = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditeMetedataFirm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox dataCon_txtbox;
        private System.Windows.Forms.TextBox dataName_txtbox;
        private System.Windows.Forms.TextBox inputTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox outputTextbox;
        private CCWin.SkinControl.SkinButton outputBtn;
        private CCWin.SkinControl.SkinButton inputBtn;
        private CCWin.SkinControl.SkinProgressBar ProgressBar1;
        private CCWin.SkinControl.SkinButton OkBtn;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}