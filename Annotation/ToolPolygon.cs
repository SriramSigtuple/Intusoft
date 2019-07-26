using System;
using System.Windows.Forms;
using System.Drawing;
using INTUSOFT.EventHandler;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Common;
namespace Annotation
{
    /// <summary>
    /// Polygon tool
    /// </summary>
    public class ToolPolygon : ToolObject
    {
        public ToolPolygon()
        {
            Cursor = new Cursor(GetType(), "Pencil.cur");
            if (_eventHandler == null)
                _eventHandler = IVLEventHandler.getInstance();
        }
        public static bool islastPointConnect = false;
        public static bool modifyPolygon = false;
        private int lastX;
        private int lastY;
        public static DrawPolygon DrawCupPolygon;
        public static DrawPolygon DrawDiscPolygon;
        public DrawPolygon newPolygon;
        private const int minDistance = 25 * 25;// Changed the distance from 15* 15 to 20* 20 so as to show less number of hit points by sriram on september 1st 2015
        IVLEventHandler _eventHandler;

        /// <summary>
        /// Left nouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            Point point1 = new Point(e.X, e.Y);
            if (AnnotationVariables.isGlaucomaTool)
                ValidatePointsForGlaucomaTool(point1, drawArea);
            else
            {
                newPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
                AddNewObject(drawArea, newPolygon);
                lastX = e.X;
                lastY = e.Y;
                Args arg = new Args();//Added by darshan on 25-07-2016 as per NR:0001211 Note no:(0002558)
                arg["Print"] = true;
                arg["Save"] = true;
                arg["Export"] = true;//set to true when points drawn . By Ashutosh 21-7-2017

                _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
            }
            drawArea.Refresh();
        }

        private void ValidatePointsForGlaucomaTool(Point e, DrawArea drawArea)
        {
            #region conditions for  GlaucomaTool
            // Create new polygon, add it to the list
            // and keep reference to it
            Point point1 = new Point(e.X, e.Y);
            if (DrawArea.isDrawCup)
            {
                if (DrawCupPolygon == null)
                {
                    if (DrawDiscPolygon != null && DrawDiscPolygon.pointArray.Count > 2)
                    {
                        if (!ToolPointer.NearestNeighbours((Point[])DrawDiscPolygon.pointArray.ToArray(typeof(Point)), point1))
                        {
                            Common.CustomMessageBox.Show(AnnotationVariables.addCupPointsWarning, AnnotationVariables.warningHeader, CustomMessageBoxIcon.Warning);
                        }
                        else
                        {
                            DrawCupPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
                            DrawCupPolygon.isCup = DrawArea.isDrawCup;
                            AddNewObject(drawArea, DrawCupPolygon);
                            DrawCupPolygon.AddPoint(point1);
                        }
                    }
                    lastX = e.X;
                    lastY = e.Y;
                    Args arg = new Args();
                    arg["ModifyDiscPoints"] = true;
                    arg["DrawCup"] = true;
                    arg["ModifyCupPoints"] = true;
                    arg["MeasureCDR"] = false;
                    arg["Print"] = false;
                    arg["Save"] = false;
                    arg["Export"] = false;//set to flase when points not present/deleted . By Ashutosh 21-7-2017
                    _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                }
                else
                {
                    Point point = new Point(e.X, e.Y);
                    if (DrawDiscPolygon != null && DrawDiscPolygon.pointArray.Count > 2)
                    {
                        if (!ToolPointer.NearestNeighbours((Point[])DrawDiscPolygon.pointArray.ToArray(typeof(Point)), point))
                        {
                            Common.CustomMessageBox.Show(AnnotationVariables.addCupPointsWarning, AnnotationVariables.warningHeader, CustomMessageBoxIcon.Warning);
                        }
                        else
                        {
                            bool isSamePosition = DrawCupPolygon.pointArray.Contains(point);
                            if (!isSamePosition)
                            {
                                DrawCupPolygon.AddPoint(point);
                                if (DrawCupPolygon.pointArray.Count > 2)
                                {
                                    Args arg = new Args();
                                    arg["ModifyDiscPoints"] = true;
                                    arg["DrawCup"] = true;
                                    arg["ModifyCupPoints"] = true;
                                    arg["MeasureCDR"] = true;
                                    arg["Print"] = true;
                                    arg["Save"] = true;
                                    arg["Export"] = true;//set to true when count greater than 2. By Ashutosh 21-7-2017
                                    _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                {
                    if (DrawDiscPolygon == null)
                    {
                        if (DrawCupPolygon == null || DrawDiscPolygon == null)
                        {
                            Point point = new Point(e.X, e.Y);
                            DrawDiscPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
                            DrawDiscPolygon.isCup = DrawArea.isDrawCup;
                            AddNewObject(drawArea, DrawDiscPolygon);
                            DrawDiscPolygon.AddPoint(point);
                            Args arg = new Args();
                            arg["ModifyDiscPoints"] = true;
                            arg["DrawCup"] = false;
                            arg["ModifyCupPoints"] = false;
                            arg["MeasureCDR"] = false;
                            arg["Print"] = false;
                            arg["Save"] = false;
                            arg["Export"] = false;//set to false when points is null . By Ashutosh 21-7-2017

                            _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                        }

                        else if (DrawCupPolygon.pointArray.Count > 2)
                        {
                            if (ToolPointer.NearestNeighbours((Point[])DrawCupPolygon.pointArray.ToArray(typeof(Point)), point1))
                            {
                                Common.CustomMessageBox.Show(AnnotationVariables.addDiscPointsWarning, AnnotationVariables.warningHeader, CustomMessageBoxIcon.Warning);
                            }
                            else
                            {
                                DrawDiscPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
                                DrawDiscPolygon.isCup = DrawArea.isDrawCup;
                                AddNewObject(drawArea, DrawDiscPolygon);
                                Args arg = new Args();
                                arg["ModifyDiscPoints"] = true;
                                arg["DrawCup"] = false;
                                arg["ModifyCupPoints"] = false;
                                arg["MeasureCDR"] = false;
                                arg["Print"] = false;
                                arg["Save"] = false;
                                arg["Export"] = false;//set to false when count lesser than 2 . By Ashutosh 21-7-2017

                                _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                            }
                        }
                    }
                    else
                    {
                        Point point = new Point(e.X, e.Y);
                        if (DrawCupPolygon != null && DrawCupPolygon.pointArray.Count > 2)
                        {
                            if (ToolPointer.NearestNeighbours((Point[])DrawCupPolygon.pointArray.ToArray(typeof(Point)), point))
                            {
                                Common.CustomMessageBox.Show(AnnotationVariables.addDiscPointsWarning, AnnotationVariables.warningHeader, CustomMessageBoxIcon.Warning);
                            }
                            else
                            {
                                DrawDiscPolygon.AddPoint(point);
                                if (DrawDiscPolygon.pointArray.Count > 2)
                                {
                                    Args arg = new Args();
                                    arg["ModifyDiscPoints"] = true;
                                    arg["DrawCup"] = true;
                                    arg["MeasureCDR"] = true;
                                    if (DrawCupPolygon.pointArray.Count <= 2)
                                    {
                                        arg["Save"] = false;
                                        arg["Print"] = false;
                                        arg["Export"] = false;//set to false when count lesser than 2 . By Ashutosh 21-7-2017
                                    }
                                    else
                                    {
                                        arg["Save"] = true;
                                        arg["Print"] = true;
                                        arg["Export"] = true;//set to true when count greater than 2 . By Ashutosh 21-7-2017

                                    }
                                    _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                                }
                            }
                        }
                        else
                        {
                            DrawDiscPolygon.AddPoint(point);
                            if (DrawDiscPolygon.pointArray.Count > 2)// This condition is added in order to enable cup marking only if more than two points added to disc by sriram
                            {
                                Args arg = new Args();
                                arg["ModifyDiscPoints"] = true;
                                arg["DrawCup"] = true;
                                arg["ModifyCupPoints"] = false;
                                arg["MeasureCDR"] = false;
                                arg["Print"] = false;
                                arg["Save"] = false;
                                arg["Export"] = false;//set to true when count greater than 2 .By Ashutosh 21-7-2017

                                _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                            }
                        }
                    }
                }
            }
            #endregion
        }


        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            if (!AnnotationVariables.isGlaucomaTool)
            {
                drawArea.Cursor = Cursor;
                if (e.Button != MouseButtons.Left)
                    return;
                if (newPolygon == null)
                    return;                 // precaution
                Point point = new Point(e.X, e.Y);
                int distance = (e.X - lastX) * (e.X - lastX) + (e.Y - lastY) * (e.Y - lastY);
                if (distance < minDistance)
                {
                    // Distance between last two points is less than minimum -
                    // move last point
                    newPolygon.MoveHandleTo(point, newPolygon.HandleCount);
                }
                else
                {
                    // Add new point
                    newPolygon.AddPoint(point);
                    lastX = e.X;
                    lastY = e.Y;
                }
                drawArea.Refresh();
            }
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            if (!AnnotationVariables.isGlaucomaTool)
            {
                islastPointConnect = true;
                //drawArea.Refresh();
                //newPolygon.ConnectFirstAndLastPoint(
                newPolygon = null;
                base.OnMouseUp(drawArea, e);
            }
        }

        /// <summary>
        /// AddExistingObject will add existing polygon to drawarea.
        /// </summary>
        /// <param name="drawArea">drawarea is where the polygon is drawn</param>
        /// <param name="s">shape class details will be in s</param>
        public void AddExistingObject(DrawArea drawArea, Shape s)
        {
            if (AnnotationVariables.isGlaucomaTool)
            {
                if (DrawArea.isDrawCup)
                {
                    if (DrawCupPolygon == null)
                    {
                        DrawCupPolygon = new DrawPolygon(s.PointArray[0].X, s.PointArray[0].Y, s.PointArray[1].X, s.PointArray[1].Y);
                        DrawCupPolygon.isCup = DrawArea.isDrawCup;
                        DrawObject.LastUsedColor = Color.FromName(AnnotationVariables.cupColor);
                        DrawCupPolygon.Color = DrawObject.LastUsedColor;
                        AddNewObject(drawArea, DrawCupPolygon);
                        ArrayList points = new ArrayList(s.PointArray);
                        DrawCupPolygon.pointArray = points;
                        DrawCupPolygon.Shape.PointArray = points.Cast<Point>().ToList<Point>();
                    }
                }
                else
                {
                    if (DrawDiscPolygon == null)
                    {
                        DrawDiscPolygon = new DrawPolygon(s.PointArray[0].X, s.PointArray[0].Y, s.PointArray[1].X, s.PointArray[1].Y);
                        DrawDiscPolygon.isCup = DrawArea.isDrawCup;
                        DrawObject.LastUsedColor = Color.Black;
                        DrawDiscPolygon.Color = Color.FromName(AnnotationVariables.discColor);
                        AddNewObject(drawArea, DrawDiscPolygon);
                        ArrayList points = new ArrayList(s.PointArray);
                        DrawDiscPolygon.pointArray = points;
                        DrawDiscPolygon.Shape.PointArray = points.Cast<Point>().ToList<Point>();
                    }
                }
            }
            else
            {
                newPolygon = new DrawPolygon(s.PointArray[0].X, s.PointArray[0].Y, s.PointArray[1].X, s.PointArray[1].Y);
                DrawObject.LastUsedColor = Color.FromName(AnnotationVariables.annotationMarkingColor);
                AddNewObject(drawArea, newPolygon);
                ArrayList points = new ArrayList(s.PointArray);
                newPolygon.pointArray = points;
                newPolygon.Shape.PointArray = points.Cast<Point>().ToList<Point>();
            }
            drawArea.ListGraphics.UnselectAll();
            drawArea.Refresh();
        }
    }
}
