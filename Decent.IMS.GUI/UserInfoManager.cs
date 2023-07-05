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
    public partial class UserInfoManager : MetroFramework.Forms.MetroForm
    {
        DecentDbEntities _context=new DecentDbEntities();
        UserInfoBL _userInfoBl=new UserInfoBL();
        List<UserInfo> _userInfos= new List<UserInfo>();
        private UserInfo _selectedUserInfo = null;
        private int _selectedIndex = 0;

        public UserInfoManager()
        {
            InitializeComponent();
        }

        private void UserInfoManager_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            try
            {
               ddlType.DataSource = EnumCollection.UserTypeList();
                ddlType.DisplayMember = "Name";
                ddlType.ValueMember = "ID";

                ddlStatus.DataSource = EnumCollection.UserStatusList();
                ddlStatus.DisplayMember = "Name";
                ddlStatus.ValueMember = "ID";

               ddlDepartment.DataSource = _context.Departments.ToList();
                ddlDepartment.DisplayMember = "Name";
                ddlDepartment.ValueMember = "ID";

                txtSearch.Text = "";

                this.LoadUserManagers();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void LoadUserManagers()
        {
            _userInfos = _userInfoBl.GetAll(txtSearch.Text);

            if (_userInfos.Count > 0)
            {
                _selectedUserInfo = _userInfos[0];
                _selectedIndex = 0;

            }
            else
            {
                _selectedUserInfo=new UserInfo();

            }
           
            this.Populate();
            this.RefreshDgv();
           
        }

        private void RefreshDgv()
        {
            dgvUserInfoList.AutoGenerateColumns = false;
            dgvUserInfoList.DataSource = _userInfos.ToList();
            dgvUserInfoList.Refresh();

            dgvUserInfoList.ClearSelection();

            for (int i = 0; i < dgvUserInfoList.Rows.Count; i++)
            {
                if (dgvUserInfoList.Rows[i].Cells[0].Value.ToString() == _selectedUserInfo.ID.ToString())
                {
                    dgvUserInfoList.Rows[i].Selected = true;
                    dgvUserInfoList.Refresh();
                    break;
                }
            }
            for (int i = 0; i < dgvUserInfoList.Rows.Count; i++)
            {
                dgvUserInfoList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void Populate()
        {
            txtID.Text = _selectedUserInfo.ID.ToString();
            txtName.Text = _selectedUserInfo.Name;
            txtPassword.Text = _selectedUserInfo.Password;
            txtEmail.Text = _selectedUserInfo.Email;
            txtPhone.Text = _selectedUserInfo.Phone;
            txtOrgId.Text = _selectedUserInfo.OrgId;
            ddlGender.Text = _selectedUserInfo.Gender;
            ddlType.SelectedValue = _selectedUserInfo.TypeId;
            ddlStatus.SelectedValue = _selectedUserInfo.StatusId;
            ddlDepartment.SelectedValue = _selectedUserInfo.DepartmentId;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Init();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            this.LoadUserManagers();
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedUserInfo = _userInfos[e.RowIndex];
                _selectedIndex = e.RowIndex;
                this.Populate();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            _selectedUserInfo=new UserInfo();
            this.Populate();
            dgvUserInfoList.ClearSelection();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (_selectedUserInfo.ID == 0)
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
            if (_userInfoBl.Delete(_selectedUserInfo.ID, out error)==false)
            {
                MetroFramework.MetroMessageBox.Show(this, error);
                return;
            }

            _userInfos.Remove(_selectedUserInfo);
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
                _selectedUserInfo.Name = txtName.Text;
                _selectedUserInfo.Password = txtPassword.Text;
                _selectedUserInfo.Email = txtEmail.Text;
                _selectedUserInfo.Phone = txtPhone.Text;
                _selectedUserInfo.OrgId = txtOrgId.Text;
                _selectedUserInfo.Gender = ddlGender.Text;
                _selectedUserInfo.TypeId = Int32.Parse(ddlType.SelectedValue.ToString());
                _selectedUserInfo.StatusId = Int32.Parse(ddlStatus.SelectedValue.ToString());
                _selectedUserInfo.DepartmentId = Int32.Parse(ddlDepartment.SelectedValue.ToString());

                bool isNew = _selectedUserInfo.ID == 0;

                string error;
                _selectedUserInfo=_userInfoBl.Save(_selectedUserInfo, out error);

                if (string.IsNullOrEmpty(error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

                this.Populate();

                if (isNew)
                {
                    _userInfos.Add(_selectedUserInfo);
                }
                else
                {
                    _userInfos[_selectedIndex] = _selectedUserInfo;
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
            string email = txtEmail.Text;
            if (!email.Contains("@gmail.com"))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Email..!!!");
                txtEmail.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Password..!!!");
                txtPassword.Focus();
                return false;
            }
            
           /* if (string.IsNullOrWhiteSpace(txtConPass.Text))
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
            }*/
            
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid Phone Number..!!!");
                txtPhone.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtOrgId.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Invalid organization id..!!!");
                txtOrgId.Focus();
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

        private void metroButton6_Click(object sender, EventArgs e)
        {
            UserDetailForm ud=new UserDetailForm();
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
            for (int i = 0; i < _userInfos.Count; i++)
            {
                string error;
                if (_userInfoBl.Delete(_userInfos[i].ID, out error) == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, error);
                    return;
                }

            }
            MetroFramework.MetroMessageBox.Show(this, "Operation Completed..!!!");
            UserInfoManager pm = new UserInfoManager();
            pm.Show();
            this.Hide();
        }


    }
}
