using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace marlie.TumbnailDotnet
{
    public partial class ImageDialog : Form
    {
        public ImageDialog()
        {
            InitializeComponent();
        }

        public void SetImage(string filename)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(SetImageIntern));
            thread.IsBackground = true;
            thread.Start(filename);
        }

        private void SetImageIntern(object filename)
        {
            this.imageViewerFull.Image = Image.FromFile((string)filename);
            this.imageViewerFull.Invalidate();
        }

        private void ImageDialog_Resize(object sender, EventArgs e)
        {
            this.imageViewerFull.Invalidate();
        }
    }
}