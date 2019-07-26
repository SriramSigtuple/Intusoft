/*
 Author:	Serban Iulian
 Date:		14 September 2002
 Class:		ToolBoxItem
 e-mail:	iulianserban@hotmail.com
 Version:	1.0 
*/
using System;

namespace ToolBoxLib
{
    /// <summary>
    /// Summary description for ToolBoxItem.
    /// </summary>
    public class ToolBoxItemDetails
    {
        public ToolBoxItemDetails()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private int nImageIndex;
        public int ImageIndex
        {
            get
            {
                return nImageIndex;
            }
            set
            {
                nImageIndex = value;
            }
        }

        private String Text;
        public String Caption
        {
            get
            {
                return Text;
            }
            set
            {
                Text = value;
            }
        }
    }
}
