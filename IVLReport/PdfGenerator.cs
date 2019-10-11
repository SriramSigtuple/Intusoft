using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Printing;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using Microsoft.Win32;
using ReportUtils;
using INTUSOFT.ThumbnailModule;
using INTUSOFT.Imaging;
using Common;
using PdfFileWriter;
using System.Reflection;
using Emgu.Util;//Emgu dll's added to apply mask for the captured image. By Ashutosh 22-08-2017
using Emgu.CV.Structure;
using Emgu.CV;
//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
namespace IVLReport
{
    public class PdfGenerator
    {
        double MappingValue;
        private double a4Width = 11.69;
        private double a4Height = 8.27;

        private double a5Width = 8.27;
        private double a5Height = 5.83;

        public Dictionary<string, string> AnnotationComments;
        public bool isJsonFormat = true;//Variable has been added to maintain status of the report format weather it is xml or json.
        string TestPdfOpen_message = string.Empty;//this line has been added to provide the message for the message box when test.pdf is opened.
        string TestPdfOpen_header = string.Empty;//this line has been added to provide the header for the message box when test.pdf is opened.
        private PdfFont ArialNormal;
        private PdfFont ArialBold;
        private PdfFont ArialItalic;
        private PdfFont ArialBoldItalic;
        private PdfFont TimesNormal;

        private PdfDocument Document;
        //private iTextSharp.text.Document Document;
       // private PdfWriter pdfWriter;
        private PdfPage Page;
        //private iTextSharp.text.pdf.PdfPage Page;
        private PdfContents Contents;
        //private iTextSharp.text.pdf.PdfContents Contents;
        private DataModel _dataModel;
        double dpi = 96;

        public PdfGenerator()
        {
            _dataModel = DataModel.GetInstance();
        }
        private void DefineFontResources()
        {
            // Define font resources
            // Arguments: PdfDocument class, font family name, font style, embed flag
            // Font style (must be: Regular, Bold, Italic or Bold | Italic) All other styles are invalid.
            // Embed font. If true, the font file will be embedded in the PDF file.
            // If false, the font will not be embedded
            String FontName1 = "Arial";
            String FontName2 = "Times New Roman";
            //iTextSharp.text.pdf.PdfFont f = new iTextSharp.text.pdf.PdfFont();

            ArialNormal = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Regular, true);
            ArialBold = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Bold, true);
            ArialItalic = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Italic, true);
            ArialBoldItalic = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Bold | FontStyle.Italic, true);
            TimesNormal = PdfFont.CreatePdfFont(Document, FontName2, FontStyle.Regular, true);

            //ArialNormal = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Regular, true);
            //ArialBold = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Bold, true);
            //ArialItalic = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Italic, true);
            //ArialBoldItalic = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Bold | FontStyle.Italic, true);
            //TimesNormal = PdfFont.CreatePdfFont(Document, FontName2, FontStyle.Regular, true);

            return;
        }

        public void AddPDFTable(ReportControlProperties props)
        {
            double x = props.Location._X;
            double y = Math.Abs(props.Location._Y - props.Size.Height);

            PdfTable pdfTable = new PdfTable(Page, Contents, ArialNormal, props.Font.FontSize);
            string result = string.Empty;
            if (AnnotationComments != null && AnnotationComments.Count > 0)//check if count value is greater than zero.pdf table created only if count >0. Sriram sir/ ashutosh 20/7
            {
                if (LayoutDetails.Current.Orientation.ToString().Contains("A4"))
                    pdfTable.SetColumnWidth(Convert.ToSingle(AnnotationComments["$tableColumnHeightA4"]), Convert.ToSingle(AnnotationComments["$tableColumnWidthA4"]));
                else
                    pdfTable.SetColumnWidth(Convert.ToSingle(AnnotationComments["$tableColumnHeightA5"]), Convert.ToSingle(AnnotationComments["$tableColumnWidthA5"]));
                AnnotationComments.Remove("$tableColumnHeightA4");
                AnnotationComments.Remove("$tableColumnWidthA4");
                AnnotationComments.Remove("$tableColumnHeightA5");
                AnnotationComments.Remove("$tableColumnWidthA5");
                PdfRectangle rect = pdfTable.TableArea;

                rect.Top = (MappingValue - (double)props.Location._Y) / dpi;

                rect.Right = (double)(props.Location._X + props.Size.Width) / dpi;
                rect.Left = (double)x / dpi;
                rect.Bottom = y / dpi;
                pdfTable.TableArea = rect;
                //for (int i = 0; i < AnnotationComments.Count; i++)
                foreach (KeyValuePair<string, string> item in AnnotationComments)
                {
                    {
                        if (item.Value != null)
                            result = Regex.Replace(item.Value, @"\r\n?|\n", " ");
                        else
                            result = string.Empty;
                        PdfFileWriter.TextBox Box = pdfTable.Cell[1].CreateTextBox();
                        Box.AddText(TimesNormal, props.Font.FontSize, Color.Black, result);
                        pdfTable.Cell[0].Value = item.Key;
                        pdfTable.DrawRow();
                    }
                }
                pdfTable.Close();
            }
        }

        private void addPdfLabel(ReportControlProperties props)
        {
            PdfFont pdf = PdfFont.CreatePdfFont(Document, props.Font.FontFamily, props.Font.FontStyle, true);
            //PdfFont pdf = PdfFont.CreatePdfFont(Document, props.Font.FontFamily, props.Font.FontStyle, true);
            //PdfFont pdf = PdfFont.CreatePdfFont(Document, props.Font.FontFamily, props.Font.FontStyle, true);
            //BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //PdfContentByte cb = pdfWriter.DirectContent;
           
          //  iTextSharp.text.pdf.PdfFont pdf = new iTextSharp.text.pdf.PdfFont(BaseFont.TIMES_BOLD,props.Font.FontSize);
            PaintOp p = PaintOp.CloseStroke;

            Color color = Color.FromName(props.Font.FontColor);
            if (!color.IsKnownColor)
            {
                color = ColorTranslator.FromHtml("#" + color.Name);
            }
            
            //if (props.Text != null)
            //    props.Text = Regex.Replace(props.Text, @"\r\n?|\n", " ");

            //cb.BeginText();
            //cb.SetFontAndSize(f_cn, props.Font.FontSize);
            //cb.SetColorFill(new BaseColor(color.R, color.G, color.B, color.A));
            ////PageSize.A4
            //cb.SetTextMatrix(props.Location._X, PageSize.A4.Height - props.Location._Y);
            //cb.ShowText(props.Text);
            //cb.EndText();

            //double xMargin = -14.0;
            //double originX = (double)(props.Location._X + props.MarginDecrementValue);
            double originX = (double)(props.Location._X )/dpi;
            double originY = (MappingValue - (double)(props.Location._Y + props.Size.Height))/dpi;
            
            double originWidth = (double)props.Size.Width/dpi;
            double originHeight = (double)props.Size.Height/dpi;
            

             PdfFileWriter.TextBox box = new PdfFileWriter.TextBox(originWidth);
             box.AddText(pdf, props.Font.FontSize,color, props.Text);
         
             
            double fontSize = props.Font.FontSize;
            double widthVal = 0;

            if (!string.IsNullOrEmpty(props.Text))
            {
                if (props.Border)
                {
                    if (props.MultiLine)
                    {
                        originY = (MappingValue - (double)(props.Location._Y )) / dpi;
                        originY -= 0.11;

                    }
                    else
                    {
                        double boxHeight = (double)props.Size.Height / 2.0;
                        originY = (MappingValue - ((double)props.Location._Y + boxHeight)) / dpi;
                        originY -= 0.02;

                    }

                }

                //widthVal = Contents.DrawText(originX - 0.12 + (double)props.MarginDecrementValue/dpi, ref originY, 0, 0, box);
                widthVal = Contents.DrawText(pdf, props.Font.FontSize, originX - 0.12 + (double)props.MarginDecrementValue/dpi, originY + (double)props.YMarginDecrementValue / dpi,TextJustify.Left,0,color,color,props.Text);

            }
            if (props.Border)
            {
                //if(originY == -1)
                originY = (MappingValue - (double)(props.Location._Y + props.Size.Height )) / dpi;
                Contents.DrawRectangle(originX -0.15 - (double)props.MarginDecrementValue / dpi, originY  + (double)props.YMarginDecrementValue/dpi , originWidth, originHeight, p);
                //Contents.DrawRectangle(originX - 0.12, originY + 0.12, originWidth - 0.13, originHeight - 0.13, p);
            }
            Contents.SaveGraphicsState();
            // change nonstroking (fill) color to purple
            Contents.SetColorNonStroking(Color.Black);
            Contents.RestoreGraphicsState();
        }
        //private void addPdfLabel(ReportControlProperties props)
        //{
        //    PdfFont pdf = PdfFont.CreatePdfFont(Document, props.Font.FontFamily, props.Font.FontStyle, true);

        //    PaintOp p = PaintOp.CloseStroke;

        //    Color color = Color.FromName(props.Font.FontColor);
        //    if (!color.IsKnownColor)
        //    {
        //        color = ColorTranslator.FromHtml("#" + color.Name);
        //    }
        //    if (props.Text != null)
        //        props.Text = Regex.Replace(props.Text, @"\r\n?|\n", " ");

        //    // double xMargin = -14.0;
        //    double xMargin = 0;
        //    //double originX = (double)(props.Location._X + props.MarginDecrementValue);
        //    double originX = (double)(props.Location._X + xMargin);
        //    originX = originX / dpi;
        //    PdfRectangle rect = pdf.TextBoundingBox(props.Font.FontSize, props.Text);

        //    //double originY = (double)(props.Location._Y + props.YMarginDecrementValue);
        //    double originY = MappingValue - (double)(props.Location._Y + props.YMarginDecrementValue);

        //    //if(LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPE_A4 || LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPE_A5)
        //    //  originY = (double)(props.Location._Y + props.YMarginDecrementValue);
        //    originY = originY / dpi;
        //    if (originY < 0)
        //        originY *= -1;
        //    if (props.Border)
        //    {
        //        originX += 0.02;
        //        originY -= 0.02;
        //    }
        //    double originWidth = (double)props.Size.Width;
        //    originWidth = originWidth / dpi;
        //    double originHeight = (double)props.Size.Height;
        //    originHeight = originHeight / dpi;
        //    // double widthVal =   Contents.DrawText(pdf, props.Font.FontSize, originX, originY, TextJustify.Left, 0.0, color, color, props.Text);
        //    double widthVal = Contents.DrawText(pdf, props.Font.FontSize, originX, originY, TextJustify.Left, DrawStyle.Normal, color, props.Text);

        //    //Contents.DrawText(
        //    if (props.Border)
        //    {
        //        originX = (double)props.Location._X;
        //        originX = originX / dpi;
        //        originY = MappingValue - (double)(props.Location._Y + props.Size.Height);

        //        originY = originY / dpi;
        //        if (rect != null)
        //        {
        //            originY += (rect.Top + rect.Height - 0.01);
        //            originWidth = rect.Width + 0.05;
        //            originHeight = rect.Height + 0.05;
        //        }
        //        //originY -= 0.02;
        //        //originX += 0.02;
        //        // if (originY < 0)
        //        //     originY *= -1;


        //        Contents.DrawRectangle(originX, originY, originWidth, originHeight, p);
        //    }
        //    Contents.SaveGraphicsState();
        //    // change nonstroking (fill) color to purple
        //    Contents.SetColorNonStroking(Color.Black);
        //    Contents.RestoreGraphicsState();
        //}
        public void AddPDFBitmap(ReportControlProperties IVLProps)
        {
            PdfImageControl ImageControl = new PdfImageControl();
            ImageControl.Resolution = 300;
            ImageControl.ImageQuality = 80;
            double x = IVLProps.Location._X;
            double y = IVLProps.Location._Y;
            int imgCount = 0;
            double width = IVLProps.Size.Width;
            double height = IVLProps.Size.Height;
            double originX = 0; double originY = 0;
            double originHeight = 0; double originWidth = 0;
            PictureBox pb = new PictureBox();
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Margin = new System.Windows.Forms.Padding(0);
            if (File.Exists(IVLProps.ImageName))
            {

                //pb.Index = ++imgCount;
                pb.Image = new Bitmap(IVLProps.ImageName);
            }
            else if (IVLProps.Binding.Contains("QRCode"))
            {
                pb.Image = _dataModel.qrBitmap;

            }
            if (pb.Image != null)
            {//pb.Image.
                pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                //pb.Location = new Point((int)x, (int)y);
                {
                    pb.Location = new Point((int)x, (int)(MappingValue - y));
                }
                pb.Location = new Point(Convert.ToInt32(pb.Location.X), Convert.ToInt32(pb.Location.Y - height));
                pb.Size = new Size((int)width, (int)height - 20);
                var wfactor = (double)pb.Image.Width / pb.ClientSize.Width;
                var hfactor = (double)pb.Image.Height / pb.ClientSize.Height;
                var resizeFactor = Math.Max(wfactor, hfactor);
                Size zoomedImageSize = new Size((int)(pb.Image.Width / resizeFactor), (int)(pb.Image.Height / resizeFactor));
                Point p = new Point(pb.ClientRectangle.X, pb.ClientRectangle.Y);
                PropertyInfo pInfo = pb.GetType().GetProperty("ImageRectangle", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                Rectangle rectangle = (Rectangle)pInfo.GetValue(pb, null);//rectangle will get the inner location of the 

                originX = (double)(pb.Location.X);
                originX = originX / dpi;
                //double originY = MappingValue - (double)(pb.Location.Y + rectangle.Y);
                originY = (double)(pb.Location.Y + rectangle.Y);
                originY = originY / dpi;
                if (originY < 0)
                    originY *= -1;
                originWidth = ((double)pb.Image.Width / resizeFactor);
                originWidth = originWidth / dpi;
                originHeight = (double)pb.Image.Height / resizeFactor;
                originHeight = originHeight / dpi;
            }

            if (!string.IsNullOrEmpty(IVLProps.ImageName) && File.Exists(IVLProps.ImageName))
            {
                PdfImage Image1 = new PdfImage(Document, IVLProps.ImageName, ImageControl);

                Contents.DrawImage(Image1, originX + (double)IVLProps.MarginDecrementValue / dpi, originY + (double)IVLProps.YMarginDecrementValue / dpi, originWidth, originHeight);
            }
            else if (IVLProps.Binding.Contains("QRCode"))
            {
                if (_dataModel.qrBitmap != null)
                {
                    PdfImage Image1 = new PdfImage(Document, _dataModel.qrBitmap, ImageControl);

                    Contents.DrawImage(Image1, originX + (double)IVLProps.MarginDecrementValue / dpi, originY + (double)IVLProps.YMarginDecrementValue / dpi, originWidth, originHeight);
                }

            }
            if (IVLProps.Border)
            {
                PaintOp paintOp = PaintOp.Stroke;
                Contents.DrawRectangle(originX + (double)IVLProps.MarginDecrementValue / dpi, originY + (double)IVLProps.YMarginDecrementValue / dpi, originWidth, originHeight, paintOp);
            }
        }

        public void AddPDFRectangle(ReportControlProperties IVLProps)
        {
            if (_dataModel.CurrentImgFiles.Length > 0)
            {

                double x = IVLProps.Location._X;
                double y = IVLProps.Location._Y;
                int imgCount = 0;
                double width = IVLProps.Size.Width / IVLProps.RowsColumns._Columns;
                double height = IVLProps.Size.Height / IVLProps.RowsColumns._Rows;
                List<string> imageFilePaths = new List<string>();
                List<string> imageFileNames = new List<string>();
                List<string> maskSettingsList = new List<string>();
                List<string> cameraSettingsList = new List<string>();
                if (IVLProps.Binding.ToLower().Contains("righteyeimages"))
                {
                    //foreach (var item in )
                    for (int j = 0; j < _dataModel.CurrentImageNames.Length; j++)
                    {
                        if (_dataModel.CurrentImageNames[j].Contains("OD"))
                        {
                            imageFileNames.Add(_dataModel.CurrentImageNames[j]);
                            imageFilePaths.Add(_dataModel.CurrentImgFiles[j]);
                            maskSettingsList.Add(_dataModel.CurrentImageMaskSettings[j]);
                            cameraSettingsList.Add(_dataModel.CurrentImageCameraSettings[j]);
                        }
                    }
                }
                else if (IVLProps.Binding.ToLower().Contains("lefteyeimages"))
                {
                    for (int j = 0; j < _dataModel.CurrentImageNames.Length; j++)
                    {
                        if (_dataModel.CurrentImageNames[j].Contains("OS"))
                        {
                            imageFileNames.Add(_dataModel.CurrentImageNames[j]);
                            imageFilePaths.Add(_dataModel.CurrentImgFiles[j]);
                            maskSettingsList.Add(_dataModel.CurrentImageMaskSettings[j]);
                            cameraSettingsList.Add(_dataModel.CurrentImageCameraSettings[j]);
                        }
                    }
                }
                //if (imageFileNames.Count == 0)
                if (!IVLProps.Binding.ToLower().Contains("righteyeimages") && imageFileNames.Count == 0 && !IVLProps.Binding.ToLower().Contains("lefteyeimages"))//this has been added to add images for old templates
                {
                    imageFileNames.AddRange(_dataModel.CurrentImageNames);
                    imageFilePaths.AddRange(_dataModel.CurrentImgFiles);
                    maskSettingsList.AddRange(_dataModel.CurrentImageMaskSettings);
                    cameraSettingsList.AddRange(_dataModel.CurrentImageCameraSettings);
                }
                x = IVLProps.Location._X;

                for (int i = IVLProps.RowsColumns._Rows - 1; i >= 0; i--)
                {
                    for (int j = 0; j < IVLProps.RowsColumns._Columns; j++)
                    {
                        //Code has been changed on 17 april 2017 by darshan to print the images in the exact location as they were in the report form.
                        if (imgCount < imageFileNames.Count)
                        {
                            PdfImageControl ImageControl = new PdfImageControl();
                            ImageControl.Resolution = 300.0;
                            ImageControl.ImageQuality = 80;
                            LeftRightPosition left = LeftRightPosition.Left;

                            //string[] imageDetails = { _dataModel.CurrentImgFiles[imgCount], _dataModel.CurrentImageNames[imgCount] };
                            string[] imageDetails = { imageFilePaths[imgCount], imageFileNames[imgCount] };
                            // PdfImage Image1 = new PdfImage(Document, imageDetails[0], ImageControl);
                            PictureBox pb = new PictureBox();
                            pb.SizeMode = PictureBoxSizeMode.Zoom;
                            pb.Margin = new System.Windows.Forms.Padding(0);
                            //pb.Index = ++imgCount;
                            pb.Image = new Bitmap(imageDetails[0]);
                            //pb.Image.
                            pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                            //pb.Location = new Point((int)x, (int)y);
                            {
                                pb.Location = new Point((int)x, (int)(MappingValue - y));
                            }
                            pb.Location = new Point(Convert.ToInt32(pb.Location.X), Convert.ToInt32(pb.Location.Y - height));
                            pb.Size = new Size((int)width, (int)height - 20);
                            var wfactor = (double)pb.Image.Width / pb.ClientSize.Width;
                            var hfactor = (double)pb.Image.Height / pb.ClientSize.Height;
                            var resizeFactor = Math.Max(wfactor, hfactor);
                            Size zoomedImageSize = new Size((int)(pb.Image.Width / resizeFactor), (int)(pb.Image.Height / resizeFactor));
                            Point p = new Point(pb.ClientRectangle.X, pb.ClientRectangle.Y);
                            PropertyInfo pInfo = pb.GetType().GetProperty("ImageRectangle", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                            Rectangle rectangle = (Rectangle)pInfo.GetValue(pb, null);//rectangle will get the inner location of the 

                            Bitmap bm = new Bitmap(imageDetails[0]);

                            #region check for the older version to apply camera and mask settings
                            string maskSetting = "";
                            string cameraSetting = "";
                            if (_dataModel.ReportData.ContainsKey("$CurrentImagebm"))
                            {
                                bm = _dataModel.ReportData["$CurrentImagebm"] as Bitmap;
                            }

                            if (_dataModel.CurrentImageMaskSettings.Length == _dataModel.CurrentImgFiles.Length)
                                maskSetting = maskSettingsList[imgCount];
                            if (_dataModel.CurrentImageMaskSettings.Length == _dataModel.CurrentImgFiles.Length)
                                cameraSetting = cameraSettingsList[imgCount];
                            if (imageFileNames[imgCount].Contains("OS"))//this has been added to set the image side and to send it to ApplyLogo function
                                left = LeftRightPosition.Left;
                            else
                                left = LeftRightPosition.Right;
                            //ApplyMaskToCapturedImage(ref bm, _dataModel.CurrentImageMaskSettings[imgCount], _dataModel.CurrentImageCameraSettings[imgCount], _dataModel.CurrentImageNames[imgCount]);//images and names of images(CurrentImageNames) selected by user and masksettings properties along with datamodel passed to ApplyMaskToCapturedImage method.By Ashutosh 22-08-2017.CurrentImageCameraSettings added By Ashutosh 31-08-2017.
                            ApplyPostProcessingForPrintImage(ref bm, maskSetting, cameraSetting, imageDetails[1], left);//images and names of images(CurrentImageNames) selected by user and masksettings properties along with datamodel passed to ApplyMaskToCapturedImage method.By Ashutosh 22-08-2017.
                            #endregion

                            double originX = (double)(pb.Location.X + rectangle.X );
                            originX = originX / dpi;
                            //double originY = MappingValue - (double)(pb.Location.Y + rectangle.Y);
                            double originY =  (double)(pb.Location.Y + rectangle.Y);
                            originY = originY / dpi;
                            if (originY < 0)
                                originY *= -1;
                            double originWidth = ((double)pb.Image.Width / resizeFactor);
                            originWidth = originWidth / dpi;
                            double originHeight = (double)pb.Image.Height / resizeFactor;
                            originHeight = originHeight / dpi;

                            addPdfImage(IVLProps, originX, originY, originWidth, originHeight, imageDetails, ImageControl, bm);//argument bm passed to handle change in mask colour for input image/s.By Ashutosh 22-08-2017
                             x += width + 5;
                            imgCount++;
                            bm.Dispose();

                        }
                    }
                    x = IVLProps.Location._X;
                    
                    y += height + 10;
                }
            }
        }

        private void addPdfTextBox(ReportControlProperties props)
        {
            addPdfLabel(props);
            return;
            //PdfFont pdf = PdfFont.CreatePdfFont(Document, props.Font.FontFamily, props.Font.FontStyle, true);
            //PaintOp p = PaintOp.CloseStroke;
            //Color color = Color.FromName(props.Font.FontColor);
            //if (!color.IsKnownColor)
            //{
            //    color = ColorTranslator.FromHtml("#" + color.Name);
            //}
            //if (props.Text != null)
            //    props.Text = Regex.Replace(props.Text, @"\r\n?|\n", " ");

            //double originX = (double)(props.Location._X + props.MarginDecrementValue);
            //originX = originX / dpi;
            //PdfRectangle rect = pdf.TextBoundingBox(props.Font.FontSize, props.Text);

            ////double originY = (double)(props.Location._Y + props.YMarginDecrementValue);
            //double originY = MappingValue - (double)(props.Location._Y + props.YMarginDecrementValue);

            ////if(LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPE_A4 || LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPE_A5)
            ////  originY = (double)(props.Location._Y + props.YMarginDecrementValue);
            //originY = originY / dpi;
            //if (originY < 0)
            //    originY *= -1;
            //if (props.Border)
            //{
            //    originX += 0.02;
            //    originY -= 0.02;
            //}
            //double originWidth = (double)props.Size.Width;
            //originWidth = originWidth / dpi;
            //double originHeight = (double)props.Size.Height;
            //originHeight = originHeight / dpi;
            //Contents.DrawText(pdf, props.Font.FontSize, originX, originY, TextJustify.Left, 0.0, color, color, props.Text);

            //if (props.Border)
            //{
            //    originX = (double)props.Location._X;
            //    originX = originX / dpi;
            //    originY = MappingValue - (double)(props.Location._Y + props.Size.Height);

            //    originY = originY / dpi;
            //    if (rect != null)
            //        originY += rect.Height;
            //    //originY -= 0.02;
            //    //originX += 0.02;
            //    // if (originY < 0)
            //    //     originY *= -1;


            //    Contents.DrawRectangle(originX, originY, originWidth, originHeight, p);
            //}
            //Contents.SaveGraphicsState();
            //// change nonstroking (fill) color to purple
            //Contents.SetColorNonStroking(Color.Black);
            //Contents.RestoreGraphicsState();

            //addPdfLabel(props);
            //return;
            //// save graphics state
            //Contents.SaveGraphicsState();
            //// translate origin to PosX=1.1" and PosY=1.1" this is the bottom left corner of the text box example
            ////Contents.Translate(1.1, 1.1);
            //PdfFont pdf = PdfFont.CreatePdfFont(Document, props.Font.FontFamily, props.Font.FontStyle, true);
            //PaintOp p = PaintOp.Stroke;
            //Color color = Color.FromName(props.Font.FontColor);
            //if (!color.IsKnownColor)
            //{
            //    color = ColorTranslator.FromHtml("#" + color.Name);
            //}
            //// Create text box object width 3.25"
            //// First line indent of 0.25"
            //string result = string.Empty;
            //if (props.Text != null)
            //    result = Regex.Replace(props.Text, @"\r\n?|\n|\t", " ");
            //double boxWidth = (double)props.Size.Width / dpi;
            //PdfFileWriter.TextBox Box = new PdfFileWriter.TextBox(boxWidth);
            //// add text to the text box
            //{
            //    Box.AddText(pdf, props.Font.FontSize, result);//
            //}
            //if (!props.Binding.ToString().Equals("Doctor"))
            //{
            //    double originX = (double)(props.Location._X + props.MarginDecrementValue);
            //    originX = originX / dpi;
            //    double originY = MappingValue - (double)(props.Location._Y - props.Size.Height + props.YMarginDecrementValue);
            //    originY = originY / dpi;
            //    if (originY < 0)
            //        originY *= -1;
            //    double originWidth = (double)props.Size.Width / dpi;
            //    double originHeight = (double)props.Size.Height / dpi;
            //    if (props.Border)
            //        Contents.DrawRectangle(originX, originY, originWidth, originHeight, p);
            //    double ylocation = MappingValue - props.Location._Y;
            //    //Contents.DrawText(originX, ref originY, 0.0, 0, 0.015, 0.05, TextBoxJustify.FitToWidth, Box);

            //    Contents.DrawText(pdf, props.Font.FontSize, originX, originY, TextJustify.Left, 0.0, color, color, result);

            //    //Contents.DrawText(pdf, originX,  originY, 0.0, 0, 0.015, 0.05, TextBoxJustify.FitToWidth, Box);
            //}
            //else
            //{


            //    Contents.DrawText(pdf, props.Font.FontSize, props.Location._X + props.MarginDecrementValue + 2.0, MappingValue - props.Location._Y - props.Size.Height + props.YMarginDecrementValue, TextJustify.Left, 0.0, color, color, result);
            //}
            //// Create text box object width 3.25"
            //// No first line indent
            //// restore graphics state
            //Contents.RestoreGraphicsState();
            //return;
        }

        private void addPdfImage(ReportControlProperties props, double xLoc, double yLoc, double width, double height, string[] ImageDetails, PdfImageControl ImageControl, Image ImageObj)//ImageObj of type Image passed as one of the parameter. by Ashutosh 22-08-2017.
        {
            if (!string.IsNullOrEmpty(ImageDetails[0]))
            {
                // ImageControl.Resolution = 300.0;
                PdfImage Image1 = null;//Image1 of type PdfImage set to null. By Ashutosh 22-08-2017
                if (ImageObj == null)//checks if ImageObj is null.By Ashutosh 22-08-2017
                    Image1 = new PdfImage(Document, ImageDetails[0], ImageControl);//if null then Constructor for image file created.Document (is PDFDocument parent object) , ImageDetails[0](Image file name), and Image control.By Ashutosh 22-08-2017
                else
                    Image1 = new PdfImage(Document, ImageObj, ImageControl);//if not present then ImageObj passed instead of Image file name.By Ashutosh 22-08-2017

                // save graphics state
                Contents.SaveGraphicsState();
                // translate coordinate origin to the center of the picture
                //Contents.Translate(2.6, 5.0);
                // adjust image size an preserve aspect ratio
                // PdfRectangle NewSize = Image1.ImageSizePosition(1.75, 1.5, ContentAlignment.MiddleCenter);
                // clipping path
                //Contents.DrawOval(NewSize.Left, NewSize.Bottom, NewSize.Width, NewSize.Height, PaintOp.ClipPathEor);
                // draw image
                int fontSize = Convert.ToInt32(props.Font.FontSize);
                Contents.DrawImage(Image1, xLoc, yLoc, width, height);
                double xShiftFactor = 20.0 / dpi;
                double yshiftFactor = 2.0 / dpi;
                double xloc_for_imglabel = (xLoc + (width / 2.0)) - xShiftFactor;
                double yloc_for_imglabel = yLoc + height + yshiftFactor;
                //double xloc_for_imglabel = (xLoc + (width / 2.0)) - 20.0;
                //double yloc_for_imglabel = yLoc + height + 2.0;

                //string imageLoc = _dataModel.CurrentImgFiles[indx];
                //string labelName = _dataModel.CurrentImageNames[indx];
                #region To create pdf with Right eye and Left eye instead of OD and OS if it is DR report
                string name = string.Empty;
                if (!props.ImageMedicalName)
                {
                    if (ImageDetails[1].Contains("OS"))
                        name = "Left Eye";
                    else if (ImageDetails[1].Contains("OD"))
                        name = "Right Eye";
                }
                #endregion
                else
                    name = ImageDetails[1];
                //Contents.DrawText(ArialNormal, fontSize, xloc_for_imglabel, yloc_for_imglabel, TextJustify.Left, 0.00, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), name);
                // restore graphics state
                Contents.RestoreGraphicsState();
                Image1.Dispose();
            }
        }

        /// <summary>
        /// This method created to apply mask of particular colour selected by user on captured image. By Ashutosh 22-08-2017
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="maskSettings"></param>
        //private void ApplyMaskToCapturedImage(ref Bitmap bm, string maskSettings, string cameraSettings, string imageName)//imageName parameter handles the name of the images selected. By Ashutosh 29-08-2017.cameraSettings By Ashutosh 31-08-2017.
        private void ApplyPostProcessingForPrintImage(ref Bitmap bm, string maskSettings, string cameraSettings, string imageName, LeftRightPosition left)//imageName parameter handles the name of the images selected. By Ashutosh 29-08-2017.
        {
            PostProcessing printPP = PostProcessing.GetInstance(); //instanation to use ApplyLogo method is postprocessing.By Ashutosh 29-08-2017.
            INTUSOFT.Imaging.CaptureLog data1 = null;
            INTUSOFT.Imaging.ImagingMode mode = ImagingMode.Posterior_Prime;
            MaskSettings data = null;//varaible data of type MaskSettings(in Reports.cs) and is set to null.By Ashutosh 22-08-2017.

            if (!string.IsNullOrEmpty(cameraSettings))
            {
                   try
                        {
                        using (StringReader sw = new StringReader(cameraSettings))
                         {
                            XmlReaderSettings settings = new XmlReaderSettings();
                                   using (XmlReader xmlReader = XmlReader.Create(sw, settings))//sr-from which XML data is read.settings- object used to configure.
                                {
                        
                                        XmlSerializer xmlSer = new XmlSerializer(typeof(INTUSOFT.Imaging.CaptureLog));
                                        data1 = (INTUSOFT.Imaging.CaptureLog)xmlSer.Deserialize(xmlReader);
                                }
                         }  
                     }  
                   catch (Exception)
                        {
                            data1 = (INTUSOFT.Imaging.CaptureLog)Newtonsoft.Json.JsonConvert.DeserializeObject(cameraSettings, typeof(INTUSOFT.Imaging.CaptureLog));// added the type to handle defect 0001885 deserialization of CaptureLog in json
                        }
                    mode = data1.currentCameraType;
               
            }
            else
                if (INTUSOFT.Configuration.ConfigVariables._ivlConfig != null)
                    mode = (ImagingMode)Enum.Parse(typeof(ImagingMode), INTUSOFT.Configuration.ConfigVariables._ivlConfig.Mode.ToString());

            if (INTUSOFT.Configuration.ConfigVariables._ivlConfig != null)
            {
                INTUSOFT.Configuration.AdvanceSettings.PrinterPPSettings ppSettings = INTUSOFT.Configuration.ConfigVariables.CurrentSettings.PrinterPPSettings;

                if (mode == ImagingMode.Posterior_Prime)
                {
                    CCSettings ccSettings = new CCSettings();
                    ccSettings.isApplyColorCorrection = Convert.ToBoolean(ppSettings.CCSettings._IsApplyColorCorrection.val);
                    ccSettings.rrVal = Convert.ToSingle(ppSettings.CCSettings._RRCompensation.val);
                    ccSettings.rgVal = Convert.ToSingle(ppSettings.CCSettings._RGCompensation.val);
                    ccSettings.rbVal = Convert.ToSingle(ppSettings.CCSettings._RBCompensation.val);
                    ccSettings.grVal = Convert.ToSingle(ppSettings.CCSettings._GRCompensation.val);
                    ccSettings.ggVal = Convert.ToSingle(ppSettings.CCSettings._GGCompensation.val);
                    ccSettings.gbVal = Convert.ToSingle(ppSettings.CCSettings._GBCompensation.val);
                    ccSettings.brVal = Convert.ToSingle(ppSettings.CCSettings._BRCompensation.val);
                    ccSettings.bgVal = Convert.ToSingle(ppSettings.CCSettings._BGCompensation.val);
                    ccSettings.bbVal = Convert.ToSingle(ppSettings.CCSettings._BBCompensation.val);
                    if (ccSettings.isApplyColorCorrection)
                        printPP.Applycolorcorrection(ref bm, ccSettings, true);
                    int channelCode = 0;
                    if(Convert.ToBoolean(ppSettings.LUTSettings._IsApplyLutSettings.val))
                        channelCode = 4;

                    if (Convert.ToBoolean(ppSettings.LUTSettings._IsApplyLutSettings.val))
                    {
                        PostProcessing.ImageProc_CalculateLut(Convert.ToDouble(ppSettings.LUTSettings._LUTSineFactor.val), Convert.ToDouble(ppSettings.LUTSettings._LUTInterval1.val), Convert.ToDouble(ppSettings.LUTSettings._LUTInterval2.val), 8, false, Convert.ToInt32(ppSettings.LUTSettings._LUTOffset.val), Convert.ToBoolean(ppSettings.LUTSettings._IsChannelWiseLUT.val),channelCode);
                        printPP.ApplyLut(ref bm, true, true);
                    }
                }
                #region Get mask settings from the dictionary deserialize to mask settings class
                if (!string.IsNullOrEmpty(maskSettings))
                {
                    try
                    {
                        using (StringReader sr = new StringReader(maskSettings))//StringReader-Initializes a new instance of the System.IO.StringReader class that reads from the specified string(maskSettings).
                        {
                            XmlReaderSettings settings = new XmlReaderSettings();
                            using (XmlReader xmlReader = XmlReader.Create(sr, settings))//sr-from which XML data is read.settings- object used to configure.
                            {
                                XmlSerializer xmlSer = new XmlSerializer(typeof(MaskSettings));
                                data = (MaskSettings)xmlSer.Deserialize(xmlReader);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        data = (MaskSettings)Newtonsoft.Json.JsonConvert.DeserializeObject(maskSettings, typeof(MaskSettings));//Json Deserialization  operation // added the type to handle defect 0001885 deserialization of masksettings in json
                    }
                 
                }
                else
                {
                    if (INTUSOFT.Configuration.ConfigVariables._ivlConfig != null)
                    {
                        data = new MaskSettings();
                        data.maskHeight = Convert.ToInt32(INTUSOFT.Configuration.ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val);
                        data.maskWidth = Convert.ToInt32(INTUSOFT.Configuration.ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val);
                        data.maskCentreX = Convert.ToInt32(INTUSOFT.Configuration.ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX.val);
                        data.maskCentreY = Convert.ToInt32(INTUSOFT.Configuration.ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY.val);
                    }
                }


                #endregion

            }
            if (data != null)
            {
                Bitmap tempBm = new Bitmap(bm.Width, bm.Height, bm.PixelFormat);//tempBm object of type bitmap.By Ashutosh 22-08-2017
                Bitmap maskbm = new Bitmap(bm.Width, bm.Height, bm.PixelFormat);//maskbm object of type bitmap.By Ashutosh 22-08-2017
                Graphics g = Graphics.FromImage(tempBm);//Creates a new System.Drawing.Graphics(g) from the specified System.Drawing.Image(tempBm).By Ashutosh 22-08-2017.
                Graphics g1 = Graphics.FromImage(maskbm);//Creates a new System.Drawing.Graphics(g1) from the specified System.Drawing.Image(maskbm).By Ashutosh 22-08-2017
                if (!_dataModel.ReportData.ContainsKey("$ChangeMaskColour"))
                    _dataModel.ReportData.Add("$ChangeMaskColour", INTUSOFT.Configuration.ConfigVariables.CurrentSettings.ReportSettings.ChangeMaskColour.val);
                Color maskBgColor = Color.FromKnownColor((KnownColor)Enum.Parse(typeof(KnownColor), _dataModel.ReportData["$ChangeMaskColour"] as string));//colour chosen by user in string form converted to enum object and given to maskBgColor.By Ashutosh 22-08-2017
                SolidBrush s = new SolidBrush(maskBgColor);//s object of type Solidbrush , color of the brush is users choice.By Ashutosh 22-08-2017

                g.FillRectangle(s, new Rectangle(0, 0, bm.Width, bm.Height));// Fill the output image with the color chosen in the report settings for background.By Ashutosh 22-08-2017
                g1.FillEllipse(Brushes.White, new Rectangle(data.maskCentreX - data.maskWidth / 2, data.maskCentreY - data.maskHeight / 2, data.maskWidth, data.maskHeight));//Fills the interior of an ellipse (white colour) defined by a bounding rectangle.By Ashutosh 22-08-2017

                //IVLVariables.ivl_Camera.camPropsHelper.PostProcessing.ApplyLogo(ref OriginalBm);

                Image<Gray, byte> maskImg = new Image<Gray, byte>(maskbm);//maskImg object to which maskbm is given.By Ashutosh 22-08-2017
                Image<Bgr, byte> returnImg = new Image<Bgr, byte>(tempBm);//returnImg object to which tempBm is given. tempBm.By Ashutosh 22-08-2017
                Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);//input image bm given to object inp.By Ashutosh 22-08-2017
                inp.Copy(returnImg, maskImg);//returnImg is the destination.mask is applied on the input image to provide desired result.By Ashutosh 22-08-2017
                bm = returnImg.ToBitmap();//masked image given to bm.By Ashutosh 22-08-2017


                printPP.ApplyLogo(ref bm, imageName, maskBgColor, left);//passing arguments to ApplyLogo for application of logo suitable to mask colour. By Ashutosh 29-08-2017.

                if (mode == ImagingMode.FFA_Plus || mode == ImagingMode.FFAColor)
                {
                    if (data1 != null)
                    {
                        if (maskBgColor == Color.Black)
                            maskBgColor = Color.LimeGreen;
                        else
                            maskBgColor = Color.Black;
                        printPP.ApplyTimeStamp(ref bm, data1.ImageTime, maskBgColor);
                    }
                }
                returnImg.Dispose();//disposes returnImg.By Ashutosh 22-08-2017
                inp.Dispose();// disposes inp.
                maskImg.Dispose();// disposes maskImg.
                tempBm.Dispose();// disposes tempBm.
                maskbm.Dispose();// disposes maskbm.
            }
        }
        /// <summary>
        /// This method is added to generate the report pdf.
        /// </summary>
        /// <param name="FileName"></param>
        public string GenaratePdf( List<ReportControlsStructure>[] reportControlStructList, string ReportFileName)
        {
          string reportFileName = "report_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".pdf";
          string ReportName = reportFileName;
            string ReportPath = string.Empty;
            if (!string.IsNullOrEmpty(_dataModel.appDir))
                ReportPath = Path.Combine(new string[] { _dataModel.appDir, "Reports" });
            else
                ReportPath = "Reports";

            //dpi = 96;
            //A4LandscapeHeight = (int)(dpi * a4Height);
            //A4LanscapeWidth = (int)(dpi * a4Width);

            //A4PortraitHeight = (int)(dpi * a4Width);
            //A4PortraitWidth = (int)(dpi * a4Height);

            //A5LandscapeHeight = (int)(dpi * a5Height);
            //A5LanscapeWidth = (int)(dpi * a5Width);

            //A5PortraitHeight = (int)(dpi * a5Width);
            //A5PortraitWidth = (int)(dpi * a5Height);

            DirectoryInfo dirinf = new DirectoryInfo(ReportPath);
            if (!dirinf.Exists)
                dirinf = Directory.CreateDirectory(ReportPath);

           dirinf =new DirectoryInfo(dirinf.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd"));//added to create a date folder for saving reports by kishore on 12 september 2017.
           if (!dirinf.Exists)
               dirinf.Create();
           string reportMainDirPath = dirinf.FullName;
           string FundusReportDirPath = reportMainDirPath + Path.DirectorySeparatorChar + "General-Reports";
           string CDR_ReportDirPath = reportMainDirPath + Path.DirectorySeparatorChar + "CDR-Reports";
           string AnnotationReportDirPath = reportMainDirPath + Path.DirectorySeparatorChar + "Annotation-Reports";
           dirinf = new DirectoryInfo(FundusReportDirPath);//added to create the folder Fundus reoports.
           if (!dirinf.Exists)
               dirinf.Create();
           dirinf = new DirectoryInfo(CDR_ReportDirPath);//added to create the folder CDR reports.
           if (!dirinf.Exists)
               dirinf.Create();
           dirinf = new DirectoryInfo(AnnotationReportDirPath);//added to create a folder Annotation reports.
           if (!dirinf.Exists)
               dirinf.Create();
           if (!_dataModel.isFromCDR && !_dataModel.isFromAnnotation)
           {
               ReportName = reportFileName;
               ReportPath = FundusReportDirPath + Path.DirectorySeparatorChar;
               ReportFileName =  ReportPath+ ReportName;
           }
           else
           {
               if (_dataModel.isFromAnnotation)
               {
                   ReportName = "Annotation_" + reportFileName;
                   ReportPath = AnnotationReportDirPath + Path.DirectorySeparatorChar;
                   ReportFileName =ReportPath+ ReportName;
               }

               else
               {
                   ReportName = "CDR_" + reportFileName;
                   ReportPath = CDR_ReportDirPath + Path.DirectorySeparatorChar;
                   ReportFileName = ReportPath  + ReportName;
               }


           }
            if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPE_A4 || LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPEDR_A4)
            {
                Document = new PdfDocument(PaperType.A4, true, UnitOfMeasure.Inch, ReportFileName);
                //Document = new PdfDocument(PaperType.A4, true, UnitOfMeasure.Inch, ReportFileName);
                MappingValue = a4Height * dpi;// A4LandscapeHeight;
               // MappingValue = A4LandscapeHeight ;
            }
            else if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPE_A5 || LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPEDR_A5)
            {
                Document = new PdfDocument(PaperType.A5, true, UnitOfMeasure.Inch, ReportFileName);
               // MappingValue = A5LandscapeHeight;
                MappingValue = a5Height * dpi;// A4LandscapeHeight;

            }
            else if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT_A4 || LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAITDR_A4)
            {
                Document = new PdfDocument(PaperType.A4, false, UnitOfMeasure.Inch, ReportFileName);
                //Document = new Document(iTextSharp.text. PageSize.A4, 0, 0, 0, 0);
                
                //pdfWriter = PdfWriter.GetInstance(Document, fs);
                
                //Document.Open();
                //MappingValue = A4PortraitHeight;
                MappingValue = a4Width*dpi;
            }
            else if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT_A5 || LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAITDR_A5)
            {
                Document = new PdfDocument(PaperType.A5, false, UnitOfMeasure.Inch, ReportFileName);
                //MappingValue = A5PortraitHeight;
                MappingValue = a5Width * dpi;
            
            }
            MappingValue = Math.Round(MappingValue, MidpointRounding.AwayFromZero);

            DefineFontResources();
            foreach (var item1 in reportControlStructList)
            {
                Page = new PdfPage(Document);
                // Step 4:Add contents to page
                Contents = new PdfContents(Page);

                foreach (ReportControlsStructure item in item1)
                {
                    if (item.reportControlProperty.Name.Contains("Label"))
                    {
                        addPdfLabel(item.reportControlProperty);
                    }
                    else
                        if (item.reportControlProperty.Name.Contains("TextBox"))
                    {
                        addPdfTextBox(item.reportControlProperty);
                    }
                    else
                            if (item.reportControlProperty.Name.Contains("PictureBox"))
                    {
                        AddPDFBitmap(item.reportControlProperty);
                    }
                    else
                                if (item.reportControlProperty.Name.Contains("IVL_ImagePanel"))
                    {
                        AddPDFRectangle(item.reportControlProperty);
                    }
                    else
                                    if (item.reportControlProperty.Name.Contains("Table"))
                    {
                        AddPDFTable(item.reportControlProperty);
                    }
                }
            }
           
            //Document.Close();
            //fs.Close();
            //fs.Dispose();
                Document.CreateFile();

            CustomFolderBrowser.fileName = ReportName;//to get the file name while exporting.
            CustomFolderBrowser.filePath = ReportPath;//to get the folder path while exporting.
            return ReportName;
        }
    }
}

