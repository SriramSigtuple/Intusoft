using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
using System.Windows.Forms;
using System.Drawing;

namespace Annotation
{
	/// <summary>
	/// Ellipse graphic object
	/// </summary>
	public class DrawEllipse : DrawRectangle
	{
        Image<Bgr, byte> inp;
		public DrawEllipse()
		{
            SetRectangle(0, 0, 1, 1);
            Initialize();
		}

        public DrawEllipse(int x, int y, int width, int height)
        {
            Rectangle = new Rectangle(x, y, width, height);
            
            Initialize();
        }

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);

            g.DrawEllipse(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));
            
            Shape._shapeType = ShapeType.Annotation_Ellipse;
            Shape.StartPoint = new Point(Rectangle.X,Rectangle.Y);
            Shape.Width = Rectangle.Width;
            Shape.Height = Rectangle.Height;
            pen.Dispose();
        }

        //This below method was added by Darshan on 19-10-2015(Description given below).
        /// <summary>
        /// This method will draw the graph on the panel to the image.
        /// </summary>
        /// <param name="srcBitmap">Image name</param>
        public void DrawEllipseToBitmap(ref Bitmap srcBitmap,int id,Common.PicBoxSizeMode sizeMode = Common.PicBoxSizeMode.Zoom)
        {
            SolidBrush brush = new SolidBrush(Color.FromName(AnnotationVariables.annotationMarkingColor));
            
            Common.CommonMethods _common = Common.CommonMethods.CreateInstance();
            if (sizeMode == Common.PicBoxSizeMode.Zoom)
                inp = new Image<Bgr, byte>(Form1.tempBm.Width, Form1.tempBm.Height);
            else
                inp = new Image<Bgr, byte>(Form1.pbxWidth, Form1.pbxHeight);

            List<Point> TempPointArr = new List<Point>();
            _common.Image = inp.ToBitmap();
            _common.Height = Form1.pbxHeight;
            _common.Width = Form1.pbxWidth;

            Point actualImageRectStartPoint = _common.TranslatePointToImageCoordinates(new Point(rectangle.X, rectangle.Y),sizeMode);
            Point actualImageRectWidthPoint = _common.TranslatePointToImageCoordinates(new Point(rectangle.X + rectangle.Width, rectangle.Y), sizeMode);
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
            Rectangle ellipseRectangle = new Rectangle(actualImageRectStartPoint, new Size(width, height));
            Graphics g = Graphics.FromImage(srcBitmap);
            g.DrawEllipse(new Pen(Color.FromName(AnnotationVariables.annotationMarkingColor), 9.0f), DrawRectangle.GetNormalizedRectangle(ellipseRectangle));
            //DrawAnnotationNumber(g, id);
            g.DrawString(id.ToString(), new Font(FontFamily.GenericSansSerif, 43.2f, FontStyle.Bold, GraphicsUnit.Pixel), brush, new PointF(xCenter-10, y-60));


            g.Dispose();
            

        }


	}
}
