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
    public partial class DayPaymentManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        DayPaymentBL _dayPaymentBl=new DayPaymentBL();
        List<DayPayment> _dayPayments= new List<DayPayment>();
        private DayPayment _selectedDayPayment = null;
        private int _selectedIndex = 0;

        public DayPaymentManager()
        {
            InitializeComponent();
        }

        private void DayPaymentManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                txtSearch.Text = "";

                this.LoadDayPaymentManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadDayPaymentManagers()
        {
            _dayPayments = _dayPaymentBl.GetAll(txtSearch.Text);

            if (_dayPayments.Count > 0)
            {
                _selectedDayPayment = _dayPayments[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedDayPayment=new DayPayment();

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvDayPaymentList.AutoGenerateColumns = false;
            dgvDayPaymentList.DataSource = _dayPayments.ToList();
            dgvDayPaymentList.Refresh();

            dgvDayPaymentList.ClearSelection();

            for (int i = 0; i < dgvDayPaymentList.Rows.Count; i++)
            {
                if (dgvDayPaymentList.Rows[i].Cells[0].Value.ToString() == _selectedDayPayment.ID.ToString())
                {
                    dgvDayPaymentList.Rows[i].Selected = true;
                    dgvDayPaymentList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvDayPaymentList.Rows.Count; i++)
            {
                dgvDayPaymentList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedDayPayment.ID.ToString();
            txtName.Text = _selectedDayPayment.Name;
            txtAmount.Text = Convert.ToString(_selectedDayPayment.Amount);
            
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadDayPaymentManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedDayPayment = _dayPayments[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedDayPayment=new DayPayment();
            this.Populate();
            dgvDayPaymentList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedDayPayment.ID == 0)
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
            if (_dayPaymentBl.Delete(_selectedDayPayment.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _dayPayments.Remove(_selectedDayPayment);
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
                _selectedDayPayment.Name = txtName.Text;
                _selectedDayPayment.Amount = Convert.ToSingle(txtAmount.Text);
               

                bool isNew = _selectedDayPayment.ID == 0;

                string error;
                _selectedDayPayment=_dayPaymentBl.Save(_selectedDayPayment, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _dayPayments.Add(_selectedDayPayment);
                }
                else
                {
                    _dayPayments[_selectedIndex] = _selectedDayPayment;
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
           
            
            if (string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Amount..!!!");
                txtAmount.Focus();
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
            for (int i = 0; i < _dayPayments.Count; i++)
            {
                string error;
                if (_dayPaymentBl.Delete(_dayPayments[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            DayPaymentManager pm = new DayPaymentManager();
            pm.Show();
            this.Hide();
        }


    }
}
