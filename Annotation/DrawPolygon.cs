using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Generic;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Linq;
namespace Annotation
{
	/// <summary>
	/// Polygon graphic object
	/// </summary>
	public class DrawPolygon :DrawLine
	{
        public ArrayList pointArray;         // list of points
        private Cursor handleCursor;
        public bool isCup;

        private const string entryLength = "Length";
        private const string entryPoint = "Point";
        static Bitmap discImage, cupImage;
        [DllImport("GlaucomaToolMeasurements.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CDRInit")]
        private static extern void CDRInit(int width, int height);
        [DllImport("GlaucomaToolMeasurements.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CDRExit")]
        private static extern void CDRExit();
        [DllImport("GlaucomaToolMeasurements.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CalculateCDR")]
        private static extern IntPtr CalculateCDR(IntPtr DiscImagePtr, IntPtr CupImagePtr);

        Image<Bgr, byte> inp;
		public DrawPolygon()
		{
            pointArray = new ArrayList();
                                   
            LoadCursor();
            Initialize();
		}
        //This below method was added by Darshan on 19-10-2015(Description given below).
        /// <summary>
        /// This method will draw the graph on the panel to the image.
        /// </summary>
        /// <param name="srcBitmap">Image name</param>
        public void DrawPolygonToImage(ref Bitmap srcBitmap,int id,Common.PicBoxSizeMode sizeMode = Common.PicBoxSizeMode.Zoom)
        {
            SolidBrush brush = new SolidBrush(Color.FromName(AnnotationVariables.annotationMarkingColor));

            Common.CommonMethods _common = Common.CommonMethods.CreateInstance();
            int width = 0;
            int height = 0;
            if (AnnotationVariables.isGlaucomaTool)
            {
                inp = new Image<Bgr, byte>(GlaucomaTool.tempBm.Width, GlaucomaTool.tempBm.Height);

                if (sizeMode == Common.PicBoxSizeMode.Zoom)
                    inp = new Image<Bgr, byte>(GlaucomaTool.tempBm.Width, GlaucomaTool.tempBm.Height);
                else
                    inp = new Image<Bgr, byte>(GlaucomaTool.pbxWidth, GlaucomaTool.pbxHeight);
                _common.Height = GlaucomaTool.pbxHeight;
                _common.Width = GlaucomaTool.pbxWidth;
            }
            else
            {
                if (sizeMode == Common.PicBoxSizeMode.Zoom)
                    inp = new Image<Bgr, byte>(Form1.tempBm.Width, Form1.tempBm.Height);
                else
                    inp = new Image<Bgr, byte>(Form1.pbxWidth, Form1.pbxHeight);
                _common.Height = Form1.pbxHeight;
                _common.Width = Form1.pbxWidth;
            }
            List<Point> TempPointArr = new List<Point>();
            _common.Image = inp.ToBitmap();
           
           
            for (int i = 0; i < this.pointArray.Count; i++)
            {
                Point actualImageRectStartPoint = _common.TranslatePointToImageCoordinates((Point)this.pointArray[i], sizeMode); 
                if (sizeMode != Common.PicBoxSizeMode.Zoom)
                {

                    actualImageRectStartPoint.X = Convert.ToInt32((float)actualImageRectStartPoint.X * (float)srcBitmap.Width / (float)inp.Width);
                    actualImageRectStartPoint.Y = Convert.ToInt32((float)actualImageRectStartPoint.Y * (float)srcBitmap.Height / (float)inp.Height);
                }
                TempPointArr.Add(actualImageRectStartPoint);

            }
            List<Point> TempPointArr1 = new List<Point>();
                //ReorderPointList(ref TempPointArr, ref TempPointArr1);// cartesian reordering of points commented by sriram
                PolarReorderPointList(ref TempPointArr, ref TempPointArr1);
                Graphics g = Graphics.FromImage(srcBitmap);
                //DrawAnnotationNumber(g, id);

            if(this.isCup)
                g.DrawPolygon(new Pen(Color.FromName(AnnotationVariables.cupColor), 3.0f), TempPointArr1.ToArray());
            else
                g.DrawPolygon(new Pen(Color.FromName(AnnotationVariables.discColor), 3.0f), TempPointArr1.ToArray());

            if (!AnnotationVariables.isGlaucomaTool)
            {
                g.DrawString(id.ToString(), new Font(FontFamily.GenericSansSerif, 43.2f, FontStyle.Bold, GraphicsUnit.Pixel), brush, new PointF(TempPointArr[1].X - 55, TempPointArr[1].Y - 45));
                g.DrawPolygon(new Pen(Color.FromName(AnnotationVariables.annotationMarkingColor), 9.0f), TempPointArr1.ToArray());

            }
           
            g.Dispose();
        }
        /// <summary>
        /// Method to translate the points drawn on the screen to the actual the points are obtained from the screen and zoom factor translation is done and a convex polygon for the same points are drawn on the image saving of the image is done for debugging which is commented
        /// </summary>
        public void GetContourValue()
        {

            Common.CommonMethods _common = Common.CommonMethods.CreateInstance();
            inp = new Image<Bgr, byte>(GlaucomaTool.tempBm.Width,GlaucomaTool.tempBm.Height);
          INTUSOFT.Custom.Controls.PictureBoxExtended temp = new INTUSOFT.Custom.Controls.PictureBoxExtended();
          temp.SizeMode = PictureBoxSizeMode.Zoom;
          temp.Image = inp.ToBitmap();
          List<Point> TempPointArr = new  List<Point>();
            _common.Image = temp.Image;
            _common.Height = GlaucomaTool.pbxHeight;
            _common.Width = GlaucomaTool.pbxWidth;
                for (int i = 0; i < this.pointArray.Count; i++)
                {
                    TempPointArr.Add(_common.TranslatePointToImageCoordinates((Point)this.pointArray[i], Common.PicBoxSizeMode.Zoom));
                }
                List<Point> TempPointArr1 = new List<Point>();
                //ReorderPointList(ref TempPointArr, ref TempPointArr1);// cartesian reordering of points commented by sriram
                PolarReorderPointList(ref TempPointArr, ref TempPointArr1);
                if (this.isCup)
                {

                    cupImage = inp.ToBitmap();
                    Graphics g = Graphics.FromImage(cupImage);
                    g.FillPolygon(Brushes.White, TempPointArr1.ToArray());
                    g.Dispose();
                }
                else
                {
                    discImage = inp.ToBitmap();
                    Graphics g = Graphics.FromImage(discImage);
                    g.FillPolygon(Brushes.White, TempPointArr1.ToArray());
                    g.Dispose();

                }
        }
        public void ReorderPointList(ref List<Point> inlist, ref List<Point> outlist)
        {
            outlist.Add(inlist[0]);
            inlist.RemoveAt(0);
            int list_count = inlist.Count;

            Point Last_point = outlist[0];
            int x2, y2;
            int dist, min_dist, min_dist_index;
            for (int i = 0; i < list_count; i++)
            {
                min_dist = 1000000000;
                min_dist_index = 0;
                for (int j = 0; j < inlist.Count; j++)
                {
                    x2 = (Last_point.X - inlist[j].X);
                    x2 *= x2; // squaring
                    y2 = (Last_point.Y - inlist[j].Y);
                    y2 *= y2; // squaring
                    dist = x2 + y2; // gets the distance squared.

                    if (dist < min_dist )
                    {
                        min_dist_index = j;
                        min_dist = dist;
                    }

                }
                Last_point = inlist[min_dist_index];
                outlist.Add(inlist[min_dist_index]);
                inlist.RemoveAt(min_dist_index);
            }
        }
        public void PolarReorderPointList(ref List<Point> inlist, ref List<Point> outlist)
        {
            //outlist.Add(inlist[0]);

            // find centroid of all the points
            Point center = new Point();
            foreach (Point item in inlist)
            {
                center.X += item.X;
                center.Y += item.Y;
            }
            center.X /= inlist.Count;
            center.Y /= inlist.Count;

            // convert to polar co-ordinates and sort by angle.
            float[] rad = new float[inlist.Count];
            float[] angle = new float[inlist.Count];
            List<PointF> points_order = new List<PointF>();
            float x2, y2, diffx, diffy;
            float dist, ratio, ang;
            int index = 0;
            foreach (Point item in inlist)
            {
                diffx = (item.X - center.X);
                x2 = diffx * diffx; // squaring
                diffy = (center.Y - item.Y) ; // changing sign of y
                y2 = diffy * diffy; // squaring
                dist = x2 + y2;
                dist = (float) Math.Sqrt(dist);

                ratio = diffy / diffx;
                ang = (float) Math.Atan(ratio) ; // return from - pi/2 to + pi/2
                rad[index] = dist;
                angle[index] = ang * (180.0f / (float) Math.PI);

                if (diffx < 0)
                    angle[index] += 180;
                angle[index] += 360;
                angle[index] %= 360;

                points_order.Add(new PointF((float)index, angle[index]));
                index++;
            }

            points_order = points_order.OrderBy(p => p.Y).ToList();

            foreach (PointF item in points_order)
            {
                outlist.Add(inlist[(int)item.X]);
            }
           

        }

        public static void MeasureCDR()
        {
           
            
                BitmapData cupData = cupImage.LockBits(new Rectangle(0, 0, cupImage.Width, cupImage.Height), ImageLockMode.ReadWrite, cupImage.PixelFormat);
                BitmapData DiscData = discImage.LockBits(new Rectangle(0, 0, discImage.Width, discImage.Height), ImageLockMode.ReadWrite, discImage.PixelFormat);
            
            CDRInit(cupImage.Width, cupImage.Height);
           unsafe
            {
                DrawArea.retCDRValues= (DrawArea.CDRStruct*)CalculateCDR(DiscData.Scan0, cupData.Scan0);

            }
            cupImage.UnlockBits(cupData);
            discImage.UnlockBits(DiscData);
            cupImage.Dispose();
            discImage.Dispose();
            CDRExit();
        }
        public DrawPolygon(int x1, int y1, int x2, int y2)
        {
             this.pointArray = new ArrayList();
             this.pointArray.Add(new Point(x1, y1));
             
             if (!AnnotationVariables.isGlaucomaTool)
             {
                 
                 pointArray.Add(new Point(x2, y2));
                 Shape.PointArray = this.pointArray.Cast<Point>().ToList();

             }
            
            LoadCursor();
            Initialize();
        }

        public override void Draw(Graphics g)
        {
            int x1 = 0, y1 = 0;     // previous point
            int x2, y2;             // current point

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen=null;
            if(AnnotationVariables.isGlaucomaTool)//This if statement has been added by darshan to resovle the issue of polygon annotation marking not getting visible.

                pen = new Pen(Color, PenWidth);
            else
                pen = new Pen(Color.FromName(AnnotationVariables.annotationMarkingColor), PenWidth);

            IEnumerator enumerator = this.pointArray.GetEnumerator();

            if ( enumerator.MoveNext() )
            {
                x1 = ((Point)enumerator.Current).X;
                y1 = ((Point)enumerator.Current).Y;
            }

            while ( enumerator.MoveNext() )
            {
                x2 = ((Point)enumerator.Current).X;
                y2 = ((Point)enumerator.Current).Y;

                Point[] pArr = new Point[2];
                pArr[0] = new Point(x1,y1);
                pArr[1] = new Point(x2, y2);
                //g.DrawCurve(pen, x1, y1, x2, y2);
                //Shape.PointArray.Add(new Point(x2, y2));
               

                g.DrawCurve(pen,pArr);

                x1 = x2;
                y1 = y2;
            }
            ConnectFirstAndLastPoint(g);
            ToolPolygon.islastPointConnect = false;
            //if (ToolPolygon.islastPointConnect)
            //{
               
            //}
            Shape._shapeType = ShapeType.Annotation_Polygon;

            pen.Dispose();
        }

        public void ConnectFirstAndLastPoint(Graphics e )
        {
            IEnumerator enumerator = this.pointArray.GetEnumerator();
            //List<Point> pointList = pointArray;
            Pen pen = new Pen(Color, PenWidth);
            e.DrawLine(pen, (Point)this.pointArray[0], (Point)this.pointArray[pointArray.Count - 1]);
                ToolPolygon.islastPointConnect = false;
        }

        public void AddPoint(Point point)
        {
            if(!pointArray.Contains(point))
            this.pointArray.Add(point);
            if(AnnotationVariables.isGlaucomaTool)
            {
                List<Point> tempArr = this.pointArray.Cast<Point>().ToList();
                List<Point> tempArr1 = new List<Point>(tempArr.Count);
                //ReorderPointList(ref tempArr, ref tempArr1);// old reorder of points for cartesian reordering commented by sriram
                 PolarReorderPointList(ref tempArr, ref tempArr1);
                  this.pointArray.Clear();
                this.pointArray = new ArrayList(tempArr1);
            }
            Shape.PointArray = this.pointArray.Cast<Point>().ToList();
        }

        public override int HandleCount
        {
            get
            {
                return this.pointArray.Count;
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            if ( handleNumber < 1 )
                handleNumber = 1;

            if (handleNumber > this.pointArray.Count)
                handleNumber = this.pointArray.Count;

            return ((Point)this.pointArray[handleNumber - 1]);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            return handleCursor;
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if ( handleNumber < 1 )
                handleNumber = 1;

            if (handleNumber > this.pointArray.Count)
                handleNumber = this.pointArray.Count;

            this.pointArray[handleNumber - 1] = point;
            
            {
                Shape.PointArray[handleNumber - 1] = point;
            }
            
            Invalidate();
        }

        public override void Move(int deltaX, int deltaY)
        {
            int n = this.pointArray.Count;
            Point point;

            for ( int i = 0; i < n; i++ )
            {
                point = new Point(((Point)this.pointArray[i]).X + deltaX, ((Point)this.pointArray[i]).Y + deltaY);

                this.pointArray[i] = point;
                Shape.PointArray[i] = point;
            }

            Invalidate();
        }

        public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, orderNumber),
                 this.pointArray.Count);

            int i = 0;
            foreach (Point p in this.pointArray)
            {
                info.AddValue(
                    String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}-{2}",
                    entryPoint, orderNumber, i++),
                    p);

            }

            base.SaveToStream (info, orderNumber);  // ??
        }

        public override void LoadFromStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            Point point;
            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, orderNumber));

            for ( int i = 0; i < n; i++ )
            {
                point = (Point)info.GetValue(
                    String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}-{2}",
                    entryPoint, orderNumber, i),
                    typeof(Point));

                this.pointArray.Add(point);
            }

            base.LoadFromStream (info, orderNumber);
        }


        /// <summary>
        /// Create graphic object used for hit test
        /// </summary>
        protected override void CreateObjects()
        {
            if ( AreaPath != null )
                return;

            // Create closed path which contains all polygon vertexes
            AreaPath = new GraphicsPath();

            int x1 = 0, y1 = 0;     // previous point
            int x2, y2;             // current point

            IEnumerator enumerator = this.pointArray.GetEnumerator();

            if ( enumerator.MoveNext() )
            {
                x1 = ((Point)enumerator.Current).X;
                y1 = ((Point)enumerator.Current).Y;
            }

            while ( enumerator.MoveNext() )
            {
                x2 = ((Point)enumerator.Current).X;
                y2 = ((Point)enumerator.Current).Y;

                AreaPath.AddLine(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            AreaPath.CloseFigure();
            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }

        private void LoadCursor()
        {
            handleCursor = new Cursor(GetType(), "PolyHandle.cur");
        }
	}
}
