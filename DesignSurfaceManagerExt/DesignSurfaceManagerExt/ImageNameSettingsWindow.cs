using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReportUtils;
namespace DesignSurfaceManagerExtension
{
    public partial class ImageNameSettingsWindow : Form
    {
        public bool ImageMedicalName = false;

        public TextAlign TextAlignVal;

        public ImageNameSettingsWindow()
        {
            InitializeComponent();
        }

        private void medicalName_rb_CheckedChanged(object sender, EventArgs e)
        {
            ImageMedicalName = medicalName_rb.Checked;
        }

        private void ImageNameSettingsWindow_Load(object sender, EventArgs e)
        {
            medicalName_rb.Checked = ImageMedicalName;
        }

        private void apply_btn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void horizontalTextAlign_cmbx_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


    }

}
