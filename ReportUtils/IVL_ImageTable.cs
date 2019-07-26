using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTUSOFT.Custom.Controls;
using System.Windows.Forms;
using System.Drawing;

namespace ReportUtils
{

    public class IVL_ImageTable : TableLayoutPanel
    {
        public string[] images;
        public bool isPortrait = false;
        public IVL_ImageTable()
        {
            
            //this.BorderStyle = BorderStyle.FixedSingle;
            // this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.RowCount = 2;
            this.ColumnCount = 2;
            if (isPortrait)
                this.Size = new System.Drawing.Size(480, 360);
            else
                this.Size = new System.Drawing.Size(800, 600);
            UpdateRowsCols(this.RowCount, this.ColumnCount);
           
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
           
        }

        public void createTable()
        {
            switch (images.Length)
            {
                case 1:
                    {
                        this.RowCount = 1;
                        this.ColumnCount = 1;
                        break;
                    }
                case 2:
                    if (isPortrait)
                    {
                        this.RowCount = 2;
                        this.ColumnCount = 1;
                    }
                    else
                    {
                        this.RowCount = 1;
                        this.ColumnCount = 2;
                    }
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    break;

                case 3:
                    this.RowCount = 2;
                    this.ColumnCount = 2;
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    break;

                case 4:
                    this.RowCount = 2;
                    this.ColumnCount = 2;
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    break;

                case 5:
                    if (isPortrait)
                    {
                        this.RowCount = 3;
                        this.ColumnCount = 2;
                    }
                    else
                    {
                        //this.RowCount = 2;
                        //this.ColumnCount = 3;
                        this.RowCount = 3;
                        this.ColumnCount = 2;
                    }
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    break;

                case 6:
                    if (isPortrait)
                    {
                        this.RowCount = 3;
                        this.ColumnCount = 2;
                    }
                    else
                    {
                        //this.RowCount = 2;
                        //this.ColumnCount = 3;
                        this.RowCount = 3;
                        this.ColumnCount = 2;
                    }
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    break;

                case 7:
                    {
                        //this.ColumnStyles.RemoveAt(0);
                        //this.ColumnStyles.RemoveAt(0);
                        
                        this.RowCount = 4;
                        this.ColumnCount = 2;
                        //this.ColumnStyles.Insert(0, new ColumnStyle(SizeType.Percent, 25));
                        //this.ColumnStyles.Insert(1, new ColumnStyle(SizeType.Percent, 25));
                        //this.ColumnStyles.Insert(2, new ColumnStyle(SizeType.Percent, 25));
                        //this.ColumnStyles.Insert(3, new ColumnStyle(SizeType.Percent, 25));
                        this.RowStyles.Insert(0, new RowStyle(SizeType.Percent, 25));
                        this.RowStyles.Insert(1, new RowStyle(SizeType.Percent, 25));
                        this.RowStyles.Insert(2, new RowStyle(SizeType.Percent, 25));
                        this.RowStyles.Insert(3, new RowStyle(SizeType.Percent, 25));
                        //this.RowCount = 4;
                        //this.ColumnCount = 2;
                        //this.RowCount = 3;
                        //this.ColumnCount = 3;
                    }
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    //this.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
                    break;


                default:
                    this.RowCount = 2;
                    this.ColumnCount = 2;
                    break;
            }

            int imgCnt = 0;
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                    Label lbl = new Label();
                    PictureBoxExtended pb = new PictureBoxExtended();
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Margin = new System.Windows.Forms.Padding(0);
                    try
                    {
                        pb.Image = new Bitmap("Test1.jpg");
                        pb.Dock = DockStyle.Fill;
                       
                        lbl.BackColor = Color.Transparent;
                        lbl.Text = "Image " + (1 + imgCnt).ToString();
                        lbl.Margin = new System.Windows.Forms.Padding(0);
                        lbl.Dock = DockStyle.Top;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Panel pnl = new Panel();
                    pnl.Dock = DockStyle.Fill;
                    pnl.Controls.Add(pb);
                    pnl.Controls.Add(lbl);
                    this.Controls.Add(pnl, j, i);
                    //MessageBox.Show("Image " + (1 + imgCnt).ToString() + "PictureBox details" + "Location " + "x=" + pb.Location.X.ToString() + "y =" + pb.Location.Y.ToString() + "Size width=" + pb.Image.Width + "Height=" + pb.Image.Height);
                    //MessageBox.Show(" Panel Details" + "Location " + "x=" + pnl.Location.X.ToString() + "y =" + pnl.Location.Y.ToString() + "Size width=" + pnl.Width + "Height=" + pnl.Height);
                    imgCnt++;
                }
            }
            //GetImageWidthHeight();
        }


        public void GetImageWidthHeight()
        {
            int image = 0;
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                  Control ctrl=  this.GetControlFromPosition(j, i);
                }
            }
        }

        public void UpdateRowsCols(int rows, int cols)
        {

            this.Controls.Clear();
            //this.RowCount -= rows;
            //this.ColumnCount -= cols;
            if(rows == 1 && cols != 1 )
                images= new string[cols];
            else if (rows == 1 && cols == 1)
                images = new string[rows];
            else if(rows != 1 && cols == 1 )
                images = new string[rows];

            else if (rows != 1 && cols != 1)
                images = new string[rows+cols];


            for (int i = 0; i < images.Length; i++)
            {
                images[i] = "Test1.jpg";
            }
            createTable();
            
        }

        public void UpdateTable()
        {
            int imgCnt = 0;
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColumnCount; j++)
                {
                    Label lbl = new Label();
                    PictureBox pb = new PictureBox();
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Dock = DockStyle.Fill;
                    pb.Margin = new System.Windows.Forms.Padding(0);
                    try
                    {
                        pb.Image = new Bitmap("Test1.jpg");
                        lbl.BackColor = Color.Transparent;
                        lbl.Text = "Image " + (1 + imgCnt).ToString();
                        lbl.Margin = new System.Windows.Forms.Padding(0);
                        lbl.Dock = DockStyle.Top;
                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
}
