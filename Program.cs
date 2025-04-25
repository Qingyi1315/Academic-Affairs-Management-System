using System;
using System.Windows.Forms;

namespace 教务管理系统
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new fileED());
            // 主循环：持续运行直到用户彻底退出
            //while (true)
            //{
            //    using (var loginForm = new landing_form())
            //    {
            //        // 显示登录窗体并等待结果
            //        var result = loginForm.ShowDialog();

            //        if (result == DialogResult.OK && loginForm.IsLoginSuccessful)
            //        {
            //            if (GlobalVariables.CurrentRole == "学生")
            //            {
            //                using (var stuForm = new stu_table_viewmenu())
            //                {
            //                    Application.Run(stuForm); 
            //                }
            //            }
            //            else if (GlobalVariables.CurrentRole == "教师")
            //            {
            //                using (var teaForm = new tea_table_viewmenu())
            //                {
            //                    Application.Run(teaForm); 
            //                }
            //            }
            //            else if (GlobalVariables.CurrentRole == "管理员")
            //            {
            //                using (var adminForm = new admin_table_viewmenu())
            //                {
            //                    Application.Run(adminForm); 
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show("未知角色，请联系管理员。");

            //            }
            //        }
            //        else
            //        {
            //            // 用户点击取消或直接关闭登录窗体，退出整个应用程序
            //            break;
            //        }
            //    }
            //}
        }
    }
}
