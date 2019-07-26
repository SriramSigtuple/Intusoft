// -----------------------------------------------------------------------
// <copyright file="Login.cs" company="John">
// Socia Member club Demo ©2013
// </copyright>
// -----------------------------------------------------------------------

namespace INTUSOFT.Desktop
{
    using System;
    using System.Windows.Forms;
    using INTUSOFT.Desktop.Properties;
    using INTUSOFT.Desktop.Forms;
    using Common;

    /// <summary>
    /// Login form
    /// </summary>
    public partial class Login : Form
    {
        /// <summary>
        /// Initializes a new instance of the Login class
        /// </summary>
        public Login()
        {
            this.InitializeComponent();
            this.InitializeResourceString();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //this.TopMost = true;//has been commented to prevent the custom message box getting background.
        }

        void frmManage_thisClosed(string a, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Initializes resource strings
        /// </summary>
        private void InitializeResourceString()
        {
            lblUserName.Text = IVLVariables.LangResourceManager.GetString( "Login_Username_Label_Text",IVLVariables.LangResourceCultureInfo);
            lblPassword.Text = IVLVariables.LangResourceManager.GetString( "Login_Password_Label_Text",IVLVariables.LangResourceCultureInfo);
            btnLogin.Text = IVLVariables.LangResourceManager.GetString("Login_Login_Button_Text", IVLVariables.LangResourceCultureInfo);
        }

        /// <summary>
        /// Click event to handle the login process
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event data</param>
        private void Login_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == INTUSOFT.Desktop.Properties.Settings.Default.Username && txtPassword.Text.Trim() == INTUSOFT.Desktop.Properties.Settings.Default.Password)
            {
                this.Hide();
            }
            else
            {
                Common.CustomMessageBox.Show(
                    IVLVariables.LangResourceManager.GetString( "Login_Validation_Message",IVLVariables.LangResourceCultureInfo),
                    IVLVariables.LangResourceManager.GetString( "Login_Validation_Message_Title",IVLVariables.LangResourceCultureInfo),
                    Common.CustomMessageBoxButtons.OK,
                    Common.CustomMessageBoxIcon.Information);
            }
        }

    }
}
