﻿using System;
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

using System.Timers;

namespace YinLunTestBench
{
    public partial class AirTestForm : Form
    {
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

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        public System.Timers.Timer m_ShowSystemStatusTimer = new System.Timers.Timer();
        private System.Timers.Timer m_SampleDataTimer = new System.Timers.Timer();
        private System.Timers.Timer m_SampleStartTimer = new System.Timers.Timer();

        private string m_KeepstrFilePath = Application.StartupPath + @"\AirKeepTest.ini";//获取INI文件路径
        private string m_KeepstrSec = "AirKeepTest"; //INI文件名
        private string m_BoomstrFilePath = Application.StartupPath + @"\AirBoomTest.ini";//获取INI文件路径
        private string m_BoomstrSec = "AirBoomTest"; //INI文件名

        private int m_KeepTestNum = 0;//试验计数;
        private string m_KeepTestNo;//试验编号
        private int m_BoobTestNum = 0;//试验计数;
        private string m_BoobTestNo;//试验编号
        private float m_yMax = 0;
        private float m_xMax = 30;

        private string m_TableName = "TestResult";

        private DrawOneCurveInMap m_DrawCurveHandle = new DrawOneCurveInMap();
        public static AirTestForm AirTestFormHandle = null;
        private Form1 m_MainFormHandle;
        public event FormShowState ShowMainForm;
        private bool m_isTesting = false;
        public bool m_isTestingInt
        {
            get
            {
                return m_isTesting;
            }
        }

        public class KeepTestSaveDataStruct
        {
            public int m_Num;
            public float m_SetPressure;
            public float m_InitPressure;
            public float m_StableTime;
            public float m_KeepTime;
            public float m_DropPressure;
            public float m_ActualPressure;
            public float m_PressureRate;
            public KeepTestSaveDataStruct(int num, float SetPressure, float InitPressure, float StableTime, float KeepTime, float DropPressure,
                                          float ActualPressure, float PressureRate)
            {
                m_Num = num; m_SetPressure = SetPressure; m_InitPressure = InitPressure; m_StableTime = StableTime;
                m_KeepTime = KeepTime; m_DropPressure = DropPressure; m_ActualPressure = ActualPressure; m_PressureRate = PressureRate;
            }
        }
        public List<KeepTestSaveDataStruct> m_KeepTestSaveDataStructs = new List<KeepTestSaveDataStruct>();


        public class BoobTestSaveDataStruct
        {
            public int m_Num;
            public float m_SetPressure;
            public float m_InitPressure;
            public float m_StableTime;
            public float m_KeepTime;
            public float m_PressureRate;
            public BoobTestSaveDataStruct(int num, float SetPressure, float InitPressure, float StableTime, float KeepTime, 
                                          float PressureRate)
            {
                m_Num = num; m_SetPressure = SetPressure; m_InitPressure = InitPressure; m_StableTime = StableTime;
                m_KeepTime = KeepTime;  m_PressureRate = PressureRate;
            }
        }
        public List<BoobTestSaveDataStruct> m_BoobTestSaveDataStructs = new List<BoobTestSaveDataStruct>();
        public AirTestForm()
        {
            InitializeComponent();
            //m_MainFormHandle = handle;
            
        }

        public static void GetInstance(ref AirTestForm Handle)
        {
            if (AirTestFormHandle == null)
            {
                AirTestFormHandle = new AirTestForm();
            }
            Handle = AirTestFormHandle;
        }

        private bool m_FuctionSelect = false;
        private void AirTestForm_Load(object sender, EventArgs e)
        {
            m_MainFormHandle = Form1.m_SelfHandle;
            ShowMainForm += new FormShowState(m_MainFormHandle.FormStateShow);
            //ShowMainFormInfo("气压测试");

            m_ShowSystemStatusTimer.AutoReset = true;
            m_ShowSystemStatusTimer.Interval = 1000;
            m_ShowSystemStatusTimer.Elapsed += new ElapsedEventHandler(ShowSystemStatusFun);
            m_ShowSystemStatusTimer.Enabled = true;

            m_SampleDataTimer.AutoReset = true;
            m_SampleDataTimer.Interval = 200;
            m_SampleDataTimer.Elapsed += new ElapsedEventHandler(SampleDataFun);

            m_SampleStartTimer.AutoReset = true;
            m_SampleStartTimer.Interval = 600;
            m_SampleStartTimer.Elapsed += new ElapsedEventHandler(SampleStartFun);
            m_SampleStartTimer.Enabled = true;           

            m_DrawCurveHandle.Init(panel_Curve, "BTypeLine", Color.Red, 1.5f, 20, 10);
        }

        private void ShowMainFormInfo(string text)
        {
            if (ShowMainForm != null)
            {
                ShowMainForm(text);
            }
        }

        private void ShowSystemStatusFun(object o, ElapsedEventArgs e)
        {
            if (m_FuctionSelect)//静压
            {
                ShowKeepInfo();
            }
            else//爆破
            {
                ShowBoobInfo();
            }
        }

        private PointF[] m_PointF = new PointF[2];
        private int m_PtCount = 0;
        private void SampleDataFun(object o, ElapsedEventArgs e)
        {
            if (m_FuctionSelect)//静压
            {
                SampleKeepData();
            }
            else//爆破
            {
                SampleBoobData();
            }
        }

        private DateTime m_StartTime;//试验起始时间
        private bool m_isStart = false;
        private DateTime m_EndTime;//结束时间
        private bool m_isEndTest = false;
        private int m_EndCount = 0;
        private float m_ActualDropPressure = 0;
        private bool m_isCalActualPreRaise = false;
        private float m_ActualPressure = 0.0f;//用于升压速率的起始压力保存
        private DateTime m_ActualPreRaiseStartTime = default(DateTime);
        private void SampleStartFun(object o, ElapsedEventArgs e)
        {
            DateTime dt = default(DateTime);
            byte StartFlag = 0;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StartTest", ref StartFlag, ref dt);
            if (StartFlag == 1)
            {
                textBox_RunState.Text = "开始";
                if (!m_isStart)
                {
                    m_DrawCurveHandle.ClearSourcePointF();
                    m_StartTime = DateTime.Now;
                    textBox_StartTime.Text = m_StartTime.ToString("HH:mm:ss");

                    m_PtCount = 0;
                    m_isFirstSapmle = true;
                    m_SampleDataTimer.Enabled = true;
                    m_xMax = 30;
                    m_StableTime = 0;
                    m_KeepTime = 0;
                    m_Stage = 1;
                    m_KeepTestSaveDataStructs.Clear();
                    m_BoobTestSaveDataStructs.Clear();
                    m_isStart = true;

                    textBox_Actual_DropPressure.Text = "0";
                    textBox_ContinueTime.Text = "0";
                    textBox_ActualStableTime.Text = "0";
                    textBox_BoomPressure.Text = "0";
                    textBox_EndTime.Text = "0";
                    if (m_SaveContinue)
                    {
                        TB_Save.Enabled = false;
                        TB_CancelSave.Enabled = false;
                    }
                    else
                    {
                        TB_Save.Enabled = true;
                        TB_CancelSave.Enabled = true;
                    }
                    m_isCalActualPreRaise = true;
                    m_ActualPreRaiseStartTime = DateTime.Now;
                    m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref m_ActualPressure, ref dt);
                }
                
                m_isTesting = true;


            }

            byte EndFlag = 0;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_EndTest", ref EndFlag, ref dt);
            if (EndFlag == 1)
            {
                //m_DrawCurveHandle.ClearSourcePointF();
                m_isTesting = false;
                if (m_isStart)
                {
                    m_EndTime = DateTime.Now;
                    textBox_EndTime.Text = m_EndTime.ToString("HH:mm:ss");

                    TimeSpan tp = m_EndTime - m_StartTime;
                    textBox_ContinueTime.Text = tp.Hours.ToString() +":" + tp.Minutes.ToString() + ":"+ tp.Seconds.ToString();

                    //textBox_RunState.Text = "停止";
                    //textBox_ActualStableTime.Text = "0";

                    m_isEndTest = true;

                    //m_SampleDataTimer.Enabled = false;

                    //if (!m_FuctionSelect)
                    //{
                    //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_BoomPressure", ref m_BoobPressure, ref dt);
                    //    textBox_BoomPressure.Text = m_BoobPressure.ToString("0.00");
                    //}

                    m_isStart = false;
                }
            }

            //2s后停止采样
            if (m_isEndTest)
            {
                m_EndCount++;
                if (m_EndCount >= 10)
                {
                    m_SampleDataTimer.Enabled = false;
                    m_EndCount = 0;
                    m_isEndTest = false;
                    for (int i = 0; i < 2; i++)
                    {
                        m_PointF[i].X = 0;
                        m_PointF[i].Y = 0;
                    }
                    for (int i = 0; i < m_SampleMaxNum; i++)
                    {
                        m_SamplePt[i].X = 0;
                        m_SamplePt[i].Y = 0;
                    }
                    if (!m_FuctionSelect)
                    {
                        SaveBoobDataToStruct();
                    }

                    if (m_SaveContinue)
                    {
                        if (m_FuctionSelect)
                        {
                            SaveKeepDataToDataBase();
                        }
                        else
                        {
                            SaveBoobDataToDataBase();
                        }
                    }

                    else
                    {
                        DialogResult MsgBoxResult;//设置对话框的返回值
                        MsgBoxResult = MessageBox.Show("确定保存吗？",//对话框的显示内容
                        "提示",//对话框的标题
                        MessageBoxButtons.YesNo,//定义对话框的按钮，这里定义了YSE和NO两个按钮
                        MessageBoxIcon.Exclamation,//定义对话框内的图表式样，这里是一个黄色三角型内加一个感叹号
                        MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
                        if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）
                        {
                            if (m_FuctionSelect)
                            {
                                SaveKeepDataToDataBase();
                            }
                            else
                            {
                                SaveBoobDataToDataBase();
                            }
                            TB_Save.Enabled = false;
                        }


                        if (MsgBoxResult == DialogResult.No)//如果对话框的返回值是NO（按"N"按钮）
                        {
                            m_DrawCurveHandle.ClearSourcePointF();
                            m_KeepTestSaveDataStructs.Clear();
                            m_BoobTestSaveDataStructs.Clear();
                            TB_CancelSave.Enabled = false;
                        }
                    }

                    //textBox2.Text = m_Stage.ToString();
                    textBox_RunState.Text = "停止";
                    m_isCalActualPreRaise = false;
                }
            }

            float ActPressure = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref ActPressure, ref dt);
            if (ActPressure <= 0)
            {
                ActPressure = 0.01f;
            }
            textBox_ActualPressure.Text = ActPressure.ToString("0.00");

            float DropPressure = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_DropPressure", ref DropPressure, ref dt);
            textBox_Actual_DropPressure.Text = DropPressure.ToString("0.00");
            if (DropPressure > 0.001f)
            {
                m_ActualDropPressure = DropPressure;
            }

            if (!m_FuctionSelect)
            {
                m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_BoomPressure", ref m_BoobPressure, ref dt);
                textBox_BoomPressure.Text = m_BoobPressure.ToString("0.00");
            }


            //float KeepTime = 0.0f;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Fluid_Actual_KeepTime", ref KeepTime, ref dt);
            //textBox_ActualStableTime.Text = KeepTime.ToString("0.00");
            if (m_FuctionSelect)//静压
            {
                SampleKeepTestSystemState();
            }
            else//爆破
            {
                SampleBoobTestSystemState();
            }

            //升压速率
            if (m_isCalActualPreRaise)
            {
                DateTime t1 = DateTime.Now;
                TimeSpan tp = t1 - m_ActualPreRaiseStartTime;
                int secend = tp.Hours * 3600 + tp.Minutes * 60 + tp.Seconds;
                if (secend >= 1)
                {
                    float PreRaise = (ActPressure - m_ActualPressure) * 1000 / secend;
                    textBox_ActualPreRaise.Text = PreRaise.ToString("0.00");
                }
            }

            //采集稳压状态
            byte isStableState = 0 ;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Stable", ref isStableState, ref dt);
            if (isStableState == 1 && m_isCalActualPreRaise)
            {
                m_isCalActualPreRaise = false;
            }
        }


        public void GetTestInfo(ref bool isTesting)
        {
            isTesting = m_isTesting;
        }

        public void GetFunctionSelect(bool FunctionSelect)
        {
            m_FuctionSelect = FunctionSelect;
        }

        public void SetFormStyle()
        {
            if (m_FuctionSelect)
            {
                textBox1.Text = "气 压 静 压 试 验";
                textBox_BoomPressure.Visible = false;
                label_BoomPressure.Visible = false;
                m_yMax = 3.2f;

                label5.Text = m_yMax.ToString() + "MPa";
                label4.Text = (m_yMax * 3 / 4).ToString() + "MPa";
                label3.Text = (m_yMax * 2 / 4).ToString() + "MPa";
                label2.Text = (m_yMax * 1 / 4).ToString() + "MPa";
                label1.Text = (m_yMax * 0 / 4).ToString() + "MPa";
            }
            else
            {
                textBox1.Text = "气 压 爆 破 试 验";
                textBox_BoomPressure.Visible = true;
                label_BoomPressure.Visible = true;
                m_yMax = 3.2f;

                label5.Text = m_yMax.ToString() + "MPa";
                label4.Text = (m_yMax * 3 / 4).ToString() + "MPa";
                label3.Text = (m_yMax * 2 / 4).ToString() + "MPa";
                label2.Text = (m_yMax * 1 / 4).ToString() + "MPa";
                label1.Text = (m_yMax * 0 / 4).ToString() + "MPa";
            }
        }

        private string ContentValue(string Section, string key, string strFilePath)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }

        private void GetTimeSpan(DateTime now, DateTime BaseTime, ref float TotalTime)
        {
            TimeSpan tp = now - BaseTime;
            TotalTime = (tp.Days * 24 * 3600) +
                        (tp.Hours * 3600) +
                        (tp.Minutes * 60) +
                        (tp.Seconds) +
                        tp.Milliseconds / 1000.0f;
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


        private void TB_Save_Click(object sender, EventArgs e)
        {
            if (m_FuctionSelect)
            {
                SaveKeepDataToDataBase();
            }
            else
            {
                SaveBoobDataToDataBase();
            }
            MessageBox.Show("保存成功", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TB_Save.Enabled = false;
        }

        private void TB_CancelSave_Click(object sender, EventArgs e)
        {
            m_DrawCurveHandle.ClearSourcePointF();
            m_KeepTestSaveDataStructs.Clear();
            m_BoobTestSaveDataStructs.Clear();
            MessageBox.Show("取消保存成功", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TB_CancelSave.Enabled = false;
        }

        private void TB_Exit_Click(object sender, EventArgs e)
        {
            if (m_isTesting)
            {
                MessageBox.Show("请先停止测试", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Hide();
            m_MainFormHandle.Show();
            ShowMainFormInfo("主菜单");
        }


        #region Keep
        private void ShowKeepInfo()
        {
            //基本信息
            textBox_Date.Text = DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss");
            if (m_FuctionSelect)
            {
                textBox_TestType.Text = "气压静压试验";
            }
            else
            {
                textBox_TestType.Text = "气压爆破试验";
            }
           
            string TestTemp = ContentValue(m_KeepstrSec, "TestNo", m_KeepstrFilePath);
            if (m_KeepTestNo != TestTemp)
            {
                m_KeepTestNum = 0;
                m_KeepTestNo = TestTemp;
            }
            textBox_TestNo.Text = m_KeepTestNo + m_KeepTestNum.ToString();
            //textBox_PressureRate2.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure", m_BoomstrFilePath);

            string Stage1 = ContentValue(m_KeepstrSec, "Air_Chanel1", m_KeepstrFilePath);
            if (Stage1 == "0")
            {
                Grp_Stage1.Enabled = false;
            }
            if (Stage1 == "1")
            {
                Grp_Stage1.Enabled = true;
                textBox_SetPre1.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure1", m_KeepstrFilePath);
                textBox_DropPre1.Text = ContentValue(m_KeepstrSec, "Air_DropPressure1", m_KeepstrFilePath);
                textBox_KeepTime1.Text = ContentValue(m_KeepstrSec, "Air_KeepTime1", m_KeepstrFilePath);
                textBox_StableTime1.Text = ContentValue(m_KeepstrSec, "Air_StableTime1", m_KeepstrFilePath);
                textBox_PressureRate1.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure1", m_KeepstrFilePath);
                //m_SetPressure = Convert.ToSingle(textBox_SetPre1.Text);
            }

            string Stage2 = ContentValue(m_KeepstrSec, "Air_Chanel2", m_KeepstrFilePath);
            if (Stage2 == "0")
            {
                Grp_Stage2.Enabled = false;
            }
            if (Stage2 == "1")
            {
                Grp_Stage2.Enabled = true;
                textBox_SetPre2.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure2", m_KeepstrFilePath);
                textBox_DropPre2.Text = ContentValue(m_KeepstrSec, "Air_DropPressure2", m_KeepstrFilePath);
                textBox_KeepTime2.Text = ContentValue(m_KeepstrSec, "Air_KeepTime2", m_KeepstrFilePath);
                textBox_StableTime2.Text = ContentValue(m_KeepstrSec, "Air_StableTime2", m_KeepstrFilePath);
                textBox_PressureRate2.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure2", m_KeepstrFilePath);
            }

            string Stage3 = ContentValue(m_KeepstrSec, "Air_Chanel3", m_KeepstrFilePath);
            if (Stage3 == "0")
            {
                Grp_Stage3.Enabled = false;
            }
            if (Stage3 == "1")
            {
                Grp_Stage3.Enabled = true;
                textBox_SetPre3.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure3", m_KeepstrFilePath);
                textBox_DropPre3.Text = ContentValue(m_KeepstrSec, "Air_DropPressure3", m_KeepstrFilePath);
                textBox_KeepTime3.Text = ContentValue(m_KeepstrSec, "Air_KeepTime3", m_KeepstrFilePath);
                textBox_StableTime3.Text = ContentValue(m_KeepstrSec, "Air_StableTime3", m_KeepstrFilePath);
                textBox_PressureRate3.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure3", m_KeepstrFilePath);
            }

            string Stage4 = ContentValue(m_KeepstrSec, "Air_Chanel4", m_KeepstrFilePath);
            if (Stage4 == "0")
            {
                Grp_Stage4.Enabled = false;
            }
            if (Stage4 == "1")
            {
                Grp_Stage4.Enabled = true;
                textBox_SetPre4.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure4", m_KeepstrFilePath);
                textBox_DropPre4.Text = ContentValue(m_KeepstrSec, "Air_DropPressure4", m_KeepstrFilePath);
                textBox_KeepTime4.Text = ContentValue(m_KeepstrSec, "Air_KeepTime4", m_KeepstrFilePath);
                textBox_StableTime4.Text = ContentValue(m_KeepstrSec, "Air_StableTime4", m_KeepstrFilePath);
                textBox_PressureRate4.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure4", m_KeepstrFilePath);
            }

            string Stage5 = ContentValue(m_KeepstrSec, "Air_Chanel5", m_KeepstrFilePath);
            if (Stage5 == "0")
            {
                Grp_Stage5.Enabled = false;
            }
            if (Stage5 == "1")
            {
                Grp_Stage5.Enabled = true;
                textBox_SetPre5.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure5", m_KeepstrFilePath);
                textBox_DropPre5.Text = ContentValue(m_KeepstrSec, "Air_DropPressure5", m_KeepstrFilePath);
                textBox_KeepTime5.Text = ContentValue(m_KeepstrSec, "Air_KeepTime5", m_KeepstrFilePath);
                textBox_StableTime5.Text = ContentValue(m_KeepstrSec, "Air_StableTime5", m_KeepstrFilePath);
                textBox_PressureRate5.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure5", m_KeepstrFilePath);
            }
            //DateTime dt = default(DateTime);
            //float ActPressure = 0.0f;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref ActPressure, ref dt);
            //textBox_ActualPressure.Text = ActPressure.ToString("0.00");
        }

        private int m_Stage = 1;
        private float m_StableTime = 0.0f;
        private float m_KeepTime = 0.0f;
        private int m_SampleMaxNum = 5;
        private PointF[] m_SamplePt = new PointF[5];
        private int m_SamplePtCount = 0;
        private bool m_isFirstSapmle = true;
        private void SampleKeepData()
        {
            DateTime dt = default(DateTime);
            float PressureValue = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref PressureValue, ref dt);
            if (PressureValue <= 0)
            {
                PressureValue = 0.1f;
            }

            float TotalTime = 0;
            GetTimeSpan(dt, m_StartTime, ref TotalTime);
            if (m_PtCount >= 2 || m_PtCount <= 0)
            {
                m_PtCount = 0;
            }
            //m_PointF[m_PtCount].X = TotalTime;
            //m_PointF[m_PtCount].Y = PressureValue;
            //m_PtCount++;

            m_SamplePt[m_SamplePtCount].X = TotalTime;
            m_SamplePt[m_SamplePtCount].Y = PressureValue;
            m_SamplePtCount++;

            PointF pt = default(PointF);
            if (m_SamplePtCount >= m_SampleMaxNum)
            {
                float y = 0.0f;
                float x = 0.0f;
                x = m_SamplePt[m_SampleMaxNum - 1].X;
                for (int i = 0; i < m_SampleMaxNum; i++)
                {
                    y += m_SamplePt[i].Y;
                    //x += m_SamplePt[i].X;
                    m_SamplePt[i].Y = 0;
                    m_SamplePt[i].X = 0;
                }
                pt.X = x;
                pt.Y = y / m_SampleMaxNum;
                m_SamplePtCount = 0;
                if (m_PtCount >= 2 || m_PtCount <= 0)
                {
                    m_PtCount = 0;
                }
                m_PointF[m_PtCount].X = pt.X;
                m_PointF[m_PtCount].Y = pt.Y;
                m_PtCount++;
            }

            if (TotalTime >= m_xMax)
            {
                m_xMax += 30;
            }

            if (m_isFirstSapmle)
            {
                m_PointF[0].Y = 0.01f;
                m_isFirstSapmle = false;
            }

            if (m_PtCount >= 2)
            {
                m_DrawCurveHandle.SaveSourcePointF(m_PointF);
                m_DrawCurveHandle.DrawCurve(panel_Curve, Color.Red, m_xMax, m_yMax);
                m_PtCount = 0;
            }

            int H = 0;
            int M = 0;
            int S = 0;
            int second = (int)m_xMax + 1;
            DateTime startTime = m_StartTime;
            label6.Text = startTime.ToString("HH:mm:ss");

            GetlableTime(startTime, second / 6, ref H, ref M, ref S);
            label7.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 2 / 6, ref H, ref M, ref S);
            label8.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 3 / 6, ref H, ref M, ref S);
            label9.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 4 / 6, ref H, ref M, ref S);
            label10.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 5 / 6, ref H, ref M, ref S);
            label11.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 6 / 6, ref H, ref M, ref S);
            label12.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            //byte stableState = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Stable", ref stableState, ref dt);
            //if (stableState == 1)
            //{
            //    float stableTime = 0.0f;
            //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_StableTime", ref stableTime, ref dt);
            //    textBox_ActualStableTime.Text = stableTime.ToString("0.00");
            //    m_StableTime = stableTime;
            //}

            //byte KeepState = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Keep", ref KeepState, ref dt);
            //if (KeepState == 1)
            //{
            //    float KeepTime = 0.0f;
            //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_KeepTime", ref KeepTime, ref dt);
            //    textBox_ActualStableTime.Text = KeepTime.ToString("0.00");
            //    m_KeepTime = KeepTime;
            //}

            //byte StageFinish = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StageFinish", ref StageFinish, ref dt);
            //if (StageFinish == 1)
            //{
            //    SaveKeepDataToStruct();
            //    m_Stage++;
            //    StageFinish = 0;
            //    m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StageFinish", StageFinish);
            //}        

        }

        //private bool m_KeepStableState = false;
        //private bool m_KeepState = false;
        //private bool m_KeepStageFinish = false;
        private void SampleKeepTestSystemState()
        {
            DateTime dt = default(DateTime);
            //byte stableState = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Stable", ref stableState, ref dt);
            //if (stableState == 1 && m_KeepStableState == false)
            //{
            //    float stableTime = 0.0f;
            //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_StableTime", ref stableTime, ref dt);
            //    textBox_ActualStableTime.Text = stableTime.ToString("0.00");
            //    m_StableTime = stableTime;
            //    m_KeepStableState = true;
            //}
            //if (stableState == 0 && m_KeepStableState == true)
            //{
            //    m_KeepStableState = false;
            //}

            //byte KeepState = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Keep", ref KeepState, ref dt);
            //if (KeepState == 1 && m_KeepState == false)
            //{
            //    float KeepTime = 0.0f;
            //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_KeepTime", ref KeepTime, ref dt);
            //    textBox_ActualStableTime.Text = KeepTime.ToString("0.00");
            //    m_KeepTime = KeepTime;
            //    m_KeepState = true;
            //}
            //if (KeepState == 0 && m_KeepState == true)
            //{
            //    m_KeepState = false;
            //}

            float KeepTime = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_KeepTime", ref KeepTime, ref dt);
            textBox_ActualStableTime.Text = KeepTime.ToString("0.00");

            byte StageFinish = 0;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StageFinish", ref StageFinish, ref dt);
            if (StageFinish == 1)// && m_KeepStageFinish == false)
            {
                StageFinish = 0;
                m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StageFinish", StageFinish);
                SaveKeepDataToStruct();
                m_Stage++;

                m_ActualPreRaiseStartTime = DateTime.Now;
                m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref m_ActualPressure, ref dt);
                m_isCalActualPreRaise = true;
                //m_KeepStageFinish = true;
            }
            //if (StageFinish == 1 && m_KeepStageFinish == true)
            //{
            //    m_KeepStageFinish = false;
            //}

        }

        //private bool t_test = false;
        private void SaveKeepDataToStruct()
        {
            DateTime dt = default(DateTime);
            float InitPressure = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_InitPressure", ref InitPressure, ref dt);
            //t_test = t_test ? false : true;
            //if (t_test)
            //{
            //    textBox2.BackColor = Color.Red;
            //}
            //else
            //{
            //    textBox2.BackColor = Color.Green;
            //}
            //textBox2.Text = InitPressure.ToString() + "--" + m_KeepTestSaveDataStructs.Count + "--" + m_Stage.ToString(); ;
            try
            {
                if (m_Stage == 1)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre1.Text);
                    float DropPressure = Convert.ToSingle(textBox_DropPre1.Text);
                    float ActualDropPressure = m_ActualDropPressure;
                    float PressureRate1 = Convert.ToSingle(textBox_PressureRate1.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime1.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime1.Text);

                    m_KeepTestSaveDataStructs.Add(new KeepTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             DropPressure, ActualDropPressure, PressureRate1));
                }

                if (m_Stage == 2)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre2.Text);
                    float DropPressure = Convert.ToSingle(textBox_DropPre2.Text);
                    float ActualDropPressure = m_ActualDropPressure;
                    float PressureRate2 = Convert.ToSingle(textBox_PressureRate2.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime2.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime2.Text);
                    m_KeepTestSaveDataStructs.Add(new KeepTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             DropPressure, ActualDropPressure, PressureRate2));
                }

                if (m_Stage == 3)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre3.Text);
                    float DropPressure = Convert.ToSingle(textBox_DropPre3.Text);
                    float ActualDropPressure = m_ActualDropPressure;
                    float PressureRate3 = Convert.ToSingle(textBox_PressureRate3.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime3.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime3.Text);
                    m_KeepTestSaveDataStructs.Add(new KeepTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             DropPressure, ActualDropPressure, PressureRate3));
                }

                if (m_Stage == 4)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre4.Text);
                    float DropPressure = Convert.ToSingle(textBox_DropPre4.Text);
                    float ActualDropPressure = m_ActualDropPressure;
                    float PressureRate4 = Convert.ToSingle(textBox_PressureRate4.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime4.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime4.Text);
                    m_KeepTestSaveDataStructs.Add(new KeepTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             DropPressure, ActualDropPressure, PressureRate4));
                }

                if (m_Stage == 5)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre5.Text);
                    float DropPressure = Convert.ToSingle(textBox_DropPre5.Text);
                    float ActualDropPressure = m_ActualDropPressure;
                    float PressureRate5 = Convert.ToSingle(textBox_PressureRate5.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime5.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime5.Text);
                    m_KeepTestSaveDataStructs.Add(new KeepTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             DropPressure, ActualDropPressure, PressureRate5));
                }
            }
            catch (Exception)
            {
                m_Stage--;
                return;
            }
        }


        private void SaveKeepDataToDataBase()
        {
            string TestNo = m_KeepTestNo + m_KeepTestNum.ToString();
            m_KeepTestNum++;

            int index = 0;
            m_MainFormHandle.m_AirKeepAccessHandle.AddNewRow(m_TableName, ref index);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleData(m_TableName, index, "TestNo", TestNo);

            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleData(m_TableName, index, "StartTime", m_StartTime);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleData(m_TableName, index, "yMax", m_yMax);

            PointF[] pt;
            m_DrawCurveHandle.GetSourcePointF(out pt);

            int len = pt.Length;
            if (len < 4)
            {
                m_MainFormHandle.m_AirKeepAccessHandle.DeleteRow(m_TableName, index);
                return;
            }

            float[] x = new float[len];
            float[] y = new float[len];

            for (int i = 0; i < len; i++)
            {
                x[i] = pt[i].X;
                y[i] = pt[i].Y;
            }
            m_MainFormHandle.m_AirKeepAccessHandle.WriteFloatsData(m_TableName, index, "Point_x", x);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteFloatsData(m_TableName, index, "Point_y", y);

            int count = m_KeepTestSaveDataStructs.Count;
            int TestCount = count * 8;
            float[] TestData = new float[TestCount];

            for (int i = 0; i < count; i++)
            {
                TestData[i * 8 + 1] = m_KeepTestSaveDataStructs[i].m_SetPressure;
                TestData[i * 8 + 2] = m_KeepTestSaveDataStructs[i].m_InitPressure;
                TestData[i * 8 + 3] = m_KeepTestSaveDataStructs[i].m_StableTime;
                TestData[i * 8 + 4] = m_KeepTestSaveDataStructs[i].m_KeepTime;
                TestData[i * 8 + 5] = m_KeepTestSaveDataStructs[i].m_DropPressure;
                TestData[i * 8 + 6] = m_KeepTestSaveDataStructs[i].m_ActualPressure;
                TestData[i * 8 + 7] = m_KeepTestSaveDataStructs[i].m_PressureRate;
                TestData[i * 8 + 0] = m_KeepTestSaveDataStructs[i].m_Num;

            }
            m_MainFormHandle.m_AirKeepAccessHandle.WriteSigleData(m_TableName, index, "TestDataNum", count);
            m_MainFormHandle.m_AirKeepAccessHandle.WriteFloatsData(m_TableName, index, "TestData", TestData);
            m_MainFormHandle.m_AirKeepAccessHandle.SaveDataToBuffer();
            m_MainFormHandle.m_AirKeepAccessHandle.SaveDateToDataBase();
        }

        #endregion

        #region Boob
        //private float m_SetPressure;
        //private float m_PressureRate;
        private void ShowBoobInfo()
        {
            //基本信息
            
            textBox_Date.Text = DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss");
            if (m_FuctionSelect)
            {
                textBox_TestType.Text = "气压静压试验";
            }
            else
            {
                textBox_TestType.Text = "气压爆破试验";
            }
            string TestTemp = ContentValue(m_BoomstrSec, "TestNo", m_BoomstrFilePath);
            if (m_BoobTestNo != TestTemp)
            {
                m_BoobTestNum = 0;
                m_BoobTestNo = TestTemp;
            }
            textBox_TestNo.Text = m_BoobTestNo + m_BoobTestNum.ToString();
            //textBox_PressureRate2.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure", m_KeepstrFilePath);

            string Stage1 = ContentValue(m_BoomstrSec, "Air_Chanel1", m_BoomstrFilePath);
            if (Stage1 == "0")
            {
                Grp_Stage1.Enabled = false;
            }
            if (Stage1 == "1")
            {
                Grp_Stage1.Enabled = true;
                textBox_SetPre1.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure1", m_BoomstrFilePath);
                textBox_DropPre1.Text = ContentValue(m_BoomstrSec, "Air_DropPressure1", m_BoomstrFilePath);
                textBox_KeepTime1.Text = ContentValue(m_BoomstrSec, "Air_KeepTime1", m_BoomstrFilePath);
                textBox_StableTime1.Text = ContentValue(m_BoomstrSec, "Air_StableTime1", m_BoomstrFilePath);
                textBox_PressureRate1.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure1", m_BoomstrFilePath);

                //m_SetPressure = Convert.ToSingle(textBox_SetPre1.Text);              
            }

            string Stage2 = ContentValue(m_BoomstrSec, "Air_Chanel2", m_BoomstrFilePath);
            if (Stage2 == "0")
            {
                Grp_Stage2.Enabled = false;
            }
            if (Stage2 == "1")
            {
                Grp_Stage2.Enabled = true;
                textBox_SetPre2.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure2", m_BoomstrFilePath);
                textBox_DropPre2.Text = ContentValue(m_BoomstrSec, "Air_DropPressure2", m_BoomstrFilePath);
                textBox_KeepTime2.Text = ContentValue(m_BoomstrSec, "Air_KeepTime2", m_BoomstrFilePath);
                textBox_StableTime2.Text = ContentValue(m_BoomstrSec, "Air_StableTime2", m_BoomstrFilePath);
                textBox_PressureRate2.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure2", m_BoomstrFilePath);
                //m_SetPressure = Convert.ToSingle(textBox_SetPre2.Text);   

            }

            string Stage3 = ContentValue(m_BoomstrSec, "Air_Chanel3", m_BoomstrFilePath);
            if (Stage3 == "0")
            {
                Grp_Stage3.Enabled = false;
            }
            if (Stage3 == "1")
            {
                Grp_Stage3.Enabled = true;
                textBox_SetPre3.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure3", m_BoomstrFilePath);
                textBox_DropPre3.Text = ContentValue(m_BoomstrSec, "Air_DropPressure3", m_BoomstrFilePath);
                textBox_KeepTime3.Text = ContentValue(m_BoomstrSec, "Air_KeepTime3", m_BoomstrFilePath);
                textBox_StableTime3.Text = ContentValue(m_BoomstrSec, "Air_StableTime3", m_BoomstrFilePath);
                textBox_PressureRate3.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure3", m_BoomstrFilePath);

               // m_SetPressure = Convert.ToSingle(textBox_SetPre3.Text);   
            }

            string Stage4 = ContentValue(m_BoomstrSec, "Air_Chanel4", m_BoomstrFilePath);
            if (Stage4 == "0")
            {
                Grp_Stage4.Enabled = false;
            }
            if (Stage4 == "1")
            {
                Grp_Stage4.Enabled = true;
                textBox_SetPre4.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure4", m_BoomstrFilePath);
                textBox_DropPre4.Text = ContentValue(m_BoomstrSec, "Air_DropPressure4", m_BoomstrFilePath);
                textBox_KeepTime4.Text = ContentValue(m_BoomstrSec, "Air_KeepTime4", m_BoomstrFilePath);
                textBox_StableTime4.Text = ContentValue(m_BoomstrSec, "Air_StableTime4", m_BoomstrFilePath);
                textBox_PressureRate4.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure4", m_BoomstrFilePath);

                //m_SetPressure = Convert.ToSingle(textBox_SetPre4.Text);   
            }

            string Stage5 = ContentValue(m_BoomstrSec, "Air_Chanel5", m_BoomstrFilePath);
            if (Stage5 == "0")
            {
                Grp_Stage5.Enabled = false;
            }
            if (Stage5 == "1")
            {
                Grp_Stage5.Enabled = true;
                textBox_SetPre5.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure5", m_BoomstrFilePath);
                textBox_DropPre5.Text = ContentValue(m_BoomstrSec, "Air_DropPressure5", m_BoomstrFilePath);
                textBox_KeepTime5.Text = ContentValue(m_BoomstrSec, "Air_KeepTime5", m_BoomstrFilePath);
                textBox_StableTime5.Text = ContentValue(m_BoomstrSec, "Air_StableTime5", m_BoomstrFilePath);
                textBox_PressureRate5.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure5", m_BoomstrFilePath);

                //m_SetPressure = Convert.ToSingle(textBox_SetPre5.Text);   
            }
            DateTime dt = default(DateTime);
            float ActPressure = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref ActPressure, ref dt);
            textBox_ActualPressure.Text = ActPressure.ToString("0.00");
        }

        private float m_BoobPressure = 0.0f;
        private void SampleBoobData()
        {
            DateTime dt = default(DateTime);
            float PressureValue = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref PressureValue, ref dt);
            if (PressureValue <= 0)
            {
                PressureValue = 0.1f;
            }

            float TotalTime = 0;
            GetTimeSpan(dt, m_StartTime, ref TotalTime);
            if (m_PtCount >= 2 || m_PtCount <= 0)
            {
                m_PtCount = 0;
            }
            //m_PointF[m_PtCount].X = TotalTime;
            //m_PointF[m_PtCount].Y = PressureValue;
            //m_PtCount++;

            m_SamplePt[m_SamplePtCount].X = TotalTime;
            m_SamplePt[m_SamplePtCount].Y = PressureValue;
            m_SamplePtCount++;

            PointF pt = default(PointF);
            if (m_SamplePtCount >= m_SampleMaxNum)
            {
                float y = 0.0f;
                float x = 0.0f;
                x = m_SamplePt[m_SampleMaxNum - 1].X;
                for (int i = 0; i < m_SampleMaxNum; i++)
                {
                    y += m_SamplePt[i].Y;
                    //x += m_SamplePt[i].X;
                    m_SamplePt[i].Y = 0;
                    m_SamplePt[i].X = 0;
                }
                pt.X = x;
                pt.Y = y / m_SampleMaxNum;
                m_SamplePtCount = 0;
                if (m_PtCount >= 2 || m_PtCount <= 0)
                {
                    m_PtCount = 0;
                }
                m_PointF[m_PtCount].X = pt.X;
                m_PointF[m_PtCount].Y = pt.Y;
                m_PtCount++;
            }

            if (TotalTime >= m_xMax)
            {
                m_xMax += 30;
            }

            if (m_isFirstSapmle)
            {
                m_PointF[0].Y = 0.01f;
                m_isFirstSapmle = false;
            }

            if (m_PtCount >= 2)
            {
                m_DrawCurveHandle.SaveSourcePointF(m_PointF);
                m_DrawCurveHandle.DrawCurve(panel_Curve, Color.Red, m_xMax, m_yMax);
                m_PtCount = 0;
            }

            if (m_BoobPressure >= PressureValue)
            {
                m_BoobPressure = PressureValue;
            }

            int H = 0;
            int M = 0;
            int S = 0;
            int second = (int)m_xMax + 1;
            DateTime startTime = m_StartTime;
            label6.Text = startTime.ToString("HH:mm:ss");

            GetlableTime(startTime, second / 6, ref H, ref M, ref S);
            label7.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 2 / 6, ref H, ref M, ref S);
            label8.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 3 / 6, ref H, ref M, ref S);
            label9.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 4 / 6, ref H, ref M, ref S);
            label10.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 5 / 6, ref H, ref M, ref S);
            label11.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            GetlableTime(startTime, second * 6 / 6, ref H, ref M, ref S);
            label12.Text = H.ToString() + ":" + M.ToString() + ":" + S.ToString();

            //byte stableState = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Stable", ref stableState, ref dt);
            //if (stableState == 1)
            //{
            //    float stableTime = 0.0f;
            //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_StableTime", ref stableTime, ref dt);
            //    textBox_ActualStableTime.Text = stableTime.ToString("0.00");
            //}

            //byte KeepState = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Keep", ref KeepState, ref dt);
            //if (KeepState == 1)
            //{
            //    float KeepTime = 0.0f;
            //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_KeepTime", ref KeepTime, ref dt);
            //    textBox_ActualStableTime.Text = KeepTime.ToString("0.00");
            //}

            //byte StageFinish = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StageFinish", ref StageFinish, ref dt);
            //if (StageFinish == 1)
            //{
            //    SaveBoobDataToStruct();
            //    m_Stage++;
            //    StageFinish = 0;
            //    m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StageFinish", StageFinish);
            //}  

        }

        //private bool m_BoobStableState = false;
        //private bool m_BoobKeepState = false;
        //private bool m_BoobStageFinish = false;
        private void SampleBoobTestSystemState()
        {
            DateTime dt = default(DateTime);
            //byte stableState = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Stable", ref stableState, ref dt);
            //if (stableState == 1 && m_BoobStableState == false)
            //{
            //    float stableTime = 0.0f;
            //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_StableTime", ref stableTime, ref dt);
            //    textBox_ActualStableTime.Text = stableTime.ToString("0.00");
            //    m_BoobStableState = true;
            //}
            //if (stableState == 0 && m_BoobStableState == true)
            //{
            //    m_BoobStableState = false;
            //}

            //byte KeepState = 0;
            //m_MainFormHandle.m_PLCCommHandle.ReadData("Air_State_Keep", ref KeepState, ref dt);
            //if (KeepState == 1 && m_BoobKeepState == false)
            //{
            //    float KeepTime = 0.0f;
            //    m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_KeepTime", ref KeepTime, ref dt);
            //    textBox_ActualStableTime.Text = KeepTime.ToString("0.00");
            //    m_BoobKeepState = true;
            //}

            float KeepTime = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Actual_KeepTime", ref KeepTime, ref dt);
            textBox_ActualStableTime.Text = KeepTime.ToString("0.00");

            byte StageFinish = 0;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StageFinish", ref StageFinish, ref dt);
            if (StageFinish == 1 )//&& m_BoobStageFinish == false)
            {
                SaveBoobDataToStruct();
                m_Stage++;
                StageFinish = 0;
                m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StageFinish", StageFinish);

                m_ActualPreRaiseStartTime = DateTime.Now;
                m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref m_ActualPressure, ref dt);
                m_isCalActualPreRaise = true;
                //m_BoobStageFinish = true;
            }
            //if (StageFinish == 0 && m_BoobStageFinish == true)
            //{
            //    m_BoobStageFinish = false;
            //}
        }

        private void SaveBoobDataToStruct()
        {
            DateTime dt = default(DateTime);
            float InitPressure = 0.0f;
            m_MainFormHandle.m_PLCCommHandle.ReadData("Air_InitPressure", ref InitPressure, ref dt);

            try
            {
                if (m_Stage == 1)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre1.Text);
                    float PressureRate1 = Convert.ToSingle(textBox_PressureRate1.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime1.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime1.Text);
                    m_BoobTestSaveDataStructs.Add(new BoobTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             PressureRate1));
                }

                if (m_Stage == 2)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre2.Text);
                    float PressureRate2 = Convert.ToSingle(textBox_PressureRate2.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime2.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime2.Text);
                    m_BoobTestSaveDataStructs.Add(new BoobTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             PressureRate2));
                }

                if (m_Stage == 3)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre3.Text);
                    float PressureRate3 = Convert.ToSingle(textBox_PressureRate3.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime3.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime3.Text);
                    m_BoobTestSaveDataStructs.Add(new BoobTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             PressureRate3));
                }

                if (m_Stage == 4)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre4.Text);
                    float PressureRate4 = Convert.ToSingle(textBox_PressureRate4.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime4.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime4.Text);
                    m_BoobTestSaveDataStructs.Add(new BoobTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             PressureRate4));
                }

                if (m_Stage == 5)
                {
                    float setPressure = Convert.ToSingle(textBox_SetPre5.Text);
                    float PressureRate5 = Convert.ToSingle(textBox_PressureRate5.Text);
                    m_StableTime = Convert.ToSingle(textBox_StableTime5.Text);
                    m_KeepTime = Convert.ToSingle(textBox_KeepTime5.Text);
                    m_BoobTestSaveDataStructs.Add(new BoobTestSaveDataStruct(m_Stage, setPressure, InitPressure, m_StableTime, m_KeepTime,
                                                                             PressureRate5));
                }
            }
            catch (Exception)
            {
                m_Stage--;
                return;
            }
            
        }

        private void SaveBoobDataToDataBase()
        {
            string TestNo = m_BoobTestNo + m_BoobTestNum.ToString();
            m_BoobTestNum++;

            int index = 0;
            m_MainFormHandle.m_AirBoobAccessHandle.AddNewRow(m_TableName, ref index);
            m_MainFormHandle.m_AirBoobAccessHandle.WriteSigleData(m_TableName, index, "TestNo", TestNo);

            m_MainFormHandle.m_AirBoobAccessHandle.WriteSigleData(m_TableName, index, "StartTime", m_StartTime);
            m_MainFormHandle.m_AirBoobAccessHandle.WriteSigleData(m_TableName, index, "yMax", m_yMax);

            //m_MainFormHandle.m_AirBoobAccessHandle.WriteSigleData(m_TableName, index, "SetPressure", m_SetPressure);
            m_MainFormHandle.m_AirBoobAccessHandle.WriteSigleData(m_TableName, index, "BurstPressure", m_BoobPressure);
            //m_PressureRate = Convert.ToSingle(textBox_PressureRate2.Text);
            //m_MainFormHandle.m_AirBoobAccessHandle.WriteSigleData(m_TableName, index, "PressureRate", m_PressureRate);

            int testDataNum = m_BoobTestSaveDataStructs.Count;
            m_MainFormHandle.m_AirBoobAccessHandle.WriteSigleData(m_TableName, index, "TestDataNum", testDataNum);

            float[] TestData = new float[testDataNum * 6];
            for (int i = 0; i < testDataNum; i++)
            {
                TestData[i * 6 + 1] = m_BoobTestSaveDataStructs[i].m_SetPressure;
                TestData[i * 6 + 2] = m_BoobTestSaveDataStructs[i].m_InitPressure;
                TestData[i * 6 + 3] = m_BoobTestSaveDataStructs[i].m_StableTime;
                TestData[i * 6 + 4] = m_BoobTestSaveDataStructs[i].m_KeepTime;
                TestData[i * 6 + 5] = m_BoobTestSaveDataStructs[i].m_PressureRate;
                TestData[i * 6 + 0] = m_BoobTestSaveDataStructs[i].m_Num;
            }

            m_MainFormHandle.m_AirBoobAccessHandle.WriteFloatsData(m_TableName, index, "TestData", TestData);

            PointF[] pt;
            m_DrawCurveHandle.GetSourcePointF(out pt);

            int len = pt.Length;
            if (len < 4)
            {
                m_MainFormHandle.m_AirBoobAccessHandle.DeleteRow(m_TableName, index);
                return;
            }

            float[] x = new float[len];
            float[] y = new float[len];

            for (int i = 0; i < len; i++)
            {
                x[i] = pt[i].X;
                y[i] = pt[i].Y;
            }
            m_MainFormHandle.m_AirBoobAccessHandle.WriteFloatsData(m_TableName, index, "Point_x", x);
            m_MainFormHandle.m_AirBoobAccessHandle.WriteFloatsData(m_TableName, index, "Point_y", y);
            m_MainFormHandle.m_AirBoobAccessHandle.SaveDataToBuffer();
            m_MainFormHandle.m_AirBoobAccessHandle.SaveDateToDataBase();
        }

        #endregion

        public bool m_SaveContinue = false;
        private void TB_SaveContinue_Click(object sender, EventArgs e)
        {
            m_SaveContinue = m_SaveContinue ? false : true;
            if (m_SaveContinue)
            {
                TB_SaveContinue.BackColor = Color.Green;
                TB_Save.Enabled = false;
                TB_CancelSave.Enabled = false;
            }
            else
            {
                TB_SaveContinue.BackColor = TB_CancelSave.BackColor;
                TB_Save.Enabled = true;
                TB_CancelSave.Enabled = true;
            }
        }



    }
}
