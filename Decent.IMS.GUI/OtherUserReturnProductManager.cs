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
using MetroFramework;

namespace Decent.IMS.GUI
{
    public partial class OtherUserReturnProductManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();

        SellDetailsBL _sellDetailsBl=new SellDetailsBL();
        List<SellDetail> _sellDetailss= new List<SellDetail>();
        private SellDetail _selectedSellDetails = null;
        private int _selectedIndex = 0;


        ProductBL _productBl = new ProductBL();
        SalesmanBL _salesmanBl = new SalesmanBL();
        DaySellBL _daySellBl = new DaySellBL();
        List<Product> _products = new List<Product>();
        List<Salesman> _salesman = new List<Salesman>();
        List<DaySell> _daySell = new List<DaySell>();
        private Product _selectedProduct = null;
        private DaySell _selectedDaySell = null;
        private int _selectedIndex2 = 0;

        CustomerBL _customerBl = new CustomerBL();
        List<Customer> _customers = new List<Customer>();
        private Customer _selectedCustomer = null;
        private int _selectedIndex4 = 0;


        CustomerDaySellBL _customerDaySellBl = new CustomerDaySellBL();
        List<CustomerDaySell> _customerDaySells = new List<CustomerDaySell>();
        private CustomerDaySell _selectedCustomerDaySell = null;

        
        public OtherUserReturnProductManager()
        {
            InitializeComponent();
        }

        private void SellDetailsManager_Load(object sender, EventArgs e)
        {
            this.Init();
            
        }

        private void Init()
        {
            try
            {
                
                lblPhone.Hide();
                txtPhoneSearch.Hide();
                btnPhoneOk.Hide();
                lblProName.Hide();
                txtProName.Hide();
                btnProNameOk.Hide();
                lblCategory.Hide();
                ddlCategory.Hide();
                btnCatOk.Hide();
                txtSearch1.Focus();
                lblPrice.Hide();
                txtPriceRate.Hide();
                btnPriceRateOk.Hide();
                lblDate.Hide();
                dtpDateTime.Hide();
                btnDateOk.Hide();
                lblID.Hide();
                txtID.Hide();
                btnIdOk.Hide();
                lblAmount.Hide();
                txtAmount.Hide();
                btnAmountOk.Hide();
                lblID2.Hide();
                txtId2.Hide();
                btnIdOk2.Hide();
                lblAmount2.Hide();
                txtAmount2.Hide();
                btnAmountOk2.Hide();
                lblCategory2.Hide();
                ddlCategory2.Hide();
                btnOkCat2.Hide();

                txtSearch1.Text = "";
                txtPhoneSearch.Text = "";
                txtProName.Text = "";
                //txtProCat.Text = "";
                txtPriceRate.Text = "";
                txtID.Text = "";
                txtAmount.Text = "";
                txtId2.Text = "";
                txtAmount2.Text = "";

                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                ddlCategory2.DataSource = _context.ProductCategories.ToList();
                ddlCategory2.DisplayMember = "Name";
                ddlCategory2.ValueMember = "ID";
                
                this.LoadOtherUserReturnProductManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadOtherUserReturnProductManagers()
        {
            _sellDetailss = _sellDetailsBl.GetAll("");

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedSellDetails = new SellDetail()
                {
                    Date = DateTime.Now
                };

            }

           
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvSellDetailsList.AutoGenerateColumns = false;
           dgvSellDetailsList.DataSource = _sellDetailss.ToList();
            dgvSellDetailsList.Refresh();

            dgvSellDetailsList.ClearSelection();

            for (int i = 0; i < dgvSellDetailsList.Rows.Count; i++)
            {
                if (dgvSellDetailsList.Rows[i].Cells[0].Value.ToString() == _selectedSellDetails.ID.ToString())
                {
                    dgvSellDetailsList.Rows[i].Selected = true;
                    dgvSellDetailsList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvSellDetailsList.Rows.Count; i++)
            {
                dgvSellDetailsList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedSellDetails = _sellDetailss[e.RowIndex];
                _selectedIndex = e.RowIndex;
               
            }
        }

        
        private void metroPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            OtherUserHomeForm of = new OtherUserHomeForm();
            of.Show();
            this.Hide();
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            this.Init();
            txtSearch1.Focus();
        }

        
        private void metroButton3_Click(object sender, EventArgs e)
        {
            _sellDetailss = _sellDetailsBl.GetAllPhone(txtPhoneSearch.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex = 0;
                lblCategory.Show();
                ddlCategory.Show();
                ddlCategory.Focus();
                ddlCategory.DroppedDown = true;
                btnCatOk.Show();


            }
            else
            {
                _selectedSellDetails = new SellDetail()
                {
                    Date = DateTime.Now
                };
                
            }

            this.RefreshDgv();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            _sellDetailss = _sellDetailsBl.GetAllPhoneCatProName(txtPhoneSearch.Text,ddlCategory.Text,txtProName.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex = 0;


                lblPrice.Show();
                txtPriceRate.Show();
                txtPriceRate.Focus();
                btnPriceRateOk.Show();
                
            }
            else
            {
                _selectedSellDetails = new SellDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.RefreshDgv();
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            _sellDetailss = _sellDetailsBl.GetAllPhoneCat(txtPhoneSearch.Text,ddlCategory.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex = 0;

                lblProName.Show();
                txtProName.Show();
                txtProName.Focus();
                btnProNameOk.Show();

            }
            else
            {
                _selectedSellDetails = new SellDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.RefreshDgv();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            _sellDetailss = _sellDetailsBl.GetAllOrder(txtSearch1.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];

                lblCategory2.Show();
                ddlCategory2.Show();
                ddlCategory2.Focus();
                btnOkCat2.Show();

                _selectedIndex = 0;
                
            }
            else
            {
                _selectedSellDetails = new SellDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.RefreshDgv();
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            LoginForm lf=new LoginForm();
            lf.Show();
            this.Hide();
        }

        private void txtSearch1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Right)
            {
                btnSearch.Focus();
            }
            if (e.KeyCode == Keys.Left)
            {
                btnRefresh.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSearch1.Text))
            {
                btnSearch.PerformClick();
            }
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            lblPhone.Show();
            txtPhoneSearch.Show();
            txtPhoneSearch.Focus();
            btnPhoneOk.Show();
        }

        private void txtPhoneSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtPhoneSearch.Text))
            {
                btnPhoneOk.PerformClick();
            }
        }

        private void txtPhoneSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtProName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                ddlCategory.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtProName.Text))
            {
                btnProNameOk.PerformClick();
                
            }
        }


        private void txtPriceRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtProName.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtPriceRate.Text))
            {
                btnPriceRateOk.PerformClick();
                
            }
        }

        private void btnPriceRateOk_Click(object sender, EventArgs e)
        {
            _sellDetailss = _sellDetailsBl.GetAllPhoneCatProNamePriceRate(txtPhoneSearch.Text,ddlCategory.Text,txtProName.Text, txtPriceRate.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex = 0;
                lblDate.Show();
                dtpDateTime.Show();
                dtpDateTime.Focus();
                btnDateOk.Show();
            }
            else
            {
                _selectedSellDetails = new SellDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.RefreshDgv();
        }

        private void btnDateOk_Click(object sender, EventArgs e)
        {
            _sellDetailss = _sellDetailsBl.GetAllPhoneCatProNamePriceRateDate(txtPhoneSearch.Text, ddlCategory.Text, txtProName.Text, txtPriceRate.Text,dtpDateTime.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex = 0;
                lblID2.Show();
                txtId2.Show();
                txtId2.Focus();
                btnIdOk2.Show();

            }
            

            this.RefreshDgv();
        }

        private void txtSearch1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void ddlCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnCatOk.PerformClick();
                
            }
            if (e.KeyCode == Keys.Down)
            {
                ddlCategory.DroppedDown = true;

            }
        }

        private void txtPriceRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }


            if (e.KeyCode == Keys.Up)
            {
                ddlCategory2.Focus();
            }
            
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtID.Text))
            {
                btnIdOk.PerformClick();

            }
        }

        private void btnIdOk_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtID.Text);
            int cashMemoId = Convert.ToInt32(txtSearch1.Text);

            _sellDetailss = _sellDetailsBl.GetAllOrder(txtSearch1.Text);

            for (int i = 0; i<_sellDetailss.Count; i++)
            {
                if(_sellDetailss[i].CashMemoId==cashMemoId && _sellDetailss[i].ID==id)
                {
                    
                    _selectedSellDetails = _sellDetailss[i];
                        lblAmount.Show();
                        txtAmount.Show();
                        txtAmount.Focus();
                        btnAmountOk.Show();
                        break;
                   
                }
            }
            
            
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }

            if (e.KeyCode == Keys.Up)
            {
                txtID.Focus();
            }

            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                btnAmountOk.PerformClick();

            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtId2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }

            if (e.KeyCode == Keys.Up)
            {
                dtpDateTime.Focus();
            }

            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtId2.Text))
            {
                btnIdOk2.PerformClick();

            }
        }

        private void txtId2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void btnIdOk2_Click(object sender, EventArgs e)
        {
            try
            {
                _sellDetailss = _sellDetailsBl.GetAllPhoneCatProNamePriceRateDate(txtPhoneSearch.Text, ddlCategory.Text, txtProName.Text, txtPriceRate.Text, dtpDateTime.Text);

                int id = Convert.ToInt32(txtId2.Text);
               var product = _sellDetailss.FirstOrDefault(u => u.ID == id);

                if (product != null)
                {
                    lblAmount2.Show();
                    txtAmount2.Show();
                    txtAmount2.Focus();
                    btnAmountOk2.Show();
                }
                
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                txtId2.Focus();
                return;
            }
            
        }

        private void btnAmountOk2_Click(object sender, EventArgs e)
        {
            _sellDetailss = _sellDetailsBl.GetAllPhoneCatProNamePriceRateDate(txtPhoneSearch.Text, ddlCategory.Text, txtProName.Text, txtPriceRate.Text, dtpDateTime.Text);

            int id = Convert.ToInt32(txtId2.Text);
            int amount = Convert.ToInt32(txtAmount2.Text);

            var product = _sellDetailss.FirstOrDefault(u => u.ID == id);

            if (product != null)
            {

                double perBenifit = product.Benifit / product.Amount;

                double totalPrice = product.SellPriceRate * amount;




                if (product.Amount < amount || amount == 0)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invaid Amount");
                    txtAmount2.Focus();
                    return;
                }






                if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
                {
                    OtherUserReturnProductManager o = new OtherUserReturnProductManager();
                    o.Show();
                    this.Hide();
                    return;
                }





                var productManager = _context.Products.FirstOrDefault(u => u.Name == product.ProductName &&
                                                                           u.ProductCategoryId == product.ProductCategoryId &&
                                                                           u.PriceRate == product.RealPriceRate);
                if (productManager != null)
                {
                    productManager.Availability += amount;
                    productManager.Benifit -= (perBenifit * amount);

                    string error;
                    _productBl.Save(productManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                }






                var salesmanManager = _context.Salesmen.FirstOrDefault(u => u.Name == product.Salesman);

                if (salesmanManager != null)
                {
                    salesmanManager.Benifit -= (perBenifit * amount);

                    string error;
                    _salesmanBl.Save(salesmanManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                }






                var customerManager =
                    _context.Customers.FirstOrDefault(u => u.Name == product.Customer &&
                                                           u.Phone == product.CustomerPhone);

                if (customerManager != null)
                {
                    customerManager.TotalPrice -= totalPrice;
                    customerManager.Payment -= totalPrice;
                    customerManager.Benifit -= (perBenifit * amount);

                    string error;
                    _customerBl.Save(customerManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                }






                _daySell = _daySellBl.GetAll("");

                for (int i = 0; i < _daySell.Count; i++)
                {
                    if (_daySell[i].Name == product.ProductName &&
                        _daySell[i].ProductCategory == product.Category &&
                        _daySell[i].Time == product.Time && _daySell[i].Amount != 0)
                    {
                        double perSell = _daySell[i].Sell / _daySell[i].Amount;
                        double perBeni = _daySell[i].Benifit / _daySell[i].Amount;

                        if (perSell == product.SellPriceRate && perBeni == perBenifit)
                        {
                            _daySell[i].Sell -= totalPrice;
                            _daySell[i].Amount -= amount;
                            _daySell[i].Benifit -= perBenifit * amount;

                            string error;
                            _daySellBl.Save(_daySell[i], out error);

                            if (string.IsNullOrEmpty(error) == false)
                            {
                                MetroFramework.MetroMessageBox.Show(this, error);
                                return;
                            }
                            break;
                        }
                    }
                }

                for (int i = 0; i < _daySell.Count; i++)
                {
                    if (_daySell[i].Amount == 0)
                    {
                        string error;
                        if (_daySellBl.Delete(_daySell[i].ID, out error) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error);
                            return;
                        }
                    }
                }









                var custoDaySellManager = _context.CustomerDaySells.FirstOrDefault(u => u.Date == product.Date &&
                                                                                         u.Time == product.Time);

                if (custoDaySellManager != null)
                {
                    custoDaySellManager.TotalPrice -= product.SellPriceRate * amount;
                    custoDaySellManager.Payment -= product.SellPriceRate * amount;
                    custoDaySellManager.Benifit -= perBenifit * amount;

                    string error;
                    _customerDaySellBl.Save(custoDaySellManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                }
                _customerDaySells = _customerDaySellBl.GetAll("");
                for (int i = 0; i < _customerDaySells.Count; i++)
                {
                    if (_customerDaySells[i].Benifit == 0)
                    {
                        string error;
                        if (_customerDaySellBl.Delete(_customerDaySells[i].ID, out error) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error);
                            return;
                        }
                    }
                }







                var sellDetailsManager = _context.SellDetails.FirstOrDefault(u => u.ID == product.ID);
                if (sellDetailsManager != null)
                {
                    sellDetailsManager.Amount -= amount;
                    sellDetailsManager.TotalPrice -= product.SellPriceRate * amount;
                    sellDetailsManager.Benifit -= perBenifit * amount;

                    string error;
                    _sellDetailsBl.Save(sellDetailsManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                }
                _sellDetailss = _sellDetailsBl.GetAll("");
                for (int i = 0; i < _sellDetailss.Count; i++)
                {
                    if (_sellDetailss[i].Amount == 0)
                    {
                        string error;
                        if (_sellDetailsBl.Delete(_sellDetailss[i].ID, out error) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error);
                            return;
                        }
                    }
                }




                MetroFramework.MetroMessageBox.Show(this, "Operation completed..!!");




                if (
                    MetroFramework.MetroMessageBox.Show(this, "Do you want to buy another product...??", "Confirmation",
                        MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    OtherUserSellProductManager op = new OtherUserSellProductManager();
                    op.Show();
                    this.Hide();
                    return;
                }

                OtherUserReturnProductManager os = new OtherUserReturnProductManager();
                os.Show();
                this.Hide();
            }
        }

        private void btnAmountOk_Click(object sender, EventArgs e)
        {
            
            int id = Convert.ToInt32(txtID.Text);
            int cashMemoId = Convert.ToInt32(txtSearch1.Text);
            int amount = Convert.ToInt32(txtAmount.Text);

            
            var product = _context.SellDetails.FirstOrDefault(u => u.ID == id && u.CashMemoId == cashMemoId);
            if (product != null)
            {


                double perBenifit=product.Benifit/product.Amount;

                double totalPrice=product.SellPriceRate* amount;




                if (product.Amount < amount || amount == 0)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invaid Amount");
                    txtAmount.Focus();
                    return;
                }






                if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
                {
                    OtherUserReturnProductManager of = new OtherUserReturnProductManager();
                    of.Show();
                    this.Hide();
                    return;
                }





                var productManager = _context.Products.FirstOrDefault(u => u.Name == product.ProductName &&
                                                                           u.ProductCategoryId == product.ProductCategoryId &&
                                                                           u.PriceRate == product.RealPriceRate);
                if (productManager != null)
                {
                    productManager.Availability += amount;
                    productManager.Benifit -= (perBenifit*amount);

                    string error;
                    _productBl.Save(productManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                }








                var salesmanManager = _context.Salesmen.FirstOrDefault(u => u.Name == product.Salesman);

                if (salesmanManager != null)
                {
                    salesmanManager.Benifit -= (perBenifit * amount);

                    string error;
                    _salesmanBl.Save(salesmanManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                }








                var customerManager =
                    _context.Customers.FirstOrDefault(u => u.Name == product.Customer && 
                                                           u.Phone == product.CustomerPhone);

                if (customerManager != null)
                {
                    customerManager.TotalPrice -= totalPrice;
                    customerManager.Payment -= totalPrice;
                    customerManager.Benifit -= (perBenifit * amount);

                    string error;
                    _customerBl.Save(customerManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                }







                _daySell = _daySellBl.GetAll("");

                for (int i = 0; i < _daySell.Count; i++)
                {
                    if (_daySell[i].Name == product.ProductName &&
                        _daySell[i].ProductCategory == product.Category &&
                        _daySell[i].Time == product.Time && _daySell[i].Amount !=0)
                    {
                        double perSell=_daySell[i].Sell/_daySell[i].Amount;
                        double perBeni = _daySell[i].Benifit / _daySell[i].Amount;

                        if (perSell == product.SellPriceRate && perBeni==perBenifit)
                        {
                            _daySell[i].Sell -= totalPrice;
                            _daySell[i].Amount -= amount;
                            _daySell[i].Benifit -= perBenifit * amount;

                            string error;
                            _daySellBl.Save(_daySell[i], out error);

                            if (string.IsNullOrEmpty(error) == false)
                            {
                                MetroFramework.MetroMessageBox.Show(this, error);
                                return;
                            }
                            break;
                        }
                    }
                }

                for (int i = 0; i < _daySell.Count; i++)
                {
                    if (_daySell[i].Amount == 0)
                    {
                        string error;
                        if (_daySellBl.Delete(_daySell[i].ID, out error) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error);
                            return;
                        }
                    }
                }
               







                var custoDaySellManager = _context.CustomerDaySells.FirstOrDefault(u => u.Date == product.Date &&
                                                                                        u.Time == product.Time);

                if (custoDaySellManager != null)
                {
                    custoDaySellManager.TotalPrice -= product.SellPriceRate*amount;
                    custoDaySellManager.Payment -= product.SellPriceRate * amount;
                    custoDaySellManager.Benifit -= perBenifit*amount;

                    string error;
                    _customerDaySellBl.Save(custoDaySellManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                }
                _customerDaySells = _customerDaySellBl.GetAll("");
                for (int i = 0; i < _customerDaySells.Count; i++)
                {
                    if (_customerDaySells[i].Benifit == 0)
                    {
                        string error;
                        if (_customerDaySellBl.Delete(_customerDaySells[i].ID, out error) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error);
                            return;
                        }
                    }
                }







                var sellDetailsManager = _context.SellDetails.FirstOrDefault(u => u.ID==product.ID);
                if (sellDetailsManager != null)
                {
                    sellDetailsManager.Amount -= amount;
                    sellDetailsManager.TotalPrice -= product.SellPriceRate*amount;
                    sellDetailsManager.Benifit -= perBenifit*amount;

                    string error;
                    _sellDetailsBl.Save(sellDetailsManager, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                }
                _sellDetailss = _sellDetailsBl.GetAll("");
                for (int i = 0; i < _sellDetailss.Count; i++)
                {
                    if (_sellDetailss[i].Amount == 0)
                    {
                        string error;
                        if (_sellDetailsBl.Delete(_sellDetailss[i].ID, out error) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error);
                            return;
                        }
                    }
                }





                MetroFramework.MetroMessageBox.Show(this, "Operation completed..!!");




                if (
                    MetroFramework.MetroMessageBox.Show(this, "Do you want to buy another product...??", "Confirmation",
                        MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    OtherUserSellProductManager op=new OtherUserSellProductManager();
                    op.Show();
                    this.Hide();
                    return;
                }

                OtherUserReturnProductManager os = new OtherUserReturnProductManager();
                os.Show();
                this.Hide();
            }
        }

        private void txtAmount2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }

            if (e.KeyCode == Keys.Up)
            {
                txtId2.Focus();
            }

            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtAmount2.Text))
            {
                btnAmountOk2.PerformClick();

            }
        }

        private void txtAmount2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void dtpDateTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btnDateOk.PerformClick();

            }
        }

       

        private void ddlCategory2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btnOkCat2.PerformClick();
            }
            if (e.KeyCode == Keys.Down)
            {
                ddlCategory2.DroppedDown = true;
            }
           
        }

        private void btnOkCat2_Click(object sender, EventArgs e)
        {
            _sellDetailss = _sellDetailsBl.GetAllOrderCategory(txtSearch1.Text, ddlCategory2.Text);

            if (_sellDetailss.Count > 0)
            {
                _selectedSellDetails = _sellDetailss[0];
                _selectedIndex = 0;

                lblID.Show();
                txtID.Show();
                txtID.Focus();
                btnIdOk.Show();

            }
            else
            {
                _selectedSellDetails = new SellDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.RefreshDgv();
  
        }


    }
}
