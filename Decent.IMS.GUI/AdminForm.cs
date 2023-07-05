using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decent.IMS.BL;
using Decent.IMS.Data;

namespace Decent.IMS.GUI
{
    public partial class AdminForm : MetroFramework.Forms.MetroForm
    {
        
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
           
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            UserDetailForm ud=new UserDetailForm();
            ud.Show();
            this.Hide();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            ProductDetailForm pd=new ProductDetailForm();
            pd.Show();
            this.Hide();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {

        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            LoginForm lf=new LoginForm();
            lf.Show();
            this.Hide();
        }

        private void metroButton3_Click_1(object sender, EventArgs e)
        {
            SalesmanManager sm=new SalesmanManager();
            sm.Show();
            this.Hide();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            DaySellManager dm=new DaySellManager();
            dm.Show();
            this.Hide();
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            EndDayDateTimeForm ed=new EndDayDateTimeForm();
            ed.Show();
            this.Hide();
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            DayIncomeManager di=new DayIncomeManager();
            di.Show();
            this.Hide();
            
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            DayCostManager a=new DayCostManager();
            a.Show();
            this.Hide();
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            SellerManager a=new SellerManager();
            a.Show();
            this.Hide();
        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            SellerNewProductManager a=new SellerNewProductManager();
            a.Show();
            this.Hide();
        }

        private void metroButton10_Click(object sender, EventArgs e)
        {
            DayPaymentManager a = new DayPaymentManager();
            a.Show();
            this.Hide();
        }

        private void metroButton12_Click(object sender, EventArgs e)
        {
            SupplierPaymentManager a=new SupplierPaymentManager();
            a.Show();
            this.Hide();
        }

        private void metroButton13_Click(object sender, EventArgs e)
        {
            CustomerManager a=new CustomerManager();
            a.Show();
            this.Hide();
        }

        private void metroButton14_Click(object sender, EventArgs e)
        {
            TemporarySellManager a=new TemporarySellManager();
            a.Show();
            this.Hide();
        }

        private void metroButton17_Click(object sender, EventArgs e)
        {
            SellDetailsManager a=new SellDetailsManager();
            a.Show();
            this.Hide();
        }

        private void metroButton15_Click(object sender, EventArgs e)
        {
            CustomerPaymentDetailsManager a=new CustomerPaymentDetailsManager();
            a.Show();
            this.Hide();
        }

        private void metroButton18_Click(object sender, EventArgs e)
        {
            CustomerDaySellManager a=new CustomerDaySellManager();
            a.Show();
            this.Hide();
        }

        private void metroButton16_Click(object sender, EventArgs e)
        {
            SalesmanPaymentManager a=new SalesmanPaymentManager();
            a.Show();
            this.Hide();
        }

        private void metroButton19_Click(object sender, EventArgs e)
        {
            AdminCostManager a=new AdminCostManager();
            a.Show();
            this.Hide();
        }

        private void btnCashMemo_Click(object sender, EventArgs e)
        {
            CashMemoManager cm=new CashMemoManager();
            cm.Show();
            this.Hide();
        }

        private void metroButton20_Click(object sender, EventArgs e)
        {
            CompanyListsManager a=new CompanyListsManager();
            a.Show();
            this.Hide();
        }

        private void btnBackToOtherUser_Click(object sender, EventArgs e)
        {
            OtherUserProductForm a=new OtherUserProductForm();
            a.Show();
            this.Hide();
        }
    }
}
