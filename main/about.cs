using System;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        private void about_Load(object sender, EventArgs e)
        {

        }
        protected override void WndProc(ref Message m)
        {
            const int WM_NCLBUTTONDBLCLK = 0x00A3;

            if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                return; // 阻止双击标题栏行为
            }

            base.WndProc(ref m);
        }



    }
}
