using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;


namespace Annotation
{
	/// <summary>
	/// Base class for all tools which create new graphic object
	/// </summary>G:\Projects\intusoft_sqlitedbBranch\Annotation\ToolObject.cs
	public abstract class ToolObject : Tool
	{
        int idVal = 0;
        private Cursor cursor;
        /// <summary>
        /// Tool cursor.
        /// </summary>
        protected Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }


        /// <summary>
        /// Left mouse is released.
        /// New object is created and resized.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
       
        static AnnotationText c;
        int AnnotationId = 0;
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {

            c = new AnnotationText();
            //this if condition has been added by darshan to resolve defect no 0000539.
            
            if (drawArea.ListGraphics.Count > 0)
            {
                for (int i = 0; i < drawArea.ListGraphics.Count; i++)
                {
                    //if (drawArea.ListGraphics[i] is DrawLine)
                    if (drawArea.ListGraphics[i].GetType().ToString().Equals("Annotation.DrawLine"))
                    {

                    }
                    else
                    {

                        idVal = drawArea.ListGraphics.ids.ElementAtOrDefault(i);
                     if(idVal !=0)
                     {
                       c.ID = drawArea.ListGraphics.ids[i];
                        drawArea.ListGraphics[0].Normalize();
                        drawArea.CommentsAdded(c);
                     }
                    }
                }
             
                //c.Dock = DockStyle.Top;
                //c.SetBounds(c.Location.X, c.Location.Y, c.Width, c.Height);
                //}
                drawArea.ActiveTool = DrawArea.DrawToolType.Pointer;
                //drawArea.Capture = false;
                //annotationPanel.Location = drawArea.active_point;
                //c.ac.X = annotationPanel.Location.X;
                //c.ac.Y = annotationPanel.Location.Y;
                //c.Show();
                //drawArea.comments.Add(c.ac);
                 drawArea.Refresh();
            }
            //drawArea.Refresh();
        }
        public static void text_move(DrawArea drawArea, MouseEventArgs e) 
        {
           c.Location = drawArea.active_point;
             drawArea.Refresh();
        }
         /// <summary>
        /// Add new object to draw area.
        /// Function is called when user left-clicks draw area,
        /// and one of ToolObject-derived tools is active.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="o"></param>
        protected void AddNewObject(DrawArea drawArea, DrawObject o)
        {
            //if (!o.GetType().ToString().Equals("Annotation.DrawLine"))
                drawArea.ListGraphics.UnselectAll();
                o.Selected = true;
                 drawArea.ListGraphics.Add(o);
                //o.ID = drawArea.ListGraphics.Count;
                drawArea.Capture = true;
                drawArea.Refresh();
            
           

           // drawArea.SetDirty();
        }
	}
}
