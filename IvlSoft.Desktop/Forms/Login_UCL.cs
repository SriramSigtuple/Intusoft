using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using INTUSOFT.Desktop.Properties;
using Common;

namespace INTUSOFT.Desktop.Forms
{
    public partial class Login_UCL : UserControl
    {
        public Login_UCL()
        {
            InitializeComponent();
            InitializeResourceString();
            this.SetStyle(ControlStyles.Selectable, false);
        }
        public delegate void loggedIn(string s, EventArgs e);
        public event loggedIn _loggedIn;
        EventArgs e = null;

        /// <summary>
        /// Click event to handle the login process
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event data</param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (txtUsername.Text.Trim() == INTUSOFT.Desktop.Properties.Settings.Default.Username && txtPassword.Text.Trim() == INTUSOFT.Desktop.Properties.Settings.Default.Password)
            {
               // frmManage.Show();

                this.Hide();
                _loggedIn("Logged in", e);
                
            }
            else
            {
                Common.CustomMessageBox.Show(
                    IVLVariables.LangResourceManager.GetString("Login_Validation_Message", IVLVariables.LangResourceCultureInfo),
                    IVLVariables.LangResourceManager.GetString("Login_Validation_Message_Title", IVLVariables.LangResourceCultureInfo),
                    Common.CustomMessageBoxButtons.OK,
                    Common.CustomMessageBoxIcon.Information);
            }
            this.Cursor = Cursors.Default;
        }
        private void InitializeResourceString()
        {
            lblUserName.Text = IVLVariables.LangResourceManager.GetString( "Login_Username_Label_Text",IVLVariables.LangResourceCultureInfo);
            lblPassword.Text = IVLVariables.LangResourceManager.GetString( "Login_Password_Label_Text",IVLVariables.LangResourceCultureInfo);;
            btnLogin.Text = IVLVariables.LangResourceManager.GetString("Login_Login_Button_Text", IVLVariables.LangResourceCultureInfo); ;
        }

       
    }
}
