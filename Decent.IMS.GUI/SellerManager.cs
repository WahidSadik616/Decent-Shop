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
    public partial class SellerManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        SellerBL _sellerBl=new SellerBL();
        List<Seller> _sellers= new List<Seller>();
        private Seller _selectedSeller = null;
        private int _selectedIndex = 0;

        public SellerManager()
        {
            InitializeComponent();
        }

        private void SellerManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                

                txtSearch.Text = "";

                this.LoadSellerManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadSellerManagers()
        {
            _sellers = _sellerBl.GetAll(txtSearch.Text);

            if (_sellers.Count > 0)
            {
                _selectedSeller = _sellers[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedSeller=new Seller();

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvSellerList.AutoGenerateColumns = false;
            dgvSellerList.DataSource = _sellers.ToList();
            dgvSellerList.Refresh();

            dgvSellerList.ClearSelection();

            for (int i = 0; i < dgvSellerList.Rows.Count; i++)
            {
                if (dgvSellerList.Rows[i].Cells[0].Value.ToString() == _selectedSeller.ID.ToString())
                {
                    dgvSellerList.Rows[i].Selected = true;
                    dgvSellerList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvSellerList.Rows.Count; i++)
            {
                dgvSellerList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedSeller.ID.ToString();
            txtName.Text = _selectedSeller.Name;
            txtAddress.Text = _selectedSeller.Address;
            txtPhone.Text = _selectedSeller.Phone;
            txtEmail.Text = _selectedSeller.Email;
            txtTotalPrice.Text = Convert.ToString(_selectedSeller.TotalPrice);
            txtPayment.Text = Convert.ToString(_selectedSeller.Payment);
            
           
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadSellerManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedSeller = _sellers[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedSeller=new Seller();
            this.Populate();
            dgvSellerList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedSeller.ID == 0)
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
            if (_sellerBl.Delete(_selectedSeller.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }
         
            _sellers.Remove(_selectedSeller);
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
               
                _selectedSeller.Name = txtName.Text;
                _selectedSeller.Address = txtAddress.Text;
                _selectedSeller.Phone = txtPhone.Text;
                _selectedSeller.Email = txtEmail.Text;
                _selectedSeller.TotalPrice = Convert.ToSingle(txtTotalPrice.Text);
                _selectedSeller.Payment = Convert.ToSingle(txtPayment.Text);
                
               

                bool isNew = _selectedSeller.ID == 0;

                string error;
                _selectedSeller=_sellerBl.Save(_selectedSeller, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _sellers.Add(_selectedSeller);
                }
                else
                {
                    _sellers[_selectedIndex] = _selectedSeller;
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

           
            
            
           
            return true;
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            AdminForm ud = new AdminForm();
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
            for (int i = 0; i < _sellers.Count; i++)
            {
                string error;
                if (_sellerBl.Delete(_sellers[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }
               
            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            SellerManager pm = new SellerManager();
            pm.Show();
            this.Hide();
        }


    }
}
