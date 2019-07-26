/*
 Author:	Serban Iulian
 Date:		14 September 2002
 Class:		ToolBoxTab
 e-mail:	iulianserban@hotmail.com
 Version:	1.0 
*/
using System;
using System.Windows.Forms;
using System.Drawing;
using INTUSOFT.ImageListView;
using System.Collections;
using System.Collections.Generic;
namespace ToolBoxLib
{
	/// <summary>
	/// Summary description for ToolBoxCategory.
	/// </summary>
	public class ToolBoxTab
	{
		public ToolBoxTab()
		{
			//
			// TODO: Add constructor logic here
			//


            this.ImgListView.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right);
            this.ImgListView.BackColor = System.Drawing.SystemColors.Control;
            this.ImgListView.Name = "Image List View";
            this.ImgListView.Size = new System.Drawing.Size(192, 384);

		}

        public List<System.Drawing.Design.ToolboxItem> arrItems = new List<System.Drawing.Design.ToolboxItem>();



        public int ItemCount
        {
            get { return arrItems.Count; }
        }

        public List<System.Drawing.Design.ToolboxItem> GetItems()
        {
            return arrItems;
        }

		public void AddItem(System.Drawing.Design.ToolboxItem Item)
		{
			arrItems.Add(Item);
		}
        public void Remove(System.Drawing.Design.ToolboxItem Item)
        {
            arrItems.Remove(Item);
        }


        private ImageListView imgListView=new ImageListView();

        public ImageListView ImgListView
        {
           
            get { return imgListView; }
            set { imgListView = value; }
        }




	}
}
