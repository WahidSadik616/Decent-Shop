using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decent.IMS.BL;
using Decent.IMS.Data;

namespace Decent.IMS.GUI
{
    public partial class OtherUserSupplierPaymentForm : MetroFramework.Forms.MetroForm
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

        public OtherUserSupplierPaymentForm()
        {
            InitializeComponent();
        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {
            
            try
            {
                ddlSellerList.DataSource = _context.Sellers.ToList();
                ddlSellerList.DisplayMember = "Name";
                ddlSellerList.ValueMember = "ID";

                 _sellers = _sellerBl.GetAll("");
                 _products = _productBl.GetAll("");
                 _paymentDetail = _paymentDetailsBl.GetAll("");

                 if (_products.Count > 0)
                 {
                     _selectedProduct = _products[0];
                     ddlSellerList.SelectedValue = _selectedProduct.SellerId;
                   
                 }
             
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            OtherUserHomeForm a = new OtherUserHomeForm();
            a.Show();
            this.Hide();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            LoginForm a=new LoginForm();
            a.Show();
            this.Hide();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;
            try
            {

                var seller = _context.Sellers.FirstOrDefault(u => u.Name == ddlSellerList.Text);

                if (seller != null)
                {
                    double payment = Convert.ToSingle(txtAmount.Text);
                    _selectedPaymentDetail = new PaymentDetail()
                    {
                        Date = DateTime.Now
                    };

                    _selectedPaymentDetail.Date = Convert.ToDateTime(dtpDate.Text);
                    _selectedPaymentDetail.SellerName = ddlSellerList.Text;
                    _selectedPaymentDetail.Phone = seller.Phone;
                    _selectedPaymentDetail.TotalDue = seller.Due;
                    _selectedPaymentDetail.Payment = payment;

                    string error;
                    _selectedPaymentDetail = _paymentDetailsBl.Save(_selectedPaymentDetail, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    _paymentDetail.Add(_selectedPaymentDetail);



                    seller.Payment += payment;

                    string error1;
                    _selectedSeller = _sellerBl.Save(seller, out error1);

                    if (string.IsNullOrEmpty(error1) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error1);
                        return;
                    }


                    _selectedDayPayment = new DayPayment();
                    _selectedDayPayment.Name = ddlSellerList.Text;
                    _selectedDayPayment.Amount = payment;

                    string error2;
                    _selectedDayPayment = _dayPaymentBl.Save(_selectedDayPayment, out error2);

                    if (string.IsNullOrEmpty(error2) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error2);
                        return;
                    }

                    _dayPayments.Add(_selectedDayPayment);


                    if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                  DialogResult.No)
                    {
                        return;
                    }

                    MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");

                    OtherUserMenuForm of = new OtherUserMenuForm();
                    of.Show();
                    this.Hide();

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "No found..!!!");
                }
            }
            catch (Exception exception)
            {

                MetroFramework.MetroMessageBox.Show(this, exception.Message);
            }
           
        }

        private bool isValid()
        {
            if (string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Amount please..!!!");
                txtAmount.Focus();
                return false;
            }
            try
            {
                double amount = Convert.ToSingle(txtAmount.Text);
                if (amount <= 0)
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

        private void metroLabel2_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            var seller=_context.Sellers.FirstOrDefault(u => u.Name == ddlSellerList.Text);
            if (seller != null)
            {
                txtDue.Text = Convert.ToString(seller.Due);
            }
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            var seller = _context.Sellers.FirstOrDefault(u => u.Name == ddlSellerList.Text);
            if (seller != null)
            {
                txtPhone.Text = Convert.ToString(seller.Phone);
            }
        }
    }
}
