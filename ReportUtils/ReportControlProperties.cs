using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ReportUtils.ReportEnums;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtils
{
    public enum TextAlign { TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight }
   [Serializable]
   public class ReportControlProperties
   {

       #region Properties
        private IVLFont font;

        /// <summary>
        /// Sets and gets the value of variable Font.
        /// </summary>
        public IVLFont Font
        {
            get { return font; }
            set { font = value; }
        }
        private System.Drawing.Size size;

        /// <summary>
        /// Sets and gets the value of variable Size.
        /// </summary>
        public System.Drawing.Size Size
        {
            get { return size; }
            set { size = value; }
        }
        private string binding;

        /// <summary>
        /// Sets and gets the value of variable Binding.
        /// </summary>
        public string Binding
        {
            get { return binding; }
            set { binding = value; }
        }

        private Location location;

        /// <summary>
        /// Sets and gets the value of variable Location.
        /// </summary>
        public Location Location
        {
            get { return location; }
            set { location = value; }
        }
        private bool multiLine;

        /// <summary>
        /// Sets and gets the value of variable MultiLine.
        /// </summary>
        public bool MultiLine
        {
            get { return multiLine; }
            set { multiLine = value; }
        }

        private bool border;

        /// <summary>
        /// Sets and gets the value of variable BorderType.
        /// </summary>
        public bool Border
        {
            get { return border; }
            set { border = value; }
        }

        private string imageName;

        /// <summary>
        /// Sets and gets the value of variable ImageName.
        /// </summary>
        public string ImageName
        {
            get { return imageName; }
            set { imageName = value; }
        }

        private string type;

        /// <summary>
        /// Sets and gets the value of variable ImageName.
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private RowsCols rowsColumns;

        /// <summary>
        /// Sets and gets the value of variable RowsColumns.
        /// </summary>
       public RowsCols RowsColumns
        {
            get { return rowsColumns; }
            set { rowsColumns = value; }
        }

       private string text;

       /// <summary>
       /// Sets and gets the value of variable text.
       /// </summary>
       public string Text
       {
           get { return text; }
           set { text = value; }
       }

       private string name;

       /// <summary>
       /// Sets and gets the value of variable Name.
       /// </summary>
       public string Name
       {
           get { return name; }
           set { name = value; }
       }

        private int marginDecrementValue;

        /// <summary>
        /// Sets and gets the value of variable MarginDecrementValue.
        /// </summary>
        public int MarginDecrementValue
        {
            get { return marginDecrementValue; }
            set { marginDecrementValue = value; }
        }

        private int ymarginDecrementValue;

        /// <summary>
        /// Sets and gets the value of variable MarginDecrementValue.
        /// </summary>
        public int YMarginDecrementValue
        {
            get { return ymarginDecrementValue; }
            set { ymarginDecrementValue = value; }
        }
        private bool imageMedicalName = false;

        public bool ImageMedicalName
        {
            get { return imageMedicalName; }
            set { imageMedicalName = value; }
        }
        private TextAlign textAlign;

        public TextAlign TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }
        private int numberOfImages;

        public int NumberOfImages
        {
            get { return numberOfImages; }
            set { numberOfImages = value; }
        }
       #endregion

       /// <summary>
       /// Constructor
       /// </summary>
       public ReportControlProperties()
       {
           Location = new Location();
           Font = new IVLFont();
           Text = string.Empty;
           Name = string.Empty;
           RowsColumns = new RowsCols();
           Type = string.Empty;
           Binding = "None";
           ImageMedicalName = true;
           NumberOfImages = 1;
       }
    }

    //[Serializable]
    //public class ReportControlPropertiesList
    //{
    //   public List<ReportControlProperties> reportTemplateControls = null;
    //   public string orientation = string.Empty;
    //    public ReportControlPropertiesList()
    //    {
    //        reportTemplateControls = new List<ReportControlProperties>();
    //    }
    //}
}
