#include "Header.h"
#include "clahe.h"

int TMP_HEIGHT =1802;
int TMP_WIDTH =1538;
using namespace std;
Mat parabolaImage;
float * parabolaImage_float;
//stitchRet* retinalImageStich;
//IplImage* parabolaHotspotImage;
//IplImage* inp ;
//IplImage* inpH;
//IplImage* inpS;
//IplImage* inpV;
//IplImage* inp2;
//IplImage* maskImg1 	   ;
//IplImage* maskImg2 	   ;
//IplImage* maskImg3 	   ;
//IplImage* maskImg4 	   ;
//IplImage* maskImg23	  ;
//IplImage* maskImg12	  ;
//IplImage* temp ;

Mat parabolaHotspotImage;
Mat inp ;
Mat inp2;
Mat maskImg1;
Mat maskImg2;
Mat maskImg3;
Mat maskImg4;
Mat maskImg23;
Mat maskImg12;
Mat temp;

int percentageR = 20, percentageG = 16, percentageB = 6 ,ShadowSpotRad1 =175,ShadowSpotRad2 = 150,presetGain = 200,currentGain = 200,gainSlope = 5,hotspotRad1 = 30,hotspotRad2 = 70;
int hotspotOffsetR = 10, hotspotOffsetG = 15, hotspotOffsetB = 3;
float pi = 0.0;
ushort*	LUTValues;
uint8_t* LUTValues_8;
uint8_t* LUTValues_8R;
uint8_t* LUTValues_8G;
uint8_t* LUTValues_8B;
int* histArrayB;
int* histArrayG;
int* histArrayR;
int* CumhistArrayB;
int* CumhistArrayG;
int* CumhistArrayR;
int pixelMinB = 0,pixelMaxB =0 , pixelMinR = 0,pixelMaxR =0, pixelMinG = 0,pixelMaxG =0;
//IplImage* srcImg ;
Mat srcImg ;

//IplImage* srcImg1;
Mat srcImg1;
uint16_t*	chr_Gr;
uint16_t*	chr_R ;
uint16_t*	chr_B ;
uint16_t*	chr_Gb;
uint16_t*	chr_G ;

uint8_t*	chr_Gr_8;
uint8_t*	chr_R_8 ;
uint8_t*	chr_B_8 ;
uint8_t*	chr_Gb_8;
uint8_t*	chr_G_8 ;

struct ImageParamsStruct
{
	 double Entropy_red_shadow_mask_1;
	double Entropy_green_shadow_mask_1;
	double Entropy_blue_shadow_mask_1;

	double Entropy_red_shadow_mask_2;
	double Entropy_green_shadow_mask_2;
	double Entropy_blue_shadow_mask_2;

	double Entropy_red_shadow_mask_3;
	double Entropy_green_shadow_mask_3;
	double Entropy_blue_shadow_mask_3;

	double Entropy_red_shadow_mask_4;
	double Entropy_green_shadow_mask_4;
	double Entropy_blue_shadow_mask_4;

	double Entropy_red_shadow_mask_5;
	double Entropy_green_shadow_mask_5;
	double Entropy_blue_shadow_mask_5;

	double Entropy_red_shadow_mask_6;
	double Entropy_green_shadow_mask_6;
	double Entropy_blue_shadow_mask_6;

	double Entropy_red_shadow_mask_7;
	double Entropy_green_shadow_mask_7;
	double Entropy_blue_shadow_mask_7;

	double Entropy_red_shadow_mask_8;
	double Entropy_green_shadow_mask_8;
	double Entropy_blue_shadow_mask_8;

	double Entropy_red_circum_mask_1;
	double Entropy_green_circum_mask_1;
	double Entropy_blue_circum_mask_1;

	double Entropy_red_circum_mask_2;
	double Entropy_green_circum_mask_2;
	double Entropy_blue_circum_mask_2;

	double Entropy_red_circum_mask_3;
	double Entropy_green_circum_mask_3;
	double Entropy_blue_circum_mask_3;

	double Entropy_red_circum_mask_4;
	double Entropy_green_circum_mask_4;
	double Entropy_blue_circum_mask_4;

	double Entropy_red_circum_mask_5;
	double Entropy_green_circum_mask_5;
	double Entropy_blue_circum_mask_5;

	double Entropy_red_circum_mask_6;
	double Entropy_green_circum_mask_6;
	double Entropy_blue_circum_mask_6;

	double Entropy_red_circum_mask_7;
	double Entropy_green_circum_mask_7;
	double Entropy_blue_circum_mask_7;

	double Entropy_red_circum_mask_8;
	double Entropy_green_circum_mask_8;
	double Entropy_blue_circum_mask_8;

	double STD_red_shadow_mask_1;
	double STD_green_shadow_mask_1;
	double STD_blue_shadow_mask_1;

	double STD_red_shadow_mask_2;
	double STD_green_shadow_mask_2;
	double STD_blue_shadow_mask_2;

	double STD_red_shadow_mask_3;
	double STD_green_shadow_mask_3;
	double STD_blue_shadow_mask_3;

	double STD_red_shadow_mask_4;
	double STD_green_shadow_mask_4;
	double STD_blue_shadow_mask_4;

	double STD_red_shadow_mask_5;
	double STD_green_shadow_mask_5;
	double STD_blue_shadow_mask_5;

	double STD_red_shadow_mask_6;
	double STD_green_shadow_mask_6;
	double STD_blue_shadow_mask_6;

	double STD_red_shadow_mask_7;
	double STD_green_shadow_mask_7;
	double STD_blue_shadow_mask_7;

	double STD_red_shadow_mask_8;
	double STD_green_shadow_mask_8;
	double STD_blue_shadow_mask_8;

	double STD_red_circum_mask_1;
	double STD_green_circum_mask_1;
	double STD_blue_circum_mask_1;

	double STD_red_circum_mask_2;
	double STD_green_circum_mask_2;
	double STD_blue_circum_mask_2;

	double STD_red_circum_mask_3;
	double STD_green_circum_mask_3;
	double STD_blue_circum_mask_3;

	double STD_red_circum_mask_4;
	double STD_green_circum_mask_4;
	double STD_blue_circum_mask_4;

	double STD_red_circum_mask_5;
	double STD_green_circum_mask_5;
	double STD_blue_circum_mask_5;

	double STD_red_circum_mask_6;
	double STD_green_circum_mask_6;
	double STD_blue_circum_mask_6;

	double STD_red_circum_mask_7;
	double STD_green_circum_mask_7;
	double STD_blue_circum_mask_7;

	double STD_red_circum_mask_8;
	double STD_green_circum_mask_8;
	double STD_blue_circum_mask_8;

	double Entropy_red_full;
	double Entropy_green_full;
	double Entropy_blue_full;

	double Entropy_red_shadowed;
	double Entropy_green_shadowed;
	double Entropy_blue_shadowed;

	double Entropy_red_circum;
	double Entropy_green_circum;
	double Entropy_blue_circum;

	double Mean_red_full;
	double Mean_green_full;
	double Mean_blue_full;

	double Mean_red_shadowed;
	double Mean_green_shadowed;
	double Mean_blue_shadowed;

	double Mean_red_circum;
	double Mean_green_circum;
	double Mean_blue_circum;

	double std_red_full;
	double std_green_full;
	double std_blue_full;

	double std_red_shadowed;
	double std_green_shadowed;
	double std_blue_shadowed;

	double std_red_circum;
	double std_green_circum;
	double std_blue_circum;

	double percentageEntropyChange_R;
	double percentageEntropyChange_G;
	double percentageEntropyChange_B;

	double percentageSTDChange_R;
	double percentageSTDChange_G;
	double percentageSTDChange_B;
};

// cvCreateImage- creates image header and saves it.
// this method uses cvCreateImage and later saves the image along with info of num of btes

extern "C" __declspec(dllexport) void ImageProc_writeBytes(char* ptr)
{
	IplImage* temp = cvCreateImage(cvSize(3072,2048),IPL_DEPTH_16U,1);
	temp->imageData = ptr;
	//cvShowImage("temp",temp);
	//cvNamedWindow("temp",CV_WINDOW_AUTOSIZE);
	cvSaveImage("rawImage.raw",temp);
}
// initization , creates image header using cvCreateImage and gives it pointers.
//cvDrawCircle used draw a circle in red-maskImg1 = cvCreateImage(cvGetSize(redImg),IPL_DEPTH_8U,1);
extern "C" __declspec(dllexport) void ImageProc_Demosaic_Init(int IMG_WIDTH, int IMG_HEIGHT ,bool isFourteenBit,bool isRawMode,int centreX,int centreY )
{
	/*free(chr_B);
	free(chr_Gr);
	free(chr_Gb);
	free(chr_R);*/
	pi = 4 * std::atan(1);
	int TMP_HEIGHT = IMG_HEIGHT+2;
	int TMP_WIDTH = IMG_WIDTH +2;
	if(isRawMode)
	{
	if(IMG_WIDTH > 2048)
	{
		chr_Gr = (uint16_t*) malloc(sizeof(uint16_t)*TMP_WIDTH*TMP_HEIGHT);
		chr_R = (uint16_t*) malloc(sizeof(uint16_t)*TMP_WIDTH*TMP_HEIGHT);
		chr_B = (uint16_t*) malloc(sizeof(uint16_t)*TMP_WIDTH*TMP_HEIGHT);
		chr_Gb = (uint16_t*) malloc(sizeof(uint16_t)*TMP_WIDTH*TMP_HEIGHT);
		chr_G = (uint16_t*) malloc(sizeof(uint16_t)*TMP_WIDTH*TMP_HEIGHT);
	}
	else
	{
		chr_Gr_8 =( unsigned char*) malloc(sizeof(unsigned char)*TMP_WIDTH*TMP_HEIGHT);
		chr_R_8 = (unsigned char*) malloc(sizeof(unsigned char)*TMP_WIDTH*TMP_HEIGHT);
		chr_B_8 = (unsigned char*) malloc(sizeof(unsigned char)*TMP_WIDTH*TMP_HEIGHT);
		chr_Gb_8 = (unsigned char*) malloc(sizeof(unsigned char)*TMP_WIDTH*TMP_HEIGHT);
		chr_G_8 = (unsigned char*) malloc(sizeof(unsigned char)*TMP_WIDTH*TMP_HEIGHT);

	}
	}

	parabolaImage = Mat(IMG_HEIGHT,IMG_HEIGHT,CV_32FC1);
	parabolaImage_float = (float *)malloc(sizeof(float)*IMG_WIDTH*IMG_HEIGHT);
	/*parabolaHotspotImage = cvCreateImage(cvSize(IMG_WIDTH,IMG_HEIGHT),IPL_DEPTH_32F,1);
	inp = cvCreateImage(cvSize(IMG_WIDTH, IMG_HEIGHT),IPL_DEPTH_8U,3);      
	inpH = cvCreateImage(cvSize(IMG_WIDTH,IMG_HEIGHT),IPL_DEPTH_8U,1);      
	inpS = cvCreateImage(cvSize(IMG_WIDTH,IMG_HEIGHT),IPL_DEPTH_8U,1);      
	inpV = cvCreateImage(cvSize(IMG_WIDTH,IMG_HEIGHT),IPL_DEPTH_8U,1);      
	inp2 = cvCreateImage(cvSize(IMG_WIDTH,IMG_HEIGHT),IPL_DEPTH_32F,1);    
	redImg = cvCreateImage(cvSize(IMG_WIDTH,IMG_HEIGHT),IPL_DEPTH_8U,1);
	greenImg = cvCreateImage(cvSize(IMG_WIDTH,IMG_HEIGHT),IPL_DEPTH_8U,1);
	blueImg = cvCreateImage(cvSize(IMG_WIDTH,IMG_HEIGHT),IPL_DEPTH_8U,1);*/

	parabolaHotspotImage = Mat( IMG_HEIGHT,IMG_WIDTH,CV_32FC1);
	inp =  Mat(IMG_HEIGHT,IMG_WIDTH,CV_8UC3);      
	inp2 = Mat(IMG_HEIGHT,IMG_WIDTH,CV_32FC1);    
	/*actual_ROI = 	cvCreateImage(cvGetSize(redImg),redImg->depth,inp->nChannels);
	circum_ROI = 	cvCreateImage(cvGetSize(redImg),redImg->depth,inp->nChannels);
	Rim = 	cvCreateImage(cvGetSize(redImg),redImg->depth,inp->nChannels);
	EntropyImg = cvCreateImage(cvGetSize(redImg),IPL_DEPTH_8U,3);
	inner_mask_boundry =cvCreateImage(cvGetSize(redImg),redImg->depth,inp->nChannels);
	outer_mask_boundry =cvCreateImage(cvGetSize(redImg),redImg->depth,inp->nChannels);
	circum_mask_boundry =cvCreateImage(cvGetSize(redImg),redImg->depth,inp->nChannels);*/

	srcImg = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8UC3);
	temp = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8UC3);
	srcImg1 =Mat(IMG_HEIGHT,IMG_WIDTH,CV_8UC3);
	grayImg = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8UC1);
	maskImg1 = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8U); 
	maskImg2 = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8U); 
	maskImg3 = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8UC1); 
	maskImg4 = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8U); 
	/*maskImg23 = cvCreateImage(cvGetSize(redImg),IPL_DEPTH_8U,1);
	maskImg12 = cvCreateImage(cvGetSize(redImg),IPL_DEPTH_8U,1);*/

	CaptureMask = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8UC1);
	LiveMask = Mat(IMG_HEIGHT,IMG_WIDTH,CV_8UC1);
	histArrayB = (int*) malloc(sizeof(int)*1024);
	histArrayG = (int*) malloc(sizeof(int)*1024);
	histArrayR = (int*) malloc(sizeof(int)*1024);
	CumhistArrayB = (int*) malloc(sizeof(int)*1024);
	CumhistArrayG = (int*) malloc(sizeof(int)*1024);
	CumhistArrayR = (int*) malloc(sizeof(int)*1024);
	
	// 14-bit imagedate, first row is GB, 
	            int r1 = 120;
                int r2 = 300;
                int r3 = 600;
                int r4 = 900;
				Scalar color = Scalar(255);
				circle(maskImg1,Point(centreX,centreY),r1,color,-1,8);
				circle(maskImg2,Point(centreX,centreY),r2,color,-1,8);
				circle(maskImg3,Point(centreX,centreY),r3,color,-1,8);
				circle(maskImg4,Point(centreX,centreY),r4,color,-1,8);
	/*cvDrawCircle(maskImg1,cvPoint(centreX,centreY),r1,cvScalarAll(255),-1,8);
	cvDrawCircle(maskImg2,cvPoint(centreX,centreY),r2,cvScalarAll(255),-1,8);
	cvDrawCircle(maskImg3,cvPoint(centreX,centreY),r3,cvScalarAll(255),-1,8);
	cvDrawCircle(maskImg4,cvPoint(centreX,centreY),r4,cvScalarAll(255),-1,8);*/
}

// creates an object which is pointer too and returns it.
extern "C" __declspec(dllexport)ImageParamsStruct *ImageProc_CalculateImageParams( char* ImgPtr,int width,int height, int method, int CenterX, int CenterY, int Rim_thickness,int Shadow_thickness, int Hotspot_thickness)
{
	ImageParamsStruct *_ImageParamsStruct = new  ImageParamsStruct();

	////EntropyImg = cvCreateImage(cvSize(width,height),IPL_DEPTH_8U,3);
	//if(EntropyImg->width == width && EntropyImg->height == height)
	//{
	//	EntropyImg->imageData = ImgPtr;
	//	//cvCopy(ImgPtr, EntropyImg);
	//	CvScalar mean,std,mean_shadow,std_shadow,mean_rim,std_rim;
	//	//1:- Entropy_rectangle (uncomment cvDrawRect), Entropy_circular, STD_circular, 2:- STD_circular_HotspotMask
	//	//********************************************************************************************************************

	//	if (method==1)
	//	{
	//		Center_shadow = cvPoint(CenterX,CenterY);
	//		radius_shadow = Shadow_thickness;
	//		rim_thickness = Shadow_thickness +Rim_thickness;

	//		GetShadowMask(Center_shadow,radius_shadow,actual_ROI);
	//		GetShadowMask(Center_shadow,rim_thickness,circum_ROI);

	//		cvSub(circum_ROI,actual_ROI,circum_ROI);
	//		cvAnd(EntropyImg, actual_ROI,actual_ROI);
	//		cvAnd(EntropyImg,circum_ROI,circum_ROI);

	//	}
	//	else if (method==2)
	//	{
	//		Center_shadow = cvPoint(CenterX,CenterY);
	//		radius_hotspot = Hotspot_thickness;
	//		radius_shadow = Shadow_thickness;
	//		rim_thickness = Shadow_thickness +Rim_thickness;

	//		GetROI(actual_ROI,circum_ROI);

	//		cvAnd(EntropyImg, actual_ROI,actual_ROI);
	//		cvAnd(EntropyImg, circum_ROI,circum_ROI);

	//	}


	//	/* if (method==3)
	//	{
	//	Center_shadow = cvPoint(CenterX,CenterY);
	//	radius_shadow = Shadow_thickness;
	//	rim_thickness = Shadow_thickness +Rim_thickness;

	//	GetRectangle(Center_shadow,radius_hotspot,inner_mask_boundry);
	//	rect_shadow =	GetRectangle(Center_shadow,radius_shadow,outer_mask_boundry);
	//	rect_circum = GetRectangle(Center_shadow,rim_thickness,circum_mask_boundry);

	//	}*/




	//	cvSplit(actual_ROI,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_blue_shadowed =	GetEntropy(blueImg ,NULL);
	//	_ImageParamsStruct->Entropy_green_shadowed = GetEntropy(greenImg,NULL);
	//	_ImageParamsStruct->Entropy_red_shadowed =	 GetEntropy(redImg,NULL);

	//	cvAvgSdv(actual_ROI,&mean_shadow,&std_shadow);

	//	_ImageParamsStruct->Mean_blue_shadowed = mean_shadow.val[0];
	//	_ImageParamsStruct->Mean_green_shadowed =mean_shadow.val[1];
	//	_ImageParamsStruct->Mean_red_shadowed =	mean_shadow.val[2];

	//	_ImageParamsStruct->std_blue_shadowed = std_shadow.val[0];
	//	_ImageParamsStruct->std_green_shadowed =std_shadow.val[1];
	//	_ImageParamsStruct->std_red_shadowed =	std_shadow.val[2];
	//	////***********************************************************************************************************************************************


	//	cvSplit(circum_ROI,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum  = GetEntropy(redImg,NULL);
	//	_ImageParamsStruct->Entropy_green_circum = GetEntropy(greenImg,NULL);
	//	_ImageParamsStruct->Entropy_blue_circum = GetEntropy(blueImg,NULL);


	//	cvAvgSdv(circum_ROI,&mean_rim,&std_rim);
	//	//cvSaveImage("circumImag.png" , circum_ROI);
	//	_ImageParamsStruct->Mean_blue_circum = mean_rim.val[0];
	//	_ImageParamsStruct->Mean_green_circum =mean_rim.val[1];
	//	_ImageParamsStruct->Mean_red_circum =	mean_rim.val[2];

	//	_ImageParamsStruct->std_blue_circum = std_rim.val[0];
	//	_ImageParamsStruct->std_green_circum =std_rim.val[1];
	//	_ImageParamsStruct->std_red_circum =  std_rim.val[2];

	//	_ImageParamsStruct->percentageEntropyChange_B = GetPercentageChange(_ImageParamsStruct->Entropy_blue_shadowed,_ImageParamsStruct->Entropy_blue_circum);
	//	_ImageParamsStruct->percentageEntropyChange_G = GetPercentageChange(_ImageParamsStruct->Entropy_green_shadowed,_ImageParamsStruct->Entropy_green_circum);
	//	_ImageParamsStruct->percentageEntropyChange_R = GetPercentageChange(_ImageParamsStruct->Entropy_red_shadowed,_ImageParamsStruct->Entropy_red_circum);

	//	_ImageParamsStruct->percentageSTDChange_B = GetPercentageChange(_ImageParamsStruct->std_blue_shadowed,_ImageParamsStruct->std_blue_circum);
	//	_ImageParamsStruct->percentageSTDChange_G = GetPercentageChange(_ImageParamsStruct->std_green_shadowed,_ImageParamsStruct->std_green_circum);
	//	_ImageParamsStruct->percentageSTDChange_R = GetPercentageChange(_ImageParamsStruct->std_red_shadowed,_ImageParamsStruct->std_red_circum);

	//	cvSub (outer_mask_boundry,inner_mask_boundry,actual_ROI);
	//	cvAnd(EntropyImg,actual_ROI,actual_ROI);
	//	point_1_shadow = cvPoint(rect_shadow.x,rect_shadow.y);
	//	point_2_shadow = cvPoint(rect_shadow.x+rect_shadow.width/2,rect_shadow.y);
	//	point_3_shadow = cvPoint(rect_shadow.x+rect_shadow.width,rect_shadow.y);
	//	point_4_shadow = cvPoint(rect_shadow.x+rect_shadow.width,rect_shadow.y+rect_shadow.height/2);
	//	point_5_shadow = cvPoint(rect_shadow.x+rect_shadow.width,rect_shadow.y+rect_shadow.height);
	//	point_6_shadow = cvPoint(rect_shadow.x+rect_shadow.width/2,rect_shadow.y+rect_shadow.height);
	//	point_7_shadow = cvPoint(rect_shadow.x,rect_shadow.y+rect_shadow.height);
	//	point_8_shadow = cvPoint(rect_shadow.x,rect_shadow.y+rect_shadow.height/2);

	//	GetROO_Mask(shadow_mask_1,Center_shadow,point_1_shadow,point_2_shadow);
	//	cvAnd(actual_ROI,shadow_mask_1,shadow_mask_1);
	//	cvSplit(shadow_mask_1,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_shadow_mask_1=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_shadow_mask_1 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_shadow_mask_1=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_1,&mean_shadow_mask_1,&std_shadow_mask_1);

	//	_ImageParamsStruct->STD_red_shadow_mask_1=	std_shadow_mask_1.val[0];
	//	_ImageParamsStruct->STD_green_shadow_mask_1 =	std_shadow_mask_1.val[1];
	//	_ImageParamsStruct->STD_blue_shadow_mask_1=	std_shadow_mask_1.val[2];

	//	GetROO_Mask(shadow_mask_2,Center_shadow,point_2_shadow,point_3_shadow);
	//	cvAnd(actual_ROI,shadow_mask_2,shadow_mask_2);
	//	cvSplit(shadow_mask_2,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_shadow_mask_2=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_shadow_mask_2 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_shadow_mask_2=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_2,&mean_shadow_mask_2,&std_shadow_mask_2);

	//	_ImageParamsStruct->STD_red_shadow_mask_2=	std_shadow_mask_2.val[0];
	//	_ImageParamsStruct->STD_green_shadow_mask_2 =	std_shadow_mask_2.val[1];
	//	_ImageParamsStruct->STD_blue_shadow_mask_2=	std_shadow_mask_2.val[2];

	//	GetROO_Mask(shadow_mask_3,Center_shadow,point_3_shadow,point_4_shadow);
	//	cvAnd(actual_ROI,shadow_mask_3,shadow_mask_3);
	//	cvSplit(shadow_mask_3,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_shadow_mask_3=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_shadow_mask_3 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_shadow_mask_3=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_3,&mean_shadow_mask_3,&std_shadow_mask_3);

	//	_ImageParamsStruct->STD_red_shadow_mask_3=	std_shadow_mask_3.val[0];
	//	_ImageParamsStruct->STD_green_shadow_mask_3 =	std_shadow_mask_3.val[1];
	//	_ImageParamsStruct->STD_blue_shadow_mask_3=	std_shadow_mask_3.val[2];

	//	GetROO_Mask(shadow_mask_4,Center_shadow,point_4_shadow,point_5_shadow);
	//	cvAnd(actual_ROI,shadow_mask_4,shadow_mask_4);
	//	cvSplit(shadow_mask_4,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_shadow_mask_4=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_shadow_mask_4 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_shadow_mask_4=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_4,&mean_shadow_mask_4,&std_shadow_mask_4);

	//	_ImageParamsStruct->STD_red_shadow_mask_4=	std_shadow_mask_4.val[0];
	//	_ImageParamsStruct->STD_green_shadow_mask_4 =	std_shadow_mask_4.val[1];
	//	_ImageParamsStruct->STD_blue_shadow_mask_4=	std_shadow_mask_4.val[2];

	//	GetROO_Mask(shadow_mask_5,Center_shadow,point_5_shadow,point_6_shadow);
	//	cvAnd(actual_ROI,shadow_mask_5,shadow_mask_5);

	//	cvSplit(shadow_mask_5,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_shadow_mask_5=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_shadow_mask_5 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_shadow_mask_5=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_5,&mean_shadow_mask_5,&std_shadow_mask_5);

	//	_ImageParamsStruct->STD_red_shadow_mask_5=	std_shadow_mask_5.val[0];
	//	_ImageParamsStruct->STD_green_shadow_mask_5 =	std_shadow_mask_5.val[1];
	//	_ImageParamsStruct->STD_blue_shadow_mask_5=	std_shadow_mask_5.val[2];

	//	GetROO_Mask(shadow_mask_6,Center_shadow,point_6_shadow,point_7_shadow);
	//	cvAnd(actual_ROI,shadow_mask_6,shadow_mask_6);

	//	cvSplit(shadow_mask_6,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_shadow_mask_6=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_shadow_mask_6 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_shadow_mask_6=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_6,&mean_shadow_mask_6,&std_shadow_mask_6);

	//	_ImageParamsStruct->STD_red_shadow_mask_6=	std_shadow_mask_6.val[0];
	//	_ImageParamsStruct->STD_green_shadow_mask_6 =	std_shadow_mask_6.val[1];
	//	_ImageParamsStruct->STD_blue_shadow_mask_6=	std_shadow_mask_6.val[2];

	//	GetROO_Mask(shadow_mask_7,Center_shadow,point_7_shadow,point_8_shadow);
	//	cvAnd(actual_ROI,shadow_mask_7,shadow_mask_7);
	//	cvSplit(shadow_mask_7,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_shadow_mask_7=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_shadow_mask_7 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_shadow_mask_7=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_7,&mean_shadow_mask_7,&std_shadow_mask_7);

	//	_ImageParamsStruct->STD_red_shadow_mask_7=	std_shadow_mask_7.val[0];
	//	_ImageParamsStruct->STD_green_shadow_mask_7 =	std_shadow_mask_7.val[1];
	//	_ImageParamsStruct->STD_blue_shadow_mask_7=	std_shadow_mask_7.val[2];

	//	GetROO_Mask(shadow_mask_8,Center_shadow,point_8_shadow,point_1_shadow);
	//	cvAnd(actual_ROI,shadow_mask_8,shadow_mask_8);
	//	cvSplit(shadow_mask_8,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_shadow_mask_8=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_shadow_mask_8 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_shadow_mask_8=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_8,&mean_shadow_mask_8,&std_shadow_mask_8);

	//	_ImageParamsStruct->STD_red_shadow_mask_8=	std_shadow_mask_8.val[0];
	//	_ImageParamsStruct->STD_green_shadow_mask_8 =	std_shadow_mask_8.val[1];
	//	_ImageParamsStruct->STD_blue_shadow_mask_8=	std_shadow_mask_8.val[2];


	//	cvSub (circum_mask_boundry,outer_mask_boundry,circum_ROI);
	//	cvAnd(EntropyImg,circum_ROI,circum_ROI);
	//	point_1_circum = cvPoint(rect_circum.x,rect_circum.y);
	//	point_2_circum = cvPoint(rect_circum.x+rect_circum.width/2,rect_circum.y);
	//	point_3_circum = cvPoint(rect_circum.x+rect_circum.width,rect_circum.y);
	//	point_4_circum = cvPoint(rect_circum.x+rect_circum.width,rect_circum.y+rect_circum.height/2);
	//	point_5_circum = cvPoint(rect_circum.x+rect_circum.width,rect_circum.y+rect_circum.height);
	//	point_6_circum = cvPoint(rect_circum.x+rect_circum.width/2,rect_circum.y+rect_circum.height);
	//	point_7_circum = cvPoint(rect_circum.x,rect_circum.y+rect_circum.height);
	//	point_8_circum = cvPoint(rect_circum.x,rect_circum.y+rect_circum.height/2);

	//	GetROO_Mask(shadow_mask_1,Center_circum,point_1_circum,point_2_circum);
	//	cvAnd(actual_ROI,shadow_mask_1,shadow_mask_1);
	//	cvSplit(shadow_mask_1,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum_mask_1=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_circum_mask_1 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_circum_mask_1=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_1,&mean_circum_mask_1,&std_circum_mask_1);

	//	_ImageParamsStruct->STD_red_circum_mask_1=	std_circum_mask_1.val[0];
	//	_ImageParamsStruct->STD_green_circum_mask_1 =	std_circum_mask_1.val[1];
	//	_ImageParamsStruct->STD_blue_circum_mask_1=	std_circum_mask_1.val[2];

	//	GetROO_Mask(shadow_mask_2,Center_circum,point_2_circum,point_3_circum);
	//	cvAnd(actual_ROI,shadow_mask_2,shadow_mask_2);
	//	cvSplit(shadow_mask_2,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum_mask_2=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_circum_mask_2 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_circum_mask_2=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_2,&mean_circum_mask_2,&std_circum_mask_2);

	//	_ImageParamsStruct->STD_red_circum_mask_2=	std_circum_mask_2.val[0];
	//	_ImageParamsStruct->STD_green_circum_mask_2 =	std_circum_mask_2.val[1];
	//	_ImageParamsStruct->STD_blue_circum_mask_2=	std_circum_mask_2.val[2];

	//	GetROO_Mask(shadow_mask_3,Center_circum,point_3_circum,point_4_circum);
	//	cvAnd(actual_ROI,shadow_mask_3,shadow_mask_3);
	//	cvSplit(shadow_mask_3,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum_mask_3=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_circum_mask_3 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_circum_mask_3=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_3,&mean_circum_mask_3,&std_circum_mask_3);

	//	_ImageParamsStruct->STD_red_circum_mask_3=	std_circum_mask_3.val[0];
	//	_ImageParamsStruct->STD_green_circum_mask_3 =	std_circum_mask_3.val[1];
	//	_ImageParamsStruct->STD_blue_circum_mask_3=	std_circum_mask_3.val[2];

	//	GetROO_Mask(shadow_mask_4,Center_circum,point_4_circum,point_5_circum);
	//	cvAnd(actual_ROI,shadow_mask_4,shadow_mask_4);
	//	cvSplit(shadow_mask_4,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum_mask_4=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_circum_mask_4 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_circum_mask_4=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_4,&mean_circum_mask_4,&std_circum_mask_4);

	//	_ImageParamsStruct->STD_red_circum_mask_4=	std_circum_mask_4.val[0];
	//	_ImageParamsStruct->STD_green_circum_mask_4 =	std_circum_mask_4.val[1];
	//	_ImageParamsStruct->STD_blue_circum_mask_4=	std_circum_mask_4.val[2];

	//	GetROO_Mask(shadow_mask_5,Center_circum,point_5_circum,point_6_circum);
	//	cvAnd(actual_ROI,shadow_mask_5,shadow_mask_5);

	//	cvSplit(shadow_mask_5,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum_mask_5=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_circum_mask_5 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_circum_mask_5=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_5,&mean_circum_mask_5,&std_circum_mask_5);

	//	_ImageParamsStruct->STD_red_circum_mask_5=	std_circum_mask_5.val[0];
	//	_ImageParamsStruct->STD_green_circum_mask_5 =	std_circum_mask_5.val[1];
	//	_ImageParamsStruct->STD_blue_circum_mask_5=	std_circum_mask_5.val[2];

	//	GetROO_Mask(shadow_mask_6,Center_circum,point_6_circum,point_7_circum);
	//	cvAnd(actual_ROI,shadow_mask_6,shadow_mask_6);

	//	cvSplit(shadow_mask_6,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum_mask_6=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_circum_mask_6 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_circum_mask_6=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_6,&mean_circum_mask_6,&std_circum_mask_6);

	//	_ImageParamsStruct->STD_red_circum_mask_6=	std_circum_mask_6.val[0];
	//	_ImageParamsStruct->STD_green_circum_mask_6 =	std_circum_mask_6.val[1];
	//	_ImageParamsStruct->STD_blue_circum_mask_6=	std_circum_mask_6.val[2];

	//	GetROO_Mask(shadow_mask_7,Center_circum,point_7_circum,point_8_circum);
	//	cvAnd(actual_ROI,shadow_mask_7,shadow_mask_7);
	//	cvSplit(shadow_mask_7,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum_mask_7=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_circum_mask_7 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_circum_mask_7=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_7,&mean_circum_mask_7,&std_circum_mask_7);

	//	_ImageParamsStruct->STD_red_circum_mask_7=	std_circum_mask_7.val[0];
	//	_ImageParamsStruct->STD_green_circum_mask_7 =	std_circum_mask_7.val[1];
	//	_ImageParamsStruct->STD_blue_circum_mask_7=	std_circum_mask_7.val[2];

	//	GetROO_Mask(shadow_mask_8,Center_circum,point_8_circum,point_1_circum);
	//	cvAnd(actual_ROI,shadow_mask_8,shadow_mask_8);
	//	cvSplit(shadow_mask_8,blueImg,greenImg,redImg,0);

	//	_ImageParamsStruct->Entropy_red_circum_mask_8=	GetEntropy(blueImg);
	//	_ImageParamsStruct->Entropy_green_circum_mask_8 =	GetEntropy(greenImg);
	//	_ImageParamsStruct->Entropy_blue_circum_mask_8=	GetEntropy(redImg);

	//	cvAvgSdv(shadow_mask_8,&mean_circum_mask_8,&std_circum_mask_8);

	//	_ImageParamsStruct->STD_red_circum_mask_8=	std_circum_mask_8.val[0];
	//	_ImageParamsStruct->STD_green_circum_mask_8 =	std_circum_mask_8.val[1];
	//	_ImageParamsStruct->STD_blue_circum_mask_8=	std_circum_mask_8.val[2];
	//	
	//}

	return _ImageParamsStruct;
}
struct HotSpotParams
{
	double r1Entropy[3];
	double r2Entropy[3];
	double r3Entropy[3];
	double r4Entropy[3];

	double r1STD[3];
	double r2STD[3];
	double r3STD[3];
	double r4STD[3];

	double r1Brighntess[3];
	double r2Brighntess[3];
	double r3Brighntess[3];
	double r4Brighntess[3];

	double r1Contrast[3];
	double r2Contrast[3];
	double r3Contrast[3];
	double r4Contrast[3];

	/*double r23Contrast[3];
	double r23Brighntess[3];
	double r23STD[3];
	double r23Entropy[3];

	double r12Contrast[3];
	double r12Brighntess[3];
	double r12STD[3];
	double r12Entropy[3];*/

	double FocusVal[3];

};

 
//splits the source image(multi channel) to single channel.
//gets entrophy value of green channel and using object it's given to r3Entropy.
//performs cvscalarAll function is used.
//cvAvSdv-Calculates mean and standard deviation of pixel values- it's param's are srcImage, mean, std and maskImg3.
//STD values are given to r3STD using object.same way mean value is given to r3Brighntess.
//finally calculates using calculate_Contrast_Brightness_Std and returns the object.
extern "C" __declspec(dllexport) HotSpotParams* ImageProc_CalculateHotspotImageParams(char* ImgPtr,int CenterX, int CenterY,int width,int height,int r1,int r2,int r3,int r4,IplImage* redChanImg)
{
	HotSpotParams * _HotSpotParams = new  HotSpotParams();



	// Create Mask by drawing circle of radii r1 r2 r3
	

	/*cvCopy(maskImg2,maskImg12);
	cvCopy(maskImg3,maskImg23);*/

	//cvSub(maskImg2,maskImg1,maskImg4); // create mask by subtracting mask1 with mask2
	//cvSub(maskImg3,maskImg2,maskImg3); // create mask by subtracting mas3 with mask2
	
	srcImg.data =(uchar*) ImgPtr;
	//srcImg->imageData = ImgPtr;
	Mat bgr[3];
	cv::split(srcImg,bgr);
	//cvSplit(srcImg,blueImg,greenImg,redChanImg,0);

	//cvCvtColor(srcImg,srcImg1,CV_BGR2HSV);
	//cvSplit(srcImg1,0,0,blueImg,0);
	// Calculate Entropy for  radius 1
	//_HotSpotParams->r1Entropy[0] =	GetEntropy(blueImg,maskImg1);
	//_HotSpotParams->r1Entropy[1] =	GetEntropy(greenImg,maskImg1);
	//_HotSpotParams->r1Entropy[2] =	GetEntropy(redImg,maskImg1);
	//
	//	// Calculate Entropy for region beteen radius 1 and radius 2
	//_HotSpotParams->r2Entropy[0] =	GetEntropy(blueImg,maskImg2);
	//_HotSpotParams->r2Entropy[1] =	GetEntropy(greenImg,maskImg2);
	//_HotSpotParams->r2Entropy[2] =	GetEntropy(redImg,maskImg2);

		// Calculate Entropy for region beteen radius 2 and radius 3
	//_HotSpotParams->r3Entropy[0] =	GetEntropy(blueImg,maskImg3);
	_HotSpotParams->r3Entropy[1] =	GetEntropy(bgr[1],maskImg3);
	//_HotSpotParams->r3Entropy[2] =	GetEntropy(redImg,maskImg3);

		/*_HotSpotParams->r4Entropy[0] =	GetEntropy(blueImg,maskImg4);
	_HotSpotParams->r4Entropy[1] =	GetEntropy(greenImg,maskImg4);
	_HotSpotParams->r4Entropy[2] =	GetEntropy(redImg,maskImg4);*/

	 /*	cvSetImageROI(blueImg,cvRect(CenterX-r3/2,CenterY-r3/2, r3, r3));
	 	cvSetImageROI(greenImg,cvRect(CenterX-r3/2,CenterY-r3/2, r3, r3));
	 	cvSetImageROI(redImg,cvRect(CenterX-r3/2,CenterY-r3/2, r3, r3));*/

	/*_HotSpotParams->r23Entropy[0] =	GetEntropy(blueImg,NULL);
	_HotSpotParams->r23Entropy[1] =	GetEntropy(greenImg,NULL);
	_HotSpotParams->r23Entropy[2] =	GetEntropy(redImg,NULL);*/
	/*_HotSpotParams->r4Entropy[0] =	GetEntropy(blueImg,maskImg4);
	_HotSpotParams->r4Entropy[1] =	GetEntropy(greenImg,maskImg4);
	_HotSpotParams->r4Entropy[2] =	GetEntropy(redImg,maskImg4);*/
		/*cvResetImageROI(blueImg);
		cvResetImageROI(greenImg);
		cvResetImageROI(redImg);*/

	/*CvScalar mean = cvScalarAll(0);
	CvScalar STD = cvScalarAll(0);*/
	Scalar meanVal ;//= Scalar(0,0,0,0);
	Scalar STDVal;//= Scalar(0,0,0,0);
		// Calculate Standard deviation for  radius 1
	/*cvAvgSdv(srcImg,&mean,&STD,maskImg1);
	_HotSpotParams->r1STD[0] =	STD.val[0];
	_HotSpotParams->r1STD[1] =	STD.val[1];
	_HotSpotParams->r1STD[2] =	STD.val[2];

	_HotSpotParams->r1Brighntess[0] =	mean.val[0];
	_HotSpotParams->r1Brighntess[1] =	mean.val[1];
	_HotSpotParams->r1Brighntess[2] =	mean.val[2];
	calculate_Contrast_Brightness_Std(blueImg,greenImg,redImg,_HotSpotParams->r1Brighntess,_HotSpotParams->r1Contrast,maskImg1);*/
		// Calculate  Standard deviation for region beteen radius 1 and radius 2
	/*cvAvgSdv(srcImg,&mean,&STD,maskImg2);
	

	_HotSpotParams->r2STD[0] =	STD.val[0];
	_HotSpotParams->r2STD[1] =	STD.val[1];
	_HotSpotParams->r2STD[2] =	STD.val[2];

	_HotSpotParams->r2Brighntess[0] =	mean.val[0];
	_HotSpotParams->r2Brighntess[1] =	mean.val[1];
	_HotSpotParams->r2Brighntess[2] =	mean.val[2];
	calculate_Contrast_Brightness_Std(blueImg,greenImg,redImg,_HotSpotParams->r2Brighntess,_HotSpotParams->r2Contrast,maskImg2);*/

		// Calculate  Standard deviation for region beteen radius 2 and radius 3
	//cvAvgSdv(srcImg,&mean,&STD,maskImg3);
	//meanStdDev(srcImg,meanVal,STDVal,maskImg3);
	/*Mat maskTemp ; 
	maskImg3.convertTo(maskTemp,CV_8UC3);*/
	meanStdDev(bgr[0],meanVal,STDVal,maskImg3)	;
	_HotSpotParams->r3STD[0] =	STDVal.val[0];
	_HotSpotParams->r3Brighntess[0] =	meanVal.val[0];

	meanStdDev(bgr[1],meanVal,STDVal,maskImg3);
	_HotSpotParams->r3Brighntess[1] =	meanVal.val[0];
	_HotSpotParams->r3STD[1] =	STDVal.val[0];

	meanStdDev(bgr[2],meanVal,STDVal,maskImg3);
	_HotSpotParams->r3STD[2] =	STDVal.val[0];

	_HotSpotParams->r3Brighntess[2] =	meanVal.val[0];
	//Mat matArr[3] = {blueImg,greenImg,redImg};
	calculate_Contrast_Brightness_Std(bgr,_HotSpotParams->r3Brighntess,_HotSpotParams->r3Contrast,maskImg3);


	//cvAvgSdv(srcImg,&mean,&STD,maskImg4);

	/*_HotSpotParams->r4STD[0] =	STD.val[0];
	_HotSpotParams->r4STD[1] =	STD.val[1];
	_HotSpotParams->r4STD[2] =	STD.val[2];

	_HotSpotParams->r4Brighntess[0] =	mean.val[0];
	_HotSpotParams->r4Brighntess[1] =	mean.val[1];
	_HotSpotParams->r4Brighntess[2] =	mean.val[2];
	calculate_Contrast_Brightness_Std(blueImg,greenImg,redImg,_HotSpotParams->r4Brighntess,_HotSpotParams->r4Contrast,maskImg4);

*/
	/*cvSetZero(maskImg23);
	cvSetImageROI(maskImg23,cvRect(CenterX-r3/2,CenterY-r3/2, r3, r3));
	cvSet(maskImg23,cvScalarAll(255));*/
	//cvResetImageROI(maskImg23);
	//	cvSetImageROI(srcImg,cvRect(CenterX-r3/2,CenterY-r3/2, r3, r3));
	/*	if(maskImg23!=NULL)
	cvAvgSdv(srcImg,&mean,&STD,maskImg23);
		else*/
	//cvAvgSdv(srcImg,&mean,&STD);

	////cvResetImageROI(srcImg);

	//_HotSpotParams->r23STD[0] =	STD.val[0];
	//_HotSpotParams->r23STD[1] =	STD.val[1];
	//_HotSpotParams->r23STD[2] =	STD.val[2];

	//_HotSpotParams->r23Brighntess[0] =	mean.val[0];
	//_HotSpotParams->r23Brighntess[1] =	mean.val[1];
	//_HotSpotParams->r23Brighntess[2] =	mean.val[2];


	////calculate_Contrast_Brightness_Std(blueImg,greenImg,redImg,_HotSpotParams->r23Brighntess,_HotSpotParams->r23Contrast,maskImg23);
	//calculate_Contrast_Brightness_Std(blueImg,greenImg,redImg,_HotSpotParams->r23Brighntess,_HotSpotParams->r23Contrast,maskImg23,CenterX,CenterY,r3,r3);
	
	int kSize = 9;
	//_HotSpotParams->FocusVal[0] =tenengrad((cv::Mat)blueImg,kSize,maskImg23,CenterX,CenterY,r3);
	//_HotSpotParams->FocusVal[1] =tenengrad((cv::Mat)greenImg,kSize,maskImg23,CenterX,CenterY,r3);
	//_HotSpotParams->FocusVal[2] =tenengrad((cv::Mat)redImg,kSize,maskImg23,CenterX,CenterY,r3);
	//_HotSpotParams->FocusVal[0] = GetSharpness(blueImg,blueImg->width,blueImg->height,CenterX,CenterY,r3);

	//_HotSpotParams->FocusVal[1] = GetSharpness(greenImg,blueImg->width,blueImg->height,CenterX,CenterY,r3);
	//_HotSpotParams->FocusVal[2] = GetSharpness(redImg,blueImg->width,blueImg->height,CenterX,CenterY,r3);
	return _HotSpotParams;

}
// OpenCV port of 'LAPV' algorithm (Pech2000)
//double varianceOfLaplacian(const cv::Mat& src,const cv::Mat& mask)
//{
//    /*cv::Mat lap;
//    cv::Laplacian(src, lap, CV_64F);
//
//    cv::Scalar mu, sigma;
//    cv::meanStdDev(lap, mu, sigma);
//
//    double focusMeasure = sigma.val[0]*sigma.val[0];
//    return focusMeasure;*/
//	//return	tenengrad(src,9,mask);
//}

//adjusts the RoI for masked image, applies sobel(edge detection) on maskedImage.
//per-element matrix multiplication by means of matrix expression on Gx and Gy. and send it to FM.
//mean of FM.val is obatined and returned.
double tenengrad(cv::Mat& src, int ksize,const cv::Mat& mask,int centreX,int centreY,int SquareSize)
{
    cv::Mat Gx, Gy;
	cv::Mat maskedImage;
	//src.copyTo(maskedImage,mask);
	//cv::multiply(src,mask,maskedImage);
	maskedImage =  src.adjustROI(centreY-SquareSize/2,centreY + SquareSize/2,centreX-SquareSize/2,centreX + SquareSize/2);
	cv::Sobel(maskedImage, Gx, CV_64F, 1, 0, ksize);
    cv::Sobel(maskedImage, Gy, CV_64F, 0, 1, ksize);

    cv::Mat FM = Gx.mul(Gx) + Gy.mul(Gy);

    double focusMeasure = cv::mean(FM).val[0];
	maskedImage.release();
	Gx.release();
	Gy.release();
	FM.release();
	return  std::sqrt( focusMeasure);
}
//to get the sharpness of the image. is this being used ? has been commented. and its being not called .
short GetSharpness(IplImage* data, unsigned int width, unsigned int height,int centreX,int centreY,int SquareSize)
{
    // assumes that your image is already in planner yuv or 8 bit greyscale
		int x =centreX - SquareSize/2;
		int y = centreY - SquareSize/2;

	cvSetImageROI(data,cvRect(x,y,SquareSize,SquareSize));
    IplImage* in = cvCreateImage(cvSize(SquareSize,SquareSize),IPL_DEPTH_8U,1);
    IplImage* out = cvCreateImage(cvSize(SquareSize,SquareSize),IPL_DEPTH_16S,1);
    //memcpy(in->imageData,data,width*height);
	//cvCopy(data,in);
	cvEqualizeHist(data,in);
    // aperture size of 1 corresponds to the correct matrix
    cvLaplace(in, out, 1);

    short maxLap = -32767;
    short* imgData = (short*)out->imageData;//typecasting, char to short.
    for(int i =0;i<(out->imageSize/2);i++)
    {
        if(imgData[i] > maxLap) maxLap = imgData[i];
    }
	cvResetImageROI(data);
    cvReleaseImage(&in);
    cvReleaseImage(&out);
    return maxLap;
}

// called by ImageProc_CalculateHotspotImageParams.
//sets the RoI of srcImg. uses the height and width of ROI.
//cvGet2D - used to get elements of 2D image.
//contrast is calculated . Role of divider ?
void calculate_Contrast_Brightness_Std(IplImage* srcImg,IplImage* srcImg1,IplImage* srcImg2,double* brightness,double * contrast,IplImage* mask,int x,int y,int w,int h)
{

	cvSetImageROI(srcImg,cvRect(x,y,w,h));
	cvSetImageROI(srcImg1,cvRect(x,y,w,h));
	cvSetImageROI(srcImg2,cvRect(x,y,w,h));
	
	for (int i = 0; i < srcImg->roi->height; i++)
	{
		for (int j = 0; j < srcImg->roi->width; j++)
		{
			//if(cvGet2D(mask,i,j).val[0] > 0)
			{
			double TempVal=  cvGet2D(srcImg,i,j).val[0] -  brightness[0];
			contrast[0]+=(TempVal*TempVal);

				 TempVal=  cvGet2D(srcImg1,i,j).val[0] -  brightness[1];
			contrast[1]+=(TempVal*TempVal);

				 TempVal=  cvGet2D(srcImg2,i,j).val[0] -  brightness[2];
			contrast[2]+=(TempVal*TempVal);
			}
		}
	}
	int divider =0;
	if(srcImg->roi !=NULL)
	 divider = (srcImg->roi->height*srcImg->roi->width);
	else
	 divider = (srcImg->height*srcImg->width);

	contrast[0] = std::sqrt(contrast[0]/(divider));
	contrast[1] = std::sqrt(contrast[1]/(divider));
	contrast[2] = std::sqrt(contrast[2]/(divider));
	cvResetImageROI(srcImg);
	cvResetImageROI(srcImg1);
	cvResetImageROI(srcImg2);

}
//difference between this and above method is that , here we are not using the ROI.
void calculate_Contrast_Brightness_Std(Mat srcImg[],double* brightness,double * contrast,Mat mask)
{

	int divider = 0;//cvCountNonZero(mask);
	for (int i = 0; i < srcImg[0].rows; i++)
	{
		for (int j = 0; j < srcImg[0].cols; j++)
		{

			double maskVal =mask.at<uchar>(i,j);
		//double maskVal =	cvGetReal2D(&mask,i,j);
			if(maskVal> 0)
			{
				divider+=1;
			double TempVal= 0;
				 /*TempVal=  cvGet2D(srcImg,i,j).val[0] -  brightness[0];
			contrast[0]+=(TempVal*TempVal);*/

			double pixVal = srcImg[1].at<uchar>(i,j);
			TempVal= pixVal  -  brightness[1];
			//TempVal=  cvGetReal2D(&mask,i,j) -  brightness[1];
			contrast[1]+=(TempVal*TempVal);

				/* TempVal=  cvGet2D(srcImg2,i,j).val[0] -  brightness[2];
			contrast[2]+=(TempVal*TempVal);*/
			}
		}
	}
	//contrast[0] = std::sqrt(contrast[0]/(divider));
	contrast[1] = std::sqrt(contrast[1]/(divider));
	//contrast[2] = std::sqrt(contrast[2]/(divider));

}
//called by ComputeParabola method in postprocessing.
extern "C" __declspec(dllexport)void ImageProc_ComputeParabolaForVignatting( int centerWidth, int centerHeight, float percentFactor, int radius,int width,int height)
{
	float a = percentFactor / (radius * radius);
	float b = percentFactor / (radius * radius);

	for (int y = 0; y < height; y++)
	{
		int offsetY = y - centerHeight;
		int y2 = offsetY * offsetY;
		float mul_factorY = b * y2; //b*y^2
		mul_factorY += percentFactor; // c

		int ybyWidth = y * width;
		for (int x = 0; x < width; x++)
		{

			int offsetX = x - centerWidth;
			int x2 = offsetX * offsetX;
			float mul_factor = mul_factorY + a * x2;
			CvScalar val =  cvScalarAll(mul_factor);// cvGet2D(inp,x,y);
			// int new_R = val.val[2] * mul_factor;
			//int new_R = (int)((float)inp.Data[x, y, 2] * mul_factor);

			//new_R =  std::min(255, new_R);
			//val.val[2] = new_R;
				cvSetReal2D(&parabolaImage,y,x,val.val[0]); //yth row and xth column
			parabolaImage_float[ybyWidth + x] = mul_factor;
			//inp.Data[x, y, 2] =   Convert.ToByte(new_R);

		}
	}
}

extern "C" __declspec(dllexport)void ImageProc_ComputeParabolaForVignattingHotspot( int centerWidth, int centerHeight, float percentFactor, int radius,int width,int height, float hotspotFactor, int hotspotRadius)
{
	float a = percentFactor / (radius * radius);
	float b = percentFactor / (radius * radius);

	// hotspot compensation required

	//float radius2 = hotspotRadius * hotspotRadius;
	float dist;


	for (int y = 0; y < height; y++)
	{
		int offsetY = y - centerHeight;
		int y2 = offsetY * offsetY;
		float mul_factorY = b * y2; //b*y^2
		mul_factorY += percentFactor; // c


		for (int x = 0; x < width; x++)
		{

			int offsetX = x - centerWidth;
			int x2 = offsetX * offsetX;
			float mul_factor = mul_factorY + a * x2;



			// for hotspot compensation
			float mul_factor_Hotspot = 1;
			dist = x2 + y2;

			//dist /= radius2;
			//if (dist < 1)
			//            {
			// mul_factor_Hotspot += hotspotFactor * 0.7 * (cosf(M_PI_2 * dist));// Cos of pi/2 * the distance factor //added by sriram 18th july 2015 in order to reduce the effect of hot spot
			//                mul_factor *= mul_factor_Hotspot;
			//}


			// =(1+COS(PI()*(ABS(distance ratio)-0.3)))/2
			dist =sqrtf(dist);
			dist /= hotspotRadius;

			if (dist < 1.3)
			{

				mul_factor_Hotspot += hotspotFactor * 0.9 * 0.5*(1 + (cosf(M_PI * (dist - 0.3))));// Cos of pi/2 * the distance factor //added by sriram 18th july 2015 in order to reduce the effect of hot spot
				mul_factor *= mul_factor_Hotspot;
			}


			CvScalar val =  cvScalarAll(mul_factor);// cvGet2D(inp,x,y);
			//cvSet2D(parabolaHotspotImage,y,x,val); //yth row and xth column
			cvSetReal2D(&parabolaHotspotImage,y,x,val.val[0]); //yth row and xth column

		}
	}
}

extern "C" __declspec(dllexport) void ImageProc_Demosaic_Exit( )
{
	inp.~Mat();
	inp.~Mat();
	inp2.~Mat();
	parabolaHotspotImage.~Mat();
	parabolaImage.~Mat();

	//free(parabolaImage_float);
	actual_ROI.~Mat();
	//circum_ROI.~Mat;
	Rim.~Mat();
	inner_mask_boundry.~Mat();
	outer_mask_boundry.~Mat();
	circum_mask_boundry.~Mat();
	//cvReleaseImage(&inp);
	//cvReleaseImage(&inpH);
	//cvReleaseImage(&inpS);
	//cvReleaseImage(&inpV);
	//cvReleaseImage(&inp2);
	//cvReleaseImage(&parabolaHotspotImage);
	//cvReleaseImage(&parabolaImage);

	////free(parabolaImage_float);
	//cvReleaseImage ( &redImg);
	//cvReleaseImage ( &blueImg);
	//cvReleaseImage ( &greenImg);
	//cvReleaseImage ( &actual_ROI);
	//cvReleaseImage ( &circum_ROI);
	//cvReleaseImage ( &Rim);
	//cvReleaseImage ( &inner_mask_boundry);
	//cvReleaseImage ( &outer_mask_boundry);
	//cvReleaseImage ( &circum_mask_boundry);

	/*free(chr_B);
	free(chr_Gr);

	free(chr_Gb);
	free(chr_R);

	free(chr_B_8);
	free(chr_Gr_8);

	free(chr_Gb_8);
	free(chr_R_8);*/
}
extern "C" __declspec(dllexport) void ImageProc_ApplyParabolicCompensationLive( Mat srcImg,Mat outputImg)
{

	/*cvCvtColor(srcImg, srcImg, CV_RGB2HSV);
	cvSplit(srcImg,inpH,inpS,inpV,0);
	cvConvertScale(inpV,inp2);
	cvMul(inp2,parabolaImage,inp2);
	cvConvertScale(inp2,outputImg);*/

	//cvCvtColor(srcImg, srcImg, CV_RGB2HSV);
	//cvSplit(srcImg,inpH,inpS,inpV,0);
	cvConvertScale(&srcImg,&inp2);
	multiply(inp2,parabolaImage,inp2);
	
	//cvMul(inp2,parabolaImage,inp2);
	cvConvertScale(&inp2,&outputImg);
	//cvMerge(inpH,inpS,inpV,0,srcImg);
	//cvCvtColor(srcImg, srcImg, CV_HSV2RGB);

}

extern "C" __declspec(dllexport) void ImageProc_ApplyParabolicHotspotCompensationLive( Mat srcImg,Mat outputImg)
{

	cvConvertScale(&srcImg,&inp2);
	multiply(inp2,parabolaHotspotImage,inp2);
	cvConvertScale(&inp2,&outputImg);

}


extern "C" __declspec(dllexport) void ImageProc_ApplyParabolicCompensationPostProcessing( Mat srcImg)
{


	//cvCvtColor(srcImg, srcImg, CV_RGB2HSV);
	cvtColor(srcImg,srcImg,CV_RGB2HSV);
	Mat inpHSV[3];
	split(srcImg,inpHSV);
	//(srcImg,inpH,inpS,inpV,0);
	cvConvertScale(&inpHSV[2],&inp2);
	multiply(inp2,parabolaImage,inp2);
	//cvMul(inp2,parabolaImage,inp2);
	cvConvertScale(&inp2,&inpHSV[2]);
	merge(inpHSV,3,srcImg);
	cvCvtColor(&srcImg, &srcImg, CV_HSV2RGB);

}

extern "C" __declspec(dllexport) void ImageProc_ApplyParabolicCompensationPostProcessing_New( unsigned char * srcImg,int IMG_WIDTH, int IMG_HEIGHT)
{
	float val_factor;
	float value;

	int k = 0; // raw data index
	for (int i = 0; i < IMG_HEIGHT; i++)
	{
		int ibywidth = i*IMG_WIDTH;
		for (int j = 0; j < IMG_WIDTH; j++)
		{
			val_factor = parabolaImage_float[ibywidth + j];

			for (int j1 = 0; j1 < 3; j1++)
			{
				value = (float) srcImg[k] * val_factor;
				if(value>255) value = 255;
				srcImg[k++] = (unsigned char) value;
			}
		}

	}

}

extern "C" __declspec(dllexport) void ImageProc_Demosaic(unsigned char* GbPtr,unsigned char* GrPtr,unsigned char * outputImg,int IMG_WIDTH, int IMG_HEIGHT,bool isFlash)
{

	int TMP_HEIGHT = IMG_HEIGHT+2;
	int TMP_WIDTH = IMG_WIDTH +2;


	// we have only GRBG images (1st row is GRGR pixels, 2nd row is BGBG pixels
	int i, j,k;
	for ( i = 0,k=0; i < IMG_HEIGHT; i++)
	{
		for ( j = 0; j < IMG_WIDTH; j+=2)
		{
			int m = (i+1) * TMP_WIDTH + j + 1;
			int a = m+1;
			if(i % 2  == 0) // GR Rows
			{

				chr_Gr_8[m] = GrPtr[k++];
				chr_Gr_8[a] =unsigned char (0);
				chr_R_8[m] = unsigned char (0);
				chr_R_8[a] = GrPtr[k++];

				//chr_Gb_8[m] = unsigned char (0);
				chr_Gb_8[a] = unsigned char (0);
				chr_B_8[m] = unsigned char (0);
				chr_B_8[a] = unsigned char (0);


			}
			else
			{
				if(isFlash)
				{
					chr_B_8[m] = GrPtr[k++];
					chr_B_8[a] = unsigned char (0);
					chr_Gb_8[m] = unsigned char (0);
					chr_Gb_8[a] = GrPtr[k++];
				}
				else
				{
					chr_B_8[m] = GbPtr[k++];
					chr_B_8[a] = unsigned char (0);
					chr_Gb_8[m] = unsigned char (0);
					chr_Gb_8[a] = GbPtr[k++];
				}
				chr_Gr_8[m] = unsigned char (0);
				chr_Gr_8[a] = unsigned char (0);
				chr_R_8[m] = unsigned char (0);
				chr_R_8[a] =unsigned char (0);

			}
		}
	}


	// fill up the dummp rows

	// First column, with R, Gb data
	for(i=0,k=2; i<TMP_HEIGHT;i+=TMP_WIDTH, k+= TMP_WIDTH)
	{
		chr_R_8[i] = chr_R_8[k];
		chr_Gb_8[i] = chr_Gb_8[k];

	}
	// Last column, with Gr, B data
	for(i=TMP_WIDTH-3,k=TMP_WIDTH-1; i<TMP_HEIGHT;i+=TMP_WIDTH, k+= TMP_WIDTH)
	{
		chr_B_8[i] = chr_B_8[k];
		chr_Gr_8[i] = chr_Gr_8[k];

	}

	// interpolation of all rows
	//Horizontal interpolation of column values of known rows
	for (int i = 1; i < TMP_HEIGHT-1; i++)
	{

		if(i%2 ==1) // GR Row // Horizontal interpolation of Gr and R values, alternately
		{
			for (int j = i*TMP_WIDTH+1; j < ((i+1)*TMP_WIDTH)-1; j++)
			{
				int sum = (int) chr_R_8[j-1] + (int) chr_R_8[j+1];
				sum /=2;
				chr_R_8[j] = (unsigned char) sum;

				j++;
				sum = (int) chr_Gr_8[j-1] + (int) chr_Gr_8[j+1];
				sum /=2;
				chr_Gr_8[j] = (unsigned char) sum;
			}
		}
		else // GB Row // Horizontal interpolation of B and Gb values, alternately
		{
			for (int j = i*TMP_WIDTH+1; j < ((i+1)*TMP_WIDTH)-1; j++)
			{
				int sum = (int) chr_Gb_8[j-1] + (int) chr_Gb_8[j+1];
				sum /=2;
				chr_Gb_8[j] = (unsigned char) sum;

				j++;
				sum = (int) chr_B_8[j-1] + (int) chr_B_8[j+1];
				sum /=2;
				chr_B_8[j] = (unsigned char) sum;
			}
		}
	}

	// top row to be filled with Gb,B data
	for(i=0,k=2*TMP_WIDTH;i<TMP_WIDTH;i++,k++)
	{
		chr_B_8[i] = chr_B_8[k];
		chr_Gb_8[i] = chr_Gb_8[k];
	}
	// Last row with Gr, R data
	for(i=(TMP_HEIGHT-3)*TMP_WIDTH,k=(TMP_HEIGHT-1)*TMP_WIDTH;i<TMP_WIDTH;i++,k++)
	{
		chr_R_8[i] = chr_R_8[k];
		chr_Gr_8[i] = chr_Gr_8[k];
	}

	//Vertical interpolation of unknon row values
	for (int i = 1; i < TMP_HEIGHT-1; i++)
	{

		if(i%2 ==1) // GR Row // Horizontal interpolation of Gr and R values, alternately
		{
			for (int j = i*TMP_WIDTH+1; j < ((i+1)*TMP_WIDTH)-1; j++)
			{
				int sum = (int) chr_B_8[j-TMP_WIDTH] + (int) chr_B_8[j+TMP_WIDTH];
				sum /=2;
				chr_B_8[j] = (unsigned char) sum;

				sum = (int) chr_Gb_8[j-TMP_WIDTH] + (int) chr_Gb_8[j+TMP_WIDTH];
				sum /=2;
				chr_Gb_8[j] = (unsigned char) sum;
			}
		}
		else // GB Row // Horizontal interpolation of B and Gb values, alternately
		{
			for (int j = i*TMP_WIDTH+1; j < ((i+1)*TMP_WIDTH)-1; j++)
			{
				int sum = (int) chr_Gr_8[j-TMP_WIDTH] + (int) chr_Gr_8[j+TMP_WIDTH];
				sum /=2;
				chr_Gr_8[j] = (unsigned char) sum;

				sum = (int) chr_R_8[j-TMP_WIDTH] + (int) chr_R_8[j+TMP_WIDTH];
				sum /=2;
				chr_R_8[j] = (unsigned char) sum;
			}
		}
	}


	for (int i = 0; i < IMG_HEIGHT; i++)
	{
		for (int j = 0; j < IMG_WIDTH; j++)
		{
			int m = (i * IMG_WIDTH + j)*3;
			int k = (i+1) * TMP_WIDTH + j +1;
			int val1 = ((int)chr_Gr_8[k]+ (int)chr_Gb_8[k])/2;


			outputImg[m] = chr_B_8[k];
			outputImg[m+1] = (unsigned char) (val1);
			//chr_Gr[k] = (unsigned char) (val1);// for debug
			outputImg[m+2] = chr_R_8[k];

		}
	}

}
extern "C" __declspec(dllexport) void ImageProc_Demosaic_16bit(uint16_t* GbPtr,unsigned char* outputImg,int IMG_WIDTH, int IMG_HEIGHT,bool isApplyLUT)
{
	int image_dump = 0;
	int TMP_HEIGHT = IMG_HEIGHT+2;
	int TMP_WIDTH = IMG_WIDTH +2;


	//we have only GRBG images 1st row is GRGR pixels, 2nd row is BGBG pixels
	int i, j,k;
	for ( i = 0,k=0; i < IMG_HEIGHT; i++)
	{
		for ( j = 0; j < IMG_WIDTH; j+=2)
		{
			int m = (i+1) * TMP_WIDTH + j + 1;
			int a = m+1;
			if(i % 2  == 0) // GB Rows
			{

				chr_Gb[m] = GbPtr[k++];
				chr_Gb[a] =uint16_t (0);
				chr_B[m] = uint16_t(0);
				chr_B[a] = GbPtr[k++];


				chr_Gr[m] = uint16_t (0);
				chr_Gr[a] = uint16_t (0);
				chr_R[m] = uint16_t (0);
				chr_R[a] = uint16_t (0);


			}
			else
			{
				chr_R[m] = GbPtr[k++];
				chr_R[a] = uint16_t (0);
				chr_Gr[m] = uint16_t (0);
				chr_Gr[a] = GbPtr[k++];

				chr_Gb[m] = uint16_t (0);
				chr_Gb[a] = uint16_t (0);
				chr_B[m] = uint16_t (0);
				chr_B[a] =uint16_t (0);

			}
		}
	}


#pragma region// fill up the dummp rows . First column, with B, Gr data
	chr_B[0] = 0;
	chr_Gr[0] = 0;
	for(i=TMP_WIDTH,k=TMP_WIDTH+2; i<TMP_WIDTH*TMP_HEIGHT-1;i+=TMP_WIDTH, k+= TMP_WIDTH)
	{
		chr_B[i] = chr_B[k];
		chr_Gr[i] = chr_Gr[k];
	}
	chr_B[i] = 0;
	chr_Gr[i] = 0;
#pragma endregion

#pragma region// Last column, with R,Gb data
	chr_R[TMP_WIDTH-1] = 0;
	chr_Gb[TMP_WIDTH-1] = 0;
	for(i=2*TMP_WIDTH-1,k=2*TMP_WIDTH-3; i<(TMP_WIDTH-1)*TMP_HEIGHT;i+=TMP_WIDTH, k+= TMP_WIDTH)
	{
		chr_R[i] = chr_R[k];
		chr_Gb[i] = chr_Gb[k];

	}
	chr_R[i] = 0;
	chr_Gb[i] = 0;
#pragma endregion


#pragma region interpolation of all rows .Horizontal interpolation of column values of known rows
	for (i = 1; i < TMP_HEIGHT-1; i++)
	{
		if(i%2 ==1) // GB Row // Horizontal interpolation of Gb and B values, alternately
		{
			for (j = i*TMP_WIDTH+1; j < ((i+1)*TMP_WIDTH)-1; j++)
			{
				uint16_t sum =  chr_B[j-1] + chr_B[j+1];
				sum /=2;
				chr_B[j] =  sum;

				j++;
				sum =  chr_Gb[j-1] +  chr_Gb[j+1];
				sum /=2;
				chr_Gb[j] = sum;
			}
		}
		else // GR Row // Horizontal interpolation of R and Gr values, alternately
		{
			for (int j = i*TMP_WIDTH+1; j < ((i+1)*TMP_WIDTH)-1; j++)
			{
				uint16_t sum =  chr_Gr[j-1] +  chr_Gr[j+1];
				sum /=2;
				chr_Gr[j] =  sum;

				j++;
				sum = chr_R[j-1] +  chr_R[j+1];
				sum /=2;
				chr_R[j] =  sum;

			}
		}
	}
#pragma endregion
	// top row to be filled with R,Gr data
	for(i=0,k=2*TMP_WIDTH;i<TMP_WIDTH;i++,k++)
	{
		chr_R[i] = chr_R[k];
		chr_Gr[i] = chr_Gr[k];
	}
	// Last row with Gr, R data
	for(i=(TMP_HEIGHT-1)*TMP_WIDTH,k=(TMP_HEIGHT-3)*TMP_WIDTH;i<TMP_HEIGHT*TMP_WIDTH;i++,k++)
	{
		chr_B[i] = chr_B[k];
		chr_Gb[i] = chr_Gb[k];
	}

	//Vertical interpolation of unknon row values
	for (i = 1; i < TMP_HEIGHT-1; i++)
	{
		if(i%2 ==1) // Missing GR Row // Horizontal interpolation of Gr and R values, alternately
		{
			for (j = i*TMP_WIDTH+1; j < ((i+1)*TMP_WIDTH)-1; j++)
			{
				uint16_t sum = chr_R[j-TMP_WIDTH] + chr_R[j+TMP_WIDTH];
				sum /=2;
				chr_R[j] =sum;

				sum =  chr_Gr[j-TMP_WIDTH] +  chr_Gr[j+TMP_WIDTH];
				sum /=2;
				chr_Gr[j] =  sum;
			}
		}
		else // Missing GB Row // Horizontal interpolation of B and Gb values, alternately
		{
			for (int j = i*TMP_WIDTH+1; j < ((i+1)*TMP_WIDTH)-1; j++)
			{
				uint16_t sum =  chr_Gb[j-TMP_WIDTH] +  chr_Gb[j+TMP_WIDTH];
				sum /=2;
				chr_Gb[j] =  sum;

				sum =  chr_B[j-TMP_WIDTH] +  chr_B[j+TMP_WIDTH];
				sum /=2;
				chr_B[j] =  sum;
			}
		}
	}


	if(image_dump == 1)
	{
		FILE* fR = fopen("Image_Raw_R.raw","wb");
		FILE* fGb = fopen("Image_Raw_Gb.raw","wb");
		FILE* fGr = fopen("Image_Raw_Gr.raw","wb");
		FILE* fB = fopen("Image_Raw_B.raw","wb");
		unsigned char * pointerR = (unsigned char *) chr_R;
		unsigned char * pointerGb = (unsigned char *) chr_Gb;
		unsigned char * pointerGr = (unsigned char *) chr_Gr;
		unsigned char * pointerB = (unsigned char *) chr_B;
		for (int j = 2*TMP_WIDTH + 2; j < 2*TMP_WIDTH * (TMP_HEIGHT -1) - 1 ; j+=2*TMP_WIDTH)
		{

			fwrite(&pointerR[j],1,IMG_WIDTH*2,fR);
			fwrite(&pointerGb[j],1,IMG_WIDTH*2,fGb);
			fwrite(&pointerGr[j],1,IMG_WIDTH*2,fGr);
			fwrite(&pointerB[j],1,IMG_WIDTH*2,fB);

		}
		fclose(fR);
		fclose(fGb);
		fclose(fGr);
		fclose(fB);
	}


	for (int i = 0; i < IMG_HEIGHT; i++)
	{
		for (int j = 0; j < IMG_WIDTH; j++)
		{
			int m = (i * IMG_WIDTH + j)*3;
			int k = (i+1) * TMP_WIDTH + j +1;
			uint16_t	temp1;
			chr_G[k] = (chr_Gr[k]+ chr_Gb[k])/2;
#pragma region lut Application to use for uint16_t images
			if(isApplyLUT)  // S-curved look up table application 
			{
				temp1 = chr_B[k];
				chr_B[k] =	LUTValues[temp1]; 
				temp1 = chr_R[k];
				chr_R[k] =	LUTValues[temp1];
				temp1 = chr_G[k];
				chr_G[k] =	LUTValues[temp1];
			}

#pragma endregion
			chr_B[k] >>= 4; // reduction to 10 bits image
			chr_R[k] >>= 4;
			chr_G[k] >>= 4 ;
		}
	}

	//free(chr_Gb);
	//free(chr_Gr);


	if(image_dump == 1)
	{
		FILE* fR = fopen("Image_Raw_R1.raw","wb");
		FILE* fG = fopen("Image_Raw_G1.raw","wb");
		FILE* fB = fopen("Image_Raw_B1.raw","wb");
		unsigned char * pointerR = (unsigned char *) chr_R;
		unsigned char * pointerG = (unsigned char *) chr_G;
		unsigned char * pointerB = (unsigned char *) chr_B;
		for (int j = 2*TMP_WIDTH + 2; j < 2*TMP_WIDTH * (TMP_HEIGHT -1) - 1 ; j+=2*TMP_WIDTH)
		{
			fwrite(&pointerR[j],1,IMG_WIDTH*2,fR);
			fwrite(&pointerG[j],1,IMG_WIDTH*2,fG);
			fwrite(&pointerB[j],1,IMG_WIDTH*2,fB);
		}
		fclose(fR);
		fclose(fG);
		fclose(fB);
	}

	float minVal, maxVal;
	if(isApplyLUT)
	{
		for (int i = 0; i < 1024; i++)
		{
			histArrayB[i] =histArrayG[i] =histArrayR[i] =0;
		}

		for (int i = 0; i < IMG_HEIGHT; i++)
		{
			for (int j = 0; j < IMG_WIDTH; j++)
			{
				int k = (i+1) * TMP_WIDTH + j +1;

				histArrayB[chr_B[k]]++;
				histArrayG[chr_G[k]]++;
				histArrayR[chr_R[k]]++;
			}
		}
	}

	//maxVal = ThresholdmaxValue(0.999);
	maxVal = 800;
	bool isPWL = false;
	float zoneRange1 = maxVal/3;
	float zoneRange2 = (2*maxVal)/3;
	for (int i = 0; i < IMG_HEIGHT; i++)
	{
		for (int j = 0; j < IMG_WIDTH; j++)
		{
			int m = (i * IMG_WIDTH + j)*3;
			int k = (i+1) * TMP_WIDTH + j +1;
			uint16_t valB =0;
			uint16_t valG =0;
			uint16_t valR =0;
			float mapping[3]; 
			mapping[0] = chr_B[k];
			mapping[1]  =chr_G[k];
			mapping[2]  =chr_R[k];

			if(isPWL)
			{
				// 3 piece PWL
				for(int x = 0; x<3; x++)
				{
					if(mapping[x] < zoneRange1)
						mapping[x] = (float)(2* (mapping[x]/maxVal) * 243.0);
					else if(mapping[x] >= zoneRange1 &&mapping[x] < zoneRange2)
						mapping[x] = 162+(float)(2 * 243.0* (mapping[x] - zoneRange1)/maxVal)/3;// - minVal)/(float)maxVal) * 255.0;
					else if(mapping[x] >= zoneRange2 && mapping[x] < maxVal)
						mapping[x] =216+(float)(243 *(mapping[x] - zoneRange2)/maxVal)/3;// - minVal)/(float)maxVal) * 255.0;
					else
						mapping[x] = 245;
					outputImg[m+x] =(unsigned char) std::floorf(mapping[x] ); //chr_B[k];
				}
			}

			else
			{
				for(int x = 0; x<3; x++)
				{
					mapping[x] = (float)( (mapping[x]/maxVal) * 250.0);
					if(mapping[x] > 255) mapping[x] = 255;
					outputImg[m+x] =(unsigned char) std::floorf(mapping[x] ); //chr_B[k];

				}
			}
		}
	}
	/*free(chr_B);
	free(chr_G);
	free(chr_R);*/


}
//extern "C"__declspec(dllexport) void  ImageProc_ApplyHotSpotCompensationCPlusCplus(unsigned char* bm, int centreX, int centreY, int method,float percentFactor, int radius, float hotSpotAmp, int radHotSpot
//																				   ,int ShadowRadSpot1,int ShadowRadSpot2 ,int hsRad1,int hsRad2,int currentGain,int presetGain, int percentageR, int percentageG,int percentageB,int HSR,int HSG,int HSB,int gainSlope)
//{
//	/*centreX = 1030;
//	centreY = 730;
//	method = 4;*/
//	//int method = 2;
//	hotspotOffsetB = HSB;
//	hotspotOffsetG = HSG;
//	hotspotOffsetR = HSR;
//	hotspotRad1 = hsRad1;
//	hotspotRad2 = hsRad2;
//	if (method == 1) // cos pi by 2 into dist square ratio
//	{
//#pragma region
//		//Image<Rgb, byte> inp1 = new Image<Rgb, byte>(bm);
//
//		// Image<Hsv, byte> inp = new Image<Hsv, byte>(bm.Width, bm.Height);
//		//CvInvoke.cvCvtColor(inp1, inp, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_RGB2HSV);
//		// inp = inp1.Convert<Hsv, byte>();
//		//float a = percentFactor / (radius * radius);
//		//float b = percentFactor / (radius * radius);
//		//float rad2 = radHotSpot * radHotSpot;
//		// x corresponds to the columns along the width of the image
//		for (int x = centreX - radHotSpot; x < centreX + radHotSpot; x++)
//		{
//			int offsetX = x - centreX;
//			int x2 = offsetX * offsetX;
//			//float mul_factorX = a * x2; //a*x^2
//			//mul_factorX += percentFactor; // c
//
//			// x corresponds to the rows along the height of the image
//			for (int y = centreY - radHotSpot; y < centreY + radHotSpot; y++)
//			{
//				int m = (y * (centreX + (int)((float)radHotSpot) + x))*3;
//
//				int offsetY = y - centreY;
//				int y2 = offsetY * offsetY;
//				float mul_factorR = 1;
//				//float mul_factor2 = 0;
//				float mul_factorG = 1;
//				float mul_factorB = 1;
//				float dist = x2 + y2;
//				dist /= rad2;
//				if (dist < 1)
//				{
//					mul_factorR += hotSpotAmp * ((float)(std::cos((float)4 * std::atan(1) * 0.5 * dist)));// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					mul_factorG += hotSpotAmp * 0.6f * ((float)(std::cos((float)4 * std::atan(1)  * 0.5 * dist)));// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					mul_factorB += hotSpotAmp * 0.3f * ((float)(std::cos((float)4 * std::atan(1)  * 0.5 * dist)));// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					// mul_factorG += 0.5f * hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//					//mul_factor *= mul_factor2;
//					//mul_factor += hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//
//
//					int new_R = (int)(bm[m+2] * mul_factorR);
//					int new_G = (int)(bm[m+1] * mul_factorG);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					int new_B = (int)(bm[m] * mul_factorB);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					new_R = std::min(255, new_R);
//					new_G = std::min(255, new_G);
//					new_B = std::min(255, new_B);
//					//inp.Data[x, y, 2] = Convert.ToByte(new_R);
//					// bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//					bm[m] = new_B;
//					bm[m+1] = new_G;
//					bm[m+2] = new_R;
//				}
//
//
//			}
//		}
//#pragma endregion method 1
//	}
//	else if (method == 2) // (1 plus cos pi into abs(dist ratio) - 0.3)/2 // gives a central notch to suppress the hotspot on lens impact
//	{
//#pragma region
//		float distanceMultiplier = 1.1;
//		float distanceOffset = distanceMultiplier - 1.0;
//		for (int x = centreX - (int)((float)radHotSpot * distanceMultiplier); x < centreX + (int)((float)radHotSpot * distanceMultiplier); x++)
//		{
//			int offsetX = x - centreX;
//			int x2 = offsetX * offsetX;
//
//			//float mul_factorX = a * x2; //a*x^2
//			//mul_factorX += percentFactor; // c
//
//			// x corresponds to the rows along the height of the image
//			for (int y = centreY - (int)((float)radHotSpot * distanceMultiplier); y < centreY + (int)((float)radHotSpot * distanceMultiplier); y++)
//			{
//				int m = (y * (centreX + (int)((float)radHotSpot * distanceMultiplier) + x))*3;
//
//				int offsetY = y - centreY;
//				int y2 = offsetY * offsetY;
//				float mul_factorR = 1;
//				//float mul_factor2 = 0;
//				float mul_factorG = 1;
//				float mul_factorB = 1;
//				float dist = x2 + y2;
//				dist = (float)std::sqrt(dist);
//				dist /= radHotSpot;
//				if (dist < distanceMultiplier)
//				{
//					mul_factorR += hotSpotAmp * (1 + (float)(std::cos((float)4 * std::atan(1) * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					mul_factorG += hotSpotAmp * 0.7f * (1 + (float)(std::cos((float)4 * std::atan(1) * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					mul_factorB += hotSpotAmp * 0.5f * (1 + (float)(std::cos((float)4 * std::atan(1) * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					// mul_factorG += 0.5f * hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//					//mul_factor *= mul_factor2;
//					//mul_factor += hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//
//
//					int new_R = (int)(bm[m+2]* mul_factorR);
//					int new_G = (int)(bm[m+1]* mul_factorG);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					int new_B = (int)(bm[m]* mul_factorB);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					new_R = std::min(255, new_R);
//					new_G = std::min(255, new_G);
//					new_B = std::min(255, new_B);
//					//inp.Data[x, y, 2] = Convert.ToByte(new_R);
//					//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//					bm[m] = new_B;
//					bm[m+1] = new_G;
//					bm[m+2] = new_R;
//				}
//
//
//			}
//		}
//#pragma endregion method 2
//	}
//#pragma region new method 3 commented by sriram
//	//else if (method == 3)
//	//{
//	//    float distanceMultiplier = 1.1f;
//
//	//    float distanceOffset = distanceMultiplier - 1f;
//	//    float offsetR = (float)percentageR + (float)(currentGain - presetGain) * (gainSlope * percentageR) / (float)(10 * 100);
//	//    float offsetG = (float)percentageG + (float)(currentGain - presetGain) * (gainSlope * percentageG) / (float)(10 * 100);
//	//    float offsetB = (float)percentageB + (float)(currentGain - presetGain) * (gainSlope * percentageB) / (float)(10 * 100);
//	//    //int radSpot1 = 175, radSpot2 = 125;
//	//    //int radSpot1 = 150, radSpot2 = 100;
//
//	//    int totalRadius = radSpot1 + radSpot2;
//
//	//    for (int x = centreX - totalRadius; x < centreX + totalRadius; x++)
//	//    {
//	//        int offsetX = x - centreX;
//	//        int x2 = offsetX * offsetX;
//
//	//        //float mul_factorX = a * x2; //a*x^2
//	//        //mul_factorX += percentFactor; // c
//
//	//        // x corresponds to the rows along the height of the image
//
//	//        for (int y = centreY - totalRadius; y < centreY + totalRadius; y++)
//	//        {
//
//	//            int offsetY = y - centreY;
//	//            int y2 = offsetY * offsetY;
//	//            float mul_factorR = 1;
//	//            //float mul_factor2 = 0;
//	//            float mul_factorG = 1;
//	//            float mul_factorB = 1;
//	//            float dist = x2 + y2;
//	//            dist = (float)Math.Sqrt(dist);
//
//	//            //dist /= radHotSpot;
//	//            if (dist < radSpot1)
//	//            {
//	//                if (dist < hotspotRad1)
//	//                {
//	//                    int new_R = (int)(bm.GetPixel(x, y).R - offsetR);
//	//                    int new_G = (int)(bm.GetPixel(x, y).G - offsetG);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                    int new_B = (int)(bm.GetPixel(x, y).B - offsetB);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//
//	//                    new_R = Math.Min(255, new_R);
//	//                    new_G = Math.Min(255, new_G);
//	//                    new_B = Math.Min(255, new_B);
//	//                    //inp.Data[x, y, 2] = Convert.ToByte(new_R);
//	//                    bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//	//                }
//	//                else if (dist > hotspotRad1 && dist < hotspotRad2)
//	//                {
//	//                    dist -= hotspotRad1;
//	//                    dist /= hotspotRad2;
//	//                    mul_factorR = (1 + (float)(Math.Cos(Math.PI * (dist)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                    // mul_factorG += hotSpotAmp * 0.7f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                    // mul_factorB += hotSpotAmp * 0.5f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                    // mul_factorG += 0.5f * hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//	//                    //mul_factor *= mul_factor2;
//	//                    //mul_factor += hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//
//
//	//                    int new_R = (int)(bm.GetPixel(x, y).R + offsetR * mul_factorR);
//	//                    int new_G = (int)(bm.GetPixel(x, y).G + offsetG * mul_factorR);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                    int new_B = (int)(bm.GetPixel(x, y).B + offsetB * mul_factorR);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                    new_R = Math.Min(255, new_R);
//	//                    new_G = Math.Min(255, new_G);
//	//                    new_B = Math.Min(255, new_B);
//	//                    //inp.Data[x, y, 2] = Convert.ToByte(new_R);
//	//                    bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//	//                }
//
//
//	//            }
//	//            else if (dist > radSpot1 && dist < totalRadius)
//	//            {
//	//                dist -= radSpot1;
//	//                dist /= radSpot2;
//	//                mul_factorR = (1 + (float)(Math.Cos(Math.PI * (dist)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                // mul_factorG += hotSpotAmp * 0.7f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                // mul_factorB += hotSpotAmp * 0.5f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                // mul_factorG += 0.5f * hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//	//                //mul_factor *= mul_factor2;
//	//                //mul_factor += hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//
//
//	//                int new_R = (int)(bm.GetPixel(x, y).R + offsetR * mul_factorR);
//	//                int new_G = (int)(bm.GetPixel(x, y).G + offsetG * mul_factorR);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                int new_B = (int)(bm.GetPixel(x, y).B + offsetB * mul_factorR);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//	//                new_R = Math.Min(255, new_R);
//	//                new_G = Math.Min(255, new_G);
//	//                new_B = Math.Min(255, new_B);
//	//                //inp.Data[x, y, 2] = Convert.ToByte(new_R);
//	//                bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//	//            }
//
//
//	//        }
//	//    }
//	//}
//#pragma endregion
//	else if (method == 3)
//	{
//#pragma region 
//		float distanceMultiplier = 1.1;
//
//		float distanceOffset = distanceMultiplier - 1.0;
//		// int radSpot1 = 15, radSpot2 = 125;
//
//		int totalRadius = ShadowRadSpot1 + ShadowRadSpot2;
//
//		for (int x = centreX - totalRadius; x < centreX + totalRadius; x++)
//		{
//			int offsetX = x - centreX;
//			int x2 = offsetX * offsetX;
//
//			//float mul_factorX = a * x2; //a*x^2
//			//mul_factorX += percentFactor; // c
//
//			// x corresponds to the rows along the height of the image
//
//			for (int y = centreY - totalRadius; y < centreY + totalRadius; y++)
//			{
//
//				int m = (y * (centreX+totalRadius) + x)*3;
//
//				int offsetY = y - centreY;
//				int y2 = offsetY * offsetY;
//				float mul_factorR = 1;
//				//float mul_factor2 = 0;
//				float mul_factorG = 1;
//				float mul_factorB = 1;
//				float dist = x2 + y2;
//				dist = (float)std::sqrt(dist);
//
//				//dist /= radHotSpot;
//				if (dist < ShadowRadSpot1)
//				{
//					int new_R = (int)(bm[m+2] + percentageR);
//					int new_G = (int)(bm[m+1] + percentageG);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					int new_B = (int)(bm[m] + percentageB);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					new_R = std::min(255, new_R);
//					new_G = std::min(255, new_G);
//					new_B = std::min(255, new_B);
//					//inp.Data[x, y, 2] = Convert.ToByte(new_R);
//					bm[m] = new_B;
//					bm[m+1] = new_G;
//					bm[m+2] = new_R;
//					// bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//
//				}
//				else if (dist > ShadowRadSpot1 && dist < totalRadius)
//				{
//					dist -= ShadowRadSpot1;
//					dist /= ShadowRadSpot2;
//					mul_factorR = (1 + (float)(std::cos((float)4*atan(1)* (dist)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					// mul_factorG += hotSpotAmp * 0.7f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					// mul_factorB += hotSpotAmp * 0.5f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					// mul_factorG += 0.5f * hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//					//mul_factor *= mul_factor2;
//					//mul_factor += hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//
//
//					int new_R = (int)(  bm[m] + percentageR * mul_factorR);
//					int new_G = (int)(bm[m+1] + percentageG * mul_factorR);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					int new_B = (int)(bm[m+2] + percentageB * mul_factorR);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					new_R = std::min(255, new_R);
//					new_G = std::min(255, new_G);
//					new_B = std::min(255, new_B);
//					//inp.Data[x, y, 2] = Convert.ToByte(new_R);
//					// bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//					bm[m] = new_B;
//					bm[m+1] = new_G;
//					bm[m+2] = new_R;
//				}
//
//
//			}
//		}
//#pragma endregion method 3
//	}
//	else if (method == 4)
//	{
//#pragma region 
//		//else if (method == 3)
//		//{
//
//		int radSpot1 = ShadowRadSpot1;
//		int radSpot2 = ShadowRadSpot2;
//
//		float distanceMultiplier = 1.1f;
//
//		//gainSlope = 2;
//
//		float distanceOffset = distanceMultiplier - 1.0;
//		float offsetR = (float)percentageR + (float)(currentGain - presetGain) * (gainSlope * percentageR) / (float)(10 * 100);
//		float offsetG = (float)percentageG + (float)(currentGain - presetGain) * (gainSlope * percentageG) / (float)(10 * 100);
//		float offsetB = (float)percentageB + (float)(currentGain - presetGain) * (gainSlope * percentageB) / (float)(10 * 100);
//
//
//		//int radSpot1 = 175, radSpot2 = 125;
//		//int radSpot1 = 150, radSpot2 = 100;
//
//		int totalRadius = radSpot1 + radSpot2;
//		int totalHotspotRadius = hotspotRad1 + hotspotRad2;
//
//		// x is along width
//		for (int x = centreX - totalRadius; x < centreX + totalRadius; x++)
//		{
//			int offsetX = x - centreX;
//			int x2 = offsetX * offsetX;
//			// y corresponds to row number // from top to bottom
//			for (int y = centreY - totalRadius; y < centreY + totalRadius; y++)
//			{
//
//				int m = ((y * parabolaImage->width) + x)*3; // 3 bytes per pixel
//
//				int offsetY = y - centreY;
//				int y2 = offsetY * offsetY;
//				float mul_factorR = 1;
//				//float mul_factor2 = 0;
//				float mul_factorG = 1;
//				float mul_factorB = 1;
//				float dist = x2 + y2;
//				dist = (float)std::sqrt(dist);
//				//dist /= radHotSpot;
//				if (dist < radSpot1) // Central shadow zone
//				{
//					if (dist < hotspotRad1) // central hotspot zone
//					{
//
//						int new_R = (int)(bm[m+2]);//.GetPixel(x, y).R);
//						if (new_R != 255) // no processing on saturated pixel
//							new_R = (int)((float)new_R + offsetR) - hotspotOffsetR;
//						int new_G = (int)bm[m+1];//.GetPixel(x, y).G;
//						if (new_G != 255)
//							new_G = (int)((float)new_G + offsetG) - hotspotOffsetG;//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//						int new_B = (int)bm[m];//.GetPixel(x, y).B;
//						if (new_B != 255)
//							new_B = (int)(new_B + offsetB) - hotspotOffsetB;//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//
//						// overflow limit for pixel value
//						new_R = std::min(255, new_R);
//						new_G = std::min(255, new_G);
//						new_B = std::min(255, new_B);
//						//inp.Data[x, y, 2] = Convert.ToByte(new_R);
//						// underflow limit for pixel value
//						new_R = std::max(0, new_R);
//						new_G = std::max(0, new_G);
//						new_B = std::max(0, new_B);
//						bm[m] = new_B;
//						bm[m+1] = new_G;
//						bm[m+2] = new_R;
//						//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//					}
//					else if (dist < totalHotspotRadius) // outer hotspot zone
//					{
//						dist -= hotspotRad1;
//						dist /= hotspotRad2;
//						mul_factorR = (1 + (float)(std::cos(pi  * dist))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//						// mul_factorG += hotSpotAmp * 0.7f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//						// mul_factorB += hotSpotAmp * 0.5f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//						// mul_factorG += 0.5f * hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//						//mul_factor *= mul_factor2;
//						//mul_factor += hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//
//
//						int new_R = (int)(bm[m+2]);//.GetPixel(x, y).R);
//						if (new_R != 255)
//							new_R = (int)(new_R + offsetR - hotspotOffsetR * mul_factorR);
//						int new_G = (int)bm[m+1];//.GetPixel(x, y).G;
//						if (new_G != 255)
//							new_G = (int)(new_G + offsetG - hotspotOffsetG * mul_factorR);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//						int new_B = (int)bm[m];//.GetPixel(x, y).B;
//						if (new_B != 255)
//							new_B = (int)(new_B + offsetB - hotspotOffsetB * mul_factorR);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//						new_R = std::min(255, new_R);
//						new_G = std::min(255, new_G);
//						new_B = std::min(255, new_B);
//
//						new_R = std::max(0, new_R);
//						new_G = std::max(0, new_G);
//						new_B = std::max(0, new_B);
//						//inp.Data[x, y, 2] = Convert.ToByte(new_R);
//						bm[m] = new_B;
//						bm[m+1] = new_G;
//						bm[m+2] = new_R;
//						// bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//					}
//					else
//					{
//						int new_R = (int)bm[m+2] + offsetR; // added offset to the actual pixel 
//						int new_G = (int)bm[m+1] + offsetG;// added offset to the actual pixel  //; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//						int new_B = (int)bm[m] + offsetB;// added offset to the actual pixel //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//
//						new_R = std::min(255, new_R);
//						new_G = std::min(255, new_G);
//						new_B = std::min(255, new_B);
//						//inp.Data[x, y, 2] = Convert.ToByte(new_R);
//						bm[m] = new_B;
//						bm[m+1] = new_G;
//						bm[m+2] = new_R;
//						// bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//					}
//
//				}
//				else if (dist > radSpot1 && dist < totalRadius)
//				{
//					dist -= radSpot1;
//					dist /= radSpot2;
//					mul_factorR = (1 + (float)(std::cos(pi* dist))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					// mul_factorG += hotSpotAmp * 0.7f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					// mul_factorB += hotSpotAmp * 0.5f * (1 + (float)(Math.Cos(Math.PI * (dist - distanceOffset)))) * 0.5f;// //added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					// mul_factorG += 0.5f * hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//					//mul_factor *= mul_factor2;
//					//mul_factor += hotSpotAmp * (1 + (float)Math.Cos(Math.PI * dist));
//
//
//					int new_R = (int)((float)bm[m+2]+ offsetR * mul_factorR);
//					int new_G = (int)((float)bm[m+1]+ offsetG * mul_factorR);//; * mul_factor);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					int new_B = (int)((float)bm[m]+ offsetB * mul_factorR);//added by sriram 18th july 2015 in order to reduce the effect of hot spot
//					new_R = std::min(255, new_R);
//					new_G = std::min(255, new_G);
//					new_B = std::min(255, new_B);
//					//inp.Data[x, y, 2] = Convert.ToByte(new_R);
//					bm[m] = new_B;
//					bm[m+1] = new_G;
//					bm[m+2] = new_R;
//					//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
//				}
//
//
//			}
//		}
//#pragma endregion new method 4 commented by sriram
//	}
//	// CvInvoke.cvCvtColor(inp, inp1, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_HSV2RGB);
//	//  bm = inp1.ToBitmap();
//	//inp1.Dispose();
//	//inp.Dispose();
//
//}

extern "C" __declspec(dllexport)void ImageProc_HotspotCompensation(unsigned char* bm,IplImage*MonoChromeImg, int HsCenterX, int HsCenterY,  int imgWidth, int imgHeight, int HsRedPeak, int HsGreenPeak, int HsBluePeak,  int HsRedRadius, int HsGreenRadius, int HsBlueRadius, bool isColor)
{
		
      	if(isColor)
	{
		#pragma region Color mode Hotspot Compensation
	//returns Max of the 3 radius
			int maxRadius = HsRedRadius>HsGreenRadius ?
				HsRedRadius>HsBlueRadius ? HsRedRadius : HsBlueRadius  :
				HsGreenRadius>HsBlueRadius ? HsGreenRadius : HsBlueRadius ;

			for (int x = HsCenterX - maxRadius; x < HsCenterX + maxRadius; x++)
			{
				int offsetX = x - HsCenterX;
				int x2 = offsetX * offsetX;
				// y corresponds to row number // from top to bottom
						for (int y = HsCenterY - maxRadius; y < HsCenterY + maxRadius; y++)
						{

										int m = ((y * imgWidth) + x)*3; // 3 bytes per pixel

										int offsetY = y - HsCenterY;
										int y2 = offsetY * offsetY;
										float dist = x2 + y2;
										dist = (float)std::sqrt(dist);

										if (dist < HsRedRadius)
										{
											float ratio1 = (float) dist / (float) HsRedRadius;

											float cos_value = (float)(std::cos(pi * ratio1 ));
											float norm_cos = (1+cos_value) / 2; // restrict range from 1 to 0.

											// norm_cos *= norm_cos;// cos squared function
											int new_R = (int)(bm[m+2]);//.GetPixel(x, y).R);
											if (new_R != 255) // no processing on saturated pixel
												new_R = (int)((float)new_R - ((float)HsRedPeak * norm_cos));

											// overflow limit for pixel value
											new_R = std::max(0, new_R);
											bm[m+2] = new_R;
										}
										if (dist < HsGreenRadius)
										{
											float ratio1 = (float) dist / (float) HsGreenRadius;

											float cos_value = (float)(std::cos(pi * ratio1 ));
											float norm_cos = (1+cos_value) / 2; // restrict range from 1 to 0.

											// norm_cos *= norm_cos;// cos squared function
											int new_G = (int)(bm[m+1]);//.GetPixel(x, y).R);
											if (new_G != 255) // no processing on saturated pixel
												new_G = (int)((float)new_G - ((float)HsGreenPeak * norm_cos));

											// overflow limit for pixel value
											new_G= std::max(0, new_G);
											bm[m+1] = new_G;
										}
										if (dist < HsBlueRadius)
										{
											float ratio1 = (float) dist / (float) HsBlueRadius;

											float cos_value = (float)(std::cos(pi * ratio1 ));
											float norm_cos = (1+cos_value) / 2; // restrict range from 1 to 0.

											// norm_cos *= norm_cos;// cos squared function
											int new_B = (int)(bm[m]);//.GetPixel(x, y).R);
											if (new_B != 255) // no processing on saturated pixel
												new_B = (int)((float)new_B - ((float)HsBluePeak * norm_cos));

											// overflow limit for pixel value
											new_B = std::max(0, new_B);
											bm[m] = new_B;
										}
						}
			}
#pragma endregion
	}
	else
		HotspotCompensationMonoChrome(MonoChromeImg,HsCenterX, HsCenterY, imgWidth, imgHeight, HsRedPeak, HsRedRadius);
}


void HotspotCompensationMonoChrome(IplImage* bm, int HsCenterX, int HsCenterY,  int imgWidth, int imgHeight, int HsPeak, int HsRadius)
{
	//returns Max of the 3 radius
	int maxRadius = HsRadius;

	for (int x = HsCenterX - maxRadius; x < HsCenterX + maxRadius; x++)
	{
		int offsetX = x - HsCenterX;
		int x2 = offsetX * offsetX;
		// y corresponds to row number // from top to bottom
		for (int y = HsCenterY - maxRadius; y < HsCenterY + maxRadius; y++)
		{

			int m = ((y * imgWidth) + x); // 3 bytes per pixel

			int offsetY = y - HsCenterY;
			int y2 = offsetY * offsetY;
			float dist = x2 + y2;
			dist = (float)std::sqrt(dist);

			if (dist < HsRadius)
			{
				float ratio1 = (float) dist / (float) HsRadius;

				float cos_value = (float)(std::cos(pi * ratio1 ));
				float norm_cos = (1+cos_value) / 2; // restrict range from 1 to 0.

				// norm_cos *= norm_cos;// cos squared function
				int val = cvGet2D(bm,x,y).val[0];
				//uchar val = bm->at<uchar>(x,y);
				int new_R = (int)val;//.GetPixel(x, y).R);
				if (new_R != 255) // no processing on saturated pixel
					new_R = (int)((float)new_R - ((float)HsPeak * norm_cos));

				// overflow limit for pixel value
				new_R = std::max(0, new_R);
				cvSet2D(bm,x,y,cvScalarAll(new_R));
				//bm->at<uchar>(x,y) = (uchar)new_R;
			}
		}
	}

}


extern "C" __declspec(dllexport)void ImageProc_ShadowCompensation(unsigned char* bm,IplImage* MonoChromeImg, int imgCenterX, int imgCenterY,  int imgWidth, int imgHeight,  
																 int innerRad, int PeakRad1, int PeakRad2, int outerRad, 
																 int PeakDropPercentageR,int PeakDropPercentageG,int PeakDropPercentageB,bool isColor)
																 // all 3 shadow radius assumed same
{
	/*FILE* f = fopen("cosValues.csv","wb");
	fprintf(f,"cosval1,,cosval2 \n");*/
	if(isColor)
	{
	for (int x = imgCenterX - outerRad; x < imgCenterX + outerRad; x++)
	{
		int offsetX = x -imgCenterX;
		int x2 = offsetX * offsetX;
		// y corresponds to row number // from top to bottom
		for (int y =imgCenterY - outerRad; y < imgCenterY + outerRad; y++)
		{

			int m = ((y * imgWidth) + x)*3; // 3 bytes per pixel

			int offsetY = y -imgCenterY;
			int y2 = offsetY * offsetY;
			float dist = x2 + y2;
			dist = (float)std::sqrt(dist);
				if (dist < PeakRad1) // central increasing Shadow region 
			{
				if(innerRad > 0)  // if inner radius is positive
				{
					int shift1 = dist - innerRad; 
					int denom1 = PeakRad1 - innerRad; 

					float ratio1 = (float) shift1 / (float) denom1;

					float cos_value = 1 + (float)(std::cos(pi * ratio1 ));
					float norm_cos = (2-cos_value) / 2;
					float denomR =  1 - (((float)PeakDropPercentageR/100)*norm_cos); // 
					float denomG =  1 - (((float)PeakDropPercentageG/100)*norm_cos); // 
					float denomB =  1 - (((float)PeakDropPercentageB/100)*norm_cos); // 

					int new_R = (int)(bm[m+2]);//.GetPixel(x, y).R);
					if (new_R != 255) // no processing on saturated pixel
						new_R = (int)((float)new_R/denomR);
					int new_G = (int)bm[m+1];//.GetPixel(x, y).G;
					if (new_G != 255)
						new_G = (int)((float)new_G/denomG);
					int new_B = (int)bm[m];//.GetPixel(x, y).B;
					if (new_B != 255)
						new_B = (int)((float)new_B/denomB);

					// overflow limit for pixel value
					new_R = std::min(255, new_R);
					new_G = std::min(255, new_G);
					new_B = std::min(255, new_B);
					//inp.Data[x, y, 2] = Convert.ToByte(new_R);

					bm[m] = new_B;
					bm[m+1] = new_G;
					bm[m+2] = new_R;

				}
				else // inner radius less than zero
				{
					if(dist > std::abs(innerRad)) // controllered by single cos curve
					{
						int shift1 = dist - innerRad; 
						int denom1 = PeakRad1 - innerRad; 

						float ratio1 = (float) shift1 / (float) denom1;


						float cos_value = 1 + (float)(std::cos(pi * ratio1 ));
						float norm_cos = (2-cos_value) / 2;
						float denomR =  1 - (((float)PeakDropPercentageR/100)*norm_cos); // 
						float denomG =  1 - (((float)PeakDropPercentageG/100)*norm_cos); // 
						float denomB =  1 - (((float)PeakDropPercentageB/100)*norm_cos); // 

						int new_R = (int)(bm[m+2]);//.GetPixel(x, y).R);
						if (new_R != 255) // no processing on saturated pixel
							new_R = (int)((float)new_R/denomR);
						int new_G = (int)bm[m+1];//.GetPixel(x, y).G;
						if (new_G != 255)
							new_G = (int)((float)new_G/denomG);
						int new_B = (int)bm[m];//.GetPixel(x, y).B;
						if (new_B != 255)
							new_B = (int)((float)new_B/denomB);

						// overflow limit for pixel value
						new_R = std::min(255, new_R);
						new_G = std::min(255, new_G);
						new_B = std::min(255, new_B);
						//inp.Data[x, y, 2] = Convert.ToByte(new_R);

						bm[m] = new_B;
						bm[m+1] = new_G;
						bm[m+2] = new_R;
						//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
					}
					else // controlled by 2 intersecting cos curves
					{
						float norm_cos = 0.0;
						int shift2 = dist - innerRad;  // range us -innerrad to peakrad
						int denom2 = PeakRad1 - innerRad; 
						float ratio2 = (float)shift2/(float)denom2;
						float cos_Value2 =  1 + (float)(std::cos(pi * ratio2 )); // range 1 to -1

						float norm_cos2 = (2-cos_Value2) / 2;

						float shift1 =  std::abs( innerRad) - (float)dist; 
						int denom1 = PeakRad1  - innerRad; // range of the cos distribution 

						float ratio1 = (float) shift1 / (float) denom1;

						float cos_Value1 = 1+ (float)(std::cos(pi * ratio1 )); // range 2 to 0
						float cos_Value = cos_Value2 - (2- cos_Value1);

						//if(y == imgCenterY)
						//{
						//	string st = "";
						//	st =to_string(cos_Value1)+","+ to_string(cos_Value2)+","+ to_string(cos_Value)+"\n";
						//	//fprintf(f,st.c_str());
						//}
						//cos_Value1 = 2 - cos_Value1;
						//float norm_cos1 = (2-cos_Value1) / 2;   

						norm_cos = (2-cos_Value)/2; // invert range from 1 to 0
						//norm_cos  = norm_cos2 + norm_cos1;
						//norm_cos = norm_cos1+norm_cos2;

						float denomR =  1 - (((float)PeakDropPercentageR/100)*norm_cos); // 
						float denomG =  1 - (((float)PeakDropPercentageG/100)*norm_cos); // 
						float denomB =  1 - (((float)PeakDropPercentageB/100)*norm_cos); // 

						int new_R = (int)(bm[m+2]);//.GetPixel(x, y).R);
						if (new_R != 255) // no processing on saturated pixel
							new_R = (int)((float)new_R/denomR);
						int new_G = (int)bm[m+1];//.GetPixel(x, y).G;
						if (new_G != 255)
							new_G = (int)((float)new_G/denomG);
						int new_B = (int)bm[m];//.GetPixel(x, y).B;
						if (new_B != 255)
							new_B = (int)((float)new_B/denomB);

						// overflow limit for pixel value
						new_R = std::min(255, new_R);
						new_G = std::min(255, new_G);
						new_B = std::min(255, new_B);
						//inp.Data[x, y, 2] = Convert.ToByte(new_R);

						bm[m] = new_B;
						bm[m+1] = new_G;
						bm[m+2] = new_R;
					}
				}
			}
			else if (dist < PeakRad2 ) // central constant Shadow
			{
				float denomR =  1 - (((float)PeakDropPercentageR/100)); // 
				float denomG =  1 - (((float)PeakDropPercentageG/100)); // 
				float denomB =  1 - (((float)PeakDropPercentageB/100)); // 

				int new_R = (int)(bm[m+2]);//.GetPixel(x, y).R);
				if (new_R != 255) // no processing on saturated pixel
					new_R = (int)((float)new_R/denomR);
				int new_G = (int)bm[m+1];//.GetPixel(x, y).G;
				if (new_G != 255)
					new_G = (int)((float)new_G/denomG);
				int new_B = (int)bm[m];//.GetPixel(x, y).B;
				if (new_B != 255)
					new_B = (int)((float)new_B/denomB);

				// overflow limit for pixel value
				new_R = std::min(255, new_R);
				new_G = std::min(255, new_G);
				new_B = std::min(255, new_B);
				//inp.Data[x, y, 2] = Convert.ToByte(new_R);

				bm[m] = new_B;
				bm[m+1] = new_G;
				bm[m+2] = new_R;
				//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));

			}
			else if (dist < outerRad) // Outer Decreasing Shadow region 
			{
				int shift2 = dist - PeakRad2; 
				int denom3 = outerRad - PeakRad2; 

				float ratio2 = (float) shift2 / (float) denom3;
				float ratio3 = 1-ratio2; // decreasing factor as distance increases
				float cos_value = 1 + (float)(std::cos(pi * ratio3 ));
				float norm_cos = (2-cos_value) / 2;
				float denomR =  1 - (((float)PeakDropPercentageR/100)*norm_cos); // 
				float denomG =  1 - (((float)PeakDropPercentageG/100)*norm_cos); // 
				float denomB =  1 - (((float)PeakDropPercentageB/100)*norm_cos); // 

				int new_R = (int)(bm[m+2]);//.GetPixel(x, y).R);
				if (new_R != 255) // no processing on saturated pixel
					new_R = (int)((float)new_R/denomR);
				int new_G = (int)bm[m+1];//.GetPixel(x, y).G;
				if (new_G != 255)
					new_G = (int)((float)new_G/denomG);
				int new_B = (int)bm[m];//.GetPixel(x, y).B;
				if (new_B != 255)
					new_B = (int)((float)new_B/denomB);

				// overflow limit for pixel value
				new_R = std::min(255, new_R);
				new_G = std::min(255, new_G);
				new_B = std::min(255, new_B);
				//inp.Data[x, y, 2] = Convert.ToByte(new_R);

				bm[m] = new_B;
				bm[m+1] = new_G;
				bm[m+2] = new_R;
				//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
			}

		}
	}
	}
	else
		ShadowCompensationMonoChrome(MonoChromeImg,imgCenterX,imgCenterY,imgWidth,imgHeight,innerRad,PeakRad1,PeakRad2,outerRad,PeakDropPercentageR);
	//fclose(f);

}


void ShadowCompensationMonoChrome(IplImage* bm, int imgCenterX, int imgCenterY,  int imgWidth, int imgHeight,  
																 int innerRad, int PeakRad1, int PeakRad2, int outerRad, 
																 int PeakDropPercentage)
																 // all 3 shadow radius assumed same
{
	/*FILE* f = fopen("cosValues.csv","wb");
	fprintf(f,"cosval1,,cosval2 \n");*/
	for (int x = imgCenterX - outerRad; x < imgCenterX + outerRad; x++)
	{
		int offsetX = x -imgCenterX;
		int x2 = offsetX * offsetX;
		// y corresponds to row number // from top to bottom
		for (int y =imgCenterY - outerRad; y < imgCenterY + outerRad; y++)
		{

			int m = ((y * imgWidth) + x); // 3 bytes per pixel

			int offsetY = y -imgCenterY;
			int y2 = offsetY * offsetY;
			float dist = x2 + y2;
			dist = (float)std::sqrt(dist);
			if (dist < PeakRad1) // central increasing Shadow region 
			{
				if(innerRad > 0)  // if inner radius is positive
				{
					int shift1 = dist - innerRad; 
					int denom1 = PeakRad1 - innerRad; 

					float ratio1 = (float) shift1 / (float) denom1;

					float cos_value = 1 + (float)(std::cos(pi * ratio1 ));
					float norm_cos = (2-cos_value) / 2;
					float denomB =  1 - (((float)PeakDropPercentage/100)*norm_cos); // 
					//int val = bm->at<uchar>(x,y);
					int val = cvGet2D(bm,x,y).val[0];
					int new_B = (int)val; //.GetPixel(x, y).B;
					if (new_B != 255)
						new_B = (int)((float)new_B/denomB);

					// overflow limit for pixel value
					new_B = std::min(255, new_B);
					//inp.Data[x, y, 2] = Convert.ToByte(new_R);

					//bm->at<uchar>(x,y) =(uchar) new_B;
					cvSet2D(bm,x,y,cvScalarAll(new_B));

				}
				else // inner radius less than zero
				{
					if(dist > std::abs(innerRad)) // controllered by single cos curve
					{
						int shift1 = dist - innerRad; 
						int denom1 = PeakRad1 - innerRad; 

						float ratio1 = (float) shift1 / (float) denom1;


						float cos_value = 1 + (float)(std::cos(pi * ratio1 ));
						float norm_cos = (2-cos_value) / 2;
						float denomB =  1 - (((float)PeakDropPercentage/100)*norm_cos); // 
						//int val = bm->at<uchar>(x,y);
					int val = cvGet2D(bm,x,y).val[0];
						int new_B = (int)val;//.GetPixel(x, y).B;
						if (new_B != 255)
							new_B = (int)((float)new_B/denomB);

						// overflow limit for pixel value
						new_B = std::min(255, new_B);
						//inp.Data[x, y, 2] = Convert.ToByte(new_R);

						//bm->at<uchar>(x,y) =(uchar) new_B;
					cvSet2D(bm,x,y,cvScalarAll(new_B));
						//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
					}
					else // controlled by 2 intersecting cos curves
					{
						float norm_cos = 0.0;
						int shift2 = dist - innerRad;  // range us -innerrad to peakrad
						int denom2 = PeakRad1 - innerRad; 
						float ratio2 = (float)shift2/(float)denom2;
						float cos_Value2 =  1 + (float)(std::cos(pi * ratio2 )); // range 1 to -1

						float norm_cos2 = (2-cos_Value2) / 2;

						float shift1 =  std::abs( innerRad) - (float)dist; 
						int denom1 = PeakRad1  - innerRad; // range of the cos distribution 

						float ratio1 = (float) shift1 / (float) denom1;

						float cos_Value1 = 1+ (float)(std::cos(pi * ratio1 )); // range 2 to 0
						float cos_Value = cos_Value2 - (2- cos_Value1);

						//if(y == imgCenterY)
						//{
						//	string st = "";
						//	st =to_string(cos_Value1)+","+ to_string(cos_Value2)+","+ to_string(cos_Value)+"\n";
						//	//fprintf(f,st.c_str());
						//}
						//cos_Value1 = 2 - cos_Value1;
						//float norm_cos1 = (2-cos_Value1) / 2;   

						norm_cos = (2-cos_Value)/2; // invert range from 1 to 0
						//norm_cos  = norm_cos2 + norm_cos1;
						//norm_cos = norm_cos1+norm_cos2;

						float denomB =  1 - (((float)PeakDropPercentage/100)*norm_cos); // 

						//int val = bm->at<uchar>(x,y);
					int val = cvGet2D(bm,x,y).val[0];
						int new_B = (int)val; //.GetPixel(x, y).B;
						if (new_B != 255)
							new_B = (int)((float)new_B/denomB);

						// overflow limit for pixel value
						new_B = std::min(255, new_B);
						//inp.Data[x, y, 2] = Convert.ToByte(new_R);

						//bm->at<uchar>(x,y) =(uchar) new_B;
					cvSet2D(bm,x,y,cvScalarAll(new_B));
					}
				}
			}
			else if (dist < PeakRad2 ) // central constant Shadow
			{
				float denomB =  1 - (((float)PeakDropPercentage/100)); // 

				//int val = bm->at<uchar>(x,y);
					int val = cvGet2D(bm,x,y).val[0];
				int new_B = (int)val;//.GetPixel(x, y).B;
				if (new_B != 255)
					new_B = (int)((float)new_B/denomB);

				// overflow limit for pixel value
				new_B = std::min(255, new_B);
				//inp.Data[x, y, 2] = Convert.ToByte(new_R);

				//bm->at<uchar>(x,y) =(uchar) new_B;
					cvSet2D(bm,x,y,cvScalarAll(new_B));
				//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));

			}
			else if (dist < outerRad) // Outer Decreasing Shadow region 
			{
				int shift2 = dist - PeakRad2; 
				int denom3 = outerRad - PeakRad2; 

				float ratio2 = (float) shift2 / (float) denom3;
				float ratio3 = 1-ratio2; // decreasing factor as distance increases
				float cos_value = 1 + (float)(std::cos(pi * ratio3 ));
				float norm_cos = (2-cos_value) / 2;
				float denomB =  1 - (((float)PeakDropPercentage/100)*norm_cos); // 

				//int val = bm->at<uchar>(x,y);
					int val = cvGet2D(bm,x,y).val[0];
				int new_B = (int)val;//.GetPixel(x, y).B;
				if (new_B != 255)
					new_B = (int)((float)new_B/denomB);

				// overflow limit for pixel value
				new_B = std::min(255, new_B);
				//inp.Data[x, y, 2] = Convert.ToByte(new_R);

				//bm->at<uchar>(x,y) =(uchar) new_B;
					cvSet2D(bm,x,y,cvScalarAll(new_B));
				//bm.SetPixel(x, y, Color.FromArgb(new_R, new_G, new_B));
			}

		}
	}
	//fclose(f);

}

extern "C" __declspec(dllexport)void ImageProc_ApplyShift(unsigned char * bm,int factorX,int factorY,int width,int height)
{
	//// Image<Hsv, byte> inp = new Image<Hsv, byte>(bm);
	//           //Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
	//IplImage* inp = cvCreateImage(cvSize(width,height),IPL_DEPTH_8U,3);

	//           // take central portion of the image
	//           // Channel Zero is Blue. 
	//           // cannot set ROI independently
	//cvSetImageROI(inp,cvRect(factorX, factorY, inp->width - (2 * factorX), inp->height - (2 * factorY));
	////inp.ROI = new Rectangle(factorX, factorY, inp.Width - (2 * factorX), inp.Height - (2 * factorY));
	//IplImage* tempImg = cvCreateImage(cvSize(inp->width,inp->height),IPL_DEPTH_8U,1);
	//IplImage* tempImg1 = cvCreateImage(cvSize(inp->width,inp->height),IPL_DEPTH_8U,1);
	//cvSplit(inp,tempImg,0,0,0);
	//     
	//		//CvInvoke.cvCopy(inp[0], tempImg, IntPtr.Zero);

	//           // stretch the blue image by the input factors
	//cvResetImageROI(inp);        
	////inp.ROI = new Rectangle();
	//cvResize(tempImg,tempImg1,CV_INTER_CUBIC);  
	//
	////inp[0] = tempImg.Resize(inp.Width, inp.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);


	//           // shrink red channel image
	//           tempImg = inp[2].Resize(inp.Width - 2 * factorX, inp.Height - 2 * factorY, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

	//           Image<Gray, byte> tempImg2 = new Image<Gray, byte>(inp.Width, inp.Height);
	//           tempImg2.ROI = new Rectangle(factorX, factorY, inp.Width - 2 * factorX, inp.Height - 2 * factorY);
	//           CvInvoke.cvCopy(tempImg, tempImg2, IntPtr.Zero);

	//           tempImg2.ROI = new Rectangle();

	//           CvInvoke.cvMerge(inp[0], inp[1], tempImg2, IntPtr.Zero, inp);

	//           // resize image to the smaller size after shifting,
	//           // Did not work
	//           //inp.ROI = new Rectangle(factorX, factorY, inp.Width - (2 * factorX), inp.Height - (2 * factorY));

	//           bm = inp.ToBitmap();
	//           tempImg.Dispose();
	//           tempImg2.Dispose();
	//           inp.Dispose();
	//           GC.Collect();
}

void GetShadowMask(CvPoint centerShadow, int radiusShadow , Mat dstImg)
{
	CvRect Rect = cvRect((centerShadow.x-radiusShadow),(centerShadow.y-radiusShadow),(2*radiusShadow),(2*radiusShadow));
	//for method 1 use cvDrawRect
	//cvDrawRect(dstImg,cvPoint(Rect.x,Rect.y),cvPoint(Rect.x + Rect.width ,Rect.y +  Rect.height),cvScalarAll(255),-1);
	//cvDrawCircle(dstImg,cvPoint(Rect.x+Rect.width/2,Rect.y+ Rect.height/2), Rect.width/2 ,cvScalarAll(255),-1);
	circle(dstImg,Point(Rect.x+Rect.width/2,Rect.y+ Rect.height/2),Rect.width/2,Scalar(255),-1);
}

void GetROI(Mat dstImg1,Mat dstImg2)
{
	GetShadowMask(Center_shadow,radius_hotspot,inner_mask_boundry);
	GetShadowMask(Center_shadow,radius_shadow,outer_mask_boundry);
	GetShadowMask(Center_shadow,rim_thickness,circum_mask_boundry);

	subtract(outer_mask_boundry,inner_mask_boundry,dstImg1);
	subtract (outer_mask_boundry,inner_mask_boundry,dstImg1);
	//cvSaveImage("dstimg.png",dstImg1);
	subtract (circum_mask_boundry,outer_mask_boundry,dstImg2);
	//cvSaveImage("dstimg1.png",dstImg2);

}
void ContrastEnhance( Mat inps[], float clipVals[])
{

	Ptr<CLAHE> clahe = createCLAHE(1,Size(32,32));
	int size = *(&inps + 1) - inps;
	Mat dst;
	vector<Mat> channels;
	/*for (int i = 0; i < size; i++)
	{
			clahe->setClipLimit(clipVals[i]);
	clahe->apply(inps[i],inps[i]);
	}*/
	for (int i = 0; i < size; i++)
	{
			clahe->setClipLimit(clipVals[i]);
			clahe->apply(inps[i],inps[i]);
			channels.push_back(inps[i]);
		/*IplImage temp1 = inps[i];
		cvCLAdaptEqualize(&temp1,&temp1,clipVals[1]);*/
	}
	//merge(channels,*output);
	//cvMerge(inps[0],inps[1],inps[2],0,output);
	/*clahe->setClipLimit(clipVals[2]);
	clahe->apply(inps[2],inps[2]);

	clahe->setClipLimit(clipVals[1]);
	clahe->apply(inps[1],inps[1]);

	clahe->setClipLimit(clipVals[1]);
	clahe->apply(inps[1],inps[1]);*/
	//float clipG=0;	//green channel clip val
	//cvAvgSdv(grayImgR,&avgRGB,&stdV);
	//if(avgRGB.val[0]<24)

	{
		//clipG= 0.001;
	}
	/*else if(avgRGB.val[0]<80)
	{
	clipG= 0.0025;
	}else{
	clipG= 0.0015;
	}*/
	//cvCLAdaptEqualize(grayImgR, grayImgR, clipR);								//clahe Red channelcvAvgSdv(grayImgG,&avgRGB,&stdV);

	//cvAvgSdv(grayImgG,&avgRGB,&stdV);

	//if(avgRGB.val[0]<24)
	//{
	//	clipG= 0.002;
	//}
	//else if(avgRGB.val[0]<80)
	//{
	//	clipG= 0.0025;
	//}else{
	//clipG= 0.001;
	//}
	//cvCLAdaptEqualize(grayImgG, grayImgG, clipG);								//clahe Green channel
	//cvAvgSdv(grayImgB,&avgRGB,&stdV);

	//if(avgRGB.val[0]<24)
	//{
	//	clipG= 0.002;
	//}
	//else if(avgRGB.val[0]<80)
	//{
	//	clipG= 0.05;
	//}else{
	//clipG= 0.001;
	//}
	//cvCLAdaptEqualize(grayImgB, grayImgB, clipB);								//clahe Blue channel
}

extern "C" __declspec(dllexport)void ApplyClaheCPlusPlus(uchar* inps , Mat* output, double clipVals[],int count)
{

	Ptr<CLAHE> clahe = createCLAHE(1,Size(32,32));
	
	Mat dst ,src[3];
	vector<Mat> channels;
	/*for (int i = 0; i < size; i++)
	{
			clahe->setClipLimit(clipVals[i]);
	clahe->apply(inps[i],inps[i]);
	}*/
	 inp.data = inps;
	/*if(count == 1)
	{
		chan1->imageData = inps->imageData;
	channels.push_back(cvarrToMat(chan1));

	}
	else*/
	{
		split(inp,src);
	// cvSplit(inps,chan1,chan2,chan3,0);
	}
	

	for (int i = 0; i < count; i++)
	{

			clahe->setClipLimit(clipVals[i]);
			clahe->apply(src[i],src[i]);
			channels.push_back(src[i]);
		/*IplImage temp1 = inps[i];
		cvCLAdaptEqualize(&temp1,&temp1,clipVals[1]);*/
	}
	merge(channels,dst);
	dst.copyTo(*output);
	dst.release();
	channels.clear();
	src->release();
	//inps = dst.data;
	/*if(count > 1)
	{
	cvReleaseImage(&chan2);
	cvReleaseImage(&chan3);
	}*///cvMerge(inps[0],inps[1],inps[2],0,output);
	/*clahe->setClipLimit(clipVals[2]);
	clahe->apply(inps[2],inps[2]);

	clahe->setClipLimit(clipVals[1]);
	clahe->apply(inps[1],inps[1]);

	clahe->setClipLimit(clipVals[1]);
	clahe->apply(inps[1],inps[1]);*/
	//float clipG=0;	//green channel clip val
	//cvAvgSdv(grayImgR,&avgRGB,&stdV);
	//if(avgRGB.val[0]<24)

	{
		//clipG= 0.001;
	}
	/*else if(avgRGB.val[0]<80)
	{
	clipG= 0.0025;
	}else{
	clipG= 0.0015;
	}*/
	//cvCLAdaptEqualize(grayImgR, grayImgR, clipR);								//clahe Red channelcvAvgSdv(grayImgG,&avgRGB,&stdV);

	//cvAvgSdv(grayImgG,&avgRGB,&stdV);

	//if(avgRGB.val[0]<24)
	//{
	//	clipG= 0.002;
	//}
	//else if(avgRGB.val[0]<80)
	//{
	//	clipG= 0.0025;
	//}else{
	//clipG= 0.001;
	//}
	//cvCLAdaptEqualize(grayImgG, grayImgG, clipG);								//clahe Green channel
	//cvAvgSdv(grayImgB,&avgRGB,&stdV);

	//if(avgRGB.val[0]<24)
	//{
	//	clipG= 0.002;
	//}
	//else if(avgRGB.val[0]<80)
	//{
	//	clipG= 0.05;
	//}else{
	//clipG= 0.001;
	//}
	//cvCLAdaptEqualize(grayImgB, grayImgB, clipB);								//clahe Blue channel
}
//void ContrastEnhance( IplImage* grayImgB, IplImage* grayImgG, IplImage* grayImgR,float clipR,float clipG,float clipB)
//{
//	Ptr<CLAHE> clahe = createCLAHE();
//	clahe->setClipLimit(clipR);
//	clahe->apply(grayImgR,grayImgR);
//	//float clipG=0;	//green channel clip val
//	//cvAvgSdv(grayImgR,&avgRGB,&stdV);
//	//if(avgRGB.val[0]<24)
//
//	{
//		//clipG= 0.001;
//	}
//	/*else if(avgRGB.val[0]<80)
//	{
//	clipG= 0.0025;
//	}else{
//	clipG= 0.0015;
//	}*/
//	cvCLAdaptEqualize(grayImgR, grayImgR, clipR);								//clahe Red channelcvAvgSdv(grayImgG,&avgRGB,&stdV);
//
//	//cvAvgSdv(grayImgG,&avgRGB,&stdV);
//
//	//if(avgRGB.val[0]<24)
//	//{
//	//	clipG= 0.002;
//	//}
//	//else if(avgRGB.val[0]<80)
//	//{
//	//	clipG= 0.0025;
//	//}else{
//	//clipG= 0.001;
//	//}
//	cvCLAdaptEqualize(grayImgG, grayImgG, clipG);								//clahe Green channel
//	//cvAvgSdv(grayImgB,&avgRGB,&stdV);
//
//	//if(avgRGB.val[0]<24)
//	//{
//	//	clipG= 0.002;
//	//}
//	//else if(avgRGB.val[0]<80)
//	//{
//	//	clipG= 0.05;
//	//}else{
//	//clipG= 0.001;
//	//}
//	cvCLAdaptEqualize(grayImgB, grayImgB, clipB);								//clahe Blue channel
//}
//void ContrastEnhance( IplImage* grayImg ,float clipVal)
//{
//
//
//	cvCLAdaptEqualize(grayImgR, grayImgR, clipR);								//clahe Red channelcvAvgSdv(grayImgG,&avgRGB,&stdV);
//
//}
void GetHistogram(Mat srcImg,Mat mask)
{

	int hist_size = 256;      
	float Histrange[]={0,255};
	//float* ranges[] = { Histrange };

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
	cv::calcHist( &srcImg, 1, 0, mask, // do not use mask
             hist, 1, histSize, ranges,
             true, // the histogram is uniform
             false );
}
double GetEntropy(Mat srcImg , Mat mask)
{
	
	GetHistogram(srcImg,mask);
	double dProbability = 0.0;
	double dEntropy =0.0;
	double nPixels =0;
	if(!mask.empty())
	nPixels = (double) countNonZero(mask);// srcImg->width * srcImg->height;
	else
	{
		Size wholeSize;
		Point p;
		srcImg.locateROI(wholeSize,p);
		if(wholeSize.height != 0 && wholeSize.width != 0)
			nPixels = (double)wholeSize.height * wholeSize.width ;//->height;
		else
	nPixels = (double)srcImg.cols * srcImg.rows;
	}
	for(uint32_t i = 0; i <= 255; i++)
	{

		//double val =  cvGetReal1D(hist->bins,i);
		double val = hist.at<float>(i); //cvGetReal1D(hist->bins,i);

		dProbability =(double)val / nPixels;
		if(dProbability > 0.000000001)
			dEntropy += dProbability*log(dProbability) ;
	}
	dEntropy=dEntropy/log(2.0);
	hist.~Mat();
	//cvReleaseHist(&hist);
	return (-dEntropy);
}

double GetPercentageChange (double valueShadowed,double valueCircum)
{
	double percentageChange = (valueShadowed - valueCircum)*100/valueShadowed;
	return (percentageChange);
}

CvRect GetRectangle(CvPoint centerShadow, int radiusShadow , IplImage* dstImg)
{
	CvRect Rect = cvRect((centerShadow.x-radiusShadow),(centerShadow.y-radiusShadow),(2*radiusShadow),(2*radiusShadow));

	//cvDrawRect(dstImg,cvPoint(Rect.x,Rect.y),cvPoint(Rect.x + Rect.width ,Rect.y +  Rect.height),cvScalarAll(255),-1);
	cvDrawCircle(dstImg,cvPoint(Rect.x+Rect.width/2,Rect.y+ Rect.height/2), Rect.width/2 ,cvScalarAll(255),-1);
	return Rect;
}
float ThresholdmaxValue(float high_threshold)
{
	CumhistArrayB[0] =histArrayB[0];
	CumhistArrayG[0] =histArrayG[0];
	CumhistArrayR[0] =histArrayR[0];

	for (int i = 1; i < 1024; i++)
	{
		CumhistArrayB[i] = histArrayB[i] +CumhistArrayB [i-1];
		CumhistArrayG[i] = histArrayG[i]+CumhistArrayG[i-1];
		CumhistArrayR[i]  =histArrayR[i]+CumhistArrayR[i-1];
	}

	/*FILE* f = fopen("hist.csv","wb");
	fprintf(f,"pixelNo,,valB,,valG,,valR,,cumsumB,,cumsumG,,cumsumR \n");
	for (int i = 0; i < 1024; i++)
	{

	string st =to_string(i)+", ,"+ to_string((int)histArrayB[i])+", ,"+ to_string((int)histArrayG[i])+", ,"+to_string((int)histArrayR[i])+", ,"+ to_string((int)CumhistArrayB[i])+", ,"+ to_string((int)CumhistArrayG[i])+", ,"+to_string((int)CumhistArrayR[i])+"\n";

	fprintf(f,st.c_str());
	}
	fclose(f);*/



	float maxHistRangeB =(float)CumhistArrayB[1023] * high_threshold;
	float maxHistRangeG =(float)CumhistArrayG[1023] * high_threshold;
	float maxHistRangeR =(float)CumhistArrayR[1023] * high_threshold;
	for (int i = 1023; i > 0; i--)
	{
		if(CumhistArrayB[i] < maxHistRangeB)
		{
			pixelMaxB = i;
			break;
		}
	}
	for (int i = 1023; i > 0; i--)
	{
		if(CumhistArrayG[i] < maxHistRangeG)
		{
			pixelMaxG = i;
			break;
		}
	}
	for (int i = 1023; i > 0; i--)
	{
		if(CumhistArrayR[i] < maxHistRangeR)
		{
			pixelMaxR = i;
			break;
		}
	}

	return (pixelMaxB>pixelMaxG)?
		(pixelMaxB>pixelMaxR)?pixelMaxB:pixelMaxR
		:(pixelMaxG>pixelMaxR)?pixelMaxG:pixelMaxR;
}
int ImageProc_computeOutput(int x, int r1, int s1, int r2, int s2)
{
	float result;
	if(0 <= x && x <= r1){
		result = s1/r1 * x;
	}else if(r1 < x && x <= r2){
		result = ((s2 - s1)/(r2 - r1)) * (x - r1) + s1;
	}else if(r2 < x && x <= 255){
		result = ((255 - s2)/(255 - r2)) * (x - r2) + s2;
	}
	return (int)result;
}
void create_histogram_image(IplImage* gray_img, IplImage* hist_img) {
	CvHistogram *hist;

	int hist_size = 256;     
	float rangeHist[]={0,256};
	float* ranges[] = { rangeHist };
	float max_value = 0.0;
	float w_scale = 0.0 ;//#000000;">;

	// create array to hold histogram values
	hist = cvCreateHist(1, &hist_size, CV_HIST_ARRAY, ranges, 1);

	// calculate histogram values 
	cvCalcHist( &gray_img, hist, 0, NULL );

	// Get the minimum and maximum values of the histogram 
	cvGetMinMaxHistValue( hist, 0, &max_value, 0, 0 );

	// set height by using maximim value
	cvScale( hist->bins, hist->bins, ((float)hist_img->height)/max_value, 0 );

	// calculate width
	w_scale = ((float)hist_img->width)/hist_size;

	// plot the histogram 
	for( int i = 0; i < hist_size; i++ ) {

		cvRectangle( hist_img, cvPoint((int)i*w_scale , hist_img->height),
			cvPoint((int)(i+1)*w_scale, hist_img->height - cvRound(cvGetReal1D(hist->bins,i))),
			cvScalar(0), -1, 8, 0 );

	}
}
extern "C" __declspec(dllexport)void ImageProc_ApplyUnsharpMask(uchar* inp,IplImage* MonoChromeImg, double thres,double amount,double sigma,int medianFilterSize, bool isColor)
{
    #pragma region Color Un sharp Mask
	if(isColor)
	{
		Mat blurred,img; 
		//srcImg->imageData = inp;
		srcImg.data = inp;

		//IplImage* blurred = cvCreateImage(cvGetSize(srcImg),srcImg->depth,srcImg->nChannels); 
		//srcImg->imageData = inp;
		//Mat img = cvarrToMat(srcImg);
		 srcImg.copyTo(img);
		//cvSmooth(srcImg,srcImg,CV_MEDIAN,medianFilterSize);
		medianBlur(img,img,medianFilterSize);
		
		//
		cv::Size s = Size();
		//sigma = 1;
		//cvSmooth(srcImg,blurred,CV_GAUSSIAN,sigma);
		GaussianBlur(img, blurred, s, sigma, sigma);
		//GaussianBlur(img, blurred, 3, sigma, sigma);
		//img.copyTo(blurred);
		//Mat lowConstrastMask = abs(img - blurred);
		// lowConstrastMask = lowConstrastMask < thres;
		//IplImage* dest1 = cvCloneImage(&(IplImage)lowConstrastMask);

		//threshold(lowConstrastMask,lowConstrastMask,thres,255,1);
		/*cvConvertScale(srcImg,srcImg,(1+amount));
		cvConvertScale(blurred,blurred,-amount);
		cvAdd(srcImg,blurred,srcImg);*/
		 //Mat lowContrastMask = abs(img-blurred)<thres;  
		srcImg = img*(1+amount) + blurred*(-amount);
		//srcImg = srcImg*(1+amount) + blurred*(-amount);
		//imwrite("save.png",sharpened);
		//img.copyTo(sharpened, lowContrastMask);
		//IplImage* dest1 = cvCloneImage(&(IplImage)sharpened);
		//img.release();
		//lowContrastMask.release();
		/*sharpened.copyTo(srcImg);
		sharpened.release();*/
		//cvCopy(dest1,srcImg);
		//cvReleaseImage(&dest1);
		/*cvReleaseImage(&blue);
		cvReleaseImage(&green);
		cvReleaseImage(&red);*/

		blurred.release();
		img.release();
		//Mat blurred; 

		//Mat img(inp);
		//inp = srcImg->imageData;
		inp = srcImg.data;
		////cvtColor(img,dst,CV_BGR2GRAY);
		////cvtColor(dst,img,CV_GRAY2BGR);
		//GaussianBlur(img, blurred, Size(), sigma, sigma);
		//Mat lowConstrastMask = abs(img - blurred);
		//// lowConstrastMask = lowConstrastMask < thresh;
		//IplImage* dest1 = cvCloneImage(&(IplImage)lowConstrastMask);

		//threshold(lowConstrastMask,lowConstrastMask,thres,255,1);


		//Mat sharpened = img*(1+amount) + blurred*(-amount);
		////imwrite("save.png",sharpened);
		////inp = cvCloneImage(&(IplImage)sharpened);
		//inp = cvCloneImage(&(IplImage)sharpened);

		////img.copyTo(sharpened, lowConstrastMask);
		//img.release();
		//lowConstrastMask.release();
		//sharpened.release();
		////cvCopy(dest1,inp);
		////cvReleaseImage(&dest1);

		//blurred.release();
		// return dest1;
	}
#pragma endregion

#pragma region Monochrome Un Sharp Mask
	else
	{
		ApplyUnsharpMaskMonoChrome(MonoChromeImg,thres,amount,sigma,medianFilterSize);
	}
#pragma endregion
}

void ApplyUnsharpMaskMonoChrome(IplImage* inp,double thres,double amount,double sigma,int medianFilterSize)
{
		Mat blurred,img; 
		grayImg.data = (uchar*)inp->imageData;

		grayImg.copyTo(img);
		medianBlur(img,img,medianFilterSize);
		
		cv::Size s = Size();
		GaussianBlur(img, blurred, s, sigma, sigma);
		grayImg = img*(1+amount) + blurred*(-amount);

		blurred.release();
		img.release();
		inp->imageData =(char*) grayImg.data;//copyTo(inp);
}

extern "C" _declspec(dllexport)void ImageProc_Denoise(unsigned char* inpPtr,int filterVal)
{
	srcImg.data = inpPtr;
	medianBlur(srcImg,srcImg,filterVal);
	inpPtr = srcImg.data;


}
extern "C" _declspec(dllexport)void ImageProc_BoostImage(unsigned char* inpPtr,float BoostVal)
{
	srcImg.data = inpPtr;
	srcImg = srcImg * BoostVal;
	inpPtr = srcImg.data;


}
extern "C" _declspec(dllexport)void CreateMask(Mat maskImg,bool isLive)
{
	if(isLive)
	{
		if(LiveMask.cols != maskImg.cols|| LiveMask.rows!= maskImg.rows)
	{
		LiveMask.~Mat();
		//cvReleaseImage(&LiveMask);
		LiveMask = Mat(maskImg.rows,maskImg.cols,CV_8UC3,3);
	}
	//cvMerge(maskImg,maskImg,maskImg,0,LiveMask);
	//cvCopy(maskImg,LiveMask);
		//cv::convertsc
	cvConvertScale(&maskImg,&LiveMask,1/255.0);
	}
	else
	{
		if(CaptureMask.cols != maskImg.cols|| CaptureMask.rows!= maskImg.rows)
	{
		CaptureMask.~Mat();
		CaptureMask = Mat(maskImg.rows,maskImg.cols,CV_8UC3,3);
    // CaptureMask = cvCreateImage(cvGetSize(maskImg),maskImg->depth,1);
	}
	//cvMerge(maskImg,maskImg,maskImg,0,CaptureMask);

	//cvCopy(maskImg,CaptureMask);
	//cvConvertScale(maskImg,CaptureMask,1/255.0);

	}
}

// To apply mask on the image
extern "C" _declspec(dllexport) void ImageProc_ApplyMask( uchar * inputptr,bool isLive,int width,int height)
{
	
	//IplImage* temp1 = cvCreateImage(cvGetSize(mask),mask->depth,3);
	//if(temp.cols != width || temp.rows != height)
	//{
	//	temp.~Mat();
 //    //cvReleaseImage(&temp);
	// //temp = cvCreateImage(cvSize(width,height),IPL_DEPTH_8U,3);
	//	temp = Mat(height,width,CV_8UC3);
	//}
	Mat maskArr[] = {CaptureMask,CaptureMask,CaptureMask}; 
	merge(maskArr,3,temp);
	// cvSaveImage("maskImage.png",CaptureMask);
	//if(srcImg1.cols != width || srcImg1.rows != height)
	//{
	//	srcImg1.~Mat();
 //    //cvReleaseImage(&srcImg1);
	// srcImg1 = Mat(height,width,CV_8UC3);
	//}
	srcImg1.data = inputptr;
	//cvSetZero(temp);// set the output image to zero	
	//
	//if(isLive)
	//{
	bitwise_and(srcImg1,temp,srcImg1);
	//cvAnd(srcImg1,temp,srcImg1);
	//	cvCopy(srcImg1,temp,LiveMask);// copy the input to the destination image using the mask
	//}
	//else
	//{
	//	//cvCopy(srcImg1,temp,CaptureMask);// copy the input to the destination image using the mask
	//}
	//cvCopy(temp,srcImg1);//copy the destination to the input image;
	/*if(isLive)
		for (int i = 0; i < LiveMask->width; i++)
	{
		for (int j = 0; j < LiveMask->height; j++)
		{
			int indx = width * j + i;
			if(cvGet2D(LiveMask,j,i).val[0] == 0)
			{
				inputptr[indx] = 0;
				inputptr[indx+1] = 0;
				inputptr[indx+2] = 0;
			}


		}
	}
	else
		for (int i = 0; i < CaptureMask->width; i++)
	{
		for (int j = 0; j < CaptureMask->height; j++)
		{
			int indx = width * j + i;
			if(cvGet2D(CaptureMask,j,i).val[0] == 0)
			{
				inputptr[indx] = 0;
				inputptr[indx+1] = 0;
				inputptr[indx+2] = 0;
			}
		}
	}*/
	//cvSaveImage("result.png",srcImg1);

	//cvCopy(temp,srcImg1);
	//inputptr = (char *) srcImg1->imageData; // give result ptr to input ptr
	inputptr =  srcImg1.data; // give result ptr to input ptr
}
//extern "C"_declspec(dllexport) void ImageProc_ApplyClaheSingleChannel(iplimage)
//{
//}
extern "C" _declspec(dllexport)void ImageProc_GetGreenRedAvgImage(uchar* inputptr)
{
	srcImg.data = inputptr;// input ptr to result pointer
	Mat inps[3];
	split(srcImg,inps);
	//cvSplit(srcImg,0,greenImg,redImg,0); // get red and green channel from actual image
	//inps[1] = inps[1] + inps[2];
	add(inps[1],inps[2],inps[1]);
	//cvAdd(greenImg,redImg,greenImg); // add green and red channel image put the result to green channel image
	inps[1].convertTo(inps[2],CV_8UC1,0.5);
	//cvConvertScale(greenImg,greenImg,0.5); // Divide the result image of green+ red by 2 to get the average of the two channels
     inps[0] = inps[1];
      inps[2] = inps[1];
     merge(inps,3,srcImg);
	//cvMerge(greenImg,greenImg,greenImg,0,srcImg);// put all three channels with (green+red)/2 image
	inputptr = srcImg.data; // result pointer to input pointer
}
extern "C" _declspec(dllexport)void ImageProc_GetMonoChromeImage( uchar* inputptr)
{
	srcImg.data = inputptr;
	Mat inps[3];
	split(srcImg,inps);
	 inps[0] = inps[1];
      inps[2] = inps[1];
     merge(inps,3,srcImg);
	 inps->release();
	//cvSplit(srcImg,0,greenImg,0,0);
	//cvMerge(greenImg,greenImg,greenImg,0,srcImg);
	//inputptr = (char *) srcImg->imageData;
	inputptr = srcImg.data;

}
extern "C" _declspec(dllexport)void ImageProc_ApplyClahe(unsigned char* inputPtr,Mat* MonoChromeImg,int width ,int height,float ClipValR ,float ClipValG ,float ClipValB,bool isColor)
{
	if(isColor)
	{
	/*IplImage* inpImg  = cvCreateImage(cvSize(width, height ),IPL_DEPTH_8U,3);
	IplImage* RImg  = cvCreateImage(cvSize(width, height),IPL_DEPTH_8U,1);
	IplImage* GImg  = cvCreateImage(cvSize(width, height),IPL_DEPTH_8U,1);
	IplImage* BImg  = cvCreateImage(cvSize(width, height),IPL_DEPTH_8U,1);*/
	//inpImg->imageData =(char*) inputPtr;
	srcImg.data =inputPtr;
	//cvCvtColor(inpImg,inpImg,CV_BGR2HSV);
	//cvSplit(inpImg,BImg,GImg,RImg,0);
	Mat splitArr[3];
	split(srcImg,splitArr);
	float arr[] = {ClipValB,ClipValG,ClipValR};
	//createCLAHE(
	//ContrastEnhance(BImg,GImg,RImg,ClipValR,ClipValG,ClipValB);
	ContrastEnhance(splitArr,arr);
	//cvMerge(BImg,GImg,RImg,0,inpImg);
	//cvCvtColor(inpImg,inpImg,CV_HSV2BGR);
	merge(splitArr,3,srcImg);
	splitArr->release();
	inputPtr=(unsigned char *) srcImg.data;
	//splitArr->release();
	}
	else
	{
		ApplyClaheMonoChrome(MonoChromeImg,width,height,ClipValR);
	}
	/*cvReleaseImage(&BImg);
	cvReleaseImage(&RImg);
	cvReleaseImage(&GImg);*/
}

void ApplyClaheMonoChrome(Mat* inputPtr,int width ,int height,float ClipVal )
{
	
	/*IplImage* inpImg  = cvCreateImage(cvSize(width, height ),IPL_DEPTH_8U,3);
	IplImage* RImg  = cvCreateImage(cvSize(width, height),IPL_DEPTH_8U,1);
	IplImage* GImg  = cvCreateImage(cvSize(width, height),IPL_DEPTH_8U,1);
	IplImage* BImg  = cvCreateImage(cvSize(width, height),IPL_DEPTH_8U,1);*/
	//inpImg->imageData =(char*) inputPtr;
	//srcImg.data =inputPtr;
	//cvCvtColor(inpImg,inpImg,CV_BGR2HSV);
	//cvSplit(inpImg,BImg,GImg,RImg,0);
	/*Mat splitArr[3];
	split(srcImg,splitArr);*/
	//createCLAHE(
	//ContrastEnhance(BImg,GImg,RImg,ClipValR,ClipValG,ClipValB);
	//Mat tempImg = Mat(height,width,grayImg.type());
	grayImg = cvarrToMat(inputPtr);
	//grayImg.data = inputPtr;
	/*Mat splitArr[] = { grayImg };
	float arr[] = { ClipVal };*/
	/*ContrastEnhance(splitArr,arr);*/
	Ptr<CLAHE> clahe = createCLAHE(1,Size(32,32));
	/*for (int i = 0; i < size; i++)
	{
			clahe->setClipLimit(clipVals[i]);
	clahe->apply(inps[i],inps[i]);
	}*/
	{
			clahe->setClipLimit(ClipVal);
			clahe->apply(grayImg,grayImg);
			inputPtr = &grayImg;
		/*IplImage temp1 = inps[i];
		cvCLAdaptEqualize(&temp1,&temp1,clipVals[1]);*/
	}

	//cvMerge(BImg,GImg,RImg,0,inpImg);
	//cvCvtColor(inpImg,inpImg,CV_HSV2BGR);
	//merge(splitArr,3,srcImg);
	//splitArr->release();
	//cvCopy(grayImg,inputPtr);

	//grayImg.copyTo(inputPtr);
	//inputPtr = &(IplImage) grayImg.clone();

	/*cvReleaseImage(&BImg);
	cvReleaseImage(&RImg);
	cvReleaseImage(&GImg);*/
}


extern "C" _declspec(dllexport)void ImageProc_ApplyHSVBOOSt(uchar* inputPtr,Mat outputImg,float Value,int width,int height)
{
	Mat rgbImg_byte;
	outputImg.copySize(rgbImg_byte);
	rgbImg_byte.data = inputPtr;

	Mat rgbImg_float(rgbImg_byte.cols,rgbImg_byte.rows, CV_32FC3, Scalar(0,0,0));
	Mat hsvimg_float(rgbImg_byte.cols,rgbImg_byte.rows, CV_32FC3, Scalar(0,0,0));
	Mat vChan(rgbImg_byte.cols,rgbImg_byte.rows, CV_32FC1, Scalar(0,0,0));
	rgbImg_byte.convertTo(rgbImg_float,CV_32FC3,1.0,0);
	Mat channels[3];
	cvtColor(rgbImg_float,hsvimg_float,CV_BGR2HLS);
	split(hsvimg_float,channels);
	//cvSplit(hsvimg_float,channels[0],channels[1],channels[2],0);
	//channels[1].copyTo(vChan);
	//vChan = channels[1].clone();
	Value = Value / (float)(mean(channels[1]).val[0]);//.GetAverage().Intensity;
	channels[1].convertTo(channels[1],CV_32FC1,Value,0);
	//channels[1] = vChan.clone();
	merge(channels,3,hsvimg_float);
	cvtColor(hsvimg_float, rgbImg_float, CV_HLS2BGR);
	rgbImg_float.convertTo(rgbImg_byte,CV_8UC3, 1,0);

	//inputImg=cvCloneImage(&(IplImage)rgbImg_byte);
	rgbImg_byte.copyTo(outputImg);
	//cvCopy(inputImg,outputImg);

	rgbImg_byte.release();
	rgbImg_float.release();
	hsvimg_float.release();
	vChan.release();

}

extern "C" __declspec(dllexport) void ImageProc_ApplyLUT(unsigned char* inp,IplImage* MonoChromeImg,int width,int height,bool is8bit,bool isColor, bool isChannelWise)
{
	if(isChannelWise)
		ApplyLUT_ChannelWise(inp, MonoChromeImg,width, height, is8bit, isColor);
	else
		ApplyLUT(inp, MonoChromeImg,width, height, is8bit, isColor);

}

void ApplyLUT(unsigned char* inp,IplImage* MonoChromeImg,int width,int height,bool is8bit,bool isColor)
{
	if(isColor)
	{
		if(is8bit)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int m = (i * width + j)*3;

						uint8_t val = inp[m];
						val = LUTValues_8[inp[m]];
						inp[m] = val;
						val = inp[m+1];
						val = LUTValues_8[inp[m+1]];
						if(val > 255)
							val = 255;

						inp[m+1] = val ;
						val = inp[m+2];
						val = LUTValues_8[inp[m+2]];
						inp[m+2] = val;


				}
			}
		}
	}
	else
	{
		ApplyLUTMonoChrome(MonoChromeImg,width,height,is8bit);
	}
}


void ApplyLUT_ChannelWise(unsigned char* inp,IplImage* MonoChromeImg,int width,int height,bool is8bit,bool isColor)
{
	if(isColor)
	{
		if(is8bit)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int m = (i * width + j)*3;

						uint8_t val = inp[m];
						val = LUTValues_8B[inp[m]];
						inp[m] = val;
						val = inp[m+1];
						val = LUTValues_8G[inp[m+1]];
						if(val > 255)
							val = 255;

						inp[m+1] = val ;
						val = inp[m+2];
						val = LUTValues_8R[inp[m+2]];
						inp[m+2] = val;


				}
			}
		}
	}
	else
	{
		ApplyLUTMonoChrome(MonoChromeImg,width,height,is8bit);
	}
}

void ApplyLUTMonoChrome(IplImage* inp,int width,int height,bool is8bit)
{
	grayImg.data = (uchar*)inp->imageData;
		if(is8bit)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					uint8_t val = grayImg.at<uchar>(i,j) ;
					val = LUTValues_8[val];
					grayImg.at<uchar>(i,j)  = val;
				}
			}
			
		}
		inp->imageData = (char*)grayImg.data;
 }
extern "C" __declspec(dllexport) void ImageProc_CalculateLut(double sineFactor,double interval1,double interval2 ,int bitDepth,bool isForuteenBit,int offset, bool isChannelWise, int channelCode )
{
	if(isChannelWise)
		ImageProc_CalculateChannelWiseLut(sineFactor,interval1,interval2, bitDepth,isForuteenBit,offset,channelCode);
	else
		ImageProc_NonChannelWiseCalculateLut(sineFactor,interval1,interval2, bitDepth,isForuteenBit,offset);
}

void ImageProc_NonChannelWiseCalculateLut(double sineFactor,double interval1,double interval2 ,int bitDepth,bool isForuteenBit,int offset)
{
	pi = 4 * std::atan(1);
	int  maxValue =(int)std::pow(2, bitDepth);
	int quaterthValue = (int)maxValue / 4;
	int halfthValue = (int)2* quaterthValue ;
	int threeFourthValue = (int)3 * quaterthValue;
	//	sineFactor = 4096;
	//	interval1 = 4096;  // 1st phase of curve is from 0 to interval 1 -- Sine wave (0 to PI/2)//
	//	interval2 = 8192; // 2nd phase of curve is from interval1 to interval1+interval 2  -- Cosine Wave (0 to PI)
	// 3rd phase of the curve is from interval1+interval2 upto max value -- Linear

	double CosineFactor = sineFactor/2; // nominally be sinefactor/2
	FILE* f = fopen("lut.csv","wb");
	fprintf(f,"pixelNo,,val \n");
	if(isForuteenBit)
		LUTValues = (uint16_t*) malloc(sizeof(uint16_t)*maxValue);// uint16_t lut 
	else
		LUTValues_8 = (uint8_t*) malloc(sizeof(uint8_t)*maxValue);// uint8_t bit lut

	for (int i = 0; i < maxValue; i++)
	{
		{
		if (i < interval1)
		{
			double value = offset + ( (sineFactor -offset) *  std::sin((double)i / interval1 * pi/2));
			if(isForuteenBit)
				LUTValues[i] = (ushort)(i + (ushort)value);

			else
				LUTValues_8[i] = (uint8_t)(i + (uint8_t)value);
		}
		else if (i < interval1+interval2)
		{


			double value = CosineFactor * (1 + std::cos(((double)i - interval1) / interval2 * (pi)));
			if(isForuteenBit)
				LUTValues[i] = (ushort)(i + (ushort)value);
			else
				LUTValues_8[i] = (uint8_t)(i + (uint8_t)value);
		}
		else
		{
			if(isForuteenBit)
				LUTValues[i] = i;
			else
				LUTValues_8[i] = i;
		}
		}
		string st = "";
		if(isForuteenBit)
			st =to_string(i)+", ,"+ to_string(LUTValues[i])+"\n";
		else
			st =to_string(i)+", ,"+ to_string(LUTValues_8[i])+"\n";

		fprintf(f,st.c_str());
	}
	fclose(f);


}


void ImageProc_CalculateChannelWiseLut(double sineFactor,double interval1,double interval2 ,int bitDepth,bool isForuteenBit,int offset, int channelCode)
{
	FILE* f;
	pi = 4 * std::atan(1);
	int  maxValue =(int)std::pow(2, bitDepth);
	int quaterthValue = (int)maxValue / 4;
	int halfthValue = (int)2* quaterthValue ;
	int threeFourthValue = (int)3 * quaterthValue;
	//	sineFactor = 4096;
	//	interval1 = 4096;  // 1st phase of curve is from 0 to interval 1 -- Sine wave (0 to PI/2)//
	//	interval2 = 8192; // 2nd phase of curve is from interval1 to interval1+interval 2  -- Cosine Wave (0 to PI)
	// 3rd phase of the curve is from interval1+interval2 upto max value -- Linear

	double CosineFactor = sineFactor/2; // nominally be sinefactor/2
	
	
	if(channelCode == 1)
		f = fopen("lutR.csv","wb");
	else if(channelCode == 2)
		f = fopen("lutG.csv","wb");
	else if(channelCode == 3)
		f = fopen("lutB.csv","wb");
	else
		f = fopen("lut.csv","wb");

	fprintf(f,"pixelNo,,val \n");
	if(isForuteenBit)
		LUTValues = (uint16_t*) malloc(sizeof(uint16_t)*maxValue);// uint16_t lut 
	else
	{
		if(channelCode == 1)
			LUTValues_8R = (uint8_t*) malloc(sizeof(uint8_t)*maxValue);// uint8_t bit lut
		else if(channelCode == 2)
			LUTValues_8G = (uint8_t*) malloc(sizeof(uint8_t)*maxValue);// uint8_t bit lut
		else if(channelCode == 3)
			LUTValues_8B = (uint8_t*) malloc(sizeof(uint8_t)*maxValue);// uint8_t bit lut
		else if(channelCode == 4)
		{
			LUTValues_8R = (uint8_t*) malloc(sizeof(uint8_t)*maxValue);// uint8_t bit lut
			LUTValues_8G = (uint8_t*) malloc(sizeof(uint8_t)*maxValue);// uint8_t bit lut
			LUTValues_8B = (uint8_t*) malloc(sizeof(uint8_t)*maxValue);// uint8_t bit lut

		}

	}

	for (int i = 0; i < maxValue; i++)
	{
		{
		if (i < interval1)
		{
			double value = offset + ( (sineFactor -offset) *  std::sin((double)i / interval1 * pi/2));
			if(isForuteenBit)
				LUTValues[i] = (ushort)(i + (ushort)value);

			else
			{
				if(channelCode == 1)
					LUTValues_8R[i] = (uint8_t)(i + (uint8_t)value);
				else if(channelCode == 2)
					LUTValues_8G[i] = (uint8_t)(i + (uint8_t)value);
				else if(channelCode == 3)
					LUTValues_8B[i] = (uint8_t)(i + (uint8_t)value);
				else if(channelCode == 4)
				{
					LUTValues_8R[i] = (uint8_t)(i + (uint8_t)value);
					LUTValues_8G[i] = (uint8_t)(i + (uint8_t)value);
					LUTValues_8B[i] = (uint8_t)(i + (uint8_t)value);

				}
			}
		}
		else if (i < interval1+interval2)
		{


			double value = CosineFactor * (1 + std::cos(((double)i - interval1) / interval2 * (pi)));
			if(isForuteenBit)
				LUTValues[i] = (ushort)(i + (ushort)value);
			else
				{
					if(channelCode == 1)
						LUTValues_8R[i] = (uint8_t)(i + (uint8_t)value);
					else if(channelCode == 2)
						LUTValues_8G[i] = (uint8_t)(i + (uint8_t)value);
					else if(channelCode == 3)
						LUTValues_8B[i] = (uint8_t)(i + (uint8_t)value);
					else if(channelCode == 4)
					{
						LUTValues_8R[i] = (uint8_t)(i + (uint8_t)value);
						LUTValues_8G[i] = (uint8_t)(i + (uint8_t)value);
						LUTValues_8B[i] = (uint8_t)(i + (uint8_t)value);

					}
				}
			
			
		}
		else
		{
			if(isForuteenBit)
				LUTValues[i] = i;
			else
			{
				if(channelCode == 1)
					LUTValues_8R[i] = i;
				else if(channelCode == 2)
					LUTValues_8G[i] = i;
				else if(channelCode == 3)
					LUTValues_8B[i] = i;
				else if(channelCode == 4)
				{
					LUTValues_8R[i] = i;
					LUTValues_8G[i] = i;
					LUTValues_8B[i] = i;

				}
			}
		}
		}
		string st = "";
		if(isForuteenBit)
			st =to_string(i)+", ,"+ to_string(LUTValues[i])+"\n";
		else
		{
			if(channelCode == 1)
				st =to_string(i)+", ,"+ to_string(LUTValues_8R[i])+"\n";
			else if(channelCode == 2)
				st =to_string(i)+", ,"+ to_string(LUTValues_8G[i])+"\n";
			else if(channelCode == 3)
				st =to_string(i)+", ,"+ to_string(LUTValues_8B[i])+"\n";
			else if(channelCode == 4)
			{
				st =to_string(i)+", ,"+ to_string(LUTValues_8R[i])+"\n";
				st =to_string(i)+", ,"+ to_string(LUTValues_8G[i])+"\n";
				st =to_string(i)+", ,"+ to_string(LUTValues_8B[i])+"\n";

			}

		}
		fprintf(f,st.c_str());
	}
	fclose(f);
	


}


extern "C" __declspec(dllexport) void ImageProc_CalculateMidtoneLut(int Amplitude,int StartPoint,int EndPoint ,int bitDepth)
{
	pi = 4 * std::atan(1);
	int  maxValue =(int)std::pow(2, bitDepth);
	int quaterthValue = (int)maxValue / 4;
	int halfthValue = (int)2* quaterthValue ;
	int threeFourthValue = (int)3 * quaterthValue;
	//sineFactor = 4096;
	//interval1 = 4000;  // 1st phase of curve is from 0 to interval 1 -- Sine wave (0 to PI/2)
	//interval2 = 10000; // 2nd phase of curve is from interval1 to interval1+interval 2  -- Cosine Wave (0 to PI)
	// 3rd phase of the curve is from interval1+interval2 upto max value -- Linear

	//double CosineFactor = sineFactor/2; // nominally be sinefactor/2
	FILE* f = fopen("lut.csv","wb");
	fprintf(f,"pixelNo,,val \n");
	LUTValues = (uint16_t*) malloc(sizeof(uint16_t)*maxValue);
	double rangeVal = EndPoint - StartPoint;
	double zone1 = StartPoint + rangeVal/3;
	for (ushort i = 0; i < maxValue; i++)
	{

		/*if (i < EndPoint && i> StartPoint)
		{
		double value = sineFactor *  std::sin((double)i / interval1 * pi/2);
		LUTValues[i] = (ushort)(i + (ushort)value);
		}
		else*/

		if (i < EndPoint && i> StartPoint)
		{

			if(i < zone1)
			{
				double numberVal = i - StartPoint;
				double value = Amplitude * (1 + std::cos( (1 - (3*numberVal/ rangeVal)) * pi));
				LUTValues[i] = (ushort)(i + (ushort)value);
			}
			else
			{
				double numberVal = i - zone1;
				double value = Amplitude * (1 + std::cos((( numberVal / (2* rangeVal/3)) * pi)));
				LUTValues[i] = (ushort)(i + (ushort)value);
			}

		}
		else
		{
			LUTValues[i] = i;
		}
		string st =to_string(i)+", ,"+ to_string(LUTValues[i])+"\n";

		fprintf(f,st.c_str());
	}
	fclose(f);


}

//extern "C"_declspec(dllexport)void ImageProc_InitStitcher()
//{
//	retinalImageStich = new stitchRet(); 
//}
//
//extern "C"_declspec(dllexport) stitchRet::StitchStruct*  ImageProc_ImageStich(IplImage* SrcImg1[], int arrcnt, stitchRet::StitchingParams* params,int* width, int* height,int rowMax,int colMax)
//{
//	return	retinalImageStich->Stitch(SrcImg1, arrcnt, params, width, height, rowMax, colMax) ;//
//}
//extern "C"_declspec(dllexport) void ImageProc_GetStitchedImage(IplImage* outputImage)
//{
//	retinalImageStich->GetStitchedImage(outputImage);
//}
