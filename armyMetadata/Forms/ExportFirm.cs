using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using armyMetadata.CommonFun;

namespace armyMetadata.Forms
{
    public partial class ExportFirm : DevExpress.XtraEditors.XtraForm
    {
        //TODO:加锁
        private bool isFreed = true;
        private bool isRunning = false;
        private FormThread.ExportThread mThread = null;
        public ExportFirm()
        {
            InitializeComponent();
            this.BarProgress.Properties.Maximum = 100;
            this.BarProgress.Properties.Minimum = 0;
            this.BarProgress.Position = 0;//当前值
            this.BarProgress.Properties.ShowTitle = true;//是否显示进度数据
            this.BarProgress.Properties.PercentView = true; //是否显示百分比
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //设定字体大小为12px                
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
        }

        private void InputFileButton_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaFolderBrowserDialog opd = new Ookii.Dialogs.VistaFolderBrowserDialog();
            if (opd.ShowDialog() != DialogResult.OK)
                return;
            this.inputTextBox.Text = opd.SelectedPath;
        }

        private void OutputFileButton_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaFolderBrowserDialog opd = new Ookii.Dialogs.VistaFolderBrowserDialog();
            if (opd.ShowDialog() != DialogResult.OK)
                return;
            this.outputTextBox.Text = opd.SelectedPath;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            isFreed = false;
            this.BarProgress.Properties.Maximum = 100;
            this.BarProgress.Properties.Minimum = 0;
            this.BarProgress.Position = 0;//当前值
            this.BarProgress.Properties.ShowTitle = true;//是否显示进度数据
            this.BarProgress.Properties.PercentView = true; //是否显示百分比            
            this.OkBtn.Enabled = false;
            this.groupBox1.Enabled = false;
            if (CheckParamsVaild() != 0) return;
            //FormThread.ExportThread mThread = new FormThread.ExportThread();
            mThread = new FormThread.ExportThread(this.inputTextBox.Text, this.outputTextBox.Text, this.cbLR.Checked);
            mThread.ExportStart();
            mThread.TransferProgressDelegate += this.SetProgress;
            //mThread.ExportFile(this.inputTextBox.Text, this.outputTextBox.Text, this.cbLR.Checked);            
            isRunning = true;
            this.ucWaiting1.StartWaiting();

        }
        private void SetProgress(string progress)
        {
            if (isFreed == false)
                this.Invoke(new ProgressEventHandler(ProgressDelegate), progress);
        }
        private delegate void ProgressEventHandler(string progress);
        private void ProgressDelegate(string progress)
        {
            if (progress.IndexOf("CloseError") == -1)
            {
                PublicFun.AddLog(this.richTextBox1, progress, enumLogType.enumLogTypeInfo);
                try
                {
                    int iPro = 0;
                    string spro = progress.Substring(progress.LastIndexOf(":") + 1);
                    if (int.TryParse(spro, out iPro))
                    {
                        this.BarProgress.Position = iPro;
                        if (iPro == 100)
                        {
                            isRunning = false;
                            isFreed = true;
                            this.ucWaiting1.StopWaiting();
                            XtraMessageBox.Show("数据处理完成", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //this.DialogResult = DialogResult.OK;
                            //System.Diagnostics.Process.GetCurrentProcess().Kill();
                            this.groupBox1.Enabled = true;
                            this.OkBtn.Enabled = true;

                        }
                    }
                }
                catch 
                {
                   
                }               
            }
            else
            {
                PublicFun.AddLog(this.richTextBox1, progress, enumLogType.enumLogTypeError);
                XtraMessageBox.Show("数据处理失败" + progress, "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isRunning = false;
                isFreed = true;
                this.OkBtn.Enabled = true;
                this.groupBox1.Enabled = true;
                this.ucWaiting1.StopWaiting();
            }
        }

        private int CheckParamsVaild()
        {
            if (this.inputTextBox.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择输入路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.OkBtn.Enabled = true;
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.inputTextBox.Text.IndexOf(" ") >= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择输入路径不存在空格", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.OkBtn.Enabled = true;
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.outputTextBox.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择输出路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.OkBtn.Enabled = true;
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.outputTextBox.Text.IndexOf(" ") >= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择输出路径不存在空格", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.OkBtn.Enabled = true;
                this.groupBox1.Enabled = true;
                return -1;
            }
            return 0;
        }

        private void ExportFirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
            {
                if (XtraMessageBox.Show("确定要结束进程吗!", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    e.Cancel = true;
                else
                {
                    this.ucWaiting1.StopWaiting();
                    isFreed = true;
                    // KillProcess();
                    e.Cancel = false;
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void CancleBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}