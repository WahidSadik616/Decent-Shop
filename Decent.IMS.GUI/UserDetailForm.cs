using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Decent.IMS.GUI
{
    public partial class UserDetailForm : MetroFramework.Forms.MetroForm
    {
        public UserDetailForm()
        {
            InitializeComponent();
        }

        private void UserDetailForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            UserInfoManager um=new UserInfoManager();
            um.Show();
            this.Hide();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            DepartmentManager dm=new DepartmentManager();
            dm.Show();
            this.Hide();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            AdminForm af=new AdminForm();
            af.Show();
            this.Hide();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            LoginForm lf=new LoginForm();
            lf.Show();
            this.Hide();
        }
    }
}
