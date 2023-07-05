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
    public partial class SalesmanManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        SalesmanBL _salesmanBl=new SalesmanBL();
        List<Salesman> _salesmans= new List<Salesman>();
        private Salesman _selectedSalesman = null;
        private int _selectedIndex = 0;

        public SalesmanManager()
        {
            InitializeComponent();
        }

        private void SalesmanManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
               
                txtSearch.Text = "";

                this.LoadSalesmanManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadSalesmanManagers()
        {
            _salesmans = _salesmanBl.GetAll(txtSearch.Text);

            if (_salesmans.Count > 0)
            {
                _selectedSalesman = _salesmans[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedSalesman=new Salesman();

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvSalesmanList.AutoGenerateColumns = false;
            dgvSalesmanList.DataSource = _salesmans.ToList();
            dgvSalesmanList.Refresh();

            dgvSalesmanList.ClearSelection();

            for (int i = 0; i < dgvSalesmanList.Rows.Count; i++)
            {
                if (dgvSalesmanList.Rows[i].Cells[0].Value.ToString() == _selectedSalesman.ID.ToString())
                {
                    dgvSalesmanList.Rows[i].Selected = true;
                    dgvSalesmanList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvSalesmanList.Rows.Count; i++)
            {
                dgvSalesmanList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedSalesman.ID.ToString();
            txtName.Text = _selectedSalesman.Name;
            txtSalary.Text = Convert.ToString(_selectedSalesman.Salary);
            txtPaySalary.Text = Convert.ToString(_selectedSalesman.PaySalary);
            txtPhone.Text = _selectedSalesman.Phone;
            txtEmail.Text = _selectedSalesman.Email;
            txtAddress.Text = _selectedSalesman.Address;
            txtBenifit.Text = Convert.ToString(_selectedSalesman.Benifit);
           
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadSalesmanManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedSalesman = _salesmans[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedSalesman=new Salesman();
            this.Populate();
            dgvSalesmanList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedSalesman.ID == 0)
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
            if (_salesmanBl.Delete(_selectedSalesman.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _salesmans.Remove(_selectedSalesman);
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
                _selectedSalesman.Name = txtName.Text;
                _selectedSalesman.Salary = Convert.ToSingle(txtSalary.Text);
                _selectedSalesman.PaySalary = Convert.ToSingle(txtPaySalary.Text);
                _selectedSalesman.Phone = txtPhone.Text;
                _selectedSalesman.Email = txtEmail.Text;
                _selectedSalesman.Address = txtAddress.Text;
                _selectedSalesman.Benifit = Convert.ToSingle(txtBenifit.Text);
              
                bool isNew = _selectedSalesman.ID == 0;

                string error;
                _selectedSalesman=_salesmanBl.Save(_selectedSalesman, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _salesmans.Add(_selectedSalesman);
                }
                else
                {
                    _salesmans[_selectedIndex] = _selectedSalesman;
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
            DecentDbEntities _context = new DecentDbEntities();
            var salesmanPhone = _context.Salesmen.FirstOrDefault(u => u.Phone == txtPhone.Text);
            var salesmanEmail = _context.Salesmen.FirstOrDefault(u => u.Email == txtEmail.Text);
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Name..!!!");
                txtName.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string email = txtEmail.Text;
                if (!email.Contains("@gmail.com"))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid Email..!!!");
                    txtEmail.Focus();
                    return false;
                }
                var salesmanMail=_context.Salesmen.FirstOrDefault(u => u.ID == _selectedSalesman.ID);
                if (salesmanMail == null)
                {
                    if (salesmanEmail != null)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Email Id is already exist...!!!");
                        txtEmail.Focus();
                        return false;
                    }
                }
                
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Phone Number..!!!");
                txtPhone.Focus();
                return false;
            }

            var salesmanPhon=_context.Salesmen.FirstOrDefault(u => u.ID == _selectedSalesman.ID);
            if (salesmanPhon == null)
            {
                if (salesmanPhone != null)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Phone number is already exist...!!!");
                    txtPhone.Focus();
                    return false;
                }
            }
           

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Address Please..!!!");
                txtAddress.Focus();
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
            _selectedSalesman.Benifit = 0;
            string error;
            _selectedSalesman = _salesmanBl.Save(_selectedSalesman, out error);
            this.Populate();
            this.RefreshDgv();
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
               DialogResult.No)
            {
                return;
            }
            for (int i = 0; i < _salesmans.Count; i++)
            {
                string error;
                if (_salesmanBl.Delete(_salesmans[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            SalesmanManager pm = new SalesmanManager();
            pm.Show();
            this.Hide();
        }


    }
}
