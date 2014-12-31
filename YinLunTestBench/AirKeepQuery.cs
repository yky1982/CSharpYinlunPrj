using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO;
using AccessDLL__AddBuffer;
using DrawCurve_BaseFunction;
using ExcelLib__Speed;

namespace YinLunTestBench
{
    public partial class AirKeepQuery : Form
    {
        #region AccessDll
        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "Init", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init(string FileName);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "CreatFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CreatFile();

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "AddTable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddTable(string TableName);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "AddColumn", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddColumn(string TableName, string ColName, string Datatype);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "AddNewRow", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddNewRow(string TableName);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "DeleteRow", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DeleteRow(string TableName, int index);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "WriteFloatsData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteFloatsData(string TableName, int index, string ColName, float[] data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "WriteIntsData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteIntsData(string TableName, int index, string ColName, int[] data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "WriteSigleData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteSigleData(string TableName, int index, string ColName, object data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "ReadFloatsData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReadFloatsData(string TableName, int index, string ColName, out float[] data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "ReadIntsData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReadIntsData(string TableName, int index, string ColName, out int[] data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "ReadSigleData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReadSigleData(string TableName, int index, string ColName, ref object data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "QueryData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QueryData(string TableName, int index, string ColName, ref object data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "RefreshBuffer", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void RefreshBuffer();

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "SaveDataToBuffer", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SaveDataToBuffer();

        #endregion

        #region DrawCurve
        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "Init", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init(Panel panel, string CurveType, Color col, float LineWidth, int Gridwidth, int Gridheight);

        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "SaveSourcePointF", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SaveSourcePointF(PointF[] pt);

        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "ClearSourcePointF", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearSourcePointF();

        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "GetSourcePointF", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetSourcePointF(out PointF[] pt);

        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "DrawCurve", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DrawCurve(Panel panel, Color col, float xMax, float yMax);

        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "ZoomOutBasePoint", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZoomOutBasePoint(Panel panel, Point StartPt, Point EndPt);

        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "ZoomInBasePoint", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZoomInBasePoint(Panel panel, Point StartPt, Point EndPt);

        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "ZoomOutBaseXY", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZoomOutBaseXY(Panel panel, float StartX, float StartY, float EndX, float EndY);

        [DllImport("DrawCurve_BaseFunction.dll", EntryPoint = "ZoomInBaseXY", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZoomInBaseXY(Panel panel, float StartX, float StartY, float EndX, float EndY);

        #endregion

        #region ExcelDLL
        [DllImport("ExcelLib__Speed.dll", EntryPoint = "Init", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init(string FilePath, string Sheetname);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "OpenFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OpenFile();

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "CloseFile", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseFile();

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "SetCellColor", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCellColor(string StartCellPos, string EndCellPos, string Command);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "SetCellFontColor", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCellFontColor(string StartCellPos, string EndCellPos, string Command);

        [DllImport("DrawCurve.dll", EntryPoint = "SetCellAutoFit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCellAutoFit(string StartCellPos, string EndCellPos, string Command);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "SetCellFont", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCellFont(string StartCellPos, string EndCellPos, string FontStyle, int size);

        [DllImport("DrawCurve.dll", EntryPoint = "SetCellAlignment", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCellAlignment(string StartCellPos, string EndCellPos, string HorizontalPosStyle, string VerticalPosStyle);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "MergeCell", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MergeCell(string StartCellPos, string EndCellPos);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteData(string CellPos, string Value);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReadData(string CellPos, ref string Value);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "InsertPic", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void InsertPic(string StartCellPos, string EndCellPos, string PicPath);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "InsertRow", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void InsertRow(int RowIndex);

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "Save", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Save();

        [DllImport("ExcelLib__Speed.dll", EntryPoint = "SaveAs", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SaveAs(string FilePath);
        #endregion

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        private Form1 m_MainFormHandle;
        public event FormShowState ShowMainForm;
        public AirKeepQuery(Form1 Handle)
        {
            InitializeComponent();
            m_MainFormHandle = Handle;
            ShowMainForm += new FormShowState(m_MainFormHandle.FormStateShow);
        }

        private DrawOneCurveInMap m_DrawCurveHandle = new DrawOneCurveInMap();
        private ExecelLibInterface m_ExcelHandle = new ExecelLibInterface();
        public AirTestForm m_AirTestFormHandle = null;
        public FliudTestForm m_FliudTestFormHandle = null;

        private string m_FilePath = Application.StartupPath + @"\AirKeep\AirKeepDataBase.mdb";//获取INI文件路径
        private string m_TableName = "TestResult";
        private string m_PicFilePath = Application.StartupPath + @"\AirKeep\pressure.png";//获取INI文件路径
        private string m_ExcelModuleFilePath = Application.StartupPath + @"\AirKeep\Report\AirKeepModule.xls";//获取INI文件路径
        private string m_ExcelReportFilePath = Application.StartupPath + @"\AirKeep\Report\";//获取INI文件路径

        private string m_INIstrFilePath = Application.StartupPath + @"\AirKeep\info.ini";//获取INI文件路径
        private string m_INIstrSec = "info"; //INI文件名

        private int m_RecordId = 0;
        private PointF[] m_HistoryPt;
        private DateTime m_StartTime;
        private float m_yMax = 0;

        private void AirKeepQuery_Load(object sender, EventArgs e)
        {
            //m_MainFormHandle.m_AirKeepAccessHandle.Init(m_FilePath, m_TableName, 1);
            AirTestForm.GetInstance(ref m_AirTestFormHandle);
            FliudTestForm.GetInstance(ref m_FliudTestFormHandle);

            string com = "select * from " + m_TableName;
            m_MainFormHandle.m_AirKeepAccessHandle.QueryData(com, DG_DataBase);

            m_DrawCurveHandle.Init(panel_Curve, "BTypeLine", Color.Red, 1.5f, 20, 10);
            DG_TestData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            m_ExcelHandle.Init(m_ExcelModuleFilePath, "sheet1");
        }

        public void RefreshDataBase()
        {
            string com = "select * from " + m_TableName;
            m_MainFormHandle.m_AirKeepAccessHandle.QueryData(com, DG_DataBase);
        }

        private void ShowMainFormInfo(string text)
        {
            if (ShowMainForm != null)
            {
                ShowMainForm(text);
            }
        }

        private void DG_DataBase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_RecordId = Convert.ToInt32(DG_DataBase[0, e.RowIndex].Value.ToString());

            ReadDataFromDataBase();
            m_DrawCurveHandle.ClearSourcePointF();
            m_DrawCurveHandle.SaveSourcePointF(m_HistoryPt);

            int len = m_HistoryPt.Length;
            if (len < 4)
            {
                return;
            }
            float xMax = m_HistoryPt[len - 1].X;
            m_DrawCurveHandle.DrawCurve(panel_Curve, Color.Red, xMax + 2, m_yMax);

            label5.Text = ((float)m_yMax).ToString("0.0") + "MPa";
            label4.Text = ((float)(m_yMax * 3 / 4)).ToString("0.0") + "MPa";
            label3.Text = ((float)(m_yMax * 2 / 4)).ToString("0.0") + "MPa";
            label2.Text = ((float)(m_yMax * 1 / 4)).ToString("0.0") + "MPa";
            label1.Text = ((float)(m_yMax * 0 / 4)).ToString("0.0") + "MPa";



            int H = 0;
            int M = 0;
            int S = 0;
            int second = (int)xMax + 1;
            DateTime startTime = m_StartTime;
            label6.Text = startTime.ToString("HH:mm:ss");

            GetlableTime(startTime, second / 7, ref H, ref M, ref S);
            label7.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 2 / 7, ref H, ref M, ref S);
            label8.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 3 / 7, ref H, ref M, ref S);
            label9.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 4 / 7, ref H, ref M, ref S);
            label10.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 5 / 7, ref H, ref M, ref S);
            label11.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 6 / 7, ref H, ref M, ref S);
            label12.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 7 / 7, ref H, ref M, ref S);
            label13.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();
        }

        private void GetlableTime(DateTime startTime, int t, ref int H, ref int M, ref int S)
        {
            H = (int)(t / 3600);
            M = (int)((t - H * 3600) / 60);
            S = (int)((t - H * 3600) % 60);

            S += startTime.Second;
            if (S > 59)
            {
                M++;
                S -= 60;
            }
            M += startTime.Minute;
            if (M > 59)
            {
                H++;
                M -= 60;
            }
            H += startTime.Hour;
            if (H > 23)
            {
                H = H - 24;
            }
        }

        private void ReadDataFromDataBase()
        {
            int Index = 0;
            m_MainFormHandle.m_AirKeepAccessHandle.GetIndexBaseKeyWord(m_TableName, m_RecordId, ref Index);
            object data = default(object);
            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "Product", ref data);
            string sProduct = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "TestNo", ref data);
            string TestNo = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "InspectDate", ref data);
            string sInspectDate = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "Standard", ref data);
            string sStandard = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "EquipModule", ref data);
            string sModule = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "PrintDate", ref data);
            string sPrintDate = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "Result", ref data);
            string sResult = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "Tester", ref data);
            string sTester = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "Reviewer", ref data);
            string sReviewer = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "Admin", ref data);
            string sAdmin = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "Mark", ref data);
            string sMark = data.ToString();

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "StartTime", ref data);
            m_StartTime = Convert.ToDateTime(data);

            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "yMax", ref data);
            m_yMax = Convert.ToInt32(data);

            float[] x;
            m_MainFormHandle.m_AirKeepAccessHandle.ReadFloatsData(m_TableName, Index, "Point_x", out x);

            float[] y;
            m_MainFormHandle.m_AirKeepAccessHandle.ReadFloatsData(m_TableName, Index, "Point_y", out y);

            float[] TestData;
            m_MainFormHandle.m_AirKeepAccessHandle.ReadFloatsData(m_TableName, Index, "TestData", out TestData);

            int len = x.Length;
            m_HistoryPt = new PointF[len];
            for (int i = 0; i < len; i++)
            {
                m_HistoryPt[i].X = x[i];
                m_HistoryPt[i].Y = y[i];
            }

            int TestDataNum = 0;
            m_MainFormHandle.m_AirKeepAccessHandle.ReadSigleData(m_TableName, Index, "TestDataNum", ref data);
            TestDataNum = Convert.ToInt32(data);

            DG_TestData.ColumnCount = 9;
            DG_TestData.Rows.Clear();
            for (int i = 0; i < TestDataNum; i++)    //
            {
                DG_TestData.Rows.Add();
            }
            DG_TestData.Rows[0].Cells[0].Value = "级数";
            DG_TestData.Rows[0].Cells[1].Value = "设定压力(MPa)";         
            DG_TestData.Rows[0].Cells[2].Value = "初始压力";
            DG_TestData.Rows[0].Cells[3].Value = "稳压时间(s)";
            DG_TestData.Rows[0].Cells[4].Value = "保压时间(s)";
            DG_TestData.Rows[0].Cells[5].Value = "理论压降(MPa)";
            DG_TestData.Rows[0].Cells[6].Value = "实际压降(MPa)";
            DG_TestData.Rows[0].Cells[7].Value = "升压速率(KPa/s)";
            DG_TestData.Rows[0].Cells[8].Value = "本阶段试验是否合格";
            for (int i = 1; i < TestDataNum + 1; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //DG_TestData.Rows[i].Cells[j].Value = TestData[(i - 1) * 8 + j].ToString("0.00");
                    if (j == 0)
                    {
                        DG_TestData.Rows[i].Cells[j].Value = TestData[(i - 1) * 8 + j];
                    }
                    else
                    {
                        DG_TestData.Rows[i].Cells[j].Value = TestData[(i - 1) * 8 + j].ToString("0.00");
                    }
                }
            }


            textBox_ProductName.Text = sProduct;
            textBox_InspectDate.Text = sInspectDate;
            textBox_InspectStandard.Text = sStandard;
            textBox_ProductModule.Text = sModule;
            textBox_PrintDate.Text = sPrintDate;
            textBox_Mark.Text = sMark;
            textBox_Result.Text = sResult;
            textBox_Tester.Text = sTester;
            textBox_Reviewer.Text = sReviewer;
            textBox_Admin.Text = sAdmin;
            textBox_TestNo.Text = TestNo;

        }

        private void button_CreatReport_Click(object sender, EventArgs e)
        {
            bool flag = SavePic();
            if (!flag)
            {
                return;
            }

            SaveDataToDataBase();
            CreatExcelResult();
            SaveInfo();
        }

        /// <summary>
        /// 截取图片
        /// </summary>
        /// <returns></returns>
        private bool SavePic()
        {
            int width = 1180;
            int height = 310;
            Bitmap image = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(image);
            Point p1 = default(Point);
            Point p2 = default(Point);
            p1.X = 0;
            p1.Y = 0;
            p2.X = this.Left + 18;
            p2.Y = this.Top + 587;

            g.CopyFromScreen(p2, p1, image.Size);

            string FileName = m_PicFilePath;
            image.Save(FileName);

            return true;
        }

        private void SaveDataToDataBase()
        {
            string sProduct = textBox_ProductName.Text;
            string sInspectDate = textBox_InspectDate.Text;
            string sStandard = textBox_InspectStandard.Text;
            string sModule = textBox_ProductModule.Text;
            string sPrintDate = textBox_PrintDate.Text;
            string sMark = textBox_Mark.Text;
            string sResult = textBox_Result.Text;
            string sTester = textBox_Tester.Text;
            string sReviewer = textBox_Reviewer.Text;
            string sAdmin = textBox_Admin.Text;
            string TestNo = textBox_TestNo.Text;

            int Index = 0;
            m_MainFormHandle.m_AirKeepAccessHandle.GetIndexBaseKeyWord(m_TableName, m_RecordId, ref Index);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "Product", sProduct);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "TestNo", TestNo);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "InspectDate", sInspectDate);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "Standard", sStandard);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "EquipModule", sModule);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "PrintDate", sPrintDate);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "Result", sResult);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "Tester", sTester);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "Reviewer", sReviewer);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "Admin", sAdmin);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleDataToExistCells(m_TableName, Index, "Mark", sMark);

            m_MainFormHandle.m_AirKeepAccessHandle.SaveDateToDataBase();
        }

        private void CreatExcelResult()
        {
            string sProduct = textBox_ProductName.Text;
            string sInspectDate = textBox_InspectDate.Text;
            string sStandard = textBox_InspectStandard.Text;
            string sModule = textBox_ProductModule.Text;
            string sPrintDate = textBox_PrintDate.Text;
            string sMark = textBox_Mark.Text;
            string sResult = textBox_Result.Text;
            string sTester = textBox_Tester.Text;
            string sReviewer = textBox_Reviewer.Text;
            string sAdmin = textBox_Admin.Text;
            string TestNo = textBox_TestNo.Text;

            m_ExcelHandle.OpenFile();
            m_ExcelHandle.WriteData("A3", "试验编号：" + TestNo);

            m_ExcelHandle.WriteData("C4", sProduct);
            m_ExcelHandle.WriteData("J4", sInspectDate);
            m_ExcelHandle.WriteData("P4", sStandard);

            m_ExcelHandle.WriteData("C6", sModule);
            m_ExcelHandle.WriteData("J6", sPrintDate);
            m_ExcelHandle.WriteData("P6", sMark);

            for (int i = 1; i < DG_TestData.Rows.Count; i++)
            {
                string t1 = DG_TestData[0, i].Value.ToString();
                m_ExcelHandle.WriteData("A"+(9 + i).ToString() , t1);
                string t2 = DG_TestData[1, i].Value.ToString();
                m_ExcelHandle.WriteData("C" + (9 + i).ToString(), t2);
                string t3 = DG_TestData[2, i].Value.ToString();
                m_ExcelHandle.WriteData("F" + (9 + i).ToString(), t3);
                string t4 = DG_TestData[3, i].Value.ToString();
                m_ExcelHandle.WriteData("H" + (9 + i).ToString(), t4);
                string t5 = DG_TestData[4, i].Value.ToString();
                m_ExcelHandle.WriteData("J" + (9 + i).ToString(), t5);
                string t6 = DG_TestData[5, i].Value.ToString();
                m_ExcelHandle.WriteData("L" + (9 + i).ToString(), t6);
                string t7 = DG_TestData[6, i].Value.ToString();
                m_ExcelHandle.WriteData("N" + (9 + i).ToString(), t7);
                string t8 = DG_TestData[7, i].Value.ToString();
                m_ExcelHandle.WriteData("P" + (9 + i).ToString(), t8);
                //string t9 = DG_TestData[8, i].Value.ToString();
                //m_ExcelHandle.WriteData("R" + (9 + i).ToString(), t9);
            }

            m_ExcelHandle.WriteData("C29", sResult);

            m_ExcelHandle.WriteData("C31", sTester);
            m_ExcelHandle.WriteData("J31", sReviewer);
            m_ExcelHandle.WriteData("Q31", sAdmin);

            m_ExcelHandle.InsertPic("C15", "R29", m_PicFilePath);

            string fileName = m_ExcelReportFilePath + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            m_ExcelHandle.SaveAs(fileName);
            m_ExcelHandle.CloseFile();

            System.Diagnostics.Process.Start(fileName);
        }

        private void SaveInfo()
        {
            string sProduct = textBox_ProductName.Text;
            string sInspectDate = textBox_InspectDate.Text;
            string sStandard = textBox_InspectStandard.Text;
            string sModule = textBox_ProductModule.Text;
            string sPrintDate = textBox_PrintDate.Text;
            string sMark = textBox_Mark.Text;
            string sResult = textBox_Result.Text;
            string sTester = textBox_Tester.Text;
            string sReviewer = textBox_Reviewer.Text;
            string sAdmin = textBox_Admin.Text;
            string TestNo = textBox_TestNo.Text;

            WritePrivateProfileString(m_INIstrSec, "Product", sProduct, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "TestNo", TestNo, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "InspectDate", sInspectDate, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "Standard", sStandard, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "EquipModule", sModule, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "PrintDate", sPrintDate, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "Mark", sMark, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "Result", sResult, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "Tester", sTester, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "Reviewer", sReviewer, m_INIstrFilePath);
            WritePrivateProfileString(m_INIstrSec, "Admin", sAdmin, m_INIstrFilePath);
        }

        private void ReadInfo()
        {
            textBox_ProductName.Text = ContentValue(m_INIstrSec, "Product", m_INIstrFilePath);
            textBox_InspectDate.Text = ContentValue(m_INIstrSec, "InspectDate", m_INIstrFilePath);
            textBox_InspectStandard.Text = ContentValue(m_INIstrSec, "Standard", m_INIstrFilePath);
            textBox_ProductModule.Text = ContentValue(m_INIstrSec, "EquipModule", m_INIstrFilePath);
            textBox_PrintDate.Text = ContentValue(m_INIstrSec, "PrintDate", m_INIstrFilePath);
            textBox_Mark.Text = ContentValue(m_INIstrSec, "Mark", m_INIstrFilePath);
            textBox_Result.Text = ContentValue(m_INIstrSec, "Result", m_INIstrFilePath);
            textBox_Tester.Text = ContentValue(m_INIstrSec, "Tester", m_INIstrFilePath);
            textBox_Reviewer.Text = ContentValue(m_INIstrSec, "Reviewer", m_INIstrFilePath);
            textBox_Admin.Text = ContentValue(m_INIstrSec, "Admin", m_INIstrFilePath);
        }

        private string ContentValue(string Section, string key, string strFilePath)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }

        private void button_Return_Click(object sender, EventArgs e)
        {
            this.Hide();

            bool isTesting = false;
            m_AirTestFormHandle.GetTestInfo(ref isTesting);
            if (isTesting)
            {
                m_AirTestFormHandle.Show();
                ShowMainFormInfo("气压测试");
                return;
            }
            m_FliudTestFormHandle.GetTestInfo(ref isTesting);
            if (isTesting)
            {
                m_FliudTestFormHandle.Show();
                ShowMainFormInfo("液压测试");
                return;
            }
            m_MainFormHandle.Show();
            ShowMainFormInfo("主菜单");
        }
    }
}
