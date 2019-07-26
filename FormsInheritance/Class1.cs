using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
namespace INTUSOFT.Custom.Controls
{
    public class FormButtons : Button
    {
        public FormButtons()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }
    public class FormCheckBox : CheckBox
    {
        public FormCheckBox()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }
   public class FormComboBox : ComboBox
   {
       public FormComboBox()
       {
           this.SetStyle(ControlStyles.Selectable, false);
       }
	}
    
    public class PictureBoxExtended : PictureBox
    {

        private int index;

        #region Events
        public PictureBoxExtended()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
        /// <summary>
        /// Handler for when the mouse moves over the image part of the picture box
        /// </summary>
        public delegate void MouseMoveOverImageHandler(object sender, MouseEventArgs e);

        /// <summary>
        /// Occurs when the mouse have moved over the image part of a picture box
        /// </summary>
        public event MouseMoveOverImageHandler MouseMoveOverImage;

        #endregion


        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        #region Properties
        /// <summary>
        /// Gets the mouse position relative to the <see cref="PictureBox.Image">Image</see> top left corner
        /// </summary>
        /// <value>The location of the mouse translated onto the <see cref="PictureBox.Image">Image</see> .</value>
        public Point MousePositionOnImage
        {
            get { Point local = PointToClient(MousePosition); return TranslatePointToImageCoordinates(local); }
        }
        #endregion
        #region Methods
        #region Public Methods
        /// <summary>
        /// Translates a point to coordinates relative to the <see cref="PictureBox.Image">Image</see>.
        /// The supplied point is taken relativce to the control's upper left corner
        /// </summary>
        /// <param name="controlCoordinates">The point to translate, relative to the control's upper left corner.</param>
        /// <returns>A new point representing where over the <see cref="PictureBox.Image">Image</see> the supplied point is.</returns>
        public Point TranslatePointToImageCoordinates(Point controlCoordinates)
        {
            switch (SizeMode)
            {
                case PictureBoxSizeMode.Normal:
                    return TranslateNormalMousePosition(controlCoordinates);
                case PictureBoxSizeMode.AutoSize:
                    return TranslateAutoSizeMousePosition(controlCoordinates);
                case PictureBoxSizeMode.CenterImage:
                    return TranslateCenterImageMousePosition(controlCoordinates);
                case PictureBoxSizeMode.StretchImage:
                    return TranslateStretchImageMousePosition(controlCoordinates);
                case PictureBoxSizeMode.Zoom:
                    return TranslateZoomMousePosition(controlCoordinates);
            }
            throw new NotImplementedException("PictureBox.SizeMode was not in a valid state");
        }
        #endregion
        #region Protected Methods
        /// <summary>
        /// Gets the mouse position over the image when the <see cref="PictureBox">PictureBox's</see> <see cref="PictureBox.SizeMode">SizeMode</see> is set to AutoSize
        /// </summary>
        /// <param name="coordinates">Point to translate</param>
        /// <returns>A point relative to the top left corner of the <see cref="PictureBox.Image">Image</see></returns>
        /// <remarks>
        /// In AutoSize mode, the <see cref="PictureBox">PictureBox</see> is automagically resized* to the size of the <see cref="PictureBox.Image">Image.</see>
        /// Thus, the image is at the top left corner of the control, and no translation takes place.
        /// * This is not necessary true.  The <see cref="PictureBox">PictureBox</see> may NOT be resized depending on how it is docked in it's parent.
        /// However, even in these cases no translation is needed, as the image is rendered the same as if it was in Normal mode
        /// </remarks>
        protected Point TranslateAutoSizeMousePosition(Point coordinates)
        {
            //TODO: When we implement scrolling, we will have to make sure we test that properly. As of now, not sure how the rendering will take place
            return coordinates;
        }

        /// <summary>
        /// Gets the mouse position over the image when the <see cref="PictureBox">PictureBox's</see> <see cref="PictureBox.SizeMode">SizeMode</see> is set to Zoom
        /// </summary>
        /// <param name="coordinates">Point to translate</param>
        /// <returns>A point relative to the top left corner of the <see cref="PictureBox.Image">Image</see>
        /// If the Image is null, no translation is performed
        /// </returns>
        protected Point TranslateZoomMousePosition(Point coordinates)
        {
            //	test to make sure our image is not null
            if (Image == null) return coordinates;
            //	Make sure our control width and height are not 0 and our image width and height are not 0
            if (Width == 0 || Height == 0 || Image.Width == 0 || Image.Height == 0) return coordinates;
            //	This is the one that gets a little tricky.  Essentially, need to check the aspect ratio of the image to the aspect ratio of the control
            // to determine how it is being rendered
            float imageAspect = (float)Image.Width / Image.Height;
            float controlAspect = (float)Width / Height;
            float newX = coordinates.X;
            float newY = coordinates.Y;
            if (imageAspect > controlAspect)
            {
                //	This means that we are limited by width, meaning the image fills up the entire control from left to right
                float ratioWidth = (float)Image.Width / Width;
                newX *= ratioWidth;
                float scale = (float)Width / Image.Width;
                float displayHeight = scale * Image.Height;
                float diffHeight = Height - displayHeight;
                diffHeight /= 2;
                newY -= diffHeight;
                newY /= scale;
            }
            else
            {
                //	This means that we are limited by height, meaning the image fills up the entire control from top to bottom
                float ratioHeight = (float)Image.Height / Height;
                newY *= ratioHeight;
                float scale = (float)Height / Image.Height;
                float displayWidth = scale * Image.Width;
                float diffWidth = Width - displayWidth;
                diffWidth /= 2;
                newX -= diffWidth;
                newX /= scale;
            }
            return new Point((int)newX, (int)newY);
        }

        /// <summary>
        /// Gets the mouse position over the image when the <see cref="PictureBox">PictureBox's</see> <see cref="PictureBox.SizeMode">SizeMode</see> is set to StretchImage
        /// </summary>
        /// <param name="coordinates">Point to translate</param>
        /// <returns>A point relative to the top left corner of the <see cref="PictureBox.Image">Image</see>
        /// If the Image is null, no translation is performed
        /// </returns>
        protected Point TranslateStretchImageMousePosition(Point coordinates)
        {
            //	test to make sure our image is not null
            if (Image == null) return coordinates;
            //	Make sure our control width and height are not 0
            if (Width == 0 || Height == 0) return coordinates;
            //	First, get the ratio (image to control) the height and width
            float ratioWidth = (float)Image.Width / Width;
            float ratioHeight = (float)Image.Height / Height;
            //	Scale the points by our ratio
            float newX = coordinates.X;
            float newY = coordinates.Y;
            newX *= ratioWidth;
            newY *= ratioHeight;
            return new Point((int)newX, (int)newY);
        }

        /// <summary>
        /// Gets the mouse position over the image when the <see cref="PictureBox">PictureBox's</see> <see cref="PictureBox.SizeMode">SizeMode</see> is set to Center
        /// </summary>
        /// <param name="coordinates">Point to translate</param>
        /// <returns>A point relative to the top left corner of the <see cref="PictureBox.Image">Image</see>
        /// If the Image is null, no translation is performed
        /// </returns>
        protected Point TranslateCenterImageMousePosition(Point coordinates)
        {
            //	Test to make sure our image is not null
            if (Image == null) return coordinates;
            //	First, get the top location (relative to the top left of the control) of the image itself
            // To do this, we know that the image is centered, so we get the difference in size (width and height) of the image to the control
            int diffWidth = Width - Image.Width;
            int diffHeight = Height - Image.Height;
            //	We now divide in half to accomadate each side of the image
            diffWidth /= 2;
            diffHeight /= 2;
            //	Finally, we subtract this numer from the original coordinates
            // In the case that the image is larger than the picture box, this still works
            coordinates.X -= diffWidth;
            coordinates.Y -= diffHeight;
            return coordinates;
        }

        /// <summary>
        /// Gets the mouse position over the image when the <see cref="PictureBox">PictureBox's</see> <see cref="PictureBox.SizeMode">SizeMode</see> is set to Normal
        /// </summary>
        /// <param name="coordinates">Point to translate</param>
        /// <returns>A point relative to the top left corner of the <see cref="PictureBox.Image">Image</see></returns>
        /// <remarks>
        /// In normal mode, the image is placed in the top left corner, and as such the point does not need to be translated.
        /// The resulting point is the same as the original point
        /// </remarks>
        protected Point TranslateNormalMousePosition(Point coordinates)
        {
            //	TODO: When we implement scrolling in this, we will need to test for scroll offset
            //	NOTE: As it stands now, this could be made static, but in the future we will be making this handle scaling
            return coordinates;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
        /// If the mouse is over the <see cref="PictureBox.Image">Image</see>, raises the <see cref="MouseMoveOverImage"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //if (Image != null)
            //{
            //    if (MouseMoveOverImage != null)
            //    {
            //        Point p = TranslatePointToImageCoordinates(e.Location);
            //        if (p.X >= 0 && p.X < Image.Width && p.Y >= 0 && p.Y < Image.Height)
            //        {
            //            MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta);
            //            MouseMoveOverImage(this, ne);
            //        }
            //    }
            //}
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        #endregion
        #endregion
    }
    public class FormDataGridView : DataGridView
    {
        public FormDataGridView()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            this.VerticalScrollBar.Visible = true;
            //this.VerticalScrollBar.VisibleChanged += VerticalScrollBar_VisibleChanged;
        }
        public void SetCellsTransparent()
        {
            this.EnableHeadersVisualStyles = false;
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.Transparent;
            this.RowHeadersDefaultCellStyle.BackColor = Color.Transparent;


            foreach (DataGridViewColumn col in this.Columns)
            {
                col.DefaultCellStyle.BackColor = Color.Transparent;
                col.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            }
        }
        

      

        //void VerticalScrollBar_VisibleChanged(object sender, EventArgs e)
        //{
        //    if (!VerticalScrollBar.Visible)
        //    {
        //        int width = VerticalScrollBar.Width;
        //        VerticalScrollBar.Location =
        //          new Point(ClientRectangle.Width - width, 1);
        //        VerticalScrollBar.Size =
        //          new Size(width, ClientRectangle.Height+17 - 1 - this.HorizontalScrollBar.Height);
        //        VerticalScrollBar.Show();
        //    }
        //}

    }
    public class FormTextBox : TextBox
    {
        public FormTextBox()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }

    }
    public class FormUserControl : UserControl
    {
        public FormUserControl()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }
    public class FormFlowLayoutPanel : FlowLayoutPanel
    {
        public FormFlowLayoutPanel()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
       
       
    }
    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }
    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;
        public bool Enabled
        {
            get
            {
                return enabledValue;
            }
            set
            {
                enabledValue = value;
            }
        }

        // Override the Clone method so that the Enabled property is copied. 
        public override object Clone()
        {
            DataGridViewDisableButtonCell cell =
                (DataGridViewDisableButtonCell)base.Clone();
          
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the button cell. 
        public DataGridViewDisableButtonCell()
        {
            this.enabledValue = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,   
            // background, and disabled button for the cell. 
            if (!this.enabledValue)
            {
                // Draw the cell background, if specified. 
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified. 
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment =
                    this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;
                
                // Draw the disabled button.                
                ButtonRenderer.DrawButton(graphics, buttonArea,
                    PushButtonState.Disabled);

                // Draw the disabled button text.  
                if (this.FormattedValue is String)
                {
                    TextRenderer.DrawText(graphics,
                        (string)this.FormattedValue,
                        this.DataGridView.Font,
                        buttonArea, SystemColors.GrayText);
                   
                }
            }
            else
            {
                // The button cell is enabled, so let the base class  
                // handle the painting. 
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }

    public class FormToolStripRenderer : ToolStripRenderer
    {
        public bool isButtonClicked = false;
        public FormToolStripRenderer()
        {

        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {

            if (e.ToolStrip.GetType() == typeof(ToolStrip))
            {
                // skip render border
            }
            else
            {
                // do render border
                base.OnRenderToolStripBorder(e);
            }

        }
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
                    if (!e.Item.Selected)
                    {
                        base.OnRenderButtonBackground(e);
                    }

                    else
                    {
                        Brush b = new SolidBrush(Color.FromArgb(90, 255, 255, 255));

                        Rectangle rectangle = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height - 1);
                        e.Graphics.FillRectangle(b, rectangle);

                    }
            }

        }
    public class BaseGradientForm : Form
    {

        private Color _Color1 = Color.Black;
        private Color _Color2 = Color.White;
        private float _ColorAngle = 30f;
        private Color _FontForeColor = Color.White;
        private string _ThemeName = string.Empty;

        #region Public Properties

        public Color Color1
        {
            get { return _Color1; }
            set
            {
                _Color1 = value;
                this.Invalidate(); // Tell the Form to repaint itself

                this.Update();

            }
        }

        public Color Color2
        {
            get { return _Color2; }
            set
            {
                _Color2 = value;
                this.Invalidate(); // Tell the Form to repaint itself
                this.Update();

            }
        }
        public Color FontColor
        {
            get { return _FontForeColor; }
            set
            {
                _FontForeColor = value;

                List<Control> controls = GetControls(this).ToList();
                foreach (Control c in controls)
                {
                    //if (c.Name != emr_btn.Name)
                        c.ForeColor = value;
                }
                this.Invalidate(); // Tell the Form to repaint itself
                this.Update();

            }
        }
        public static IEnumerable<Control> GetControls(Control form)
        {
            foreach (Control childControl in form.Controls)
            {   // Recurse child controls.
                foreach (Control grandChild in GetControls(childControl))
                {
                    yield return grandChild;
                }
                yield return childControl;
            }
        }
        public string ThemeName
        {
            get { return _ThemeName; }
            set
            {
                _ThemeName = value;
                this.Invalidate(); // Tell the Form to repaint itself
                this.Update();

            }
        }
        public float ColorAngle
        {
            get { return _ColorAngle; }
            set
            {
                _ColorAngle = value;
                this.Invalidate(); // Tell the Form to repaint itself
                this.Update();
            }
        }
        public BaseGradientForm()
        {
            SetStyles();
        }
        private void SetStyles()
        {
            // Makes sure the form repaints when it was resized
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Getting the graphics object
            Graphics g = pevent.Graphics;

            // Creating the rectangle for the gradient
            Rectangle rBackground = new Rectangle(0, 0, this.Width, this.Height);

            // Creating the lineargradient
            System.Drawing.Drawing2D.LinearGradientBrush bBackground
                = new System.Drawing.Drawing2D.LinearGradientBrush(rBackground, _Color1, _Color2, _ColorAngle);

            // Draw the gradient onto the form

            g.FillRectangle(bBackground, rBackground);
           // Draw the gradient onto the title bar
            //Rectangle screenRectangle=RectangleToScreen(this.ClientRectangle);
            //Rectangle rTitleBar = new Rectangle(screenRectangle.X,0,screenRectangle.Width, screenRectangle.Top - this.Top);

            //g.FillRectangle(bBackground, rTitleBar);
            //
            // Disposing of the resources held by the brush
            bBackground.Dispose();
        }

        #endregion
    }
}