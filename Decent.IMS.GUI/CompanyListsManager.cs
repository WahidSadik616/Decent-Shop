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
    public partial class CompanyListsManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        CompanyListsBL _companyListsBl=new CompanyListsBL();
        List<CompanyList> _companyLists= new List<CompanyList>();
        private CompanyList _selectedCompanyLists = null;
        private int _selectedIndex = 0;

        public CompanyListsManager()
        {
            InitializeComponent();
        }

        private void CompanyListsManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                txtSearch.Text = "";

                this.LoadCompanyListsManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadCompanyListsManagers()
        {
            _companyLists = _companyListsBl.GetAll(txtSearch.Text);

            if (_companyLists.Count > 0)
            {
                _selectedCompanyLists = _companyLists[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedCompanyLists=new CompanyList();

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvCompanyListsList.AutoGenerateColumns = false;
            dgvCompanyListsList.DataSource = _companyLists.ToList();
            dgvCompanyListsList.Refresh();

            dgvCompanyListsList.ClearSelection();

            for (int i = 0; i < dgvCompanyListsList.Rows.Count; i++)
            {
                if (dgvCompanyListsList.Rows[i].Cells[0].Value.ToString() == _selectedCompanyLists.ID.ToString())
                {
                    dgvCompanyListsList.Rows[i].Selected = true;
                    dgvCompanyListsList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvCompanyListsList.Rows.Count; i++)
            {
                dgvCompanyListsList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedCompanyLists.ID.ToString();
            txtName.Text = _selectedCompanyLists.CompanyName;
            
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadCompanyListsManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedCompanyLists = _companyLists[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedCompanyLists=new CompanyList();
            this.Populate();
            dgvCompanyListsList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedCompanyLists.ID == 0)
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
            if (_companyListsBl.Delete(_selectedCompanyLists.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _companyLists.Remove(_selectedCompanyLists);
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
                _selectedCompanyLists.CompanyName = txtName.Text;
                

                bool isNew = _selectedCompanyLists.ID == 0;

                string error;
                _selectedCompanyLists=_companyListsBl.Save(_selectedCompanyLists, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _companyLists.Add(_selectedCompanyLists);
                }
                else
                {
                    _companyLists[_selectedIndex] = _selectedCompanyLists;
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
            for (int i = 0; i < _companyLists.Count; i++)
            {
                string error;
                if (_companyListsBl.Delete(_companyLists[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            CompanyListsManager pm = new CompanyListsManager();
            pm.Show();
            this.Hide();
        }


    }
}
