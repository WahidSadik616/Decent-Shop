using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decent.IMS.BL;
using Decent.IMS.Data;
using Decent.IMS.Framework;

namespace Decent.IMS.GUI
{
    public partial class DaySellManager : MetroFramework.Forms.MetroForm
    {
        private DecentDbEntities _context = new DecentDbEntities();
        private DaySellBL _daySellBl = new DaySellBL();
        private List<DaySell> _daySells = new List<DaySell>();
        private DaySell _selectedDaySell = null;
        private int _selectedIndex = 0;

        public DaySellManager()
        {
            InitializeComponent();
        }

        private void DaySellManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                txtSearch.Text = "";

                this.LoadDaySellManagers();

                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadDaySellManagers()
        {
            _daySells = _daySellBl.GetAll(txtSearch.Text);

            if (_daySells.Count > 0)
            {
                _selectedDaySell = _daySells[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedDaySell = new DaySell();

            }

            this.Populate();
            this.RefreshDgv();

        }

        private void RefreshDgv()
        {
            dgvDaySellList.AutoGenerateColumns = false;
            dgvDaySellList.DataSource = _daySells.ToList();
            dgvDaySellList.Refresh();

            dgvDaySellList.ClearSelection();

            for (int i = 0; i < dgvDaySellList.Rows.Count; i++)
            {
                if (dgvDaySellList.Rows[i].Cells[0].Value.ToString() == _selectedDaySell.ID.ToString())
                {
                    dgvDaySellList.Rows[i].Selected = true;
                    dgvDaySellList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvDaySellList.Rows.Count; i++)
            {
                dgvDaySellList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedDaySell.ID.ToString();
            txtName.Text = _selectedDaySell.Name;
            txtTime.Text = _selectedDaySell.Time;

            txtSell.Text = Convert.ToString(_selectedDaySell.Sell);
            txtAmount.Text = Convert.ToString(_selectedDaySell.Amount);
            txtSalesman.Text =_selectedDaySell.Salesman;
            txtBenifit.Text = Convert.ToString(_selectedDaySell.Benifit);

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadDaySellManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedDaySell = _daySells[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedDaySell = new DaySell();
            this.Populate();
            dgvDaySellList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedDaySell.ID == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Please...Select a row first!!!");
                return;
            }
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }

            string error;
            if (_daySellBl.Delete(_selectedDaySell.ID, out error) == false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _daySells.Remove(_selectedDaySell);
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
                _selectedDaySell.Name = txtName.Text;
                _selectedDaySell.Time = txtTime.Text;
                _selectedDaySell.ProductCategory = ddlCategory.Text;
                _selectedDaySell.Sell = Convert.ToSingle(txtSell.Text);
                _selectedDaySell.Amount = Convert.ToInt32(txtAmount.Text);
                _selectedDaySell.Salesman = txtSalesman.Text;
                _selectedDaySell.Benifit = Convert.ToSingle(txtBenifit.Text);


                bool isNew = _selectedDaySell.ID == 0;

                string error;
                _selectedDaySell = _daySellBl.Save(_selectedDaySell, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _daySells.Add(_selectedDaySell);
                }
                else
                {
                    _daySells[_selectedIndex] = _selectedDaySell;
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
                MetroFramework.MetroMessageBox.Show(this, "you cannot save...!!!");
                return false;
            }

            if (Convert.ToSingle(txtSell.Text) == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "you cannot save...!!!");
                return false;
            }

            if (Convert.ToSingle(txtBenifit.Text) == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "you cannot save...!!!");
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
            LoginForm lf = new LoginForm();
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
            for (int i = 0; i < _daySells.Count; i++)
            {
                string error;
                if (_daySellBl.Delete(_daySells[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            DaySellManager pm = new DaySellManager();
            pm.Show();
            this.Hide();
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {

            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }
            for (int i = 0; i < _daySells.Count; i++)
            {
                _daySells[i].Name = "";
                _daySells[i].Sell = 0;
                _daySells[i].Benifit = 0;

                string error;
                _daySells[i] = _daySellBl.Save(_daySells[i], out error);
                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            
            for (int i = 0; i < _daySells.Count; i++)
            {
               string error;
                    if (_daySellBl.Delete(_daySells[i].ID, out error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                
                    
              
            }
            this.Init();
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            DaySellManager pm = new DaySellManager();
            pm.Show();
            this.Hide();
       }

    }
}

