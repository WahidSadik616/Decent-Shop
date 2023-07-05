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
    public partial class DayIncomeManager : MetroFramework.Forms.MetroForm
    {
        private DecentDbEntities _context = new DecentDbEntities();
        private DayIncomeBL _dayIncomeBl = new DayIncomeBL();
        private List<DayIncome> _dayIncomes = new List<DayIncome>();
        private DayIncome _selectedDayIncome = null;
        private int _selectedIndex = 0;

        public DayIncomeManager()
        {
            InitializeComponent();
        }

        private void DayIncomeManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                txtSearch.Text = "";

                this.LoadDayIncomeManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadDayIncomeManagers()
        {
            _dayIncomes = _dayIncomeBl.GetAll(txtSearch.Text);

            if (_dayIncomes.Count > 0)
            {
                _selectedDayIncome = _dayIncomes[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedDayIncome = new DayIncome()
                {
                    Date = DateTime.Now
                };
                

            }

            this.Populate();
            this.RefreshDgv();

        }

        private void RefreshDgv()
        {
            dgvDayIncomeList.AutoGenerateColumns = false;
            dgvDayIncomeList.DataSource = _dayIncomes.ToList();
            dgvDayIncomeList.Refresh();

            dgvDayIncomeList.ClearSelection();

            for (int i = 0; i < dgvDayIncomeList.Rows.Count; i++)
            {
               if (dgvDayIncomeList.Rows[i].Cells[0].Value.ToString() == _selectedDayIncome.ID.ToString())
                {
                    dgvDayIncomeList.Rows[i].Selected = true;
                    dgvDayIncomeList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvDayIncomeList.Rows.Count; i++)
            {
                dgvDayIncomeList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedDayIncome.ID.ToString();
            txtSell.Text = Convert.ToString(_selectedDayIncome.Sell);
            txtCost.Text = Convert.ToString(_selectedDayIncome.Cost);
            txtPayment.Text = Convert.ToString(_selectedDayIncome.Payment);
            txtBenifit.Text = Convert.ToString(_selectedDayIncome.Benifit);
            dtpDate.Text = _selectedDayIncome.Date.ToString();

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadDayIncomeManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedDayIncome = _dayIncomes[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
          _selectedDayIncome = new DayIncome()
            {
                Date = DateTime.Now
            };
         
            this.Populate();
            dgvDayIncomeList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedDayIncome.ID == 0)
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
            if (_dayIncomeBl.Delete(_selectedDayIncome.ID, out error) == false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

           _dayIncomes.Remove(_selectedDayIncome);
            this.RefreshDgv();
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!");
            this.Init();
        }

        private void metroButton5_Click_1(object sender, EventArgs e)
        {
            if (!isValid())
                return;
            try
            {
                _selectedDayIncome.Date = Convert.ToDateTime(dtpDate.Text);
                _selectedDayIncome.Sell = Convert.ToSingle(txtSell.Text);
                _selectedDayIncome.Cost = Convert.ToSingle(txtCost.Text);
                _selectedDayIncome.Payment = Convert.ToSingle(txtPayment.Text);
                _selectedDayIncome.Benifit = Convert.ToSingle(txtBenifit.Text);
                


                bool isNew = _selectedDayIncome.ID == 0;

                string error;
                _selectedDayIncome = _dayIncomeBl.Save(_selectedDayIncome, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _dayIncomes.Add(_selectedDayIncome);
                }
                else
                {
                    _dayIncomes[_selectedIndex] = _selectedDayIncome;
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
            if (string.IsNullOrWhiteSpace(txtSell.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Name..!!!");
                txtSell.Focus();
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
            for (int i = 0; i < _dayIncomes.Count; i++)
            {
                string error;
                if (_dayIncomeBl.Delete(_dayIncomes[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            DayIncomeManager pm = new DayIncomeManager();
            pm.Show();
            this.Hide();
        }

        private void metroLabel6_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

       

    }

}
