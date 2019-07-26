using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace IVLTemplateCreator.IVLControls
{
    class IVL_TextBox : TextBox
    {
        public IVL_TextBox()
        {
            this.BorderStyle = BorderStyle.None;
            this.Multiline = true;
            this.MinimumSize = new Size(30, 15);
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
