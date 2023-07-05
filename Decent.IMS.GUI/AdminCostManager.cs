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
    public partial class AdminCostManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context = new DecentDbEntities();
        CostDetailsBL _costDetailBl = new CostDetailsBL();
        List<CostDetail> _costDetails = new List<CostDetail>();
        private CostDetail _selectedCostDetail = null;
        private int _selectedIndex = 0;

        public AdminCostManager()
        {
            InitializeComponent();
        }

        private void CostDetailManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {


                txtSearch.Text = "";

                this.LoadCostDetailsManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadCostDetailsManagers()
        {
            _costDetails = _costDetailBl.GetAll(txtSearch.Text);

            if (_costDetails.Count > 0)
            {
                _selectedCostDetail = _costDetails[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedCostDetail = new CostDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.Populate();
            this.RefreshDgv();

        }

        private void RefreshDgv()
        {
            dgvCostDetailsList.AutoGenerateColumns = false;
            dgvCostDetailsList.DataSource = _costDetails.ToList();
            dgvCostDetailsList.Refresh();

            dgvCostDetailsList.ClearSelection();

            for (int i = 0; i < dgvCostDetailsList.Rows.Count; i++)
            {
                if (dgvCostDetailsList.Rows[i].Cells[0].Value.ToString() == _selectedCostDetail.ID.ToString())
                {
                    dgvCostDetailsList.Rows[i].Selected = true;
                    dgvCostDetailsList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvCostDetailsList.Rows.Count; i++)
            {
                dgvCostDetailsList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedCostDetail.ID.ToString();
            dtpDate.Text = _selectedCostDetail.Date.ToString();
            txtName.Text = _selectedCostDetail.Name;
            txtAmount.Text = Convert.ToString(_selectedCostDetail.Amount);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadCostDetailsManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedCostDetail = _costDetails[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedCostDetail = new CostDetail()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvCostDetailsList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedCostDetail.ID == 0)
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
            if (_costDetailBl.Delete(_selectedCostDetail.ID, out error) == false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _costDetails.Remove(_selectedCostDetail);
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
                _selectedCostDetail.Date = Convert.ToDateTime(dtpDate.Text);
                _selectedCostDetail.Name = txtName.Text;
                _selectedCostDetail.Amount = Convert.ToSingle(txtAmount.Text);


                bool isNew = _selectedCostDetail.ID == 0;

                string error;
                _selectedCostDetail = _costDetailBl.Save(_selectedCostDetail, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _costDetails.Add(_selectedCostDetail);
                }
                else
                {
                    _costDetails[_selectedIndex] = _selectedCostDetail;
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
            for (int i = 0; i < _costDetails.Count; i++)
            {
                string error;
                if (_costDetailBl.Delete(_costDetails[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            
        }


    }
}
