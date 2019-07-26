using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Annotation
{
    [Serializable]
   public class AnnotationComments
    {
        private string comments;
        private string header;
        private int x;
        private int y;
        private int id;
        private DateTime dt;

        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public AnnotationComments()
        {

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
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
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
