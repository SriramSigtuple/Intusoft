using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.Custom.Controls;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Repository;
using Common;

namespace INTUSOFT.Desktop.Forms
{
    public partial class RegisterUserWindow : BaseGradientForm
    {
        CustomMessageBox messageBox;
        bool isValidUserName = false;

        public bool IsValidUserName
        {
            get => isValidUserName;
            set
            {
                isValidUserName = value;
                txtPassword.Enabled = isValidUserName;

            }
        }

        public RegisterUserWindow()
        {
            InitializeComponent();
            messageBox = new CustomMessageBox();
            this.Color1 = IVLVariables.GradientColorValues.Color1;
            this.Color2 = IVLVariables.GradientColorValues.Color2;
            this.ColorAngle = IVLVariables.GradientColorValues.ColorAngle;
            this.FontColor = IVLVariables.GradientColorValues.FontForeColor;
            txtUsername.TextChanged += TxtUsername_TextChanged;
            UpdateFontForeColor();
            userExist_lbl.Visible = false;
        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {
            if (NewDataVariables.Users.Where(x => x.username == txtUsername.Text).Any())
            {
                userExist_lbl.Visible = true;
                userExist_lbl.Text = "Username Exists";
                userExist_lbl.ForeColor = Color.Red;
                IsValidUserName = false;
                CustomMessageBox.Show("User Name Already Exists", "Register New User", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Error);

            }
            else
            {
                userExist_lbl.Visible = true;
                userExist_lbl.Text = "Username Available";
                userExist_lbl.ForeColor = Color.Green;
                IsValidUserName = true;
            }
        }

        private void UpdateFontForeColor()
        {
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {

                if (c is TextBox || c is Button)
                {
                    c.ForeColor = Color.Black;
                    c.Refresh();
                }
                

            }

        }

        private void BtnSaveUser_Click(object sender, EventArgs e)
        {
            if (IsValidUserName)
            {
                users user = users.CreateNewUsers();
                user.userId = 1;
                users newUser = new users();
                newUser.person = new Person();
                newUser.person.createdBy = user;
                newUser.createdBy = user;
                newUser.username = txtUsername.Text;
                newUser.password = txtPassword.Text.GetMd5Hash();
                newUser.systemId = "operator" + (NewDataVariables.Users.Count + 1).ToString();
                NewIVLDataMethods.AddUser(newUser);
                this.Close();
            }
            else
                CustomMessageBox.Show("User Name Already Exists, Please try different user name", "Register New User", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Error);

        }

        private void TxtUsername_Leave(object sender, EventArgs e)
        {
          

        }

        

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != null && txtPassword.Text.Length >= 6)
                btnSaveUser.Enabled = true;
            else
                btnSaveUser.Enabled = false;
        }
    }
}
