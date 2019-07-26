// $URL:$
// $LastChangedBy:$ on $LastChangedDate:$

//Copyright (C) 2012, Forus Health Pvt. Ltd.
// sriram@forushealth.com
/* **********************************************************************************
* File name : AssistedFocus_FindFocus.h
*  
* This class header file is used  by the AssistedFocus project 
* Which mainly computes the frame state and returns the state. 
* Author: Sriram
* Last modifed: 
* 
*************************************************************************************
*/
#ifndef Include_FindFocus
#define Include_FindFocus
#include "AssistedFocus_Globals.h"
#include"AssistedFocus_Sharpness.h"

class AssistedFocus_FindFocus
{
	AssistedFocus_Sharpness *sharpnessObj ;
	IplImage* ASSISTEDFOCUS_greenImg;
	static enum AssistedFocusFrameStateType {ASSISTEDFOCUS_FRAME_NONUNIFORM , ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS  , ASSISTEDFOCUS_FRAME_UNIFORM_FOCUS};

	float CurrentSharpnessStrength ;
	float prevSharpnessStrength;
	float prevPrevSharpnessStrength;
	float prevPrevPrevSharpnessStrength;
	int ASSISTEDFOCUS_entryPeakPercentage;
	int ASSISTEDFOCUS_exitPeakPercentage;
	int AvgBrightnessUpperLimit;
	int AvgBrightnessLowerLimit;
	int covUpperLimit;
	int covLowerLimit;
	int PeakCount;
	int PeakThresholdLowerLimit;
	// Added by sriram to fix TS1X-1099
	public: bool isFocusEnteredAtMaxGain;
    int currentGain;
	int MaxGain;
    public:  bool isGainMax;

	struct FrameStateDetails
	{
		int frameState;
		float sharpnessStrength;
		float exitPeakStrength;
		float entryPeakStrength ;
		int currentIdx ;
		int buffer_idx ;
		float currentPeakSharpnessStrength;
		int avgBrightnessExitLowerLimit;
		int	avgBrightnessExitUpperLimit;
		int entryPeakPercentage;
		int exitPeakPercentage;
		int coeffiecientOfVariation;
	    int AvgBrightness;
		
		void  setFrameStateDetails()
		{
			frameState =0;
			sharpnessStrength=0;
			exitPeakStrength=0;
			entryPeakStrength=0;
			avgBrightnessExitLowerLimit=0;
			avgBrightnessExitUpperLimit=0;
            coeffiecientOfVariation=0;
	        AvgBrightness=0;
			currentIdx=0;
			buffer_idx=0;
			currentPeakSharpnessStrength=0;
		}
	}frameStateDetails;

	void AssistedFocus_AddToBuffer();
	void AssistedFocus_ManageState();
	void AssistedFocus_PeakUpdation();
	

	int CoefficientOfVariation(IplImage* srcImg);

	
public:
	void AssistedFocus_FindFocus_Init(int covLowerLimit,int covUpperLimit,int avgBrightnessUpperLimit,int avgBrightnessLowerLimit
		,int avgBrightnessExitLowerLimit,int avgBrightnessExitUpperLimit,int entryPeakPercentage,int exitPeakPercentge,int noOfScales,int peakThresholdLowerLimit, int width, int height);
	void AssistedFocus_FindFocus_Exit();
	void AssistedFocus_FindFocus_GetDebugInfo(int*,int*,float*,float*,float*);
	int AssistedFocus_computeFrameState(IplImage* srcImg);
	void AssistedFocus_ResetFrameDetails();
	int AssistedFocus_FindFrameUniformity(IplImage * srcImg);

};
#endif