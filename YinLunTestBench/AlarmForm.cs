using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace YinLunTestBench
{
    public partial class AlarmForm : Form
    {
        private Form1 m_MainFormHandle;
        public event FormShowState ShowMainForm;
        private AirTestForm m_AirTestHandle = null;
        private FliudTestForm m_FliudTestHandle = null;
        public AlarmForm(Form1 handle)
        {
            InitializeComponent();
            m_MainFormHandle = handle;
            ShowMainForm += new FormShowState(m_MainFormHandle.FormStateShow);
        }

        public class AlarmStruct
        {
            public string m_StartTime;
            public string m_EndTime;
            public string m_AlarmName;
            public bool m_isCreatflag;
            public bool m_isEndFlag;
            public AlarmStruct(string startTime, string endTime, string Name, bool isCreat, bool isEnd)
            {
                m_StartTime = startTime;
                m_EndTime = endTime;
                m_AlarmName = Name;
                m_isCreatflag = isCreat;
                m_isEndFlag = isEnd;
            }
        }
        public List<AlarmStruct> m_AlarmStructs = new List<AlarmStruct>();
        private object O_LockAlarmStruct = new object();

        private string m_HistoryAlarmFilePath = Application.StartupPath + "\\HisAlarm.txt";

        private void AlarmForm_Load(object sender, EventArgs e)
        {
            AirTestForm.GetInstance(ref m_AirTestHandle);
            FliudTestForm.GetInstance(ref m_FliudTestHandle);
        }

        public void DisActualAlarm()
        {
            bool isCreat = false;
            bool isEnd = false;
            string AlarmContent = "";
            lock (O_LockAlarmStruct)
            {
                int count = m_AlarmStructs.Count;
                for (int i = 0; i < count; i++)
                {
                    isCreat = m_AlarmStructs[i].m_isCreatflag;
                    isEnd = m_AlarmStructs[i].m_isEndFlag;
                    if (isCreat == true && isEnd == false)
                    {
                        AlarmContent += m_AlarmStructs[i].m_StartTime + "          " + m_AlarmStructs[i].m_AlarmName + "\n";
                    }
                }
            }
            RB_Alarm_Dis.Text = AlarmContent;
        }

        public void PutAlarmToHistory()
        {
            bool isCreat = false;
            bool isEnd = false;
            string AlarmContent = "";
            lock (O_LockAlarmStruct)
            {
                //int count = m_AlarmStructs.Count;
                for (int i = 0; i < m_AlarmStructs.Count; i++)
                {
                    isCreat = m_AlarmStructs[i].m_isCreatflag;
                    isEnd = m_AlarmStructs[i].m_isEndFlag;
                    if (isCreat == true && isEnd == true)
                    {
                        AlarmContent += m_AlarmStructs[i].m_StartTime + "          " + 
                                        m_AlarmStructs[i].m_EndTime + "          " + 
                                        m_AlarmStructs[i].m_AlarmName + "\n";
                    }
                    m_AlarmStructs.RemoveAt(i);
                }
            }

            string s = "";

            //ReadLog(m_HistoryAlarmFilePath, ref s);
            s += AlarmContent;

            WriteLog(m_HistoryAlarmFilePath, s);
        }

        public void CreatAlarm(string startTime, string Name)
        {
            lock (O_LockAlarmStruct)
            {
                m_AlarmStructs.Add(new AlarmStruct(startTime, "", Name, true, false));
            }
        }

        public void ClearAlarm(string endTime, string Name)
        {
            int index = 0;
            lock (O_LockAlarmStruct)
            {
                index = m_AlarmStructs.FindIndex(r => r.m_AlarmName == Name);
            }
            if (index < 0)
            {
                return;
            }
            lock (O_LockAlarmStruct)
            {
                m_AlarmStructs[index].m_EndTime = endTime;
                m_AlarmStructs[index].m_isEndFlag = true;
            }

        }

        private void ShowPageInfo(string text)
        {
            if (ShowMainForm != null)
            {
                ShowMainForm(text);
            }
        }

        public void GetAirTestHandle(AirTestForm handle)
        {
            m_AirTestHandle = handle;
        }

        public void GetFluidTestHandle(FliudTestForm handle)
        {
            m_FliudTestHandle = handle;
        }

        public void WriteLog(string LogFileName, string s)
        {
            if (s == null)
            {
                return;
            }
            FileStream filestream = new FileStream(LogFileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(filestream, Encoding.GetEncoding("gb2312"));
            StreamReader sr = new StreamReader(filestream, Encoding.GetEncoding("gb2312"));
            string sTem = sr.ReadToEnd();
            //sw.WriteLine("{0}" + s, sTem);
            sw.WriteLine(s);

            sw.Flush();
            sw.Close();
            sr.Close();
            filestream.Close();
        }

        public void ReadLog(string LogFileName, ref string s)
        {
            FileStream filestream = new FileStream(LogFileName, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(filestream, Encoding.GetEncoding("gb2312"));
            string sTem = sr.ReadToEnd();
            s = sTem;
            sr.Close();
            filestream.Close();
        }

        private void TB_ActualAlarm_Click(object sender, EventArgs e)
        {
            DisActualAlarm();
        }

        private void TB_HistoryAlarm_Click(object sender, EventArgs e)
        {
            string s = "";
            ReadLog(m_HistoryAlarmFilePath, ref s);
            RB_Alarm_Dis.Text = s;
        }

        private void TB_Exit_Click(object sender, EventArgs e)
        {
            this.Hide();
            bool isTesting = false;
            m_AirTestHandle.GetTestInfo(ref isTesting);
            if (isTesting)
            {
                m_AirTestHandle.Show();
                ShowPageInfo("气压测试");
                return;
            }
            m_FliudTestHandle.GetTestInfo(ref isTesting);
            if (isTesting)
            {
                m_FliudTestHandle.Show();
                ShowPageInfo("液压测试");
                return;
            }
            m_MainFormHandle.Show();
            ShowPageInfo("主菜单");
            return;
        }
    }
}
