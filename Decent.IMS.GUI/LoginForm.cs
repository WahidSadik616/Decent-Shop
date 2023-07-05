using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decent.IMS.Data;
using Decent.IMS.Framework;

namespace Decent.IMS.GUI
{
    public partial class LoginForm : MetroFramework.Forms.MetroForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (!isValid())
                return;
            try
            {
              
                DecentDbEntities _context = new DecentDbEntities();
                var userInfo =
                    _context.UserInfoes.FirstOrDefault(u => u.Name == txtUserName.Text && u.Password == txtPassword.Text);

                if (userInfo == null)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Invalid User Name or Password..!!!");
                    txtPassword.Focus();
                    return;
                }
                if (userInfo.StatusId == (int)EnumCollection.UserStatusEnum.Pending)
                {
                    MetroFramework.MetroMessageBox.Show(this, "You are in pending...please wait or contact with administrator!!!");
                    return;
                }
                if (userInfo.StatusId == (int)EnumCollection.UserStatusEnum.Reject)
                {
                    MetroFramework.MetroMessageBox.Show(this, "You are rejected...please contact with administrator!!!");
                    return;
                }
                if (userInfo.TypeId == (int)EnumCollection.UserTypeEnum.Admin)
                {
                    AdminForm af = new AdminForm();
                    af.Show();
                }
                else
                {
                    OtherUserHomeForm of = new OtherUserHomeForm();
                    of.Show();
                }
                this.Hide();

            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, exception.Message);
            }
            

        }

        private bool isValid()
        {
            return true;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            RegistrationForm rf=new RegistrationForm();
            rf.Show();
            this.Hide();
        }

        

       
        private void LoginForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Confirm to close", "Exit", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (confirm == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtUserName.Text)) || e.KeyCode == Keys.Down)
            {
                txtPassword.Focus();
            }
        }

        private void txtUserName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                btnSignUp.Focus();
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                txtUserName.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                btnLogIn.Focus();
            }
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                btnLogIn.PerformClick();
            }
        }
    }
}
