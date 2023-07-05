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
    public partial class OtherUserCustomerPaymentForm : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context = new DecentDbEntities();

        CustomerPaymentDetailsBL _customerPaymentDetailsBl = new CustomerPaymentDetailsBL();
        List<CustomerPaymentDetail> _customerPaymentDetailss = new List<CustomerPaymentDetail>();
        private CustomerPaymentDetail _selectedCustomerPaymentDetails = null;

        CustomerBL _customerBl = new CustomerBL();
        List<Customer> _customers = new List<Customer>();
        private Customer _selectedCustomer = null;

        public OtherUserCustomerPaymentForm()
        {
            InitializeComponent();
        }

        private void CustomerPaymentDetailsForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            LoginForm a=new LoginForm();
            a.Show();
            this.Hide();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            PaymentTypeForm a = new PaymentTypeForm();
            a.Show();
            this.Hide();
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            var customer = _context.Customers.FirstOrDefault(u => u.Phone == txtPhone.Text);
            if (customer != null)
            {
                txtCustomerName.Text = customer.Name;
                txtDue.Text = Convert.ToString(100); //Convert.ToString(customer.Due);
            }
            if (customer == null)
            {
                MetroFramework.MetroMessageBox.Show(this, "No found..!!!");
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;

            try
            {

                var customer = _context.Customers.FirstOrDefault(u => u.Phone == txtPhone.Text);

                if (customer != null)
                {
                    double payment = Convert.ToSingle(txtAmount.Text);
                    _selectedCustomerPaymentDetails = new CustomerPaymentDetail()
                    {
                        Date = DateTime.Now
                    };

                    _selectedCustomerPaymentDetails.Date = Convert.ToDateTime(dtpDate.Text);
                    _selectedCustomerPaymentDetails.CustomerName = txtCustomerName.Text;
                    _selectedCustomerPaymentDetails.Phone = customer.Phone;
                    _selectedCustomerPaymentDetails.TotalDue = 100;//customer.Due;
                    _selectedCustomerPaymentDetails.Payment = payment;

                    string error;
                    _selectedCustomerPaymentDetails = _customerPaymentDetailsBl.Save(_selectedCustomerPaymentDetails, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    _customerPaymentDetailss.Add(_selectedCustomerPaymentDetails);



                    customer.Payment += payment;

                    string error1;
                    _customerBl.Save(customer, out error1);

                    if (string.IsNullOrEmpty(error1) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error1);
                        return;
                    }

                    

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
    }
}
