#include"AssistedFocus_FindFocus.h"

void AssistedFocus_FindFocus::AssistedFocus_ManageState()
{

	if(	(frameStateDetails.frameState  == ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS))
	{
		frameStateDetails.entryPeakStrength = frameStateDetails.entryPeakPercentage * frameStateDetails.currentPeakSharpnessStrength / 100;
		if (frameStateDetails.currentPeakSharpnessStrength != 0 && frameStateDetails.sharpnessStrength > frameStateDetails.entryPeakStrength )
		{
			if(this->PeakCount > 5)
			{
			 frameStateDetails.exitPeakStrength = frameStateDetails.exitPeakPercentage* frameStateDetails.currentPeakSharpnessStrength / 100;
			 frameStateDetails.frameState = ASSISTEDFOCUS_FRAME_UNIFORM_FOCUS;
			}
		}
	}
	else if (frameStateDetails.frameState  == ASSISTEDFOCUS_FRAME_UNIFORM_FOCUS)
	{
		if (frameStateDetails.sharpnessStrength < frameStateDetails.exitPeakStrength)
		{
			frameStateDetails.frameState = ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS;
		}

	}

}
void AssistedFocus_FindFocus::AssistedFocus_PeakUpdation()

{
	if (frameStateDetails.buffer_idx > 3)
	{
		if (this->prevPrevPrevSharpnessStrength >0 && this->CurrentSharpnessStrength > 0)
		{
			if (this->prevPrevPrevSharpnessStrength < this->prevPrevSharpnessStrength
				&& this->prevPrevSharpnessStrength < this->prevSharpnessStrength && 
				this->prevSharpnessStrength > this->CurrentSharpnessStrength &&
				this->prevSharpnessStrength >(float)this->PeakThresholdLowerLimit)
			{
				// For first peak no averaging is done directly the peak edge strength is considered
				
				if (frameStateDetails.currentPeakSharpnessStrength == 0)
				{
					frameStateDetails.currentPeakSharpnessStrength = this->prevSharpnessStrength;
				}
				else
				{
					frameStateDetails.currentPeakSharpnessStrength = (frameStateDetails.currentPeakSharpnessStrength + this->prevSharpnessStrength) / 2;
				}
               this->PeakCount++;
			}
		}
	}

}


void AssistedFocus_FindFocus::AssistedFocus_AddToBuffer()
{
	++this->frameStateDetails.buffer_idx;

	this->prevPrevPrevSharpnessStrength = this->prevPrevSharpnessStrength;
	this->prevPrevSharpnessStrength = this->prevSharpnessStrength ;
	this->prevSharpnessStrength = this->CurrentSharpnessStrength;
	this->CurrentSharpnessStrength = frameStateDetails.sharpnessStrength;
   

}

int AssistedFocus_FindFocus:: AssistedFocus_computeFrameState(IplImage * srcImg)
{

	int frameState;
//	cvSplit(srcImg,0,ASSISTEDFOCUS_greenImg,0,0);
	//cvCopy(srcImg,ASSISTEDFOCUS_greenImg);
	//frameState = this->AssistedFocus_FindFrameUniformity(ASSISTEDFOCUS_greenImg);

	//if(frameState == ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS)
	{
		/*if(frameStateDetails.frameState == ASSISTEDFOCUS_FRAME_NONUNIFORM)
			frameStateDetails.frameState = ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS;*/
		double* t2 =0;
		double* t3 =0;
		//frameStateDetails.sharpnessStrength = sharpnessObj->AssistedFocus_Compute_FrameSharpness(ASSISTEDFOCUS_greenImg);
		frameStateDetails.sharpnessStrength = sharpnessObj->AssistedFocus_Compute_FrameSharpness(srcImg);
		/*if(frameStateDetails.sharpnessStrength <0)  
			frameStateDetails.sharpnessStrength=0;*/

		//AssistedFocus_AddToBuffer();
		////if(this->PeakCount<5)
		//AssistedFocus_PeakUpdation();
		//AssistedFocus_ManageState();

	}
	/*else
		frameStateDetails.frameState = frameState;*/
	return frameStateDetails.frameState;

}

void AssistedFocus_FindFocus::AssistedFocus_FindFocus_Exit()
{
	this->sharpnessObj->AssistedFocus_Sharpness_Exit();
	delete	this->sharpnessObj ;
	cvReleaseImage(&ASSISTEDFOCUS_greenImg);
	delete this;
}

void AssistedFocus_FindFocus:: AssistedFocus_ResetFrameDetails()
{
	this->prevPrevPrevSharpnessStrength=0;
	this->prevPrevSharpnessStrength=0;
	this->prevSharpnessStrength=0;
	this->CurrentSharpnessStrength=0;
	this->frameStateDetails.setFrameStateDetails();
	
}
void AssistedFocus_FindFocus::AssistedFocus_FindFocus_Init(int covLowerLimit,int covUpperLimit,int avgBrightnessUpperLimit,
														   int avgBrightnessLowerLimit,int avgBrightnessExitLowerLimit,int avgBrightnessExitUpperLimit,
														   int entryPeakPercentage,int exitPeakPercentage,int noOfScales,int peakThresoldLowerLimit,int width,int height)
{
	this->sharpnessObj = new AssistedFocus_Sharpness();
	this->sharpnessObj->AssistedFocus_Sharpness_Init(noOfScales);

	this->frameStateDetails.setFrameStateDetails();
	this->ASSISTEDFOCUS_greenImg = cvCreateImage(cvSize(width,height),IPL_DEPTH_8U,1);
	this->frameStateDetails.exitPeakPercentage = exitPeakPercentage;
	this->frameStateDetails.entryPeakPercentage = entryPeakPercentage;
	this->frameStateDetails.avgBrightnessExitLowerLimit = avgBrightnessExitLowerLimit;
	this->frameStateDetails.avgBrightnessExitUpperLimit = avgBrightnessExitUpperLimit;

	this->covLowerLimit = covLowerLimit;
	this->covUpperLimit = covUpperLimit;
	this->AvgBrightnessLowerLimit = avgBrightnessLowerLimit;
	this->AvgBrightnessUpperLimit = avgBrightnessUpperLimit;

    this->PeakCount=0;
	this->prevPrevPrevSharpnessStrength=0;
	this->prevPrevSharpnessStrength=0;
	this->prevSharpnessStrength=0;
	this->CurrentSharpnessStrength=0;
	this->PeakThresholdLowerLimit=peakThresoldLowerLimit;
}

void AssistedFocus_FindFocus::AssistedFocus_FindFocus_GetDebugInfo(int * cv, int *avgB, float *curPeakStrength, float *curFrameSharpness,float *exitPeakpercentage)
{
	*curPeakStrength = this->frameStateDetails.currentPeakSharpnessStrength;
	*cv = this->frameStateDetails.coeffiecientOfVariation;
	*avgB  = this->frameStateDetails.AvgBrightness;
	if(this->frameStateDetails.currentPeakSharpnessStrength!=0)
	{
		*curFrameSharpness = (this->frameStateDetails.sharpnessStrength/this->frameStateDetails.currentPeakSharpnessStrength)*100;
	}
	if(this->frameStateDetails.entryPeakStrength!=0)
		*exitPeakpercentage = this->frameStateDetails.sharpnessStrength*100/(this->frameStateDetails.entryPeakStrength);
}
int AssistedFocus_FindFocus::AssistedFocus_FindFrameUniformity(IplImage* srcImg)
{
	CvScalar sdv,MeanVal;

	// calculate Average and standard deviation of the  green channel image.
	cvAvgSdv(srcImg,&MeanVal,&sdv);

	// calculate the co-efficient of variation of the image by dividing the standard deviation by the average value of the image.
	double temp1 = (sdv.val[0]*100);
	double temp2 = MeanVal.val[0];
	temp1  = temp1/temp2;
	this->frameStateDetails.coeffiecientOfVariation =(int)(temp1);

	// Return the average gray value of the image.
	this->frameStateDetails.AvgBrightness = (int)(temp2);
	
	bool isCoefficientOfVariationInRange = frameStateDetails.coeffiecientOfVariation > this->covLowerLimit && 
		                                     frameStateDetails.coeffiecientOfVariation < this->covUpperLimit;
	
	bool isAvgBrightnessInRange = frameStateDetails.AvgBrightness > this->AvgBrightnessLowerLimit && 
		                                    frameStateDetails.AvgBrightness <this->AvgBrightnessUpperLimit;
    // Added by sriram to fix TS1X-1099
	if(currentGain == MaxGain)
	this->isGainMax = true;
	else
	this->isGainMax = false;

	if(isCoefficientOfVariationInRange && isAvgBrightnessInRange)
	{
	    isFocusEnteredAtMaxGain = false;
		return ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS;
	}
	else if(isCoefficientOfVariationInRange && this->isGainMax)
	{
		    isFocusEnteredAtMaxGain = true;
			return ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS;
	}
	
	else if(frameStateDetails.frameState == ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS||
		                            frameStateDetails.frameState == ASSISTEDFOCUS_FRAME_UNIFORM_FOCUS)
	{
		isCoefficientOfVariationInRange = frameStateDetails.coeffiecientOfVariation > Exit_Cv_Lower_Value &&
			                              frameStateDetails.coeffiecientOfVariation < Exit_Cv_Upper_Value;
		// Added by sriram to fix TS1X-1099
		if(this->isGainMax)
         isAvgBrightnessInRange = isGainMax;
		else if (isFocusEnteredAtMaxGain)
		{
			if(currentGain < 0.95* MaxGain)
             isAvgBrightnessInRange = frameStateDetails.AvgBrightness > frameStateDetails.avgBrightnessExitLowerLimit && 
			                              frameStateDetails.AvgBrightness < frameStateDetails.avgBrightnessExitUpperLimit;
			else
			 isAvgBrightnessInRange = true;
		}
		else
		{
			isAvgBrightnessInRange = frameStateDetails.AvgBrightness > frameStateDetails.avgBrightnessExitLowerLimit && 
			                              frameStateDetails.AvgBrightness < frameStateDetails.avgBrightnessExitUpperLimit;
		}
		if( isCoefficientOfVariationInRange && isAvgBrightnessInRange) 
			return ASSISTEDFOCUS_FRAME_UNIFORM_DEFOCUS ;
		else
			return ASSISTEDFOCUS_FRAME_NONUNIFORM;
	}
	return ASSISTEDFOCUS_FRAME_NONUNIFORM;

}
