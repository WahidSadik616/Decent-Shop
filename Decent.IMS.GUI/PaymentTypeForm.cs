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
    public partial class PaymentTypeForm : MetroFramework.Forms.MetroForm
    {
        public PaymentTypeForm()
        {
            InitializeComponent();
        }

        private void PaymentTypeForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            OtherUserCustomerPaymentForm a=new OtherUserCustomerPaymentForm();
            a.Show();
            this.Hide();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            OtherUserSupplierPaymentForm a = new OtherUserSupplierPaymentForm();
            a.Show();
            this.Hide();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            LoginForm a=new LoginForm();
            a.Show();
            this.Hide();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            OtherUserMenuForm a=new OtherUserMenuForm();
            a.Show();
            this.Hide();
        }
    }
}
