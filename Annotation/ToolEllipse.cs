using System;
using System.Windows.Forms;
using INTUSOFT.EventHandler;

namespace Annotation
{
	/// <summary>
	/// Ellipse tool
	/// </summary>
	public class ToolEllipse : ToolRectangle
	{
        IVLEventHandler _eventHandler;
		public ToolEllipse()
		{
            
            {
                Cursor = new Cursor(GetType(), "Ellipse.cur");
                if (_eventHandler == null)
                    _eventHandler = IVLEventHandler.getInstance();
            }
            
		}

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawEllipse(e.X, e.Y, 1, 1));
            Args arg = new Args();//Added by darshan on 25-07-2016 as per NR:0001211 Note no:(0002558)
            arg["Print"] = true;
            arg["Save"] = true;
            arg["Export"] = true;
            _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
        }
        /// <summary>
        /// AddExistingObject will add existing ellipse to drawarea.
        /// </summary>
        /// <param name="drawArea">drawarea is where the ellipse is drawn</param>
        /// <param name="s">shape class details will be in s</param>
        public void AddExistingObject(DrawArea drawArea, Shape s)
        {
            AddNewObject(drawArea, new DrawEllipse(s.StartPoint.X, s.StartPoint.Y, s.Width, s.Height));
            drawArea.ListGraphics.UnselectAll();
            drawArea.Refresh();

        }
	}
}
