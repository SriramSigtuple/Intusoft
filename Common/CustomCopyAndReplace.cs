using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Common
{
    public partial class CustomCopyAndReplace : Form
    {
        public static string copyReplaceBtn = string.Empty;
        public static string copyAndKeepReplaceBtn = string.Empty;
        public static string cancelBtnText = string.Empty;
        public static string dontCopyBtnText = string.Empty;
        public static string noOfConflitsChkbxText2 = string.Empty;
        public static string noOfConflitsChkbxText1 = string.Empty;
        public static string noOfConflitsChkbxText = string.Empty;
        public static string dialogText = string.Empty;
        public static string warningLblText = string.Empty;
        public static bool isDoForAllConflicts = false;
        

        public CustomCopyAndReplace()
        {
            InitializeComponent();
            string appLogoFilePath = @"ImageResources\LogoImageResources\IntuSoft.ico";

            if (File.Exists(appLogoFilePath))
                this.Icon = new System.Drawing.Icon(appLogoFilePath, 256, 256);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            if (string.IsNullOrEmpty(dialogText))
                this.Text = dialogText;
            this.KeyPreview = true;
        }

        /// <summary>
        /// set the check box properties which in on the window.
        /// </summary>
        /// <param name="n">no of conflits to be displayed</param>
        public  void setNoOfConflicts(int n)
        {
            if (n > 0)
            {
                noOfConflitsChkbxText = noOfConflitsChkbxText1 + n.ToString() + noOfConflitsChkbxText2;
                noOfConflits_chkbx.Text = noOfConflitsChkbxText;
                noOfConflits_chkbx.Visible = true;
            }
            else
            {
                noOfConflits_chkbx.Visible = false;
            }
        }

        /// <summary>
        /// set the button and label properties which are on the window.
        /// </summary>
        public void setButtonTexts()
        {
            if(!string.IsNullOrEmpty(copyReplaceBtn))
                copyAndReplace_btn.Text = copyReplaceBtn;//Sets the copyAndReplace_btn text to the value of the string copyReplaceBtn.
            copyAndReplace_btn.DialogResult = DialogResult.OK;
            copyAndReplace_btn.FlatAppearance.BorderSize = 0;

            if (!string.IsNullOrEmpty(copyAndKeepReplaceBtn))
                copyButKeepBoth_btn.Text = copyAndKeepReplaceBtn;//Sets the copyButKeepBoth_btn text to the value of the string copyAndKeepReplaceBtn.
            copyButKeepBoth_btn.DialogResult = DialogResult.Yes;
            copyButKeepBoth_btn.FlatAppearance.BorderSize = 0;

            if (!string.IsNullOrEmpty(dontCopyBtnText))
                dontCopy_btn.Text = dontCopyBtnText;//Sets the dontCopy_btn text to the value of the string dontCopyBtnText.
            dontCopy_btn.DialogResult = DialogResult.Ignore;
            dontCopy_btn.FlatAppearance.BorderSize = 0;

            if (!string.IsNullOrEmpty(cancelBtnText))
                cancel_btn.Text = cancelBtnText;//Sets the cancel_btn text to the value of the string cancelBtnText.
            cancel_btn.DialogResult = DialogResult.Cancel;
            cancel_btn.FlatAppearance.BorderSize = 0;

            if (!string.IsNullOrEmpty(warningLblText))
                warning_lbl.Text = warningLblText;
        }

        /// <summary>
        /// Display the dialog for copy and replace the file if they already exists.
        /// </summary>
        /// <param name="n">no of conflits</param>
        /// <returns></returns>
        public static DialogResult Show(int n)
        { 
            CustomCopyAndReplace frmMessage = new CustomCopyAndReplace();
            frmMessage.setButtonTexts();
            frmMessage.setNoOfConflicts(n);
            return frmMessage.ShowDialog();
        }

        private void noOfConflits_chkbx_CheckedChanged(object sender, EventArgs e)
        {
            isDoForAllConflicts = noOfConflits_chkbx.Checked;
        }

    }
}
