using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
using System.Runtime.Serialization;

namespace Annotation
{
	/// <summary>
	/// Rectangle graphic object
	/// </summary>
	public class DrawRectangle :DrawObject
	{
        public Rectangle rectangle;
        public static int right_m;
        public static int bottom_m;
        private const string entryRectangle = "Rect";
       public ArrayList pointArray;

        protected Rectangle Rectangle
        {
            get
            {
                return rectangle;
            }
            set
            {
                rectangle = value;
            }
        }
        
		public DrawRectangle()
		{
            SetRectangle(0, 0, 1,1 );
            Initialize();
		}

        Image<Bgr, byte> inp;
        public DrawRectangle(int x, int y, int width, int height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            this.pointArray = new ArrayList();
            this.pointArray.Add(new Point(x, y));
            rectangle.Width = width;      
            rectangle.Height = height;
           
            {
                Shape.StartPoint = new Point(x, y);
                Shape.Width = width;
                Shape.Height = height;
                Shape._shapeType = ShapeType.Annotation_Rectangle;
            }
            
            Initialize();
        }
        //This below method was added by Darshan on 19-10-2015(Description given below).
        /// <summary>
        /// This method will draw the graph on the panel to the image.
        /// </summary>
        /// <param name="srcBitmap">Image name</param>
        public void DrawRectangleToBitmap(ref Bitmap srcBitmap,int id,Common.PicBoxSizeMode sizeMode = Common.PicBoxSizeMode.Zoom)
        {
            SolidBrush brush = new SolidBrush(Color.FromName(AnnotationVariables.annotationMarkingColor));

            Common.CommonMethods _common = Common.CommonMethods.CreateInstance();
            if(sizeMode == Common.PicBoxSizeMode.Zoom)
            inp = new Image<Bgr, byte>(Form1.tempBm.Width, Form1.tempBm.Height);
            else
            inp = new Image<Bgr, byte>(Form1.pbxWidth, Form1.pbxHeight);

            List<Point> TempPointArr = new List<Point>();
            _common.Image = inp.ToBitmap();


                _common.Height = Form1.pbxHeight;
                _common.Width = Form1.pbxWidth;


            Point actualImageRectStartPoint =  _common.TranslatePointToImageCoordinates(new Point(rectangle.X, rectangle.Y), sizeMode);
            Point actualImageRectWidthPoint =  _common.TranslatePointToImageCoordinates(new Point(rectangle.X + rectangle.Width, rectangle.Y), sizeMode);
            Point actualImageRectHeightPoint = _common.TranslatePointToImageCoordinates(new Point(rectangle.X, rectangle.Y + rectangle.Height), sizeMode);

            if (sizeMode != Common.PicBoxSizeMode.Zoom)
            {

                actualImageRectStartPoint.X = Convert.ToInt32((float)actualImageRectStartPoint.X * (float)srcBitmap.Width / (float)inp.Width);
                actualImageRectStartPoint.Y = Convert.ToInt32((float)actualImageRectStartPoint.Y * (float)srcBitmap.Height / (float)inp.Height);
                actualImageRectWidthPoint.X = Convert.ToInt32((float)actualImageRectWidthPoint.X * (float)srcBitmap.Width / (float)inp.Width);
                actualImageRectWidthPoint.Y = Convert.ToInt32((float)actualImageRectWidthPoint.Y * (float)srcBitmap.Height / (float)inp.Height);
                actualImageRectHeightPoint.X = Convert.ToInt32((float)actualImageRectHeightPoint.X * (float)srcBitmap.Width / (float)inp.Width);
                actualImageRectHeightPoint.Y = Convert.ToInt32((float)actualImageRectHeightPoint.Y * (float)srcBitmap.Height / (float)inp.Height);
            }
            int width = (int)Math.Sqrt(Math.Pow(((double)actualImageRectStartPoint.X - (double)actualImageRectWidthPoint.X), 2));
            int height = (int)Math.Sqrt(Math.Pow(((double)actualImageRectStartPoint.Y - (double)actualImageRectHeightPoint.Y), 2));
            int x, y, xCenter, yCenter;

            xCenter = actualImageRectStartPoint.X + width / 2;
            yCenter = actualImageRectStartPoint.Y + height / 2;
            x = actualImageRectStartPoint.X;
            y = actualImageRectStartPoint.Y;
            Bitmap bm = inp.ToBitmap();
            Graphics g = Graphics.FromImage(srcBitmap);
        
            g.DrawRectangle(new Pen(Color.FromName(AnnotationVariables.annotationMarkingColor), 9.0f), new Rectangle(actualImageRectStartPoint, new Size(width, height)));
              g.DrawString(id.ToString(), new Font(FontFamily.GenericSansSerif, 43.2f, FontStyle.Bold, GraphicsUnit.Pixel), brush, new PointF(xCenter - 5, y - 60));

            g.Dispose();
             

        }
       


        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);
            GraphicsPath path = new GraphicsPath();
            path = MakeRoundedRect(Rectangle, 10, 10, true, true, true, true);
            g.DrawPath(pen, path);
            //g.DrawRectangle(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));

            pen.Dispose();
        }


        private GraphicsPath MakeRoundedRect(
    RectangleF rect, float xradius, float yradius,
    bool round_ul, bool round_ur, bool round_lr, bool round_ll)
        {
            // Make a GraphicsPath to draw the rectangle.
            PointF point1, point2;
            GraphicsPath path = new GraphicsPath();

            // Upper left corner.
            if (round_ul)
            {
                RectangleF corner = new RectangleF(
                    rect.X, rect.Y,
                    2 * xradius, 2 * yradius);
                path.AddArc(corner, 180, 90);
                point1 = new PointF(rect.X + xradius, rect.Y);
            }
            else point1 = new PointF(rect.X, rect.Y);

            // Top side.
            if (round_ur)
                point2 = new PointF(rect.Right - xradius, rect.Y);
            else
                point2 = new PointF(rect.Right, rect.Y);
            path.AddLine(point1, point2);

            // Upper right corner.
            if (round_ur)
            {
                RectangleF corner = new RectangleF(
                    rect.Right - 2 * xradius, rect.Y,
                    2 * xradius, 2 * yradius);
                path.AddArc(corner, 270, 90);
                point1 = new PointF(rect.Right, rect.Y + yradius);
            }
            else point1 = new PointF(rect.Right, rect.Y);

            // Right side.
            if (round_lr)
                point2 = new PointF(rect.Right, rect.Bottom - yradius);
            else
                point2 = new PointF(rect.Right, rect.Bottom);
            path.AddLine(point1, point2);

            // Lower right corner.
            if (round_lr)
            {
                RectangleF corner = new RectangleF(
                    rect.Right - 2 * xradius,
                    rect.Bottom - 2 * yradius,
                    2 * xradius, 2 * yradius);
                path.AddArc(corner, 0, 90);
                point1 = new PointF(rect.Right - xradius, rect.Bottom);
            }
            else point1 = new PointF(rect.Right, rect.Bottom);

            // Bottom side.
            if (round_ll)
                point2 = new PointF(rect.X + xradius, rect.Bottom);
            else
                point2 = new PointF(rect.X, rect.Bottom);
            path.AddLine(point1, point2);

            // Lower left corner.
            if (round_ll)
            {
                RectangleF corner = new RectangleF(
                    rect.X, rect.Bottom - 2 * yradius,
                    2 * xradius, 2 * yradius);
                path.AddArc(corner, 90, 90);
                point1 = new PointF(rect.X, rect.Bottom - yradius);
            }
            else point1 = new PointF(rect.X, rect.Bottom);

            // Left side.
            if (round_ul)
                point2 = new PointF(rect.X, rect.Y + yradius);
            else
                point2 = new PointF(rect.X, rect.Y);
            path.AddLine(point1, point2);

            // Join with the start point.
            path.CloseFigure();

            return path;
        }

        protected void SetRectangle(int x, int y, int width, int height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Shape.StartPoint = new Point(x, y);
            Shape.Width = width;
            Shape.Height = height;
            if(Shape._shapeType != ShapeType.Annotation_Rectangle)
               Shape._shapeType = ShapeType.Annotation_Rectangle;

        }


        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }


        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            int x, y, xCenter, yCenter;

            xCenter = rectangle.X + rectangle.Width/2;
            yCenter = rectangle.Y + rectangle.Height/2;
            x = rectangle.X;
            y = rectangle.Y;

            switch ( handleNumber )
            {
                case 1:
                    x = rectangle.X;
                    y = rectangle.Y;
                    break;
                case 2:
                    x = xCenter;
                    y = rectangle.Y;
                    break;
                case 3:
                    x = rectangle.Right;
                    y = rectangle.Y;
                    break;
                case 4:
                    x = rectangle.Right;
                    y = yCenter;
                    break;
                case 5:
                    x = rectangle.Right;
                    y = rectangle.Bottom;
                    break;
                case 6:
                    x = xCenter;
                    y = rectangle.Bottom;
                    break;
                case 7:
                    x = rectangle.X;
                    y = rectangle.Bottom;
                    break;
                case 8:
                    x = rectangle.X;
                    y = yCenter;
                    break;
            }

            return new Point(x, y);

        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int HitTest(Point point)
        {
            if ( Selected )
            {
                for ( int i = 1; i <= HandleCount; i++ )
                {
                    if ( GetHandleRectangle(i).Contains(point) )
                        return i;
                }
            }

            if ( PointInObject(point) )
                return 0;

            return -1;
        }
        //This below method has been added by darshan to solve defect no:0000514
        public override int PointContainer(Point point)
        {
            int a = rectangle.X + rectangle.Width;
            int b = rectangle.Y + rectangle.Height;
            if (point.X >= rectangle.X && point.X <= a && point.Y == rectangle.Y)// top line of the rectangle line from (x,y)  to (x+ w,y)
                return 0;
            else
                if (point.X == rectangle.X && point.Y <= b && point.Y >= rectangle.Y) // left most line of the rectangle (x,y)  to (x, y + h)
                    return 0;
                else
                    if (point.X >= rectangle.X && point.X < a && point.Y == b) // bottom most line of the rectangle (x,y+h)  to (x+w, y + h)
                        return 0;
                    else
                        if (point.X == a && point.Y <= b && point.Y >= rectangle.Y) // left most line of the rectangle (x+w,y)  to (x+w, y + h)
                            return 0;
                        else if (rectangle.Contains(point))
                            return 0;
                        else
                    return -1;
       
        }
        
        protected override bool PointInObject(Point point)
        {
            return rectangle.Contains(point);
        }
        


        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                    return Cursors.SizeNWSE;
                case 2:
                    return Cursors.SizeNS;
                case 3:
                    return Cursors.SizeNESW;
                case 4:
                    return Cursors.SizeWE;
                case 5:
                    return Cursors.SizeNWSE;
                case 6:
                    return Cursors.SizeNS;
                case 7:
                    return Cursors.SizeNESW;
                case 8:
                    return Cursors.SizeWE;
                default:
                    return Cursors.Default;
            }
        }

        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            int left = Rectangle.Left;
            int top = Rectangle.Top;
            int right = Rectangle.Right;
            int bottom = Rectangle.Bottom;

            switch ( handleNumber )
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;
                case 2:
                    top = point.Y;
                    break;
                case 3:
                    right = point.X;
                    top = point.Y;
                    break;
                case 4:
                    right = point.X;
                    break;
                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;
                case 6:
                    bottom = point.Y;
                    break;
                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;
                case 8:
                    left = point.X;
                    break;
            }

            SetRectangle(left, top, right - left, bottom - top);
        }


        public override bool IntersectsWith(Rectangle rectangle)
        {
            return Rectangle.IntersectsWith(rectangle);
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY)
        {
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
        }

        public override void Dump()
        {
            base.Dump ();

            Trace.WriteLine("rectangle.X = " + rectangle.X.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Y = " + rectangle.Y.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Width = " + rectangle.Width.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Height = " + rectangle.Height.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = DrawRectangle.GetNormalizedRectangle(rectangle);
        }

        /// <summary>
        /// Save objevt to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryRectangle, orderNumber),
                rectangle);

            base.SaveToStream (info, orderNumber);
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public override void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            rectangle = (Rectangle)info.GetValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryRectangle, orderNumber),
                typeof(Rectangle));

            base.LoadFromStream (info, orderNumber);
        }

     

        #region Helper Functions

        public static Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if ( x2 < x1 )
            {
                int tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if ( y2 < y1 )
            {
                int tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }

        #endregion

    }
}
