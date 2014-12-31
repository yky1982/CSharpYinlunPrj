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

namespace YinLunTestBench
{
    public partial class AirPressTestSetting : Form
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);


        private Form1 m_MainFormHandle;
        public event FormShowState ShowMainForm;
        private GroupBox m_Grp;
        private string m_KeepstrFilePath = Application.StartupPath + @"\AirKeepTest.ini";//获取INI文件路径
        private string m_KeepstrSec = "AirKeepTest"; //INI文件名
        private string m_BoomstrFilePath = Application.StartupPath + @"\AirBoomTest.ini";//获取INI文件路径
        private string m_BoomstrSec = "AirBoomTest"; //INI文件名

        public AirTestForm m_AirTestFormHandle = null;
        public FliudTestForm m_FliudTestFormHandle = null;
        public AirPressTestSetting(Form1 handle, GroupBox grp)
        {
            InitializeComponent();
            m_MainFormHandle = handle;
            m_Grp = grp;
            ShowMainForm += new FormShowState(m_MainFormHandle.FormStateShow);
        }

        private void AirPressTestSetting_Load(object sender, EventArgs e)
        {
            ShowMainFormInfo("气压测试设置");
            AirTestForm.GetInstance(ref m_AirTestFormHandle);
            FliudTestForm.GetInstance(ref m_FliudTestFormHandle);

            Grp_Boom.Enabled = false;
            Grp_KeepPressure.Enabled = false;

            int InitDate = 0;

            checkBox_Keep_Chanel1.CheckState = CheckState.Checked;
            checkBox_Keep_Chanel2.CheckState = CheckState.Checked;
            checkBox_Keep_Chanel3.CheckState = CheckState.Checked;
            checkBox_Keep_Chanel4.CheckState = CheckState.Checked;
            checkBox_Keep_Chanel5.CheckState = CheckState.Checked;

            textBox_Keep_KeepTime1.Text = InitDate.ToString();
            textBox_Keep_KeepPressure1.Text = InitDate.ToString();
            textBox_Keep_DropPressure1.Text = InitDate.ToString();
            textBox_Keep_StableTime1.Text = InitDate.ToString();

            textBox_Keep_KeepTime2.Text = InitDate.ToString();
            textBox_Keep_KeepPressure2.Text = InitDate.ToString();
            textBox_Keep_DropPressure2.Text = InitDate.ToString();
            textBox_Keep_StableTime2.Text = InitDate.ToString();

            textBox_Keep_KeepTime3.Text = InitDate.ToString();
            textBox_Keep_KeepPressure3.Text = InitDate.ToString();
            textBox_Keep_DropPressure3.Text = InitDate.ToString();
            textBox_Keep_StableTime3.Text = InitDate.ToString();

            textBox_Keep_KeepTime4.Text = InitDate.ToString();
            textBox_Keep_KeepPressure4.Text = InitDate.ToString();
            textBox_Keep_DropPressure4.Text = InitDate.ToString();
            textBox_Keep_StableTime4.Text = InitDate.ToString();

            textBox_Keep_KeepTime5.Text = InitDate.ToString();
            textBox_Keep_KeepPressure5.Text = InitDate.ToString();
            textBox_Keep_DropPressure5.Text = InitDate.ToString();
            textBox_Keep_StableTime5.Text = InitDate.ToString();

            textBox_Keep_RaiseRate1.Text = InitDate.ToString();
            textBox_Keep_RaiseRate2.Text = InitDate.ToString();
            textBox_Keep_RaiseRate3.Text = InitDate.ToString();
            textBox_Keep_RaiseRate4.Text = InitDate.ToString();
            textBox_Keep_RaiseRate5.Text = InitDate.ToString();
            textBox_Keep_TestNo.Text = ContentValue(m_KeepstrSec, "TestNo", m_KeepstrFilePath);

            checkBox_Boob_Chanel1.CheckState = CheckState.Checked;
            checkBox_Boob_Chanel2.CheckState = CheckState.Checked;
            checkBox_Boob_Chanel3.CheckState = CheckState.Checked;
            checkBox_Boob_Chanel4.CheckState = CheckState.Checked;
            checkBox_Boob_Chanel5.CheckState = CheckState.Checked;

            textBox_Boob_KeepTime1.Text = InitDate.ToString();
            textBox_Boom_KeepPressure1.Text = InitDate.ToString();
            textBox_Boom_DropPressure1.Text = InitDate.ToString();
            textBox_Boob_StableTime1.Text = InitDate.ToString();

            textBox_Boob_KeepTime2.Text = InitDate.ToString();
            textBox_Boom_KeepPressure2.Text = InitDate.ToString();
            textBox_Boom_DropPressure2.Text = InitDate.ToString();
            textBox_Boob_StableTime2.Text = InitDate.ToString();

            textBox_Boob_KeepTime3.Text = InitDate.ToString();
            textBox_Boom_KeepPressure3.Text = InitDate.ToString();
            textBox_Boom_DropPressure3.Text = InitDate.ToString();
            textBox_Boob_StableTime3.Text = InitDate.ToString();

            textBox_Boob_KeepTime4.Text = InitDate.ToString();
            textBox_Boom_KeepPressure4.Text = InitDate.ToString();
            textBox_Boom_DropPressure4.Text = InitDate.ToString();
            textBox_Boob_StableTime4.Text = InitDate.ToString();

            textBox_Boob_KeepTime5.Text = InitDate.ToString();
            textBox_Boom_KeepPressure5.Text = InitDate.ToString();
            textBox_Boom_DropPressure5.Text = InitDate.ToString();
            textBox_Boob_StableTime5.Text = InitDate.ToString();

            textBox_Boob_RaiseRate1.Text = InitDate.ToString();
            textBox_Boob_RaiseRate2.Text = InitDate.ToString();
            textBox_Boob_RaiseRate3.Text = InitDate.ToString();
            textBox_Boob_RaiseRate4.Text = InitDate.ToString();
            textBox_Boob_RaiseRate5.Text = InitDate.ToString();
            textBox_Boob_TestNo.Text = ContentValue(m_BoomstrSec, "TestNo", m_BoomstrFilePath);



        }

        private void ShowMainFormInfo(string text)
        {
            if (ShowMainForm != null)
            {
                ShowMainForm(text);
            }
        }

        public void iDispose()
        {
            m_AirTestFormHandle = null;
        }

        private string ContentValue(string Section, string key, string strFilePath)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }

        private bool m_FunctionSelect = false;
        private void radioButton_KeepPressure_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_KeepPressure.Checked == true)
            {
                Grp_KeepPressure.Enabled = true;
                Grp_Boom.Enabled = false;
                m_FunctionSelect = true;
                m_AirTestFormHandle.GetFunctionSelect(m_FunctionSelect);
            }
            LoadKeepParaFromINI();

        }

        private void radioButton_Bomb_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Bomb.Checked)
            {
                Grp_KeepPressure.Enabled = false;
                Grp_Boom.Enabled = true;
                m_FunctionSelect = false;
                m_AirTestFormHandle.GetFunctionSelect(m_FunctionSelect);
            }
            LoadBoobParaFromINI();
        }

        #region KeepTestSet
        private bool m_isKeepChanel1 = true;
        private bool m_isKeepChanel2 = true;
        private bool m_isKeepChanel3 = true;
        private bool m_isKeepChanel4 = true;
        private bool m_isKeepChanel5 = true;
        private void checkBox_Keep_Chanel1_Click(object sender, EventArgs e)
        {
            if (checkBox_Keep_Chanel1.CheckState == CheckState.Checked)
            {
                if (!m_isKeepChanel2 && !m_isKeepChanel3 && !m_isKeepChanel4 && !m_isKeepChanel5)
                {
                    m_isKeepChanel1 = true;
                    textBox_Keep_KeepPressure1.Enabled = true;
                    textBox_Keep_DropPressure1.Enabled = true;
                    textBox_Keep_KeepTime1.Enabled = true;
                    textBox_Keep_StableTime1.Enabled = true;
                    textBox_Keep_RaiseRate1.Enabled = true;
                }
                else
                {
                    checkBox_Keep_Chanel1.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (!m_isKeepChanel2 && !m_isKeepChanel3 && !m_isKeepChanel4 && !m_isKeepChanel5)
                {
                    m_isKeepChanel1 = false;
                    textBox_Keep_KeepPressure1.Enabled = false;
                    textBox_Keep_DropPressure1.Enabled = false;
                    textBox_Keep_KeepTime1.Enabled = false;
                    textBox_Keep_StableTime1.Enabled = false;
                    textBox_Keep_RaiseRate1.Enabled = false;
                }
                else
                {
                    checkBox_Keep_Chanel1.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void checkBox_Keep_Chanel2_Click(object sender, EventArgs e)
        {
            if (checkBox_Keep_Chanel2.CheckState == CheckState.Checked)
            {
                if (m_isKeepChanel1 && !m_isKeepChanel3 && !m_isKeepChanel4 && !m_isKeepChanel5)
                {
                    m_isKeepChanel2 = true;
                    textBox_Keep_KeepPressure2.Enabled = true;
                    textBox_Keep_DropPressure2.Enabled = true;
                    textBox_Keep_KeepTime2.Enabled = true;
                    textBox_Keep_StableTime2.Enabled = true;
                    textBox_Keep_RaiseRate2.Enabled = true;
                }
                else
                {
                    checkBox_Keep_Chanel2.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (m_isKeepChanel1 && !m_isKeepChanel3 && !m_isKeepChanel4 && !m_isKeepChanel5)
                {
                    m_isKeepChanel2 = false;
                    textBox_Keep_KeepPressure2.Enabled = false;
                    textBox_Keep_DropPressure2.Enabled = false;
                    textBox_Keep_KeepTime2.Enabled = false;
                    textBox_Keep_StableTime2.Enabled = false;
                    textBox_Keep_RaiseRate2.Enabled = false;
                }
                else
                {
                    checkBox_Keep_Chanel2.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void checkBox_Keep_Chanel3_Click(object sender, EventArgs e)
        {
            if (checkBox_Keep_Chanel3.CheckState == CheckState.Checked)
            {
                if (m_isKeepChanel1 && m_isKeepChanel2 && !m_isKeepChanel4 && !m_isKeepChanel5)
                {
                    m_isKeepChanel3 = true;
                    textBox_Keep_KeepPressure3.Enabled = true;
                    textBox_Keep_DropPressure3.Enabled = true;
                    textBox_Keep_KeepTime3.Enabled = true;
                    textBox_Keep_StableTime3.Enabled = true;
                    textBox_Keep_RaiseRate3.Enabled = true;
                }
                else
                {
                    checkBox_Keep_Chanel3.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (m_isKeepChanel1 && m_isKeepChanel2 && !m_isKeepChanel4 && !m_isKeepChanel5)
                {
                    m_isKeepChanel3 = false;
                    textBox_Keep_KeepPressure3.Enabled = false;
                    textBox_Keep_DropPressure3.Enabled = false;
                    textBox_Keep_KeepTime3.Enabled = false;
                    textBox_Keep_StableTime3.Enabled = false;
                    textBox_Keep_RaiseRate3.Enabled = false;
                }
                else
                {
                    checkBox_Keep_Chanel3.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void checkBox_Keep_Chanel4_Click(object sender, EventArgs e)
        {
            if (checkBox_Keep_Chanel4.CheckState == CheckState.Checked)
            {
                if (m_isKeepChanel1 && m_isKeepChanel2 && m_isKeepChanel3 && !m_isKeepChanel5)
                {
                    m_isKeepChanel4 = true;
                    textBox_Keep_KeepPressure4.Enabled = true;
                    textBox_Keep_DropPressure4.Enabled = true;
                    textBox_Keep_KeepTime4.Enabled = true;
                    textBox_Keep_StableTime4.Enabled = true;
                    textBox_Keep_RaiseRate4.Enabled = true;
                }
                else
                {
                    checkBox_Keep_Chanel4.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (m_isKeepChanel1 && m_isKeepChanel2 && m_isKeepChanel3 && !m_isKeepChanel5)
                {
                    m_isKeepChanel4 = false;
                    textBox_Keep_KeepPressure4.Enabled = false;
                    textBox_Keep_DropPressure4.Enabled = false;
                    textBox_Keep_KeepTime4.Enabled = false;
                    textBox_Keep_StableTime4.Enabled = false;
                    textBox_Keep_RaiseRate4.Enabled = false;
                }
                else
                {
                    checkBox_Keep_Chanel4.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void checkBox_Keep_Chanel5_Click(object sender, EventArgs e)
        {
            if (checkBox_Keep_Chanel5.CheckState == CheckState.Checked)
            {
                if (m_isKeepChanel1 && m_isKeepChanel2 && m_isKeepChanel3 && m_isKeepChanel4)
                {
                    m_isKeepChanel5 = true;
                    textBox_Keep_KeepPressure5.Enabled = true;
                    textBox_Keep_DropPressure5.Enabled = true;
                    textBox_Keep_KeepTime5.Enabled = true;
                    textBox_Keep_StableTime5.Enabled = true;
                    textBox_Keep_RaiseRate5.Enabled = true;
                }
                else
                {
                    checkBox_Keep_Chanel5.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                if (m_isKeepChanel1 && m_isKeepChanel2 && m_isKeepChanel3 && m_isKeepChanel4)
                {
                    m_isKeepChanel5 = false;
                    textBox_Keep_KeepPressure5.Enabled = false;
                    textBox_Keep_DropPressure5.Enabled = false;
                    textBox_Keep_KeepTime5.Enabled = false;
                    textBox_Keep_StableTime5.Enabled = false;
                    textBox_Keep_RaiseRate5.Enabled = false;
                }
                else
                {
                    checkBox_Keep_Chanel5.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void button_Keep_Setting_Click(object sender, EventArgs e)
        {
            float KeepTime1 = 0.0f;
            float SetPressure1 = 0.0f;
            float DropPressure1 = 0.0f;
            UInt16 StableTime1 = 0;
            //int Chanel1 = 0;

            float KeepTime2 = 0.0f;
            float SetPressure2 = 0.0f;
            float DropPressure2 = 0.0f;
            UInt16 StableTime2 = 0;
            //int Chanel2 = 0;

            float KeepTime3 = 0.0f;
            float SetPressure3 = 0.0f;
            float DropPressure3 = 0.0f;
            UInt16 StableTime3 = 0;
            //int Chanel3 = 0;

            float KeepTime4 = 0.0f;
            float SetPressure4 = 0.0f;
            float DropPressure4 = 0.0f;
            UInt16 StableTime4 = 0;
            //int Chanel4 = 0;

            float KeepTime5 = 0.0f;
            float SetPressure5 = 0.0f;
            float DropPressure5 = 0.0f;
            UInt16 StableTime5 = 0;
            //int Chanel5 = 0;

            float RaisePressure1 = 0.0f;
            float RaisePressure2 = 0.0f;
            float RaisePressure3 = 0.0f;
            float RaisePressure4 = 0.0f;
            float RaisePressure5 = 0.0f;

            string TestNo = textBox_Keep_TestNo.Text;
            UInt16 Air_Chanel_Select = 0;

            try
            {
                KeepTime1 = Convert.ToSingle(textBox_Keep_KeepTime1.Text);
                SetPressure1 = Convert.ToSingle(textBox_Keep_KeepPressure1.Text);
                DropPressure1 = Convert.ToSingle(textBox_Keep_DropPressure1.Text);
                StableTime1 = Convert.ToUInt16(textBox_Keep_StableTime1.Text);

                KeepTime2 = Convert.ToSingle(textBox_Keep_KeepTime2.Text);
                SetPressure2 = Convert.ToSingle(textBox_Keep_KeepPressure2.Text);
                DropPressure2 = Convert.ToSingle(textBox_Keep_DropPressure2.Text);
                StableTime2 = Convert.ToUInt16(textBox_Keep_StableTime2.Text);

                KeepTime3 = Convert.ToSingle(textBox_Keep_KeepTime3.Text);
                SetPressure3 = Convert.ToSingle(textBox_Keep_KeepPressure3.Text);
                DropPressure3 = Convert.ToSingle(textBox_Keep_DropPressure3.Text);
                StableTime3 = Convert.ToUInt16(textBox_Keep_StableTime3.Text);

                KeepTime4 = Convert.ToSingle(textBox_Keep_KeepTime4.Text);
                SetPressure4 = Convert.ToSingle(textBox_Keep_KeepPressure4.Text);
                DropPressure4 = Convert.ToSingle(textBox_Keep_DropPressure4.Text);
                StableTime4 = Convert.ToUInt16(textBox_Keep_StableTime4.Text);

                KeepTime5 = Convert.ToSingle(textBox_Keep_KeepTime5.Text);
                SetPressure5 = Convert.ToSingle(textBox_Keep_KeepPressure5.Text);
                DropPressure5 = Convert.ToSingle(textBox_Keep_DropPressure5.Text);
                StableTime5 = Convert.ToUInt16(textBox_Keep_StableTime5.Text);

                RaisePressure1 = Convert.ToSingle(textBox_Keep_RaiseRate1.Text);
                RaisePressure2 = Convert.ToSingle(textBox_Keep_RaiseRate2.Text);
                RaisePressure3 = Convert.ToSingle(textBox_Keep_RaiseRate3.Text);
                RaisePressure4 = Convert.ToSingle(textBox_Keep_RaiseRate4.Text);
                RaisePressure5 = Convert.ToSingle(textBox_Keep_RaiseRate5.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("参数设置有误,请核对重新输入", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int code = 1;
            if (m_isKeepChanel1)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime1", KeepTime1);
                //if (code != 1)
                //{
                //    MessageBox.Show("一阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure1", SetPressure1);
                //if (code != 1)
                //{
                //    MessageBox.Show("一阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure1", DropPressure1);
                //if (code != 1)
                //{
                //    MessageBox.Show("一阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime1", StableTime1);
                //if (code != 1)
                //{
                //    MessageBox.Show("一阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure1", RaisePressure1 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率1设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel1 = 1;
                Air_Chanel_Select = 1;
            }

            if (m_isKeepChanel2)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime2", KeepTime2);
                //if (code != 1)
                //{
                //    MessageBox.Show("二阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure2", SetPressure2);
                //if (code != 1)
                //{
                //    MessageBox.Show("二阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure2", DropPressure2);
                //if (code != 1)
                //{
                //    MessageBox.Show("二阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime2", StableTime2);
                //if (code != 1)
                //{
                //    MessageBox.Show("二阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure2", RaisePressure2 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率2设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel2 = 1;
                Air_Chanel_Select = 2;
            }

            if (m_isKeepChanel3)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime3", KeepTime3);
                //if (code != 1)
                //{
                //    MessageBox.Show("三阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure3", SetPressure3);
                //if (code != 1)
                //{
                //    MessageBox.Show("三阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure3", DropPressure3);
                //if (code != 1)
                //{
                //    MessageBox.Show("三阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime3", StableTime3);
                //if (code != 1)
                //{
                //    MessageBox.Show("三阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure3", RaisePressure3 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率3设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel3 = 1;
                Air_Chanel_Select = 3;
            }

            if (m_isKeepChanel4)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime4", KeepTime4);
                //if (code != 1)
                //{
                //    MessageBox.Show("四阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure4", SetPressure4);
                //if (code != 1)
                //{
                //    MessageBox.Show("四阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure4", DropPressure4);
                //if (code != 1)
                //{
                //    MessageBox.Show("四阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime4", StableTime4);
                //if (code != 1)
                //{
                //    MessageBox.Show("四阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure4", RaisePressure4 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率4设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel4 = 1;
                Air_Chanel_Select = 4;
            }

            if (m_isKeepChanel5)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime5", KeepTime5);
                //if (code != 1)
                //{
                //    MessageBox.Show("五阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure5", SetPressure5);
                //if (code != 1)
                //{
                //    MessageBox.Show("五阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure5", DropPressure5);
                //if (code != 1)
                //{
                //    MessageBox.Show("五阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime5", StableTime5);
                //if (code != 1)
                //{
                //    MessageBox.Show("五阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure5", RaisePressure5 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率5设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel5 = 1;
                Air_Chanel_Select = 5;
            }

            if (Air_Chanel_Select == 0)
            {
                MessageBox.Show("请选择一组参数", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte FunctionSelect = 1;
            code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_FunctionSelect", FunctionSelect);
            //if (code != 1)
            //{
            //    MessageBox.Show("功能选择设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Chanel_Select", Air_Chanel_Select);
            //if (code != 1)
            //{
            //    MessageBox.Show("试验通道设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            SaveKeepSetParaToINI();
            MessageBox.Show("设置成功", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            
        }

        private void button_Keep_PLcRead_Click(object sender, EventArgs e)
        {
            float KeepTime1 = 0.0f;
            float SetPressure1 = 0.0f;
            float DropPressure1 = 0.0f;
            UInt16 StableTime1 = 0;

            float KeepTime2 = 0.0f;
            float SetPressure2 = 0.0f;
            float DropPressure2 = 0.0f;
            UInt16 StableTime2 = 0;

            float KeepTime3 = 0.0f;
            float SetPressure3 = 0.0f;
            float DropPressure3 = 0.0f;
            UInt16 StableTime3 = 0;

            float KeepTime4 = 0.0f;
            float SetPressure4 = 0.0f;
            float DropPressure4 = 0.0f;
            UInt16 StableTime4 = 0;

            float KeepTime5 = 0.0f;
            float SetPressure5 = 0.0f;
            float DropPressure5 = 0.0f;
            UInt16 StableTime5 = 0;

            float RaisePressure1 = 0.0f;
            float RaisePressure2 = 0.0f;
            float RaisePressure3 = 0.0f;
            float RaisePressure4 = 0.0f;
            float RaisePressure5 = 0.0f;


            int code = 1;
            DateTime dt = default(DateTime);
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime1", ref SetPressure1, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure1", ref SetPressure1, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure1", ref DropPressure1, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime1", ref StableTime1, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime2", ref SetPressure2, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure2", ref SetPressure2, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure2", ref DropPressure2, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime2", ref StableTime2, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime3", ref SetPressure3, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure3", ref SetPressure3, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure3", ref DropPressure3, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime3", ref StableTime3, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime4", ref SetPressure4, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure4", ref SetPressure4, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure4", ref DropPressure4, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime4", ref StableTime4, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime5", ref SetPressure5, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure5", ref SetPressure5, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure5", ref DropPressure5, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime5", ref StableTime5, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure1", ref RaisePressure1, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure2", ref RaisePressure2, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure3", ref RaisePressure3, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure4", ref RaisePressure4, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure5", ref RaisePressure5, ref dt);
            //if (code != 1)
            //{
            //    return;
            //}

            checkBox_Keep_Chanel1.CheckState = CheckState.Checked;
            checkBox_Keep_Chanel2.CheckState = CheckState.Checked;
            checkBox_Keep_Chanel3.CheckState = CheckState.Checked;
            checkBox_Keep_Chanel4.CheckState = CheckState.Checked;
            checkBox_Keep_Chanel5.CheckState = CheckState.Checked;

            textBox_Keep_KeepTime1.Text = KeepTime1.ToString();
            textBox_Keep_KeepPressure1.Text = SetPressure1.ToString();
            textBox_Keep_DropPressure1.Text = DropPressure1.ToString();
            textBox_Keep_StableTime1.Text = StableTime1.ToString();

            textBox_Keep_KeepTime2.Text = KeepTime2.ToString();
            textBox_Keep_KeepPressure2.Text = SetPressure2.ToString();
            textBox_Keep_DropPressure2.Text = DropPressure2.ToString();
            textBox_Keep_StableTime2.Text = StableTime2.ToString();

            textBox_Keep_KeepTime3.Text = KeepTime3.ToString();
            textBox_Keep_KeepPressure3.Text = SetPressure3.ToString();
            textBox_Keep_DropPressure3.Text = DropPressure3.ToString();
            textBox_Keep_StableTime3.Text = StableTime3.ToString();

            textBox_Keep_KeepTime4.Text = KeepTime4.ToString();
            textBox_Keep_KeepPressure4.Text = SetPressure4.ToString();
            textBox_Keep_DropPressure4.Text = DropPressure4.ToString();
            textBox_Keep_StableTime4.Text = StableTime4.ToString();

            textBox_Keep_KeepTime5.Text = KeepTime5.ToString();
            textBox_Keep_KeepPressure5.Text = SetPressure5.ToString();
            textBox_Keep_DropPressure5.Text = DropPressure5.ToString();
            textBox_Keep_StableTime5.Text = StableTime5.ToString();

            textBox_Keep_RaiseRate1.Text = (1000 * RaisePressure1).ToString();
            textBox_Keep_RaiseRate2.Text = (1000 * RaisePressure2).ToString();
            textBox_Keep_RaiseRate3.Text = (1000 * RaisePressure3).ToString();
            textBox_Keep_RaiseRate4.Text = (1000 * RaisePressure4).ToString();
            textBox_Keep_RaiseRate5.Text = (1000 * RaisePressure5).ToString();
            textBox_Keep_TestNo.Text = ContentValue(m_KeepstrSec, "TestNo", m_KeepstrFilePath);
        }

        private void button_Keep_StartTest_Click(object sender, EventArgs e)
        {
            this.Hide();
            //m_AirTestFormHandle = new AirTestForm();
            m_AirTestFormHandle.TopLevel = false;
            m_Grp.Controls.Add(m_AirTestFormHandle);
            m_AirTestFormHandle.Show();
            //m_AirTestFormHandle.m_ShowSystemStatusTimer.Enabled = true;
            ShowMainFormInfo("气压静压试验");
            m_AirTestFormHandle.SetFormStyle();
        }

        private void button_Keep_Return_Click(object sender, EventArgs e)
        {
            this.Hide();
            bool isTesting = false;
            m_AirTestFormHandle.GetTestInfo(ref isTesting);
            if (isTesting)
            {
                m_AirTestFormHandle.Show();
                ShowMainFormInfo("气压测试");
                //m_AirTestFormHandle.m_ShowSystemStatusTimer.Enabled = true;
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
            //this.Dispose();
        }
        #endregion


        #region BoomTestSet
        private void button_Boom_PLcRead_Click(object sender, EventArgs e)
        {
            float KeepTime1 = 0.0f;
            float SetPressure1 = 0.0f;
            float DropPressure1 = 0.0f;
            UInt16 StableTime1 = 0;

            float KeepTime2 = 0.0f;
            float SetPressure2 = 0.0f;
            float DropPressure2 = 0.0f;
            UInt16 StableTime2 = 0;

            float KeepTime3 = 0.0f;
            float SetPressure3 = 0.0f;
            float DropPressure3 = 0.0f;
            UInt16 StableTime3 = 0;

            float KeepTime4 = 0.0f;
            float SetPressure4 = 0.0f;
            float DropPressure4 = 0.0f;
            UInt16 StableTime4 = 0;

            float KeepTime5 = 0.0f;
            float SetPressure5 = 0.0f;
            float DropPressure5 = 0.0f;
            UInt16 StableTime5 = 0;

            float RaisePressure1 = 0.0f;
            float RaisePressure2 = 0.0f;
            float RaisePressure3 = 0.0f;
            float RaisePressure4 = 0.0f;
            float RaisePressure5 = 0.0f;


            int code = 1;
            DateTime dt = default(DateTime);
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime1", ref SetPressure1, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure1", ref SetPressure1, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure1", ref DropPressure1, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime1", ref StableTime1, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime2", ref SetPressure2, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure2", ref SetPressure2, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure2", ref DropPressure2, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime2", ref StableTime2, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime3", ref SetPressure3, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure3", ref SetPressure3, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure3", ref DropPressure3, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime3", ref StableTime3, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime4", ref SetPressure4, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure4", ref SetPressure4, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure4", ref DropPressure4, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime4", ref StableTime4, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepTime5", ref SetPressure5, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_KeepPressure5", ref SetPressure5, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_DropPressure5", ref DropPressure5, ref dt);
            if (code != 1)
            {
                return;
            }
            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_StableTime5", ref StableTime5, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure1", ref RaisePressure1, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure2", ref RaisePressure2, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure3", ref RaisePressure3, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure4", ref RaisePressure4, ref dt);
            if (code != 1)
            {
                return;
            }

            code = m_MainFormHandle.m_PLCCommHandle.ReadData("Air_Keep_RaisePressure5", ref RaisePressure5, ref dt);
            if (code != 1)
            {
                return;
            }

            checkBox_Boob_Chanel1.CheckState = CheckState.Checked;
            checkBox_Boob_Chanel2.CheckState = CheckState.Checked;
            checkBox_Boob_Chanel3.CheckState = CheckState.Checked;
            checkBox_Boob_Chanel4.CheckState = CheckState.Checked;
            checkBox_Boob_Chanel5.CheckState = CheckState.Checked;

            textBox_Boob_KeepTime1.Text = KeepTime1.ToString();
            textBox_Boom_KeepPressure1.Text = SetPressure1.ToString();
            textBox_Boom_DropPressure1.Text = DropPressure1.ToString();
            textBox_Boob_StableTime1.Text = StableTime1.ToString();

            textBox_Boob_KeepTime2.Text = KeepTime2.ToString();
            textBox_Boom_KeepPressure2.Text = SetPressure2.ToString();
            textBox_Boom_DropPressure2.Text = DropPressure2.ToString();
            textBox_Boob_StableTime2.Text = StableTime2.ToString();

            textBox_Boob_KeepTime3.Text = KeepTime3.ToString();
            textBox_Boom_KeepPressure3.Text = SetPressure3.ToString();
            textBox_Boom_DropPressure3.Text = DropPressure3.ToString();
            textBox_Boob_StableTime3.Text = StableTime3.ToString();

            textBox_Boob_KeepTime4.Text = KeepTime4.ToString();
            textBox_Boom_KeepPressure4.Text = SetPressure4.ToString();
            textBox_Boom_DropPressure4.Text = DropPressure4.ToString();
            textBox_Boob_StableTime4.Text = StableTime4.ToString();

            textBox_Boob_KeepTime5.Text = KeepTime5.ToString();
            textBox_Boom_KeepPressure5.Text = SetPressure5.ToString();
            textBox_Boom_DropPressure5.Text = DropPressure5.ToString();
            textBox_Boob_StableTime5.Text = StableTime5.ToString();

            textBox_Boob_RaiseRate1.Text = (1000*RaisePressure1).ToString();
            textBox_Boob_RaiseRate2.Text = (1000 * RaisePressure2).ToString();
            textBox_Boob_RaiseRate3.Text = (1000 * RaisePressure3).ToString();
            textBox_Boob_RaiseRate4.Text = (1000 * RaisePressure4).ToString();
            textBox_Boob_RaiseRate5.Text = (1000 * RaisePressure5).ToString();
            textBox_Boob_TestNo.Text = ContentValue(m_BoomstrSec, "TestNo", m_BoomstrFilePath);
        }

        private void button_Boob_Return_Click(object sender, EventArgs e)
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
            //this.Dispose();
        }

        private void button_Boob_StartTest_Click(object sender, EventArgs e)
        {
            this.Hide();
            //m_AirTestFormHandle = new AirTestForm();
            m_AirTestFormHandle.TopLevel = false;
            m_Grp.Controls.Add(m_AirTestFormHandle);
            m_AirTestFormHandle.Show();
            ShowMainFormInfo("气压爆破试验");
            m_AirTestFormHandle.SetFormStyle();
        }

        private void button_Boob_Setting_Click(object sender, EventArgs e)
        {
            float KeepTime1 = 0.0f;
            float SetPressure1 = 0.0f;
            float DropPressure1 = 0.0f;
            UInt16 StableTime1 = 0;
            //int Chanel1 = 0;

            float KeepTime2 = 0.0f;
            float SetPressure2 = 0.0f;
            float DropPressure2 = 0.0f;
            UInt16 StableTime2 = 0;
            //int Chanel2 = 0;

            float KeepTime3 = 0.0f;
            float SetPressure3 = 0.0f;
            float DropPressure3 = 0.0f;
            UInt16 StableTime3 = 0;
            //int Chanel3 = 0;

            float KeepTime4 = 0.0f;
            float SetPressure4 = 0.0f;
            float DropPressure4 = 0.0f;
            UInt16 StableTime4 = 0;
            //int Chanel4 = 0;

            float KeepTime5 = 0.0f;
            float SetPressure5 = 0.0f;
            float DropPressure5 = 0.0f;
            UInt16 StableTime5 = 0;
            //int Chanel5 = 0;

            float RaisePressure1 = 0.0f;
            float RaisePressure2 = 0.0f;
            float RaisePressure3 = 0.0f;
            float RaisePressure4 = 0.0f;
            float RaisePressure5 = 0.0f;

            string TestNo = textBox_Keep_TestNo.Text;
            UInt16 Air_Chanel_Select = 0;

            try
            {
                KeepTime1 = Convert.ToSingle(textBox_Boob_KeepTime1.Text);
                SetPressure1 = Convert.ToSingle(textBox_Boom_KeepPressure1.Text);
                DropPressure1 = Convert.ToSingle(textBox_Boom_DropPressure1.Text);
                StableTime1 = Convert.ToUInt16(textBox_Boob_StableTime1.Text);

                KeepTime2 = Convert.ToSingle(textBox_Boob_KeepTime2.Text);
                SetPressure2 = Convert.ToSingle(textBox_Boom_KeepPressure2.Text);
                DropPressure2 = Convert.ToSingle(textBox_Boom_DropPressure2.Text);
                StableTime2 = Convert.ToUInt16(textBox_Boob_StableTime2.Text);

                KeepTime3 = Convert.ToSingle(textBox_Boob_KeepTime3.Text);
                SetPressure3 = Convert.ToSingle(textBox_Boom_KeepPressure3.Text);
                DropPressure3 = Convert.ToSingle(textBox_Boom_DropPressure3.Text);
                StableTime3 = Convert.ToUInt16(textBox_Boob_StableTime3.Text);

                KeepTime4 = Convert.ToSingle(textBox_Boob_KeepTime4.Text);
                SetPressure4 = Convert.ToSingle(textBox_Boom_KeepPressure4.Text);
                DropPressure4 = Convert.ToSingle(textBox_Boom_DropPressure4.Text);
                StableTime4 = Convert.ToUInt16(textBox_Boob_StableTime4.Text);

                KeepTime5 = Convert.ToSingle(textBox_Boob_KeepTime5.Text);
                SetPressure5 = Convert.ToSingle(textBox_Boom_KeepPressure5.Text);
                DropPressure5 = Convert.ToSingle(textBox_Boom_DropPressure5.Text);
                StableTime5 = Convert.ToUInt16(textBox_Boob_StableTime5.Text);

                RaisePressure1 = Convert.ToSingle(textBox_Boob_RaiseRate1.Text);
                RaisePressure2 = Convert.ToSingle(textBox_Boob_RaiseRate2.Text);
                RaisePressure3 = Convert.ToSingle(textBox_Boob_RaiseRate3.Text);
                RaisePressure4 = Convert.ToSingle(textBox_Boob_RaiseRate4.Text);
                RaisePressure5 = Convert.ToSingle(textBox_Boob_RaiseRate5.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("参数设置有误,请核对重新输入", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int code = 1;
            if (m_isBoobChanel1)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime1", KeepTime1);
                //if (code != 1)
                //{
                //    MessageBox.Show("一阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure1", SetPressure1);
                //if (code != 1)
                //{
                //    MessageBox.Show("一阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure1", DropPressure1);
                //if (code != 1)
                //{
                //    MessageBox.Show("一阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime1", StableTime1);
                //if (code != 1)
                //{
                //    MessageBox.Show("一阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure1", RaisePressure1 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率1设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel1 = 1;
                Air_Chanel_Select = 1;
            }

            if (m_isBoobChanel2)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime2", KeepTime2);
                //if (code != 1)
                //{
                //    MessageBox.Show("二阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure2", SetPressure2);
                //if (code != 1)
                //{
                //    MessageBox.Show("二阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure2", DropPressure2);
                //if (code != 1)
                //{
                //    MessageBox.Show("二阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime2", StableTime2);
                //if (code != 1)
                //{
                //    MessageBox.Show("二阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure2", RaisePressure2 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率2设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel2 = 1;
                Air_Chanel_Select = 2;
            }

            if (m_isBoobChanel3)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime3", KeepTime3);
                //if (code != 1)
                //{
                //    MessageBox.Show("三阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure3", SetPressure3);
                //if (code != 1)
                //{
                //    MessageBox.Show("三阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure3", DropPressure3);
                //if (code != 1)
                //{
                //    MessageBox.Show("三阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime3", StableTime3);
                //if (code != 1)
                //{
                //    MessageBox.Show("三阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure3", RaisePressure3 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率3设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel3 = 1;
                Air_Chanel_Select = 3;
            }

            if (m_isBoobChanel4)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime4", KeepTime4);
                //if (code != 1)
                //{
                //    MessageBox.Show("四阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure4", SetPressure4);
                //if (code != 1)
                //{
                //    MessageBox.Show("四阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure4", DropPressure4);
                //if (code != 1)
                //{
                //    MessageBox.Show("四阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime4", StableTime4);
                //if (code != 1)
                //{
                //    MessageBox.Show("四阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure4", RaisePressure4 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率4设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel4 = 1;
                Air_Chanel_Select = 4;
            }

            if (m_isBoobChanel5)
            {
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepTime5", KeepTime5);
                //if (code != 1)
                //{
                //    MessageBox.Show("五阶保压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_KeepPressure5", SetPressure5);
                //if (code != 1)
                //{
                //    MessageBox.Show("五阶设定压力设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_DropPressure5", DropPressure5);
                //if (code != 1)
                //{
                //    MessageBox.Show("五阶允许压降设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_StableTime5", StableTime5);
                //if (code != 1)
                //{
                //    MessageBox.Show("五阶稳压时间设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Keep_RaisePressure5", RaisePressure5 / 1000);
                //if (code != 1)
                //{
                //    MessageBox.Show("升压速率设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Chanel5 = 1;
                Air_Chanel_Select = 5;
            }

            if (Air_Chanel_Select == 0)
            {
                MessageBox.Show("请选择一组参数", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte FunctionSelect = 0;
            code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_FunctionSelect", FunctionSelect);
            //if (code != 1)
            //{
            //    MessageBox.Show("升压速率设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            code = m_MainFormHandle.m_PLCCommHandle.WriteData("Air_Chanel_Select", Air_Chanel_Select);
            //if (code != 1)
            //{
            //    MessageBox.Show("试验通道设置失败，错误代码 = " + code.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            SaveBoobSetParaToINI();
            MessageBox.Show("设置成功", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool m_isBoobChanel1 = true;
        private bool m_isBoobChanel2 = true;
        private bool m_isBoobChanel3 = true;
        private bool m_isBoobChanel4 = true;
        private bool m_isBoobChanel5 = true;
        private void checkBox_Boob_Chanel1_Click(object sender, EventArgs e)
        {
            if (checkBox_Boob_Chanel1.CheckState == CheckState.Checked)
            {
                if (!m_isBoobChanel2 && !m_isBoobChanel3 && !m_isBoobChanel4 && !m_isBoobChanel5)
                {
                    m_isBoobChanel1 = true;
                    textBox_Boom_KeepPressure1.Enabled = true;
                    textBox_Boom_DropPressure1.Enabled = true;
                    textBox_Boob_KeepTime1.Enabled = true;
                    textBox_Boob_StableTime1.Enabled = true;
                    textBox_Boob_RaiseRate1.Enabled = true;
                }
                else
                {
                    checkBox_Boob_Chanel1.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (!m_isBoobChanel2 && !m_isBoobChanel3 && !m_isBoobChanel4 && !m_isBoobChanel5)
                {
                    m_isBoobChanel1 = false;
                    textBox_Boom_KeepPressure1.Enabled = false;
                    textBox_Boom_DropPressure1.Enabled = false;
                    textBox_Boob_KeepTime1.Enabled = false;
                    textBox_Boob_StableTime1.Enabled = false;
                    textBox_Boob_RaiseRate1.Enabled = false;
                }
                else
                {
                    checkBox_Boob_Chanel1.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void checkBox_Boob_Chanel2_Click(object sender, EventArgs e)
        {
            if (checkBox_Boob_Chanel2.CheckState == CheckState.Checked)
            {
                if (m_isBoobChanel1 && !m_isBoobChanel3 && !m_isBoobChanel4 && !m_isBoobChanel5)
                {
                    m_isBoobChanel2 = true;
                    textBox_Boom_KeepPressure2.Enabled = true;
                    textBox_Boom_DropPressure2.Enabled = true;
                    textBox_Boob_KeepTime2.Enabled = true;
                    textBox_Boob_StableTime2.Enabled = true;
                    textBox_Boob_RaiseRate2.Enabled = true;
                }
                else
                {
                    checkBox_Boob_Chanel2.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (m_isBoobChanel1 && !m_isBoobChanel3 && !m_isBoobChanel4 && !m_isBoobChanel5)
                {
                    m_isBoobChanel2 = false;
                    textBox_Boom_KeepPressure2.Enabled = false;
                    textBox_Boom_DropPressure2.Enabled = false;
                    textBox_Boob_KeepTime2.Enabled = false;
                    textBox_Boob_StableTime2.Enabled = false;
                    textBox_Boob_RaiseRate2.Enabled = false;
                }
                else
                {
                    checkBox_Boob_Chanel2.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void checkBox_Boob_Chanel3_Click(object sender, EventArgs e)
        {
            if (checkBox_Boob_Chanel3.CheckState == CheckState.Checked)
            {
                if (m_isBoobChanel1 && m_isBoobChanel2 && !m_isBoobChanel4 && !m_isBoobChanel5)
                {
                    m_isBoobChanel3 = true;
                    textBox_Boom_KeepPressure3.Enabled = true;
                    textBox_Boom_DropPressure3.Enabled = true;
                    textBox_Boob_KeepTime3.Enabled = true;
                    textBox_Boob_StableTime3.Enabled = true;
                    textBox_Boob_RaiseRate3.Enabled = true;
                }
                else
                {
                    checkBox_Boob_Chanel3.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (m_isBoobChanel1 && m_isBoobChanel2 && !m_isBoobChanel4 && !m_isBoobChanel5)
                {
                    m_isBoobChanel3 = false;
                    textBox_Boom_KeepPressure3.Enabled = false;
                    textBox_Boom_DropPressure3.Enabled = false;
                    textBox_Boob_KeepTime3.Enabled = false;
                    textBox_Boob_StableTime3.Enabled = false;
                    textBox_Boob_RaiseRate3.Enabled = false;
                }
                else
                {
                    checkBox_Boob_Chanel3.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void checkBox_Boob_Chanel4_Click(object sender, EventArgs e)
        {
            if (checkBox_Boob_Chanel4.CheckState == CheckState.Checked)
            {
                if (m_isBoobChanel1 && m_isBoobChanel2 && m_isBoobChanel3 && !m_isBoobChanel5)
                {
                    m_isBoobChanel4 = true;
                    textBox_Boom_KeepPressure4.Enabled = true;
                    textBox_Boom_DropPressure4.Enabled = true;
                    textBox_Boob_KeepTime4.Enabled = true;
                    textBox_Boob_StableTime4.Enabled = true;
                    textBox_Boob_RaiseRate4.Enabled = true;
                }
                else
                {
                    checkBox_Boob_Chanel4.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (m_isBoobChanel1 && m_isBoobChanel2 && m_isBoobChanel3 && !m_isBoobChanel5)
                {
                    m_isBoobChanel4 = false;
                    textBox_Boom_KeepPressure4.Enabled = false;
                    textBox_Boom_DropPressure4.Enabled = false;
                    textBox_Boob_KeepTime4.Enabled = false;
                    textBox_Boob_StableTime4.Enabled = false;
                    textBox_Boob_RaiseRate4.Enabled = false;
                }
                else
                {
                    checkBox_Boob_Chanel4.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void checkBox_Boob_Chanel5_Click(object sender, EventArgs e)
        {
            if (checkBox_Boob_Chanel5.CheckState == CheckState.Checked)
            {
                if (m_isBoobChanel1 && m_isBoobChanel2 && m_isBoobChanel3 && m_isBoobChanel4)
                {
                    m_isBoobChanel5 = true;
                    textBox_Boom_KeepPressure5.Enabled = true;
                    textBox_Boom_DropPressure5.Enabled = true;
                    textBox_Boob_KeepTime5.Enabled = true;
                    textBox_Boob_StableTime5.Enabled = true;
                    textBox_Boob_RaiseRate5.Enabled = true;
                }
                else
                {
                    checkBox_Keep_Chanel5.CheckState = CheckState.Unchecked;
                    MessageBox.Show("请先按顺序打开通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                if (m_isBoobChanel1 && m_isBoobChanel2 && m_isBoobChanel3 && m_isBoobChanel4)
                {
                    m_isBoobChanel5 = false;
                    textBox_Boom_KeepPressure5.Enabled = false;
                    textBox_Boom_DropPressure5.Enabled = false;
                    textBox_Boob_KeepTime5.Enabled = false;
                    textBox_Boob_StableTime5.Enabled = false;
                    textBox_Boob_RaiseRate5.Enabled = false;
                }
                else
                {
                    checkBox_Boob_Chanel5.CheckState = CheckState.Checked;
                    MessageBox.Show("请先按顺序关闭通道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        #endregion

        public void SaveBoobSetParaToINI()
        {
            float KeepTime1 = 0.0f;
            float SetPressure1 = 0.0f;
            float DropPressure1 = 0.0f;
            UInt16 StableTime1 = 0;
            int Chanel1 = checkBox_Boob_Chanel1.CheckState == CheckState.Checked ? 1 : 0;

            float KeepTime2 = 0.0f;
            float SetPressure2 = 0.0f;
            float DropPressure2 = 0.0f;
            UInt16 StableTime2 = 0;
            int Chanel2 = checkBox_Boob_Chanel2.CheckState == CheckState.Checked ? 1 : 0;

            float KeepTime3 = 0.0f;
            float SetPressure3 = 0.0f;
            float DropPressure3 = 0.0f;
            UInt16 StableTime3 = 0;
            int Chanel3 = checkBox_Boob_Chanel3.CheckState == CheckState.Checked ? 1 : 0;

            float KeepTime4 = 0.0f;
            float SetPressure4 = 0.0f;
            float DropPressure4 = 0.0f;
            UInt16 StableTime4 = 0;
            int Chanel4 = checkBox_Boob_Chanel4.CheckState == CheckState.Checked ? 1 : 0;

            float KeepTime5 = 0.0f;
            float SetPressure5 = 0.0f;
            float DropPressure5 = 0.0f;
            UInt16 StableTime5 = 0;
            int Chanel5 = checkBox_Boob_Chanel5.CheckState == CheckState.Checked ? 1 : 0;

            float RaisePressure1 = 0.0f;
            float RaisePressure2 = 0.0f;
            float RaisePressure3 = 0.0f;
            float RaisePressure4 = 0.0f;
            float RaisePressure5 = 0.0f;

            string TestNo = textBox_Boob_TestNo.Text;

            try
            {
                KeepTime1 = Convert.ToSingle(textBox_Boob_KeepTime1.Text);
                SetPressure1 = Convert.ToSingle(textBox_Boom_KeepPressure1.Text);
                DropPressure1 = Convert.ToSingle(textBox_Boom_DropPressure1.Text);
                StableTime1 = Convert.ToUInt16(textBox_Boob_StableTime1.Text);

                KeepTime2 = Convert.ToSingle(textBox_Boob_KeepTime2.Text);
                SetPressure2 = Convert.ToSingle(textBox_Boom_KeepPressure2.Text);
                DropPressure2 = Convert.ToSingle(textBox_Boom_DropPressure2.Text);
                StableTime2 = Convert.ToUInt16(textBox_Boob_StableTime2.Text);

                KeepTime3 = Convert.ToSingle(textBox_Boob_KeepTime3.Text);
                SetPressure3 = Convert.ToSingle(textBox_Boom_KeepPressure3.Text);
                DropPressure3 = Convert.ToSingle(textBox_Boom_DropPressure3.Text);
                StableTime3 = Convert.ToUInt16(textBox_Boob_StableTime3.Text);

                KeepTime4 = Convert.ToSingle(textBox_Boob_KeepTime4.Text);
                SetPressure4 = Convert.ToSingle(textBox_Boom_KeepPressure4.Text);
                DropPressure4 = Convert.ToSingle(textBox_Boom_DropPressure4.Text);
                StableTime4 = Convert.ToUInt16(textBox_Boob_StableTime4.Text);

                KeepTime5 = Convert.ToSingle(textBox_Boob_KeepTime5.Text);
                SetPressure5 = Convert.ToSingle(textBox_Boom_KeepPressure5.Text);
                DropPressure5 = Convert.ToSingle(textBox_Boom_DropPressure5.Text);
                StableTime5 = Convert.ToUInt16(textBox_Boob_StableTime5.Text);

                RaisePressure1 = Convert.ToSingle(textBox_Boob_RaiseRate1.Text);
                RaisePressure2 = Convert.ToSingle(textBox_Boob_RaiseRate2.Text);
                RaisePressure3 = Convert.ToSingle(textBox_Boob_RaiseRate3.Text);
                RaisePressure4 = Convert.ToSingle(textBox_Boob_RaiseRate4.Text);
                RaisePressure5 = Convert.ToSingle(textBox_Boob_RaiseRate5.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("参数设置有误,请核对重新输入", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WritePrivateProfileString(m_BoomstrSec, "Air_KeepTime1", KeepTime1.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_KeepPressure1", SetPressure1.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_DropPressure1", DropPressure1.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_StableTime1", StableTime1.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Chanel1", Chanel1.ToString(), m_BoomstrFilePath);

            WritePrivateProfileString(m_BoomstrSec, "Air_KeepTime2", KeepTime2.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_KeepPressure2", SetPressure2.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_DropPressure2", DropPressure2.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_StableTime2", StableTime2.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Chanel2", Chanel2.ToString(), m_BoomstrFilePath);

            WritePrivateProfileString(m_BoomstrSec, "Air_KeepTime3", KeepTime3.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_KeepPressure3", SetPressure3.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_DropPressure3", DropPressure3.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_StableTime3", StableTime3.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Chanel3", Chanel3.ToString(), m_BoomstrFilePath);

            WritePrivateProfileString(m_BoomstrSec, "Air_KeepTime4", KeepTime4.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_KeepPressure4", SetPressure4.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_DropPressure4", DropPressure4.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_StableTime4", StableTime4.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Chanel4", Chanel4.ToString(), m_BoomstrFilePath);

            WritePrivateProfileString(m_BoomstrSec, "Air_KeepTime5", KeepTime5.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_KeepPressure5", SetPressure5.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_DropPressure5", DropPressure5.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_StableTime5", StableTime5.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Chanel5", Chanel5.ToString(), m_BoomstrFilePath);

            WritePrivateProfileString(m_BoomstrSec, "Air_Keep_RaisePressure1", RaisePressure1.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Keep_RaisePressure2", RaisePressure2.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Keep_RaisePressure3", RaisePressure3.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Keep_RaisePressure4", RaisePressure4.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "Air_Keep_RaisePressure5", RaisePressure5.ToString(), m_BoomstrFilePath);
            WritePrivateProfileString(m_BoomstrSec, "TestNo", TestNo, m_BoomstrFilePath);
        }

        public void LoadBoobParaFromINI()
        {
            textBox_Boob_KeepTime1.Text = ContentValue(m_BoomstrSec, "Air_KeepTime1", m_BoomstrFilePath);
            textBox_Boom_KeepPressure1.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure1", m_BoomstrFilePath);
            textBox_Boom_DropPressure1.Text = ContentValue(m_BoomstrSec, "Air_DropPressure1", m_BoomstrFilePath);
            textBox_Boob_StableTime1.Text = ContentValue(m_BoomstrSec, "Air_StableTime1", m_BoomstrFilePath);
            textBox_Boob_RaiseRate1.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure1", m_BoomstrFilePath);
            string Chanel1 = ContentValue(m_BoomstrSec, "Air_Chanel1", m_BoomstrFilePath);
            if (Chanel1 == "0")
            {
                checkBox_Boob_Chanel1.CheckState = CheckState.Unchecked;
                m_isBoobChanel1 = false;
                textBox_Boom_KeepPressure1.Enabled = false;
                textBox_Boom_DropPressure1.Enabled = false;
                textBox_Boob_KeepTime1.Enabled = false;
                textBox_Boob_StableTime1.Enabled = false;
                textBox_Boob_RaiseRate1.Enabled = false;
            }
            if (Chanel1 == "1")
            {
                checkBox_Boob_Chanel1.CheckState = CheckState.Checked;
                m_isBoobChanel1 = true;
                textBox_Boom_KeepPressure1.Enabled = true;
                textBox_Boom_DropPressure1.Enabled = true;
                textBox_Boob_KeepTime1.Enabled = true;
                textBox_Boob_StableTime1.Enabled = true;
                textBox_Boob_RaiseRate1.Enabled = true;
            }

            textBox_Boob_KeepTime2.Text = ContentValue(m_BoomstrSec, "Air_KeepTime2", m_BoomstrFilePath);
            textBox_Boom_KeepPressure2.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure2", m_BoomstrFilePath);
            textBox_Boom_DropPressure2.Text = ContentValue(m_BoomstrSec, "Air_DropPressure2", m_BoomstrFilePath);
            textBox_Boob_StableTime2.Text = ContentValue(m_BoomstrSec, "Air_StableTime2", m_BoomstrFilePath);
            textBox_Boob_RaiseRate2.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure2", m_BoomstrFilePath);
            string Chanel2 = ContentValue(m_BoomstrSec, "Air_Chanel2", m_BoomstrFilePath);
            if (Chanel2 == "0")
            {
                checkBox_Boob_Chanel2.CheckState = CheckState.Unchecked;
                m_isBoobChanel2 = false;
                textBox_Boom_KeepPressure2.Enabled = false;
                textBox_Boom_DropPressure2.Enabled = false;
                textBox_Boob_KeepTime2.Enabled = false;
                textBox_Boob_StableTime2.Enabled = false;
                textBox_Boob_RaiseRate2.Enabled = false;
            }
            if (Chanel2 == "1")
            {
                checkBox_Boob_Chanel2.CheckState = CheckState.Checked;
                m_isBoobChanel2 = true;
                textBox_Boom_KeepPressure2.Enabled = true;
                textBox_Boom_DropPressure2.Enabled = true;
                textBox_Boob_KeepTime2.Enabled = true;
                textBox_Boob_StableTime2.Enabled = true;
                textBox_Boob_RaiseRate2.Enabled = true;
            }

            textBox_Boob_KeepTime3.Text = ContentValue(m_BoomstrSec, "Air_KeepTime3", m_BoomstrFilePath);
            textBox_Boom_KeepPressure3.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure3", m_BoomstrFilePath);
            textBox_Boom_DropPressure3.Text = ContentValue(m_BoomstrSec, "Air_DropPressure3", m_BoomstrFilePath);
            textBox_Boob_StableTime3.Text = ContentValue(m_BoomstrSec, "Air_StableTime3", m_BoomstrFilePath);
            textBox_Boob_RaiseRate3.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure3", m_BoomstrFilePath);
            string Chanel3 = ContentValue(m_BoomstrSec, "Air_Chanel3", m_BoomstrFilePath);
            if (Chanel3 == "0")
            {
                checkBox_Boob_Chanel3.CheckState = CheckState.Unchecked;
                m_isBoobChanel3 = false;
                textBox_Boom_KeepPressure3.Enabled = false;
                textBox_Boom_DropPressure3.Enabled = false;
                textBox_Boob_KeepTime3.Enabled = false;
                textBox_Boob_StableTime3.Enabled = false;
                textBox_Boob_RaiseRate3.Enabled = false;
            }
            if (Chanel3 == "1")
            {
                checkBox_Boob_Chanel3.CheckState = CheckState.Checked;
                m_isBoobChanel3 = true;
                textBox_Boom_KeepPressure3.Enabled = true;
                textBox_Boom_DropPressure3.Enabled = true;
                textBox_Boob_KeepTime3.Enabled = true;
                textBox_Boob_StableTime3.Enabled = true;
                textBox_Boob_RaiseRate3.Enabled = true;
            }

            textBox_Boob_KeepTime4.Text = ContentValue(m_BoomstrSec, "Air_KeepTime4", m_BoomstrFilePath);
            textBox_Boom_KeepPressure4.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure4", m_BoomstrFilePath);
            textBox_Boom_DropPressure4.Text = ContentValue(m_BoomstrSec, "Air_DropPressure4", m_BoomstrFilePath);
            textBox_Boob_StableTime4.Text = ContentValue(m_BoomstrSec, "Air_StableTime4", m_BoomstrFilePath);
            textBox_Boob_RaiseRate4.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure4", m_BoomstrFilePath);
            string Chanel4 = ContentValue(m_BoomstrSec, "Air_Chanel4", m_BoomstrFilePath);
            if (Chanel4 == "0")
            {
                checkBox_Boob_Chanel4.CheckState = CheckState.Unchecked;
                m_isBoobChanel4 = false;
                textBox_Boom_KeepPressure4.Enabled = false;
                textBox_Boom_DropPressure4.Enabled = false;
                textBox_Boob_KeepTime4.Enabled = false;
                textBox_Boob_StableTime4.Enabled = false;
                textBox_Boob_RaiseRate4.Enabled = false;
            }
            if (Chanel4 == "1")
            {
                checkBox_Boob_Chanel4.CheckState = CheckState.Checked;
                m_isBoobChanel4 = true;
                textBox_Boom_KeepPressure4.Enabled = true;
                textBox_Boom_DropPressure4.Enabled = true;
                textBox_Boob_KeepTime4.Enabled = true;
                textBox_Boob_StableTime4.Enabled = true;
                textBox_Boob_RaiseRate4.Enabled = true;
            }

            textBox_Boob_KeepTime5.Text = ContentValue(m_BoomstrSec, "Air_KeepTime5", m_BoomstrFilePath);
            textBox_Boom_KeepPressure5.Text = ContentValue(m_BoomstrSec, "Air_KeepPressure5", m_BoomstrFilePath);
            textBox_Boom_DropPressure5.Text = ContentValue(m_BoomstrSec, "Air_DropPressure5", m_BoomstrFilePath);
            textBox_Boob_StableTime5.Text = ContentValue(m_BoomstrSec, "Air_StableTime5", m_BoomstrFilePath);
            textBox_Boob_RaiseRate5.Text = ContentValue(m_BoomstrSec, "Air_Keep_RaisePressure5", m_BoomstrFilePath);
            string Chanel5 = ContentValue(m_BoomstrSec, "Air_Chanel5", m_BoomstrFilePath);
            if (Chanel5 == "0")
            {
                checkBox_Boob_Chanel5.CheckState = CheckState.Unchecked;
                m_isBoobChanel5 = false;
                textBox_Boom_KeepPressure5.Enabled = false;
                textBox_Boom_DropPressure5.Enabled = false;
                textBox_Boob_KeepTime5.Enabled = false;
                textBox_Boob_StableTime5.Enabled = false;
                textBox_Boob_RaiseRate5.Enabled = false;
            }
            if (Chanel5 == "1")
            {
                checkBox_Boob_Chanel5.CheckState = CheckState.Checked;
                m_isBoobChanel5 = true;
                textBox_Boom_KeepPressure5.Enabled = true;
                textBox_Boom_DropPressure5.Enabled = true;
                textBox_Boob_KeepTime5.Enabled = true;
                textBox_Boob_StableTime5.Enabled = true;
                textBox_Boob_RaiseRate5.Enabled = true;
            }

            textBox_Boob_TestNo.Text = ContentValue(m_BoomstrSec, "TestNo", m_BoomstrFilePath);
        }

        public void SaveKeepSetParaToINI()
        {
            float KeepTime1 = 0.0f;
            float SetPressure1 = 0.0f;
            float DropPressure1 = 0.0f;
            UInt16 StableTime1 = 0;
            int Chanel1 = checkBox_Keep_Chanel1.CheckState == CheckState.Checked ? 1 : 0;

            float KeepTime2 = 0.0f;
            float SetPressure2 = 0.0f;
            float DropPressure2 = 0.0f;
            UInt16 StableTime2 = 0;
            int Chanel2 = checkBox_Keep_Chanel2.CheckState == CheckState.Checked ? 1 : 0; ;

            float KeepTime3 = 0.0f;
            float SetPressure3 = 0.0f;
            float DropPressure3 = 0.0f;
            UInt16 StableTime3 = 0;
            int Chanel3 = checkBox_Keep_Chanel3.CheckState == CheckState.Checked ? 1 : 0; ;

            float KeepTime4 = 0.0f;
            float SetPressure4 = 0.0f;
            float DropPressure4 = 0.0f;
            UInt16 StableTime4 = 0;
            int Chanel4 = checkBox_Keep_Chanel4.CheckState == CheckState.Checked ? 1 : 0; ;

            float KeepTime5 = 0.0f;
            float SetPressure5 = 0.0f;
            float DropPressure5 = 0.0f;
            UInt16 StableTime5 = 0;
            int Chanel5 = checkBox_Keep_Chanel5.CheckState == CheckState.Checked ? 1 : 0; ;

            float RaisePressure1 = 0.0f;
            float RaisePressure2 = 0.0f;
            float RaisePressure3 = 0.0f;
            float RaisePressure4 = 0.0f;
            float RaisePressure5 = 0.0f;

            string TestNo = textBox_Keep_TestNo.Text;

            try
            {
                KeepTime1 = Convert.ToSingle(textBox_Keep_KeepTime1.Text);
                SetPressure1 = Convert.ToSingle(textBox_Keep_KeepPressure1.Text);
                DropPressure1 = Convert.ToSingle(textBox_Keep_DropPressure1.Text);
                StableTime1 = Convert.ToUInt16(textBox_Keep_StableTime1.Text);

                KeepTime2 = Convert.ToSingle(textBox_Keep_KeepTime2.Text);
                SetPressure2 = Convert.ToSingle(textBox_Keep_KeepPressure2.Text);
                DropPressure2 = Convert.ToSingle(textBox_Keep_DropPressure2.Text);
                StableTime2 = Convert.ToUInt16(textBox_Keep_StableTime2.Text);

                KeepTime3 = Convert.ToSingle(textBox_Keep_KeepTime3.Text);
                SetPressure3 = Convert.ToSingle(textBox_Keep_KeepPressure3.Text);
                DropPressure3 = Convert.ToSingle(textBox_Keep_DropPressure3.Text);
                StableTime3 = Convert.ToUInt16(textBox_Keep_StableTime3.Text);

                KeepTime4 = Convert.ToSingle(textBox_Keep_KeepTime4.Text);
                SetPressure4 = Convert.ToSingle(textBox_Keep_KeepPressure4.Text);
                DropPressure4 = Convert.ToSingle(textBox_Keep_DropPressure4.Text);
                StableTime4 = Convert.ToUInt16(textBox_Keep_StableTime4.Text);

                KeepTime5 = Convert.ToSingle(textBox_Keep_KeepTime5.Text);
                SetPressure5 = Convert.ToSingle(textBox_Keep_KeepPressure5.Text);
                DropPressure5 = Convert.ToSingle(textBox_Keep_DropPressure5.Text);
                StableTime5 = Convert.ToUInt16(textBox_Keep_StableTime5.Text);

                RaisePressure1 = Convert.ToSingle(textBox_Keep_RaiseRate1.Text);
                RaisePressure2 = Convert.ToSingle(textBox_Keep_RaiseRate2.Text);
                RaisePressure3 = Convert.ToSingle(textBox_Keep_RaiseRate3.Text);
                RaisePressure4 = Convert.ToSingle(textBox_Keep_RaiseRate4.Text);
                RaisePressure5 = Convert.ToSingle(textBox_Keep_RaiseRate5.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("参数设置有误,请核对重新输入", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WritePrivateProfileString(m_KeepstrSec, "Air_KeepTime1", KeepTime1.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_KeepPressure1", SetPressure1.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_DropPressure1", DropPressure1.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_StableTime1", StableTime1.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Chanel1", Chanel1.ToString(), m_KeepstrFilePath);

            WritePrivateProfileString(m_KeepstrSec, "Air_KeepTime2", KeepTime2.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_KeepPressure2", SetPressure2.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_DropPressure2", DropPressure2.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_StableTime2", StableTime2.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Chanel2", Chanel2.ToString(), m_KeepstrFilePath);

            WritePrivateProfileString(m_KeepstrSec, "Air_KeepTime3", KeepTime3.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_KeepPressure3", SetPressure3.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_DropPressure3", DropPressure3.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_StableTime3", StableTime3.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Chanel3", Chanel3.ToString(), m_KeepstrFilePath);

            WritePrivateProfileString(m_KeepstrSec, "Air_KeepTime4", KeepTime4.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_KeepPressure4", SetPressure4.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_DropPressure4", DropPressure4.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_StableTime4", StableTime4.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Chanel4", Chanel4.ToString(), m_KeepstrFilePath);

            WritePrivateProfileString(m_KeepstrSec, "Air_KeepTime5", KeepTime5.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_KeepPressure5", SetPressure5.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_DropPressure5", DropPressure5.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_StableTime5", StableTime5.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Chanel5", Chanel5.ToString(), m_KeepstrFilePath);

            WritePrivateProfileString(m_KeepstrSec, "Air_Keep_RaisePressure1", RaisePressure1.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Keep_RaisePressure2", RaisePressure2.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Keep_RaisePressure3", RaisePressure3.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Keep_RaisePressure4", RaisePressure4.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "Air_Keep_RaisePressure5", RaisePressure5.ToString(), m_KeepstrFilePath);
            WritePrivateProfileString(m_KeepstrSec, "TestNo", TestNo, m_KeepstrFilePath);
        }

        public void LoadKeepParaFromINI()
        {
            textBox_Keep_KeepTime1.Text = ContentValue(m_KeepstrSec, "Air_KeepTime1", m_KeepstrFilePath);
            textBox_Keep_KeepPressure1.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure1", m_KeepstrFilePath);
            textBox_Keep_DropPressure1.Text = ContentValue(m_KeepstrSec, "Air_DropPressure1", m_KeepstrFilePath);
            textBox_Keep_StableTime1.Text = ContentValue(m_KeepstrSec, "Air_StableTime1", m_KeepstrFilePath);
            textBox_Keep_RaiseRate1.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure1", m_KeepstrFilePath);
            string Chanel1 = ContentValue(m_KeepstrSec, "Air_Chanel1", m_KeepstrFilePath);
            if (Chanel1 == "0")
            {
                checkBox_Keep_Chanel1.CheckState = CheckState.Unchecked;
                m_isKeepChanel1 = false;
                textBox_Keep_KeepPressure1.Enabled = false;
                textBox_Keep_DropPressure1.Enabled = false;
                textBox_Keep_KeepTime1.Enabled = false;
                textBox_Keep_StableTime1.Enabled = false;
                textBox_Keep_RaiseRate1.Enabled = false;
            }
            if (Chanel1 == "1")
            {
                checkBox_Keep_Chanel1.CheckState = CheckState.Checked;
                m_isKeepChanel1 = true;
                textBox_Keep_KeepPressure1.Enabled = true;
                textBox_Keep_DropPressure1.Enabled = true;
                textBox_Keep_KeepTime1.Enabled = true;
                textBox_Keep_StableTime1.Enabled = true;
                textBox_Keep_RaiseRate1.Enabled = true;
            }

            textBox_Keep_KeepTime2.Text = ContentValue(m_KeepstrSec, "Air_KeepTime2", m_KeepstrFilePath);
            textBox_Keep_KeepPressure2.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure2", m_KeepstrFilePath);
            textBox_Keep_DropPressure2.Text = ContentValue(m_KeepstrSec, "Air_DropPressure2", m_KeepstrFilePath);
            textBox_Keep_StableTime2.Text = ContentValue(m_KeepstrSec, "Air_StableTime2", m_KeepstrFilePath);
            textBox_Keep_RaiseRate2.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure2", m_KeepstrFilePath);
            string Chanel2 = ContentValue(m_KeepstrSec, "Air_Chanel2", m_KeepstrFilePath);
            if (Chanel2 == "0")
            {
                checkBox_Keep_Chanel2.CheckState = CheckState.Unchecked;
                m_isKeepChanel2 = false;
                textBox_Keep_KeepPressure2.Enabled = false;
                textBox_Keep_DropPressure2.Enabled = false;
                textBox_Keep_KeepTime2.Enabled = false;
                textBox_Keep_StableTime2.Enabled = false;
                textBox_Keep_RaiseRate2.Enabled = false;
            }
            if (Chanel2 == "1")
            {
                checkBox_Keep_Chanel2.CheckState = CheckState.Checked;
                m_isKeepChanel2 = true;
                textBox_Keep_KeepPressure2.Enabled = true;
                textBox_Keep_DropPressure2.Enabled = true;
                textBox_Keep_KeepTime2.Enabled = true;
                textBox_Keep_StableTime2.Enabled = true;
                textBox_Keep_RaiseRate2.Enabled = true;
            }

            textBox_Keep_KeepTime3.Text = ContentValue(m_KeepstrSec, "Air_KeepTime3", m_KeepstrFilePath);
            textBox_Keep_KeepPressure3.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure3", m_KeepstrFilePath);
            textBox_Keep_DropPressure3.Text = ContentValue(m_KeepstrSec, "Air_DropPressure3", m_KeepstrFilePath);
            textBox_Keep_StableTime3.Text = ContentValue(m_KeepstrSec, "Air_StableTime3", m_KeepstrFilePath);
            textBox_Keep_RaiseRate3.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure3", m_KeepstrFilePath);
            string Chanel3 = ContentValue(m_KeepstrSec, "Air_Chanel3", m_KeepstrFilePath);
            if (Chanel3 == "0")
            {
                checkBox_Keep_Chanel3.CheckState = CheckState.Unchecked;
                m_isKeepChanel3 = false;
                textBox_Keep_KeepPressure3.Enabled = false;
                textBox_Keep_DropPressure3.Enabled = false;
                textBox_Keep_KeepTime3.Enabled = false;
                textBox_Keep_StableTime3.Enabled = false;
                textBox_Keep_RaiseRate3.Enabled = false;
            }
            if (Chanel3 == "1")
            {
                checkBox_Keep_Chanel3.CheckState = CheckState.Checked;
                m_isKeepChanel3 = true;
                textBox_Keep_KeepPressure3.Enabled = true;
                textBox_Keep_DropPressure3.Enabled = true;
                textBox_Keep_KeepTime3.Enabled = true;
                textBox_Keep_StableTime3.Enabled = true;
                textBox_Keep_RaiseRate3.Enabled = true;
            }

            textBox_Keep_KeepTime4.Text = ContentValue(m_KeepstrSec, "Air_KeepTime4", m_KeepstrFilePath);
            textBox_Keep_KeepPressure4.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure4", m_KeepstrFilePath);
            textBox_Keep_DropPressure4.Text = ContentValue(m_KeepstrSec, "Air_DropPressure4", m_KeepstrFilePath);
            textBox_Keep_StableTime4.Text = ContentValue(m_KeepstrSec, "Air_StableTime4", m_KeepstrFilePath);
            textBox_Keep_RaiseRate4.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure4", m_KeepstrFilePath);
            string Chanel4 = ContentValue(m_KeepstrSec, "Air_Chanel4", m_KeepstrFilePath);
            if (Chanel4 == "0")
            {
                checkBox_Keep_Chanel4.CheckState = CheckState.Unchecked;
                m_isKeepChanel4 = false;
                textBox_Keep_KeepPressure4.Enabled = false;
                textBox_Keep_DropPressure4.Enabled = false;
                textBox_Keep_KeepTime4.Enabled = false;
                textBox_Keep_StableTime4.Enabled = false;
                textBox_Keep_RaiseRate4.Enabled = false;
            }
            if (Chanel4 == "1")
            {
                checkBox_Keep_Chanel4.CheckState = CheckState.Checked;
                m_isKeepChanel4 = true;
                textBox_Keep_KeepPressure4.Enabled = true;
                textBox_Keep_DropPressure4.Enabled = true;
                textBox_Keep_KeepTime4.Enabled = true;
                textBox_Keep_StableTime4.Enabled = true;
                textBox_Keep_RaiseRate4.Enabled = true;
            }

            textBox_Keep_KeepTime5.Text = ContentValue(m_KeepstrSec, "Air_KeepTime5", m_KeepstrFilePath);
            textBox_Keep_KeepPressure5.Text = ContentValue(m_KeepstrSec, "Air_KeepPressure5", m_KeepstrFilePath);
            textBox_Keep_DropPressure5.Text = ContentValue(m_KeepstrSec, "Air_DropPressure5", m_KeepstrFilePath);
            textBox_Keep_StableTime5.Text = ContentValue(m_KeepstrSec, "Air_StableTime5", m_KeepstrFilePath);
            textBox_Keep_RaiseRate5.Text = ContentValue(m_KeepstrSec, "Air_Keep_RaisePressure5", m_KeepstrFilePath);
            string Chanel5 = ContentValue(m_KeepstrSec, "Air_Chanel5", m_KeepstrFilePath);
            if (Chanel5 == "0")
            {
                checkBox_Keep_Chanel5.CheckState = CheckState.Unchecked;
                m_isKeepChanel5 = false;
                textBox_Keep_KeepPressure5.Enabled = false;
                textBox_Keep_DropPressure5.Enabled = false;
                textBox_Keep_KeepTime5.Enabled = false;
                textBox_Keep_StableTime5.Enabled = false;
                textBox_Keep_RaiseRate5.Enabled = false;
            }
            if (Chanel5 == "1")
            {
                checkBox_Keep_Chanel5.CheckState = CheckState.Checked;
                m_isKeepChanel5 = true;
                textBox_Keep_KeepPressure5.Enabled = true;
                textBox_Keep_DropPressure5.Enabled = true;
                textBox_Keep_KeepTime5.Enabled = true;
                textBox_Keep_StableTime5.Enabled = true;
                textBox_Keep_RaiseRate5.Enabled = true;
            }

            textBox_Keep_TestNo.Text = ContentValue(m_KeepstrSec, "TestNo", m_KeepstrFilePath);
        }

    }
}
