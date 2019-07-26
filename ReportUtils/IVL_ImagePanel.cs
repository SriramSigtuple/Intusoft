using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using INTUSOFT.Custom.Controls;
using Common;


namespace ReportUtils
{
    public delegate void PbClick(object sender, EventArgs e);

    public class IVL_ImagePanel : Panel
    {
        public bool isMedicalName = true;
        public event PbClick pbclick;
        int _RowCount=1;
       public static int imgs;
        List<RowColumnFactor> FactorsList;
        //List<Size> imageSizeList;
        List<RowColumnFactorsWithSize> imageSizeList;
        int imgCount;
       public static bool isPortrait = false;
        public int RowCount
        {
            get { return _RowCount; }
            set { _RowCount = value;
            //ResetPanel();
            }
        }
        int _ColumnCount=1;
        //int numberOfImages = 3;
        public int ColumnCount
        {
            get { return _ColumnCount; }
            set {
                _ColumnCount = value;
                //ResetPanel();
            }
        }
        private int images;

        public int Images
        {
            get { return images; }
            set {
                images = value;
                imgs = value;
                ResetPanel();
                }
        }
        int paddingValue = 10;

        public string[] imagePaths;
        public string[] imageNames;
        public bool isFromReport = false;
        DataModel dataModel;
        public IVL_ImagePanel()
        {
            dataModel = DataModel.GetInstance();
            if(!isPortrait)
            this.Size = new System.Drawing.Size(800, 300);
            else
                this.Size = new System.Drawing.Size(450, 600);
            //RowCount = 2;
            //ColumnCount = 2;
            Images = 1;
           
           this.Resize += IVL_ImagePanel_Resize;
           // this.Paint += IVL_ImagePanel_Paint;
        }

        void IVL_ImagePanel_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush solidBrush = new SolidBrush(Color.Black);
            this.BackColor = Color.Red;
            IntPtr hwnd = this.Handle;

            using (Graphics g = e.Graphics)//; Graphics.FromHwnd(hwnd))
            {
                g.DrawString("Image ", new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 100), solidBrush, new PointF((float)(this.Location.X + this.Width / 2), this.Location.Y + this.Height / 2));// pictureBox_X + height));
            }
        }

        public void ResetPanel()
        {
            this.Controls.Clear();
            setRowAndColumnValue(Images);
            //createPanel();
        }

        public void SetImagePathAndNames(string[] imgPath, string[] imgNames)
        {
            imagePaths = imgPath;
            this.imageNames = imgNames;
            this.Controls.Clear();
            isFromReport = true;
            Images = imagePaths.Count();
        }


        void IVL_ImagePanel_Resize(object sender, EventArgs e)
        {
            ResetPanel();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        public void noOfImages(int row, int column)
        {
            int sum = row + column;
            int number = Convert.ToInt32(Math.Abs(Math.Sqrt(sum)));
        }

        //public void createPanel()
        //{
        //    //noOfImages(RowCount, ColumnCount);
        //    Graphics g;
        //    SolidBrush solidBrush = new SolidBrush(Color.Black);
        //    int imageWidthinPanel = (this.Size.Width / ColumnCount) - paddingValue;
        //    int imageHeightPanel = (this.Size.Height / RowCount) - paddingValue;
        //    int pictureBox_X = 5;
        //    int pictureBox_Y = 5;
        //    int imgCount = 0;
        //    for (int i = 0; i < this.RowCount; i++)
        //    {
        //        for (int j = 0; j < this.ColumnCount; j++)
        //        {
        //            Label lbl = new Label();
        //            PictureBox pb = new PictureBox();
        //            Bitmap bm = new Bitmap("Test1.jpg");
        //            g = Graphics.FromImage(bm);
        //            g.DrawImage(bm, pictureBox_X, pictureBox_Y, imageWidthinPanel, imageHeightPanel);
        //            pb.SizeMode = PictureBoxSizeMode.Zoom;
        //            pb.Margin = new System.Windows.Forms.Padding(0);
        //            //lbl.Height = 5;
        //            lbl.BackColor = Color.Transparent;
        //            lbl.AutoSize = false;
        //            lbl.Text = "Image " + (1 + imgCount).ToString();
        //            int val = lbl.Text.Length;
        //            lbl.Margin = new System.Windows.Forms.Padding(0);
        //            // lbl.TextAlign = ContentAlignment.MiddleCenter;
        //            int lblX = (imageWidthinPanel / 2) - lbl.Width / 2;
        //            int width = TextRenderer.MeasureText(lbl.Text, lbl.Font).Width; ;
        //            int height = TextRenderer.MeasureText(lbl.Text, lbl.Font).Height;
        //            lbl.Height = height;
        //            // lbl.Location = new Point((pictureBox_X + (imageWidthinPanel / 2)) - lbl.Width / 2, pictureBox_Y);
        //            lbl.Location = new Point((pictureBox_X + (imageWidthinPanel / 2)) - width / 2, pictureBox_Y);
        //            pb.Image = new Bitmap("Test1.jpg");
        //            pb.Location = new Point(pictureBox_X, pictureBox_Y + lbl.Height);
        //            pb.Size = new Size(imageWidthinPanel, imageHeightPanel - lbl.Height);
        //            this.Controls.Add(lbl);
        //            this.Controls.Add(pb);

        //            pictureBox_X += imageWidthinPanel + paddingValue;
        //            imgCount++;
        //        }
        //        pictureBox_Y += imageHeightPanel + paddingValue;
        //        pictureBox_X = 5;
        //    }
        //    this.Refresh();
        //}

        PictureBoxExtended pb;
        public void createPanel(int row,int col)
        {
            int imageWidthinPanel = (this.Size.Width / col) - paddingValue;
            int imageHeightPanel = (this.Size.Height / row) - paddingValue;
            int pictureBox_X = 5;
            int pictureBox_Y = 0;
            imgCount = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    pb = new PictureBoxExtended();
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Margin = new System.Windows.Forms.Padding(0);

                    pb.Index = ++imgCount;
                    if (imgCount <= Images)
                    {
                        if (File.Exists("Test1.jpg"))
                            pb.Image = new Bitmap("Test1.jpg");
                        else
                            pb.Image = new Bitmap(2048, 1536);
                    }
                    else
                        pb.Image = new Bitmap(2048, 1536);
                    pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    pb.Location = new Point(pictureBox_X, pictureBox_Y);

                    pb.Size = new Size(imageWidthinPanel, imageHeightPanel - paddingValue);
                    pb.Paint += pb_Paint;

                    this.Controls.Add(pb);
                    pictureBox_X += imageWidthinPanel + paddingValue;
                }
                pictureBox_Y += imageHeightPanel + paddingValue;
                pictureBox_X = 5;
            }

            this.Refresh();
        }


        public void createPanel(int row, int col,string name)
        {
            int imageWidthinPanel = (this.Size.Width / col) - paddingValue;
            int imageHeightPanel = (this.Size.Height / row) - paddingValue;
            int pictureBox_X = 5;
            int pictureBox_Y = 0;
            imgCount = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    PictureBoxExtended pb = new PictureBoxExtended();
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Margin = new System.Windows.Forms.Padding(0);

                    {
                        ToolTip tt = new ToolTip();
                        tt.AutoPopDelay = 500;
                        tt.InitialDelay = 1000;
                        tt.ReshowDelay = 500;
                        tt.SetToolTip(pb, "Select Images");
                    }
                    if (imgCount < Images && Images>0)
                    {
                        if (File.Exists(imagePaths[imgCount]))
                        {
                            Common.CommonMethods common_methods = Common.CommonMethods.CreateInstance();
                            Bitmap bm = new Bitmap(10, 10);
                            LoadSaveImage.LoadImage(imagePaths[imgCount], ref bm);
                            pb.Image = bm;
                        }
                        else
                            pb.Image = new Bitmap(2048, 1536);
                    }
                    else
                        pb.Image = new Bitmap(2048, 1536);
                    if (imgCount < Images && Images > 0 && imageNames.Length == Images)
                    {
                        pb.Tag = imageNames[imgCount];
                    }
                    pb.Index = ++imgCount;
                    pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    pb.Location = new Point(pictureBox_X, pictureBox_Y);
                    pb.Size = new Size(imageWidthinPanel, imageHeightPanel - paddingValue);
                    pb.Click += pb_Click;
                    pb.Paint += pbPaint;
                    
                    this.Controls.Add(pb);
                    pictureBox_X += imageWidthinPanel + paddingValue;
                }
                pictureBox_Y += imageHeightPanel + paddingValue;
                pictureBox_X = 5;
            }
            
            this.Refresh();
        }


        void pbPaint(object sender, PaintEventArgs e)
        {
            PictureBoxExtended pb = sender as PictureBoxExtended;
            var wfactor = (double)pb.Image.Width / pb.ClientSize.Width;
            var hfactor = (double)pb.Image.Height / pb.ClientSize.Height;
            var resizeFactor = Math.Max(wfactor, hfactor);
            var zoomedImageSize = new Size((int)(pb.Image.Width / resizeFactor), (int)(pb.Image.Height / resizeFactor));
            int height = pb.Height - zoomedImageSize.Height;
            int width = pb.Width - zoomedImageSize.Width;
            SolidBrush solidBrush = new SolidBrush(Color.Black);
            if (pb.Index <= Images)
            {
                if (pb.Tag != null)
                {
                    Graphics g = pb.Parent.CreateGraphics();
                    int stringWidth = TextRenderer.MeasureText(pb.Tag.ToString(), new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10)).Width; ;
                    #region Code to display Right eye and left eye instead of OD and OS if it is DR report
                    string name = string.Empty;
                    if (!isMedicalName)
                    {
                        if (pb.Tag.ToString().Contains("OS"))
                            name = "Left Eye";
                        else if (pb.Tag.ToString().Contains("OD"))
                            name = "Right Eye";
                    }
                    #endregion
                    else
                        name = pb.Tag.ToString();

                    g.DrawString(name, new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10), solidBrush, new PointF((float)((pb.Location.X + pb.Width / 2) - stringWidth / 2), (float)(pb.Location.Y + pb.Height)));
                }
            }
            //Console.WriteLine(pb.Location.X.ToString()+"__"+ pb.Location.Y.ToString());
        }

        void pb_Paint(object sender, PaintEventArgs e)
        {
            PictureBoxExtended pb = sender as PictureBoxExtended;
            var wfactor = (double)pb.Image.Width / pb.ClientSize.Width;
            var hfactor = (double)pb.Image.Height / pb.ClientSize.Height;
            var resizeFactor = Math.Max(wfactor, hfactor);
            var zoomedImageSize = new Size((int)(pb.Image.Width / resizeFactor), (int)(pb.Image.Height / resizeFactor));
            int height = pb.Height - zoomedImageSize.Height;
            int width = pb.Width - zoomedImageSize.Width;
            SolidBrush solidBrush = new SolidBrush(Color.Black);
            if (pb.Index <= Images)
            {
                Graphics g = pb.Parent.CreateGraphics();
                int stringWidth = TextRenderer.MeasureText("Image " + pb.Index.ToString(), new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10)).Width; ;
                g.DrawString("Image " + pb.Index.ToString(), new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10), solidBrush, new PointF((float)((pb.Location.X + pb.Width / 2) - stringWidth / 2), (float)(pb.Location.Y - 15)));
            }
            //Console.WriteLine(pb.Location.X.ToString()+"__"+ pb.Location.Y.ToString());
        }

        public void getSizesForEachRowsAndColumns(int row,int col)
        {
            int imageWidthinPanel = (this.Size.Width / col) - paddingValue;
            int imageHeightPanel = (this.Size.Height / row) - paddingValue;
            int pictureBox_X = 0;
            int pictureBox_Y = 0;
            {
                {
                    PictureBoxExtended pb = new PictureBoxExtended();
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Margin = new System.Windows.Forms.Padding(0);
                    if(File.Exists("Test1.jpg"))
                    pb.Image = new Bitmap("Test1.jpg");
                    else
                        pb.Image = new Bitmap(2048,1536);
                    pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    pb.Location = new Point(pictureBox_X, pictureBox_Y + paddingValue);
                    //if (imageHeightPanel - paddingValue > 0)
                    //    pb.Size = new Size(imageWidthinPanel, imageHeightPanel - paddingValue);
                    //else
                        pb.Size = new Size(imageWidthinPanel, imageHeightPanel);
                    var wfactor = (double)pb.Image.Width / pb.ClientSize.Width;
                    var hfactor = (double)pb.Image.Height / pb.ClientSize.Height;
                    var resizeFactor = Math.Max(wfactor, hfactor);
                    if (!imageSizeList.Contains(new RowColumnFactorsWithSize(row, col, new Size((int)(pb.Image.Width / resizeFactor), (int)(pb.Image.Height / resizeFactor)))))
                        imageSizeList.Add(new RowColumnFactorsWithSize(row, col, new Size((int)(pb.Image.Width / resizeFactor), (int)(pb.Image.Height / resizeFactor))));
                }
            }
        }

        public void setRowAndColumnValue(int noOfImages)
        {
            if (noOfImages > 1)
            {
              FactorsList = new List<RowColumnFactor>();

                imageSizeList = new List<RowColumnFactorsWithSize>();
                GetFactorsForANumber(noOfImages);
                List<RowColumnFactor> temp = new List<RowColumnFactor>();// FactorsList.Distinct().ToList();

                foreach (RowColumnFactor item in FactorsList)
                {
                    if(item.col <= images && item.row <= images)//To Check whether the column and row count is less than or equal to the images count
                    if (!temp.Contains(item))
                        temp.Add(item);
                }
                FactorsList = temp.ToList();
                foreach (RowColumnFactor item in FactorsList)
                {
                    getSizesForEachRowsAndColumns(item.row, item.col);
                }
               
                    imageSizeList.Sort(delegate(RowColumnFactorsWithSize c1, RowColumnFactorsWithSize c2) { return c1.CompareTo(c2); });
                    this.RowCount = imageSizeList[0].row;
                    this.ColumnCount = imageSizeList[0].col;
                    if (!isFromReport)
                        createPanel(imageSizeList[0].row, imageSizeList[0].col);
                    else
                        createPanel(imageSizeList[0].row, imageSizeList[0].col, null);
                
            }
            else
            {
                if (!isFromReport)
                    createPanel(1, 1);
                else
                    createPanel(1, 1, null);
                this.RowCount = 1;
                this.ColumnCount = 1;
            }
        }

        public Point GetPictureBoxPoint(PictureBoxExtended pb)
        {
            Point p = pb.PointToClient(Cursor.Position);
            Point unscaled_p = new Point();

            // image and container dimensions
            int w_i = pb.Image.Width;
            int h_i = pb.Image.Height;
            int w_c = pb.Width;
            int h_c = pb.Height;

            float imageRatio = w_i / (float)h_i; // image W:H ratio
            float containerRatio = w_c / (float)h_c; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = w_c / (float)w_i;
                float scaledHeight = h_i * scaleFactor;
                // calculate gap between top of container and top of image
                float filler = Math.Abs(h_c - scaledHeight) / 2;
                unscaled_p.X = (int)(p.X / scaleFactor);
                unscaled_p.Y = (int)((p.Y - filler) / scaleFactor);
            }
            else
            {
                // vertical image
                float scaleFactor = h_c / (float)h_i;
                float scaledWidth = w_i * scaleFactor;
                float filler = Math.Abs(w_c - scaledWidth) / 2;
                unscaled_p.X = (int)((p.X - filler) / scaleFactor);
                unscaled_p.Y = (int)(p.Y / scaleFactor);
            }

            return unscaled_p;
        }

        public  void GetFactorsForANumber(int number)
        {
            double sqrtVal = Math.Sqrt(number);  //round down
            //int factorValue = 0;
            for (int i = 1; i <= sqrtVal; ++i)
            { //test  from 1 to the square root, or the int below it, inclusive.
                if (number % i == 0)
                {
                    RowColumnFactor rowColObj = new RowColumnFactor();
                    if (!isPortrait)
                    {
                        rowColObj.row = i;
                        rowColObj.col = number / i;
                    }
                    else
                    {
                        rowColObj.col = i;
                        rowColObj.row = number / i;
                    }
                    // if (!FactorsList.Contains(rowColObj))
                    FactorsList.Add(rowColObj);
                }
            }
            if (FactorsList.Count == 1 )//|| !(d % 1 == 0))
            {
                sqrtVal += 0.5;
                sqrtVal = Math.Round(sqrtVal, MidpointRounding.AwayFromZero);
                GetFactorsForPrimeNumber(number, sqrtVal);
            }
        }

        public void GetFactorsForPrimeNumber(int number, double sqrtVal)
        {
            RowColumnFactor rowColObj = new RowColumnFactor();

            FactorsList.Add(new RowColumnFactor((int)sqrtVal, (int)sqrtVal));
            double squareNumber = Math.Pow(sqrtVal, 2);
            double count = squareNumber - number;
            for (int i = 1; i <= count; i++)
            {
                int numberParam = number + i;
                if (squareNumber != numberParam)
                    GetFactorsForANumber(numberParam);
            }
        }

        void pb_Click(object sender, EventArgs e)
        {
            pbclick(sender, e);
        }
    }

    public static class PermutationsAndCombinations
    {
        public static long nCr(int n, int r)
        {
            return nPr(n, r) / Factorial(r);
        }

        public static long nPr(int n, int r)
        {
            return (Factorial(n)/ Factorial(n - r));
        }

        private static long Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }
    }

    class RowColumnFactor : IEquatable<RowColumnFactor>
    {
        public int row = 0;
        public int col = 0;
        public RowColumnFactor()
        {

        }
        public RowColumnFactor(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        public bool Equals(RowColumnFactor other)
        {
            if (this.row == other.row && this.col == other.col)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    class RowColumnFactorsWithSize : IComparable<RowColumnFactorsWithSize>
    {
        public int row = 0;
        public int col = 0;
        public Size imageSize=new Size();
        public RowColumnFactorsWithSize(int row,int col,Size size)
        {
            this.row = row;
            this.col = col;
            imageSize = size;
        }

        public override bool Equals(object obj)
        {
            bool res = false;
            if (obj.GetType() == typeof(RowColumnFactorsWithSize))
            {
                RowColumnFactorsWithSize objCasted = (RowColumnFactorsWithSize)obj;
                res = objCasted.row == row && objCasted.col == col;
            }
            return res;
        }

        public override int GetHashCode()
        {
            return row+col;
        }

        public int CompareTo(RowColumnFactorsWithSize obj)
        {
            if (this.imageSize.Width > obj.imageSize.Width && this.imageSize.Height > obj.imageSize.Height) 
                return -1;
            if (this.imageSize.Width == obj.imageSize.Width && this.imageSize.Height == obj.imageSize.Height)
            {

                if (this.col * this.row > obj.col * obj.row)//to select best factors
                    return 0;
                else
                    return -1;
            }
            else
                return 1;
        }
    }
}
