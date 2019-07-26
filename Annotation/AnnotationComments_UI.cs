using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotation
{
    public partial class AnnotationComments_UI : UserControl
    {
        public AnnotationComments_UI()
        {
            InitializeComponent();
        }
        public void AddAnnotationText(AnnotationText c)
        {
            //this.flowLayoutPanel1.Controls.Add(c);
        }
      AnnotationText at;
        int Id;
        string comments;
        

        public void addControl2FLP(int cnt,string comments)
        {
            this.comments = comments;
            at = new AnnotationText(); 
            at.MouseClick += at_MouseClick;
            at.Size = new Size(120,45);
            at.Dock = DockStyle.Top;
            at.ID = cnt;
            //at.isActive = false;
            at.BackColor = Color.Black;
            at.setValues(cnt, comments);
            this.annotationFlowLayout1.Controls.Add(at);
            //this.annotationFlowLayout1.Controls.SetChildIndex(at, 0);
        }

        void at_MouseClick(object sender, MouseEventArgs e)
        {
            
            this.Select();
            //Id = Convert.ToInt32(at.annotationTextCount_lbl.Text);
            at = sender as AnnotationText;
            //at.isActive = true;
           
        }

       
        public void removeControl()
        {
            int i = this.annotationFlowLayout1.Controls.Count;
            foreach (AnnotationText item in annotationFlowLayout1.Controls)
            {
                if (this.annotationFlowLayout1.Controls.IndexOf(item) == i - 1)
                    this.annotationFlowLayout1.Controls.RemoveAt(i - 1);
            }
            
        }
    }
    
    }

