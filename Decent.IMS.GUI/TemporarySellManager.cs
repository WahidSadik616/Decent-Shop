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
    public partial class TemporarySellManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        TemporarySellBL _temporarySellBl=new TemporarySellBL();
        List<TemporarySell> _temporarySells= new List<TemporarySell>();
        private TemporarySell _selectedTemporarySell = null;
        private int _selectedIndex = 0;

        public TemporarySellManager()
        {
            InitializeComponent();
        }

        private void TemporarySellManager_Load(object sender, EventArgs e)
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

                this.LoadTemporarySellManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadTemporarySellManagers()
        {
            _temporarySells = _temporarySellBl.GetAll(txtSearch.Text);

            if (_temporarySells.Count > 0)
            {
                _selectedTemporarySell = _temporarySells[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedTemporarySell = new TemporarySell()
                {
                    Date = DateTime.Now
                };

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvTemporarySellList.AutoGenerateColumns = false;
            dgvTemporarySellList.DataSource = _temporarySells.ToList();
            dgvTemporarySellList.Refresh();

            dgvTemporarySellList.ClearSelection();

            for (int i = 0; i < dgvTemporarySellList.Rows.Count; i++)
            {
                if (dgvTemporarySellList.Rows[i].Cells[0].Value.ToString() == _selectedTemporarySell.ID.ToString())
                {
                    dgvTemporarySellList.Rows[i].Selected = true;
                    dgvTemporarySellList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvTemporarySellList.Rows.Count; i++)
            {
                dgvTemporarySellList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedTemporarySell.ID.ToString();
            dtpDateTime.Text = _selectedTemporarySell.Date.ToString();
            txtName.Text = _selectedTemporarySell.Name;
            ddlCategory.SelectedValue = _selectedTemporarySell.ProductCategoryId;
            txtQuantity.Text = Convert.ToString(_selectedTemporarySell.Quantity);
            txtRealPriceRate.Text = Convert.ToString(_selectedTemporarySell.RealPriceRate);
            txtPriceRate.Text = Convert.ToString(_selectedTemporarySell.PriceRate);
            txtTotalPrice.Text = Convert.ToString(_selectedTemporarySell.TotalPrice);
            txtBenifit.Text = Convert.ToString(_selectedTemporarySell.Benifit);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadTemporarySellManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedTemporarySell = _temporarySells[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedTemporarySell = new TemporarySell()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvTemporarySellList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedTemporarySell.ID == 0)
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
           if (_temporarySellBl.Delete(_selectedTemporarySell.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _temporarySells.Remove(_selectedTemporarySell);
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
                var product=_context.Products.FirstOrDefault(
                    u =>
                        u.Name == _selectedTemporarySell.Name &&
                        u.ProductCategoryId == _selectedTemporarySell.ProductCategoryId);

                if (product != null)
                {
                     double priceRate = Convert.ToSingle(txtPriceRate.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);
                    _selectedTemporarySell.Count = _temporarySells.Count;
                _selectedTemporarySell.Date = Convert.ToDateTime(dtpDateTime.Text);
                _selectedTemporarySell.Name = txtName.Text;
                _selectedTemporarySell.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                _selectedTemporarySell.Quantity = quantity;
                _selectedTemporarySell.RealPriceRate = product.PriceRate;
                _selectedTemporarySell.PriceRate = priceRate;
                _selectedTemporarySell.TotalPrice = quantity*priceRate;
                _selectedTemporarySell.Benifit = (product.PriceRate - priceRate)*quantity;
               
                bool isNew = _selectedTemporarySell.ID == 0;

                string error;
                _selectedTemporarySell=_temporarySellBl.Save(_selectedTemporarySell, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _temporarySells.Add(_selectedTemporarySell);
                }
                else
                {
                    _temporarySells[_selectedIndex] = _selectedTemporarySell;
                }
                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
                this.RefreshDgv();
            }

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
            for (int i = 0; i < _temporarySells.Count; i++)
            {
                string error;
                if (_temporarySellBl.Delete(_temporarySells[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            TemporarySellManager pm = new TemporarySellManager();
            pm.Show();
            this.Hide();
        }


    }
}
