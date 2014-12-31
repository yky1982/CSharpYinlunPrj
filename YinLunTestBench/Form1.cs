using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Timers;
using XMLConfigPLC200SampleData;
using AccessDLL__AddBuffer;

namespace YinLunTestBench
{

    public delegate void UserLoginState(string text);
    public delegate void FormShowState(string text);//状态栏中的窗口显示信息
    public delegate void RunningState(string text, Color color);//状态栏中的窗口运行信息
    public partial class Form1 : Form
    {

        #region CommDLL
        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "test", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void test(ref int RevCount, ref int SendCount, ref int SendFailCount, ref int frameNumfail);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "test2", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void test2(ref int Count, ref int d300ms, ref int d1s, ref int d5s);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "InterfaceInit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int InterfaceInit();

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "Dispose", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern new int Dispose();

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "XMLLoad", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int XMLLoad(string FileName);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "Connect", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Connect(string GuestIP, int Port);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "Start", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Start();

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "Stop", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Stop();

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "Suspend", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Suspend();

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "Resume", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Resume();

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadData(string RegName, ref bool data, ref DateTime dt);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadData(string RegName, ref byte data, ref DateTime dt);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadData(string RegName, ref sbyte data, ref DateTime dt);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadData(string RegName, ref Int16 data, ref DateTime dt);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadData(string RegName, ref UInt16 data, ref DateTime dt);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadData(string RegName, ref Int32 data, ref DateTime dt);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadData(string RegName, ref UInt32 data, ref DateTime dt);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadData(string RegName, ref float data, ref DateTime dt);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadOneData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOneData(string Regname, ref bool data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadOneData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOneData(string Regname, ref byte data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadOneData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOneData(string Regname, ref sbyte data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadOneData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOneData(string Regname, ref Int16 data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadOneData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOneData(string Regname, ref UInt16 data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadOneData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOneData(string Regname, ref Int32 data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadOneData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOneData(string Regname, ref UInt32 data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadOneData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOneData(string Regname, ref float data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReadDataInfo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadDataInfo(string Regname, ref bool RW, ref string DataType);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteData(string RegName, bool Data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteData(string RegName, byte Data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteData(string RegName, sbyte Data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteData(string RegName, Int16 Data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteData(string RegName, UInt16 Data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteData(string RegName, Int32 Data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteData(string RegName, UInt32 Data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "WriteData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteData(string RegName, float Data);

        [DllImport("XMLConfigPLC200SampleData.dll", EntryPoint = "ReportComState", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReportComState(ref bool state);


        #endregion 

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

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "WriteFloatsToExistCells", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteFloatsToExistCells(string TableName, int index, string ColName, float[] data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "WriteIntsToExistCells", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteIntsToExistCells(string TableName, int index, string ColName, int[] data);

        [DllImport("AccessDLL__AddBuffer.dll", EntryPoint = "WriteSigleDataToExistCells", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteSigleDataToExistCells(string TableName, int index, string ColName, object data);

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


        public Form1()
        {
            InitializeComponent();
        }

        public AccessDLL__AddBuffer.AccessLibInterface m_AirBoobAccessHandle = new AccessLibInterface();
        private string m_AirBoobQueryFilePath = Application.StartupPath + @"\AirBoom\AirBoobDataBase.mdb";//获取INI文件路径
        public AccessDLL__AddBuffer.AccessLibInterface m_AirKeepAccessHandle = new AccessLibInterface();
        private string m_AirKeepQueryFilePath = Application.StartupPath + @"\AirKeep\AirKeepDataBase.mdb";//获取INI文件路径
        public AccessDLL__AddBuffer.AccessLibInterface m_FluidBoobAccessHandle = new AccessLibInterface();
        private string m_FluidBoobQueryFilePath = Application.StartupPath + @"\FluidBoom\FluidBoobDataBase.mdb";//获取INI文件路径
        public AccessDLL__AddBuffer.AccessLibInterface m_FluidKeepAccessHandle = new AccessLibInterface();
        private string m_FluidKeepQueryFilePath = Application.StartupPath + @"\FluidKeep\FluidKeepDataBase.mdb";//获取INI文件路径
        private string m_TableName = "TestResult";

        public DLLInf m_PLCCommHandle = new DLLInf();
        public string m_PLCIPAddress = @"192.168.1.188";
        public int m_PLCPort = 102;
        private string m_XMLFilePath = Application.StartupPath + "\\xmlConfig.xml";
        public bool m_isComFalt = false;
        public static Form1 m_SelfHandle = null;

        public Form[] m_FormHandles;
        private int m_FormIndex = 99;

        private System.Timers.Timer m_SampleAlarmInfoTimer = new System.Timers.Timer();
        private System.Timers.Timer m_SetBackgroudTimer = new System.Timers.Timer();
        private AlarmForm m_AlarmFormHandle;
        public AirTestForm m_AirTestFormHandle = null;
        public FliudTestForm m_FliudTestFormHandle = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            PLCComInit();
            AllFormInit();

            Form.CheckForIllegalCrossThreadCalls = false;

            m_SampleAlarmInfoTimer.AutoReset = true;
            m_SampleAlarmInfoTimer.Interval = 1000;
            m_SampleAlarmInfoTimer.Elapsed += new ElapsedEventHandler(SampleAlarmInfoFun);
            m_SampleAlarmInfoTimer.Start();

            m_SetBackgroudTimer.AutoReset = true;
            m_SetBackgroudTimer.Interval = 1000;
            m_SetBackgroudTimer.Elapsed += new ElapsedEventHandler(SetBackGroudFun);
            

            comboBox_UserName.Items.Add("admin");
            comboBox_UserName.SelectedIndex = 0;

            StatusLabel_LogInUser.Text = "当前用户：无";
            StatusLabel_PageInfo.Text = "登录界面";
            textBox_Password.PasswordChar = '*';
            Grp_DisArea.Visible = false;

            MenuItem_UserLogOut.Enabled = false;
            MenuItem_UserLogIn.Enabled = true;
            MenuItem_Practice.Enabled = false;
            MenuItem_Maintance.Enabled = false;
            MenuItem_SystemDiscrition.Enabled = false;
            MenuItem_DataBase.Enabled = false;

            PicBox_BackGroud.Visible = true;

            Grp_Login.Visible = false;

            m_AlarmFormHandle = new AlarmForm(this);
            m_AlarmFormHandle.TopLevel = false;
            Grp_DisArea.Controls.Add(m_AlarmFormHandle);

            m_SelfHandle = this;

            AirTestForm.GetInstance(ref m_AirTestFormHandle);
            FliudTestForm.GetInstance(ref m_FliudTestFormHandle);
            m_AirBoobAccessHandle.Init(m_AirBoobQueryFilePath, m_TableName, 1);
            m_AirKeepAccessHandle.Init(m_AirKeepQueryFilePath, m_TableName, 1);
            m_FluidBoobAccessHandle.Init(m_FluidBoobQueryFilePath, m_TableName, 1);
            m_FluidKeepAccessHandle.Init(m_FluidKeepQueryFilePath, m_TableName, 1);

            m_SetBackgroudTimer.Start();
        }

        private void AllFormInit()
        {
            m_FormHandles = new Form[] { new SystemDescripForm(this), new AirPressTestSetting(this,Grp_DisArea), 
                                         new FluidPressTestSetting(this,Grp_DisArea),new AirBoobQuery(this), 
                                         new AirKeepQuery(this), new FliudBoomQuery(this), new FliudKeepQuery(this), 
                                         new AlarmForm(this), new AdjSensorForm(this) };

            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                m_FormHandles[i].TopLevel = false;
                Grp_DisArea.Controls.Add(m_FormHandles[i]);
                //m_FormHandles[i].WindowState = FormWindowState.Maximized;
            }
            //m_FormHandles[9].
            return;
        }

        //采集告警信号
        private bool A_isCommAlarmCreat = false;
        private bool A_isCommAlarmClear = true;
        private bool A_isEmergencyAlarmCreat = false;
        private bool A_isEmergencyAlarmClear = true;
        private bool A_isFluidLeakAlarmCreat = false;
        private bool A_isFluidLeakAlarmClear = true;
        private bool A_isAirLeakAlarmCreat = false;
        private bool A_isAirLeakAlarmClear = true;
        private bool A_isDoorOpenCreat = false;
        private bool A_isDoorOpenClear = true;
        private int m_AlarmColorChange = 0;
        private void SampleAlarmInfoFun(object o, ElapsedEventArgs e)
        {
            string CommName = "通信故障";
            string EmergencyStop = "紧急停止";
            string AirTestLeak = "气压测试泄漏";
            string FluidTestLeak = "液压测试泄漏";
            string DoorOpen = "试验台门开启";

            string sdt = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
            bool comState = false;
            m_PLCCommHandle.ReportComState(ref comState);
            if (comState && A_isCommAlarmCreat == false && A_isCommAlarmClear == true)
            {

                m_AlarmFormHandle.CreatAlarm(sdt, CommName);
                A_isCommAlarmCreat = true;
                A_isCommAlarmClear = false;
            }
            if (comState == false && A_isCommAlarmCreat == true && A_isCommAlarmClear == false)
            {

                m_AlarmFormHandle.ClearAlarm(sdt, CommName);
                A_isCommAlarmCreat = false;
                A_isCommAlarmClear = true;
                m_AlarmFormHandle.PutAlarmToHistory();
            }

            byte isEmergencyFail = 0;
            DateTime dt = default(DateTime);
            m_PLCCommHandle.ReadData("Alarm_EmergencyStop", ref isEmergencyFail, ref dt);
            if (isEmergencyFail == 1 && A_isEmergencyAlarmCreat == false && A_isEmergencyAlarmClear == true)
            {

                m_AlarmFormHandle.CreatAlarm(sdt, EmergencyStop);
                A_isEmergencyAlarmCreat = true;
                A_isEmergencyAlarmClear = false;
            }
            if (isEmergencyFail == 0 && A_isEmergencyAlarmCreat == true && A_isEmergencyAlarmClear == false)
            {

                m_AlarmFormHandle.ClearAlarm(sdt, EmergencyStop);
                A_isEmergencyAlarmCreat = false;
                A_isEmergencyAlarmClear = true;
                m_AlarmFormHandle.PutAlarmToHistory();
            }

            byte isFluidLeakFail = 0;
            m_PLCCommHandle.ReadData("Alarm_Fluid_TestLeak", ref isFluidLeakFail, ref dt);
            if (isFluidLeakFail == 1 && A_isFluidLeakAlarmCreat == false && A_isFluidLeakAlarmClear == true)
            {

                m_AlarmFormHandle.CreatAlarm(sdt, FluidTestLeak);
                A_isFluidLeakAlarmCreat = true;
                A_isFluidLeakAlarmClear = false;
            }
            if (isFluidLeakFail == 0 && A_isFluidLeakAlarmCreat == true && A_isFluidLeakAlarmClear == false)
            {

                m_AlarmFormHandle.ClearAlarm(sdt, FluidTestLeak);
                A_isFluidLeakAlarmCreat = false;
                A_isFluidLeakAlarmClear = true;
                m_AlarmFormHandle.PutAlarmToHistory();
            }

            byte isAirLeakFail = 0;
            m_PLCCommHandle.ReadData("Alarm_Air_TestLeak", ref isAirLeakFail, ref dt);
            if (isAirLeakFail == 1 && A_isAirLeakAlarmCreat == false && A_isAirLeakAlarmClear == true)
            {

                m_AlarmFormHandle.CreatAlarm(sdt, AirTestLeak);
                A_isAirLeakAlarmCreat = true;
                A_isAirLeakAlarmClear = false;
            }
            if (isAirLeakFail == 0 && A_isAirLeakAlarmCreat == true && A_isAirLeakAlarmClear == false)
            {

                m_AlarmFormHandle.ClearAlarm(sdt, AirTestLeak);
                A_isAirLeakAlarmCreat = false;
                A_isAirLeakAlarmClear = true;
                m_AlarmFormHandle.PutAlarmToHistory();
            }

            byte isDoorOpen = 0;
            m_PLCCommHandle.ReadData("Alarm_Door", ref isDoorOpen, ref dt);
            if (isDoorOpen == 1 && A_isDoorOpenCreat == false && A_isDoorOpenClear == true)
            {

                m_AlarmFormHandle.CreatAlarm(sdt, DoorOpen);
                A_isDoorOpenCreat = true;
                A_isDoorOpenClear = false;
            }
            if (isDoorOpen == 0 && A_isDoorOpenCreat == true && A_isDoorOpenClear == false)
            {

                m_AlarmFormHandle.ClearAlarm(sdt, DoorOpen);
                A_isDoorOpenCreat = false;
                A_isDoorOpenClear = true;
                m_AlarmFormHandle.PutAlarmToHistory();
            }

            Color col = default(Color);
            m_AlarmColorChange++;
            if (comState == true || isAirLeakFail == 1 || isFluidLeakFail == 1 || isEmergencyFail == 1)
            {
                if (m_AlarmColorChange == 1)
                {
                    col = Color.Red;                   
                }
                if (m_AlarmColorChange >= 2)
                {
                    col = Color.LightYellow;
                    m_AlarmColorChange = 0;
                }

                RunningStateShow("警报，请查看告警", Color.Black);
                StatusLabel_DisSysStatus.BackColor = col;
            }
            else
            {
                RunningStateShow("运行", Color.Black);
                StatusLabel_DisSysStatus.BackColor = StatusLabel_PageInfo.BackColor;
            }


        }

        private void SetBackGroudFun(object o, ElapsedEventArgs e)
        {
            if (m_AirTestFormHandle.m_isTestingInt || m_FliudTestFormHandle.m_isTestingInt)
            {
                MenuItem_SystemDiscrition.Enabled = false;
                MenuItem_Practice.Enabled = false;
                MenuItem_DataBase.Enabled = false;
                MenuItem_SensorAdj.Enabled = false;
                MenuItem_ResetPLC.Enabled = false;
            }

            if (m_AirTestFormHandle.m_isTestingInt == false && m_FliudTestFormHandle.m_isTestingInt == false)
            {
                MenuItem_SystemDiscrition.Enabled = true;
                MenuItem_Practice.Enabled = true;
                MenuItem_DataBase.Enabled = true;
                MenuItem_SensorAdj.Enabled = true;
                MenuItem_ResetPLC.Enabled = true;
            }
            StatusLabel_DisTime.Text = "当前时间: " + DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss");
        }

        private void PLCComInit()
        {
            int code = m_PLCCommHandle.InterfaceInit();
            if (code != 1)
            {
                MessageBox.Show("Plc初始化错误! 错误代码：" + code.ToString(), "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            code = m_PLCCommHandle.XMLLoad(m_XMLFilePath);
            if (code != 1)
            {
                MessageBox.Show("XML初始化错误! 错误代码：" + code.ToString(), "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            code = m_PLCCommHandle.Connect(m_PLCIPAddress, m_PLCPort);
            if (code != 1)
            {
                code = m_PLCCommHandle.Connect(m_PLCIPAddress, m_PLCPort);
                if (code != 1)
                {
                    MessageBox.Show("连接错误! 错误代码：" + code.ToString(), "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    m_isComFalt = true;
                    RunningStateShow("告警", Color.Red);
                    return;
                }
            }

            m_PLCCommHandle.Start();
            m_PLCCommHandle.Resume();

        }


        private void RunningStateShow(string text, Color color)
        {
            StatusLabel_DisSysStatus.ForeColor = color;
            StatusLabel_DisSysStatus.Text = text;
        }

        public void FormStateShow(string text)
        {
            StatusLabel_PageInfo.Text = text;
            if (text == "主菜单")
            {
                PicBox_BackGroud.Visible = true;
            }
        }

        private void MenuBar_SystemDiscrip_Click(object sender, EventArgs e)
        {
            m_FormIndex = 0;
            m_AirTestFormHandle.Hide();
            m_FliudTestFormHandle.Hide();
            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                if (i == m_FormIndex)
                {
                    m_FormHandles[i].Show();
                }
                else
                {
                    m_FormHandles[i].Hide();
                }
            }

            FormStateShow("系统描述");

            PicBox_BackGroud.Visible = false;
        }

        private void button_Log_Sure_Click(object sender, EventArgs e)
        {
            string s = comboBox_UserName.Text;
            string p = textBox_Password.Text;
            if (s == "admin" && p == "8888")
            {
                Grp_Login.Visible = false;
                MenuItem_UserLogOut.Enabled = true;
                MenuItem_UserLogIn.Enabled = false;
                MenuItem_Practice.Enabled = true;
                MenuItem_Maintance.Enabled = true;
                MenuItem_SystemDiscrition.Enabled = true;
                MenuItem_DataBase.Enabled = true;
                textBox_Password.Text = "";
                StatusLabel_LogInUser.Text = "当前用户：Admin";
                StatusLabel_PageInfo.Text = "主界面";
                Grp_DisArea.Visible = true;
                //m_FormHandles[0].Show();
            }
            else
            {
                MessageBox.Show("登录密码错误", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_Password.Text = "";
                textBox_Password.Focus();
                return;
            }
        }

        private void MenuItem_UserLogIn_Click(object sender, EventArgs e)
        {
            Grp_Login.Visible = true;
            textBox_Password.Focus();
        }

        private void MenuItem_UserLogOut_Click(object sender, EventArgs e)
        {
            DialogResult MsgBoxResult;//设置对话框的返回值
            MsgBoxResult = MessageBox.Show("确定注销本用户吗？",//对话框的显示内容
            "提示",//对话框的标题
            MessageBoxButtons.YesNo,//定义对话框的按钮，这里定义了YSE和NO两个按钮
            MessageBoxIcon.Exclamation,//定义对话框内的图表式样，这里是一个黄色三角型内加一个感叹号
            MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
            if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）
            {
                MenuItem_Practice.Enabled = false;
                MenuItem_UserLogIn.Enabled = true;
                MenuItem_UserLogOut.Enabled = false;
                MenuItem_SystemDiscrition.Enabled = false;
                MenuItem_Maintance.Enabled = false;
                MenuItem_DataBase.Enabled = false;
                Grp_DisArea.Visible = false;
                MenuItem_Maintance.Enabled = false;
                StatusLabel_LogInUser.Text = "当前用户：无";
                FormStateShow("登录界面");
            }
            if (MsgBoxResult == DialogResult.No)//如果对话框的返回值是NO（按"N"按钮）
            {

            }
        }

        private void MenuItem_UserQuit_Click(object sender, EventArgs e)
        {
            DialogResult MsgBoxResult;//设置对话框的返回值
            MsgBoxResult = MessageBox.Show("确定退出本程序吗？",//对话框的显示内容
            "提示",//对话框的标题
            MessageBoxButtons.YesNo,//定义对话框的按钮，这里定义了YSE和NO两个按钮
            MessageBoxIcon.Exclamation,//定义对话框内的图表式样，这里是一个黄色三角型内加一个感叹号
            MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
            if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）
            {
                Application.Exit();
            }
            if (MsgBoxResult == DialogResult.No)//如果对话框的返回值是NO（按"N"按钮）
            {

            }
        }

        private void MenuItem_About_Click(object sender, EventArgs e)
        {
            AboutDisForm handle = new AboutDisForm();
            handle.ShowDialog();

            handle.Close();
            handle.Dispose();
        
        }

        private void MenuItem_SensorAdj_Click(object sender, EventArgs e)
        {
            AdjSensorForm handle = new AdjSensorForm(this);
            handle.TopLevel = false;
            Grp_DisArea.Controls.Add(handle);
            handle.Show();

            PicBox_BackGroud.Visible = false;
        }

        private void MenuItem_ResetPLC_Click(object sender, EventArgs e)
        {
            byte resetValue = 1;
            int code = m_PLCCommHandle.WriteData("PLC_Reset", resetValue);
            if (code != 1)
            {
                MessageBox.Show("校验失败，错误代码：" + code.ToString() + "!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MessageBox.Show("恢复出厂设置成功", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuItem_Alarm_Click(object sender, EventArgs e)
        {
            m_FormIndex = 7;
            m_AirTestFormHandle.Hide();
            m_FliudTestFormHandle.Hide();
            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                if (i == m_FormIndex)
                {
                    //m_FormHandles[i].Show();
                    m_AlarmFormHandle.Show();
                }
                else
                {
                    m_FormHandles[i].Hide();
                }
            }
            PicBox_BackGroud.Visible = false;
            FormStateShow("告警");
            m_AlarmFormHandle.DisActualAlarm();


        }

        private void MenuItem_AirSetting_Click(object sender, EventArgs e)
        {
            m_FormIndex = 1;
            m_AirTestFormHandle.Hide();
            m_FliudTestFormHandle.Hide();
            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                if (i == m_FormIndex)
                {
                    m_FormHandles[i].Show();
                }
                else
                {
                    m_FormHandles[i].Hide();
                }
            }

            FormStateShow("气压设置");

            PicBox_BackGroud.Visible = false;
        }

        private void MenuItem_FluidSettig_Click(object sender, EventArgs e)
        {
            m_FormIndex = 2;
            m_AirTestFormHandle.Hide();
            m_FliudTestFormHandle.Hide();
            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                if (i == m_FormIndex)
                {
                    m_FormHandles[i].Show();
                }
                else
                {
                    m_FormHandles[i].Hide();
                }
            }

            FormStateShow("液压设置");

            PicBox_BackGroud.Visible = false;
        }

        private void MenuItem_AirBoomDataBase_Click(object sender, EventArgs e)
        {
            m_FormIndex = 3;
            m_AirTestFormHandle.Hide();
            m_FliudTestFormHandle.Hide();
            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                if (i == m_FormIndex)
                {
                    m_FormHandles[i].Show();
                    AirBoobQuery Handle = (AirBoobQuery)m_FormHandles[i];
                    Handle.RefreshDataBase();
                }
                else
                {
                    m_FormHandles[i].Hide();
                }
            }

            FormStateShow("气压爆破数据查询");

            PicBox_BackGroud.Visible = false;
        }

        private void MenuItem_AirKeepDataBase_Click(object sender, EventArgs e)
        {
            m_FormIndex = 4;
            m_AirTestFormHandle.Hide();
            m_FliudTestFormHandle.Hide();
            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                if (i == m_FormIndex)
                {
                    m_FormHandles[i].Show();
                    AirKeepQuery Handle = (AirKeepQuery)m_FormHandles[i];
                    Handle.RefreshDataBase();
                }
                else
                {
                    m_FormHandles[i].Hide();
                }
            }

            FormStateShow("气压静压数据查询");

            PicBox_BackGroud.Visible = false;
        }

        private void MenuItem_FluidBoomDataBase_Click(object sender, EventArgs e)
        {
            m_FormIndex = 5;
            m_AirTestFormHandle.Hide();
            m_FliudTestFormHandle.Hide();
            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                if (i == m_FormIndex)
                {
                    m_FormHandles[i].Show();
                    FliudBoomQuery Handle = (FliudBoomQuery)m_FormHandles[i];
                    Handle.RefreshDataBase();
                }
                else
                {
                    m_FormHandles[i].Hide();
                }
            }

            FormStateShow("液压爆破数据查询");

            PicBox_BackGroud.Visible = false;
        }

        private void MenuItem_KeepDataBase_Click(object sender, EventArgs e)
        {
            m_FormIndex = 6;
            m_AirTestFormHandle.Hide();
            m_FliudTestFormHandle.Hide();
            for (int i = 0; i < m_FormHandles.Length; i++)
            {
                if (i == m_FormIndex)
                {
                    m_FormHandles[i].Show();
                    FliudKeepQuery Handle = (FliudKeepQuery)m_FormHandles[i];
                    Handle.RefreshDataBase();
                }
                else
                {
                    m_FormHandles[i].Hide();
                }
            }

            FormStateShow("液压静压数据查询");

            PicBox_BackGroud.Visible = false;
        }

        private void button_Log_Cancel_Click(object sender, EventArgs e)
        {
            Grp_Login.Visible = false;
        }




    }
}
