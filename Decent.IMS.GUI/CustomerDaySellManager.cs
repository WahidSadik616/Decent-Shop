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
    public partial class CustomerDaySellManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        CustomerDaySellBL _customerDaySellBl=new CustomerDaySellBL();
        List<CustomerDaySell> _customerDaySells= new List<CustomerDaySell>();
        private CustomerDaySell _selectedCustomerDaySell = null;
        private int _selectedIndex = 0;

        public CustomerDaySellManager()
        {
            InitializeComponent();
        }

        private void CustomerDaySellManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                
                txtSearch.Text = "";

                this.LoadCustomerDaySellManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadCustomerDaySellManagers()
        {
            _customerDaySells = _customerDaySellBl.GetAll(txtSearch.Text);

            if (_customerDaySells.Count > 0)
            {
                _selectedCustomerDaySell = _customerDaySells[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedCustomerDaySell = new CustomerDaySell()
                {
                    Date = DateTime.Now
                };

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvCustomerDaySellList.AutoGenerateColumns = false;
            dgvCustomerDaySellList.DataSource = _customerDaySells.ToList();
            dgvCustomerDaySellList.Refresh();

            dgvCustomerDaySellList.ClearSelection();

            for (int i = 0; i < dgvCustomerDaySellList.Rows.Count; i++)
            {
               if (dgvCustomerDaySellList.Rows[i].Cells[0].Value.ToString() == _selectedCustomerDaySell.ID.ToString())
                {
                    dgvCustomerDaySellList.Rows[i].Selected = true;
                    dgvCustomerDaySellList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvCustomerDaySellList.Rows.Count; i++)
            {
                dgvCustomerDaySellList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
           
            dtpDate.Text = _selectedCustomerDaySell.Date.ToString();
            txtName.Text = _selectedCustomerDaySell.Name;
            txtTime.Text = _selectedCustomerDaySell.Time;
            txtPhone.Text = _selectedCustomerDaySell.Phone;
            txtTotalPrice.Text = Convert.ToString(_selectedCustomerDaySell.TotalPrice);
            txtPayment.Text = Convert.ToString(_selectedCustomerDaySell.Payment);
            txtBenifit.Text = Convert.ToString(_selectedCustomerDaySell.Benifit);
            
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadCustomerDaySellManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedCustomerDaySell = _customerDaySells[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
           _selectedCustomerDaySell = new CustomerDaySell()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvCustomerDaySellList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerDaySell.ID == 0)
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
            if (_customerDaySellBl.Delete(_selectedCustomerDaySell.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _customerDaySells.Remove(_selectedCustomerDaySell);
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
                _selectedCustomerDaySell.Date = Convert.ToDateTime(dtpDate.Text);
                _selectedCustomerDaySell.Time = txtTime.Text;
                _selectedCustomerDaySell.Name = txtName.Text;
                _selectedCustomerDaySell.Phone = txtPhone.Text;
                _selectedCustomerDaySell.TotalPrice = Convert.ToSingle(txtTotalPrice.Text);
                _selectedCustomerDaySell.Payment = Convert.ToSingle(txtPayment.Text);
                _selectedCustomerDaySell.Benifit = Convert.ToSingle(txtBenifit.Text);
                
                bool isNew = _selectedCustomerDaySell.ID == 0;

                string error;
                _selectedCustomerDaySell=_customerDaySellBl.Save(_selectedCustomerDaySell, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _customerDaySells.Add(_selectedCustomerDaySell);
                }
                else
                {
                    _customerDaySells[_selectedIndex] = _selectedCustomerDaySell;
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
            for (int i = 0; i < _customerDaySells.Count; i++)
            {
                string error;
                if (_customerDaySellBl.Delete(_customerDaySells[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            CustomerDaySellManager pm = new CustomerDaySellManager();
            pm.Show();
            this.Hide();
        }


    }
}
