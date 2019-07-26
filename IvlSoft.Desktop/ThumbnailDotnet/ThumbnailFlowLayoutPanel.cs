using System;
using System.Windows.Forms;
using System.Drawing;

namespace marlie.TumbnailDotnet
{
    public class ThumbnailFlowLayoutPanel : FlowLayoutPanel
    {
        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }
    }
}
