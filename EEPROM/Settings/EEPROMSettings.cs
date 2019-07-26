using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Globalization;
using System.ComponentModel;
using System.Resources;
namespace EEPROM
{

    public class FeatureSettings
    {

        private EEPROM_Props firmwareSettings;

        public EEPROM_Props FirmwareSettings
        {
            get { return firmwareSettings; }
            set { firmwareSettings = value; }
        }
        private EEPROM_Props postProcessingSettings;

        public EEPROM_Props PostProcessingSettings
        {
            get { return postProcessingSettings; }
            set { postProcessingSettings = value; }
        }
        private EEPROM_Props cameraSettings;

        public EEPROM_Props CameraSettings
        {
            get { return cameraSettings; }
            set { cameraSettings = value; }
        }
        private EEPROM_Props currentVoltageSettings;

        public EEPROM_Props CurrentVoltageSettings
        {
            get { return currentVoltageSettings; }
            set { currentVoltageSettings = value; }
        }
        private EEPROM_Props imageSizeSettings;

        public EEPROM_Props ImageSizeSettings
        {
            get { return imageSizeSettings; }
            set { imageSizeSettings = value; }
        }
        private EEPROM_Props motorSettings;

        public EEPROM_Props MotorSettings
        {
            get { return motorSettings; }
            set { motorSettings = value; }
        }

        public FeatureSettings()
        {

            FirmwareSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.FirmwareSettings.ToByte<StrutureTypes>()), ""); // new FirmwareSettings();
            FirmwareSettings.value = new EEPROM.FirmwareSettings();
            PostProcessingSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.PostProcessingSettings.ToByte<StrutureTypes>()), "");
            PostProcessingSettings.value = new EEPROM.PostProcessingSettings();
            CameraSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.CameraSettings.ToByte<StrutureTypes>()), "");
            CameraSettings.value = new EEPROM.CameraSettings();
            CurrentVoltageSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.CurrentVoltageSettings.ToByte<StrutureTypes>()), "");
            CurrentVoltageSettings.value = new EEPROM.CurrentVoltageSettings();
            ImageSizeSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.ImageSizeSettings.ToByte<StrutureTypes>()), "");
            ImageSizeSettings.value = new EEPROM.ImageSizeSettings();
            MotorSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.MotorSettings.ToByte<StrutureTypes>()), "");
            MotorSettings.value = new EEPROM.MotorSettings();
        }
    }
    [Serializable]
    public class EEPROM_Version_Details_Page
    {
        List<byte[]> byteArrList;
        byte[] byteArr;
        #region Variables
        public StrutureTypes EEPROMStructureType;
        PropertyInfo[] finf;
        private byte[] EEPROM_Version_Number; // To save the EEPROM version details 2 bytes 

        private static EEPROM_Version_Details_Page detailsPage;
        PageDetails pageDetails;
        byte numberOfbytes = 0;
        byte offset = 0;
        byte numberOfPages = 0;
        public Dictionary<object, object> fieldDic;
        Dictionary<object, object> resultDic;

       public  static byte pageSize = 56;
        ResourceManager resourceManager = null;
        #endregion

        #region Properties
        private EEPROM_Props installationDetailsSettings;// Object for installation details class common structure

        public EEPROM_Props InstallationDetailsSettings
        {
            get { return installationDetailsSettings; }
            set { installationDetailsSettings = value; }
        }

        private EEPROM_Props deviceDetailsSettings;

        public EEPROM_Props DeviceDetailsSettings
        {
            get { return deviceDetailsSettings; }
            set { deviceDetailsSettings = value; }
        }
        private EEPROM_Props features;

        public EEPROM_Props Features
        {
            get { return features; }
            set { features = value; }
        }

        private EEPROM_Props anteriorSettings;

        public EEPROM_Props AnteriorSettings
        {
            get { return anteriorSettings; }
            set { anteriorSettings = value; }
        }

        private EEPROM_Props posteriorSettings;

        public EEPROM_Props PosteriorSettings
        {
            get { return posteriorSettings; }
            set { posteriorSettings = value; }
        }

        private EEPROM_Props fFASettings;

        public EEPROM_Props FFASettings
        {
            get { return fFASettings; }
            set { fFASettings = value; }
        }

        private EEPROM_Props anteriorBlueSettings;

        public EEPROM_Props AnteriorBlueSettings
        {
            get { return anteriorBlueSettings; }
            set { anteriorBlueSettings = value; }
        }

        #endregion

        /// <summary>
        /// Constructor 
        /// </summary>
        private EEPROM_Version_Details_Page()
        {
            resourceManager = new ResourceManager("EEPROM.Properties.Resources", typeof(EEPROM.EEPROM_Version_Details_Page).Assembly);

            byteArrList = new List<byte[]>();
            DeviceDetailsSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.DeviceDetailsSettings.ToByte<StrutureTypes>()), "");
            DeviceDetailsSettings dds = new EEPROM.DeviceDetailsSettings();
            DeviceDetailsSettings.value = dds;
            InstallationDetailsSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.InstallationDetailsSettings.ToByte<StrutureTypes>()), "");
            InstallationDetailsSettings ids = new EEPROM.InstallationDetailsSettings();
            InstallationDetailsSettings.value = ids;
            Features = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.Features.ToByte<StrutureTypes>()), "");
            this.Features.value = new EEPROM.Features();
            FFASettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.FFASettings.ToByte<StrutureTypes>()), "");
            FeatureSettings fs = new FeatureSettings();
            FFASettings.value = fs;
            PosteriorSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.PosteriorSettings.ToByte<StrutureTypes>()), ""); 
            FeatureSettings ps = new FeatureSettings();
            PosteriorSettings.value = ps;
            AnteriorSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.AnteriorSettings.ToByte<StrutureTypes>()), "");
            FeatureSettings fs1 = new FeatureSettings();
            AnteriorSettings.value = fs1;

            AnteriorBlueSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.AnteriorBlueSettings.ToByte<StrutureTypes>())
                , "");
            AnteriorBlueSettings.value = new FeatureSettings();
        }


        public static EEPROM_Version_Details_Page GetInstance()
        {
            if (detailsPage == null)
                detailsPage = new EEPROM_Version_Details_Page();
            return detailsPage;
        }

        public void GetFields(List<byte []> eepromArr)
        {
            finf = detailsPage.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            fieldDic = new Dictionary<object, object>();
            resultDic = new Dictionary<object, object>();
            detailsPage.pageDetails = new PageDetails();
            string versionNum = "10";

            detailsPage.pageDetails.versionNumber = Encoding.UTF8.GetBytes(versionNum);
            byteArr = new byte[pageSize];
            byteArrList = new List<byte[]>();
            byteArrList.Add(byteArr);

            byteArr.Initialize();
            Array.Copy(detailsPage.pageDetails.versionNumber, 0, byteArr, 0, detailsPage.pageDetails.versionNumber.Length);
            byteArrList.Add(byteArr);
            int index = byteArrList.Count - 1;
            TraverseProperties(finf, detailsPage, fieldDic, detailsPage.pageDetails, index);// populate field dic 

            byte[] tempArr = GetBytesFromPageDetails(detailsPage.pageDetails);
            Array.Copy(tempArr, detailsPage.pageDetails.versionNumber.Length, byteArrList[index], detailsPage.pageDetails.versionNumber.Length, tempArr.Length - detailsPage.pageDetails.versionNumber.Length);
            if (eepromArr == null || eepromArr.Count == 0)
                byteArr = byteArrList[1].Clone() as byte[];
            else
            {
                eepromArr[0].CopyTo(byteArr, 0);
            }
            byte[] versionArr = new byte[3];
            Array.Copy(byteArr,0,versionArr,0,versionArr.Length);
            string byteArrVersion = Encoding.UTF8.GetString(versionArr);
            
            int LastStructIndex = ((int)byteArr[2] * (int)4);
            byte numberOfPages2Read = (byte)((int)byteArr[LastStructIndex] + (int)byteArr[LastStructIndex + 1]);
            pageDetails =  GetPageDetailsFromByteArr(byteArr);
            offset = 0;
            CreateEEPROMPropsFromByteArr(pageDetails, resultDic,null,null);
            versionNum = "200";
            if (versionNum == byteArrVersion)
            {
                fieldDic = resultDic;
            }
            else
            {
                byteArrList = eepromArr;
                SetFromByteArrDicToCurrentVersionDic(resultDic , fieldDic);
            }

         }
        public void SetFromByteArrDicToCurrentVersionDic(Dictionary<object,object> src, Dictionary<object,object>dest)
        {
            for (int i = 0; i < src.Count; i++)
            {
                 var srcItem = src.ElementAt(i);
                 EEPROM_Props srcItemKey = srcItem.Key  as EEPROM_Props;
                 string srcKeyName = "";
                 string destKeyName = "";
                if (srcItemKey == null)
                    srcKeyName = srcItem.Key.ToString();
                else
                    srcKeyName = srcItemKey.name;
                var srcItemValue = srcItem.Value;

                 var destItem = dest.ElementAt(i);
                 EEPROM_Props destItemKey = destItem.Key as EEPROM_Props;
                 var destItemValue = destItem.Value;
                 if (destItemKey == null)
                     destKeyName = destItem.Key.ToString();
                 else
                     destKeyName = destItemKey.name;
                 if (srcKeyName == destKeyName )
                 {
                     if (srcItemKey != null)
                     {
                      Dictionary<object,object> srcDic = srcItemValue as Dictionary<object, object>;
                      Dictionary<object,object> destDic = destItemValue as Dictionary<object, object>;
                         SetFromByteArrDicToCurrentVersionDic(srcDic, destDic);
                     }
                     else
                     {
                         EEPROM_Props srcValue = srcItem.Value as EEPROM_Props;
                         EEPROM_Props destValue = destItem.Value as EEPROM_Props;
                         destValue.value = srcValue.value;

                     }
                 }
            }
        }

        public byte GetNumberOfPages(PageDetails pageDetails)
        {
            int numberOfbytes = 0;
            int pages = 0;
            for (int i = 0; i < pageDetails.details.Count; i++)
            {
                if (pageDetails.details[i].NumberOfBytes == 0)
                    pages += pageDetails.details[i].StuctNumberOfPages;
                else
                numberOfbytes += pageDetails.details[i].NumberOfBytes;
            }
            if (numberOfbytes != 0)
            {
                float pagesInfloat = ((float)numberOfbytes / (float)pageSize);
                if (pagesInfloat % 1 != 0)
                    pagesInfloat += 0.5f;
                 pages= (int)Math.Round(pagesInfloat, MidpointRounding.AwayFromZero);
            }
            return Convert.ToByte(pages);
        }

        public byte[] GetBytesFromPageDetails(PageDetails pageDetails)
        {
            byte NumOfPages = 0;
            byte[] pageDetailsByteArr = new byte[(pageDetails.details.Count * 4) + 4];
            pageDetailsByteArr[3] = (byte)pageDetails.details.Count;
            byte indx = 4;
            for (int i = 0; i < pageDetails.details.Count; i++)
            {
                pageDetailsByteArr[indx] = pageDetails.details[i].StructStartPage;
                indx += (byte)(1);
                pageDetailsByteArr[indx] = pageDetails.details[i].StuctNumberOfPages;
                indx += (byte)(1);
                pageDetailsByteArr[indx] = pageDetails.details[i].NumberOfBytes;
                indx += (byte)(1);
                pageDetailsByteArr[indx] = pageDetails.details[i].TypeOfStructure;
                indx += (byte)(1);
            }
            return pageDetailsByteArr;
            
        }
        public void CreateEEPROMPropsFromByteArr(PageDetails details, Dictionary<object, object> ByteArrDic,PropertyInfo[] pinf,object obj)
        {
            EEPROM_Props val = null;
            for (int i = 0; i < details.NumberOfStructures; i++)
            {

                StructureDetails structDetails = details.details[i];
                if (structDetails.NumberOfBytes == 0)
                {
                    object result = GetSettingsInstanceFromStructureType(structDetails.TypeOfStructure);
                    StrutureTypes st = (StrutureTypes)structDetails.TypeOfStructure;
                    byte[] arr = byteArrList[structDetails.StructStartPage];
                    PropertyInfo[] propInf = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    val = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, structDetails.TypeOfStructure), result);
                    val.name = st.ToString();
                    val.text = resourceManager.GetString(val.name);
                    val.PageDetails = GetPageDetailsFromByteArr(arr);
                    Dictionary<object, object> dic = new Dictionary<object, object>();
                    offset = 0;
                    CreateEEPROMPropsFromByteArr(val.PageDetails, dic, propInf, result);
                    ByteArrDic.Add(val, dic);

                }
                else
                {
                      val = pinf[i].GetValue(obj) as EEPROM_Props;
                      val.name = pinf[i].Name;
                      val.text = resourceManager.GetString(val.name);
                    byte[] arr = new Byte[details.details[i].NumberOfBytes];

                    if (details.details[i].StuctNumberOfPages > 1)
                    {
                        byte[] byteArr1 = byteArrList[details.details[i].StructStartPage];
                        Array.Copy(byteArr1, offset, arr, 0, pageSize - offset);
                        byte[] byteArr2 = byteArrList[details.details[i].StructStartPage + 1];
                        Array.Copy(byteArr2, 0, arr, pageSize - offset, arr.Length - (pageSize - offset));


                    }
                    else
                    {
                        byteArr = byteArrList[details.details[i].StructStartPage];
                        Array.Copy(byteArr, offset, arr, 0, arr.Length);
                    }
                    val.GetValFromBytes(arr) ;
                      ByteArrDic.Add("EEPROM." + val.name, val);
                      offset += details.details[i].NumberOfBytes;
                      if (offset > pageSize)
                          offset = (byte) ((int)offset -(int) pageSize);
                      else if (offset == pageSize)
                          offset = 0;

                }
            }

        }
        public PageDetails GetPageDetailsFromByteArr(byte[] Arr)
        {
            PageDetails TempPageDetails = new PageDetails();
            TempPageDetails.NumberOfStructures = Arr[3];
            int indx = 4;
            for (int i = 0; i < TempPageDetails.NumberOfStructures; i++)
            {
                StructureDetails structDetails = new StructureDetails();
                structDetails.StructStartPage = Arr[indx];
                indx += (byte)(1);
                structDetails.StuctNumberOfPages = Arr[indx];
                indx += (byte)(1);
                structDetails.NumberOfBytes = Arr[indx];
                indx += (byte)(1);
                structDetails.TypeOfStructure = Arr[indx];
                indx += (byte)(1);
                TempPageDetails.details.Add(structDetails);

            }
            return TempPageDetails;
        }


        public void TraverseProperties(PropertyInfo[] finf, object obj, Dictionary<object, object> dic, PageDetails pageDetails, int byteArrIndx)
        {
            EEPROM_Props val = null;
            pageDetails.NumberOfStructures = (byte)finf.Length;
            numberOfbytes = 0;
            for (int i = 0; i < finf.Length; i++)
            {
                Type t = finf[i].PropertyType;
                if (t == typeof(EEPROM_Props))
                {
                    val = finf[i].GetValue(obj) as EEPROM_Props;
                    val.name = finf[i].Name;
                    val.text = resourceManager.GetString(val.name);
 
                    if (val.eepromDataType.dataType == DataTypes.Tree)
                    {
                        offset = 0;
                        val.PageDetails = new PageDetails();
                        
                        StructureDetails structureDetails = new StructureDetails();
                  
                        object objProperty = GetSettingsInstanceFromStructureType(val.eepromDataType.length);
                        PropertyInfo[] pinf = objProperty.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        val.PageDetails.NumberOfStructures = Convert.ToByte(pinf.Length);
                        
                        Dictionary<object, Object> temp = new Dictionary<object, object>();
                        byteArr = new byte[pageSize];
                        byteArrList.Add(byteArr);
                        int index = byteArrList.Count - 1;
                        TraverseProperties(pinf, objProperty, temp, val.PageDetails, index);
                        byte[] tempArr = GetBytesFromPageDetails(val.PageDetails);
                        Array.Copy(tempArr, byteArrList[index], tempArr.Length);
                        structureDetails.StructStartPage =  Convert.ToByte(index);
                        int pages = (int)GetNumberOfPages(val.PageDetails);
                        
                        structureDetails.StuctNumberOfPages = (byte)( pages + 1 ); // the + 1 is for pageDetails page.
                        structureDetails.TypeOfStructure = val.eepromDataType.length;
                        pageDetails.details.Add(structureDetails);
                     
                        dic.Add(val, temp);
                    }
                    else
                    {
                        dic.Add("EEPROM." + val.name, val);
                        byte[] arr =  val.GetBytesFromVal();
                        byte[] resArr = new byte[pageSize];
                        if (offset == 0)
                            byteArrList.Add(resArr);

                        StructureDetails structureDetails = new StructureDetails();
                        
                        structureDetails.StructStartPage = (byte)(byteArrList.Count-1);
                        //structureDetails.OffsetPageIndx = offset;
                        structureDetails.NumberOfBytes = Convert.ToByte(arr.Length);//+ (int) numberOfbytes);
                        numberOfbytes += structureDetails.NumberOfBytes;
                        float pagesInfloat = ((float)numberOfbytes / (float)pageSize);
                        if (pagesInfloat % 1 != 0)
                            pagesInfloat += 0.5f;
                        int pages = (int)Math.Round(pagesInfloat, MidpointRounding.AwayFromZero);
                        if (numberOfbytes > pageSize)
                        {
                          
                            structureDetails.StuctNumberOfPages = Convert.ToByte(pages);
                            int pageOverFlow = (((int)structureDetails.StuctNumberOfPages - 1) * (int)pageSize) ;
                            pageOverFlow -= (int)offset;
                            numberOfbytes = 0;
                            byte[] arr1 = new byte[pageSize];
                            byte[] arr2 = new byte[pageSize];
                            int index1 = pageSize - offset;
                            Array.Copy(arr, 0, byteArrList[byteArrList.Count - 1], offset, index1);
                            Array.Copy(arr, index1, arr2, 0, arr.Length - index1);
                            byteArrList.Add(arr2);
                            offset = Convert.ToByte((int)structureDetails.NumberOfBytes - pageOverFlow);
                        }
                        else 
                        {
                            structureDetails.StuctNumberOfPages = 1;
                            byte[] arr1 = byteArrList[byteArrList.Count - 1];
                            Array.Copy(arr, 0, byteArrList[byteArrList.Count - 1], offset, arr.Length);
                            offset += structureDetails.NumberOfBytes;
                            if (numberOfbytes == pageSize)
                            {
                                numberOfbytes = 0;
                                offset = 0;
                            }
                        }
                         pageDetails.details.Add(structureDetails);

                     }
                }
            }

        }

        public object GetSettingsInstanceFromStructureType(byte structureValue)
        {

            EEPROM.StrutureTypes structType = (StrutureTypes)structureValue;
          Type t = Type.GetType("EEPROM." + structType.ToString());

            if (t ==  null)
                t = Type.GetType("EEPROM.FeatureSettings");
            object objProperty = Activator.CreateInstance(t);
            return objProperty;

        }

    }
    public class PageDetails
    {
        public byte[] versionNumber = new byte[2];
        public byte NumberOfStructures;
        public List<StructureDetails> details;

        public PageDetails()
        {
            details = new List<StructureDetails>();
        }
    }
    public class StructureDetails
    {
        public byte StructStartPage;
        public byte StuctNumberOfPages;// Number of pages each structure takes
        public byte TypeOfStructure;//Type of structure is defined by the enum
        public byte NumberOfBytes;// if value is 1 it indicates its structure with more structures else if 0 it means its a field.

    }

}
