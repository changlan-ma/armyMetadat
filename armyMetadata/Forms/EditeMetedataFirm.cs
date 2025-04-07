using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using armyMetadata.CommonFun;
using CCWin;
using DevExpress.XtraEditors;

namespace armyMetadata.Forms
{
    public partial class EditeMetedataFirm : Skin_Metro
    {
        //TODO:加锁
        private bool isFreed = true;
        private bool isRunning = false;
        private FormThread.EditThread mThread = null;

        public EditeMetedataFirm()
        {
            InitializeComponent();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        private void inputBtn_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaFolderBrowserDialog opd = new Ookii.Dialogs.VistaFolderBrowserDialog();
            if (opd.ShowDialog() != DialogResult.OK)
                return;
            this.inputTextbox.Text = opd.SelectedPath;
        }

        private void outputBtn_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaFolderBrowserDialog opd = new Ookii.Dialogs.VistaFolderBrowserDialog();
            if (opd.ShowDialog() != DialogResult.OK)
                return;
            this.outputTextbox.Text = opd.SelectedPath;
        }

        private int CheckParamsVaild()
        {
            if (this.inputTextbox.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择输入路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.inputTextbox.Text.IndexOf(" ") >= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择输入路径不存在空格", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.outputTextbox.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择输出路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.outputTextbox.Text.IndexOf(" ") >= 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择输出路径不存在空格", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.dataName_txtbox.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("数据项不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.dataName_txtbox.Text.EndsWith(";"))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("数据项结尾不能为;", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.groupBox1.Enabled = true;
                return -1;
            }
            if (this.dataCon_txtbox.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("数据内容不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);               
                this.groupBox1.Enabled = true;
                return -1;
            }
           
            if (this.dataCon_txtbox.Text.EndsWith(";"))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("数据内容结尾不能为;", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.groupBox1.Enabled = true;
                return -1;
            }
            return 0;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            isFreed = false;
            this.ProgressBar1.Maximum = 100;
            this.ProgressBar1.Minimum = 0;
            //this.OkBtn.Enabled = false;
            this.groupBox1.Enabled = false;
            if (CheckParamsVaild() != 0) return;
            mThread = new FormThread.EditThread(this.inputTextbox.Text, this.outputTextbox.Text, this.dataName_txtbox.Text, this.dataCon_txtbox.Text);
            mThread.Start();
            mThread.TransferProgressDelegate += this.SetProgress;
            isRunning = true;
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
                        this.ProgressBar1.Value = iPro;
                        if (iPro == 100)
                        {
                            isRunning = false;
                            isFreed = true;
                            //this.ucWaiting1.StopWaiting();
                            XtraMessageBox.Show("数据处理完成", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //this.DialogResult = DialogResult.OK;
                            //System.Diagnostics.Process.GetCurrentProcess().Kill();
                            this.groupBox1.Enabled = true;
                           // this.OkBtn.Enabled = true;

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
               // this.OkBtn.Enabled = true;
                this.groupBox1.Enabled = true;

            }
        }

        private void EditeMetedataFirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
            {
                if (XtraMessageBox.Show("确定要结束进程吗", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    e.Cancel = true;
                else
                {
                    //this.ucWaiting1.StopWaiting();
                    isFreed = true;
                    // KillProcess();
                    e.Cancel = false;
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
        }
    }
}
