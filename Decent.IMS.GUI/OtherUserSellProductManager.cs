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
    public partial class OtherUserSellProductManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        ProductBL _productBl=new ProductBL();
        SalesmanBL _salesmanBl = new SalesmanBL();
        DaySellBL _daySellBl = new DaySellBL();
        List<Product> _products= new List<Product>();
        List<Salesman> _salesman = new List<Salesman>();
        List<DaySell> _daySell = new List<DaySell>();
        private Product _selectedProduct = null;
        private DaySell _selectedDaySell = null;
        private int _selectedIndex = 0;

        TemporarySellBL _temporarySellBl = new TemporarySellBL();
        List<TemporarySell> _temporarySells = new List<TemporarySell>();
        private TemporarySell _selectedTemporarySell = null;
        private int _selectedIndex1 = 0;

        CustomerBL _customerBl = new CustomerBL();
        List<Customer> _customers = new List<Customer>();
        private Customer _selectedCustomer = null;
        private int _selectedIndex2 = 0;


        SellDetailsBL _sellDetailsBl = new SellDetailsBL();
        List<SellDetail> _sellDetails = new List<SellDetail>();
        private SellDetail _selectedSellDetail = null;
        private int _selectedIndex3 = 0;


        CustomerDaySellBL _customerDaySellBl = new CustomerDaySellBL();
        List<CustomerDaySell> _customerDaySells = new List<CustomerDaySell>();
        private CustomerDaySell _selectedCustomerDaySell = null;

        CashMemoBL _cashMemoBl = new CashMemoBL();
        List<CashMemo> _cashMemos = new List<CashMemo>();
        private CashMemo _selectedCashMemo = null;
        private int _selectedIndex4 = 0;

        public OtherUserSellProductManager()
        {
            InitializeComponent();
        }

        private void OtherUserProductManager_Load(object sender, EventArgs e)
        {
            
            this.Init();
            btnRefresh.PerformClick();


        }

        private void Init()
        {
            try
            {
                lblPhone.Hide();
                txtPhone.Hide();
                btnOK.Hide();
                lblName.Hide();
                txtCustomerName.Hide();
                lblName1.Hide();
                txtNameCustomerName1.Hide();
                lblTotal.Hide();
                txtTotal.Hide();
                lblDue.Hide();
                txtDue.Hide();
                lblTakeMoney.Hide();
                txtTakeMoney.Hide();
                btnFinish.Hide();
                lblGiveMoney.Hide();
                txtGiveMoney.Hide();
                lblNetPayment.Hide();
                txtNetPayment.Hide();
                btnOK1.Hide();
                btnOK2.Hide();
                lblCancel.Hide();
                txtSerialNo.Hide();
                btnSerialOk.Hide();
                lblDiscount.Hide();
                txtDiscount.Hide();
                lblTime.Hide();

                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                ddlSalesman.DataSource = _context.Salesmen.ToList();
                ddlSalesman.DisplayMember = "Name";
                ddlSalesman.ValueMember = "ID";


                txtSearch.Text = "";
                txtAmount.Text = "";
                txtPriceRate.Text = "";

                this.LoadOtherUserProductManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadOtherUserProductManagers()
        {
            

            _products = _productBl.GetAll1(txtSearch.Text);

            _salesman = _salesmanBl.GetAll("");

            _temporarySells = _temporarySellBl.GetAll("");



            int id;
            if (_products.Count > 1)
            {
                _selectedProduct = _products[0];
                _selectedIndex = 0;
                txtSearch.Focus();
            }
            else if (_products.Count == 1 && int.TryParse(txtSearch.Text, out id))
            {
                _selectedProduct = _products[0];
                _selectedIndex = 0;
                txtAmount.Focus();
            }
            else if (_products.Count == 1 && !int.TryParse(txtSearch.Text, out id))
            {
                _selectedProduct = _products[0];
                _selectedIndex = 0;
                txtSearch.Focus();
            }
            
            else
            {
                _selectedProduct=new Product();

            }

          
            if (_temporarySells.Count > 0)
            {
                _selectedTemporarySell = _temporarySells[0];
                _selectedIndex1 = 0;

            }
            else
            {
                _selectedTemporarySell = new TemporarySell();

            }
            this.Populate();
            this.RefreshDgv();
        }

        private void RefreshDgv()
        {
            dgvOtherProductList.AutoGenerateColumns = false;
            dgvOtherProductList.DataSource = _products.ToList();
            dgvOtherProductList.Refresh();
           

            dgvOtherProductList.ClearSelection();

            for (int i = 0; i < dgvOtherProductList.Rows.Count; i++)
            {
               if (dgvOtherProductList.Rows[i].Cells[0].Value.ToString() == _selectedProduct.ID.ToString())
                {
                    dgvOtherProductList.Rows[i].Selected = true;
                    dgvOtherProductList.Refresh();
                    break;
                }
           }
            for (int i = 0; i < dgvOtherProductList.Rows.Count; i++)
            {
                dgvOtherProductList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                int a=Int32.Parse(dgvOtherProductList.Rows[i].Cells[3].Value.ToString());
                if (a<5)
                {
                    dgvOtherProductList.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                   
                }
                else if (a>=5 && a<10)
                {
                    dgvOtherProductList.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else
                {
                    dgvOtherProductList.Rows[i].DefaultCellStyle.BackColor = Color.Honeydew;
                }
           }


            dgvTemporarySellList.AutoGenerateColumns = false;
            dgvTemporarySellList.DataSource = _temporarySells.ToList();
            dgvTemporarySellList.Refresh();


            dgvTemporarySellList.ClearSelection();

            for (int i = 0; i < dgvTemporarySellList.Rows.Count; i++)
            {
                
            }

            for (int i = 0; i < dgvTemporarySellList.Rows.Count; i++)
            {
                dgvTemporarySellList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }

        }

       
           
       

       
        private void Populate()
        {
           
            txtName.Text = _selectedProduct.Name;
            
           
            ddlCategory.SelectedValue = _selectedProduct.ProductCategoryId;

            
        }

       
        private void metroButton6_Click(object sender, EventArgs e)
        {
            OtherUserHomeForm of = new OtherUserHomeForm();
            of.Show();
            this.Hide();
        }

        
        
        private bool isValid()
        {
            if (string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Amount please...!!!");
                txtAmount.Focus();
                return false;
            }

            int amount;
            if (!int.TryParse(txtAmount.Text, out amount) || Convert.ToInt32(txtAmount.Text) <= 0 || Convert.ToInt32(txtAmount.Text) > _selectedProduct.Availability)
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid amount...!!!");
                txtAmount.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPriceRate.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Price please...!!!");
                txtPriceRate.Focus();
                return false;
            }
            double priceRate;
            if (!double.TryParse(txtPriceRate.Text, out priceRate))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid price...!!!");
                txtPriceRate.Focus();
                return false;
            }

            

            if (ddlSalesman.SelectedItem == null)
            {
                MetroFramework.MetroMessageBox.Show(this, "Salesman name please..!!!");
                ddlSalesman.Focus();
                return false;
            }
            
            try
            {
           
                int a = Convert.ToInt32(txtAmount.Text);
                double p = Convert.ToSingle(a);
                double b = Convert.ToSingle(txtPriceRate.Text);
                double e = b / p;
                double c = _selectedProduct.PriceRate;

                if (c > e)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Sorry..You are going to lose...!!!");
                    txtPriceRate.Focus();
                    return false;
                }

                int x = _selectedProduct.Availability;
                int y = Convert.ToInt32(txtAmount.Text);

                if (x <y )
                {
                    MetroFramework.MetroMessageBox.Show(this, "Sorry..Product amount is not available...!!!");
                    txtAmount.Focus();
                    return false;
                }
             }
             catch (Exception exception)
              {
                   MetroFramework.MetroMessageBox.Show(this, "Sorry..Product amount is not available...!!!");
                  return false;
              }
                  
            return true;
        }

       

        private void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void dgvOtherProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedProduct = _products[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

       
        private void btnFinish_Click(object sender, EventArgs e)
        {
            lblGiveMoney.Show();
            txtGiveMoney.Show();
            lblPhone.Show();
            txtPhone.Show();
            txtPhone.Text = "";
            btnOK.Show();

            try
            {
                double takeMoney = Convert.ToSingle(txtTakeMoney.Text);
                double netPayment = Convert.ToSingle(txtNetPayment.Text);

                double giveMoney = takeMoney - netPayment;


                txtGiveMoney.Text = Convert.ToString(giveMoney);
            }
            catch (Exception exception)
            {

                MetroFramework.MetroMessageBox.Show(this, exception.Message);
            }

         
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            try
            {
                double totalPrice = Convert.ToSingle(txtTotal.Text);
                double duePayment = Convert.ToSingle(txtDue.Text);

                double netPayment = totalPrice - duePayment;

                lblNetPayment.Show();
                txtNetPayment.Show();
                lblTakeMoney.Show();
                txtTakeMoney.Show();
                txtTakeMoney.Focus();
                btnFinish.Show();

                txtTakeMoney.Text = "";
                txtNetPayment.Text = Convert.ToString(netPayment);
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                txtDue.Focus();
                return;
            }
            

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string phone = txtPhone.Text;
                string a = phone.Substring(0, 1);
                string b = phone.Substring(1, 1);
                string c = phone.Substring(2, 1);
                if (!(a == "0" && b == "1" && (c == "7" || c == "8" || c == "9" || c == "5" || c == "6" || c == "3"))
                    || phone.Length != 11 || (phone.Any(ch => ch < '0' || ch > '9')))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Wrong phone number!!");
                    txtPhone.Focus();
                    return;
                }

                var customer = _context.Customers.FirstOrDefault(u => u.Phone == txtPhone.Text);

                if (customer == null)
                {
                    lblName1.Show();
                    txtNameCustomerName1.Show();
                    txtNameCustomerName1.Focus();
                    btnOK2.Show();
                    lblName.Hide();
                    txtCustomerName.Hide();
                }
                else
                {
                    lblName.Show();
                    txtCustomerName.Show();
                    btnOK2.Show();
                    lblName1.Hide();
                    txtNameCustomerName1.Hide();

                    txtCustomerName.Text = customer.Name;

                    ddlSalesman.Focus();
                    ddlSalesman.DroppedDown = true;
                }
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Input is Invalid");
                txtPhone.Focus();
            }
            
        }

        private void btnOK2_Click(object sender, EventArgs e)
        {

            try
            {
                var customer = _context.Customers.FirstOrDefault(u => u.Phone == txtPhone.Text);
                var salesman = _context.Salesmen.FirstOrDefault(u => u.Name == ddlSalesman.Text);
                double totalPrice = Convert.ToSingle(txtTotal.Text);
                double payment = Convert.ToSingle(txtNetPayment.Text);
                double benifit1 = 0;
                lblTime.Text = DateTime.Now.ToLongTimeString();

                for (int i = 0; i < _temporarySells.Count; i++)
                {
                    benifit1 +=_temporarySells[i].Benifit;
                }
                if (customer == null)
                {

                    if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                    DialogResult.No)
                    {
                        OtherUserSellProductManager op1 = new OtherUserSellProductManager();
                        op1.Show();
                        this.Hide();
                        return;
                    }


                    _selectedCashMemo=new CashMemo()
                    {
                        Date = DateTime.Now
                    };
                    _selectedCashMemo.CustomerName = txtNameCustomerName1.Text;
                    _selectedCashMemo.CustomerPhone = txtPhone.Text;
                    _selectedCashMemo.Date = Convert.ToDateTime(dtpDateTime.Text);
                    _selectedCashMemo.Time = lblTime.Text;
                    _selectedCashMemo.Salesman = ddlSalesman.Text;
                    _selectedCashMemo.Phone = salesman.Phone;

                    string error4;
                    _selectedCashMemo = _cashMemoBl.Save(_selectedCashMemo, out error4);

                    if (string.IsNullOrEmpty(error4) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error4);
                        return;
                    }

                    _cashMemos.Add(_selectedCashMemo);





                    _selectedCustomer = new Customer();
                    _selectedCustomer.Name = txtNameCustomerName1.Text;
                    _selectedCustomer.Phone = txtPhone.Text;
                    _selectedCustomer.TotalPrice = totalPrice;
                    _selectedCustomer.Payment = payment;
                    _selectedCustomer.Benifit = benifit1;

                    string error;
                    _selectedCustomer = _customerBl.Save(_selectedCustomer, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    _customers.Add(_selectedCustomer);





                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                        for (int j = 0; j < _products.Count; j++)
                        {
                            if (_products[j].ID.ToString() == _temporarySells[i].ProductId.ToString())
                            {
                                _products[j].Benifit +=_temporarySells[i].Benifit;

                                int q=_products[j].Availability-_temporarySells[i].Quantity;
                                _products[j].Availability = q;

                                string error1;
                                _productBl.Save(_products[j], out error1);

                                if (string.IsNullOrEmpty(error1) == false)
                                {
                                    MetroFramework.MetroMessageBox.Show(this, error1);
                                    return;
                                }

                                break;
                            }
                        }

                    }




                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                        _selectedDaySell = new DaySell();
                        _selectedDaySell.Name = _temporarySells[i].Name;
                        _selectedDaySell.Time = lblTime.Text;
                        _selectedDaySell.ProductCategory = _temporarySells[i].ProductCategory.Name;
                        _selectedDaySell.Sell = Convert.ToSingle(_temporarySells[i].TotalPrice);
                        _selectedDaySell.Amount = Convert.ToInt32(_temporarySells[i].Quantity);
                        _selectedDaySell.Salesman = ddlSalesman.Text;
                        _selectedDaySell.Benifit = Convert.ToSingle(_temporarySells[i].Benifit);

                        string error2;
                        _selectedDaySell = _daySellBl.Save(_selectedDaySell, out error2);

                        if (string.IsNullOrEmpty(error2) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error2);
                            return;
                        }

                        _daySell.Add(_selectedDaySell);
                    }








                     
                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                      _selectedSellDetail = new SellDetail()
                        {
                            Date = DateTime.Now
                        };
                      _selectedSellDetail.Date = Convert.ToDateTime(dtpDateTime.Text);
                        _selectedSellDetail.Salesman = ddlSalesman.Text;
                        _selectedSellDetail.Customer = txtNameCustomerName1.Text;
                        _selectedSellDetail.CustomerPhone = txtPhone.Text;
                        _selectedSellDetail.ProductName = _temporarySells[i].Name;
                        _selectedSellDetail.ProductCategoryId = _temporarySells[i].ProductCategoryId;
                        _selectedSellDetail.RealPriceRate = _temporarySells[i].RealPriceRate;
                        _selectedSellDetail.SellPriceRate = _temporarySells[i].PriceRate;
                        _selectedSellDetail.Amount = _temporarySells[i].Quantity;
                        _selectedSellDetail.TotalPrice = _temporarySells[i].TotalPrice;
                        _selectedSellDetail.Benifit = _temporarySells[i].Benifit;
                        _selectedSellDetail.CashMemoId = _selectedCashMemo.ID;
                        _selectedSellDetail.Time = lblTime.Text;

                        string error2;
                        _selectedSellDetail = _sellDetailsBl.Save(_selectedSellDetail, out error2);

                        if (string.IsNullOrEmpty(error2) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error2);
                            return;
                        }

                        _sellDetails.Add(_selectedSellDetail);
                    }







                    double salesmanBenifit = 0;
                    for (int i = 0; i<_temporarySells.Count; i++)
                    {
                        salesmanBenifit += _temporarySells[i].Benifit;
                    }

                    for (int i = 0; i < _salesman.Count; i++)
                    {
                        if (_salesman[i].Name == ddlSalesman.Text)
                        {
                            string error1;
                            _salesman[i].Benifit += salesmanBenifit;
                            _salesmanBl.Save(_salesman[i], out error1);
                            if (string.IsNullOrEmpty(error1) == false)
                            {
                                MetroFramework.MetroMessageBox.Show(this, error1);
                                return;
                            }
                            break;
                        }
                    }







                    double totalPrice1 = 0;
                    double benifit = 0;
                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                        totalPrice1 +=_temporarySells[i].TotalPrice;
                        benifit +=_temporarySells[i].Benifit;
                    }
                    _selectedCustomerDaySell = new CustomerDaySell()
                    {
                        Date = DateTime.Now
                    };
                    _selectedCustomerDaySell.Date = Convert.ToDateTime(dtpDateTime.Text);
                    _selectedCustomerDaySell.Time = lblTime.Text;
                    _selectedCustomerDaySell.Name = txtNameCustomerName1.Text;
                    _selectedCustomerDaySell.Phone = txtPhone.Text;
                    _selectedCustomerDaySell.TotalPrice = totalPrice1;
                    _selectedCustomerDaySell.Payment = Convert.ToSingle(txtNetPayment.Text);
                    _selectedCustomerDaySell.Benifit = benifit;

                    string error3;
                    _customerDaySellBl.Save(_selectedCustomerDaySell, out error3);

                    if (string.IsNullOrEmpty(error3) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error3);
                        return;
                    }

                    _customerDaySells.Add(_selectedCustomerDaySell);


                    MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!");





                    if (MetroFramework.MetroMessageBox.Show(this, "Do you wanna print out...??", "Confirmation", MessageBoxButtons.YesNo) ==
                    DialogResult.No)
                    {
                        for (int i = 0; i < _temporarySells.Count; i++)
                        {
                            string error1;
                            if (_temporarySellBl.Delete(_temporarySells[i].ID, out error1) == false)
                            {
                                MetroFramework.MetroMessageBox.Show(this, error1);
                                return;
                            }

                        }


                        OtherUserSellProductManager op1 = new OtherUserSellProductManager();
                        op1.Show();
                        this.Hide();
                        return;
                    }


                    var cashMemo = _context.CashMemoes.FirstOrDefault(u => u.ID == _selectedSellDetail.CashMemoId);
                    FormPrint fp = new FormPrint(_temporarySells, cashMemo);
                    fp.Show();
                    this.Hide();

                }

                else
                {
                    if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                    DialogResult.No)
                    {
                        OtherUserSellProductManager op1 = new OtherUserSellProductManager();
                        op1.Show();
                        this.Hide();
                        return;
                    }


                    _selectedCashMemo = new CashMemo()
                    {
                        Date = DateTime.Now
                    };
                     _selectedCashMemo.CustomerName = customer.Name;
                    _selectedCashMemo.CustomerPhone = customer.Phone;
                    _selectedCashMemo.Date = Convert.ToDateTime(dtpDateTime.Text);
                    _selectedCashMemo.Time = lblTime.Text;
                    _selectedCashMemo.Salesman = ddlSalesman.Text;
                    _selectedCashMemo.Phone = salesman.Phone;

                    string error;
                    _selectedCashMemo = _cashMemoBl.Save(_selectedCashMemo, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    _cashMemos.Add(_selectedCashMemo);






                    double benifit2 = 0;

                    for (int i = 0; i< _temporarySells.Count; i++)
                    {
                        benifit2 +=_temporarySells[i].Benifit;
                    }
                    
                    customer.TotalPrice += totalPrice;
                    customer.Payment += payment;
                    customer.Benifit += benifit2;

                    string error4;
                    _selectedCustomer = _customerBl.Save(customer, out error4);

                    if (string.IsNullOrEmpty(error4) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error4);
                        return;
                    }




                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                        for (int j = 0; j < _products.Count; j++)
                        {
                            if (_products[j].ID.ToString() == _temporarySells[i].ProductId.ToString())
                            {
                                _products[j].Benifit += _temporarySells[i].Benifit;

                                int q = _products[j].Availability - _temporarySells[i].Quantity;
                                _products[j].Availability = q;

                                string error1;
                                _productBl.Save(_products[j], out error1);

                                if (string.IsNullOrEmpty(error1) == false)
                                {
                                    MetroFramework.MetroMessageBox.Show(this, error1);
                                    return;
                                }

                                break;
                            }
                        }

                    }



                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                        _selectedDaySell = new DaySell();
                        _selectedDaySell.Name = _temporarySells[i].Name;
                        _selectedDaySell.Time = lblTime.Text;
                        _selectedDaySell.ProductCategory = _temporarySells[i].ProductCategory.Name;
                        _selectedDaySell.Sell = Convert.ToSingle(_temporarySells[i].TotalPrice);
                        _selectedDaySell.Amount = Convert.ToInt32(_temporarySells[i].Quantity);
                        _selectedDaySell.Salesman = ddlSalesman.Text;
                        _selectedDaySell.Benifit = Convert.ToSingle(_temporarySells[i].Benifit);

                        string error2;
                        _selectedDaySell = _daySellBl.Save(_selectedDaySell, out error2);

                        if (string.IsNullOrEmpty(error2) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error2);
                            return;
                        }

                        _daySell.Add(_selectedDaySell);
                    }








                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                        _selectedSellDetail = new SellDetail()
                        {
                            Date = DateTime.Now
                        };
                        _selectedSellDetail.Date = Convert.ToDateTime(dtpDateTime.Text);
                        _selectedSellDetail.Salesman = ddlSalesman.Text;
                        _selectedSellDetail.Customer = txtCustomerName.Text;
                        _selectedSellDetail.CustomerPhone = customer.Phone;
                        _selectedSellDetail.ProductName = _temporarySells[i].Name;
                        _selectedSellDetail.ProductCategoryId = _temporarySells[i].ProductCategoryId;
                        _selectedSellDetail.RealPriceRate = _temporarySells[i].RealPriceRate;
                        _selectedSellDetail.SellPriceRate = _temporarySells[i].PriceRate;
                        _selectedSellDetail.Amount = _temporarySells[i].Quantity;
                        _selectedSellDetail.TotalPrice = _temporarySells[i].TotalPrice;
                        _selectedSellDetail.Benifit = _temporarySells[i].Benifit;
                        _selectedSellDetail.CashMemoId = _selectedCashMemo.ID;
                        _selectedSellDetail.Time = lblTime.Text;

                        string error2;
                        _selectedSellDetail = _sellDetailsBl.Save(_selectedSellDetail, out error2);

                        if (string.IsNullOrEmpty(error2) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error2);
                            return;
                        }

                        _sellDetails.Add(_selectedSellDetail);
                    }









                    double salesmanBenifit = 0;
                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                        salesmanBenifit += _temporarySells[i].Benifit;
                    }

                    for (int i = 0; i < _salesman.Count; i++)
                    {
                        if (_salesman[i].Name == ddlSalesman.Text)
                        {
                            string error1;
                            _salesman[i].Benifit += salesmanBenifit;
                            _salesmanBl.Save(_salesman[i], out error1);
                            if (string.IsNullOrEmpty(error1) == false)
                            {
                                MetroFramework.MetroMessageBox.Show(this, error1);
                                return;
                            }
                            break;
                        }
                    }





                    double totalPrice1 = 0;
                    double benifit = 0;
                    for (int i = 0; i < _temporarySells.Count; i++)
                    {
                        totalPrice1 += _temporarySells[i].TotalPrice;
                        benifit += _temporarySells[i].Benifit;
                    }
                    _selectedCustomerDaySell = new CustomerDaySell()
                    {
                        Date = DateTime.Now
                    };
                    _selectedCustomerDaySell.Date = Convert.ToDateTime(dtpDateTime.Text);
                    _selectedCustomerDaySell.Time = lblTime.Text;
                    _selectedCustomerDaySell.Name = txtCustomerName.Text;
                    _selectedCustomerDaySell.Phone = txtPhone.Text;
                    _selectedCustomerDaySell.TotalPrice = totalPrice1;
                    _selectedCustomerDaySell.Payment = Convert.ToSingle(txtNetPayment.Text);
                    _selectedCustomerDaySell.Benifit = benifit;

                    string error3;
                    _customerDaySellBl.Save(_selectedCustomerDaySell, out error3);

                    if (string.IsNullOrEmpty(error3) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error3);
                        return;
                    }

                    _customerDaySells.Add(_selectedCustomerDaySell);





                    
                    MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!");




                    if (MetroFramework.MetroMessageBox.Show(this, "Do you wanna print out...??", "Confirmation", MessageBoxButtons.YesNo) ==
                    DialogResult.No)
                    {
                        for (int i = 0; i < _temporarySells.Count; i++)
                        {
                            string error1;
                            if (_temporarySellBl.Delete(_temporarySells[i].ID, out error1) == false)
                            {
                                MetroFramework.MetroMessageBox.Show(this, error1);
                                return;
                            }

                        }


                        OtherUserSellProductManager op1 = new OtherUserSellProductManager();
                        op1.Show();
                        this.Hide();
                        return;
                    }

                    var cashMemo = _context.CashMemoes.FirstOrDefault(u => u.ID == _selectedSellDetail.CashMemoId);
                    FormPrint fp = new FormPrint(_temporarySells, cashMemo);
                    fp.Show();
                    this.Hide();


                }
            }
            catch (Exception exception)
            {

                MetroFramework.MetroMessageBox.Show(this, exception.Message);
            }
            
        }

        private void metroButton5_Click_1(object sender, EventArgs e)
        {
            try
            {
                int amount = Convert.ToInt32(txtAmount.Text);
                int id = Convert.ToInt32(txtSearch.Text);
                var product = _context.Products.FirstOrDefault(u => u.ID == id);

                if (product.Availability < amount)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Product amount is not available..!!!");

                    OtherUserSellProductManager op1 = new OtherUserSellProductManager();
                    op1.Show();
                    this.Hide();
                }
                else
                {
                    double sellPriceRate = Convert.ToSingle(txtPriceRate.Text);


                    var tempoProduct = _context.TemporarySells.FirstOrDefault(u => u.ProductId == id &&
                                                                                   u.PriceRate == sellPriceRate);

                    if (tempoProduct != null)
                    {
                        tempoProduct.Benifit += (tempoProduct.Benifit / tempoProduct.Quantity) * amount;
                        tempoProduct.Quantity += amount;
                        tempoProduct.TotalPrice += amount * sellPriceRate;
                        

                        string error;
                        _temporarySellBl.Save(tempoProduct, out error);

                        if (string.IsNullOrEmpty(error) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error);
                            return;
                        }

                        MetroFramework.MetroMessageBox.Show(this, "Sell Successfull..!!!");

                        OtherUserSellProductManager o=new OtherUserSellProductManager();
                        o.Show();
                        this.Hide();
                        return;
                    }

                    for (int i = 0; i < _products.Count; i++)
                    {
                        if (_products[i].Name == txtName.Text && _products[i].ProductCategoryName == ddlCategory.Text)
                        {
                            _selectedTemporarySell = new TemporarySell();
                            if (_temporarySells.Count == 0)
                            {
                                _selectedTemporarySell.Count = 1;

                            }
                            else
                            {
                                int count = _temporarySells.Count;
                                int a = _temporarySells[count - 1].Count;
                                _selectedTemporarySell.Count = a + 1;
                            }
                            _selectedTemporarySell.Date = Convert.ToDateTime(dtpDateTime.Text);
                            _selectedTemporarySell.Name = _selectedProduct.Name;
                            _selectedTemporarySell.ProductCategoryId = _selectedProduct.ProductCategoryId;
                            _selectedTemporarySell.Quantity = amount;
                            _selectedTemporarySell.RealPriceRate = _products[i].PriceRate;
                            _selectedTemporarySell.PriceRate = sellPriceRate;
                            _selectedTemporarySell.TotalPrice = amount * sellPriceRate;
                            _selectedTemporarySell.Benifit = (sellPriceRate - _selectedProduct.PriceRate) * amount;
                            _selectedTemporarySell.ProductId =_products[i].ID;


                            string error3;
                            _selectedTemporarySell = _temporarySellBl.Save(_selectedTemporarySell, out error3);

                            if (string.IsNullOrEmpty(error3) == false)
                            {
                                MetroFramework.MetroMessageBox.Show(this, error3);
                                return;
                            }

                            _temporarySells.Add(_selectedTemporarySell);
                            break;
                        }
                    }




                    MetroFramework.MetroMessageBox.Show(this, "Sell Successfull..!!!");

                    OtherUserSellProductManager op = new OtherUserSellProductManager();
                    op.Show();
                    this.Hide();
               
                }
                
            
             }
                
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, exception.Message);
            }
            
        }

      
        private void metroPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroLabel4_Click(object sender, EventArgs e)
        {

        }

        private void ddlSalesman_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroButton3_Click_1(object sender, EventArgs e)
        {
            this.Init();
            txtSearch.Focus();
        }

        private void metroButton3_Click_2(object sender, EventArgs e)
        {
            this.LoadOtherUserProductManagers();
            

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                btnSearch.PerformClick();
                
            }
            if (e.KeyCode == Keys.Left)
            {
                btnRefresh.Focus();
            }
            if (e.KeyCode == Keys.Right)
            {
                btnSearch.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
           if (_temporarySells.Count > 0)
            {
                double totalPrice = 0;

                for (int i = 0; i < _temporarySells.Count; i++)
                {
                    totalPrice += _temporarySells[i].TotalPrice;

                }

                lblTotal.Show();
                txtTotal.Show();
                lblDiscount.Show();
                txtDiscount.Text = "0";
                txtDiscount.Show();
                txtDiscount.Focus();
                
                txtDue.Text = Convert.ToString(0);
               
                txtTotal.Text = Convert.ToString(totalPrice);

            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Please buy some products..!!!");

                OtherUserSellProductManager op = new OtherUserSellProductManager();
                op.Show();
                this.Hide();
            }
            
        }

       

        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtAmount.Text) && txtAmount.Text!="0")
            {
                txtPriceRate.Focus();

            }
            
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
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

        private void txtPriceRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtAmount.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtPriceRate.Text) && txtPriceRate.Text != "0")
            {
                btnSave.PerformClick();

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

        private void txtSerialNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Right)
            {
                btnSerialOk.Focus();
            }
           
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtSerialNo.Text) && txtSerialNo.Text != "0")
            {
                btnSerialOk.PerformClick();

            }
        }

        private void txtSerialNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void btnSerialOk_Click(object sender, EventArgs e)
        {
            try
            {
                int a = Convert.ToInt32(txtSerialNo.Text);

                var tempo = _context.TemporarySells.FirstOrDefault(u => u.Count == a);

                if (tempo != null)
                {
                    if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                    DialogResult.No)
                    {
                        txtSerialNo.Focus();
                        return;
                    }
                    string error;
                    if (_temporarySellBl.DeleteTemporaryProduct(tempo.Count, out error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!");

                    OtherUserSellProductManager op = new OtherUserSellProductManager();
                    op.Show();
                    this.Hide();

                }
                if (tempo == null)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid Serial No.");
                    txtSerialNo.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                txtSerialNo.Focus();
                return;
            }
          
        }

        private void dgvTemporarySellList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedTemporarySell = _temporarySells[e.RowIndex];
                _selectedIndex1 = e.RowIndex;

            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            

        }

        private void metroButton2_Click_3(object sender, EventArgs e)
        {

            _temporarySells = _temporarySellBl.GetAll("");
            if (_temporarySells.Count > 0)
            {
                lblCancel.Show();
                txtSerialNo.Show();
                txtSerialNo.Text = "";
                txtSerialNo.Focus();
                btnSerialOk.Show();
            }

            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Please buy some product...");
                OtherUserSellProductManager op = new OtherUserSellProductManager();
                op.Show();
                this.Hide();
            }


        }

        private void metroButton2_Click_4(object sender, EventArgs e)
        {
            if (_temporarySells.Count > 0)
            {
                if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
               DialogResult.No)
                {
                    OtherUserSellProductManager op1 = new OtherUserSellProductManager();
                    op1.Show();
                    this.Hide();
                    return;
                }
                for (int i = 0; i < _temporarySells.Count; i++)
                {
                    string error;
                    if (_temporarySellBl.Delete(_temporarySells[i].ID, out error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        break;
                    }

                }
                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!");

                OtherUserSellProductManager op = new OtherUserSellProductManager();
                op.Show();
                this.Hide();
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Please buy some products...!!");

                OtherUserSellProductManager op = new OtherUserSellProductManager();
                op.Show();
                this.Hide();
            }

        }

        private void txtDue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtDue.Text))
            {
                btnOK1.PerformClick();
                txtTakeMoney.Focus();
            }

            if (e.KeyCode == Keys.Up)
            {
                txtDiscount.Focus();
            }
        }

        private void txtDue_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtTakeMoney_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtDue.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtTakeMoney.Text) && txtTakeMoney.Text!="0")
            {
                btnFinish.PerformClick();
                txtPhone.Focus();
            }
        }

        private void txtTakeMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                btnOK.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtTakeMoney.Focus();
            }
            
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtNameCustomerName1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtNameCustomerName1.Text))
            {
               // ddlSalesman.DroppedDown = true;
                ddlSalesman.Focus();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtPhone.Focus();
            }
        }

        private void ddlSalesman_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ddlSalesman.DroppedDown = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(ddlSalesman.Text))
            {
                btnOK2.PerformClick();
            }
        }

        private void txtDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                lblDue.Show();
                txtDue.Show();
                txtDue.Focus();
                btnOK1.Show();
            }
        }

        private void txtDiscount_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                double totalPrice = 0;

                for (int i = 0; i < _temporarySells.Count; i++)
                {
                    totalPrice += _temporarySells[i].TotalPrice;

                }
                
                decimal a = Convert.ToDecimal(totalPrice);

                decimal b = txtDiscount.Value;

                decimal c = b / 100;

                decimal d = a * c;

                decimal f = a - d;



                txtTotal.Text = f.ToString();
            }
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                btnRefresh.ResetFont();
            }
        }

        private void txtNetPayment_Click(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            this.Hide();
        }
    }
}
