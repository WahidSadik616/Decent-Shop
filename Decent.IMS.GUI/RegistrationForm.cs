using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decent.IMS.Data;
using Decent.IMS.Framework;

namespace Decent.IMS.GUI
{
    public partial class RegistrationForm : MetroFramework.Forms.MetroForm
    {
        public RegistrationForm()
        {
          
            InitializeComponent();
            this.ActiveControl = txtName;
            txtName.Focus();
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;
            try
            {
                SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=DecentDb;Integrated Security=True");
                connection.Open();
                string query = "Insert into UserInfoes(Name,Password,Email,Phone,Gender,OrgId,StatusId,TypeId,DepartmentId)" +
                                "values('" + txtName.Text + "','" + txtPassword.Text + "','" + txtEmail.Text + "','" + txtPhone.Text + "','" + ddlGender.SelectedItem + "','" + txtOrgId.Text + "'," + (int)EnumCollection.UserStatusEnum.Pending + "," + (int)EnumCollection.UserTypeEnum.Other + ",'2')";
                // MetroFramework.MetroMessageBox.Show(this, query);
                SqlCommand command = new SqlCommand(query, connection);
                int row = command.ExecuteNonQuery();

                if (row > 0)
                {
                    MetroFramework.MetroMessageBox.Show(this, "You are in pending...please wait or contact with administrator!!!");
                    LoginForm lf = new LoginForm();
                    lf.Show();
                    this.Hide();
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Something is wrong....!!!!");
                }
            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, exception.Message);
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
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Email..!!!");
                txtEmail.Focus();
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                
                try
                {
                   DecentDbEntities _context = new DecentDbEntities();
                    var userInfoEmail = _context.UserInfoes.FirstOrDefault(u => u.Email == txtEmail.Text);
                    string email = txtEmail.Text;
                    if (!email.Contains("@gmail.com"))
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Invalid Email..!!!");
                        txtEmail.Focus();
                        return false;
                    }

                    if (userInfoEmail != null)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Email Id is already exist...!!!");
                        txtEmail.Focus();
                        return false;

                    }
                    
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                }
            }
            
                
                
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Phone..!!!");
                txtPhone.Focus();
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                DecentDbEntities _context = new DecentDbEntities();
                var userInfoPhone = _context.UserInfoes.FirstOrDefault(u => u.Phone == txtPhone.Text);
                if (userInfoPhone != null)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Phone number is already exist...!!!");
                    txtPhone.Focus();
                    return false;
                }
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
                        return false;

                    }
                    if ((a == "0" && b == "1" && (c == "7" || c == "8" || c == "9" || c == "5" || c == "6" || c == "3"))
                        || phone.Length == 11 || !(phone.Any(ch => ch < '0' || ch > '9')))
                    {
                        txtOrgId.Focus();
                    }


                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                }
            }
            
               
            if (string.IsNullOrWhiteSpace(txtOrgId.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Organization Id..!!!");
                txtOrgId.Focus();
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Password..!!!");
                txtPassword.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtConPass.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Confirm Password..!!!");
                txtConPass.Focus();
                return false;
            }
            if (txtPassword.Text != txtConPass.Text)
            {
                MetroFramework.MetroMessageBox.Show(this, "Password and Confirm Password should be matched..!!!");
                txtConPass.Focus();
                return false;
            }
            
            if (ddlGender.SelectedItem == null)
            {
                MetroFramework.MetroMessageBox.Show(this, "please select your Gender..!!!");
                ddlGender.Focus();
                return false;
            }

            
            return true;
        }
        
        private void metroButton2_Click(object sender, EventArgs e)
        {
            LoginForm lf=new LoginForm();
            lf.Show();
            this.Hide();
        }

       
        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtPassword.Text)) || e.KeyCode == Keys.Down)
            {
                txtConPass.Focus();
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                txtOrgId.Focus();
            }
        }

       
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                try
                {
                    DecentDbEntities _context = new DecentDbEntities();
                    var userInfoEmail = _context.UserInfoes.FirstOrDefault(u => u.Email == txtEmail.Text);
                    string email = txtEmail.Text;
                    if (!email.Contains("@gmail.com"))
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Invalid Email..!!!");
                        txtEmail.Focus();
                    }

                    if (userInfoEmail != null)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Email Id is already exist...!!!");
                        txtEmail.Focus();

                    }
                    if (email.Contains("@gmail.com") && userInfoEmail == null)
                    {
                        txtPhone.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                }
                
                
            }
            if (e.KeyCode == Keys.Up)
            {
                txtName.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                txtPhone.Focus();
            }
        }


        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                DecentDbEntities _context = new DecentDbEntities();
                var userInfoPhone = _context.UserInfoes.FirstOrDefault(u => u.Phone == txtPhone.Text);
                if (userInfoPhone != null)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Phone number is already exist...!!!");
                    txtPhone.Focus();
                    return;
                }
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
                    if ((a == "0" && b == "1" && (c == "7" || c == "8" || c == "9" || c == "5" || c == "6" || c == "3"))
                        || phone.Length == 11 || !(phone.Any(ch => ch < '0' || ch > '9')))
                    {
                        txtOrgId.Focus();
                    }
                    
                    
                }
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid Input");
                }
               
            }

            if (e.KeyCode == Keys.Up)
            {
                txtEmail.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                txtOrgId.Focus();
            }

        }

        private void txtOrgId_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                txtPhone.Focus();
            }
        }

        private void txtOrgId_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtOrgId.Text)) || e.KeyCode == Keys.Down)
            {
                txtPassword.Focus();
            }
        }

        

        private void ddlGender_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ddlGender.DroppedDown = true;
            }

            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(ddlGender.Text))
            {
                btnSignIn.PerformClick();
            }
        }

        private void btnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            
            
            
        }

        private void btnCancel_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void btnSignIn_KeyUp(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.Up)
            {
                txtConPass.Focus();
            }*/
        }

        private void txtConPass_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtConPass.Text))
            {
                if (txtPassword.Text != txtConPass.Text)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Password and Confirm Password should be matched..!!!");
                    txtConPass.Focus();
                }
                else
                {
                    ddlGender.Focus();
                    ddlGender.DroppedDown = true;
                }
                
            }
            if (e.KeyCode == Keys.Down)
            {
                ddlGender.Focus();
            }
            
        }

        private void txtConPass_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                txtPassword.Focus();
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtName.Text)))
            {
                txtEmail.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                txtEmail.Focus();
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

        private void txtOrgId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8 && ch !=45)
            {
                e.Handled = true;
            }
        }

       
    }
}
