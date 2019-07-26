using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtils
{
    public class RowsCols
    {
       public byte Rows;
       public byte Columns;

       public RowsCols()
       {
           _Rows = 1;
           _Columns = 1;
       }

       /// <summary>
       /// Gets and sets the value of Rows.
       /// </summary>
       public byte _Rows
       {
           get
           {
               return Rows;
           }
           set
           {
               Rows=value;
           }
       }

        /// <summary>
        /// Gets and sets the value of Columns.
        /// </summary>
        public byte _Columns
       {
           get
           {
               return Columns;
           }
           set
           {
               Columns = value;
           }
       }
    }
}
