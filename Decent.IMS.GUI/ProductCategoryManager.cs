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
    public partial class ProductCategoryManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        ProductCategoryBL _productCategoryBl=new ProductCategoryBL();
        List<ProductCategory> _productCategorys= new List<ProductCategory>();
        private ProductCategory _selectedProductCategory = null;
        private int _selectedIndex = 0;

        public ProductCategoryManager()
        {
            InitializeComponent();
        }

        private void ProductCategoryManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                txtSearch.Text = "";

                this.LoadProductCategoryManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadProductCategoryManagers()
        {
            _productCategorys = _productCategoryBl.GetAll(txtSearch.Text);

            if (_productCategorys.Count > 0)
            {
                _selectedProductCategory = _productCategorys[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedProductCategory=new ProductCategory();

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvProductCategoryList.AutoGenerateColumns = false;
            dgvProductCategoryList.DataSource = _productCategorys.ToList();
            dgvProductCategoryList.Refresh();

            dgvProductCategoryList.ClearSelection();

            for (int i = 0; i < dgvProductCategoryList.Rows.Count; i++)
            {
                if (dgvProductCategoryList.Rows[i].Cells[0].Value.ToString() == _selectedProductCategory.ID.ToString())
                {
                    dgvProductCategoryList.Rows[i].Selected = true;
                    dgvProductCategoryList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvProductCategoryList.Rows.Count; i++)
            {
                dgvProductCategoryList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedProductCategory.ID.ToString();
            txtName.Text = _selectedProductCategory.Name;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadProductCategoryManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedProductCategory = _productCategorys[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedProductCategory=new ProductCategory();
            this.Populate();
            dgvProductCategoryList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedProductCategory.ID == 0)
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
            if (_productCategoryBl.Delete(_selectedProductCategory.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _productCategorys.Remove(_selectedProductCategory);
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
                _selectedProductCategory.Name = txtName.Text;

                bool isNew = _selectedProductCategory.ID == 0;

                string error;
                _selectedProductCategory=_productCategoryBl.Save(_selectedProductCategory, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _productCategorys.Add(_selectedProductCategory);
                }
                else
                {
                    _productCategorys[_selectedIndex] = _selectedProductCategory;
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
                MetroFramework.MetroMessageBox.Show(this, "Give the product category name please...!!!");
                txtName.Focus();
                return false;
            }
            return true;
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            LoginForm lf=new LoginForm();
            lf.Show();
            this.Hide();
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            ProductDetailForm pd=new ProductDetailForm();
            pd.Show();
            this.Hide();
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
               DialogResult.No)
            {
                return;
            }
            for (int i = 0; i < _productCategorys.Count; i++)
            {
                string error;
                if (_productCategoryBl.Delete(_productCategorys[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            ProductCategoryManager pm = new ProductCategoryManager();
            pm.Show();
            this.Hide();
        }


    }
}
