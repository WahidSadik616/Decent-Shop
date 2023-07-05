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
using System.Globalization;

namespace Decent.IMS.GUI
{
    public partial class OtherUserHomeForm : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context = new DecentDbEntities();
        SellerNewProductBL _sellerNewProductBl = new SellerNewProductBL();
        List<SellerNewProduct> _sellerNewProducts = new List<SellerNewProduct>();
        List<SellerNewProduct> s = new List<SellerNewProduct>();
        private SellerNewProduct _selectedSellerNewProduct = null;
        private int _selectedIndex = 0;


        SellDetailsBL _sellDetailsBl = new SellDetailsBL();
        List<SellDetail> _sellDetailss = new List<SellDetail>();
        private SellDetail _selectedSellDetails = null;
        private int _selectedIndex1 = 0;


        CostDetailsBL _costDetailBl = new CostDetailsBL();
        List<CostDetail> _costDetails = new List<CostDetail>();
        private CostDetail _selectedCostDetail = null;
        private int _selectedIndex2 = 0;

        SupplierPaymentDetailsBL _paymentDetailsBl = new SupplierPaymentDetailsBL();
        List<PaymentDetail> _paymentDetails = new List<PaymentDetail>();
        private PaymentDetail _selectedPaymentDetails = null;
        private int _selectedIndex3 = 0;

        public OtherUserHomeForm()
        {
            InitializeComponent();
        }

        private void OtherUserHomeForm_Load(object sender, EventArgs e)
        {
            
            this.LoadOtherUserHomeForm();
        }

        private void LoadOtherUserHomeForm()
        {

            btnPurchase.Select();



            _sellerNewProducts = _sellerNewProductBl.GetAllDateRange(dtpStart.Text, dtpEnd.Text);

            double totalPurchase = 0;

            for(int i=0;i<_sellerNewProducts.Count;i++)
            {
                totalPurchase += _sellerNewProducts[i].AfterTotalPrice;
            }
            string a=totalPurchase.ToString();
            decimal b = decimal.Parse(a, CultureInfo.InvariantCulture);
            CultureInfo bangla = new CultureInfo("bn-BD");
            string c = string.Format(bangla, "{0:c}", b);
            lblPurchaseAmount.Text = c;










            _sellDetailss = _sellDetailsBl.GetAllDateRange(dtpStart.Text, dtpEnd.Text);

            
            double totalSell = 0;

            for (int i = 0; i < _sellDetailss.Count; i++)
            {
                totalSell += _sellDetailss[i].TotalPrice;
            }
            string a1 = totalSell.ToString();
            decimal b1 = decimal.Parse(a1, CultureInfo.InvariantCulture);
            CultureInfo bangla1 = new CultureInfo("bn-BD");
            string c1 = string.Format(bangla1, "{0:c}", b1);
            lblSellAmount.Text = c1;

            








            _costDetails = _costDetailBl.GetAllDateRange(dtpStart.Text, dtpEnd.Text);

            double totalCost = 0;

            for (int i = 0; i < _costDetails.Count; i++)
            {
                totalCost += _costDetails[i].Amount;
            }
            string a2 = totalCost.ToString();
            decimal b2 = decimal.Parse(a2, CultureInfo.InvariantCulture);
            CultureInfo bangla2 = new CultureInfo("bn-BD");
            string c2 = string.Format(bangla2, "{0:c}", b2);
            lblCostAmount.Text = c2;




           




            _paymentDetails = _paymentDetailsBl.GetAllDateRange(dtpStart.Text, dtpEnd.Text);

            double totalPayment = 0;

            for (int i = 0; i < _paymentDetails.Count; i++)
            {
                totalPayment += _paymentDetails[i].Payment;
            }
            string a3 = totalPayment.ToString();
            decimal b3 = decimal.Parse(a3, CultureInfo.InvariantCulture);
            CultureInfo bangla3 = new CultureInfo("bn-BD");
            string c3 = string.Format(bangla3, "{0:c}", b3);
            lblPaymentAmount.Text = c3;








            decimal totalSell1=Convert.ToDecimal(totalSell);
            decimal totalCost1 = Convert.ToDecimal(totalCost);
            decimal totalPayment1 = Convert.ToDecimal(totalPayment);

            decimal TotalCash1 =totalSell1 - (totalCost1 + totalPayment1);

            string a4 = TotalCash1.ToString();
            decimal b4 = decimal.Parse(a4, CultureInfo.InvariantCulture);
            CultureInfo bangla4 = new CultureInfo("bn-BD");
            string c4 = string.Format(bangla4, "{0:c}", b4);
            lblCash.Text = c4;




            btnOk.PerformClick();
        }

        private void RefreshPurchaseDgv()
        {
            dgvPurchaseList.AutoGenerateColumns = false;
            dgvPurchaseList.DataSource = _sellerNewProducts.ToList();
            dgvPurchaseList.Refresh();

            dgvPurchaseList.ClearSelection();

            for (int i = 0; i < dgvPurchaseList.Rows.Count; i++)
            {
                if (dgvPurchaseList.Rows[i].Cells[1].Value.ToString() == _selectedSellerNewProduct.Date.ToString())
                {
                    dgvPurchaseList.Rows[i].Selected = true;
                    dgvPurchaseList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvPurchaseList.Rows.Count; i++)
            {
                dgvPurchaseList.Rows[i].Cells[0].Value = i+1;
                dgvPurchaseList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }


        private void RefreshSellDgv()
        {
            dgvSellList.AutoGenerateColumns = false;
            dgvSellList.DataSource = _sellDetailss.ToList();
            dgvSellList.Refresh();

            dgvSellList.ClearSelection();

            for (int i = 0; i < dgvSellList.Rows.Count; i++)
            {
                if (dgvSellList.Rows[i].Cells[1].Value.ToString() == _selectedSellDetails.Date.ToString())
                {
                    dgvSellList.Rows[i].Selected = true;
                    dgvSellList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvSellList.Rows.Count; i++)
            {
                dgvSellList.Rows[i].Cells[0].Value = i + 1;
                dgvSellList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            
            }
        }




        private void RefreshCostDgv()
        {
            dgvCostList.AutoGenerateColumns = false;
            dgvCostList.DataSource = _costDetails.ToList();
            dgvCostList.Refresh();

            dgvCostList.ClearSelection();

            for (int i = 0; i < dgvCostList.Rows.Count; i++)
            {
                if (dgvCostList.Rows[i].Cells[1].Value.ToString() == _selectedCostDetail.Date.ToString())
                {
                    dgvCostList.Rows[i].Selected = true;
                    dgvCostList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvCostList.Rows.Count; i++)
            {
                dgvCostList.Rows[i].Cells[0].Value = i + 1;
                dgvCostList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }


        private void RefreshPaymentDgv()
        {
            dgvPaymentList.AutoGenerateColumns = false;
            dgvPaymentList.DataSource = _paymentDetails.ToList();
            dgvPaymentList.Refresh();

            dgvPaymentList.ClearSelection();

            for (int i = 0; i < dgvPaymentList.Rows.Count; i++)
            {
                if (dgvPaymentList.Rows[i].Cells[1].Value.ToString() == _selectedPaymentDetails.Date.ToString())
                {
                    dgvPaymentList.Rows[i].Selected = true;
                    dgvPaymentList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvPaymentList.Rows.Count; i++)
            {
                dgvPaymentList.Rows[i].Cells[0].Value = i + 1;
                dgvPaymentList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LoginForm a=new LoginForm();
            a.Show();
            this.Hide();
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            OtherUserAddProductManager a=new OtherUserAddProductManager();
            a.Show();
            this.Hide();
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            OtherUserSellProductManager a=new OtherUserSellProductManager();
            a.Show();
            this.Hide();
        }

        private void btnCost_Click(object sender, EventArgs e)
        {
            OtherUserDayCostManager a=new OtherUserDayCostManager();
            a.Show();
            this.Hide();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            OtherUserSupplierPaymentForm a=new OtherUserSupplierPaymentForm();
            a.Show();
            this.Hide();
        }

        private void btnCustomerDue_Click(object sender, EventArgs e)
        {
            
        }

       
        private void dtpStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnAll.PerformClick();
            }
            if (e.KeyCode == Keys.Left)
            {
                btnLogOut.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                dtpEnd.Focus();
            }
        }

        private void dtpEnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnAll.PerformClick();
            }
            if (e.KeyCode == Keys.Left)
            {
                dtpStart.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btnOk.Select();
                btnOk.PerformClick();
            }
        }

        private void metroPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now;

            _sellerNewProducts = _sellerNewProductBl.GetAll("");

            if (_sellerNewProducts.Count > 0)
            {
                _selectedSellerNewProduct = _sellerNewProducts[0];
                _selectedIndex = 0;

            }

            this.RefreshPurchaseDgv();


            double totalPurchase = 0;

            for (int i = 0; i < _sellerNewProducts.Count; i++)
            {
                totalPurchase += _sellerNewProducts[i].AfterTotalPrice;
            }
            string a = totalPurchase.ToString();
            decimal b = decimal.Parse(a, CultureInfo.InvariantCulture);
            CultureInfo bangla = new CultureInfo("bn-BD");
            string c = string.Format(bangla, "{0:c}", b);
            lblPurchaseAmount.Text = c;








            _sellDetailss = _sellDetailsBl.GetAll("");

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex1 = 0;

            }

            this.RefreshSellDgv();

            double totalSell = 0;

            for (int i = 0; i < _sellDetailss.Count; i++)
            {
                totalSell += _sellDetailss[i].TotalPrice;
            }
            string a1 = totalSell.ToString();
            decimal b1 = decimal.Parse(a1, CultureInfo.InvariantCulture);
            CultureInfo bangla1 = new CultureInfo("bn-BD");
            string c1 = string.Format(bangla1, "{0:c}", b1);
            lblSellAmount.Text = c1;









            _costDetails = _costDetailBl.GetAll("");

            if (_costDetails.Count > 0)
            {
                _selectedCostDetail = _costDetails[0];
                _selectedIndex2 = 0;

            }

            this.RefreshCostDgv();

            double totalCost = 0;

            for (int i = 0; i < _costDetails.Count; i++)
            {
                totalCost += _costDetails[i].Amount;
            }
            string a2 = totalCost.ToString();
            decimal b2 = decimal.Parse(a2, CultureInfo.InvariantCulture);
            CultureInfo bangla2 = new CultureInfo("bn-BD");
            string c2 = string.Format(bangla2, "{0:c}", b2);
            lblCostAmount.Text = c2;







            _paymentDetails = _paymentDetailsBl.GetAll("");

            if (_paymentDetails.Count > 0)
            {
                _selectedPaymentDetails = _paymentDetails[0];
                _selectedIndex3 = 0;

            }

            this.RefreshPaymentDgv();

            double totalPayment = 0;

            for (int i = 0; i < _paymentDetails.Count; i++)
            {
                totalPayment += _paymentDetails[i].Payment;
            }
            string a3 = totalPayment.ToString();
            decimal b3 = decimal.Parse(a3, CultureInfo.InvariantCulture);
            CultureInfo bangla3 = new CultureInfo("bn-BD");
            string c3 = string.Format(bangla3, "{0:c}", b3);
            lblPaymentAmount.Text = c3;








            decimal totalSell1 = Convert.ToDecimal(totalSell);
            decimal totalCost1 = Convert.ToDecimal(totalCost);
            decimal totalPayment1 = Convert.ToDecimal(totalPayment);

            decimal TotalCash1 = totalSell1 - (totalCost1 + totalPayment1);

            string a4 = TotalCash1.ToString();
            decimal b4 = decimal.Parse(a4, CultureInfo.InvariantCulture);
            CultureInfo bangla4 = new CultureInfo("bn-BD");
            string c4 = string.Format(bangla4, "{0:c}", b4);
            lblCash.Text = c4;


        }


        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            dtpEnd.Text = dtpStart.Text;
            btnOk.PerformClick();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            OtherUserHomeForm a = new OtherUserHomeForm();
            a.Show();
            this.Hide();
        }

        private void btnPurchaseDetails_Click(object sender, EventArgs e)
        {
            OtherUserSellerNewProductManager a = new OtherUserSellerNewProductManager(_sellerNewProducts);
            a.ShowDialog();
            //this.Hide();
        }

        private void btnOk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                btnRefresh.Select();
            }
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            _sellerNewProducts = _sellerNewProductBl.GetAllDateRange(dtpStart.Text,dtpEnd.Text);

            if (_sellerNewProducts.Count > 0)
            {
                _selectedSellerNewProduct = _sellerNewProducts[0];
                _selectedIndex = 0;

            }

            this.RefreshPurchaseDgv();


            double totalPurchase = 0;

            for (int i = 0; i < _sellerNewProducts.Count; i++)
            {
                totalPurchase += _sellerNewProducts[i].AfterTotalPrice;
            }
            string a = totalPurchase.ToString();
            decimal b = decimal.Parse(a, CultureInfo.InvariantCulture);
            CultureInfo bangla = new CultureInfo("bn-BD");
            string c = string.Format(bangla, "{0:c}", b);
            lblPurchaseAmount.Text = c;












            _sellDetailss = _sellDetailsBl.GetAllDateRange(dtpStart.Text, dtpEnd.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex1 = 0;

            }

            this.RefreshSellDgv();

            double totalSell = 0;

            for (int i = 0; i < _sellDetailss.Count; i++)
            {
                totalSell += _sellDetailss[i].TotalPrice;
            }
            string a1 = totalSell.ToString();
            decimal b1 = decimal.Parse(a1, CultureInfo.InvariantCulture);
            CultureInfo bangla1 = new CultureInfo("bn-BD");
            string c1 = string.Format(bangla1, "{0:c}", b1);
            lblSellAmount.Text = c1;













            _costDetails = _costDetailBl.GetAllDateRange(dtpStart.Text, dtpEnd.Text);

            if (_costDetails.Count > 0)
            {
                _selectedCostDetail = _costDetails[0];
                _selectedIndex2 = 0;

            }

            this.RefreshCostDgv();

            double totalCost = 0;

            for (int i = 0; i < _costDetails.Count; i++)
            {
                totalCost += _costDetails[i].Amount;
            }
            string a2 = totalCost.ToString();
            decimal b2 = decimal.Parse(a2, CultureInfo.InvariantCulture);
            CultureInfo bangla2 = new CultureInfo("bn-BD");
            string c2 = string.Format(bangla2, "{0:c}", b2);
            lblCostAmount.Text = c2;











            _paymentDetails = _paymentDetailsBl.GetAllDateRange(dtpStart.Text, dtpEnd.Text);

            if (_paymentDetails.Count > 0)
            {
                _selectedPaymentDetails = _paymentDetails[0];
                _selectedIndex3 = 0;

            }

            this.RefreshPaymentDgv();

            double totalPayment = 0;

            for (int i = 0; i < _paymentDetails.Count; i++)
            {
                totalPayment += _paymentDetails[i].Payment;
            }
            string a3 = totalPayment.ToString();
            decimal b3 = decimal.Parse(a3, CultureInfo.InvariantCulture);
            CultureInfo bangla3 = new CultureInfo("bn-BD");
            string c3 = string.Format(bangla3, "{0:c}", b3);
            lblPaymentAmount.Text = c3;









            decimal totalSell1 = Convert.ToDecimal(totalSell);
            decimal totalCost1 = Convert.ToDecimal(totalCost);
            decimal totalPayment1 = Convert.ToDecimal(totalPayment);

            decimal TotalCash1 = totalSell1 - (totalCost1 + totalPayment1);

            string a4 = TotalCash1.ToString();
            decimal b4 = decimal.Parse(a4, CultureInfo.InvariantCulture);
            CultureInfo bangla4 = new CultureInfo("bn-BD");
            string c4 = string.Format(bangla4, "{0:c}", b4);
            lblCash.Text = c4;

        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            btnOk.PerformClick();
        }

        private void dtpStart_DropDown(object sender, EventArgs e)
        {
            dtpEnd.Text = dtpStart.Text;
        }

        private void btnCustomerReturn_Click(object sender, EventArgs e)
        {
            OtherUserReturnProductManager a = new OtherUserReturnProductManager();
            a.Show();
            this.Hide();
        }

        private void btnSupplierReturn_Click(object sender, EventArgs e)
        {

        }

        private void btnSellDetails_Click(object sender, EventArgs e)
        {
            OtherUserSellDetailsManager a = new OtherUserSellDetailsManager(_sellDetailss);
            a.ShowDialog();
            //this.Hide();
        }

        private void btnCostDetails_Click(object sender, EventArgs e)
        {
            OtherUserCostManager a = new OtherUserCostManager(_costDetails);
            a.ShowDialog();
        }

        private void metroPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPaymentDetails_Click(object sender, EventArgs e)
        {
            OtherUserSupplierPaymentDetailsManager a = new OtherUserSupplierPaymentDetailsManager(_paymentDetails);
            a.ShowDialog();
        }
    }
}
