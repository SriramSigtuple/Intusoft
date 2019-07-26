#include <iostream>
#include <highgui.h>
#include  "cv.hpp"
#include <vector>
#include <math.h>  
#include <opencv2/opencv.hpp>
#define _USE_MATH_DEFINES
#include <math.h>
#include <string>
#include <stdio.h>
#include <stdlib.h>
#include <cstdlib>
#include "cxcore.h"
#include <cv.h>
#include<cstdint>
using namespace cv;
using namespace std;



Mat hist;



double GetEntropy(Mat srcImg,Mat mask);
void GetHistogram(Mat srcImg,Mat mask);
double GetPercentageChange (double valueShadowed,double valueCircum);




double E_Red_full,E_Green_full, E_Blue_full, E_Red_shadowed,E_Green_shadowed, E_Blue_shadowed, E_Red_Rim,E_Blue_Rim,E_Green_Rim;
void GetShadowMask(CvPoint centerShadow, int radiusShadow , IplImage* dstImg);
//void GetRectangle(CvPoint centerShadow, int radiusShadow , IplImage* dstImg);
void GetPercentage(double avgNormal, double avgShadowed, double *PercB);

void GetROI(IplImage* dstImg1,IplImage* dstImg2);

Mat  inputImg,grayImg, actual_ROI, ircum_ROI, Rim , inner_mask_boundry, outer_mask_boundry, circum_mask_boundry;
Mat shadow_mask_1, shadow_mask_2,shadow_mask_3,shadow_mask_4,shadow_mask_5,shadow_mask_6,shadow_mask_7,shadow_mask_8;
Mat circum_mask_1, circum_mask_2,circum_mask_3,circum_mask_4,circum_mask_5,circum_mask_6,circum_mask_7,circum_mask_8,CaptureMask,LiveMask;
																	
			
			
double GetEntropy(IplImage *srcImg);
void GetHistogram(IplImage *srcImg);
void calculate_Contrast_Brightness_Std(Mat srcImg[],double* brightness,double * contrast,Mat mask);
void calculate_Contrast_Brightness_Std(IplImage* srcImg,IplImage* srcImg1,IplImage* srcImg2,double* brightness,double * contrast,IplImage*mask,int x,int y,int w,int h);


CvPoint Center_shadow, point_1_shadow,point_2_shadow,point_3_shadow,point_4_shadow,point_5_shadow,point_6_shadow,point_7_shadow,point_8_shadow; 
CvPoint point_1_circum,point_2_circum,point_3_circum,point_4_circum,point_5_circum,point_6_circum,point_7_circum,point_8_circum; 
//
int radius_shadow, rim_thickness, PercR, PercG, PercB, radius_hotspot;
//double E_Red_full,E_Green_full, E_Blue_full, E_Red_shadowed,E_Green_shadowed, E_Blue_shadowed, E_Red_Rim,E_Blue_Rim,E_Green_Rim;
//double averageR, averageG, averageB, averageRimR, averageRimG, averageRimB, percentageB, percentageG, percentageR, averageInputB, averageInputG , averageInputR, ContrastValB, ContrastValG,ContrastValR; 
//double GetPercentageEntropyChange (double entropyShadowed,double entropyCircum);
//
//void GetAverage(IplImage * srcImg, double *avgB, double *avgG, double *avgR);
CvRect GetRectangle(CvPoint centerShadow, int radiusShadow , IplImage* dstImg);
//void GetPercentage(double avgNormal, double avgShadowed, double *PercB);
void GetROO_Mask(IplImage* dstImg1_mask,CvPoint point_1,CvPoint point_2,CvPoint point_3);
float ThresholdmaxValue(float high_threshold);
void ContrastEnhance(IplImage* grayImgB, IplImage* grayImgG, IplImage* grayImgR,float clipG);
void ContrastEnhance(Mat inp[],float clipVals[]);
CvRect rect_shadow,rect_circum;
double varianceOfLaplacian(const cv::Mat& src,const cv::Mat& mask);
double tenengrad(cv::Mat& src, int ksize,const cv::Mat& mask,int centreX,int centreY,int SquareSize);
short GetSharpness(IplImage* data, unsigned int width, unsigned int height,int centreX,int centreY,int SquareSize);// used to for measuring focus value in the image
void ApplyUnsharpMaskMonoChrome(IplImage* inp,double thres,double amount,double sigma,int medianFilterSize);// used to apply Unsharp mask for single channel image
void ApplyClaheMonoChrome(Mat* inputPtr,int width ,int height,float ClipVal );// used to apply Clahe for single channel image
void ApplyLUTMonoChrome(IplImage* inp,int width,int height,bool is8bit);// used to apply LUT for single channel image
void HotspotCompensationMonoChrome(IplImage* bm, int HsCenterX, int HsCenterY,  int imgWidth, int imgHeight, int HsPeak, int HsRadius);
void ShadowCompensationMonoChrome(IplImage* bm, int imgCenterX, int imgCenterY,  int imgWidth, int imgHeight,  
																int innerRad, int PeakRad1, int PeakRad2, int outerRad, 
																int PeakDropPercentage);
void ApplyLUT(unsigned char* inp,IplImage* MonoChromeImg,int width,int height,bool is8bit,bool isColor);
void ApplyLUT_ChannelWise(unsigned char* inp,IplImage* MonoChromeImg,int width,int height,bool is8bit,bool isColor);

void ImageProc_CalculateChannelWiseLut(double sineFactor,double interval1,double interval2 ,int bitDepth,bool isForuteenBit,int offset, int channelCode);
void ImageProc_NonChannelWiseCalculateLut(double sineFactor,double interval1,double interval2 ,int bitDepth,bool isForuteenBit,int offset);