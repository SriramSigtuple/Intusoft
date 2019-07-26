using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace IVLTemplateCreator.IVLControls
{
    class IVL_Label : Label
    {
        public IVL_Label()
        {
            this.AutoSize = false;
            this.BorderStyle = BorderStyle.None;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.MinimumSize = new Size(20, 10);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            LayoutMouseHandler.OnMouseDown(this, e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            LayoutMouseHandler.OnMouseUp(this, e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            LayoutMouseHandler.OnMouseMove(this, e);
        }
        

    }
}
