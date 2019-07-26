#include "AssistedFocus_Globals.h"
#include "AssistedFocus_FindFocus.h"
/*

*/
extern "C" __declspec(dllexport) void AssistedFocus_Init(int AvgBrightnessRef,double covLowerLimit,double covUpperLimit,int numberOfScales , int EntryPeakPercentage,int ExitPeakPercentage,int MaxGain,int width,int height);

extern "C" __declspec(dllexport) void  AssistedFocus_GetFrameState(uchar* srcImage,int* state,int currentGain,float* sharpness);

extern "C" __declspec(dllexport)void AssistedFocus_GetDebugInfo(int* cv,int* avgB,float* curPeak,float* curFrameSharp,float* exitPeakPercentage);

extern "C" __declspec(dllexport) void AssistedFocus_Reset();

extern "C" __declspec(dllexport) void AssistedFocus_Update(int AvgBrightnessRef,double covLowerLimit,double covUpperLimit,int numberOfScales , int EntryPeakPercentage,int ExitPeakPercentage,int width,int height);

extern "C" __declspec(dllexport) void AssistedFocus_Exit();

extern "C" __declspec(dllexport) int LiveModeProcessing_ComputeGain(char* defaultGainImage,char* zeroGainImage,int defaultGain, int defaultBrightness,int expectedBrightness,int maxGain);
 