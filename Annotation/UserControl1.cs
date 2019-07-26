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
    public partial class UserControl1 : UserControl
    {
        AnnotationText at;
        public List<int> ids;
       public  int activeId;
       public delegate void ActiveannotationId(int id,bool isDelete = false);
       public event ActiveannotationId _annotationactiveid;
       public delegate void AnnotationIds(List<int> args);
       public event AnnotationIds _annotationids;
        string comments;
        public UserControl1()
        {
            InitializeComponent();
        }
        //public void Set_annotationComments(int cnt, string comments)
        //{
        //    this.comments = comments;
        //    at = new AnnotationText();
        //    at.MouseClick += at_MouseClick;
        //    at.Size = new Size(250, 100);
        //    at.Dock = DockStyle.Top;
        //    at.ID = cnt;
        //    at.setValues(cnt, comments);
        //    this.class11.Controls.Add(at);
                  
        //}
        public void addControl2FLP(int cnt,string comments)
        {
            this.comments = comments;
            at = new AnnotationText();
            at.annotation_id += at_annotation_id;
            at.MouseClick += at_MouseClick;
                    if (Screen.PrimaryScreen.Bounds.Width == 1280)
                                at.Size = new Size(235,75);
                    else
                        if (Screen.PrimaryScreen.Bounds.Width == 1366)
                            at.Size = new Size(220, 60);
                        else
                            if (Screen.PrimaryScreen.Bounds.Width == 1920)
                                at.Size = new Size(250, 50);

          
            at.Dock = DockStyle.Top;
            at.ID = cnt;
            if (!isIdExists(at.ID))
            {
                at.setValues(cnt, comments);
                this.class11.Controls.Add(at);
                //this.class11.Controls.SetChildIndex(at, 0);
                Highlight_control();
            }   

        }

        void at_annotation_id(int id,bool isDelete = false)
        {
            activeId = id;
            Highlight_control();
            _annotationactiveid(id,isDelete);

        }
        public List<AnnotationComments> Get_annotationComments()
        {
            List<AnnotationComments> retList = new List<AnnotationComments>();
            foreach (AnnotationText item in class11.Controls)
            {
                retList.Add(item.ac);
            }
            return retList;
        }
        void at_MouseClick(object sender, MouseEventArgs e)
        {
            
            this.Select();
            //Id = Convert.ToInt32(at.annotationTextCount_lbl.Text);
            at = sender as AnnotationText;
            at.isActive = true;
           
        }

        public void Release_Flp()
        {
            this.class11.Controls.Clear();
        }
        public void removeControl(int id)
        {
            int i = this.class11.Controls.Count;
            foreach (AnnotationText item in class11.Controls)
            {
                if(item.ID==id)
                    this.class11.Controls.Remove(item); ;
            
            }
            ids=new List<int>();
            foreach (AnnotationText item in class11.Controls)
            {
                ids.Add(item.ID);

            }
           
        }
        public bool isIdExists(int id)
        {
            bool idExists = false;
            foreach (AnnotationText item in class11.Controls)
            {
                if (item.ID == id)
                {
                    idExists = true;
                    break;
                }
                else
                    idExists = false;

            }
            return idExists;
        }
        public void Update_Labels(int[] b)
        {
            //at = new AnnotationText();
            //for (int i = 0; i < b.Length; i++)
            //{
            //    at.Annotation_UpdateLabels(b[i]); 

            //}
            int count = 0;
            foreach (AnnotationText item in class11.Controls)
            {
               item.ID = b[count];
               item.ac.ID = b[count];
                item.annotationTextCount_lbl.Text = b[count].ToString();
                count++;
           
            }
       
        }
        public void unselectall()
        {
            foreach (AnnotationText item in class11.Controls)
            {
               
                    item.isActive = false;
            }
       
        }
        public void Highlight_control()
        {
            foreach (AnnotationText item in class11.Controls)
            {
                if (item.ID == activeId)
                    item.isActive = true;
                else
                    item.isActive = false;
            }
       
        }
        public void Disable_control(bool isTbxEnable)
        {
            foreach (AnnotationText item in class11.Controls)
            {
                item.annotationTextCount_lbl.Enabled = true;
                item.Annotation_tbx.Enabled = isTbxEnable;
            }
        }
    }
    
}
