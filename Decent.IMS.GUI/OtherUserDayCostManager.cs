using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decent.IMS.BL;
using Decent.IMS.Data;

namespace Decent.IMS.GUI
{
    public partial class OtherUserDayCostManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        DayCostBL _dayCostBl=new DayCostBL();
        List<DayCost> _dayCosts= new List<DayCost>();
        SalesmanPaymentBL _salesmanPaymentBl = new SalesmanPaymentBL();
        List<SalesmanPayment> _salesmanPayments = new List<SalesmanPayment>();
        SalesmanBL _salesmanBl = new SalesmanBL();
        List<Salesman> _salesmans = new List<Salesman>();
        CostDetailsBL _costDetailsBl = new CostDetailsBL();
        List<CostDetail> _costDetails = new List<CostDetail>();
        private DayCost _selectedDayCost = null;
        private Salesman _selectedSalesman = null;
        private SalesmanPayment _selectedSalesmanPayment = null;
        private CostDetail _selectedCostDetails = null;
        private int _selectedIndex = 0;

        public OtherUserDayCostManager()
        {
            InitializeComponent();
        }

        private void DayCostManager_Load(object sender, EventArgs e)
        {
           
            
            this.Init();
        }

        private void Init()
        {
            try
            {
                

                lblPurpose.Hide();
                txtPurpose.Hide();
                lblSalesmanName.Hide();
                txtSalesman.Hide();
                txtDue.Hide();
                lblDue.Hide();

                txtSearch.Text = "";
                txtSalesman.Text = "";
                ddlCostName.Text = "";
                txtDue.Text = "";

                this.LoadDayCostManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadDayCostManagers()
        {
            _dayCosts = _dayCostBl.GetAll(txtSearch.Text);
            _salesmans = _salesmanBl.GetAll(txtSearch.Text);

            if (_dayCosts.Count > 0)
            {
                _selectedDayCost = new DayCost();
                dgvDayCostList1.ClearSelection();
                _selectedDayCost = _dayCosts[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedDayCost=new DayCost();

            }

            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvDayCostList1.AutoGenerateColumns = false;
            dgvDayCostList1.DataSource = _dayCosts.ToList();
            dgvDayCostList1.Refresh();

            dgvDayCostList1.ClearSelection();

            for (int i = 0; i < dgvDayCostList1.Rows.Count; i++)
            {
                if (dgvDayCostList1.Rows[i].Cells[0].Value.ToString() == _selectedDayCost.ID.ToString())
                {
                    dgvDayCostList1.Rows[i].Selected = true;
                    dgvDayCostList1.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvDayCostList1.Rows.Count; i++)
            {
                dgvDayCostList1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {

            ddlCostName.Text = "";
            txtCostAmount.Text = "";

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadDayCostManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedDayCost = _dayCosts[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedDayCost=new DayCost();
            this.Populate();
            dgvDayCostList1.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedDayCost.ID == 0)
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
            if (_dayCostBl.Delete(_selectedDayCost.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }
         
            _dayCosts.Remove(_selectedDayCost);
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
                

                _selectedDayCost.Name = ddlCostName.Text;

                if (_selectedDayCost.Name=="others")
                {
                    lblPurpose.Show();
                    txtPurpose.Show();
                    

                    if (string.IsNullOrWhiteSpace(txtPurpose.Text))
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Give the purpose name please...!!!");
                        txtPurpose.Focus();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtCostAmount.Text))
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Cost amount please..!!!");
                        txtCostAmount.Focus();
                        return;
                    }

                    double costAmount;
                    if (!double.TryParse(txtCostAmount.Text, out costAmount) || Convert.ToSingle(txtCostAmount.Text) <= 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Invalid cost amount...!!!");
                        txtCostAmount.Focus();
                        return;
                    }


                    _selectedDayCost = new DayCost();
                    

                    _selectedDayCost.Name = txtPurpose.Text;

                    _selectedDayCost.CostAmount = Convert.ToSingle(txtCostAmount.Text);


                    string error;
                    _selectedDayCost = _dayCostBl.Save(_selectedDayCost, out error);

                    if (string.IsNullOrEmpty(error) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error);
                        return;
                    }

                    _dayCosts.Add(_selectedDayCost);










                    _selectedCostDetails = new CostDetail()
                    {
                         Date = DateTime.Now
                    };

                    _selectedCostDetails.Date = Convert.ToDateTime(dtpDateTime.Text);
                    _selectedCostDetails.Name = txtPurpose.Text;
                    _selectedCostDetails.Amount = Convert.ToSingle(txtCostAmount.Text);



                    string error1;
                    _selectedCostDetails = _costDetailsBl.Save(_selectedCostDetails, out error1);

                    if (string.IsNullOrEmpty(error1) == false)
                    {
                        MetroFramework.MetroMessageBox.Show(this, error1);
                        return;
                    }


                   








                        if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                  DialogResult.No)
                        {
                            return;
                        }
                   
                    MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
                   
                   
                   
                   
                   OtherUserDayCostManager a = new OtherUserDayCostManager();
                   a.Show();
                   this.Hide();
                   

                    
                }
                else if (_selectedDayCost.Name == "salesman salary")
                {
                    
                    lblSalesmanName.Show();
                    txtSalesman.Show();
                    
                    if (string.IsNullOrWhiteSpace(txtSalesman.Text))
                    {
                        MetroFramework.MetroMessageBox.Show(this, "salesman name please...!!!");
                        txtSalesman.Focus();
                        return;
                    }

                    var salesman=_context.Salesmen.FirstOrDefault(u => u.Name == txtSalesman.Text);
                    if (salesman == null)
                    {
                        MetroFramework.MetroMessageBox.Show(this, " no found...!!!");
                        txtSalesman.Focus();
                        return;
                    }
                    txtDue.Show();
                    lblDue.Show();
                    txtDue.Text = Convert.ToString(salesman.DueSalary);
                    if (string.IsNullOrWhiteSpace(txtCostAmount.Text))
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Cost amount please..!!!");
                        txtCostAmount.Focus();
                        return;
                    }

                    double costAmount;
                    if (!double.TryParse(txtCostAmount.Text, out costAmount) || Convert.ToSingle(txtCostAmount.Text) <= 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Invalid cost amount...!!!");
                        txtCostAmount.Focus();
                        return;
                    }




                   
                    for (int i = 0; i < _salesmans.Count; i++)
                    {
                        if (_salesmans[i].Name == txtSalesman.Text)
                        {
                            double salary = Convert.ToSingle(txtCostAmount.Text);
                            if (_salesmans[i].DueSalary >= salary)
                            {

                                _salesmans[i].PaySalary += salary;

                                string error;
                                _selectedSalesman = _salesmanBl.Save(_salesmans[i], out error);

                                if (string.IsNullOrEmpty(error) == false)
                                {
                                    MetroFramework.MetroMessageBox.Show(this, error);
                                    return;
                                }

                                _salesmans.Add(_selectedSalesman);

                                _selectedDayCost.Name = txtSalesman.Text;

                                _selectedDayCost.CostAmount = Convert.ToSingle(txtCostAmount.Text);


                                string error1;
                                _selectedDayCost = _dayCostBl.Save(_selectedDayCost, out error1);

                                if (string.IsNullOrEmpty(error) == false)
                                {
                                    MetroFramework.MetroMessageBox.Show(this, error);
                                    return;
                                }


                                _dayCosts.Add(_selectedDayCost);







                                _selectedSalesmanPayment = new SalesmanPayment()
                                {
                                    Date = DateTime.Now
                                };
                                _selectedSalesmanPayment.Date = Convert.ToDateTime(dtpDateTime.Text);
                                _selectedSalesmanPayment.Name = salesman.Name;
                                _selectedSalesmanPayment.Phone = salesman.Phone;
                                _selectedSalesmanPayment.TotalDue = salesman.DueSalary;
                                _selectedSalesmanPayment.Payment = Convert.ToSingle(txtCostAmount.Text);



                                string error2;
                                _selectedSalesmanPayment = _salesmanPaymentBl.Save(_selectedSalesmanPayment, out error2);

                                if (string.IsNullOrEmpty(error2) == false)
                                {
                                    MetroFramework.MetroMessageBox.Show(this, error2);
                                    return;
                                }
                                _salesmanPayments.Add(_selectedSalesmanPayment);







                                _selectedCostDetails = new CostDetail()
                                {
                                    Date = DateTime.Now
                                };

                                _selectedCostDetails.Date = Convert.ToDateTime(dtpDateTime.Text);
                                _selectedCostDetails.Name = txtSalesman.Text;
                                _selectedCostDetails.Amount = Convert.ToSingle(txtCostAmount.Text);



                                string error3;
                                _selectedCostDetails = _costDetailsBl.Save(_selectedCostDetails, out error3);

                                if (string.IsNullOrEmpty(error3) == false)
                                {
                                    MetroFramework.MetroMessageBox.Show(this, error3);
                                    return;
                                }


                                _costDetails.Add(_selectedCostDetails);





                                if (
                                    MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation",
                                        MessageBoxButtons.YesNo) ==
                                    DialogResult.No)
                                {
                                    return;
                                }



                                MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");

                                OtherUserDayCostManager a = new OtherUserDayCostManager();
                                a.Show();
                                this.Hide();


                                break;

                            }
                            else
                            {
                                MetroFramework.MetroMessageBox.Show(this, "You have to update salesman salary..!!!");
                                break;
                                
                            }
                        }

                    }

                }



                else 
                     {
                         
                         double p=Convert.ToSingle(txtCostAmount.Text);
                         
                         if (string.IsNullOrWhiteSpace(txtCostAmount.Text))
                         {
                             MetroFramework.MetroMessageBox.Show(this, "Cost amount please..!!!");
                             txtCostAmount.Focus();
                             return;
                         }

                         double costAmount;
                         if (!double.TryParse(txtCostAmount.Text, out costAmount) ||
                             Convert.ToSingle(txtCostAmount.Text) <= 0)
                         {
                             MetroFramework.MetroMessageBox.Show(this, "Invalid cost amount...!!!");
                             txtCostAmount.Focus();
                             return;
                         }

                         
                         
                         _selectedDayCost.CostAmount = Convert.ToSingle(txtCostAmount.Text);


                         bool isNew = _selectedDayCost.ID == 0;

                         string error;
                         _selectedDayCost = _dayCostBl.Save(_selectedDayCost, out error);

                         if (string.IsNullOrEmpty(error) == false)
                         {
                             MetroFramework.MetroMessageBox.Show(this, error);
                             return;
                         }

                         this.Populate();

                         if (isNew)
                         {
                             _dayCosts.Add(_selectedDayCost);
                         }
                         else
                         {
                             _dayCosts[_selectedIndex] = _selectedDayCost;
                         }







                         _selectedCostDetails = new CostDetail()
                         {
                             Date = DateTime.Now
                         };

                         _selectedCostDetails.Date = Convert.ToDateTime(dtpDateTime.Text);
                         _selectedCostDetails.Name = ddlCostName.Text;
                         _selectedCostDetails.Amount = p;



                         string error3;
                         _selectedCostDetails = _costDetailsBl.Save(_selectedCostDetails, out error3);

                         if (string.IsNullOrEmpty(error3) == false)
                         {
                             MetroFramework.MetroMessageBox.Show(this, error3);
                             return;
                         }


                         _costDetails.Add(_selectedCostDetails);







                         if (
                                  MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation",
                                      MessageBoxButtons.YesNo) ==
                                  DialogResult.No)
                         {
                             return;
                         }
                         MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");




                         OtherUserDayCostManager a = new OtherUserDayCostManager();
                         a.Show();
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
           
            
            if (ddlCostName.SelectedItem == null)
            {
                MetroFramework.MetroMessageBox.Show(this, "select the cost name please..!!!");
                ddlCostName.Focus();
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

        private void metroButton6_Click_1(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Are You Sure??", "Confirmation", MessageBoxButtons.YesNo) ==
                DialogResult.No)
            {
                return;
            }
            for (int i = 0; i < _dayCosts.Count; i++)
            {
                string error;
                if (_dayCostBl.Delete(_dayCosts[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            OtherUserDayCostManager pm = new OtherUserDayCostManager();
            pm.Show();
            this.Hide();
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void dtpDateTime_ValueChanged(object sender, EventArgs e)
        {

        }

     
    
    }
}
