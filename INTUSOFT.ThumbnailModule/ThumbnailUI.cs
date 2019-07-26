using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using INTUSOFT.EventHandler;
//using INTUSOFT.Data.Repository;
//using INTUSOFT.Data.Model;
using System.IO;
using Common;
//using INTUSOFT.Desktop.Properties;
namespace INTUSOFT.ThumbnailModule
{
    public partial class ThumbnailUI : UserControl
    {

        private ThumbnailController m_Controller;
        private ImageViewer m_ActiveImageViewer;
        private bool isThumbnailAddEvent = false;
        //    public ImageRepository imageRepo;
        public delegate void ShowImageFromThumbnail(ThumbnailData s, EventArgs e);
        public event ShowImageFromThumbnail showImgFromThumbnail;
        public delegate void SendFocusBackToParent();
        public event SendFocusBackToParent sendFocusBackToParent;
        public delegate void ThumbnailImgSelected(KeyValuePair<string, string> ThumbnailFileName, EventArgs e);
        public event ThumbnailImgSelected thumbnailImgSelected;
        //public delegate void AddThumbnail(Dictionary<string, object> s);
        //public event AddThumbnail AddThumbnailEvent;
        public delegate void ImageAdded(Dictionary<string, object> s);
        public event ImageAdded imageadded;
        public delegate void ImageThumbnailAdded(ThumbnailData thumbnailData);
        public event ImageThumbnailAdded imageaddedThumbnailData;
        public delegate void AnotherWindowOpen(bool isOpen);
        public event AnotherWindowOpen anotherWindowOpen;
        public bool upDownArrow = false;
        public delegate void DeleteImages(Dictionary<string, object> imagname);
        public event DeleteImages deleteimgs;
        public List<int> corrupted_images;
        public bool isShiftKeyPressed = false;
        public bool isControlKeyPressed = false;
        public bool isFirstThumbnail_Selected = false;
        public string DeleteMsg = "";
        public string DeleteSingleImageMsg = "";
        public string DeletedImageMsg = "";
        public string DeletedImageHeader = "";
        public bool isannotated = false;
        public bool isCDR = false;
        public bool isCaptureSequenceInProgress = false;
        public string thumbnailString = "Image";
        public int sizeChanged = 1;
        //Code added by darshan to resolve the issue 0000657: Reports no limit is set,No images are shown.
        public string NoOfImagesToBeSelectedText1 = string.Empty;
        public string NoOfImagesToBeSelectedText2 = string.Empty;
        public string NoOfImagesToBeSelectedHeader = string.Empty;
        public int NoOfImagesToBeSelected = 0;
        public int id = 0;
        public int side = 0;
        public static bool isValueChanged = false;
        public event ThumbnailImageEventHandler OnImageSizeChanged;
        List<string> image_names;

        public void SizeChanged()
        {
            if (this.OnImageSizeChanged != null)//This condition check has been added to see that OnImageSizeChanged is initialized or not.
            this.OnImageSizeChanged(this, new ThumbnailImageEventArgs(ImageSize));
        }

        public ThumbnailUI()
        {
            InitializeComponent();
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;
            this.thumbnail_FLP.Padding = new Padding(0, 0, vertScrollWidth, 0);
            this.thumbnail_FLP.AutoScroll = false;
            this.AutoScroll = true;
            corrupted_images = new List<int>();
            //    imageRepo = new ImageRepository();
            //_eventHandler = IVLEventHandler.getInstance();
            //_eventHandler.Register(__eventHandler.ThumbnailAdd, new NotificationHandler(AddThumbnailEvent));
            //_eventHandler.Register(_eventHandler.GetImageFiles,new NotificationHandler(getImageFiles));
            //_eventHandler.Register(_eventHandler.ThumbnailSelected, new NotificationHandler(ThumbnailSelected));
            //_eventHandler.Register(_eventHandler.ChangeThumbnailSide, new NotificationHandler(changeThumNailSide));
            m_AddImageDelegate = new DelegateAddImage(this.AddImage);
            this.VisibleChanged += ThumbnailUI_VisibleChanged;
            m_Controller = new ThumbnailController();
            m_Controller.OnAdd += new ThumbnailControllerEventHandler(m_Controller_OnAdd);
            m_Controller.corruptedImages += m_Controller_corruptedImages;
            image_names = new List<string>();
        }
        public void verticalSroll(ImageViewer img)
        {

                int index = this.thumbnail_FLP.Controls.GetChildIndex(img);

                this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, img.Height * index);
        }
        

        //This below event has been added by Darshan on 18-09-2015 to check image is a valid image or not.
        void m_Controller_corruptedImages(List<int> ids)
        {
            foreach (int item in ids)
            {
                corrupted_images.Add(item);
            }
        }
        public void ResetThumbnailUI()
        {
            this.thumbnail_FLP.Controls.Clear();
            this.thumbnail_FLP.selectedThumbnails.Clear();
            //This below code is added by Darshan in order solve defect nos 0000329 and 0000322
            this.thumbnail_FLP.SelectedThumbnailFileNames.Clear();
            this.thumbnail_FLP.TotalThumbnails = 0;
        }
        public void ChangeThumbnailSide(int id, int side, bool isannotated, bool isCDR,string fileName)
        {
            //isannotated has been added by Darshan to solve defect no 0000527: Annotated images displaying wrong names.
            foreach (Control item in thumbnail_FLP.Controls)
            {
                if (item is ImageViewer)
                {
                    ImageViewer imgView = item as ImageViewer;
                    if (imgView.ImageID == id)
                    {
                        imgView.ImageSide = side;
                        this.side = side;
                        this.isannotated = isannotated;
                        this.isCDR = isCDR;
                        imgView.IsAnnotated = isannotated;
                        imgView.IsCDR = isCDR;
                        imgView.label1.Text = GetImage_Name(imgView.ImageSide, imgView.Index);
                        imgView.label1.Font = new Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
                        if (!string.IsNullOrEmpty(fileName))
                            imgView.ImageLocation = fileName;
                        imgView.LoadImage(imgView.ImageLocation, id, 256, 256);//Update the side changed image in thumbnail when image side is changed,added by Darshan(Defect no 0000702).
                        imgView.Refresh();
                    }
                }
            }
        }
        public void AddThumbnails(List<string> FileNames, List<int> ids, List<int> sides, List<bool> isannotated, List<bool> isCDR)
        {
            this.thumbnail_FLP.Controls.Clear();
            m_Controller.CreateThumbnails(FileNames, ids, sides, isannotated, isCDR);
        }
        public void AddThumbnails(List <ThumbnailData> ThumbnailList)
        {
            this.thumbnail_FLP.Controls.Clear();
            m_Controller.CreateThumbnails(ThumbnailList);
        }

        public Dictionary<string, List<string>> getImageFiles()
        {
            List<string> fileNames = new List<string>();
            List<string> ImageNames = new List<string>();
            ControlCollection items = thumbnail_FLP.Controls;
            //this code has been added to arrange the the selected thumbnails in the order in which we selected
            for (int i = 0; i < thumbnail_FLP.selectedThumbnails.Count; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    if (items[j] is ImageViewer)
                    {
                        ImageViewer imgView = items[j] as ImageViewer;
                        if (imgView.Index == thumbnail_FLP.selectedThumbnails[i])
                        {
                            fileNames.Add(imgView.ImageLocation);
                            ImageNames.Add(imgView.label1.Text);
                        }
                    }
                }
            }
            //foreach (Control item in thumbnail_FLP.Controls)

            //{
            //    if (item is ImageViewer)
            //    {
            //        ImageViewer imgView = item as ImageViewer;
            //        if (imgView.IsActive)
            //        {
            //            fileNames.Add(imgView.ImageLocation);
            //            ImageNames.Add(imgView.label1.Text);
            //        }
            //    }
            //}
            Dictionary<string, List<string>> retValFileNames = new Dictionary<string, List<string>>();
            retValFileNames.Add("FileNames", fileNames);
            retValFileNames.Add("ImageNames", ImageNames);
            return retValFileNames;
        }
        public void AddThumbnailEvent(Dictionary<string, object> val)
        {
            isThumbnailAddEvent = true;
            AddImage(val["ImageName"] as string, (int)val["id"], -1, (int)val["side"], (bool)val["isannotated"], (bool)val["isCDR"]);
            imageadded(val);
            ImageViewer img = new ImageViewer();
            img.ImageLocation = val["ImageName"] as string;
            if (!(bool)val.ContainsKey("isModifiedimage"))
                ThumbnailSelected(img.ImageLocation);
            //  thumbnailSelection(img);
        }
        public void AddThumbnailEvent(ThumbnailData thumbnailData)
        {
            isThumbnailAddEvent = true;
            AddImage(thumbnailData.fileName, thumbnailData.id, -1, thumbnailData.side, thumbnailData.isAnnotated, thumbnailData.isCDR);
           // imageadded(val);
            imageaddedThumbnailData(thumbnailData);
            ImageViewer img = new ImageViewer();
            img.ImageLocation = thumbnailData.fileName;
            if (!thumbnailData.isModified)
                ThumbnailSelected(img.ImageLocation);
            //  thumbnailSelection(img);
        }
        public void ThumbnailSelected(String val)
        {
            if (File.Exists(val))//This if statement has been added by sriram sir on 06-08-2015
            {
                foreach (Control item in this.thumbnail_FLP.Controls)
                {
                    if (item is ImageViewer)
                    {
                        ImageViewer img = item as ImageViewer;
                        if (img.ImageLocation == (string)val)//;["ThumbnailFileName"])
                        {
                            if (!image_names.Contains(img.ImageLocation))
                                image_names.Add(img.ImageLocation);
                            if (File.Exists(img.ImageLocation))
                                thumbnailSelection(img);
                            break;
                        }
                    }
                }
            }
            else
                NoImages_Selected();
        }

        public void ThumbnailSelected(ThumbnailData tdata)
        {
            if (File.Exists(tdata.fileName))//This if statement has been added by sriram sir on 06-08-2015
            {
                foreach (Control item in this.thumbnail_FLP.Controls)
                {
                    if (item is ImageViewer)
                    {
                        ImageViewer img = item as ImageViewer;
                        if (img.ImageLocation == (string)tdata.fileName)//;["ThumbnailFileName"])
                        {
                            if (!image_names.Contains(img.ImageLocation))
                                image_names.Add(img.ImageLocation);
                            if (File.Exists(img.ImageLocation))
                                thumbnailSelection(img);

                            break;
                        }
                    }
                }
            }
            else
                NoImages_Selected();
        }

        public void thumbnailDelete()
        {
            if (thumbnail_FLP.selectedThumbnails.Count != 0)
            {
                DialogResult dia;
                if (thumbnail_FLP.selectedThumbnails.Count == 1)
                {
                    anotherWindowOpen(true);
                    dia = CustomMessageBox.Show(this.DeleteSingleImageMsg, "Warning", CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Warning);
                    
                }
                else
                {
                    anotherWindowOpen(true);
                    dia = CustomMessageBox.Show(this.DeleteMsg, "Warning", CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Warning);
                    
                }
                if (dia == DialogResult.Yes)
                {
                    removeImageViewer();
                    NoImages_Selected();
                    anotherWindowOpen(false);
                }
                else
                {
                    anotherWindowOpen(false);
                    return;
                }
            }
        }

        public void NoImages_Selected()
        {
            if (this.thumbnail_FLP.SelectedThumbnailFileNames.Count == 0)
            {
                ThumbnailData data = new ThumbnailData();
                displayThumbnailImage(data);
            }
        }
        //private void changeThumNailSide(string s, Args arg)
        //{
        //    foreach (Control item in thumbnail_FLP.Controls)
        //    {
        //        if (item is ImageViewer)
        //        {
        //            ImageViewer imgView = item as ImageViewer;
        //            if (imgView.ImageID == (int)arg["id"])
        //            {
        //                imgView.ImageSide =(int) arg["side"];
        //            }
        //        }
        //    }
        //}
        void ThumbnailUI_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                this.thumbnail_FLP.Controls.Clear();
                this.thumbnail_FLP.selectedThumbnails.Clear();
            }
        }
        private void m_Controller_OnAdd(object sender, ThumbnailControllerEventArgs e)
        {
            this.AddImage(e.ImageFilename, e.id, e.index, e.side, e.IsAnnotated, e.isCDR);
            this.Invalidate();
        }
        private void m_Controller_OnRemove(object sender, ThumbnailControllerEventHandler e)
        {
        }
        private void m_Controller_OnStart(object sender, ThumbnailControllerEventArgs e)
        {

        }
        private void m_Controller_OnEnd(object sender, ThumbnailControllerEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ThumbnailControllerEventHandler(m_Controller_OnEnd), sender, e);
            }
        }
        private int ImageSize
        {
            get
            {
                return (64 * (sizeChanged + 1));
            }
        }

        delegate void DelegateAddImage(string imageFilename, int id, int index, int side, bool isannotated, bool isCDR);
        private DelegateAddImage m_AddImageDelegate;
        delegate void DelegateRemoveImage(string str, int id);

        //bool val = false;
        private string GetImage_Name(int side, int index)
        {
            string imageName = "";

            if (side == 0)
            {
                string val = string.Format("{0}{1}", "0", index);

                if (isannotated && isCDR)
                {
                    imageName = "OD" + "C" + "  " + val;

                }
                else if (isannotated)
                {
                    imageName = "OD" + "A" + "  " + val;
                }
                else if (isCDR)
                {
                    imageName = "OS" + "C" + "  " + val;
                }
                else
                {
                    imageName = "OD" + "  " + val;
                }
                //imageViewer.label1.Font = new Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            }
            else
                if (side == 1)
                {
                    string val = string.Format("{0}{1}", "0", index);
                    if (isannotated && isCDR)
                    {
                        imageName = "OS" + "C" + "  " + val;
                    }
                    else if (isannotated)
                    {
                        imageName = "OS" + "A" + "  " + val;
                    }
                    else if (isCDR)
                    {
                        imageName = "OS" + "C" + "  " + val;
                    }
                    else
                        imageName = "OS" + "  " + val;
                    //imageViewer.label1.Font = new Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
                }
            return imageName;
        }

        private void AddImage(string imageFilename, int id, int indx, int side, bool isannotated, bool isCDR)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(m_AddImageDelegate, imageFilename, id, indx, side, isannotated, isCDR);
                //this.BeginInvoke(m_AddImageDelegate, imageFilename, id, indx, side, isannotated, isCDR);
            }
            else
            {
                //int size = 192;
                ImageViewer imageViewer = new ImageViewer();
                imageViewer.Dock = DockStyle.Bottom;
                imageViewer.LoadImage(imageFilename, id, 256, 256);
                if (Screen.PrimaryScreen.Bounds.Width == 1366)
                {
                    imageViewer.Width = 118;
                    imageViewer.Height = 128;
                }
                else
                    if (Screen.PrimaryScreen.Bounds.Width == 1280)
                    {
                        imageViewer.Width = 118;
                        imageViewer.Height = 128;
                    }
                    else
                    {
                        imageViewer.Width = 192;
                        imageViewer.Height = 192;
                    }
                //This below code has been added by darshan in order to solve defect no:0000530
                this.thumbnail_FLP.AutoScrollOffset = new Point(0, imageViewer.Height);
                imageViewer.IsAnnotated = isannotated;
                imageViewer.IsCDR = isCDR;
                imageViewer.IsThumbnail = true;
                imageViewer.MouseClick += new MouseEventHandler(imageViewer_MouseClick);
                // imageViewer.textBox1.Click += textBox1_Click;
                imageViewer.label1.Click += label1_Click;
                imageViewer.ImageLocation = imageFilename;
                imageViewer.ImageSide = side;
                string format_string;
                //if (this.thumbnail_FLP.TotalThumbnails == 0)
                //{
                //    this.thumbnail_FLP.TotalThumbnails = 1;
                //}
                if (indx == -1)
                {
                    imageViewer.Index = this.thumbnail_FLP.TotalThumbnails;//this.thumbnail_FLP.TotalThumbnails;
                    indx = ++imageViewer.Index;
                    //imageViewer.Index = indx + 1;
                }
                else
                    imageViewer.Index = indx + 1;
                this.isannotated = isannotated;
                this.isCDR = isCDR;
                imageViewer.label1.Text = GetImage_Name(imageViewer.ImageSide, imageViewer.Index);
                imageViewer.label1.Font = new Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
                //if (indx == -1)
                //{
                //    imageViewer.Index = this.thumbnail_FLP.TotalThumbnails;//this.thumbnail_FLP.TotalThumbnails;
                //    indx = imageViewer.Index;
                //   // imageViewer.Index = indx + 1;

                //}
                //else
                //    imageViewer.Index = indx + 1;
                //// imageViewer.textBox1.Text = "\t\t" + this.thumbnailString + "\t\t" + imageViewer.Index.ToString();
                //imageViewer.label1.Text = this.thumbnailString + "\t\t" + imageViewer.Index.ToString();
                // imageViewer.label1.Enabled = false;
                this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);
                //imageViewer.checkBox1.CheckedChanged += checkBox1_CheckedChanged;
                this.thumbnail_FLP.Controls.Add(imageViewer);
                this.thumbnail_FLP.Controls.SetChildIndex(imageViewer, 0);
                this.thumbnail_FLP.TotalThumbnails++;
                //if (this.isFirstThumbnail_Selected)
                //{
                //    selectThumbnail(imageViewer);
                //    this.thumbnail_FLP.Controls.SetChildIndex(imageViewer, 0);
                //    isThumbnailAddEvent = false;
                //}
            }
        }

        void label1_Click(object sender, EventArgs e)
        {
            ImageViewer img = (sender as Label).Parent as ImageViewer;
            if(File.Exists(img.ImageLocation))
            thumbnailSelection(img);
        }
        //void textBox1_Click(object sender, EventArgs e)
        //{
        //    ImageViewer img = (sender as TextBox).Parent as ImageViewer;
        //    thumbnailSelection(img);
        //}
        private void displayThumbnailImage(ThumbnailData val)
        {
            EventArgs e = new EventArgs();
            if (showImgFromThumbnail != null)
                showImgFromThumbnail(val, e);
        }

        private void ControlKeyThumbNailSelection(ImageViewer sender)
        {
            m_ActiveImageViewer = (ImageViewer)sender;
            if (this.thumbnail_FLP.selectedThumbnails.Contains(m_ActiveImageViewer.Index))
            {
                m_ActiveImageViewer.IsActive = false;
                int imagename_index;
                //if (image_names.Last() == m_ActiveImageViewer.ImageLocation && image_names.First() == m_ActiveImageViewer.ImageLocation)
                //{
                imagename_index = image_names.IndexOf(m_ActiveImageViewer.ImageLocation);
                //}
                //else
                //    if (image_names.Last() == m_ActiveImageViewer.ImageLocation)
                //    {
                //        imagename_index = image_names.IndexOf(m_ActiveImageViewer.ImageLocation);
                //        imagename_index--;
                //    }
                //    else
                //    {
                //        imagename_index = image_names.IndexOf(m_ActiveImageViewer.ImageLocation);
                //    }
                image_names.Remove(m_ActiveImageViewer.ImageLocation);
                this.thumbnail_FLP.selectedThumbnails.Remove(m_ActiveImageViewer.Index);
                this.thumbnail_FLP.SelectedThumbnailFileNames.Remove(m_ActiveImageViewer.ImageLocation);
                Dictionary<string, object> thumbNailDic = new Dictionary<string, object>();
                if (image_names.Count != 0)
                {
                    //image_names.Reverse();
                    foreach (Control item in this.thumbnail_FLP.Controls)
                    {
                        if (item is ImageViewer)
                        {
                            ImageViewer imgViewer = item as ImageViewer;
                            if (imgViewer.ImageLocation == image_names[image_names.Count - 1])
                            {
                                ThumbnailData data = new ThumbnailData();
                                data.fileName = imgViewer.ImageLocation;
                                data.id = imgViewer.ImageID;
                                data.side = imgViewer.ImageSide;
                                data.Name = imgViewer.ImageName;
                                displayThumbnailImage(data);
                            }
                        }
                    }
                }
                else
                    NoImages_Selected();
            }
            else
            {
                //if (this.thumbnail_FLP.SelectedThumbnailFileNames.Count < NoOfImagesToBeSelected)
                {
                    m_ActiveImageViewer.IsActive = true;
                    //this.thumbnail_FLP.AutoScrollPosition = m_ActiveImageViewer.Location; 
                    {
                        if (!image_names.Contains(m_ActiveImageViewer.ImageLocation))//This condition has been added to prevent the unnecessary addition same image into image_names when the image is already present.
                            image_names.Add(m_ActiveImageViewer.ImageLocation);
                        ThumbnailData data = new ThumbnailData();
                        data.fileName = m_ActiveImageViewer.ImageLocation;
                        data.id = m_ActiveImageViewer.ImageID;
                        data.side = m_ActiveImageViewer.ImageSide;
                        data.Name = m_ActiveImageViewer.ImageName;
                        displayThumbnailImage(data);
                    }
                    if (!this.thumbnail_FLP.selectedThumbnails.Contains(m_ActiveImageViewer.Index))
                    this.thumbnail_FLP.selectedThumbnails.Add(m_ActiveImageViewer.Index);

                    if (!this.thumbnail_FLP.SelectedThumbnailFileNames.Contains(m_ActiveImageViewer.ImageLocation))//checks if SelectedThumbnailFileNames contains ImageLocation, if it doesn't contain , then adds the ImageLocation.By Ashutosh 06-09-2017.
                    {
                        this.thumbnail_FLP.SelectedThumbnailFileNames.Add(m_ActiveImageViewer.ImageLocation);
                    }
                ////else
                //    //Added by darshan to resolve the issue 0000657: Reports no limit is set,No images are shown Note no (0002575).
                //    CustomMessageBox.Show(NoOfImagesToBeSelectedText1 + " " + NoOfImagesToBeSelected.ToString() + " " + NoOfImagesToBeSelectedText2, NoOfImagesToBeSelectedHeader, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);
                }
            }
        }

        private void ShiftKeyThumbNailSelection(ImageViewer sender)
        {
            m_ActiveImageViewer = (ImageViewer)sender;
            int startIndx = 0;
            int endIndx = 0;
            if (this.thumbnail_FLP.selectedThumbnails.Count == 0)
            {
                Thumbnail_noshiftcntrlSelection(sender);
            }
            else
            {
                if (m_ActiveImageViewer.Index > this.thumbnail_FLP.selectedThumbnails[0])
                {
                    startIndx = this.thumbnail_FLP.selectedThumbnails[0];
                    endIndx = m_ActiveImageViewer.Index;
                }
                else
                {
                    endIndx = this.thumbnail_FLP.selectedThumbnails[0];
                    startIndx = m_ActiveImageViewer.Index;
                }
            }
            //for (int i = startIndx; i <= endIndx; i++)
            //{
            //Added by darshan to resolve the issue 0000657: Reports no limit is set,No images are shown.
            uint noOfImagesSelected = (uint)(endIndx - startIndx);
            //if ((noOfImagesSelected + 1) <= NoOfImagesToBeSelected && this.thumbnail_FLP.selectedThumbnails.Count < NoOfImagesToBeSelected)// && (this.thumbnail_FLP.selectedThumbnails.Count + noOfImagesSelected) <= NoOfImagesToBeSelected)
            {
                for (int i = endIndx; i >= startIndx; --i)
                {
                    foreach (Control item in this.thumbnail_FLP.Controls)
                    {
                        if (item is ImageViewer)
                        {
                            ImageViewer tempImgViewer = (ImageViewer)item;
                            if (tempImgViewer.Index == i)
                            {
                                if (!image_names.Contains(tempImgViewer.ImageLocation))
                                    image_names.Add(tempImgViewer.ImageLocation);
                                ThumbnailData data = new ThumbnailData();
                                data.fileName = m_ActiveImageViewer.ImageLocation;
                                data.id = m_ActiveImageViewer.ImageID;
                                data.side = m_ActiveImageViewer.ImageSide;
                                data.Name = m_ActiveImageViewer.ImageName;
                                displayThumbnailImage(data);
                                //verticalSroll(tempImgViewer);
                                tempImgViewer.IsActive = true;
                                //this.thumbnail_FLP.AutoScrollPosition = new Point(tempImgViewer.Location.X, tempImgViewer.Location.Y+10); 
                                if (!this.thumbnail_FLP.selectedThumbnails.Contains(tempImgViewer.Index))
                                this.thumbnail_FLP.selectedThumbnails.Add(tempImgViewer.Index);
                                if (!this.thumbnail_FLP.SelectedThumbnailFileNames.Contains(tempImgViewer.ImageLocation))//checks if SelectedThumbnailFileNames contains ImageLocation, if it doesn't contain , then adds the ImageLocation.By Ashutosh 06-09-2017.
                                {
                                    this.thumbnail_FLP.SelectedThumbnailFileNames.Add(tempImgViewer.ImageLocation);
                                }
                            }
                        }
                    }
                }
            }
            //else
            //{
            //    //Added by darshan to resolve the issue 0000657: Reports no limit is set,No images are shown Note no (0002575).
            //    CustomMessageBox.Show(NoOfImagesToBeSelectedText1 + " " + NoOfImagesToBeSelected.ToString() + " " + NoOfImagesToBeSelectedText2, NoOfImagesToBeSelectedHeader, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);
            //}
        }
        private void Thumbnail_noshiftcntrlSelection(ImageViewer sender)
        {
            m_ActiveImageViewer = (ImageViewer)sender;
            if (!isValueChanged)//if the brightness or contrast has been changed in view imaging screen.
            {
                for (int i = 0; i < this.thumbnail_FLP.Controls.Count; i++)
                {
                    if ((this.thumbnail_FLP.Controls[i] is ImageViewer))
                    {
                        ImageViewer tempViewer = (ImageViewer)this.thumbnail_FLP.Controls[i];
                        if (tempViewer.ImageLocation != m_ActiveImageViewer.ImageLocation)
                        {
                            tempViewer.IsActive = false;
                            if (image_names.Count > 0)
                            {
                                image_names.Clear();
                            }

                            this.thumbnail_FLP.selectedThumbnails.Remove(tempViewer.Index);
                            this.thumbnail_FLP.SelectedThumbnailFileNames.Remove(tempViewer.ImageLocation);
                        }
                    }
                }
                m_ActiveImageViewer.IsActive = true;
                if (!image_names.Contains(m_ActiveImageViewer.ImageLocation))
                    image_names.Add(m_ActiveImageViewer.ImageLocation);
                if (!this.thumbnail_FLP.selectedThumbnails.Contains(m_ActiveImageViewer.Index))
                {
                    this.thumbnail_FLP.selectedThumbnails.Add(m_ActiveImageViewer.Index);
                }
                if (!this.thumbnail_FLP.SelectedThumbnailFileNames.Contains(m_ActiveImageViewer.ImageLocation))//checks if SelectedThumbnailFileNames contains ImageLocation, if it doesn't contain , then adds the ImageLocation.By Ashutosh 06-09-2017.

                    this.thumbnail_FLP.SelectedThumbnailFileNames.Add(m_ActiveImageViewer.ImageLocation);
            }
            // this.thumbnail_FLP.AutoScrollPosition = m_ActiveImageViewer.Location;
            // if (arg == null)
            {
                //_eventHandler.Notify(_eventHandler.SetImagingScreen, arg);
                //IVLVariables.ActiveImageID = m_ActiveImageViewer.ImageID;
                
                ThumbnailData data = new ThumbnailData();
                data.fileName = m_ActiveImageViewer.ImageLocation;
                data.id = m_ActiveImageViewer.ImageID;
                data.side = m_ActiveImageViewer.ImageSide;
                data.Name = m_ActiveImageViewer.ImageName;
                displayThumbnailImage(data);
            }
            


        }
        private void thumbnailSelection(ImageViewer sender)
        {
            if (this.isControlKeyPressed)// thumbnail selected without  control key 
            {
                ControlKeyThumbNailSelection(sender);
            }
            else if (this.isShiftKeyPressed) // thumbnail selected without  shift key 
            {
                ShiftKeyThumbNailSelection(sender);
            }
            else // thumbnail selected without control or shift key 
            {
                Thumbnail_noshiftcntrlSelection(sender);
            }

        }
        private void ThumbnailUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isControlKeyPressed = true;
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                isShiftKeyPressed = true;
            }
        }
        private void ThumbnailUI_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                isControlKeyPressed = false;
            else if (e.KeyCode == Keys.ShiftKey)
                isShiftKeyPressed = false;
        }

        private void removeImageViewer()
        {
            //    VisitRepository visitRepo = new VisitRepository();
            //    VisitModel v = visitRepo.GetById(IVLVariables.ActiveVisitID);
            //    v.NoOfImages = v.NoOfImages - thumbnail_FLP.SelectedThumbnailFileNames.Count;
            //    visitRepo.Update(v);
            List<int> removedImageIds = new List<int>();
            List<string> removeImageFilePaths = new List<string>();
            foreach (var item in thumbnail_FLP.SelectedThumbnailFileNames)
            {
                foreach (var item1 in thumbnail_FLP.Controls)
                {
                    if (item1 is ImageViewer)
                    {
                        ImageViewer imgView = item1 as ImageViewer;
                        if (item.Equals(imgView.ImageLocation))
                        {
                            if (File.Exists(imgView.ImageLocation))//This
                            {
                                //ImageModel imgVal = imageRepo.GetById(imgView.ImageID);
                                //imgVal.HideShowRow = true;
                                //imageRepo.Update(imgVal);
                                if (image_names.Contains(imgView.ImageLocation))
                                    image_names.Remove(imgView.ImageLocation);
                                removedImageIds.Add(imgView.ImageID);
                                removeImageFilePaths.Add(imgView.ImageLocation);
                                thumbnail_FLP.Controls.Remove(item1 as Control);
                            }
                            else
                            {
                                CustomMessageBox.Show(DeletedImageMsg, DeletedImageHeader, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }
            }
            Dictionary<string, object> img = new Dictionary<string, object>();
            img.Add("ImageIds", removedImageIds);
            img.Add("ImageLocations", removeImageFilePaths);
            deleteimgs(img);
            this.thumbnail_FLP.selectedThumbnails.Clear();
            this.thumbnail_FLP.SelectedThumbnailFileNames.Clear();
            thumbnail_FLP.TotalThumbnails = this.thumbnail_FLP.Controls.Count;
            ImageViewer[] item2 = new ImageViewer[thumbnail_FLP.Controls.Count];//to update the items in thumbnail after deleting image.By Ashutosh 11-08-2017.
            for (int i = 0; i < thumbnail_FLP.Controls.Count; i++)
            {
                item2[i] = thumbnail_FLP.Controls[i] as ImageViewer;
                item2[i].Index = item2.Length - i;
            }
            for (int i = 0; i < thumbnail_FLP.Controls.Count; i++)
            {
                item2[i] = thumbnail_FLP.Controls[i] as ImageViewer;
                if (i == 0)
                {
                    this.thumbnailSelection(item2[i]);
                    break;
                }
            }
            RefreshThumbNailImageName();
        }

        public void RefreshThumbNailImageName()
        {
            this.Cursor = Cursors.WaitCursor;
            int count = thumbnail_FLP.TotalThumbnails;
            foreach (var item1 in thumbnail_FLP.Controls)
            {
                if (item1 is ImageViewer)
                {
                    ImageViewer imgView = item1 as ImageViewer;
                    {
                        imgView.Index = count--;
                        //This below code has been added by darshan in order to solve defect no:0000530
                        isCDR = imgView.IsCDR;
                        isannotated = imgView.IsAnnotated;
                        imgView.label1.Text = GetImage_Name(imgView.ImageSide, imgView.Index);
                        imgView.label1.Font = new Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }
        private void imageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            //if (!isValueChanged)
            if (!isCaptureSequenceInProgress)// Allow click of thumbnails if the capturing is not in progress to fix the defect 0001667
            {
                ImageViewer imgViewer = sender as ImageViewer;
                id = imgViewer.Index;
                //MessageBox.Show(imgViewer.Index.ToString());
                if (File.Exists(imgViewer.ImageLocation))
                    thumbnailSelection(imgViewer);
                else
                {
                    //Bitmap bm = SystemIcons.Error.ToBitmap();
                    //this.OnImageSizeChanged(this, new ThumbnailImageEventArgs(32));
                    //imgViewer.label1.Text = string.Empty;
                    //imgViewer.Image = bm;
                }
            }
            //else
            //    return;
        }

        public void SelectThumbnail(string imageName)
        {
            foreach (var item in thumbnail_FLP.Controls)
            {
                if (item is ImageViewer)
                {
                    ImageViewer imgView = item as ImageViewer;
                    if (imgView != null)
                    {
                        imgView.IsActive = false;
                    }
                    if (imgView.ImageLocation == imageName)
                    {
                        thumbnailSelection(imgView);
                        imgView.IsActive = true;
                        break;
                    }
                }
            }
        }
        public void ThumbnailUpArrow()
        {
            if (thumbnail_FLP.selectedThumbnails.Count != 0)
            {
                int index = 0;
                if (thumbnail_FLP.selectedThumbnails[0] < thumbnail_FLP.TotalThumbnails)
                    index = ++thumbnail_FLP.selectedThumbnails[0];
                foreach (Control item in thumbnail_FLP.Controls)
                {
                    if (item is ImageViewer)
                    {
                        ImageViewer img = item as ImageViewer;
                        if (img.Index == index)
                        {
                            //thumbnail_FLP.AutoScrollPosition = (img.Location);
                            //selectThumbnail(img);
                            if (File.Exists(img.ImageLocation))
                            {
                                verticalSroll(img);
                                thumbnailSelection(img);
                            }
                            break;
                        }
                    }
                    //if (index != 0)
                    //    thumbnail_FLP. = (int)arg["CurrentIndx"] - ((int)arg["CurrentIndx"] - 1);
                    //else
                    //    thumbnail_FLP.FirstDisplayedScrollingRowIndex = 0;
                }
            }
        }

        public void ThumbnailDownArrow()
        {
            int index = 0;
            //The Below if statement has been added by Darshan to solve Defect no 0000526: Unselected all the thumbnails,Arrow click and crash is appearing.
            if (thumbnail_FLP.selectedThumbnails.Count != 0)
            {
                if (thumbnail_FLP.selectedThumbnails[0] > 1)
                    index = --thumbnail_FLP.selectedThumbnails[0];
                foreach (Control item in thumbnail_FLP.Controls)
                {
                    if (item is ImageViewer)
                    {
                        ImageViewer img = item as ImageViewer;
                        if (img.Index == index)
                        {
                            //thumbnail_FLP.AutoScrollPosition = (img.Location);
                            if (File.Exists(img.ImageLocation))
                            {
                                verticalSroll(img);
                                thumbnailSelection(img);
                            }
                                break;
                        }
                    }
                }
            }
        }
        private void selectThumbnail(ImageViewer img)
        {
            if (m_ActiveImageViewer != null)
            {
                m_ActiveImageViewer.IsActive = false;
            }
            m_ActiveImageViewer = img;
            thumbnailSelection(m_ActiveImageViewer);
            m_ActiveImageViewer.IsActive = true;
            //Args arg = new Args();
            //arg["ThumbnailFileName"] = m_ActiveImageViewer.fileName;
            //arg["id"] = m_ActiveImageViewer.ImageID;
            //arg["side"] = m_ActiveImageViewer.ImageSide;
            //_eventHandler.Notify(_eventHandler.ThumbnailSelected, arg);
        }
        private void ThumbnailUI_Click(object sender, EventArgs e)
        {

        }

        private void ThumbnailUI_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
    public class ThumbnailImageEventArgs : EventArgs
    {
        public ThumbnailImageEventArgs(int size)
        {
            this.Size = size;
        }
        public int Size;
    }
    public delegate void ThumbnailImageEventHandler(object sender, ThumbnailImageEventArgs e);
    public class ThumbnailData
    {
        public int id;
        public int side;
        public bool isCDR;
        public bool isAnnotated;
        public string fileName;
        public string Name;
        public bool isModified;
    }
}
