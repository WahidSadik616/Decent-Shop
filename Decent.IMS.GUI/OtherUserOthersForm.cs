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
    public partial class OtherUserOthersForm : MetroFramework.Forms.MetroForm
    {
        public OtherUserOthersForm()
        {
            InitializeComponent();
        }

        private void OtherUserOthersForm_Load(object sender, EventArgs e)
        {

        }

        private void btnCost_Click(object sender, EventArgs e)
        {
            OtherUserDayCostManager c = new OtherUserDayCostManager();
            c.Show();
            this.Hide();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            PaymentTypeForm a = new PaymentTypeForm();
            a.Show();
            this.Hide();
        }
    }
}
