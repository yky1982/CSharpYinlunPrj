using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace YinLunTestBench
{
    public partial class AdjSensorForm : Form
    {
        public event FormShowState ShowMainForm; 
        private Form1 m_MainFormHandle;
        public AirTestForm m_AirTestFormHandle = null;
        public FliudTestForm m_FliudTestFormHandle = null;
        public AdjSensorForm(Form1 handle)
        {
            InitializeComponent();
            m_MainFormHandle = handle;
            ShowMainForm += new FormShowState(m_MainFormHandle.FormStateShow);
        }

        private System.Timers.Timer m_SampleSensorValueHandle = new System.Timers.Timer();
        private void AdjSensorForm_Load(object sender, EventArgs e)
        {
            AirTestForm.GetInstance(ref m_AirTestFormHandle);
            FliudTestForm.GetInstance(ref m_FliudTestFormHandle);

            m_SampleSensorValueHandle.AutoReset = true;
            m_SampleSensorValueHandle.Interval = 1000;
            m_SampleSensorValueHandle.Elapsed += new ElapsedEventHandler(SampleSensorDataFun);
            m_SampleSensorValueHandle.Start();

            ShowPageInfo("传感器校验");
        }

        private void SampleSensorDataFun(object o, ElapsedEventArgs e)
        {
            DateTime dt = default(DateTime);
            float Sensor_Boom = 0.0f;
            int code = m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_Boom", ref Sensor_Boom, ref dt);
            textBox_Bomb_Sensor.Text = Sensor_Boom.ToString("0.00");

            float Sensor_IncresePressure = 0.0f;
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_IncresePressure", ref Sensor_IncresePressure, ref dt);
            textBox_Fluid_Sensor.Text = Sensor_IncresePressure.ToString("0.00");

            float Sensor_FluidOut = 0.0f;
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_FluidOut", ref Sensor_FluidOut, ref dt);
            textBox_FluidOut.Text = Sensor_FluidOut.ToString("0.00");

            float Sensor_FluidPumpOut = 0.0f;
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_FluidPumpOut", ref Sensor_FluidPumpOut, ref dt);
            textBox_FluidPumpOut.Text = Sensor_FluidPumpOut.ToString("0.00");

            //float Sensor_FluidLever = 0.0f;
            //code = m_MainFormHandle.m_PLCCommHandle.ReadData("Sensor_FluidLever", ref Sensor_FluidLever, ref dt);
            //textBox_FluidLeverSensor.Text = Sensor_FluidLever.ToString("0.00");

            ShowPageInfo("传感器校验");
            
        }

        private void ShowPageInfo(string text)
        {
            if (ShowMainForm != null)
            {
                ShowMainForm(text);
            }
        }

        private void button_Adj_Click(object sender, EventArgs e)
        {
            byte adj = 1;
            int code = m_MainFormHandle.m_PLCCommHandle.WriteData("PLC_Sensor_Adj", adj);
            if (code != 1)
            {
                MessageBox.Show("校验失败，错误代码：" + code.ToString() + "!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MessageBox.Show("校验成功", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button_Return_Click(object sender, EventArgs e)
        {
            m_SampleSensorValueHandle.Stop();

            this.Hide();

            bool isTesting = false;
            m_AirTestFormHandle.GetTestInfo(ref isTesting);
            if (isTesting)
            {
                m_AirTestFormHandle.Show();
                ShowPageInfo("气压测试");
                return;
            }
            m_FliudTestFormHandle.GetTestInfo(ref isTesting);
            if (isTesting)
            {
                m_FliudTestFormHandle.Show();
                ShowPageInfo("液压测试");
                return;
            }

            m_MainFormHandle.Show();
            ShowPageInfo("主菜单");

            //this.Dispose();
        }
    }
}
