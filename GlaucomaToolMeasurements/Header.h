#include "core\core.hpp"
#include "highgui\highgui.hpp"
#include <iostream>
#include <highgui.h>
#include  "cv.hpp"
#include "imgproc\imgproc.hpp"
#include <vector>
#include "core\core_c.h"
#include <math.h>  

using namespace cv;
using namespace std;


struct Cup2DiscStruct
{
	double DiscArea;
	double CupArea;
	double RimArea;
	double InferiorRegionArea;
	double SuperiorRegionArea;
	double NasalRegionArea;
	double TemporalRegionArea;
	double VerticalLengthDisc;
	double VerticalLengthCup;
	double HorizontalLengthDisc;
	double HorizontalLengthCup;
	double VerticalCDR;
	double  HorizontalCDR;
	void init()
	{
		this->DiscArea =0;
		this->CupArea =0;
		this->RimArea =0;
		this->InferiorRegionArea =0;
		this->SuperiorRegionArea =0;
		this->NasalRegionArea =0;
		this->TemporalRegionArea =0;
		this->VerticalLengthDisc =0;
		this->VerticalLengthCup =0;
		this->HorizontalLengthDisc =0;
		this->HorizontalLengthCup =0;
		this->VerticalCDR =0;
		this-> HorizontalCDR =0;
	}
};

// Public methods
extern "C" __declspec(dllexport) void CDRInit(int width, int height);
extern "C" __declspec(dllexport) Cup2DiscStruct * CalculateCDR( char* discImagePtr,  char* cupImagePtr );
extern "C" __declspec(dllexport) void CDRExit();


// Private Methods For Computing Cup and disc Features
double ComputeCDR (double x, double y);
double ComputeArea (IplImage *img);
void GetBoundingRectangle(IplImage * srcImg, IplImage* dstImg, CvRect* BoundingRect);
void GetInferiormask(IplImage * srcImg, IplImage* dstImg);
void GetSuperiormask(IplImage * srcImg, IplImage* dstImg);
void GetNasalmask(IplImage * srcImg, IplImage* dstImg);
void GetTemporalmask(IplImage * srcImg, IplImage* dstImg);
void GetPoints(CvRect cupRect, CvRect discRect );
void GetWhitePixels(IplImage * discImage, IplImage *cupImage);
void computeLineProjection();
bool LineInterSection(CvPoint srcPt1, CvPoint srcpt2, CvPoint startPt1,CvPoint endpt1, CvPoint* endpt2);
char get_line_intersection(float p0_x, float p0_y, float p1_x, float p1_y, 
    float p2_x, float p2_y, float p3_x, float p3_y, float *i_x, float *i_y);
bool intersection(Point2f o1, Point2f p1, Point2f o2, Point2f p2,
                      Point2f &r);
void computeMask(CvPoint point1,CvPoint point2,CvPoint point3,IplImage* srcImg, IplImage* destImg,int val);


