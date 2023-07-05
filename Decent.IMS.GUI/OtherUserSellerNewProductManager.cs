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
    public partial class OtherUserSellerNewProductManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        SellerNewProductBL _sellerNewProductBl=new SellerNewProductBL();
        List<SellerNewProduct> _sellerNewProducts= new List<SellerNewProduct>();
        private SellerNewProduct _selectedSellerNewProduct = null;
        private int _selectedIndex = 0;

        public OtherUserSellerNewProductManager(List<SellerNewProduct>  PurchaseProduct )
        {
            _sellerNewProducts = PurchaseProduct;
            //this.LoadSellerNewProductManagers(_sellerNewProducts);
            //this.Init(_sellerNewProducts);
            
            InitializeComponent();
        }

        private void SellerNewProductManager_Load(object sender, EventArgs e)
        {
            
            this.Init();
        }

        private void Init()
        {
            try
            {
               
                ddlCategory.DataSource = _context.ProductCategories.ToList();
                ddlCategory.DisplayMember = "Name";
                ddlCategory.ValueMember = "ID";

                txtSearch.Text = "";

                this.LoadSellerNewProductManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadSellerNewProductManagers()
        { 
            
           // _sellerNewProducts = _sellerNewProductBl.GetAllDateRange(start,end);

            if (_sellerNewProducts.Count > 0)
            {
                _selectedSellerNewProduct = _sellerNewProducts[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedSellerNewProduct=new SellerNewProduct()
                {
                    Date = DateTime.Now
                };
            }
            
            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvSellerNewProductList.AutoGenerateColumns = false;
            dgvSellerNewProductList.DataSource = _sellerNewProducts.ToList();
            dgvSellerNewProductList.Refresh();

            dgvSellerNewProductList.ClearSelection();

           for (int i = 0; i < dgvSellerNewProductList.Rows.Count; i++)
            {
                if (dgvSellerNewProductList.Rows[i].Cells[0].Value.ToString() == _selectedSellerNewProduct.ID.ToString())
                {
                    dgvSellerNewProductList.Rows[i].Selected = true;
                    dgvSellerNewProductList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvSellerNewProductList.Rows.Count; i++)
            {
                dgvSellerNewProductList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedSellerNewProduct.ID.ToString();
            dtpDate.Text = _selectedSellerNewProduct.Date.ToString();
            txtName.Text = _selectedSellerNewProduct.Name;
            txtProductName.Text = _selectedSellerNewProduct.ProductName;
            txtProductAmount.Text = Convert.ToString(_selectedSellerNewProduct.ProductAmount);
            txtPriceRate.Text = Convert.ToString(_selectedSellerNewProduct.PriceRate);
            txtDiscount.Text = Convert.ToString(_selectedSellerNewProduct.Discount);
            txtCarryingCost.Text = Convert.ToString(_selectedSellerNewProduct.CarryingCost);
            ddlCategory.SelectedValue = _selectedSellerNewProduct.ProductCategoryId;
            txtTime.Text = _selectedSellerNewProduct.Time;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadSellerNewProductManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedSellerNewProduct = _sellerNewProducts[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedSellerNewProduct=new SellerNewProduct()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvSellerNewProductList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedSellerNewProduct.ID == 0)
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
            if (_sellerNewProductBl.Delete(_selectedSellerNewProduct.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }
         
            _sellerNewProducts.Remove(_selectedSellerNewProduct);
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
                int amount=Convert.ToInt32(txtProductAmount.Text);
                double priceRate = Convert.ToSingle(txtPriceRate.Text);
                _selectedSellerNewProduct.Name = txtName.Text;
                _selectedSellerNewProduct.ProductName = txtProductName.Text;
                _selectedSellerNewProduct.ProductAmount = Convert.ToInt32(txtProductAmount.Text);
                _selectedSellerNewProduct.PriceRate = Convert.ToSingle(txtPriceRate.Text);
                _selectedSellerNewProduct.TotalPrice = amount*priceRate;
                _selectedSellerNewProduct.Date = Convert.ToDateTime(dtpDate.Text);
                _selectedSellerNewProduct.ProductCategoryId = Int32.Parse(ddlCategory.SelectedValue.ToString());
                _selectedSellerNewProduct.Time = txtTime.Text;

                bool isNew = _selectedSellerNewProduct.ID == 0;

                string error;
                _selectedSellerNewProduct=_sellerNewProductBl.Save(_selectedSellerNewProduct, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _sellerNewProducts.Add(_selectedSellerNewProduct);
                }
                else
                {
                    _sellerNewProducts[_selectedIndex] = _selectedSellerNewProduct;
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
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Name..!!!");
                txtName.Focus();
                return false;
            }
           
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Password..!!!");
                txtProductName.Focus();
                return false;
            }
           
            if (string.IsNullOrWhiteSpace(txtPriceRate.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Phone Number..!!!");
                txtPriceRate.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid organization id..!!!");
                txtDiscount.Focus();
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
            for (int i = 0; i < _sellerNewProducts.Count; i++)
            {
                string error;
                if (_sellerNewProductBl.Delete(_sellerNewProducts[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            OtherUserHomeForm ud = new OtherUserHomeForm();
            
            this.Hide();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh1_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            this.LoadSellerNewProductManagers();
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            if (_selectedSellerNewProduct.ID == 0)
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
            if (_sellerNewProductBl.Delete(_selectedSellerNewProduct.ID, out error) == false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _sellerNewProducts.Remove(_selectedSellerNewProduct);
            this.RefreshDgv();
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!");
            this.Init();
        }

        private void btnLogout1_Click(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            this.Hide();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadSellerNewProductManagers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedSellerNewProduct.ID == 0)
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
            if (_sellerNewProductBl.Delete(_selectedSellerNewProduct.ID, out error) == false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _sellerNewProducts.Remove(_selectedSellerNewProduct);
            this.RefreshDgv();
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!");
            this.Init();
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                btnSearch.PerformClick();

            }
            if (e.KeyCode == Keys.Left)
            {
                btnRefresh1.Focus();
            }
            if (e.KeyCode == Keys.Right)
            {
                btnSearch.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnRefresh1.PerformClick();
            }
        }
    }
}
