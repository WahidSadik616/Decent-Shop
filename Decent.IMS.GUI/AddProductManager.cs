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
    public partial class AddProductManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();

        ProductBL _productBl=new ProductBL();
        List<Product> _products= new List<Product>();
        private Product _selectedProduct = null;
        private int _selectedIndex = 0;

        SellerNewProductBL _sellerNewProductBl = new SellerNewProductBL();
        List<SellerNewProduct> _sellerNewProducts = new List<SellerNewProduct>();
        private SellerNewProduct _selectedSellerNewProduct = null;
        private int _selectedIndex1 = 0;

        SellerBL _sellerBl = new SellerBL();
        List<Seller> _sellers = new List<Seller>();
        private Seller _selectedSeller = null;
        private int _selectedIndex2 = 0;

        ProductCategoryBL _ProductCategoryBl = new ProductCategoryBL();
        List<ProductCategory> _ProductCategories = new List<ProductCategory>();
        private ProductCategory _selectedProductCategory = null;
        private int _selectedIndex3 = 0;
        
        public AddProductManager()
        {
            InitializeComponent();
        }

        private void ProductManager_Load(object sender, EventArgs e)
        {
            this.Init();
            
        }

        private void Init()
        {
            btnBack.Focus();
            lblName.Hide();
            txtName.Hide();
            lblName1.Hide();
            txtName1.Hide();
            lblPriceRate.Hide();
            txtPriceRate.Hide();
            lblAvailability.Hide();
            txtAvailability.Hide();
            lblProductType.Hide();
            txtProductCategory.Hide();
            btnOk2.Hide();
            lblSellerName.Hide();
            txtSellerName.Hide();
            lblSellerPhone.Hide();
            txtSellerPhone.Hide();
            btnOk1.Hide();
            
            try
            {
                

                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                ddlSellerName.DataSource = _context.Sellers.ToList();
                ddlSellerName.DisplayMember = "Name";
                ddlSellerName.ValueMember = "ID";

                txtSearch.Text = "";

                this.LoadAddProductManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadAddProductManagers()
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
                _selectedProduct=new Product();
               
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
           
            txtName.Text = _selectedProduct.Name;
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
                var product=_context.Products.FirstOrDefault(u => u.Name == _selectedProduct.Name);
                var seller=_context.Sellers.FirstOrDefault(u => u.Name == ddlSellerName.Text);

                if (product != null)
                {
                    int amount = Convert.ToInt32(txtAvailability.Text);
                    _selectedProduct.Availability += amount;

                    double priceRate=Convert.ToSingle(txtPriceRate.Text);

                    _selectedProduct.PriceRate = priceRate;

                    string error;
                    _selectedProduct = _productBl.Save(_selectedProduct, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    _products.Add(_selectedProduct);


                    _selectedSellerNewProduct = new SellerNewProduct()
                    {
                        Date = DateTime.Now
                    };
                    _selectedSellerNewProduct.Date = Convert.ToDateTime(dtpDate.Text);
                    _selectedSellerNewProduct.Name = ddlSellerName.Text;
                    _selectedSellerNewProduct.ProductName = product.Name;
                    _selectedSellerNewProduct.ProductCategoryId = product.ProductCategoryId;
                    _selectedSellerNewProduct.ProductAmount = amount;
                    _selectedSellerNewProduct.PriceRate = priceRate;
                    _selectedSellerNewProduct.TotalPrice = amount*priceRate;


                    string error1;
                    _selectedSellerNewProduct = _sellerNewProductBl.Save(_selectedSellerNewProduct, out error1);

                    if (string.IsNullOrEmpty(error1) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error1);
                        return;
                    }

                    _sellerNewProducts.Add(_selectedSellerNewProduct);



                    seller.TotalPrice += (amount * priceRate);

                    string error2;
                    _selectedSeller = _sellerBl.Save(seller, out error2);

                    if (string.IsNullOrEmpty(error2) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error2);
                        return;
                    }

                   
                    


                    MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
                    AddProductManager a = new AddProductManager();
                    a.Show();
                    this.Hide();
                    
                }

                if (product == null)
                {
                    int amount = Convert.ToInt32(txtAvailability.Text);

                    double priceRate = Convert.ToSingle(txtPriceRate.Text);


                    _selectedProduct.Name = txtName1.Text;
                    _selectedProduct.PriceRate = Convert.ToSingle(txtPriceRate.Text);
                    _selectedProduct.Availability = Convert.ToInt32(txtAvailability.Text);
                    _selectedProduct.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                    _selectedProduct.SellerId = Int32.Parse(ddlSellerName.SelectedValue.ToString());

                    string error;
                    _selectedProduct = _productBl.Save(_selectedProduct, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    _products.Add(_selectedProduct);



                    _selectedSellerNewProduct = new SellerNewProduct()
                    {
                        Date = DateTime.Now
                    };
                    _selectedSellerNewProduct.Date = Convert.ToDateTime(dtpDate.Text);
                    _selectedSellerNewProduct.Name = ddlSellerName.Text;
                    _selectedSellerNewProduct.ProductName = txtName1.Text;
                    _selectedSellerNewProduct.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                    _selectedSellerNewProduct.ProductAmount = amount;
                    _selectedSellerNewProduct.PriceRate = priceRate;
                    _selectedSellerNewProduct.TotalPrice = amount * priceRate;


                    string error1;
                    _selectedSellerNewProduct = _sellerNewProductBl.Save(_selectedSellerNewProduct, out error1);

                    if (string.IsNullOrEmpty(error1) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error1);
                        return;
                    }

                     _sellerNewProducts.Add(_selectedSellerNewProduct);



                     seller.TotalPrice += (amount * priceRate);

                     string error2;
                     _selectedSeller = _sellerBl.Save(seller, out error2);

                     if (string.IsNullOrEmpty(error2) == false)
                     {
                         MetroFramework.MetroMessageBox.Show(this, error2);
                         return;
                     }


                    MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
                    AddProductManager a=new AddProductManager();
                    a.Show();
                    this.Hide();
                    this.RefreshDgv();
                }

             
            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");
                
            }

        }
        private bool isValid()
        {
            var product=_context.Products.FirstOrDefault(u => u.Name == txtName.Text);
            if (product != null)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Give the product name please...!!!");
                    txtName.Focus();
                    return false;
                }
            }
            
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


        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedSeller=new Seller();
                _selectedSeller.Name = txtSellerName.Text;
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
                AddProductManager pm = new AddProductManager();
                pm.Show();
                this.Hide();

            }
             catch (Exception exception)
             {
                 MetroFramework.MetroMessageBox.Show(this, "Input is not correct...!!");

             }

        }

        private void btnTypeOk_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedProductCategory = new ProductCategory();
                _selectedProductCategory.Name = txtProductCategory.Text;
               

                string error;
                _selectedProductCategory = _ProductCategoryBl.Save(_selectedProductCategory, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                _ProductCategories.Add(_selectedProductCategory);

                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
                AddProductManager pm = new AddProductManager();
                pm.Show();
                this.Hide();

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
                ddlSellerName.Select();
            }
        }

        private void ddlSellerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnNewSupplier.Select();
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtPriceRate.Focus();
            }
        }


        private void txtPriceRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtPriceRate.Text))
            {
                txtAvailability.Focus();
            }
           
        }

        private void txtAvailability_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtAvailability.Text))
            {
                btnSave.PerformClick();
                btnBack.Focus();
            }
            if (e.KeyCode == Keys.Up)
            {
                txtPriceRate.Focus();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Init();
            txtSearch.Focus();
        }

        private void txtSearch1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                btnSearch.PerformClick();
                btnAddOld.Focus();
            }
            if (e.KeyCode == Keys.Left)
            {
                btnRefresh.Focus();
            }
            if (e.KeyCode == Keys.Right)
            {
                btnSearch.Focus();
            }
        }

        private void metroButton1_Click_2(object sender, EventArgs e)
        {
            this.LoadAddProductManagers();
            var product = _context.Products.FirstOrDefault(u => u.Name == txtName.Text);
            if (product != null)
            {
                btnAddOld.PerformClick();
            }
            else
            {
                btnAddNew.PerformClick();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ProductDetailForm a=new ProductDetailForm();
            a.Show();
            this.Hide();
        }

        private void metroButton1_Click_3(object sender, EventArgs e)
        {

            lblName.Show();
            txtName.Show();
            lblPriceRate.Show();
            txtPriceRate.Show();
            txtPriceRate.Focus();
            lblAvailability.Show();
            txtAvailability.Show();
            lblName1.Hide();
            txtName1.Hide();

            txtAvailability.Text = "";
        }

        private void metroButton1_Click_4(object sender, EventArgs e)
        {
            txtName1.Text = "";
            _selectedProduct = new Product();
            //txtID.Text = _selectedProduct.ID.ToString();
            txtName.Text = _selectedProduct.Name;
            txtPriceRate.Text = Convert.ToString(_selectedProduct.PriceRate);
            txtAvailability.Text = Convert.ToString(_selectedProduct.Availability);
            ddlCategory.SelectedValue = _selectedProduct.ProductCategoryId;
            dgvProductList.ClearSelection();
            lblName.Hide();
            txtName.Hide();
            lblName1.Show();
            txtName1.Show();
            lblPriceRate.Show();
            txtPriceRate.Show();
            lblAvailability.Show();
            txtAvailability.Show();
            lblCategory.Show();
            ddlCategory.Show();
            ddlSellerName.Focus();
        }

        private void btnNewSupplier_Click(object sender, EventArgs e)
        {
            lblSellerName.Show();
            txtSellerName.Show();
            lblSellerPhone.Show();
            txtSellerPhone.Show();
            btnOk1.Show();
        }

        private void metroButton1_Click_5(object sender, EventArgs e)
        {
            lblProductType.Show();
            txtProductCategory.Show();
            btnOk2.Show();
        }

        private void metroButton1_Click_6(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }
            for (int i = 0; i < _products.Count; i++)
            {
                string error;
                if (_productBl.Delete(_products[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            AddProductManager pm = new AddProductManager();
            pm.Show();
            this.Hide();
        }

        private void metroButton1_Click_7(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            this.Hide();
        }

        private void AddProductManager_Load(object sender, EventArgs e)
        {

        }
    }
}
