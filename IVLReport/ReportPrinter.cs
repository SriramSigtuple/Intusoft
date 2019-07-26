using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using ReportUtils;

namespace IVLReport
{
   public static class ReportPrinter
    {

        //PdfDocument doc = new PdfDocument();
        //PdfPage page;
        //XGraphics xpdf;
        static double widthInInch;
        static double heightInInch;
        
       public static void PrintReport(List<ControlProperties> components)
       {
           const int dotsPerInch = 300;    // define the quality in DPI
           if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT)
           {
               widthInInch = 8;   // width of the bitmap in INCH
               heightInInch = 11;
           }
           else
           {
               widthInInch = 11;   // width of the bitmap in INCH
               heightInInch = 8;
           }
           Bitmap bmp = new Bitmap((int)(widthInInch * dotsPerInch), (int)(heightInInch * dotsPerInch));
           bmp.SetResolution(dotsPerInch, dotsPerInch);
          // Bitmap printBmp = new Bitmap(LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight);
           Graphics g = Graphics.FromImage(bmp);
          
           //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
           g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
           g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
          
           //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
           //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
          // g.FillRectangle(Brushes.White,new Rectangle(0,0,printBmp.Width,printBmp.Height));
           foreach (ControlProperties item in components)
           {
               drawComponent(g, item);
           }
           bmp.Save("print300.bmp");
       }

       private static void drawComponent(Graphics g,ControlProperties IVLProps)
       {
           Font font=new Font(IVLProps.FontName,IVLProps.FontSize);
           SolidBrush b = new SolidBrush(Color.FromName(IVLProps.ForeColor));
          Rectangle rect=new Rectangle(IVLProps.X,IVLProps.Y,IVLProps.Width,IVLProps.Height);
           switch (IVLProps.Type)
           {
               case "System.Windows.Forms.Label":
                   g.DrawString(IVLProps.Text, font, b, rect,StringFormat.GenericDefault);
                   break;

               case "System.Windows.Forms.Textbox":
                   g.DrawString(IVLProps.Text, font, b, rect, StringFormat.GenericDefault);
                   break;

               case "System.Windows.Forms.PictureBox":
                   Bitmap im = new Bitmap(IVLProps.ImageName);
                   g.DrawImage(im, new Rectangle(IVLProps.X, IVLProps.Y, IVLProps.Width, IVLProps.Height));
                   break;

               case "IVLReport.IVL_ImageTable":
                  
                   g.DrawRectangle(Pens.Black, new Rectangle(IVLProps.X, IVLProps.Y, IVLProps.Width, IVLProps.Height));
                  
                   //g.CopyFromScreen(new Point(IVLProps.X, IVLProps.Y), new Point(IVLProps.X, IVLProps.Y),new Size(IVLProps.Width,IVLProps.Height));
                   break;
           }
       }
       
      //public void SaveReportAsPDF(Bitmap bmp, string pdfFileName)
      //  {
      //      page = doc.AddPage();
      //      page.Size = PageSize.A4;
      //      // xpdf = XGraphics.FromPdfPage(page);
      //      // Bitmap bmp = new Bitmap((int)page.Width, (int)page.Height);

      //      const int dotsPerInch = 300;    // define the quality in DPI

      //      if (x.FirstChild.InnerText == "Portrait")
      //      {
      //          widthInInch = 8;   // width of the bitmap in INCH
      //          heightInInch = 11;
      //      }
      //      else if (x.FirstChild.InnerText == "LandScape")
      //      {
      //          widthInInch = 11;   // width of the bitmap in INCH
      //          heightInInch = 8;
      //      }
      //      Bitmap bmp = new Bitmap((int)(widthInInch * dotsPerInch), (int)(heightInInch * dotsPerInch));
      //      bmp.SetResolution(dotsPerInch, dotsPerInch);

      //      xg = Graphics.FromImage(bmp);

      //      xg.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
      //      //xg = XGraphics.FromGraphics(g, new XSize(bmp.Width, bmp.Height));
      //      xg.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
      //      xg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;



      //      foreach (XmlNode item in guiNode.ChildNodes)
      //      {
      //          MemoryStream stm = new MemoryStream();
      //          StreamWriter stw = new StreamWriter(stm);
      //          stw.Write(item.OuterXml);
      //          stw.Flush();
      //          stm.Position = 0;
      //          xp = (XProperties)ser.Deserialize(stm);
      //          if (x.FirstChild.InnerText == "Portrait")
      //          {
      //              scale2Portrait();
      //          }
      //          else if (x.FirstChild.InnerText == "LandScape")
      //          {
      //              scale2LandScape();
      //          }
      //          //TS1X-1902 method signature changed
      //          write2Pdf(xmlFile);
      //      }
      //      string bmpFileName = Directory.GetParent(xmlFile).ToString() + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(xmlFile) + ".bmp";
      //      bmp.Save(bmpFileName, System.Drawing.Imaging.ImageFormat.Png);
      //      XImage img = XImage.FromFile(bmpFileName);
      //      img.Interpolate = false;

      //      if (doc.Pages[0].Width > XUnit.FromPoint(img.Size.Width))
      //          doc.Pages[0].Width = XUnit.FromPoint(img.Size.Width);

      //      if (doc.Pages[0].Height > XUnit.FromPoint(img.Size.Height))
      //          doc.Pages[0].Height = XUnit.FromPoint(img.Size.Height);

      //      xpdf = XGraphics.FromPdfPage(page);
      //      xpdf.DrawImage(img, 0, 0);

      //      doc.Save(pdfFileName);

      //      //Statistics
      //      Statistics stat = new Statistics();
      //      stat.SetReportsGeneratedCount();

      //      bmp.Dispose();
      //      doc.Close();
      //      img.Dispose();
      //      xg.Dispose();
      //      xpdf.Dispose();
      // }
    }
}
