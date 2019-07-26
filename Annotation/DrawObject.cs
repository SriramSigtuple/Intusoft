using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
namespace Annotation
{
	/// <summary>
	/// Base class for all draw objects
	/// </summary>
	public abstract class DrawObject
	{
        #region Members

        // Object properties
        private bool selected;
        private Color color;
        private int penWidth;
        private bool _isCup;
        // Last used property values (may be kept in the Registry)
        private static Color lastUsedColor = Color.FromName(AnnotationVariables.annotationMarkingColor);

    
        private static int lastUsedPenWidth = 3;
        public int id = 0;
        // Entry names for serialization
        private const string entryColor = "Color";
        private const string entryPenWidth = "PenWidth";
        private Shape _shape=new Shape();

        #endregion

        #region Properties
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        /// <summary>
        /// Selection flag
        /// </summary>
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Pen width
        /// </summary>
        /// 
        public int PenWidth
        {
            get
            {
                return penWidth;
            }
            set
            {
                penWidth = value;
            }
        }

        /// <summary>
        /// Number of handles
        /// </summary>
        public virtual int HandleCount
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Last used color
        /// </summary>
        public static Color LastUsedColor
        {
            get
            {
                return lastUsedColor;
            }
            set
            {
                lastUsedColor = value;
            }
        }

        /// <summary>
        /// Last used pen width
        /// </summary>
        public static int LastUsedPenWidth
        {
            get
            {
                return lastUsedPenWidth;
            }
            set
            {
                lastUsedPenWidth = value;
            }
        }

        public  bool isCup
        {
            get
            {
                return _isCup;
            }
            set
            {
                _isCup = value;
            }

        }
        public Shape Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
            }
        }
        #endregion

        #region Virtual Functions

        /// <summary>
        /// Draw object
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {

        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Point GetHandle(int handleNumber)
        {
            return new Point(0, 0);
        }

        /// <summary>
        /// Get handle rectangle by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);

           return new Rectangle(point.X - 3, point.Y - 3, 7, 7); //old handle rectangle commented by sriram with width and height 7.
            //return new Rectangle(point.X - 1, point.Y - 1, 3, 3);
        }
        public virtual void DrawAnnotationNumber(Graphics g,int id)
        {
          SolidBrush  brush = new SolidBrush(Color.Aqua);
          if (!AnnotationVariables.isGlaucomaTool)// to check whether it is glaucoma tool and  to draw numbers for the objects for the same if it is not glaucoma tool by sriram 
          {
              //if(DrawArea.isDrawCup)
              //g.DrawString("C", new Font(FontFamily.GenericSansSerif, 13.2f, FontStyle.Bold, GraphicsUnit.Pixel), brush, new PointF(GetHandleRectangle(2).X - 10, GetHandleRectangle(2).Y - 15));
              //else
              //  g.DrawString("D", new Font(FontFamily.GenericSansSerif, 13.2f, FontStyle.Bold, GraphicsUnit.Pixel), brush, new PointF(GetHandleRectangle(2).X - 10, GetHandleRectangle(2).Y - 15));
          g.DrawString(id.ToString(), new Font(FontFamily.GenericSansSerif, 13.2f, FontStyle.Bold, GraphicsUnit.Pixel), brush, new PointF(GetHandleRectangle(2).X - 10, GetHandleRectangle(2).Y - 15));

          }

        }
        /// <summary>
        /// Draw tracker for selected object
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawTracker(Graphics g)
        {
            if ( ! Selected )
                return;

            SolidBrush brush = new SolidBrush(Color.Aqua);

            for ( int i = 1; i <= HandleCount; i++ )
            {
                g.FillRectangle(brush, GetHandleRectangle(i));
            }
          

            brush.Dispose();
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual int HitTest(Point point)
        {
            return -1;
        }


        /// <summary>
        /// Test whether point is inside of the object
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected virtual bool PointInObject(Point point)
        {
            return false;
        }
        //This below method has been added by darshan to solve defect no:0000514
        public virtual int PointContainer(Point point)
        {
            return -1;
        }

        /// <summary>
        /// Get curesor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return Cursors.Default;
        }

        /// <summary>
        /// Test whether object intersects with rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public virtual bool IntersectsWith(Rectangle rectangle)
        {
            return false;
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public virtual void Move(int deltaX, int deltaY)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveHandleTo(Point point, int handleNumber)
        {
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public virtual void Dump()
        {
            Trace.WriteLine("");
            Trace.WriteLine(this.GetType().Name);
            Trace.WriteLine("Selected = " + selected.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize object.
        /// Call this function in the end of object resizing.
        /// </summary>
        public virtual void Normalize()
        {
        }


        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public virtual void SaveToStream(SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryColor, orderNumber),
                Color.ToArgb());

            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryPenWidth, orderNumber),
                PenWidth);
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public virtual void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryColor, orderNumber));

            Color = Color.FromArgb(n);

            PenWidth = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryPenWidth, orderNumber));
        
        }

        #endregion

        #region Other functions

        /// <summary>
        /// Initialization
        /// </summary>
        protected void Initialize()
        {
            color = lastUsedColor;
            penWidth = LastUsedPenWidth;
        }

        #endregion
    }
}
