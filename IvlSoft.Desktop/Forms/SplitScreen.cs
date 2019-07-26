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
using System.IO;
using INTUSOFT.ThumbnailModule;
namespace INTUSOFT.Desktop.Forms
{
    public partial class SplitScreen : BaseGradientForm
    {

        public Bitmap image1;
        public Bitmap image2;
        public SplitScreen(List<ThumbnailData> ImgList1,List<ThumbnailData> ImgList2)
        {
            InitializeComponent();
    //        foreach (var item in ImgList1)
    //{
		 
    //}
    //        thumbnailUI1.AddThumbnails(

        }
    }
}
