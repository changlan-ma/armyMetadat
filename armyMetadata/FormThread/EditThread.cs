using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace armyMetadata.FormThread
{
    public class EditThread
    {
        private string[] m_IFile;
        private string m_OFile;
        private string[] m_dataStr;
        private string[] m_dataConstr;
        private Thread mThread = null;
        private int isrunning = 0;
        public delegate void TransferProgressEventHandler(string progress);
        public event TransferProgressEventHandler TransferProgressDelegate;

        public EditThread(string inPath, string outPath, string dataStr, string dataConstr)
        {
            string[] excelFile = Directory.GetFiles(inPath, "*.xlsx", SearchOption.AllDirectories);
            if (excelFile.Length == 0)
            {
                return;
            }
            else
            {
                List<string> list = new List<string>();
                //开始读取表格文件至m_IFile数组中
                for (int i = 0; i < excelFile.Length; i++)
                {
                    string mFile = excelFile[i];
                    list.Add(mFile);
                }
                m_IFile = list.ToArray();
            }
            m_OFile = outPath;
            m_dataStr = dataStr.Split(new char[] { ';' });
            m_dataConstr = dataConstr.Split(new char[] { ';' });
            mThread = new Thread(new ThreadStart(Process));
        }

        public void Start()
        {
            if (mThread != null)
            {
                this.mThread.Start();
            }
        }
        /// <summary>
        /// 元数据批量修改方法
        /// </summary>
        private void Process()
        {
            string inputExcel = string.Empty;
            // 判断文件夹是否存在，不存在则创建
            if (!Directory.Exists(m_OFile))
            {
                try
                {
                    Directory.CreateDirectory(m_OFile);
                }
                catch
                {

                }
            }
            // 使用 Math.Round() 保留两位小数
            double pro = (double)100 / m_IFile.Length;
            double roundedResult = Math.Round(pro, 2);
            if (m_dataStr.Length != m_dataConstr.Length)
            {
                isrunning = -1;
                return;
            }
            //循环读取表格
            for (int i = 0; i < m_IFile.Length; i++)
            {
                int result = 0;
                inputExcel = m_IFile[i];
                string fileNameWithoutExtion = Path.GetFileNameWithoutExtension(inputExcel);
                string fileName = Path.GetFileName(inputExcel);
                string outFile = m_OFile + "//" + fileNameWithoutExtion + "_XG.xlsx";
                TransferProgressDelegate("开始读取" + fileName);
                using (FileStream fileStream = new FileStream(inputExcel, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(fileStream);
                    if (workbook != null)
                    {
                        ISheet sheet = workbook.GetSheetAt(0);
                        int columnDataItem = -1;  // “数据项”列的索引
                        int columnDataContent = -1;  // “数据内容”列的索引
                        var headerRow = sheet.GetRow(1); // 读取第二行，查找 "数据项" 和 "数据内容" 和“序号”的列索引
                        if (headerRow != null)
                        {
                            for (int col = 0; col < headerRow.LastCellNum; col++)
                            {
                                string cellValue = headerRow.GetCell(col) == null ? string.Empty : headerRow.GetCell(col).ToString();
                                if (cellValue == "数据项")
                                {
                                    columnDataItem = col;
                                }
                                else if (cellValue == "数据内容")
                                {
                                    columnDataContent = col;
                                }
                            }
                        }
                        //检查是否找到两列
                        if (columnDataItem == -1 || columnDataContent == -1)
                        {
                            result = -1;
                            TransferProgressDelegate(fileName + "读取失败");
                            continue;
                        }
                        else
                        {
                            result = 0;
                            TransferProgressDelegate(fileName + "读取成功");
                        }
                        //
                        Dictionary<string, string> dicValue = new Dictionary<string, string>();

                        for (int j = 0; j < m_dataStr.Length; j++)
                        {
                            dicValue.Add(m_dataStr[j], m_dataConstr[j]);
                        }

                        int isCheck = 0;
                        //从第三行开始
                        for (int rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            //result = 0;
                            IRow row = sheet.GetRow(rowIndex);
                            if (row != null)
                            {
                                // 获取数据项和数据内容的列
                                ICell dataNum = row.GetCell(columnDataItem);
                                ICell dataCon = row.GetCell(columnDataContent);
                                if (dicValue.ContainsKey(dataNum.ToString()))
                                {
                                    dataCon.SetCellValue(dicValue[dataNum.ToString()]);
                                    isCheck = isCheck + 1;
                                }                                
                            }
                        }
                        if (isCheck != m_dataStr.Length)
                        {
                            TransferProgressDelegate(inputExcel + "数据项不匹配");
                            result = -1;
                        }
                    }
                    using (FileStream file = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(file);
                    }
                }
                if (result != 0)
                {
                    TransferProgressDelegate(inputExcel + "未成功输出");
                    isrunning = -1;
                }
                else
                {
                    TransferProgressDelegate(inputExcel + "输出成功");
                    string pros = (roundedResult * (i + 1)).ToString();
                    TransferProgressDelegate(pros);
                    isrunning = 0;
                }
            }
            if (isrunning == 0)
                TransferProgressDelegate("100");
            else
                TransferProgressDelegate("CloseError:算法执行异常");
        }
    }
}
