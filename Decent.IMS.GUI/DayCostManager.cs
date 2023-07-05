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
    public partial class DayCostManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context = new DecentDbEntities();
        DayCostBL _dayCostBl = new DayCostBL();
        List<DayCost> _dayCosts = new List<DayCost>();
        private DayCost _selectedDayCost = null;
        private int _selectedIndex = 0;

        public DayCostManager()
        {
            InitializeComponent();
        }

        private void DayCostManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {

                txtSearch.Text = "";

                this.LoadDayCostManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadDayCostManagers()
        {
            _dayCosts = _dayCostBl.GetAll(txtSearch.Text);

            if (_dayCosts.Count > 0)
            {
                _selectedDayCost = _dayCosts[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedDayCost = new DayCost();

            }

            this.Populate();
            this.RefreshDgv();


        }

        private void RefreshDgv()
        {
            dgvDayCostList.AutoGenerateColumns = false;
            dgvDayCostList.DataSource = _dayCosts.ToList();
            dgvDayCostList.Refresh();

            dgvDayCostList.ClearSelection();

            for (int i = 0; i < dgvDayCostList.Rows.Count; i++)
            {
                if (dgvDayCostList.Rows[i].Cells[0].Value.ToString() == _selectedDayCost.ID.ToString())
                {
                    dgvDayCostList.Rows[i].Selected = true;
                    dgvDayCostList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvDayCostList.Rows.Count; i++)
            {
                dgvDayCostList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedDayCost.ID.ToString();
            txtName.Text = _selectedDayCost.Name;
            txtCostAmount.Text = Convert.ToString(_selectedDayCost.CostAmount);
           
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadDayCostManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedDayCost = _dayCosts[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedDayCost=new DayCost();
            this.Populate();
            dgvDayCostList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedDayCost.ID == 0)
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
            if (_dayCostBl.Delete(_selectedDayCost.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _dayCosts.Remove(_selectedDayCost);
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
                _selectedDayCost.Name = txtName.Text;
                _selectedDayCost.CostAmount = Convert.ToSingle(txtCostAmount.Text);
                

                bool isNew = _selectedDayCost.ID == 0;

                string error;
                _selectedDayCost=_dayCostBl.Save(_selectedDayCost, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _dayCosts.Add(_selectedDayCost);
                }
                else
                {
                    _dayCosts[_selectedIndex] = _selectedDayCost;
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
            
            if (string.IsNullOrWhiteSpace(txtCostAmount.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Password..!!!");
                txtCostAmount.Focus();
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
            for (int i = 0; i < _dayCosts.Count; i++)
            {
                string error;
                if (_dayCostBl.Delete(_dayCosts[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }
                _dayCosts.Remove(_dayCosts[i]);

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            DayCostManager pm = new DayCostManager();
            pm.Show();
            this.Hide();
        }


    }
}
