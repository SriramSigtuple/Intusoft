using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace Annotation
{
    public partial class AnnotationText : UserControl
    {
        private int _id = 0;
        public string comments = "";
        public delegate void AnnotationUi_Id(int id,bool isDelete=false);
        public event AnnotationUi_Id annotation_id;
        private bool _mActive;
        public AnnotationComments ac;
        public DrawArea drawArea1;
        private const int MAX_LINES = 3;
        public AnnotationText()
        {
            InitializeComponent();
            ac = new AnnotationComments();
            drawArea1 = new DrawArea();

            this.Annotation_tbx.MouseClick += richTextBox1_MouseClick;
            this.annotationTextCount_lbl.MouseClick += annotationTextCount_lbl_MouseClick;
            this.toolStrip1.Renderer = new  INTUSOFT.Custom.Controls.FormToolStripRenderer();

            if (Screen.PrimaryScreen.Bounds.Width == 1920)
            {
                this.Width = 490;
            }

            Rectangle Bounds = new Rectangle(0, 0, this.Annotation_tbx.Width, this.Annotation_tbx.Height);
            //int CornerRadius = c.Bounds.Height / 2;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();


            path = RoundedRect(Bounds, 5);
            this.Annotation_tbx.Region = new Region(path);

            Bounds = new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height);
             path = new System.Drawing.Drawing2D.GraphicsPath();
            
            path = RoundedRect(Bounds, 10);
            this.Region = new Region(path);
             

        }
        public GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }












        void annotationTextCount_lbl_MouseClick(object sender, MouseEventArgs e)
        {
           
            //if(!this._mActive)
            //this.isActive = true;
            //_id = Convert.ToInt32(annotationTextCount_lbl.Text);

        }

        void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
           
            //if(!this._mActive)
            //    this.isActive = true;
            //_id = Convert.ToInt32(annotationTextCount_lbl.Text);
            //annotation_id(_id);
        }
        public void setValues(int cnt, string comments)
        {
            this.annotationTextCount_lbl.Text = cnt.ToString();
            this.Annotation_tbx.Text = comments;
            if(!string.IsNullOrEmpty(comments))
            this.ac.Comments = comments;
            this.ac.ID = cnt;
            this.Annotation_tbx.Focus();
            this.Annotation_tbx.SelectAll();
        }

        public void Annotation_UpdateLabels(int id)
        {
            this.annotationTextCount_lbl.Text = id.ToString();
        }

        public String Comments
        {
            set
            {
                comments = value;
             
            }
            get
            {
                return comments;
            }
        }
        public bool isActive
        {
            set
            {
                _mActive = value;
                 setBorder();
            }
            get
            {
                return _mActive;
            }
        }
        public int ID
        {
            set
            {
                _id = value;
                 setBorder();
            }
            get
            {
                return _id;
            }
        }
        private void setBorder()
        {
            if(_mActive)
            this.panel1.BackColor = Color.Aqua;
            else
            this.panel1.BackColor = Color.Black;
            this.tableLayoutPanel1.BackColor = Color.Black;
            this.Annotation_tbx.BackColor = Color.White;
            this.annotationTextCount_lbl.BackColor = Color.Black;
        }

        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Comments = Annotation_tbx.Text;
            //this.ac.Comments = Annotation_tbx.Text;
            //this.ac.DateTimeVal = DateTime.Now;
        }

        private void Annotation_tbx_TextChanged(object sender, EventArgs e)
        {
            Comments = Annotation_tbx.Text;
            this.ac.Comments = Annotation_tbx.Text;
            this.ac.DateTimeVal = DateTime.Now;
        }

        private void Annotation_tbx_MouseClick(object sender, MouseEventArgs e)
        {
            _id = Convert.ToInt32(annotationTextCount_lbl.Text);
            annotation_id(_id);
        }
        //This below text enter event has been added by darshan to solve defect no:0000528 on 06-08-2015
        private void Annotation_tbx_Enter(object sender, EventArgs e)
        {
            _id = Convert.ToInt32(annotationTextCount_lbl.Text);
            annotation_id(_id);
        }

        private void Annotation_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127)
                e.Handled = true;
            else if (Annotation_tbx.Lines.Length >= MAX_LINES && e.KeyChar == '\r')
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Deletion of the comments text box and the drawing itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _id = Convert.ToInt32(annotationTextCount_lbl.Text);
            annotation_id(_id,true);
        }

        //private void Annotation_tbx_MouseEnter(object sender, EventArgs e)
        //{
        //    _id = Convert.ToInt32(annotationTextCount_lbl.Text);
        //    annotation_id(_id);
        //}
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    if (g == null) return;
        //    if (this == null) return;

        //    int dw = this.Width;
        //    int dh = this.Height;
        //    int tw = this.Width - 8; // remove border, 4*4 
        //    int th = this.Height - 8; // remove border, 4*4 
        //    double zw = (tw / (double)dw);
        //    double zh = (th / (double)dh);
        //    double z = (zw <= zh) ? zw : zh;

        //    dw = (int)(dw * z);
        //    dh = (int)(dh * z);
        //    int dl = 4 + (tw - dw) / 2; // add border 2*2
        //    int dt = 4 + (th - dh) / 2; // add border 2*2

        //    // g.DrawRectangle(new Pen(Color.Gray), dl, dt, dw, dh);

        //    Pen p = new Pen(Color.LimeGreen, 8f);
        //    g.DrawRectangle(p, new Rectangle(4, 4, dw + 25, dh + 25));
        //    //g.DrawRectangle(p, new Rectangle(this.Location.X, this.Location.Y, dw + 2, dh + 2));
        //    //g.DrawRectangle(p, new Rectangle(dl - 4, dt - 4, dw + 2, dh + 2));

        //    //g.FillRectangle(Brushes.LimeGreen, dl - 8, dt - 8, this.Size.Width, this.Size.Height);
        //    //g.FillRectangle(Brushes.LimeGreen, new Rectangle(dl - 2, dt - 2, 10,10));
        //}

       

    }
}
