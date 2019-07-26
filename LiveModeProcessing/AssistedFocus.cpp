// $URL:$
// $LastChangedBy:$ on $LastChangedDate:$

//Copyright (C) 2012, Forus Health Pvt. Ltd.
// sriram@forushealth.com
/* **********************************************************************************
 * File name : AssistedFocus.cpp
 *  
 * This class is used in Trisoft contains the entry point to the AssistedFocus dll
 * Which mainly computes the frame state and returns the state. It is also used to
 * initialize  various variables used by Assisted Focus.
 * Author: Sriram
 * Last modifed: 
 * 
 *************************************************************************************
 */
#include "LiveModeProcessing.h"
#define releaseMode

#ifdef releaseMode

AssistedFocus_FindFocus *findFocus;

	int ASSISTEDFOCUS_covUpperLimit;
	int ASSISTEDFOCUS_covLowerLimit;
	int ASSISTEDFOCUS_AvgBrightnessUpperLimit;
	int ASSISTEDFOCUS_AvgBrightnessLowerLimit;
	int ASSISTEDFOCUS_AvgBrightnessExitUpperLimit;
	int ASSISTEDFOCUS_AvgBrightnessExitLowerLimit;
	int ASSISTEDFOCUS_AvgBrightnessRef;
	int ASSISTEDFOCUS_entryPeakPercentage;
	int  ASSISTEDFOCUS_exitPeakPercentage;
	int  ASSISTEDFOCUS_noOfScales;
    int AssistedFocus_PeakThresholdLowerLimit;
	IplImage* srcImg, *tempImg ,*singleChannelImg;
	IplImage * LiveModeProcessing_defaultGainImage,*LiveModeProcessing_zeroGainImage;
void calculateIntensityLimits(void);

#pragma region Dll Entry points
extern "C" __declspec(dllexport) void AssistedFocus_Init(int AvgBrightnessRef,double covLowerLimit,double covUpperLimit,int numberOfScales , int EntryPeakPercentage,int ExitPeakPercentage,int MaxGain,int width,int height)
{
	ASSISTEDFOCUS_AvgBrightnessRef = AvgBrightnessRef;
	ASSISTEDFOCUS_covUpperLimit =(int) (covUpperLimit*100);
	ASSISTEDFOCUS_covLowerLimit =(int) (covLowerLimit*100);
    ASSISTEDFOCUS_noOfScales=numberOfScales;
    ASSISTEDFOCUS_entryPeakPercentage= EntryPeakPercentage;
    ASSISTEDFOCUS_exitPeakPercentage= ExitPeakPercentage;
    ASSISTEDFOCUS_noOfScales= numberOfScales;
    
	/*ASSISTEDFOCUS_IMAGE_HEIGHT = height;
	ASSISTEDFOCUS_IMAGE_WIDTH = width;*/
	calculateIntensityLimits();
	findFocus = new AssistedFocus_FindFocus();
	findFocus->AssistedFocus_FindFocus_Init(ASSISTEDFOCUS_covLowerLimit,ASSISTEDFOCUS_covUpperLimit,ASSISTEDFOCUS_AvgBrightnessUpperLimit,
		ASSISTEDFOCUS_AvgBrightnessLowerLimit,ASSISTEDFOCUS_AvgBrightnessExitLowerLimit,ASSISTEDFOCUS_AvgBrightnessExitUpperLimit,
		EntryPeakPercentage,ExitPeakPercentage,numberOfScales,AssistedFocus_PeakThresholdLowerLimit,width,height);
	findFocus->MaxGain = MaxGain;
	srcImg = cvCreateImageHeader(cvSize(width,height),IPL_DEPTH_8U,3);
	 singleChannelImg = cvCreateImageHeader(cvSize(width,height),IPL_DEPTH_8U,1);
	 tempImg = cvCreateImage(cvSize(width,height),IPL_DEPTH_32F,3);
	 LiveModeProcessing_defaultGainImage = cvCreateImage(cvSize(width,height),IPL_DEPTH_8U,3);
	 LiveModeProcessing_zeroGainImage = cvCreateImage(cvSize(width,height),IPL_DEPTH_8U,3);

}
extern "C" __declspec(dllexport) void  AssistedFocus_GetFrameState(uchar* srcImage,int* state,int currentGain,float* sharpness)
{
	cv::Mat inp = cv::Mat(srcImg->height,srcImg->width,CV_8UC3);
	cv::Mat arr[3];
	inp.data = srcImage;
	cv::split(inp,arr);
	singleChannelImg->imageData = (char*)arr[2].data;
	//cvCopy(srcImage,singleChannelImg);
	//cvConvertScale(srcImg,tempImg);
	findFocus->currentGain = currentGain;
//*sharpness =	findFocus->sharpnessObj->AssistedFocus_phaseCong3(srcImage);

	*state =  findFocus->AssistedFocus_computeFrameState(singleChannelImg);
	*sharpness = findFocus->frameStateDetails.sharpnessStrength;//.currentPeakSharpnessStrength;
	//*timetaken = (cvGetTickCount()-*timetaken)/(cvGetTickFrequency()*1000);

}
extern "C" __declspec(dllexport)void AssistedFocus_GetDebugInfo(int* cv,int* avgB,float* curPeak,float* curFrameSharp,float* exitPeakPercentage)
{
	findFocus->AssistedFocus_FindFocus_GetDebugInfo(cv,avgB,curPeak,curFrameSharp,exitPeakPercentage);
}
extern "C" __declspec(dllexport) void AssistedFocus_Reset()
{
	findFocus->AssistedFocus_ResetFrameDetails();
}

extern "C" __declspec(dllexport) void AssistedFocus_Update(int AvgBrightnessRef,double covLowerLimit,double covUpperLimit,int numberOfScales , int EntryPeakPercentage,int ExitPeakPercentage,int width,int height)
{
	ASSISTEDFOCUS_AvgBrightnessRef = AvgBrightnessRef;
	ASSISTEDFOCUS_covUpperLimit =(int) (covUpperLimit*100);
	ASSISTEDFOCUS_covLowerLimit =(int) (covLowerLimit*100);
    ASSISTEDFOCUS_noOfScales=numberOfScales;
    ASSISTEDFOCUS_entryPeakPercentage= EntryPeakPercentage;
    ASSISTEDFOCUS_exitPeakPercentage= ExitPeakPercentage;
    ASSISTEDFOCUS_noOfScales= numberOfScales;
	calculateIntensityLimits();
	findFocus->AssistedFocus_FindFocus_Init(covLowerLimit,covUpperLimit,ASSISTEDFOCUS_AvgBrightnessUpperLimit,
		ASSISTEDFOCUS_AvgBrightnessLowerLimit,ASSISTEDFOCUS_AvgBrightnessExitLowerLimit,ASSISTEDFOCUS_AvgBrightnessExitUpperLimit,
		EntryPeakPercentage,ExitPeakPercentage,numberOfScales,AssistedFocus_PeakThresholdLowerLimit,width,height );

}
extern "C" __declspec(dllexport) void AssistedFocus_Exit()
{
	cvReleaseImage(&srcImg);
	cvReleaseImage(&tempImg);
	cvReleaseImage(&LiveModeProcessing_defaultGainImage);
	cvReleaseImage(&LiveModeProcessing_zeroGainImage);
	findFocus->AssistedFocus_FindFocus_Exit();
}
// Added by sriram to fix the CR TS1X-1100 this function returns the gain for the alternate mode.
extern "C" __declspec(dllexport) int LiveModeProcessing_ComputeGain(char* defaultGainImage,char* zeroGainImage,int defaultGain,int defaultBrightness,int expectedBrightness,int maxGain)
{
	LiveModeProcessing_defaultGainImage->imageData = defaultGainImage;
	LiveModeProcessing_zeroGainImage->imageData = zeroGainImage;
	
			CvScalar zeroGainImgBrightness,defaultGainImgBrightness;

	         // calculate Average and standard deviation of the  green channel image.
			zeroGainImgBrightness = cvAvg(LiveModeProcessing_zeroGainImage);
			defaultGainImgBrightness =cvAvg(LiveModeProcessing_defaultGainImage);
			double currentGain = (double)defaultGain * ((double)expectedBrightness - zeroGainImgBrightness.val[1]) / (defaultGainImgBrightness.val[1] - zeroGainImgBrightness.val[1]);
            if (currentGain < 0)
            {
				currentGain = 3 *defaultGain / 5;
                currentGain = currentGain+0.5;
            }
            else if (currentGain < maxGain)
                currentGain = currentGain+0.5;
            else
                currentGain = maxGain;
  return (int)currentGain;
}
void calculateIntensityLimits()
{
	double tempLowerLmit = 0.9 * ASSISTEDFOCUS_AvgBrightnessRef;
	ASSISTEDFOCUS_AvgBrightnessLowerLimit = (int)tempLowerLmit;
	double upperValue = 1.1 * ASSISTEDFOCUS_AvgBrightnessRef;
	
	if (upperValue > ASSISTEDFOCUS_AvgBrightnessRef + 10)
	{
		ASSISTEDFOCUS_AvgBrightnessUpperLimit = ASSISTEDFOCUS_AvgBrightnessRef + 10;
	}
	else
		ASSISTEDFOCUS_AvgBrightnessUpperLimit = (int)upperValue;
	upperValue = .88 * ASSISTEDFOCUS_AvgBrightnessRef;
	ASSISTEDFOCUS_AvgBrightnessExitLowerLimit = (int)upperValue;
	upperValue = 1.15 * ASSISTEDFOCUS_AvgBrightnessRef;
	ASSISTEDFOCUS_AvgBrightnessExitUpperLimit = (int)upperValue;
	AssistedFocus_PeakThresholdLowerLimit = 3* (int)ASSISTEDFOCUS_AvgBrightnessRef;

}

#pragma endregion



#else

#pragma region Function Prototypes
void readFile(char* ,CvMat*);
void ReadLogGaborRealImageH(void);
void calculateIntensityLimits(void);
void printValues(CvMat*,const char*);
#pragma endregion
extern "C"__declspec(dllexport) void DFTIDFT(IplImage* src_Image,IplImage*dest_Image)
{
	IplImage* ASSISTEDFOCUS_complexImage = cvCreateImage(cvGetSize(src_Image), src_Image->depth,2);

	cvSetImageCOI(ASSISTEDFOCUS_complexImage, 1);
	cvCopy(src_Image, ASSISTEDFOCUS_complexImage);
	cvSetImageCOI(ASSISTEDFOCUS_complexImage, 0);
	IplImage* dft_Image = cvCreateImage(cvGetSize(src_Image), src_Image->depth,2);
	cvDFT(ASSISTEDFOCUS_complexImage, dft_Image, CV_DXT_FORWARD, 0);
	cvDFT(dft_Image,dest_Image,CV_DXT_INV_SCALE,0);

}
#pragma region Private Methods

void printValues(CvMat* src_arr, const char* fileName)
{
	CvScalar pixelValue;
	ofstream myfile;
	myfile.open (fileName);
	for(int i =0;i<cols;i++)
	{
		for(int j=0;j<rows;j++)
		{

			pixelValue = 	cvGet2D(src_arr,i,j);
			myfile << pixelValue.val[0]<<"\t";
		}
		myfile<<"\n";
	}
	myfile.close();

}

void ReadLogGaborRealImageH()
{

	/*realH = cvCreateMat(rows, cols, CV_32FC1);
	imagH = cvCreateMat(rows, cols, CV_32FC1);

	log1 = cvCreateMat(rows, cols, CV_32FC1);
	log2 = cvCreateMat(rows, cols, CV_32FC1);
	log3 = cvCreateMat(rows, cols, CV_32FC1);
	log4 = cvCreateMat(rows, cols, CV_32FC1);
	log5 = cvCreateMat(rows, cols, CV_32FC1);


	char *fileNameArr="C:\\Users\\Ravi\\Documents\\Visual Studio 2008\\Projects\\dft\\dft\\bin\\Debug\\realH.txt";

	readFile(fileNameArr,realH);
	fileNameArr = "C:\\Users\\Ravi\\Documents\\Visual Studio 2008\\Projects\\dft\\dft\\bin\\Debug\\ImagH.txt";
	readFile(fileNameArr,imagH);
	cvMerge(realH, imagH, 0, 0, HArr);

	fileNameArr = "C:\\Users\\Ravi\\Documents\\Visual Studio 2008\\Projects\\dft\\dft\\bin\\Debug\\LogGabor1.txt";

	readFile(fileNameArr,log1);  

	fileNameArr = "C:\\Users\\Ravi\\Documents\\Visual Studio 2008\\Projects\\dft\\dft\\bin\\Debug\\LogGabor2.txt";


	readFile(fileNameArr,log2);

	fileNameArr = "C:\\Users\\Ravi\\Documents\\Visual Studio 2008\\Projects\\dft\\dft\\bin\\Debug\\LogGabor3.txt";

	readFile(fileNameArr,log3);

	fileNameArr = "C:\\Users\\Ravi\\Documents\\Visual Studio 2008\\Projects\\dft\\dft\\bin\\Debug\\LogGabor4.txt";

	readFile(fileNameArr,log4);

	fileNameArr = "C:\\Users\\Ravi\\Documents\\Visual Studio 2008\\Projects\\dft\\dft\\bin\\Debug\\LogGabor5.txt";

	readFile(fileNameArr,log5);*/

}


//void selectLogGaborArr(int selectionInd)
//{
//	switch(selectionInd)
//	{
//	case 0 : {cvCopy(log1,ASSISTEDFOCUS_temp1);
//		break;
//			 }
//	case 1 : {cvCopy(log2,ASSISTEDFOCUS_temp1);
//		break;
//			 }
//	case 2 : {cvCopy(log3,ASSISTEDFOCUS_temp1);
//		break;
//			 }
//	case 3 : {cvCopy(log4,ASSISTEDFOCUS_temp1);
//		break;
//			 }
//	case 4 : {cvCopy(log5,ASSISTEDFOCUS_temp1);
//		break;
//			 }
//
//	}
//}

void readFile(char* fileName,CvMat* destArr)
{
	ifstream myfile(fileName);
	string line;
	if (myfile.is_open())
	{
		int countx =0,county=0;	 
		while ( myfile.good())
		{
			if(countx!=cols-1)
			{
				getline (myfile,line);
				cvSetReal2D(destArr,county,countx,::atof(line.c_str()));
				countx++;
			}
			else
			{
				if(	county!=rows-1)	
				{ 
					county++;
					countx=0;
				}
				else
					break;
			}
		}
		myfile.close();
	}

	else cout << "Unable to open file to read"; 

}




#pragma endregion
#endif