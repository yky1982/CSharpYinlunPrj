using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YinLunTestBench
{
    public partial class SystemDescripForm : Form
    {
        private Form1 m_MainFormHandle;
        public event FormShowState ShowMainForm;
        public AirTestForm m_AirTestFormHandle = null;
        public FliudTestForm m_FliudTestFormHandle = null;
        public SystemDescripForm(Form1 handle)
        {
            InitializeComponent();
            m_MainFormHandle = handle;
            ShowMainForm += new FormShowState(m_MainFormHandle.FormStateShow);
        }

        private void SystemDescripForm_Load(object sender, EventArgs e)
        {
            ShowMainFormInfo("系统描述");
            AirTestForm.GetInstance(ref m_AirTestFormHandle);
            FliudTestForm.GetInstance(ref m_FliudTestFormHandle);
        }

        private void Button_Return_Click(object sender, EventArgs e)
        {

        }

        private void ShowMainFormInfo(string text)
        {
            if (ShowMainForm != null)
            {
                ShowMainForm(text);
            }
        }

        private void TB_Exit_Click(object sender, EventArgs e)
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

    }
}
