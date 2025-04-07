using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Text.RegularExpressions;

namespace armyMetadata.FormThread
{
    public class ExportThread
    {
        private string[] m_IFile;
        private bool b_BisHistory;
        private string m_OFile;
        private Thread mThread = null;
        private int isrunning = 0;
        private int result = 0;
        public delegate void TransferProgressEventHandler(string progress);
        public event TransferProgressEventHandler TransferProgressDelegate;
        public ExportThread(string inPath, string outPath, bool BisHistory)
        {
            b_BisHistory = BisHistory;
            string[] excelFile = Directory.GetFiles(inPath, "*.xlsx", SearchOption.AllDirectories);
            if (excelFile.Length == 0)
            {
                //TransferProgressDelegate("文件夹下不存在目标文件！");
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

            mThread = new Thread(new ThreadStart(ExportProcess));
            // ExportProcess();

        }
        public void ExportStart()
        {
            if (mThread != null)
            {
                this.mThread.Start();
            }
        }

        //读取表格输出至txt
        private void ExportProcess()
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
            TransferProgressDelegate("0.1");
            if (b_BisHistory)       //右对齐输出
            {
                //循环读取表格
                for (int i = 0; i < m_IFile.Length; i++)
                {
                   // int result = 0;
                    inputExcel = m_IFile[i];
                    string fileNameWithoutExtion = Path.GetFileNameWithoutExtension(inputExcel);
                    string fileName = Path.GetFileName(inputExcel);
                    string outFile = m_OFile + "//" + fileNameWithoutExtion + ".txt";
                    TransferProgressDelegate("开始读取" + fileName);
                    using (FileStream fileStream = new FileStream(inputExcel, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = new XSSFWorkbook(fileStream);
                        if (workbook != null)
                        {
                            ISheet sheet = workbook.GetSheetAt(0);
                            int columnNumber = -1; //“序号”列的索引
                            int columnDataItem = -1;  // “数据项”列的索引
                            int columnDataContent = -1;  // “数据内容”列的索引
                            int maxNumberItemLength = -1; //计算“序号”列的最大宽度
                            int maxDataItemLength = 0;  // 计算“数据项”列的最大宽度
                            int maxDataContentLg = 0;   // 计算“数据内容”列的最大宽度
                            // 读取第二行，查找 "数据项" 和 "数据内容" 和“序号”的列索引          
                            var headerRow = sheet.GetRow(1);
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
                                    else if (cellValue == "序号")
                                    {
                                        columnNumber = col;
                                    }
                                }
                            }

                            // 检查是否找到三列
                            if (columnDataItem == -1 || columnDataContent == -1 || columnNumber == -1)
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

                            // 计算“数据项”和“数据内容”和“序号”列的最大长度
                            for (int rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)  // 从第3行开始（跳过标题）
                            {
                                var row = sheet.GetRow(rowIndex);
                                if (row != null)
                                {
                                    var numberCell = row.GetCell(columnNumber);
                                    var dataCell = row.GetCell(columnDataItem);
                                    var contentCell = row.GetCell(columnDataContent);
                                    if (dataCell != null)
                                    {
                                        string cellText = dataCell.ToString().Trim();
                                        maxDataItemLength = Math.Max(maxDataItemLength, cellText.Length);
                                    }
                                    if (numberCell != null)
                                    {
                                        string cellText = numberCell.ToString().Trim();
                                        maxNumberItemLength = Math.Max(maxNumberItemLength, cellText.Length);
                                    }
                                    if (contentCell != null)
                                    {
                                        string cellText = contentCell.ToString().Trim();
                                        maxDataContentLg = Math.Max(maxDataContentLg, cellText.Length);
                                    }
                                }
                            }

                            //创建并打开输出文件流
                            using (StreamWriter writer = new StreamWriter(outFile))
                            {
                                //List<string> dt = new List<string>();
                                // 遍历 Excel 数据
                                for (int rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)  // 从第3行开始（跳过标题）
                                {
                                    var row = sheet.GetRow(rowIndex);
                                    if (row != null)
                                    {
                                        string formattedLine = string.Empty;
                                        DataFormatter forMatter = new DataFormatter();
                                        //获取“序号”列内容
                                        ICell dataNumberCell = row.GetCell(columnNumber);
                                        string dataNumber = dataNumberCell != null ? dataNumberCell.ToString().Trim() : "";

                                        //获取“数据项”列内容
                                        ICell dataItemCell = row.GetCell(columnDataItem);
                                        string dataItem = dataItemCell != null ? dataItemCell.ToString().Trim() : "";
                                        //dt.Add(dataItem);
                                        //获取“数据内容”列内容
                                        ICell dataContentCell = row.GetCell(columnDataContent);
                                        string dataContent = string.Empty;
                                        if (dataContentCell != null)
                                        {
                                            if (dataContentCell.CellType == CellType.Numeric)
                                            {
                                                dataContent = forMatter.FormatCellValue(dataContentCell);
                                            }
                                            else
                                            {
                                                dataContent = dataContentCell.ToString().Trim();
                                            }
                                        }
                                        else
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.ToString().Trim() : "";
                                        }
                                        #region 旧方法
                                        /******************
                                        dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        if (dataItem.Contains("更新区域平面中误差"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("左上角点纬度"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("左上角点经度"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("右下角点纬度"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("右下角点经度（横坐标）"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("规则DEM格网间距"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("平面位置中误差"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("数据量"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("正射影像地面分辨率"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("原始影像地面分辨率"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else if (dataItem.Contains("更新影像地面分辨率"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("F" + dataContentCell.CellStyle.DataFormat).Trim() : "";
                                        }
                                        else
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.ToString().Trim() : "";
                                        }
                                        ********************/
                                        #endregion
                                        //右对齐输出格式设置
                                        int leftCount = Regex.Matches(dataItem, "[a-zA-Z]").Count;
                                        int rightCount = Regex.Matches(dataContent, "[a-zA-Z]").Count + Regex.Matches(dataContent, @"\d").Count + Regex.Matches(dataContent, @"[^\w、。，：；（）]").Count;
                                        if (leftCount == 0)
                                        {
                                            // 右对齐“数据项”，左对齐“数据内容”
                                            if (rightCount == 0)
                                            {
                                                formattedLine = dataNumber.PadLeft(maxNumberItemLength) + "  " + dataItem.PadLeft(maxDataItemLength + maxDataItemLength - dataItem.Length) + "  " + dataContent.PadLeft(maxDataContentLg + maxDataContentLg - dataContent.Length);  // 两列间隔 2 个空格
                                            }
                                            else
                                            {
                                                formattedLine = dataNumber.PadLeft(maxNumberItemLength) + "  " + dataItem.PadLeft(maxDataItemLength + maxDataItemLength - dataItem.Length) + "  " + dataContent.PadLeft(maxDataContentLg + maxDataContentLg - dataContent.Length + rightCount);
                                            }
                                        }
                                        else
                                        {
                                            if (rightCount != 0)
                                            {
                                                formattedLine = dataNumber.PadLeft(maxNumberItemLength) + "  " + dataItem.PadLeft(maxDataItemLength + maxDataItemLength - dataItem.Length + leftCount) + "  " + dataContent.PadLeft(maxDataContentLg + maxDataContentLg - dataContent.Length + rightCount);  // 两列间隔 2 个空格
                                            }
                                            else
                                            {
                                                formattedLine = dataNumber.PadLeft(maxNumberItemLength) + "  " + dataItem.PadLeft(maxDataItemLength + maxDataItemLength - dataItem.Length + leftCount) + "  " + dataContent.PadLeft(maxDataContentLg + maxDataContentLg - dataContent.Length);  // 两列间隔 2 个空格
                                            }
                                        }
                                        writer.WriteLine(formattedLine);
                                    }
                                }
                            }
                        }
                    }
                    if (result != 0)
                    {
                        TransferProgressDelegate(inputExcel + "未成功输出");
                        isrunning = -1;
                    }
                    else
                    {
                        TransferProgressDelegate(inputExcel + "输出成功！");
                        string pros = (roundedResult * (i + 1)).ToString();
                        TransferProgressDelegate(pros);
                        isrunning = 0;
                    }
                }
            }
            //左对齐输出
            else
            {
                //循环读取表格
                for (int i = 0; i < m_IFile.Length; i++)
                {
                    inputExcel = m_IFile[i];
                    string fileNameWithoutExtion = Path.GetFileNameWithoutExtension(inputExcel);
                    string fileName = Path.GetFileName(inputExcel);
                    string outFile = m_OFile + "//" + fileNameWithoutExtion + ".txt";

                    TransferProgressDelegate("开始读取" + fileName);
                    using (FileStream fileStream = new FileStream(inputExcel, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = new XSSFWorkbook(fileStream);
                        if (workbook != null)
                        {
                            ISheet sheet = workbook.GetSheetAt(0);
                            int columnNumber = -1; //“序号”列的索引
                            int columnDataItem = -1;  // “数据项”列的索引
                            int columnDataContent = -1;  // “数据内容”列的索引
                            int maxDataItemLength = 0;  // 计算“数据项”列的最大宽度
                            int maxNumberItemLength = 0; //计算“序号”列的最大宽度

                            // 读取第二行，查找 "数据项" 和 "数据内容" 和“序号”的列索引                            
                            var headerRow = sheet.GetRow(1);
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
                                    else if (cellValue == "序号")
                                    {
                                        columnNumber = col;
                                    }
                                }
                            }
                            // 检查是否找到三列
                            if (columnDataItem == -1 || columnDataContent == -1 || columnNumber == -1)
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
                            // 计算“数据项”列和“序号”列的最大长度
                            for (int rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)  // 从第3行开始（跳过标题）
                            {
                                var row = sheet.GetRow(rowIndex);
                                if (row != null)
                                {
                                    var dataCell = row.GetCell(columnDataItem);
                                    var numberCell = row.GetCell(columnNumber);
                                    if (dataCell != null)
                                    {
                                        string cellText = dataCell.ToString().Trim();
                                        maxDataItemLength = Math.Max(maxDataItemLength, cellText.Length);
                                    }
                                    if (numberCell != null)
                                    {
                                        string cellText = numberCell.ToString().Trim();
                                        maxNumberItemLength = Math.Max(maxNumberItemLength, cellText.Length);
                                    }
                                }
                            }

                            //创建并打开输出文件流
                            using (StreamWriter writer = new StreamWriter(outFile))
                            {
                                // 遍历 Excel 数据
                                for (int rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)  // 从第3行开始（跳过标题）
                                {
                                    var row = sheet.GetRow(rowIndex);
                                    if (row != null)
                                    {
                                        string formattedLine = string.Empty;
                                        DataFormatter forMatter = new DataFormatter();
                                        //获取“序号”列内容
                                        ICell dataNumberCell = row.GetCell(columnNumber);
                                        string dataNumber = dataNumberCell != null ? dataNumberCell.ToString().Trim() : "";

                                        //获取“数据项”列内容
                                        ICell dataItemCell = row.GetCell(columnDataItem);
                                        string dataItem = dataItemCell != null ? dataItemCell.ToString().Trim() : "";

                                        //获取“数据内容”列内容
                                        ICell dataContentCell = row.GetCell(columnDataContent);
                                        //string dataContent = dataContentCell != null ? dataContentCell.ToString().Trim() : "";
                                        string dataContent = string.Empty;

                                        if (dataContentCell != null)
                                        {
                                            if (dataContentCell.CellType == CellType.Numeric)
                                            {
                                                dataContent = forMatter.FormatCellValue(dataContentCell);
                                            }
                                            else
                                            {
                                                dataContent = dataContentCell.ToString().Trim();
                                            }
                                        }
                                        else
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.ToString().Trim() : "";
                                        }
                                        #region 旧方法
                                        /*****************************
                                        if (dataItem.Contains("更新区域平面中误差"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.00").Trim() : "";
                                        }
                                        else if (dataItem.Contains("左上角点纬度"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.00").Trim() : "";
                                        }
                                        else if (dataItem.Contains("左上角点经度"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.00").Trim() : "";
                                        }
                                        else if (dataItem.Contains("右下角点纬度"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.00").Trim() : "";
                                        }
                                        else if (dataItem.Contains("右下角点经度"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.00").Trim() : "";
                                        }
                                        else if (dataItem.Contains("规则DEM格网间距"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.00").Trim() : "";
                                        }
                                        else if (dataItem.Contains("平面位置中误差"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.00").Trim() : "";
                                        }
                                        else if (dataItem.Contains("数据量"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.00").Trim() : "";
                                        }
                                        else if (dataItem.Contains("正射影像地面分辨率"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.0").Trim() : "";
                                        }
                                        else if (dataItem.Contains("原始影像地面分辨率"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.0").Trim() : "";
                                        }
                                        else if (dataItem.Contains("更新影像地面分辨率"))
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.NumericCellValue.ToString("0.0").Trim() : "";
                                        }
                                        else
                                        {
                                            dataContent = dataContentCell != null ? dataContentCell.ToString().Trim() : "";
                                        }
                                        *****************************/
                                        #endregion
                                        //左对齐输出格式设置
                                        int letterCount = Regex.Matches(dataItem, "[a-zA-Z]").Count;
                                        if (letterCount == 0)
                                        {
                                            // 左对齐“数据项”，左对齐“数据内容”,左对齐“序号”
                                            formattedLine = dataNumber.PadRight(maxNumberItemLength) + "  " + dataItem.PadRight(maxDataItemLength + maxDataItemLength - dataItem.Length) + "  " + dataContent;  // 两列间隔2个空格
                                        }
                                        else
                                        {
                                            formattedLine = dataNumber.PadRight(maxNumberItemLength) + "  " + dataItem.PadRight(maxDataItemLength + maxDataItemLength - dataItem.Length + letterCount) + "  " + dataContent;  // 两列间隔2个空格
                                        }
                                        writer.WriteLine(formattedLine);
                                    }
                                }
                            }
                        }
                    }
                    if (result != 0)
                    {
                        TransferProgressDelegate(inputExcel + "未成功输出");
                        isrunning = -1;
                    }
                    else
                    {
                        TransferProgressDelegate(inputExcel + "输出成功！");
                        string pros = (roundedResult * (i + 1)).ToString();
                        TransferProgressDelegate(pros);
                        isrunning = 0;
                    }
                }
            }
            if (isrunning == 0)
                TransferProgressDelegate("100");
            else
                TransferProgressDelegate("CloseError:算法执行异常");
        }
    }
}
