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
    public partial class OtherUserProductForm : MetroFramework.Forms.MetroForm
    {
        public OtherUserProductForm()
        {
            InitializeComponent();
            btnAddProduct.Select();

        }

        private void OtherUserForm_Load(object sender, EventArgs e)
        {
            //this.KeyPreview = true;

        }

       
        private void btnHome_Click(object sender, EventArgs e)
        {
            OtherUserMenuForm a=new OtherUserMenuForm();
            a.Show();
            this.Hide();
        }

        private void btnHome_Click_1(object sender, EventArgs e)
        {
            OtherUserMenuForm a = new OtherUserMenuForm();
            a.Show();
            this.Hide();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LoginForm a=new LoginForm();
            a.Show();
            this.Hide();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            OtherUserAddProductManager a=new OtherUserAddProductManager();
            a.Show();
            this.Hide();
        }

        private void btnSellProduct_Click(object sender, EventArgs e)
        {
            OtherUserSellProductManager a=new OtherUserSellProductManager();
            a.Show();
            this.Hide();
        }

        private void btnReturnProduct_Click(object sender, EventArgs e)
        {
            OtherUserReturnForm a=new OtherUserReturnForm();
            a.Show();
            this.Hide();
        }

        private void metroPanel3_Paint(object sender, PaintEventArgs e)
        {
            
        }
        
        
    }
}
