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
    public partial class FormPrint : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context = new DecentDbEntities();
        TemporarySellBL _temporarySellBl = new TemporarySellBL();
        List<TemporarySell> _temporarySells = new List<TemporarySell>();
        private TemporarySell _selectedTemporarySell = null;
        private int _selectedIndex = 0;

        CashMemo cashMemo;
        


        Memo rpt;

        public FormPrint(List<TemporarySell> tempo, CashMemo cash)
        {
            _temporarySells = tempo;
            cashMemo = cash;
            InitializeComponent();
        }

        private void FormPrint_Load(object sender, EventArgs e)
        {
            btnFinish.Focus();
            _temporarySells = _temporarySellBl.GetAll("");

            if (_temporarySells.Count > 0)
            {
                rpt = new Memo();
                rpt.SetDataSource(_temporarySells);
                rpt.SetParameterValue("pCustomerName", cashMemo.CustomerName);
                rpt.SetParameterValue("pCustomerPhone", cashMemo.CustomerPhone);
                rpt.SetParameterValue("pSalesmanName", cashMemo.Salesman);
                rpt.SetParameterValue("pPhone", cashMemo.Phone);
                rpt.SetParameterValue("pMemoNumber", cashMemo.ID.ToString());
                rpt.SetParameterValue("pDate", cashMemo.Date.ToString("MM/dd/yyyy"));
                crystalReportViewer.ReportSource = rpt;
                crystalReportViewer.Refresh();
                crystalReportViewer.ShowRefreshButton = false;
                

                for (int i = 0; i < _temporarySells.Count; i++)
                {
                    string error1;
                    if (_temporarySellBl.Delete(_temporarySells[i].ID, out error1) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error1);
                        return;
                    }

                }


            }

        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            OtherUserSellProductManager op=new OtherUserSellProductManager();
            op.Show();
            this.Hide();
        }

        

    }
}
