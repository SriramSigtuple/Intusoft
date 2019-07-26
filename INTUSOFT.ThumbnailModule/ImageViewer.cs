using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using INTUSOFT.Custom.Controls;
using Common;
namespace INTUSOFT.ThumbnailModule
{
    public partial class ImageViewer : FormUserControl
    {
        private Image m_Image;
        //This below code has been added by darshan in order to solve defect no:0000530

        public bool m_IsAnnotated;
        public static bool isValidImage;
        public bool m_isCDR;
        private string m_ImageLocation;
        private int imageId;
        private int imageSide;
        private Label m_ImageName;
        private bool m_IsThumbnail;
        private bool m_IsActive;
        private int m_Index;
        private int totalCount;
       
        private bool _imgMouseClick;
        public ImageViewer()
        {
            m_IsThumbnail = false;
            m_IsActive = false;
            
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        bool val = false;
        public bool ImageMouseClick
        {
            set { _imgMouseClick = value; }
            get { return _imgMouseClick; }
        }
        public int TotalCount
        {
            set { totalCount = value; }
            get { return totalCount; }
        }
        public int Index
        {
            set { m_Index = value; }
            get { return m_Index; }
        }
        public Image Image
        {
            set { m_Image = value; }
            get { return m_Image; }
        }

        public int ImageID
        {
            set { imageId = value; }
            get { return imageId; }
        }
        public int ImageSide
        {
            set { imageSide = value; }
            get { return imageSide; }
        }
        public string ImageLocation
        {
            set { m_ImageLocation = value; }
            get { return m_ImageLocation; }
        }
        public string ImageLabelLocation
        {
            set { m_ImageName.Text = value; }
            get { return m_ImageName.Text; }
        }
        
        public bool IsActive
        {
            set
            {
                m_IsActive = value;
                this.Invalidate();
            }
            get { return m_IsActive; }
        }

        public bool IsThumbnail
        {
            set { m_IsThumbnail = value; }
            get { return m_IsThumbnail; }
        }
        //This below code has been added by darshan in order to solve defect no:0000530

        public bool IsAnnotated
        {
            set { m_IsAnnotated = value; }
            get { return m_IsAnnotated; }
        }
        public bool IsCDR
        {
            set { m_isCDR = value; }
            get { return m_isCDR; }
        }
        public void ImageSizeChanged(object sender, ThumbnailImageEventArgs e)
        {
            this.Width = e.Size;
            this.Height = e.Size;
            this.Invalidate();
        }
        private string _ImageName;
        

public string ImageName
{
  get { return  _ImageName = label1.Text; }
}
        public void LoadImage(string imageFilename, int Id, int width, int height)
        {
            Common.CommonMethods c = Common.CommonMethods.CreateInstance();
            Bitmap bm = new Bitmap(10, 10);
            System.IO.FileInfo finf = new System.IO.FileInfo(imageFilename);
            isValidImage = false;// to reset image valid to false
            if (finf.Exists)
            {
                string[] strArr = finf.Name.Split('.');
                string thumbnailName = finf.DirectoryName + System.IO.Path.DirectorySeparatorChar + strArr[0] + "_tb." + strArr[1];
                if (System.IO.File.Exists(thumbnailName))
                    isValidImage = LoadSaveImage.LoadImage(thumbnailName, ref bm);
                if (!isValidImage)
                {
                    isValidImage = LoadSaveImage.LoadImage(imageFilename, ref bm);
                    if (isValidImage)
                    {
                        int mulfactor = width / 4;

                        Bitmap tempBM = new Bitmap(width, 3 * mulfactor);
                        Graphics graphics = Graphics.FromImage(tempBM);
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(bm, 0, 0, width, 3 * mulfactor);
                        graphics.Dispose();
                        tempBM.Save(thumbnailName);
                        bm.Dispose();
                        bm = new Bitmap(tempBM);
                        tempBM.Dispose();
                    }
                    else
                        return;
                }

            }
            else
            {
                return;// To fix defect 0001696 if the file doesn't exists then make the image invalid or corrupted.
            }
            Image tempImage = bm;
            m_ImageLocation = imageFilename;
            ImageID = Id;
            int dw = tempImage.Width;
            int dh = tempImage.Height;
            int tw = width;
            int th = height;
            double zw = (tw / (double)dw);
            double zh = (th / (double)dh);
            double z = (zw <= zh) ? zw : zh;
            dw = (int)(dw * z);
            dh = (int)(dh * z);

            m_Image = new Bitmap(dw, dh);
            Graphics g = Graphics.FromImage(m_Image);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(tempImage, 0, 0, dw, dh);
            g.Dispose();
            m_ImageName = new Label();
            m_ImageName.Location = new Point(0, dh);
            tempImage.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (g == null) return;
            if (m_Image == null) return;

            int dw = m_Image.Width;
            int dh = m_Image.Height;
            int tw = this.Width - 8; // remove border, 4*4 
            int th = this.Height - 8; // remove border, 4*4 
            double zw = (tw / (double)dw);
            double zh = (th / (double)dh);
            double z = (zw <= zh) ? zw : zh;

            dw = (int)(dw * z);
            dh = (int)(dh * z);
            int dl = 4 + (tw - dw) / 2; // add border 2*2
            int dt = 4 + (th - dh) / 2; // add border 2*2

           // g.DrawRectangle(new Pen(Color.Gray), dl, dt, dw, dh);
            //g.FillRectangle(Brushes.Gray, dl - 8, dt - 8, 25, 25);

            if (m_IsThumbnail)
                for (int j = 0; j < 3; j++)
                {
                    //g.DrawLine(new Pen(Color.DarkGray),
                    //    new Point(dl + 3, dt + dh + 1 + j),
                    //    new Point(dl + dw + 3, dt + dh + 1 + j));
                    //g.DrawLine(new Pen(Color.DarkGray),
                    //    new Point(dl + dw + 1 + j, dt + 3),
                    //    new Point(dl + dw + 1 + j, dt + dh + 3));
                   // g.FillRectangle(Brushes.Gray, dl - 8, dt - 8, 25, 25);
                }
            // if (ImageMouseClick)
            //this.checkBox1.Checked = false;

            g.DrawImage(m_Image, dl, dt, dw, dh);

            if (m_IsActive)
            {
                
                // g.DrawRectangle(new Pen(Color.White, 1), dl, dt, dw, dh);
                //g.DrawRectangle(new Pen(Color.Aqua, 1), dl, dt, dw, dh);
                g.DrawRectangle(new Pen(Color.Aqua, 8), dl - 2, dt - 2, dw + 4, dh + 4);// border rectangle for the thumbnail
                //g.DrawLine(new Pen(Color.Red, 2), new Point(dl - 2, 22 ), new Point(dl - 2  + 4, 45));
               // g.DrawLine(Pens.Aqua, new Point(dl - 2 + 25, dt - 8 ), new Point(dl - 2 + 3, 25));

               // g.DrawRectangle(new Pen(Color.White, 1), dl - 2, dt - 2, 25, 25);
                
                g.FillRectangle(Brushes.Aqua, dl - 2, dt - 2, 10, 10);// notch in the thumbnail hightlight


            }
            //else if(!ImageMouseClick)
            //{
            //    this.checkBox1.Checked = false;
            //}
            //else if(ImageMouseClick)
            //    ImageMouseClick = false;

        }

        private void OnResize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void UnloadImage()
        {
        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {

        }




    }
}
