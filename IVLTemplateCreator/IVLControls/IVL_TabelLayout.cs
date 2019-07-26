using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;

namespace IVLTemplateCreator.IVLControls
{
    class IVL_TabelLayout:TableLayoutPanel
    {
        public  int rows = 2;
        public  int cols = 2;
        public int colPercent = 50;
        public int rowPercent = 50;
        public IVL_TabelLayout()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.CellPaint += IVL_TabelLayout_CellPaint;
            this.Click+=IVL_TabelLayout_Click;
        }

        void IVL_TabelLayout_Click(object sender, EventArgs e)
        {
            createTable();
        }

        void IVL_TabelLayout_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Top));

        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
         
            createTable();
        }
        private void createTable()
        {
            
            rows=this.RowCount;
            cols = this.ColumnCount;
                for (int y = 0; y < this.RowCount; y++)
                {
                   
                    for (int x = 0; x < this.ColumnCount; x++)
                    {
                          this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,colPercent));
                }
                    this.RowStyles.Add(new RowStyle(SizeType.Percent, rowPercent));
            }
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
