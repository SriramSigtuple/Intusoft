using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtils
{
   public class Location
    {
       private short X;
       private short Y;
       public Location()
       {
           _X = 10;
           _Y = 10;
       }

       /// <summary>
       /// Gets and sets the value of X.
       /// </summary>
       public short _X
       {
           get
           {
               return X;
           }
           set
           {
               X = value;
           }
       }

       /// <summary>
       /// Gets and sets the value of Y.
       /// </summary>
       public short _Y
       {
           get
           {
               return Y;
           }
           set
           {
               Y = value;
           }
       }
    }
}
