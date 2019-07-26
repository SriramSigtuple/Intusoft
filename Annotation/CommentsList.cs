//using System;
//using System.Runtime.Serialization;
//using System.Windows.Forms;
//using System.Drawing;
//using System.Security.Permissions;
//using System.Globalization;
//using System.Collections;
//using System.Diagnostics;
//using System.Reflection;


//namespace Annotation
//{
//    /// <summary>
//    /// List of graphic objects
//    /// </summary>
//    [Serializable]
//    public class CommentsList : ISerializable
//    {
//        private ArrayList commentsList;

//        private const string entryCount = "Count";
//        private const string entryType = "Type";

//        public CommentsList()
//        {
//            commentsList = new ArrayList();
//        }

//        protected CommentsList(SerializationInfo info, StreamingContext context)
//        {
//            commentsList = new ArrayList();

//            int n = info.GetInt32(entryCount);
//            string typeName;
//            object drawObject;

//            for (int i = 0; i < n; i++)
//            {
//                typeName = info.GetString(
//                    String.Format(CultureInfo.InvariantCulture,
//                        "{0}{1}",
//                    entryType, i));

//                drawObject = Assembly.GetExecutingAssembly().CreateInstance(
//                    typeName);

//                ((AnnotationText)drawObject).LoadFromStream(info, i);

//                commentsList.Add(drawObject);
//            }

//        }

//        /// <summary>
//        /// Save object to serialization stream
//        /// </summary>
//        /// <param name="info"></param>
//        /// <param name="context"></param>
//        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
//        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
//        {
//            info.AddValue(entryCount, commentsList.Count);

//            int i = 0;

//            foreach (AnnotationText o in commentsList)
//            {
//                info.AddValue(
//                    String.Format(CultureInfo.InvariantCulture,
//                        "{0}{1}",
//                        entryType, i),
//                    o.GetType().FullName);

//                o.SaveToStream(info, i);

//                i++;
//            }
//        }

//        public void Draw(Graphics g)
//        {
//            int n = commentsList.Count;
//            AnnotationText o;

//            // Enumerate list in reverse order
//            // to get first object on the top
//            for (int i = n - 1; i >= 0; i--)
//            {
//                o = (AnnotationText)commentsList[i];

               
//            }
//        }

//        /// <summary>
//        /// Clear all objects in the list
//        /// </summary>
//        /// <returns>
//        /// true if at least one object is deleted
//        /// </returns>
//        public bool Clear()
//        {
//            bool result = (commentsList.Count > 0);
//            commentsList.Clear();
//            return result;
//        }

//        /// <summary>
//        /// Count and this [nIndex] allow to read all graphics objects
//        /// from GraphicsList in the loop.
//        /// </summary>
//        public int Count
//        {
//            get
//            {
//                return commentsList.Count;
//            }
//        }

//        public AnnotationText this[int index]
//        {
//            get
//            {
//                if (index < 0 || index >= commentsList.Count)
//                    return null;

//                return ((AnnotationText)commentsList[index]);
//            }
//        }

//        /// <summary>
//        /// SelectedCount and GetSelectedObject allow to read
//        /// selected objects in the loop
//        /// </summary>
//        public int SelectionCount
//        {
//            get
//            {
//                int n = 0;

//                foreach (AnnotationText o in commentsList)
//                {
//                    if (o.Selected)
//                        n++;
//                }

//                return n;
//            }
//        }

//        public AnnotationText GetSelectedObject(int index)
//        {
//            int n = -1;

//            foreach (AnnotationText o in commentsList)
//            {
//                if (o.Selected)
//                {
//                    n++;

//                    if (n == index)
//                        return o;
//                }
//            }

//            return null;
//        }

//        public void Add(AnnotationText obj)
//        {
//            // insert to the top of z-order
//            commentsList.Insert(0, obj);
//        }

//        public void SelectInRectangle(Rectangle rectangle)
//        {
//            UnselectAll();

//            foreach (AnnotationText o in commentsList)
//            {
//                if (o.IntersectsWith(rectangle))
//                    o.Selected = true;
//            }

//        }

//        public void UnselectAll()
//        {
//            foreach (AnnotationText o in commentsList)
//            {
//                o.Selected = false;
//            }
//        }

//        public void SelectAll()
//        {
//            foreach (AnnotationText o in commentsList)
//            {
//                o.Selected = true;
//            }
//        }

//        /// <summary>
//        /// Delete selected items
//        /// </summary>
//        /// <returns>
//        /// true if at least one object is deleted
//        /// </returns>
//        public bool DeleteSelection()
//        {
//            bool result = false;

//            int n = commentsList.Count;

//            for (int i = n - 1; i >= 0; i--)
//            {
//                if (((AnnotationText)commentsList[i]).Selected)
//                {
//                    commentsList.RemoveAt(i);
//                    result = true;
//                }
//            }

//            return result;
//        }


//        /// <summary>
//        /// Move selected items to front (beginning of the list)
//        /// </summary>
//        /// <returns>
//        /// true if at least one object is moved
//        /// </returns>
//        public bool MoveSelectionToFront()
//        {
//            int n;
//            int i;
//            ArrayList tempList;

//            tempList = new ArrayList();
//            n = commentsList.Count;

//            // Read source list in reverse order, add every selected item
//            // to temporary list and remove it from source list
//            for (i = n - 1; i >= 0; i--)
//            {
//                if (((AnnotationText)commentsList[i]).Selected)
//                {
//                    tempList.Add(commentsList[i]);
//                    commentsList.RemoveAt(i);
//                }
//            }

//            // Read temporary list in direct order and insert every item
//            // to the beginning of the source list
//            n = tempList.Count;

//            for (i = 0; i < n; i++)
//            {
//                commentsList.Insert(0, tempList[i]);
//            }

//            return (n > 0);
//        }

//        /// <summary>
//        /// Move selected items to back (end of the list)
//        /// </summary>
//        /// <returns>
//        /// true if at least one object is moved
//        /// </returns>
//        public bool MoveSelectionToBack()
//        {
//            int n;
//            int i;
//            ArrayList tempList;

//            tempList = new ArrayList();
//            n = commentsList.Count;

//            // Read source list in reverse order, add every selected item
//            // to temporary list and remove it from source list
//            for (i = n - 1; i >= 0; i--)
//            {
//                if (((DrawObject)commentsList[i]).Selected)
//                {
//                    tempList.Add(commentsList[i]);
//                    commentsList.RemoveAt(i);
//                }
//            }

//            // Read temporary list in reverse order and add every item
//            // to the end of the source list
//            n = tempList.Count;

//            for (i = n - 1; i >= 0; i--)
//            {
//                commentsList.Add(tempList[i]);
//            }

//            return (n > 0);
//        }

//        /// <summary>
//        /// Get properties from selected objects and fill GraphicsProperties instance
//        /// </summary>
//        /// <returns></returns>
//        private CommentsProperties GetProperties()
//        {
//            CommentsProperties properties = new CommentsProperties();

//            int n = SelectionCount;

//            if (n < 1)
//                return properties;

//            AnnotationText o = GetSelectedObject(0);

//            //int firstColor = o.Color.ToArgb();
//            //int firstPenWidth = o.PenWidth;

//            bool allColorsAreEqual = true;
//            bool allWidthAreEqual = true;

//            for (int i = 1; i < n; i++)
//            {
//                //if (GetSelectedObject(i). != firstColor)
//                //    allColorsAreEqual = false;

//                //if (GetSelectedObject(i).PenWidth != firstPenWidth)
//                //    allWidthAreEqual = false;
//            }

//            if (allColorsAreEqual)
//            {
//                //properties.ColorDefined = true;
//                //properties.Color = Color.FromArgb(firstColor);
//            }

//            if (allWidthAreEqual)
//            {
//                //properties. = true;
//                //properties.PenWidth = firstPenWidth;
//            }

//            return properties;
//        }

//        /// <summary>
//        /// Apply properties for all selected objects
//        /// </summary>
//        private void ApplyProperties(CommentsProperties properties)
//        {
//            foreach (AnnotationText o in commentsList)
//            {
//                //if (o.Selected)
//                //{
//                //    if (properties.ColorDefined)
//                //    {
//                //        o.Color = properties.Color;
//                //        DrawObject.LastUsedColor = properties.Color;
//                //    }

//                //    if (properties.PenWidthDefined)
//                //    {
//                //        o.PenWidth = properties.PenWidth;
//                //        DrawObject.LastUsedPenWidth = properties.PenWidth;
//                //    }
//                //}
//            }
//        }

//        /// <summary>
//        /// Show Properties dialog. Return true if list is changed
//        /// </summary>
//        /// <param name="parent"></param>
//        /// <returns></returns>
//        //public bool ShowPropertiesDialog(IWin32Window parent)
//        //{
//        //    if ( SelectionCount < 1 )
//        //        return false;

//        //    GraphicsProperties properties = GetProperties();
//        //    PropertiesDialog dlg = new PropertiesDialog();
//        //    dlg.Properties = properties;

//        //    if ( dlg.ShowDialog(parent) != DialogResult.OK )
//        //        return false;

//        //    ApplyProperties(properties);

//        //    return true;
//        //}
//    }
//}
