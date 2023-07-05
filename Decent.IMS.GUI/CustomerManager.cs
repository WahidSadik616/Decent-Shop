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
    public partial class CustomerManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        CustomerBL _customerBl=new CustomerBL();
        List<Customer> _customers= new List<Customer>();
        private Customer _selectedCustomer = null;
        private int _selectedIndex = 0;

        public CustomerManager()
        {
            InitializeComponent();
        }

        private void CustomerManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                txtSearch.Text = "";

                this.LoadCustomerManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadCustomerManagers()
        {
            _customers = _customerBl.GetAll(txtSearch.Text);

            if (_customers.Count > 0)
            {
                _selectedCustomer = _customers[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedCustomer=new Customer();

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvCustomerList.AutoGenerateColumns = false;
            dgvCustomerList.DataSource = _customers.ToList();
            dgvCustomerList.Refresh();

            dgvCustomerList.ClearSelection();

            for (int i = 0; i < dgvCustomerList.Rows.Count; i++)
            {
                if (dgvCustomerList.Rows[i].Cells[0].Value.ToString() == _selectedCustomer.ID.ToString())
                {
                    dgvCustomerList.Rows[i].Selected = true;
                    dgvCustomerList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvCustomerList.Rows.Count; i++)
            {
                dgvCustomerList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedCustomer.ID.ToString();
            txtName.Text = _selectedCustomer.Name;
            txtAddress.Text = _selectedCustomer.Address;
            txtPhone.Text = _selectedCustomer.Phone;
            txtEmail.Text = _selectedCustomer.Email;
            txtTotalPrice.Text = Convert.ToString(_selectedCustomer.TotalPrice);
            txtPayment.Text = Convert.ToString(_selectedCustomer.Payment);
            txtBenifit.Text = Convert.ToString(_selectedCustomer.Benifit);
            
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadCustomerManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedCustomer = _customers[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedCustomer=new Customer();
            this.Populate();
            dgvCustomerList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedCustomer.ID == 0)
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
            if (_customerBl.Delete(_selectedCustomer.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _customers.Remove(_selectedCustomer);
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
                _selectedCustomer.Name = txtName.Text;
                _selectedCustomer.Address = txtAddress.Text;
                _selectedCustomer.Phone = txtPhone.Text;
                _selectedCustomer.Email = txtEmail.Text;
                _selectedCustomer.TotalPrice = Convert.ToSingle(txtTotalPrice.Text);
                _selectedCustomer.Payment = Convert.ToSingle(txtPayment.Text);
                _selectedCustomer.Benifit = Convert.ToSingle(txtBenifit.Text);
                

                bool isNew = _selectedCustomer.ID == 0;

                string error;
                _selectedCustomer=_customerBl.Save(_selectedCustomer, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _customers.Add(_selectedCustomer);
                }
                else
                {
                    _customers[_selectedIndex] = _selectedCustomer;
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
            
           
            if (string.IsNullOrWhiteSpace(txtTotalPrice.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid total price..!!!");
                txtTotalPrice.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPayment.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Payment..!!!");
                txtPayment.Focus();
                return false;
            }
           
            return true;
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            AdminForm ud = new AdminForm();
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
            for (int i = 0; i < _customers.Count; i++)
            {
                string error;
                if (_customerBl.Delete(_customers[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            CustomerManager pm = new CustomerManager();
            pm.Show();
            this.Hide();
        }


    }
}
