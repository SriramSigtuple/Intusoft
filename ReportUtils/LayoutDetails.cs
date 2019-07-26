using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtils
{
   public static class LayoutDetails
    {
        //relative % Height wrt form height
        public const int Portrait_Images_Height = 75;
        public const int Portrait_Header_Height = 20;
        public const int Portrait_Footer_Height = 5;

        public const int Landscape_Images_Height = 100;
        public const int Landscape_Header_Height = 10;
        public const int Landscape_Footer_Height = 10;

        public static int Portrait_Width = 564;
        public static int Portrait_Height = 750;
        public static int Landscape_Width = 960;
        public static int Landscape_Height = 800;

        public const int LeftMargin = 10;
        public const int RightMargin = 10;
        public const int TopMargin = 10;
        public const int BottomtMargin = 10;

        public const int A4LanscapeWidth = 842;
        public const int A4LandscapeHeight = 595;
        public const int A4PortraitWidth = 595;
        public const int A4PortraitHeight = 842;
        public const int A5LanscapeWidth = 595;
        public const int A5LandscapeHeight = 419;
        public const int A5PortraitWidth = 419;
        public const int A5PortraitHeight = 595;

        public enum PageOrientation
        {
            PORTRAIT,
            LANDSCAPE,
            LANDSCAPE_A4,
            LANDSCAPE_A5,
            PORTRAIT_A4,
            PORTRAIT_A5,
            LANDSCAPEDR_A4,
            LANDSCAPEDR_A5,
            PORTRAITDR_A4,
            PORTRAITDR_A5,
            NONE
        }
        public enum DPI
        {
            DPI_72,DPI_96
        }
        //public static class Current
        //{
        //    public static PageOrientation Orientation = PageOrientation.PORTRAIT;

        //    private static int pageWidth = 0;
        //    public static int PageWidth
        //    {
        //        get { return (Orientation == PageOrientation.PORTRAIT) ? Portrait_Width - LeftMargin - RightMargin : Landscape_Width - LeftMargin - RightMargin; }
        //        set { pageWidth = value; }
        //    }

        //    private static int pageHeight = 0;
        //    public static int PageHeight
        //    {
        //        get { return (Orientation == PageOrientation.PORTRAIT) ? Portrait_Height - TopMargin - BottomtMargin : Landscape_Height - TopMargin - BottomtMargin ; }
        //        set { pageHeight = value; }
        //    }

        //    public static int ReportImgCnt = 1;

        //}

        public static class Current
        {
            public static PageOrientation Orientation = PageOrientation.LANDSCAPE_A4;

            public static DPI Dpi = DPI.DPI_72;

            private static int pageWidth = 0;
            public static int PageWidth
            {
                get { return pageWidth; }
                set { pageWidth = value; }
            }

            private static int pageHeight = 0;
            public static int PageHeight
            {
                get { return pageHeight; }
                set { pageHeight = value; }
            }

            public static int ReportImgCnt = 1;

        }
    }

   
}
