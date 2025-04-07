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

namespace armyMetadata
{
    public partial class MainFirm : DevExpress.XtraEditors.XtraForm
    {
        public MainFirm()
        {
            InitializeComponent();
        }

        private void BatchExportBtn_Click(object sender, EventArgs e)
        {
            Forms.Form1 frm = new Forms.Form1();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void ExportTxtBtn_Click(object sender, EventArgs e)
        {
            Forms.ExportFirm frm = new Forms.ExportFirm();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void SoonBtn_Click(object sender, EventArgs e)
        {
            Forms.EditeMetedataFirm frm = new Forms.EditeMetedataFirm();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }
    }
}