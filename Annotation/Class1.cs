using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace Annotation
{
 public class Class1:FlowLayoutPanel
    {
     protected override Point ScrollToControl(Control activeControl)
     {
         //this.AutoScroll = true;
         //this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         //this.VScroll = true;
         //this.HScroll = false;
         return this.AutoScrollPosition;

     }
    }
}
