using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace YinLunTestBench
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //限制该程序只能运行一个实例
            bool bCreatedNew;
            Mutex m = new Mutex(false, "Setup.exe", out bCreatedNew);
            if (bCreatedNew) //未运行
            {
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("该程序已经运行！");
                System.Environment.Exit(0);
                return;
            }
        }
    }
}
