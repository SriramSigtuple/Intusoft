using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotation
{
    /// <summary>
    /// Helper class used to show properties
    /// for one or more comments objects
    /// </summary>
  public  class CommentsProperties
    {
        private string comments;
        private string header;
        private int x;
        private int y;
        private DateTime dt;
        public CommentsProperties()
        {
            comments = "";
            header = "Diabetic Retinopathy";
            x = 0;
            y = 0;
            dt = DateTime.Now;
        }

        public DateTime DateTimeVal
        {
            get
            {
                return dt;
            }
            set
            {
                dt = value;
            }
        }
        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
            }
        }

        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
    }
}
