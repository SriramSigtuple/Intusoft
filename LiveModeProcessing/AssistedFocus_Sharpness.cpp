#include "AssistedFocus_Sharpness.h"



void AssistedFocus_Sharpness::AssistedFocus_CalculateNscale()
{
	for ( this->iter_num = 0; this->iter_num < nscale; this->iter_num++)
	{
		
		cvMul(this->ASSISTEDFOCUS_temp6,this->lut_obj->ASSISTEDFOCUS_log_Array[this->iter_num],this->ASSISTEDFOCUS_temp2,1);

		cvMul(this->ASSISTEDFOCUS_temp7,this->lut_obj->ASSISTEDFOCUS_log_Array[this->iter_num],this->ASSISTEDFOCUS_temp3,1);

		cvMerge(this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_temp3, 0,0,this->ASSISTEDFOCUS_dftArr);

		cvDFT(this->ASSISTEDFOCUS_dftArr, this->ASSISTEDFOCUS_dftArr, CV_DXT_INV_SCALE ,rows);

		cvSplit(this->ASSISTEDFOCUS_dftArr,this->ASSISTEDFOCUS_f,0,0,0);

		cvMul(this->ASSISTEDFOCUS_temp2, this->lut_obj->ASSISTEDFOCUS_imagH, this->ASSISTEDFOCUS_temp4, 1);//ad

		cvMul(this->ASSISTEDFOCUS_temp2, this->lut_obj->ASSISTEDFOCUS_realH, this->ASSISTEDFOCUS_temp1, 1);//ac
		cvMul(this->ASSISTEDFOCUS_temp3, this->lut_obj->ASSISTEDFOCUS_imagH, this->ASSISTEDFOCUS_temp2, 1);//bd

		cvMul(this->ASSISTEDFOCUS_temp3, this->lut_obj->ASSISTEDFOCUS_realH, this->ASSISTEDFOCUS_temp3, 1);//bc


		cvSub(this->ASSISTEDFOCUS_temp1, this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_temp2);// ac-bd real part

		cvAdd(this->ASSISTEDFOCUS_temp3, this->ASSISTEDFOCUS_temp4, this->ASSISTEDFOCUS_temp4);// ad+ bc imaginary part.



		cvMerge(this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_temp4, 0, 0, this->ASSISTEDFOCUS_dftArr);


		cvDFT(this->ASSISTEDFOCUS_dftArr, this->ASSISTEDFOCUS_dftArr, CV_DXT_INV_SCALE ,rows);

		cvSplit(this->ASSISTEDFOCUS_dftArr,this->ASSISTEDFOCUS_temp1,this->ASSISTEDFOCUS_temp2,0,0);//split real and imaginary parts h1 and h2
		
		cvAdd(this->ASSISTEDFOCUS_temp1, this->ASSISTEDFOCUS_sumH1, this->ASSISTEDFOCUS_sumH1);

		cvAdd(this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_sumH2, this->ASSISTEDFOCUS_sumH2);

		cvAdd(this->ASSISTEDFOCUS_f, this->ASSISTEDFOCUS_sumf, this->ASSISTEDFOCUS_sumf);

		cvPow(this->ASSISTEDFOCUS_temp1, this->ASSISTEDFOCUS_temp3, 2);

		cvPow(this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_temp4, 2);

		cvPow(this->ASSISTEDFOCUS_f, this->ASSISTEDFOCUS_temp5, 2);

		cvAdd(this->ASSISTEDFOCUS_temp3, this->ASSISTEDFOCUS_temp4, this->ASSISTEDFOCUS_temp2);

		cvAdd(this->ASSISTEDFOCUS_temp5, this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_temp2);

		cvPow(this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_temp2, 0.5);

		cvAdd(this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_sumAn, this->ASSISTEDFOCUS_sumAn);

		double histSum = 0, median = 0;
		if (this->iter_num == 0)
		{
			cvCopy(this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_MaxAn);
			AssistedFocus_CalculateMedian();
		}
		else
		{
			cvMax(this->ASSISTEDFOCUS_temp2, this->ASSISTEDFOCUS_MaxAn, this->ASSISTEDFOCUS_MaxAn);

		}
		
	}
}

void  AssistedFocus_Sharpness::AssistedFocus_CalculateMedian()
{
	double minVal=0 , maxVal=0;
	CvPoint minLoc = cvPoint(0,0);
	CvPoint maxLoc =  cvPoint(0,0);
	float median =0;
	cvMinMaxLoc(this->ASSISTEDFOCUS_sumAn, & minVal, & maxVal, & minLoc, & maxLoc);
	int noOFBins = 256;
	double ASSISTEDFOCUS_MULTFac =(double) (noOFBins-1) / (maxVal - minVal);

	cvConvertScale(this->ASSISTEDFOCUS_sumAn,this->histImg, ASSISTEDFOCUS_MULTFac, 0);

	//cvConvertScale(this->ASSISTEDFOCUS_temp5, histImg, 1, 0);

	cvMinMaxLoc(histImg, & minVal, & maxVal, & minLoc, & maxLoc);
	if(minVal ==maxVal)
	{
		median = (float)maxVal;
	}
	else
	{
		/*float range[] = {(float)minVal,(float) maxVal};
		float *ranges[] = { range };
		CvHistogram *histogramFirstBin = cvCreateHist(1, &noOFBins, CV_HIST_ARRAY, ranges, 1);
		cvCalcHist( &this->histImg, histogramFirstBin, 0, 0);

		int arrVal1[ 256] ;
		int currInd=0, prevInd=0;
		float tempVal = 0;*/
		float tempVal = 0;
		int hist_size = 256;  
		cv::Mat hist;
		cv::Mat histSrc = cv::cvarrToMat(histImg);
	float Histrange[]={(float)minVal,(float) maxVal};
	//float* ranges[] = { Histrange };
	int arrVal1[ 256] ;
	int currInd=0, prevInd=0;
	float max_value = 0.0;
	float max = 0.0;
	float w_scale = 0.0;
	/* Create a 1-D Arrays to hold the histograms */
	//hist = cvCreateHist(1, &hist_size, CV_HIST_ARRAY, ranges, 1);
	/*hist = cvCreateHist(1, &hist_size, CV_HIST_ARRAY, ranges, 1);
	calcHist(&srcImg,1,*/
	//cvCalcHist( &srcImg, hist, 0, mask);
	  int hbins = 256, sbins = 32;
	  int histSize[] = {hbins};
    // hue varies from 0 to 179, see cvtColor
    float hranges[] = { 0, 255 };
    // saturation varies from 0 (black-gray-white) to
    // 255 (pure spectrum color)
    float sranges[] = { 0, 256 };
    const float* ranges[] = { hranges };
    //Mat hist;
    // we compute the histogram from the 0-th and 1-st channels
    int channels[] = {0};
	calcHist( &histSrc, 1, 0, cv::Mat(), // do not use mask
             hist, 1, histSize, ranges,
             true, // the histogram is uniform
             false );




		for (int k = 0; k < noOFBins; k++)
		{
			if (k == 0)
			{  
				
				tempVal =	hist.at<float>(k);
				//empVal = (float)cvGetReal1D(histogramFirstBin,k); 
				arrVal1[k] = (int)tempVal;
			} 
			else
			{
				tempVal =	hist.at<float>(k);

				//tempVal = (float) cvGetReal1D(histogramFirstBin,k); 
				arrVal1[k] = (int)tempVal + arrVal1[k-1];
			}
			int currentFreqVal = 2 * arrVal1[k];
			if (currentFreqVal >= (this->ASSISTEDFOCUS_sumAn->width * this->ASSISTEDFOCUS_sumAn->height))
			{
				currInd = k;
				break;
			}
		}
		
		//float var1 =  (float)cvGetReal1D(histogramFirstBin, currInd);
		float var1 =  hist.at<float>(currInd);
		float cumulativeFreq=0;
		if(currInd ==0)
			cumulativeFreq = (float)arrVal1[0];
		else
			cumulativeFreq = (float)arrVal1[(int)(currInd - 1)];
		float classValue =(float) (this->ASSISTEDFOCUS_sumAn->width*this->ASSISTEDFOCUS_sumAn->height*0.5) - cumulativeFreq;
		classValue = classValue/var1;
		float median1 = currInd + classValue ;
		float var = (float)ASSISTEDFOCUS_MULTFac;

		var = median1/var;// = (float)arrVal.GetValue((arrVal.Length/2) +1);
		median =var;
		hist.~Mat();
		histSrc.~Mat();
		//cvReleaseHist(&histogramFirstBin);
	}

	this->tau = (double)median / 1.1774;

}

void  AssistedFocus_Sharpness::AssistedFocus_Sharpness_Init(int noOfScales)
{
	this->tau = 0;
 this->nscale =noOfScales;
 this->C_Value=0;
 this->divVal =0;
 this->lut_obj = new AssistedFocus_LUT();
	this->lut_obj->ASSISTEDFOCUS_realH = cvCreateMat(rows,cols,CV_32FC1);
	this->lut_obj->ASSISTEDFOCUS_imagH = cvCreateMat(rows,cols,CV_32FC1);
	this->lut_obj->ASSISTEDFOCUS_log_Array[0] = cvCreateMat(rows,cols,CV_32FC1);
	this->lut_obj->ASSISTEDFOCUS_log_Array[1] = cvCreateMat(rows,cols,CV_32FC1);
	this->lut_obj->ASSISTEDFOCUS_log_Array[2] = cvCreateMat(rows,cols,CV_32FC1);
	this->lut_obj->ASSISTEDFOCUS_log_Array[3] = cvCreateMat(rows,cols,CV_32FC1);
	this->lut_obj->ASSISTEDFOCUS_log_Array[4] = cvCreateMat(rows,cols,CV_32FC1);
	this->lut_obj->ASSISTEDFOCUS_log_Array[5] = cvCreateMat(rows,cols,CV_32FC1);
	this->lut_obj->ASSISTEDFOCUS_lut = cvCreateMat(rows,1,CV_8UC1);

	this->ASSISTEDFOCUS_temp5 = cvCreateMat(rows, cols, CV_32FC1);

	//// h1, h2,ASSISTEDFOCUS_f
	this->ASSISTEDFOCUS_sumAn = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_sumH2 = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_sumH1 = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_sumf = cvCreateMat(rows, cols, CV_32FC1);

	this->ASSISTEDFOCUS_f  = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_temp1 = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_temp2 = cvCreateMat(rows, cols, CV_32FC1);
	ASSISTEDFOCUS_temp3 = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_temp4 = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_temp6 = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_temp7 = cvCreateMat(rows, cols, CV_32FC1);

	this->ASSISTEDFOCUS_MaxAn = cvCreateMat(rows, cols, CV_32FC1);
	this->ASSISTEDFOCUS_dftArr = cvCreateMat(rows, cols, CV_32FC2);
	this->ASSISTEDFOCUS_origDftArr = cvCreateMat(rows, cols, CV_32FC2);

	this->ASSISTEDFOCUS_lutByteArr = cvCreateMat(rows, cols, CV_8UC1);
	this->ASSISTEDFOCUS_complexImage = cvCreateImage(cvSize(rows,cols), IPL_DEPTH_8U,2);
	double a = 1 - (1 / ASSISTEDFOCUS_MULT);
	double b = pow( (1 / ASSISTEDFOCUS_MULT),this->nscale);
	histImg =cvCreateImage(cvGetSize(this->ASSISTEDFOCUS_dftArr),IPL_DEPTH_8U,1);

	b = 1 - b;
	C_Value = b / a;
	divVal = 1.0 / ((double)nscale - 1.0);
	lut_obj->AssistedFocus_AssignLUTtoMAt();

}
double AssistedFocus_Sharpness::AssistedFocus_Compute_FrameSharpness(IplImage * srcImg)
{
	return this->AssistedFocus_phaseCong3(srcImg);
}
double  AssistedFocus_Sharpness::AssistedFocus_phaseCong3(IplImage * srcImg)
{

	// Reseting the interatively used variables
	/*cvSetZero(this->ASSISTEDFOCUS_sumH1);
	cvSetZero(this->ASSISTEDFOCUS_sumH2);
	cvSetZero(this->ASSISTEDFOCUS_sumf);
	cvSetZero(this->ASSISTEDFOCUS_sumAn);*/
    this->iter_num=0;
	//Set Roi of the IR Frame
	//cvSetImageROI(srcImg,cvRect(256, 384, 256, 256));// older centred roi

	cvSetImageROI(srcImg,cvRect(srcImg->width/2 -300, srcImg->height/2-300, 600, 600));// Roi to the right for a left eye image.
	//cvSetImageROI(srcImg,cvRect(576, 256, 256, 256));// Roi to the right for a left eye image.

	// Set channel of interest of the complex image 
	cvSetImageCOI(ASSISTEDFOCUS_complexImage, 1);
	//Copy the roi set green channel frame to the real part of the complex image
	cvCopy(srcImg, ASSISTEDFOCUS_complexImage);
	//reset complex image channels
	cvSetImageCOI(ASSISTEDFOCUS_complexImage, 0);
	// release the roi of the green channel frame
	cvResetImageROI(srcImg);
	cvConvertScale(ASSISTEDFOCUS_complexImage,this->ASSISTEDFOCUS_origDftArr);

	// DFT of the complex image 
	cvDFT(this->ASSISTEDFOCUS_origDftArr,this->ASSISTEDFOCUS_origDftArr, CV_DXT_FORWARD,0);
	cvSplit(this->ASSISTEDFOCUS_origDftArr,this->ASSISTEDFOCUS_temp6,this->ASSISTEDFOCUS_temp7,0,0);
	//////Sum values
	//*timetaken = cvGetTickCount();
	AssistedFocus_CalculateNscale();
	//*timetaken = (cvGetTickCount()-*timetaken)/(cvGetTickFrequency()*1000);
		
	double totalTau = tau * C_Value ;
	double EstNoiseEnergyMean = totalTau * sqrt(PI/ 2);
	double EstNoiseEnergySigma = totalTau * sqrt((4 - PI) / 2);
	double T = EstNoiseEnergyMean + 2 * EstNoiseEnergySigma;

	cvMul(this->ASSISTEDFOCUS_sumf,this->ASSISTEDFOCUS_sumf,this->ASSISTEDFOCUS_sumf,1);
	//cvPow(ASSISTEDFOCUS_sumf, ASSISTEDFOCUS_sumf, 2);
	cvMul(this->ASSISTEDFOCUS_sumH1,this->ASSISTEDFOCUS_sumH1,this->ASSISTEDFOCUS_sumH1);
	//cvPow(ASSISTEDFOCUS_sumH1, ASSISTEDFOCUS_sumH1, 2);
	cvMul(this->ASSISTEDFOCUS_sumH2,this->ASSISTEDFOCUS_sumH2,this->ASSISTEDFOCUS_sumH2);
	//cvPow(ASSISTEDFOCUS_sumH2, ASSISTEDFOCUS_sumH2, 2);



	cvAdd(this->ASSISTEDFOCUS_sumf, this->ASSISTEDFOCUS_sumH1,this->ASSISTEDFOCUS_sumH1);

	cvAdd(this->ASSISTEDFOCUS_sumH1, this->ASSISTEDFOCUS_sumH2, this->ASSISTEDFOCUS_sumH1);

	cvPow(this->ASSISTEDFOCUS_sumH1, this->ASSISTEDFOCUS_sumH1 ,0.5);

	cvConvertScale(this->ASSISTEDFOCUS_sumH1, this->ASSISTEDFOCUS_sumH1, 1, ASSISTEDFOCUS_EPSILON);
	cvAddS(this->ASSISTEDFOCUS_sumH1,cvScalar( ASSISTEDFOCUS_EPSILON),this->ASSISTEDFOCUS_sumH1);
	cvConvertScale(this->ASSISTEDFOCUS_MaxAn, this->ASSISTEDFOCUS_MaxAn, 1, ASSISTEDFOCUS_EPSILON);

	cvDiv(this->ASSISTEDFOCUS_sumAn, this->ASSISTEDFOCUS_MaxAn, this->ASSISTEDFOCUS_temp5, 1);

	//cvConvertScale(ASSISTEDFOCUS_temp5, ASSISTEDFOCUS_temp5, 1, -1);
	cvAddS(this->ASSISTEDFOCUS_temp5,cvScalar(-1),this->ASSISTEDFOCUS_temp5);
	cvConvertScale(this->ASSISTEDFOCUS_temp5, this->ASSISTEDFOCUS_temp5,divVal, 0);

	cvSubRS(this->ASSISTEDFOCUS_temp5,cvScalar(0.5), this->ASSISTEDFOCUS_temp5);

	cvConvertScale(this->ASSISTEDFOCUS_temp5, this->ASSISTEDFOCUS_temp5, 10, 0);

	cvExp(this->ASSISTEDFOCUS_temp5, this->ASSISTEDFOCUS_temp5);

	cvConvertScale(this->ASSISTEDFOCUS_temp5, this->ASSISTEDFOCUS_temp5, 1, 1);

	cvPow(this->ASSISTEDFOCUS_temp5, this->ASSISTEDFOCUS_temp1, -1);

	cvConvertScale(this->ASSISTEDFOCUS_sumAn, this->ASSISTEDFOCUS_sumAn, 1, ASSISTEDFOCUS_EPSILON);


	cvDiv(this->ASSISTEDFOCUS_sumH1, this->ASSISTEDFOCUS_sumAn, this->ASSISTEDFOCUS_sumH1, 1);

	ASSISTEDFOCUS_lutByteArr = cvCreateMat(rows, cols, CV_8UC1);

	cvConvertScale(this->ASSISTEDFOCUS_sumH1, ASSISTEDFOCUS_lutByteArr, 255, 0);

	cvConvertScale(this->ASSISTEDFOCUS_sumAn, this->ASSISTEDFOCUS_sumAn, 1 / T, 0);

	cvPow(this->ASSISTEDFOCUS_sumAn, this->ASSISTEDFOCUS_sumAn, -1);

	cvLUT(ASSISTEDFOCUS_lutByteArr, ASSISTEDFOCUS_lutByteArr, this->lut_obj->ASSISTEDFOCUS_lut);


	cvConvertScale(ASSISTEDFOCUS_lutByteArr, this->ASSISTEDFOCUS_sumH1, 1, 0);

	cvConvertScale(this->ASSISTEDFOCUS_sumH1, this->ASSISTEDFOCUS_sumH1, 1/128.0, 0);

	cvAdd(this->ASSISTEDFOCUS_sumH1, this->ASSISTEDFOCUS_sumAn, this->ASSISTEDFOCUS_sumH1);

	cvSubRS(this->ASSISTEDFOCUS_sumH1, cvScalar(1), this->ASSISTEDFOCUS_sumH1);
	cvMaxS(this->ASSISTEDFOCUS_sumH1, 0.0,this-> ASSISTEDFOCUS_sumH1);

	cvMul(this->ASSISTEDFOCUS_sumH1, this->ASSISTEDFOCUS_temp1, this->ASSISTEDFOCUS_temp1, 1);

	CvScalar sumVal = cvSum(this->ASSISTEDFOCUS_temp1);
	
	return sumVal.val[0];

}


void AssistedFocus_Sharpness::AssistedFocus_Sharpness_Exit()
{
	this->lut_obj->AssistedFocus_ReleaseLUTMAt();
	cvReleaseMat(&this->ASSISTEDFOCUS_sumAn);
	cvReleaseMat(&this->ASSISTEDFOCUS_sumf);
	cvReleaseMat(&this->ASSISTEDFOCUS_sumH1);
	cvReleaseMat(&this->ASSISTEDFOCUS_sumH2);
	cvReleaseMat(&this->ASSISTEDFOCUS_f);
	cvReleaseMat(&this->ASSISTEDFOCUS_temp1);
	cvReleaseMat(&this->ASSISTEDFOCUS_temp2);
	cvReleaseMat(&this->ASSISTEDFOCUS_temp3);
	cvReleaseMat(&this->ASSISTEDFOCUS_temp4);
	cvReleaseMat(&this->ASSISTEDFOCUS_temp5);
	cvReleaseMat(&this->ASSISTEDFOCUS_temp6);
	cvReleaseMat(&this->ASSISTEDFOCUS_temp7);
	cvReleaseMat(&this->ASSISTEDFOCUS_MaxAn);
	cvReleaseMat(&this->ASSISTEDFOCUS_dftArr);
	cvReleaseMat(&this->ASSISTEDFOCUS_origDftArr);
	cvReleaseImage(&this->ASSISTEDFOCUS_complexImage);
	cvReleaseMat(&this->ASSISTEDFOCUS_lutByteArr);
}



















































