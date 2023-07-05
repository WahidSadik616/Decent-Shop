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
    public partial class SupplierPaymentManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        SupplierPaymentDetailsBL _paymentDetailsBl=new SupplierPaymentDetailsBL();
        List<PaymentDetail> _paymentDetails= new List<PaymentDetail>();
        private PaymentDetail _selectedPaymentDetails = null;
        private int _selectedIndex = 0;

        public SupplierPaymentManager()
        {
            InitializeComponent();
        }

        private void PaymentDetailsManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
                txtSearch.Text = "";

                this.LoadPaymentDetailsManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadPaymentDetailsManagers()
        {
            _paymentDetails = _paymentDetailsBl.GetAll(txtSearch.Text);

            if (_paymentDetails.Count > 0)
            {
                _selectedPaymentDetails = _paymentDetails[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedPaymentDetails = new PaymentDetail()
                {
                    Date = DateTime.Now
                };

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvPaymentDetailsList.AutoGenerateColumns = false;
            dgvPaymentDetailsList.DataSource = _paymentDetails.ToList();
            dgvPaymentDetailsList.Refresh();

            dgvPaymentDetailsList.ClearSelection();

            for (int i = 0; i < dgvPaymentDetailsList.Rows.Count; i++)
            {
                if (dgvPaymentDetailsList.Rows[i].Cells[0].Value.ToString() == _selectedPaymentDetails.ID.ToString())
                {
                    dgvPaymentDetailsList.Rows[i].Selected = true;
                    dgvPaymentDetailsList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvPaymentDetailsList.Rows.Count; i++)
            {
                dgvPaymentDetailsList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedPaymentDetails.ID.ToString();
            dtpDate.Text = _selectedPaymentDetails.Date.ToString();
            txtSellerName.Text = _selectedPaymentDetails.SellerName;
            txtPhone.Text = _selectedPaymentDetails.Phone;
            txtTotalDue.Text = Convert.ToString(_selectedPaymentDetails.TotalDue);
            txtPayment.Text = Convert.ToString(_selectedPaymentDetails.Payment);
           
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadPaymentDetailsManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
               _selectedPaymentDetails = _paymentDetails[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedPaymentDetails = new PaymentDetail()
            {
                Date = DateTime.Now
            };
            this.Populate();
            dgvPaymentDetailsList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedPaymentDetails.ID == 0)
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
            if (_paymentDetailsBl.Delete(_selectedPaymentDetails.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }
          
            _paymentDetails.Remove(_selectedPaymentDetails);
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
                var seller=_context.Sellers.FirstOrDefault(u => u.Name == txtSellerName.Text);
                if (seller != null)
                {
                    _selectedPaymentDetails.Date = Convert.ToDateTime(dtpDate.Text);
                    _selectedPaymentDetails.SellerName = txtSellerName.Text;
                    _selectedPaymentDetails.Phone = txtPhone.Text;
                    _selectedPaymentDetails.TotalDue = seller.Due;
                    _selectedPaymentDetails.Payment = Convert.ToSingle(txtPayment.Text);


                    bool isNew = _selectedPaymentDetails.ID == 0;

                    string error;
                    _selectedPaymentDetails = _paymentDetailsBl.Save(_selectedPaymentDetails, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    this.Populate();

                    if (isNew)
                    {
                        _paymentDetails.Add(_selectedPaymentDetails);
                    }
                    else
                    {
                        _paymentDetails[_selectedIndex] = _selectedPaymentDetails;
                    }
                    MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
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
            if (string.IsNullOrWhiteSpace(txtSellerName.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Name..!!!");
                txtSellerName.Focus();
                return false;
            }
           
            if (string.IsNullOrWhiteSpace(txtPayment.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Password..!!!");
                txtPayment.Focus();
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
            for (int i = 0; i < _paymentDetails.Count; i++)
            {
                string error;
                if (_paymentDetailsBl.Delete(_paymentDetails[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            SupplierPaymentManager pm = new SupplierPaymentManager();
            pm.Show();
            this.Hide();
        }


    }
}
