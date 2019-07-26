//#ifndef stitchRet_H_
//#define stitchRet_H_
//#include "opencv\cv.h"
//#include "opencv\highgui.h"
//#include "opencv2\stitching\stitcher.hpp"
//#include "opencv2/core/core.hpp"
//#include "opencv2/highgui/highgui.hpp"
//#include "opencv2/imgproc/imgproc.hpp"
//
//using namespace cv;
//using namespace std;
//using namespace cv::detail;
//class stitchRet
//{
//public:
//
//	struct StitchStruct
//		
//	{
//		Stitcher::Status StitchStatus;
//		unsigned long stitchTime; 
//		int Width;
//		int Height;
//		
//	};
//	struct StitchingParams
//	{
//		double ConfThresh;
//		int oct;
//		int layer;
//		int oct_desc;
//		int layer_desc;
//	};
//	IplImage* stitchedImage;
//	StitchStruct* result;
//	StitchStruct* Stitch(IplImage* inputImg1[],int arrCnt, StitchingParams* Params,int* width,int* height,int rowMax,int colMax); 
//	void GetStitchedImage(IplImage* outputImage);
//};
//#endif 
//
//
