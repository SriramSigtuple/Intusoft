using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Common;
namespace IVLReport
{
    public delegate void PbDoubleClick(object sender,EventArgs e);
    public delegate void PbClick(object sender, EventArgs e);

    class IVL_ImageTable : TableLayoutPanel
    {
        public event PbDoubleClick pbdDoubleClick;
        public event PbClick pbclick;
        string[] images;
        string[] imageNames;

        bool isPortrait = true;
        public  PictureBox pb;
        //private DataModel _dataModel;
        public IVL_ImageTable(string[] images,string[] imageNames ,bool isPortrait)
        {
            //_dataModel = DataModel.GetInstance();
            this.images = images;
            this.imageNames = imageNames;
            this.isPortrait = isPortrait;
            //this.BorderStyle = BorderStyle.FixedSingle;
            // this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            if (isPortrait)
                this.Size = new Size(800, 600);// changed size for portait 
            else
                this.Size = new Size(800, 600);
            createTable();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

        }
        private void createTable()
        {
            //switch case has been removed and for loop is added in order accomodate as many images as possible by sriram on september 9th 2015
            //switch (images.Length)
            //{
            //    case 2:
            //        if (isPortrait)
            //        {
            //            this.RowCount = 2;
            //            this.ColumnCount = 1;
            //        }
            //        else
            //        {
            //            this.RowCount = 1;
            //            this.ColumnCount = 2;
            //        }                
            //        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            //        this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            //        break;
            //    case 3:
            //        this.RowCount = 2;
            //        this.ColumnCount = 2;
            //        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            //        this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            //        break;

            //    case 4:
            //        this.RowCount = 2;
            //        this.ColumnCount = 2;
            //        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            //        this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            //        break;

            //    case 5:
            //        if (isPortrait)
            //        {
            //            this.RowCount = 3;
            //            this.ColumnCount = 2;
            //        }
            //        else
            //        {
            //            this.RowCount = 2;
            //            this.ColumnCount = 3;
            //        }
            //        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            //        this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            //        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            //        this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            //        break;

            //    case 6:
            //        if (isPortrait)
            //        {
            //            this.RowCount = 3;
            //            this.ColumnCount = 2;
            //        }
            //        else
            //        {
            //            this.RowCount = 2;
            //            this.ColumnCount = 3;
            //        }
            //        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            //        this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            //        this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            //        this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            //        break;
                 
            //    default:
            ////if (images.Length == 1)
            ////{
            //this.RowCount = 1;
            //this.ColumnCount = 1;
            //this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            //this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            //break;
            //}
            //else if(images.Length  > 1)
            //{
            //    //if(images.Length % 2 == 0 )

            //}
            if (images.Length > 2)
            {
                if (images.Length % 2 == 0)
                {
                    if (isPortrait)
                    {
                        this.ColumnCount = 2;
                        this.RowCount = images.Length / 2;
                        //this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        //this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    }
                    else
                    {
                        this.RowCount = 2;
                        this.ColumnCount = images.Length / 2;
                        //this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        //this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    }
                }
                else
                {
                    int temp = images.Length + 1;
                    if (isPortrait)
                    {
                        this.ColumnCount = 2;
                        this.RowCount = temp / 2;
                        //this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        //this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    }
                    else
                    {
                        this.RowCount = 2;
                        this.ColumnCount = temp / 2;
                        //this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        //this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    }
                }
            }
            else if (images.Length == 1 || images.Length == 0)//The additional condition images.Length == 0 has been added to enable the click on imagetable when image count is zero;
            {
            this.RowCount = 1;
            this.ColumnCount = 1;
            //this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            }
            else if (images.Length == 2)
            {
                {
                    this.RowCount = 1;
                    this.ColumnCount = 2;
                }
            }
            int imgCnt = 0;
            //_dataModel.ImageFileNamesFromTable = new string[this.RowCount, this.ColumnCount];
            //_dataModel.ImageNamesFromTable = new string[this.RowCount, this.ColumnCount];
            for (int i = 0; i < this.RowCount; i++)
            {
                this.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            }
            for (int i = 0; i < this.ColumnCount; i++)
            {
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            }
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                     {
                    Label lbl = new Label();
                    pb = new PictureBox();
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Dock = DockStyle.Fill;
                    pb.Margin = new System.Windows.Forms.Padding(0);
                    //if (!Report.isFromCDR)//This if statement has been added for the purpose of not showing the tool tip when Report is opened from CDR.
                    {
                        ToolTip tt = new ToolTip();
                        tt.AutoPopDelay = 500;
                        tt.InitialDelay = 1000;
                        tt.ReshowDelay = 500;
                        tt.SetToolTip(pb, "Select Images");
                    }
                    if (Report.isNew)
                    {
                        pb.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        pb.Cursor = Cursors.Default;
                    }
                        pb.DoubleClick += pb_DoubleClick;
                        pb.Click += pb_Click;
                    {
                        if (imgCnt < images.Length)
                        {
                            Common.CommonMethods common_methods = Common.CommonMethods.CreateInstance();
                            Bitmap bm = new Bitmap(10, 10);
//                            common_methods.LoadImage(images[imgCnt], ref bm);
                            LoadSaveImage.LoadImage(images[imgCnt], ref bm);
                            pb.Image = bm;
                            lbl.BackColor = Color.Transparent;
                            lbl.Text = this.imageNames[imgCnt];// "Image " + (1 + imgCnt).ToString();
                            lbl.Margin = new System.Windows.Forms.Padding(0);
                            lbl.Dock = DockStyle.Top;
                            if (images.Length == 2)
                            {
                                lbl.Dock = DockStyle.Top;
                                lbl.TextAlign = ContentAlignment.BottomCenter;
                            }
                            else
                            {
                                lbl.Dock = DockStyle.Top;
                                lbl.TextAlign = ContentAlignment.MiddleCenter;
                            }
                            //_dataModel.ImageFileNamesFromTable[i, j] = images[imgCnt];
                            //_dataModel.ImageNamesFromTable[i, j] = this.imageNames[imgCnt];
                        }
                    }
                    Panel pnl = new Panel();
                    pnl.Dock = DockStyle.Fill;
                    pnl.Controls.Add(pb);
                    pnl.Controls.Add(lbl);
                    this.Controls.Add(pnl, j, i);
                    imgCnt++;
                    }
                }
            }
        }

        void pb_Click(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
            pbclick(sender, e);
        }

        void pb_DoubleClick(object sender, EventArgs e)
        {
            pbdDoubleClick(sender, e);
        }
    }
}
