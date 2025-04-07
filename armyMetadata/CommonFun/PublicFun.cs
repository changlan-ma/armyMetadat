using System;
using System.Drawing;
using System.Windows.Forms;

namespace armyMetadata.CommonFun
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum enumLogType
    {
        enumLogTypeUnknown = -1,
        enumLogTypeInfo = 0,
        enumLogTypeError = 1,
        enumLogTypeSuccess = 2
    }
    public class PublicFun
    {
        #region 日志打印
        public static void AddLog(System.Windows.Forms.RichTextBox richTextBox1, string strResult, enumLogType LogType)
        {
            Color SelectionColor = Color.Black;
            string constCrlf = "\r\n";
            switch (LogType)
            {
                case enumLogType.enumLogTypeUnknown:
                    SelectionColor = Color.DarkOrange;
                    break;
                case enumLogType.enumLogTypeInfo:
                    SelectionColor = Color.Black;
                    break;
                case enumLogType.enumLogTypeError:
                    SelectionColor = Color.Red;
                    richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style | FontStyle.Bold);
                    break;
                case enumLogType.enumLogTypeSuccess:
                    SelectionColor = Color.Green;
                    richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style | FontStyle.Bold);
                    break;
            }
            richTextBox1.SelectionColor = SelectionColor;
            string text = "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + strResult;
            if (strResult != null && !string.IsNullOrWhiteSpace(strResult))
            {
                if (strResult.StartsWith(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    text = "" + strResult;
                }
            }
            if (richTextBox1.Text == "")
            {
                richTextBox1.SelectedText = text;
            }
            else
            {
                richTextBox1.SelectedText = constCrlf + text;
            }
            richTextBox1.Select(richTextBox1.TextLength, 0);
            //滚动到光标处
            richTextBox1.ScrollToCaret();
            Application.DoEvents();
        }
        #endregion
    }
}