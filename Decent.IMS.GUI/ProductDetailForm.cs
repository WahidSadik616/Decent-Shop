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
    public partial class ProductDetailForm : MetroFramework.Forms.MetroForm
    {
        public ProductDetailForm()
        {
            InitializeComponent();
        }

        private void ProductDetailForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            ProductManager pm=new ProductManager();
            pm.Show();
            this.Hide();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            ProductCategoryManager pc=new ProductCategoryManager();
            pc.Show();
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

        private void metroButton5_Click(object sender, EventArgs e)
        {
            AddProductManager a=new AddProductManager();
            a.Show();
            this.Hide();
        }
    }
}
