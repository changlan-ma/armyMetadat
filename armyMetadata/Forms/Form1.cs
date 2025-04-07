using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinClass;
using CCWin.SkinControl;
using Com.Cnhige.Geo;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Model;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using System.IO;
using NPOI.XWPF;
using NPOI.XWPF.UserModel;
namespace armyMetadata.Forms
{
    public partial class Form1 : Skin_Metro
    {
        public Form1()
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }
        int comboxItemClickFlag = 1;
        int comboxgetFieldClick = 1;
        private string modePath = null;

        private string regularPath = null;

        private string logsPath = Environment.CurrentDirectory + "\\错误信息\\Logs.txt";

        public static Dictionary<string, string> dict = new Dictionary<string, string>();

        private List<string> modeContents = new List<string>();

        private List<string> JBTuhaoList = new List<string>();

        private List<string> doubleContents = new List<string>(){"左上角点纬度（纵坐标）", "左上角点经度（横坐标）","右下角点经度（横坐标）",
        "右下角点纬度（纵坐标）","成图比例尺分母","数据量" ,"正射影像地面分辨率","像素行数","像素列数","中央经线","投影带号",
        "规则DEM格网间距","原始影像地面分辨率","更新影像地面分辨率","更新区域平面中误差","高程中误差","起始格网点经度","起始格网点纬度"
        ,"起始格网点横坐标","起始格网点纵坐标","更新区域高程中误差","格网行数","格网列数","平面位置中误差","原始影像获取时间"};

        public class IPoint
        {
            public double X;
            public double Y;
        }
        private void Invoke_performProgressStep()
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new MethodInvoker(delegate ()
                {
                    progressBar.PerformStep();
                }));
            }
            else
            {
                progressBar.PerformStep();
            }
        }
        private void Invoke_setProgressValue(int n)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new MethodInvoker(delegate ()
                {
                    progressBar.Value = n;
                }));
            }
            else
            {
                progressBar.Value = n;
            }
        }
        private void Invoke_setProgressMax(int n)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new MethodInvoker(delegate ()
                {
                    progressBar.Maximum = n;
                }));
            }
            else
            {
                progressBar.Maximum = n;
            }
        }
        /// <summary>
        /// 用于输出txt格式成果
        /// </summary>
        /// <param name="excelPath"></param>
        private void readExcelMode(string excelPath)
        {
            modeContents.Clear();
            XSSFWorkbook wk = null;
            using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                wk = new XSSFWorkbook(fs);
                fs.Close();
            }
            ISheet mySheet = wk.GetSheetAt(0);
            int rowNum = mySheet.LastRowNum;
            for (int i = 2; i <= rowNum; i++)
            {
                IRow row = mySheet.GetRow(i);
                NPOI.SS.UserModel.ICell cell = row.GetCell(1);//数据项
                modeContents.Add(cell.ToString());
            }
        }
        /// <summary>
        /// 读取模版项文件DOM_regular/DEM_regular文件，把要填写所有项加载进dict词典
        /// </summary>
        /// <param name="excelPath"></param>
        private void readExcelRegular(string excelPath)
        {
            XSSFWorkbook wk = null;
            using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                wk = new XSSFWorkbook(fs);
                fs.Close();
            }
            ISheet mySheet = wk.GetSheetAt(0);
            int rowNum = mySheet.LastRowNum;
            for (int i = 0; i <= rowNum; i++)
            {
                IRow row = mySheet.GetRow(i);
                NPOI.SS.UserModel.ICell cell0 = row.GetCell(1);//数据项
                NPOI.SS.UserModel.ICell cell = row.GetCell(3);//项目内容
                string value;
                if (cell == null)
                {
                    value = " ";
                }
                else
                {
                    value = cell.ToString();
                }
                try
                {
                    dict[cell0.ToString()] = value;
                }
                catch
                {
                }
            }
        }
        private void setBorderStyle(XSSFCellStyle cellstyle, NPOI.SS.UserModel.ICell targetCell)
        {
            cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            targetCell.CellStyle = cellstyle;

        }
        private XSSFCellStyle setExcelFormat(XSSFCellStyle cellstyle, int ifProjected, string ID, XSSFWorkbook xWookBook)
        {
            XSSFDataFormat format = (XSSFDataFormat)xWookBook.CreateDataFormat();
            if (ifProjected == 1)
            {
                switch (ID)
                {
                    case "左上角点纬度（纵坐标）":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "左上角点经度（横坐标）":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "正射影像地面分辨率":
                        cellstyle.DataFormat = format.GetFormat("0.0");
                        break;
                    case "右下角点纬度（纵坐标）":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "右下角点经度（横坐标）":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "数据量":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "原始影像地面分辨率":
                        cellstyle.DataFormat = format.GetFormat("0.0");
                        break;
                    case "更新区域平面中误差":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "更新影像地面分辨率":
                        cellstyle.DataFormat = format.GetFormat("0.0");
                        break;
                    case "高程中误差":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "起始格网点经度":
                        cellstyle.DataFormat = format.GetFormat("0.000000");
                        break;
                    case "起始格网点纬度":
                        cellstyle.DataFormat = format.GetFormat("0.000000");
                        break;
                    case "起始格网点横坐标":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "起始格网点纵坐标":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "更新区域高程中误差":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "平面位置中误差":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "规则DEM格网间距":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    default:
                        cellstyle.DataFormat = format.GetFormat("@");
                        break;

                }
            }
            else
            {
                switch (ID)
                {
                    case "左上角点纬度（纵坐标）":
                        cellstyle.DataFormat = format.GetFormat("0.000000");
                        break;
                    case "左上角点经度（横坐标）":
                        cellstyle.DataFormat = format.GetFormat("0.000000");
                        break;
                    case "正射影像地面分辨率":
                        cellstyle.DataFormat = format.GetFormat("0.0");
                        break;
                    case "右下角点纬度（纵坐标）":
                        cellstyle.DataFormat = format.GetFormat("0.000000");
                        break;
                    case "右下角点经度（横坐标）":
                        cellstyle.DataFormat = format.GetFormat("0.000000");
                        break;
                    case "数据量":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "原始影像地面分辨率":
                        cellstyle.DataFormat = format.GetFormat("0.0");
                        break;
                    case "更新区域平面中误差":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "更新影像地面分辨率":
                        cellstyle.DataFormat = format.GetFormat("0.0");
                        break;
                    case "高程中误差":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "起始格网点经度":
                        cellstyle.DataFormat = format.GetFormat("0.000000");
                        break;
                    case "起始格网点纬度":
                        cellstyle.DataFormat = format.GetFormat("0.000000");
                        break;
                    case "起始格网点横坐标":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "起始格网点纵坐标":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "更新区域高程中误差":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "平面位置中误差":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    case "规则DEM格网间距":
                        cellstyle.DataFormat = format.GetFormat("0.00");
                        break;
                    default:
                        cellstyle.DataFormat = format.GetFormat("@");
                        break;
                }

            }
            return cellstyle;

        }
        /// <summary>
        /// 输出excel
        /// </summary>
        /// <param name="targetExcel"></param>
        private void writeExcel(string targetExcel, int ifProjected)
        {
            using (FileStream fs = new FileStream(targetExcel, FileMode.Open, FileAccess.ReadWrite))
            {
                if (this.modePath.IndexOf(".xlsx") > 0)
                {
                    XSSFWorkbook xWookBook = new XSSFWorkbook(fs);
                    XSSFSheet targetSheet = (XSSFSheet)xWookBook.GetSheetAt(0);
                    int targetRowNum = targetSheet.LastRowNum;
                    for (int i = 0; i <= targetRowNum; i++)
                    {
                        IRow targetRow = targetSheet.GetRow(i);
                        NPOI.SS.UserModel.ICell targetCell = targetRow.GetCell(1);
                        string ID = targetCell.ToString();
                        string contens = null;
                        double contens_double;
                        bool isHave = dict.TryGetValue(ID, out contens);
                        contens = dict[ID];


                        //设置边框
                        // XSSFCellStyle cellstyle = (XSSFCellStyle)xWookBook.CreateCellStyle();
                        //setBorderStyle(cellstyle, targetCell);                        
                        if (isHave)
                        {

                            if (doubleContents.Contains(ID))
                            {
                                XSSFCellStyle cellstyle = (XSSFCellStyle)xWookBook.CreateCellStyle();
                                //XSSFDataFormat format = (XSSFDataFormat)xWookBook.CreateDataFormat();
                                if (ID == "正射影像地面分辨率" & ifProjected != 1)
                                {
                                    targetCell = targetRow.CreateCell(3);
                                    targetCell.SetCellValue(contens);
                                }

                                if (ID == "原始影像获取时间")
                                {
                                    ICellStyle style = xWookBook.CreateCellStyle();
                                    style.DataFormat = xWookBook.CreateDataFormat().GetFormat("@"); // 设置为文本格式
                                    targetCell = targetRow.CreateCell(3);
                                    targetCell.SetCellValue(contens);
                                }

                                else
                                {
                                    bool contens_double_bool = double.TryParse(dict[ID], out contens_double);
                                    targetCell = targetRow.CreateCell(3);
                                    targetCell.SetCellValue(contens_double);
                                }
                                cellstyle = setExcelFormat(cellstyle, ifProjected, ID, xWookBook);
                                cellstyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                                setBorderStyle(cellstyle, targetCell);

                            }
                            else
                            {
                                targetCell = targetRow.CreateCell(3);
                                targetCell.SetCellValue(contens);
                                XSSFCellStyle cellstyle = (XSSFCellStyle)xWookBook.CreateCellStyle();

                                cellstyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                                setBorderStyle(cellstyle, targetCell);
                            }
                        }
                        else
                        {
                            string warming = string.Format("请确定固定项文件[regular.xlsx]中存在[{0}]项，如果确定存在请联系开发人员", targetCell.ToString());
                            MessageBox.Show(warming);
                            Environment.Exit(0);
                        }
                    }
                    FileStream writer = File.Open(targetExcel, FileMode.Create, FileAccess.Write);
                    xWookBook.Write(writer);
                    writer.Close();
                    fs.Close();
                }
                else
                {
                    MessageBox.Show("请确定输入模板的格式是否为xlsx格式！！！");
                }
            }
        }
        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="exceptions"></param>
        private void writeLogs(string exceptions)
        {
            string filePath = Path.GetDirectoryName(logsPath);
            if (!Directory.Exists(filePath))
            {
                try
                {
                    Directory.CreateDirectory(filePath);
                }
                catch
                {

                }
            }
            StreamWriter logsWriter = new StreamWriter(this.logsPath, true, Encoding.Default);
            logsWriter.WriteLine(exceptions);
            logsWriter.Close();
        }
        /// <summary>
        /// 经纬度转高斯
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Mlati"></param>
        /// <param name="DH"></param>
        /// <param name="ifHaveDH"></param>
        /// <returns></returns>
        private double[] geoToGauss(double x, double y, double Mlati, int DH, bool ifHaveDH)
        {
            InitEllipsoid ed = new InitEllipsoid();
            double longAxlex = ed.getCgcs2000().getA();
            double flatRate = ed.getCgcs2000().getF();
            AlgorithmGroup ag = new AlgorithmGroup();
            double[] darr = ag.GeoToGauss2(x, y, longAxlex, flatRate, Mlati * 3.1415926535897931 / 180.0, DH);
            if (!ifHaveDH)
            {
                darr[1] = darr[1] - (double)(DH * 1000000);
            }
            return darr;
        }

        /// <summary>
        /// 高斯转经纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="longtitude"></param>
        /// <returns></returns>
        private double[] gaussToGeo(double x, double y, double longtitude)
        {
            InitEllipsoid ed = new InitEllipsoid();
            double longAxlex = ed.getCgcs2000().getA();
            double flatRate = ed.getCgcs2000().getF();
            AlgorithmGroup ag = new AlgorithmGroup();
            return ag.GaussToGeo(x, y, longAxlex, flatRate, longtitude, false);
        }
        /// <summary>
        /// DEM数据处理
        /// </summary>
        /// <param name="outExcel"></param>
        /// <param name="datasPath"></param>
        private void DEM(string outExcel, string datasPath)
        {
            string[] DEMfile = Directory.GetFiles(datasPath, "*.grd", SearchOption.AllDirectories);
            string outTxtPath = null;
            if (DEMfile.Length == 0)
            {
                MessageBox.Show("文件夹下不存在grd文件！！！");
            }
            else
            {
                Gdal.AllRegister();
                this.Invoke_performProgressStep();
                this.Invoke_setProgressMax(DEMfile.Length);
                for (int i = 0; i < DEMfile.Length; i++)
                {
                    string fileNameWithoutExtion = Path.GetFileNameWithoutExtension(DEMfile[i]);
                    string outExcelPath = outExcel + "//" + fileNameWithoutExtion + "_YSJ.xlsx";

                    //是否输出txt格式的元数据
                    if (checkBox_DEM_txt.Checked)
                    {
                        outTxtPath = outExcel + "//" + fileNameWithoutExtion + "_YSJ.SMS";
                        readExcelMode(modePath);
                    }

                    string dataPath = DEMfile[i];

                    if (File.Exists(outExcelPath))
                    {
                        File.Delete(outExcelPath);
                    }
                    File.Copy(this.modePath, outExcelPath);
                    if (!File.Exists(outExcelPath))
                    {
                        string exceptions = string.Format("DEM模块：老大，不存在目标文件{0}，可能是拷贝DEM_mode文件不成功", outExcelPath);
                        MessageBox.Show(exceptions);
                        this.writeLogs(exceptions);
                    }
                    else
                    {
                        Dataset dataSet = Gdal.Open(dataPath, Access.GA_ReadOnly);
                        if (dataSet == null)
                        {
                            string exceptions = string.Format("DEM模块：老大，DEM影像{0}，打开失败", DEMfile[i]);
                            this.writeLogs(exceptions);
                        }
                        else
                        {
                            int imageH = dataSet.RasterYSize;
                            int imageW = dataSet.RasterXSize;
                            double[] transform = new double[6];
                            dataSet.GetGeoTransform(transform);
                            string proj = dataSet.GetProjectionRef();
                            SpatialReference osrs = new SpatialReference(proj);
                            int ifProject = osrs.IsProjected();
                            if (transform[0] > 180.0)
                            {
                                ifProject = 1;
                            }
                            else
                            {
                                ifProject = 0;
                            }
                            if (ifProject == 1)
                            {
                                if (string.Empty == skinTextBox_DEM_midLat.Text)
                                {
                                    MessageBox.Show("DEM模块:如果输入的是高斯平面坐标成果，那么中央经线不能为空！！！");
                                    writeLogs(string.Format("{0} 如果输入的是高斯平面坐标成果，那么中央经线不能为空！！", DEMfile[i]));
                                    break;
                                }
                            }
                            double longtitude;
                            bool flag = double.TryParse(this.skinTextBox_DEM_midLat.Text, out longtitude);
                            double mLongti;
                            int DH;
                            if (ifProject == 1)
                            {
                                regularPath = Environment.CurrentDirectory + "\\模版\\DEM_regular_Guass.xlsx";
                                readExcelRegular(regularPath);
                                //要计算做下角坐标为起始格网点
                                double leftUpY = transform[0] + 0.0 * transform[1] + 0.0 * transform[2];
                                double leftUpX = transform[3] + 0.0 * transform[4] + 0.0 * transform[5];

                                double leftDownY = leftUpY;
                                double leftDownX = transform[3] + (double)imageW * transform[4] + (double)imageH * transform[5];


                                mLongti = longtitude;
                                DH = (int)mLongti / 3;
                                double[] leftDown_geo = this.gaussToGeo(leftDownX, leftDownY, mLongti);


                                dict["起始格网点经度"] = string.Format("{0:####.######}", leftDown_geo[0]);
                                dict["起始格网点纬度"] = string.Format("{0:####.######}", leftDown_geo[1]);
                                dict["起始格网点横坐标"] = string.Format("{0:F}", leftDownY);
                                dict["起始格网点纵坐标"] = string.Format("{0:F}", leftDownX);
                            }
                            else
                            {
                                regularPath = Environment.CurrentDirectory + "\\模版\\DEM_regular_Geo.xlsx";
                                readExcelRegular(this.regularPath);

                                double leftUpY = transform[0] + 0.0 * transform[1] + 0.0 * transform[2];//经度
                                double leftUpX = transform[3] + 0.0 * transform[4] + 0.0 * transform[5];//纬度

                                double leftDownY = leftUpY;
                                double leftDownX = transform[3] + (double)imageW * transform[4] + (double)imageH * transform[5];

                                DH = (int)Math.Floor((leftUpY - 1.5) / 3.0) + 1;
                                mLongti = (double)(3 * DH);
                                double[] leftDown_Guass = this.geoToGauss(leftDownY, leftDownX, mLongti, DH, false);

                                dict["起始格网点经度"] = string.Format("{0:####.######}", leftDownY);
                                dict["起始格网点纬度"] = string.Format("{0:####.######}", leftDownX);
                                dict["起始格网点横坐标"] = string.Format("{0:F}", leftDown_Guass[1]);
                                dict["起始格网点纵坐标"] = string.Format("{0:F}", leftDown_Guass[0]);
                            }
                            dict["图号"] = fileNameWithoutExtion.Split('_')[0];
                            //dict["中央经线"] = mLongti.ToString();
                            //dict["投影带号"] = DH.ToString();
                            dict["格网行数"] = imageH.ToString();
                            dict["格网列数"] = imageW.ToString();

                            FileInfo data = new FileInfo(dataPath);

                            double length = (double)data.Length / 1048576.0;
                            string strLength = length.ToString("f3");
                            int strCount = strLength.LastIndexOf('.');
                            string lengthNum = strLength.Substring(0, strCount + 3);
                            //string lengthNum = length.ToString().Substring(0, 4);
                            //string lengthNum = string.Format("{0:2}", length.ToString());
                            dict["数据量"] = lengthNum;

                            //是否接边
                            //是否接边
                            if (skinTextBox_DEM_shp.Text != string.Empty)
                            {
                                bool rightIfLink = false;
                                bool leftIfLink = false;
                                bool upIfLink = false;
                                bool downIfLink = false;
                                string shpPath = skinTextBox_DEM_shp.Text;//获得结合表路径
                                string fieldName = comboBox_DEM_getField.SelectedItem.ToString();//获得属性表字段名称

                                boader(fileNameWithoutExtion, fieldName, shpPath, ifProject, out upIfLink, out downIfLink, out leftIfLink, out rightIfLink);

                                if (upIfLink)
                                {
                                    dict["北边接边情况"] = "已接";
                                }
                                else
                                {
                                    dict["北边接边情况"] = "自由边";
                                }

                                if (downIfLink)
                                {
                                    dict["南边接边情况"] = "已接";
                                }
                                else
                                {
                                    dict["南边接边情况"] = "自由边";
                                }

                                if (rightIfLink)
                                {
                                    dict["东边接边情况"] = "已接";
                                }
                                else
                                {
                                    dict["东边接边情况"] = "自由边";
                                }

                                if (leftIfLink)
                                {
                                    dict["西边接边情况"] = "已接";
                                }
                                else
                                {
                                    dict["西边接边情况"] = "自由边";
                                }
                            }

                            this.writeExcel(outExcelPath, ifProject);

                            //是否输出txt格式文件
                            if (checkBox_DEM_txt.Checked)
                            {
                                writeTxtMetadata(outTxtPath, ifProject);

                            }

                            this.Invoke_setProgressValue(i);
                            dict.Remove("起始格网点纬度");
                            dict.Remove("起始格网点纬度");
                            dict.Remove("起始格网点横坐标");
                            dict.Remove("起始格网点纵坐标");
                            dict.Remove("图号");
                            //dict.Remove("中央经线");
                            //dict.Remove("投影带号");
                            dict.Remove("数据量");
                            dataSet.Dispose();
                        }
                    }
                }
            }

        }
        private void writeTxtMetadata(string outTxt, int ifproj)
        {
            string contents = null;
            StreamWriter writer = new StreamWriter(outTxt, false, Encoding.Default);
            try
            {

                //按照顺序读取模板的内容项
                for (int i = 1; i <= modeContents.Count(); i++)
                {
                    contents = i.ToString().PadLeft(2, ' ') + "     " + modeContents[i - 1].PadRight(20, ' ') + dict[modeContents[i - 1]].PadRight(25, ' ');
                    writer.WriteLine(contents);
                }
            }
            catch
            {

            }
            writer.Close();
        }
        private void boader(string tuhao, string fieldName, string shpPath, int ifProject, out bool upIfLink, out bool downIfLink, out bool leftIfLink, out bool rightIfLink)
        {
            rightIfLink = false;
            leftIfLink = false;
            upIfLink = false;
            downIfLink = false;
            Ogr.RegisterAll();
            DataSource ds = Ogr.Open(shpPath, 0);
            Layer layer = ds.GetLayerByIndex(0);

            if (ds == null)
            {
                DialogResult result = MessageBox.Show("判断是否接边矢量数据打开失败！！！", "警告", MessageBoxButtons.YesNo);
                if (result == DialogResult.OK)
                {

                }
                else
                {
                    return;
                }
            }

            Feature feature = layer.GetNextFeature();
            //先找到对应元数据相对应的要素矢量
            Geometry geo = null;
            double geoMaxX = 0;
            double geoMaxY = 0;
            double geoMinX = 0;
            double geoMinY = 0;
            Envelope geoEnvelope = new Envelope();
            Envelope linkEnvelope = new Envelope();
            while (feature != null)
            {
                string tuhaoField = feature.GetFieldAsString(fieldName);
                if (tuhaoField == tuhao)
                {
                    //获得相应的要素范围

                    geo = feature.GetGeometryRef();

                    geo.GetEnvelope(geoEnvelope);

                    geoMaxX = Math.Round(geoEnvelope.MaxX, 5);
                    geoMaxY = Math.Round(geoEnvelope.MaxY, 5);
                    geoMinX = Math.Round(geoEnvelope.MinX, 5);
                    geoMinY = Math.Round(geoEnvelope.MinY, 5);
                    break;
                }
                feature = layer.GetNextFeature();
            }
            if (geoMaxX == 0 & geoMaxY == 0 & geoMinX == 0 & geoMinY == 0)
            {
                string contents = string.Format("结合表中缺少 {0} 图廓线,是否继续？", tuhao);
                DialogResult DR = MessageBox.Show(contents, "警告", MessageBoxButtons.YesNo);
                if (DR == DialogResult.OK)
                {

                }
                else
                {
                    return;
                }

            }
            //寻找与图号要素相交的图框要素
            layer.ResetReading();
            feature = layer.GetNextFeature();

            while (feature != null)
            {
                string tuhaoField = feature.GetFieldAsString(fieldName);
                //if (tuhaoField == tuhao) continue;//跳过相同的要素
                Geometry linkGeo = feature.GetGeometryRef();

                feature = layer.GetNextFeature();
                if (tuhaoField == tuhao) continue;//跳过相同的要素                
                bool is_linked = geo.Intersects(linkGeo);
                if (is_linked)
                {
                    linkGeo.GetEnvelope(linkEnvelope);
                    //获得范围坐标
                    double linkMinX = Math.Round(linkEnvelope.MinX, 5);
                    double linkMinY = Math.Round(linkEnvelope.MinY, 5);
                    double linkMaxX = Math.Round(linkEnvelope.MaxX, 5);
                    double linkMaxY = Math.Round(linkEnvelope.MaxY, 5);
                    //判断相交图框在目标图框的哪一侧
                    //在目标图框的上下

                    if (ifProject == 1)//高斯坐标要求小于200的阈值
                    {
                        if (Math.Abs(linkMinX - geoMinX) < 200 && Math.Abs(linkMaxX - geoMaxX) < 200)
                        {
                            //在上边
                            if (linkMinY > geoMinY & linkMaxY > geoMaxY)
                            {
                                upIfLink = true;
                            }
                            //在下边
                            if (linkMinY < geoMinY & linkMaxY < geoMaxY)
                            {
                                downIfLink = true;
                            }
                        }

                        //在目标图框的左右
                        if (Math.Abs(linkMinY - geoMinY) < 200 && Math.Abs(linkMaxY - geoMaxY) < 200)
                        {
                            //左侧
                            if (linkMinX < geoMinX & linkMaxX < geoMaxX)
                            {
                                leftIfLink = true;
                            }
                            //在右侧
                            if (linkMinX > geoMinX & linkMaxX > geoMaxX)
                            {
                                rightIfLink = true;
                            }
                        }

                    }
                    else //地理坐标要求相等
                    {
                        if (linkMinX == geoMinX && linkMaxX == geoMaxX)
                        {
                            //在上边
                            if (linkMinY > geoMinY & linkMaxY > geoMaxY)
                            {
                                upIfLink = true;
                            }
                            //在下边
                            if (linkMinY < geoMinY & linkMaxY < geoMaxY)
                            {
                                downIfLink = true;
                            }
                        }

                        //在目标图框的左右
                        if (linkMinY == geoMinY && linkMaxY == geoMaxY)
                        {
                            //左侧
                            if (linkMinX < geoMinX & linkMaxX < geoMaxX)
                            {
                                leftIfLink = true;
                            }
                            //在右侧
                            if (linkMinX > geoMinX & linkMaxX > geoMaxX)
                            {
                                rightIfLink = true;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// DOM数据处理
        /// </summary>
        private void DOM(string outExcel, string datasPath, string shpFile, string mosaicFile, string inputExcel)
        {
            string[] DOMfile = Directory.GetFiles(datasPath, "*.tif", SearchOption.AllDirectories);
            string outTxtPath = null;
            if (DOMfile.Length == 0)
            {
                string error = string.Format("{0}文件夹下不存在tif文件！！！", datasPath);
                MessageBox.Show(error);
            }
            else
            {
                //开始计算 计算项
                Invoke_performProgressStep();
                Invoke_setProgressMax(DOMfile.Length);
                // 为了支持中文路径，请添加下面这句代码
                OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
                // 为了使属性表字段支持中文，请添加下面这句
                OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "");
                // 注册所有的驱动
                Ogr.RegisterAll();

                for (int j = 0; j < DOMfile.Length; j++)
                {
                    string fileNameWithoutExtion = Path.GetFileNameWithoutExtension(DOMfile[j]);
                    string outExcelPath = outExcel + "//" + fileNameWithoutExtion + "_YSJ.xlsx";
                    string dataPath = DOMfile[j];
                    //是否输出txt格式的元数据
                    if (checkBox_DOM_txt.Checked)
                    {
                        outTxtPath = outExcel + "//" + fileNameWithoutExtion + "_YSJ.SMS";
                        readExcelMode(modePath);
                    }

                    //默认输出Excel

                    if (File.Exists(outExcelPath))
                    {
                        File.Delete(outExcelPath);
                    }
                    File.Copy(this.modePath, outExcelPath);
                    if (!File.Exists(outExcelPath))
                    {
                        string exceptions = string.Format("DOM模块：老大，不存在目标文件{0}，可能是拷贝DOM_mode文件不成功", outExcelPath);
                        MessageBox.Show(exceptions);
                        this.writeLogs(exceptions);
                    }
                    else
                    {
                        Dataset dataSet = Gdal.Open(dataPath, Access.GA_ReadOnly);

                        if (dataSet == null)
                        {
                            string exceptions = string.Format("DOM模块：老大，DOM影像{0}，打开失败", DOMfile[j]);
                            this.writeLogs(exceptions);
                        }
                        else
                        {
                            double[] transform = new double[6];
                            dataSet.GetGeoTransform(transform);
                            string proj = dataSet.GetProjection();
                            SpatialReference osrs = new SpatialReference(proj);
                            int ifProject = osrs.IsProjected();
                            if (ifProject == 1)
                            {
                                //如果投影的就读取DOM_regular_Guass.xlsx
                                regularPath = Environment.CurrentDirectory + "\\模版\\DOM_regular_Guass.xlsx";
                                readExcelRegular(this.regularPath);
                            }
                            else
                            {
                                //如果经纬度就读取DOM_regular_Geo.xlsx
                                regularPath = Environment.CurrentDirectory + "\\模版\\DOM_regular_Geo.xlsx";
                                readExcelRegular(this.regularPath);
                            }
                            FileInfo data = new FileInfo(dataPath);
                            double length = (double)data.Length / 1048576.0;
                            string strLength = length.ToString("f3");
                            int strCount = strLength.LastIndexOf('.');
                            string lengthNum = strLength.Substring(0, strCount + 3);
                            //string lengthNum = string.Format("{0:F}", length);
                            dict["数据量"] = lengthNum;
                            int imageH = dataSet.RasterXSize;
                            int imageW = dataSet.RasterYSize;
                            dict["像素行数"] = imageW.ToString();
                            dict["像素列数"] = imageH.ToString();

                            string skinTextbox_DOM_midLat = skinTextBox_DOM_midLat.Text;
                            Console.WriteLine("中央经线:" + skinTextbox_DOM_midLat);

                            if (ifProject == 1)
                            {
                                if (string.Empty == this.skinTextBox_DOM_midLat.Text)
                                {
                                    //string ex= string.Format("DOM模块:如果输入的是高斯平面坐标成果，那么中央经线不能为空！！！", DOMfile[j]);
                                    //this.writeLogs(ex);
                                    MessageBox.Show("DOM模块:如果输入的是高斯平面坐标成果，那么中央经线不能为空！！！");
                                    break;
                                }
                            }
                            double longtitude;
                            bool flag = double.TryParse(this.skinTextBox_DOM_midLat.Text, out longtitude);
                            int DH;
                            int mLongti;
                            if (ifProject == 1)
                            {
                                double leftUpY = transform[0] + 0.5 * transform[1] + 0.0 * transform[2];
                                double leftUpX = transform[3] + 0.0 * transform[4] + 0.5 * transform[5];
                                double rightDownY = leftUpY + (double)(imageH - 1) * transform[1] + (double)(imageW - 1) * transform[2];
                                double rightDownX = leftUpX + (double)(imageH - 1) * transform[4] + (double)(imageW - 1) * transform[5];
                                //double[] rightDown = this.gaussToGeo(rightDownX, rightDownY, longtitude);
                                double[] leftUp = this.gaussToGeo(leftUpX, leftUpY, longtitude);
                                //DH = (int)Math.Floor((leftUp[0] - 1.5) / 3.0) + 1;
                                int skinTextboxDOMMidlat = Convert.ToInt32(skinTextbox_DOM_midLat);
                                if (comboBoxFDH.SelectedIndex == 0)
                                {
                                    DH = skinTextboxDOMMidlat / 3;
                                }
                                else
                                {
                                    DH = (skinTextboxDOMMidlat + 3) / 6;
                                }
                                mLongti = (int)longtitude;
                                dict["左上角点纬度（纵坐标）"] = string.Format("{0:####.##}", leftUpX);
                                dict["左上角点经度（横坐标）"] = string.Format("{0:####.##}", leftUpY);
                                dict["右下角点纬度（纵坐标）"] = string.Format("{0:####.##}", rightDownX);
                                dict["右下角点经度（横坐标）"] = string.Format("{0:####.##}", rightDownY);
                                dict["中央经线"] = mLongti.ToString();
                                dict["投影带号"] = DH.ToString();
                            }
                            else
                            {
                                double leftUpY = transform[0] + 0.5 * transform[1] + 0.0 * transform[2];
                                double leftUpX = transform[3] + 0.0 * transform[4] + 0.5 * transform[5];
                                DH = (int)Math.Floor((leftUpY - 1.5) / 3.0) + 1;
                                mLongti = 3 * DH;
                                double rightDownY = leftUpY + (double)(imageH - 1) * transform[1] + (double)(imageW - 1) * transform[2];
                                double rightDownX = leftUpX + (double)(imageH - 1) * transform[4] + (double)(imageW - 1) * transform[5];
                                dict["左上角点纬度（纵坐标）"] = string.Format("{0:####.######}", leftUpX);
                                dict["左上角点经度（横坐标）"] = string.Format("{0:####.######}", leftUpY);
                                dict["右下角点纬度（纵坐标）"] = string.Format("{0:####.######}", rightDownX);
                                dict["右下角点经度（横坐标）"] = string.Format("{0:####.######}", rightDownY);
                                dict["坐标单位"] = "度";
                            }
                            dict["图号"] = fileNameWithoutExtion.Split('_')[0];

                            #region 获取数据的获取时间
                            DataSource shpdr = Ogr.Open(shpFile, 0);
                            DataSource mosaicdr = Ogr.Open(mosaicFile, 0);
                            if (shpdr == null || mosaicdr == null)
                            {
                                MessageBox.Show("无法打开结合表和镶嵌线！");
                                return;
                            }
                            //属性值
                            string targetField = "NAME";
                            string targetValue = fileNameWithoutExtion;
                            //建空字符串存放目标值
                            string targetTime = string.Empty;
                            string targetSensor = string.Empty;
                            //获取图层
                            Layer shpLayer = shpdr.GetLayerByIndex(0);
                            Layer mosaicLayer = mosaicdr.GetLayerByIndex(0);
                            if (shpLayer == null || mosaicLayer == null)
                            {
                                MessageBox.Show("无法获取结合表和镶嵌线的图层！");
                                return;
                            }
                            //获取结合表的指定要素
                            string strCom = string.Format("{0}='{1}'", targetField, targetValue);
                            shpLayer.SetAttributeFilter(strCom);
                            // shpLayer.SetAttributeFilter($"{targetField} = '{targetValue}'");
                            Feature feature = shpLayer.GetNextFeature();
                            if (feature != null)
                            {
                                Geometry shpgeom = feature.GetGeometryRef();
                                Feature mosaicfeature;
                                mosaicLayer.ResetReading();
                                while ((mosaicfeature = mosaicLayer.GetNextFeature()) != null)
                                {
                                    //mosaicLayer.ResetReading();
                                    //获取要素几何体
                                    Geometry mosaicgeom = mosaicfeature.GetGeometryRef();
                                    //是否相交
                                    bool is_Intersected = shpgeom.Intersects(mosaicgeom);
                                    if (is_Intersected)
                                    {
                                        int fieldIndex1 = mosaicfeature.GetFieldIndex("time");
                                        string time = mosaicfeature.GetFieldAsString(fieldIndex1);
                                        targetTime = targetTime + time + "、";
                                    }
                                }
                            }
                            targetTime = targetTime.TrimEnd('、');
                            // 使用顿号分割字符串
                            string[] parts = targetTime.Split('、');
                            // 将字符串数组转换为整数数组，并去重、排序
                            var uniqueSortedNumbers = parts.Select(int.Parse) // 将字符串转换为整数
                                                           .Distinct()        // 去除重复项
                                                           .OrderBy(x => x)   // 按数字升序排列
                                                           .ToArray();
                            // 将去重后的字符串重新组合成一个字符串                         
                            string timeResult = string.Join("、", uniqueSortedNumbers);
                            dict["原始影像获取时间"] = timeResult;
                            #endregion

                            #region 获取中误差
                            Dictionary<string, string> excelData = new Dictionary<string, string>();
                            try
                            {
                                if (!File.Exists(inputExcel))
                                {
                                    throw new FileNotFoundException("Excel文件不存在", inputExcel);
                                }
                                else
                                {
                                    using (FileStream sourceStream = new FileStream(inputExcel, FileMode.Open, FileAccess.Read))
                                    {
                                        IWorkbook workbook;
                                        if (inputExcel.EndsWith(".xlsx"))
                                        {
                                            workbook = new XSSFWorkbook(sourceStream);
                                        }
                                        else if (inputExcel.EndsWith(".xls"))
                                        {
                                            workbook = new HSSFWorkbook(sourceStream);
                                        }
                                        else
                                        {
                                            throw new NotSupportedException("Unsupported file format");
                                        }

                                        ISheet sheet = workbook.GetSheetAt(0);

                                        // 遍历行和列，将数据存入字典
                                        for (int i = 0; i <= sheet.LastRowNum; i++)
                                        {
                                            IRow row = sheet.GetRow(i);  //读取当前行数据
                                            if (row != null)
                                            {
                                                NPOI.SS.UserModel.ICell keyCell = row.GetCell(0);  //第一列是Key
                                                NPOI.SS.UserModel.ICell valueCell = row.GetCell(1);//第二列是Value
                                                if (keyCell != null && valueCell != null)
                                                {
                                                    string key = keyCell.ToString();
                                                    string value = valueCell.ToString();
                                                    if (!excelData.ContainsKey(key))
                                                    {
                                                        excelData.Add(key, value);
                                                    }
                                                    else
                                                    {
                                                        //  MessageBox.Show($"Duplicate key found: {key}, skipping...");
                                                    }
                                                }

                                            }
                                        }
                                        //检查键是否存在当前图号
                                        if (excelData.ContainsKey(fileNameWithoutExtion))
                                        {
                                            dict["平面位置中误差"] = excelData[fileNameWithoutExtion];
                                        }
                                        else
                                        {
                                            this.writeLogs("Key 'Data' not found in the dictionary.");
                                        }
                                    }
                                }
                            }
                            catch (FileNotFoundException ex)
                            {
                                // 捕获文件不存在的异常并处理
                                this.writeLogs(ex.Message);
                            }
                            catch (NotSupportedException ex)
                            {
                                this.writeLogs(ex.Message);
                            }

                            #endregion

                            //是否接边
                            if (skinTextBox_DOM_shp.Text != string.Empty)
                            {
                                bool rightIfLink = false;
                                bool leftIfLink = false;
                                bool upIfLink = false;
                                bool downIfLink = false;
                                string shpPath = skinTextBox_DOM_shp.Text;//获得结合表路径
                                string fieldName = comboBox_DOM_getField.SelectedItem.ToString();//获得属性表字段名称

                                boader(fileNameWithoutExtion, fieldName, shpPath, ifProject, out upIfLink, out downIfLink, out leftIfLink, out rightIfLink);

                                if (upIfLink)
                                {
                                    dict["北边接边情况"] = "已接";
                                }
                                else
                                {
                                    dict["北边接边情况"] = "自由";
                                }

                                if (downIfLink)
                                {
                                    dict["南边接边情况"] = "已接";
                                }
                                else
                                {
                                    dict["南边接边情况"] = "自由";
                                }

                                if (rightIfLink)
                                {
                                    dict["东边接边情况"] = "已接";
                                }
                                else
                                {
                                    dict["东边接边情况"] = "自由";
                                }

                                if (leftIfLink)
                                {
                                    dict["西边接边情况"] = "已接";
                                }
                                else
                                {
                                    dict["西边接边情况"] = "自由";
                                }
                            }

                            writeExcel(outExcelPath, ifProject);

                            //是否输出txt格式文件
                            if (checkBox_DOM_txt.Checked)
                            {
                                writeTxtMetadata(outTxtPath, ifProject);

                            }
                            Invoke_setProgressValue(j + 1);
                            dict.Remove("左上角点纵坐标");
                            dict.Remove("左上角点横坐标");
                            dict.Remove("右下角点纵坐标");
                            dict.Remove("右下角点横坐标");
                            dict.Remove("图号");
                            if (1 == ifProject)
                            {
                                dict.Remove("中央经线");
                                dict.Remove("投影带号");
                            }
                            dict.Remove("数据量");
                            dict.Remove("像素行数");
                            dict.Remove("像素列数");
                            dataSet.Dispose();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 判断路径
        /// </summary>
        /// <returns></returns>
        private bool CheckParamsVaild()
        {
            if (this.skinTextBoxDOM_dataPath.Text == "")
            {
                MessageBox.Show("请选择数据路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.skinButton1_DOM.Enabled = true;
                this.Butoon_DOM_dataPath.Enabled = true;
                this.skinButton_DOM_outPath.Enabled = true;
                this.skinButton_DOM_openshp.Enabled = true;
                this.skinButton_DOM_mosaicshp.Enabled = true;
                return false;
            }
            if (this.skinTextBoxDOM_dataPath.Text.IndexOf(" ") >= 0)
            {
                MessageBox.Show("请设置数据路径不存在空格", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.skinButton1_DOM.Enabled = true;
                this.Butoon_DOM_dataPath.Enabled = true;
                this.skinButton_DOM_outPath.Enabled = true;
                this.skinButton_DOM_openshp.Enabled = true;
                this.skinButton_DOM_mosaicshp.Enabled = true;
                return false;
            }
            if (this.skinTextBoxDOM_outPath.Text == "")
            {
                MessageBox.Show("请选择输出路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.skinButton1_DOM.Enabled = true;
                this.Butoon_DOM_dataPath.Enabled = true;
                this.skinButton_DOM_outPath.Enabled = true;
                this.skinButton_DOM_openshp.Enabled = true;
                this.skinButton_DOM_mosaicshp.Enabled = true;
                return false;
            }
            if (this.skinTextBoxDOM_outPath.Text.IndexOf(" ") >= 0)
            {
                MessageBox.Show("请设置输出路径不存在空格", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.skinButton1_DOM.Enabled = true;
                this.Butoon_DOM_dataPath.Enabled = true;
                this.skinButton_DOM_outPath.Enabled = true;
                this.skinButton_DOM_openshp.Enabled = true;
                this.skinButton_DOM_mosaicshp.Enabled = true;
                return false;
            }
            if (this.skinTextBox_DOM_shp.Text == "")
            {
                MessageBox.Show("请选择结合表", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.skinButton1_DOM.Enabled = true;
                this.Butoon_DOM_dataPath.Enabled = true;
                this.skinButton_DOM_outPath.Enabled = true;
                this.skinButton_DOM_openshp.Enabled = true;
                this.skinButton_DOM_mosaicshp.Enabled = true;
                return false;
            }
            if (this.skinTextBox_DOM_shp.Text.IndexOf(" ") >= 0)
            {
                MessageBox.Show("请选择结合表文件路径不存在空格", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.skinButton1_DOM.Enabled = true;
                this.Butoon_DOM_dataPath.Enabled = true;
                this.skinButton_DOM_outPath.Enabled = true;
                this.skinButton_DOM_openshp.Enabled = true;
                this.skinButton_DOM_mosaicshp.Enabled = true;
                return false;
            }
            if (this.skinTextBox_moasicPath.Text == "")
            {
                MessageBox.Show("请选择镶嵌线文件", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.skinButton1_DOM.Enabled = true;
                this.Butoon_DOM_dataPath.Enabled = true;
                this.skinButton_DOM_outPath.Enabled = true;
                this.skinButton_DOM_openshp.Enabled = true;
                this.skinButton_DOM_mosaicshp.Enabled = true;
                return false;
            }
            if (this.skinTextBox_moasicPath.Text.IndexOf(" ") >= 0)
            {
                MessageBox.Show("请选择镶嵌线文件路径不存在空格", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.skinButton1_DOM.Enabled = true;
                this.Butoon_DOM_dataPath.Enabled = true;
                this.skinButton_DOM_outPath.Enabled = true;
                this.skinButton_DOM_openshp.Enabled = true;
                this.skinButton_DOM_mosaicshp.Enabled = true;
                return false;
            }
            return true;
        }
        private void method_DOM()
        {
            string datapath = this.skinTextBoxDOM_dataPath.Text;
            string outExcel = this.skinTextBoxDOM_outPath.Text;
            string shpFile = this.skinTextBox_DOM_shp.Text;
            string mosaicFile = this.skinTextBox_moasicPath.Text;
            string inputExcel = this.skinTextBox_Exl.Text;
            modePath = Environment.CurrentDirectory + "\\模版\\DOM_mode.xlsx";
            //regularPath = Environment.CurrentDirectory + "\\模版\\DOM_regular.xlsx";
            //readExcel(this.regularPath);
            DOM(outExcel, datapath, shpFile, mosaicFile, inputExcel);
            MessageBox.Show("老大,DOM 元数据信息录入完毕！！！");
        }
        private void method_DEM()
        {
            string datapath = this.skinTextBox_DEM_dataPath.Text;
            string outExcel = this.skinTextBox_DEM_outPath.Text;
            modePath = Environment.CurrentDirectory + "\\模版\\DEM_mode.xlsx";

            DEM(outExcel, datapath);
            MessageBox.Show("老大,DEM 元数据信息录入完毕！！！");
        }

        private void Butoon_DOM_dataPath_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaFolderBrowserDialog fileDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();
            //FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.Description = "请选择输入文件夹";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.skinTextBoxDOM_dataPath.Text = fileDialog.SelectedPath;
            }
        }

        private void skinButton_DOM_outPath_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaFolderBrowserDialog fileDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();
            //FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.Description = "请选择输出文件夹";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.skinTextBoxDOM_outPath.Text = fileDialog.SelectedPath;
            }
        }

        private void skinButton_DEM_dataPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.Description = "请选择输入文件夹";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.skinTextBox_DEM_dataPath.Text = fileDialog.SelectedPath;
            }
        }

        private void skinButton_DEM_outPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.Description = "请选择输出文件夹";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.skinTextBox_DEM_outPath.Text = fileDialog.SelectedPath;
            }
        }

        private void skinButton_DEM_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//关闭跨线程检查
            Task task_DEM = new Task(new Action(this.method_DEM));
            task_DEM.Start();
        }

        private void skinButton1_DOM_Click(object sender, EventArgs e)
        {
            this.progressBar.Maximum = 100;
            this.progressBar.Minimum = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            // if (!CheckParamsVaild()) return;
            Task task_DOM = new Task(new Action(this.method_DOM));
            task_DOM.Start();
        }

        private void button_inimgdir_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaFolderBrowserDialog fileDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();
            fileDialog.Description = "请选择输入文件夹";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.skinTextBox_imgdir.Text = fileDialog.SelectedPath;
            }
        }

        private void button_outimgdir_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaFolderBrowserDialog fileDialog = new Ookii.Dialogs.VistaFolderBrowserDialog();
            fileDialog.Description = "请选择输出文件夹";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.skinTextBox_outimgdir.Text = fileDialog.SelectedPath;
            }
        }
        /// <summary>
        /// 新建文件夹功能
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="tuhao"></param>
        /// <returns></returns>
        private List<string> findSameImage(List<string> paths, string tuhao)
        {
            List<string> sameImage = new List<string>();

            for (int i = 0; i < paths.Count; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(paths[i]);
                string name = dir.Name;
                if (name.Contains(tuhao))
                {
                    sameImage.Add(paths[i]);
                }
            }
            return sameImage;

        }
        /// <summary>
        /// 新建文件夹功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_DOM_Click(object sender, EventArgs e)
        {
            string imgDir = skinTextBox_imgdir.Text;
            string outDir = skinTextBox_outimgdir.Text;
            List<string> haveproced = new List<string>();
            if (string.Empty == imgDir || string.Empty == outDir)
            {
                MessageBox.Show("路径不能为空!!!");
                return;
            }

            string[] imgPaths = Directory.GetFiles(imgDir);
            List<string> imgPathss = imgPaths.ToList<string>();
            if (imgPaths.Length == 0)
            {
                MessageBox.Show("没有可用的图幅文件！！！");
                return;
            }
            else
            {
                //便利所有文件进行拷贝
                for (int i = 0; i < imgPaths.Length; i++)
                {
                    DirectoryInfo dir = new DirectoryInfo(imgPaths[i]);
                    string fileName = dir.Name;
                    //判断是否已经拷贝完毕的文件，则跳过
                    int index = fileName.IndexOf('.');
                    if (index != -1)
                    {
                        List<string> sameFiles = new List<string>();
                        if (fileName.Contains("DOM") || fileName.Contains("DEM"))
                        {
                            string tuhao = fileName.Substring(0, index);
                            if (haveproced.Contains(tuhao))
                            {
                                continue;
                            }
                            haveproced.Add(tuhao);
                            //建立文件夹
                            string outPath = outDir + "\\" + tuhao;
                            Directory.CreateDirectory(outPath);
                            //然后把文件拷贝进文件夹
                            List<string> files = findSameImage(imgPathss, tuhao);
                            for (int j = 0; j < files.Count; j++)
                            {
                                string name = Path.GetFileName(files[j]);
                                File.Copy(files[j], Path.Combine(outPath, name), true);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }

                }

            }
            MessageBox.Show("影像数据文件夹建立成功！！");
        }

        private void button_imgdir_DEM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("亲，DOM的工具同样适合您的需求，请移步DOM同款工具，你值得拥有！！！");
            return;

        }

        private void button_inimgdir_DEM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("亲，DOM的工具同样适合您的需求，请移步DOM同款工具，你值得拥有！！！");
            return;
        }

        private void button_outimgdir_DEM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("亲，DOM的工具同样适合您的需求，请移步DOM同款工具，你值得拥有！！！");
            return;
        }
        /// <summary>
        /// 写出docx记录文档
        /// </summary>
        /// <param name="docxPath"></param>
        /// <param name="contents"></param>
        private void writeDocx(string docxPath, string contents)
        {
            FileStream fs = new FileStream(docxPath, FileMode.Create, FileAccess.Write);
            XWPFDocument docx = new XWPFDocument();
            XWPFParagraph paragraph = docx.CreateParagraph();
            paragraph.Alignment = ParagraphAlignment.LEFT;
            XWPFRun run = paragraph.CreateRun();
            run.FontFamily = "宋体";
            run.FontSize = 12;
            run.SetText(contents);
            docx.Write(fs);
            fs.Close();
        }
        /// <summary>
        /// 生成检查记录部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        bool _worklevelClicked = false;
        private void button_doc_dom_Click(object sender, EventArgs e)
        {
            string imgPath = textBox_imgpath_dom_doc.Text;
            string outPath = textBox_outpath_dom_doc.Text;
            string contents = textBox_doc_dom_contents.Text;
            List<string> contentsList = new List<string>();
            string workLevel = comboBox_doc_dom.Text;
            if (_worklevelClicked) workLevel = comboBox_doc_dom.SelectedItem.ToString();
            if (string.Empty == imgPath || string.Empty == outPath)
            {
                MessageBox.Show("路径不能为空!!!");
                return;
            }
            string[] imgPaths = Directory.GetFiles(imgPath, "*.tif", SearchOption.AllDirectories);
            //List<string> imgPathss = imgPaths.ToList<string>();
            if (imgPaths.Length == 0)
            {
                MessageBox.Show("没有可用的图幅文件！！！");
                return;
            }
            else
            {
                //根据影像名称创建检查记录doc
                for (int i = 0; i < imgPaths.Length; i++)
                {
                    DirectoryInfo dir = new DirectoryInfo(imgPaths[i]);
                    string fileName = Path.GetFileNameWithoutExtension(dir.Name).Substring(0, 11);
                    string docxPath = Path.Combine(outPath, fileName + workLevel + ".docx");
                    //创建docx
                    string[] contentsArray = contents.Split('\n');

                    writeDocx(docxPath, contents);
                }
            }
        }
        private void button_imgPath_DOM_doc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.Description = "请选择输入文件夹";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_imgpath_dom_doc.Text = fileDialog.SelectedPath;
            }
        }
        private void button_outpath_dom_doc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.Description = "请选择出文件夹";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_outpath_dom_doc.Text = fileDialog.SelectedPath;
            }
        }
        private void comboBox_doc_dom_MouseClick(object sender, MouseEventArgs e)
        {
            if (comboxItemClickFlag == 1)
            {
                comboBox_doc_dom.Items.Add("一检");
                comboBox_doc_dom.Items.Add("二检");
                comboBox_doc_dom.Items.Add("检验");
                comboxItemClickFlag++;
                _worklevelClicked = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxFDH.Items.Clear();
            comboBoxFDH.Items.AddRange(new string[] { "3", "6" });
            comboBoxFDH.SelectedIndex = 0;
        }

        private void comboBox_DOM_getField_MouseClick(object sender, MouseEventArgs e)
        {
            //遍历字段名称
            if (comboxgetFieldClick == 1)
            {
                if (skinTextBox_DOM_shp.Text != string.Empty)
                {
                    string shpPath = skinTextBox_DOM_shp.Text;
                    Ogr.RegisterAll();
                    DataSource ds = Ogr.Open(shpPath, 0);
                    if (ds == null)
                    {
                        MessageBox.Show("打开结合表矢量数据失败！！！");
                        return;
                    }
                    Layer layer = ds.GetLayerByIndex(0);
                    FeatureDefn featureDefn = layer.GetLayerDefn();
                    int fieldCount = featureDefn.GetFieldCount();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        string fieldName = featureDefn.GetFieldDefn(i).GetName();
                        comboBox_DOM_getField.Items.Add(fieldName);
                    }
                    comboxgetFieldClick++;
                }
            }
        }

        private void skinButton_DOM_openshp_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaOpenFileDialog fileDialog = new Ookii.Dialogs.VistaOpenFileDialog();
            // OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "检查规则文件(*.shp)|*.shp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                skinTextBox_DOM_shp.Text = fileDialog.FileName;
            }
        }
        private void comboBox_DEM_getField_MouseClick(object sender, MouseEventArgs e)
        {
            //遍历字段名称
            if (comboxgetFieldClick == 1)
            {
                if (skinTextBox_DEM_shp.Text != string.Empty)
                {
                    string shpPath = skinTextBox_DEM_shp.Text;
                    Ogr.RegisterAll();
                    DataSource ds = Ogr.Open(shpPath, 0);
                    if (ds == null)
                    {
                        MessageBox.Show("打开结合表矢量数据失败！！！");
                        return;
                    }
                    Layer layer = ds.GetLayerByIndex(0);
                    FeatureDefn featureDefn = layer.GetLayerDefn();
                    int fieldCount = featureDefn.GetFieldCount();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        string fieldName = featureDefn.GetFieldDefn(i).GetName();
                        comboBox_DEM_getField.Items.Add(fieldName);
                    }
                    comboxgetFieldClick++;
                }
            }

        }
        private void skinButton_DEM_openshp_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "检查规则文件(*.shp)|*.shp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                skinTextBox_DEM_shp.Text = fileDialog.FileName;
            }
        }

        private void skinButton_DOM_mosaicshp_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaOpenFileDialog fileDialog = new Ookii.Dialogs.VistaOpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "镶嵌线(*.shp)|*.shp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                skinTextBox_moasicPath.Text = fileDialog.FileName;
            }
        }

        private void skinButton_OpenExl_Click(object sender, EventArgs e)
        {
            Ookii.Dialogs.VistaOpenFileDialog fileDialog = new Ookii.Dialogs.VistaOpenFileDialog();
            //OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "精度检测表(*.xls;*.xlsx)|*.xls;*.xlsx";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                skinTextBox_Exl.Text = fileDialog.FileName;
            }
        }
    }
}
