using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace INTUSOFT.ThumbnailModule
{
    public class ThumbnailFlowLayoutPanel : FlowLayoutPanel
    {
        public List<int> selectedThumbnails = new List<int>();
        public int TotalThumbnails = 0;
        public List<string> SelectedThumbnailFileNames = new List<string>();
        
        protected override Point ScrollToControl(Control activeControl)
            {
            //this.AutoScroll = true;
            //this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            //this.VScroll = true;
            //this.HScroll = false;
            return this.AutoScrollPosition;

        }
        //private void buttonUp_Click(object sender, EventArgs e)
        //{
        //    if (i >= 0) i = -1;
        //    yourPanel.ScrollUp(i--);
        //}
        //private void buttonDown_Click(object sender, EventArgs e)
        //{
        //    if (i < 0) i = 0;
        //    yourPanel.ScrollDown(i++);
        //}
  
    }

     //This can help you control the scrollbar with scrolling up and down.
        //The position is a little special.
        //Position for scrolling up should be negative.
        //Position for scrolling down should be positive
        public static class PanelExtension
        {
            public static void ScrollDown(this Panel p,ImageViewer c)
            {
                //pos passed in should be positive
                {
                 int index =   p.Controls.GetChildIndex(c);
                 //p.AutoScrollPosition = new Point(p.AutoScrollPosition.X, c.Size.Height * index);

                    p.ScrollControlIntoView(p.Controls[index]);
                }
            }
            public static void ScrollUp(this Panel p, ImageViewer c)
            {
                //pos passed in should be negative
                {
                    p.ScrollControlIntoView(c);
                }
            }
        }
}
