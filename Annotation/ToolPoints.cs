using System;
using System.Windows.Forms;
using System.Drawing;


namespace Annotation
{
	/// <summary>
	/// Polygon tool
	/// </summary>
	public class ToolPoints : ToolObject
	{
		public ToolPoints()
		{
            Cursor = new Cursor(GetType(), "Pencil.cur");
        }
        public static bool islastPointConnect = false;
        private int lastX;
        private int lastY;
        private DrawPolygon newPolygon;
        private const int minDistance = 25*25;// Changed the distance from 15* 15 to 20* 20 so as to show less number of hit points by sriram on september 1st 2015

        /// <summary>
        /// Left nouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            // Create new polygon, add it to the list
            // and keep reference to it
            newPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
            newPolygon.isCup = DrawArea.isDrawCup;
            AddNewObject(drawArea, newPolygon);
            lastX = e.X;
            lastY = e.Y;
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;
            int shiftVal = 0;
            if (e.Button != MouseButtons.Left)
                return;

            //if ( newPolygon == null )
            //    return;                 // precaution

            //Point point = new Point(e.X + shiftVal, e.Y + shiftVal);
            //int distance = (e.X + shiftVal - lastX) * (e.X + shiftVal - lastX) + (e.Y + shiftVal - lastY) * (e.Y + shiftVal - lastY);

            //if ( distance < minDistance )
            //{
            //    // Distance between last two points is less than minimum -
            //    // move last point
            //    newPolygon.MoveHandleTo(point, newPolygon.HandleCount);
            //}
            //else
            //{
            //    // Add new point
            //    newPolygon.AddPoint(point);
            //    lastX = e.X + shiftVal;
            //    lastY = e.Y + shiftVal;
            //}

            //drawArea.Refresh();
          
        }
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            islastPointConnect = true;
            //drawArea.Refresh();
            //newPolygon.ConnectFirstAndLastPoint(
            newPolygon = null;
               
            base.OnMouseUp (drawArea, e);
        }

       

	}
}
