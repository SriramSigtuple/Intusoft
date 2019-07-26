using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Annotation
{
	/// <summary>
	/// Line graphic object
	/// </summary>
	public class DrawLine : DrawObject
	{
        public Point startPoint;
        public Point endPoint;

        private const string entryStart = "Start";
        private const string entryEnd = "End";

        /// <summary>
        ///  Graphic objects for hit test
        /// </summary>
        private GraphicsPath areaPath = null;
        private Pen areaPen = null;
        private Region areaRegion = null;


		public DrawLine()
		{
            startPoint.X = 0;
            startPoint.Y = 0;
            endPoint.X = 1;
            endPoint.Y = 1;

            Initialize();
		}

        public DrawLine(int x1, int y1, int x2, int y2)
        {
            startPoint.X = x1;
            startPoint.Y = y1;
            endPoint.X = x2;
            endPoint.Y = y2;
            
            Initialize();
        }


        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color, 5);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            Shape.StartPoint.X = startPoint.X;
            Shape.StartPoint.Y = startPoint.Y;
            Shape.EndPoint.X = endPoint.X;
            Shape.EndPoint.Y = endPoint.Y;
            Shape._shapeType = ShapeType.Annotation_Line;
            pen.Dispose();
        }

        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }
        Image<Bgr, byte> inp;
        //This below method was added by Darshan on 19-10-2015(Description given below).
        /// <summary>
        /// This method will draw the graph on the panel to the image.
        /// </summary>
        /// <param name="srcBitmap">Image name</param>
        public void DrawLineToBitmap(ref Bitmap srcBitmap, Common.PicBoxSizeMode sizeMode = Common.PicBoxSizeMode.Zoom)
        {
            Common.CommonMethods _common = Common.CommonMethods.CreateInstance();
            if(sizeMode == Common.PicBoxSizeMode.Zoom)
            inp = new Image<Bgr, byte>(Form1.tempBm.Width, Form1.tempBm.Height);
            else
                inp = new Image<Bgr, byte>(Form1.pbxWidth, Form1.pbxHeight);

            List<Point> TempPointArr = new List<Point>();
            _common.Image = inp.ToBitmap();
            _common.Height = inp.Width;
            _common.Width = inp.Height;
            Point actualImageLineStartPoint = _common.TranslatePointToImageCoordinates(new Point(startPoint.X, startPoint.Y), sizeMode);
            Point actualImageLineEndPoint = _common.TranslatePointToImageCoordinates(new Point(endPoint.X, endPoint.Y), sizeMode);

            if (sizeMode != Common.PicBoxSizeMode.Zoom)
            {

                actualImageLineStartPoint.X = Convert.ToInt32((float)actualImageLineStartPoint.X * (float)srcBitmap.Width / (float)inp.Width);
                actualImageLineStartPoint.Y = Convert.ToInt32((float)actualImageLineStartPoint.Y * (float)srcBitmap.Height / (float)inp.Height);
                actualImageLineEndPoint.X = Convert.ToInt32((float)actualImageLineEndPoint.X * (float)srcBitmap.Width / (float)inp.Width);
                actualImageLineEndPoint.Y = Convert.ToInt32((float)actualImageLineEndPoint.Y * (float)srcBitmap.Height / (float)inp.Height);
            }
            
            Pen pen = new Pen(Color.FromName(AnnotationVariables.annotationMarkingColor), 10);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            Bitmap bm = inp.ToBitmap();
            Graphics g = Graphics.FromImage(srcBitmap);
            g.DrawLine(pen, actualImageLineStartPoint.X, actualImageLineStartPoint.Y, actualImageLineEndPoint.X, actualImageLineEndPoint.Y);
            g.Dispose();

        }
        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            if ( handleNumber == 1 )
                return startPoint;
            else
                return endPoint;
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
                if (PointInObject(point))
                    return 0;
            return -1;
        }

        //This below method has been added by darshan to solve defect no:0000514
        public override int PointContainer(Point point)
        {
            if (PointInObject(point))
                            return 0;
                        else
                            return -1;
        }

        protected override bool PointInObject(Point point)
        {
            {
                CreateObjects();
                if (AreaRegion != null)
              return AreaRegion.IsVisible(point);
            }
            return true;
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            CreateObjects();
            return AreaRegion.IsVisible(rectangle);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;
                default:
                    return Cursors.Default;
            }
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if ( handleNumber == 1 )
                startPoint = point;
            else
                endPoint = point;
            Invalidate();
        }

        public override void Move(int deltaX, int deltaY)
        {
            startPoint.X += deltaX;
            startPoint.Y += deltaY;

            endPoint.X += deltaX;
            endPoint.Y += deltaY;

            Invalidate();
        }

        public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryStart, orderNumber),
                startPoint);

            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryEnd, orderNumber),
                endPoint);

            base.SaveToStream (info, orderNumber);
        }

        public override void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            startPoint = (Point)info.GetValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryStart, orderNumber),
                typeof(Point));

            endPoint = (Point)info.GetValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryEnd, orderNumber),
                typeof(Point));
            base.LoadFromStream (info, orderNumber);
        }

        /// <summary>
        /// Invalidate object.
        /// When object is invalidated, path used for hit test
        /// is released and should be created again.
        /// </summary>
        protected void Invalidate()
        {
            if ( AreaPath != null )
            {
                AreaPath.Dispose();
                AreaPath = null;
            }

            if ( AreaPen != null )
            {
                AreaPen.Dispose();
                AreaPen = null;
            }

            if ( AreaRegion != null )
            {
                AreaRegion.Dispose();
                AreaRegion = null;
            }
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>
        protected virtual void CreateObjects()
        {
            if ( AreaPath != null )
                return;
            // Create path which contains wide line
            // for easy mouse selection
            if (startPoint.X != endPoint.X && startPoint.Y != endPoint.Y)
            {
                {
                    AreaPath = new GraphicsPath();
                    AreaPen = new Pen(Color.Aquamarine, 20);
                    {
                        AreaPath.AddLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                        AreaPath.Widen(AreaPen);
                    }
                    // Create region from the path
                    AreaRegion = new Region(AreaPath);
                }
            }
        }

        protected GraphicsPath AreaPath
        {
            get
            {
                return areaPath;
            }
            set
            {
                areaPath = value;
            }
        }

        protected Pen AreaPen
        {
            get
            {
                return areaPen;
            }
            set
            {
                areaPen = value;
            }
        }

        protected Region AreaRegion
        {
            get
            {
                return areaRegion;
            }
            set
            {
                areaRegion = value;
            }
        }
	}
}
