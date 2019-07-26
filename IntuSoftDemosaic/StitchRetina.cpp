//#include "stitchRet.h"
//
//stitchRet::StitchStruct* stitchRet::Stitch(IplImage* inputImg1[],int arrCnt, StitchingParams* Params,int* width,int*height,int rowMax,int colMax ) // defining function "Stitch"
//{
//	result= new StitchStruct(); 
//	vector< Mat > vImg;
//	Mat rImg;
//	double featureFinderThresh = 260;
//	for (int i = 0; i < arrCnt; i++)
//	{
//		vImg.push_back(inputImg1[i]);
//	}
//	Stitcher stitcher = Stitcher::createDefault(true);
//	stitcher.rowMax=rowMax;
//	stitcher.colMax=colMax;
//	stitcher.setFeaturesFinder(new detail::SurfFeaturesFinder( Params->ConfThresh,Params->oct,Params->layer,Params->oct_desc,Params->layer_desc));
//	unsigned long AAtime=0, BBtime=0; //check processing time
//	AAtime = getTickCount(); //check processing time
//	result->StitchStatus = stitcher.stitch(vImg, rImg);//result->StitchStatus this doesn't take OK by deafult , rather takes what is returned.
//	result->stitchTime=BBtime = getTickCount(); //check processing time 
//	if (Stitcher::OK == result->StitchStatus) // OK=0,ERR_NEED_MORE_IMGS=1,ERR_SIZE_OVERFLOW=2// if stitching occurs then status is 0, and this will means OK
//	{
//	stitchedImage =  cvCloneImage(&(IplImage)rImg);//rImg cloned and given to stitchedImage.
//    *width = stitchedImage->width;//the width is given to pointer
//	*height = stitchedImage->height;
//	result->Height = stitchedImage->height;
//	result->Width = stitchedImage->width;
//	
//	rImg.release();
//	vImg.clear();
//	}
//
//	return result;
//}
//void stitchRet::GetStitchedImage(IplImage* outputImage)
//{
//		cvCopy(stitchedImage,outputImage);
//		cvReleaseImage(&stitchedImage);
//
//}
//	