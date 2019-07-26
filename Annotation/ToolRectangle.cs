using System;
using System.Windows.Forms;
using System.Drawing;
using INTUSOFT.EventHandler;


namespace Annotation
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	public class ToolRectangle : ToolObject
	{
        IVLEventHandler _eventHandler;
		public ToolRectangle()
		{
            
            {
                Cursor = new Cursor(GetType(), "Rectangle.cur");
                if (_eventHandler == null)
                    _eventHandler = IVLEventHandler.getInstance();
            }
           
            
		}

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawRectangle(e.X, e.Y, 1, 1));
            Args arg = new Args();
            arg["Print"] = true;
            arg["Save"] = true;
            arg["Export"] = true;
            _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
        }
        /// <summary>
        /// AddExistingObject will add existing rectangle to drawarea.
        /// </summary>
        /// <param name="drawArea">drawarea is where the rectangle is drawn</param>
        /// <param name="s">shape class details will be in s</param>
        public void AddExistingObject(DrawArea drawArea, Shape s )
        {
            AddNewObject(drawArea, new DrawRectangle(s.StartPoint.X, s.StartPoint.Y, s.Width, s.Height));
            drawArea.ListGraphics.UnselectAll();
            drawArea.Refresh();

        }
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;
            //this if condition has been added by darshan to resolve defect no 0000539.

            if (drawArea.ListGraphics.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point point = new Point(e.X, e.Y);
                    drawArea.active_point = point;
                    drawArea.ListGraphics[0].MoveHandleTo(point, 5);
                    drawArea.Refresh();
                }
            }
        }
	}
}
