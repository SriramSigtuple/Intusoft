// $LastChangedBy:$ on $LastChangedDate:$

//Copyright (C) 2012, Forus Health Pvt. Ltd.
// sriram@forushealth.com
/* **********************************************************************************
 * File name : AssistedFocus_Sharpness.h
 *  
 * This class header file is used  by the AssistedFocus project 
 * Which mainly computes sharpness strength of the IR frame returns it. 
 * Author: Sriram
 * Last modifed: 
 * 
 *************************************************************************************
 */
#ifndef INCLUDED_Sharpness
#define INCLUDED_Sharpness
#include "AssistedFocus_Globals.h"
#include "AssistedFocus_LUT.h"
class AssistedFocus_Sharpness
{
 CvMat  *ASSISTEDFOCUS_dftArr,*ASSISTEDFOCUS_origDftArr,*ASSISTEDFOCUS_sumAn,*ASSISTEDFOCUS_sumH1,*ASSISTEDFOCUS_sumH2,*ASSISTEDFOCUS_sumf,
		*ASSISTEDFOCUS_temp5,*ASSISTEDFOCUS_temp6,*ASSISTEDFOCUS_temp7,*ASSISTEDFOCUS_MaxAn,*ASSISTEDFOCUS_f,*ASSISTEDFOCUS_temp1,*ASSISTEDFOCUS_temp2,*ASSISTEDFOCUS_temp3,*ASSISTEDFOCUS_temp4,*ASSISTEDFOCUS_lutByteArr;
IplImage* ASSISTEDFOCUS_complexImage;
IplImage*  histImg;
 
	 double tau ;
 int nscale;
 double C_Value;
 double divVal;
 int iter_num;
	AssistedFocus_LUT *lut_obj;
	void AssistedFocus_CalculateNscale();
	void AssistedFocus_CalculateMedian();
	double AssistedFocus_phaseCong3(IplImage* srcImg);

public:
	void AssistedFocus_Sharpness_Init(int noOfScales);
	void AssistedFocus_Sharpness_Exit();
	double AssistedFocus_Compute_FrameSharpness(IplImage* srcImg);
};
#endif
