using System;
using System.Linq;
using System.Windows.Forms;
using INTUSOFT.Data.Repository;
using INTUSOFT.Data.NewDbModel;
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
            this.Load += Login_UCL_Load;
           
        }

        private void Login_UCL_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            NewDataVariables.Users = NewDataVariables._Repo.GetAll<users>().ToList();
            txtUsername.Focus();
            
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
            Login();
        }
        private void InitializeResourceString()
        {
            lblUserName.Text = IVLVariables.LangResourceManager.GetString( "Login_Username_Label_Text",IVLVariables.LangResourceCultureInfo);
            lblPassword.Text = IVLVariables.LangResourceManager.GetString( "Login_Password_Label_Text",IVLVariables.LangResourceCultureInfo);;
            btnLogin.Text = IVLVariables.LangResourceManager.GetString("Login_Login_Button_Text", IVLVariables.LangResourceCultureInfo); ;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterUserWindow newUserRegister = new RegisterUserWindow();
            newUserRegister.ShowDialog();
        }


        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void LblPassword_Click(object sender, EventArgs e)
        {

        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void LblUserName_Click(object sender, EventArgs e)
        {

        }


        private bool CheckUserNamePassword()
        {
            var pw = txtPassword.Text.GetMd5Hash();
            if (NewDataVariables.Users.Where(x => x.username == txtUsername.Text && x.password == pw).Any())
            {
                NewDataVariables.Active_User = NewDataVariables.Users.Where(x => x.username == txtUsername.Text).FirstOrDefault();
                return true;
            }
            else
                return false;
        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
           //if( char.IsControl(e.KeyChar))
           // e.Handled = true;
        }


        public void Login()
        {
            this.Cursor = Cursors.WaitCursor;
            //if (txtUsername.Text.Trim() == INTUSOFT.Desktop.Properties.Settings.Default.Username && txtPassword.Text.Trim() == INTUSOFT.Desktop.Properties.Settings.Default.Password)
            //{
            //   // frmManage.Show();

            //    this.Hide();
            //    _loggedIn("Logged in", e);

            //}
            if (CheckUserNamePassword())
            {
                this.Hide();
                txtUsername.Clear();
                txtPassword.Clear();
                
                _loggedIn("Logged in", e);
                IVLVariables.pageDisplayed = PageDisplayed.Emr;
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

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
                e.Handled = true;
        }

        private void TxtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
                e.Handled = true;
        }
    }
}
