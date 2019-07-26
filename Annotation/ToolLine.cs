using System;
using System.Windows.Forms;
using System.Drawing;
using INTUSOFT.EventHandler;

namespace Annotation
{
	/// <summary>
	/// Line tool
	/// </summary>
	public class ToolLine : ToolObject
	{
        IVLEventHandler _eventHandler;
        public ToolLine()
        {
            Cursor = new Cursor(GetType(), "Line.cur");
            if (_eventHandler == null)
                _eventHandler = IVLEventHandler.getInstance();
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawLine(e.X, e.Y, e.X + 1, e.Y + 1));
            Args arg = new Args();//Added by darshan on 25-07-2016 as per NR:0001211 Note no:(0002558)
            arg["Print"] = true;
            arg["Save"] = true;
            arg["Export"] = true;
            _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
        }
        /// <summary>
        /// AddExistingObject will add existing line to drawarea.
        /// </summary>
        /// <param name="drawArea">drawarea is where the line is drawn</param>
        /// <param name="s">shape class details will be in s</param>
        public void AddExistingObject(DrawArea drawArea, Shape s)
        {
            AddNewObject(drawArea, new DrawLine(s.StartPoint.X, s.StartPoint.Y, s.EndPoint.X, s.EndPoint.Y));
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
                    drawArea.ListGraphics[0].MoveHandleTo(point, 2);
                    drawArea.Refresh();
                }
            }
        }
    }
}
