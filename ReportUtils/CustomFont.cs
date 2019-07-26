using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace ReportUtils
{
    [Serializable]
    public class IVLFont
    {

        //private Color fontColor;

        //public Color FontColor
        //{
        //    get { return fontColor; }
        //    set { fontColor = value; }
        //}

        private string fontColor;

        public string FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }
        private string fontFamily;

        public string FontFamily
        {
            get { return fontFamily; }
            set { fontFamily = value; }
        }
        private FontStyle fontStyle;

        public FontStyle FontStyle
        {
            get { return fontStyle; }
            set { fontStyle = value; }
        }

        private float fontSize;

        public float FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
       public IVLFont()
       {

       }
    }
}
