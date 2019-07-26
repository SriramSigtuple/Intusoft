using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Plasmoid.Extensions;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.ComponentModel;

namespace AssemblySoftware
{
    public class HarrProgressBar : Panel
    {
        public int RoundedCornerAngle { get; set; }
        public int LeftBarSize { get; set; }
        public int RightBarSize { get; set; }
        public int StatusBarSize { get; set; }
        public Padding Padding { get; set; }

        public Font Font { get; set; }
        public string LeftText { get; set; }
        public string StatusText { get; set; }

        private Color StatusColor1;
        private Color StatusColor2;
        private int _StatusBarColorIndex;
        /// <summary>
        /// ColorIndex. [0 - Raw active] [1 - Raw inactive] [2 - Dry active] [3 - Dry inactive].
        /// </summary>
        public int StatusBarColor
        {
            get { return _StatusBarColorIndex; }
            set
            {
                switch (value)
                {
                    case 0:
                        // Raw active
                        StatusColor1 = Color.OliveDrab;
                        StatusColor2 = Color.DarkOliveGreen;
                        break;
                    case 1:
                        // Raw inactive
                        StatusColor1 = Color.OliveDrab;
                        StatusColor2 = Color.Gray;
                        break;
                    case 2:
                        // Dry active
                        StatusColor1 = Color.Goldenrod;
                        StatusColor2 = Color.DarkGoldenrod;
                        break;
                    case 3:
                        // Dry inactive
                        StatusColor1 = Color.Goldenrod;
                        StatusColor2 = Color.Gray;
                        break;
                    default:
                        StatusColor1 = Color.DimGray;
                        StatusColor2 = Color.DimGray;
                        break;
                }
            }
        }

        private Color FirstColor;
        private Color SecondColor;
        private int _FillDegree = 50;
        public int FillDegree
        {
            get { return _FillDegree; }
            set 
            {
                if (value >= 100)
                {
                    FirstColor = Color.Red;
                    SecondColor = Color.DarkRed;
                }
                else if (value > 90)
                {
                    FirstColor = Color.Orange;
                    SecondColor = Color.DarkOrange;
                }
                else if (value > 80)
                {
                    FirstColor = Color.Gold;
                    SecondColor = Color.DarkGoldenrod;
                }               
                else
                {
                    FirstColor = Color.Green;
                    SecondColor = Color.DarkGreen;
                }
                _FillDegree = value;
            }
        }

        //Check radius for begin drag n drop
        public bool AllowDrag { get; set; }
        private bool _isDragging = false;
        private int _DDradius = 40;
        private int _mX=0;
        private int _mY=0;

        public HarrProgressBar()
        {
            Font = new Font("Arial", 10);
            FillDegree = 50;
            RoundedCornerAngle = 10;
            Margin = new Padding(0);
            LeftText = "LT";
            StatusText = "Not set";
            LeftBarSize = 30;
            StatusBarSize = 60;
            RightBarSize = 30;
            StatusBarColor = 99;
            AllowDrag = true;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            this.BackColor = Color.SandyBrown;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.BackColor = Color.Transparent;
            base.OnLostFocus(e);
        }

        protected override void OnClick(EventArgs e)
        {
            this.Focus();
            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
            _mX = e.X;
            _mY = e.Y;
            this._isDragging = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_isDragging)
            {
                // This is a check to see if the mouse is moving while pressed.
                // Without this, the DragDrop is fired directly when the control is clicked, now you have to drag a few pixels first.
                if (e.Button == MouseButtons.Left && _DDradius > 0 && this.AllowDrag)
                {
                    int num1 = _mX - e.X;
                    int num2 = _mY - e.Y;
                    if (((num1 * num1) + (num2 * num2)) > _DDradius)
                    {
                        DoDragDrop(this, DragDropEffects.All);
                        _isDragging = true;
                        return;
                    }
                }
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            base.OnMouseUp(e);
        }        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            paintThisShit(e.Graphics);
        }

        public void paintThisShit(Graphics _graphics)
        {
            // Textformat
            StringFormat f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Center;

            // Misc
            _graphics = this.CreateGraphics();
            System.Drawing.Drawing2D.LinearGradientBrush _LeftAndRightBrush = new LinearGradientBrush(GetMainArea(), Color.DimGray, Color.Black, LinearGradientMode.Vertical);
            System.Drawing.Drawing2D.LinearGradientBrush _StatusBrush = new LinearGradientBrush(GetMainArea(), StatusColor1, StatusColor2, LinearGradientMode.Vertical);
            System.Drawing.Drawing2D.LinearGradientBrush _MainBrush = new LinearGradientBrush(GetMainArea(), FirstColor, SecondColor, LinearGradientMode.Vertical);
            
            // Draw left
            if (LeftBarSize > 0)
            {
                _graphics.FillRoundedRectangle(_LeftAndRightBrush, this.GetLeftArea(), this.RoundedCornerAngle, RectangleEdgeFilter.TopLeft | RectangleEdgeFilter.BottomLeft);
                _graphics.DrawString(this.LeftText, this.Font, Brushes.White, this.GetLeftArea(), f);
            }
            
            // Draw status
            if (StatusBarSize > 0)
            {
                _graphics.FillRoundedRectangle(_StatusBrush, this.GetStatusArea(), this.RoundedCornerAngle, RectangleEdgeFilter.None);
                _graphics.DrawString(this.StatusText, this.Font, Brushes.White, this.GetStatusArea(), f);
            }

            // Draw main background
            _graphics.FillRoundedRectangle(Brushes.DimGray, GetMainAreaBackground(), this.RoundedCornerAngle, RectangleEdgeFilter.None);


            // Clean up
            _LeftAndRightBrush.Dispose();
            _MainBrush.Dispose();
            _StatusBrush.Dispose();
        }

        private Rectangle GetLeftArea()
        {
            return new Rectangle(
                Padding.Left,
                Padding.Top, 
                LeftBarSize,
                this.ClientRectangle.Height - Padding.Bottom - Padding.Top);
        }

        private Rectangle GetStatusArea()
        {
            return new Rectangle(
                Padding.Left + LeftBarSize,
                Padding.Top,
                StatusBarSize,
                this.ClientRectangle.Height - Padding.Bottom - Padding.Top);
        }

        private Rectangle GetMainArea()
        {
            return new Rectangle(
                Padding.Left + LeftBarSize + StatusBarSize,
                Padding.Top,
                Convert.ToInt32(((this.ClientRectangle.Width - (Padding.Left + LeftBarSize + StatusBarSize + RightBarSize + Padding.Right)) * FillDegree) / 100),
                this.ClientRectangle.Height - Padding.Bottom - Padding.Top);
        }

        private Rectangle GetMainAreaBackground()
        {
            return new Rectangle(
                   Padding.Left + LeftBarSize + StatusBarSize,
                   Padding.Top,
                   this.ClientRectangle.Width - (Padding.Left + LeftBarSize + StatusBarSize + RightBarSize + Padding.Right),
                   this.ClientRectangle.Height - Padding.Bottom - Padding.Top);
        }

        private Rectangle GetRightArea()
        {
            return new Rectangle(
                this.ClientRectangle.Width - (RightBarSize + Padding.Right),
                Padding.Top, 
                RightBarSize,
                this.ClientRectangle.Height - Padding.Bottom - Padding.Top);
        }
    }
}
