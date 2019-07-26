using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using INTUSOFT.Custom.Controls;
namespace Annotation
{
	/// <summary>
	/// Working area.
	/// Handles mouse input and draws graphics objects.
	/// </summary>
    /// 
    [Serializable]
	public class DrawArea : PictureBoxExtended
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        public int activeselectId = 0;
        public int idVal = 0;
        public bool isDelete = false;
        public bool ShowComments = true;
        public int[] ids;
        public bool iscup_drawn = false;
        public delegate void Unselectallannotationtext();
        public event Unselectallannotationtext _unselectall;
        public delegate void CommentsAddedEvent(AnnotationText c, EventArgs e);
        public event CommentsAddedEvent _commentsAddedEvent;
        public delegate void CurrentId(int id);
            public event CurrentId _currentId;
            public Color cupColor;
        public static bool isDrawCup = false;
        public static bool isDrawLine = false;

        public bool isZoom = false;
        private Point startingPoint = Point.Empty;
        private Point movingPoint = Point.Empty;
        private bool panning = false;

       unsafe public static CDRStruct* retCDRValues;

        unsafe public struct CDRStruct
        {
            public double DiscArea;
            public double CupArea;
            public double RimArea;
            public double InferiorRegionArea;
            public double SuperiorRegionArea;
            public double NasalRegionArea;
            public double TemporalRegionArea;
            public double VerticalLengthDisc;
            public double VerticalLengthCup;
            public double HorizontalLengthDisc;
            public double HorizontalLengthCup;
            public double VerticalCDR;
            public double HorizontalCDR;
        }
        #region Constructor, Dispose

		public DrawArea()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            iscup_List = new List<bool>();
            //annotationProperties = new AnnotationXMLProperties();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        #endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // DrawArea
            // 
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawArea_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        #region Enumerations

        public enum DrawToolType
        {
            Pointer,
            Rectangle,
            Ellipse,
            Line,
            Polygon,
            NumberOfDrawTools
        };

        #endregion

        #region Members

        public GraphicsList graphicsList;    // list of draw objects
                          // (instances of DrawObject-derived classes)
        public List<bool> iscup_List;
        private DrawToolType activeTool;      // active drawing tool
        private Tool[] tools;                 // array of tools

        // group selection rectangle
        private Rectangle netRectangle;
        private bool drawNetRectangle = false;

        // Information about owner form
        private Form1 owner;
        private GlaucomaTool ownerGlaucoma;

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the owner form
        /// </summary>
        public Form1 Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }

        /// <summary>
        /// Reference to the owner form
        /// </summary>
        public GlaucomaTool OwnerGlaucoma
        {
            get
            {
                return ownerGlaucoma;
            }
            set
            {
                ownerGlaucoma = value;
            }
        }
        /// <summary>
        /// Reference to DocManager
        /// </summary>
        //public DocManager DocManager
        //{
        //    get
        //    {
        //        return docManager;
        //    }
        //    set
        //    {
        //        docManager = value;
        //    }
        //}

        /// <summary>
        /// Group selection rectangle. Used for drawing.
        /// </summary>
        public Rectangle NetRectangle
        {
            get
            {
                return netRectangle;
            }
            set
            {
                netRectangle = value;
            }
        }

        /// <summary>
        /// Flas is set to true if group selection rectangle should be drawn.
        /// </summary>
        public bool DrawNetRectangle
        {
            get
            {
                return drawNetRectangle;
            }
            set
            {
                drawNetRectangle = value;
            }
        }

        /// <summary>
        /// Active drawing tool.
        /// </summary>
        public DrawToolType ActiveTool
        {
            get
            {
                return activeTool;
            }
            set
            {
                activeTool = value;
            }
        }

        /// <summary>
        /// List of graphics objects.
        /// </summary>
        public GraphicsList ListGraphics
        {
            get
            {
                return  graphicsList;
            }
            set
            {
                graphicsList = value;
            }
        }

        #endregion
        public Point active_point;
        #region Event Handlers


        /// <summary>
        /// Draw graphic objects and 
        /// group selection rectangle (optionally)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void DrawArea_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
            //e.Graphics.FillRectangle(brush, 
            //    this.ClientRectangle);
            //if (!isZoom)
            {
                if (graphicsList != null)
                {
                    //if (!isDrawLine)
                    graphicsList.Draw(e.Graphics);
                    //else
                    //    graphicsList.DrawLine(e.Graphics);
                    if (graphicsList.SelectionCount > 0)
                    {
                        //if (!graphicsList.currentType.Equals("Annotation.DrawLine"))
                        {
                            if (graphicsList.ids.Contains(graphicsList.selection_id))
                            {
                                activeselectId = graphicsList.selection_id;
                                _currentId(activeselectId);
                            }
                            else
                            {
                                //graphicsList.UnselectAll();
                                unselectalltext();
                            }
                        }
                    }
                }

                DrawNetSelection(e.Graphics);
            }
            //else
            //{
            //    e.Graphics.Clear(Color.White);
            //    e.Graphics.DrawImage(this.Image, movingPoint);
            //}
            //brush.Dispose();
        }

        /// <summary>
        /// Mouse down.
        /// Left button down event is passed to active tool.
        /// Right button down event is handled in this class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //This below code has been added by Darshan to resolve defect no 0000510: Duplicate numbering in comments if pressed on control key.
            //if (!isZoom)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    return;
                }
                else
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (isDrawCup && activeTool == DrawToolType.Polygon)
                        {
                            DrawObject.LastUsedColor = Color.FromName(AnnotationVariables.cupColor);
                            iscup_drawn = isDrawCup;

                        }
                        else
                        {
                            if (AnnotationVariables.isGlaucomaTool)
                                DrawObject.LastUsedColor = Color.FromName(AnnotationVariables.discColor);
                            else
                                DrawObject.LastUsedColor = Color.FromName(AnnotationVariables.annotationMarkingColor);

                        }
                        tools[(int)activeTool].OnMouseDown(this, e);
                        
                    }
                    else if (e.Button == MouseButtons.Right)
                        return;
                }
            }
            //else if(!ToolPointer.isZoom && isZoom)
            //{
            //    panning = true;
            //    this.Cursor = Cursors.Hand;
            //    startingPoint = new Point(e.Location.X - movingPoint.X,
            //                              e.Location.Y - movingPoint.Y);
            //}
                //OnContextMenu(e);
        }


        /// <summary>
        /// Mouse move.
        /// Moving without button pressed or with left button pressed
        /// is passed to active tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //This below code has been added by Darshan to resolve defect no 0000510: Duplicate numbering in comments if pressed on control key.
            //if (!isZoom)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    return;
                }
                else
                {
                    if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
                        tools[(int)activeTool].OnMouseMove(this, e);
                    else
                        this.Cursor = Cursors.Default;
                }
            }
            //else if (!ToolPointer.isZoom && isZoom)

            //{
            //    if (panning)
            //    {
            //        movingPoint = new Point(e.Location.X - startingPoint.X,
            //                                e.Location.Y - startingPoint.Y);
            //        this.Invalidate();
            //    }
            //    //Display_pbx.Invalidate();
            //}
        }

        /// <summary>
        /// Mouse up event.
        /// Left button up event is passed to active tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //This below code has been added by Darshan to resolve defect no 0000510: Duplicate numbering in comments if pressed on control key.
            //if ((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            //{
            //    return;
            //}
            //else
            //if (!isZoom)
            {
                if (e.Button == MouseButtons.Left)
                {
                    tools[(int)activeTool].OnMouseUp(this, e);
                    //isDrawCup = false;
                }
            }
            //else if (!ToolPointer.isZoom && isZoom)
            //{
            //    panning = false;
            //    this.Cursor = Cursors.Default;
            //}

        }

        public void CalculatePolygonArea()
        {

            
                for (int i = 0; i < graphicsList.Count; i++)
                {
                    if (graphicsList[i] is DrawPolygon)
                    {
                        DrawPolygon polyGon = graphicsList[i] as DrawPolygon;
                        if (iscup_List.Count > 0)
                        {
                            polyGon.isCup = iscup_List[i];
                         }
                    
                        polyGon.GetContourValue();
                    }
                }
                DrawPolygon.MeasureCDR();
           
        }

        #endregion
       
     public   ArrayList collapsablePanels;
     //public AnnotationXMLProperties annotationProperties;
     public List<AnnotationComments> comments;
        #region Other Functions

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="docManager"></param>
        public void Initialize(Form1 owner)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            // Keep reference to owner form
            this.Owner = owner;
            //this.DocManager = docManager;

            // set default tool
            activeTool = DrawToolType.Pointer;

            // create list of graphic objects
            graphicsList = new GraphicsList();
            
            collapsablePanels = new ArrayList();
            comments = new List<AnnotationComments>();
            // create array of drawing tools
            tools = new Tool[(int)DrawToolType.NumberOfDrawTools];
            tools[(int)DrawToolType.Pointer] = new ToolPointer();
            tools[(int)DrawToolType.Rectangle] = new ToolRectangle();
            tools[(int)DrawToolType.Ellipse] = new ToolEllipse();
            tools[(int)DrawToolType.Line] = new ToolLine();
            tools[(int)DrawToolType.Polygon] = new ToolPolygon();
        }
        public void Initialize(GlaucomaTool owner)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            // Keep reference to owner form
            this.OwnerGlaucoma = owner;
            //this.DocManager = docManager;

            // set default tool
            activeTool = DrawToolType.Pointer;

            // create list of graphic objects
            graphicsList = new GraphicsList();
            collapsablePanels = new ArrayList();
            comments = new List<AnnotationComments>();
            // create array of drawing tools
            tools = new Tool[(int)DrawToolType.NumberOfDrawTools];
            tools[(int)DrawToolType.Pointer] = new ToolPointer();
            tools[(int)DrawToolType.Rectangle] = new ToolRectangle();
            tools[(int)DrawToolType.Ellipse] = new ToolEllipse();
            tools[(int)DrawToolType.Line] = new ToolLine();
            tools[(int)DrawToolType.Polygon] = new ToolPolygon();
        }
        public void unselectalltext()
        {
            _unselectall();
        }
        public void CommentsAdded(AnnotationText c)
        {
            if (this.ShowComments)
            {
                EventArgs e = null;
                _commentsAddedEvent(c, e);
            }
        }

        ///// <summary>
        ///// Set dirty flag (file is changed after last save operation)
        ///// </summary>
        //public void SetDirty()
        //{
        //    DocManager.Dirty = true;
        //}

        /// <summary>
        ///  Draw group selection rectangle
        /// </summary>
        /// <param name="g"></param>
        public void DrawAfterdelete(int[] ids)
        {
            //DrawObject o;
            //for(int i=0;i<ids.Length;i++)
            //{
            //    o = (DrawObject)graphicsList[i];
            //    o.DrawAnnotationNumber(g, ids[i]);
            //}
           
        }
        public void GraphSelect(int id)
        {
             DrawObject o = null;
             ListGraphics.UnselectAll();
             int n = ListGraphics.Count;
            for ( int i = 0; i < n; i++ )
            {
                o = ListGraphics[i];
                if (o.id == id)
                {
                    o.Selected = true;
                   
                }
            }

            //if ( o != null )
            //{
            //    if ( ! o.Selected )
            //        GraphicsList.UnselectAll();
        }
        public void DrawNetSelection(Graphics g)
        {
            if ( ! DrawNetRectangle )
                return;
            
            ControlPaint.DrawFocusRectangle(g, NetRectangle, Color.Black, Color.Transparent);
        }

        /// <summary>
        /// Right-click handler
        /// </summary>
        /// <param name="e"></param>
        private void OnContextMenu(MouseEventArgs e)
        {
            // Change current selection if necessary

            Point point = new Point(e.X, e.Y);

            int n = ListGraphics.Count;
            DrawObject o = null;

            for ( int i = 0; i < n; i++ )
            {
                if (ListGraphics[i].HitTest(point) == 0)
                {
                    o = ListGraphics[i];
                    break;
                }
            }

            if ( o != null )
            {
                if ( ! o.Selected )
                    ListGraphics.UnselectAll();

                // Select clicked object
                o.Selected = true;
            }
            else
            {
                ListGraphics.UnselectAll();
            }

            Refresh();

            // Show context menu.
            // Make ugly trick which saves a lot of code.
            // Get menu items from Edit menu in main form and
            // make context menu from them.
            // These menu items are handled in the parent form without
            // any additional efforts.

            MainMenu mainMenu = Owner.Menu;    // Main menu
            MenuItem editItem = mainMenu.MenuItems[1];            // Edit submenu

            // Make array of items for ContextMenu constructor
            // taking them from the Edit submenu
            MenuItem[] items = new MenuItem[editItem.MenuItems.Count];

            for ( int i = 0; i < editItem.MenuItems.Count; i++ )
            {
                items[i] = editItem.MenuItems[i];
            }

           // Owner.SetStateOfControls();  // enable/disable menu items

            // Create and show context menu
            ContextMenu menu = new ContextMenu(items);
            menu.Show(this, point);

            // Restore items in the Edit menu (without this line Edit menu
            // is empty after forst right-click)
            editItem.MergeMenu(menu);
        }

        
        public void loadDrawings()
        {
          
        }

        #endregion

        

        
	}
    public class SerializationEventArgs : System.EventArgs
    {
        private IFormatter formatter;
        private Stream stream;
        private string fileName;
        private bool errorFlag;

        public SerializationEventArgs(IFormatter formatter, Stream stream,
            string fileName)
        {
            this.formatter = formatter;
            this.stream = stream;
            this.fileName = fileName;
            errorFlag = false;
        }

        public bool Error
        {
            get
            {
                return errorFlag;
            }
            set
            {
                errorFlag = value;
            }
        }

        public IFormatter Formatter
        {
            get
            {
                return formatter;
            }
        }

        public Stream SerializationStream
        {
            get
            {
                return stream;
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }
        }
    }

}
