using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.InteropServices;
using System.Reflection;
namespace EEPROM
{
    public enum StrutureTypes:byte
    {
        InstallationDetailsSettings = 1,
        DeviceDetailsSettings,
        Features,
        FeatureSettings,
        FirmwareSettings,
        PostProcessingSettings,
        CameraSettings,
        CurrentVoltageSettings,
        ImageSizeSettings,
        MotorSettings,
        BrightnessContrastSettings,
        ClaheSettings,
        ColorCorrectionSettings,
        HotSpotCorrectionSettings,
        ImageShiftSettings,
        LutSettings,
        MaskSettings,
        UnsharpMaskSettings,
        VignattingSettings,
        AnteriorSettings,
        AnteriorBlueSettings,
        PosteriorSettings,
        FFASettings,
        ShadowCorrectionSettings,
        HotSpotSettings,
        CameraModeSettings,
        ExposureGainSettings,
        TemperatureTintSettings
    }
    public enum DataTypes:byte
    {
        Byte =1,SByte, Int16, Int32, Single, ByteArr, Tree,UInt16,UInt32
    }

    public enum SetValueItem:byte{
        Min = 1,Max,Value
    }
    public class EEPROM_Data_Types<T>
    {
        private T _min;

        public T Min
        {
            get { return _min; }
            set { _min = value; }
        }

        private T _max;

        public T Max
        {
            get { return _max; }
            set { _max = value; }
        }

        private T _value;

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public byte Size()
        {
            return Convert.ToByte(3 * Marshal.SizeOf(typeof(T)));

        }
        public byte[] GetBytes()
        {
            int offset = 0;
            byte[] arr = new byte[Size()];

            PropertyInfo[] pinf = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < pinf.Length; i++)
            {
                object obj = pinf[i].GetValue(this);
                byte[] tempArr = new byte[Marshal.SizeOf(obj)];
                var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
                Marshal.StructureToPtr(obj, ptr, false);
                Marshal.Copy(ptr, tempArr, 0, Marshal.SizeOf(obj));
                Marshal.FreeHGlobal(ptr);
                Array.Copy(tempArr, 0, arr, offset, tempArr.Length);
                offset += tempArr.Length;
            }
            return arr;

        }
        public EEPROM_Data_Types(T value, T min, T max )
        {
            this.Min = min;
            this.Max = max;
            this.Value = value;
        }

      

    }


    public class EEPROM_Type_Info
    {
      public  DataTypes dataType;
      public  byte length;

       
         public static EEPROM_Type_Info CreateEEPROM_TypeInfo( DataTypes EEPROM_Type,byte Length = 0
           )
        {
            return new EEPROM_Type_Info
            {
               
                length  = Length,
                dataType = EEPROM_Type
            };
        }
         private EEPROM_Type_Info()
        {

        }
    }
    public static class Convert2Byte
    {
        public static byte ToByte<T>(this T value)
        {
            return Convert.ToByte(value);
        }
    }
    public static class TypeInfo
    {
    public static byte[] GetBytesFromVal(this EEPROM_Props value)
        {
            byte length = 0;
            byte[] arr = null;
            switch (value.eepromDataType.dataType)
            {
                case DataTypes.ByteArr:
                    {

                     arr =   Encoding.UTF8.GetBytes(value.value as string);
                     if (arr.Length == 0)
                       arr = new byte [value.eepromDataType.length];
                        break;
                    }
                case DataTypes.SByte:
                    {
                        EEPROM_Data_Types<sbyte> result = value.value as EEPROM_Data_Types<sbyte>;
                        arr = result.GetBytes();
                        break;
                    }
                case DataTypes.Byte:
                    {
                        EEPROM_Data_Types<byte> result = value.value as EEPROM_Data_Types<byte>;
                        arr = result.GetBytes();
                        break;
                    }
                case DataTypes.Single:
                    {
                        EEPROM_Data_Types<float> result = value.value as EEPROM_Data_Types<float>;
                        arr = result.GetBytes();
                        break;
                    }
                case DataTypes.Int16:
                    {
                        EEPROM_Data_Types<Int16> result = value.value as EEPROM_Data_Types<Int16>;
                        arr = result.GetBytes();
                        break;
                    }
                case DataTypes.Int32:
                    {
                        EEPROM_Data_Types<Int32> result = value.value as EEPROM_Data_Types<Int32>;
                        arr = result.GetBytes();
                        break;
                    }
                case DataTypes.UInt16:
                    {
                        EEPROM_Data_Types<UInt16> result = value.value as EEPROM_Data_Types<UInt16>;
                        arr = result.GetBytes();
                        break;
                    }
                case DataTypes.UInt32:
                    {
                        EEPROM_Data_Types<UInt32> result = value.value as EEPROM_Data_Types<UInt32>;
                        arr = result.GetBytes();
                        break;
                    }
            }
            return arr;
        }

    public static void GetValFromBytes(this EEPROM_Props value,byte[] arr)
    {
        byte length = 0;
        int offset = 0;
        switch (value.eepromDataType.dataType)
        {
            case DataTypes.ByteArr:
                {

                    value.value = Encoding.UTF8.GetString(arr);
                    break;
                }
            case DataTypes.SByte:
                {
                    EEPROM_Data_Types<sbyte> result = value.value as EEPROM_Data_Types<sbyte>;
                    result.Value =(SByte) arr[2];
                    result.Min = (SByte)arr[0];
                    result.Max = (SByte)arr[1];
                    break;
                }
            case DataTypes.Byte:
                {
                    EEPROM_Data_Types<byte> result = value.value as EEPROM_Data_Types<byte>;
                    result.Value = arr[2];
                    result.Min = arr[0];
                    result.Max = arr[1];
                    break;
                }
            case DataTypes.Single:
                {
                    EEPROM_Data_Types<float> result = value.value as EEPROM_Data_Types<float>;
                    result.Min = BitConverter.ToSingle(arr,offset);
                    offset += sizeof(float);
                    result.Max = BitConverter.ToSingle(arr, offset);
                    offset += sizeof(float);

                    result.Value = BitConverter.ToSingle(arr, offset);
                    offset += sizeof(float);
                    break;
                }
            case DataTypes.Int16:
                {
                    EEPROM_Data_Types<Int16> result = value.value as EEPROM_Data_Types<Int16>;
                    result.Min = BitConverter.ToInt16(arr, offset);
                    offset += sizeof(Int16);
                    result.Max = BitConverter.ToInt16(arr, offset);
                    offset += sizeof(Int16);

                    result.Value = BitConverter.ToInt16(arr, offset);
                    offset += sizeof(Int16);
                    break;
                }
               
            case DataTypes.Int32:
                {
                    EEPROM_Data_Types<Int32> result = value.value as EEPROM_Data_Types<Int32>;
                    result.Min = BitConverter.ToInt32(arr, offset);
                    offset += sizeof(Int32);
                    result.Max = BitConverter.ToInt32(arr, offset);
                    offset += sizeof(Int32);

                    result.Value = BitConverter.ToInt32(arr, offset);
                    offset += sizeof(Int32);
                    break;
                }
            case DataTypes.UInt16:
                {
                    EEPROM_Data_Types<UInt16> result = value.value as EEPROM_Data_Types<UInt16>;
                    result.Min = BitConverter.ToUInt16(arr, offset);
                    offset += sizeof(UInt16);
                    result.Max = BitConverter.ToUInt16(arr, offset);
                    offset += sizeof(UInt16);

                    result.Value = BitConverter.ToUInt16(arr, offset);
                    offset += sizeof(UInt16);
                    break;
                }
            case DataTypes.UInt32:
                {
                    EEPROM_Data_Types<UInt32> result = value.value as EEPROM_Data_Types<UInt32>;
                    result.Min = BitConverter.ToUInt32(arr, offset);
                    offset += sizeof(UInt32);
                    result.Max = BitConverter.ToUInt32(arr, offset);
                    offset += sizeof(UInt32);

                    result.Value = BitConverter.ToUInt32(arr, offset);
                    offset += sizeof(UInt32);
                    break;
                }
        }
    }
    public static void SetEEPROM_DataType_Value(this EEPROM_Props obj, object value, SetValueItem itemEnum)
    {
        byte length = 0;
        byte[] arr = null;
       // obj.value = SetValueRMinRMax(obj.value, value, itemEnum);
        switch (obj.eepromDataType.dataType)
        {
            case DataTypes.SByte:
                {
                    EEPROM_Data_Types<sbyte> result = obj.value as EEPROM_Data_Types<sbyte>;
                    obj.value = (EEPROM_Data_Types<sbyte>)SetValueRMinRMax<sbyte>(result, Convert.ToSByte( value), itemEnum);            
                    break;
                }
            case DataTypes.Byte:
                {
                    EEPROM_Data_Types<byte> result = obj.value as EEPROM_Data_Types<byte>;
                  //  obj.value = (EEPROM_Data_Types<byte>)
                    obj.value = SetValueRMinRMax<byte>(result, Convert.ToByte(value), itemEnum);            

                    break;
                }
            case DataTypes.Single:
                {
                    EEPROM_Data_Types<float> result = obj.value as EEPROM_Data_Types<float>;
                    obj.value = SetValueRMinRMax<float>(result, Convert.ToSingle( value), itemEnum);            

                    break;
                }
            case DataTypes.Int16:
                {
                    EEPROM_Data_Types<Int16> result = obj.value as EEPROM_Data_Types<Int16>;
                    obj.value = SetValueRMinRMax<Int16>(result, Convert.ToInt16(value), itemEnum);            

                    break;
                }
            case DataTypes.Int32:
                {
                    EEPROM_Data_Types<Int32> result = obj.value as EEPROM_Data_Types<Int32>;
                    obj.value = SetValueRMinRMax<Int32>(result, Convert.ToInt32(value), itemEnum);            

                    break;

                }
            case DataTypes.UInt16:
                {
                    EEPROM_Data_Types<UInt16> result = obj.value as EEPROM_Data_Types<UInt16>;
                    obj.value = SetValueRMinRMax<UInt16>(result, Convert.ToUInt16(value), itemEnum);

                    break;

                }
            case DataTypes.UInt32:
                {
                    EEPROM_Data_Types<UInt32> result = obj.value as EEPROM_Data_Types<UInt32>;
                    obj.value = SetValueRMinRMax<UInt32>(result, Convert.ToUInt32(value), itemEnum);

                    break;

                }
        }
    }

    public static EEPROM_Data_Types<T> SetValueRMinRMax<T>(EEPROM_Data_Types<T> obj, T value, SetValueItem enumVal)
        {
            switch (enumVal)
            {
                case SetValueItem.Max:
                    {
                        obj.Max = value;
                        break;
                    }
                case SetValueItem.Min:
                    {
                        obj.Min = value;
                        break;
                    }
                case SetValueItem.Value:
                    {
                        obj.Value = value;
                        break;
                    }
            }

            return obj;
        }

       
    }
}
