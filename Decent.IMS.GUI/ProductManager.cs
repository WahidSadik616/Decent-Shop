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
    public partial class ProductManager : MetroFramework.Forms.MetroForm
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

        public ProductManager()
        {
            InitializeComponent();
        }

        private void ProductManager_Load(object sender, EventArgs e)
        {
            
            this.Init();
        }

        private void Init()
        {
           
            metroButton11.Hide();
            metroButton12.Hide();

            lblBenifitData.Text = "";
           
            try
            {
               
                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                ddlSellerName.DataSource = _context.Sellers.ToList();
                ddlSellerName.DisplayMember = "Name";
                ddlSellerName.ValueMember = "ID";

            
                txtSearch.Text = "";

                this.LoadProductManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadProductManagers()
        {
            _products = _productBl.GetAll(txtSearch.Text);
           

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
            txtID.Text = _selectedProduct.ID.ToString();
            txtName.Text = _selectedProduct.Name;
            txtPriceRate.Text = Convert.ToString(_selectedProduct.PriceRate);
            txtBenifit.Text = Convert.ToString(_selectedProduct.Benifit);
            txtAvailability.Text = Convert.ToString(_selectedProduct.Availability);
            ddlCategory.SelectedValue = _selectedProduct.ProductCategoryId;
            ddlSellerName.SelectedValue = _selectedProduct.SellerId;
           
        }



        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadProductManagers();
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

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedProduct=new Product();
           
            this.Populate();
         
            dgvProductList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedProduct.ID == 0)
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
            if (_productBl.Delete(_selectedProduct.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _products.Remove(_selectedProduct);
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
               

                _selectedProduct.Name = txtName.Text;
                _selectedProduct.PriceRate = Convert.ToSingle(txtPriceRate.Text);
                _selectedProduct.Availability = Convert.ToInt32(txtAvailability.Text);
                _selectedProduct.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                _selectedProduct.SellerId = Int32.Parse(ddlSellerName.SelectedValue.ToString());

                bool isNew = _selectedProduct.ID == 0;

                string error;
                _selectedProduct=_productBl.Save(_selectedProduct, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _products.Add(_selectedProduct);
                }
                else
                {
                    _products[_selectedIndex] = _selectedProduct;
                }

            
                
                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
                ProductManager pm=new ProductManager();
                pm.Show();
                this.Hide();
                
               

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
                MetroFramework.MetroMessageBox.Show(this, "Give the product name please...!!!");
                txtName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPriceRate.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Give the product price please...!!!");
                txtPriceRate.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtAvailability.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Give the product amount please...!!!");
                txtAvailability.Focus();
                return false;
            }
            return true;
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            ProductDetailForm pd=new ProductDetailForm();
            pd.Show();
            this.Hide();
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            LoginForm lf=new LoginForm();
            lf.Show();
            this.Hide();
        }

        private void BenifitGrideView()
        {
            lblBenifitData.Text = "Product Benifit Data";
           
            for (int i = 0; i < dgvProductList.Rows.Count; i++)
            {
                dgvProductList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                int a = Int32.Parse(dgvProductList.Rows[i].Cells[6].Value.ToString());
                if (a < 1000)
                {
                    dgvProductList.Rows[i].DefaultCellStyle.BackColor = Color.Honeydew;

                }
                else if (a >= 1000 && a < 2000)
                {
                    dgvProductList.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (a >= 2000 && a < 3000)
                {
                    dgvProductList.Rows[i].DefaultCellStyle.BackColor = Color.SpringGreen;
                }
                else
                {
                    dgvProductList.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                }
            }
        }

        

        private void metroButton9_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }
            _selectedProduct.Benifit = 0;
            string error;
            _selectedProduct = _productBl.Save(_selectedProduct, out error);
            this.Populate();
            this.RefreshDgv();
           
        }

        private void metroButton12_Click(object sender, EventArgs e)
        {
            ProductManager pm=new ProductManager();
            pm.Show();
            this.Hide();
        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
           
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }
            for (int i = 0; i < _products.Count; i++)
            {
                _products[i].Benifit = 0;
               
                string error;
                _products[i] = _productBl.Save(_products[i], out error);
                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }
              
            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            ProductManager pm=new ProductManager();
            pm.Show();
            this.Hide();
        }

        private void metroButton10_Click(object sender, EventArgs e)
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
            ProductManager pm = new ProductManager();
            pm.Show();
            this.Hide();
        }

       

     
        
       

        private void metroPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

       
        private void metroButton14_Click(object sender, EventArgs e)
        {
          
            lblID.Hide();
            lblName.Hide();
            lblPriceRate.Hide();
            lblAvailability.Hide();
            lblBenifit.Hide();
            lblCategory.Hide();

            metroButton1.Hide();
            metroButton2.Hide();
            metroButton3.Hide();
            metroButton4.Hide();
            metroButton6.Hide();
            
            metroButton5.Hide();
            metroButton14.Hide();
            metroButton9.Hide();

            txtID.Hide();
            txtName.Hide();
            txtPriceRate.Hide();
            txtAvailability.Hide();
            txtSearch.Hide();
            txtBenifit.Hide();

            ddlCategory.Hide();



            metroButton11.Show();
            metroButton12.Show();

            this.RefreshDgv();
            this.BenifitGrideView();
        }

   }
}
