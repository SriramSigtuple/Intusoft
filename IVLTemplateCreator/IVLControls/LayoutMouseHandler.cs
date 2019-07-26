using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using ReportUtils;
namespace IVLTemplateCreator.IVLControls
{
   static class LayoutMouseHandler
    {
        static Rectangle boundary = new Rectangle();
        static Point moveOffset = Point.Empty;
        static bool isResize = false;
        static int MOVE_STEP = 4;
        public static void OnMouseDown(Control c, MouseEventArgs e)
        {
            boundary = new Rectangle(LayoutDetails.LeftMargin,LayoutDetails.TopMargin,LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight);
            moveOffset = new Point(e.X, e.Y);
        }
        public static void OnMouseUp(Control c, MouseEventArgs e)
        {
            moveOffset = Point.Empty;
        }
        public static void OnMouseMove(Control c, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (moveOffset != Point.Empty)
                {
                    if (isResize)
                    {
                       c.Width = (e.X / MOVE_STEP) * MOVE_STEP;
                       c.Height = (e.Y / MOVE_STEP) * MOVE_STEP;
                    }
                    else
                    {
                        Point newlocation = c.Location;
                        newlocation.X += e.X - moveOffset.X;
                        newlocation.Y += e.Y - moveOffset.Y;
                        if (newlocation.X <= boundary.X || newlocation.X + c.Width > boundary.Width || newlocation.Y <= boundary.Y || newlocation.Y + c.Height >= boundary.Height)
                        {
                            return;
                        }
                        else
                        {
                            c.Location = newlocation;
                        }
                    }
                }
            }
            else
            {
                if ((e.X > c.Width - 10) && (e.Y > c.Height - 10))
                {
                    isResize = true;
                    c.Cursor = Cursors.SizeNWSE;
                }
                else
                {
                    isResize = false;
                    c.Cursor = Cursors.SizeAll;
                }
            }
        }
    }
}
