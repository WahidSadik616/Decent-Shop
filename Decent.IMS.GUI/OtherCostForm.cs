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
    public partial class OtherCostForm : MetroFramework.Forms.MetroForm
    {
        public OtherCostForm()
        {
            InitializeComponent();
        }

        private void OtherCostForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            OtherUserDayCostManager a=new OtherUserDayCostManager();
            a.Show();
            this.Hide();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            LoginForm a=new LoginForm();
            a.Show();
            this.Hide();
        }
    }
}
