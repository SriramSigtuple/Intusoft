using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace IVLTemplateCreator.IVLControls
{
    class IVL_ImageTable:TableLayoutPanel
    {
        string[] images;
        bool isPortrait = true;
        public IVL_ImageTable(string[]images,bool isPortrait)
        {
            this.images = images;
            this.isPortrait = isPortrait;
            this.BorderStyle = BorderStyle.FixedSingle;
           // this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            if (isPortrait)
                this.Size = new Size(480, 360);
            else
                this.Size = new Size(800, 600);

        }
        protected override void OnCreateControl()
        {
                base.OnCreateControl();
                createTable();
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            LayoutMouseHandler.OnMouseDown(this, e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            LayoutMouseHandler.OnMouseUp(this, e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            LayoutMouseHandler.OnMouseMove(this, e);
        }

        private void createTable()
        {
            switch (images.Length)
            {
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
                        this.RowCount = 2;
                        this.ColumnCount = 3;
                    }
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
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
                        this.RowCount = 2;
                        this.ColumnCount = 3;
                    }
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    break;

                default:
                    this.RowCount = 1;
                    this.ColumnCount = 1;
                    this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    break;
            }

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
                        pb.Image = new Bitmap(images[imgCnt]);
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
