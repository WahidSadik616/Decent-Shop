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
    public partial class DepartmentManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        DepartmentBL _departmentBl=new DepartmentBL();
        List<Department> _departments= new List<Department>();
        private Department _selectedDepartment = null;
        private int _selectedIndex = 0;

        public DepartmentManager()
        {
            InitializeComponent();
        }

        private void DepartmentManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                txtSearch.Text = "";

                this.LoadDepartmentManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadDepartmentManagers()
        {
            _departments = _departmentBl.GetAll(txtSearch.Text);

            if (_departments.Count > 0)
            {
                _selectedDepartment = _departments[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedDepartment=new Department();

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvDepartmentList.AutoGenerateColumns = false;
            dgvDepartmentList.DataSource = _departments.ToList();
            dgvDepartmentList.Refresh();

            dgvDepartmentList.ClearSelection();

            for (int i = 0; i < dgvDepartmentList.Rows.Count; i++)
            {
                if (dgvDepartmentList.Rows[i].Cells[0].Value.ToString() == _selectedDepartment.ID.ToString())
                {
                    dgvDepartmentList.Rows[i].Selected = true;
                    dgvDepartmentList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvDepartmentList.Rows.Count; i++)
            {
                dgvDepartmentList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedDepartment.ID.ToString();
            txtName.Text = _selectedDepartment.Name;
            
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadDepartmentManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedDepartment = _departments[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedDepartment=new Department();
            this.Populate();
            dgvDepartmentList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedDepartment.ID == 0)
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
            if (_departmentBl.Delete(_selectedDepartment.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _departments.Remove(_selectedDepartment);
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
                _selectedDepartment.Name = txtName.Text;
               

                bool isNew = _selectedDepartment.ID == 0;

                string error;
                _selectedDepartment=_departmentBl.Save(_selectedDepartment, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _departments.Add(_selectedDepartment);
                }
                else
                {
                    _departments[_selectedIndex] = _selectedDepartment;
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
            UserDetailForm ud=new UserDetailForm();
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
            for (int i = 0; i < _departments.Count; i++)
            {
                string error;
                if (_departmentBl.Delete(_departments[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            DepartmentManager pm = new DepartmentManager();
            pm.Show();
            this.Hide();
        }


    }
}
