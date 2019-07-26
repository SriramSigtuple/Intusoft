using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Common;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace INTUSOFT.Imaging
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]

    public class eeprom_var_int32
    {

        public Int32 value;
        public Int32 min;
        public Int32 max;
        public eeprom_var_int32(Int32 valMin, Int32 valMax, Int32 value)
        {
            this.value = value;
            this.min = valMin;
            this.max = valMax;
        }
        public eeprom_var_int32()
        {

        }
        public byte[] ToByteArray()
        {
            return EEPromUtils.ObjectToByteArray(this);
        }
    }
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]

    public class eeprom_var_int16
    {

        public Int16 value;
        public Int16 min;
        public Int16 max;
        public eeprom_var_int16(Int16 valMin, Int16 valMax, Int16 value)
        {
            this.value = value;
            this.min = valMin;
            this.max = valMax;
        }
        public eeprom_var_int16()
        {

        }
        public byte[] ToByteArray()
        {
            return EEPromUtils.ObjectToByteArray(this);
        }
    }
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]

    public class eeprom_var_float
    {

        public float value;
        public float min;
        public float max;
        public eeprom_var_float(float valMin, float valMax, float value)
        {
            this.value = value;
            this.min = valMin;
            this.max = valMax;
        }
        public eeprom_var_float()
        {

        }
        public byte[] ToByteArray()
        {
            return EEPromUtils.ObjectToByteArray(this);
        }
    }
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]

    public class eeprom_var_byte
    {

        public byte value;
        public byte min;
        public byte max;
        public eeprom_var_byte(byte valMin, byte valMax, byte value)
        {
            this.value = value;
            this.min = valMin;
            this.max = valMax;
        }
        public eeprom_var_byte()
        {

        }
        public byte[] ToByteArray()
        {
            return EEPromUtils.ObjectToByteArray(this);
        }
    }
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]

    public class eepromVar
    {
        //private static byte[] _value;

        public byte[] value;
        //{
        //    get { return eepromVar._value; }
        //    set { eepromVar._value = value; }
        //}

        public eepromVar(byte valLength)
        {
            value = new byte[valLength];

        }
        public byte[] ToByteArray()
        {
            return value;
        }

    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class EEPROM
    {

        //First Version of EEPROM Structure 

        //Each structure, to be limited to max of 64 bytes
        #region Variables
        static FieldInfo[] fieldInf;
        private static EEPROM _eeprom;

        public EEPROM _EEPROM
        {
            get { return _eeprom; }
            set { _eeprom = value; }
        }
        public DeviceDetails deviceDetails;
        public ColorCorrectionSettings colorCorrectionSettings;
        public HotSpotCorrectionSettings hotSpotCorrectionSettings;
        public ImageSettings imageSettings;
        public DeviceDetailedStatus deviceDetailedStatus;
        public DeviceStatus deviceStatus;
        public DeviceHWParameters deviceHWParameters;
        public DeviceStats deviceStats;
        public InstallationDetails installationDetails;
        #endregion
        public static EEPROM GetInstance()
        {
            if (_eeprom == null)
                _eeprom = new EEPROM();


            return _eeprom;
        }
        public static void traverseNodes(FieldInfo[] finf)
        {

        }
        public EEPROM()
        {

            deviceDetails = new DeviceDetails();
            installationDetails = new InstallationDetails();
            deviceStats = new DeviceStats();
            deviceStatus = new DeviceStatus();
            deviceHWParameters = new DeviceHWParameters();
            imageSettings = new ImageSettings();
            hotSpotCorrectionSettings = new HotSpotCorrectionSettings();
            colorCorrectionSettings = new ColorCorrectionSettings();
            deviceDetailedStatus = new DeviceDetailedStatus();
            // WriteEEPROM();
            fieldInf = this.GetType().GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

            ReadEEPROM();

        }
        int numberOfBytes;
        public void ReadEEPROM()
        {
            numberOfBytes = 0;
            for (int i = 0; i < fieldInf.Length; i++)
            {

                object obj = fieldInf[i].GetValue(this);
                if (obj != null)
                    numberOfBytes += EEPromUtils.Get_Number_Of_bytes(obj);

            }


            float val = (float)numberOfBytes / (float)64;
            if (val % 1 != 0)
                val += 0.5f;
            int pages = (int)Math.Round(val, MidpointRounding.AwayFromZero);
            byte[] arr = new byte[64 * pages];
            byte[] retArr = new byte[70];

            for (int i = 1; i <= pages; i++)
            {

                EEPromUtils.ReadEEProm(Convert.ToByte(i), ref retArr);
                Array.Copy(IntucamBoardCommHelper.EEPROM_DATA_ARRAY, 0, arr, (i - 1) * 64, retArr.Length - 6);

            }
            int indx = 0;
            byte[] byteArrList = new byte[numberOfBytes];
            Array.Copy(arr, 0, byteArrList, 0, byteArrList.Length);
            int tempBytesCnt = 0;
            for (int i = 0; i < fieldInf.Length; i++)
            {
                object obj = fieldInf[i].GetValue(this);
                if (obj != null)
                {
                    tempBytesCnt = EEPromUtils.Get_Number_Of_bytes(obj);
                    if (tempBytesCnt != 0)
                    {
                        byte[] SrcArr = new byte[tempBytesCnt];
                        Array.Copy(byteArrList, indx, SrcArr, 0, SrcArr.Length);
                        indx += tempBytesCnt;
                        EEPromUtils.GetObject_Iteratively(ref obj, SrcArr);
                        fieldInf[i].SetValue(this, obj);
                    }
                }
            }

        }
        public void ReadEEPROM(byte pageNumber)
        {
            {
                IntucamBoardCommHelper.EEPROM_DATA_ARRAY = new byte[70];

                byte[] tempArr = new byte[70];
                EEPromUtils.ReadEEProm(pageNumber, ref tempArr);
                string str = pageNumber.ToString() + " " + Encoding.UTF8.GetString(IntucamBoardCommHelper.EEPROM_DATA_ARRAY);
                CustomMessageBox.Show(str);
            }
        }
        public unsafe void WriteEEPROM()
        {
            if (numberOfBytes == 0)
            {
                for (int i = 0; i < fieldInf.Length; i++)
                {

                    object obj = fieldInf[i].GetValue(this);
                    if (obj != null)
                        numberOfBytes += EEPromUtils.Get_Number_Of_bytes(obj);

                }
            }
            float val = (float)numberOfBytes / (float)64;
            if (val % 1 != 0)
                val += 0.5f;
            int pages = (int)Math.Round(val, MidpointRounding.AwayFromZero);
            byte[] arr = new byte[pages * 64];

            int indx = 0;
            for (int i = 0; i < EEPROM_UC.eepromFields.Count; i++)
            {
                KeyValuePair<string, object> obj = EEPROM_UC.eepromFields.ElementAt(i);//.GetValue(null);
                //if (obj != null)
                {
                    byte[] temp = EEPromUtils.GetBytes(obj);
                    if (temp.Length > 0)
                    {
                        Array.Copy(temp, 0, arr, indx, temp.Length);
                        indx += temp.Length;
                    }
                }
            }



            byte[] TempArr = new byte[64];


            EEPROM_Version version = EEPROM_Version.GetInstance();

            version.Bytes_To_Read_Write.value = Convert.ToByte(pages);
            string str = DateTime.Now.ToString("dd/MM/yyyy");
            byte[] tempArr = Encoding.UTF8.GetBytes(str);
            version.DateModified.value = Encoding.UTF8.GetBytes(str);
            str = "1.0.0";
            version.EEPROM_Version_Number.value = Encoding.UTF8.GetBytes(str);

            byte[] verionBytes = EEPromUtils.GetBytes_Iteratively(version);
            Array.Copy(verionBytes, 0, TempArr, 0, verionBytes.Length);

            EEPromUtils.WriteEEProm(199, TempArr);
            //EEPromUtils.
            for (int i = 1; i <= pages; i++)
            {
                Array.Copy(arr, (i - 1) * 64, TempArr, 0, TempArr.Length);
                EEPromUtils.WriteEEProm(Convert.ToByte(i), TempArr);
                EEPromUtils.ReadEEProm(Convert.ToByte(i), ref TempArr);
                string readStr = Encoding.UTF8.GetString(IntucamBoardCommHelper.EEPROM_DATA_ARRAY);
            }
            CustomMessageBox.Show("Writing Done");

        }
        public void ResetEEPROM()
        {
            byte[] TempArr = new byte[64];
            Array.ConvertAll<byte, byte>(TempArr, b => b = byte.MinValue);

            for (int i = 1; i <= 255; i++)
            {
                EEPromUtils.WriteEEProm(Convert.ToByte(i), TempArr);

            }
        }
        public void GetBytes()
        {

        }
        public void WriteEEPROM(byte pageNumber, string data)
        {
            data = pageNumber.ToString() + " " + data;
            fieldInf = _eeprom.GetType().GetFields();

            for (int i = 0; i < fieldInf.Length; i++)
            {
                FieldInfo[] f = fieldInf[i].FieldType.GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

            }


            byte[] DataArr = Encoding.UTF8.GetBytes(data);//EEPromUtils.ObjectToByteArr(this);
            byte[] TempArr = new byte[64];
            //EEPromUtils.
            {
                Array.Copy(DataArr, 0, TempArr, 0, DataArr.Length);
                Array.Copy(TempArr, 0, IntucamBoardCommHelper.EEPROM_DATA_ARRAY, 0, TempArr.Length);

                EEPromUtils.WriteEEProm(pageNumber, TempArr);
                EEPromUtils.ReadEEProm(pageNumber, ref TempArr);

                IntucamBoardCommHelper.EEPROM_DATA_ARRAY = new byte[70];
                CustomMessageBox.Show(Encoding.UTF8.GetString(IntucamBoardCommHelper.EEPROM_DATA_ARRAY));

            }
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class DeviceDetails
    {
        //public int Page_Number;
        //public int Number_Of_Bytes;
        //public int Number_Of_Pages;

        private static eepromVar _DeviceNumber;
        public eepromVar DeviceNumber
        {
            get
            {
                return _DeviceNumber;
            }
            set
            {
                _DeviceNumber = value;
            }
        }// For eg: IVL-FC1-0001
        private static eepromVar _DeviceModel;
        public eepromVar DeviceModel
        {
            get
            {
                return _DeviceModel;
            }
            set
            {
                _DeviceModel = value;
            }
        }//IntuCam-45a or b  
        private static eepromVar _Production_Batch; // 2015_02
        public eepromVar Production_Batch
        {
            get
            {
                return _Production_Batch;
            }
            set
            {
                _Production_Batch = value;
            }
        }
        private static eepromVar _Optics_Version; // 1.0.A
        public eepromVar Optics_Version
        {
            get
            {
                return _Optics_Version;
            }
            set
            {
                _Optics_Version = value;
            }
        }

        private static eepromVar _Camera_Model; // V1M1 = (Vendor 1, Model 1)
        public eepromVar Camera_Model
        {
            get
            {
                return _Camera_Model;
            }
            set
            {
                _Camera_Model = value;
            }
        }
        private static eepromVar _Hardware_Board_Version; // “v2p0”  of PDB
        public eepromVar Hardware_Board_Version
        {
            get
            {
                return _Hardware_Board_Version;
            }
            set
            {
                _Hardware_Board_Version = value;
            }
        }

        public DeviceDetails()
        {
            DeviceNumber = new eepromVar(Convert.ToByte(16));// For eg: IVL-FC1-0001
            DeviceModel = new eepromVar(Convert.ToByte(12));//IntuCam-45a or b  
            Production_Batch = new eepromVar(Convert.ToByte(8)); // 2015_02
            Optics_Version = new eepromVar(Convert.ToByte(8)); // 1.0.A
            Camera_Model = new eepromVar(Convert.ToByte(8)); // V1M1 = (Vendor 1, Model 1)
            Hardware_Board_Version = new eepromVar(Convert.ToByte(8)); // “v2p0”  of PDB
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class InstallationDetails // EEPROM page 2
    {
        private static eepromVar InstallationSite;// For Name of Hospital/clinic 

        public eepromVar _InstallationSite
        {
            get { return InstallationDetails.InstallationSite; }
            set { InstallationDetails.InstallationSite = value; }
        }
        private static eepromVar InstallationCity;

        public eepromVar _InstallationCity
        {
            get { return InstallationDetails.InstallationCity; }
            set { InstallationDetails.InstallationCity = value; }
        }
        private static eepromVar InstallationDate;

        public eepromVar _InstallationDate
        {
            get { return InstallationDetails.InstallationDate; }
            set { InstallationDetails.InstallationDate = value; }
        }
        public InstallationDetails()
        {
            InstallationSite = new eepromVar(Convert.ToByte(32));
            InstallationCity = new eepromVar(Convert.ToByte(16));
            InstallationDate = new eepromVar(Convert.ToByte(12));
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class DeviceStats // for service information // EEPROM page 3
    {
        private static eeprom_var_int32 _ImageCaptureCount; // for number of images capture

        public eeprom_var_int32 ImageCaptureCount
        {
            get { return _ImageCaptureCount; }
            set { _ImageCaptureCount = value; }
        }
        private static eeprom_var_int32 _IR_LED_On_time;    // (in mins) Read only for software 

        public eeprom_var_int32 IR_LED_On_time
        {
            get { return _IR_LED_On_time; }
            set { _IR_LED_On_time = value; }
        }
        private static eeprom_var_int32 _Flash_LED_On_time; // (in mins)  Read only for software

        public eeprom_var_int32 Flash_LED_On_time
        {
            get { return _Flash_LED_On_time; }
            set { _Flash_LED_On_time = value; }
        }
        private static eeprom_var_int32 _Number_of_Trigger_button_operations;

        public eeprom_var_int32 Number_of_Trigger_button_operations
        {
            get { return _Number_of_Trigger_button_operations; }
            set { _Number_of_Trigger_button_operations = value; }
        }
        private static eeprom_var_int32 _Rotary_operations_count;

        public eeprom_var_int32 Rotary_operations_count
        {
            get { return _Rotary_operations_count; }
            set { _Rotary_operations_count = value; }
        }
        private static eeprom_var_int32 _Motor_Movement_Count;

        public eeprom_var_int32 Motor_Movement_Count
        {
            get { return _Motor_Movement_Count; }
            set { _Motor_Movement_Count = value; }
        }

        public DeviceStats()
        {
            ImageCaptureCount = new eeprom_var_int32(0, 0, 0);
            ImageCaptureCount = new eeprom_var_int32(0, 0, 0); // for number of images capture
            IR_LED_On_time = new eeprom_var_int32(0, 0, 0);    // (in mins) Read only for software 
            Flash_LED_On_time = new eeprom_var_int32(0, 0, 0); // (in mins)  Read only for software
            Number_of_Trigger_button_operations = new eeprom_var_int32(0, 0, 0);
            Rotary_operations_count = new eeprom_var_int32(0, 0, 0);
            Motor_Movement_Count = new eeprom_var_int32(0, 0, 0);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class DeviceHWParameters // device particular parameters // EEPROM page 4
    {
        private static eeprom_var_int16 _MotorCompensationStepsIR2Flash; // 2 bytes to save number of motor compensation from IR to flash light. 

        public eeprom_var_int16 MotorCompensationStepsIR2Flash
        {
            get { return _MotorCompensationStepsIR2Flash; }
            set { _MotorCompensationStepsIR2Flash = value; }
        }
        private static eeprom_var_int32 _Motor_Steps_Range;

        public eeprom_var_int32 Motor_Steps_Range
        {
            get { return _Motor_Steps_Range; }
            set { _Motor_Steps_Range = value; }
        }
        private static eeprom_var_int32 _Motor_Reset_Position;

        public eeprom_var_int32 Motor_Reset_Position
        {
            get { return _Motor_Reset_Position; }
            set { _Motor_Reset_Position = value; }
        }
        // Dheeraj to Add the Digipot Variable

        private static eeprom_var_int32 _Factory_IR_Current;

        public eeprom_var_int32 Factory_IR_Current
        {
            get { return _Factory_IR_Current; }
            set { _Factory_IR_Current = value; }
        }
        private static eeprom_var_int32 _Factory_Flash_Current;

        public eeprom_var_int32 Factory_Flash_Current
        {
            get { return _Factory_Flash_Current; }
            set { _Factory_Flash_Current = value; }
        }
        // Dheeraj : Difference in terms ADC conversion step size,
        // Actual current is arrived at by multiplying the steps with a floating point number – in API
        private static eeprom_var_byte _IR_Max_Voltage;// Voltage Regulator settings

        public eeprom_var_byte IR_Max_Voltage
        {
            get { return _IR_Max_Voltage; }
            set { _IR_Max_Voltage = value; }
        }
        private static eeprom_var_byte _Flash_Max_Voltage; // Voltage Regulator settings

        public eeprom_var_byte Flash_Max_Voltage
        {
            get { return _Flash_Max_Voltage; }
            set { _Flash_Max_Voltage = value; }
        }
        private static eeprom_var_byte _Flash_Boost_Enable; // Enable Flash Boost

        public eeprom_var_byte Flash_Boost_Enable
        {
            get { return _Flash_Boost_Enable; }
            set { _Flash_Boost_Enable = value; }
        }
        private static eeprom_var_byte _Single_Frame_Capture_Enable; // Enable Single Frame Capture

        public eeprom_var_byte Single_Frame_Capture_Enable
        {
            get { return _Single_Frame_Capture_Enable; }
            set { _Single_Frame_Capture_Enable = value; }
        }
        private static eeprom_var_byte _Flash_Boost_Value; // Flash Boost Value

        public eeprom_var_byte Flash_Boost_Value
        {
            get { return _Flash_Boost_Value; }
            set { _Flash_Boost_Value = value; }
        }

        private static eeprom_var_byte _Flash_Offset_Value1; // Flash Offset value 1

        public eeprom_var_byte Flash_Offset_Value1
        {
            get { return Flash_Offset_Value1; }
            set { Flash_Offset_Value1 = value; }
        }

        private static eeprom_var_byte _Flash_Offset_Value2; // Flash Offset value 2

        public eeprom_var_byte Flash_Offset_Value2
        {
            get { return _Flash_Offset_Value2; }
            set { _Flash_Offset_Value2 = value; }
        }



        public DeviceHWParameters()
        {
            MotorCompensationStepsIR2Flash = new eeprom_var_int16(0, 100, 25);// 25; // 2 bytes to save number of motor compensation from IR to flash light. 
            Motor_Steps_Range = new eeprom_var_int32(0, 100, 25); ;
            Motor_Reset_Position = new eeprom_var_int32(0, 100, 25); ;

            Factory_IR_Current = new eeprom_var_int32(0, 255, 100); ;
            Factory_Flash_Current = new eeprom_var_int32(0, 255, 100); ;
            IR_Max_Voltage = new eeprom_var_byte(0, 255, 100); ;// Voltage Regulator settings
            Flash_Max_Voltage = new eeprom_var_byte(0, 255, 100); ; // Voltage Regulator settings
            Single_Frame_Capture_Enable = new eeprom_var_byte(0, 1, 0);
            Flash_Boost_Enable = new eeprom_var_byte(0, 1, 0);
            Flash_Boost_Value = new eeprom_var_byte(0, 255, 70);
            Flash_Offset_Value1 = new eeprom_var_byte(0, 255, 80);
            Flash_Offset_Value2 = new eeprom_var_byte(0, 255, 20);

        }

    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class DeviceStatus // not for EEPROM
    {


        static eeprom_var_byte _FlashStatus;

        public eeprom_var_byte FlashStatus
        {
            get { return _FlashStatus; }
            set { _FlashStatus = value; }
        }
        static eeprom_var_byte _IRStatus;

        public eeprom_var_byte IRStatus
        {
            get { return _IRStatus; }
            set { _IRStatus = value; }
        }

        private static eeprom_var_byte _LR_Sensor_Status;

        public eeprom_var_byte LR_Sensor_Status
        {
            get { return _LR_Sensor_Status; }
            set { _LR_Sensor_Status = value; }
        }
        static eeprom_var_byte _Camera_status;

        public eeprom_var_byte Camera_status
        {
            get { return _Camera_status; }
            set { _Camera_status = value; }
        }
        private static eeprom_var_byte _Trigger_Status; // Trigger 

        public eeprom_var_byte Trigger_Status
        {
            get { return _Trigger_Status; }
            set { _Trigger_Status = value; }
        }
        private static eeprom_var_byte _Motor_Sensor_Status;

        public eeprom_var_byte Motor_Sensor_Status
        {
            get { return _Motor_Sensor_Status; }
            set { _Motor_Sensor_Status = value; }
        }
        static eeprom_var_byte _Operating_Mode;

        public eeprom_var_byte Operating_Mode
        {
            get { return _Operating_Mode; }
            set { _Operating_Mode = value; }
        }
        public DeviceStatus()
        {
            FlashStatus = new eeprom_var_byte(0, 1, 0);
            IRStatus = new eeprom_var_byte(0, 1, 0);
            LR_Sensor_Status = new eeprom_var_byte(0, 1, 0);
            Camera_status = new eeprom_var_byte(0, 1, 0);
            Trigger_Status = new eeprom_var_byte(0, 1, 0);
            Motor_Sensor_Status = new eeprom_var_byte(0, 1, 0);
            Operating_Mode = new eeprom_var_byte(0, 1, 0);
        }

    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class ImageSettings  //EEPROM Page 5
    {

        private static eeprom_var_byte ImagingMode; // raw, CC mode

        public eeprom_var_byte _ImagingMode
        {
            get { return ImagingMode; }
            set { ImagingMode = value; }
        }
        private static eeprom_var_int16 image_Width;

        public eeprom_var_int16 Image_Width
        {
            get { return image_Width; }
            set { image_Width = value; }
        }
        private static eeprom_var_int16 image_height;

        public eeprom_var_int16 Image_height
        {
            get { return image_height; }
            set { image_height = value; }
        }

        private static eeprom_var_int16 ImageCentreX;// 2 bytes to save image centre x- axis

        public eeprom_var_int16 _ImageCentreX
        {
            get { return ImageCentreX; }
            set { ImageCentreX = value; }
        }
        private static eeprom_var_int16 ImageCentreY;// 2 bytes to save image centre y- axis

        public eeprom_var_int16 _ImageCentreY
        {
            get { return ImageCentreY; }
            set { ImageCentreY = value; }
        }

        private static eeprom_var_int16 MaskWidth;// 2 bytes to save image mask width 

        public eeprom_var_int16 _MaskWidth
        {
            get { return MaskWidth; }
            set { MaskWidth = value; }
        }
        private static eeprom_var_int16 MaskHeight;// 2 bytes to save image mask height

        public eeprom_var_int16 _MaskHeight
        {
            get { return MaskHeight; }
            set { MaskHeight = value; }
        }

        private static eeprom_var_byte ImageShiftX;//1 byte to save image shift horizontal

        public eeprom_var_byte _ImageShiftX
        {
            get { return ImageShiftX; }
            set { ImageShiftX = value; }
        }
        private static eeprom_var_byte ImageShiftY;//1 byte to save image shift vertical

        public eeprom_var_byte _ImageShiftY
        {
            get { return ImageShiftY; }
            set { ImageShiftY = value; }
        }

        public ImageSettings()
        {
            ImagingMode = new eeprom_var_byte(0, 5, 0);
            image_Width = new eeprom_var_int16(0, 10000, 2048);
            image_height = new eeprom_var_int16(0, 10000, 1536);
            ImageCentreX = new eeprom_var_int16(0, 10000, 2048);
            ImageCentreY = new eeprom_var_int16(0, 10000, 2048);
            MaskWidth = new eeprom_var_int16(0, 10000, 2048);
            MaskHeight = new eeprom_var_int16(0, 10000, 2048);
            ImageShiftX = new eeprom_var_byte(0, 255, 4);
            ImageShiftY = new eeprom_var_byte(0, 255, 4);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class HotSpotCorrectionSettings   //EEPROM Page 6
    {
        #region HotSpotParameters
        private static eeprom_var_byte MethodOfHotSpotCorrection;// 1 byte to Signify which to save which method of hotspot correction  to be used.

        public eeprom_var_byte _MethodOfHotSpotCorrection
        {
            get { return MethodOfHotSpotCorrection; }
            set { MethodOfHotSpotCorrection = value; }
        }

        private static eeprom_var_int16 preset_live_Gain;

        public eeprom_var_int16 Preset_live_Gain
        {
            get { return preset_live_Gain; }
            set { preset_live_Gain = value; }
        }
        private static eeprom_var_int16 preset_capture_Gain;

        public eeprom_var_int16 Preset_capture_Gain
        {
            get { return preset_capture_Gain; }
            set { preset_capture_Gain = value; }
        }
        private static eeprom_var_int16 preset_live_exposure; // multiply byte value by 100 

        public eeprom_var_int16 Preset_live_exposure
        {
            get { return preset_live_exposure; }
            set { preset_live_exposure = value; }
        }
        private static eeprom_var_int16 preset_capture_exposrue; // multiply byte value by 100 

        public eeprom_var_int16 Preset_capture_exposrue
        {
            get { return preset_capture_exposrue; }
            set { preset_capture_exposrue = value; }
        }

        private static eeprom_var_byte GainSlope; // 1 byte to save Gain Slope value for actual gain value to preset value.

        public eeprom_var_byte _GainSlope
        {
            get { return GainSlope; }
            set { GainSlope = value; }
        }
        private static eeprom_var_int16 ShadowOuterRingSize;// 2 byte to save image shadow outer radius

        public eeprom_var_int16 _ShadowOuterRingSize
        {
            get { return ShadowOuterRingSize; }
            set { ShadowOuterRingSize = value; }
        }
        private static eeprom_var_int16 ShadowRadiusInner;// 2 byte to save image shadow inner radius

        public eeprom_var_int16 _ShadowRadiusInner
        {
            get { return ShadowRadiusInner; }
            set { ShadowRadiusInner = value; }
        }
        private static eeprom_var_int16 HotSpotOuterRingSize;// 2 byte to save image HotSpot outer radius

        public eeprom_var_int16 _HotSpotOuterRingSize
        {
            get { return HotSpotOuterRingSize; }
            set { HotSpotOuterRingSize = value; }
        }
        private static eeprom_var_int16 HotSpotRadiusInner;// 2 byte to save image HotSpot inner radius

        public eeprom_var_int16 _HotSpotRadiusInner
        {
            get { return HotSpotRadiusInner; }
            set { HotSpotRadiusInner = value; }
        }


        private static eeprom_var_byte ShadowValueR;// 1 byte to save image Shadow Value R channel

        public eeprom_var_byte _ShadowValueR
        {
            get { return ShadowValueR; }
            set { ShadowValueR = value; }
        }
        private static eeprom_var_byte ShadowValueG;// 1 byte to save image Shadow Value G channel

        public eeprom_var_byte _ShadowValueG
        {
            get { return ShadowValueG; }
            set { ShadowValueG = value; }
        }
        private static eeprom_var_byte ShadowValueB;// 1 byte to save image Shadow Value B channel

        public eeprom_var_byte _ShadowValueB
        {
            get { return ShadowValueB; }
            set { ShadowValueB = value; }
        }

        private static eeprom_var_byte HotSpotValueR;// 1 byte to save image HotSpot Value R channel

        public eeprom_var_byte _HotSpotValueR
        {
            get { return HotSpotValueR; }
            set { HotSpotValueR = value; }
        }
        private static eeprom_var_byte HotSpotValueG;// 1 byte to save image HotSpot Value G channel

        public eeprom_var_byte _HotSpotValueG
        {
            get { return HotSpotValueG; }
            set { HotSpotValueG = value; }
        }
        private static eeprom_var_byte HotSpotValueB;// 1 byte to save image  HotSpot Value B channel

        public eeprom_var_byte _HotSpotValueB
        {
            get { return HotSpotValueB; }
            set { HotSpotValueB = value; }
        }

        #endregion

        public HotSpotCorrectionSettings()
        {
            preset_live_Gain = new eeprom_var_int16(0, 5000, 150);
            preset_capture_Gain = new eeprom_var_int16(0, 5000, 150);
            preset_live_exposure = new eeprom_var_int16(0, 3000, 775);
            preset_capture_exposrue = new eeprom_var_int16(0, 3000, 775);
            GainSlope = new eeprom_var_byte(0, 255, 5);
            ShadowOuterRingSize = new eeprom_var_int16(0, 5000, 300);
            ShadowRadiusInner = new eeprom_var_int16(0, 5000, 300);
            HotSpotOuterRingSize = new eeprom_var_int16(0, 5000, 300);
            HotSpotRadiusInner = new eeprom_var_int16(0, 5000, 300);
            ShadowValueB = new eeprom_var_byte(0, 100, 7);
            ShadowValueG = new eeprom_var_byte(0, 100, 20);
            ShadowValueR = new eeprom_var_byte(0, 100, 20);
            HotSpotValueB = new eeprom_var_byte(0, 100, 3);
            HotSpotValueG = new eeprom_var_byte(0, 100, 5);
            HotSpotValueR = new eeprom_var_byte(0, 100, 10);

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class ColorCorrectionSettings_EERPOM
    {

    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class ColorCorrectionSettings  //EEPROM Page 7
    {
        #region Color Correction Settings

        private static eeprom_var_float RRCompensation; // 4 bytes of floating point value to save Red in Red channel color correction

        public eeprom_var_float _RRCompensation
        {
            get { return RRCompensation; }
            set { RRCompensation = value; }
        }
        private static eeprom_var_float RGCompensation;// 4 bytes of floating point value to save Red in Green channel color correction

        public eeprom_var_float _RGCompensation
        {
            get { return RGCompensation; }
            set { RGCompensation = value; }
        }
        private static eeprom_var_float RBCompensation;// 4 bytes of floating point value to save Red in Blue channel color correction

        public eeprom_var_float _RBCompensation
        {
            get { return RBCompensation; }
            set { RBCompensation = value; }
        }
        private static eeprom_var_float GRCompensation;// 4 bytes of floating point value to save Red in Red Green channel color correction

        public eeprom_var_float _GRCompensation
        {
            get { return GRCompensation; }
            set { GRCompensation = value; }
        }
        private static eeprom_var_float GGCompensation;// 4 bytes of floating point value to save Green in Green channel color correction

        public eeprom_var_float _GGCompensation
        {
            get { return GGCompensation; }
            set { GGCompensation = value; }
        }
        private static eeprom_var_float GBCompensation;// 4 bytes of floating point value to save Green in Blue channel color correction

        public eeprom_var_float _GBCompensation
        {
            get { return GBCompensation; }
            set { GBCompensation = value; }
        }
        private static eeprom_var_float BRCompensation;// 4 bytes of floating point value to save Blue in Red channel color correction

        public eeprom_var_float _BRCompensation
        {
            get { return BRCompensation; }
            set { BRCompensation = value; }
        }
        private static eeprom_var_float BGCompensation;// 4 bytes of floating point value to save Blue in Green channel color correction

        public eeprom_var_float _BGCompensation
        {
            get { return BGCompensation; }
            set { BGCompensation = value; }
        }
        private static eeprom_var_float BBCompensation;// 4 bytes of floating point value to save Blue in Blue channel color correction

        public eeprom_var_float _BBCompensation
        {
            get { return BBCompensation; }
            set { BBCompensation = value; }
        }
        #endregion
        public ColorCorrectionSettings()
        {
            _RRCompensation = new eeprom_var_float(0, 10, 1);
            _RGCompensation = new eeprom_var_float(0, 10, 0.4f);
            _RBCompensation = new eeprom_var_float(0, 10, 0);
            _GRCompensation = new eeprom_var_float(0, 10, 0);
            _GBCompensation = new eeprom_var_float(0, 10, 0);
            _GGCompensation = new eeprom_var_float(0, 10, 1);
            _BRCompensation = new eeprom_var_float(0, 10, 0);
            _BGCompensation = new eeprom_var_float(0, 10, 0.2f);
            _BBCompensation = new eeprom_var_float(0, 10, 1.5f);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class DeviceDetailedStatus // not for EEPROM
    {

        private static eeprom_var_int32 _Motor_Current_Position; // current position of the Motor

        public eeprom_var_int32 Motor_Current_Position
        {
            get { return _Motor_Current_Position; }
            set { _Motor_Current_Position = value; }
        }
        private static eeprom_var_int16 _Device_IR_Current; // read back value of IR current Through ADCs

        public eeprom_var_int16 Device_IR_Current
        {
            get { return _Device_IR_Current; }
            set { _Device_IR_Current = value; }
        }
        private static eeprom_var_int16 _Device_Flash_Current; // read back value of IR current Through ADCs

        public eeprom_var_int16 Device_Flash_Current
        {
            get { return _Device_Flash_Current; }
            set { _Device_Flash_Current = value; }
        }
        private static eeprom_var_byte _Frame_Rate; // keeps count of strobe events per second

        public eeprom_var_byte Frame_Rate
        {
            get { return _Frame_Rate; }
            set { _Frame_Rate = value; }
        }

        public DeviceDetailedStatus()
        {
            Motor_Current_Position = new eeprom_var_int32(0, 500, 10);
            Device_IR_Current = new eeprom_var_int16(0, 500, 10);
            Device_Flash_Current = new eeprom_var_int16(0, 500, 10);
            Frame_Rate = new eeprom_var_byte(0, 255, 10);
        }
    }
    public unsafe struct EEPROM_Versrion_Struct
    {
        public byte Bytes_To_Read_Write;
        public fixed byte EEPROM_Version_Number[5];
        public fixed byte DateModified[10];

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [Serializable]
    public unsafe class EEPROM_Version// The data is saved in pages 199 and 200
    {

        private static eepromVar _EEPROM_Version_Number;//X.X.X

        public eepromVar EEPROM_Version_Number
        {
            get { return EEPROM_Version._EEPROM_Version_Number; }
            set { EEPROM_Version._EEPROM_Version_Number = value; }
        }
        private static eepromVar _DateModified; // dd/mm/yyyy

        public eepromVar DateModified
        {
            get { return EEPROM_Version._DateModified; }
            set { EEPROM_Version._DateModified = value; }
        }

        private static eeprom_var_byte _Bytes_To_Read_Write;// x bytes to be read

        public eeprom_var_byte Bytes_To_Read_Write
        {
            get { return EEPROM_Version._Bytes_To_Read_Write; }
            set { EEPROM_Version._Bytes_To_Read_Write = value; }
        }

        private static eeprom_var_byte _Start_Page_Number;// 'N' Page Number

        public eeprom_var_byte Start_Page_Number
        {
            get { return EEPROM_Version._Start_Page_Number; }
            set { EEPROM_Version._Start_Page_Number = value; }
        }

        private static eepromVar _Version_Release_Date;// Version Release Date dd/mm/yyyy

        public eepromVar Version_Release_Date
        {
            get { return EEPROM_Version._Version_Release_Date; }
            set { EEPROM_Version._Version_Release_Date = value; }
        }
        private static eeprom_var_byte _Number_Of_Pages;// X number of pages

        public eeprom_var_byte Number_Of_Pages
        {
            get { return EEPROM_Version._Number_Of_Pages; }
            set { EEPROM_Version._Number_Of_Pages = value; }
        }


        private static EEPROM_Version _eepromVerion;

        public static EEPROM_Version GetInstance()
        {
            if (_eepromVerion == null)
            {
                _eepromVerion = new EEPROM_Version();
            }
            return _eepromVerion;
        }
        private EEPROM_Version()
        {
            EEPROM_Version_Number = new eepromVar(5);
            DateModified = new eepromVar(10);
            Bytes_To_Read_Write = new eeprom_var_byte(0, 255, 1);
        }

        public void WriteEEPROMVersion()
        {

        }


        public void ReadEEPROMVersion()
        {
            if(File.Exists("eeprom.txt"))
            File.ReadAllBytes("eeprom.txt");
            byte[] tempArr = new byte[70];
            EEPromUtils.ReadEEProm(Convert.ToByte(1), ref tempArr);

            int numberOfBytes = 0;
            FieldInfo[] fieldInf = _eepromVerion.GetType().GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);


            int indx = 0;


            for (int i = 0; i < fieldInf.Length; i++)
            {
                object obj = fieldInf[i].GetValue(_eepromVerion);
                numberOfBytes = EEPromUtils.Get_Byte_Count(obj);

                if (numberOfBytes > 0)
                {
                    byte[] SrcArr = new byte[numberOfBytes];
                    Array.Copy(IntucamBoardCommHelper.EEPROM_DATA_ARRAY, indx, SrcArr, 0, SrcArr.Length);
                    indx += numberOfBytes;
                    EEPromUtils.GetObject(ref obj, SrcArr, fieldInf[i]);
                    fieldInf[i].SetValue(_eepromVerion, obj);
                }
            }






        }
        public void ResetEEPROMVersion()
        {
            //    Bytes_To_Read_Write = 0;
            //    EEPROM_Version_Number = new byte[5];
            //    DateModified = new byte[10];
            EEPromUtils.WriteEEProm(Convert.ToByte(1), EEPromUtils.GetBytes_Iteratively(this));
        }
    }
    public static class EEPromUtils
    {
        private static int BoardCmdIterCnt = 3;

        public static void ReadEEProm(byte PageNumber, ref byte[] retArr)
        {
            IntucamBoardCommHelper.EEPROM_DATA_ARRAY = new byte[70];

            IntucamBoardCommHelper.eepromRead(PageNumber);
            //return IntucamBoardCommHelper.EEPROM_DATA_ARRAY;
        }

        public static void WriteEEProm(byte PageNumber, byte[] SrcArr)
        {
            //byte[] SrcArr = ObjectToByteArray(val);
            IntucamBoardCommHelper.EEPROM_DATA_ARRAY = new byte[70];

            Array.Copy(SrcArr, 0, IntucamBoardCommHelper.EEPROM_DATA_ARRAY, 0, SrcArr.Length);
            // IntucamBoardCommHelper.eepromWrite(PageNumber, SrcArr);

            IntucamBoardCommHelper.eepromWrite(PageNumber);
        }
        public static byte[] ObjectToByteArray(Object obj)
        {

            byte[] arr = new byte[Marshal.SizeOf(obj)];
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, arr, 0, Marshal.SizeOf(obj));
            Marshal.FreeHGlobal(ptr);

            return arr;
        }
        public static byte[] ObjectToByteArray(Object obj, Type t)
        {
            //byte[] arr = new byte[Marshal.SizeOf(t)];
            //var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            //Marshal.StructureToPtr(obj, ptr, false);
            //Marshal.Copy(ptr, arr, 0, Marshal.SizeOf(obj));
            //Marshal.FreeHGlobal(ptr);
            try
            {
                MemoryStream ms = new MemoryStream();
                XmlSerializer xmlS = new XmlSerializer((t));
                XmlTextWriter xmlTW = new XmlTextWriter(ms, Encoding.Unicode);

                xmlS.Serialize(xmlTW, obj);
                ms = (MemoryStream)xmlTW.BaseStream;
                return ms.ToArray();// arr;
            }
            catch (Exception ex)
            {

                byte[] ret = new byte[1];

                return ret;
            }

        }
        public static object GetObjectFromBytes(byte[] buffer, Type objType)
        {
            try
            {

                object obj = null;
                if ((buffer != null) && (buffer.Length > 0))
                {
                    IntPtr ptrObj = IntPtr.Zero;
                    try
                    {
                        int objSize = Marshal.SizeOf(objType);
                        if (objSize > 0)
                        {
                            if (buffer.Length < objSize)
                                throw new Exception(String.Format("Buffer smaller than needed for creation of object of type {0}", objType));
                            ptrObj = Marshal.AllocHGlobal(objSize);
                            if (ptrObj != IntPtr.Zero)
                            {
                                Marshal.Copy(buffer, 0, ptrObj, objSize);
                                try
                                {
                                    obj = (object)Marshal.PtrToStructure(ptrObj, objType);

                                }
                                catch (Exception ex)
                                {


                                }

                            }
                            else
                                throw new Exception(String.Format("Couldn't allocate memory to create object of type {0}", objType));
                        }
                    }
                    finally
                    {
                        if (ptrObj != IntPtr.Zero)
                            Marshal.FreeHGlobal(ptrObj);
                    }
                }

                return obj;
            }
            catch (Exception ex)
            {

                return new object();
            }
        }
        public static int Get_Number_Of_bytes(object obj)
        {
            FieldInfo[] fieldInf = obj.GetType().GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

            int ret_Number_Of_Bytes = 0;
            for (int i = 0; i < fieldInf.Length; i++)
            {

                switch (fieldInf[i].FieldType.Name)
                {
                    case "eeprom_var_byte":
                        {


                            eeprom_var_byte var = fieldInf[i].GetValue(obj) as eeprom_var_byte;
                            ret_Number_Of_Bytes += var.ToByteArray().Length;

                            //fieldInf[i].SetValue(null);

                            break;
                        }
                    case "eeprom_var_float":
                        {
                            eeprom_var_float var = fieldInf[i].GetValue(obj) as eeprom_var_float;
                            ret_Number_Of_Bytes += var.ToByteArray().Length;

                            break;
                        }
                    case "eeprom_var_int32":
                        {
                            eeprom_var_int32 var = fieldInf[i].GetValue(obj) as eeprom_var_int32;
                            ret_Number_Of_Bytes += var.ToByteArray().Length;


                            break;
                        }
                    case "eeprom_var_int16":
                        {
                            eeprom_var_int16 var = fieldInf[i].GetValue(obj) as eeprom_var_int16;
                            ret_Number_Of_Bytes += var.ToByteArray().Length;


                            break;
                        }
                    case "eepromVar":
                        {
                            eepromVar var = fieldInf[i].GetValue(obj) as eepromVar;
                            ret_Number_Of_Bytes += var.value.Length;

                            break;
                        }

                }

            }
            return ret_Number_Of_Bytes;
        }
        public static int Get_Byte_Count(object obj)
        {
            string[] res = obj.GetType().ToString().Split('.');
            string compareStr = res[res.Length - 1];
            switch (compareStr)
            {
                case "eeprom_var_byte":
                    {
                        eeprom_var_byte var = (obj) as eeprom_var_byte;
                        return var.ToByteArray().Length;
                    }
                case "eeprom_var_float":
                    {
                        eeprom_var_float var = (obj) as eeprom_var_float;
                        return var.ToByteArray().Length;
                    }
                case "eeprom_var_int32":
                    {
                        eeprom_var_int32 var = (obj) as eeprom_var_int32;
                        return var.ToByteArray().Length;
                    }
                case "eeprom_var_int16":
                    {
                        eeprom_var_int16 var = (obj) as eeprom_var_int16;
                        return var.ToByteArray().Length;
                    }
                case "eepromVar":
                    {
                        eepromVar var = (obj) as eepromVar;
                        return var.value.Length;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        public static void SetValue(KeyValuePair<string, object> f, string val, ref object value)
        {
            string[] res = f.Value.GetType().ToString().Split('.');
            string compareStr = res[res.Length - 1];
            switch (compareStr)
            {
                case "eeprom_var_byte":
                    {
                        eeprom_var_byte var = f.Value as eeprom_var_byte;
                        var.value = Convert.ToByte(val);
                        // f.Value = var;
                        value = var;
                        // var = f.Value as eeprom_var_byte;
                        break;
                    }
                case "eeprom_var_float":
                    {
                        eeprom_var_float var = f.Value as eeprom_var_float;
                        // eeprom_var_float var = f.GetValue(obj) as eeprom_var_float;
                        var.value = Convert.ToSingle(val);
                        value = var;

                        break;
                    }
                case "eeprom_var_int32":
                    {
                        eeprom_var_int32 var = f.Value as eeprom_var_int32;
                        var.value = Convert.ToInt32(val);
                        value = var;

                        //  f.Value(var, var);
                        //var = f.Value as eeprom_var_int32;
                        break;
                    }
                case "eeprom_var_int16":
                    {
                        eeprom_var_int16 var = f.Value as eeprom_var_int16;
                        var.value = Convert.ToInt16(val);
                        value = var;

                        // f.SetValue(var, var);
                        // var = f.GetValue(obj) as eeprom_var_int16;
                        break;
                    }
                case "eepromVar":
                    {
                        eepromVar var = f.Value as eepromVar;
                        val = val.Trim('\0');
                        byte[] setVal = Encoding.UTF8.GetBytes(val);
                        for (int i = 0; i < var.value.Length; i++)
                        {
                            var.value[i] = byte.MinValue;
                        }
                        if (setVal.Length < var.value.Length)

                            Array.Copy(setVal, 0, var.value, 0, setVal.Length);
                        else
                            Array.Copy(setVal, 0, var.value, 0, var.value.Length);


                        value = var;


                        //f.SetValue(EEPROM.GetInstance(), var);

                        var = value as eepromVar;

                        string str = Encoding.UTF8.GetString(var.value);
                        break;
                    }
            }
        }
        public static void GetObject_Iteratively(ref object obj, byte[] srcByteArr)
        {
            int indx = 0;
            int length = 0;
            FieldInfo[] fieldInf = obj.GetType().GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

            for (int i = 0; i < fieldInf.Length; i++)
            {

                {
                    switch (fieldInf[i].FieldType.Name)
                    {
                        case "eeprom_var_byte":
                            {


                                eeprom_var_byte var = fieldInf[i].GetValue(obj) as eeprom_var_byte;
                                byte[] byteArr = var.ToByteArray();
                                length = byteArr.Length;
                                Array.Copy(srcByteArr, indx, byteArr, 0, byteArr.Length);
                                var = (eeprom_var_byte)EEPromUtils.GetObjectFromBytes(byteArr, typeof(eeprom_var_byte));
                                fieldInf[i].SetValue(var, var);
                                //fieldInf[i].SetValue(var, var.min);
                                //fieldInf[i].SetValue(var, var.max);

                                break;
                            }
                        case "eeprom_var_float":
                            {
                                eeprom_var_float var = fieldInf[i].GetValue(obj) as eeprom_var_float;
                                byte[] byteArr = var.ToByteArray();
                                length = byteArr.Length;

                                Array.Copy(srcByteArr, indx, byteArr, 0, byteArr.Length);
                                var = (eeprom_var_float)EEPromUtils.GetObjectFromBytes(byteArr, typeof(eeprom_var_float));

                                try
                                {
                                    fieldInf[i].SetValue(var, var);
                                    //fieldInf[i].SetValue(var, var.min);
                                    //fieldInf[i].SetValue(var, var.max);
                                }
                                catch (Exception ex)
                                {
                                }

                                break;
                            }
                        case "eeprom_var_int32":
                            {
                                eeprom_var_int32 var = fieldInf[i].GetValue(obj) as eeprom_var_int32;
                                byte[] byteArr = var.ToByteArray();
                                length = byteArr.Length;

                                Array.Copy(srcByteArr, indx, byteArr, 0, byteArr.Length);
                                var = (eeprom_var_int32)EEPromUtils.GetObjectFromBytes(byteArr, typeof(eeprom_var_int32));
                                fieldInf[i].SetValue(var, var);
                                //fieldInf[i].SetValue(var, var.min);
                                //fieldInf[i].SetValue(var, var.max);
                                break;
                            }
                        case "eeprom_var_int16":
                            {
                                eeprom_var_int16 var = fieldInf[i].GetValue(obj) as eeprom_var_int16;
                                byte[] byteArr = var.ToByteArray();
                                length = byteArr.Length;

                                Array.Copy(srcByteArr, indx, byteArr, 0, byteArr.Length);
                                var = (eeprom_var_int16)EEPromUtils.GetObjectFromBytes(byteArr, typeof(eeprom_var_int16));
                                fieldInf[i].SetValue(var, var);
                                //fieldInf[i].SetValue(var, var.min);
                                //fieldInf[i].SetValue(var, var.max);
                                break;
                            }
                        case "eepromVar":
                            {
                                eepromVar var = fieldInf[i].GetValue(obj) as eepromVar;
                                byte[] byteArr = var.value;
                                length = byteArr.Length;

                                Array.Copy(srcByteArr, indx, var.value, 0, var.value.Length);
                                fieldInf[i].SetValue(var, var);

                                break;
                            }
                    }
                    indx += length;

                }

            }
        }

        public static void GetObject(ref object obj, byte[] srcByteArr, FieldInfo fieldInf)
        {
            int indx = 0;
            int length = 0;

            string[] res = obj.GetType().ToString().Split('.');
            string compareStr = res[res.Length - 1];
            switch (compareStr)
            {
                case "eeprom_var_byte":
                    {


                        eeprom_var_byte var = fieldInf.GetValue(obj) as eeprom_var_byte;
                        byte[] byteArr = var.ToByteArray();
                        length = byteArr.Length;
                        Array.Copy(srcByteArr, indx, byteArr, 0, byteArr.Length);
                        var = (eeprom_var_byte)EEPromUtils.GetObjectFromBytes(byteArr, typeof(eeprom_var_byte));
                        fieldInf.SetValue(var, var);
                        //fieldInf[i].SetValue(var, var.min);
                        //fieldInf[i].SetValue(var, var.max);

                        break;
                    }
                case "eeprom_var_float":
                    {
                        eeprom_var_float var = fieldInf.GetValue(obj) as eeprom_var_float;
                        byte[] byteArr = var.ToByteArray();
                        length = byteArr.Length;

                        Array.Copy(srcByteArr, indx, byteArr, 0, byteArr.Length);
                        var = (eeprom_var_float)EEPromUtils.GetObjectFromBytes(byteArr, typeof(eeprom_var_float));

                        try
                        {
                            fieldInf.SetValue(var, var);
                            //fieldInf[i].SetValue(var, var.min);
                            //fieldInf[i].SetValue(var, var.max);
                        }
                        catch (Exception ex)
                        {

                        }

                        break;
                    }
                case "eeprom_var_int32":
                    {
                        eeprom_var_int32 var = fieldInf.GetValue(obj) as eeprom_var_int32;
                        byte[] byteArr = var.ToByteArray();
                        length = byteArr.Length;

                        Array.Copy(srcByteArr, indx, byteArr, 0, byteArr.Length);
                        var = (eeprom_var_int32)EEPromUtils.GetObjectFromBytes(byteArr, typeof(eeprom_var_int32));
                        fieldInf.SetValue(var, var);
                        //fieldInf[i].SetValue(var, var.min);
                        //fieldInf[i].SetValue(var, var.max);
                        break;
                    }
                case "eeprom_var_int16":
                    {
                        eeprom_var_int16 var = fieldInf.GetValue(obj) as eeprom_var_int16;
                        byte[] byteArr = var.ToByteArray();
                        length = byteArr.Length;

                        Array.Copy(srcByteArr, indx, byteArr, 0, byteArr.Length);
                        var = (eeprom_var_int16)EEPromUtils.GetObjectFromBytes(byteArr, typeof(eeprom_var_int16));
                        fieldInf.SetValue(var, var);
                        //fieldInf[i].SetValue(var, var.min);
                        //fieldInf[i].SetValue(var, var.max);
                        break;
                    }
                case "eepromVar":
                    {
                        eepromVar var = fieldInf.GetValue(obj) as eepromVar;
                        byte[] byteArr = var.value;
                        length = byteArr.Length;

                        Array.Copy(srcByteArr, indx, var.value, 0, var.value.Length);
                        fieldInf.SetValue(var, var);

                        break;
                    }
            }
            indx += length;

        }

        public static byte[] GetBytes_Iteratively(object obj)
        {
            FieldInfo[] fieldInf = obj.GetType().GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            List<byte> tempbyteList = new List<byte>();
            for (int i = 0; i < fieldInf.Length; i++)
            {
                switch (fieldInf[i].FieldType.Name)
                {
                    case "eeprom_var_byte":
                        {
                            eeprom_var_byte var = fieldInf[i].GetValue(obj) as eeprom_var_byte;
                            byte[] byteArr = var.ToByteArray();
                            for (int j = 0; j < byteArr.Length; j++)
                            {
                                tempbyteList.Add(byteArr[j]);
                            }
                            break;
                        }
                    case "eeprom_var_float":
                        {
                            eeprom_var_float var = fieldInf[i].GetValue(obj) as eeprom_var_float;
                            byte[] byteArr = var.ToByteArray();
                            for (int j = 0; j < byteArr.Length; j++)
                            {
                                tempbyteList.Add(byteArr[j]);
                            }
                            break;
                        }
                    case "eeprom_var_int32":
                        {
                            eeprom_var_int32 var = fieldInf[i].GetValue(obj) as eeprom_var_int32;
                            byte[] byteArr = var.ToByteArray();
                            for (int j = 0; j < byteArr.Length; j++)
                            {
                                tempbyteList.Add(byteArr[j]);
                            }
                            break;
                        }
                    case "eeprom_var_int16":
                        {
                            eeprom_var_int16 var = fieldInf[i].GetValue(obj) as eeprom_var_int16;
                            byte[] byteArr = var.ToByteArray();
                            for (int j = 0; j < byteArr.Length; j++)
                            {
                                tempbyteList.Add(byteArr[j]);
                            }
                            break;
                        }
                    case "eepromVar":
                        {
                            eepromVar var = fieldInf[i].GetValue(obj) as eepromVar;
                            byte[] byteArr = var.ToByteArray();
                            for (int j = 0; j < byteArr.Length; j++)
                            {
                                tempbyteList.Add(byteArr[j]);
                            }
                            break;
                        }
                }


            }
            return tempbyteList.ToArray();
        }


        public static byte[] GetBytes(KeyValuePair<string, object> obj)
        {
            string[] res = obj.Value.GetType().ToString().Split('.');
            string compareStr = res[res.Length - 1];
            switch (compareStr)
            {
                case "eeprom_var_byte":
                    {
                        eeprom_var_byte var = obj.Value as eeprom_var_byte;
                        return var.ToByteArray();

                    }
                case "eeprom_var_float":
                    {
                        eeprom_var_float var = obj.Value as eeprom_var_float;
                        return var.ToByteArray();
                    }
                case "eeprom_var_int32":
                    {
                        eeprom_var_int32 var = obj.Value as eeprom_var_int32;
                        return var.ToByteArray();
                    }
                case "eeprom_var_int16":
                    {
                        eeprom_var_int16 var = obj.Value as eeprom_var_int16;
                        return var.ToByteArray();
                    }
                case "eepromVar":
                    {
                        eepromVar var = obj.Value as eepromVar;
                        return var.ToByteArray();
                    }
                default:
                    return new byte[1];
            }


        }
    }
}