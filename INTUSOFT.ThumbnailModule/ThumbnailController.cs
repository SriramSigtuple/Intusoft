using System;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
namespace INTUSOFT.ThumbnailModule
{
    public class ThumbnailControllerEventArgs : EventArgs
    {
        public ThumbnailControllerEventArgs(string imageFilename,int id ,int index,int side,bool isannotated,bool isCDR)
        {
            this.ImageFilename = imageFilename;
            this.id = id;
            this.index = index;
            this.side = side;
            this.IsAnnotated = isannotated;
            this.isCDR = isCDR;
        }
        public ThumbnailControllerEventArgs(string imageFilename, bool isRemove)
        {
            this.ImageFilename = imageFilename;
            this.isRemove = isRemove;
        }
        public string ImageFilename;
        public int id;
        public int side;
        public bool isRemove;
        public int index;
        public bool IsAnnotated;
        public bool isCDR;
    }

    public delegate void ThumbnailControllerEventHandler(object sender, ThumbnailControllerEventArgs e);

    public class ThumbnailController
    {
        private bool m_CancelScanning;
        static readonly object cancelScanningLock = new object();

        public bool CancelScanning
        {
            get
            {
                lock (cancelScanningLock)
                {
                    return m_CancelScanning;
                }
            }
            set
            {
                lock (cancelScanningLock)
                {
                    m_CancelScanning = value;
                }
            }
        }

        public event ThumbnailControllerEventHandler OnAdd;
        public event ThumbnailControllerEventHandler OnRemove;
        public event ThumbnailControllerEventHandler invalid_images;
        public delegate void CorruptedImages(List<int> ids);
        public event CorruptedImages corruptedImages;

        public ThumbnailController()
        {
            
        }
        //String[] ThumbnailNames;
        public void CreateThumbnails( List<string> FileNames,List<int> ids,List<int> sides,List<bool> isannotated,List<bool> isCDR)
        {
            CancelScanning = false;
            this.AddFolderIntern(FileNames, ids, sides, isannotated,isCDR);
            //ThumbnailNames = FileNames;
            //ThreadStart threadStart = new ThreadStart(AddFolder);

            //Thread thread = new Thread(threadStart);
            //thread.IsBackground = true;
            //thread.Start();
        }
        public void CreateThumbnails(List<ThumbnailData> ThumbnailList)
        {
            CancelScanning = false;
            this.AddFolderIntern(ThumbnailList);
            //ThumbnailNames = FileNames;
            //ThreadStart threadStart = new ThreadStart(AddFolder);

            //Thread thread = new Thread(threadStart);
            //thread.IsBackground = true;
            //thread.Start();
        }
        private void AddFolder()
        {
            //string path = (string)folderPath;
           // string[] FileNames = folderPath as string[];
            //if (this.OnStart != null)
            //{
            //    this.OnStart(this, new ThumbnailControllerEventArgs());
            //}

           // this.AddFolderIntern(ThumbnailNames,ids,sides);

            //if (this.OnEnd != null)
            //{
            //    this.OnEnd(this, new ThumbnailControllerEventArgs(null));
            //}

            CancelScanning = false;
        }
        private void DeleteThumbnail(string str,bool isRemove)
        {
            this.OnRemove(this, new ThumbnailControllerEventArgs(str, isRemove));
        }
      //This method has been added by Darshan on 18-09-2015 to check image is a valid image or not.
        public int courrouptedimages = 0;
        //This below line has been added by Darshan on 18-09-2015 to check image is a valid image or not.
         
        public List<int> image_ids = new List<int>();  
 private void AddFolderIntern(List<string> folderPath,List<int> ids,List<int> sides,List<bool> isannotated,List<bool> isCDR)
        {
            int count =0;
            foreach(var file in folderPath)
            {
               // if (CancelScanning) break;
                int indx = folderPath.IndexOf(file); ;
                 this.OnAdd(this, new ThumbnailControllerEventArgs(file, ids[indx], count, sides[indx], isannotated[indx],isCDR[indx]));
                if(!ImageViewer.isValidImage) // check validity of the image
                    image_ids.Add(ids[indx]);// add corrupted images id to a list

                count++;
                }
      corruptedImages(image_ids);// Fire remove corrupted image id event 

            }

 private void AddFolderIntern(List<ThumbnailData> ThumbnailList)
 {
     int count = 0;
     foreach (ThumbnailData file in ThumbnailList)
     {
         // if (CancelScanning) break;
         int indx = ThumbnailList.IndexOf(file); ;
         this.OnAdd(this, new ThumbnailControllerEventArgs(file.fileName, file.id, count, file.side, file.isAnnotated, file.isCDR));
         if (!ImageViewer.isValidImage) // check validity of the image
             image_ids.Add(file.id);// add corrupted images id to a list

         count++;
     }
     corruptedImages(image_ids);// Fire remove corrupted image id event 

 }

        }
}
