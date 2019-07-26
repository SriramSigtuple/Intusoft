using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

using System.Linq;
using System.Collections;
using INTUSOFT.EventHandler;

namespace Annotation
{
	/// <summary>
	/// Pointer tool
	/// </summary>
	public class ToolPointer : Tool
	{
        private enum SelectionMode
        {
            None,
            NetSelection,   // group selection is active
            Move,           // object(s) are moves
            Size            // object is resized
        }

        private SelectionMode selectMode = SelectionMode.None;

        // Object which is currently resized:
        private DrawObject resizedObject;
        private int resizedObjectHandle;

        // Keep state about last and current point (used to move and resize objects)
        private Point lastPoint = new Point(0,0);
        private Point startPoint = new Point(0, 0);
        IVLEventHandler _eventHandler;
        public static bool isZoom = false;
		public ToolPointer()
		{
            if (_eventHandler == null)
                _eventHandler = IVLEventHandler.getInstance();
		}

        /// <summary>
        /// Left mouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            selectMode = SelectionMode.None;
            Point point = new Point(e.X, e.Y);

            // Test for resizing (only if control is selected, cursor is on the handle)
            int n = drawArea.ListGraphics.SelectionCount;

            for ( int i = 0; i < n; i++ )
            {
                
                DrawObject o = drawArea.ListGraphics.GetSelectedObject(i);
                int handleNumber = o.HitTest(point);

                if ( handleNumber > 0 )
                {
                    selectMode = SelectionMode.Size;

                    // keep resized object in class members
                    resizedObject = o;
                    resizedObjectHandle = handleNumber;

                    // Since we want to resize only one object, unselect all other objects
                    drawArea.ListGraphics.UnselectAll();
                    o.Selected = true;

                    break;
                }
            }

            // Test for move (cursor is on the object)
            if ( selectMode == SelectionMode.None )
            {
                List<AnnotationComments> annocmt=null;
                int n1 = drawArea.ListGraphics.Count;
                DrawObject o = null;
                
                if (drawArea.comments.Count != 0)
                {
                    annocmt = drawArea.comments;
                    annocmt.Reverse();
                }
                string[] str = drawArea.ListGraphics.graphicType;
                for ( int i = 0; i < n1; i++ )
                {
                   
                    int a = drawArea.ListGraphics[i].HitTest(point);
                    if (drawArea.ListGraphics[i].PointContainer(point) == 0)
                    {
                        o = drawArea.ListGraphics[i];
                        break;
                    }
                    //if (drawArea.ListGraphics[i].HitTest(point) == 0)
                    //{
                    //    o = drawArea.ListGraphics[i];
                    //    //if(annocmt!=null)
                    //    //    //o.ID = annocmt[i].ID;
                    //    break;
                    //}
                }

                if ( o != null )
                {
                    isZoom = false;
                    selectMode = SelectionMode.Move;

                    // Unselect all if Ctrl is not pressed and clicked object is not selected yet
                    if ((Control.ModifierKeys & Keys.Control) == 0 && !o.Selected)
                        drawArea.ListGraphics.UnselectAll();

                    // Select clicked object
                    
                    o.Selected = true;
                    if (o is DrawPolygon)
                    {
                        DrawPolygon polyGon = o as DrawPolygon;

                        if (AnnotationVariables.isGlaucomaTool)
                        {
                            _eventHandler.Notify(_eventHandler.EnableMovePointInCDRTool, new Args());//public void Notify(String n, Args args) defined in IVLEventHandler.By ashutosh 25-07-2017
                        }


                    }

                    drawArea.Cursor = Cursors.SizeAll;
                }
            }
            // Net selection
            if ( selectMode == SelectionMode.None )
            {
                // click on background
                if ((Control.ModifierKeys & Keys.Control) == 0)
                {
                    drawArea.ListGraphics.UnselectAll();
                    drawArea.unselectalltext();
                }
                selectMode = SelectionMode.NetSelection;
                isZoom = true;
                drawArea.DrawNetRectangle = true;
            }
            lastPoint.X = e.X;
            lastPoint.Y = e.Y;
            startPoint.X = e.X;
            startPoint.Y = e.Y;
            drawArea.Capture = true;
            drawArea.NetRectangle = DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint);
            drawArea.Refresh();
        }


        /// <summary>
        /// Mouse is moved.
        /// None button is pressed, ot left button is pressed.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            Point point1 = new Point();

            // set cursor when mouse button is not pressed
            if (e.Button == MouseButtons.None)
            {
                Cursor cursor = null;

                for (int i = 0; i < drawArea.ListGraphics.Count; i++)
                {
                    int n = drawArea.ListGraphics[i].HitTest(point);

                    if (n == 5)
                    {
                        point1 = drawArea.ListGraphics[i].GetHandleCursor(n).HotSpot;
                        //annotateText.Location = drawArea.active_point;
                    }
                    if (n > 0)
                    {
                        cursor = drawArea.ListGraphics[i].GetHandleCursor(n);
                        break;
                    }
                }



                if (cursor == null)
                    cursor = Cursors.Default;

                drawArea.Cursor = cursor;

                return;
            }


            if (e.Button != MouseButtons.Left)
                return;

            /// Left button is pressed

            // Find difference between previous and current position
            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;


            lastPoint.X = e.X;
            lastPoint.Y = e.Y;
            int ControlIndx = 0;
            int shiftVal = 0;
            // resize
            if (selectMode == SelectionMode.Size)
            {
                if (resizedObject != null)
                {
                    if (AnnotationVariables.isGlaucomaTool)
                    {
                        if (resizedObject is DrawPolygon)
                        {
                            if (ToolPolygon.modifyPolygon)
                            {
                                if (e.Button != MouseButtons.Left)
                                    return;
                                DrawPolygon poly = resizedObject as DrawPolygon;// this line has been added in order to get the polygon details instead of directly getting from the generic object which solves the problem cup going outside disc and disc coming inside cup by sriram
                                if (!poly.isCup)
                                {
                                    if (ToolPolygon.DrawDiscPolygon == null)
                                        return;                 // precaution
                                    // if (distance < minDistance)
                                    {
                                        // Distance between last two points is less than minimum -
                                        // move last point
                                        if (ToolPolygon.DrawCupPolygon != null)
                                        {
                                            if (!NearestNeighbours((Point[])ToolPolygon.DrawCupPolygon.pointArray.ToArray(typeof(Point)), point))
                                                resizedObject.MoveHandleTo(point, resizedObjectHandle);
                                            if (AnnotationVariables.isGlaucomaToolViewing)
                                            {
                                              
                                                    Args arg = new Args();
                                                    arg["Save"] = true;
                                                    arg["IsViewed"] = false;
                                                    _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                                            }
                                        }
                                        else
                                            resizedObject.MoveHandleTo(point, resizedObjectHandle);
                                    }
                                }
                                else
                                {
                                    if (ToolPolygon.DrawCupPolygon == null)
                                        return;                 // precaution
                                    // if (distance < minDistance)
                                    {
                                        // Distance between last two points is less than minimum -
                                        // move last point
                                        if (ToolPolygon.DrawDiscPolygon != null)
                                        {
                                            if (NearestNeighbours((Point[])ToolPolygon.DrawDiscPolygon.pointArray.ToArray(typeof(Point)), point))
                                                //if (!InsidePolygon((Point[])ToolPolygon.DrawDiscPolygon.pointArray.ToArray(typeof(Point)), point))
                                                resizedObject.MoveHandleTo(point, resizedObjectHandle);
                                            if (AnnotationVariables.isGlaucomaToolViewing)
                                            {
                                                Args arg = new Args();
                                                if (drawArea.ListGraphics[0].HandleCount >= 3 && drawArea.ListGraphics[1].HandleCount >= 3)//this has been added to solve the defect no 0001515 
                                                {
                                                    arg["Save"] = true;
                                                    arg["IsViewed"] = false;
                                                }
                                                else
                                                {
                                                    arg["Save"] = false;
                                                    arg["IsViewed"] = true;
                                                }
                                                _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                                            }
                                        }
                                        else
                                            resizedObject.MoveHandleTo(point, resizedObjectHandle);
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        resizedObject.MoveHandleTo(point, resizedObjectHandle);
                        Args arg = new Args();
                        arg["Save"] = true;
                        arg["IsViewed"] = false;
                        _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
                    }
                    drawArea.Refresh();
                    Rectangle handleRect = drawArea.ListGraphics[ControlIndx].GetHandleRectangle(5);

                    point.X = handleRect.X;
                    point.Y = handleRect.Y;
                    //drawArea.active_point = point;
                    ////ToolObject.text_move(drawArea, e);
                    //(drawArea.Controls[ControlIndx] as Panel).Location = drawArea.active_point;

                    //drawArea.Controls[ControlIndx].SendToBack();
                    //drawArea.Refresh();



                }
            }

            // move
            if (!AnnotationVariables.isGlaucomaTool)
            {
                if (selectMode == SelectionMode.Move)
                {
                    int n = drawArea.ListGraphics.SelectionCount;

                    for (int i = 0; i < n; i++)
                    {
                        drawArea.ListGraphics.GetSelectedObject(i).Move(dx, dy);
                        //if (i == 5)
                        //{
                        //    drawArea.active_point = drawArea.ListGraphics[i].GetHandleCursor(n).HotSpot;
                        //}


                    }
                    //This below if statement was added by Darshan on 25-08-2015 to solve Defect no 0000596: system is getting crashed.
                    if (n > 0)
                    {
                        Rectangle handleRect = drawArea.ListGraphics[ControlIndx].GetHandleRectangle(5);

                        point.X = handleRect.X;
                        point.Y = handleRect.Y;
                        drawArea.Refresh();

                        drawArea.active_point = point;
                    }
                    Args arg = new Args();
                    arg["Save"] = true;
                    arg["IsViewed"] = false;
                    _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
                    //drawArea.Location = new Point(point.X, point.Y);

                    //foreach (AnnotationText item in drawArea.Controls)
                    //{
                    //    item.Location = drawArea.active_point;
                    //}

                    // (drawArea.Controls[ControlIndx] as Panel).Location = drawArea.active_point;
                    // //ToolObject.text_move(drawArea, e);

                    // drawArea.Controls[ControlIndx].SendToBack();
                    // drawArea.Cursor = Cursors.SizeAll;
                    //drawArea.Refresh();
                }

                if (selectMode == SelectionMode.NetSelection)
                {
                    drawArea.NetRectangle = DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint);
                    drawArea.Refresh();
                    return;
                }

            }
    }
        

        /// <summary>
        /// Right mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            if ( selectMode == SelectionMode.NetSelection )
       
            {
                // Group selection
                drawArea.ListGraphics.SelectInRectangle(drawArea.NetRectangle);

                selectMode = SelectionMode.None;
                drawArea.DrawNetRectangle = false;
            }

            if ( resizedObject != null )
            {
                // after resizing
                resizedObject.Normalize();
                resizedObject = null;
            }

            drawArea.Capture = false;
            drawArea.Refresh();
        }

        public static bool NearestNeighbours(Point[] poly, Point pnt)
        {
            int i, j;
            int nvert = poly.Length;
            bool c = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((poly[i].Y > pnt.Y) != (poly[j].Y > pnt.Y)) &&
                 (pnt.X < (poly[j].X - poly[i].X) * (pnt.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) + poly[i].X))
                    c = !c;
            }
            return c;
            //List<double> pointsDistance = new List<double>();
            //List<double> pointsDistance1 = new List<double>();
            //Point nearestPoint1 = new Point();
            //Point nearestPoint2 = new Point();
            //foreach (Point item in pointsArr)
            //{
            //    double dist = (item.X - p.X) * (item.X - p.X) + (item.Y - p.X) * (item.Y - p.X);
            //    dist = Math.Sqrt(dist);
            //    pointsDistance.Add(dist);

            //}
            //pointsDistance1 = pointsDistance.OrderByDescending(x => x).Reverse().ToList();
            //nearestPoint1 = (Point)pointsArr[pointsDistance.IndexOf(pointsDistance1[0])];
            //nearestPoint2 = (Point)pointsArr[pointsDistance.IndexOf(pointsDistance1[1])];

            //var val = (nearestPoint2.X - nearestPoint1.X) * (p.Y - nearestPoint1.Y) - (nearestPoint2.Y - nearestPoint1.Y) * (p.X - nearestPoint1.X);
            //if (val <= 0)
            //    return false;
            //else
            //    return true;
        }

        bool InsidePolygon(Point[] poly , Point p)
        {
            int i;
            double angle = 0;
            Point p1 = new Point(), p2 = new Point();

            for (i = 0; i < poly.Length; i++)
            {
                p1.X = poly[i].X - p.Y;
                p1.Y = poly[i].Y - p.Y;
                p2.X = poly[(i + 1) % poly.Length].X - p.X;
                p2.Y = poly[(i + 1) % poly.Length].Y - p.Y;
                angle += Angle2D(p1.X, p1.Y, p2.X, p2.Y);
            }

            if (Math.Abs (angle) < Math.PI)
                return (false);
            else
                return (true);
        }

        double Angle2D(double x1, double y1, double x2, double y2)
        {
            double dtheta, theta1, theta2;
            theta1 = Math.Atan2(y1, x1);
            theta2 = Math.Atan2(y2, x2);
            dtheta = theta2 - theta1;
            while (dtheta > Math.PI)
                dtheta -= (2 * Math.PI);
            while (dtheta < -Math.PI)
                dtheta += (2 * Math.PI);
            return (dtheta);
        }
	}
}
