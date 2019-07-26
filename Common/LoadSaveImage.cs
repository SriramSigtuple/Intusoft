using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
namespace Common
{
   public static class LoadSaveImage
    {
       public static bool LoadImage(string fileName, ref Bitmap bm)
       {
           if (!string.IsNullOrEmpty(fileName))
           { 
               try
               {
               FileStream fStream = File.Open(fileName, FileMode.Open);
              
                   Bitmap LoadBm = new Bitmap(fStream);
                   if (LoadBm != null)
                   {
                       if (bm == null)
                           bm = new Bitmap(LoadBm.Width, LoadBm.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                       else if (bm.Width != LoadBm.Width && bm.Height != LoadBm.Height)
                       {
                           bm.Dispose();
                           bm = new Bitmap(LoadBm.Width, LoadBm.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                       }
                       Graphics g = Graphics.FromImage(bm);
                       g.DrawImage(LoadBm, new Rectangle(0, 0, LoadBm.Width, LoadBm.Height));
                       g.Dispose();
                   }
                   fStream.Close();
                   fStream.Dispose();
                   LoadBm.Dispose();
                   bool isInvalidBm = false;
                   if (LoadBm == null)
                       isInvalidBm = false;
                   else
                       isInvalidBm = true;
                   return isInvalidBm;
               }
               catch (Exception)
               {

                   return false;
               }
             
           }
           return false;
       }
       public static void SaveImage(string filename, Bitmap bm)
       {
       }

    }
}
