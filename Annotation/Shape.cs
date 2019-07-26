using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Annotation
{
    public enum ShapeType
    {
        Annotation_Rectangle, Annotation_Ellipse, Annotation_Polygon, Annotation_Line
    }
    [Serializable]
  public  class Shape
    {
        public Point StartPoint;
        public Point EndPoint;
        public int Width;
        public int Height;
        public List<Point> PointArray;
        public ShapeType _shapeType;
        //public enum Rectangle
        //{

        //};
        //public enum Line
        //{

        //};
        //public enum Polygon
        //{

        //};
     
     
    public Shape()
      {
          StartPoint = new Point();
          EndPoint = new Point();
          Width = 0;
          Height = 0;
          PointArray = new List<Point>();
          
      }

    }

   //public class RectangleShape:Shape
   // {
   //     public RectangleShape()
   //     {
   //         StartPoint = new Point();
   //         EndPoint = new Point();
   //         Width = 0;
   //         Height = 0;
   //         PointArray = new List<Point>();
   //         _shapeType = ShapeType.Annotation_Rectangle;
   //     }
   // }
   //public class EllipseShape : Shape
   //{
   //    public EllipseShape()
   //    {
   //        StartPoint = new Point();
   //        EndPoint = new Point();
   //        Width = 0;
   //        Height = 0;
   //        PointArray = new List<Point>();
   //        _shapeType = ShapeType.Annotation_Ellipse;
   //    }
   //}
   //public class LineShape : Shape
   //{
   //    public LineShape()
   //    {
   //        StartPoint = new Point();
   //        EndPoint = new Point();
   //        Width = 0;
   //        Height = 0;
   //        PointArray = new List<Point>();
   //        _shapeType = ShapeType.Annotation_Line;
   //    }
   //}
   //public class PolygonShape : Shape
   //{
   //    public PolygonShape()
   //    {
   //        StartPoint = new Point();
   //        EndPoint = new Point();
   //        Width = 0;
   //        Height = 0;
   //        PointArray = new List<Point>();
   //        _shapeType = ShapeType.Annotation_Polygon;
   //    }
   //}

}
