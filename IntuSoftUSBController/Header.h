#pragma once
#include "libusb.h"
#include <stdio.h>
#include <string.h>
#include <cstdlib>
#include<cstdlib>
#include<cstdio>
#include<iostream>
using namespace std;


extern "C" __declspec(dllexport)bool IsBoardPresent();

extern "C" __declspec(dllexport)void USBOpen(bool* IsSuccess, int* ErrCode);

extern "C" __declspec(dllexport) void USBClose();

extern "C" __declspec(dllexport) void USB_Read(uint8_t ReadData[], bool* IsSuccess, int* ReadErrorCode, unsigned int timeout);

extern "C" __declspec(dllexport) void USB_Read_Write(uint8_t WriteData[], uint8_t ReadData[], bool* ReadIsSuccess, bool* WriteIsSucess, int* ErrCode, int* ReadErrorCode, int* WriteErrorCode, uint8_t DataArray[], int DataLength, unsigned int readTimeout, unsigned int writeTimeout);

extern "C" __declspec(dllexport) void USB_Read_Write4(uint8_t WriteData[], uint8_t ReadData[], bool* ReadIsSuccess, bool* WriteIsSuccess, int* ErrCode, int* ReadErrCode, int* WriteErrCode, unsigned int readTimeout, unsigned int writeTimeout);

extern "C" __declspec(dllexport) void USBControllerInit();

void USB_Init();

extern "C" __declspec(dllexport) void USBReadInterrupt(bool* IsSuccess, int* ErrCode, int* intrCode, unsigned char intrData[], unsigned int interruptTimeout);

extern "C" __declspec(dllexport) void USBIsOpen(bool* isUsb);

extern  "C" __declspec(dllexport) void IS_USB_Device_Present(bool* isDevicePresent, uint16_t* pids, uint16_t* vids);

extern "C" __declspec(dllexport) void USB_Write(uint8_t WriteData[], bool* IsSuccess, int* WriteErrorCode, unsigned int timeout);

