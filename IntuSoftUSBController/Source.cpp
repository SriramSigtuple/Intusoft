/* Header file for LibUSB functionalit */
#include "Header.h"

/* Device vendor and product Id */
#define MY_VID 0x0456
#define MY_PID 0x0512

/* Number of bytes to be Sent or Received */
#define NUMBER_BYTES_TO_SEND_RECEIVE 20

/* Number of Interrupt bytes to Receive */
#define NUMBER_OF_INTR_BYTES 11

/***************** LIBUSB Variable Declarations ******************/

/* To Get the handle to the usb device */
libusb_device_handle *handle; 
libusb_device *dev; 
libusb_device_handle *Test_Handle;

/* interrupt data from the Device */
unsigned char intr_data[70];
/* 20ms time out for reading and writing to the device */

/* Data length Received/Sent */
int dataLength;

/* EEPROM Data */
uint8_t EEPROM_Data[70];
/* EndPoint Address declaration */
/*
0x01 = For Writing data
0x82 = For Reading data
0x83 = For Interrupt messages
*/
unsigned char EndPoint_Address[3]={0x01,0x82,0x83};

/* USB initialization flag */
bool USB_init_Done=false;

/* IS Open Bool TO Disable un-nessary calls */
bool isOpen = false;

/* Enum to determine the interrupt that has occurred */
enum InterruptCode{ 
	TriggerPressed=1,
	MotorFastForward,
	MotorFastBackward,
	FlashOnDone,
	FlashOffDone,
	RotaryDone,
	MotorResetDone,
	PotIntensityChanged,
    LR_Event,
	Camera_Arrived,
	Camera_Removed,
	PowerRemoved,
	OtherCommands,
	Timeout,
	Halted,
	DeviceRemoved,
	Overflow
};

/* Loop and Status Variables */
int r,j;
/***************** End of variable declarations ******************/



/******************* Functions Implementation ******************/



/* For opening the USB Device */
/*
ErrCode = 0 -> Connected
ErrCode = -1 -> Not Connected
*/
extern "C" __declspec(dllexport)bool IsBoardPresent()
{
	bool returnValue = false;
	libusb_device_handle* handle = NULL;


	//if(!USB_init_Done) // get the handle if the libusb functions are initialized by sriram on 1st september 2015
	//{
	//	/* Get a Handle to control the device */ 
	//	USB_Init();
	//	
	//	
	//}
	handle = libusb_open_device_with_vid_pid(NULL, MY_VID, MY_PID);
	/* If USB Device is not found */
	if (handle == NULL || handle == 0)
	{
		returnValue = false;
	}
	else
	{
		returnValue = true;
	}
	handle = NULL;
	return returnValue;
}

extern "C" __declspec(dllexport) void USBOpen(bool *IsSuccess, int *ErrCode)
{
	isOpen = false;
	handle=NULL;


	//if(!USB_init_Done) // get the handle if the libusb functions are initialized by sriram on 1st september 2015
	//{
	//	/* Get a Handle to control the device */ 
	//	USB_Init();
	//	
	//	
	//}
	handle = libusb_open_device_with_vid_pid(NULL, MY_VID, MY_PID);
	/* If USB Device is not found */
	if (handle == NULL || handle == 0)
	{
		*IsSuccess=false;
		/* Unsucessful */
		*ErrCode=-1; 
		return;
	}

	/* If USB handle is Acheived get the device */
	dev = libusb_get_device(handle);

	/* Claim the interface with the usb device */
	libusb_claim_interface(handle,0x00);

	/* The Device is Connected.... */

	*IsSuccess=true;
	/* Succesfully Connected */
	*ErrCode=0; 

	isOpen =true;
}


/* For Closing the USB Device */
extern "C" __declspec(dllexport) void USBClose()
{

	/* Release the Handle to the Device */ 
	if(handle!=NULL)
	libusb_release_interface(handle,0x00);
	/* To close the device*/
	if(handle!=NULL)
		libusb_close(handle);
	handle = NULL;
	//USB_init_Done = false;
	isOpen = false;
}


/* For Checking if the Device is Still Connected */
extern "C" __declspec(dllexport) void USBIsOpen(bool* isUsb)
{

	//if(USB_init_Done)
	//{
	//	/* Get a Handle to control the device */ 
	//	Test_Handle = libusb_open_device_with_vid_pid(NULL, MY_VID, MY_PID);
	//}
	//
	/* Test If Connected */
	
			//else 
			//{
			//	//handle = Test_Handle;
			//	/* Device Connected */
			//	//isOpen = true;
			//	isOpen =true;
			//}
	        if(handle==NULL || !USB_init_Done || handle == 0)
			{
				/* Device Connection Lost */
				int errCode = 0;
				isOpen = false;
			}
			else
			{
				isOpen = true;
			}

			*isUsb = isOpen;
}
extern  "C" __declspec(dllexport) void IS_USB_Device_Present(bool * isDevicePresent,uint16_t* pids,uint16_t* vids)
{

	libusb_device **devs;
	libusb_device_descriptor desc;
	int32_t cnt = libusb_get_device_list(NULL,&devs);
	pids = new uint16_t[cnt];
	vids = new uint16_t[cnt];
	for (int i = 0; i < cnt; i++)
	{
		int r = libusb_get_device_descriptor(devs[i], &desc);
		pids[i] = desc.idProduct;
		vids[i]=desc.idVendor;

		if(r > 0 && desc.idProduct == MY_PID && desc.idVendor == MY_VID)
		{
			*isDevicePresent = true;
			
		}
	}
}

/* For Reading / Writing to the USB Device */
/*
ReadWrite = 1 -> Write Data
ReadWrite = 0 -> Read Data
ErrCode = 0 -> Succesfully Completed
ErrCode = -1 -> Unsucessful
*/

 extern "C" __declspec(dllexport) void USB_Write(uint8_t WriteData[], bool *IsSuccess ,int *WriteErrorCode, unsigned int timeout)
{
	if(isOpen && handle != 0)
		//if(DataLength<=20)
		{

			/* Writing Data */
			j = libusb_bulk_transfer(handle, EndPoint_Address[0],WriteData,NUMBER_BYTES_TO_SEND_RECEIVE,&dataLength,timeout);
			*WriteErrorCode =j;
	}
	if(j == 0 )
				*IsSuccess = true;
			else
				*IsSuccess = false;


}
 // Function to read usb bulk pipe
 extern "C" __declspec(dllexport) void USB_Read( uint8_t ReadData[], bool *IsSuccess ,int *ReadErrorCode,unsigned int timeout )
{
	if(isOpen && handle != 0)
		{

			/* Reading Data */
			j = libusb_bulk_transfer(handle, EndPoint_Address[1],ReadData,NUMBER_BYTES_TO_SEND_RECEIVE,&dataLength,timeout);
			*ReadErrorCode =j ;
			//*WriteErrorCode =j;
			if(j == 0 )
				*IsSuccess = true;
			else
				*IsSuccess = false;

		}
}
extern "C" __declspec(dllexport) void USB_Read_Write(uint8_t WriteData[], uint8_t ReadData[], bool *ReadIsSuccess ,bool *WriteIsSucess, int *ErrCode ,int *ReadErrorCode,int *WriteErrorCode, uint8_t DataArray[], int DataLength,unsigned int readTimeout,unsigned int writeTimeout )
{

	if(isOpen && handle != 0)
	{

		/* Normal read write Operations */
		if(DataLength<=20)
		{

			/* Writing Data */
			j = libusb_bulk_transfer(handle, EndPoint_Address[0],WriteData,NUMBER_BYTES_TO_SEND_RECEIVE,&dataLength,writeTimeout);
			*WriteErrorCode =j;
			if(j==0)
			{
				*WriteIsSucess=true;
				/* Sucessful */
				*ErrCode=0; 
			}
			else
			{
				return;
			}


			/* Reading Data */
			j = libusb_bulk_transfer(handle, EndPoint_Address[1],ReadData,NUMBER_BYTES_TO_SEND_RECEIVE,&dataLength,readTimeout);
			*ReadErrorCode = j;

			/* Succesful Reading Done */
			if(j==0)
			{
				*ReadIsSuccess = true;
				/* Sucessful */
				*ErrCode=0; 
			}
			

		}

		else if(DataLength==64)
		{
			if(!(strncmp((char*)WriteData,"EEWR",4)))
			{
				/* For writing the data to EEPROM */
				strncpy_s((char*)EEPROM_Data,65,(char*)WriteData,5);
				EEPROM_Data[5]=0x01;

				/* Copying the data from the dataArray to EEPROM data */
				for(int i=0; i<32; i++)
				{
					EEPROM_Data[6+i]=DataArray[i];

				}

				//strncpy_s((char*)(EEPROM_Data+6),65,(char*)DataArray,32); // older implementation, removed coz it was not performing with null characters.




				/* Writing Data */
				//j = libusb_bulk_transfer(handle, EndPoint_Address[0],EEPROM_Data,37,&dataLength,timeout);
				j = libusb_bulk_transfer(handle, EndPoint_Address[0],EEPROM_Data,38,&dataLength,writeTimeout);

				/* Reading Data */
				j = libusb_bulk_transfer(handle, EndPoint_Address[1],ReadData,NUMBER_BYTES_TO_SEND_RECEIVE,&dataLength,readTimeout);

				if(!(strncmp((char*)ReadData,"Part 1 received",15)))
				{
					// send command to write second 32 bytes to the EEPROM //
					// EEWR - page number - data 32 byte //
					strncpy_s((char*)EEPROM_Data,65,(char*)WriteData,5);
					EEPROM_Data[5]=0x02;

					/* Copying the data from the dataArray to EEPROM data */
					for(int j=0; j<32; j++)
					{
						EEPROM_Data[6+j]=DataArray[32+j];

					}

					//strncpy_s((char*)(EEPROM_Data+6),65,(char*)(DataArray+32),32);

					/* Writing Data */
					j = libusb_bulk_transfer(handle, EndPoint_Address[0],EEPROM_Data,38,&dataLength,writeTimeout);

					/* Reading Data */
					j = libusb_bulk_transfer(handle, EndPoint_Address[1],EEPROM_Data,64,&dataLength,readTimeout);

					/*if(!(strncmp((char*)ReadData,(char*)DataArray,64)))
					{
						*ErrCode=0;
						*IsSuccess=true;
					}

					else
					{
						*ErrCode=-1;
						*IsSuccess=false;
						return;
					}*/
				}
			}

			else
			{
				/* For copying the Data command  to the  EEPROM data pointer*/
				strncpy_s((char*)EEPROM_Data,65,(char*)WriteData,10);

				/* Writing Data command */
				j = libusb_bulk_transfer(handle, EndPoint_Address[0],EEPROM_Data,NUMBER_BYTES_TO_SEND_RECEIVE,&dataLength,writeTimeout);

				/* Reading Data from the EEPROM via controller */
				j = libusb_bulk_transfer(handle, EndPoint_Address[1],DataArray,64,&dataLength,readTimeout);

				//strncpy_s((char*)(EEPROM_Data+5),65,(char*)DataArray,32);
				//DataArray=&EEPROM_Data[0];

				/* Succesful Reading Done */
				if(j==0)
				{
					//*IsSuccess=true;
					/* Sucessful */
					*ErrCode=0; 
					return;
				}

				/* Unsucessful Read */
				else
				{
					//*IsSuccess=false;
					/* Unsucessful */
					*ErrCode=-1; 
					return;
				}

			}
		}

		else
		{
			/* Unsuccesfull */
			*ErrCode=-1;
		}

	}
	else 
	{
		if(handle ==0)
		{
			*ErrCode = -2;
			isOpen = false;
		}
		}
}
extern "C" __declspec(dllexport) void USB_Read_Write4(uint8_t WriteData[], uint8_t ReadData[], bool *ReadIsSuccess, bool *WriteIsSuccess, int *ErrCode ,int *ReadErrCode,int*WriteErrCode,unsigned int readTimeout, unsigned int writeTimeout)
{
	uint8_t DataArray[] ={0,0,0};
	int DataLength =20;

	USB_Read_Write( WriteData,  ReadData, ReadIsSuccess, WriteIsSuccess,  ErrCode, ReadErrCode,WriteErrCode,  DataArray, DataLength,readTimeout,writeTimeout);
}



extern "C" __declspec(dllexport) void USBControllerInit()
{
	USB_Init();
}

void USB_Init()
{
	r = -1;
	// Load all functions of libusb to be used this function needs to be called before controller connection is established by sriram on 1st september 2015
	r = libusb_init(NULL);
	libusb_set_debug(NULL,4);
	if(r == 0)
		USB_init_Done = true;
	else 
		USB_init_Done = false;
}

extern "C" __declspec(dllexport) void USBReadInterrupt(bool *IsSuccess, int *ErrCode, int *intrCode,unsigned char intrData[],unsigned int interruptTimeout)
{
	if(!isOpen)
		return;
	if(handle == 0x0000000000000000)
		return;
	if(isOpen && handle != 0x0000000000000000  && handle != NULL)
	{
		j = libusb_interrupt_transfer(handle, EndPoint_Address[2],intr_data,NUMBER_OF_INTR_BYTES,&dataLength,interruptTimeout);

		/* Succesful Interrupt Received */
		if( j == 0 && dataLength > 0  )
		{
			*IsSuccess=true;
			/* Sucessful */
			*ErrCode=0; 
		     
			
			memcpy( (void*)intrData, (void*)intr_data, sizeof(intr_data) );
			/* Determine Which Interrupt Occured */
			/* Trigger Pressed */
			if(!(strncmp((char*)intr_data,"TRGP",4)))
			{
				*intrCode=TriggerPressed;
			}
			/* Motor Fast Forward Done*/
			else if(!(strncmp((char*)intr_data,"MFWD",4)))
			{
				*intrCode=MotorFastForward;
			}
			/* Motor Fast Backward Done*/
			else if(!(strncmp((char*)intr_data,"MBWD",4)))
			{
				*intrCode=MotorFastBackward;
			}
			/*  Flash on done interrupt */
			else if(!(strncmp((char*)intr_data,"FODN",4)))// 
			{
				*intrCode=FlashOnDone;
			}

			/*  Flash off done interrupt */
			else if(!(strncmp((char*)intr_data,"FFDN",4)))
			{
				*intrCode=FlashOffDone;
			}
			/* Rotary movement done interrupt */
			else if(!(strncmp((char*)intr_data,"ROTD",4)))
			{
				*intrCode=RotaryDone;
			}
			else if(!(strncmp((char*)intr_data,"MRTD",4)))
			{
				*intrCode=MotorResetDone;
			}
			else if(!(strncmp((char*)intr_data,"POTC",4)))
			{
				*intrCode=PotIntensityChanged;
			}
			else if(!(strncmp((char*)intr_data,"CAMC",4)))
			{
				*intrCode=Camera_Arrived;
			}
			else if(!(strncmp((char*)intr_data,"CAMD",4)))
			{
				*intrCode = Camera_Removed;
			}
			else if(!(strncmp((char*)intr_data,"LR",2)))
			{
				*intrCode = LR_Event;
			}
			else 
			{
				*intrCode=OtherCommands;

			}
		}
		else if(j==LIBUSB_ERROR_TIMEOUT)
		{
		*IsSuccess=true;
		*intrCode = Timeout;
		}
		else if(j == LIBUSB_ERROR_IO)
	{
		handle = 0;
		USBClose();
		isOpen = false;
		*intrCode = DeviceRemoved;
		*ErrCode=-10; // Indicates USB is either not connected or powered off
		//handle = 0;
		//USBClose();
		//isOpen = false;
		//*intrCode = PowerRemoved;
		//*ErrCode=-10; // Indicates USB is either not connected or powered off

	}
		else if(j == LIBUSB_ERROR_TIMEOUT)
		{
			*intrCode = Timeout;

		}
		else if(j == LIBUSB_ERROR_NO_DEVICE)
		{
			handle = 0;
		USBClose();
		isOpen = false;
		*intrCode = DeviceRemoved;
		*ErrCode=-10; // Indicates USB is either not connected or powered off
		}
		/* Unsucessful */
		else
		{
			//*IsSuccess=false;
			/* Unsucessful */
			*ErrCode=j; 
		}
	}
}

/************ End of function definition *******/