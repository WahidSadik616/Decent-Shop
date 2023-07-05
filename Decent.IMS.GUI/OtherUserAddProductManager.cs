using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decent.IMS.BL;
using Decent.IMS.Data;

namespace Decent.IMS.GUI
{
    public partial class OtherUserAddProductManager : MetroFramework.Forms.MetroForm
    {
        private DecentDbEntities _context = new DecentDbEntities();

        private ProductBL _productBl = new ProductBL();
        private List<Product> _products = new List<Product>();
        private Product _selectedProduct = null;
        private int _selectedIndex = 0;

        private SellerNewProductBL _sellerNewProductBl = new SellerNewProductBL();
        private List<SellerNewProduct> _sellerNewProducts = new List<SellerNewProduct>();
        private SellerNewProduct _selectedSellerNewProduct = null;
        private int _selectedIndex1 = 0;

        private SellerBL _sellerBl = new SellerBL();
        private List<Seller> _sellers = new List<Seller>();
        private Seller _selectedSeller = null;
        private int _selectedIndex2 = 0;

        private ProductCategoryBL _ProductCategoryBl = new ProductCategoryBL();
        private List<ProductCategory> _ProductCategories = new List<ProductCategory>();
        private ProductCategory _selectedProductCategory = null;
        private int _selectedIndex3 = 0;


        private CompanyListsBL _companyListBl = new CompanyListsBL();
        private List<CompanyList> _companyLists = new List<CompanyList>();
        private CompanyList _selectedCompany = null;
        private int _selectedIndex4 = 0;


        CostDetailsBL _costDetailBl = new CostDetailsBL();
        List<CostDetail> _costDetails = new List<CostDetail>();
        private CostDetail _selectedCostDetail = null;
        private int _selectedIndex5 = 0;

        public OtherUserAddProductManager()
        {
            InitializeComponent();
        }

        private void ProductManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            lblCategory.Hide();
            ddlCategory.Hide();
            btnAddCategory.Hide();
            btnAddCategoryOk.Hide();
            lblProductType.Hide();
            txtProductCategory.Hide();
            btnTypeOk.Hide();
            lblSellerList.Hide();
            ddlSellerName.Hide();
            btnAddSupplier.Hide();
            btnAddSupplierOk.Hide();
            lblSellerName.Hide();
            txtSellerName.Hide();
            lblSellerPhone.Hide();
            txtSellerPhone.Hide();
            btnOk.Hide();
            lblProductList.Hide();
            ddlProduct.Hide();
            btnAddProduct.Hide();
            btnAddProductOk.Hide();
            lblAddProductName.Hide();
            txtProductName.Hide();
            btnProOk.Hide();
            lblPriceRate.Hide();
            txtPriceRate.Hide();
            lblAvailability.Hide();
            txtAvailability.Hide();
            lblDiscountPrice.Hide();
            txtDisTaka.Hide();
            lblCarryingCost.Hide();
            txtCarryingCost.Hide();
            lblTotalPrice.Hide();
            txtTotalPrice.Hide();
            btnSave.Hide();
            try
            {
                txtSearch.Focus();

                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                ddlSellerName.DataSource = _context.Sellers.ToList();
                ddlSellerName.DisplayMember = "Name";
                ddlSellerName.ValueMember = "ID";

                ddlProduct.DataSource = _context.CompanyLists.ToList();
                ddlProduct.DisplayMember = "CompanyName";
                ddlProduct.ValueMember = "ID";

                txtSearch.Text = "";

                this.LoadOtherUserAddProductManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadOtherUserAddProductManagers()
        {
            _products = _productBl.GetAll(txtSearch.Text);
            _sellerNewProducts = _sellerNewProductBl.GetAll("");
            _sellers = _sellerBl.GetAll("");

            if (_products.Count > 0)
            {
                _selectedProduct = _products[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedProduct = new Product();

            }

            this.Populate();
            this.RefreshDgv();

        }

        private void RefreshDgv()
        {
            dgvProductList.AutoGenerateColumns = false;
            dgvProductList.DataSource = _products.ToList();
            dgvProductList.Refresh();

            dgvProductList.ClearSelection();

            for (int i = 0; i < dgvProductList.Rows.Count; i++)
            {
                if (dgvProductList.Rows[i].Cells[0].Value.ToString() == _selectedProduct.ID.ToString())
                {
                    dgvProductList.Rows[i].Selected = true;
                    dgvProductList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvProductList.Rows.Count; i++)
            {
                dgvProductList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtPriceRate.Text = Convert.ToString(_selectedProduct.PriceRate);
            ddlCategory.SelectedValue = _selectedProduct.ProductCategoryId;
            ddlSellerName.SelectedValue = _selectedProduct.SellerId;
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedProduct = _products[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }


        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;
            try
            {
                double totalPrice = Convert.ToSingle(txtTotalPrice.Text);
                    double carryingCost = Convert.ToSingle(txtCarryingCost.Text);
                    int amount = Convert.ToInt32(txtAvailability.Text);
                double pricaRate=Convert.ToSingle(txtPriceRate.Text);

                    double priceRateForSale = totalPrice / amount;



                var oldPro = _context.Products.FirstOrDefault(u => u.Name == ddlProduct.Text &&
                                                                   u.ProductCategory.Name == ddlCategory.Text &&
                                                                   u.Seller.Name == ddlSellerName.Text &&
                                                                   u.PriceRate==priceRateForSale);


                
                if (oldPro == null)
                {
                   
                   Product newProduct = new Product();
                    newProduct.Name = ddlProduct.Text;
                    newProduct.PriceRate = (double) Math.Round(priceRateForSale, 2, MidpointRounding.ToEven); //Convert.ToSingle(txtPriceRate.Text);
                    newProduct.Availability = Convert.ToInt32(txtAvailability.Text);
                    newProduct.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                    newProduct.SellerId = Int32.Parse(ddlSellerName.SelectedValue.ToString());

                    string error;
                    _productBl.Save(newProduct, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    _products.Add(newProduct);

                    _products = _productBl.GetAll("");
                    
                    int id = _products[(_products.Count - 1)].ID ;

                    string id1 = id.ToString();

                    MetroFramework.MetroMessageBox.Show(this, "Product id is---->" + id1); 
                    




                    _selectedSellerNewProduct = new SellerNewProduct()
                    {
                        Date = DateTime.Now
                    };

                    _selectedSellerNewProduct.Date = Convert.ToDateTime(dtpDate.Text);
                    _selectedSellerNewProduct.Time = DateTime.Now.ToString("HH:mm:ss tt");
                    _selectedSellerNewProduct.Name = ddlSellerName.Text;
                    _selectedSellerNewProduct.ProductName = ddlProduct.Text;
                    _selectedSellerNewProduct.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                    _selectedSellerNewProduct.ProductAmount = amount;
                    _selectedSellerNewProduct.PriceRate = (double)Math.Round(pricaRate, 2, MidpointRounding.ToEven);
                    _selectedSellerNewProduct.TotalPrice = (double)Math.Round((amount * pricaRate), 2, MidpointRounding.ToEven);

                    string discount = txtDisTaka.Text;

                    if (discount.Contains("%"))
                    {
                        _selectedSellerNewProduct.Discount = discount;

                        _selectedSellerNewProduct.AfterTotalPrice = totalPrice-carryingCost;
                        _selectedSellerNewProduct.CarryingCost = Convert.ToSingle(txtCarryingCost.Text);
                        _selectedSellerNewProduct.AfterPriceRate = (double) Math.Round(priceRateForSale, 2, MidpointRounding.ToEven);
                    }
                    else
                    {
                        _selectedSellerNewProduct.Discount = discount + "/=";

                        _selectedSellerNewProduct.AfterTotalPrice = totalPrice-carryingCost;
                        _selectedSellerNewProduct.CarryingCost = Convert.ToSingle(txtCarryingCost.Text);
                        _selectedSellerNewProduct.AfterPriceRate = (double) Math.Round(priceRateForSale, 2, MidpointRounding.ToEven);
                    }




                    string error1;
                    _selectedSellerNewProduct = _sellerNewProductBl.Save(_selectedSellerNewProduct, out error1);

                    if (string.IsNullOrEmpty(error1) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error1);
                        return;
                    }

                    _sellerNewProducts.Add(_selectedSellerNewProduct);








                    var seller = _context.Sellers.FirstOrDefault(u => u.Name == ddlSellerName.Text);
                    seller.TotalPrice += totalPrice-carryingCost;

                    string error2;
                    _selectedSeller = _sellerBl.Save(seller, out error2);

                    if (string.IsNullOrEmpty(error2) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error2);
                        return;
                    }




                    if((Convert.ToSingle(txtCarryingCost.Text)>0))
                    {
                        _selectedCostDetail = new CostDetail()
                        {
                            Date = DateTime.Now
                        };
                        _selectedCostDetail.Date = Convert.ToDateTime(dtpDate.Text);
                        _selectedCostDetail.Name = "Carrying Cost";
                        _selectedCostDetail.Amount = Convert.ToSingle(txtCarryingCost.Text);
                        string error3;
                        _selectedCostDetail = _costDetailBl.Save(_selectedCostDetail, out error3);

                        if (string.IsNullOrEmpty(error3) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error3);
                            return;
                        }
                        _costDetails.Add(_selectedCostDetail);

                    }




                    if (MetroFramework.MetroMessageBox.Show(this, "Do you want to payment right now?", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
                    {
                        OtherUserPaymentAfterBuyForm p = new OtherUserPaymentAfterBuyForm(ddlSellerName.Text);
                        p.Show();
                        this.Hide();
                        return;
                    }


                    
                        OtherUserAddProductManager bbbb = new OtherUserAddProductManager();
                        bbbb.Show();
                        this.Hide();
                    
                }

                if (oldPro != null)
                {

                    oldPro.Availability += Convert.ToInt32(txtAvailability.Text);
                    

                    string error;
                    _productBl.Save(oldPro, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }
                    MetroFramework.MetroMessageBox.Show(this, "Product id is---->:"+oldPro.ID.ToString());






                    _selectedSellerNewProduct = new SellerNewProduct()
                    {
                        Date = DateTime.Now
                    };

                    _selectedSellerNewProduct.Date = Convert.ToDateTime(dtpDate.Text);
                    _selectedSellerNewProduct.Time = DateTime.Now.ToString("HH:mm:ss tt");
                    _selectedSellerNewProduct.Name = ddlSellerName.Text;
                    _selectedSellerNewProduct.ProductName = ddlProduct.Text;
                    _selectedSellerNewProduct.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                    _selectedSellerNewProduct.ProductAmount = amount;
                    _selectedSellerNewProduct.PriceRate = (double)Math.Round(pricaRate, 2, MidpointRounding.ToEven);
                    _selectedSellerNewProduct.TotalPrice = (double)Math.Round((amount * pricaRate), 2, MidpointRounding.ToEven);

                    string discount = txtDisTaka.Text;

                    if (discount.Contains("%"))
                    {
                        _selectedSellerNewProduct.Discount = discount;

                        _selectedSellerNewProduct.AfterTotalPrice = totalPrice - carryingCost;
                        _selectedSellerNewProduct.CarryingCost = Convert.ToSingle(txtCarryingCost.Text);
                        _selectedSellerNewProduct.AfterPriceRate = (double)Math.Round(priceRateForSale, 2, MidpointRounding.ToEven);
                    }
                    else
                    {
                        _selectedSellerNewProduct.Discount = discount + "/=";

                        _selectedSellerNewProduct.AfterTotalPrice = totalPrice - carryingCost;
                        _selectedSellerNewProduct.CarryingCost = Convert.ToSingle(txtCarryingCost.Text);
                        _selectedSellerNewProduct.AfterPriceRate = (double)Math.Round(priceRateForSale, 2, MidpointRounding.ToEven);
                    }




                    string error1;
                    _selectedSellerNewProduct = _sellerNewProductBl.Save(_selectedSellerNewProduct, out error1);

                    if (string.IsNullOrEmpty(error1) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error1);
                        return;
                    }

                    _sellerNewProducts.Add(_selectedSellerNewProduct);







                    var seller = _context.Sellers.FirstOrDefault(u => u.Name == ddlSellerName.Text);
                    seller.TotalPrice += totalPrice - carryingCost;

                    string error2;
                    _selectedSeller = _sellerBl.Save(seller, out error2);

                    if (string.IsNullOrEmpty(error2) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error2);
                        return;
                    }





                    if ((Convert.ToSingle(txtCarryingCost.Text) > 0))
                    {
                        _selectedCostDetail = new CostDetail()
                        {
                            Date = DateTime.Now
                        };
                        _selectedCostDetail.Date = Convert.ToDateTime(dtpDate.Text);
                        _selectedCostDetail.Name = "Carrying Cost";
                        _selectedCostDetail.Amount = Convert.ToSingle(txtCarryingCost.Text);
                        string error3;
                        _selectedCostDetail = _costDetailBl.Save(_selectedCostDetail, out error3);

                        if (string.IsNullOrEmpty(error3) == false)
                        {
                            MetroFramework.MetroMessageBox.Show(this, error3);
                            return;
                        }
                        _costDetails.Add(_selectedCostDetail);

                    }




                    if (MetroFramework.MetroMessageBox.Show(this, "Do you want to payment right now?", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
                    {
                        OtherUserPaymentAfterBuyForm p = new OtherUserPaymentAfterBuyForm(ddlSellerName.Text);
                        p.Show();
                        this.Hide();
                        return;
                    }
                    
                        OtherUserAddProductManager bbbb = new OtherUserAddProductManager();
                        bbbb.Show();
                        this.Hide();
                    
                }
                

            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");
                
            }

           
            }


        private bool isValid()
        {
            
            if (string.IsNullOrEmpty(txtPriceRate.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Give the product price please...!!!");
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
           

            if (string.IsNullOrEmpty(txtAvailability.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Give the product amount please...!!!");
                txtAvailability.Focus();
                return false;
            }

            int amount;
            if (!int.TryParse(txtAvailability.Text, out amount) || Convert.ToInt32(txtAvailability.Text) <= 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid amount...!!!");
                txtAvailability.Focus();
                return false;
            }

            return true;
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            OtherUserHomeForm ud = new OtherUserHomeForm();
            ud.Show();
            this.Hide();
        }

       

        private void metroButton10_Click_1(object sender, EventArgs e)
        {
            this.Init();
            txtSearch.Focus();
        }

        private void metroButton3_Click_1(object sender, EventArgs e)
        {
            this.LoadOtherUserAddProductManagers();
           
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                btnSearch.PerformClick();
                //btnAddOld.Focus();
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

       
        

        private void txtPriceRate_KeyDown(object sender, KeyEventArgs e)
        {
            double priceRate;
            
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtPriceRate.Text) 
                && double.TryParse(txtPriceRate.Text, out priceRate))
            {
                double a = Convert.ToSingle(txtPriceRate.Text);
                if (a > 0)
                {
                    lblAvailability.Show();
                    txtAvailability.Show();
                    txtAvailability.Text = "";
                    txtAvailability.Focus(); 
                }
                
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                ddlProduct.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                txtAvailability.Focus();
            }
        }

        private void txtAvailability_KeyDown(object sender, KeyEventArgs e)
        {
            int availability;
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtAvailability.Text)
                && int.TryParse(txtAvailability.Text, out availability))
            {
                int a=Convert.ToInt32(txtAvailability.Text);
                if (a > 0)
                {
                    lblDiscountPrice.Show();
                    txtDisTaka.Show();
                    txtDisTaka.Text = "0";
                    txtDisTaka.Focus();
                    
                }
                
                try
                {
                    int amount = Convert.ToInt32(txtAvailability.Text);
                    double priceRate = Convert.ToSingle(txtPriceRate.Text);
                    double totalPrice = amount * priceRate;
                    double finalTotalPrice=(double)Math.Round(totalPrice,2,MidpointRounding.ToEven);
                    lblTotalPrice.Show();
                    txtTotalPrice.Show();
                    txtTotalPrice.Text = Convert.ToString(finalTotalPrice);
                    btnSave.Show();
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                    btnRefresh.PerformClick();
                }
                

            }
            if (e.KeyCode == Keys.Up)
            {
                txtPriceRate.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                txtDisTaka.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
        }

        private void metroButton1_Click_4(object sender, EventArgs e)
        {
            txtSellerName.Text = "";
            txtSellerPhone.Text = "";
            lblSellerName.Show();
            txtSellerName.Show();
            txtSellerName.Focus();
            lblSellerPhone.Show();
            txtSellerPhone.Show();

            btnOk.Show();
        }

        private void metroButton1_Click_5(object sender, EventArgs e)
        {
          
            try
            {
                DecentDbEntities _context = new DecentDbEntities();
                var supplierPhone = _context.Sellers.FirstOrDefault(u => u.Phone == txtSellerPhone.Text);
                if (supplierPhone != null)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Supplier is already exist...!!!");
                    txtSellerPhone.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtSellerName.Text))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Seller Name Please...!!!");
                    txtSellerName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtSellerPhone.Text))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Phone number Please...!!!");
                    txtSellerPhone.Focus();

                    return;
                }
                
                string phone = txtSellerPhone.Text;
                string a = phone.Substring(0, 1);
                string b = phone.Substring(1, 1);
                string c = phone.Substring(2, 1);
                if (!(a == "0" && b == "1" && (c == "7" || c == "8" || c == "9" || c == "5" || c=="6" || c == "3")) 
                    || phone.Length != 11 || (phone.Any(ch => ch < '0' || ch > '9')))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Wrong phone number!!");
                    txtSellerPhone.Focus();
                    return;
                }
                
                a = txtSellerName.Text;

                string k=a.Substring(0, 1).ToUpper() + a.Substring(1).ToLower();
                
                _selectedSeller = new Seller();
                _selectedSeller.Name = k;
                _selectedSeller.Phone = txtSellerPhone.Text;
                _selectedSeller.TotalPrice = 0;
                _selectedSeller.Payment = 0;

                string error;
                _selectedSeller = _sellerBl.Save(_selectedSeller, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                _sellers.Add(_selectedSeller);

                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");

                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                ddlSellerName.DataSource = _context.Sellers.ToList();
                ddlSellerName.DisplayMember = "Name";
                ddlSellerName.ValueMember = "ID";

                txtSellerName.Hide();
                txtSellerPhone.Hide();
                lblSellerName.Hide();
                lblSellerPhone.Hide();
                btnOk.Hide();
                ddlSellerName.Focus();
                ddlSellerName.Focus();
                ddlSellerName.DroppedDown = true;

                
                ddlSellerName.Focus();
                ddlSellerName.DroppedDown = true;


            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");

            }
        }

        private void dtpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ddlCategory.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            
        }

        private void ddlSellerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ddlSellerName.DroppedDown = true;
            }

            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(ddlSellerName.Text))
            {
                btnAddSupplierOk.PerformClick();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Right)
            {
                btnAddSupplier.Focus();
            }
        }

        private void txtSellerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                btnAddSupplier.Focus();
            }
            if (e.KeyCode == Keys.Down || (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSellerName.Text)))
            {
                txtSellerPhone.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }

        }

        private void txtSellerPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSellerPhone.Text))
            {
                btnOk.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtSellerName.Focus();
            }
            
        }

       
        private void ddlCategory_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ddlCategory.DroppedDown = true;
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(ddlCategory.Text))
            {
                
                btnAddCategoryOk.PerformClick();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Right)
            {
                btnAddCategory.Focus();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            txtProductCategory.Text = "";
            lblProductType.Show();
            txtProductCategory.Show();
            txtProductCategory.Focus();
            btnTypeOk.Show();
        }

        private void txtProductCategory_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                btnAddCategory.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtProductCategory.Text))
            {
                btnTypeOk.PerformClick();
            }
        }

        private bool isValid2()
        {

            if (string.IsNullOrEmpty(txtProductCategory.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Product category please...!!!");
                txtProductCategory.Focus();
                return false;
            }

            var productCategory = _context.ProductCategories.FirstOrDefault(u => u.Name == txtProductCategory.Text);

            if (productCategory != null)
            {
                MetroFramework.MetroMessageBox.Show(this, "Category is already exist...!!!");
                txtProductCategory.Focus();
                return false;
            }
            return true;
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            if (!isValid2())
                return;
            try
            {
                _selectedProductCategory = new ProductCategory();

                string a=txtProductCategory.Text;


                string k = a.Substring(0, 1).ToUpper() + a.Substring(1).ToLower();

                _selectedProductCategory.Name = k;

                string error;
                _selectedProductCategory = _ProductCategoryBl.Save(_selectedProductCategory, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                _ProductCategories.Add(_selectedProductCategory);

                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");

               
                btnRefresh.PerformClick();
                lblCategory.Show();
                ddlCategory.Show();
                ddlCategory.Focus();
                ddlCategory.DroppedDown = true;
                btnAddCategory.Show();
                btnAddCategoryOk.Show();

               ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                txtProductCategory.Hide();
                lblProductType.Hide();
                btnTypeOk.Hide();
                ddlCategory.Focus();
                ddlCategory.Focus();
                ddlCategory.DroppedDown = true;

                

            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");

            }
        }

        private void btnAddSupplierOk_Click(object sender, EventArgs e)
        {
            lblProductList.Show();
            ddlProduct.Show();
            btnAddProduct.Show();
            btnAddProductOk.Show();
            ddlProduct.Focus();
            ddlProduct.DroppedDown = true;
        }

        private void btnAddCategoryOk_Click(object sender, EventArgs e)
        {
            lblSellerList.Show();
            ddlSellerName.Show();
            ddlSellerName.Focus();
            ddlSellerName.DroppedDown = true;
            btnAddSupplier.Show();
            btnAddSupplierOk.Show();

            
        }

        
        private void txtCarryingCost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtCarryingCost.Text))
            {
                try
                {

                    int amount = Convert.ToInt32(txtAvailability.Text);

                    double priceRateB = Convert.ToSingle(txtPriceRate.Text);

                    string discout1 = txtDisTaka.Text;

                    if (discout1.Contains("%"))
                    {
                        try
                        {
                            double _tempoTotalPrice = amount * priceRateB;


                            string discountAmountString1 = discout1.Substring(0, discout1.Length - 1);

                            double discountAmount1 = Convert.ToSingle(discountAmountString1);

                            double aa1 = discountAmount1 / 100;

                            double bb1 = aa1 * _tempoTotalPrice;

                            double totalPriceAfterDis1 = _tempoTotalPrice - bb1;

                            double carryingCost = Convert.ToSingle(txtCarryingCost.Text);
                            
                            double totalPriceWithCarryingCost = totalPriceAfterDis1 + carryingCost;

                            double finalTotalPriceWithCarryingCost = (double)Math.Round(totalPriceWithCarryingCost, 2, MidpointRounding.ToEven);

                            txtTotalPrice.Text = Convert.ToString(finalTotalPriceWithCarryingCost);
                        }
                        catch (Exception ex)
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");
                            txtDisTaka.Focus();
                            return;
                        }

                    }
                    else
                    {
                        try
                        {
                            double _tempoTotalPrice = amount * priceRateB;
                            double aa2 = Convert.ToSingle(discout1);
                            double bb2 = _tempoTotalPrice - aa2;
                            
                            double carryingCost = Convert.ToSingle(txtCarryingCost.Text);

                            double totalPriceWithCarryingCost = bb2 + carryingCost;

                            double finalTotalPriceWithCarryingCost = (double)Math.Round(totalPriceWithCarryingCost, 2, MidpointRounding.ToEven);

                            txtTotalPrice.Text = Convert.ToString(finalTotalPriceWithCarryingCost);

                        }
                        catch (Exception ex)
                        {

                            MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");
                            txtDisTaka.Focus();
                            return;

                        }

                    }





                    
                }
                catch (Exception)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");
                    txtCarryingCost.Focus();
                    return;
                }
                txtTotalPrice.Focus();
                btnSave.Show();
                
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtDisTaka.Focus();
            }
        }

        private void txtCarryingCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            this.Hide();
        }

        

        private void metroButton1_Click_3(object sender, EventArgs e)
        {
            lblCategory.Show();
            ddlCategory.Show();
            btnAddCategory.Show();
            ddlCategory.Focus();
            ddlCategory.DroppedDown = true;
            btnAddCategoryOk.Show();
           
        }

        private void ddlProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ddlProduct.DroppedDown = true;
            }

            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(ddlSellerName.Text))
            {
                lblPriceRate.Show();
                txtPriceRate.Show();
                txtPriceRate.Text = "";
                txtPriceRate.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Right)
            {
                btnAddProduct.Focus();
            }
        }

        private void btnAddProductOk_Click(object sender, EventArgs e)
        {
            lblPriceRate.Show();
            txtPriceRate.Show();
            txtPriceRate.Text = "";
            txtPriceRate.Focus();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            lblAddProductName.Show();
            txtProductName.Show();
            txtProductName.Text = "";
            txtProductName.Focus();
            btnProOk.Show();
        }

        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                btnAddProduct.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                btnProOk.PerformClick();
            }
        }

        private void btnProOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Company Name please...!!!");
                txtProductName.Focus();
                return;
            }

            var productName = _context.CompanyLists.FirstOrDefault(u => u.CompanyName == txtProductName.Text);

            if (productName != null)
            {
                MetroFramework.MetroMessageBox.Show(this, "Company name is already exist");
                txtProductName.Focus();
                return;
            }
            try
            {
                _selectedCompany = new CompanyList();

                string a = txtProductName.Text;


                string k = a.Substring(0, 1).ToUpper() + a.Substring(1).ToLower();

                _selectedCompany.CompanyName = k;

                string error;
                _selectedCompany = _companyListBl.Save(_selectedCompany, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                _companyLists.Add(_selectedCompany);

                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");

                ddlProduct.DataSource = _context.CompanyLists.ToList();
                ddlProduct.DisplayMember = "Name";
                ddlProduct.ValueMember = "ID";

                lblAddProductName.Hide();
                txtProductName.Hide();
                btnProOk.Hide();


                ddlProduct.Focus();
                ddlProduct.Focus();
                ddlProduct.DroppedDown = true;
              
                
                
               
            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");

            }
            
        }

        private void txtPriceRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8 && ch !=46)
            {
                e.Handled = true;
            }
        }

        private void txtAvailability_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtDisTaka_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8 && ch !=37)
            {
                e.Handled = true;
            }
        }

        private void txtDisTaka_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtAvailability.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                txtCarryingCost.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtDisTaka.Text))
            {
                int amount = Convert.ToInt32(txtAvailability.Text);

                double priceRateB = Convert.ToSingle(txtPriceRate.Text);

                string discout1 = txtDisTaka.Text;

                if (discout1.Contains("%"))
                {
                    try
                    {
                        double _tempoTotalPrice = amount * priceRateB;

                        
                        string discountAmountString1 = discout1.Substring(0, discout1.Length - 1);

                        double discountAmount1 = Convert.ToSingle(discountAmountString1);

                        double aa1 = discountAmount1 / 100;

                        double bb1 = aa1 * _tempoTotalPrice;

                        double totalPriceAfterDis1 = _tempoTotalPrice - bb1;

                        double finalTotalPriceAfterDis1 = (double)Math.Round(totalPriceAfterDis1, 2, MidpointRounding.ToEven);


                        txtTotalPrice.Text = Convert.ToString(finalTotalPriceAfterDis1);
                    }
                    catch (Exception ex)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");
                        txtDisTaka.Focus();
                        return;
                    }
                    
                }
                else
                {
                    try
                    {
                        double _tempoTotalPrice = amount * priceRateB;
                        double aa2 = Convert.ToSingle(discout1);
                        double bb2 = _tempoTotalPrice - aa2;
                        int finalbb2 = (int)Math.Round(bb2);
                        txtTotalPrice.Text = Convert.ToString(finalbb2);
                    }
                    catch (Exception ex)
                    {

                        MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");
                        txtDisTaka.Focus();
                        return;

                    }
                    
                }
                lblCarryingCost.Show();
                txtCarryingCost.Show();
                txtCarryingCost.Text = "0";
                txtCarryingCost.Focus();
            }
            
        }

        private void txtTotalPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh.PerformClick();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtCarryingCost.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtTotalPrice.Text))
            {
                btnSave.PerformClick();
            }
        }

        private void txtSellerPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

       
    }
}
