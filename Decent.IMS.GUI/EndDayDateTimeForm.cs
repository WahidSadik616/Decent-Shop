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

namespace Decent.IMS.GUI
{
    public partial class EndDayDateTimeForm : MetroFramework.Forms.MetroForm
    {
        private DecentDbEntities _context = new DecentDbEntities();
        private DaySellBL _daySellBl = new DaySellBL();
        private List<DaySell> _daySells = new List<DaySell>();
        private DayIncomeBL _dayIncomeBl = new DayIncomeBL();
        private List<DayIncome> _dayIncome = new List<DayIncome>();
        private DayCostBL _dayCostBl = new DayCostBL();
        private List<DayCost> _dayCosts = new List<DayCost>();
        private DayPaymentBL _dayPaymentBl = new DayPaymentBL();
        private List<DayPayment> _dayPayments = new List<DayPayment>();
        private DayIncome _selectedDayIncome = null;
        public EndDayDateTimeForm()
        {
            InitializeComponent();
        }

        private void EndDayDateTimeForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            AdminForm af=new AdminForm();
            af.Show();
            this.Hide();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            LoginForm lf=new LoginForm();
            lf.Show();
            this.Hide();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }

            _daySells = _daySellBl.GetAll("");
            _dayIncome = _dayIncomeBl.GetAll("");
            _dayCosts = _dayCostBl.GetAll("");
            _dayPayments = _dayPaymentBl.GetAll("");

            double totalDaySell = 0;
            double totalDayBenifit = 0;
            double totalDayCost = 0;
            double totalDayPayment = 0;

            for (int i = 0; i < _daySells.Count; i++)
            {
                totalDaySell += _daySells[i].Sell;
                totalDayBenifit += _daySells[i].Benifit;

            }

            for (int i = 0; i < _dayCosts.Count; i++)
            {
                totalDayCost += _dayCosts[i].CostAmount;

            }

            for (int i = 0; i < _dayPayments.Count; i++)
            {
                totalDayPayment += _dayPayments[i].Amount;

            }


            _selectedDayIncome = new DayIncome();


            _selectedDayIncome.Date = Convert.ToDateTime(dtpDateTime.Text);
            _selectedDayIncome.Sell = totalDaySell;
            _selectedDayIncome.Benifit = totalDayBenifit;
            _selectedDayIncome.Cost = totalDayCost;
            _selectedDayIncome.Payment = totalDayPayment;
            


            bool isNew1 = _selectedDayIncome.ID == 0;

            string error2;
            _selectedDayIncome = _dayIncomeBl.Save(_selectedDayIncome, out error2);

            if (string.IsNullOrEmpty(error2) == false)
            {
                MetroFramework.MetroMessageBox.Show(this, error2);
                return;
            }
            if (isNew1)
            {
                _dayIncome.Add(_selectedDayIncome);
            }
            else
            {
                //_daySell[_selectedIndex] = _selectedDayIncome;
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

            for (int i = 0; i < _dayCosts.Count; i++)
            {
                string error;
                if (_dayCostBl.Delete(_dayCosts[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

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

            
            DayIncomeManager di = new DayIncomeManager();
            di.Show();
            this.Hide();
        }
    }
}
