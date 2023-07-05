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
    public partial class CashMemoManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        CashMemoBL _cashMemoBl=new CashMemoBL();
        List<CashMemo> _cashMemos= new List<CashMemo>();
        private CashMemo _selectedCashMemo = null;
        private int _selectedIndex = 0;

        public CashMemoManager()
        {
            InitializeComponent();
        }

        private void CashMemoManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                
                txtSearch.Text = "";

                this.LoadCashMemoManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadCashMemoManagers()
        {
            _cashMemos = _cashMemoBl.GetAll(txtSearch.Text);

            if (_cashMemos.Count > 0)
            {
                _selectedCashMemo = _cashMemos[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedCashMemo = new CashMemo()
                {
                    Date = DateTime.Now
                };

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvCashMemoList.AutoGenerateColumns = false;
            dgvCashMemoList.DataSource = _cashMemos.ToList();
            dgvCashMemoList.Refresh();

            dgvCashMemoList.ClearSelection();

            for (int i = 0; i < dgvCashMemoList.Rows.Count; i++)
            {
                if (dgvCashMemoList.Rows[i].Cells[0].Value.ToString() == _selectedCashMemo.ID.ToString())
                {
                    dgvCashMemoList.Rows[i].Selected = true;
                    dgvCashMemoList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvCashMemoList.Rows.Count; i++)
            {
                dgvCashMemoList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedCashMemo.ID.ToString();
            txtCustomerName.Text = _selectedCashMemo.CustomerName;
            txtCustomerPhone.Text = _selectedCashMemo.CustomerPhone;
            txtTime.Text = _selectedCashMemo.Time;
            txtSalesman.Text = _selectedCashMemo.Salesman;
            txtSalesPhn.Text = _selectedCashMemo.Phone;

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadCashMemoManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedCashMemo = _cashMemos[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedCashMemo = new CashMemo()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvCashMemoList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
           if (_selectedCashMemo.ID == 0)
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
            if (_cashMemoBl.Delete(_selectedCashMemo.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _cashMemos.Remove(_selectedCashMemo);
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
                _selectedCashMemo.CustomerName = txtCustomerName.Text;
                _selectedCashMemo.CustomerPhone = txtCustomerPhone.Text;
                _selectedCashMemo.Time = txtTime.Text;
                _selectedCashMemo.Salesman = txtSalesman.Text;
                _selectedCashMemo.Phone = txtSalesPhn.Text;
                _selectedCashMemo.Date = Convert.ToDateTime(dtpDate.Text);
                

                bool isNew = _selectedCashMemo.ID == 0;

                string error;
                _selectedCashMemo=_cashMemoBl.Save(_selectedCashMemo, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _cashMemos.Add(_selectedCashMemo);
                }
                else
                {
                    _cashMemos[_selectedIndex] = _selectedCashMemo;
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
           
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Customer Name..!!!");
                txtCustomerName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCustomerPhone.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Customer Phone..!!!");
                txtCustomerName.Focus();
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
           for (int i = 0; i < _cashMemos.Count; i++)
            {
                string error;
               if (_cashMemoBl.Delete(_cashMemos[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

           }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            CashMemoManager pm = new CashMemoManager();
            pm.Show();
            this.Hide();
        }

        private void metroButton8_Click_1(object sender, EventArgs e)
        {

        }
    }
}
