using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Decent.IMS.GUI
{
    public partial class OtherUserReturnForm : MetroFramework.Forms.MetroForm
    {
        public OtherUserReturnForm()
        {
            InitializeComponent();
        }

        private void OtherUserReturnForm_Load(object sender, EventArgs e)
        {
            btnReturnCustomerProduct.Select();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            OtherUserProductForm a = new OtherUserProductForm();
            a.Show();
            this.Hide();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            OtherUserReturnProductManager a = new OtherUserReturnProductManager();
            a.Show();
            this.Hide();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LoginForm a=new LoginForm();
            a.Show();
            this.Hide();
            
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            OtherUserMenuForm a=new OtherUserMenuForm();
            a.Show();
            this.Hide();
        }
    }
}
