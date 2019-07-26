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
namespace INTUSOFT.Desktop.Forms
{
    public partial class DiagnosisUC : UserControl
    {
        public delegate void RemoveDiagnosisDelegate(DiagnosisUC val);
        public event RemoveDiagnosisDelegate RemoveDiagnosisEvent;
        public DiagnosisUC(string text)
        {
            InitializeComponent();
            this.toolStrip1.Renderer = new FormToolStripRenderer();
            DiagnosisLblText = text;
            diagnosis_lbl.Text = DiagnosisLblText;
            diagnosis_lbl.ForeColor = Color.Black;
        }

        private string diagnosisLblText;

        public string DiagnosisLblText
        {
            get { return diagnosisLblText; }
            set { diagnosisLblText = value; }
        }
        private void removeDiagnosis_btn_Click(object sender, EventArgs e)
        {
            RemoveDiagnosisEvent(this);
           
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
        internal string GetDiagnosis()
        {
          return this.diagnosis_lbl.Text;
        }
        private void DiagnosisUC_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path = RoundedRect(Bounds, 10);
            this.Region = new Region(path);
        }
    }
    public class FormToolStripRenderer : ToolStripRenderer
    {
        public bool isButtonClicked = false;
        public FormToolStripRenderer()
        {

        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {

            if (e.ToolStrip.GetType() == typeof(ToolStrip))
            {
                // skip render border
            }
            else
            {
                // do render border
                base.OnRenderToolStripBorder(e);
            }

        }
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected)
            {
                base.OnRenderButtonBackground(e);
            }

            else
            {
                Brush b = new SolidBrush(Color.FromArgb(90, 255, 255, 255));

                Rectangle rectangle = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height - 1);
                e.Graphics.FillRectangle(b, rectangle);

            }
        }

    }
}
