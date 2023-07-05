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
    public partial class SalesmanPaymentManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        SalesmanPaymentBL _salesmanPaymentBl=new SalesmanPaymentBL();
        List<SalesmanPayment> _salesmanPayments= new List<SalesmanPayment>();
        private SalesmanPayment _selectedSalesmanPayment = null;
        private int _selectedIndex = 0;

        public SalesmanPaymentManager()
        {
            InitializeComponent();
        }

        private void SalesmanPaymentManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                
                txtSearch.Text = "";

                this.LoadSalesmanPaymentManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadSalesmanPaymentManagers()
        {
            _salesmanPayments = _salesmanPaymentBl.GetAll(txtSearch.Text);

            if (_salesmanPayments.Count > 0)
            {
                _selectedSalesmanPayment = _salesmanPayments[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedSalesmanPayment = new SalesmanPayment()
                {
                    Date = DateTime.Now
                };

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvSalesmanPaymentList.AutoGenerateColumns = false;
            dgvSalesmanPaymentList.DataSource = _salesmanPayments.ToList();
            dgvSalesmanPaymentList.Refresh();

            dgvSalesmanPaymentList.ClearSelection();

            for (int i = 0; i < dgvSalesmanPaymentList.Rows.Count; i++)
            {
                if (dgvSalesmanPaymentList.Rows[i].Cells[0].Value.ToString() == _selectedSalesmanPayment.ID.ToString())
                {
                    dgvSalesmanPaymentList.Rows[i].Selected = true;
                    dgvSalesmanPaymentList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvSalesmanPaymentList.Rows.Count; i++)
            {
                dgvSalesmanPaymentList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            dtpDate.Text = _selectedSalesmanPayment.Date.ToString();
            txtID.Text = _selectedSalesmanPayment.ID.ToString();
            txtName.Text = _selectedSalesmanPayment.Name;
            txtPhone.Text = _selectedSalesmanPayment.Phone;
            txtTotalDue.Text = Convert.ToString(_selectedSalesmanPayment.TotalDue);
            txtPayment.Text = Convert.ToString(_selectedSalesmanPayment.Payment);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadSalesmanPaymentManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedSalesmanPayment = _salesmanPayments[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedSalesmanPayment = new SalesmanPayment()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvSalesmanPaymentList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedSalesmanPayment.ID == 0)
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
            if (_salesmanPaymentBl.Delete(_selectedSalesmanPayment.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _salesmanPayments.Remove(_selectedSalesmanPayment);
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
                _selectedSalesmanPayment.Date = Convert.ToDateTime(dtpDate.Text);
                _selectedSalesmanPayment.Name = txtName.Text;
                _selectedSalesmanPayment.Phone = txtPhone.Text;
                _selectedSalesmanPayment.TotalDue = Convert.ToSingle(txtTotalDue.Text);
                _selectedSalesmanPayment.Payment = Convert.ToSingle(txtPayment.Text);
                
                
                bool isNew = _selectedSalesmanPayment.ID == 0;

                string error;
                _selectedSalesmanPayment=_salesmanPaymentBl.Save(_selectedSalesmanPayment, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _salesmanPayments.Add(_selectedSalesmanPayment);
                }
                else
                {
                    _salesmanPayments[_selectedIndex] = _selectedSalesmanPayment;
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
            for (int i = 0; i < _salesmanPayments.Count; i++)
            {
                string error;
                if (_salesmanPaymentBl.Delete(_salesmanPayments[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            SalesmanPaymentManager pm = new SalesmanPaymentManager();
            pm.Show();
            this.Hide();
        }


    }
}
