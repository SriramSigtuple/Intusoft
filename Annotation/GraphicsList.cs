using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Permissions;
using System.Globalization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using INTUSOFT.EventHandler;


namespace Annotation
{
    /// <summary>
    /// List of graphic objects
    /// </summary>
    [Serializable]
    public class GraphicsList : ISerializable
    {
        public ArrayList graphicsList;
        public ArrayList lineGraphicsList;
        public List<int> countList;
        public int id = 0;
        public int[] ids;
        public string[] graphicType;
        public string currentType = string.Empty;
        public bool isDelete = false;
        public int selection_id = 0;
        private const string entryCount = "Count";
        private const string entryType = "Type";
        static IVLEventHandler _eventHandler;

        public GraphicsList()
        {
            if (_eventHandler == null)
                _eventHandler = IVLEventHandler.getInstance();
            graphicsList = new ArrayList();
            lineGraphicsList = new ArrayList();
            countList = new List<int>();
        }

        protected GraphicsList(SerializationInfo info, StreamingContext context)
        {
            graphicsList = new ArrayList();

            int n = info.GetInt32(entryCount);
            string typeName;
            object drawObject;

            for (int i = 0; i < n; i++)
            {
                typeName = info.GetString(
                    String.Format(CultureInfo.InvariantCulture,
                        "{0}{1}",
                    entryType, i));

                drawObject = Assembly.GetExecutingAssembly().CreateInstance(
                    typeName);

                ((DrawObject)drawObject).LoadFromStream(info, i);

                graphicsList.Add(drawObject);
            }

        }

        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(entryCount, graphicsList.Count);

            int i = 0;

            foreach (DrawObject o in graphicsList)
            {
                info.AddValue(
                    String.Format(CultureInfo.InvariantCulture,
                        "{0}{1}",
                        entryType, i),
                    o.GetType().FullName);
                o.SaveToStream(info, i);
                i++;
            }
        }

        public void Draw(Graphics g)
        {
            int n = graphicsList.Count;
            ids = new int[n];
            graphicType = new string[n];
            deletedSelection = n;
            DrawObject o;
            int count = 1;
            // Enumerate list in reverse order
            // to get first object on the top
            for (int i = n - 1; i >= 0; i--)
            {
                graphicType[i] = graphicsList[i].GetType().ToString();

                o = (DrawObject)graphicsList[i];
                //if (ToolPolygon.islastPointConnect)
                //{
                //    if (o is DrawPolygon)
                //    {
                //        DrawPolygon d = o as DrawPolygon;
                //        d.ConnectFirstAndLastPoint(g);
                //    }
                //    }
                //else
                o.Draw(g);
                currentType = graphicsList[i].GetType().ToString();
                if (!graphicType[i].Equals("Annotation.DrawLine"))
                {
                    ids[i] = count;
                    countList.Add(count);
                    o.DrawAnnotationNumber(g, count);
                    count++;
                    o.ID = ids[i];

                    if (o.Selected == true)
                    {
                        selection_id = ids[i];

                        o.DrawTracker(g);
                    }
                }
                else
                {
                    //selection_id=-1;
                    if (o.Selected == true)
                    {
                        selection_id = ids[i];
                        o.DrawTracker(g);

                    }

                    //graphicsList.RemoveAt(i);
                }
            }

            if (ids.Contains(0))
            {

                ids = ids.Where(val => val != 0).ToArray();

            }
        }
        public void DrawLine(Graphics g)
        {
            int n = lineGraphicsList.Count;
            ids = new int[n];
            graphicType = new string[n];
            deletedSelection = n;
            DrawObject o;
            int count = 1;
            // Enumerate list in reverse order
            // to get first object on the top
            for (int i = n - 1; i >= 0; i--)
            {
                graphicType[i] = lineGraphicsList[i].GetType().ToString();

                o = (DrawObject)lineGraphicsList[i];
                //if (ToolPolygon.islastPointConnect)
                //{
                //    if (o is DrawPolygon)
                //    {
                //        DrawPolygon d = o as DrawPolygon;
                //        d.ConnectFirstAndLastPoint(g);
                //    }
                //    }
                //else
                o.Draw(g);
                currentType = lineGraphicsList[i].GetType().ToString();

                {
                    ids[i] = count;

                    count++;
                    o.ID = ids[i];


                }

            }
        }

        /// <summary>
        /// Clear all objects in the list
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool Clear()
        {
            bool result = (graphicsList.Count > 0);
            graphicsList.Clear();
            return result;
        }

        /// <summary>
        /// Count and this [nIndex] allow to read all graphics objects
        /// from GraphicsList in the loop.
        /// </summary>
        public int Count
        {
            get
            {
                return graphicsList.Count;
            }
        }


        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= graphicsList.Count)
                    return null;

                return ((DrawObject)graphicsList[index]);
            }
        }

        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;

                foreach (DrawObject o in graphicsList)
                {
                    if (o.Selected)

                        n++;


                }

                return n;
            }
        }

        public DrawObject GetSelectedObject(int index)
        {
            int n = -1;
            DrawObject d = null;
            //for (int i = 0; i < graphicsList.Count; i++)
            //{
            //     d = graphicsList[i] as DrawObject;
            //}
            for (int i = 0; i < graphicsList.Count; i++)
            //foreach (DrawObject o in graphicsList)
            {
                DrawObject o = graphicsList[i] as DrawObject;
                if (o.Selected)
                {
                    n++;

                    if (n == index)
                        return o;
                }

            }

            return null;
        }

        public void Add(DrawObject obj)
        {
            // insert to the top of z-order
            //if (!(obj is Annotation.DrawLine))
            graphicsList.Insert(0, obj);
            //else
            //    lineGraphicsList.Insert(0, obj);


        }

        public void SelectInRectangle(Rectangle rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in graphicsList)
            {
                //deletedSelection = o.ID;

                if (o.IntersectsWith(rectangle))
                {

                    o.Selected = true;
                }
            }

        }

        public void UnselectAll()
        {
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = false;
            }
           
        }

        public void SelectAll()
        {
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = true;
            }
        }
        public int deletedSelection = 0;
        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        /// 
        public void Deleteselection_id()
        {
            bool result = false;
            int n = graphicsList.Count;
            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)graphicsList[i]).Selected)
                {
                    result = true;
                }
            }
        }

        public void DeleteAll()
        {
            int n = graphicsList.Count;
            for (int i = n - 1; i >= 0; i--)
            {
                if (AnnotationVariables.isGlaucomaTool)
                {
                    if (graphicsList[i] is DrawPolygon)
                    {
                        DrawPolygon d = graphicsList[i] as DrawPolygon;
                        if (d.isCup)
                        {
                            ToolPolygon.DrawCupPolygon = null;
                            graphicsList.RemoveAt(i);
                        }
                        else
                        {
                            ToolPolygon.DrawDiscPolygon = null;
                            graphicsList.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    graphicsList.RemoveAt(i);
                }
            }
            if (AnnotationVariables.isGlaucomaTool)
            {
                Args arg = new Args();
                arg["ModifyDiscPoints"] = false;
                arg["DrawCup"] = false;
                arg["ModifyCupPoints"] = false;
                arg["MeasureCDR"] = false;
                arg["Print"] = false;
                arg["Save"] = false;
                arg["Export"] = false;//set to false when all points are deleted.By Ashutosh 21-7-2017

                _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
            }
        }

        public bool DeleteSelection()
        {
            bool result = false;

            int n = graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)graphicsList[i]).Selected)
                {
                    if (AnnotationVariables.isGlaucomaTool)
                    {
                        if (graphicsList[i] is DrawPolygon)
                        {

                            DrawPolygon d = graphicsList[i] as DrawPolygon;
                            Args arg = new Args();
                            arg["ModifyCupPoints"] = false;
                            arg["MeasureCDR"] = false;
                            arg["Print"] = false;
                            arg["Save"] = false;
                            arg["Export"] = false;//set to false. when points of polygon are deleted.By Ashutosh 21-7-2017
                            graphicsList.RemoveAt(i);
                            if (d.isCup)
                            {
                                ToolPolygon.DrawCupPolygon = null;
                            }
                            else
                            {
                                arg["ModifyDiscPoints"] = false;
                                arg["DrawCup"] = false;
                                ToolPolygon.DrawDiscPolygon = null;
                            }
                            _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                        }
                    }
                    else
                    {
                        currentType = graphicsList[i].GetType().ToString();
                        graphicsList.RemoveAt(i);
                        if (graphicsList.Count == 0)//Added by darshan on 25-07-2016 as per NR:0001211 Note no:(0002558)
                        {
                            Args arg = new Args();
                            arg["Print"] = false;
                            arg["Save"] = false;
                            arg["Export"] = false;//set to false. when points are deleted.By Ashutosh 21-7-2017

                            _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
                        }
                    }
                    result = true;
                }
            }
            return result;
        }



        /// <summary>
        /// Move selected items to front (beginning of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToFront()
        {
            int n;
            int i;
            ArrayList tempList;

            tempList = new ArrayList();
            n = graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)graphicsList[i]).Selected)
                {
                    tempList.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for (i = 0; i < n; i++)
            {
                graphicsList.Insert(0, tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Move selected items to back (end of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToBack()
        {
            int n;
            int i;
            ArrayList tempList;

            tempList = new ArrayList();
            n = graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)graphicsList[i]).Selected)
                {
                    tempList.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for (i = n - 1; i >= 0; i--)
            {
                graphicsList.Add(tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Get properties from selected objects and fill GraphicsProperties instance
        /// </summary>
        /// <returns></returns>
        private GraphicsProperties GetProperties()
        {
            GraphicsProperties properties = new GraphicsProperties();

            int n = SelectionCount;

            if (n < 1)
                return properties;

            DrawObject o = GetSelectedObject(0);

            int firstColor = o.Color.ToArgb();
            int firstPenWidth = o.PenWidth;

            bool allColorsAreEqual = true;
            bool allWidthAreEqual = true;

            for (int i = 1; i < n; i++)
            {
                if (GetSelectedObject(i).Color.ToArgb() != firstColor)
                    allColorsAreEqual = false;

                if (GetSelectedObject(i).PenWidth != firstPenWidth)
                    allWidthAreEqual = false;
            }

            if (allColorsAreEqual)
            {
                properties.ColorDefined = true;
                properties.Color = Color.FromArgb(firstColor);
            }

            if (allWidthAreEqual)
            {
                properties.PenWidthDefined = true;
                properties.PenWidth = firstPenWidth;
            }

            return properties;
        }

        /// <summary>
        /// Apply properties for all selected objects
        /// </summary>
        private void ApplyProperties(GraphicsProperties properties)
        {
            foreach (DrawObject o in graphicsList)
            {
                if (o.Selected)
                {
                    if (properties.ColorDefined)
                    {
                        o.Color = properties.Color;
                        DrawObject.LastUsedColor = properties.Color;
                    }

                    if (properties.PenWidthDefined)
                    {
                        o.PenWidth = properties.PenWidth;
                        DrawObject.LastUsedPenWidth = properties.PenWidth;
                    }
                }
            }
        }

        /// <summary>
        /// Show Properties dialog. Return true if list is changed
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        //public bool ShowPropertiesDialog(IWin32Window parent)
        //{
        //    if ( SelectionCount < 1 )
        //        return false;

        //    GraphicsProperties properties = GetProperties();
        //    PropertiesDialog dlg = new PropertiesDialog();
        //    dlg.Properties = properties;

        //    if ( dlg.ShowDialog(parent) != DialogResult.OK )
        //        return false;

        //    ApplyProperties(properties);

        //    return true;
        //}
    }
}
