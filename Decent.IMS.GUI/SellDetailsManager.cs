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
    public partial class SellDetailsManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        SellDetailsBL _sellDetailsBl=new SellDetailsBL();
        List<SellDetail> _sellDetailss= new List<SellDetail>();
        private SellDetail _selectedSellDetails = null;
        private int _selectedIndex = 0;

        public SellDetailsManager()
        {
            InitializeComponent();
        }

        private void SellDetailsManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                txtSearch.Text = "";

                this.LoadSellDetailsManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadSellDetailsManagers()
        {
            _sellDetailss = _sellDetailsBl.GetAll(txtSearch.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedSellDetails = new SellDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvSellDetailsList.AutoGenerateColumns = false;
            dgvSellDetailsList.DataSource = _sellDetailss.ToList();
            dgvSellDetailsList.Refresh();

            dgvSellDetailsList.ClearSelection();

            for (int i = 0; i < dgvSellDetailsList.Rows.Count; i++)
            {
                if (dgvSellDetailsList.Rows[i].Cells[0].Value.ToString() == _selectedSellDetails.ID.ToString())
                {
                    dgvSellDetailsList.Rows[i].Selected = true;
                    dgvSellDetailsList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvSellDetailsList.Rows.Count; i++)
            {
                dgvSellDetailsList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedSellDetails.ID.ToString();
            dtpDate.Text = _selectedSellDetails.Date.ToString();
            txtSalesman.Text = _selectedSellDetails.Salesman;
            txtCustomer.Text = _selectedSellDetails.Customer;
            txtCusPhn.Text = _selectedSellDetails.CustomerPhone;
            txtProduct.Text = _selectedSellDetails.ProductName;
            ddlCategory.SelectedValue = _selectedSellDetails.ProductCategoryId;
            txtRealPrice.Text = Convert.ToString(_selectedSellDetails.RealPriceRate);
            txtSellPrice.Text = Convert.ToString(_selectedSellDetails.SellPriceRate);
            txtAmount.Text = Convert.ToString(_selectedSellDetails.Amount);
            txtTotalPrice.Text = Convert.ToString(_selectedSellDetails.TotalPrice);
            txtBenifit.Text = Convert.ToString(_selectedSellDetails.Benifit);
            txtMemo.Text = Convert.ToString(_selectedSellDetails.CashMemoId);
            txtTime.Text= _selectedSellDetails.Time;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadSellDetailsManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedSellDetails = _sellDetailss[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedSellDetails = new SellDetail()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvSellDetailsList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedSellDetails.ID == 0)
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
            if (_sellDetailsBl.Delete(_selectedSellDetails.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _sellDetailss.Remove(_selectedSellDetails);
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
                _selectedSellDetails.Date = Convert.ToDateTime(dtpDate.Text);
                _selectedSellDetails.Salesman = txtSalesman.Text;
                _selectedSellDetails.Customer = txtCustomer.Text;
                _selectedSellDetails.CustomerPhone = txtCusPhn.Text;
                _selectedSellDetails.ProductName = txtProduct.Text;
                _selectedSellDetails.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                _selectedSellDetails.RealPriceRate = Convert.ToSingle(txtRealPrice.Text);
                _selectedSellDetails.SellPriceRate = Convert.ToSingle(txtSellPrice.Text);
                _selectedSellDetails.Amount = Convert.ToInt32(txtAmount.Text);
                _selectedSellDetails.TotalPrice = Convert.ToSingle(txtTotalPrice.Text);
                _selectedSellDetails.Benifit = Convert.ToSingle(txtBenifit.Text);
                _selectedSellDetails.CashMemoId = Convert.ToInt32(txtMemo.Text);
                _selectedSellDetails.Time = txtTime.Text;

                bool isNew = _selectedSellDetails.ID == 0;

                string error;
                _selectedSellDetails=_sellDetailsBl.Save(_selectedSellDetails, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _sellDetailss.Add(_selectedSellDetails);
                }
                else
                {
                    _sellDetailss[_selectedIndex] = _selectedSellDetails;
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
            if (string.IsNullOrWhiteSpace(txtSalesman.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Name..!!!");
                txtSalesman.Focus();
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
            for (int i = 0; i < _sellDetailss.Count; i++)
            {
                string error;
                if (_sellDetailsBl.Delete(_sellDetailss[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            SellDetailsManager pm = new SellDetailsManager();
            pm.Show();
            this.Hide();
        }

        private void metroPanel3_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
