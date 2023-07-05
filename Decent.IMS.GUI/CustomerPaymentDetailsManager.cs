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
using Decent.IMS.Framework;

namespace Decent.IMS.GUI
{
    public partial class CustomerPaymentDetailsManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        CustomerPaymentDetailsBL _customerPaymentDetailsBl=new CustomerPaymentDetailsBL();
        List<CustomerPaymentDetail> _customerPaymentDetailss= new List<CustomerPaymentDetail>();
        private CustomerPaymentDetail _selectedCustomerPaymentDetails = null;
        private int _selectedIndex = 0;

        public CustomerPaymentDetailsManager()
        {
            InitializeComponent();
        }

        private void CustomerPaymentDetailsManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
               

                txtSearch.Text = "";

                this.LoadCustomerPaymentDetailsManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadCustomerPaymentDetailsManagers()
        {
            _customerPaymentDetailss = _customerPaymentDetailsBl.GetAll(txtSearch.Text);

            if (_customerPaymentDetailss.Count > 0)
            {
                _selectedCustomerPaymentDetails = _customerPaymentDetailss[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedCustomerPaymentDetails = new CustomerPaymentDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvCustomerPaymentDetailsList.AutoGenerateColumns = false;
            dgvCustomerPaymentDetailsList.DataSource = _customerPaymentDetailss.ToList();
            dgvCustomerPaymentDetailsList.Refresh();

            dgvCustomerPaymentDetailsList.ClearSelection();

            for (int i = 0; i < dgvCustomerPaymentDetailsList.Rows.Count; i++)
            {
                if (dgvCustomerPaymentDetailsList.Rows[i].Cells[0].Value.ToString() == _selectedCustomerPaymentDetails.ID.ToString())
                {
                    dgvCustomerPaymentDetailsList.Rows[i].Selected = true;
                    dgvCustomerPaymentDetailsList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvCustomerPaymentDetailsList.Rows.Count; i++)
            {
                dgvCustomerPaymentDetailsList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedCustomerPaymentDetails.ID.ToString();
            dtpDate.Text = _selectedCustomerPaymentDetails.Date.ToString();
            txtName.Text = _selectedCustomerPaymentDetails.CustomerName;
            txtPhone.Text = _selectedCustomerPaymentDetails.Phone;
            txtTotalDue.Text = Convert.ToString(_selectedCustomerPaymentDetails.TotalDue);
            txtPayment.Text = Convert.ToString(_selectedCustomerPaymentDetails.Payment);
           
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadCustomerPaymentDetailsManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedCustomerPaymentDetails = _customerPaymentDetailss[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedCustomerPaymentDetails = new CustomerPaymentDetail()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvCustomerPaymentDetailsList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerPaymentDetails.ID == 0)
            {
                MetroFramework.MetroMessageBox.Show(this,"Please...Select a row first!!!");
                return;
            }
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }

            string error;
            if (_customerPaymentDetailsBl.Delete(_selectedCustomerPaymentDetails.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _customerPaymentDetailss.Remove(_selectedCustomerPaymentDetails);
            this.RefreshDgv();
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!");
            this.Init();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;
            try
            {
                _selectedCustomerPaymentDetails.Date = Convert.ToDateTime(dtpDate.Text);
                _selectedCustomerPaymentDetails.CustomerName = txtName.Text;
                _selectedCustomerPaymentDetails.Phone = txtPhone.Text;
                _selectedCustomerPaymentDetails.TotalDue = Convert.ToSingle(txtTotalDue.Text);
                _selectedCustomerPaymentDetails.Payment = Convert.ToSingle(txtPayment.Text);

                bool isNew = _selectedCustomerPaymentDetails.ID == 0;

                string error;
                _selectedCustomerPaymentDetails=_customerPaymentDetailsBl.Save(_selectedCustomerPaymentDetails, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _customerPaymentDetailss.Add(_selectedCustomerPaymentDetails);
                }
                else
                {
                    _customerPaymentDetailss[_selectedIndex] = _selectedCustomerPaymentDetails;
                }
                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
                this.RefreshDgv();
            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");
                
            }

        }
        private bool isValid()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Name..!!!");
                txtName.Focus();
                return false;
            }
            
           
            return true;
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            AdminForm ud=new AdminForm();
            ud.Show();
            this.Hide();
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            LoginForm lf=new LoginForm();
            lf.Show();
            this.Hide();
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }
            for (int i = 0; i < _customerPaymentDetailss.Count; i++)
            {
                string error;
                if (_customerPaymentDetailsBl.Delete(_customerPaymentDetailss[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            CustomerPaymentDetailsManager pm = new CustomerPaymentDetailsManager();
            pm.Show();
            this.Hide();
        }


    }
}
