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
    public partial class OtherUserPaymentAfterBuyForm : MetroFramework.Forms.MetroForm
    {

        DecentDbEntities _context = new DecentDbEntities();

        ProductBL _productBl = new ProductBL();
        List<Product> _products = new List<Product>();
        private Product _selectedProduct = null;
        private int _selectedIndex1 = 0;

        SellerBL _sellerBl = new SellerBL();
        List<Seller> _sellers = new List<Seller>();
        private Seller _selectedSeller = null;
        private int _selectedIndex = 0;
        
        SupplierPaymentDetailsBL _paymentDetailsBl=new SupplierPaymentDetailsBL();
        List<PaymentDetail> _paymentDetail = new List<PaymentDetail>();
        private PaymentDetail _selectedPaymentDetail = null;
        private int _selectedIndex2 = 0;

        DayPaymentBL _dayPaymentBl = new DayPaymentBL();
        List<DayPayment> _dayPayments = new List<DayPayment>();
        private DayPayment _selectedDayPayment = null;
        private int _selectedIndex3 = 0;
       
        

        public OtherUserPaymentAfterBuyForm(string supplierName)
        {

            InitializeComponent();

            string sellerName = supplierName;
            var seller=_context.Sellers.FirstOrDefault(u => u.Name == sellerName);
            txtSellerName.Text = seller.Name;
            txtPhone.Text = seller.Phone;
            txtDue.Text=seller.Due.ToString();
            
        }

        private void PaymentAfterBuyForm_Load(object sender, EventArgs e)
        {
            txtAmount.Select();
        }

        

        private void metroButton3_Click(object sender, EventArgs e)
        {
            
           
        }

        

        private void metroLabel2_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

       
        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                btnOk.PerformClick();
            }

            if (e.KeyCode == Keys.Up)
            {
                btnBack.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                btnOk.Focus();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            OtherUserAddProductManager a = new OtherUserAddProductManager();
            a.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm a = new LoginForm();
            a.Show();
            this.Hide();
        }

        private bool isValid()
        {
            
            try
            {
                double due = Convert.ToSingle(txtDue.Text);
                double amount = Convert.ToSingle(txtAmount.Text);
                if (amount == 0 || due<amount)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid amount..!!!");
                    txtAmount.Focus();
                    return false;
                }

            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, exception.Message);
                return false;
            }

            return true;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;
            try
            {
                if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
              DialogResult.No)
                {
                    txtAmount.Focus();
                    return;
                }

                double payment = Convert.ToSingle(txtAmount.Text);
                _selectedPaymentDetail = new PaymentDetail()
                {
                    Date = DateTime.Now
                };

                _selectedPaymentDetail.Date = Convert.ToDateTime(dtpDate.Text);
                _selectedPaymentDetail.SellerName = txtSellerName.Text;
                _selectedPaymentDetail.Phone = txtPhone.Text;
                _selectedPaymentDetail.TotalDue = Convert.ToSingle(txtDue.Text);
                _selectedPaymentDetail.Payment = payment;

                string error;
                _selectedPaymentDetail = _paymentDetailsBl.Save(_selectedPaymentDetail, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                _paymentDetail.Add(_selectedPaymentDetail);


                var seller = _context.Sellers.FirstOrDefault(u => u.Name == txtSellerName.Text);
                seller.Payment += payment;

                string error1;
                _selectedSeller = _sellerBl.Save(seller, out error1);

                if (string.IsNullOrEmpty(error1) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error1);
                    return;
                }


                _selectedDayPayment = new DayPayment();
                _selectedDayPayment.Name = txtSellerName.Text;
                _selectedDayPayment.Amount = payment;

                string error2;
                _selectedDayPayment = _dayPaymentBl.Save(_selectedDayPayment, out error2);

                if (string.IsNullOrEmpty(error2) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error2);
                    return;
                }

                _dayPayments.Add(_selectedDayPayment);


                

                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");


                OtherUserPaymentAfterBuyForm a=new OtherUserPaymentAfterBuyForm(txtSellerName.Text);
                a.Show();
                this.Hide();


            }
            catch (Exception exception)
            {

                MetroFramework.MetroMessageBox.Show(this, exception.Message);
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }
    }
}
